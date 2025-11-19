using System;
using System.Collections.Generic;
using System.Text;

namespace NetBazaar.Domain.Entities.VisitorContext
{
    public class VisitorVersion    // نسخه مرورگر یا خانواده عمومی مرورگر
    {
        public string Id { get; set; }
        public string Family { get; set; }   // مثلاً "Chrome", "Firefox", "Windows 10"
        public string Version { get; set; }  // ورژن دقیق
    }
}
