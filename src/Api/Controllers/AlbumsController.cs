using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Common.Models;
using Application.DTOs;
using Application.Services;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlbumsController(IAlbumService albumService) : ControllerBase
    {

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<AlbumDto>>> GetAll()
        {
            var albums = await albumService.GetAllAsync();
            return Ok(albums);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<AlbumDto>> GetById(int id)
        {
            var album = await albumService.GetByIdAsync(id);
            if (album == null)
            {
                return NotFound();
            }
            return Ok(album);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<AlbumDto>> Create(AlbumDto albumDto)
        {
            var createdAlbum = await albumService.CreateAsync(albumDto);
            return CreatedAtAction(nameof(GetById), new { id = createdAlbum.AlbumId }, createdAlbum);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<AlbumDto>> Update(int id, AlbumDto albumDto)
        {
            if (id != albumDto.AlbumId)
            {
                return BadRequest();
            }
            var updatedAlbum = await albumService.UpdateAsync(albumDto);
            return Ok(updatedAlbum);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> Delete(int id)
        {
            await albumService.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("paged")]
        [Authorize]
        public async Task<ActionResult<PaginatedResult<AlbumDto>>> GetPaged([FromQuery] QueryParameters parameters)
        {
            var pagedAlbums = await albumService.GetPagedAsync(parameters);
            return Ok(pagedAlbums);
        }

    }
}

