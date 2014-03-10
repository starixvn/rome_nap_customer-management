/* File Created: March 7, 2014 */
(function ($) {
    // Add clear field icon to textbox which will have the same functionality with the IE9's native textbox.
    $.fn.clearfield = function () {
        var $this = (this);

        $this.addClass('clearable').focus(function () {
            if ($this.val() !== '') {
                $this.addClass('x-icon');
            }
            console.log($this.val() !== '');
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
                onKeyPress: function (cep, event, currentField, options) {
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
})(jQuery);