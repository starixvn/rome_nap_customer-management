/* File Created: March 6, 2014 */
/* jshint freeze: false */
/* jshint strict: false */

// String Format
String.prototype.format = function () {
    var args = arguments;
    return this.replace(/{(\d+)}/g, function (match, number) {
        return typeof args[number] !== 'undefined' ? args[number] : match;
    });
};

// Encode string: replace special characters by their codes.
String.prototype.encode = function () {
    return this.replace(/&/g, '&amp;').replace(/>/g, '&gt;').replace(/</g, '&lt;').replace(/"/g, '&quot;');
};

// Decode string: replace special characters' codes by their real values.
String.prototype.decode = function () {
    return this.replace(/&amp;/g, '&').replace(/&gt;/g, '>').replace(/&lt;/g, '<').replace(/&quot;/g, '"');
};

// Array Remove - By John Resig (MIT Licensed)
Array.prototype.remove = function (from, to) {
    var rest = this.slice((to || from) + 1 || this.length);
    this.length = from < 0 ? this.length + from : from;
    return this.push.apply(this, rest);
};