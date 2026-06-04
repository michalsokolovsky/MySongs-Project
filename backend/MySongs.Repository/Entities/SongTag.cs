using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Microsoft.EntityFrameworkCore;

namespace MySongs.Repository.Entities
{
    [PrimaryKey(nameof(SongId), nameof(TagId))]
    public class SongTag
    {
        // מפתח זר לשיר - בלי תגית [Key]
        public int SongId { get; set; }

        [ForeignKey("SongId")]
        public virtual Songs Song { get; set; }

        // מפתח זר לתגית - בלי תגית [Key]
        public int TagId { get; set; }

        [ForeignKey("TagId")]
        public virtual Tag Tag { get; set; }
    }
}