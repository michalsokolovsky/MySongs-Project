using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySongs.Common.DTOs;
using MySongs.Services.Interfaces;

namespace MySongs.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ListeningHistoryController : ControllerBase
    {
        private readonly IListeningHistoryService _listeningHistoryService;

        public ListeningHistoryController(IListeningHistoryService listeningHistoryService)
        {
            _listeningHistoryService = listeningHistoryService;
        }

        [HttpGet("user/{userId}")]
        [Authorize]
        public async Task<IActionResult> GetByUser(int userId)
        {
            var history = await _listeningHistoryService.GetAll();
            return Ok(history.Where(h => h.UserId == userId).ToList());
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add([FromBody] ListeningHistoryDto history)
        {
            history.ListenDate = DateTime.Now; // ← הוסיפי את זה!
            await _listeningHistoryService.Add(history);
            return Ok("ההאזנה נרשמה");
        }
    }
}