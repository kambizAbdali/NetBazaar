using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetBazaar.Domain.Entities.VisitorContext
{
    // پایه بازدید
    public class Visitor
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)] // For GUID
        public string Id { get; set; } = Guid.NewGuid().ToString(); // MongoDB _id

        // Unique identifier for this visitor, stored in a cookie.
        // - If the cookie exists, use its value.
        // - If the cookie does not exist, generate a new value, set the cookie, and use it.
        // This helps persist visitor data across requests/sessions.
        public string VisitorId { get; set; }

        // The IP address of the visitor
        public string IpAddress { get; set; }

        // The current URL being visited
        public string CurrentUrl { get; set; }

        // The URL of the referring page (if any)
        public string ReferrerUrl { get; set; }

        // The HTTP method of the request (GET, POST, etc.)
        public string HttpMethod { get; set; }

        // The request scheme: http or https
        public string Scheme { get; set; } // http, https

        // The physical path of the requested resource on the server
        public string PhysicalPath { get; set; } // Path of the visited resource

        public string ReferringController { get; set; }
        public string ReferringAction { get; set; }

        public VisitorVersion Browser { get; set; }
        public VisitorVersion OperatingSystem { get; set; }
        public VisitorDevice Device { get; set; }

        public bool IsBot { get; set; } // Whether the visitor is a bot or a human
        public DateTime VisitedAt { get; set; } = DateTime.UtcNow;
    }
}