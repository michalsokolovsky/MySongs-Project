using MySongs.Repository.Entities;

namespace MySongs.Repository.Interfaces
{
    public interface ISongTagRepository
    {
        Task<List<SongTag>> GetTagsBySongId(int songId);
        Task Add(SongTag songTag);
        Task Delete(int songId, int tagId);
    }
}