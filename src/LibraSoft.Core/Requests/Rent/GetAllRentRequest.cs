﻿using LibraSoft.Core.Enums;

namespace LibraSoft.Core.Requests.Rent
{
    public class GetAllRentRequest
    {
        public ERentStatus? Status { get; set; }
        public string? SearchEmail { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
