using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using MuseumExhibits.Application.Abstractions;
using MuseumExhibits.Application.DTO;
using MuseumExhibits.Core.Models;


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
        public async Task<IActionResult> GetExhibits([FromQuery] ExhibitQueryParameters queryParams)
        {
            try
            {
                var pagedResult = await _exhibitService.Get(queryParams);
                return Ok(pagedResult);
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An unexpected error occurred.", Details = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ExhibitRequest exhibitDto)
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
        public async Task<IActionResult> Update(Guid id, [FromBody] ExhibitRequest exhibitDto)
        {

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

        [HttpPatch("{id:guid}")]
        [Consumes("application/json-patch+json")]
        public async Task<IActionResult> PatchExhibit(Guid id, [FromBody] JsonPatchDocument<ExhibitRequest> patchDoc)
        {

            if (patchDoc == null)
                return BadRequest("Invalid patch data.");

            try
            {
                await _exhibitService.PartialUpdate(id, patchDoc);
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


    }
}
