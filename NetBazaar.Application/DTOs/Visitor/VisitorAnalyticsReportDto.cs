using System;
using System.Collections.Generic;
using System.Text;

namespace NetBazaar.Application.DTOs.Visitor
{
    public class VisitorAnalyticsReportDto
    {
        public required GeneralStatusDto Result { get; set; }
        public required TodayDto Today { get; set; }

        // اضافه شده
        public ChartDataDto DailyChart { get; set; } = new();   // 24 ساعت گذشته
        public ChartDataDto MonthlyChart { get; set; } = new(); // 30 روز گذشته
        public ChartDataDto YearlyChart { get; set; } = new();  // 12 ماه گذشته

        public List<VisitorDto> LastVisitors { get; set; } = new();
        public int TotalVisitorsCount { get; set; }
    }
}
