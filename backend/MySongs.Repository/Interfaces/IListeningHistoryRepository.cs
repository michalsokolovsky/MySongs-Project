using MySongs.Repository.Entities;

namespace MySongs.Repository.Interfaces
{
    public interface IListeningHistoryRepository
    {
        Task<List<ListeningHistory>> GetAll();
        Task Add(ListeningHistory history);
    }
}