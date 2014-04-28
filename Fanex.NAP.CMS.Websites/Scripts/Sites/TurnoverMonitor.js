/* File Created: April 16, 2014 */
/* global Namespace, Fanex, CMS, labels, datepickerSetting */

'use strict';

$(document).ready(function () {
    var report = new Fanex.CMS.TurnoverMonitor();

    report.InitPage();
});

Namespace.Register("Fanex.CMS.TurnoverMonitor");
Fanex.CMS.TurnoverMonitor = function () {
    var $this = this;
    var urls = {
        GetSportsByEventDate: CMS.resolveRootUrl('TurnoverMonitor/GetSportsByEventDate'),
        GetLeagueOfSport: CMS.resolveRootUrl('TurnoverMonitor/GetLeaguesOfSport'),
        GetMatchesOfLeague: CMS.resolveRootUrl('TurnoverMonitor/GetMatchesOfLeague'),
        GetTurnoverReport: CMS.resolveRootUrl('TurnoverMonitor/GetTurnoverReport')
    };
    var domElements = {
        divDatepickerPlugin: null,
        ddlSelectTop: null,
        ddlSport: null,
        ddlLeague: null,
        ddlMatch: null,
        txtSMAName: null,
        txtPercentageUsed: null,
        btnSearch: null,
        divTurnoverMonitor: null
    };
    var parameters = {
        FromDate: null,
        ToDate: null,
        SelectTop: null,
        SportID: null,
        LeagueID: null,
        MatchID: null,
        SMAName: null,
        Used: null
    };

    // Cache DOM elements
    var cacheDOM = function () {
        domElements.divDatepickerPlugin = $('#divDatepickerPlugin');
        domElements.ddlSelectTop = $('#ddlSelectTop');
        domElements.ddlSport = $('#ddlSport');
        domElements.ddlLeague = $('#ddlLeague');
        domElements.ddlMatch = $('#ddlMatch');
        domElements.txtSMAName = $('#txtSMAName');
        domElements.txtPercentageUsed = $('#txtPercentageUsed');
        domElements.btnSearch = $('#btnSearch');
        domElements.divTurnoverMonitor = $('#divTurnoverMonitor');
    };

    // Initialize fields and handle events.
    var initializeFields = function () {
        domElements.divDatepickerPlugin.nexDatePicker({
            nexDatePickerType: 'daterange',
            firstDatePicker: {
                Id: 'dpkFromDate',
                Label: labels.FromDate,
                InitDate: datepickerSetting.Today,
                LabelCss: 'lbl-field col_3',
                TextboxCss: 'edit-field col_8'
            },
            secondDatePicker: {
                Id: 'dpkToDate',
                Label: labels.ToDate,
                InitDate: datepickerSetting.Today,
                LabelCss: 'lbl-field col_2',
                TextboxCss: 'edit-field col_12'
            },

            datePickerFormat: {
                dateFormat: CMS.formats.DateFormat,
                changeMonth: true,
                changeYear: true,
                showOtherMonths: true,
                selectOtherMonths: true,
                minDate: new Date(datepickerSetting.MoveBetsDay)
            }
        });

        domElements.txtPercentageUsed.numberField(0, false, null);

        domElements.ddlSport.on('change', function () {
            //@@TODO: Update leagues.
        });

        domElements.ddlLeague.on('change', function () {
            //@@TODO: Update matches.
        });

        domElements.btnSearch.on('click', function () {
            var dateRange = domElements.divDatepickerPlugin.nexDatePicker('getDateRange');

            parameters.FromDate = dateRange.FromDate;
            parameters.ToDate = dateRange.ToDate;
            parameters.SelectTop = domElements.ddlSelectTop.val();
            parameters.SportID = domElements.ddlSport.val();
            parameters.LeagueID = domElements.ddlLeague.val();
            parameters.MatchID = domElements.ddlMatch.val();
            parameters.SMAName = domElements.txtSMAName.val();
            parameters.Used = domElements.txtPercentageUsed.val().replace(/,/g, '');

            getTurnoverReport();
        });
    };

    // Render grid.
    var renderGrid = function (data) {
        var progressBarFormat = '<div class="percentage-used" data-value="{0}"><div class="percentage-label">{0}</div></div>';
        var gridOptions = {
            columns: [{ HeaderText: labels.SMAName, Width: 250, Name: 'ColSMAName', HeaderAlign: 'center', CellAlign: 'left' },
                      { HeaderText: labels.Level, Width: 70, Name: 'ColLevel', HeaderAlign: 'center', CellAlign: 'center' },
                      { HeaderText: labels.Currency, Width: 100, Name: 'ColCurrency', HeaderAlign: 'center', CellAlign: 'center' },
                      { HeaderText: labels.GivenCredit, Width: 150, Name: 'ColGivenCredit', HeaderAlign: 'center', CellAlign: 'right',
                          FormatNumber: CMS.formats.DecimalFormat
                      },
                      { HeaderText: labels.MaxTORate, Width: 120, Name: 'ColMaxTORate', HeaderAlign: 'center', CellAlign: 'right',
                          FormatNumber: CMS.formats.NumberFormat
                      },
                      { HeaderText: labels.MaxTOAmount, Width: 150, Name: 'ColMaxTOAmount', HeaderAlign: 'center', CellAlign: 'right',
                          FormatNumber: CMS.formats.DecimalFormat
                      },
                      { HeaderText: labels.MemberTO, Width: 150, Name: 'ColMemberTO', HeaderAlign: 'center', CellAlign: 'right',
                          FormatNumber: CMS.formats.DecimalFormat
                      },
                      { HeaderText: labels.PercentageUsed, Width: 150, Name: 'ColPercentageUsed', HeaderAlign: 'center', CellAlign: 'center'}],
            gridExpandHeight: 150,
            showDistinctRow: false
        };
        var bodyData = [];
        var row;

        if (data && data.length > 0) {
            $.each(data, function (index, item) {
                row = {
                    Columns: [{ Name: 'ColSMAName', Value: item.Username },
                              { Name: 'ColLevel', Value: item.Level },
                              { Name: 'ColCurrency', Value: item.Currency },
                              { Name: 'ColGivenCredit', Value: item.GivenCredit },
                              { Name: 'ColMaxTORate', Value: item.MaxTORate },
                              { Name: 'ColMaxTOAmount', Value: item.MaxTOAmount },
                              { Name: 'ColMemberTO', Value: item.MemberTO },
                              { Name: 'ColPercentageUsed', Value: progressBarFormat.format(item.PercentageUsed)}]
                };
                bodyData.push(row);
            });
        }

        gridOptions.bodyRows = bodyData;
        domElements.divTurnoverMonitor.fanexGrid(gridOptions);
        handleGridEvents();
    };

    var handleGridEvents = function () {
        domElements.divTurnoverMonitor.find('div.percentage-used').each(function () {
            var $obj = $(this);
            var percentage = $obj.attr('data-value');

            $obj.progressbar({
                value: 70
            });
        });
    };

    // Get SMA turnover report.
    var getTurnoverReport = function () {
        CMS.disableInput();
        $.ajax({
            type: 'POST',
            url: urls.GetTurnoverReport,
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            //data: JSON.stringify(parameters),
            success: function (result) {
                renderGrid(result);
            },
            error: function (xhr) {
                CMS.handleAjaxError(xhr);
            },
            complete: function () {
                CMS.enableInput();
            }
        });
    };

    $this.InitPage = function () {
        cacheDOM();
        initializeFields();
    };
};