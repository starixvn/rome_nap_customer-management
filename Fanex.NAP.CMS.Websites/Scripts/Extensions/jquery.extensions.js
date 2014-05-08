/* File Created: March 7, 2014 */
'use strict';

(function ($) {
    // Add clear field icon to textbox which will have the same functionality with the IE9's native textbox.
    $.fn.clearfield = function () {
        var $this = (this);

        $this.addClass('clearable').focus(function () {
            if ($this.val() !== '') {
                $this.addClass('x-icon');
            }
        }).focusout(function () {
            $this.removeClass('x-icon');
        }).keyup(function () {
            if ($this.val() !== '') {
                $this.addClass('x-icon');
            } else {
                $this.removeClass('x-icon');
            }
        }).mousemove(function (e) {
            if ($this.hasClass('x-icon')) {
                if (this.offsetWidth - 18 < e.clientX - this.getBoundingClientRect().left) {
                    $this.addClass('on-x-icon');
                } else {
                    $this.removeClass('on-x-icon');
                }
            }
        });

        $(document).on('click', '.on-x-icon', function () {
            $(this).val('');
            $(this).removeClass('x-icon');
            $(this).removeClass('on-x-icon');
        });

        return $this;
    };

    // Mask input field to allow enter IP format only.
    $.fn.ipAddress = function () {
        var $this = $(this);

        if ($this.mask) {
            var options = {
                onKeyPress: function (cep, event, currentField) {
                    if (cep) {
                        var ipArray = cep.split('.');
                        var lastValue = ipArray[ipArray.length - 1];
                        var resultingValue = '';

                        if (lastValue !== '' && parseInt(lastValue) > 255) {
                            ipArray[ipArray.length - 1] = '255';
                            resultingValue = ipArray.join(".");
                            currentField.val(resultingValue);
                        }
                    }
                },
                translation: {
                    'Z': { pattern: /[0-9]/, optional: true }
                }
            };

            $this.mask('0ZZ.0ZZ.0ZZ.0ZZ', options);
        }
    };

    // Check box group.
    $.fn.checkboxGroup = function () {
        var $this = $(this),
            id = $(this).attr('id'),
            childCheckboxes = $('input[type="checkbox"][data-chk-group="' + id + '"]'),

            updateGroupValue = function (groupId, value) {
                var group = $('input[type="checkbox"][data-chk-group="' + groupId + '"]'),
                    groupChk = $('input[type="checkbox"][id="' + groupId + '"]');

                if (value) {
                    if (group.filter(':checked').length === group.length) {
                        groupChk.trigger('update', true);
                    }
                } else {
                    if (group.filter(':checked').length === group.length - 1) {
                        groupChk.trigger('update', false);
                    }
                }
            };

        $this.change(function () {
            $this.trigger('updatingGroup');

            if ($this.is(':checked')) {
                childCheckboxes.prop('checked', true);
            } else {
                childCheckboxes.prop('checked', false);
            }

            childCheckboxes.trigger('change');
            $this.trigger('updatedGroup');
        });

        $this.on('update', function (event, value) {
            var groupId = $this.attr('data-chk-group');

            $(this).prop('checked', value);
            updateGroupValue(groupId, value);
        });

        childCheckboxes.click(function () {
            var groupId = $(this).attr('data-chk-group'),
                value = $(this).is(':checked');

            updateGroupValue(groupId, value);
        });
    };

    // Input field allows digits and on exceptional character at the beginning of the input value.
    $.fn.numberField = function (digits, allowNegative, firstChar) {
        var $this = $(this);

        // Get pointer position in the text box.
        var getCaret = function (element) {
            var position = 0;
            var ctrl = element.get(0);
            var ctrlSelStart = ctrl.selectionStart;
            var selection = document.selection;
            var selRange = null;

            if (ctrlSelStart || ctrlSelStart === '0') {
                position = ctrlSelStart;
            } else if (selection) {
                ctrl.focus();
                selRange = selection.createRange();
                selRange.moveStart('character', -ctrl.value.length);
                position = selRange.text.length;
            }

            return position;
        };

        // Set pointer position in the text box.
        var setCaret = function (element, position) {
            var ctrl = element.get(0);
            var selRange = null;

            if (ctrl.setSelectionRange) {
                ctrl.focus();
                ctrl.setSelectionRange(position, position);
            } else if (ctrl.createTextRange) {
                selRange = ctrl.createTextRange();
                selRange.collapse(true);
                selRange.moveEnd('character', position);
                selRange.moveStart('character', position);
                selRange.select();
            }
        };

        var getSelectedText = function(element) {
            var ctrl = element.get(0);
            var value = ctrl.value;

            if (ctrl.selectionStart !== undefined) {
                return value.substr(ctrl.selectionStart, ctrl.selectionEnd - ctrl.selectionStart);
            } else if (document.selection) {
                return document.selection.createRange().text;
            } else {
                return '';
            }
        };

        $this.on('keypress', function (event) {
            var $obj = $(this);
            var value = $.trim($obj.val());
            var decimalPos = value.indexOf('.');
            var minusPos = value.indexOf('-');
            var cursorPos = getCaret($obj);
            var keyCode = event.which;
            var selectedText = getSelectedText($obj);

            // Allow ctrl, shilf, alt, tab, enter, backspace, delete, home, end, escape, left, right, up, down keys.
            if (event.ctrlKey || event.shilfKey || event.altkey ||
                (event.keyCode >= 35 && event.keyCode <= 40) ||
                ($.inArray(event.keyCode, [8, 9, 27, 13, 190]) !== -1) ||
                (keyCode === 0 && event.keyCode === 46)) {
                return;
            }

            // Allow decimal character if there is no decimal character has been input yet
            // and some digits have already been input before the decimal character.
            if ((digits > 0) &&
                (keyCode === 46) &&
                ((decimalPos < 0) || (decimalPos > 0 && cursorPos <= decimalPos)) &&
                (value.length > 0)) {
                return;
            }

            // Allow only one minus character at the begining of the value.
            if (allowNegative && keyCode === 45 &&
                ((cursorPos === 0 && minusPos < 0) || (selectedText.length === value.length))) {
                return;
            }

            // Allow digits.
            if ((48 <= keyCode) && (keyCode <= 57)) {
                // Prevent any digit if there is a alphabet character input.
                if (value.length > 0 && firstChar && firstChar.length > 0 &&
                    value[0].toLowerCase() === firstChar[0].toLowerCase() &&
                    selectedText.length !== value.length) {
                    return false;
                }

                // Check whether the number of digits after the decimal meets the requrement or not.
                if ((decimalPos < 0) ||
                    (decimalPos > 0 && cursorPos <= decimalPos) ||
                    ((decimalPos >= 0) && (value.length - decimalPos <= digits))) {
                    return;
                }
            }

            // Allow an exception at the beginning of string.
            if (firstChar && firstChar.length > 0 &&
                (value.length === 0 || selectedText.length === value.length) &&
                firstChar[0].toLowerCase() === String.fromCharCode(keyCode).toLowerCase()) {
                return;
            }

            return false;
        }).on('keyup', function (event) {
            var $obj = $(this);
            var value = $.trim($obj.val());
            var rawValue = value.replace(/,/g, '');

            if ($.isNumeric(rawValue)) {
                var decimalPos = rawValue.indexOf('.');
                var cursorPos = getCaret($obj);
                var cursorLastPos = cursorPos === value.length;

                rawValue = rawValue === '' ? '0' : rawValue;

                if (decimalPos < 0) {
                    value = numeral(rawValue).format('0,0');
                } else if (decimalPos < rawValue.length - 1) {
                    value = numeral(rawValue).format('0,0.0[000]');
                }

                if ((48 <= event.keyCode && event.keyCode <= 57) ||
                    (96 <= event.keyCode && event.keyCode <= 105) ||
                    (event.keyCode === 45 || event.keyCode === 46) ||
                    (event.keyCode === 109 || event.keyCode === 110)) {
                    cursorPos = cursorLastPos ? value.length : cursorPos;
                    $obj.val(value);
                    setCaret($obj, cursorPos);
                }
            }
        });

        return $this;
    };

    // Auto-refresh control.
    $.fn.autoRefresh = function (values, defaultValue, callbackFunction, auto) {
        var $obj = $(this);
        var $refreshValue = $('<span class="refresh-value"></span>');
        var $refreshIcon = $('<span id="spanRefreshIcon" class="refresh-icon"></span>');
        var $dropdownIcon = $('<span class="dropdown-icon"></span>');
        var $options = $('<ul class="refresh-options"></ul>');
        var refreshTime = 0;
        var timeout;

        // Count down timer.
        var runTimer = function () {
            var counter = refreshTime;

            clearInterval(timeout);

            if (counter > 0) {
                timeout = setInterval(function () {
                    counter--;
                    $refreshValue.text(counter);

                    if (counter === 0) {
                        callbackFunction();
                        
                        if (auto) {
                            counter = refreshTime;
                        } else {
                            clearInterval(timeout);
                        }
                    }
                }, 1000);
            }
        };
        
        refreshTime = defaultValue || 0;
        defaultValue = refreshTime === 0 ? 'Not Refresh' : refreshTime;
        callbackFunction = callbackFunction || function () {};
        auto = auto === undefined ? true : auto;

        // Add not refresh value.
        $options.append($('<li class="option" data-value="0">Not Refresh</li>'));

        $.each(values, function (index, item) {
            $options.append($('<li class="option" data-value="' + item + '">' + item + ' sec</li>'));
        });

        $refreshValue.text(defaultValue);
        $refreshIcon.click(callbackFunction);
        $dropdownIcon.click(function () {
            $options.toggle();
        });

        $options.find('li').click(function () {
            var selectedValue = parseInt($(this).attr('data-value'));

            refreshTime = selectedValue;
            selectedValue = selectedValue === 0 ? 'Not Refresh' : selectedValue;
            $refreshValue.text(selectedValue);
            runTimer();
        });

        $('html').click(function (event) {
            if (!$(event.target).hasClass('dropdown-icon')) {
                $options.hide();
            }
        });

        $refreshIcon.append($refreshValue);
        $obj.addClass('auto-refresh').append($refreshIcon).append($dropdownIcon).append($options);
        runTimer();

        $obj.run = function () {
            runTimer();
        };

        $obj.stop = function () {
            clearInterval(timeout);
        };

        return $obj;
    };
})(jQuery);