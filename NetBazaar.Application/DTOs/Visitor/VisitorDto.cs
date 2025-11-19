using System;
using System.Collections.Generic;
using System.Text;

namespace NetBazaar.Application.DTOs.Visitor
{
    public class VisitorDto
    {
        public string VisitorId { get; set; }
        public DateTime VisitedAt { get; set; }
        public string VisitedAtPersian { get; set; } // زمان شمسی
        public string OperatingSystem { get; set; }
        public string Browser { get; set; }
        public string ReferrerUrl { get; set; }
        public string IpAddress { get; set; }
        public bool IsBot { get; set; }
        public int RowNumber { get; set; } // شماره ردیف
    }



}
