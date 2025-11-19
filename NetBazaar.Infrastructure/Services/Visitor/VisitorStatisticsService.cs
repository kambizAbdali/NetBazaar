using MongoDB.Driver;
using NetBazaar.Application.DTOs.Visitor;
using NetBazaar.Application.Interfaces.Visitor;
using NetBazaar.Domain.Entities.VisitorContext;
using NetBazaar.Persistence.Interfaces.DatabaseContext;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using VisitorModel = NetBazaar.Domain.Entities.VisitorContext.Visitor;

namespace NetBazaar.Infrastructure.Services.Visitor
{
    public class VisitorStatisticsService : IVisitorStatisticsService
    {
        private readonly IMongoCollection<VisitorModel> _collection;
        private readonly PersianCalendar _persianCalendar = new();

        public VisitorStatisticsService(IMongoDbContext<VisitorModel> dbContext)
        {
            if (dbContext == null)
                throw new ArgumentNullException(nameof(dbContext));

            _collection = dbContext.GetCollection();
        }

        public GeneralStatusDto GeneralStatus
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }
        public async Task<List<VisitorDto>> GetVisitorsAsync(int page = 1, int pageSize = 20)
        {
            var skip = (page - 1) * pageSize;

            var visitors = await _collection
                .Find(Builders<VisitorModel>.Filter.Empty)
                .SortByDescending(v => v.VisitedAt)
                .Skip(skip)
                .Limit(pageSize)
                .ToListAsync();

            return visitors.Select(v => new VisitorDto
            {
                VisitorId = v.VisitorId,
                VisitedAt = v.VisitedAt,
                OperatingSystem = v.OperatingSystem?.Family,
                Browser = v.Browser?.Family,
                ReferrerUrl = v.ReferrerUrl,
                IpAddress = v.IpAddress,
                IsBot = v.IsBot
            }).ToList();
        }
        public async Task<VisitorAnalyticsReportDto> ExecuteAsync(int page = 1, int pageSize = 20)
        {
            var today = DateTime.UtcNow.Date;
            var todayFilter = BuildTodayFilter(today);

            var (totalVisits, uniqueVisitors) = await GetTodayStatisticsAsync(todayFilter);
            var (totalPageViewsAllTime, totalVisitorsAllTime) = await GetAllTimeStatisticsAsync();

            var dailyChart = await GenerateDailyChartAsync(today);
            var monthlyChart = await GenerateMonthlyChartAsync();
            var yearlyChart = await GenerateYearlyChartAsync();

            // گرفتن آخرین بازدیدها با Paging
            var skip = (page - 1) * pageSize;
            var lastVisitors = await _collection
                .Find(Builders<VisitorModel>.Filter.Empty)
                .SortByDescending(v => v.VisitedAt)
                .Skip(skip)
                .Limit(pageSize)
                .ToListAsync();

            var pc = new PersianCalendar();
            int rowIndex = skip + 1;

            var visitorDtos = lastVisitors.Select(v => new VisitorDto
            {
                VisitorId = v.VisitorId,
                VisitedAt = v.VisitedAt,
                VisitedAtPersian = $"{pc.GetYear(v.VisitedAt)}/{pc.GetMonth(v.VisitedAt):00}/{pc.GetDayOfMonth(v.VisitedAt):00} {v.VisitedAt.Hour:00}:{v.VisitedAt.Minute:00}",
                OperatingSystem = v.OperatingSystem?.Family,
                Browser = v.Browser?.Family,
                ReferrerUrl = v.ReferrerUrl,
                IpAddress = v.IpAddress,
                IsBot = v.IsBot,
                RowNumber = rowIndex++
            }).ToList();
            var totalCount = await _collection.CountDocumentsAsync(Builders<VisitorModel>.Filter.Empty);
            return new VisitorAnalyticsReportDto
            {
                Result = CreateGeneralStatusDto(totalPageViewsAllTime, totalVisitorsAllTime),
                Today = CreateTodayDto(totalVisits, uniqueVisitors),
                DailyChart = dailyChart,
                MonthlyChart = monthlyChart,
                YearlyChart = yearlyChart,
                LastVisitors = visitorDtos,
                TotalVisitorsCount = (int)totalCount
            };
        }

        private static FilterDefinition<VisitorModel> BuildTodayFilter(DateTime today)
        {
            var startOfToday = today;
            var endOfToday = today.AddDays(1).AddTicks(-1);

            return Builders<VisitorModel>.Filter.And(
                Builders<VisitorModel>.Filter.Gte(v => v.VisitedAt, startOfToday),
                Builders<VisitorModel>.Filter.Lte(v => v.VisitedAt, endOfToday)
            );
        }

