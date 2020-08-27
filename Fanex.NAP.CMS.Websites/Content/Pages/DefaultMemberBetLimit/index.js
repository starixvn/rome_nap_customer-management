$(document).ready(function () {
    var defaultMemberBetLimit = new Fanex.CMS.DefaultMemberBetLimit();
    defaultMemberBetLimit.InitPage();
});

Namespace.Register("Fanex.CMS.DefaultMemberBetLimit");
Fanex.CMS.DefaultMemberBetLimit = function () {
    var isHost = false;
    var $frmAdd;
    var $frmUpdate;
    var queryParams;

    function IsPositiveNumber(number) {
        if (number && number >= 0) {
            return true;
        }
        return false;
    }

    function FormatNumber(number) {
        if (number === null || isNaN(number)) {
            return "N/A";
        } else {
            return numeral(number).format(CMSConstants.NumberFormat.IntNumber);
        }
    }

    function showAddDialog() {
        CMS.modal('New Default Bet Limit', $frmAdd.clone(), { resizable: false });
        $("#btnCancelAdd").click(CMS.closePopup);
        $('#btnSaveAdd').click(function () {
            var betLimitTypeId = parseInt($('#selectBetLimitTypeAdd').val());
            if (betLimitTypeId === 0) {
                CMS.showTipsyError($("#selectBetLimitTypeAdd"), 'This field is required', "w");
                return;
            }

            var username = $.trim($("#txtUsernameAdd").val());
            if (!username) {
                CMS.showTipsyError($("#txtUsernameAdd"), 'This field is required', "w");
                return;
            }

            var maxPerMatchString = $('#txtMaxPerMatchAdd').val();
            if (!maxPerMatchString) {
                CMS.showTipsyError($('#txtMaxPerMatchAdd'), 'This field is required', "w");
                return;
            }

            var maxPerMatch = numeral().unformat(maxPerMatchString);
            if (!IsPositiveNumber(maxPerMatch)) {
                CMS.showTipsyError($('#txtMaxPerMatchAdd'), 'Please input a positive whole number.', "w");
                return;
            }

            var maxBetString = $('#txtMaxBetAdd').val();
            if (!maxBetString) {
                CMS.showTipsyError($('#txtMaxBetAdd'), 'This field is required', "w");
                return;
            }

            var maxBet = numeral().unformat(maxBetString);
            if (!IsPositiveNumber(maxBet)) {
                CMS.showTipsyError($('#txtMaxBetAdd'), 'Please input a positive whole number.', "w");
                return;
            }

            if (maxBet > maxPerMatch) {
                CMS.showTipsyError($('#txtMaxPerMatchAdd'), 'Default Max Per Match must be greater than or equal to Default Max Bet.', "w");
                return;
            }

            var params = {
                betLimitTypeId,
                username,
                maxPerMatch,
                maxBet
            };
            CMS.disableInput();
            CMS.hideInlineMsg($('#msgAdd'));
            $.ajax({
                type: 'POST',
                url: CMS.resolveRootUrl('DefaultMemberBetLimit/Insert'),
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                data: JSON.stringify(params),
                success: function (data) {
                    if (data.Code) {
                        CMS.showInlineErrorMsg(data.Message, $('#msgAdd'), false, 15000);
                        return;
                    }

                    CMS.showInlineSuccessMsg('New Default Bet Limit Setting has been successfully created', $('#msgAdd'), true);
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
        });
    }

    function showUpdateDialog(custId, username, betLimitTypeId, betLimitType, maxBet, maxPerMatch) {
        CMS.modal('Update Default Bet Limit', $frmUpdate.clone(), { resizable: false });
        $('#txtBetLimitTypeUpdate').text(betLimitType);
        $('#txtUsernameUpdate').text(username);
        $('#txtMaxPerMatchUpdate').val(FormatNumber(maxPerMatch));
        $('#txtMaxBetUpdate').val(FormatNumber(maxBet));
        $("#btnCancelUpdate").click(CMS.closePopup);
        $('#btnSaveUpdate').click(function () {
            var maxPerMatchString = $('#txtMaxPerMatchUpdate').val();
            if (!maxPerMatchString) {
                CMS.showTipsyError($('#txtMaxPerMatchUpdate'), 'This field is required', "w");
                return;
            }

            var maxPerMatch = numeral().unformat(maxPerMatchString);
            if (!IsPositiveNumber(maxPerMatch)) {
                CMS.showTipsyError($('#txtMaxPerMatchUpdate'), 'Please input a positive whole number.', "w");
                return;
            }

            var maxBetString = $('#txtMaxBetUpdate').val();
            if (!maxBetString) {
                CMS.showTipsyError($('#txtMaxBetUpdate'), 'This field is required', "w");
                return;
            }

            var maxBet = numeral().unformat(maxBetString);
            if (!IsPositiveNumber(maxBet)) {
                CMS.showTipsyError($('#txtMaxBetUpdate'), 'Please input a positive whole number.', "w");
                return;
            }

            if (maxBet > maxPerMatch) {
                CMS.showTipsyError($('#txtMaxPerMatchUpdate'), 'Default Max Per Match must be greater than or equal to Default Max Bet.', "w");
                return;
            }

            var params = {
                betLimitTypeId,
                custId,
                maxPerMatch,
                maxBet
            };
            CMS.disableInput();
            CMS.hideInlineMsg($('#msgUpdate'));
            $.ajax({
                type: 'POST',
                url: CMS.resolveRootUrl('DefaultMemberBetLimit/Update'),
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                data: JSON.stringify(params),
                success: function (data) {
                    if (data.Code) {
                        CMS.showInlineErrorMsg(data.Message, $('#msgUpdate'), false, 15000);
                        return;
                    }

                    CMS.showInlineSuccessMsg('Default Bet Limit Setting has been successfully updated', $('#msgUpdate'), true);
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
        });
    }

    function search() {
        var betLimitTypeID = parseInt($('#selectBetLimitType').val());
        var username = $.trim($('#selectUsername').val());
        queryParams = {
            betLimitTypeID,
            username
        };
        searchWithParams(queryParams);
    }

    function searchWithParams(params) {
        CMS.disableInput();
        $.ajax({
            type: 'POST',
            url: CMS.resolveRootUrl('DefaultMemberBetLimit/Search'),
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
                { HeaderText: 'Bet Limit Type', Width: 300, Name: 'BetLimitType', HeaderAlign: 'center', CellAlign: 'left' },
                { HeaderText: 'SMA Username', Width: 200, Name: 'Username', HeaderAlign: 'center', CellAlign: 'left' },
                { HeaderText: 'Level', Width: 100, Name: 'Level', HeaderAlign: 'center', CellAlign: 'center' },
                { HeaderText: 'Currency', Width: 100, Name: 'Currency', HeaderAlign: 'center', CellAlign: 'center' },
                { HeaderText: 'Default Max Per Match', Width: 150, Name: 'DefaultMaxPerMatch', HeaderAlign: 'center', CellAlign: 'right' },
                { HeaderText: 'Default Max Bet', Width: 150, Name: 'DefaultMaxBet', HeaderAlign: 'center', CellAlign: 'right' }
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
            var row = {
                Columns: [
                    {
                        Name: 'BetLimitType',
                        TdClass: 'vertical-top break-word',
                        Value: item.BetLimitType
                    },
                    {
                        Name: 'Username',
                        TdClass: 'vertical-top break-word',
                        Value: item.Username
                    },
                    {
                        Name: 'Level',
                        TdClass: 'vertical-top',
                        Value: item.Level
                    },
                    {
                        Name: 'Currency',
                        TdClass: 'vertical-top',
                        Value: item.Currency
                    },
                    {
                        Name: 'DefaultMaxPerMatch',
                        TdClass: 'vertical-top',
                        Value: FormatNumber(item.MaxPerMatch)
                    },
                    {
                        Name: 'DefaultMaxBet',
                        TdClass: 'vertical-top',
                        Value: FormatNumber(item.MaxBet)
                    }
                ]
            };
            if (isHost) {
                row.Columns.push({
                    Name: 'Action',
                    TdClass: 'vertical-top',
                    Value: '<span title="Update" class="ico-edit" data-betLimitTypeId="' + item.BetLimitTypeId + '" data-custId="' + item.CustId + '" data-username="' + item.Username + '" data-betLimitType="' + item.BetLimitType + '" data-maxBet="' + item.MaxBet + '" data-maxPerMatch="' + item.MaxPerMatch + '"></span><span title="Delete" class="ico-delete mid-margin-left" data-betLimitTypeId="' + item.BetLimitTypeId + '" data-custId="' + item.CustId + '"></span>'
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
            var custId = parseInt($(this).attr('data-custId'));
            var betLimitTypeId = parseInt($(this).attr('data-betLimitTypeId'));
            confirmDelete(custId, betLimitTypeId);
        });
        $('.ico-edit').click(function () {
            var custId = parseInt($(this).attr('data-custId'));
            var betLimitTypeId = parseInt($(this).attr('data-betLimitTypeId'));
            var betLimitType = $(this).attr('data-betLimitType');
            var username = $(this).attr('data-username');
            var maxBet = parseFloat($(this).attr('data-maxBet'));
            var maxPerMatch = parseFloat($(this).attr('data-maxPerMatch'));
            showUpdateDialog(custId, username, betLimitTypeId, betLimitType, maxBet, maxPerMatch);
        });
    }

    function confirmDelete(custId, betLimitTypeId) {
        CMS.showConfirmMessage('<div class="mid-margin-bottom">You are going to delete the Default Bet Setting for the selected account. The setting will not be applied for the new members.</div><label><input type="checkbox" id="isDownlineAffected" /> Also delete the Bet Limit Type for all members of the selected account.</label>', function () {
            CMS.disableInput();
            CMS.closeMsgBox();

            var isDownlineAffected = $('#isDownlineAffected').is(':checked');
            var model = {
                custId,
                betLimitTypeId,
                isDownlineAffected
            }
            $.ajax({
                type: 'POST',
                url: CMS.resolveRootUrl('DefaultMemberBetLimit/Delete'),
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                data: JSON.stringify(model),
                success: function (data) {
                    if (data.Code) {
                        CMS.showErrorMessage(data.Message);
                        return;
                    }

                    CMS.showSuccessMessage('Default Bet Limit Setting has been successfully deleted');
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
        $frmAdd = $('#frmAdd').clone();
        $frmUpdate = $('#frmUpdate').clone();
        $("#div-popup").remove();
        $('#btnAdd').click(function () {
            showAddDialog();
        });
        $('#btnSearch').click(search);
        $('#selectBetLimitType').combobox('refresh');
    }
}