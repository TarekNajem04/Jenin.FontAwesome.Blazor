"use strict";

// https://prismjs.com/docs/Prism.html
class PrismUtilities {
    /**
     * This is the most high-level function in Prism’s API.
     * It fetches all the elements that have a `.language-xxxx` class and then calls {@link Prism.highlightElement} on
     * each one of them.
     *
     * This is equivalent to `Prism.highlightAllUnder(document, async, callback)`.
     *
     * @param {boolean} [asyncopt=false] Same as in {@link Prism.highlightAllUnder}.
     * @param {HighlightCallback(element)} [callback] Same as in {@link Prism.highlightAllUnder}.
     * @memberof PrismUtilities
     * @public
     */
    highlightAll(asyncopt, callback) {
        Prism.highlightAll(asyncopt, callback);
    };

    /**
     * Fetches all the descendants of container that have a .language-xxxx class and then calls Prism.highlightElement on each one of them.
     *
     * @param {ParentNode} [container] The root element, whose descendants that have a .language-xxxx class will be highlighted.
     * @param {boolean} [asyncopt=false] Same as in {@link Prism.highlightAllUnder}.
     * @param {HighlightCallback(element)} [callback] Same as in {@link Prism.highlightAllUnder}.
     * @memberof PrismUtilities
     * @public
     */
    highlightAllUnder(container, asyncopt, callback) {
        Prism.highlightAllUnder(container, asyncopt, callback);
    };

    /**
     * Fetches all the descendants of container that have a .language-xxxx class and then calls Prism.highlightElement on each one of them.
     *
     * @param {string} [containerId] The root element Id, whose descendants that have a .language-xxxx class will be highlighted.
     * @param {boolean} [asyncopt=false] Same as in {@link Prism.highlightAllUnder}.
     * @param {HighlightCallback(element)} [callback] Same as in {@link Prism.highlightAllUnder}.
     * @memberof PrismUtilities
     * @public
     */
    highlightAllUnderById(containerId, asyncopt, callback) {
        var container = document.getElementById(containerId);

        if (container)
            Prism.highlightAllUnder(container, asyncopt, callback);
    };

    /**
     * Fetches all the descendants of container that have a .language-xxxx class and then calls Prism.highlightElement on each one of them.
     *
     * @param {Element} [element] The element containing the code.
     * It must have a class of language-xxxx to be processed, where xxxx is a valid language identifier.
     * @param {boolean} [asyncopt=false] Same as in {@link Prism.highlightAllUnder}.
     * @param {HighlightCallback(element)} [callback] Same as in {@link Prism.highlightAllUnder}.
     * @memberof PrismUtilities
     * @public
     */
    highlightElement(element, asyncopt, callback) {
        Prism.highlightElement(element, asyncopt, callback);
    };

    /**
     * Fetches all the descendants of container that have a .language-xxxx class and then calls Prism.highlightElement on each one of them.
     *
     * @param {string} [elementId] The element id containing the code.
     * It must have a class of language-xxxx to be processed, where xxxx is a valid language identifier.
     * @param {boolean} [asyncopt=false] Same as in {@link Prism.highlightAllUnder}.
     * @param {HighlightCallback(element)} [callback] Same as in {@link Prism.highlightAllUnder}.
     * @memberof PrismUtilities
     * @public
     */
    highlightElementById(elementId, asyncopt, callback) {
        var element = document.getElementById(elementId);

        if (element)
            Prism.highlightElement(element, asyncopt, callback);
    };

}

window["PrismUtilities"] = new PrismUtilities();