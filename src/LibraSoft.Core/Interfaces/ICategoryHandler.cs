﻿using LibraSoft.Core.Models;
using LibraSoft.Core.Requests.Category;
using LibraSoft.Core.Responses.Category;

namespace LibraSoft.Core.Interfaces
{
    public interface ICategoryHandler
    {
        public Task CreateAsync(CreateCategoryRequest request);
        public Task<Category?> GetByTitle(string title);
        public Task<List<CategoryResponse>?> GetAll();
        public Task<Category?> GetById(Guid id, bool asNoTracking = false);
    }
}
