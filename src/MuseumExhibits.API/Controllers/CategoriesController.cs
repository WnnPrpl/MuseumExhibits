using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MuseumExhibits.Application.DTO;
using MuseumExhibits.Application.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.RateLimiting;

namespace MuseumExhibits.API.Controllers
{
    [ApiController]
    [Route("api/categories")]
    [EnableRateLimiting("GlobalLimiter")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService service)
        {
            _categoryService = service;
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CategoryDTO>> GetById(Guid id)
        {
            var category = await _categoryService.GetById(id);
            return Ok(category);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetAll()
        {
            var categories = await _categoryService.GetAll();
            return Ok(categories);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CategoryDTO categoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            categoryDto.Id = Guid.NewGuid();

            Guid categoryId = await _categoryService.Create(categoryDto);
            return CreatedAtAction(nameof(GetById), new { id = categoryId }, categoryId);
        }

        [Authorize]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] CategoryDTO categoryDto)
        {
            categoryDto.Id = id;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _categoryService.Update(id, categoryDto);
            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _categoryService.Delete(id);
            return NoContent();
        }

        [HttpGet("paged")]
        public async Task<ActionResult> GetByPage([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            if (page < 1 || pageSize < 1)
                return BadRequest("Page and page size must be greater than zero.");

            var paginatedCategories = await _categoryService.GetByPage(page, pageSize);
            return Ok(paginatedCategories);
        }
    }

}
