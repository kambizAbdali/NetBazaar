using System;
using System.Collections.Generic;
using System.Text;

namespace NetBazaar.Domain.Entities.VisitorContext
{
    public class VisitorDeviceDto      // اطلاعات Device
    {
        public string Id { get; set; }
        public string Brand { get; set; }   // برند دستگاه
        public string Model { get; set; }   // مدل دستگاه
        public string Family { get; set; } // Mobile, Desktop, Tablet, Bot
        public bool IsBot { get; set; } // آیا بازدیدکننده ربات است یا انسان

    }
}