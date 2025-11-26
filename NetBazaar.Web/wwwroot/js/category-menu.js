document.addEventListener("DOMContentLoaded", function () {
    initializeCustomCategoryMenu();
});

function initializeCustomCategoryMenu() {
    // مدیریت کلیک روی هدر دسته‌بندی‌ها
    document.querySelectorAll('.category-header').forEach(header => {
        header.addEventListener('click', function (e) {
            e.preventDefault();
            e.stopPropagation();

            const parentItem = this.parentElement;
            const submenu = parentItem.querySelector(':scope > .category-submenu');

            if (submenu) {
                // بستن سایر منوهای هم‌سطح
                closeSiblingMenus(parentItem);

                // toggle منوی جاری
                parentItem.classList.toggle('active');
            }
        });
    });

    // جستجوی پیشرفته
    const searchInput = document.getElementById('searchInput');
    if (searchInput) {
        let searchTimeout;
        searchInput.addEventListener('input', function () {
            clearTimeout(searchTimeout);
            searchTimeout = setTimeout(() => {
                const keyword = this.value.trim();
                handleSearch(keyword);
            }, 300);
        });

        // پاک کردن جستجو با ESC
        searchInput.addEventListener('keydown', function (e) {
            if (e.key === 'Escape') {
                this.value = '';
                handleSearch('');
            }
        });
    }

    // بستن منوها با کلیک خارج
    document.addEventListener('click', function (e) {
        if (!e.target.closest('.custom-category-menu')) {
            closeAllMenus();
        }
    });

    console.log('Custom category menu initialized');
}

function closeSiblingMenus(currentItem) {
    const siblings = Array.from(currentItem.parentElement.children);
    siblings.forEach(sibling => {
        if (sibling !== currentItem && sibling.classList.contains('active')) {
            sibling.classList.remove('active');
        }
    });
}

function closeAllMenus() {
    document.querySelectorAll('.category-item.active').forEach(item => {
        item.classList.remove('active');
    });
}

function handleSearch(keyword) {
    const output = document.getElementById('searchResult');

    if (!keyword) {
        output.innerHTML = '';
        closeAllMenus();
        removeHighlights();
        return;
    }

    const result = searchInCategories(keyword);

    if (result) {
        output.innerHTML = `
            <div class="search-result-item">
                <strong>مسیر:</strong> ${result.path} 
                <br>
                <a href="${result.link}" class="btn btn-sm btn-outline-primary mt-2">
                    مشاهده دسته
                </a>
            </div>
        `;
    } else {
        output.innerHTML = '<div class="text-danger text-center">نتیجه‌ای یافت نشد</div>';
    }
}

function searchInCategories(keyword) {
    let found = null;
    const searchTerm = keyword.toLowerCase();

    removeHighlights();
    closeAllMenus();

    const allItems = document.querySelectorAll('.category-item');

    // از حلقه قابل شکستن استفاده می‌کنیم تا پس از یافتن اولین نتیجه خارج شویم
    for (const item of allItems) {
        // فقط فرزند مستقیم .category-link یا .category-header را بگیر
        const link = item.querySelector(':scope > .category-link');
        const header = item.querySelector(':scope > .category-header');
        const textElement = link ? link.querySelector(':scope > .category-label') : header ? header.querySelector(':scope > .category-label') : null;

        if (textElement) {
            const text = textElement.textContent.toLowerCase();

            if (text.includes(searchTerm)) {
                found = highlightAndOpenPath(item);
                break; // اولین نتیجه را برگردان و حلقه را قطع کن
            }
        }
    }

    return found;
}

function highlightAndOpenPath(startItem) {
    let path = [];
    let targetLink = null;
    let current = startItem;

    // ساخت مسیر از پایین به بالا
    while (current) {
        // فقط فرزند مستقیم هر آیتم را بگیر تا از گرفتن لینک/لیبل فرزند جلوگیری شود
        const link = current.querySelector(':scope > .category-link');
        const header = current.querySelector(':scope > .category-header');
        const label = link ? link.querySelector(':scope > .category-label') : header ? header.querySelector(':scope > .category-label') : null;

        if (label) {
            const text = label.textContent.trim();
            path.unshift(text); // اضافه کردن به ابتدای آرایه

            // هایلایت کردن
            label.classList.add('highlight');

            // ذخیره اولین لینک معتبر (لینک مستقیم همین آیتم)
            if (link && link.href && !targetLink) {
                targetLink = link.href;
            }
        }

        // باز کردن منوهای والد
        if (current.classList.contains('has-children')) {
            current.classList.add('active');
        }

        // رفتن به والد مستقیم .category-item
        current = current.parentElement ? current.parentElement.closest('.category-item') : null;
    }

    // اگر لینک مستقیم پیدا نشد، از startItem لینک بگیر
    if (!targetLink) {
        const directLink = startItem.querySelector(':scope > .category-link');
        if (directLink && directLink.href) {
            targetLink = directLink.href;
        }
    }

    return {
        path: path.join(' → '),
        link: targetLink || '#'
    };
}

function removeHighlights() {
    document.querySelectorAll('.highlight').forEach(el => {
        el.classList.remove('highlight');
    });
}

// توابع کمکی برای استفاده خارجی
function openCategory(categoryId) {
    const targetItem = document.querySelector(`[data-category-id="${categoryId}"]`);
    if (targetItem) {
        let current = targetItem;
        while (current) {
            if (current.classList.contains('has-children')) {
                current.classList.add('active');
            }
            current = current.parentElement ? current.parentElement.closest('.category-item') : null;
        }
    }
}

function collapseAll() {
    closeAllMenus();
    removeHighlights();
    const searchResult = document.getElementById('searchResult');
    const searchInput = document.getElementById('searchInput');
    if (searchResult) searchResult.innerHTML = '';
    if (searchInput) searchInput.value = '';
}
