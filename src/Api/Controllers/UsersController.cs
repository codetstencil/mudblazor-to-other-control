using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Common.Models;
using Application.DTOs;
using Application.Services;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService ?? throw new ArgumentNullException(nameof(userService));

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<UserDto>> GetById(string id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Only allow users to access their own data unless they're an admin
            if (User.Identity?.Name != user.Username && !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            return Ok(user);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<UserDto>> Create(UserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdUser = await _userService.CreateAsync(userDto);
            return CreatedAtAction(
                nameof(GetById),
                new { id = createdUser.Id },
                createdUser);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<UserDto>> Update(string id, UserDto userDto)
        {
            if (id != userDto.Id)
            {
                return BadRequest("ID mismatch");
            }

            // Only allow users to update their own data unless they're an admin
            if (User.Identity?.Name != userDto.Username && !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            var updatedUser = await _userService.UpdateAsync(userDto);
            return Ok(updatedUser);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> Delete(string id)
        {
            await _userService.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("paged")]
        [Authorize]
        public async Task<ActionResult<PaginatedResult<UserDto>>> GetPaged([FromQuery] QueryParameters parameters)
        {
            var pagedUsers = await _userService.GetPagedAsync(parameters);
            return Ok(pagedUsers);
        }

        [HttpGet("username/{username}")]
        [Authorize]
        public async Task<ActionResult<UserDto>> GetByUsername(string username)
        {
            var user = await _userService.GetByUsernameAsync(username);
            if (user == null)
            {
                return NotFound();
            }

            // Only allow users to access their own data unless they're an admin
            if (User.Identity?.Name != username && !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            return Ok(user);
        }
    }
}
