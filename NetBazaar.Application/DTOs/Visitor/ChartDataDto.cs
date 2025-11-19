using System;
using System.Collections.Generic;
using System.Text;

namespace NetBazaar.Application.DTOs.Visitor
{
    public class ChartDataDto
    {
        public List<string> Labels { get; set; } = new();
        public List<long> Data { get; set; } = new();
    }
}
