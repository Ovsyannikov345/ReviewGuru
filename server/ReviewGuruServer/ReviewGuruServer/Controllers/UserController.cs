using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReviewGuru.BLL.DTOs;
using ReviewGuru.BLL.Services.IServices;
using ReviewGuru.BLL.Utilities.Constants;

namespace ReviewGuru.API.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;

        [HttpGet]
        [ActionName("GetAllUsers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllUsersAsync(CancellationToken cancellationToken, int pageNumber = Pagination.PageNumber, int pageSize = Pagination.PageSize)
        {
            var users = await _userService.GetAllAsync(pageNumber, pageSize, cancellationToken);
            return Ok(users.ToList());
        }

        [HttpPost]
        [ActionName("CreateUser")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateUserAsync([FromBody] UserDTO userDTO, CancellationToken cancellationToken = default)
        {
            await _userService.CreateAsync(userDTO, cancellationToken);
            return Created();
        }

        [HttpPut]
        [ActionName("UpdateUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateUserAsync([FromBody] UserDTO userToUpdateDTO, CancellationToken cancellationToken = default)
        {
            await _userService.UpdateAsync(userToUpdateDTO, cancellationToken);
            return Ok(userToUpdateDTO);
        }

        [HttpDelete]
        [ActionName("DeleteUser")]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteUserAsync([FromRoute] int id, CancellationToken cancellationToken = default)
        {
            await _userService.DeleteAsync(id, cancellationToken);
            return Ok();
        }
    }

}
