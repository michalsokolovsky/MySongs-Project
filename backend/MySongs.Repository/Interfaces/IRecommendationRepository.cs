using MySongs.Repository.Entities;

namespace MySongs.Repository.Interfaces
{
    public interface IRecommendationRepository
    {
        Task<List<Recommendation>> GetByUserId(int userId);
        Task Add(Recommendation recommendation);
        Task Delete(int recommendationId);

    }
}