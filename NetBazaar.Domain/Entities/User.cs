using Microsoft.AspNetCore.Identity;
using NetBazaar.Domain.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetBazaar.Domain.Entities
{
    [Auditable]
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public string LastName { get; set; }
    }
}