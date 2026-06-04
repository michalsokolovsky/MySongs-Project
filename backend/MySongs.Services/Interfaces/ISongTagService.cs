using MySongs.Common.DTOs;

namespace MySongs.Services.Interfaces
{
    public interface ISongTagService
    {
        Task<List<SongTagDto>> GetTagsBySongId(int songId);
        Task Add(SongTagDto songTag);
        Task Delete(int songId, int tagId);
    }
}