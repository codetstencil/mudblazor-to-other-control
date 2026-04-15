using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.DTOs;
using Application.Services;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResponseDto>> Login(LoginDto loginDto)
        {
            try
            {
                var authResponse = await authService.LoginAsync(loginDto);
                if (authResponse.IsAuthenticated)
                {
                    return Ok(authResponse);
                }
                else
                {
                    return Unauthorized(authResponse);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new AuthResponseDto
                {
                    IsAuthenticated = false,
                    Message = $"Login failed: {ex.Message}"
                });
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await authService.LogoutAsync();
            return Ok();
        }
    }
}
