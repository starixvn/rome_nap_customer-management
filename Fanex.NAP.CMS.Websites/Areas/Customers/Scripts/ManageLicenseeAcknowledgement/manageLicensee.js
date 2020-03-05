"use strict";
(function ($, resources) {
    var elements = {
        msgContainer: $("#div-successInlineMessage")
    };

    function initGridOptions(bodyRows) {
        var columns = [
            { HeaderText: resources.licensee, Width: 200, Name: "colLicensee", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: resources.active, Width: 100, Name: "colActive", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.acknowledgementStatus, Width: 200, Name: "colAcknowledgementStatus", HeaderAlign: "center", CellAlign: "center" },
        ];

        return {
            showTitle: false,
            fullWidth: true,
            fullHeight: true,
            showDistinctRow: false,
            showHighlightRow: true,
            headerColumnHeight: 25,
            gridExpandHeight: 130,
            columns: columns,
            enableHiddenColumns: false,
            bodyRows: bodyRows
        };
    }

    function renderLicenseeAcknowledgement(licensees) {
        var bodyRows = [];
        var cell, licensee;
        var dataLength = licensees ? licensees.length : 0;
        for (var i = 0; i < dataLength; i++) {
            licensee = licensees[i];
            cell = [
                { Name: "colLicensee", Value: licensee.LicenseeName },
                { Name: "colActive", Value: licensee.IsActive ? "Yes" : "No" },
                { Name: "colAcknowledgementStatus", Value: renderAcknowledgementStatus(licensee.LicenseeId, licensee.IsAcknowledgement) }
            ];

            bodyRows.push({ Columns: cell });
        }

        return bodyRows;
    }

    function renderAcknowledgementStatus(licenseeId, isAcknowledgement) {
        return [
            '<input type="checkbox" class="acknowledgement-status" data-licenseeid="',
            licenseeId,
            '"',
            isAcknowledgement ? " checked" : "",
            '/>'
        ].join("");
    }

    function bindEventsAfterRenderLicenseeAcknowledgement() {
        $(".acknowledgement-status").click(function (event) {
            event.preventDefault();

            var $this = $(this);
            var requestParams = {
                licenseeId: $this.data("licenseeid"),
                isAcknowledgement: this.checked
            };

            updateAcknowledgementStatus($this, requestParams);
        });
    }

    function updateAcknowledgementStatus(checkbox, requestParams) {
        CMS.disableInput();
        CMS.hideInlineMsg(elements.msgContainer);

        CMS.callAjax(
            this,
            CMS.resolveRootUrl("Customers/ManageLicenseeAcknowledgement/Update"),
            requestParams,
            function (data) {
                var dataJSON = $.parseJSON(data);
                var code = dataJSON.Code,
                    message = dataJSON.Message;

                if (code === 0) {
                    checkbox.prop("checked", requestParams.isAcknowledgement);
                    CMS.showInlineSuccessMsg(message, elements.msgContainer);
                } else if (code === 101) {
                    CMS.showConfirmMessage(message, function () {
                        updateAcknowledgementStatus(checkbox, {
                            licenseeId: requestParams.licenseeId,
                            isAcknowledgement: requestParams.isAcknowledgement,
                            confirmed: true
                        });
                    });
                } else {
                    CMS.showInlineErrorMsg(message, elements.msgContainer);
                }
            });
    }

    $(function () {
        var bodyRows = renderLicenseeAcknowledgement(licensees);
        var gridOptions = initGridOptions(bodyRows);
        $("#freegrid").removeClass("display-none").fanexGrid(gridOptions);

        bindEventsAfterRenderLicenseeAcknowledgement();
    });
})(jQuery, resources);
