using System.ComponentModel.DataAnnotations;

namespace MySongs.Repository.Entities
{
    public class Songs
    {
        [Key]
        public int SongId { get; set; }

        [Required(ErrorMessage = "שם השיר הוא שדה חובה")]
        [MaxLength(100, ErrorMessage = "שם השיר לא יכול להיות ארוך מ-100 תווים")]
        public string Title { get; set; } = "";

        [Required(ErrorMessage = "שם האמן הוא שדה חובה")]
        [MaxLength(100, ErrorMessage = "שם האמן לא יכול להיות ארוך מ-100 תווים")]
        public string ArtistName { get; set; } = "";

        [Required(ErrorMessage = "ז'אנר הוא שדה חובה")]
        [MaxLength(50, ErrorMessage = "ז'אנר לא יכול להיות ארוך מ-50 תווים")]
        public string Genre { get; set; } = "";

        public DateTime ReleaseDate { get; set; }

        [MaxLength(500, ErrorMessage = "תיאור לא יכול להיות ארוך מ-500 תווים")]
        public string? LyricsSummary { get; set; }

        [MaxLength(300, ErrorMessage = "כתובת האודיו לא יכולה להיות ארוכה מ-300 תווים")]
        public string? AudioUrl { get; set; }

        [MaxLength(32, ErrorMessage = "Hash לא יכול להיות ארוך מ-32 תווים")]
        public string? FileHash { get; set; }

        public virtual ICollection<SongTag> SongTags { get; set; }
    }
}