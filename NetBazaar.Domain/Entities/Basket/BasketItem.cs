using NetBazaar.Domain.Attributes;
using NetBazaar.Domain.Common;
using NetBazaar.Domain.Entities.Catalog;

namespace NetBazaar.Domain.Entities.Basket
{
    [Auditable]
    public class BasketItem 
    {
        // سازنده خصوصی برای EF Core
        private BasketItem() { }

        // سازنده اصلی
        public BasketItem(long catalogItemId, int quantity, decimal unitPrice)
        {
            CatalogItemId = catalogItemId;
            SetQuantity(quantity);
            UnitPrice = unitPrice;
        }

        public int Id { get; private set; }

        // تعداد آیتم
        public int Quantity { get; private set; }

        // قیمت واحد
        public decimal UnitPrice { get; private set; }

        // ارتباط با محصول
        public long CatalogItemId { get; private set; }
        public CatalogItem CatalogItem { get; private set; }

        // ارتباط با سبد خرید
        public int BasketId { get; private set; }
        public Basket Basket { get; private set; }
        public bool IsRemoved { get; private set; }

        // متد برای soft delete
        public void MarkAsRemoved()
        {
            IsRemoved = true;
        }

        // متد برای بازیابی (در صورت نیاز)
        public void MarkAsUnremoved()
        {
            IsRemoved = false;
        }

        // متد برای افزایش تعداد
        public void AddQuantity(int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));

            SetQuantity(Quantity + quantity);
        }

        // متد برای تنظیم تعداد
        public void SetQuantity(int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));

            Quantity = quantity;
        }

        // محاسبه قیمت کل برای این آیتم
        public decimal GetTotalPrice()
        {
            return Quantity * UnitPrice;
        }

        // بررسی موجودی (می‌تواند بعداً با موجودی واقعی محصول چک شود)
        public bool IsStockAvailable(int availableStock)
        {
            return Quantity <= availableStock;
        }
    }
}