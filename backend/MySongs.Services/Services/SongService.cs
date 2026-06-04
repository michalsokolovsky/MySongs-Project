using AutoMapper;
using MySongs.Common.DTOs;
using MySongs.Repository.Entities;
using MySongs.Repository.Interfaces;
using MySongs.Services.Interfaces;

namespace MySongs.Services.Services
{
    public class SongService : ISongService
    {
        private readonly ISongRepository _repository;
        private readonly IMapper _mapper;

        public SongService(ISongRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<SongDto>> GetAll()
        {
            return _mapper.Map<List<SongDto>>(await _repository.GetAll());
        }

        public async Task<SongDto> GetById(int id)
        {
            var song = await _repository.GetById(id);
            if (song == null) return null;
            return _mapper.Map<SongDto>(song);
        }

        public async Task Add(SongDto song)
        {
            await _repository.Add(_mapper.Map<Songs>(song));
        }

        public async Task Update(SongDto song)
        {
            await _repository.Update(_mapper.Map<Songs>(song));
        }

        public async Task Delete(int id)
        {
            await _repository.Delete(id);
        }

        public async Task<List<SongDto>> Search(string query)
        {
            var q = query.ToLower();
            var all = await _repository.GetAll();
            var results = all.Where(s =>
                    s.Title.ToLower().Contains(q) ||
                    s.ArtistName.ToLower().Contains(q) ||
                    s.Genre.ToLower().Contains(q) ||
                    (s.LyricsSummary != null && s.LyricsSummary.ToLower().Contains(q)))
                .ToList();
            return _mapper.Map<List<SongDto>>(results);
        }
    }
}