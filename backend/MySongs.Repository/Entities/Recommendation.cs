using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MySongs.Repository.Entities;

namespace MySongs.Repository.Entities
{
    public class Recommendation
    {
        [Key]
        public int RecommendationId { get; set; }

        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual Users User { get; set; }

        public int SongId { get; set; }
        [ForeignKey("SongId")]
        public virtual Songs Song { get; set; }

        public double Score { get; set; } // ציון התאמה (0.0 עד 1.0)
        public bool IsSeen { get; set; }
    }
}