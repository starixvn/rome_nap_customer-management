$(document).ready(function () {
    $(document).on('input', '.number-separator', function (e) {
        var currentValue = numeral().unformat($(this).val());
        $(this).val(numeral(currentValue).format(CMSConstants.NumberFormat.IntNumber));
    });
});