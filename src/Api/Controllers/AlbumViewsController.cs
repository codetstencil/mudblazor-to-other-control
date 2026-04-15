using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Common.Models;
using Application.DTOs;
using Application.Services;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlbumViewsController(IAlbumViewService albumViewService) : ControllerBase
    {
        private readonly IAlbumViewService _albumViewService =  albumViewService ?? throw new ArgumentNullException(nameof(albumViewService));

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<AlbumViewDto>>> GetAll()
        {
            var albumViews = await _albumViewService.GetAllAsync();
            return Ok(albumViews);
        }

        [HttpGet("paged")]
        [Authorize]
        public async Task<ActionResult<PaginatedResult<AlbumViewDto>>> GetPaged([FromQuery] QueryParameters parameters)
        {
            var pagedAlbumViews = await _albumViewService.GetPagedAsync(parameters);
            return Ok(pagedAlbumViews);
        }

        [HttpGet("search")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<AlbumViewDto>>> Search([FromQuery] string term)
        {
            if (string.IsNullOrWhiteSpace(term))
            {
                return BadRequest("Search term cannot be empty");
            }

            var albumViews = await _albumViewService.SearchAsync(term);
            return Ok(albumViews);
        }

/*
        [HttpGet("artist/{name}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<AlbumViewDto>>> GetByName(string name)
        {
            var albumViews = await albumViewService.GetByNameAsync(name);
            return Ok(albumViews);
        }
*/

    }
}

