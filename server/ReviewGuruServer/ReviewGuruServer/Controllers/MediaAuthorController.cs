using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReviewGuru.BLL.DTOs;
using ReviewGuru.BLL.Services.IServices;
using ReviewGuru.BLL.Utilities.Constants;

namespace ReviewGuru.API.Controllers
{
    [Route("api/mediaAuthor")]
    [ApiController]
    public class MediaAuthorController(IMediaAuthorService mediaAuthorService) : ControllerBase
    {
        private readonly IMediaAuthorService _mediaAuthorService = mediaAuthorService;

        [HttpGet]
        [ActionName("GetAllMediaAuthors")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllMediaAuthorsAsync(CancellationToken cancellationToken, int pageNumber = Pagination.PageNumber, int pageSize = Pagination.PageSize)
        {
            var mediaAuthors = await _mediaAuthorService.GetAllAsync(pageNumber, pageSize, cancellationToken);
            return Ok(mediaAuthors.ToList());
        }

        [HttpPost]
        [ActionName("CreateMediaAuthor")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateMediaAuthorAsync([FromBody] MediaAuthorDTO mediaAuthorDTO, CancellationToken cancellationToken = default)
        {
            await _mediaAuthorService.CreateAsync(mediaAuthorDTO, cancellationToken);
            return Created();
        }

        [HttpPut]
        [ActionName("UpdateMediaAuthor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateMediaAuthorAsync([FromBody] MediaAuthorDTO mediaAuthorToUpdateDTO, CancellationToken cancellationToken = default)
        {
            await _mediaAuthorService.UpdateAsync(mediaAuthorToUpdateDTO, cancellationToken);
            return Ok(mediaAuthorToUpdateDTO);
        }

        [HttpDelete]
        [ActionName("DeleteMediaAuthor")]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteMediaAuthorAsync([FromRoute] int id, CancellationToken cancellationToken = default)
        {
            await _mediaAuthorService.DeleteAsync(id, cancellationToken);
            return Ok();
        }
    }

}
