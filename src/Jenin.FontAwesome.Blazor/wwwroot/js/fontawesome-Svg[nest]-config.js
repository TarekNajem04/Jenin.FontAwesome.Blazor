"use strict";
/*
 Before we use this approach, we must run this code to Prevent fontawesome to translate tag to svg tag:
 Notice how this gets configured before we load Font Awesome
 */

window.FontAwesomeConfig = {
    autoReplaceSvg: "nest",
    showMissingIcons: true,
};