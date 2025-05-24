using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using MuseumExhibits.Application.Abstractions;
using MuseumExhibits.Application.DTO;

namespace MuseumExhibits.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    [EnableRateLimiting("LoginLimiter")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        //[HttpPost("register")]
        //public async Task<ActionResult> Register([FromBody] RegisterRequest request)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    try
        //    {
        //        var token = await _authService.RegisterAsync(request);
        //        return Ok(new { Token = token });
        //    }
        //    catch (Exception ex)
        //    {
        //        if (ex.Message.Contains("already exist"))
        //            return Conflict(new { Error = ex.Message });

        //        return StatusCode(500, new { message = ex.Message });
        //    }
        //}

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var token = await _authService.LoginAsync(request);
                if (string.IsNullOrEmpty(token))
                    return Unauthorized(new { Error = "Invalid email or password." });

                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
