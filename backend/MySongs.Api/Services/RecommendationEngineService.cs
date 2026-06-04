using MySongs.Common.DTOs;
using MySongs.Services.Interfaces;
using Newtonsoft.Json;
using System.Text;

namespace MySongs.Api.Services
{
    /// <summary>
    /// שירות לייצור המלצות מוזיקה חכמות באמצעות GPT-4.
    /// מנתח את היסטוריית ההאזנה של המשתמש וממליץ על שירים שלא שמע עדיין,
    /// תוך מתן עדיפות לז'אנרים שהאזין אליהם הכי הרבה.
    /// </summary>
    public class RecommendationEngineService
    {
        private readonly IUserService _userService;                           // שירות לניהול משתמשים
        private readonly ISongService _songService;                           // שירות לניהול שירים
        private readonly IRecommendationService _recommendationService;       // שירות לניהול המלצות
        private readonly IListeningHistoryService _listeningHistoryService;   // שירות לניהול היסטוריית האזנה
        private readonly string _apiKey;                                      // מפתח API של OpenAI

        public RecommendationEngineService(
            IUserService userService,
            ISongService songService,
            IRecommendationService recommendationService,
            IListeningHistoryService listeningHistoryService,
            IConfiguration configuration)
        {
            _userService = userService;
            _songService = songService;
            _recommendationService = recommendationService;
            _listeningHistoryService = listeningHistoryService;
            _apiKey = configuration["OpenAI:ApiKey"]!;
        }

        /// <summary>
        /// מעדכן המלצות לכל המשתמשים במערכת.
        /// נקרא אוטומטית כאשר מתווסף שיר חדש למאגר,
        /// כדי שכל משתמש יוכל לקבל המלצה על השיר החדש אם הוא מתאים לו.
        /// </summary>
        /// <param name="newSongId">מזהה השיר החדש שנוסף</param>
        public async Task UpdateRecommendationsForAllUsers(int newSongId)
        {
            // שליפת כל המשתמשים מהמערכת
            var users = await _userService.GetAll();

            // עבור כל משתמש — יצירת המלצות מעודכנות בנפרד
            foreach (var user in users)
            {
                await GenerateRecommendationsForUser(user.UserId);
            }
        }

        /// <summary>
        /// מייצר המלצות מותאמות אישית למשתמש ספציפי.
        /// התהליך:
        /// 1. מחיקת המלצות ישנות
        /// 2. ניתוח היסטוריית האזנה
        /// 3. בניית Prompt חכם ל-GPT
        /// 4. קבלת המלצות מ-GPT ושמירתן ב-DB
        /// </summary>
        /// <param name="userId">מזהה המשתמש שעבורו מייצרים המלצות</param>
        public async Task GenerateRecommendationsForUser(int userId)
        {
            // שלב 1: מחיקת המלצות ישנות לפני יצירת חדשות
            // כדי שלא יצטברו המלצות מיושנות שכבר לא רלוונטיות
            var oldRecs = await _recommendationService.GetByUserId(userId);
            foreach (var rec in oldRecs)
            {
                await _recommendationService.Delete(rec.RecommendationId);
            }

            // שלב 2: שליפת כל השירים וסינון היסטוריית המשתמש הספציפי
            var allSongs = await _songService.GetAll();
            var allHistory = await _listeningHistoryService.GetAll();
            var history = allHistory.Where(h => h.UserId == userId).ToList();

            // אם המשתמש לא האזין לאף שיר — אין מספיק מידע ליצירת המלצות
            if (!history.Any()) return;

            // שלב 3: הפרדה בין שירים שנשמעו לשירים שלא נשמעו
            // שירים שנשמעו — ישמשו לניתוח העדפות המשתמש
            var listenedSongs = history
                .Select(h => allSongs.FirstOrDefault(s => s.SongId == h.SongId))
                .Where(s => s != null)
                .ToList();

            // שירים שלא נשמעו — אלה המועמדים להמלצה
            var notListened = allSongs
                .Where(s => !history.Any(h => h.SongId == s.SongId))
                .ToList();

            // אם המשתמש שמע את כל השירים במאגר — אין מה להמליץ
            if (!notListened.Any()) return;

            // שלב 4: חישוב העדפות הז'אנר לפי מספר האזנות
            // למשל: "מזרחי (5 האזנות), פופ (3 האזנות)"
            var genreCount = listenedSongs
                .GroupBy(s => s!.Genre)
                .OrderByDescending(g => g.Count())
                .Select(g => $"{g.Key} ({g.Count()} האזנות)");

            // הכנת הנתונים כטקסט לשליחה ל-GPT
            var listenedText = string.Join(", ", listenedSongs.Select(s => $"{s!.Title} ({s.Genre})"));
            var genrePreference = string.Join(", ", genreCount);
            var candidatesText = string.Join("\n", notListened.Select(s => $"ID:{s.SongId} - {s.Title} ({s.Genre}) - {s.ArtistName}"));

            // שלב 5: בניית Prompt חכם ל-GPT עם כל הנתונים הרלוונטיים
            var prompt = $@"משתמש האזין לשירים הבאים: {listenedText}
                         העדפות ז'אנר לפי תדירות האזנה: {genrePreference}
                         מתוך הרשימה הבאה, בחר עד 3 שירים שהכי מתאימים לו - תן עדיפות לז'אנרים שהאזין אליהם הכי הרבה:
                         {candidatesText} 
                          החזר JSON בלבד בפורמט: {{""songIds"": [1, 2, 3]}}";

            try
            {
                // שלב 6: שליחת הבקשה ל-OpenAI API
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _apiKey);

                var requestBody = new
                {
                    model = "gpt-4o",
                    messages = new[]
                    {
                        new { role = "system", content = "You are a music recommendation expert. Return only valid JSON." },
                        new { role = "user", content = prompt }
                    },
                    response_format = new { type = "json_object" } // מבטיח שהתשובה תהיה JSON תקין
                };

                var content = new StringContent(
                    JsonConvert.SerializeObject(requestBody),
                    Encoding.UTF8,
                    "application/json");

                // שליחה וקבלת תשובה מ-GPT
                var response = await client.PostAsync("https://api.openai.com/v1/chat/completions", content);
                var responseString = await response.Content.ReadAsStringAsync();

                // פענוח תשובת GPT וחילוץ מזהי השירים המומלצים
                dynamic jsonResponse = JsonConvert.DeserializeObject(responseString)!;
                string jsonContent = jsonResponse.choices[0].message.content;
                dynamic result = JsonConvert.DeserializeObject(jsonContent)!;

                // שלב 7: שמירת ההמלצות החדשות ב-DB
                var existingRecs = await _recommendationService.GetByUserId(userId);
                var existingIds = existingRecs.Select(r => r.SongId).ToList();

                foreach (var songId in result.songIds)
                {
                    int id = (int)songId;
                    // בדיקה למניעת כפילויות — לא מוסיף המלצה שכבר קיימת
                    if (!existingIds.Contains(id))
                    {
                        await _recommendationService.Add(new RecommendationDto
                        {
                            UserId = userId,
                            SongId = id,
                            RecommendedAt = DateTime.Now
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                // רישום השגיאה ל-Console — במקרה של בעיה בתקשורת עם OpenAI
                Console.WriteLine($"שגיאה בהמלצות: {ex.Message}");
            }
        }
    }
}