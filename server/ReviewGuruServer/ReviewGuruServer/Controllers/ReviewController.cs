using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReviewGuru.BLL.DTOs;
using ReviewGuru.BLL.Services;
using ReviewGuru.BLL.Services.IServices;
using ReviewGuru.BLL.Utilities.Constants;
using ReviewGuru.DAL.Entities.Models;

namespace ReviewGuru.API.Controllers
{
    [Route("api/review")]
    [ApiController]
    public class ReviewController(IReviewService reviewService) : ControllerBase
    {
        private readonly IReviewService _reviewService = reviewService;

        [HttpGet]
        [ActionName("GetAllReviews")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetReviewListAsync(
        int pageNumber = Pagination.PageNumber,
        int pageSize = Pagination.PageSize,
        string searchText = "",
        string mediaType = "",
        int? minRating = null,
        int? maxRating = null,
        CancellationToken cancellationToken = default)
        {
            var reviews = await _reviewService.GetAllAsync(pageNumber, pageSize, searchText, mediaType, cancellationToken : cancellationToken);

            return Ok(reviews.ToList());
        }

        [HttpPost]
        [ActionName("CreateReview")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateReviewAsync([FromBody] ReviewDTO reviewDTO, CancellationToken cancellationToken = default)
        {
            await _reviewService.CreateAsync(reviewDTO, cancellationToken : cancellationToken);
            return Created();
        }

        [HttpPut]
        [ActionName("UpdateReview")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateReviewAsync([FromBody] ReviewDTO reviewToUpdateDTO, CancellationToken cancellationToken = default)
        {
            await _reviewService.UpdateAsync(reviewToUpdateDTO, cancellationToken);
            return Ok(reviewToUpdateDTO);
        }

        [HttpDelete]
        [ActionName("DeleteReview")]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteReviewAsync([FromRoute] int id, CancellationToken cancellationToken = default)
        {
            await _reviewService.DeleteAsync(id, cancellationToken);
            return Ok();
        }
    }

}
