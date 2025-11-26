using MongoDB.Driver;
using NetBazaar.Application.Interfaces.Visitor;
using NetBazaar.Domain.Entities.Visitor;
using NetBazaar.Persistence.Interfaces.DatabaseContext;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NetBazaar.Infrastructure.Services.Visitor
{
    public class OnlineVisitorsService : IOnlineVisitorsService
    {
        private readonly IMongoCollection<OnlineVisitor> _collection;

        public OnlineVisitorsService(IMongoDbContext<OnlineVisitor> dbContext)
        {
            _collection = dbContext.GetCollection();
        }

        public async Task UpsertOnConnectAsync(string visitorId, string connectionId, string ip, string userAgent)
        {
            var filter = Builders<OnlineVisitor>.Filter.Eq(v => v.ConnectionId, connectionId);
            var update = Builders<OnlineVisitor>.Update
                .SetOnInsert(v => v.Id, Guid.NewGuid().ToString())
                .Set(v => v.VisitorId, visitorId)
                .Set(v => v.ConnectionId, connectionId)
                .Set(v => v.IpAddress, ip)
                .Set(v => v.UserAgent, userAgent)
                .Set(v => v.ConnectedAt, DateTime.UtcNow)
                .Set(v => v.IsOnline, true)
                .Unset(v => v.DisconnectedAt);
            await _collection.UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true });
        }

        public async Task SetOfflineOnDisconnectAsync(string connectionId)// per any connectionId (enen many opened tab in one btowser)
        {
            /* // If we want to update the collection without deleting any rows.
            var filter = Builders<OnlineVisitor>.Filter.Eq(v => v.ConnectionId, connectionId);
            var update = Builders<OnlineVisitor>.Update
                .Set(v => v.IsOnline, false)
                .Set(v => v.DisconnectedAt, DateTime.UtcNow);

            await _collection.UpdateOneAsync(filter, update);*/

            //If we want to delete the collection
            await _collection.FindOneAndDeleteAsync(o => o.ConnectionId == connectionId);
        }

        public async Task MarkVisitorOnlineAsync(string visitorId, string ip, string userAgent) //"Many tabs may be open in one browser, but consider a single visitorId."
        {
            var filter = Builders<OnlineVisitor>.Filter.Eq(v => v.VisitorId, visitorId);
            var update = Builders<OnlineVisitor>.Update
                .Set(v => v.VisitorId, visitorId)
                .Set(v => v.IpAddress, ip)
                .Set(v => v.UserAgent, userAgent)
                .Set(v => v.ConnectedAt, DateTime.UtcNow)
                .Set(v => v.IsOnline, true)
                .Unset(v => v.DisconnectedAt);

            await _collection.UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true });
        }

        public async Task MarkVisitorOfflineAsync(string visitorId)
        {
            var filter = Builders<OnlineVisitor>.Filter.Eq(v => v.VisitorId, visitorId);
            var update = Builders<OnlineVisitor>.Update
                .Set(v => v.IsOnline, false)
                .Set(v => v.DisconnectedAt, DateTime.UtcNow);

            await _collection.UpdateOneAsync(filter, update);
        }

        public async Task<int> GetOnlineCountAsync()
        {
            var filter = Builders<OnlineVisitor>.Filter.Eq(v => v.IsOnline, true);
            return (int)await _collection.CountDocumentsAsync(filter);
        }

    }
}
