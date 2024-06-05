using LibraSoft.Core.Exceptions;
using LibraSoft.Core.Interfaces;
using LibraSoft.Core.Requests.Author;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraSoft.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("admin")]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorHandler _handler;

        public AuthorController(IAuthorHandler authorHandler)
        {
            _handler = authorHandler;   
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(CreateAuthorRequest req)
        {
            var exists = await _handler.GetByNameAsync(req.Name);

            if (exists is not null)
            {
                return BadRequest(new AuthorAlreadyExistsError(req.Name));
            }

            await _handler.CreateAsync(req);

            return Created();
        }   
    }
}
