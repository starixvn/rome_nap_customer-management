$(document).ready(function () {
    $(document).on('input', '.number-separator', function (e) {
        if ($(this).val() === '-' || $(this).val() === '+') {
            return;
        }

        var currentValue = numeral().unformat($(this).val());
        $(this).val(numeral(currentValue).format(CMSConstants.NumberFormat.IntNumber));
    });
});