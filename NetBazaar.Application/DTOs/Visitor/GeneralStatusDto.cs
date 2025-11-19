using System;
using System.Collections.Generic;
using System.Text;

namespace NetBazaar.Application.DTOs.Visitor
{
    public class GeneralStatusDto
    {
        public long TotalPageViews { get; set; }
        public long TotalVisitors { get; set; }
        public float PageViewsPerVisit { get; set; }
    }
}
