using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using MuseumExhibits.Application.Abstractions;
using MuseumExhibits.Application.DTO;


namespace MuseumExhibits.API.Controllers
{
    [ApiController]
    [Route("api/exhibits")]
    [EnableRateLimiting("GlobalLimiter")]
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
            var exhibit = await _exhibitService.GetById(id);
            return Ok(exhibit);
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] ExhibitQueryParameters queryParams)
        {
            bool isAdmin = User.Identity?.IsAuthenticated == true;
            var pagedResult = await _exhibitService.Get(queryParams, isAdmin);
            return Ok(pagedResult);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ExhibitRequest exhibitDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var id = await _exhibitService.Create(exhibitDto);
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        [Authorize]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ExhibitRequest exhibitDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _exhibitService.Update(id, exhibitDto);
            return NoContent();
        }

        [Authorize]
        [HttpPatch("{id:guid}")]
        [Consumes("application/json-patch+json")]
        public async Task<IActionResult> Patch(Guid id, [FromBody] JsonPatchDocument<ExhibitRequest> patchDoc)
        {
            if (patchDoc == null)
                return BadRequest("Invalid patch data.");

            await _exhibitService.PartialUpdate(id, patchDoc);
            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _exhibitService.Delete(id);
            return NoContent();
        }

    }
}
