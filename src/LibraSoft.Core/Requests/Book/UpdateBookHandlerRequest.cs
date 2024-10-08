﻿using LibraSoft.Core.Enums;
using LibraSoft.Core.ValueObjects;

namespace LibraSoft.Core.Requests.Book
{
    public class UpdateBookHandlerRequest
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public string Publisher { get; set; } = string.Empty;
        public string Isbn { get; set; } = string.Empty;
        public int PageCount { get; set; }
        public string Sinopse { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public Dimensions Dimensions { get; set; } = new();
        public ECoverType CoverType { get; set; }
        public DateTime PublicationAt { get; set; }
        public required Models.Author Author { get; set; }
        public int CopiesAvailable { get; set; }
        public List<Models.Category> Categories { get; set; } = [];
    }
}
