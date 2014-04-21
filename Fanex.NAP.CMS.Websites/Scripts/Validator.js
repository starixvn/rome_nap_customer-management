/* File Created: March 7, 2014 */
var Validator = (function ($) {
    'use strict';

    return {
        showError: function ($obj, message, gravity) {
            if ($obj.is(':hidden')) {
                return;
            }

            $obj.attr('title', message);
            $obj.tipsy({
                trigger: 'manual',
                gravity: gravity ? gravity : 'sw',
                type: 'error',
                inDocument: false,
                opacity: 0.97
            });
            $obj.tipsy('show').triggerHandler('raiseError');
            $obj.focus();

            // Hide error when changing the field.
            $obj.change(function () {
                Validator.clearError($obj);
            });
        },
        clearError: function ($obj) {
            $obj.tipsy('hide');
            $obj.removeAttr('original-title').triggerHandler('clearError');
        },
        clearErrors: function ($form) {
            $form.find('[original-title]').each(function () {
                Validator.clearError($(this));
            });
        },
        validateRequiredField: function ($obj, gravity) {
            var $ele = $obj;
            var errMsg = $obj.attr('data-val-required');
            var value;

            if (!$obj.is('input') && !$obj.is('select') && !$obj.is('textarea')) {
                $ele = $obj.find('input:first');
            }

            value = $.trim($ele.val());

            if (errMsg && !$ele.prop('disabled') && !value) {
                Validator.showError($obj, errMsg, gravity);
                return false;
            } else {
                return true;
            }
        },
        validateNumberField: function ($obj, gravity) {
            if (!$obj.attr('original-title')) {
                var $ele = $obj;
                var errMsg = $obj.attr('data-val-number');
                var value;

                if (!$obj.is('input') && !$obj.is('select') && !$obj.is('textarea')) {
                    $ele = $obj.find('input:first');
                }

                value = $.trim($ele.val().replace(/,/g, ''));

                if (errMsg && !$ele.prop('disabled') && (!$.isNumeric(value))) {
                    Validator.showError($obj, errMsg, gravity);
                    return false;
                }
            }

            return true;
        },
        validateMinLengthField: function ($obj, gravity) {
            if (!$obj.attr('original-title')) {
                var $ele = $obj;
                var errMsg = $obj.attr('data-val-length');
                var minLen = parseInt($obj.attr('data-min-length'));
                var value;

                if (!$obj.is('input') && !$obj.is('select') && !$obj.is('textarea')) {
                    $ele = $obj.find('input:first');
                }

                value = $ele.val();

                if (errMsg && !$ele.prop('disabled') && (value.length < minLen)) {
                    Validator.showError($obj, errMsg, gravity);
                    return false;
                }
            }

            return true;
        },
        validateRegexField: function ($obj, gravity) {
            if (!$obj.attr('original-title')) {
                var $ele = $obj;
                var errMsg = $obj.attr('data-val-regex');
                var pattern = new RegExp($obj.attr('data-val-regex-pattern'));
                var value;

                if (!$obj.is('input') && !$obj.is('select') && !$obj.is('textarea')) {
                    $ele = $obj.find('input:first');
                }

                value = $ele.val();

                if (errMsg && !$ele.prop('disabled') && !pattern.test(value)) {
                    Validator.showError($obj, errMsg, gravity);
                    return false;
                }
            }

            return true;
        },
        validateIPField: function ($obj, gravity) {
            if (!$obj.attr('original-title')) {
                var $ele = $obj;
                var errMsg = $obj.attr('data-val-ip');
                var isValid = true;
                var ip;
                var ipArray;

                if (!$obj.is('input') && !$obj.is('select') && !$obj.is('textarea')) {
                    $ele = $obj.find('input:first');
                }

                ip = $.trim($ele.val());
                ipArray = ip.split('.');

                if (errMsg && !$ele.prop('disabled')) {
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
        validateForm: function ($form, gravity) {
            var errorCounter = 0;

            Validator.clearErrors($form);

            $('[data-val-required]', $form).each(function () {
                if (!Validator.validateRequiredField($(this), gravity)) {
                    errorCounter++;
                }
            });

            $('[data-val-number]', $form).each(function () {
                if (!Validator.validateNumberField($(this), gravity)) {
                    errorCounter++;
                }
            });

            $('[data-val-length]', $form).each(function () {
                if (!Validator.validateMinLengthField($(this), gravity)) {
                    errorCounter++;
                }
            });

            $('[data-val-regex]', $form).each(function () {
                if (!Validator.validateRegexField($(this), gravity)) {
                    errorCounter++;
                }
            });

            $('[data-val-ip]', $form).each(function () {
                if (!Validator.validateIPField($(this), gravity)) {
                    errorCounter++;
                }
            });

            return errorCounter === 0;
        },
        getValidatedFields: function ($form) {
            var fields = [];

            fields.push($('[data-val-required]', $form));
            fields.push($('[data-val-number]', $form));
            fields.push($('[data-val-length]', $form));
            fields.push($('[data-val-regex]', $form));
            fields.push($('[data-val-ip]', $form));

            return fields;
        },
        countErrors: function ($form) {
            return $('[original-title]', $form).length;
        }
    };
} (jQuery));