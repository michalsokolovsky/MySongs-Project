using Microsoft.EntityFrameworkCore;
using MySongs.Repository.Entities;
using MySongs.Repository.Interfaces;

namespace MySongs.Repository.Repositories
{
    public class ListeningHistoryRepository : IListeningHistoryRepository
    {
        private readonly IContext _context;

        public ListeningHistoryRepository(IContext context)
        {
            _context = context;
        }

        public async Task<List<ListeningHistory>> GetAll()
        {
            return await _context.ListeningHistorys.ToListAsync();
        }

        public async Task Add(ListeningHistory history)
        {
            _context.ListeningHistorys.Add(history);
            await _context.SaveChangesAsync();
        }
    }
}