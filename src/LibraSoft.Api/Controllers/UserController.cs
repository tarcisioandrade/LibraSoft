using System.Security.Claims;
using LibraSoft.Api.Events;
using LibraSoft.Core.Commons;
using LibraSoft.Core.Exceptions;
using LibraSoft.Core.Interfaces;
using LibraSoft.Core.Requests.User;
using LibraSoft.Core.Responses.Punishment;
using LibraSoft.Core.Responses.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraSoft.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class UserController : ControllerBase
    {
        private readonly IUserHandler _handler;
        public UserController(IUserHandler userHandler)
        {
            _handler = userHandler;
        }


        [HttpPatch("{id}/inactive")]
        [Authorize("admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Inactive(Guid id)
        {
            await _handler.InactiveAsync(id);
            return NoContent();
        }

        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
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
                Status = user.Status,
            };

            return Ok(response);
        }

        [HttpPatch]
        [Authorize]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(UserUpdateRequest req)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var user = await _handler.GetByIdAsync(userId);
            if (user is null)
            {
                return NotFound(new UserNotFoundError());
            }
            var response = await _handler.UpdateAsync(req, user);
            return Ok(response);
        }

        [HttpPatch("change-password")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest req, ChangePasswordAlertEvent alertEvent)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var user = await _handler.GetByIdAsync(userId);
            if (user is null)
            {
                return NotFound(new UserNotFoundError());
            }

            var verifyPassword = BCrypt.Net.BCrypt.Verify(req.Password, user.Password);
            if (!verifyPassword)
            {
                return BadRequest(new UserOrPasswordIncorrectError());
            }

            await _handler.ChangePasswordAsync(req, user);
            alertEvent.Execute(user);

            return NoContent();
        }

        [HttpGet("punishments")]
        [Authorize]
        [ProducesResponseType(typeof(Response<List<PunishmentResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllPunishments()
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var user = await _handler.GetByIdAsync(userId);
            if (user is null)
            {
                return NotFound(new UserNotFoundError());
            }

            var response = user.PunishmentsDetails.Select(p => new PunishmentResponse
            {
                PunishInitDate = p.PunishInitDate,
                PunishEndDate = p.PunishEndDate,
                Status = p.Status
            }).OrderByDescending(p => p.PunishInitDate).ToList();

            return Ok(new Response<List<PunishmentResponse>>(response));
        }
    }
}
