using System.ComponentModel.DataAnnotations;

namespace MySongs.Repository.Entities
{
    public class Users
    {
        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "שם משתמש הוא שדה חובה")]
        [MaxLength(50, ErrorMessage = "שם משתמש לא יכול להיות ארוך מ-50 תווים")]
        public string Username { get; set; }

        [Required(ErrorMessage = "אימייל הוא שדה חובה")]
        [MaxLength(100, ErrorMessage = "אימייל לא יכול להיות ארוך מ-100 תווים")]
        public string Email { get; set; }

        [Required(ErrorMessage = "סיסמה היא שדה חובה")]
        [MaxLength(100)]
        public string Password { get; set; }

        [MaxLength(20)]
        public string Role { get; set; } = "User";

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public virtual ICollection<ListeningHistory> History { get; set; }
        public virtual ICollection<Recommendation> Recommendations { get; set; }
    }
}