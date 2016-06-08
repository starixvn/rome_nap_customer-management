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
        DeleteSMA: CMS.resolveRootUrl('LossMonitor/DeleteSMA')
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
        if (viewResources.hasHostPermission)
        {
            domElements.textboxSMAName.keyup(function (event) {
                if (event.keyCode === keyboardManager.Enums.KeyCode.Enter) {
                    self.btnSearchClick();
                }
            });

            if (!viewResources.hasHostPermission) {
                return;
            }

            domElements.btnAdd.unbind("click");
            domElements.btnAdd.click(btnAddClick);
        }
    };

    var btnAddClick = function () {
        var form = domElements.divAddSMA.clone();

        CMS.modal("Add New SMA", form, {width : 570, height: 220});
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
        var smaNameFormat = '<span class="{0} level-indicator"></span><span>{1}</span>';
        var progressBarFormat = '<div class="percentage-bar" data-value="{0}"><div class="percentage-label"><span>{1}%<span></div></div>';
        var smaNameCol = '';
        var percentageCol = '';
        var smaCssClass = '';

        var counter = 0;
        var gridOptions = {
            columns: [{ HeaderText: labels.No, Width: 70, Name: 'ColNo', HeaderAlign: 'center', CellAlign: 'center', ShowCollapseExpand: true },
                      { HeaderText: labels.SMAName, Width: 250, Name: 'ColSMAName', HeaderAlign: 'center', CellAlign: 'left' },
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
                customButtons: '<div id="divAutoRefresh"></div>'
            }
        };
        var bodyData = [];
        var row;
        var subRows;

        if (data && data.length > 0) {
            var maxLossFormat = '<a class="c hand edit-maxloss" href="JavaScript:void(0);" data-custid="{0}" data-username="{1}"" >{2}</a>' +
                                '<input class="display-none width-95 clearable textbox-maxloss" type="text" maxlength="13" value="{3}" />';

            var maxLossFormatNoHost = '<span>{0}</span>';

            var deleteButtonFormat = '<a class="c hand ico-delete" href="JavaScript:void(0);" data-custid="{0}" data-username="{1}"></a>';


            $.each(data, function (groupIndex, group) {
                subRows = [];

                $.each(group.SMALossItems, function (index, item) {
                    if (item.ChangedLevel > 0) {
                        smaCssClass = 'ico-upward-arrow';
                    } else if (item.ChangedLevel < 0) {
                        smaCssClass = 'ico-downward-arrow';
                    } else {
                        smaCssClass = 'ico-horizontal-line';
                    }

                    counter++;
                    smaNameCol = smaNameFormat.format(smaCssClass, item.Username);
                    percentageCol = progressBarFormat.format(item.LossPercentage, numeral(item.LossPercentage).format(CMS.formats.IntegerFormat));

                    row = {
                        Columns: [{ Name: 'ColNo', Value: counter, CssClass: 'order-column' },
                                  { Name: 'ColSMAName', Value: smaNameCol },
                                  { Name: 'ColLevel', Value: item.Level },
                                  { Name: 'ColCurrency', Value: item.Currency },
                                  { Name: 'ColYesterdayBalance', Value: item.YesterdayBalance },
                                  { Name: 'ColTotalBalance', Value: item.TotalBalance },
                                  {
                                      Name: 'ColMaxLoss', Value: viewResources.hasHostPermission ? 
                                          maxLossFormat.format(item.CustId, item.Username,
                                          numeral(item.MaxLoss).format(CMSConstants.NumberFormat.TwoDecimalNumber),
                                          numeral(item.MaxLoss).format(CMSConstants.NumberFormat.TwoDecimalNumber))
                                          :
                                          maxLossFormatNoHost.format(numeral(item.MaxLoss).format(CMSConstants.NumberFormat.TwoDecimalNumber))
                                  },
                                  { Name: 'ColLossPercentage', Value: percentageCol },
                                  {
                                      Name: "ColDelete",
                                      Value: deleteButtonFormat.format(item.CustId, item.Username)
                                  }
                        ],
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

        if (!viewResources.hasHostPermission)
        {
            gridOptions.hiddenColumns = [8];
        }

        gridOptions.bodyRows = bodyData;
        domElements.divSMALossMonitor.fanexGrid(gridOptions);
        handleGridEvents();
    };

    var handleGridEvents = function () {
        if (viewResources.hasHostPermission)
        {
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

        domElements.divSMALossMonitorContainer.find('#divAutoRefresh').each(function () {
            domElements.divAutoRefresh = $(this).autoRefresh(refreshTimer.Interval, refreshTimer.Default, getSMALossReport, true, false);

            if (domElements.divAutoRefresh && domElements.divAutoRefresh.run) {
                domElements.divAutoRefresh.run();
            }
        });

        domElements.divAutoRefresh.find('li.option').click(function () {
            var selectedValue = parseInt($(this).attr('data-value'));

            refreshTimer.Default = selectedValue;
        });

        domElements.divSMALossMonitor.find('div.percentage-bar').each(function () {
            var $obj = $(this);
            var percentage = parseInt($obj.attr('data-value'));

            $obj.progressbar({
                value: percentage
            });

            if (percentage >= 100) {
                $obj.addClass('full-percents');
            }
        });
        domElements.divSMALossMonitor.fanexGrid('resizeGrid');
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
                    "Are you sure you want to update max loss of " + cachedData.username + "?",
                    function () {
                        if (isNaN(cachedData.maxLoss))
                        {
                            CMS.showInlineErrorMsg("Max Loss should be a negative number!", domElements.divResult);
                            return;
                        }

                        $.get(urls.EditMaxLoss, { custId: cachedData.custId, maxLoss: cachedData.maxLoss }).success(function (result) {
                            if (result.Code) {
                                CMS.showInlineErrorMsg(result.Message, domElements.divResult);
                                return;
                            }

                            CMS.showInlineSuccessMsg("Updated Max Loss successfully.", domElements.divResult, true);

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
            "Are you sure you want to remove " + cachedData.username + "?",
            function () {
                CMS.disableInput();
                $.get(urls.DeleteSMA, { custId: cachedData.custId }).success(function (result) {
                    if (result.Code) {
                        CMS.showInlineErrorMsg(result.Message, domElements.divResult);
                        return;
                    }

                    CMS.showInlineSuccessMsg("Removed Max Loss successfully.", domElements.divResult, true);

                    getSMALossReport();
                });
            });
    };

    var bindPopupEventhandlers = function () {
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
            var username = popupElements.txtUsername.val();
            var maxLoss = popupElements.txtMaxLoss.val();
            maxLoss = parseFloat(maxLoss.replace(/,/g, ""));

            if (Validator.validateForm(popupElements.divForm, 'w')) {
                if (isNaN(maxLoss))
                {
                    CMS.showInlineErrorMsg("Max Loss should be a negative number!", popupElements.divErrorSuccessMsg);
                    return;
                }


                $.get(urls.AddSMA, { username: username, maxLoss: maxLoss }).success(function (result) {
                    if (result.IsSuccessful) {
                        CMS.showInlineSuccessMsg("Added SMA successfully.", popupElements.divErrorSuccessMsg, false, function () {
                            setTimeout(CMS.closePopup, 3000);
                        });
                        disableRequestForm();
                        getSMALossReport();
                    } else {
                        CMS.showInlineErrorMsg(result.Message, popupElements.divErrorSuccessMsg);
                    }
                });
            }
        });

        popupElements.btnCancel.click(function () {
            CMS.closePopup();
        });
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

    $this.InitPage = function () {
        cacheDOM();
        initElements();
        bindEvents();
        getSMALossReport();
    };
};