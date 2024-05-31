using LibraSoft.Core.Commons;
using LibraSoft.Core.Exceptions;
using LibraSoft.Core.Handlers;
using LibraSoft.Core.Requests.Users;
using Microsoft.AspNetCore.Mvc;

namespace LibraSoft.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create(CreateUserRequest req, IUserHandler handler)
        {
            var request = new GetByEmailRequest { Email = req.Email };

            var exists = await handler.GetByEmailAsync(request);

            if(exists is not null)
            {
                return BadRequest(new UserAlreadyExistsError("User already exists."));
            }

            await handler.CreateAsync(req);

            return Created();
        }
    }
}
