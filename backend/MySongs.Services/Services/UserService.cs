using AutoMapper;
using MySongs.Common.DTOs;
using MySongs.Repository.Entities;
using MySongs.Repository.Interfaces;
using MySongs.Services.Interfaces;

namespace MySongs.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<UserDto>> GetAll()
        {
            return _mapper.Map<List<UserDto>>(await _repository.GetAll());
        }

        public async Task<UserDto> GetById(int id)
        {
            var user = await _repository.GetById(id);
            if (user == null) return null;
            return _mapper.Map<UserDto>(user);
        }

        public async Task Add(UserDto user)
        {
            var entity = _mapper.Map<Users>(user);
            entity.CreatedAt = DateTime.Now;
            await _repository.Add(entity);
        }

        public async Task Update(UserDto user)
        {
            await _repository.Update(_mapper.Map<Users>(user));
        }

        public async Task Delete(int id)
        {
            await _repository.Delete(id);
        }
    }
}