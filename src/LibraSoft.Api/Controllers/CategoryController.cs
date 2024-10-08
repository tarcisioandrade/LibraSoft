﻿using LibraSoft.Api.Constants;
using LibraSoft.Core.Commons;
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
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryHandler _handler;
        private readonly ICacheService _cache;

        public CategoryController(ICategoryHandler handler, ICacheService cache)
        {
            _handler = handler;
            _cache = cache;
        }

        [HttpPost]
        [Authorize("admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create(CreateCategoryRequest request, ICategoryHandler handler)
            {
            var exists = await handler.GetByTitle(request.Title);

            if (exists is not null)
            {
                return BadRequest(new CategoryAlreadyExistsError());
            }

            await handler.CreateAsync(request);

            await _cache.InvalidateCacheAsync(CacheTagConstants.Category);

            return Created();
        }

        [HttpGet]
        [ProducesResponseType(typeof(Response<List<CategoryResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _cache.GetOrCreateAsync("get-all-category", async () =>
            {
                var categoriesFromDb = await _handler.GetAll();
                return categoriesFromDb;
            }, tag: CacheTagConstants.Category);

            
            return Ok(categories);
        }
    }
}
