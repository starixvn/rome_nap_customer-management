"use strict";
function EditGlobalSetting(viewResources, table, resultMessageDiv, reloadGridFunction, sportType) {
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

    function editHandler(custId, profileId, maxTurnover, isGroupHeadline) {
        CMS.disableInput();
        CMS.callAjax(
            this,
            CMS.resolveRootUrl("TurnoverControl/GlobalSetting/Edit"),
            { custId: custId, profileId: profileId, maxTurnover: maxTurnover, isGroupHeadline: isGroupHeadline, sportType: sportType },
            function (data) {
                var jsonData = $.parseJSON(data);

                if (jsonData.Code === 0) {
                    CMS.showInlineSuccessMsg(jsonData.Message, resultMessageDiv, true, reloadGridFunction);
                } else {
                    CMS.showInlineErrorMsg(jsonData.Message, resultMessageDiv);
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
                '" data-profileid="',
                $this.data("profileid"),
                '" />'
            ].join("");

            $this.addClass("display-none");
            $this.closest(".customer-row").addClass("current-editing-row");
            $this.closest(".edit-maxturnover-container").append(inputHtml);

            var $input = $this.next();
            $input.off()
                .on("keypress", CMS.forceNumberInput)
                .on("keyup", function (event) {
                    var $this = $(this);
                    var code = event.keyCode || event.which;

                    if (code === keyboardManager.Enums.KeyCode.Escape) {
                        escapeEditMaxTurnover($this);
                    } else if (code === keyboardManager.Enums.KeyCode.Enter) {
                        var maxTurnoverValue = $.trim($this.val());

                        if (maxTurnoverValue.length !== 0 && CMS.isNumeric(maxTurnoverValue)) {
                            var maxTurnover = parseInt(maxTurnoverValue.replace(/,/g, ""));

                            if (0 <= maxTurnover && maxTurnover <= maxIntValue) {
                                var custId = $this.data("custid");
                                var profileId = $this.data("profileid");
                                var isGroupHeadline = $this.data("isgroupheadline");

                                CMS.showConfirmMessage(
                                    viewResources.GlobalSetting_EditMaxTurnover_Confirm.format($this.data("username")),
                                    function () { editHandler(custId, profileId, maxTurnover, isGroupHeadline); });
                            }
                        }
                    } else {
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
