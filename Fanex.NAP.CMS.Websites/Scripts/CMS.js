/* File Created: March 3, 2014 */
'use strict';

var CMS = (function ($) {

    return {
        CMSRoot: null,
        dialogMsg: null,
        dialogPopup: null,
        headlineMsgBox: null,
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

        pagerRanges: [50, 100, 200, 300, 500],
        pageSize: 50,

        formats: {
            DateFormat: 'mm/dd/yy'
        },

        initCoreValues: function () {
            CMS.CMSRoot = $('#CMSRoot').val();
            CMS.dialogPopup = $('#dialog-popup');
            CMS.accessDeniedError = $('#AccessDeniedError').val();
            CMS.notFoundError = $('#NotFoundError').val();
            CMS.internalError = $('#InternalError').val();

            CMS.msieVersion = CMS.getIeVersion();
            CMS.agentIsOldIe = CMS.msieVersion !== -1 && CMS.msieVersion < 9.0;
            CMS.agentIsApple = CMS.agentIs('apple') != null;
            CMS.agentIsMobile = CMS.agentIs('mobile') != null;

            CMS.errorSuccessMsg = $('#divErrorSuccessMsg');

            CMS.headlineMsgBox = $.msgbox({
                message: '',
                //containerId: 'msg-bar',
                type: 'err',
                modal: false,
                position: ['top:130', 'center'],
                theme: '',
                zIndex: 9999,
                useAnimation: false
            });
        },
        resolveRootUrl: function (relativeUrl) {
            return CMS.CMSRoot + relativeUrl;
        },
        // Handles common error when making ajax request
        handleAjaxError: function (xhr) {
            if (xhr.status === 403) {
                CMS.headlineMsgBox.showErr(CMS.accessDeniedError, { sticky: true });
            } else if (xhr.status === 404) {
                CMS.headlineMsgBox.showErr(CMS.notFoundError, { sticky: true });
            } else {
                CMS.headlineMsgBox.showErr(CMS.internalError, { sticky: true });
            }
        },
        showInfoMessage: function (message) {
            CMS.headlineMsgBox.showInfo(message, { duration: 3000 });
        },
        showErrorMessage: function (message) {
            CMS.headlineMsgBox.showErr(message, { duration: 3000 });
        },
        showWarningMessage: function (message) {
            CMS.headlineMsgBox.showWarn(message, { duration: 3000 });
        },
        showInlineAjaxErrorMsg: function (xhr, $obj) {
            var $msgDiv = $obj || CMS.errorSuccessMsg;
            var message = CMS.internalError;

            if (xhr.status === 403) {
                message = CMS.accessDeniedError;
            } else if (xhr.status === 404) {
                message = CMS.notFoundError;
            }

            $msgDiv.html(message);
            $msgDiv.removeClass('success').addClass('error').slideDown();
        },
        showInlineErrorMsg: function (message, $obj) {
            var $msgDiv = $obj || CMS.errorSuccessMsg;

            $msgDiv.html(message);
            $msgDiv.removeClass('success').addClass('error').slideDown();
        },
        showInlineInfoMsg: function (message, $obj, autoHide, callbackFunc) {
            var $msgDiv = $obj || CMS.errorSuccessMsg;

            autoHide = autoHide === undefined ? true : autoHide;
            $msgDiv.html(message);
            $msgDiv.removeClass('error').addClass('success').slideDown();

            if (autoHide) {
                setTimeout(function () {
                    CMS.hideInlineMsg($msgDiv, callbackFunc);
                }, 3000);
            } else if (callbackFunc) {
                callbackFunc();
            }
        },
        hideInlineMsg: function ($obj, callbackFunc) {
            var $msgDiv = $obj || CMS.errorSuccessMsg;

            callbackFunc = callbackFunc || function () { };
            $msgDiv.removeClass('error').removeClass('success').slideUp(callbackFunc);
            $msgDiv.html('');
        },
        getIeVersion: function () {
            var version = -1; // Return value assumes failure.

            if (window.navigator.userAgent.indexOf("MSIE") > 0) {
                var usrAgent = navigator.userAgent;
                var reg = new RegExp("MSIE ([0-9]{1,}[.0-9]{0,})");

                if (reg.exec(usrAgent) != null) {
                    version = parseFloat(RegExp.$1);
                }
            }

            return version;
        },
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
            CMS.dialogPopup.html(html).dialog({
                width: 'auto',
                modal: true,
                position: ['center', 160],
                title: title
            });

            if (option) {
                CMS.dialogPopup.dialog('option', option);
            }
        },
        popup: function (title, html, option) {
            CMS.dialogPopup.html(html).dialog({
                width: 'auto',
                modal: false,
                position: ['center', 160],
                title: title
            });

            if (option) {
                CMS.dialogPopup.dialog('option', option);
            }
        },
        closePopup: function () {
            CMS.dialogPopup.dialog('close');
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
            window.location.replace(CMS.resolveRootUrl(url));
        },
        htmlEncode: function (value) {
            return $('<div/>').text(value).html();
        },
        htmlDecode: function (value) {
            return $('<div/>').html(value).text();
        },

        encodeURI: function (str) {
            // ref: https://developer.mozilla.org/en-US/docs/JavaScript/Reference/Global_Objects/encodeURI
            return encodeURI(str).replace(/%5B/g, '[').replace(/%5D/g, ']');
        }
    };

} (jQuery));

$(document).ready(function () {
    CMS.initCoreValues();
});