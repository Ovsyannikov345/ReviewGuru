using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReviewGuru.BLL.DTOs;
using ReviewGuru.BLL.Services;
using ReviewGuru.BLL.Services.IServices;
using ReviewGuru.BLL.Utilities.Constants;
using ReviewGuru.DAL.Entities.Models;

namespace ReviewGuru.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController(IReviewService reviewService) : ControllerBase
    {
        private readonly IReviewService _reviewService = reviewService;

        [HttpGet]
        [Route("AllReviews")]
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
            var reviews = await _reviewService.GetAllAsync(pageNumber, pageSize, searchText, mediaType, minRating, maxRating, cancellationToken: cancellationToken);

            return Ok(reviews.ToList());
        }

        [HttpGet]
        [Route("MyReviews")]
        [ActionName("GetAllCurrentUserReviews")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMyReviewsListAsync(
        int pageNumber = Pagination.PageNumber,
        int pageSize = Pagination.PageSize,
        string searchText = "",
        string mediaType = "",
        int? minRating = null,
        int? maxRating = null,
        CancellationToken cancellationToken = default)
        {
            int userId = int.Parse(HttpContext.User.FindFirst("Id")!.Value);

            var reviews = await _reviewService.GetCurrentUserReviewsAsync(userId, pageNumber, pageSize, searchText, mediaType, minRating, maxRating, cancellationToken: cancellationToken);

            return Ok(reviews.ToList());
        }

        [HttpGet]
        [Route("OthersReviews")]
        [ActionName("GetAllExceptCurrentUserReviews")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllExceptMyReviewsListAsync(
        int pageNumber = Pagination.PageNumber,
        int pageSize = Pagination.PageSize,
        string searchText = "",
        string mediaType = "",
        int? minRating = null,
        int? maxRating = null,
        CancellationToken cancellationToken = default)
        {
            int userId = int.Parse(HttpContext.User.FindFirst("Id")!.Value);

            var reviews = await _reviewService.GetAllExceptCurrentUserReviewsAsync(userId, pageNumber, pageSize, searchText, mediaType, minRating, maxRating, cancellationToken: cancellationToken);

            return Ok(reviews.ToList());
        }

        [HttpPost]
        [Route("CreateReview")]
        [ActionName("CreateReview")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateReviewAsync([FromBody] ReviewToCreateDTO reviewDTO, CancellationToken cancellationToken = default)
        {
            int userId = int.Parse(HttpContext.User.FindFirst("Id")!.Value);

            await _reviewService.CreateAsync(reviewDTO, userId, cancellationToken : cancellationToken);
            return Created();
        }

        [HttpPut]
        [Route("ChangeReview")]
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
        [Route("RemoveReview/{id}")]
        [ActionName("DeleteReview")]
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
