// wwwroot/js/basket-update.js

class BasketUpdater {
    constructor() {
        this.basketElement = document.getElementById('basket-dropdown');
        this.init();
    }

    init() {
        // گوش دادن به تغییرات سبد خرید
        this.setupEventListeners();

        // شروع به‌روزرسانی دوره‌ای
        this.startPeriodicUpdate();
    }

    setupEventListeners() {
        // گوش دادن به رویدادهای مربوط به سبد خرید
        document.addEventListener('basketUpdated', (e) => {
            this.refreshBasket();
        });
    }

    startPeriodicUpdate() {
        // به‌روزرسانی هر 30 ثانیه
        setInterval(() => {
            this.refreshBasket();
        }, 30000);
    }

    async refreshBasket() {
        try {
            const response = await fetch('/basket/mini', {
                method: 'GET',
                headers: {
                    'X-Requested-With': 'XMLHttpRequest'
                }
            });

            if (response.ok) {
                const html = await response.text();
                this.updateBasketUI(html);
            }
        } catch (error) {
            console.error('Error refreshing basket:', error);
        }
    }

    updateBasketUI(html) {
        // به‌روزرسانی بخش سبد خرید در نوار导航
        const tempDiv = document.createElement('div');
        tempDiv.innerHTML = html;
        const newBasket = tempDiv.querySelector('.dropdown');

        if (newBasket && this.basketElement) {
            this.basketElement.replaceWith(newBasket);
            this.basketElement = newBasket;

            // راه‌اندازی مجدد event listeners
            this.setupBasketEventListeners();
        }
    }

    setupBasketEventListeners() {
        // راه‌اندازی event listeners برای سبد خرید جدید
        const removeButtons = this.basketElement.querySelectorAll('form[action*="RemoveItem"]');
        removeButtons.forEach(button => {
            button.addEventListener('submit', this.handleRemoveItem.bind(this));
        });
    }

    handleRemoveItem(e) {
        e.preventDefault();

        if (confirm('آیا از حذف این محصول مطمئن هستید؟')) {
            const form = e.target;
            this.submitForm(form);
        }
    }

    async submitForm(form) {
        try {
            const formData = new FormData(form);
            const response = await fetch(form.action, {
                method: 'POST',
                body: formData
            });

            if (response.ok) {
                // انتشار رویداد به‌روزرسانی سبد خرید
                document.dispatchEvent(new CustomEvent('basketUpdated'));
            }
        } catch (error) {
            console.error('Error removing item:', error);
        }
    }
}

// راه‌اندازی هنگامی که DOM آماده است
document.addEventListener('DOMContentLoaded', function () {
    new BasketUpdater();
});