using MySongs.Common.DTOs;
using MySongs.Repository.Entities;

namespace MySongs.Services
{
    public class Mapper
    {
        // Songs
        public SongDto ToDto(Songs song)
        {
            return new SongDto
            {
                SongId = song.SongId,
                Title = song.Title,
                ArtistName = song.ArtistName,
                Genre = song.Genre,
                ReleaseDate = song.ReleaseDate,
                LyricsSummary = song.LyricsSummary
            };
        }

        public Songs ToEntity(SongDto dto)
        {
            return new Songs
            {
                SongId = dto.SongId,
                Title = dto.Title,
                ArtistName = dto.ArtistName,
                Genre = dto.Genre,
                ReleaseDate = dto.ReleaseDate,
                LyricsSummary = dto.LyricsSummary
            };
        }

        // Tags
        public TagDto ToDto(Tag tag)
        {
            return new TagDto { TagId = tag.TagId, TagName = tag.TagName };
        }

        public Tag ToEntity(TagDto dto)
        {
            return new Tag { TagId = dto.TagId, TagName = dto.TagName };
        }

        // Users
        public UserDto ToDto(Users user)
        {
            return new UserDto
            {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email,
                CreatedAt = user.CreatedAt
            };
        }

        public Users ToEntity(UserDto dto)
        {
            return new Users
            {
                UserId = dto.UserId,
                Username = dto.Username,
                Email = dto.Email,
                CreatedAt = dto.CreatedAt
            };
        }

        // Recommendation
        public RecommendationDto ToDto(Recommendation recommendation)
        {
            return new RecommendationDto
            {
                RecommendationId = recommendation.RecommendationId,
                UserId = recommendation.UserId,
                SongId = recommendation.SongId,
                Score = recommendation.Score,
                IsSeen = recommendation.IsSeen
            };
        }

        public Recommendation ToEntity(RecommendationDto dto)
        {
            return new Recommendation
            {
                RecommendationId = dto.RecommendationId,
                UserId = dto.UserId,
                SongId = dto.SongId,
                Score = dto.Score,
                IsSeen = dto.IsSeen
            };
        }

        // ListeningHistory
        public ListeningHistoryDto ToDto(ListeningHistory history)
        {
            return new ListeningHistoryDto
            {
                HistoryId = history.HistoryId,
                UserId = history.UserId,
                SongId = history.SongId,
                ListenDate = history.ListenDate,
                Duration = history.Duration
            };
        }

        public ListeningHistory ToEntity(ListeningHistoryDto dto)
        {
            return new ListeningHistory
            {
                HistoryId = dto.HistoryId,
                UserId = dto.UserId,
                SongId = dto.SongId,
                ListenDate = dto.ListenDate,
                Duration = dto.Duration
            };
        }

        // SongTag
        public SongTagDto ToDto(SongTag songTag)
        {
            return new SongTagDto { SongId = songTag.SongId, TagId = songTag.TagId };
        }

        public SongTag ToEntity(SongTagDto dto)
        {
            return new SongTag { SongId = dto.SongId, TagId = dto.TagId };
        }
    }
}