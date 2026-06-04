using AutoMapper;
using MySongs.Common.DTOs;
using MySongs.Repository.Entities;

namespace MySongs.Services
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Songs
            CreateMap<Songs, SongDto>();
            CreateMap<SongDto, Songs>();

            
            // Users
            CreateMap<Users, UserDto>();
            CreateMap<UserDto, Users>();
            CreateMap<Users, UserResponseDto>();


            // ListeningHistory
            CreateMap<ListeningHistory, ListeningHistoryDto>();
            CreateMap<ListeningHistoryDto, ListeningHistory>();

            // Recommendation
            CreateMap<Recommendation, RecommendationDto>();
            CreateMap<RecommendationDto, Recommendation>();

            // SongTag
            CreateMap<SongTag, SongTagDto>();
            CreateMap<SongTagDto, SongTag>();

            // Tag
            CreateMap<Tag, TagDto>();
            CreateMap<TagDto, Tag>();
        }
    }
}