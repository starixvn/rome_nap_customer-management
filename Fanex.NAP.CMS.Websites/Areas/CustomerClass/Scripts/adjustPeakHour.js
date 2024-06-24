"use strict";

const adjustPeakHour = (function ($) {
    let addNewOrEditForm = null,
        weekDate = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'],
        cachedPeakHour = [];
    function init(reloadGridFunc) {
        addNewOrEditForm = $("#adjustPeakHour");
        // Empty the #divPopupContent since it's children was cached.
        // It also helps to prevent the 'for' attributes do not work correctly in the popup.
        $("#adjustPeakHour").remove();
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
                close: null
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
            $(`#peak-hour-${peak.WeekDay}`).append(`<div class="peak-hour-item">${peak.StartRange.substring(0, peak.StartRange.length - 3)} - ${peak.EndRange.substring(0, peak.EndRange.length - 3)}<div class="delete-range delete-btn" data-id="${peak.RangeID}"><span class="ico-delete-borderless"></span></div></div>`);
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
        $(".delete-range").off("click").on('click', handleDeletePeakHour);
        $(".add-peak").off("click").on('click', openAddPeakPopUp);
    }

    function handleDeletePeakHour() {
        var rangeID = $(this).data('id');
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

    function openAddPeakPopUp() {
        var weekDay = +$(this).data("weekday");
        let html = [
            '<div class="form" id="addPeakByWeekDayForm">',
        ];
        
        html.push(`<div class="row"><div class="label-add"><label for="txtFromTimeAdd">From</label></div><div class="add-time"><input type="text" id="txtFromTimeAdd" maxlength="5" class="input-time" autocomplete="off"/></div></div>`);
        html.push(`<div class="row"><div class="label-add"><label for="txtToTimeAdd">To</label></div><div class="add-time"><input type="text" id="txtToTimeAdd" maxlength="5" class="input-time" autocomplete="off"/></div></div>`);
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

        $("#btnAddPeakHour").off("click").on("click", function () { processAdd(weekDay, $addDialog); });
    }

    function processAdd(weekDay, $addDialog) {
        var startRange = $.trim($('#txtFromTimeAdd').val());
        var endRange = $.trim($('#txtToTimeAdd').val());
        var timeRegex = /^([0-9][0-9]):[0-9][0-9]$/;
        if (!timeRegex.test(startRange)) {
            CMS.showTipsyError($('#txtFromTimeAdd'), 'Invalid time format');
            return;
        }

        if (!timeRegex.test(endRange)) {
            CMS.showTipsyError($('#txtToTimeAdd'), 'Invalid time format');
            return;
        }

        startRange = startRange + ":00";
        endRange = endRange + ":00";

        var request = {
            weekDay,
            startRange,
            endRange
        };

        CMS.disableInput();
        CMS.callAjax(
            this,
            CMS.resolveRootUrl(`PeakHour/AddPeakHour`),
            request,
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

    return {
        init: init,
        openModal: openModal
    };
})(jQuery);

