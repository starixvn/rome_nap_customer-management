(function ($) {
    "use strict";
    /* AddSetting()
     * ==========
     * Add customer's setting to Loss Monitor page.
     *
     * @usage: var instance = new AddCustomer();
     *
     */
    function AddSetting(formatMaxLoss, reloadGridFunction) {
        var resultAddMessageDiv = null,
            smaRadio = null,
            headlineRadio = null,
            userNameTextbox = null,
            maxLossLabel = null,
            maxLossTextbox = null;

        function cacheDOM() {
            resultAddMessageDiv = $("#div-result-add-message");
            smaRadio = $("#rdoSMA");
            headlineRadio = $("#rdoHeadline");
            userNameTextbox = $("#txt-username");
            maxLossLabel = $("#lbl-maxloss");
            maxLossTextbox = $("#txt-maxloss");
        }

        function addSettingHandler(form) {
            CMS.disableInput();
            var addSettingParams = getAddSettingParams();

            if (!addSettingParams) {
                CMS.enableInput();
                return;
            }

            CMS.callAjax(
                this,
                form.action,
                addSettingParams,
                function (data) {
                    if (!data.IsSuccessful) {
                        CMS.showInlineErrorMsg(data.Message, resultAddMessageDiv);
                    }
                    else {
                        CMS.showInlineSuccessMsg(
                            messages.AddCustomerSuccess.format(addSettingParams.UserName),
                            resultAddMessageDiv,
                            true,
                            function () {
                                CMS.closePopup();
                                reloadGridFunction();
                            });
                    }
                });
        }

        function getAddSettingParams() {
            var isValid = true;
            var maxLoss;
            var userName = $.trim(userNameTextbox.val());
            var maxLossValue = $.trim(maxLossTextbox.val().replace(/,/g, ""));
            var notifyLetsTalk = $("#chk-letstalk-add").attr("checked") ? true : false;

            if (userName.length === 0) {
                CMS.showTipsyError(userNameTextbox, messages.Error_RequiredField, "w");
                isValid = false;
            } else {
                CMS.clearTipsyError(userNameTextbox);
            }

            if (maxLossValue.length === 0) {
                CMS.showTipsyError(maxLossTextbox, messages.Error_RequiredField, "w");
                isValid = false;
            } else if (!$.isNumeric(maxLossValue)) {
                CMS.showTipsyError(maxLossTextbox, messages.Error_InvalidValue, "w");
                isValid = false;
            } else {
                maxLoss = parseInt(maxLossValue);

                if (maxLoss > 0) {
                    CMS.showTipsyError(maxLossTextbox, messages.MaxLossNeedNegative, "se");
                    isValid = false;
                }
                else if (maxLoss < minIntValue) {
                    CMS.showTipsyError(maxLossTextbox, messages.Error_InvalidValue, "w");
                    isValid = false;
                } else {
                    CMS.clearTipsyError(maxLossTextbox);
                }
            }

            return !isValid ? null :
                {
                    UserName: userName,
                    MaxLoss: maxLoss,
                    IsGroupHeadline: headlineRadio.is(":checked"),
                    NotifyLetsTalk: notifyLetsTalk
                };
        }

        this.initialize = function (userName) {
            cacheDOM();

            smaRadio
                .off("change")
                .on("change", function () {
                    maxLossLabel.text(labels.MaxLoss);
                });

            headlineRadio
                .off("change")
                .on("change", function () {
                    maxLossLabel.text(labels.MaxLoss + " " + labels.RM);
                });

            maxLossTextbox
                .off()
                .on("keypress", CMS.forceNegativeNumberInput)
                .on("keyup", function () {
                    formatMaxLoss(this);
                });

            $("#formAddSetting")
                .off("submit")
                .on("submit", function (e) {
                    e.preventDefault();
                    addSettingHandler(this);
                });

            $("#btnCancel")
                .off("click")
                .on("click", CMS.closePopup);

            if (userName) {
                userNameTextbox.val(userName);
                maxLossTextbox.focus();
            } else {
                userNameTextbox.focus();
            }
        };
    }
    /* EditMaxLoss()
     * ==========
     * Edit customer's Max Loss.
     *
     * @usage: var instance = new EditMaxLoss();
     *
     */
    function EditMaxLoss() {
        var self = this;

        function inputMaxLoss_KeyUp(e) {
            var $this = $(this);
            var code = e.keyCode || e.which;

            if (code === keyboardManager.Enums.KeyCode.Escape) {
                escapeEditMaxLoss($this);
            } else if (code === keyboardManager.Enums.KeyCode.Enter) {
                var maxLossValue = $.trim($this.val().replace(/,/g, ""));

                if (maxLossValue.length !== 0 && $.isNumeric(maxLossValue)) {
                    var maxLoss = parseInt(maxLossValue);

                    if (minIntValue <= maxLoss && maxLoss <= 0) {
                        var custId = $this.data("custid");
                        var userName = $this.data("username");
                        var isGroupHeadline = $this.data("isgroupheadline");

                        CMS.showConfirmMessage(
                            messages.UpdateMaxLossConfirm.format(userName, numeral(maxLoss).format(CMSConstants.NumberFormat.TwoDecimalNumber)),
                            function () {
                                editMaxLossHandler(custId, maxLoss, isGroupHeadline);
                            });
                    }
                }
            } else {
                formatMaxLoss(this);
            }
        }

        function editMaxLossHandler(custId, maxLoss, isGroupHeadline) {
            CMS.disableInput();
            CMS.callAjax(
                self,
                rootUrl + "LossMonitor/Update",
                { CustId: custId, MaxLoss: maxLoss, IsGroupHeadline: isGroupHeadline },
                function (data) {
                    if (!data.IsSuccessful) {
                        CMS.showInlineErrorMsg(data.Message, resultMessageDiv);
                    }
                    else {
                        CMS.showInlineSuccessMsg(
                            messages.UpdateMaxLossSuccess,
                            resultMessageDiv,
                            true,
                            getSMALossReport);
                    }
                });
        }

        function escapeEditMaxLoss(inputTag) {
            var custId = inputTag.data("custid");

            inputTag.closest("#tr" + custId).removeClass("current-editing-row");
            inputTag.prev().removeClass("display-none");
            inputTag.remove();
        }

        self.inlineEditing = function (linkTag) {
            var custId = linkTag.data("custid");
            var text = linkTag.text();
            var inputHtml = [
                '<input type="text" class="clearable x-icon textbox-maxloss width-95" maxlength="13" value="',
                text.substring(0, text.indexOf(".")),
                '" data-custid="',
                custId,
                '" data-username="',
                linkTag.data("username"),
                '" data-isgroupheadline="',
                linkTag.data("isgroupheadline"),
                '" />'
            ].join("");

            linkTag.addClass("display-none");
            linkTag.closest("#tr" + custId).addClass("current-editing-row");
            linkTag.closest("#div" + custId).append(inputHtml);

            var $input = linkTag.next();
            $input
                .off()
                .on("keypress", CMS.forceNegativeNumberInput)
                .on("keyup", inputMaxLoss_KeyUp)
                .one("blur", function () {
                    escapeEditMaxLoss($(this));
                });
            $input.clearfield().focus();
        };
    }
    /* Loss Monitor.
     * ==========
     * ???
     *
     * @usage: ???
     *
     */
    var resultMessageDiv = $("#div-result-message"),
        divSMALossMonitor = $("#divSMALossMonitor"),
        addSettingForm = $("#formAddSetting");
    var gridOptions = null;

    function formatMaxLoss(inputTag) {
        var tagValue = inputTag.value.replace(/,/g, "");
        var maxLoss = parseInt(tagValue);

        if (maxLoss.toString() === tagValue) {
            inputTag.value = numeral(maxLoss).format(CMSConstants.NumberFormat.IntNumber);
        }
    }

    function toggleGroupDownlines() {
        var childDiv = $(this).children();
        var custId = childDiv.data("custid");
        var icon = childDiv.find("#icon" + custId);
        var groupDownlines = divSMALossMonitor.find(".group-downlines-" + custId);

        if (icon.hasClass("grid-expand")) {
            groupDownlines.addClass("display-none");
            divSMALossMonitor.fanexGrid("resizeGrid");
            icon.removeClass().addClass("grid-collapse");
        } else if (groupDownlines.length > 0) {
            groupDownlines.removeClass("display-none");
            divSMALossMonitor.fanexGrid("resizeGrid");
            icon.removeClass().addClass("grid-expand");
        } else {
            var groupHeadlineId = childDiv.data("groupheadlineid");
            CMS.getJson(
                rootUrl + "LossMonitor/GetGroupDownlines?GroupHeadlineId=" + groupHeadlineId,
                function (data) {
                    if (data.length > 0) {
                        var dataRow = $("#tr" + custId);
                        dataRow.after(buildGroupDownlines(data, custId, getTreeClass(dataRow.prop("class"))));
                        divSMALossMonitor.fanexGrid("resizeGrid");
                        initProgessBar();
                        icon.removeClass().addClass("grid-expand");

                        $(".chk-letstalk").off("click")
                            .on("click", function () {
                                var $this = $(this);
                                toggleLetsTalkCheckbox($this);
                            });
                    }
                });
        }
    }

    function getTreeClass(className) {
        var name;
        var classArray = className.split(" ");

        for (var i = 0, max = classArray.length; i < max; i++) {
            name = classArray[i];

            if (name.length !== 0 && name.indexOf("divSMALossMonitor_tree_") >= 0) {
                return name;
            }
        }

        return "";
    }

    function buildGroupDownlines(data, custId, className) {
        var html = [];
        $.each(data, function (i, item) {
            html.push('<tr class="highlight-row group-downlines-');
            html.push(custId);
            html.push(" ");
            html.push(className);
            html.push(' child-row" id="tr');
            html.push(item.CustId);
            html.push('">');
            html.push('<td class="free-grid-td-0" style="height:21px;"><div class="table-cell order-column" style="width:50px;padding-left:20px;" align="center"></div></td>');

            html.push('<td class="free-grid-td-1 username-column" style="height:21px;"><div class="table-cell" style="width:250px;" align="left" data-custid="');
            html.push(item.CustId);
            html.push('" data-groupheadlineid="')
            html.push(item.GroupHeadlineId);
            html.push('"><div class="username-column-child">');
            html.push(renderUserName(item, true));
            html.push("</div></div></td>");

            html.push('<td class="free-grid-td-2" style="height:21px;"><div class="table-cell" style="width:70px;" align="center">');
            html.push(item.RoleName);
            html.push("</div></td>");

            html.push('<td class="free-grid-td-3" style="height:21px;"><div class="table-cell" style="width:70px;" align="center">');
            html.push(item.Currency);
            html.push("</div></td>");

            html.push('<td class="free-grid-td-4" style="height:21px;"><div class="table-cell" style="width:150px;" align="right">');
            html.push(formatNegativeZero(numeral(item.YesterdayBalance).format(CMSConstants.NumberFormat.TwoDecimalNumber)));
            html.push("</div></td>");

            html.push('<td class="free-grid-td-5" style="height:21px;"><div class="table-cell" style="width:150px;" align="right">');
            html.push(formatNegativeZero(numeral(item.TotalBalance).format(CMSConstants.NumberFormat.TwoDecimalNumber)));
            html.push("</div></td>");

            html.push('<td class="free-grid-td-6" style="height:21px;"><div class="table-cell" style="width:120px;" align="right" id="div');
            html.push(item.CustId);
            html.push('">');
            html.push(renderMaxLoss(item));
            html.push("</div></td>");

            html.push('<td class="free-grid-td-7" style="height:21px;"><div class="table-cell" style="width:150px;" align="center">');
            html.push(renderPercentage(item));
            html.push("</div></td>");

            if (hasReadPermission) {
                html.push('<td class="free-grid-td-8" style="height:21px;"><div class="table-cell" style="width:80px;" align="center">');
                html.push(renderLetsTalk(item));
                html.push("</div></td>");
            }

            if (hasHostPermission) {
                html.push('<td class="free-grid-td-8" style="height:21px;"><div class="table-cell" style="width:60px;" align="center">');
                html.push(renderDelete(item, 2));
                html.push("</div></td>");
            }

            html.push('</tr>');
        });

        return html.join("");
    }

    function getSMALossReport() {
        CMS.hideInlineMsg(resultMessageDiv);
        CMS.getJson(
            rootUrl + "LossMonitor/GetSMALossReport",
            function (data) {
                gridOptions = gridOptions || getGridOptions();
                gridOptions.bodyRows = renderBodyRows(data);
                divSMALossMonitor.removeClass("display-none").fanexGrid(gridOptions);

                if (gridOptions.bodyRows.length > 0) {

                    initProgessBar();

                    divSMALossMonitor.find(".group-expand-indicator")
                        .off("click")
                        .on("click", toggleGroupDownlines);
                }

                if (hasHostPermission) {
                    $("#chk-letstalk-all").off("click").on("click", toggleLetsTalkAll);
                    $(".chk-letstalk").off("click")
                        .on("click", function () {
                            var $this = $(this);
                            toggleLetsTalkCheckbox($this);
                        });
                } else {
                    $("#chk-letstalk-all").attr("disabled", "disabled");
                }
            });
    }

    function toggleLetsTalkAll() {
        var isEnable = $("#chk-letstalk-all").attr("checked") ? true : false;

        // Revert status because confirm box not allow revert checkbox after cancel
        $("#chk-letstalk-all").attr("checked", isEnable ? null : "checked");

        var message = isEnable ? labels.EnableLetsTalkAll : labels.DisableLetsTalkAll;
        CMS.showConfirmMessage(message, function () {
            updateLetsTalk(0, isEnable, 0, function () {
                $("#chk-letstalk-all").attr("checked", isEnable ? "checked" : null);
                $(".chk-letstalk").attr("checked", isEnable ? "checked" : null);
                CMS.enableInput();
            });
        });
    }

    function toggleLetsTalkCheckbox(element) {
        var custId = element.data("custid");
        var isEnable = element.attr("checked") ? true : false;
        var isGroupHeadline = element.data("isgroupheadline");

        updateLetsTalk(custId, isEnable, isGroupHeadline, function () { });
    }

    function updateLetsTalk(custId, isEnable, isGroupHeadline, callback) {
        custId = custId;

        CMS.disableInput();
        CMS.callAjax(
            self,
            rootUrl + "LossMonitor/UpdateLetsTalkNotification",
            { CustId: custId, IsEnable: isEnable, IsGroupHeadline: isGroupHeadline },
            function (data) {
                if (!data.IsSuccessful) {
                    CMS.showInlineErrorMsg(data.Message, resultMessageDiv);

                    if (custId === 0) {
                        callback();
                    }
                }
                else {
                    CMS.hideInlineMsg(resultMessageDiv, callback);
                }
            });
    }

    function getGridOptions() {
        var headerColumns = [
            { HeaderText: labels.No, Width: 70, Name: "ColNo", HeaderAlign: "center", CellAlign: "center", ShowCollapseExpand: true },
            { HeaderText: labels.Username, Width: 250, Name: "ColUsername", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: labels.Level, Width: 70, Name: "ColLevel", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: labels.Currency, Width: 70, Name: "ColCurrency", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: labels.YesterdayBalance, Width: 150, Name: "ColYesterdayBalance", HeaderAlign: "center", CellAlign: "right" },
            { HeaderText: labels.TotalBalance, Width: 150, Name: "ColTotalBalance", HeaderAlign: "center", CellAlign: "right" },
            { HeaderText: labels.MaxLoss, Width: 120, Name: "ColMaxLoss", HeaderAlign: "center", CellAlign: "right" },
            { HeaderText: labels.LossPercentage, Width: 150, Name: "ColLossPercentage", HeaderAlign: "center", CellAlign: "center" }];

        if (hasReadPermission) {
            headerColumns.push({
                HeaderText: labels.LetsTalk + '<br /><input id="chk-letstalk-all" type="checkbox"' + (hasHostPermission ? '' : ' disabled=""disabled"') + ' checked="checked" />',
                Width: 80,
                Name: "ColLetsTalk",
                HeaderAlign: "center",
                CellAlign: "center"
            });
        }

        if (hasHostPermission) {
            headerColumns.push({ HeaderText: labels.Delete, Width: 60, Name: "ColDelete", HeaderAlign: "center", CellAlign: "center" });
        }

        return {
            columns: headerColumns,
            gridExpandHeight: 110,
            showDistinctRow: false,
            showTree: true
        };
    }

    function renderBodyRows(data) {
        var index = 0;
        var mergeColNumber = hasHostPermission ? 10 :
            hasReadPermission ? 9 : 8;
        var bodyData = [];

        if (data) {
            var row, subRows;
            $.each(data, function (i, group) {
                subRows = [];
                $.each(group.SMALossItems, function (j, item) {
                    row = {
                        Columns: [
                            { Name: "ColNo", Value: ++index, CssClass: "order-column" },
                            {
                                Name: "ColUsername",
                                Value: renderUserName(item),
                                TdClass: "username-column" + (item.GroupHeadlineId > 0 ? " group-expand-indicator" : ""),
                                CustomAttr: 'data-custid="' + item.CustId + '" data-groupheadlineid="' + item.GroupHeadlineId + '"'
                            },
                            { Name: "ColLevel", Value: item.RoleName },
                            { Name: "ColCurrency", Value: item.Currency },
                            { Name: "ColYesterdayBalance", Value: formatNegativeZero(numeral(item.YesterdayBalance).format(CMSConstants.NumberFormat.TwoDecimalNumber)) },
                            { Name: "ColTotalBalance", Value: formatNegativeZero(numeral(item.TotalBalance).format(CMSConstants.NumberFormat.TwoDecimalNumber)) },
                            { Name: "ColMaxLoss", Value: renderMaxLoss(item), CustomAttr: 'id="div' + item.CustId + '"' },
                            { Name: "ColLossPercentage", Value: renderPercentage(item) }
                        ], CustomAttr: 'id="tr' + item.CustId + '"'
                    };

                    if (hasReadPermission) {
                        row.Columns.push({ Name: "ColLetsTalk", Value: renderLetsTalk(item) });
                    }

                    if (hasHostPermission) {
                        row.Columns.push({ Name: "ColDelete", Value: renderDelete(item, item.GroupHeadlineId > 0 ? 1 : 0) });
                    }

                    subRows.push(row);
                });

                row = {
                    Columns: [{
                        Name: "ColNo",
                        Value: group.LossLevelRange + " (" + group.SMALossItems.length + ")",
                        CellAlign: "left",
                        MergeColNumber: mergeColNumber
                    }],
                    SubRows: subRows,
                    CssClass: "loss-level-" + group.LossLevel
                };

                bodyData.push(row);
            });
        }

        return bodyData;
    }

    function renderUserName(item) {
        var html = [];

        if (item.MaxLoss !== null) {
            html.push('<span class="');
            html.push(getUserNameClassName(item.Movement));
            html.push(' level-indicator"></span>');
        } else {
            html.push('<span style="display:inline-block;width:18px"></span>');
        }

        if (item.GroupHeadlineId > 0) {
            html.push('<span class="grid-collapse" id="icon');
            html.push(item.CustId);
            html.push('"></span>');
        }

        html.push("<span>");
        html.push(item.Username);
        html.push("</span>");

        return html.join("");
    }

    function getUserNameClassName(movement) {
        if (movement > 0) {
            return "ico-upward-arrow";
        }

        if (movement < 0) {
            return "ico-downward-arrow";
        }

        return "ico-horizontal-line";
    }

    function renderMaxLoss(item) {
        if (!hasHostPermission) {
            return [
                "<span>",
                item.MaxLoss === null ?
                labels.NotAvailable :
                formatNegativeZero(numeral(item.MaxLoss).format(CMSConstants.NumberFormat.TwoDecimalNumber)),
                "</span>"
            ].join("");
        }

        return [
            '<a class="c hand edit-maxloss" href="javascript:;" data-custid="',
            item.CustId,
            '" data-username="',
            item.Username,
            '" data-isgroupheadline="',
            item.GroupHeadlineId > 0,
            '">',
            item.MaxLoss === null ? labels.NotAvailable : formatNegativeZero(numeral(item.MaxLoss).format(CMSConstants.NumberFormat.TwoDecimalNumber)),
            "</a>"
        ].join("");
    }

    function renderPercentage(item) {
        if (item.MaxLoss === null) {
            return [
                '<div class="percentage-bar original" data-value="100"><div class="percentage-label"><span>',
                labels.NotAvailable,
                '<span></div></div>'
            ].join("");
        }

        return [
            '<div class="percentage-bar original" data-value="',
            item.PercentageLoss,
            '"><div class="percentage-label"><span>',
            numeral(item.PercentageLoss).format(CMS.formats.IntegerFormat),
            '%<span></div></div>'
        ].join("");
    }

    function renderLetsTalk(item) {
        return item.MaxLoss === null ?
            "" :
            '<input type="checkbox" class="c chk-letstalk" data-custid="' + item.CustId +
                '" data-username="' + item.Username + '" ' +
                'data-isgroupheadline="' + (item.GroupHeadlineId > 0) + '"' +
                (item.NotifyLetsTalk ? ' checked="checked"' : '') +
                (hasHostPermission ? '' : 'disabled="disabled"') + ' />';
    }

    function renderDelete(item, mode) {
        return item.MaxLoss === null ? "" : [
            '<a class="c hand ico-delete" href="javascript:;" data-custid="',
            item.CustId,
            '" data-username="',
            item.Username,
            '" data-mode="',
            mode,
            '"></a>'
        ].join("");
    }

    function initProgessBar() {
        divSMALossMonitor.find(".percentage-bar.original").each(function () {
            var $this = $(this);
            var percentage = parseInt($this.data("value"));
            $this.removeClass("original")
                .progressbar({ value: percentage });

            if ($this.find("span").text() === labels.NotAvailable) {
                $this.addClass("not-available-percents");
            } else if (percentage >= 100) {
                $this.addClass("full-percents");
            }
        });
    }

    function formatNegativeZero(value) {
        return value === "-0.00" ? "0.00" : value;
    }

    function initialize() {
        $("#btnRefresh").click(getSMALossReport).trigger("click");

        if (hasHostPermission) {
            var addHandler = new AddSetting(formatMaxLoss, getSMALossReport);
            $("#btnAddSetting").click(function () {
                CMS.modal(labels.AddNewSetting, addSettingForm.clone(), { width: 570, height: 280, resizable: false });
                addHandler.initialize();
            });

            divSMALossMonitor.on("click", ".ico-delete", function () {
                var $this = $(this);
                var custId = $this.data("custid");
                var userName = $this.data("username");
                var mode = $this.data("mode");

                var removeSetting = function () {
                    CMS.disableInput();
                    CMS.callAjax(
                        this,
                        rootUrl + "LossMonitor/Remove",
                        { CustId: custId, Mode: mode },
                        function (data) {
                            if (!data.IsSuccessful) {
                                CMS.showInlineErrorMsg(data.Message, resultMessageDiv);
                            } else {
                                CMS.showInlineSuccessMsg(
                                    messages.RemoveCustomerSuccess,
                                    resultMessageDiv,
                                    true,
                                    getSMALossReport);
                            }
                        });
                };

                CMS.showConfirmMessage(messages.RemoveCustomer.format(userName), removeSetting);
            });

            var editHandler = new EditMaxLoss(resultMessageDiv, getSMALossReport);
            divSMALossMonitor.on("click", ".edit-maxloss", function () {
                var $this = $(this);
                if ($this.text() === labels.NotAvailable) {
                    CMS.modal(labels.AddNewSetting, addSettingForm.clone(), { width: 570, height: 280, resizable: false });
                    addHandler.initialize($this.data("username"));
                } else {
                    editHandler.inlineEditing($this);
                }
            });
        }

        // Remove the #divPopupContent element since it was cached.
        // It also helps to prevent the 'for' attributes do not work correctly in the popup.
        $("#divPopupContent").empty();
    }

    initialize();
})(jQuery);
