using Microsoft.EntityFrameworkCore;
using MySongs.Repository.Entities;
using MySongs.Repository.Interfaces;

namespace MySongs.Repository.Repositories
{
    public class RecommendationRepository : IRecommendationRepository
    {
        private readonly IContext _context;

        public RecommendationRepository(IContext context)
        {
            _context = context;
        }

        public async Task<List<Recommendation>> GetByUserId(int userId)
        {
            return await _context.Recommendations
                .Where(r => r.UserId == userId)
                .ToListAsync();
        }

        public async Task Add(Recommendation recommendation)
        {
            _context.Recommendations.Add(recommendation);
            await _context.SaveChangesAsync();
        }
        public async Task Delete(int recommendationId)
        {
            var rec = await _context.Recommendations
                .FirstOrDefaultAsync(r => r.RecommendationId == recommendationId);
            if (rec != null)
            {
                _context.Recommendations.Remove(rec);
                await _context.SaveChangesAsync();
            }
        }
    }
}