"use strict";

const adjustPeakHour = (function ($) {
    let addNewOrEditForm = null,
        weekDate = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'],
        cachedPeakHour = [],
        search = null;
    function init(reloadGridFunc) {
        addNewOrEditForm = $("#adjustPeakHour");
        // Empty the #divPopupContent since it's children was cached.
        // It also helps to prevent the 'for' attributes do not work correctly in the popup.
        $("#adjustPeakHour").remove();
        search = reloadGridFunc;
    }

    function openModal() {
        CMS.modal(
            resources.adjustPeakHourRange,
            addNewOrEditForm.clone(),
            {
                resizable: false,
                position: {
                    my: "center top",
                    at: "center top",
                    of: "#headline"
                },
                close: function () {
                    search();
                }
            });
        getPeakHour();
    }

    function getPeakHour() {
        cachedPeakHour = [];
        CMS.disableInput();
        CMS.callAjax(
            this,
            CMS.resolveRootUrl(`PeakHour/GetPeakHour`),
            null,
            function (result) {
                cachedPeakHour = typeof result === "string" ? JSON.parse(result) : result;
                if (cachedPeakHour.Message) {
                    CMS.showErrorMessage(cachedPeakHour.Message);
                    cachedPeakHour = [];
                    renderPeakHourGrid(new Array());
                } else {
                    renderPeakHourGrid(cachedPeakHour);
                }
            },
            null,
            function () {
                CMS.enableInput();
            });
    }

    function renderPeakHourGrid(peakHours) {
        $(".peak-hour-container").html('');
        peakHours.forEach(function (peak) {
            var fromTime = peak.StartRange.substring(0, peak.StartRange.length - 3);
            var toTime = peak.EndRange.substring(0, peak.EndRange.length - 3);
            $(`#peak-hour-${peak.WeekDay}`).append(`<div class="peak-hour-item" data-id="${peak.RangeID}" data-weekday="${peak.WeekDay}" data-from="${fromTime}" data-to="${toTime}" >${fromTime} - ${toTime}<div class="delete-range delete-btn" data-id="${peak.RangeID}"><span class="ico-delete-borderless"></span></div></div>`);
            var startHour = +peak.StartRange.substring(0, 2);
            var endHour = +peak.EndRange.substring(0, 2);
            for (var i = startHour; i <= endHour; i++) {
                if (!$(`#hour-${i}-${peak.WeekDay}`).hasClass("hour-selected")) {
                    $(`#hour-${i}-${peak.WeekDay}`).addClass("hour-selected");
                }
            }
        });

        bindingEvent();
    }

    function bindingEvent() {
        $(".delete-range").off("click").on('click', showDeleteConfirmPopup);
        $(".add-peak").off("click").on('click', openAddEditPeakPopUp);
        $(".peak-hour-item").off("click").on('click', openAddEditPeakPopUp);
    }


    function showDeleteConfirmPopup() {
        var rangeID = $(this).data('id');
        CMS.showConfirmMessage(`Are you sure you want to <span class="color-red">delete</span> this range?`, function () { handleDeletePeakHour(rangeID); });
    }

    function handleDeletePeakHour(rangeID) {
        CMS.disableInput();
        CMS.callAjax(
            this,
            CMS.resolveRootUrl(`PeakHour/DeletePeakHour?rangeId=${rangeID}`),
            null,
            function (result) {
                const resultJson = typeof result === "string" ? JSON.parse(result) : result;
                if (resultJson.IsSuccess) {
                    CMS.showSuccessMessage(resultJson.Message);
                    openModal();
                } else {
                    CMS.showErrorMessage(resultJson.Message);
                }
            });
    }

    function openAddEditPeakPopUp() {
        var weekDay = +$(this).data("weekday");
        var rangeId = $(this).data("id");
        var fromTime = $(this).data("from") || "&nbsp;";
        var toTime = $(this).data("to") || "&nbsp;";
        let html = [
            '<div class="form" id="addPeakByWeekDayForm">'
        ];
        
        html.push(`<div class="row"><div class="label-add"><label for="txtFromTimeAdd">From</label></div><div class="add-time"><input type="text" value=${fromTime} id="txtFromTimeAdd" maxlength="5" class="input-time" autocomplete="off"/></div></div>`);
        html.push(`<div class="row"><div class="label-add"><label for="txtToTimeAdd">To</label></div><div class="add-time"><input type="text" id="txtToTimeAdd" value=${toTime} maxlength="5" class="input-time" autocomplete="off"/></div></div>`);
        html.push(`<div><button type="button" class="submit" id="btnAddPeakHour">OK</button><div>`);
        html.push('</div>');

        var $addDialog = CMS.modal(
            weekDate[weekDay -1],
            html.join(""),
            {
                resizable: false,
                position: {
                    my: "center top",
                    at: "center top",
                    of: "#headline"
                },
                close: function () {
                    $(this).remove();
                    openModal();
                }

            });

        var timePickerOptions = {
            timeFormat: 'HH:mm',
            interval: 1,
            dynamic: false,
            dropdown: true,
            scrollbar: true,
            change: function () {
                CMS.clearTipsyError($(this));
            }
        };

        $('#txtFromTimeAdd').timepicker(timePickerOptions);
        $('#txtToTimeAdd').timepicker(timePickerOptions);

        $("#btnAddPeakHour").off("click").on("click", function () { processAddEdit(weekDay, rangeId, $addDialog); });
    }

    function processAddEdit(weekDay, rangeId, $addDialog) {
        var startRange = $.trim($('#txtFromTimeAdd').val());
        var endRange = $.trim($('#txtToTimeAdd').val());
        var timeRegex = /^([0-9][0-9]):[0-9][0-9]$/;
        if (!timeRegex.test(startRange)) {
            CMS.showErrorMessage('Invalid From Time format');
            return;
        }

        if (!timeRegex.test(endRange)) {
            CMS.showErrorMessage('Invalid To Time format');
            return;
        }

        startRange = startRange + ":00";
        endRange = endRange + ":59";

        var startRangeTimes = startRange.split(":");
        var endRangeTimes = endRange.split(":");

        var startDate = (new Date()).setHours(startRangeTimes[0], startRangeTimes[1], startRangeTimes[2]);
        var endDate = (new Date()).setHours(endRangeTimes[0], endRangeTimes[1], endRangeTimes[2]);
        if (endDate <= startDate) {
            CMS.showErrorMessage('The To Time must come after the From Time');
            return;
        }

        var request = {
            rangeId,
            weekDay,
            startRange,
            endRange
        };

        var url = rangeId ? CMS.resolveRootUrl(`PeakHour/EditPeakHour`) : CMS.resolveRootUrl(`PeakHour/AddPeakHour`);

        CMS.disableInput();
        CMS.callAjax(
            this,
            url,
            request,
            function (result) {
                const resultJson = typeof result === "string" ? JSON.parse(result) : result;
                if (resultJson.IsSuccess) {
                    CMS.showSuccessMessage(resultJson.Message);
                    $addDialog.dialog("close");
                } else {
                    CMS.showErrorMessage(resultJson.Message);
                }
            });
    }

    return {
        init: init,
        openModal: openModal
    };
})(jQuery);

