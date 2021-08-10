using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        { 
        }

        public DbSet<AppUser> users { get; set; }
        public DbSet<Photo> photos { get; set; }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            base.OnModelCreating(mb);

            mb.Entity<AppUser>()
                .HasMany(x => x.Photos)
                .WithOne(x => x.AppUser)
                .HasForeignKey(x => x.AppUserId);
        }
    }
}