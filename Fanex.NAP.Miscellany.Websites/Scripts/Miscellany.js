/* File Created: March 3, 2014 */
/* jshint freeze: false */
'use strict';

var Miscellany = (function ($) {

    return {
        MiscellanyRoot: null,
        dialogMsg: null,
        dialogPopup: null,
        headlineMsgBox: null,
        loadingBox: null,
        alertCallback: null,
        confirmCallback: null,

        accessDeniedError: null,
        notFoundError: null,
        internalError: null,

        msieVersion: null,
        agentIsOldIe: null,
        agentIsApple: null,
        agentIsMobile: null,

        errorSuccessMsg: null,

        pagerRanges: [50, 100, 200, 500],
        pageSize: 50,

        formats: {
            DateFormat: 'mm/dd/yy'
        },

        initCoreValues: function () {
            Miscellany.MiscellanyRoot = $('#MiscellanyRoot').val();
            Miscellany.dialogPopup = $('#dialog-popup');
            Miscellany.accessDeniedError = $('#AccessDeniedError').val();
            Miscellany.notFoundError = $('#NotFoundError').val();
            Miscellany.internalError = $('#InternalError').val();

            Miscellany.msieVersion = Miscellany.getIeVersion();
            Miscellany.agentIsOldIe = Miscellany.msieVersion !== -1 && Miscellany.msieVersion < 9.0;
            Miscellany.agentIsApple = Miscellany.agentIs('apple') != null;
            Miscellany.agentIsMobile = Miscellany.agentIs('mobile') != null;

            Miscellany.errorSuccessMsg = $('#divErrorSuccessMsg');
        },
        resolveRootUrl: function (relativeUrl) {
            return Miscellany.MiscellanyRoot + relativeUrl;
        },
        // Handles common error when making ajax request
        handleAjaxError: function (xhr) {
            if (xhr.status === 403) {
                Miscellany.headlineMsgBox.showErr(Miscellany.accessDeniedError, { sticky: true });
            } else if (xhr.status === 404) {
                Miscellany.headlineMsgBox.showErr(Miscellany.notFoundError, { sticky: true });
            } else {
                Miscellany.headlineMsgBox.showErr(Miscellany.internalError, { sticky: true });
            }
        },
        showInfoMessage: function (message) {
            Miscellany.headlineMsgBox.showInfo(message, { duration: 3000 });
        },
        showErrorMessage: function (message) {
            Miscellany.headlineMsgBox.showErr(message, { duration: 3000 });
        },
        showWarningMessage: function (message) {
            Miscellany.headlineMsgBox.showWarn(message, { duration: 3000 });
        },
        showInlineErrorMsg: function (message, $obj) {
            var $msgDiv = $obj || Miscellany.errorSuccessMsg;

            $msgDiv.html(message);
            $msgDiv.removeClass('success').addClass('error').slideDown();
            setTimeout(function () {
                Miscellany.hideInlineMsg($msgDiv);
            }, 3000);
        },
        showInlineInfoMsg: function (message, $obj) {
            var $msgDiv = $obj || Miscellany.errorSuccessMsg;
            console.log('su');
            $msgDiv.html(message);
            $msgDiv.removeClass('error').addClass('success').slideDown();
            setTimeout(function () {
                Miscellany.hideInlineMsg($msgDiv);
            }, 3000);
        },
        hideInlineMsg: function ($obj) {
            var $msgDiv = $obj || Miscellany.errorSuccessMsg;

            $msgDiv.removeClass('error').removeClass('success').slideUp();
            $msgDiv.html('');
        },
        /* jshint ignore:start */
        getIeVersion: function () {
            var version = -1; // Return value assumes failure.

            if (window.navigator.userAgent.indexOf("MSIE") > 0) {
                var usrAgent = navigator.userAgent;
                var reg = new RegExp("MSIE ([0-9]{1,}[\.0-9]{0,})");

                if (reg.exec(usrAgent) != null) {
                    version = parseFloat(RegExp.$1);
                }
            }

            return version;
        },
        /* jshint ignore:end */
        agentIs: function (name) {
            switch (name.toLowerCase()) {
                case 'ipad':
                    return navigator.userAgent.match(/iPad/i);
                case 'iphone':
                    return navigator.userAgent.match(/iPhone/i);
                case 'ipod':
                    return navigator.userAgent.match(/iPod/i);
                case 'apple':
                    return navigator.userAgent.match(/iPad/i) ||
                        navigator.userAgent.match(/iPhone/i) ||
                        navigator.userAgent.match(/iPod/i);
                case 'mobile':
                    return navigator.userAgent.match(/iPad/i) ||  // Apple
                        navigator.userAgent.match(/iPhone/i) ||
                        navigator.userAgent.match(/iPod/i) ||
                        navigator.userAgent.match(/BlackBerry/i) || // RIM
                        navigator.userAgent.match(/PlayBook/i) ||
                        navigator.userAgent.match(/Windows Phone/i) || // Windows Phone
                        navigator.userAgent.match(/Android/i) || // Android
                        navigator.userAgent.match(/Touch/i);  // General
                default:
                    return null;
            }
        },
        modal: function (title, html, option) {
            Miscellany.dialogPopup.html(html).dialog({
                width: 'auto',
                modal: true,
                position: ['center', 160],
                title: title
            });

            if (option) {
                Miscellany.dialogPopup.dialog('option', option);
            }
        },
        popup: function (title, html, option) {
            Miscellany.dialogPopup.html(html).dialog({
                width: 'auto',
                modal: false,
                position: ['center', 160],
                title: title
            });

            if (option) {
                Miscellany.dialogPopup.dialog('option', option);
            }
        },
        closePopup: function () {
            Miscellany.dialogPopup.dialog('close');
        },
        downloadFile: function (url, data) {
            //url and data options required
            if (url && data) {
                //data can be string of parameters or array/object
                data = typeof data === 'string' ? data : $.param(data);
                var downloadUrl = url + "?" + data;
                var iframe = $('body iframe');
                if (iframe.length === 0) {
                    iframe = $('<iframe width="0" height="0" class="display-none"></iframe>');
                    iframe.appendTo('body');
                }

                iframe.attr('src', downloadUrl);
            }
        },
        disableInput: function () {
            try { $.blockUI(); } catch (e) { }
        },
        enableInput: function () {
            try { $.unblockUI(); } catch (e) { }
        },
        navigateToRoot: function (url) {
            window.location.replace(Miscellany.resolveRootUrl(url));
        },
        htmlEncode: function (value) {
            return $('<div/>').text(value).html();
        },
        htmlDecode: function (value) {
            return $('<div/>').html(value).text();
        },
        showSuccessMsg: function (msg, url, msec) {
            var msgWrp = $('.validation-summary-valid');

            msgWrp.addClass('success')
                .find('li:first').html(Miscellany.htmlDecode(msg)).show();

            Miscellany.showSuccessMsgCountDown(url, msec / 1000);
        },
        showSuccessMsgCountDown: function (url, msec) {
            if (msec-- > 0) {
                setTimeout('Miscellany.showSuccessMsgCountDown("' + url + '", ' + msec + ')', 1000);
                $('.validation-summary-valid .msec').html(msec);
            } else {
                window.location.replace(Miscellany.resolveClientUrl(url));
            }
        },

        encodeURI: function (str) {
            // ref: https://developer.mozilla.org/en-US/docs/JavaScript/Reference/Global_Objects/encodeURI
            return encodeURI(str).replace(/%5B/g, '[').replace(/%5D/g, ']');
        }
    };

} (jQuery));

$(document).ready(function () {
    Miscellany.initCoreValues();

    Miscellany.headlineMsgBox = $.msgbox({
        message: '',
        //containerId: 'msg-bar',
        type: 'err',
        modal: false,
        position: ['top:130', 'center'],
        theme: '',
        zIndex: 9999,
        useAnimation: false
    });

    Miscellany.loadingBox = $.msgbox({
        message: 'Loading...',
        type: 'warn',
        modal: true,
        position: ['top:0', 'center'],
        closeBtn: false,
        theme: '',
        zIndex: 9999,
        sticky: true,
        useAnimation: false
    });
});