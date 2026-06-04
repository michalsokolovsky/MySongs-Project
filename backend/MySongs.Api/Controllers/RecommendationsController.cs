using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySongs.Api.Services;
using MySongs.Common.DTOs;
using MySongs.Services.Interfaces;

namespace MySongs.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecommendationsController : ControllerBase
    {
        private readonly IRecommendationService _recommendationService;
        private readonly RecommendationEngineService _recommendationEngine;

        public RecommendationsController(
            IRecommendationService recommendationService,
            RecommendationEngineService recommendationEngine)
        {
            _recommendationService = recommendationService;
            _recommendationEngine = recommendationEngine;
        }

        [HttpGet("user/{userId}")]
        [Authorize]
        public async Task<IActionResult> GetByUser(int userId)
        {
            var recommendations = await _recommendationService.GetByUserId(userId);
            return Ok(recommendations);
        }

        [HttpPost("generate/{userId}")]
        [Authorize]
        public async Task<IActionResult> Generate(int userId)
        {
            await _recommendationEngine.GenerateRecommendationsForUser(userId);
            var recommendations = await _recommendationService.GetByUserId(userId);
            return Ok(recommendations);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add([FromBody] RecommendationDto recommendation)
        {
            await _recommendationService.Add(recommendation);
            return Ok("ההמלצה נוספה");
        }
    }
}