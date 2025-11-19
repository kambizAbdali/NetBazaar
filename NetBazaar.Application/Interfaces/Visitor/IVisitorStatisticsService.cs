using NetBazaar.Application.DTOs.Visitor;
using System.Threading.Tasks;

namespace NetBazaar.Application.Interfaces.Visitor
{
    public interface IVisitorStatisticsService
    {
        GeneralStatusDto GeneralStatus { get; set; }

        /// <summary>
        /// Executes the report generation and returns today's analytics data
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains the today's report data.</returns>
        Task<VisitorAnalyticsReportDto> ExecuteAsync(int page = 1, int pageSize = 20);
    }
}