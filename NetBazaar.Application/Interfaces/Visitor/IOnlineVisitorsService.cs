using System;
using System.Collections.Generic;
using System.Text;

namespace NetBazaar.Application.Interfaces.Visitor
{
    public interface IOnlineVisitorsService
    {
        Task<int> GetOnlineCountAsync();
        Task UpsertOnConnectAsync(string visitorId, string connectionId, string ip, string userAgent);
        Task SetOfflineOnDisconnectAsync(string connectionId);
        Task MarkVisitorOnlineAsync(string visitorId, string ip, string userAgent);
        Task MarkVisitorOfflineAsync(string visitorId);
    }
}
