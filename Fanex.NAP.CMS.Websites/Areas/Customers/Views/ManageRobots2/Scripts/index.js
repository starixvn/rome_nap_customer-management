var ManageRobots2Index = function () {
    var self = this;
    this.elements = {
        txtUsername: null,
        divRobots: null,
        reportContainer: null,
        divAddRobot: null,
        divFromToDatePicker: null,

        formAddRobot: null,
        ddlLevelAddRobot: null,
        txtUsernameAddRobot: null,
        divAddResult: null,
        ddCurrency: null,
        ddCounter: null
    };

    this.searchParam = {
        pageSize: 50,
        pageIndex: 1,
        fromDate: null,
        toDate: null,
        sortColumnName: null,
        sortDirection: null
    };
    this.alreadyGetRobots = false;
    this.currentRecordsNumber = 0;

    this.init = function () {
        this.cachedElements();
        this.initFields();
        this.bindEvents();
        this.getSites();
    };

    this.cachedElements = function () {
        this.elements.txtUsername = $("#txt-username");
        this.elements.ddLevel = $("#ddl-level");
        this.elements.ddlSite = $("#ddl-site");
        this.elements.divRobots = $("#div-robots");
        this.elements.reportContainer = $("#report-container");
        this.elements.divAddRobot = $("#div-add-robot");
        this.elements.ddCurrency = $("#ddl-currency");
        this.elements.ddCounter = $("#ddl-counter");
        this.elements.divFromToDatePicker = $("#fromtodate-picker");
    };

    this.initFields = function () {
        this.elements.divFromToDatePicker.nexDatePicker({
            nexDatePickerType: "daterange",
            blankable: true,
            datePickerFormat: {
                dateFormat: CMS.formats.DateFormat,
                changeMonth: true,
                changeYear: true,
                showOtherMonths: true,
                selectOtherMonths: true,
                maxDate: resources.CurrentDate,
            },
            firstDatePicker: {
                Id: "dpk-fromdate",
                Label: resources.From_Date,
                LabelCss: "lbl-field lblFilter",
                CurrentDate: resources.CurrentDate,
                TextboxCss: "edit-field col_5"
            },
            secondDatePicker: {
                Id: "dpk-todate",
                Label: resources.To_Date,
                CurrentDate: resources.CurrentDate,
                LabelCss: "lbl-field lblFilter",
                TextboxCss: "edit-field col_5"
            },
        });
    }

    this.bindEvents = function () {
        $("#btn-search").on("click touchend", function () { self.enterSearch(); });
        this.elements.txtUsername.on("keyup", function (e) {
            var code = e.keyCode || e.which;
            if (code === keyboardManager.Enums.KeyCode.Enter) {
                self.enterSearch();
            }
        });

        $("#btn-add-robot").on("click touchend", function () { self.addRobot(); });
        $("#note-info").off("click touchend").on("click touchend", function () {
            CMS.popup(resources.RobotRulesTitle, $("#robot-rules-container").html(), { resizable: false });
        });
    };
    this.getSites = function () {
        CMS.disableInput();

        $.ajax({
            type: 'POST',
            url: CMS.resolveRootUrl('ManageRobots2/GetSites'),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: null,
            success: function (result) {
                self.renderSites(result);
            },
            error: function (xhr) {
                CMS.handleAjaxError(xhr);
            },
            complete: function () {
                CMS.enableInput();
            }
        });
    };
    this.renderSites = function (rs) {
        var sites = [];

        var data;
        var group = [];
        for (var i = 0, max = rs.Credits.length; i < max; i++) {
            data = rs.Credits[i];
            group.push({
                id: data.SiteId,
                value: data.SiteId,
                text: data.SiteName,
                checked: true,
                enable: true
            });
        }

        sites.push({
            id: 1,
            value: 1,
            text: resources.Credit,
            checked: true,
            enable: true,
            items: group
        });

        group = [];
        for (var i = 0, max = rs.Licensees.length; i < max; i++) {
            data = rs.Licensees[i];
            group.push({
                id: data.SiteId,
                value: data.SiteId,
                text: data.SiteName,
                checked: false,
                enable: true
            });
        }

        sites.push({
            id: 2,
            value: 2,
            text: resources.Licensee,
            checked: false,
            enable: true,
            items: group
        });

        sites.push({
            id: 2,
            value: 2,
            text: resources.NoSite,
            checked: true,
            enable: true,
            items: []
        });

        this.elements.ddlSite.fanexMultiSelect({
            minWidth: 140,
            checkAllText: resources.All,
            isCheckAll: false,
            listData: sites
        });
    };
    this.enterSearch = function () {
        var sites = this.elements.ddlSite.fanexMultiSelect('getSelectedTexts', true);
        if (sites.length && sites[sites.length - 1] === resources.NoSite) {
            sites[sites.length - 1] = "NULL";
        }

        var dateRange = this.elements.divFromToDatePicker.nexDatePicker("getDateRange");

        if ((dateRange.FromDate === "" ? 1 : 0) ^ (dateRange.ToDate === "" ? 1 : 0)) {
            dateRange.FromDate === "" ?
                CMS.showTipsyError($("#dpk-fromdate"), resources.MissingFilterDate, "s")
                : CMS.showTipsyError($("#dpk-todate"), resources.MissingFilterDate, "s");
            return;
        }

        CMS.clearTipsyError($("#dpk-fromdate"));
        CMS.clearTipsyError($("#dpk-todate"));

        self.searchParam = {
            username: this.elements.txtUsername.val().trim(),
            level: this.elements.ddLevel.val(),
            site: sites.join(','),
            pageSize: 50,
            pageIndex: 1,
            currency: this.elements.ddCurrency.val(),
            fromDate: dateRange.FromDate = "" ? null : dateRange.FromDate,
            toDate: dateRange.ToDate = "" ? null : dateRange.ToDate,
            sortColumnName: null,
            sortDirection: null
        };

        this.getRobots(self.searchParam);
    };
    this.getRobots = function (searchParam) {
        CMS.disableInput();
        CMS.closeMsgBox();

        $.ajax({
            type: 'POST',
            url: CMS.resolveRootUrl('ManageRobots2/GetRobots'),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: JSON.stringify(searchParam),
            success: function (result) {
                self.renderRobots(result);
            },
            error: function (xhr) {
                CMS.handleAjaxError(xhr);
            },
            complete: function () {
                CMS.enableInput();
            }
        });
    };
    this.renderRobots = function (rs) {
        this.alreadyGetRobots = true;
        if (rs.ResultCode) {
            CMS.showErrorMessage(rs.ResultMessage);
            return;
        }

        var gridOptions = {
            columns: [
                { HeaderText: "#", Width: 50, Name: 'ColOrder', HeaderAlign: 'center', CellAlign: 'center' },
                { HeaderText: resources.CustId, Width: 80, Name: 'ColCustId', HeaderAlign: 'center', CellAlign: 'left', AllowTruncate: true },
                { HeaderText: resources.Username, Width: 200, Name: 'ColUsername', HeaderAlign: 'center', CellAlign: 'left', AllowTruncate: true },
                { HeaderText: resources.Level, Width: 80, Name: 'ColLevel', HeaderAlign: 'center', CellAlign: 'center', AllowTruncate: true },
                { HeaderText: resources.Currency, Width: 80, Name: 'ColCurrency', HeaderAlign: 'center', CellAlign: 'center', AllowTruncate: true, Sort: true, SortDirection: self.getSortDirection("ColCurrency") },
                { HeaderText: resources.Site, Width: 80, Name: 'ColSite', HeaderAlign: 'center', CellAlign: 'left', AllowTruncate: true, Sort: true, SortDirection: self.getSortDirection("ColSite") },
                { HeaderText: resources.IP, Width: 120, Name: 'ColIP', HeaderAlign: 'center', CellAlign: 'left', AllowTruncate: true },
                { HeaderText: resources.Country, Width: 100, Name: 'ColCountry', HeaderAlign: 'center', CellAlign: 'center', AllowTruncate: true, Sort: true, SortDirection: self.getSortDirection("ColCountry") },
                { HeaderText: resources.Region, Width: 100, Name: 'ColRegion', HeaderAlign: 'center', CellAlign: 'center', AllowTruncate: true, Sort: true, SortDirection: self.getSortDirection("ColRegion") },
                { HeaderText: resources.LastChangeTime, Width: 140, Name: 'ColLastChangeTime', HeaderAlign: 'center', CellAlign: 'center', AllowTruncate: true, Sort: true, SortDirection: self.getSortDirection("ColLastChangeTime") },
                { HeaderText: resources.Counter, Width: 80, Name: 'ColCounter', HeaderAlign: 'center', CellAlign: 'center', AllowTruncate: true, Sort: true, SortDirection: self.getSortDirection("ColCounter") },
                { HeaderText: resources.ServerIP, Width: 80, Name: 'ColServerIP', HeaderAlign: 'center', CellAlign: 'left', AllowTruncate: true },
                { HeaderText: resources.AddedBy, Width: 140, Name: 'ColAddedBy', HeaderAlign: 'center', CellAlign: 'left', AllowTruncate: true },
                { HeaderText: resources.AddedDate, Width: 80, Name: 'ColAddedDate', HeaderAlign: 'center', CellAlign: 'center', AllowTruncate: true },
                { HeaderText: resources.Remark, Width: 300, Name: 'ColRemark', HeaderAlign: 'center', CellAlign: 'left', AllowTruncate: true },
                { HeaderText: resources.Del, Width: 60, Name: 'ColDel', HeaderAlign: 'center', CellAlign: 'center', AllowTruncate: true },
            ],
            showDistinctRow: false,
            showNumberColumn: true,
            showPager: true,
            customPager: true,
            showTitle: true,
            pagerOption: {
                itemsPerPage: rs.PageSize,
                currentPage: rs.PageIndex - 1,
                itemsPerPageArray: CMS.pagerRanges,
                pagerSelectCallBack: self.changePager
            },
            gridExpandHeight: 200,
            sortOption: {
                serverSortFunction: function (sortColumnName, sortDirection) {
                    self.searchParam.sortColumnName = sortColumnName;
                    self.searchParam.sortDirection = sortDirection;
                    self.searchParam.pageIndex = 1;
                    self.getRobots(self.searchParam);
                }
            },
            titleInfo: {
                text: "",
                showExcelButton: true,
                exportExcelFunction: function () {
                    CMS.downloadFile(CMS.resolveRootUrl("ManageRobots2/ExportRobots"), self.searchParam);
                }
            },
        };

        var bodyData = [];
        var row;
        for (var i = 0; i < rs.Robots.length; i++) {
            var robot = rs.Robots[i];
            row = {
                Columns: [{ Name: 'ColOrder', Value: (i + 1) },
                { Name: 'ColCustId', Value: robot.CustId },
                { Name: 'ColUsername', Value: robot.Username },
                { Name: 'ColLevel', Value: robot.Level },
                { Name: 'ColCurrency', Value: robot.Currency },
                { Name: 'ColSite', Value: robot.Site },
                { Name: 'ColIP', Value: robot.IP },
                { Name: 'ColCountry', Value: robot.Country },
                { Name: 'ColRegion', Value: robot.Region },
                { Name: 'ColLastChangeTime', Value: robot.LastChangeTimeStr },
                { Name: 'ColCounter', Value: robot.CounterStr },
                { Name: 'ColServerIP', Value: robot.ServerIP },
                { Name: 'ColAddedBy', Value: robot.AddedBy },
                { Name: 'ColAddedDate', Value: robot.AddedDateStr },
                { Name: 'ColRemark', Value: robot.Remark },
                { Name: 'ColDel', Value: "<span class='ico-delete' title='" + resources.Delete + "' data-cust-id='" + robot.CustId + "'></span>" }]
            };

            bodyData.push(row);
        }

        gridOptions.bodyRows = bodyData;
        gridOptions.pagerOption.totalItem = rs.TotalItems;
        this.currentRecordsNumber = rs.Robots.length;

        this.elements.divRobots.fanexGrid(gridOptions);

        this.elements.divRobots.find(".ico-delete").on("click touchend", function () {
            var custId = $(this).attr("data-cust-id");
            self.deleteRobot(custId);
        });

        this.elements.reportContainer.removeClass("display-none");
    };
    this.changePager = function (pageIndex, itemsPerPage) {
        self.searchParam.pageSize = itemsPerPage;
        self.searchParam.pageIndex = pageIndex + 1;
        self.getRobots(self.searchParam);
    };
    this.deleteRobot = function (custId) {
        CMS.showConfirmMessage(resources.DeleteConfirmation, function () {
            CMS.disableInput();
            CMS.closeMsgBox();

            $.ajax({
                type: 'POST',
                url: CMS.resolveRootUrl('ManageRobots2/DeleteRobot'),
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                data: JSON.stringify({ custId: custId }),
                success: function (rs) {
                    if (rs.Code) {
                        CMS.showErrorMessage(rs.Message);
                        return;
                    }

                    CMS.showSuccessMessage(resources.Message_DeleteSuccessfully);

                    self.currentRecordsNumber--;
                    if (self.currentRecordsNumber == 0) {
                        self.searchParam.pageIndex--;
                    }

                    if (self.searchParam.pageIndex == 0) {
                        self.searchParam.pageIndex == 1;
                    }

                    self.getRobots(self.searchParam);
                },
                error: function (xhr) {
                    CMS.handleAjaxError(xhr);
                },
                complete: function () {
                    CMS.enableInput();
                }
            });
        });
    };
    this.addRobot = function () {
        CMS.closeMsgBox();

        var form = this.elements.divAddRobot.clone();
        this.elements.formAddRobot = form;
        CMS.modal(resources.AddRobot, form, { resizable: false });
        form.find("#ddl-level-add-robot").addClass('combobox-autocomplete').combobox();

        this.elements.ddlLevelAddRobot = form.find("#ddl-level-add-robot.ui-combobox");
        this.elements.txtUsernameAddRobot = form.find("#txt-username-add-robot");
        this.elements.divAddResult = form.find("#div-add-result");

        this.elements.txtUsernameAddRobot.on("blur", function () {
            var usernamesValue = self.elements.txtUsernameAddRobot.val();
            usernamesValue = usernamesValue.replace(/\r\n/g, "\n").replace(/\n/g, ",");
            self.elements.txtUsernameAddRobot.val(usernamesValue);
        });

        form.find("#btn-cancel-add-robot").on("click touchend", function () {
            CMS.closePopup();
        });
        form.find("#btn-save-add-robot").on("click touchend", function () {
            self.validateAddRobot();
        });
    };
    this.validateAddRobot = function () {
        Validator.clearErrors(this.elements.formAddRobot);
        CMS.hideInlineMsg(self.elements.divAddResult);

        var level = this.elements.formAddRobot.find("#ddl-level-add-robot").val();
        if (level == 0) {
            Validator.showError(this.elements.ddlLevelAddRobot, resources.ThisFieldIsRequired, 'w');
            return;
        }

        var username = this.elements.txtUsernameAddRobot.val().trim();
        if (username == "") {
            Validator.showError(this.elements.txtUsernameAddRobot, resources.ThisFieldIsRequired, 'w');
            return;
        }

        if (username.indexOf("\n") != -1 || username.indexOf("\\") != -1) {
            Validator.showError(this.elements.txtUsernameAddRobot, resources.AddRobot_UsernameError, 'w');
            return;
        }

        if (username.split(",").length > 100) {
            Validator.showError(this.elements.txtUsernameAddRobot, resources.AddRobot_UsernameExceeds, 'w');
            return;
        }

        CMS.disableInput();

        var param = {
            Usernames: username,
            Remark: this.elements.formAddRobot.find("#txt-remark-add-robot").val().encode(),
            Level: level,
            ConfirmUpline: false
        };

        $.ajax({
            type: 'POST',
            url: CMS.resolveRootUrl('ManageRobots2/AddRobot'),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: JSON.stringify(param),
            success: function (rs) {
                if (rs.Code == 103) {
                    var message = rs.Message + "<br />" + resources.ProcceedConfirm;
                    CMS.showConfirmMessage(message, function () {
                        self.addRobotAfterValidate();
                    });

                    return;
                }

                if (rs.Code) {
                    CMS.showInlineErrorMsg(rs.Message, self.elements.divAddResult, false, 15000);
                    return;
                }

                CMS.showInlineSuccessMsg(resources.AddRobot_Success, self.elements.divAddResult, true);
                self.updateAfterAdd();
            },
            error: function (xhr) {
                CMS.handleAjaxError(xhr);
            },
            complete: function () {
                CMS.enableInput();
            }
        });
    };
    this.addRobotAfterValidate = function () {
        CMS.disableInput();
        var param = {
            Usernames: this.elements.txtUsernameAddRobot.val().trim(),
            Remark: this.elements.formAddRobot.find("#txt-remark-add-robot").val(),
            Level: this.elements.formAddRobot.find("#ddl-level-add-robot").val(),
            ConfirmUpline: true
        };

        $.ajax({
            type: 'POST',
            url: CMS.resolveRootUrl('ManageRobots2/AddRobot'),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: JSON.stringify(param),
            success: function (rs) {
                if (rs.Code) {
                    CMS.showInlineErrorMsg(rs.Message, self.elements.divAddResult, false, 15000);
                    return;
                }

                CMS.showInlineSuccessMsg(resources.AddRobot_Success, self.elements.divAddResult, true);
                self.updateAfterAdd();
            },
            error: function (xhr) {
                CMS.handleAjaxError(xhr);
            },
            complete: function () {
                CMS.enableInput();
            }
        });
    };
    this.updateAfterAdd = function () {
        if (self.alreadyGetRobots) {
            self.getRobots(self.searchParam);
        }
    };
    this.getSortDirection = function (columnName) {
        return this.searchParam.sortColumnName == columnName ? this.searchParam.sortDirection : null;
    };
};

$(document).ready(function () {
    var manageRobots2Index = new ManageRobots2Index();

    manageRobots2Index.init();
});