        private async Task<(long totalVisits, int uniqueVisitors)> GetTodayStatisticsAsync(FilterDefinition<VisitorModel> todayFilter)
        {
            var totalVisitsTask = _collection.CountDocumentsAsync(todayFilter);
            var uniqueVisitorsTask = _collection.DistinctAsync(v => v.VisitorId, todayFilter);

            await Task.WhenAll(totalVisitsTask, uniqueVisitorsTask);

            var uniqueVisitors = (await uniqueVisitorsTask).ToList();
            return (await totalVisitsTask, uniqueVisitors.Count);
        }

        private async Task<(long totalPageViews, int totalVisitors)> GetAllTimeStatisticsAsync()
        {
            var allTimeFilter = Builders<VisitorModel>.Filter.Empty;

            var totalPageViewsTask = _collection.CountDocumentsAsync(allTimeFilter);
            var totalVisitorsTask = _collection.DistinctAsync(v => v.VisitorId, allTimeFilter);

            await Task.WhenAll(totalPageViewsTask, totalVisitorsTask);

            var totalVisitors = (await totalVisitorsTask).ToList();
            return (await totalPageViewsTask, totalVisitors.Count);
        }

        private async Task<ChartDataDto> GenerateDailyChartAsync(DateTime today)
        {
            var labels = new List<string>();
            var data = new List<long>();

            for (int hour = 0; hour < 24; hour++)
            {
                var startHour = today.AddHours(hour);
                var endHour = startHour.AddHours(1);

                var hourFilter = Builders<VisitorModel>.Filter.And(
                    Builders<VisitorModel>.Filter.Gte(v => v.VisitedAt, startHour),
                    Builders<VisitorModel>.Filter.Lt(v => v.VisitedAt, endHour)
                );

                var count = await _collection.CountDocumentsAsync(hourFilter);
                labels.Add($"{hour:00}:00");
                data.Add(count);
            }

            return new ChartDataDto { Labels = labels, Data = data };
        }

        private async Task<ChartDataDto> GenerateMonthlyChartAsync()
        {
            var labels = new List<string>();
            var data = new List<long>();
            var now = DateTime.UtcNow;

            for (int dayOffset = 29; dayOffset >= 0; dayOffset--)
            {
                var day = now.Date.AddDays(-dayOffset);
                var (startDay, endDay) = GetDayRange(day);

                var dayFilter = Builders<VisitorModel>.Filter.And(
                    Builders<VisitorModel>.Filter.Gte(v => v.VisitedAt, startDay),
                    Builders<VisitorModel>.Filter.Lt(v => v.VisitedAt, endDay)
                );

                var count = await _collection.CountDocumentsAsync(dayFilter);
                var label = $"{_persianCalendar.GetMonth(day)}/{_persianCalendar.GetDayOfMonth(day)}";

                labels.Add(label);
                data.Add(count);
            }

            return new ChartDataDto { Labels = labels, Data = data };
        }

        private async Task<ChartDataDto> GenerateYearlyChartAsync()
        {
            var labels = new List<string>();
            var data = new List<long>();
            var now = DateTime.UtcNow;

            for (int monthOffset = 11; monthOffset >= 0; monthOffset--)
            {
                var monthStart = new DateTime(now.Year, now.Month, 1).AddMonths(-monthOffset);
                var monthEnd = monthStart.AddMonths(1);

                var monthFilter = Builders<VisitorModel>.Filter.And(
                    Builders<VisitorModel>.Filter.Gte(v => v.VisitedAt, monthStart),
                    Builders<VisitorModel>.Filter.Lt(v => v.VisitedAt, monthEnd)
                );

                var count = await _collection.CountDocumentsAsync(monthFilter);
                var label = GetPersianMonthName(_persianCalendar.GetMonth(monthStart));

                labels.Add(label);
                data.Add(count);
            }

            return new ChartDataDto { Labels = labels, Data = data };
        }

        private static (DateTime start, DateTime end) GetDayRange(DateTime day)
        {
            var start = day.Date;
            var end = start.AddDays(1);
            return (start, end);
        }

        private static string GetPersianMonthName(int month)
        {
            if (month < 1 || month > 12)
                throw new ArgumentOutOfRangeException(nameof(month), "Month must be between 1 and 12");

            string[] months =
            {
                "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور",
                "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند"
            };

            return months[month - 1];
        }

        private static GeneralStatusDto CreateGeneralStatusDto(long totalPageViews, int totalVisitors)
        {
            return new GeneralStatusDto
            {
                TotalPageViews = totalPageViews,
                TotalVisitors = totalVisitors,
                PageViewsPerVisit = totalVisitors > 0 ? (float)totalPageViews / totalVisitors : 0
            };
        }

        private static TodayDto CreateTodayDto(long pageViews, int visitors)
        {
            return new TodayDto
            {
                PageViews = pageViews,
                Visitors = visitors,
                ViewsPerVisitors = visitors > 0 ? (float)pageViews / visitors : 0
            };
        }
    }
}