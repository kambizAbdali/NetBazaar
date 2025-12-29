// کدهای موجود...
using NetBazaar.Domain.Attributes;
using NetBazaar.Domain.Discounts;
using System.Collections.ObjectModel;

namespace NetBazaar.Domain.Entities.Basket
{
    [Auditable]
    public class Basket
    {
        private readonly List<BasketItem> _items = new();

        // سازنده خصوصی برای EF Core
        private Basket() { }

        public int Id { get; private set; }
        public string BuyerId { get; private set; }
        public IReadOnlyCollection<BasketItem> Items => new ReadOnlyCollection<BasketItem>(_items);

        // New code: فیلدهای جدید برای تخفیف
        public int? DiscountId { get; private set; }
        public decimal DiscountAmount { get; private set; }
        public Discount? Discount { get; private set; }

        // New code: متد اعمال تخفیف
        public void ApplyDiscount(Discount discount)
        {
            if (discount == null)
                throw new ArgumentNullException(nameof(discount));

            // محاسبه مبلغ تخفیف بر اساس کل سبد خرید
            var totalPrice = GetTotalPriceWithoutDiscount();
            DiscountAmount = discount.CalculateDiscountAmount(totalPrice);
            DiscountId = discount.Id;
            Discount = discount;
        }

        // New code: متد حذف تخفیف
        public void RemoveDiscount()
        {
            DiscountAmount = 0;
            DiscountId = null;
            Discount = null;
        }

        // New code: اضافه کردن پراپرتی برای دسترسی عمومی به قیمت بدون تخفیف
        public decimal TotalPriceWithoutDiscount => GetTotalPriceWithoutDiscount();

        // New code: اضافه کردن پراپرتی برای دسترسی عمومی به قیمت با تخفیف
        public decimal TotalPriceWithDiscount => GetTotalPrice();

        // متدهای قبلی که قبلاً اضافه شده‌اند...
        public decimal GetTotalPriceWithoutDiscount()
        {
            return _items.Sum(item => item.GetTotalPrice());
        }

        public decimal GetTotalPrice()
        {
            var total = GetTotalPriceWithoutDiscount();
            return total - DiscountAmount;
        }

        public void AddItem(long catalogItemId, int quantity, decimal unitPrice)
        {
            var existingItem = _items.FirstOrDefault(item => item.CatalogItemId == catalogItemId);

            if (existingItem != null)
            {
                if (existingItem.IsRemoved)
                {
                    existingItem.MarkAsUnremoved();
                    existingItem.SetQuantity(quantity);
                }
                else
                {
                    // اگر آیتم از قبل وجود دارد، تعداد را افزایش می‌دهیم
                    existingItem.AddQuantity(quantity);
                }
            }
            else
            {
                // اگر آیتم جدید است، آن را ایجاد می‌کنیم
                var newItem = new BasketItem(catalogItemId, quantity, unitPrice);
                _items.Add(newItem);
            }
        }

        // New code: به‌روزرسانی تخفیف پس از تغییرات در آیتم‌ها
        private void UpdateDiscountAfterItemChange()
        {
            if (Discount != null)
            {
                var totalPrice = GetTotalPriceWithoutDiscount();
                DiscountAmount = Discount.CalculateDiscountAmount(totalPrice);
            }
        }

        public void RemoveItem(int basketItemId)
        {
            var itemToRemove = _items.FirstOrDefault(item => item.Id == basketItemId);
            if (itemToRemove != null)
            {
                _items.Remove(itemToRemove);
                UpdateDiscountAfterItemChange(); // New code
            }
        }

        public void Clear()
        {
            _items.Clear();
            RemoveDiscount(); // New code: تخفیف نیز حذف شود
        }

        public bool IsRemoved { get; private set; } = false;

        public Basket(string buyerId)
        {
            BuyerId = buyerId ?? throw new ArgumentNullException(nameof(buyerId));
            IsRemoved = false;
        }

        public void MarkAsRemoved() => IsRemoved = true;
        public void MarkAsUnremoved() => IsRemoved = false;
        public int GetTotalItemsCount()
        {
            return _items.Count; 
        }
    }
}