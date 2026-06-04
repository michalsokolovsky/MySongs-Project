using AutoMapper;
using MySongs.Common.DTOs;
using MySongs.Repository.Entities;
using MySongs.Repository.Interfaces;
using MySongs.Services.Interfaces;

namespace MySongs.Services.Services
{
    public class ListeningHistoryService : IListeningHistoryService
    {
        private readonly IListeningHistoryRepository _repository;
        private readonly IMapper _mapper;

        public ListeningHistoryService(IListeningHistoryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<ListeningHistoryDto>> GetAll()
        {
            return _mapper.Map<List<ListeningHistoryDto>>(await _repository.GetAll());
        }

        public async Task Add(ListeningHistoryDto history)
        {
            await _repository.Add(_mapper.Map<ListeningHistory>(history));
        }
    }
}