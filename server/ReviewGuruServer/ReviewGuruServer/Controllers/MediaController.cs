using Microsoft.AspNetCore.Mvc;
using ReviewGuru.BLL.DTOs;
using ReviewGuru.BLL.Services.IServices;
using ReviewGuru.BLL.Utilities.Constants;

namespace ReviewGuru.API.Controllers
{
    [Route("api/media")]
    [ApiController]
    public class MediaController(IMediaService mediaService) : ControllerBase
    {
        private readonly IMediaService _mediaService = mediaService;

        [HttpGet]
        [ActionName("GetAllMedia")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllMediaAsync(CancellationToken cancellationToken, int pageNumber = Pagination.PageNumber, int pageSize = Pagination.PageSize)
        {
            var media = await _mediaService.GetAllAsync(pageNumber, pageSize, cancellationToken);
            return Ok(media.ToList());
        }

        [HttpPost]
        [ActionName("CreateMedia")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateMediaAsync([FromBody] MediaDTO mediaDTO, CancellationToken cancellationToken = default)
        {
            await _mediaService.CreateAsync(mediaDTO, cancellationToken);
            return Created();
        }

        [HttpPut]
        [ActionName("UpdateMedia")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateMediaAsync([FromBody] MediaDTO mediaToUpdateDTO, CancellationToken cancellationToken = default)
        {
            await _mediaService.UpdateAsync(mediaToUpdateDTO, cancellationToken);
            return Ok(mediaToUpdateDTO);
        }

        [HttpDelete]
        [ActionName("DeleteMedia")]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteMediaAsync([FromRoute] int id, CancellationToken cancellationToken = default)
        {
            await _mediaService.DeleteAsync(id, cancellationToken);
            return Ok();
        }
    }

}
