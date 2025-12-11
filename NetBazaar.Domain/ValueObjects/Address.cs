namespace NetBazaar.Domain.ValueObjects
{
    public class Address
    {
        private Address() { } // For EF Core

        public Address(string postalCode, string city, string receiverName, string addressText, string phoneNumber)
        {
            ReceiverName = receiverName;
            City = city;
            PostalCode = postalCode;
            AddressText = addressText;
            PhoneNumber = phoneNumber;
        }

        public string ReceiverName { get; private set; }
        public string City { get; private set; }
        public string PostalCode { get; private set; }
        public string PhoneNumber { get; private set; }
        public string AddressText { get; private set; }
    }
}