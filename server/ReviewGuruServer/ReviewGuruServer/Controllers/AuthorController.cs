using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReviewGuru.BLL.Services.IServices;

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
        public async Task<IActionResult> GetAllAuthorsAsync(CancellationToken cancellationToken, int pageNumber = 1, int pageSize = 10)
        {
            var authors = await _authorService.GetAllAsync(pageNumber, pageSize, cancellationToken);
            return Ok(authors.ToList());
        }
    }
}
