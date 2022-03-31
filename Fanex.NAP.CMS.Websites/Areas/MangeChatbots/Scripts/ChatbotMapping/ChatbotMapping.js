"use strict";
(function ($) {
    var elements = {
        resultMessageDiv: $("#div-result-message"),
        table: $("#globalsetting-list"),
        addGlobalSettingForm: $("#formAddGlobalSetting"),

        usernameTextbox: $("#txt-username"),
        currencyDropdown: $("#ddl-currency"),
        siteDropdown: $("#ddl-site"),
        statusDropdown: $("#ddl-status"),
        levelDropdown: $("#ddl-level"),
        linkStatusDropdown: $("#ddl-linkStatus"),
    };
    const renderCurrencyDropdown = () => {
        var html = [], currency = [];
        console.log("currencies", currencies)
        for (var i = 0, max = currencies.length; i < max; i++) {
            currency = currencies[i];
            html.push('<option value="');
            html.push(currency.Id);
            html.push('">');
            html.push(currency.Name);
            html.push("</option>");
        }

        if (html.length) {
            currency = currencies[1];
            elements.currencyDropdown.empty().append(html.join("")).val(currency.Id);
            $("#ddl-currency.ui-combobox input").val(currency.Name);
        }
    };
    const renderLinkStatusDropdown = () => {
        var html = [];
        const status = [
            {Id: 0, Name: "All"},
            {Id: 1, Name: "Linked"},
            {Id: 2, Name: "Verifying"},
            {Id: 3, Name: "Unlinked"}]
        console.log("status", status)
        for (var i = 0, max = status.length; i < max; i++) {
            html.push('<option value="');
            html.push(status[i].Id);
            html.push('">');
            html.push(status[i].Name);
            html.push("</option>");
        }
        console.log("html",html)
        if (html.length) {
            elements.statusDropdown.empty().append(html.join("")).val(status[0].Id);
            $("#ddl-currency.ui-combobox input").val(status[0].Name);
        }
    };
    $(function () {


        renderCurrencyDropdown();
        renderLinkStatusDropdown();
        console.log("aaaaaaaaaaaaaaaaaaaaaaaaa")

    });
})(jQuery);