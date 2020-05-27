$(document).ready(function () {
    var betLimitTypeControl = new Fanex.CMS.BetLimitTypeControl();
    betLimitTypeControl.InitPage();
});

Namespace.Register("Fanex.CMS.BetLimitTypeControl");
Fanex.CMS.BetLimitTypeControl = function () {
    var $this = this;
    var filterData;
    var $addDialog;

    function generateOptionsHtml(data, valueKey, textKey, customText) {
        var html = [];
        $.each(data, function (index, row) {
            var text = customText ? customText(row) : row[textKey];
            html.push('<option value="' + row[valueKey] + '">' + text + '</option>');
        });

        return html.join('');
    }

    function filterSportTypes(productIds) {
        return $.grep(filterData.SportTypes, function (sportType) {
            return productIds.indexOf(sportType.ProductId) !== -1;
        });
    }

    function filterBetTypes(productIds, sportTypeIds) {
        return $.grep(filterData.BetTypes, function (betType) {
            return productIds.indexOf(betType.ProductId) !== -1 && sportTypeIds.indexOf(betType.SportTypeId) !== -1;
        });
    }

    function onProductChange() {
        var productId = parseInt($('#selectProduct').val());
        var sportTypes = productId === 0 ? filterData.SportTypes : filterSportTypes([productId]);
        sportTypes = [{ SportTypeId: 0, SportTypeName: 'All' }].concat(sportTypes);
        $('#selectSportType')
            .html(generateOptionsHtml(sportTypes, 'SportTypeId', 'SportTypeName'))
            .combobox('refresh')
            .trigger('change');
    }

    function onSportTypeChange() {
        var productId = parseInt($('#selectProduct').val());
        var sportTypeId = parseInt($('#selectSportType').val());
        var betTypes = sportTypeId === 0 ? filterData.BetTypes : filterBetTypes([productId], [sportTypeId]);
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

    function getMultiselectSource(list, valueKey, textKey, customText) {
        var dataList = [];

        for (var i = 0; i < list.length; i++) {
            var row = list[i];
            var value = row[valueKey];
            var text = customText ? customText(row) : row[textKey];
            dataList.push({
                id: value,
                value: value,
                text: text,
                checked: true,
                enable: true
            });
        }

        return dataList;
    }

    function customCheckAllTextFn() {
        return 'All (' + this.listData.length + ')';
    }

    function showAddDialog() {
        var minWidth = 345;
        var maxHeight = 250;
        function onProductCheckChange() {
            CMS.clearTipsyError($("#selectProductAdd"));
            var selectedProductIds = $.map($('#selectProductAdd').fanexMultiSelect("getItems", true), function (x) {
                return parseInt(x);
            });
            var selectedSportTypes = filterSportTypes(selectedProductIds);

            var sportTypesDataSource = getMultiselectSource(selectedSportTypes, 'SportTypeId', 'SportTypeName');
            $('#selectSportTypeAdd').fanexMultiSelect({
                minWidth: minWidth,
                maxHeight: maxHeight,
                checkAllText: 'All',
                isCheckAll: true,
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
            var selectedBetTypes = filterBetTypes(selectedProductIds, selectedSportTypeIds);

            var betTypesDataSource = getMultiselectSource(selectedBetTypes, 'BetTypeId', 'BetTypeName', function (row) {
                var betTypeId = row.BetTypeId;
                var betTypeName = row.BetTypeName;
                return betTypeName + ' [' + betTypeId + ']';
            });
            $('#selectBetTypeAdd').fanexMultiSelect({
                minWidth: minWidth,
                maxHeight: maxHeight,
                checkAllText: 'All',
                isCheckAll: true,
                useCustomCheckAllText: true,
                customCheckAllText: customCheckAllTextFn,
                listData: betTypesDataSource,
                onCheck: onBetTypeCheckChange
            });
        }

        function onBetTypeCheckChange() {
            CMS.clearTipsyError($("#selectBetTypeAdd"));
        }

        var productsDataSource = getMultiselectSource(filterData.Products, 'ProductId', 'ProductName');
        $('#selectProductAdd').fanexMultiSelect({
            minWidth: minWidth,
            maxHeight: maxHeight,
            checkAllText: 'All',
            isCheckAll: true,
            useCustomCheckAllText: true,
            customCheckAllText: customCheckAllTextFn,
            listData: productsDataSource,
            onCheck: onProductCheckChange
        });
        onProductCheckChange();

        $('#selectLiveIndicatorAdd').prop("selectedIndex", 0);
        $('#txtTypeAdd').val('');
        $('#txtLeagueAdd').val('');
        $('#txtFromTimeAdd').val('');
        $('#txtToTimeAdd').val('');
        $('#chkSpecificTime')
            .change(function () {
                if ($(this).is(':checked')) {
                    $('#txtFromTimeAdd').prop("disabled", false);
                    $('#txtToTimeAdd').prop("disabled", false);
                } else {
                    $('#txtFromTimeAdd').prop("disabled", true);
                    $('#txtToTimeAdd').prop("disabled", true);
                }
            })
            .prop('checked', false)
            .trigger('change');
        $('#chkRestrictVIP').prop('checked', false);

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
            $('#btnSaveAdd').click(add);
            $('#txtLeagueAdd').blur(reformatAndValidateLeague);
            $addDialog = CMS.modal("New Bet Limit Type", $('#frmAdd'), { resizable: false });
        } else {
            $addDialog.dialog('open');
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

    function bindEvents() {
        $('#btnAdd').click(showAddDialog);
        $('#btnSearch').click(search);
    }

    function add() {
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
        CMS.hideInlineMsg($('#msgAdd'));
        $.ajax({
            type: 'POST',
            url: CMS.resolveRootUrl('BetLimitTypeControl/Insert'),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: JSON.stringify(params),
            success: function (data) {
                if (data.Code) {
                    CMS.showInlineErrorMsg(data.Message, $('#msgAdd'), false, 15000);
                    return;
                }

                CMS.showInlineSuccessMsg('New bet limit type has been successfully created', $('#msgAdd'), true);
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
        var params = {
            productId,
            sportTypeId,
            betTypeId,
            liveIndicator
        };
        CMS.disableInput();
        $.ajax({
            type: 'POST',
            url: CMS.resolveRootUrl('BetLimitTypeControl/Search'),
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(params),
            dataType: 'json',
            success: function (data) {
                console.log(data);
            },
            error: function (xhr) {
                CMS.handleAjaxError(xhr);
            },
            complete: function () {
                CMS.enableInput();
            }
        });
    }

    this.InitPage = function () {
        loadFilter();
        bindEvents();
    }
}