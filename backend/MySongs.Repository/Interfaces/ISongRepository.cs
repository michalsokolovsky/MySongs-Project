using MySongs.Repository.Entities;

namespace MySongs.Repository.Interfaces
{
    public interface ISongRepository
    {
        Task<List<Songs>> GetAll();
        Task<Songs> GetById(int id);
        Task Add(Songs song);
        Task Update(Songs song);
        Task Delete(int id);
    }
}