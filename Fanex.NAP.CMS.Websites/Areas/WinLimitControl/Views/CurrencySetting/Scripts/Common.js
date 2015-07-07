/* global CMS */

var winlimitctrl = winlimitctrl || {};
(function ($) {
    'use strict';

    winlimitctrl.parseFloat = function(value) {
        if (isNaN(value)) {
            return -1;
        }

        return parseFloat(value);
    };

    winlimitctrl.renderSelectHtml = function(data) {
        var item, html = [];
        for (var i = 0, max = data.length; i < max; i++) {
            item = data[i];
            html.push('<option value="');
            html.push(item.Id);
            html.push('">');
            html.push(item.Name);
            html.push('</option>');
        }

        return html.join('');
    };

    winlimitctrl.renderNumber = function(tblWinLimit) {
        var spanNumbers = tblWinLimit.find('.spanNumber');
        $.each(spanNumbers, function (index, value) {
            value.innerHTML = index + 1;
        });
    };
   
    winlimitctrl.showAddedResults = function(formContainer, results, callbackFunc) {
        var failed = false;
        var html = [];
        $.each(results, function (index, value) {
            html.push(value.Message);
            if (value.Code !== 0) {
                failed = true;
            }
        });

        var message = html.join('<br/>');
        var messageContainer = formContainer.find('.inline-message');
        var hideMessageAndReloadGlobalSetting = function () {
            CMS.closePopup();
            callbackFunc();
        };

        if (failed) {
            CMS.showInlineInfoMsg(message, messageContainer, true, 15 * 1000, hideMessageAndReloadGlobalSetting);
        }
        else {
            CMS.showInlineSuccessMsg(message, messageContainer, true, hideMessageAndReloadGlobalSetting);
        }
    };

    winlimitctrl.duplicatedSelectValue = function(selects, duplicatedMessage) {
        var uniques = {};

        var $select;
        var selectValue;
        for (var i = 0, max = selects.length; i < max; i++) {
            $select = $(selects[i]);
            selectValue = $select.val();
            if (!uniques[selectValue]) {
                uniques[selectValue] = true;
            } 
            else {
                CMS.showTipsyError($select.next(), duplicatedMessage);
                return true;
            }
        }
        
        return false;    
    };
})(jQuery);