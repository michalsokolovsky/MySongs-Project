using MySongs.Common.DTOs;

namespace MySongs.Services.Interfaces
{
    public interface IRecommendationService
    {
        Task<List<RecommendationDto>> GetByUserId(int userId);
        Task Add(RecommendationDto recommendation);
        Task Delete(int recommendationId);

    }
}