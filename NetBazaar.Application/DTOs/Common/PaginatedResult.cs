using System;
using System.Collections.Generic;
using System.Text;

namespace NetBazaar.Application.DTOs.Common
{
    public class PaginatedResult<T>
    {
        public List<T> Data { get; set; } = new();
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;
    }
}