using Microsoft.EntityFrameworkCore;
using MySongs.Repository.Entities;
using MySongs.Repository.Interfaces;

namespace MySongs.Repository.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly IContext _context;

        public TagRepository(IContext context)
        {
            _context = context;
        }

        public async Task<List<Tag>> GetAll()
        {
            return await _context.Tags.ToListAsync();
        }

        public async Task Add(Tag tag)
        {
            _context.Tags.Add(tag);
            await _context.SaveChangesAsync();
        }

        public async Task<Tag> GetOrCreate(string tagName)
        {
            var existing = await _context.Tags.FirstOrDefaultAsync(t => t.TagName == tagName);
            if (existing != null) return existing;
            var newTag = new Tag { TagName = tagName };
            _context.Tags.Add(newTag);
            await _context.SaveChangesAsync();
            return newTag;
        }
    }
}