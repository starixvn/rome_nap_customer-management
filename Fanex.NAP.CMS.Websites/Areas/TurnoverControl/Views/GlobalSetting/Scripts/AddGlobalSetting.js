function AddGlobalSetting(reloadGridFunction, sportType) {
    "use strict";
    var domElements = null;

    function cacheDomElements() {
        domElements = {
            resultMessageDiv: $("#div-popupResult-message"),
            smaRadio: $("#rdoSMA"),
            headlineRadio: $("#rdoHeadline"),
            userNameTextbox: $("#txt-popupUsername"),
            maxTurnoverLabel: $("#lbl-maxturnover"),
            maxTurnoverTextbox: $("#txt-maxturnover")
        };
    }

    function formatMaxTurnover() {
        var tagValue = domElements.maxTurnoverTextbox.val().replace(/,/g, "");
        var maxTurnover = parseInt(tagValue);

        if (maxTurnover.toString() === tagValue) {
            domElements.maxTurnoverTextbox.val(numeral(maxTurnover).format(CMSConstants.NumberFormat.IntNumber));
        }
    }

    function getAddParams() {
        var isSuccess = true;
        var userName = $.trim(domElements.userNameTextbox.val());

        if (userName.length === 0) {
            CMS.showTipsyError(domElements.userNameTextbox, viewResources.Error_RequiredField, "w");
            isSuccess = false;
        } else {
            CMS.clearTipsyError(domElements.userNameTextbox);
        }

        var maxTurnoverValue = $.trim(domElements.maxTurnoverTextbox.val());

        if (maxTurnoverValue.length === 0) {
            CMS.showTipsyError(domElements.maxTurnoverTextbox, viewResources.Error_RequiredField, "w");
            isSuccess = false;
        }
        else if (!CMS.isNumeric(maxTurnoverValue)) {
            CMS.showTipsyError(domElements.maxTurnoverTextbox, viewResources.Error_InvalidValue, "w");
            isSuccess = false;
        }
        else {
            var maxTurnover = parseInt(maxTurnoverValue.replace(/,/g, ""));

            if (maxTurnover < 0) {
                CMS.showTipsyError(domElements.maxTurnoverTextbox, viewResources.Error_InvalidValue, "w");
                isSuccess = false;
            }
            else if (maxTurnover > maxIntValue) {
                CMS.showTipsyError(domElements.maxTurnoverTextbox, viewResources.GlobalSetting_MaxTurnover_ExceedMaxTurnover, "w");
                isSuccess = false;
            }
            else {
                CMS.clearTipsyError(domElements.maxTurnoverTextbox);
            }
        }

        if (!isSuccess) {
            return null;
        }

        return {
            UserName: userName,
            MaxTurnover: maxTurnover,
            IsGroupHeadline: domElements.headlineRadio.is(":checked"),
            SportType: sportType
        };
    }

    function addHandler(form) {
        var addParams = getAddParams();

        if (!addParams) {
            return;
        }

        CMS.disableInput();
        CMS.callAjax(
            this,
            form.action,
            addParams,
            function (data) {
                var result = $.parseJSON(data);

                if (result.Code !== 0) {
                    CMS.showInlineErrorMsg(result.Message, domElements.resultMessageDiv);
                }
                else {
                    CMS.showInlineSuccessMsg(
                        result.Message,
                        domElements.resultMessageDiv,
                        true,
                        function () {
                            CMS.closePopup();
                            reloadGridFunction();
                        });
                }
            });
    }

    this.initialize = function () {
        cacheDomElements();

        domElements.maxTurnoverTextbox
            .off()
            .on("keypress", CMS.forceNumberInput)
            .on("keyup", formatMaxTurnover);

        domElements.smaRadio
            .off("change")
            .on("change", function (e) {
                domElements.maxTurnoverLabel.text(viewResources.Max_Turnover);
            });

        domElements.headlineRadio
            .off("change")
            .on("change", function (e) {
                domElements.maxTurnoverLabel.text(viewResources.Max_Turnover + " " + viewResources.RM);
            });

        $("#formAddGlobalSetting")
            .off("submit")
            .on("submit", function (e) {
                e.preventDefault();

                addHandler(this);
            });

        $("#btn-cancel")
            .off("click")
            .on("click", CMS.closePopup);

        domElements.userNameTextbox.focus();
    };
}
