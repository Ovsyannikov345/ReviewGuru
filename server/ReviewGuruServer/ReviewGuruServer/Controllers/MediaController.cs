﻿using Microsoft.AspNetCore.Mvc;
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
        [ActionName("GetAllMedia")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllMediaAsync(
            CancellationToken cancellationToken = default,
            int pageNumber = Pagination.PageNumber,
            int pageSize = Pagination.PageSize,
            string searchText = "",
            string mediaType = "")
        {
            var media = await _mediaService.GetMediaListAsync(pageNumber, pageSize, searchText, mediaType, cancellationToken);

            return Ok(media);
        }
    }
}
