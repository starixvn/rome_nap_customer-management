/* File Created: May 23, 2014 */
/* global Namespace, viewResources, numeral, Fanex, CMS, labels, keyboardManager, popUpManager */
/* jshint strict: false */
// 'use strict';

$(document).ready(function () {
    var report = new Fanex.CMS.LossMonitor();

    report.InitPage();
});

Namespace.Register("Fanex.CMS.LossMonitor");
Fanex.CMS.LossMonitor = function () {
    var $this = this;

    var maxLossFormat = '<a class="c hand edit-maxloss" href="JavaScript:void(0);" data-custid="{0}" data-username="{1}">{2}</a>' +
                              '<input class="display-none width-95 clearable textbox-maxloss" type="text" maxlength="13" value="{3}" />';

    var maxLossFormatNoHost = '<span>{0}</span>';
    var progressBarFormat = '<div class="percentage-bar" data-value="{0}"><div class="percentage-label"><span>{1}<span></div></div>';
    var notAvailablePercentage = labels.NotAvailable.toLowerCase() + "<span></span>";

    var cachedData = {
        custId: 0,
        username: '',
        maxLoss: 0,
        oldMaxLoss: 0,
        current: null
    };

    var urls = {
        GetSMALossReport: CMS.resolveRootUrl('LossMonitor/GetSMALossReport'),
        AddSMA: CMS.resolveRootUrl('LossMonitor/AddSMA'),
        EditMaxLoss: CMS.resolveRootUrl('LossMonitor/EditMaxLoss'),
        DeleteSMA: CMS.resolveRootUrl('LossMonitor/DeleteSMA'),
        GetGroupDownlines: CMS.resolveRootUrl('LossMonitor/GetGroupDownlines')
    };
    var domElements = {
        divSMALossMonitor: null,
        btnAdd: null,
        divSMALossMonitorContainer: null,
        divAutoRefresh: null,
        divResult: null,
        divAddSMA: null,
        divPopup: null
    };
    var refreshTimer = {
        Interval: [90, 120, 150, 180],
        Default: 90
    };

    // Cache DOM elements
    var cacheDOM = function () {
        domElements.divAutoRefresh = $('#divAutoRefresh');
        domElements.divSMALossMonitor = $('#divSMALossMonitor');
        domElements.divSMALossMonitorContainer = $('#divSMALossMonitorContainer');
        domElements.btnAdd = $('#btn-add');
        domElements.textboxSMAName = $('#txt-username');
        domElements.divResult = $("#div-result-message");
        domElements.divAddSMA = $("#divAddSMA");
        domElements.divPopup = $("#dialog-popup");
    };

    var popupElements = {
        divErrorSuccessMsg: null,
        divForm: null,
        txtUsername: null,
        txtMaxLoss: null,
        btnSave: null,
        btnCancel: null
    };

    var cachePopupElements = function () {
        popupElements.divErrorSuccessMsg = $('#div-result-add-message', domElements.divPopup);
        popupElements.divForm = $('#divAddSMA', domElements.divPopup);
        popupElements.txtUsername = $('#txt-username', domElements.divPopup);
        popupElements.txtMaxLoss = $('#txt-maxloss', domElements.divPopup);
        popupElements.btnSave = $('#btnSave', domElements.divPopup);
        popupElements.btnCancel = $('#btnCancel', domElements.divPopup);
    };

    var initElements = function () {
        if (!viewResources.hasHostPermission) {
            return;
        }

        domElements.btnAdd.removeClass('display-none');
    };

    var bindEvents = function () {
        if (!viewResources.hasHostPermission) {
            return;
        }

        domElements.textboxSMAName.keyup(function (event) {
            if (event.keyCode === keyboardManager.Enums.KeyCode.Enter) {
                self.btnSearchClick();
            }
        });

        domElements.btnAdd.unbind("click");
        domElements.btnAdd.click(btnAddClick);
    };

    var btnAddClick = function () {
        var form = domElements.divAddSMA.clone();

        CMS.modal(labels.AddNewSetting, form, { width: 570, height: 220 });
        cachePopupElements();

        bindPopupEventhandlers();
    };

    // Get SMA turnover report.
    var getSMALossReport = function () {
        CMS.disableInput();

        if (domElements.divAutoRefresh && domElements.divAutoRefresh.stop) {
            domElements.divAutoRefresh.stop();
        }

        $.ajax({
            type: 'POST',
            url: urls.GetSMALossReport,
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
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

    // Render grid.
    var renderGrid = function (data) {
        var smaNameCol = '';
        var percentageCol = '';
        var smaCssClass = '';

        var counter = 0;
        var gridOptions = {
            columns: [{ HeaderText: labels.No, Width: 70, Name: 'ColNo', HeaderAlign: 'center', CellAlign: 'center', ShowCollapseExpand: true },
                      { HeaderText: labels.Username, Width: 250, Name: 'ColUsername', HeaderAlign: 'center', CellAlign: 'left' },
                      { HeaderText: labels.Level, Width: 70, Name: 'ColLevel', HeaderAlign: 'center', CellAlign: 'center' },
                      { HeaderText: labels.Currency, Width: 70, Name: 'ColCurrency', HeaderAlign: 'center', CellAlign: 'center' },
                      {
                          HeaderText: labels.YesterdayBalance, Width: 150, Name: 'ColYesterdayBalance', HeaderAlign: 'center', CellAlign: 'right',
                          FormatNumber: CMS.formats.DecimalFormat
                      },
                      {
                          HeaderText: labels.TotalBalance, Width: 150, Name: 'ColTotalBalance', HeaderAlign: 'center', CellAlign: 'right',
                          FormatNumber: CMS.formats.DecimalFormat
                      },
                      {
                          HeaderText: labels.MaxLoss, Width: 120, Name: 'ColMaxLoss', HeaderAlign: 'center', CellAlign: 'right',
                          FormatNumber: CMS.formats.DecimalFormat
                      },
                      { HeaderText: labels.LossPercentage, Width: 150, Name: 'ColLossPercentage', HeaderAlign: 'center', CellAlign: 'center' },
                      { HeaderText: labels.Delete, Width: 60, Name: "ColDelete", HeaderAlign: "center", CellAlign: "center" }],
            gridExpandHeight: 110,
            showDistinctRow: false,
            enableHiddenColumns: false,
            showTree: true,
            showTitle: true,
            titleInfo: {
                showButtonsWhenNoData: true,
                customButtons: '<div id="divAutoRefresh" class="auto-refresh"><span id="spanRefreshIcon" class="refresh-icon"><span class="refresh-value">' + labels.Refresh + '</span></span></div>'
            }
        };
        var bodyData = [];
        var row;
        var subRows;

        if (data && data.length > 0) {
            var deleteButtonFormat = '<a class="c hand ico-delete" href="JavaScript:void(0);" data-custid="{0}" data-username="{1}"></a>';

            $.each(data, function (groupIndex, group) {
                subRows = [];

                $.each(group.SMALossItems, function (index, item) {

                    smaCssClass = getChangeLevel(item.ChangedLevel);
                    counter++;

                    var smaNameFormat = '<span class="{0} level-indicator"></span>' +
                        (item.IsViewDownline ? '<span class="grid-collapse group-expand-indicator" group-id="{1}"></span>' : '') +
                        '<span>{2}</span>';

                    smaNameCol = smaNameFormat.format(smaCssClass, item.CustId, item.Username);
                    percentageCol = progressBarFormat.format(item.LossPercentage, numeral(item.LossPercentage).format(CMS.formats.IntegerFormat));

                    row = {
                        Columns: [{ Name: 'ColNo', Value: counter, CssClass: 'order-column' },
                                  { Name: 'ColUsername', Value: smaNameCol, TdClass: 'username-column' },
                                  { Name: 'ColLevel', Value: item.Level },
                                  { Name: 'ColCurrency', Value: item.Currency },
                                  { Name: 'ColYesterdayBalance', Value: item.YesterdayBalance },
                                  { Name: 'ColTotalBalance', Value: item.TotalBalance },
                                  {
                                      Name: 'ColMaxLoss', Value: getMaxLoss(item)
                                  },
                                  { Name: 'ColLossPercentage', Value: getPercentageCol(item) },
                                  {
                                      Name: "ColDelete",
                                      Value: deleteButtonFormat.format(item.CustId, item.Username)
                                  }
                        ],
                        CustomAttr: item.IsViewDownline ? 'row-group-id="' + item.CustId + '"' : ''
                    };
                    subRows.push(row);
                });

                row = {
                    Columns: [{
                        Name: 'ColNo',
                        Value: '{0} ({1})'.format(group.LossLevelRange, group.SMALossItems.length),
                        CellAlign: 'left',
                        MergeColNumber: 9
                    }],
                    SubRows: subRows,
                    CssClass: 'loss-level-' + group.LossLevel
                };
                bodyData.push(row);
            });
        }

        if (!viewResources.hasHostPermission) {
            gridOptions.hiddenColumns = [8];
        }

        gridOptions.bodyRows = bodyData;
        domElements.divSMALossMonitor.fanexGrid(gridOptions);
        handleGridEvents();
    };

    var handleGridEvents = function () {
        domElements.divSMALossMonitorContainer.find('#divAutoRefresh').click(function () {
            getSMALossReport();
        });

        domElements.divSMALossMonitor.find('[row-group-id] .username-column').click(function () {
            showHideDownlines($(this));
        });

        handleSMAEvents();

        domElements.divSMALossMonitor.fanexGrid('resizeGrid');
    };
    var handleSMAEvents = function () {
        if (viewResources.hasHostPermission) {
            var editIcons = domElements.divSMALossMonitor.find(".edit-maxloss");
            editIcons.on('click', function () {
                var $link = $(this);

                editMaxLoss($link);
            });

            var deleteIcons = domElements.divSMALossMonitor.find(".ico-delete");
            deleteIcons.on('click', function () {
                var $link = $(this);
                var custId = $link.attr('data-custid');
                var username = $link.attr('data-username');

                deleteSMA(username, custId);
            });
        }

        domElements.divSMALossMonitor.find('div.percentage-bar').each(function () {
            bindProgessBar($(this), notAvailablePercentage);
        });
    };

    var editMaxLoss = function (link) {
        var custId = link.attr('data-custid');
        var username = link.attr('data-username');
        var oldMaxLoss = parseInt(link.html().replace(/,/g, ""));

        cachedData.current = link;

        link.addClass("current-editing-row");
        link.addClass("display-none");

        var textboxEdit = link.next();
        textboxEdit.attr("align", "center");
        textboxEdit.removeClass('display-none');

        textboxEdit.focus();
        textboxEdit.blur(function () {
            $(this).val(numeral(oldMaxLoss).format(CMSConstants.NumberFormat.TwoDecimalNumber));
            completeEditMaxLoss(link);
        });

        textboxEdit.clearfield();
        textboxEdit.addClass('x-icon');

        textboxEdit.unbind("keypress");
        textboxEdit.keypress(function (e) {
            var allowInput = CMS.forceNegativeNumberInput(e);
            if (!allowInput) {
                return false;
            }

            textboxEdit.addClass('x-icon');
        });

        textboxEdit.unbind("keyup");
        textboxEdit.keyup(function (e) {
            var textboxValue = $(this).val();
            var maxLoss = parseInt(textboxValue.replace(/,/g, ""));

            formatMaxLossGrid(textboxEdit);

            cachedData.maxLoss = maxLoss;
            cachedData.oldMaxLoss = oldMaxLoss;

            var code = "";
            if (e.which) {
                code = e.which;
            }
            else if (e.keyCode) {
                code = e.keyCode;
            }

            if (code === keyboardManager.Enums.KeyCode.Escape) {
                $(this).val(numeral(oldMaxLoss).format(CMSConstants.NumberFormat.TwoDecimalNumber));

                completeEditMaxLoss(link);

                return;
            }

            if (code === keyboardManager.Enums.KeyCode.Enter) {
                cachedData.custId = custId;
                cachedData.username = username;

                CMS.showConfirmMessage(
                    messages.UpdateMaxLossConfirm.format(cachedData.username, numeral(cachedData.maxLoss).format(CMSConstants.NumberFormat.TwoDecimalNumber)),
                    function () {
                        if (isNaN(cachedData.maxLoss)) {
                            CMS.showInlineErrorMsg(messages.MaxLossNeedNegative, domElements.divResult);
                            return;
                        }

                        $.get(urls.EditMaxLoss, { custId: cachedData.custId, maxLoss: cachedData.maxLoss }).success(function (result) {
                            if (result.Code) {
                                CMS.showInlineErrorMsg(result.Message, domElements.divResult);
                                return;
                            }

                            CMS.showInlineSuccessMsg(messages.UpdateMaxLossSuccess, domElements.divResult, true);

                            cachedData.current.html(numeral(cachedData.maxLoss).format(CMSConstants.NumberFormat.TwoDecimalNumber));
                            var textboxEdit = cachedData.current.next();
                            textboxEdit.val(numeral(cachedData.maxLoss).format(CMSConstants.NumberFormat.TwoDecimalNumber));

                        });
                    });
                return;
            }
        });

        textboxEdit.on("paste", function (e) {
            setTimeout(function () {
                formatMaxLossGrid(textboxEdit);
            }, 100);
        });
    };
    var formatMaxLossGrid = function (textboxEdit) {
        var textboxValue = textboxEdit.val();
        var maxLoss = parseInt(textboxValue.replace(/,/g, ""));

        if (!isNaN(maxLoss)) {
            textboxEdit.val(numeral(maxLoss).format(CMSConstants.NumberFormat.IntNumber));
        }
    };
    var completeEditMaxLoss = function (link) {
        link.removeClass("current-editing-row");
        link.removeClass("display-none");

        var textboxEdit = link.next();
        textboxEdit.addClass('display-none');
    };

    var deleteSMA = function (username, custId) {
        cachedData.custId = custId;
        cachedData.username = username;

        CMS.showConfirmMessage(
            messages.RemoveSMA.format(cachedData.username),
            function () {
                CMS.disableInput();
                $.get(urls.DeleteSMA, { custId: cachedData.custId }).success(function (result) {
                    if (result.Code) {
                        CMS.enableInput();
                        CMS.showInlineErrorMsg(result.Message, domElements.divResult);
                        return;
                    }

                    CMS.showInlineSuccessMsg(messages.RemoveSMASuccess, domElements.divResult, true);

                    getSMALossReport();
                });
            });
    };

    var bindPopupEventhandlers = function () {
        popupElements.divForm.find("#radio-sma").click();
        popupElements.divForm.find('label[for]').click(function () {
            var $this = $(this);
            var forValue = $this.attr("for");
            var target = popupElements.divForm.find("#" + forValue);
            target.focus();
            target.click();
        });

        popupElements.divForm.find('[name="radio-level"]').click(function () {
            var $this = $(this);
            var value = $this.val().toLowerCase();

            popupElements.divForm.find(".lbl-maxloss").addClass("display-none");
            popupElements.divForm.find("#lbl-maxloss-" + value).removeClass("display-none");
        });

        popupElements.divForm.unbind("keyup");
        popupElements.divForm.keyup(function (e) {
            var code = "";
            if (e.which) {
                code = e.which;
            }
            else if (e.keyCode) {
                code = e.keyCode;
            }

            if (code === keyboardManager.Enums.KeyCode.Enter) {
                addNewSMA();
            }
        });

        popupElements.txtMaxLoss.blur(function () {
            formatMaxLoss();
        });

        popupElements.txtMaxLoss.unbind("keypress");
        popupElements.txtMaxLoss.keypress(function (e) {
            var allowInput = CMS.forceNegativeNumberInput(e);
            if (!allowInput) {
                return false;
            }
        });

        popupElements.txtMaxLoss.unbind("keyup");
        popupElements.txtMaxLoss.keyup(function (e) {
            setTimeout(function () {
                formatMaxLoss();
            }, 100);
        });

        popupElements.txtMaxLoss.on("paste", function (e) {
            setTimeout(function () {
                formatMaxLoss();
            }, 100);
        });

        popupElements.btnSave.click(function () {
            addNewSMA();
        });

        popupElements.btnCancel.click(function () {
            CMS.closePopup();
        });
    };

    var addNewSMA = function () {
        var username = popupElements.txtUsername.val();
        var maxLoss = popupElements.txtMaxLoss.val();
        var isHeadline = popupElements.divForm.find('[name="radio-level"]:checked').val().toLowerCase() === "headline";

        maxLoss = parseFloat(maxLoss.replace(/,/g, ""));

        if (Validator.validateForm(popupElements.divForm, 'w')) {
            if (isNaN(maxLoss)) {
                CMS.showInlineErrorMsg(messages.MaxLossNeedNegative, popupElements.divErrorSuccessMsg);
                return;
            }

            var params = {
                username: username,
                maxLoss: maxLoss,
                isHeadline: isHeadline
            };

            $.get(urls.AddSMA, params).success(function (result) {
                if (result.IsSuccessful) {
                    CMS.showInlineSuccessMsg(messages.AddSMASuccess, popupElements.divErrorSuccessMsg, false, function () {
                        setTimeout(CMS.closePopup, 3000);
                    });
                    disableRequestForm();
                    getSMALossReport();
                } else {
                    CMS.showInlineErrorMsg(result.Message, popupElements.divErrorSuccessMsg);
                }
            });
        }
    };

    var formatMaxLoss = function () {
        var textboxValue = popupElements.txtMaxLoss.val();
        var maxLoss = parseInt(textboxValue.replace(/,/g, ""));
        if (!isNaN(maxLoss)) {
            popupElements.txtMaxLoss.val(numeral(maxLoss).format(CMSConstants.NumberFormat.IntNumber));
        }
    };

    var disableRequestForm = function () {
        popupElements.txtUsername.prop('disabled', true);
        popupElements.txtMaxLoss.prop('disabled', true);
        popupElements.btnSave.prop('disabled', true);
        popupElements.btnCancel.prop('disabled', true);
    };

    var showHideDownlines = function (groupColumn) {
        var groupExpandIndicator = groupColumn.find(".group-expand-indicator");
        var groupdId = groupExpandIndicator.attr('group-id');
        var currentShow = groupColumn.attr("show-downline") === "true";

        if (currentShow) {
            domElements.divSMALossMonitor.find('.row-downline[row-group-id="' + groupdId + '"]').addClass("display-none");
            groupExpandIndicator.removeClass("grid-expand").addClass("grid-collapse");
        } else {
            groupExpandIndicator.removeClass("grid-collapse").addClass("grid-expand");

            var loadedDownlines = groupExpandIndicator.attr("loaded-downlines");
            if (loadedDownlines === "1") {
                domElements.divSMALossMonitor.find('.row-downline[row-group-id="' + groupdId + '"]').removeClass("display-none");
            }
            else {
                $.ajax({
                    type: 'POST',
                    url: urls.GetGroupDownlines,
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    data: JSON.stringify({ CustId: groupdId }),
                    success: function (result) {
                        renderDownlines(groupColumn, groupdId, result);
                        groupExpandIndicator.attr("loaded-downlines", 1);
                    },
                    error: function (xhr) {
                        CMS.handleAjaxError(xhr);
                    },
                    complete: function () {
                        CMS.enableInput();
                    }
                });
            }
        }

        groupColumn.attr("show-downline", currentShow ? "false" : "true");
        domElements.divSMALossMonitor.fanexGrid('resizeGrid');
    };

    var renderDownlines = function (groupColumn, groupdId, downlines) {
        var downlinesHtml = '';

        $.each(downlines, function (downlineIndex, downline) {
            downlinesHtml += '<tr class="highlight-row  divSMALossMonitor_tree_0 child-row row-downline" row-group-id="' + groupdId + '" highlightatt="highlight-att-main-0-0-0-0">\
                                    <td class="free-grid-td-0" style="height:21px;">\
                                        <div class="table-cell order-column" style="width:50px;padding-left:20px;" align="center"></div>\
                                    </td>\
                                    <td class="free-grid-td-1" style="height:21px;">\
                                        <div class="table-cell" style="width:250px;" align="left">\
                                                <span class="' + getChangeLevel(downline.ChangedLevel) + ' level-indicator"></span>\
                                                <span>' + downline.Username + '</span>\
                                        </div>\
                                    </td>\
                                    <td class="free-grid-td-2" style="height:21px;">\
                                        <div class="table-cell" style="width:70px;" align="center">' + downline.Level + '</div>\
                                    </td>\
                                    <td class="free-grid-td-3" style="height:21px;">\
                                        <div class="table-cell" style="width:70px;" align="center">' + downline.Currency + '</div>\
                                    </td>\
                                    <td class="free-grid-td-4" style="height:21px;">\
                                        <div class="table-cell" style="width:150px;" align="right">' + numeral(downline.YesterdayBalance).format(CMS.formats.DecimalFormat) + '</div>\
                                    </td>\
                                    <td class="free-grid-td-5" style="height:21px;">\
                                        <div class="table-cell" style="width:150px;" align="right">' + numeral(downline.TotalBalance).format(CMS.formats.DecimalFormat) + '</div>\
                                    </td>\
                                    <td class="free-grid-td-6" style="height:21px;">\
                                        <div class="table-cell" style="width:120px;" align="right">' + getMaxLoss(downline) + '</div>\
                                    </td>\
                                    <td class="free-grid-td-7" style="height:21px;">\
                                        <div class="table-cell" style="width:150px;" align="center">' + getPercentageCol(downline) + '</div>\
                                    </td>\
                                    <td class="free-grid-td-8 " style="height:21px;">\
                                        <div class="table-cell  " style="width:60px;" align="center">' +
                                            (downline.MaxLoss === null ? '' :
                                            '<a class="c hand ico-delete" href="JavaScript:void(0);" data-custid="' + downline.CustId + '" data-username="' + downline.Username + '"></a>') +
                                        '</div>\
                                    </td>\
                               </tr>';
        });

        var $downlinesHtml = $(downlinesHtml);

        $downlinesHtml.insertAfter(groupColumn.parent());

        handleSMAEvents();

        var addIcons = domElements.divSMALossMonitor.find(".add-maxloss");
        addIcons.on('click', function () {
            var $link = $(this);

            addMaxLoss($link);
        });
    };
    var getChangeLevel = function (changedLevel) {
        if (changedLevel > 0) {
            return 'ico-upward-arrow';
        }

        if (changedLevel < 0) {
            return 'ico-downward-arrow';
        }

        return 'ico-horizontal-line';
    };
    var getMaxLoss = function (item) {
        if (!viewResources.hasHostPermission && item.MaxLoss === null) {
            return maxLossFormatNoHost.format(labels.NotAvailable);
        }

        if (!viewResources.hasHostPermission) {
            return maxLossFormatNoHost.format(numeral(item.MaxLoss).format(CMSConstants.NumberFormat.TwoDecimalNumber));
        }

        if (item.MaxLoss === null) {
            return '<a class="c hand add-maxloss" href="JavaScript:void(0);" data-custid="' + item.CustId + '" data-username="' + item.Username + '">' + labels.NotAvailable + '</a>';
        }

        return maxLossFormat.format(item.CustId, item.Username,
            numeral(item.MaxLoss).format(CMSConstants.NumberFormat.TwoDecimalNumber),
            numeral(item.MaxLoss).format(CMSConstants.NumberFormat.TwoDecimalNumber));
    };
    var getPercentageCol = function (item) {
        if (item.MaxLoss === null) {
            return progressBarFormat.format(100, labels.NotAvailable);
        }

        return progressBarFormat.format(item.LossPercentage, numeral(item.LossPercentage).format(CMS.formats.IntegerFormat) + "%");
    };
    var bindProgessBar = function ($this, notAvailablePercentage) {
        var percentage = parseInt($this.attr('data-value'));

        $this.progressbar({
            value: percentage
        });

        if ($this.find('span').html().toLowerCase() === notAvailablePercentage) {
            $this.addClass('not-available-percents');
            return;
        }

        if (percentage >= 100) {
            $this.addClass('full-percents');
        }
    };
    var addMaxLoss = function (link) {
        var username = link.attr('data-username');

        var form = domElements.divAddSMA.clone();
        form.find("#txt-username").val(username);

        CMS.modal(labels.AddNewSetting, form, { width: 570, height: 220 });
        cachePopupElements();

        form.find("#txt-maxloss").focus();
        bindPopupEventhandlers();
    };

    $this.InitPage = function () {
        cacheDOM();
        initElements();
        bindEvents();
        getSMALossReport();
    };
};