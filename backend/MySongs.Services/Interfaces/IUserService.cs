using MySongs.Common.DTOs;

namespace MySongs.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<UserDto>> GetAll();
        Task<UserDto> GetById(int id);
        Task Add(UserDto user);
        Task Update(UserDto user);
        Task Delete(int id);
    }
}