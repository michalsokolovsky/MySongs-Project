using AutoMapper;
using MySongs.Common.DTOs;
using MySongs.Repository.Entities;
using MySongs.Repository.Interfaces;
using MySongs.Services.Interfaces;

namespace MySongs.Services.Services
{
    public class SongTagService : ISongTagService
    {
        private readonly ISongTagRepository _repository;
        private readonly IMapper _mapper;

        public SongTagService(ISongTagRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<SongTagDto>> GetTagsBySongId(int songId)
        {
            return _mapper.Map<List<SongTagDto>>(await _repository.GetTagsBySongId(songId));
        }

        public async Task Add(SongTagDto songTag)
        {
            await _repository.Add(_mapper.Map<SongTag>(songTag));
        }

        public async Task Delete(int songId, int tagId)
        {
            await _repository.Delete(songId, tagId);
        }
    }
}