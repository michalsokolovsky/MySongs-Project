namespace MySongs.Common.DTOs
{
    public class AIAnalysisResult
    {
        /// <summary>
        /// תגיות שה-AI הציע לשיר (למשל: "שבת", "אהבה", "קיץ", "עצב")
        /// </summary>
        public List<string> SuggestedTags { get; set; } = new();

        /// <summary>
        /// תיאור קצר שה-AI יצר על השיר
        /// </summary>
        public string AIDescription { get; set; } = "";

        /// <summary>
        /// קהל יעד מומלץ (למשל: "מתאים לשבת", "מתאים לילדים")
        /// </summary>
        public string TargetAudience { get; set; } = "";

        /// <summary>
        /// מצב-רוח כללי של השיר
        /// </summary>
        public string Mood { get; set; } = "";
    }
}
