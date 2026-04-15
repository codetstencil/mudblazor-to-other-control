using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Common.Models;
using Application.DTOs;
using Application.Services;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArtistsController(IArtistService artistService) : ControllerBase
    {

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ArtistDto>>> GetAll()
        {
            var artists = await artistService.GetAllAsync();
            return Ok(artists);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<ArtistDto>> GetById(int id)
        {
            var artist = await artistService.GetByIdAsync(id);
            if (artist == null)
            {
                return NotFound();
            }
            return Ok(artist);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ArtistDto>> Create(ArtistDto artistDto)
        {
            var createdArtist = await artistService.CreateAsync(artistDto);
            return CreatedAtAction(nameof(GetById), new { id = createdArtist.ArtistId }, createdArtist);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<ArtistDto>> Update(int id, ArtistDto artistDto)
        {
            if (id != artistDto.ArtistId)
            {
                return BadRequest();
            }
            var updatedArtist = await artistService.UpdateAsync(artistDto);
            return Ok(updatedArtist);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> Delete(int id)
        {
            await artistService.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("paged")]
        [Authorize]
        public async Task<ActionResult<PaginatedResult<ArtistDto>>> GetPaged([FromQuery] QueryParameters parameters)
        {
            var pagedArtists = await artistService.GetPagedAsync(parameters);
            return Ok(pagedArtists);
        }

    }
}

