using Microsoft.EntityFrameworkCore;
using MuseumExhibits.Core.Models;

namespace MuseumExhibits.Infrastructure.Data
{
    public class MuseumExhibitsDbContext(DbContextOptions<MuseumExhibitsDbContext> options) : DbContext(options)
    {
        public DbSet<Exhibit> Exhibits { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Collection> Collections { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>()
                .HasIndex(c => c.Name)
                .IsUnique();

            modelBuilder.Entity<Exhibit>()
                .Property(e => e.Cost)
                .HasPrecision(18, 2);

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

            modelBuilder.Entity<Collection>()
                .HasMany(c => c.Exhibits)
                .WithMany(e => e.Collections)
                .UsingEntity(j => j.ToTable("CollectionExhibits"));
        }
    }
}
