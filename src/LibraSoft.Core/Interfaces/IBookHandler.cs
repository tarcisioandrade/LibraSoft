﻿using LibraSoft.Core.Commons;
using LibraSoft.Core.Models;
using LibraSoft.Core.Requests.Book;
using LibraSoft.Core.Responses.Book;

namespace LibraSoft.Core.Interfaces
{
    public interface IBookHandler
    {
        public Task CreateAsync(CreateBookRequest request, List<Category> categories,  Author author);
        public Task<Book?> GetByIdAsync(Guid id, bool asNoTracking = false);
        public Task<Book?> GetByIsbnAsync(string isbn, bool asNoTracking = false);
        public Task<PagedResponse<IEnumerable<BookResponse>?>> GetAllAsync(GetAllBookRequest request);
        public Task DeleteAsync(Book book);
        public Task InactiveAsync(Book book);
        public Task ReactivatedAsync(Book book);
        public Task UpdateRatingAsync(Book book);
        public Task<List<Book>?> GetWithCategoriesAsync(Book book);
        public Task<bool> UpdateAsync(UpdateBookHandlerRequest request, Book book);
    }   
}
