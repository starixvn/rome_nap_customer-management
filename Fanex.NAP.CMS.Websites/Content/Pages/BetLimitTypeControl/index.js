$(document).ready(function () {
    var betLimitTypeControl = new Fanex.CMS.BetLimitTypeControl();
    betLimitTypeControl.InitPage();
});

Namespace.Register("Fanex.CMS.BetLimitTypeControl");
Fanex.CMS.BetLimitTypeControl = function () {
    var filterData;
    var $addDialog;
    var isHost = false;
    var liveIndicators = {};
    var queryParams;
    var queryData;

    function generateOptionsHtml(data, valueKey, textKey, customText) {
        var html = [];
        $.each(data, function (index, row) {
            var text = customText ? customText(row) : row[textKey];
            html.push('<option value="' + row[valueKey] + '">' + text + '</option>');
        });

        return html.join('');
    }

    function filterAndDistinctSportTypes(productIds) {
        var filterSportTypes = [];
        var filterSportTypeIds = [];
        for (var sportType of filterData.SportTypes) {
            if (filterSportTypeIds.indexOf(sportType.SportTypeId) === -1 && productIds.indexOf(sportType.ProductId) !== -1) {
                filterSportTypeIds.push(sportType.SportTypeId);
                filterSportTypes.push(sportType);
            }
        }

        return filterSportTypes;
    }

    function filterAndDistinctBetTypes(productIds, sportTypeIds) {
        var filterBetTypeIds = [];
        var filterBetTypes = [];
        for (var betType of filterData.BetTypes) {
            if (filterBetTypeIds.indexOf(betType.BetTypeId) === -1 && productIds.indexOf(betType.ProductId) !== -1 && sportTypeIds.indexOf(betType.SportTypeId) !== -1) {
                filterBetTypeIds.push(betType.BetTypeId);
                filterBetTypes.push(betType);
            }
        }

        return filterBetTypes;
    }

    function onProductChange() {
        var productId = parseInt($('#selectProduct').val());
        var sportTypes = productId === 0 ? filterData.SportTypes : filterAndDistinctSportTypes([productId]);
        sportTypes = [{ SportTypeId: 0, SportTypeName: 'All' }].concat(sportTypes);
        $('#selectSportType')
            .html(generateOptionsHtml(sportTypes, 'SportTypeId', 'SportTypeName'))
            .combobox('refresh')
            .trigger('change');
    }

    function onSportTypeChange() {
        var productId = parseInt($('#selectProduct').val());
        var sportTypeId = parseInt($('#selectSportType').val());
        var betTypes = sportTypeId === 0 ? filterData.BetTypes : filterAndDistinctBetTypes([productId], [sportTypeId]);
        betTypes = [{ BetTypeId: 0, BetTypeName: 'All' }].concat(betTypes);
        $('#selectBetType')
            .html(generateOptionsHtml(betTypes, 'BetTypeId', 'BetTypeName', function (row) {
                var betTypeId = row.BetTypeId;
                var betTypeName = row.BetTypeName;
                if (betTypeId === 0) {
                    return betTypeName;
                } else {
                    return betTypeName + ' [' + betTypeId + ']';
                }
            }))
            .combobox('refresh');
    }

    function renderFilter() {
        var products = [{ ProductId: 0, ProductName: 'All' }].concat(filterData.Products);
        $('#selectSportType')
            .on('change', onSportTypeChange);
        $('#selectProduct')
            .html(generateOptionsHtml(products, 'ProductId', 'ProductName'))
            .combobox('refresh')
            .on('change', onProductChange)
            .trigger('change');
    }

    function loadFilter() {
        CMS.disableInput();
        $.ajax({
            type: 'POST',
            url: CMS.resolveRootUrl('BetLimitTypeControl/QueryFilter'),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (data) {
                filterData = data;
                renderFilter();
                search();
            },
            error: function (xhr) {
                CMS.handleAjaxError(xhr);
            },
            complete: function () {
                CMS.enableInput();
            }
        });
    }

    function getMultiselectSource(list, valueKey, textKey, customText, checkedList) {
        var dataList = [];

        for (var i = 0; i < list.length; i++) {
            var row = list[i];
            var value = row[valueKey];
            var text = customText ? customText(row) : row[textKey];
            dataList.push({
                id: value,
                value: value,
                text: text,
                checked: checkedList ? checkedList.indexOf(value) !== -1 : true,
                enable: true
            });
        }

        return dataList;
    }

    function customCheckAllTextFn() {
        return 'All (' + this.listData.length + ')';
    }

    function showAddOrUpdateDialog(betLimitTypeData) {
        var productCheckedList, sportTypeCheckedList, betTypeCheckedList;
        var isAdd = true;
        if (betLimitTypeData) {
            isAdd = false;
            productCheckedList = $.map(betLimitTypeData.ProductList.split(','), function (x) { return parseInt(x) });
            sportTypeCheckedList = $.map(betLimitTypeData.SportTypeList.split(','), function (x) { return parseInt(x) });
            betTypeCheckedList = $.map(betLimitTypeData.BetTypeList.split(','), function (x) { return parseInt(x) });
        }

        var minWidth = 345;
        var maxHeight = 250;
        function onProductCheckChange() {
            CMS.clearTipsyError($("#selectProductAdd"));
            var selectedProductIds = $.map($('#selectProductAdd').fanexMultiSelect("getItems", true), function (x) {
                return parseInt(x);
            });
            var selectedSportTypes = filterAndDistinctSportTypes(selectedProductIds);

            var sportTypesDataSource = getMultiselectSource(selectedSportTypes, 'SportTypeId', 'SportTypeName', null, sportTypeCheckedList);
            $('#selectSportTypeAdd').fanexMultiSelect({
                minWidth: minWidth,
                maxHeight: maxHeight,
                checkAllText: 'All',
                useCustomCheckAllText: true,
                customCheckAllText: customCheckAllTextFn,
                listData: sportTypesDataSource,
                onCheck: onSportTypeCheckChange
            });
            onSportTypeCheckChange();
        }

        function onSportTypeCheckChange() {
            CMS.clearTipsyError($("#selectSportTypeAdd"));
            var selectedProductIds = $.map($('#selectProductAdd').fanexMultiSelect("getItems", true), function (x) {
                return parseInt(x);
            });
            var selectedSportTypeIds = $.map($('#selectSportTypeAdd').fanexMultiSelect("getItems", true), function (x) {
                return parseInt(x);
            });
            var selectedBetTypes = filterAndDistinctBetTypes(selectedProductIds, selectedSportTypeIds);

            var betTypesDataSource = getMultiselectSource(selectedBetTypes, 'BetTypeId', 'BetTypeName', function (row) {
                var betTypeId = row.BetTypeId;
                var betTypeName = row.BetTypeName;
                return betTypeName + ' [' + betTypeId + ']';
            }, betTypeCheckedList);
            $('#selectBetTypeAdd').fanexMultiSelect({
                minWidth: minWidth,
                maxHeight: maxHeight,
                checkAllText: 'All',
                useCustomCheckAllText: true,
                customCheckAllText: customCheckAllTextFn,
                listData: betTypesDataSource,
                onCheck: onBetTypeCheckChange
            });
        }

        function onBetTypeCheckChange() {
            CMS.clearTipsyError($("#selectBetTypeAdd"));
        }

        var productsDataSource = getMultiselectSource(filterData.Products, 'ProductId', 'ProductName', null, productCheckedList);
        $('#selectProductAdd').fanexMultiSelect({
            minWidth: minWidth,
            maxHeight: maxHeight,
            checkAllText: 'All',
            useCustomCheckAllText: true,
            customCheckAllText: customCheckAllTextFn,
            listData: productsDataSource,
            onCheck: onProductCheckChange
        });
        onProductCheckChange();

        productCheckedList = null;
        sportTypeCheckedList = null;
        betTypeCheckedList = null;

        if (isAdd) {
            $('#selectLiveIndicatorAdd').prop("selectedIndex", 0);
        } else {
            $('#selectLiveIndicatorAdd').val(betLimitTypeData.LiveIndicator);
        }

        $('#txtTypeAdd').val(isAdd ? '' : betLimitTypeData.BetLimitType);
        $('#txtLeagueAdd').val(isAdd ? '' : betLimitTypeData.LeagueList);
        $('#txtFromTimeAdd').val(isAdd ? '' : betLimitTypeData.BeginTime || '');
        $('#txtToTimeAdd').val(isAdd ? '' : betLimitTypeData.EndTime || '');
        $('#chkSpecificTime')
            .change(function () {
                if ($(this).is(':checked')) {
                    $('#txtFromTimeAdd').prop("disabled", false);
                    $('#txtToTimeAdd').prop("disabled", false);
                } else {
                    $('#txtFromTimeAdd').prop("disabled", true).val('');
                    $('#txtToTimeAdd').prop("disabled", true).val('');
                }
            })
            .prop('checked', isAdd ? false : betLimitTypeData.BeginTime !== betLimitTypeData.EndTime)
            .trigger('change');
        $('#chkRestrictVIP').prop('checked', isAdd ? false : betLimitTypeData.RestrictVIP);

        var title = isAdd ? "New Bet Limit Type" : "Update Bet Limit Type";
        CMS.closeMsgBox();
        if (!$addDialog) {
            var timePickerOptions = {
                timeFormat: 'HH:mm',
                interval: 30,
                dynamic: false,
                dropdown: true,
                scrollbar: true,
                change: function () {
                    CMS.clearTipsyError($(this));
                }
            }
            $('#txtFromTimeAdd').timepicker(timePickerOptions);
            $('#txtToTimeAdd').timepicker(timePickerOptions);
            $('#btnCancelAdd').click(CMS.closePopup);
            $('#btnSaveAdd').click(function () {
                addOrUpdate(betLimitTypeData.BetLimitTypeId);
            });
            $('#txtLeagueAdd').blur(reformatAndValidateLeague);
            $addDialog = CMS.modal(title, $('#frmAdd'), { resizable: false });
        } else {
            $addDialog.dialog('open');
            $('#ui-dialog-title-dialog-popup').text(title);
        }
    }

    function reformatAndValidateLeague() {
        var leagueIds = [];
        var invalidLeagueIds = [];
        var duplicatedLeagueIds = [];
        var text = $('#txtLeagueAdd').val();
        var words = text.split(',');
        var validWords = [];
        for (var word of words) {
            var trimmedWord = $.trim(word);
            if (trimmedWord) {
                var leagueId = parseInt(trimmedWord);
                if (leagueId.toString() !== trimmedWord) {
                    invalidLeagueIds.push(trimmedWord);
                } else {
                    if (leagueIds.indexOf(leagueId) !== -1 && duplicatedLeagueIds.indexOf(leagueId) === -1) {
                        duplicatedLeagueIds.push(leagueId);
                    }

                    leagueIds.push(leagueId);
                }

                validWords.push(trimmedWord);
            }
        }

        $('#txtLeagueAdd').val(validWords.join(','));
        return {
            invalidLeagueIds: invalidLeagueIds,
            duplicatedLeagueIds: duplicatedLeagueIds
        };
    }

    function addOrUpdate(betLimitTypeId) {
        var betLimitType = $.trim($("#txtTypeAdd").val());
        if (!betLimitType) {
            CMS.showTipsyError($("#txtTypeAdd"), 'This field is required', "w");
            return;
        }

        var productList = $.map($('#selectProductAdd').fanexMultiSelect("getItems", true), function (x) {
            return parseInt(x);
        });
        if (productList.length === 0) {
            CMS.showTipsyError($("#selectProductAdd"), 'This field is required', "w");
            return;
        }

        var sportTypeList = $.map($('#selectSportTypeAdd').fanexMultiSelect("getItems", true), function (x) {
            return parseInt(x);
        });
        if (sportTypeList.length === 0) {
            CMS.showTipsyError($("#selectSportTypeAdd"), 'This field is required', "w");
            return;
        }

        var betTypeList = $.map($('#selectBetTypeAdd').fanexMultiSelect("getItems", true), function (x) {
            return parseInt(x);
        });
        if (betTypeList.length === 0) {
            CMS.showTipsyError($("#selectBetTypeAdd"), 'This field is required', "w");
            return;
        }

        var validateLeagueResult = reformatAndValidateLeague();
        if (validateLeagueResult.invalidLeagueIds.length > 0) {
            CMS.showTipsyError($("#txtLeagueAdd"), 'Invalid League ID: ' + validateLeagueResult.invalidLeagueIds.join(', '));
            return;
        }

        if (validateLeagueResult.duplicatedLeagueIds.length > 0) {
            CMS.showTipsyError($("#txtLeagueAdd"), 'Duplicated League ID: ' + validateLeagueResult.duplicatedLeagueIds.join(', '));
            return;
        }

        var leagueList = $("#txtLeagueAdd").val();
        var beginTime;
        var endTime;
        var isSpecificTime = $('#chkSpecificTime').is(':checked');
        if (isSpecificTime) {
            beginTime = $.trim($('#txtFromTimeAdd').val());
            endTime = $.trim($('#txtToTimeAdd').val());
            var timeRegex = /^([0-9][0-9]):[0-9][0-9]$/;
            if (!timeRegex.test(beginTime)) {
                CMS.showTipsyError($("#txtFromTimeAdd"), 'Invalid time format');
                return;
            }

            if (!timeRegex.test(endTime)) {
                CMS.showTipsyError($("#txtToTimeAdd"), 'Invalid time format');
                return;
            }
        }

        var liveIndicator = $('#selectLiveIndicatorAdd').val();
        var restrictVIP = $('#chkRestrictVIP').is(':checked');
        var params = {
            betLimitTypeId: betLimitTypeId,
            betLimitType: betLimitType,
            productList: productList.join(','),
            sportTypeList: sportTypeList.join(','),
            betTypeList: betTypeList.join(','),
            liveIndicator: liveIndicator,
            leagueList: leagueList,
            beginTime: beginTime,
            endTime: endTime,
            restrictVIP: restrictVIP
        };
        CMS.disableInput();
        CMS.hideInlineMsg($('#msgAdd'));
        $.ajax({
            type: 'POST',
            url: CMS.resolveRootUrl(betLimitTypeId ? 'BetLimitTypeControl/Update' : 'BetLimitTypeControl/Insert'),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: JSON.stringify(params),
            success: function (data) {
                if (data.Code) {
                    CMS.showInlineErrorMsg(data.Message, $('#msgAdd'), false, 15000);
                    return;
                }

                var message = betLimitTypeId ? 'Bet limit has been successfully updated' : 'New bet limit type has been successfully created';
                CMS.showInlineSuccessMsg(message, $('#msgAdd'), true);
                searchWithParams(queryParams);
                setTimeout(CMS.closePopup, 3000);
            },
            error: function (xhr) {
                CMS.handleAjaxError(xhr);
            },
            complete: function () {
                CMS.enableInput();
            }
        });
    }

    function search() {
        var productId = parseInt($('#selectProduct').val());
        var sportTypeId = parseInt($('#selectSportType').val());
        var betTypeId = parseInt($('#selectBetType').val());
        var liveIndicator = parseInt($('#selectLiveIndicator').val());
        queryParams = {
            productId,
            sportTypeId,
            betTypeId,
            liveIndicator
        };
        searchWithParams(queryParams);
    }

    function searchWithParams(params) {
        CMS.disableInput();
        $.ajax({
            type: 'POST',
            url: CMS.resolveRootUrl('BetLimitTypeControl/Search'),
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(params),
            dataType: 'json',
            success: function (data) {
                renderGrid(data);
            },
            error: function (xhr) {
                CMS.handleAjaxError(xhr);
            },
            complete: function () {
                CMS.enableInput();
            }
        });
    }

    function renderGrid(data) {
        queryData = data;
        var gridOptions = {
            columns: [
                { HeaderText: 'Type <span class="type-toggle-all">[+]</span>', Width: 200, Name: 'BetLimitType', HeaderAlign: 'center', CellAlign: 'left' },
                { HeaderText: 'Product', Width: 200, Name: 'ProductList', HeaderAlign: 'center', CellAlign: 'left' },
                { HeaderText: 'Sport', Width: 200, Name: 'SportTypeList', HeaderAlign: 'center', CellAlign: 'left' },
                { HeaderText: 'Bet Type', Width: 280, Name: 'BetTypeList', HeaderAlign: 'center', CellAlign: 'left' },
                { HeaderText: 'Live / Non Live', Width: 100, Name: 'LiveIndicator', HeaderAlign: 'center', CellAlign: 'center' },
                { HeaderText: 'Sportsbook League ID', Width: 200, Name: 'LeagueList', HeaderAlign: 'center', CellAlign: 'left' },
                { HeaderText: 'Time (GMT+8)', Width: 100, Name: 'Time', HeaderAlign: 'center', CellAlign: 'center' },
                { HeaderText: 'Restrict VIP' + '<span class="light-note-info-message small-margin-left" title="' + $('#restrict-vip-title').html() + '"></span>', Width: 100, Name: 'RestrictVIP', HeaderAlign: 'center', CellAlign: 'center' },
            ],
            fullWidth: true,
            fullHeight: true,
            showDistinctRow: false,
            showHighlightRow: true,
            gridExpandHeight: 180
        };
        if (isHost) {
            gridOptions.columns.push({ HeaderText: 'Action', Width: 50, Name: 'Action', HeaderAlign: 'center', CellAlign: 'center' });
        }
        var bodyData = [];
        for (var item of data) {
            row = {
                Columns: [
                    {
                        Name: 'BetLimitType',
                        TdClass: 'vertical-top',
                        Value: '<span class="type-toggle">[+]</span>' + item.BetLimitType
                    },
                    {
                        Name: 'ProductList',
                        TdClass: 'vertical-top',
                        Value: '<div data-isAll="' + item.IsAllProduct + '" data-productList="' + item.ProductList + '"></div>'
                    },
                    {
                        Name: 'SportTypeList',
                        TdClass: 'vertical-top',
                        Value: '<div data-isAll="' + item.IsAllSportType + '" data-sportTypeList="' + item.SportTypeList + '"></div>'
                    },
                    {
                        Name: 'BetTypeList',
                        TdClass: 'vertical-top',
                        Value: '<div data-isAll="' + item.IsAllBetType + '" data-betTypeList="' + item.BetTypeList + '"></div>'
                    },
                    {
                        Name: 'LiveIndicator',
                        TdClass: 'vertical-top',
                        Value: liveIndicators[item.LiveIndicator]
                    },
                    {
                        Name: 'LeagueList',
                        TdClass: 'vertical-top',
                        Value: '<div data-isAll="' + item.IsAllLeague + '" data-leagueList="' + item.LeagueList + '"></div>'
                    },
                    {
                        Name: 'Time',
                        TdClass: 'vertical-top',
                        Value: item.BeginTime === null ? 'All' : item.BeginTime + ' - ' + item.EndTime
                    },
                    {
                        Name: 'RestrictVIP',
                        TdClass: 'vertical-top',
                        Value: item.RestrictVIP ? 'Yes' : 'No'
                    }
                ]
            };
            if (isHost) {
                row.Columns.push({
                    Name: 'Action',
                    TdClass: 'vertical-top',
                    Value: '<span class="ico-edit" data-id="' + item.BetLimitTypeId + '"></span><span class="ico-delete mid-margin-left" data-id="' + item.BetLimitTypeId + '"></span>'
                });
            }
            bodyData.push(row);
        }

        gridOptions.bodyRows = bodyData;
        $('#grid').fanexGrid(gridOptions);
        $(".light-note-info-message").tipsy({
            className: "custom-grid-colheader-note",
            html: true,
            opacity: 0.9,
            gravity: "n"
        });
        $('.ico-delete').click(function () {
            var id = parseInt($(this).attr('data-id'));
            confirmDelete(id);
        });
        $('.ico-edit').click(function () {
            var id = parseInt($(this).attr('data-id'));
            confirmEdit(id);
        });
        $('.type-toggle-all').click(function () {
            var status = $(this).data('toggle') || 'collapse';
            $('.type-toggle').trigger('toggle', status);
            if (status === 'collapse') {
                status = 'expand';
                $(this).text('[−]');
            } else {
                status = 'collapse';
                $(this).text('[+]');
            }
            $(this).data('toggle', status);
        });
        $('.type-toggle')
            .click(function () {
                var status = $(this).data('toggle') || 'collapse';
                if (status === 'collapse') {
                    status = 'expand';
                    $(this).text('[−]');
                } else {
                    status = 'collapse';
                    $(this).text('[+]');
                }
                $(this).data('toggle', status);
                var $tr = $(this).closest('tr');
                $tr.find('[data-productList]').trigger('toggle', status);
                $tr.find('[data-sportTypeList]').trigger('toggle', status);
                $tr.find('[data-betTypeList]').trigger('toggle', status);
                $tr.find('[data-leagueList]').trigger('toggle', status);
                $('#grid').fanexGrid('resizeGrid');
            }).on('toggle', function (e, status) {
                $(this).data('toggle', status).trigger('click');
            });
        registerToggledTextControl('data-productList', filterData.Products, 'ProductId', 'ProductName');
        registerToggledTextControl('data-sportTypeList', filterData.SportTypes, 'SportTypeId', 'SportTypeName');
        registerToggledTextControl('data-betTypeList', filterData.BetTypes, 'BetTypeId', 'BetTypeName', function (row) {
            return row.BetTypeName + ' [' + row.BetTypeId + ']';
        });
        registerToggledTextControl('data-leagueList');
        $('#grid').fanexGrid('resizeGrid');
    }

    function registerToggledTextControl(queryDataName, dataSource, keyValue, keyName, customNameFn) {
        $('[' + queryDataName + ']').each(function () {
            var $control = $(this);
            var isAll = $(this).attr('data-isAll') === 'true';
            $control.data('isAll', isAll);
            var dataList = $control.attr(queryDataName);
            var ids = [];
            if (dataList) {
                ids = $.map(dataList.split(','), function (x) { return parseInt(x) });
            }

            if (dataSource) {
                $control.data('allText', 'All (' + dataSource.length + ')');
                var names = [];
                for (var id of ids) {
                    for (var row of dataSource) {
                        if (row[keyValue] === id) {
                            names.push(customNameFn ? customNameFn(row) : row[keyName]);
                            break;
                        }
                    }
                }
                $control.data('list', names);
            } else {
                $control.data('allText', 'All');
                $control.data('list', ids);
            }

            $control.on('toggle', function (e, status) {
                var isAll = $(this).data('isAll');
                var list = $(this).data('list');
                if (status === 'collapse') {
                    if (isAll) {
                        $(this).html($(this).data('allText'));
                    } else {
                        var html = [];
                        for (var i = 0; i < list.length && i < 3; i++) {
                            html.push('<div>' + list[i] + '</div>');
                        }
                        if (list.length >= 4) {
                            if (list.length === 4) {
                                html.push('<div>' + list[3] + '</div>');
                            } else {
                                html.push('<div>...</div>');
                            }
                        }

                        $(this).html(html.join(''));
                    }
                } else {
                    if (isAll && list.length === 0) {
                        $(this).html($(this).data('allText'));
                    } else {
                        var html = [];
                        for (var i = 0; i < list.length; i++) {
                            html.push('<div>' + list[i] + '</div>');
                        }

                        $(this).html(html.join(''));
                    }
                }
            });

            $control.trigger('toggle', 'collapse');
        });
    }

    function confirmEdit(id) {
        var selectedRow;
        for (var row of queryData) {
            if (row.BetLimitTypeId === id) {
                selectedRow = row;
                break;
            }
        }

        showAddOrUpdateDialog(selectedRow);
    }

    function confirmDelete(id) {
        CMS.showConfirmMessage('After deleting this Bet Limit Type, the accounts applying this Bet Limit Type will not be restricted by it no more.<br />Do you want to delete the selected bet limit?', function () {
            CMS.disableInput();
            CMS.closeMsgBox();

            $.ajax({
                type: 'POST',
                url: CMS.resolveRootUrl('BetLimitTypeControl/Delete'),
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                data: JSON.stringify({ betLimitTypeId: id }),
                success: function (data) {
                    if (data.Code) {
                        CMS.showErrorMessage(data.Message);
                        return;
                    }

                    CMS.showSuccessMessage('Bet limit has been successfully deleted');
                    searchWithParams(queryParams);
                },
                error: function (xhr) {
                    CMS.handleAjaxError(xhr);
                },
                complete: function () {
                    CMS.enableInput();
                }
            });
        });
    }

    this.InitPage = function () {
        isHost = $('.filter-bar').attr('isHost') === 'True';
        $('#selectLiveIndicator option').each(function (index, option) {
            liveIndicators[$(option).val()] = $(option).text();
        });
        $('#btnAdd').click(function () {
            showAddOrUpdateDialog();
        });
        $('#btnSearch').click(search);
        loadFilter();
    }
}