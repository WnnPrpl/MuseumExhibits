using Microsoft.EntityFrameworkCore;
using MuseumExhibits.Core.Models;

namespace MuseumExhibits.Infrastructure.Data
{
    public class MuseumExhibitsDbContext : DbContext
    {
        public MuseumExhibitsDbContext(DbContextOptions<MuseumExhibitsDbContext> options) : base(options)
        {
        }

        public DbSet<Exhibit> Exhibit { get; set; }
        public DbSet<Image> Image { get; set; }
        public DbSet<Category> Category { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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
