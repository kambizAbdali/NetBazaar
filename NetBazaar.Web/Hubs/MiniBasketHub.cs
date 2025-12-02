// برای به‌روزرسانی زنده سبد خرید می‌توانید از SignalR استفاده کنید
// فایل Hub/MiniBasketHub.cs

using Microsoft.AspNetCore.SignalR;

namespace NetBazaar.Web.Hubs
{
    public class MiniBasketHub : Hub
    {
        public async Task JoinBasketGroup(string buyerId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"basket-{buyerId}");
        }

        public async Task LeaveBasketGroup(string buyerId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"basket-{buyerId}");
        }
    }
}