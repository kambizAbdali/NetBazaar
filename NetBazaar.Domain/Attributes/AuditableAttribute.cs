using System;
using System.Collections.Generic;
using System.Text;

namespace NetBazaar.Domain.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AuditableAttribute: Attribute
    {
    }
}
