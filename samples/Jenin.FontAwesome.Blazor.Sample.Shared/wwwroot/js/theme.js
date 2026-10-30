/*
//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
*/

/*
 * Global light/dark theme for the sample host. The chosen theme is stored on the <html> element as
 * `data-theme="light|dark"`; the CSS keys every override off that attribute. The preference is
 * persisted in localStorage, and when none has been saved yet the operating-system preference wins.
 * An inline bootstrap script in the document head calls `window.sampleTheme.apply()` before first
 * paint so the page never flashes the wrong palette.
 */
(function () {
    'use strict';

    var STORAGE_KEY = 'jenin-fa-theme';
    var media = window.matchMedia('(prefers-color-scheme: dark)');

    function stored() {
        try {
            var value = window.localStorage.getItem(STORAGE_KEY);
            return value === 'light' || value === 'dark' ? value : null;
        } catch (error) {
            return null;
        }
    }

    function preferred() {
        return stored() || (media.matches ? 'dark' : 'light');
    }

    function apply(theme) {
        var resolved = theme === 'light' || theme === 'dark' ? theme : preferred();
        document.documentElement.setAttribute('data-theme', resolved);
        document.documentElement.style.colorScheme = resolved;
        window.dispatchEvent(new CustomEvent('themechange', { detail: { theme: resolved } }));
        return resolved;
    }

    function persist(theme) {
        try {
            window.localStorage.setItem(STORAGE_KEY, theme);
        } catch (error) {
            /* Storage can be unavailable (private mode); fall back to in-memory only. */
        }
    }

    window.sampleTheme = {
        current: function () {
            return document.documentElement.getAttribute('data-theme') || preferred();
        },
        set: function (theme) {
            var resolved = apply(theme);
            persist(resolved);
            return resolved;
        },
        toggle: function () {
            return window.sampleTheme.set(window.sampleTheme.current() === 'dark' ? 'light' : 'dark');
        },
        apply: function () {
            return apply(preferred());
        }
    };

    // Follow the operating-system preference until the user makes an explicit choice.
    media.addEventListener('change', function () {
        if (!stored()) {
            apply(preferred());
        }
    });

    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', function () {
            apply(preferred());
        });
    } else {
        apply(preferred());
    }
})();
