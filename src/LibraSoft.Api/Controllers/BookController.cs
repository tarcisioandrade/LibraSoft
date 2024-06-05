using LibraSoft.Core.Interfaces;
using LibraSoft.Core.Requests.Book;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraSoft.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("admin")]
    public class BookController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create(CreateBookRequest request, IBookHandler handler)
        {
            await handler.CreateAsync(request);

            return Created();
        }
    }
}
