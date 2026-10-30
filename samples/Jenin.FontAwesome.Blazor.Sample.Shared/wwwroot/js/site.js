
function enableAllTooltips() {
    document.querySelectorAll('[data-bs-toggle="tooltip"]')
        .forEach(function (tooltip) {
            new bootstrap.Tooltip(tooltip);
        });
}

// Resets the sample content scroller to the top when the user navigates to a different page. The
// content region is an inner scroll container, so Blazor's enhanced navigation leaves its offset
// untouched unless we reset it explicitly.
function scrollSampleContentToTop() {
    var content = document.querySelector('.app-content');
    if (content) {
        content.scrollTop = 0;
    }
}
