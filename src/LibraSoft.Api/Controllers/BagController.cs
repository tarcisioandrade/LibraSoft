using System.Security.Claims;
using LibraSoft.Core.Commons;
using LibraSoft.Core.Exceptions;
using LibraSoft.Core.Interfaces;
using LibraSoft.Core.Requests.Bag;
using LibraSoft.Core.Responses.Bag;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraSoft.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public class BagController : ControllerBase
    {
        private readonly IBagHandler _handler;
        public BagController(IBagHandler handler)
        {
            _handler = handler;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(CreateBagRequest request)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var bags = await _handler.GetAllAsync(userId);

            if (bags.Count >= 5) return BadRequest(new BagLimitExcedeed());

            var exists = bags.Any(b => b.BookId == request.BookId);
            if (exists) return NoContent();

            await _handler.CreateAsync(request, userId);
            return Created();
        }

        [HttpGet]
        [ProducesResponseType(typeof(Response<IEnumerable<BagResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var bagsInDb = await _handler.GetAllAsync(userId);
            var bags = bagsInDb.Select(b =>
            {
                return new BagResponse
                {
                    Id = b.Id,
                    Book = new BookInBag
                    {
                        Id = b.Book.Id,
                        Title = b.Book.Title,
                        AverageRating = b.Book.AverageRating,
                        CopiesAvaliable = b.Book.CopiesAvailable,
                        AuthorName = b.Book.Author.Name,
                        Image = b.Book.Image,
                        Publisher = b.Book.Publisher,
                        CoverType = b.Book.CoverType,
                        Status = b.Book.Status
                    },
                    CreatedAt = b.CreatedAt
                };
            });
            var response = new Response<IEnumerable<BagResponse>>(bags);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var bag = await _handler.GetAsync(id);
            if (bag is null) return BadRequest(new BagNotFound());
            await _handler.DeleteAsync(bag);
            return NoContent();
        }
    }
}
