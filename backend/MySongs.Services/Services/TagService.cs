using AutoMapper;
using MySongs.Common.DTOs;
using MySongs.Repository.Entities;
using MySongs.Repository.Interfaces;
using MySongs.Services.Interfaces;

namespace MySongs.Services.Services
{
    public class TagService : ITagService
    {
        private readonly ITagRepository _repository;
        private readonly IMapper _mapper;

        public TagService(ITagRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<TagDto>> GetAll()
        {
            return _mapper.Map<List<TagDto>>(await _repository.GetAll());
        }

        public async Task Add(TagDto tag)
        {
            await _repository.Add(_mapper.Map<Tag>(tag));
        }

        public async Task<TagDto> GetOrCreate(string tagName)
        {
            return _mapper.Map<TagDto>(await _repository.GetOrCreate(tagName));
        }
    }
}