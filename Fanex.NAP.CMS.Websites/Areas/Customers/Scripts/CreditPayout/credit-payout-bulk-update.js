"use strict";

const creditPayoutBulkUpdate = (function ($) {

    let inlineMessage = null,
        bulkUpdateForm = null,
        usernameList = null,
        custIDList = null,
        ddlEditStatus = null,
        spanEditStatus= null,
        reloadGridFunction = null;

    function init(reloadGrid) {
        bulkUpdateForm = $("#bulkUpdateForm");
        $("#divMainPopupContent").remove();
        reloadGridFunction = reloadGrid;
    }

    function openModal(params, payout) {
        CMS.modal(
            resources.updatePayouts,
            bulkUpdateForm.clone(),
            {
                resizable: false,
                position: {
                    my: "center top",
                    at: "center top",
                    of: "#headline"
                }
            });
        cacheElements(params, payout);
        bindEvents();
    }

    function getStatusValue(status) {
        if (status === "Open") {
            return "0";
        }
        if (status === "Verified") {
            return "1";
        }
        if (status === "Completed") {
            return "2";
        }
        if (status === "Cancelled") {
            return 3;
        }
    }

    function cacheElements(params, payout) {
        usernameList = params.usernameList;
        custIDList = params.custIDList;
        inlineMessage = $("#inlineBulkMessage");
        $("#lblUsernames").text(`${params.usernames.length} selected account(s):`);
        $("#txtUsernames").text(params.usernames.join(", "));
        const dpkPaymentDate = $('#dpkPaymentDate');
        Layout.createDatepicker(
            dpkPaymentDate,
            'popup-paymentDate',
            resources.paymentDate,
            '',
            null,
            null);
        if (payout) {
            var status = getStatusValue(payout.Status);
            $("#ddlEditStatus").val(status);
            if (status === '2') {
                $(".row-payment").show();
                $('#popup-paymentDate').val(payout.PaymentDate).removeAttr('readonly');
            } else {
                $(".row-payment").hide();
                $('#popup-paymentDate').val(currrentDate).removeAttr('readonly');
            }
            if (status !== '1') {
                $("#ddlEditStatus option[value='1']").remove();
            }
            $("#txtRemark").val(payout.Remark);
        } else {
            $('#popup-paymentDate').val(currrentDate).removeAttr('readonly');
            $("#ddlEditStatus option[value='1']").remove();
        }
        ddlEditStatus = $("#ddlEditStatus").combobox();
        spanEditStatus = ddlEditStatus.next("#ddlEditStatus");

        ddlEditStatus.off("change")
            .change(function () {
            if ($("#ddlEditStatus").val() === '2') {
                $(".row-payment").show();
            } else {
                $(".row-payment").hide();
            }
        });
    }

    function bindEvents() {
        $("#btnSave").prop("disabled", false)
        .off("click")
        .click(function () {
            clearAllTipsyErrors();
            processUpdateStatus();
        });
        $("#btnCancel").off("click").click(CMS.closePopup);
    }

    function processUpdateStatus() {
        clearAllTipsyErrors();
        var params = {
            UsernameList: usernameList,
            CustIDList: custIDList,
            Status: ddlEditStatus.val(),
            PaymentDate: $("#popup-paymentDate").val(),
            Remark: $("#txtRemark").val(),
            typeID: 1
        };

        if (params.Status === "PleaseSelect") {
            CMS.showTipsyError(spanEditStatus, resources.requiredField, "ne");
            return;
        }

        if (params.Status !== "2") {
            params.PaymentDate = null;
        }

        CMS.callAjax(
            this,
            CMS.resolveRootUrl(`CreditPayout/Update`),
            params,
            function (data) {
                var result = typeof data === "string" ? JSON.parse(data) : data;
                if (Array.isArray(result) && result[0].Code !== 0) {
                    var messsage = [];
                    result.forEach(function (err) {
                        messsage.push(err.Message);
                    });
                    CMS.showInlineErrorMsg(messsage.join("</br>"), inlineMessage);
                } else {
                    CMS.showInlineSuccessMsg("Update Successfully!", inlineMessage, true, function () {
                        CMS.closePopup();
                        reloadGridFunction && reloadGridFunction();
                    }, 1000);
                }
            });
    }

    function clearAllTipsyErrors() {
        $("[original-title]").each(function () {
            CMS.clearTipsyError($(this));
        });
    }

    return {
        init: init,
        openModal: openModal
    };
})(jQuery);