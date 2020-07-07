"use strict";
function EditMaxTurnover(table, resultMessageDiv, reloadGridFunction) {
    function formatMaxTurnover(inputTag) {
        var tagValue = inputTag.val().replace(/,/g, "");
        var maxTurnover = parseInt(tagValue);

        if (maxTurnover.toString() === tagValue) {
            inputTag.val(numeral(maxTurnover).format(CMSConstants.NumberFormat.IntNumber));
        }
    }

    function escapeEditMaxTurnover(inputTag) {
        var custId = inputTag.data("custid");

        inputTag.closest("#tr" + custId).removeClass("current-editing-row");
        inputTag.prev().removeClass("display-none");
        inputTag.remove();
    }

    function editHandler(custId, matchId, maxTurnover, isGroupHeadline, sportType, profileId) {
        CMS.disableInput();
        CMS.callAjax(
            this,
            CMS.resolveRootUrl("TurnoverControl/TurnoverMonitor/UpdateMaxTurnover"),
            { CustId: custId, MatchId: matchId, MaxTurnover: maxTurnover, IsGroupHeadline: isGroupHeadline, SportType: sportType, ProfileId: profileId },
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
        table.on("click", ".max-turnover-container", function () {
            var $this = $(this);
            var custId = $this.data("custid");
            var text = $this.find("#link" + custId).text();
            var inputHtml = [
                '<input type="text" class="clearable x-icon textbox-maxturnover" maxlength="16" value="',
                text,
                '" data-custid="',
                custId,
                '" data-username="',
                $this.data("username"),
                '" data-matchid="',
                $this.data("matchid"),
                '" data-isgroupheadline="',
                $this.data("isgroupheadline"),
                '" data-sporttype="',
                $this.data("sporttype"),
                '" data-profileid="',
                $this.data("profileid"),
                '" />'
            ].join("");

            $this.addClass("display-none");
            $this.closest("#tr" + custId).addClass("current-editing-row");
            $this.closest("#div" + custId).append(inputHtml);

            var $input = $this.next();
            $input
                .off()
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
                                var matchId = $this.data("matchid");
                                var isGroupHeadline = $this.data("isgroupheadline");
                                var sportType = $this.data("sporttype");
                                var profileId = $this.data("profileid");

                                CMS.showConfirmMessage(
                                    viewResources.GlobalSetting_EditMaxTurnover_Confirm.format($this.data("username")),
                                    function () {
                                        editHandler(custId, matchId, maxTurnover, isGroupHeadline, sportType, profileId);
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