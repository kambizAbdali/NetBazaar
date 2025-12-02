using NetBazaar.Domain.Attributes;
using NetBazaar.Domain.Common;
using System.Collections.ObjectModel;

namespace NetBazaar.Domain.Entities.Basket
{
    [Auditable]
    public class Basket 
    {
        private readonly List<BasketItem> _items = new();

        // سازنده خصوصی برای EF Core
        private Basket() { }

        // سازنده اصلی
        public Basket(string buyerId)
        {
            BuyerId = buyerId ?? throw new ArgumentNullException(nameof(buyerId));
        }

        public int Id { get; private set; }

        // شناسه خریدار (می‌تواند UserId برای کاربران لاگین‌شده یا GUID برای کاربران ناشناس باشد)
        public string BuyerId { get; private set; }

        // آیتم‌های سبد خرید (فقط قابل خواندن از بیرون)
        public IReadOnlyCollection<BasketItem> Items => new ReadOnlyCollection<BasketItem>(_items);

        // متد برای افزودن آیتم به سبد خرید
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

        // متد برای حذف آیتم از سبد خرید
        public void RemoveItem(int basketItemId)
        {
            var itemToRemove = _items.FirstOrDefault(item => item.Id == basketItemId);
            if (itemToRemove != null)
            {
                _items.Remove(itemToRemove);
            }
        }

        // متد برای خالی کردن سبد خرید
        public void Clear()
        {
            _items.Clear();
        }

        // محاسبه جمع کل سبد خرید
        public decimal GetTotalPrice()
        {
            return _items.Sum(item => item.GetTotalPrice());
        }

        // محاسبه تعداد کل آیتم‌ها
        public int GetTotalItemsCount()
        {
            return _items.Sum(item => item.Quantity);
        }
    }
}