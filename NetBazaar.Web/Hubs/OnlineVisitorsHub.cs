using Microsoft.AspNetCore.SignalR;
using NetBazaar.Application.Interfaces.Visitor;

namespace NetBazaar.Web.EndPoint.Hubs
{
    public class OnlineVisitorsHub : Hub
    {
        private readonly IOnlineVisitorsService _onlineService;

        public OnlineVisitorsHub(IOnlineVisitorsService onlineService)
        {
            _onlineService = onlineService;
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var visitorId = httpContext?.Request.Cookies["VisitorId"];
            var ip = httpContext?.Connection.RemoteIpAddress?.ToString() ?? "";
            var userAgent = httpContext?.Request.Headers["User-Agent"].ToString() ?? "";

            if (string.IsNullOrWhiteSpace(visitorId))
            {
                visitorId = Guid.NewGuid().ToString();
                httpContext?.Response.Cookies.Append("VisitorId", visitorId, new CookieOptions
                {
                    HttpOnly = true,
                    IsEssential = true,
                    SameSite = SameSiteMode.Lax,
                    Expires = DateTimeOffset.UtcNow.AddYears(1)
                });
            }

            // فقط اگر قبلاً آنلاین نبوده، ثبت کن
            await _onlineService.MarkVisitorOnlineAsync(visitorId!, ip, userAgent);

            var count = await _onlineService.GetOnlineCountAsync();
            await Clients.All.SendAsync("OnlineCountUpdated", count);

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var visitorId = Context.GetHttpContext()?.Request.Cookies["VisitorId"];
            if (!string.IsNullOrWhiteSpace(visitorId))
                await _onlineService.MarkVisitorOfflineAsync(visitorId!);

            var count = await _onlineService.GetOnlineCountAsync();
            await Clients.All.SendAsync("OnlineCountUpdated", count);

            await base.OnDisconnectedAsync(exception);
        }

        public async Task<int> GetCurrentOnlineCount() // call from NetBazaar.Admin.EndPoint project
        {
            return await _onlineService.GetOnlineCountAsync();
        }

    }
}
