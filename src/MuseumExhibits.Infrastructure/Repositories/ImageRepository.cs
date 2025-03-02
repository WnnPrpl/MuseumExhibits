using Microsoft.EntityFrameworkCore;
using MuseumExhibits.Core.Abstractions;
using MuseumExhibits.Core.Models;
using MuseumExhibits.Infrastructure.Data;

namespace MuseumExhibits.Infrastructure.Repostories
{
    public class ImageRepository : IImageRepository
    {
        private readonly MuseumExhibitsDbContext _context;

        public ImageRepository(MuseumExhibitsDbContext context)
        {
            _context = context;
        }

        public async Task<Image> GetByIdAsync(Guid id)
        {
            return await _context.Images
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<IEnumerable<Image>> GetByEntityIdAsync(Guid entityId)
        {
            return await _context.Images
                .Where(i => i.ExhibitId == entityId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task <Guid> CreateAsync(Image image)
        {
            _context.Images.Add(image);
            await _context.SaveChangesAsync();
            return image.Id;
        }

        public async Task SetTitleImageAsync(Guid entityId, Guid imageId)
        {
            var images = await _context.Images
                .Where(i => i.ExhibitId == entityId)
                .ToListAsync();

            var imageToSet = images.FirstOrDefault(i => i.Id == imageId);
            if (imageToSet == null)
            {
                throw new KeyNotFoundException($"Image with ID {imageId} not found for {entityId}.");
            }

            foreach (var image in images)
            {
                image.IsTitleImage = false;
            }

            imageToSet.IsTitleImage = true;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var image = await GetByIdAsync(id);
            if (image == null)
            {
                throw new KeyNotFoundException($"Image with ID {id} not found.");
            }

            _context.Images.Remove(image);
            await _context.SaveChangesAsync();
        }

        public async Task<Image> GetTitleImageByEntityIdAsync(Guid entityId)
        {
            return await _context.Images
                .Where(i => i.ExhibitId == entityId && i.IsTitleImage) 
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public async Task<ITransaction> BeginTransactionAsync()
        {
            var transaction = await _context.Database.BeginTransactionAsync();
            if (transaction == null)
            {
                throw new InvalidOperationException("Failed to begin transaction.");
            }
            return new EfCoreTransaction(transaction);
        }
    }
}
