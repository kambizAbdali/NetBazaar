// Global site functions
$(document).ready(function () {
    initializeSite();
});

function initializeSite() {
    // Initialize tooltips
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });

    // Initialize popovers
    var popoverTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="popover"]'));
    var popoverList = popoverTriggerList.map(function (popoverTriggerEl) {
        return new bootstrap.Popover(popoverTriggerEl);
    });

    // Auto-dismiss alerts
    $('.alert').not('.alert-permanent').delay(5000).fadeOut(400);

    // Add to cart functionality
    $(document).on('click', '.add-to-cart', function () {
        const productId = $(this).data('product-id');
        addToCart(productId);
    });
}

function addToCart(productId, quantity = 1) {
    $.ajax({
        url: '/api/cart/add',
        type: 'POST',
        data: { productId, quantity },
        success: function (response) {
            showNotification('محصول به سبد خرید اضافه شد', 'success');
            updateCartHeader(response);
        },
        error: function () {
            showNotification('خطا در افزودن به سبد خرید', 'error');
        }
    });
}

function updateCartHeader(cart) {
    $('.cart-summary').text(cart.totalItems + ' محصول - ' + cart.finalPrice.toLocaleString() + ' تومان');
}

function showNotification(message, type = 'info') {
    const alertClass = {
        'success': 'alert-success',
        'error': 'alert-danger',
        'warning': 'alert-warning',
        'info': 'alert-info'
    }[type] || 'alert-info';

    const notification = $(`
        <div class="alert ${alertClass} alert-dismissible fade show notification" role="alert">
            ${message}
            <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
        </div>
    `);

    $('body').append(notification);

    setTimeout(() => {
        notification.alert('close');
    }, 5000);
}

// Persian Datepicker initialization
function initializeDatepicker(selector) {
    $(selector).persianDatepicker({
        format: 'YYYY/MM/DD',
        autoClose: true,
        initialValue: false,
        position: "auto",
        observer: true,
        calendar: {
            persian: {
                locale: "fa",
                showHint: true
            }
        }
    });
}

// AJAX global setup
$.ajaxSetup({
    beforeSend: function (xhr) {
        xhr.setRequestHeader("RequestVerificationToken", $('input[name="__RequestVerificationToken"]').val());
    }
});

document.addEventListener('DOMContentLoaded', function () {
    // نمونه: دکمه رفتن به بالای صفحه
    const goTop = document.createElement('button');
    goTop.textContent = '▲';
    goTop.className = 'btn btn-primary position-fixed';
    goTop.style.bottom = '20px';
    goTop.style.left = '20px';
    goTop.style.opacity = '0.6';
    goTop.addEventListener('click', () => window.scrollTo({ top: 0, behavior: 'smooth' }));
    document.body.appendChild(goTop);

    // نمونه: نمایش سال جاری در فوتر
    const yearSpan = document.getElementById('currentYear');
    if (yearSpan) {
        yearSpan.textContent = new Date().getFullYear();
    }
});