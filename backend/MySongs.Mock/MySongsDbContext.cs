using Microsoft.EntityFrameworkCore;
using MySongs.Repository.Entities;
using MySongs.Repository.Interfaces;

namespace MySongs.Mock
{
    public class MySongsDbContext : DbContext, IContext
    {
        public MySongsDbContext() { }
        public MySongsDbContext(DbContextOptions<MySongsDbContext> options) : base(options) { }

        public DbSet<Songs> Songs { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<SongTag> SongTags { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Recommendation> Recommendations { get; set; }
        public DbSet<ListeningHistory> ListeningHistorys { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=MySongsDB;Trusted_Connection=True;");
            }
        }
    }
}