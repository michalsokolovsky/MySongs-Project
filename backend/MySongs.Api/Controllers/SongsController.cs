using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySongs.Api.Services;
using MySongs.Common.DTOs;
using MySongs.Services.Interfaces;

namespace MySongs.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SongsController : ControllerBase
    {
        private readonly ISongService _songService;
        private readonly ITagService _tagService;
        private readonly ISongTagService _songTagService;
        private readonly RecommendationEngineService _recommendationEngine;
        private readonly IConfiguration _configuration;
        private readonly IAIService _aiService;


        public SongsController(
            ISongService songService,
            ITagService tagService,
            ISongTagService songTagService,
            RecommendationEngineService recommendationEngine,
            IConfiguration configuration,
                IAIService aiService) 

        {
            _songService = songService;
            _tagService = tagService;
            _songTagService = songTagService;
            _recommendationEngine = recommendationEngine;
            _configuration = configuration;
            _aiService = aiService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _songService.GetAll());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var song = await _songService.GetById(id);
            if (song == null) return NotFound();
            return Ok(song);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string q)
        {
            if (string.IsNullOrWhiteSpace(q))
                return Ok(await _songService.GetAll());
            return Ok(await _songService.Search(q));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add([FromBody] SongDto song)
        {
            await _songService.Add(song);
            return Ok("השיר נוסף");
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, [FromBody] SongDto song)
        {
            song.SongId = id;
            await _songService.Update(song);
            return Ok("השיר עודכן");
        }
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            // שליפת השיר לפני המחיקה כדי לקבל את נתיב הקובץ
            var song = await _songService.GetById(id);

            if (song == null)
                return NotFound("השיר לא נמצא");

            // מחיקת קובץ האודיו מהשרת אם קיים
            if (!string.IsNullOrEmpty(song.AudioUrl))
            {
                // חילוץ שם הקובץ מה-URL
                var fileName = Path.GetFileName(new Uri(song.AudioUrl).LocalPath);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "audio", fileName);

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            // מחיקת השיר מה-DB
            await _songService.Delete(id);
            return Ok("השיר נמחק");
        }

        [HttpGet("{id}/tags")]
        public async Task<IActionResult> GetSongTags(int id)
        {
            var songTags = await _songTagService.GetTagsBySongId(id);
            var allTags = await _tagService.GetAll();
            var tagNames = songTags
                .Select(st => allTags.FirstOrDefault(t => t.TagId == st.TagId)?.TagName)
                .Where(n => n != null)
                .ToList();
            return Ok(tagNames);
        }

        [HttpPost("upload")]
        [Authorize]
        public async Task<IActionResult> UploadSong(IFormFile audioFile, [FromForm] string artistName)
        {
            if (audioFile == null || audioFile.Length == 0)
                return BadRequest("לא הועלה קובץ");

            string fileHash;
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                using var hashStream = audioFile.OpenReadStream();
                var hashBytes = await md5.ComputeHashAsync(hashStream);
                fileHash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }

            var allSongs = await _songService.GetAll();
            var existingByHash = allSongs.FirstOrDefault(s => s.FileHash == fileHash);
            if (existingByHash != null)
                return Conflict($"השיר '{existingByHash.Title}' של {existingByHash.ArtistName} כבר קיים במאגר!");

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "audio");
            Directory.CreateDirectory(uploadsFolder);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(audioFile.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await audioFile.CopyToAsync(stream);
            }

            var audioUrl = $"{Request.Scheme}://{Request.Host}/audio/{fileName}";
           

            try
            {
                var result = await _aiService.AnalyzeSongMetadata(filePath);

                if (string.IsNullOrWhiteSpace(result.Title) ||
                    result.Title.Length > 100 ||
                    result.Title.Contains("Exception") ||
                    result.Title.Contains("error") ||
                    result.Title.Contains("Error") ||
                    result.Title.Contains("Blocked") ||
                    result.Title.Contains("stream") ||
                    result.Title.Contains("copying") ||
                    result.Title.Contains("418") ||
                    result.Title.Contains("success:") ||
                    result.Genre == null)
                {
                    System.IO.File.Delete(filePath);
                    return BadRequest("שגיאה בניתוח השיר - ייתכן שאין חיבור לאינטרנט. נסי שוב.");
                }

                var song = new SongDto
                {
                    Title = result.Title,
                    ArtistName = artistName,
                    Genre = result.Genre,
                    LyricsSummary = result.Summary,
                    AudioUrl = audioUrl,
                    FileHash = fileHash,
                    ReleaseDate = DateTime.Now
                };

                await _songService.Add(song);

                var savedSongs = await _songService.GetAll();
                var savedSong = savedSongs.OrderByDescending(s => s.SongId).FirstOrDefault();

                if (savedSong != null)
                {
                    _ = Task.Run(async () =>
                    {
                        await _recommendationEngine.UpdateRecommendationsForAllUsers(savedSong.SongId);
                    });
                }

                return Ok(savedSong);
            }
            catch (Exception ex)
            {
                System.IO.File.Delete(filePath);
                return BadRequest($"שגיאה: {ex.Message}");
            }
        }
    }
}