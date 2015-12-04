/// <reference path="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js" />


/**
 * Assign [action] to [fromName] and Submit the form
 * @param {String} formName Form to apply action to 
 * @param {String} action Action to apply to Form
 * @return {Number} sum
 */
function submitForm(formName, action) {
    document.getElementById(formName).action = action;
    document.getElementById(formName).submit();
}