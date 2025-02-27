using Microsoft.EntityFrameworkCore;
using MuseumExhibits.Core.Models;

namespace MuseumExhibits.Infrastructure.Data
{
    public class MuseumExhibitsDbContext : DbContext
    {
        public MuseumExhibitsDbContext(DbContextOptions<MuseumExhibitsDbContext> options) : base(options)
        {
        }

        public DbSet<Exhibit> Exhibits { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>()
                .HasIndex(c => c.Name)
                .IsUnique();

            modelBuilder.Entity<Exhibit>()
                .HasOne(e => e.Category)
                .WithMany(c => c.Exhibits)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Image>()
                .HasOne(i => i.Exhibit)
                .WithMany(e => e.Images)
                .HasForeignKey(i => i.ExhibitId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
