using Microsoft.EntityFrameworkCore;
using MySongs.Repository.Entities;
using MySongs.Repository.Interfaces;

namespace MySongs.Repository.Repositories
{
    public class SongTagRepository : ISongTagRepository
    {
        private readonly IContext _context;

        public SongTagRepository(IContext context)
        {
            _context = context;
        }

        public async Task<List<SongTag>> GetTagsBySongId(int songId)
        {
            return await _context.SongTags
                .Where(st => st.SongId == songId)
                .ToListAsync();
        }

        public async Task Add(SongTag songTag)
        {
            _context.SongTags.Add(songTag);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int songId, int tagId)
        {
            var songTag = await _context.SongTags
                .FirstOrDefaultAsync(st => st.SongId == songId && st.TagId == tagId);
            if (songTag != null)
            {
                _context.SongTags.Remove(songTag);
                await _context.SaveChangesAsync();
            }
        }
    }
}