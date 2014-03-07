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
})(jQuery);