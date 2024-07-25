using System.Security.Claims;
using LibraSoft.Core.Commons;
using LibraSoft.Core.Exceptions;
using LibraSoft.Core.Interfaces;
using LibraSoft.Core.Models;
using LibraSoft.Core.Requests.Bag;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraSoft.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        public async Task<IActionResult> Create(CreateBagRequest request)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var bags = await _handler.GetAllAsync(userId);
            if (bags?.Count >= 5)
            {
                return BadRequest(new BagLimitExcedeed());
            }
            await _handler.CreateAsync(request, userId);
            return Created();
        }

        [HttpGet]
        [ProducesResponseType(typeof(Response<List<Bag>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var bags = await _handler.GetAllAsync(userId);
            var response = new Response<List<Bag>>(bags);
            return Ok(response);
        }
    }
}
