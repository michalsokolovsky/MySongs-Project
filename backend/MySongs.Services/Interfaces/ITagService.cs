using MySongs.Common.DTOs;

namespace MySongs.Services.Interfaces
{
    public interface ITagService
    {
        Task<List<TagDto>> GetAll();
        Task Add(TagDto tag);
        Task<TagDto> GetOrCreate(string tagName);
    }
}