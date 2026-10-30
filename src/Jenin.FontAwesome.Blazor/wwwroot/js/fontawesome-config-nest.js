"use strict";

/*
    Font Awesome enhanced (SVG) mode.

    Sets only autoReplaceSvg to 'nest', preserving any unrelated FontAwesomeConfig options the
    host has already defined. Non-destructive and idempotent; safe under duplicate inclusion.

    Must load before the Font Awesome runtime script.
*/

window.FontAwesomeConfig = Object.assign({}, window.FontAwesomeConfig, { autoReplaceSvg: "nest" });
