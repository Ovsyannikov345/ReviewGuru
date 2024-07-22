using Microsoft.AspNetCore.Mvc;
using OMDbApiNet.Model;
using ReviewGuru.BLL.DTOs;
using ReviewGuru.BLL.Services;
using ReviewGuru.BLL.Services.IServices;


namespace ReviewGuru.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OMDbController(IOMDbService OMDbService) : ControllerBase
    {
        private readonly IOMDbService _OMDbService = OMDbService;

        [HttpPost]
        [Route("CreateReview")]
        [ActionName("CreateReview")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateReviewAsync([FromBody] ReviewToCreateAPIDTO reviewDTO, CancellationToken cancellationToken = default)
        {
            int userId = int.Parse(HttpContext.User.FindFirst("Id")!.Value);

            await _OMDbService.CreateWithAPIAsync(reviewDTO, userId, cancellationToken: cancellationToken);
            return Created();
        }
    }
}

