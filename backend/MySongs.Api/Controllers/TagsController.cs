using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySongs.Common.DTOs;
using MySongs.Services.Interfaces;

namespace MySongs.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TagsController : ControllerBase
    {
        private readonly ITagService _tagService;

        public TagsController(ITagService tagService)
        {
            _tagService = tagService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _tagService.GetAll());
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add([FromBody] TagDto tag)
        {
            await _tagService.Add(tag);
            return Ok("התגית נוספה");
        }
    }
}