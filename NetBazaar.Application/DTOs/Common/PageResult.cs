using System;
using System.Collections.Generic;
using System.Text;

namespace NetBazaar.Application.DTOs.Common
{
    public class PageResult<T>
    {
        public IReadOnlyList<T> Data { get; }
        public int TotalCount { get; }
        public int Page { get; }
        public int PageSize { get; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

        public PageResult(IReadOnlyList<T> data, int totalCount, int page, int pageSize)
        {
            Data = data;
            TotalCount = totalCount;
            Page = page;
            PageSize = pageSize;
        }
    }
}
