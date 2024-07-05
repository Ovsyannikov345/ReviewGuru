//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using ReviewGuru.BLL.Services.IServices;
//using ReviewGuru.BLL.Utilities.Constants;

//namespace ReviewGuru.API.Controllers
//{
//    [Route("api/mediaAuthor")]
//    [ApiController]
//    public class MediaAuthorController(IMediaAuthorService mediaAuthorService) : ControllerBase
//    {
//        private readonly IMediaAuthorService _mediaAuthorService;

//        [HttpGet]
//        [ActionName("GetAllMediaAuthors")]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]

//        public async Task<IActionResult> GetAllAuthorsAsync(CancellationToken cancellationToken, int pageNumber = Pagination.PageNumber, int pageSize = Pagination.PageSize)
//        {

//        }
//    }
//}
