using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MuseumExhibits.Application.Abstractions;
using MuseumExhibits.Application.DTO;


namespace MuseumExhibits.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExhibitsController : ControllerBase
    {
        private readonly IExhibitService _exhibitService;

        public ExhibitsController(IExhibitService exhibitService)
        {
            _exhibitService = exhibitService;
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var exhibit = await _exhibitService.GetById(id);
                return Ok(exhibit);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var exhibits = await _exhibitService.GetAll();
            return Ok(exhibits);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ExhibitDTO exhibitDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
                        try
            {
                var id = await _exhibitService.Create(exhibitDto);
                return CreatedAtAction(nameof(GetById), new { id }, id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ExhibitDTO exhibitDto)
        {
            if (id != exhibitDto.Id)
            {
                return BadRequest("ID mismatch.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _exhibitService.Update(id, exhibitDto);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _exhibitService.Delete(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("category/{categoryId:guid}")]
        public async Task<IActionResult> GetByCategoryId(Guid categoryId)
        {
            var exhibits = await _exhibitService.GetByCategoryId(categoryId);
            return Ok(exhibits);
        }

        [HttpGet("page")]
        public async Task<IActionResult> GetByPage(int page = 1, int pageSize = 10)
        {
            if (page <= 0 || pageSize <= 0)
            {
                return BadRequest("Page and page size must be positive integers.");
            }

            var exhibits = await _exhibitService.GetByPage(page, pageSize);
            return Ok(exhibits);
        }
    }
}
