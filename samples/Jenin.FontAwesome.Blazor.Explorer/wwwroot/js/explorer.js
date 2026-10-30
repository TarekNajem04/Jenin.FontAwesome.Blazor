//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//

// Measures an element's width and reports changes back to the .NET grid component. The grid uses the
// width to decide how many fixed-width cards fit on a row; a round-trip happens only when the rounded
// width actually changes, so scrolling and re-renders stay cheap.
export function observeWidth(element, dotNetRef) {
    if (!element) {
        return { dispose() { } };
    }

    let last = -1;

    const notify = (width) => {
        const rounded = Math.round(width);
        if (rounded === last || rounded <= 0) {
            return;
        }

        last = rounded;
        dotNetRef.invokeMethodAsync('OnWidthChanged', rounded);
    };

    const observer = new ResizeObserver((entries) => {
        for (const entry of entries) {
            notify(entry.contentRect.width);
        }
    });

    observer.observe(element);
    notify(element.clientWidth);

    return {
        dispose() {
            observer.disconnect();
        },
    };
}

// Resets a scroll container to the top. Used when the result set changes so a new search or style
// filter starts at the first row instead of inheriting the previous scroll offset.
export function scrollToTop(element) {
    if (element) {
        element.scrollTop = 0;
    }
}

// Copies text to the system clipboard, preferring the async Clipboard API with a hidden-textarea
// fallback for browsers or contexts where the async API is unavailable (for example, non-secure
// origins during local testing). Resolves to true when the copy succeeded.
export async function copyToClipboard(text) {
    try {
        if (navigator.clipboard && window.isSecureContext) {
            await navigator.clipboard.writeText(text);
            return true;
        }
    } catch {
        // Fall through to the legacy path.
    }

    try {
        const area = document.createElement('textarea');
        area.value = text;
        area.setAttribute('readonly', '');
        area.style.position = 'fixed';
        area.style.opacity = '0';
        document.body.appendChild(area);
        area.select();
        const ok = document.execCommand('copy');
        document.body.removeChild(area);
        return ok;
    } catch {
        return false;
    }
}

// Theme persistence. The explorer stores the user's explicit choice in localStorage and stamps it on
// the root element; when nothing is stored the system preference governs (handled purely in CSS).
const THEME_KEY = 'jenin.fa.explorer.theme';

export function getStoredTheme() {
    try {
        return window.localStorage.getItem(THEME_KEY);
    } catch {
        return null;
    }
}

export function setStoredTheme(theme) {
    try {
        if (theme) {
            window.localStorage.setItem(THEME_KEY, theme);
        } else {
            window.localStorage.removeItem(THEME_KEY);
        }
    } catch {
        // Persistence is best-effort; the in-memory toggle still works.
    }
}

// Stamps the theme on the explorer root and persists it. Passing null clears the explicit choice so
// the operating-system colour scheme takes over again.
export function applyTheme(theme) {
    const root = document.querySelector('.explorer');
    if (root) {
        if (theme) {
            root.setAttribute('data-theme', theme);
        } else {
            root.removeAttribute('data-theme');
        }
    }

    setStoredTheme(theme);
}

// Reports whether the operating system currently prefers a dark colour scheme.
export function systemPrefersDark() {
    return window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches;
}

// A host page that manages its own theme (the sample application, for instance) stamps the theme on
// the document element. When such a host is present the explorer inherits that theme instead of
// keeping a separate preference, so the host's single switch controls the whole page.
export function getHostTheme() {
    try {
        const root = document.documentElement;
        return root ? root.getAttribute('data-theme') : null;
    } catch {
        return null;
    }
}

// Watches the document element for theme changes so an inheriting explorer stays in step with its
// host. The callback receives the new theme, or null when the host clears it.
export function observeHostTheme(dotNetRef) {
    const root = document.documentElement;
    if (!root || typeof MutationObserver === 'undefined') {
        return { dispose() { } };
    }

    const observer = new MutationObserver(() => {
        dotNetRef.invokeMethodAsync('OnHostThemeChanged', root.getAttribute('data-theme'));
    });

    observer.observe(root, { attributes: true, attributeFilter: ['data-theme'] });

    return {
        dispose() {
            observer.disconnect();
        },
    };
}

// Stamps the theme on the explorer root without touching the explorer's own stored preference, so
// an inherited theme never overwrites the standalone preference.
export function setRootTheme(theme) {
    const root = document.querySelector('.explorer');
    if (!root) {
        return;
    }

    if (theme) {
        root.setAttribute('data-theme', theme);
    } else {
        root.removeAttribute('data-theme');
    }
}

// Focuses an element without scrolling the page; used by the "/" and "?" shortcuts.
export function focusElement(element) {
    element?.focus({ preventScroll: true });
}

// Focuses the search input by id; used by the "/" shortcut so the caret lands in the field wherever
// the user is on the page.
export function focusSearch(id) {
    const input = document.getElementById(id);
    if (input) {
        input.focus();
        input.select();
        return true;
    }

    return false;
}

// Reports whether the currently focused element is a text-entry control, so the explorer's global
// single-letter shortcuts ("/", "?", "c") do not fire while the user is typing a search term.
export function isTypingTarget() {
    const element = document.activeElement;
    if (!element) {
        return false;
    }

    const tag = element.tagName;
    return tag === 'INPUT' || tag === 'TEXTAREA' || tag === 'SELECT' || element.isContentEditable === true;
}

// Focuses the first element matching a selector; used to place focus inside the help dialog.
export function focusSelector(selector) {
    const element = document.querySelector(selector);
    if (element) {
        element.focus();
        return true;
    }

    return false;
}

// Moves focus between the rendered grid cards. Because the grid is virtualized only a subset of cards
// exists in the DOM, so navigation is relative to the currently focused card rather than a global
// index; when the neighbour is not rendered the browser scroll only nudges the virtualizer, and the
// next keydown continues from the newly rendered card. `delta` is the row length for up/down moves.
export function moveCardFocus(delta) {
    const cards = Array.from(document.querySelectorAll('.explorer-card'));
    if (cards.length === 0) {
        return false;
    }

    const current = cards.indexOf(document.activeElement);
    const base = current >= 0 ? current : (delta > 0 ? -1 : cards.length);
    const next = Math.max(0, Math.min(cards.length - 1, base + delta));
    const target = cards[next];
    if (target) {
        target.focus({ preventScroll: false });
        return true;
    }

    return false;
}

// Moves focus to the first or last rendered card; used by Home/End.
export function moveCardFocusTo(edge) {
    const cards = Array.from(document.querySelectorAll('.explorer-card'));
    const target = edge === 'last' ? cards[cards.length - 1] : cards[0];
    if (target) {
        target.focus({ preventScroll: false });
        return true;
    }

    return false;
}

// Focuses the first rendered card; used when the search shortcut or an empty-to-populated transition
// should hand focus to the grid.
export function focusFirstCard() {
    const first = document.querySelector('.explorer-card');
    if (first) {
        first.focus({ preventScroll: false });
        return true;
    }

    return false;
}
