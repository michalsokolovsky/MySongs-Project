using MySongs.Repository.Entities;

namespace MySongs.Repository.Interfaces
{
    public interface ITagRepository
    {
        Task<List<Tag>> GetAll();
        Task Add(Tag tag);
        Task<Tag> GetOrCreate(string tagName);
    }
}