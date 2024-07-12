$(document).ready(function () {
    var betLimitTypeControl = new Fanex.CMS.BetLimitTypeControl();
    betLimitTypeControl.InitPage();
});

Namespace.Register('Fanex.CMS.BetLimitTypeControl');
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
        var productIds = [];
        if (productId === 0) {
            for (var product of filterData.Products) {
                productIds.push(product.ProductId);
            }
        } else {
            productIds.push(productId);
        }
        var sportTypes = filterAndDistinctSportTypes(productIds);
        sportTypes = [{ SportTypeId: 0, SportTypeName: 'All' }].concat(sportTypes);
        $('#selectSportType')
            .html(generateOptionsHtml(sportTypes, 'SportTypeId', 'SportTypeName'))
            .combobox('refresh')
            .trigger('change');
    }

    function onSportTypeChange() {
        var productId = parseInt($('#selectProduct').val());
        var productIds = [];
        if (productId === 0) {
            for (var product of filterData.Products) {
                productIds.push(product.ProductId);
            }
        } else {
            productIds.push(productId);
        }

        var sportTypeId = parseInt($('#selectSportType').val());
        var sportTypeIds = [];
        if (sportTypeId === 0) {
            for (var sportType of filterData.SportTypes) {
                if (productId === 0 || sportType.ProductId === productId) {
                    sportTypeIds.push(sportType.SportTypeId);
                }
            }
        } else {
            sportTypeIds.push(sportTypeId);
        }

        var betTypes = filterAndDistinctBetTypes(productIds, sportTypeIds);
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
                CMS.enableInput();
                filterData = data;
                renderFilter();
                search();
            },
            error: function (xhr) {
                CMS.enableInput();
                CMS.handleAjaxError(xhr);
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
        CMS.hideInlineMsg($('#msgAdd'));
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
            CMS.clearTipsyError($('#selectProductAdd'));
            var selectedProductIds = $.map($('#selectProductAdd').fanexMultiSelect('getItems', true), function (x) {
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
            CMS.clearTipsyError($('#selectSportTypeAdd'));
            var selectedProductIds = $.map($('#selectProductAdd').fanexMultiSelect('getItems', true), function (x) {
                return parseInt(x);
            });
            var selectedSportTypeIds = $.map($('#selectSportTypeAdd').fanexMultiSelect('getItems', true), function (x) {
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
            CMS.clearTipsyError($('#selectBetTypeAdd'));
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

        var $ddlLeagueGroupAdd = null;

        if (isAdd) {
            $('#selectLiveIndicatorAdd').prop('selectedIndex', 0);
            $ddlLeagueGroupAdd = renderDropdownListLeagueGroup("#ddlLeagueGroupAdd", "");
        } else {
            $('#selectLiveIndicatorAdd').val(betLimitTypeData.LiveIndicator);
            var leagueGroupResult = getLeagueGroupList(betLimitTypeData.LeagueGroupList, betLimitTypeData.ExcludedLeagueGroupList);
            $ddlLeagueGroupAdd = renderDropdownListLeagueGroup("#ddlLeagueGroupAdd", leagueGroupResult.leagueGroupList);
            $("#ddlLeagueGroupInNotIn").val(leagueGroupResult.isExcludedLeagueGroup ? "1" : "0");
        }
        $("#ddlLeagueGroupInNotIn").combobox();

        
        $('#txtTypeAdd').val(isAdd ? '' : betLimitTypeData.BetLimitType);
        $('#txtFromTimeAdd').val(isAdd ? '' : betLimitTypeData.BeginTime || '');
        $('#txtToTimeAdd').val(isAdd ? '' : betLimitTypeData.EndTime || '');
        $('#chkSpecificTime')
            .change(function () {
                if ($(this).is(':checked')) {
                    $('#txtFromTimeAdd').prop('disabled', false);
                    $('#txtToTimeAdd').prop('disabled', false);
                } else {
                    $('#txtFromTimeAdd').prop('disabled', true).val('');
                    $('#txtToTimeAdd').prop('disabled', true).val('');
                }
            })
            .prop('checked', isAdd ? false : !!betLimitTypeData.BeginTime)
            .trigger('change');
        $('#chkRestrictVIP').prop('checked', isAdd ? false : betLimitTypeData.RestrictVIP);

        var title = isAdd ? 'New Bet Limit Type' : 'Update Bet Limit Type';
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
            $('#txtLeagueAdd').blur(function () {
                reformatAndValidateInputId($(this));
            });
            $addDialog = CMS.modal(title, $('#frmAdd'), {
                resizable: false,
                position: {
                    my: 'center top',
                    at: 'center top',
                    of: '#headline'
                }
            });
        } else {
            $addDialog.dialog('open');
            $('#ui-dialog-title-dialog-popup').text(title);
            $addDialog.find('.error-tipsy').remove();
        }

        $('#btnSaveAdd').off('click').on('click', function () {
            if (betLimitTypeData) {
                addOrUpdate(betLimitTypeData.BetLimitTypeId);
            } else {
                addOrUpdate();
            }
        });
    }

    function renderDropdownListLeagueGroup(selectElementId, selectedLeagueGroup) {
        var sabaItems = renderOptionLeagueGroup(viewModel.SabaSportsLeagueGroups, selectedLeagueGroup);
        var isCheckAllSaba = true;
        sabaItems.some(function (item) {
            if (item.checked === false) {
                isCheckAllSaba = false;
                return true;
            }
        });

        var otherItems = renderOptionLeagueGroup(viewModel.OtherLeagueGroups, selectedLeagueGroup);
        var isCheckAllOther = true;
        otherItems.some(function (item) {
            if (item.checked === false) {
                isCheckAllOther = false;
                return true;
            }
        });
        
        let optionList = [
            {
                value: "sabasports",
                text: "Saba Sports",
                checked: isCheckAllSaba,
                enable: true,
                items: renderOptionLeagueGroup(viewModel.SabaSportsLeagueGroups, selectedLeagueGroup)
            },
            {
                value: "others",
                text: "Others",
                checked: isCheckAllOther,
                enable: true,
                items: renderOptionLeagueGroup(viewModel.OtherLeagueGroups, selectedLeagueGroup)
            }
        ];

        return $(selectElementId).fanexMultiSelect({
            checkAllText: "All",
            noneSelectedText: "Please select",
            noItemText: "There is no data",
            noItemFoundText: "No item found",
            filterPlaceHolder: "",
            minWidth: 345,
            maxHeight: 230,
            listData: optionList,
            showFilter: true
        });
    }

    function renderOptionLeagueGroup(leagueGroups, seletecIds) {
        var options = [];
        leagueGroups.forEach(lg => options.push({ value: lg.Id, text: lg.Name, checked: seletecIds.includes(lg.Id.toString()), enable: true }));

        return options;
    }

    function reformatAndValidateInputId(textarea) {
        var idList = [];
        var invalidIdList = [];
        var duplicatedIdList = [];
        var text = textarea.val();
        var words = text.replace(/ /g, ',').replace(/\n/g, ',').split(',');
        var validWords = [];
        for (var word of words) {
            var trimmedWord = $.trim(word);
            if (trimmedWord) {
                var id = parseInt(trimmedWord);
                if (id.toString() !== trimmedWord) {
                    invalidIdList.push(trimmedWord);
                } else {
                    if (idList.indexOf(id) !== -1 && duplicatedIdList.indexOf(id) === -1) {
                        duplicatedIdList.push(id);
                    }

                    idList.push(id);
                }

                validWords.push(trimmedWord);
            }
        }

        textarea.val(validWords.join(','));
        return {
            invalidIdList: invalidIdList,
            duplicatedIdList: duplicatedIdList
        };
    }

    function addOrUpdate(betLimitTypeId) {
        var betLimitType = $.trim($('#txtTypeAdd').val());
        if (!betLimitType) {
            CMS.showTipsyError($('#txtTypeAdd'), 'This field is required', 'w');
            return;
        }

        var productUncheckedList = $('#selectProductAdd').fanexMultiSelect('getItems', false);
        var productCheckedList = $.map($('#selectProductAdd').fanexMultiSelect('getItems', true), function (x) {
            return parseInt(x);
        });
        if (productCheckedList.length === 0) {
            CMS.showTipsyError($('#selectProductAdd'), 'This field is required', 'w');
            return;
        }

        var sportTypeUncheckedList = $('#selectSportTypeAdd').fanexMultiSelect('getItems', false);
        var sportTypeCheckedList = $.map($('#selectSportTypeAdd').fanexMultiSelect('getItems', true), function (x) {
            return parseInt(x);
        });
        if (sportTypeCheckedList.length === 0) {
            CMS.showTipsyError($('#selectSportTypeAdd'), 'This field is required', 'w');
            return;
        }

        var betTypeUncheckedList = $('#selectBetTypeAdd').fanexMultiSelect('getItems', false);
        var betTypeCheckedList = $.map($('#selectBetTypeAdd').fanexMultiSelect('getItems', true), function (x) {
            return parseInt(x);
        });
        if (betTypeCheckedList.length === 0) {
            CMS.showTipsyError($('#selectBetTypeAdd'), 'This field is required', 'w');
            return;
        }
        var leagueGroupUncheckedList = $('#ddlLeagueGroupAdd').fanexMultiSelect('getItems', false);
        var leagueGroupCheckedList = $.map($('#ddlLeagueGroupAdd').fanexMultiSelect('getItems', true), function (x) {
            return parseInt(x);
        });
        if (leagueGroupCheckedList.length === 0) {
            CMS.showTipsyError($('#ddlLeagueGroupAdd'), 'This field is required', 'w');
            return;
        }

        var $txtLeagueAdd = $('#txtLeagueAdd');
        var validateLeagueResult = reformatAndValidateInputId($txtLeagueAdd);
        if (validateLeagueResult.invalidIdList.length > 0) {
            CMS.showTipsyError($txtLeagueAdd, 'Invalid League ID: ' + validateLeagueResult.invalidIdList.join(', '));
            return;
        }

        if (validateLeagueResult.duplicatedIdList.length > 0) {
            CMS.showTipsyError($txtLeagueAdd, 'Duplicated League ID: ' + validateLeagueResult.duplicatedIdList.join(', '));
            return;
        }

        var leagueList = $txtLeagueAdd.val();
        var beginTime;
        var endTime;
        var isSpecificTime = $('#chkSpecificTime').is(':checked');
        if (isSpecificTime) {
            beginTime = $.trim($('#txtFromTimeAdd').val());
            endTime = $.trim($('#txtToTimeAdd').val());
            var timeRegex = /^([0-9][0-9]):[0-9][0-9]$/;
            if (!timeRegex.test(beginTime)) {
                CMS.showTipsyError($('#txtFromTimeAdd'), 'Invalid time format');
                return;
            }

            if (!timeRegex.test(endTime)) {
                CMS.showTipsyError($('#txtToTimeAdd'), 'Invalid time format');
                return;
            }
        }

        var liveIndicator = $('#selectLiveIndicatorAdd').val();
        var restrictVIP = $('#chkRestrictVIP').is(':checked');
        var params = {
            betLimitTypeId: betLimitTypeId,
            betLimitType: betLimitType,
            productList: productUncheckedList.length === 0 ? '0' : productCheckedList.join(','),
            sportTypeList: sportTypeUncheckedList.length === 0 ? '0' : sportTypeCheckedList.join(','),
            betTypeList: betTypeUncheckedList.length === 0 ? '0' : betTypeCheckedList.join(','),
            liveIndicator: liveIndicator,
            leagueGroupList: leagueGroupUncheckedList.length === 0 ? '-1' : leagueGroupCheckedList.join(","),
            isExcludedLeagueGroup: $('#ddlLeagueGroupInNotIn').val() === "1",
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

                var message = betLimitTypeId ? 'Bet Limit Type has been successfully updated' : 'New Bet Limit Type has been successfully created';
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
                { HeaderText: 'Type' + (data.length > 0 ? '<span class="type-toggle-all">[+]</span>' : ''), Width: 200, Name: 'BetLimitType', HeaderAlign: 'center', CellAlign: 'left' },
                { HeaderText: 'Product', Width: 180, Name: 'ProductList', HeaderAlign: 'center', CellAlign: 'left' },
                { HeaderText: 'Sport', Width: 180, Name: 'SportTypeList', HeaderAlign: 'center', CellAlign: 'left' },
                { HeaderText: 'Bet Type', Width: 280, Name: 'BetTypeList', HeaderAlign: 'center', CellAlign: 'left' },
                { HeaderText: 'Live / Non Live', Width: 100, Name: 'LiveIndicator', HeaderAlign: 'center', CellAlign: 'center' },
                { HeaderText: 'Sportsbook League Group ID', Width: 160, Name: 'LeagueGroupList', HeaderAlign: 'center', CellAlign: 'left' },
                { HeaderText: 'Sportsbook League ID', Width: 140, Name: 'LeagueList', HeaderAlign: 'center', CellAlign: 'left' },
                { HeaderText: 'Time (GMT+8)', Width: 100, Name: 'Time', HeaderAlign: 'center', CellAlign: 'center' },
                { HeaderText: 'Restrict VIP<span class="light-note-info-message small-margin-left" title="' + $('#restrict-vip-title').html() + '"></span>', Width: 100, Name: 'RestrictVIP', HeaderAlign: 'center', CellAlign: 'center' },
            ],
            fullWidth: true,
            fullHeight: true,
            showDistinctRow: false,
            showHighlightRow: true,
            gridExpandHeight: 200
        };
        if (isHost) {
            gridOptions.columns.push({ HeaderText: 'Action', Width: 50, Name: 'Action', HeaderAlign: 'center', CellAlign: 'center' });
        }
        var bodyData = [];
        for (var item of data) {
            var leagueGroupResult = getLeagueGroupList(item.LeagueGroupList, item.ExcludedLeagueGroupList);
            var row = {
                Columns: [
                    {
                        Name: 'BetLimitType',
                        TdClass: 'vertical-top break-word',
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
                        Name: 'LeagueGroupList',
                        TdClass: 'vertical-top' + (leagueGroupResult.isExcludedLeagueGroup ? ' text-red' : ''),
                        Value: '<div data-isAll="' + item.IsAllLeagueGroup
                            + '" data-leaguegrouplist="' + leagueGroupResult.leagueGroupList + '"'
                            + (leagueGroupResult.isExcludedLeagueGroup ? ' data-appendedtext="Not in"' : '')
                            + '></div>'
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
                    Value: '<span title="Update" class="ico-edit" data-id="' + item.BetLimitTypeId + '"></span><span title="Delete" class="ico-delete mid-margin-left" data-id="' + item.BetLimitTypeId + '"></span>'
                });
            }
            bodyData.push(row);
        }

        gridOptions.bodyRows = bodyData;
        $('#grid').fanexGrid(gridOptions);
        $('.light-note-info-message').tipsy({
            className: 'custom-grid-colheader-note',
            html: true,
            opacity: 0.9,
            gravity: 'n'
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
                $tr.find('[data-leaguegrouplist]').trigger('toggle', status);
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
        registerToggledTextControl('data-leaguegrouplist');
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
                $control.data('allText', 'All (' + ids.length + ')');
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
                var appendedText = $(this).data('appendedtext');
                if (status === 'collapse') {
                    if (isAll) {
                        $(this).html($(this).data('allText'));
                    } else {
                        var html = [];
                        if (appendedText) {
                            html.push('<div>' + appendedText + '</div>');
                        }
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
                        if (appendedText) {
                            html.push('<div>' + appendedText + '</div>');
                        }
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

                    CMS.showSuccessMessage('Bet Limit Type has been successfully deleted');
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

    function getLeagueGroupList(leagueGroupList, excludedLeagueGroupList) {
        return {
            leagueGroupList: leagueGroupList || excludedLeagueGroupList,
            isExcludedLeagueGroup: excludedLeagueGroupList.length > 0
        };
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