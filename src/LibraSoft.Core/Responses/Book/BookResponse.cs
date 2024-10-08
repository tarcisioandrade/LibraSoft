﻿using LibraSoft.Core.Enums;
using LibraSoft.Core.Responses.Author;
using LibraSoft.Core.Responses.Category;
using LibraSoft.Core.ValueObjects;

namespace LibraSoft.Core.Responses.Book
{
    public class BookResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Publisher { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public string Isbn { get; set; } = string.Empty;
        public DateTime PublicationAt { get; set; }
        public int CopiesAvaliable { get; set; }
        public double AverageRating { get; set; }
        public int PageCount { get; set; }
        public string Sinopse { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public Dimensions Dimensions { get; set; } = new();
        public ECoverType CoverType { get; set; }
        public int ReviewsCount { get; set; }
        public IEnumerable<CategoryResponse> Categories { get; set; } = [];
        public required AuthorResponse Author { get; set; }
        public EStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        
    }
}
