using MongoDB.Bson.Serialization.Attributes;
using NetBazaar.Domain.Entities.VisitorContext;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetBazaar.Application.DTOs.Visitor
{
    public class RequestSaveVisitorInfoDTO
    {
        public string Id { get; set; } // برای MongoDB _id
        public string VisitorId { get; set; } 
        public string IpAddress { get; set; }
        public string CurrentUrl { get; set; }
        public string ReferrerUrl { get; set; }
        public string HttpMethod { get; set; }
        public string Protocol { get; set; } // http, https
        public string PhysicalPath { get; set; } // مسیر فیزیکی بازدید
        public string ReferringController { get; set; }
        public string ReferringAction { get; set; }

        public VisitorVersionDto Browser { get; set; }
        public VisitorVersionDto OperatingSystem { get; set; }
        public VisitorDeviceDto Device { get; set; }


        public bool IsBot { get; set; } // آیا بازدیدکننده ربات است یا انسان
        [BsonDateTimeOptions(Kind =DateTimeKind.Local)]
        public DateTime VisitedAt { get; set; } = DateTime.UtcNow;
    }
}
