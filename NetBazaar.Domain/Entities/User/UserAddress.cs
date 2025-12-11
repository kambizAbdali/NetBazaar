namespace NetBazaar.Domain.Entities.Users
{
    public class UserAddress
    {
        public int Id { get; private set; }
        public string UserId { get; private set; } = null!;
        public string ReceiverName { get; private set; } = null!;
        public string PhoneNumber { get; private set; } = null!;
        public string PostalCode { get; private set; } = null!;
        public string AddressText { get; private set; } = null!;
        public string City { get; set; } = null!;
        public bool IsDefault { get; private set; }

        // سازنده EF
        private UserAddress() { }

        public UserAddress(string userId, string receiverName, string phone, string postalCode, string addressText, bool isDefault = false)
        {
            UserId = userId;
            ReceiverName = receiverName;
            PhoneNumber = phone;
            PostalCode = postalCode;
            AddressText = addressText;
            IsDefault = isDefault;
        }
        public void UpdateInfo(string receiverName, string phoneNumber, string postalCode, string addressText, bool isDefault)
        {
            ReceiverName = receiverName;
            PhoneNumber = phoneNumber;
            PostalCode = postalCode;
            AddressText = addressText;
            IsDefault = isDefault;
        }
        public void SetAsDefault() => IsDefault = true;
        public void SetUserId(string userId) => UserId = userId;
        public void UnsetDefault() => IsDefault = false;
    }
}
