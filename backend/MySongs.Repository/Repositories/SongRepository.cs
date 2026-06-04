using Microsoft.EntityFrameworkCore;
using MySongs.Repository.Entities;
using MySongs.Repository.Interfaces;

namespace MySongs.Repository.Repositories
{
    public class SongRepository : ISongRepository
    {
        private readonly IContext _context;

        public SongRepository(IContext context)
        {
            _context = context;
        }

        public async Task<List<Songs>> GetAll()
        {
            return await _context.Songs.ToListAsync();
        }

        public async Task<Songs> GetById(int id)
        {
            return await _context.Songs.FirstOrDefaultAsync(s => s.SongId == id);
        }

        public async Task Add(Songs song)
        {
            _context.Songs.Add(song);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Songs song)
        {
            _context.Songs.Update(song);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var song = await _context.Songs.FirstOrDefaultAsync(s => s.SongId == id);
            if (song != null)
            {
                _context.Songs.Remove(song);
                await _context.SaveChangesAsync();
            }
        }
    }
}