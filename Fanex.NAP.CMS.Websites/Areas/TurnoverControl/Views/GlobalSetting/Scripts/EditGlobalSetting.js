function EditGlobalSetting(table, resultMessageDiv, reloadGridFunction, sportType) {
    "use strict";
    var editUrl = rootUrl + "GlobalSetting/Edit";

    function formatMaxTurnover(inputTag) {
        var tagValue = inputTag.val().replace(/,/g, "");
        var maxTurnover = parseInt(tagValue);

        if (maxTurnover.toString() === tagValue) {
            inputTag.val(numeral(maxTurnover).format(CMSConstants.NumberFormat.IntNumber));
        }
    }

    function escapeEditMaxTurnover(inputTag) {
        inputTag.closest(".customer-row").removeClass("current-editing-row");
        inputTag.prev().removeClass("display-none");
        inputTag.remove();
    }

    function editHandler(custId, maxTurnover, isGroupHeadline) {
        CMS.disableInput();
        CMS.callAjax(
            this,
            editUrl,
            { CustId: custId, MaxTurnover: maxTurnover, IsGroupHeadline: isGroupHeadline, SportType: sportType },
            function (data) {
                var result = $.parseJSON(data);

                if (result.Code !== 0) {
                    CMS.showInlineErrorMsg(result.Message, resultMessageDiv);
                }
                else {
                    CMS.showInlineSuccessMsg(
                        result.Message,
                        resultMessageDiv,
                        true,
                        reloadGridFunction);
                }
            });
    }

    this.initialize = function () {
        table.on("click", ".edit-maxturnover", function () {
            var $this = $(this);
            var text = $this.text();
            var inputHtml = [
                '<input type="text" class="clearable x-icon textbox-maxturnover" maxlength="13" value="',
                text.substring(0, text.indexOf(".")),
                '" data-custid="',
                $this.data("custid"),
                '" data-username="',
                $this.data("username"),
                '" data-isgroupheadline="',
                $this.data("isgroupheadline"),
                '" />'
            ].join("");

            $this.addClass("display-none");
            $this.closest(".customer-row").addClass("current-editing-row");
            $this.closest(".edit-maxturnover-container").append(inputHtml);

            var $input = $this.next();
            $input.off()
                .on("keypress", CMS.forceNumberInput)
                .on("keyup", function (e) {
                    var $this = $(this);
                    var code = e.keyCode || e.which;

                    if (code === keyboardManager.Enums.KeyCode.Escape) {
                        escapeEditMaxTurnover($this);
                    }
                    else if (code === keyboardManager.Enums.KeyCode.Enter) {
                        var maxTurnoverValue = $.trim($this.val());

                        if (maxTurnoverValue.length !== 0 && CMS.isNumeric(maxTurnoverValue)) {
                            var maxTurnover = parseInt(maxTurnoverValue.replace(/,/g, ""));

                            if (0 <= maxTurnover && maxTurnover <= maxIntValue) {
                                var custId = $this.data("custid");
                                var isGroupHeadline = $this.data("isgroupheadline");

                                CMS.showConfirmMessage(
                                    viewResources.GlobalSetting_EditMaxTurnover_Confirm.format($this.data("username")),
                                    function () {
                                        editHandler(custId, maxTurnover, isGroupHeadline);
                                    });
                            }
                        }
                    }
                    else {
                        formatMaxTurnover($this);
                    }
                })
                .one("blur", function () {
                    escapeEditMaxTurnover($(this));
                });

            $input.clearfield().focus();
        });
    };
}
