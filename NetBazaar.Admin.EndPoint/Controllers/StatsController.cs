using Microsoft.AspNetCore.Mvc;
using NetBazaar.Application.DTOs.Visitor;
using NetBazaar.Application.Interfaces.Visitor;
using System.Runtime.InteropServices;

namespace NetBazaar.Admin.EndPoint.Controllers
{
    public class StatsController : Controller
    {
        private readonly IVisitorStatisticsService _getTodayReportService;
        public VisitorAnalyticsReportDto resultTodayReportDto { get; set; }
        public StatsController(IVisitorStatisticsService getTodayReportService) => _getTodayReportService = getTodayReportService;

        public async Task<IActionResult> Index(int page = 1, int pageSize = 20)
        {
            var report = await _getTodayReportService.ExecuteAsync(page, pageSize);
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalCount = report.TotalVisitorsCount;
            return View(report);
        }
    }
}