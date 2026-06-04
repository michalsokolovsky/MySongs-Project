using MySongs.Common.DTOs;

namespace MySongs.Services.Interfaces
{
    public interface ISongService
    {
        Task<List<SongDto>> GetAll();
        Task<SongDto> GetById(int id);
        Task Add(SongDto song);
        Task Update(SongDto song);
        Task Delete(int id);
        Task<List<SongDto>> Search(string query);
    }
}