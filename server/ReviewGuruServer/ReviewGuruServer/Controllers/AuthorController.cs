using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReviewGuru.BLL.DTOs;
using ReviewGuru.BLL.Services.IServices;
using ReviewGuru.BLL.Utilities.Constants;

namespace ReviewGuru.API.Controllers
{
    [Route("api/author")]
    [ApiController]
    public class AuthorController(IAuthorService authorService) : ControllerBase
    {
        private readonly IAuthorService _authorService = authorService;

        [HttpGet]
        [ActionName("GetAllAuthors")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllAuthorsAsync(CancellationToken cancellationToken, int pageNumber = Pagination.PageNumber , int pageSize = Pagination.PageSize)
        {
            var authors = await _authorService.GetAllAsync(pageNumber, pageSize, cancellationToken);
            return Ok(authors.ToList());
        }

        [HttpPost]
        [ActionName("CreateAuthor")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAuthorAsync([FromBody] AuthorDTO authorDTO, CancellationToken cancellationToken = default)
        {
            await _authorService.CreateAsync(authorDTO, cancellationToken);
            return Created();
        }

        [HttpPut]
        [ActionName("UpdateAuthor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateAuthorAsync([FromBody] AuthorDTO authorToUpdateDTO, CancellationToken cancellationToken = default)
        {
            await _authorService.UpdateAsync(authorToUpdateDTO, cancellationToken);
            return Ok(authorToUpdateDTO);
        }

        [HttpDelete]
        [ActionName("DeleteAuthor")]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAuthorAsync([FromRoute] int id, CancellationToken cancellationToken = default)
        {
            await _authorService.DeleteAsync(id, cancellationToken);
            return Ok();
        }
    }
}
