// ابزارهای Ajax (خام)
window.ajax = {
    get: function (url, cb) { $.get(url, cb); },
    post: function (url, data, cb) { $.post(url, data, cb); }
};