﻿namespace LibraSoft.Core.Requests.Author
{
    public class GetAllAuthorRequest
    {
        public bool IncludeInactive { get; set; }
        public string? Search {  get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
