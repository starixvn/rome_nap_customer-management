/* global payoutManagementLabels, filterData, currentDate, initFromDate, CMS, CMSConstants, numeral */
(function () {
    'use strict';
    var gridOptions = null,
        freegrid = $('#freegrid');
    var rootUrl = $('#CMSRoot').val();
    var $formFilter = $('#form-filterTicket'),
        $ticketDetailContainer = $('#ticketDetailContainer'),
        $username = $('#username'),
        $siteslicensees = $('#siteslicensees'),
        $products = $('#products'),
        $statuses = $('#statuses'),
        $fromtoDatePicker = $('#fromtodate-picker');
    var searchUrl = $formFilter.attr('action') + '?',
        formData = {};

    function formatNumber(number) {
        return numeral(number).format(CMSConstants.NumberFormat.TwoDecimalNumber);
    }

    function initGridOptions() {
        var columns = [
            { HeaderText: '#', Width: 40, Name: 'colNumber', HeaderAlign: 'center', CellAlign: 'center' },
            { HeaderText: payoutManagementLabels.Product, Width: 160, Name: 'colProduct', HeaderAlign: 'center', CellAlign: 'left' },
            { HeaderText: payoutManagementLabels.Username, Width: 200, Name: 'colUsername', HeaderAlign: 'center', CellAlign: 'left' },
            { HeaderText: payoutManagementLabels.SiteLicensee, Width: 150, Name: 'colSiteLicensee', HeaderAlign: 'center', CellAlign: 'left' },
            { HeaderText: payoutManagementLabels.Date, Width: 120, Name: 'colDate', HeaderAlign: 'center', CellAlign: 'center' },
            { HeaderText: payoutManagementLabels.RefNo, Width: 140, Name: 'colRefNo', HeaderAlign: 'center', CellAlign: 'right' },
            { HeaderText: payoutManagementLabels.Currency, Width: 100, Name: 'colCurrency', HeaderAlign: 'center', CellAlign: 'center' },
            { HeaderText: payoutManagementLabels.Amount, Width: 120, Name: 'colAmount', HeaderAlign: 'center', CellAlign: 'right' },
            { HeaderText: payoutManagementLabels.Status, Width: 100, Name: 'colStatus', HeaderAlign: 'center', CellAlign: 'left' },
            { HeaderText: payoutManagementLabels.Detail, Width: 50, Name: 'colDetail', HeaderAlign: 'center', CellAlign: 'center' }
        ];

        return {
            showTitle: false,
            fullWidth: true,
            fullHeight: true,
            showDistinctRow: false,
            showHighlightRow: true,
            headerColumnHeight: 25,
            gridExpandHeight: 190,
            columns: columns,
            showPager: false
        };
    }

    function buildBodyRows(results) {
        var bodyRows = [];
        var cell, ticket;
        var resultLength = results ? results.length : 0;
        for (var i = 0; i < resultLength; i++) {
            ticket = results[i];
            cell = [
                { Name: 'colNumber', Value: (i + 1) },
                { Name: 'colProduct', Value: ticket.Product.Name },
                { Name: 'colUsername', Value: ticket.Member.Name },
                { Name: 'colSiteLicensee', Value: ticket.Site },
                { Name: 'colDate', Value: ticket.DateDisplay },
                { Name: 'colRefNo', Value: ticket.RefNo },
                { Name: 'colCurrency', Value: ticket.Currency.Name },
                { Name: 'colAmount', Value: renderAmount(ticket.Id, ticket.RefNo, ticket.Member.Id, ticket.Product.Id, ticket.Amount) },
                { Name: 'colStatus', Value: ticket.Status.Name },
                { Name: 'colDetail', Value: renderDetail(ticket.Id, ticket.Status.Id, ticket.HasNotes, ticket.Product.Id) }
            ];

            bodyRows.push({ Columns: cell });
        }

        return bodyRows;
    }

    function renderAmount(id, refno, memberId, productId, amount) {
        var html = [
            '<span class="link amount" data-transid="',
            id,
            '" data-refno="',
            refno,
            '" data-member="',
            memberId,
            '" data-product="',
            productId,
            '">',
            formatNumber(amount),
            '</span>'
        ];

        return html.join('');
    }

    function renderDetail(id, statusId, hasNotes, productId) {
        var html = [
            '<a class="',
            hasNotes ? 'ico-remark' : 'ico-noremark',
            '" href="',
            rootUrl,
            'PayoutManagement/PayoutList/PayoutDetail?id=',
            id,
            '&status=',
            statusId,
            '&productId=',
            productId,
            '"></a>'
        ];

        return html.join('');
    }

    function getPayoutList() {
        var successFunc = function (results) {
            var resultsJson = $.parseJSON(results);
            gridOptions.bodyRows = buildBodyRows(resultsJson);
            freegrid.removeClass('display-none').fanexGrid(gridOptions);
            bindEvents();
        };

        CMS.getJson(searchUrl + $.param(formData), successFunc);
    }

    function bindEvents() {
        $('.amount').unbind('click').bind('click', function () {
            var $this = $(this);
            var refno = $this.data('refno'),
                product = parseInt($this.data('product')),
                id = $this.data('transid'),
                member = $this.data('member');

            var successFunc = function (data) {
                $ticketDetailContainer.html(data);
                CMS.modal(payoutManagementLabels.ViewTicketDetailTitle + ' ' + refno, $ticketDetailContainer, { resizable: false });
            };

            var action;
            if (product === 204) {
                action = 'ColossusBets?id=' + id + '&member=' + member;
            }
            else if (product === 205) {
                action = 'Microgaming?id=' + id + '&member=' + member;
            }
            else if (product === 208 || product === 209 || product === 210) {
                action = 'RNGCasinoJackpot?refno=' + refno;
            }
            else {
                return;
            }

            var url = rootUrl + 'PayoutManagement/BetTicket/' + action;
            CMS.getHtml(url, function (data) { successFunc(data); });
        });
    }

    function setFormData() {
        var fromtoDate = $fromtoDatePicker.nexDatePicker('getDateRange');
        formData = {
            username: $username.val().trim(),
            site: $siteslicensees.fanexMultiSelect('getItems', true).join(','),
            product: $products.fanexMultiSelect('getItems', true).join(','),
            status: $statuses.fanexMultiSelect('getItems', true).join(','),
            from: fromtoDate.FromDate,
            to: fromtoDate.ToDate
        };
    }

    function getGroupMultiselectSource(sources) {
        var dataList = [];

        var group;
        for (var i = 0, max = sources.length; i < max; i++) {
            group = sources[i];
            dataList.push({
                id: group.GroupName,
                value: group.GroupName,
                text: group.GroupName,
                checked: true,
                enable: true,
                items: getMultiselectSource(group.ChildItems)
            });
        }

        return dataList;
    }

    function getMultiselectSource(sources) {
        var dataList = [];

        var data;
        for (var i = 0, max = sources.length; i < max; i++) {
            data = sources[i];
            dataList.push({
                id: data.Id,
                value: data.Id,
                text: data.Name,
                checked: true,
                enable: true
            });
        }

        return dataList;
    }

    function initMultiSelects() {
        $siteslicensees.fanexMultiSelect({
            minWidth: 180,
            checkAllText: 'All',
            isCheckAll: true,
            listData: getGroupMultiselectSource(filterData.SitesLicensees)
        });

        $products.fanexMultiSelect({
            minWidth: 180,
            checkAllText: 'All',
            isCheckAll: true,
            listData: getMultiselectSource(filterData.Products)
        });

        $statuses.fanexMultiSelect({
            minWidth: 180,
            checkAllText: 'All',
            isCheckAll: true,
            listData: getMultiselectSource(filterData.Statuses)
        });
    }

    function initDateRangeSelect() {
        $fromtoDatePicker.nexDatePicker({
            nexDatePickerType: 'daterange',
            datePickerFormat: {
                dateFormat: CMS.formats.DateFormat,
                changeMonth: true,
                changeYear: true,
                showOtherMonths: true,
                selectOtherMonths: true,
                minDate: '01/01/2012',
                maxDate: null
            },
            firstDatePicker: {
                Id: 'dpk-fromdate',
                Label: payoutManagementLabels.From,
                LabelCss: 'lbl-field col_4',
                TextboxCss: 'edit-field col_8'
            },
            secondDatePicker: {
                Id: 'dpk-todate',
                Label: payoutManagementLabels.To,
                InitDate: currentDate,
                CurrentDate: currentDate,
                LabelCss: 'lbl-field col_3',
                TextboxCss: 'edit-field col_8'
            }
        });

        $('#dpk-fromdate').datepicker('setDate', initFromDate);
        $formFilter.trigger('submit');
    }

    function initialize() {
        gridOptions = initGridOptions();

        $formFilter.submit(function (e) {
            e.preventDefault();

            setFormData();
            getPayoutList();
        });

        initMultiSelects();
        setTimeout(initDateRangeSelect, 100);

        $('#divModals').empty();
        $('#username').focus();
    }

    initialize();
})();
