"use strict";
(function ($) {
    var elements = {
        resultMessageDiv: $("#div-result-message"),
        usernameTextbox: $("#txt-username"),
        currencyDropdown: $("#ddl-currency"),
        table: $("#globalsetting-list"),
        addGlobalSettingForm: $("#formAddGlobalSetting")
    };
    function renderCurrencyDropdown() {
        var html = [], currency;

        for (var i = 0, max = currencies.length; i < max; i++) {
            currency = currencies[i];
            html.push('<option value="');
            html.push(currency.Id);
            html.push('">');
            html.push(currency.Name);
            html.push("</option>");
        }

        if (html.length) {
            currency = currencies[0];
            elements.currencyDropdown.empty().append(html.join("")).val(currency.Id);
            $("#ddl-currency.ui-combobox input").val(currency.Name);
        }
    }
    $(function () {
       

        renderCurrencyDropdown();

       
        
    });
})(jQuery);