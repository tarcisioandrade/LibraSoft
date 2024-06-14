using LibraSoft.Core.Exceptions;
using LibraSoft.Core.Interfaces;
using LibraSoft.Core.Requests.Category;
using LibraSoft.Core.Responses.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraSoft.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("admin")]
    public class CategoryController : ControllerBase
    {
        private ICategoryHandler _handler;
        public CategoryController(ICategoryHandler handler)
        {
            _handler = handler;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create(CreateCategoryRequest request, ICategoryHandler handler)
            {
            var exists = await handler.GetByTitle(request.Title);

            if (exists is not null)
            {
                return BadRequest(new CategoryAlreadyExistsError());
            }

            await handler.CreateAsync(request);

            return Created();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _handler.GetAll();

            if (categories?.Count > 0)
            {
                var response = categories.Select(category => new CategoryResponse
                {
                    Id = category.Id,
                    Title = category.Title
                }).ToList();

                return Ok(response);
            }

            return Ok(categories);
        }
    }
}
