/* File Created: Oct 03, 2014 */
/* global viewResources,
Namespace,
minDate,
currentDate,
CMS,
defaultPercentageUsed,
CMSConstants,
numeral,
popUpManager,
maxTurnoverValue,
keyboardManager */
/* jshint strict: false,
camelcase: false,
unused: vars */
var turnoverMonitorManager;
$(document).ready(function () {
    turnoverMonitorManager.initPage();
});

Namespace.Register("Fanex.Nap.CMS.TurnoverControl.TurnoverMonitorManager");
Fanex.Nap.CMS.TurnoverControl.TurnoverMonitorManager = function () {
    var self = this;
    self.domElements = {
        btnSearch: null,
        divResult: null,
        ddlSport: null,
        ddlLeague: null,
        txtPercentageUsed: null,

        dpkFromDate: null,
        dpkToDate: null
    };
    self.configurations = {
        loadSportsLink: CMS.resolveRootUrl('/../../TurnoverMonitor/Sports'),
        loadLeaguesLink: CMS.resolveRootUrl('/../../TurnoverMonitor/Leagues'),
        getSMATurnoverListLink: CMS.resolveRootUrl('/../../TurnoverMonitor/SMATurnoverList'),
        viewCompanyTurnoverBalanceLink: CMS.resolveRootUrl('/../../TurnoverMonitor/OpenCompanyTurnoverView'),
        updateMaxTurnoverLink: CMS.resolveRootUrl('/../../TurnoverMonitor/UpdateCustomerMaxTurnoverByMatch'),
        css: {
            displayNone: 'display-none'
        },
        maxTurnoverPercentage: 100
    };
    self.params = {
        getLeagues: {
            FromDate: null,
            ToDate: null,
            SportId: 0
        },
        getSMATurnoverList: {
            FromDate: null,
            ToDate: null,
            SportId: 0,
            LeagueId: 0,
            PercentageUsed: 0
        },
        updateMaxTurnover: {
            MatchId: 0,
            CustId: 0,
            MaxTurnover: 0
        }
    };
    self.cachedData = {
        refreshIntervalCounter: 0,
        countdownTime: 0,
        refreshIntervalEvent: null,
        smaTurnoverList: null,
        editingRowIndex: null
    };

    self.initPage = function () {
        self.cacheDOMElements();
        self.initElements();
        self.bindEvents();
        self.loadSports();
    };
    self.cacheDOMElements = function () {
        self.domElements.btnSearch = $('#btn-search');
        self.domElements.divResult = $("#div-result-message");

        self.domElements.ddlSport = $('#ddl-sport');
        self.domElements.ddlLeague = $('#ddl-league');
        self.domElements.txtPercentageUsed = $('#txt-percentageused');

        self.domElements.dpkFromDate = $('#dpk-fromdate');
        self.domElements.dpkToDate = $('#dpk-todate');

        self.domElements.smaTurnoverContainer = $('#sma-turnover-container');
        self.domElements.tblSMATurnoverBody = $("#sma-turnover-content tbody");

        self.domElements.maxLevelUsedBox = $('.max-level-used-box');
        self.domElements.lblMaxLevelUsedCounter = $('.lbl-max-level-used-counter');
        self.domElements.refreshBox = $('.refreshBox');
        self.domElements.gridTitle = $('#div-smaturnover-grid-title');
    };
    self.initElements = function () {
        $("#fromtodate-picker").nexDatePicker({
            nexDatePickerType: 'daterange',
            datePickerFormat: {
                dateFormat: CMS.formats.DateFormat,
                changeMonth: true,
                changeYear: true,
                showOtherMonths: true,
                selectOtherMonths: true,
                minDate: new Date(minDate)
            },
            firstDatePicker: {
                Id: 'dpk-fromdate',
                Label: viewResources.From_Date,
                InitDate: new Date(currentDate),
                LabelCss: 'lbl-field col_3',
                TextboxCss: 'edit-field col_7'
            },
            secondDatePicker: {
                Id: 'dpk-todate',
                Label: viewResources.To_Date,
                InitDate: new Date(currentDate),
                CurrentDate: new Date(currentDate),
                LabelCss: 'lbl-field col_3',
                TextboxCss: 'edit-field col_7'
            },
            onSelectDatePicker: function (selectedDate) {
                self.loadSports();
            }
        });

        self.domElements.datePickerPlugin = $("#fromtodate-picker");
        self.domElements.txtPercentageUsed.val(defaultPercentageUsed);
        self.domElements.smaTurnoverContainer.initGrid({
            isShowPager: false
        });
    };
    self.bindEvents = function () {
        self.domElements.btnSearch.unbind("click");
        self.domElements.btnSearch.click(self.btnSearchClick);

        var refreshIcon = $(".refreshBox .refreshIcon");
        refreshIcon.unbind("click");
        refreshIcon.click(function () {
            if (self.cachedData.refreshIntervalCounter > 1) {
                var refreshIcon = $(".refreshBox .refreshIcon");
                self.cachedData.countdownTime = self.cachedData.refreshIntervalCounter / 1000;
                window.clearInterval(self.cachedData.refreshIntervalEvent);
                self.cachedData.refreshIntervalEvent = setInterval(function () {
                    self.cachedData.countdownTime = self.cachedData.refreshIntervalCounter / 1000;
                    self.refreshSMATurnoverList();
                }, self.cachedData.refreshIntervalCounter);

                refreshIcon.html(self.cachedData.countdownTime);
                self.refreshSMATurnoverList();
            }
        });

        var refreshDropdownIcon = $(".refreshBox .refreshDropdownIcon");
        refreshDropdownIcon.unbind("click");
        refreshDropdownIcon.click(function () {
            self.showCountDownOptions($(this));
        });

        var countDownOptions = $(".countDownOptionContainer .countDownOption");
        countDownOptions.unbind("click");
        countDownOptions.click(function () {
            self.saveCountDownRefreshOption(this);
        });
    };
    self.loadSports = function () {
        CMS.disableInput();
        CMS.callAjax(self, self.configurations.loadSportsLink, {}, self.renderSportList);
    };
    self.renderSportList = function (result) {
        var options = '';
        var sportLength = result ? result.length : 0;
        for (var i = 0; i < sportLength; i++) {
            var sport = result[i];
            options += '<option value="' + sport.Id + '"' +
                (i === 0 ? ' selected="selected"' : '') +
                ' >' + sport.Name + '</option>';
        }

        self.domElements.ddlSport.empty().append(options);
        if (sportLength) {
            $("#ddl-sport.ui-combobox input").val(result[0].Name);
        }

        self.domElements.ddlSport.change(function () {
            self.loadLeagues();
        });
        self.loadLeagues();
    };
    self.loadLeagues = function () {
        CMS.disableInput();
        var dateRange = self.domElements.datePickerPlugin.nexDatePicker("getDateRange");
        self.params.getLeagues = {
            FromDate: dateRange.FromDate,
            ToDate: dateRange.ToDate,
            SportId: self.domElements.ddlSport.val()
        };

        CMS.callAjax(self, self.configurations.loadLeaguesLink, self.params.getLeagues, self.renderLeagueList);
    };
    self.renderLeagueList = function (result) {
        var options = '';
        var leagueLength = result ? result.length : 0;
        options += '<option value="' + CMSConstants.LeagueId.All + '" selected="selected" >' +
                viewResources.All_Leagues +
            '</option>';
        for (var i = 0; i < leagueLength; i++) {
            var league = result[i];
            options += '<option value="' + league.Id + '">' + league.Name + '</option>';
        }

        self.domElements.ddlLeague.empty().append(options);
        $("#ddl-league.ui-combobox input").val(viewResources.All_Leagues);
    };
    self.btnSearchClick = function () {
        CMS.disableInput();
        self.params.getSMATurnoverList = {
            FromDate: self.params.getLeagues.FromDate,
            PercentageUsed: self.domElements.txtPercentageUsed.val(),
            ToDate: self.params.getLeagues.ToDate,
            SportId: self.params.getLeagues.SportId,
            LeagueId: self.domElements.ddlLeague.val()
            //FromDate: '09/13/2014',
            //PercentageUsed: 0
        };

        CMS.callAjax(self, self.configurations.getSMATurnoverListLink, self.params.getSMATurnoverList, self.renderSMATurnoverList);
    };
    self.renderSMATurnoverList = function (result) {
        self.cachedData.smaTurnoverList = result;
        var leagueLength = result ? result.ChildTurnoverInfo.length : 0;
        var html = '';
        var reachedMaxPercentageCount = 0;
        if (leagueLength === 0) {
            html += self.renderSMATurnoverList_NoData();
        } else {
            for (var i = 0; i < leagueLength; i++) {
                var league = result.ChildTurnoverInfo[i];
                var matchLength = league.ChildTurnoverInfo.length;
                for (var j = 0; j < matchLength; j++) {
                    var match = league.ChildTurnoverInfo[j];
                    html += self.renderSMATurnoverList_MatchHeadline(league, match);
                    var renderResult = self.renderSMATurnoverList_Match(i, j, match);
                    reachedMaxPercentageCount += renderResult.reachedMaxPercentageCount;
                    html += renderResult.html;
                }
            }
        }

        self.domElements.lblMaxLevelUsedCounter.html(numeral(reachedMaxPercentageCount).format(CMSConstants.NumberFormat.IntNumber));
        self.domElements.gridTitle.removeClass(self.configurations.css.displayNone);

        self.domElements.tblSMATurnoverBody.empty().append(html);
        self.renderSMATurnoverList_ApplyOptions();
        self.renderSMATurnoverList_BindEvent();
    };

    self.renderSMATurnoverList_MatchHeadline = function (league, match) {
        var html = '<tr>\
                        <td class="rowLeague" colspan="7">\
                            <table class="normalTable">\
                                <tr>\
                                    <td>\
                                        <div class="lblLeagueHeadline">' + league.Name + '</div>\
                                        <div class="lblTeamInfo">' +
                                            match.TurnoverInfo.MatchInfo.HomeTeam.Name +
                                            ' -vs- ' +
                                            match.TurnoverInfo.MatchInfo.AwayTeam.Name +
                                        '</div>\
                                        <div class="rightArrow_LeagueInfo"></div>\
                                        <div class="eventDate_LeagueInfo">' +
                                            viewResources.Event_Date + ': ' +
                                            match.TurnoverInfo.MatchInfo.EventDateString +
                                        '</div>\
                                    </td>\
                                    <td class="width-160">\
                                        <div>&nbsp;</div>\
                                    </td>\
                                </tr>\
                            </table>\
                        </td>\
                    </tr>';
        return html;
    };
    self.renderSMATurnoverList_NoData = function () {
        var html = '<tr>\
                        <td colspan="7">\
                            <div class="align-center no-data-not-frozen">' + viewResources.Message_NoReportData + '</div>\
                        </td>\
                     </tr>';
        return html;
    };
    self.renderSMATurnoverList_Match = function (leagueIndex, matchIndex, match) {
        var html = '';
        var reachedMaxPercentageCount = 0;
        var customerLength = match.ChildTurnoverInfo.length;
        for (var k = 0; k < customerLength; k++) {
            var rowIndex = ' league-index="' + leagueIndex + '"' +
                ' match-index="' + matchIndex + '"' +
                ' customer-index="' + k + '"';
            var customerTurnoverInfo = match.ChildTurnoverInfo[k].TurnoverInfo;
            if (customerTurnoverInfo.PercentageUsed >= self.configurations.maxTurnoverPercentage) {
                reachedMaxPercentageCount++;
            }

            html += '<tr class="row-customer-turnover-info"' + rowIndex + '>\
                        <td>\
                            <div class="col-sma-name-body align-left">' + customerTurnoverInfo.Name + '</div>\
                        </td>\
                        <td>\
                            <div class="col-level-body align-center">' + customerTurnoverInfo.RoleInfo.Name + '</div>\
                        </td>\
                        <td>\
                            <div class="col-currency-body align-center">' + customerTurnoverInfo.Currency.Name + '</div>\
                        </td>\
                        <td>\
                            <div class="col-member-turnover-body align-right">' +
                                numeral(customerTurnoverInfo.MemberTurnover).format(CMSConstants.NumberFormat.TwoDecimalNumber) +
                            '</div>\
                        </td>\
                        <td>\
                            <div class="col-company-turnover-body align-right">' +
                                '<a class="c hand" href="JavaScript:void(0);"' + rowIndex + '>' +
                                    numeral(customerTurnoverInfo.CompanyTurnover).format(CMSConstants.NumberFormat.TwoDecimalNumber) +
                                '</a>\
                            </div>\
                        </td>';

            var canEditMaxTurnover = viewResources.hasHostPermission &&
                match.TurnoverInfo.MatchInfo.EventStatus !== "completed";
            if (canEditMaxTurnover) {
                html += '<td class="td-company-turnover-body"' + rowIndex + '>\
                            <div class="col-max-turnover-body align-right">' +
                                '<a class="c hand" href="JavaScript:void(0);"' + rowIndex + '>' +
                                    numeral(customerTurnoverInfo.MaxTurnover).format(CMSConstants.NumberFormat.TwoDecimalNumber) +
                                '</a>\
                                <span class="ico-edit" title="' + viewResources.Edit + '"' + rowIndex + '></span>\
                            </div>\
                        </td>';
            }
            else {
                html += '<td class="td-maxturnover">\
                            <div class="col-max-turnover-body align-right table-cell">' +
                                numeral(customerTurnoverInfo.MaxTurnover).format(CMSConstants.NumberFormat.TwoDecimalNumber) +
                            '</div>\
                        </td>';
            }

            var percentageUsed = Math.round(customerTurnoverInfo.PercentageUsed);
            if (percentageUsed === self.configurations.maxTurnoverPercentage) {
                html += '<td>\
                            <div class="col-percentage-used-body align-center boxPercentUsed_100Percent">' + percentageUsed + '%</div>\
                        </td>';
            } else {
                html += '<td>\
                            <div class="col-percentage-used-body align-center boxPercentUsed_Low boxPercentUsed" style="background-position: -' +
                                (100 - percentageUsed) + 'px">' + percentageUsed + '%</div>\
                        </td>';
            }
        }

        return {
            html: html,
            reachedMaxPercentageCount: reachedMaxPercentageCount
        };
    };
    self.showReachedMaxPercentageUsedBox = function (reachedMaxPercentageCount) {
        self.domElements.lblMaxLevelUsedCounter.html(numeral(reachedMaxPercentageCount).format(CMSConstants.NumberFormat.IntNumber));
        self.domElements.maxLevelUsedBox.removeClass(self.configurations.css.displayNone);
    };

    self.renderSMATurnoverList_ApplyOptions = function () {
        var gridOpts = {
            tblContentId: 'sma-turnover-content',
            manualCopyHeader: true,
            fullWidth: true,

            // [200, 60, 80, 140, 140, 150, 160]
            gridWidth: 925,
            gridHeight: 250,
            freezeWidth: 10,
            numberFreezeCol: 0
        };

        self.domElements.smaTurnoverContainer.removeClass(self.configurations.css.displayNone)
            .gridScroller(gridOpts);
    };
    self.renderSMATurnoverList_BindEvent = function () {
        self.domElements.smaTurnoverContainer.find(".col-company-turnover-body a").click(function () {
            var $this = $(this);
            var leagueIndex = $this.attr("league-index");
            var matchIndex = $this.attr("match-index");
            var customerIndex = $this.attr("customer-index");
            self.viewCompanyTurnoverBalance(leagueIndex, matchIndex, customerIndex);
        });

        var colMaxTurnovers = self.domElements.smaTurnoverContainer.find(".col-max-turnover-body");
        colMaxTurnovers.find("a").click(function () {
            var $this = $(this);
            var leagueIndex = $this.attr("league-index");
            var matchIndex = $this.attr("match-index");
            var customerIndex = $this.attr("customer-index");
            self.showEditMaxTurnover(leagueIndex, matchIndex, customerIndex);
        });
        colMaxTurnovers.find(".ico-edit").click(function () {
            var $this = $(this);
            var leagueIndex = $this.attr("league-index");
            var matchIndex = $this.attr("match-index");
            var customerIndex = $this.attr("customer-index");
            self.showEditMaxTurnover(leagueIndex, matchIndex, customerIndex);
        });
    };

    self.showEditMaxTurnover = function (leagueIndex, matchIndex, customerIndex) {
        var cssRowFilter = '[league-index="' + leagueIndex + '"]' +
            '[match-index="' + matchIndex + '"]' +
            '[customer-index="' + customerIndex + '"]';
        var customerRow = self.domElements.smaTurnoverContainer.find('.row-customer-turnover-info' + cssRowFilter);
        customerRow.addClass("current-editing-row");

        var colMaxTurnover = self.domElements.smaTurnoverContainer.find(".td-company-turnover-body" + cssRowFilter);
        var rowIndex = ' league-index="' + leagueIndex + '"' +
                ' match-index="' + matchIndex + '"' +
                ' customer-index="' + customerIndex + '"';
        var customerTurnoverInfo = self.cachedData.smaTurnoverList.ChildTurnoverInfo[leagueIndex].
            ChildTurnoverInfo[matchIndex].
            ChildTurnoverInfo[customerIndex].TurnoverInfo;
        colMaxTurnover.find("a").addClass(self.configurations.css.displayNone);
        colMaxTurnover.find(".ico-edit").addClass(self.configurations.css.displayNone);

        var divMaxTurnover = colMaxTurnover.find(".col-max-turnover-body");
        divMaxTurnover.removeClass("align-right").addClass("align-center");
        divMaxTurnover.append('<input type="text"' + rowIndex +
            ' value="' +
                numeral(customerTurnoverInfo.MaxTurnover).format(CMSConstants.NumberFormat.TwoDecimalNumber) + '"' +
            ' maxlength="16" class="align-right textbox-maxturnover" />');

        var textboxMaxTurnover = divMaxTurnover.find("input");
        self.bindTextboxMaxTurnoverEvents(textboxMaxTurnover, customerTurnoverInfo, leagueIndex, matchIndex, customerIndex);
    };
    self.bindTextboxMaxTurnoverEvents = function (textboxMaxTurnover, customerTurnoverInfo,
        leagueIndex, matchIndex, customerIndex) {

        textboxMaxTurnover.focus();
        textboxMaxTurnover.blur(function () {
            self.completeEditMaxTurnover(leagueIndex, matchIndex, customerIndex);
        });

        textboxMaxTurnover.clearfield();
        textboxMaxTurnover.addClass('x-icon');

        textboxMaxTurnover.unbind("keypress");
        textboxMaxTurnover.keypress(function (e) {
            var allowInput = CMS.forceNumberInput(e);
            if (!allowInput) {
                return false;
            }

            textboxMaxTurnover.addClass('x-icon');
        });

        textboxMaxTurnover.unbind("keyup");
        textboxMaxTurnover.keyup(function (e) {
            var textboxValue = $(this).val();
            var maxTurnover = parseInt(textboxValue.replace(/,/g, ""));
            var code = "";
            if (e.which) {
                code = e.which;
            }
            else if (e.keyCode) {
                code = e.keyCode;
            }

            if (code === keyboardManager.Enums.KeyCode.Escape) {
                self.completeEditMaxTurnover(leagueIndex, matchIndex, customerIndex);
                return;
            }

            if (code === keyboardManager.Enums.KeyCode.Enter) {
                CMS.showConfirmMessage(
                    viewResources.GlobalSetting_EditMaxTurnover_Confirm.format(customerTurnoverInfo.Name),
                    function () {
                        self.updateMaxTurnover(leagueIndex, matchIndex, customerIndex, maxTurnover);
                    });
                return;
            }

            self.formatMaxTurnover(textboxMaxTurnover);
        });

        textboxMaxTurnover.on("paste", function (e) {
            setTimeout(function () {
                self.formatMaxTurnover(textboxMaxTurnover);
            }, 100);
        });
    };
    self.formatMaxTurnover = function (textboxEdit) {
        var textboxValue = textboxEdit.val();
        var maxTurnover = parseInt(textboxValue.replace(/,/g, ""));
        if (maxTurnover.toString() === textboxValue.replace(/,/g, "")) {
            textboxEdit.val(numeral(maxTurnover).format(CMSConstants.NumberFormat.IntNumber));
        }
    };
    self.completeEditMaxTurnover = function (leagueIndex, matchIndex, customerIndex) {
        var cssRowFilter = '[league-index="' + leagueIndex + '"]' +
            '[match-index="' + matchIndex + '"]' +
            '[customer-index="' + customerIndex + '"]';
        var customerRow = self.domElements.smaTurnoverContainer.find('.row-customer-turnover-info' + cssRowFilter);
        customerRow.removeClass("current-editing-row");

        var colMaxTurnover = self.domElements.smaTurnoverContainer.find(".td-company-turnover-body" + cssRowFilter);

        var divMaxTurnover = colMaxTurnover.find(".col-max-turnover-body");
        divMaxTurnover.removeClass("align-center").addClass("align-right");
        divMaxTurnover.find("input").remove();
        colMaxTurnover.find("a").removeClass(self.configurations.css.displayNone);
        colMaxTurnover.find(".ico-edit").removeClass(self.configurations.css.displayNone);
    };
    self.updateMaxTurnover = function (leagueIndex, matchIndex, customerIndex, maxTurnover) {
        CMS.disableInput();
        self.cachedData.editingRowIndex = {
            leagueIndex: leagueIndex,
            matchIndex: matchIndex,
            customerIndex: customerIndex
        };

        var customerTurnoverInfo = self.cachedData.smaTurnoverList.ChildTurnoverInfo[leagueIndex].
            ChildTurnoverInfo[matchIndex].
            ChildTurnoverInfo[customerIndex].TurnoverInfo;
        self.params.updateMaxTurnover = {
            CustId: customerTurnoverInfo.Id,
            MatchId: customerTurnoverInfo.MatchInfo.Id,
            MaxTurnover: maxTurnover
        };

        if (!self.checkUpdateMaxTurnoverParam(self.params.updateMaxTurnover)) {
            CMS.enableInput();
            return;
        }

        CMS.callAjax(self,
            self.configurations.updateMaxTurnoverLink,
            self.params.updateMaxTurnover,
            self.completeUpdateMaxTurnover,
            self.errorUpdateMaxTurnover,
            function () { });
    };
    self.completeUpdateMaxTurnover = function (result) {
        if (result.Code) {
            CMS.showInlineErrorMsg(result.Message, self.domElements.divResult);
            CMS.enableInput();
            return;
        }

        CMS.showInlineSuccessMsg(result.Message, self.domElements.divResult, true);
        self.refreshSMATurnoverList();
    };
    self.errorUpdateMaxTurnover = function (xhr) {
        CMS.handleAjaxError(xhr);
        CMS.enableInput();
    };
    self.checkUpdateMaxTurnoverParam = function (updateMaxTurnoverParam) {
        if (updateMaxTurnoverParam.MaxTurnover > maxTurnoverValue) {
            CMS.showInlineErrorMsg(viewResources.GlobalSetting_MaxTurnover_ExceedMaxTurnover, self.domElements.divResult);
            return false;
        }

        CMS.hideInlineMsg(self.domElements.divResult);
        return true;
    };
    self.viewCompanyTurnoverBalance = function (leagueIndex, matchIndex, customerIndex) {
        var customerTurnover = self.cachedData.smaTurnoverList.ChildTurnoverInfo[leagueIndex].
        ChildTurnoverInfo[matchIndex].
        ChildTurnoverInfo[customerIndex].TurnoverInfo;
        popUpManager.openPopup(
            self.configurations.viewCompanyTurnoverBalanceLink + "?" + $.param({
                custId: customerTurnover.Id,
                matchId: customerTurnover.MatchInfo.Id
            }),
            viewResources.Company_Turnover,
            650,
            350,
            true,
            false,
            parent.window,
            3000,
            null,
            null,
            ['center', 160 - CMS.popupTopMargin]);
    };
    self.refreshSMATurnoverList = function () {
        CMS.disableInput();
        CMS.callAjax(self, self.configurations.getSMATurnoverListLink, self.params.getSMATurnoverList, self.renderSMATurnoverList);
    };
    self.showCountDownOptions = function (btnDown) {
        var pnlDown = $('.countDownOptionContainer');
        var arrBtnPos = CMS.findPosition(btnDown.parent());
        if (pnlDown.css("display") === 'none') {
            pnlDown.css("left", arrBtnPos[0] + 'px');
            pnlDown.css("top", (arrBtnPos[1] + 20) + 'px');
            pnlDown.css("display", 'inline-block');
        }
        else {
            pnlDown.css("display", 'none');
        }
    };
    self.saveCountDownRefreshOption = function (btnCountDownOption) {
        btnCountDownOption = $(btnCountDownOption);
        $(".countDownOption.countDownOptionCheck").removeClass("countDownOptionCheck");
        btnCountDownOption.addClass("countDownOptionCheck");
        setTimeout(function () {
            $('.countDownOptionContainer').css("display", 'none');
            var refreshIcon = $(".refreshBox .refreshIcon");
            var coundownAfter = btnCountDownOption.attr("val");
            if (coundownAfter === viewResources.Not_Refresh) {
                refreshIcon.css("text-align", 'left');
                refreshIcon.css("padding-left", '8px');
                refreshIcon.css("width", '85px');
                refreshIcon.html(coundownAfter);

                self.cachedData.refreshIntervalCounter = 0;
                if (self.cachedData.refreshIntervalEvent > 0) {
                    window.clearInterval(self.cachedData.refreshIntervalEvent);
                    window.clearInterval(self.cachedData.updateCountdownEvent);
                }
            }
            else {
                refreshIcon.css("text-align", 'center');
                refreshIcon.css("padding-left", '0');
                refreshIcon.css("width", '93px');
                refreshIcon.html(coundownAfter);

                if (self.cachedData.refreshIntervalEvent > 0) {
                    window.clearInterval(self.cachedData.refreshIntervalEvent);
                    window.clearInterval(self.cachedData.updateCountdownEvent);
                }

                self.cachedData.refreshIntervalCounter = parseInt(coundownAfter * 1000);
                self.cachedData.countdownTime = parseInt(coundownAfter);
                self.cachedData.updateCountdownEvent = setInterval(function () {
                    if (self.cachedData.countdownTime > 1) {
                        self.cachedData.countdownTime--;
                    } else {
                        self.cachedData.countdownTime = self.cachedData.refreshIntervalCounter / 1000;
                    }

                    refreshIcon.html(self.cachedData.countdownTime);
                }, 1000);
                self.cachedData.refreshIntervalEvent = setInterval(function () {
                    self.cachedData.countdownTime = self.cachedData.refreshIntervalCounter / 1000;
                    self.refreshSMATurnoverList();
                }, self.cachedData.refreshIntervalCounter);
            }

        }, 200);
    };
};

turnoverMonitorManager = new Fanex.Nap.CMS.TurnoverControl.TurnoverMonitorManager();