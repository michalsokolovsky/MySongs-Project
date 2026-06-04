namespace MySongs.Common.DTOs
{
    public class RecommendationDto
    {
        public int RecommendationId { get; set; }
        public int UserId { get; set; }
        public int SongId { get; set; }
        public double Score { get; set; }
        public bool IsSeen { get; set; }
        public DateTime RecommendedAt { get; set; }

    }
}