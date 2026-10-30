//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//

// Scroll-reveal helper for Jenin.FontAwesome.Blazor animation triggers.
//
// This file is intentionally interop-free: it self-initializes, watches for trigger elements
// added later by Blazor renders or the Font Awesome engine, and reveals them with a single
// shared IntersectionObserver. Content is only hidden after this script runs (the .js gate in
// the companion CSS), so pages without JavaScript always show their icons.
//
// The script deliberately performs at most one class addition per node (the visible marker).
// Blazor-owned renders and the Font Awesome engine both replace observed nodes, so anything
// more chatty risks an enhancement loop; the companion CSS carries the hidden state.
//
// Include after the Blazor script when Icon.AnimationTrigger.ScrollReveal is used:
//
//   <script src="_content/Jenin.FontAwesome.Blazor/js/animation-triggers.js"></script>

(function () {
    "use strict";

    var TARGET = "jfa-anim-reveal-target";
    var VISIBLE = "jfa-anim-reveal-visible";

    // Nodes already handed to the observer, tracked by identity: Blazor re-renders and the
    // Font Awesome engine replace observed nodes with fresh equivalents.
    var seen = typeof WeakSet === "undefined" ? null : new WeakSet();

    // A single shared observer for the whole page.
    var observer = null;

    function reveal(entry) {
        if (!entry.isIntersecting) {
            return;
        }

        entry.target.classList.add(VISIBLE);

        if (observer) {
            observer.unobserve(entry.target);
        }
    }

    function ensureObserver() {
        if (!observer) {
            observer = new IntersectionObserver(function (entries) {
                for (var index = 0; index < entries.length; index++) {
                    reveal(entries[index]);
                }
            });
        }

        return observer;
    }

    function observe(root) {
        if (typeof IntersectionObserver === "undefined") {
            return;
        }

        document.documentElement.classList.add("js");

        var nodes = root.querySelectorAll ? root.querySelectorAll("." + TARGET) : [];

        for (var index = 0; index < nodes.length; index++) {
            var node = nodes[index];

            if (seen && seen.has(node)) {
                continue;
            }

            ensureObserver().observe(node);

            if (seen) {
                seen.add(node);
            }
        }
    }

    function watch(root) {
        if (typeof MutationObserver === "undefined") {
            return;
        }

        var pending = false;

        var rescan = function () {
            pending = false;
            observe(document.documentElement);
        };

        var mutations = new MutationObserver(function () {
            if (!pending) {
                pending = true;
                if (window.requestAnimationFrame) {
                    window.requestAnimationFrame(rescan);
                } else {
                    window.setTimeout(rescan, 0);
                }
            }
        });

        mutations.observe(root, { childList: true, subtree: true });
    }

    function init() {
        observe(document.documentElement);
        watch(document.documentElement);
    }

    if (document.readyState === "loading") {
        document.addEventListener("DOMContentLoaded", init);
    } else {
        init();
    }
})();
