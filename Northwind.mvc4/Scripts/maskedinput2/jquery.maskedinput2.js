var allowedSpecialCharKeyCodes = [46, 8, 37, 39, 35, 36, 9];
var numberKeyCodes = [44, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105];
var commaKeyCode = [188];
var decimalKeyCode = [190, 110];

function numbersOnly(event) {
    var legalKeyCode =
        (!event.shiftKey && !event.ctrlKey && !event.altKey)
                &&
            (jQuery.inArray(event.keyCode, allowedSpecialCharKeyCodes) >= 0
                ||
    jQuery.inArray(event.keyCode, numberKeyCodes) >= 0);
    if (legalKeyCode === false)
        event.preventDefault();
}
function numbersAndCommasOnly(event) {
    var legalKeyCode =
        (!event.shiftKey && !event.ctrlKey && !event.altKey)
                &&
            (jQuery.inArray(event.keyCode, allowedSpecialCharKeyCodes) >= 0
                ||
    jQuery.inArray(event.keyCode, numberKeyCodes) >= 0
                ||
    jQuery.inArray(event.keyCode, commaKeyCode) >= 0);
    if (legalKeyCode === false)
        event.preventDefault();
}
function decimalsOnly(event) {
    var legalKeyCode =
        (!event.shiftKey && !event.ctrlKey && !event.altKey)
                &&
            (jQuery.inArray(event.keyCode, allowedSpecialCharKeyCodes) >= 0
                ||
    jQuery.inArray(event.keyCode, numberKeyCodes) >= 0
                ||
    jQuery.inArray(event.keyCode, commaKeyCode) >= 0
                ||
    jQuery.inArray(event.keyCode, decimalKeyCode) >= 0);
    if (legalKeyCode === false)
        event.preventDefault();
}
function currenciesOnly(event) {
    var legalKeyCode =
        (!event.shiftKey && !event.ctrlKey && !event.altKey)
                &&
            (jQuery.inArray(event.keyCode, allowedSpecialCharKeyCodes) >= 0
                ||
    jQuery.inArray(event.keyCode, numberKeyCodes) >= 0
                ||
    jQuery.inArray(event.keyCode, commaKeyCode) >= 0
                ||
    jQuery.inArray(event.keyCode, decimalKeyCode) >= 0);
    // Allow for $
    if (!legalKeyCode && event.shiftKey && event.keyCode == 52)
        legalKeyCode = true;
    if (legalKeyCode === false)
        event.preventDefault();
}