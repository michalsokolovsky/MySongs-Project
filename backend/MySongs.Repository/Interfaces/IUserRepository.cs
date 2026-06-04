using MySongs.Repository.Entities;

namespace MySongs.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<List<Users>> GetAll();
        Task<Users> GetById(int id);
        Task Add(Users user);
        Task Update(Users user);
        Task Delete(int id);
    }
}