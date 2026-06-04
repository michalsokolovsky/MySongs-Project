using AutoMapper;
using MySongs.Common.DTOs;
using MySongs.Repository.Entities;
using MySongs.Repository.Interfaces;
using MySongs.Services.Interfaces;

namespace MySongs.Services.Services
{
    public class RecommendationService : IRecommendationService
    {
        private readonly IRecommendationRepository _repository;
        private readonly IMapper _mapper;

        public RecommendationService(IRecommendationRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<RecommendationDto>> GetByUserId(int userId)
        {
            return _mapper.Map<List<RecommendationDto>>(await _repository.GetByUserId(userId));
        }

        public async Task Add(RecommendationDto recommendation)
        {
            await _repository.Add(_mapper.Map<Recommendation>(recommendation));
        }
        public async Task Delete(int recommendationId)
        {
            await _repository.Delete(recommendationId);
        }
    }
}