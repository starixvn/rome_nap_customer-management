/* File Created: March 3, 2014 */
/* global keyboardManager */
/* jshint strict: false,
 camelcase: false */
// 'use strict';

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

        showMenus: null,
        popupTopMargin: null, // Used for re-position popups when re-direct from accounting site.

        msieVersion: null,
        agentIsOldIe: null,
        agentIsApple: null,
        agentIsMobile: null,

        errorSuccessMsg: null,

        pagerRanges: [50, 100, 200, 300, 500],
        pageSize: 50,

        formats: {
            DateFormat: 'mm/dd/yy',
            IntegerFormat: '0,0',
            DecimalFormat: '0,0.00'
        },

        initCoreValues: function () {
            CMS.CMSRoot = $('#CMSRoot').val();
            CMS.dialogPopup = $('#dialog-popup');
            CMS.accessDeniedError = $('#AccessDeniedError').val();
            CMS.notFoundError = $('#NotFoundError').val();
            CMS.internalError = $('#InternalError').val();

            CMS.showMenus = $('#ShowMenus').val() === 'True';
            CMS.popupTopMargin = !CMS.showMenus ? 40 : 0;

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

            // Close error/success message popup when the popup is closed.
            CMS.dialogPopup.on('dialogclose', function () {
                CMS.closeMsgBox();
            });
        },
        resolveRootUrl: function (relativeUrl) {
            return CMS.CMSRoot + relativeUrl;
        },

        // Handles common error when making ajax request
        handleAjaxError: function (xhr) {
            var message = CMS.internalError;

            if (xhr.status === 403) {
                message = CMS.accessDeniedError;
            } else if (xhr.status === 404) {
                message = CMS.notFoundError;
            }

            CMS.showErrorMessage(message);
        },
        showInfoMessage: function (message) {
            var msgWrapper = '<div id="divError" class="info">' + message + '</div>';
            var options = {
                sticky: true,
                position: ['top: ' + (40 - CMS.popupTopMargin), 'center'], //['top: 40', 'center'],
                css: 'custom-msgbox'
            };

            CMS.headlineMsgBox.showInfo(msgWrapper, options);
        },
        showErrorMessage: function (message) {
            var msgWrapper = '<div id="divError" class="error">' + message + '</div>';
            var options = {
                sticky: true,
                position: ['top: ' + (40 - CMS.popupTopMargin), 'center'], //['top: 40', 'center'],
                css: 'custom-msgbox'
            };

            CMS.headlineMsgBox.showMsgBox(msgWrapper, options);
        },
        showSuccessMessage: function (message, callbackFunc) {
            var msgWrapper = '<div id="divError" class="success">' + message + '</div>';
            var options = {
                position: ['top: ' + (40 - CMS.popupTopMargin), 'center'], //['top: 40', 'center'],
                css: 'custom-msgbox',
                duration: 3000
            };

            callbackFunc = callbackFunc || function () {
            };
            CMS.headlineMsgBox.showMsgBox(msgWrapper, options);
            setTimeout(callbackFunc, 3000);
        },
        showMixMessage: function (infoMsg, errorMsg) {
            var msgWrapper = '<div class="mix-msg"><div id="divInfo" class="success">' + infoMsg + '</div>' +
                '<div id="divError" class="error">' + errorMsg + '</div></div>';
            var options = {
                sticky: true,
                position: ['top: ' + (40 - CMS.popupTopMargin), 'center'], //['top: 40', 'center'],
                css: 'custom-msgbox'
            };

            CMS.headlineMsgBox.showMsgBox(msgWrapper, options);
            setTimeout(function () {
                CMS.headlineMsgBox.target.find('#divInfo').slideUp();
            }, 3000);
        },
        showConfirmMessage: function (message, callbackFunc) {
            var options = {
                modal: true,
                closeBtn: false,
                header: true,
                css: 'custom-confirm'
            };

            callbackFunc = callbackFunc || function () {
            };
            CMS.headlineMsgBox.confirm(message, callbackFunc, options);
        },
        showErrorBox: function (message, callbackFunc) {
            var options = {
                modal: true,
                closeBtn: false,
                header: true,
                css: 'custom-confirm'
            };

            callbackFunc = callbackFunc || function () {
            };
            CMS.headlineMsgBox.error(message, callbackFunc, options);
        },
        closeMsgBox: function () {
            CMS.headlineMsgBox.close();
        },

        // Show inline message in the case of form or popup.
        showInlineErrorMsg: function (message, $obj) {
            $obj.html(message);
            $obj.removeClass('success').addClass('error').slideDown();
        },
        showInlineWarningMsg: function (message, $obj) {
            $obj.html(message);
            $obj.removeClass('success error').addClass('warn').slideDown();
        },
        showInlineSuccessMsg: function (message, $obj, autoHide, callbackFunc, timeout = 3000) {
            autoHide = autoHide === undefined ? true : autoHide;
            $obj.html(message);
            $obj.removeClass('error').removeClass('info').addClass('success').slideDown();

            if (autoHide) {
                setTimeout(function () {
                    CMS.hideInlineMsg($obj, callbackFunc);
                }, timeout);
            } else if (callbackFunc) {
                callbackFunc();
            }
        },
        showInlineInfoMsg: function (message, $obj, autoHide, timespan, callbackFunc) {
            autoHide = autoHide === undefined ? true : autoHide;
            $obj.html(message);
            $obj.removeClass('error').removeClass('success').addClass('info').slideDown();

            if (autoHide) {
                timespan = timespan || 5000;
                setTimeout(function () {
                    CMS.hideInlineMsg($obj, callbackFunc);
                }, timespan);
            } else if (callbackFunc) {
                callbackFunc();
            }
        },
        hideInlineMsg: function ($obj, callbackFunc) {
            callbackFunc = callbackFunc || function () {
            };
            $obj.removeClass('error').removeClass('info').slideUp(callbackFunc);
            $obj.html('');
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

        nonResizableModal: function (title, html, option) {
            CMS.dialogPopup.html(html).dialog({
                width: 'auto',
                modal: true,
                height: option.height,
                resizable: false,
                position: ['center', 160 - CMS.popupTopMargin], //['center', 160],
                title: title
            });

            if (option) {
                CMS.dialogPopup.dialog('option', option);
            }
        },
        modal: function (title, html, option) {
            var $dialog = CMS.dialogPopup.html(html).dialog({
                width: 'auto',
                modal: true,
                position: ['center', 160 - CMS.popupTopMargin], //['center', 160],
                title: title
            });

            if (option) {
                CMS.dialogPopup.dialog('option', option);
            }

            return $dialog;
        },
        popup: function (title, html, option) {
            CMS.dialogPopup.html(html).dialog({
                width: 'auto',
                modal: false,
                position: ['center', 160 - CMS.popupTopMargin], //['center', 160],
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
        downloadFileWithPost: function (url, data) {
            //url and data options required
            if (url && data) {
                //data can be string of parameters or array/object
                let json = (typeof data === "string") ? data : JSON.stringify(data);

                let form = $('<form>')
                    .attr('method', 'POST')
                    .attr('action', url)
                    .attr('target', 'download_iframe');

                form.append(
                    $('<input>')
                        .attr('type', 'hidden')
                        .attr('name', 'request')
                        .val(json)
                );

                let iframe = $('#download_iframe');
                if (iframe.length === 0) {
                    iframe = $('<iframe>', {
                        id: 'download_iframe',
                        name: 'download_iframe'
                    }).css('display', 'none').appendTo('body');
                }

                form.appendTo('body').submit().remove();
            }
        },
        disableInput: function () {
            try {
                $.blockUI();
            } catch (e) {
            }
        },
        enableInput: function () {
            try {
                $.unblockUI();
            } catch (e) {
            }
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
        },

        callAjax: function (reportHandler,
            url,
            requestParams,
            successFunction,
            errorFunction,
            completeFunction) {
            $.ajax({
                type: 'POST',
                url: url,
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                data: JSON.stringify(requestParams),
                success: function (result) {
                    if (successFunction && typeof (successFunction) === 'function') {
                        successFunction.call(reportHandler, result);
                    }
                },
                error: function (xhr) {
                    if (errorFunction && typeof (errorFunction) === 'function') {
                        errorFunction.call(reportHandler, xhr);
                    } else {
                        CMS.handleAjaxError(xhr);
                    }
                },
                complete: function () {
                    if (completeFunction && typeof (completeFunction) === 'function') {
                        completeFunction();
                    } else {
                        CMS.enableInput();
                    }
                }
            });
        },
        getJson: function (url,
            successFunction,
            errorFunction,
            completeFunction) {
            $.ajax({
                type: 'GET',
                url: url,
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                beforeSend: function () {
                    CMS.disableInput();
                },
                success: function (result) {
                    if (successFunction && typeof (successFunction) === 'function') {
                        successFunction(result);
                    }
                },
                error: function (xhr) {
                    if (errorFunction && typeof (errorFunction) === 'function') {
                        errorFunction(xhr);
                    } else {
                        CMS.handleAjaxError(xhr);
                    }
                },
                complete: function () {
                    if (completeFunction && typeof (completeFunction) === 'function') {
                        completeFunction();
                    } else {
                        CMS.enableInput();
                    }
                }
            });
        },
        getHtml: function (url,
            successFunction,
            errorFunction,
            completeFunction) {
            $.ajax({
                type: 'GET',
                url: url,
                contentType: 'application/json; charset=utf-8',
                dataType: 'html',
                beforeSend: function () {
                    CMS.disableInput();
                },
                success: function (result) {
                    if (successFunction && typeof (successFunction) === 'function') {
                        successFunction(result);
                    }
                },
                error: function (xhr) {
                    if (errorFunction && typeof (errorFunction) === 'function') {
                        errorFunction(xhr);
                    } else {
                        CMS.handleAjaxError(xhr);
                    }
                },
                complete: function () {
                    if (completeFunction && typeof (completeFunction) === 'function') {
                        completeFunction();
                    } else {
                        CMS.enableInput();
                    }
                }
            });
        },
        forceNumberInput: function (e) {
            var code = "";
            if (e.which) {
                code = e.which;
            }
            else if (e.keyCode) {
                code = e.keyCode;
            }

            if (code === keyboardManager.Enums.KeyCode.End ||
                code === keyboardManager.Enums.KeyCode.Home ||
                code === keyboardManager.Enums.KeyCode.LeftArrow ||
                code === keyboardManager.Enums.KeyCode.RightArrow ||
                CMS.isCtrlCombine(e, code)) {
                return true;
            }

            if ((code < keyboardManager.Enums.KeyCode.Number_0 || keyboardManager.Enums.KeyCode.Number_9 < code) &&
                code !== keyboardManager.Enums.KeyCode.Backspace &&
                code !== keyboardManager.Enums.KeyCode.Enter &&
                code !== keyboardManager.Enums.KeyCode.None &&
                (code !== keyboardManager.Enums.KeyCode.Delete || e.which) &&
                code !== keyboardManager.Enums.KeyCode.Tab) {
                return false;
            }

            return true;
        },
        forceNegativeNumberInput: function (e) {
            var code = "";
            if (e.which) {
                code = e.which;
            }
            else if (e.keyCode) {
                code = e.keyCode;
            }

            if (code === keyboardManager.Enums.KeyCode.End ||
                code === keyboardManager.Enums.KeyCode.Home ||
                code === keyboardManager.Enums.KeyCode.LeftArrow ||
                code === keyboardManager.Enums.KeyCode.RightArrow ||
                CMS.isCtrlCombine(e, code)) {
                return true;
            }

            if ((code < keyboardManager.Enums.KeyCode.Number_0 || keyboardManager.Enums.KeyCode.Number_9 < code) &&
                code !== keyboardManager.Enums.KeyCode.Backspace &&
                code !== keyboardManager.Enums.KeyCode.Negative &&
                code !== keyboardManager.Enums.KeyCode.Enter &&
                code !== keyboardManager.Enums.KeyCode.None &&
                (code !== keyboardManager.Enums.KeyCode.Delete || e.which) &&
                code !== keyboardManager.Enums.KeyCode.Tab) {
                return false;
            }

            return true;
        },
        forceNumericInput: function (e, allowDot) {
            var code = e.which || e.keyCode;
            switch (code) {
                case keyboardManager.Enums.KeyCode.None:
                case keyboardManager.Enums.KeyCode.End:
                case keyboardManager.Enums.KeyCode.Home:
                case keyboardManager.Enums.KeyCode.LeftArrow:
                case keyboardManager.Enums.KeyCode.RightArrow:
                case keyboardManager.Enums.KeyCode.Delete:
                case keyboardManager.Enums.KeyCode.Backspace:
                case keyboardManager.Enums.KeyCode.Tab:
                case keyboardManager.Enums.KeyCode.Enter:
                case keyboardManager.Enums.KeyCode.Escape:
                    return true;
            }

            if (CMS.isCtrlCombine(e, code)) {
                return true;
            }

            if (allowDot) {
                switch (code) {
                    case keyboardManager.Enums.KeyCode.Character_Period:
                    case keyboardManager.Enums.KeyCode.Character_Period1:
                        return true;
                }
            }

            return (code >= keyboardManager.Enums.KeyCode.Number_0 && code <= keyboardManager.Enums.KeyCode.Number_9) ||
                (code >= keyboardManager.Enums.KeyCode.NumberPad_0 && code <= keyboardManager.Enums.KeyCode.NumberPad_9);
        },
        formatDecimal: function () {
            var textValue = this.value.replace(/,/g, "");
            var intValue = parseInt(textValue);

            if (intValue.toString() === textValue) {
                this.value = numeral(intValue).format(CMSConstants.NumberFormat.IntNumber);
            }
        },
        isCtrlCombine: function (e, code) {
            var charFromCode = String.fromCharCode(code).toLowerCase();
            return e.ctrlKey &&
                (charFromCode === String.fromCharCode(keyboardManager.Enums.KeyCode.Character_v).toLowerCase() ||
                    charFromCode === String.fromCharCode(keyboardManager.Enums.KeyCode.Character_c).toLowerCase() ||
                    charFromCode === String.fromCharCode(keyboardManager.Enums.KeyCode.Character_a).toLowerCase());
        },
        isNumeric: function (text) {
            text = text.replace(/\,/g, "");
            var regExp = /^-?(\d+|\d+\.\d*|\d*\.\d+)$/;
            return text.match(regExp);
        },
        // gravity: nw | n | ne | w | e | sw | s | se
        showTipsyError: function ($obj, message, gravity) {
            if ($obj.is(':hidden')) {
                return;
            }

            $obj.attr('title', message);
            $obj.tipsy({
                trigger: 'manual',
                gravity: gravity ? gravity : 'sw',
                classStyle: 'error',
                inDocument: false,
                opacity: 0.97
            });
            $obj.tipsy('show').triggerHandler('raiseError');

            // FireFox will not re-focus to the control until blur event finshes.
            setTimeout(function () {
                $obj.focus();
            }, 0);

            // Hide error when changing the field.
            $obj.change(function () {
                CMS.clearTipsyError($obj);
            });
        },
        clearTipsyError: function ($obj) {
            $obj.tipsy('hide');
            $obj.removeAttr('original-title').triggerHandler('clearError');
        },

        findPosition: function (oElement) {
            if (typeof (oElement.offsetParent()) !== 'undefined') {
                for (var posX = 0, posY = 0; oElement && oElement.length; oElement = oElement.offsetParent()) {
                    posX += oElement.offset().left;
                    posY += oElement.offset().top;
                    if (oElement[0].nodeName.toLowerCase() === 'body') {
                        break;
                    }
                }

                return [posX, posY];
            } else {
                return [oElement.x, oElement.y];
            }
        },

        getData: function (url, criteria, successFunction, errorFunction, completeFunction) {
            $.ajax(
                {
                    url: url,
                    type: 'GET',
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    data: criteria,
                    beforeSend: function () {
                        CMS.disableInput();
                    },
                    success: function (result) {
                        if (successFunction && typeof (successFunction) === 'function') {
                            successFunction(result);
                        }
                    },
                    error: function (xhr) {
                        if (errorFunction && typeof (errorFunction) === 'function') {
                            errorFunction(xhr);
                        } else {
                            CMS.handleAjaxError(xhr);
                        }
                    },
                    complete: function () {
                        if (completeFunction && typeof (completeFunction) === 'function') {
                            completeFunction();
                        } else {
                            CMS.enableInput();
                        }
                    }
                });
        },

        renderGrid: function (target,
            gridOptions,
            bodyData,
            bodyMapFunc,
            footerData,
            footerMapFunc,
            callback) {
            gridOptions.bodyRows = bodyData.map(bodyMapFunc);
            if (footerData) {
                gridOptions.footerRows = footerData.map(footerMapFunc);
            }
            target.fanexGrid(gridOptions);
            if (callback) {
                callback(target, gridOptions);
            }
        },
        isValueEqual: function (a, b) {
            var aProps = Object.getOwnPropertyNames(a);
            var bProps = Object.getOwnPropertyNames(b);

            if (aProps.length != bProps.length) {
                return false;
            }

            for (var i = 0; i < aProps.length; i++) {
                var propName = aProps[i];
                if (a[propName] !== b[propName]) {
                    return false;
                }
            }

            return true;
        }
    };

}(jQuery));

$(document).ready(function () {
    CMS.initCoreValues();
});
