"use strict";

class FontAwesomeConfigUtilities {
    setAutoReplaceSvg(value) {
        if (window.FontAwesomeConfig) {
            window.FontAwesomeConfig.autoReplaceSvg = value;
        }
    };

    getAutoReplaceSvg() {
        if (window.FontAwesomeConfig) {
            return window.FontAwesomeConfig.autoReplaceSvg;
        }

        return null;
    };
}

window["FontAwesomeConfigUtilities"] = new FontAwesomeConfigUtilities();
