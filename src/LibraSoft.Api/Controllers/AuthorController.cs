using LibraSoft.Core;
using LibraSoft.Core.Exceptions;
using LibraSoft.Core.Interfaces;
using LibraSoft.Core.Commons;
using LibraSoft.Core.Requests.Author;
using LibraSoft.Core.Responses.Author;
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

        [HttpDelete("{id}/delete")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize("admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var author = await _handler.GetByIdAsync(id);

            if (author is null)
            {
                return BadRequest(new AuthorNotFoundError());
            }

            if (author.HasBooks() is true)
            {
                return BadRequest(new AuthorHasBookAssociatedError());
            }

            await _handler.DeleteAsync(author);

            return NoContent();
        }

        [HttpDelete("{id}/inactive")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize("admin")]
        public async Task<IActionResult> Inactive(Guid id)
        {
            var author = await _handler.GetByIdAsync(id);

            if (author is null)
            {
                return BadRequest(new AuthorNotFoundError());
            }

            await _handler.InactiveAsync(author);

            return NoContent();
        }

        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(bool includeInactive,
                                                string? search,
                                                int pageNumber = Configuration.DefaultPageNumber,
                                                int pageSize = Configuration.DefaultPageSize)
        {
            var request = new GetAllAuthorRequest
            {
                IncludeInactive = includeInactive,
                Search = search,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var response = await _handler.GetAll(request);

            return Ok(response);
        }

        [HttpPatch("{id}")]
        [Authorize("admin")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Update(UpdateAuthorRequest req, Guid id)
        {
            var author = await _handler.GetByIdAsync(id);

            if (author is null)
            {
                return BadRequest(new AuthorNotFound());
            }
            await _handler.Update(req, author);
            return NoContent();
        }

        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(AuthorResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(Guid id)
        {
            var author = await _handler.GetByIdAsync(id);

            if (author is null)
            {
                return BadRequest(new AuthorNotFound());
            }

            var response = new AuthorResponse
            {
                Id = author.Id,
                Biography = author.Biography,
                DateBirth = author.DateBirth,
                Name = author.Name,
                Status = author.Status
            };

            return Ok(new Response<AuthorResponse>(response));
        }
    }
}
