using MySongs.Common.DTOs;
using System.ComponentModel.DataAnnotations;

namespace MySongs.Repository.Entities
{
    public class Tag
    {
        [Key]
        public int TagId { get; set; }

        [Required(ErrorMessage = "שם התגית הוא שדה חובה")]
        [MaxLength(50, ErrorMessage = "שם התגית לא יכול להיות ארוך מ-50 תווים")]
        public string TagName { get; set; }

        public TagType TagType { get; set; } = TagType.General;

        public virtual ICollection<SongTag> SongTags { get; set; }
    }
}