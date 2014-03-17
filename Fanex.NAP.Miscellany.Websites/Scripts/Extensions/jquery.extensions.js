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
            childCheckboxes = $('input[type="checkbox"][data-chk-group="' + id + '"]');

        $this.change(function () {
            if ($this.is(':checked')) {
                childCheckboxes.prop('checked', true);
            } else {
                childCheckboxes.prop('checked', false);
            }
        });

        childCheckboxes.change(function () {
            var groupId = $(this).attr('data-chk-group'),
                group = $('input[type="checkbox"][data-chk-group="' + groupId + '"]'),
                groupChk = $('input[type="checkbox"][id="' + groupId + '"]');

            if ($(this).is(':checked') &&
                group.filter(':checked').length === group.length) {
                groupChk.prop('checked', true);
            } else {
                groupChk.prop('checked', false);
            }
        });
    };
})(jQuery);