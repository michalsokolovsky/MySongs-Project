using Microsoft.EntityFrameworkCore;
using MySongs.Repository.Entities;

namespace MySongs.Repository.Interfaces
{
    public interface IContext
    {
        DbSet<Songs> Songs { get; set; }
        DbSet<Tag> Tags { get; set; }
        DbSet<SongTag> SongTags { get; set; }
        DbSet<Users> Users { get; set; }
        DbSet<Recommendation> Recommendations { get; set; }
        DbSet<ListeningHistory> ListeningHistorys { get; set; }
        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}