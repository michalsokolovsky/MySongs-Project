using MySongs.Common.DTOs;

namespace MySongs.Services.Interfaces
{
    public interface IListeningHistoryService
    {
        Task<List<ListeningHistoryDto>> GetAll();
        Task Add(ListeningHistoryDto history);
    }
}