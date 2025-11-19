using System;
using System.Collections.Generic;
using System.Text;

namespace NetBazaar.Domain.Entities.VisitorContext
{
    public class VisitorDevice      // اطلاعات Device
    {
        public string Id { get; set; }
        public string Brand { get; set; }   // برند دستگاه
        public string Model { get; set; }   // مدل دستگاه
        public string DeviceType { get; set; } // Mobile, Desktop, Tablet, Bot
    }
}