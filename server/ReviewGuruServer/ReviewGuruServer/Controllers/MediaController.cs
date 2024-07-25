using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReviewGuru.BLL.DTOs;
using ReviewGuru.BLL.Services.IServices;
using ReviewGuru.BLL.Utilities.Constants;

namespace ReviewGuru.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediaController(IMediaService mediaService) : ControllerBase
    {
        private readonly IMediaService _mediaService = mediaService;

        [HttpGet]
        [AllowAnonymous]
        [ActionName("GetAllMedia")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllMediaAsync(
            int pageNumber = Pagination.PageNumber,
            int pageSize = Pagination.PageSize,
            string searchText = "",
            string mediaType = "",
            CancellationToken cancellationToken = default)
        {
            var media = await _mediaService.GetMediaListAsync(pageNumber, pageSize, searchText, mediaType, cancellationToken);

            int totalMediaCount = await _mediaService.GetMediaCountAsync(searchText, mediaType, cancellationToken);

            return Ok(new { totalMediaCount, media });
        }

        [HttpGet("{mediaId}")]
        [AllowAnonymous]
        [ActionName("GetMediaData")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMediaAsync(int mediaId, CancellationToken cancellationToken = default)
        {
            var media = await _mediaService.GetMediaWithReviewsAsync(mediaId, cancellationToken);

            return Ok(media);
        }

        [HttpPost("{mediaId}/add-to-favorites")]
        [ActionName("AddMediaToFavorites")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddMediaToFavorites(int mediaId, CancellationToken cancellationToken = default)
        {
            int userId = int.Parse(HttpContext.User.FindFirst("Id")!.Value);

            await _mediaService.AddMediaToFavoritesAsync(userId, mediaId, cancellationToken);

            return Ok();
        }

        [HttpPost("{mediaId}/remove-from-favorites")]
        [ActionName("RemoveMediaFromFavorites")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveMediaFromFavorites(int mediaId, CancellationToken cancellationToken = default)
        {
            int userId = int.Parse(HttpContext.User.FindFirst("Id")!.Value);

            await _mediaService.RemoveMediaFromFavoritesAsync(userId, mediaId, cancellationToken);

            return Ok();
        }

        [HttpPost("AddMedia")]
        [ActionName("AddMedia")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddMediaAsync([FromBody] MediaToCreateYDTO mediaDto, CancellationToken cancellationToken)
        {
            await _mediaService.AddMediaAsync(mediaDto, cancellationToken);

            return Created();
        }
    }
}
