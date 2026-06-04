using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MySongs.Api.Services
{
    public class SongAnalysisResult
    {
        public string Title { get; set; } = "";
        public string Artist { get; set; } = "";
        public string Genre { get; set; } = "";
        public string Summary { get; set; } = "";
    }

    public class AIService : IAIService
    {
        private readonly string _apiKey;

        public AIService(string apiKey)
        {
            _apiKey = apiKey;
        }

        public async Task<SongAnalysisResult> AnalyzeSongMetadata(string filePath)
        {
            try
            {
                string lyrics = await TranscribeAudioWithWhisper(filePath);
                var metadata = await ExtractMetadataFromLyrics(lyrics);
                return metadata;
            }
            catch (Exception ex)
            {
                throw new Exception($"שגיאה בניתוח השיר: {ex.Message}");
            }
        }

        private async Task<string> TranscribeAudioWithWhisper(string path)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _apiKey);

            using var content = new MultipartFormDataContent();
            var fileContent = new ByteArrayContent(File.ReadAllBytes(path));
            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("audio/mpeg");

            content.Add(fileContent, "file", Path.GetFileName(path));
            content.Add(new StringContent("whisper-1"), "model");
            content.Add(new StringContent("he"), "language");

            var response = await client.PostAsync(
                "https://api.openai.com/v1/audio/transcriptions", content);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();
            dynamic json = JsonConvert.DeserializeObject(result)!;
            return json.text;
        }

        private async Task<SongAnalysisResult> ExtractMetadataFromLyrics(string lyrics)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _apiKey);

            var requestBody = new
            {
                model = "gpt-4o",
                messages = new[]
                {
                    new { role = "system", content = "You are a music expert. Analyze the lyrics and return ONLY a JSON object with: Title, Artist, Genre (one word like 'Sad', 'Happy', 'Pop'), and Summary (short description in Hebrew)." },
                    new { role = "user", content = $"Lyrics: {lyrics}" }
                },
                response_format = new { type = "json_object" }
            };

            var content = new StringContent(
                JsonConvert.SerializeObject(requestBody),
                System.Text.Encoding.UTF8,
                "application/json");

            var response = await client.PostAsync(
                "https://api.openai.com/v1/chat/completions", content);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            dynamic jsonResponse = JsonConvert.DeserializeObject(responseString)!;
            string jsonContent = jsonResponse.choices[0].message.content;

            return JsonConvert.DeserializeObject<SongAnalysisResult>(jsonContent)!;
        }
    }
}