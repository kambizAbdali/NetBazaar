using System;
using System.Collections.Generic;
using System.Text;

namespace NetBazaar.Application.DTOs.User
{
    public class UserAddressDto
    {
        public int Id { get; set; }
        public string ReceiverName { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string PostalCode { get; set; } = "";
        public string AddressText { get; set; } = "";
        public bool IsDefault { get; set; }
    }
}
