using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MuseumExhibits.Application.Abstractions;
using MuseumExhibits.Application.DTO;

namespace MuseumExhibits.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImagesController : ControllerBase
    {
        private readonly IImageService _imageService;

        public ImagesController(IImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpGet("{entityId}")]
        public async Task<IActionResult> GetByEntityId(Guid entityId)
        {
            var images = await _imageService.GetByEntityId(entityId);
            var imageResponses = images.Select(img => new ImageResponse
            {
                Id = img.Id,
                Url = img.Url,
                IsTitleImage = img.IsTitleImage
            });

            return Ok(imageResponses);
        }

        [HttpPost("{entityId}")]
        public async Task<IActionResult> UploadImage(Guid entityId, [FromForm] ImageRequest fileDTO)
        {
            var imageResponse = await _imageService.UploadImage(entityId, fileDTO);
            return CreatedAtAction(nameof(GetByEntityId), new { entityId = entityId }, imageResponse);
        }

        [HttpDelete("{imageId}")]
        public async Task<IActionResult> DeleteImage(Guid imageId)
        {
            await _imageService.DeleteImage(imageId);
            return NoContent();
        }

        [HttpDelete("entity/{entityId}")]
        public async Task<IActionResult> DeleteImagesByExhibit(Guid entityId)
        {
            await _imageService.DeleteByEntityId(entityId);
            return NoContent();
        }

        [HttpPut("{entityId}/title/{imageId}")]
        public async Task<IActionResult> SetTitleImage(Guid entityId, Guid imageId)
        {
            await _imageService.SetTitleImage(entityId, imageId);
            return NoContent();
        }
    }

}
