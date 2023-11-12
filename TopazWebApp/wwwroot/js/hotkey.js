window.blazorHotkeys = {
    registerHotkey: function (key, elementId) {
        document.addEventListener('keydown', function (e) {
            if (e.key === key) {
                var element = document.getElementById(elementId);
                if (element) {
                    element.click();
                }
            }
        });
    }
};