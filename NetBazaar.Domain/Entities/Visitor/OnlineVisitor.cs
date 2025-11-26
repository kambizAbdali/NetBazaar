using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetBazaar.Domain.Entities.Visitor
{
    public class OnlineVisitor
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string VisitorId { get; set; } // شناسه یکتا (کوکی)
        public string ConnectionId { get; set; } // شناسه اتصال SignalR
        public DateTime ConnectedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DisconnectedAt { get; set; }
        public bool IsOnline { get; set; } = true;
        public string IpAddress { get; set; }
        public string UserAgent { get; set; }
    }
}
