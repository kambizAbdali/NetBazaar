using System;
using System.Collections.Generic;
using System.Text;

namespace NetBazaar.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; } 
        public string FullName { get; set; }
    }
}
