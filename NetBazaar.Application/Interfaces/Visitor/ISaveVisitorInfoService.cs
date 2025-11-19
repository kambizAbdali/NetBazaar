using NetBazaar.Application.DTOs.Visitor;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetBazaar.Application.Interfaces.Visitor
{
    public interface ISaveVisitorInfoService
    {
        void Execute(RequestSaveVisitorInfoDTO request);
    }
}
