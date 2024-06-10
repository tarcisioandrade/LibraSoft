using System.Security.Claims;
using LibraSoft.Core.Interfaces;
using LibraSoft.Core.Requests.User;
using LibraSoft.Core.Responses.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LibraSoft.Core.Exceptions;

namespace LibraSoft.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserHandler _handler;
        public UserController(IUserHandler userHandler)
        {
            _handler = userHandler;
        }


        [HttpPatch("{id}/suspense")]
        [Authorize("admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Suspense(Guid id)
        {
            await _handler.SuspenseAsync(id);
            return NoContent();
        }

        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get()
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var user = await _handler.GetByIdAsync(userId);

            if (user is null)
            {
                return BadRequest(new UserNotFoundError());
            }

            var response = new UserResponse
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Telephone = user.Telephone,
                Address = user.Address,
                Role = user.Role,
                UserStatus = user.Status,
            };

            return Ok(response);
        }

        [HttpPatch]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Update(UserUpdateRequest req)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            await _handler.UpdateAsync(req, userId);
            return NoContent();
        }
    }
}
