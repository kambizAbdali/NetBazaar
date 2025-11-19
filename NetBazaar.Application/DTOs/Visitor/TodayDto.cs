using System;
using System.Collections.Generic;
using System.Text;

namespace NetBazaar.Application.DTOs.Visitor
{
    public class TodayDto
    {
        public long PageViews { get; set; }
        public long Visitors { get; set; }
        public float ViewsPerVisitors { get; set; }
    }
}
