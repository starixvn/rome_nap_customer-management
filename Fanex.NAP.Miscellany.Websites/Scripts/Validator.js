/* File Created: March 7, 2014 */
var Validator = (function ($) {
    'use strict';

    return {
        showError: function ($obj, message, gravity) {
            if ($obj.is(':hidden')) {
                return;
            }

            $obj.attr('title', message);
            $obj.tipsy(
            {
                trigger: 'manual',
                gravity: gravity ? gravity : 'sw',
                classStyle: 'error',
                inDocument: false,
                opacity: 0.97
            });
            $obj.tipsy('show');
            $obj.focus();

            // Hide error when changing the field.
            $obj.change(function () {
                $obj.tipsy('hide');
            });
        },
        clearError: function ($obj) {
            $obj.tipsy('hide');
            $obj.removeAttr('original-title');
        },
        clearErrors: function ($form) {
            $form.find('[original-title]').each(function () {
                $(this).tipsy('hide');
                $(this).removeAttr('original-title');
            });
        },
        valicateRequiredField: function ($obj, gravity) {
            var errMsg = $obj.attr('data-val-required');
            var value = $.trim($obj.val());

            if (errMsg && !$obj.prop('disabled') && !value) {
                Validator.showError($obj, errMsg, gravity);
                return false;
            } else {
                return true;
            }
        },
        validateNumberField: function ($obj, gravity) {
            if (!$obj.attr('original-title')) {
                var errMsg = $obj.attr('data-val-number');
                var value = $.trim($obj.val().replace(/,/g, ''));

                if (errMsg && !$obj.prop('disabled') && (!$.isNumeric(value))) {
                    Validator.showError($obj, errMsg, gravity);
                    return false;
                }
            }

            return true;
        },
        validateMinLengthField: function ($obj, gravity) {
            if (!$obj.attr('original-title')) {
                var errMsg = $obj.attr('data-val-length');
                var value = $obj.val();
                var minLen = parseInt($obj.attr('data-min-length'));

                if (errMsg && !$obj.prop('disabled') && (value.length < minLen)) {
                    Validator.showError($obj, errMsg, gravity);
                    return false;
                }
            }

            return true;
        },
        validateRegexField: function ($obj, gravity) {
            if (!$obj.attr('original-title')) {
                var errMsg = $obj.attr('data-val-regex');
                var value = $obj.val();
                var pattern = new RegExp($obj.attr('data-val-regex-pattern'));

                if (errMsg && !$obj.prop('disabled') && !pattern.test(value)) {
                    Validator.showError($obj, errMsg, gravity);
                    return false;
                }
            }

            return true;
        },
        validateIPField: function ($obj, gravity) {
            if (!$obj.attr('original-title')) {
                var errMsg = $obj.attr('data-val-ip');
                var ip = $.trim($obj.val());
                var ipArray = ip.split('.');
                var isValid = true;

                if (errMsg && !$obj.prop('disabled')) {
                    $.each(ipArray, function (index, part) {
                        if (part !== '' && parseInt(part) > 255) {
                            isValid = false;
                        }
                    });
                }

                if (!isValid) {
                    Validator.showError($obj, errMsg, gravity);
                    return false;
                }
            }

            return true;
        },
        validateForm: function (form, gravity) {
            var errorCounter = 0;

            Validator.clearErrors(form);

            $('[data-val-required]', form).each(function () {
                if (!Validator.valicateRequiredField($(this), gravity)) {
                    errorCounter++;
                }
            });

            $('[data-val-number]', form).each(function () {
                if (!Validator.validateNumberField($(this), gravity)) {
                    errorCounter++;
                }
            });

            $('[data-val-length]', form).each(function () {
                if (!Validator.validateMinLengthField($(this), gravity)) {
                    errorCounter++;
                }
            });

            $('[data-val-regex]', form).each(function () {
                if (!Validator.validateRegexField($(this), gravity)) {
                    errorCounter++;
                }
            });

            $('[data-val-ip]', form).each(function () {
                if (!Validator.validateIPField($(this), gravity)) {
                    errorCounter++;
                }
            });

            return errorCounter === 0;
        }
    };
} (jQuery));