/* global CMS, CMSConstants, winLimitControlLabels, ViewHistoryModal */
/* jshint strict: false,
camelcase: false */
(function () {
	"use strict";

	var self = this;
	var $formFilter = $("#form-filterWinLimit"),
		$datePickerPlugin = null,
		$username = $("#username"),
		$game = $("#game"),
		$currency = $("#currency"),
		$status = $("#status"),
		$freegrid = $("#freegrid"),
		$btnUnblocked = $("#btnUnblocked"),
		$divResult = $("#div-result-message"),
		$divViewHistoryContainer = $("#div-ViewHistory");

	var searchUrl = $formFilter.attr("action") + "?",
        searchParameters = null,
        gridOptions = null,
        maxPercentageUsed = 100;

	var viewHistoryModal = null;
	var isSoundAlert = false;

	var timer = {
		id: 0,
		pausing: false,
		refresh: {
			intervals: [90, 120, 150, 180],
			selected: 90
		}
	};

	function getCookieMuteSound() {
		return $.cookie("mute-sound");
	}

	function setCookieMuteSound(value) {
		return $.cookie("mute-sound", value);
	}

	function getSearchParams() {
		var dateRange = $datePickerPlugin ?
			$datePickerPlugin.nexDatePicker("getDateRange") :
			{ FromDate: currentDate, ToDate: currentDate }

		return {
			FromDate: dateRange.FromDate,
			ToDate: dateRange.ToDate,
			Username: $username.val().trim(),
			Game: $game.val(),
			Currency: $currency.val(),
			Status: $status.val(),
			PageIndex: 0,
			PageSize: CMS.pageSize
		};
	}

	function initGridOptions() {
		var columns = [
			{ HeaderText: "#", Width: 40, Name: "colNumber", HeaderAlign: "center", CellAlign: "center" },
			{ HeaderText: winLimitControlLabels.Username, Width: 200, Name: "colUsername", HeaderAlign: "center", CellAlign: "left" },
			{ HeaderText: winLimitControlLabels.Currency, Width: 75, Name: "colCurrency", HeaderAlign: "center", CellAlign: "center" },
			{ HeaderText: winLimitControlLabels.Game, Width: 200, Name: "colGame", HeaderAlign: "center", CellAlign: "left" },
			{ HeaderText: winLimitControlLabels.WinLimit, Width: 100, Name: "colWinLimit", HeaderAlign: "center", CellAlign: "right", FormatNumber: CMSConstants.NumberFormat.IntNumber },
			{ HeaderText: winLimitControlLabels.WinningAmount, Width: 100, Name: "colTotalWinningAmount", HeaderAlign: "center", CellAlign: "right", FormatNumber: CMSConstants.NumberFormat.IntNumber },
			{ HeaderText: winLimitControlLabels.ReachedLimitTime, Width: 145, Name: "colBlockedTime", HeaderAlign: "center", CellAlign: "center" },
			{ HeaderText: winLimitControlLabels.PercentageUsed, Width: 110, Name: "colPercentageUsed", HeaderAlign: "center", CellAlign: "center" },
			{ HeaderText: winLimitControlLabels.Blocked, Width: 50, Name: "colBlocked", HeaderAlign: "center", CellAlign: "center" },
			{ HeaderText: "", Width: 40, Name: "colViewHistory", HeaderAlign: "center", CellAlign: "center" }
		];

		return {
			showTitle: true,
			titleInfo: {},
			fullWidth: true,
			fullHeight: true,
			showDistinctRow: false,
			showHighlightRow: true,
			headerColumnHeight: 35,
			gridExpandHeight: 300,
			columns: columns,
			bodyRows: [],
			showPager: true,
			customPager: true,
			pagerOption: {
				itemsPerPageArray: CMS.pagerRanges,
				pagerSelectCallBack: handleChangingPage
			}
		};
	}

	function renderAutoRefresh() {
		var intervals = timer.refresh.intervals,
            selected = timer.refresh.selected;

		var interval,
            dropdown = ['<ul class="dropdown-container">'];
		for (var i = 0, max = intervals.length; i < max; i++) {
			interval = intervals[i];
			dropdown.push('<li data-value="');
			dropdown.push(interval);
			dropdown.push('"');
			if (selected === interval) {
				dropdown.push(' class="checked"');
			}

			dropdown.push('>');
			dropdown.push(interval);
			dropdown.push(' ');
			dropdown.push('sec');
			dropdown.push('</li>');
		}

		dropdown.push('</ul>');

		return '<div class="refresh-container" id="refresh-container">' +
                    '<div class="timer">' +
                        '<div title="Refresh" class="countdown">' + selected + '</div>' +
                        '<div class="dropdown"></div>' +
                     '</div>' + dropdown.join('') +
                '</div>' +
                '<span id="sound" class="' + (getCookieMuteSound() === "true" ? "sound-disabled" : "sound-enable") + '"></span>';
	}

	function handleChangingPage(pageIndex, itemsPerPage) {
		searchParameters.PageIndex = parseInt(pageIndex);
		searchParameters.PageSize = parseInt(itemsPerPage);
		getWinLimitMonitor();
	}

	function bindAutoRefreshEvents() {
		if (timer.id) {
			clearTimeout(timer.id);
		}

		var refreshContainer = $("#refresh-container");
		var countdownContainer = refreshContainer.find(".countdown");
		var dropdownContainer = refreshContainer.find(".dropdown-container");

		var autoRefreshFunc = function (countdownContainer) {
			timer.id = setTimeout(function () {
				if (!timer.pausing) {
					var currentValue = parseInt(countdownContainer.text());
					if (currentValue === 1) {
						countdownContainer.html("...");
						getWinLimitMonitor();
						return;
					}

					countdownContainer.html(--currentValue);
				}

				autoRefreshFunc(countdownContainer);
			}, 1000);
		};

		countdownContainer.unbind("click").bind("click", getWinLimitMonitor);

		dropdownContainer.find("li")
            .unbind("click")
            .bind("click", function () {
            	dropdownContainer.find("li.checked").removeClass("checked");

            	var $this = $(this);
            	$this.addClass("checked");
            	timer.refresh.selected = parseInt($this.data("value"));
            	countdownContainer.html(timer.refresh.selected);
            });

		autoRefreshFunc(countdownContainer);
	}

	function bindSoundEvent() {
		$("#sound").click(function () {
			$(this).toggleClass("sound-enable sound-disabled");
			var muteSound = getCookieMuteSound() === "true" ? "false" : "true";
			setCookieMuteSound(muteSound);
		});

		if (isSoundAlert) {
			CMS.showErrorMessage("Players have reached Company Win Limit. Please check again.");
			if (getCookieMuteSound() === "false") {
				alertSound.play();
			}

			setTimeout(CMS.closeMsgBox, 1000 * 120);
		}
	}

	function bindUnblockEvent() {
		$(".checkbox-blocked")
			.unbind("click")
			.bind("click", function (e) {
				e.preventDefault();

				var $this = $(this);
				var playerName = $this.data("playername");
				var gameName = $this.data("gamename");

				var message = winLimitControlLabels.UnblockPlayerConfirm
					.replace("{0}", playerName)
					.replace("{1}", gameName);

				var okFunc = function () {
					var unblockParam = {
						PlayerId: $this.data("playerid"),
						GameId: $this.data("gameid"),
						WinlostDate: $this.data("winlostdate"),
					};

					CMS.callAjax(self, window.rootUrl + "WinLimitControl/WinLimitMonitor/UnblockPlayer", unblockParam, function (result) {
						if (result && result.Code === 0) {
							getWinLimitMonitor();
						}
						else {
							CMS.showErrorMessage(result.Message);
						}
					});
				};

				CMS.showConfirmMessage(message, okFunc);
			});
	}

	function bindViewHistory() {
		$freegrid.find(".ico-remark")
            .unbind("click")
            .bind("click", function () {
            	var $this = $(this);
            	var player = { Id: $this.data('playerid'), Name: $this.data('playername') };
            	var game = { Id: $this.data('gameid'), Name: $this.data('gamename') };

            	timer.pausing = true;
            	viewHistoryModal.initialize(player, game);
            });
	}

	function getPercentageUsedBox(percentageUsed) {
		var displayPercent = parseInt(percentageUsed);
		if (displayPercent >= maxPercentageUsed) {
			return '<div class="col-percentage-used-body align-center boxPercentUsed_100Percent">' + displayPercent + '%</div>';
		}

		return '<div class="col-percentage-used-body align-center boxPercentUsed_Low boxPercentUsed" style="background-position: -' +
            (maxPercentageUsed - displayPercent) + 'px">' + displayPercent + '%</div>';
	}

	function renderBlockedRow(customData, isBlocked, winlostDate) {
		if (!isBlocked) {
			return '<input type="checkbox" disabled="disabled" />';
		}

		if (hostPermission === 0) {
			return '<input type="checkbox" disabled="disabled" checked="true" />';
		}

		return '<input type="checkbox" ' + customData + ' data-winlostdate="' + winlostDate + '" checked="true" class="checkbox-blocked" />';
	}

	function renderViewHistoryRow(customData) {
		var customAttr = [
            '<a href="javascript:;" class="ico-remark" ',
            customData,
            ' title="',
            winLimitControlLabels.ViewHistory,
            '"></a>'
		];

		return customAttr.join("");
	}

	function buildBodyRows(data) {
		var bodyRows = [];
		var startIndex = searchParameters.PageIndex * searchParameters.PageSize;

		var cell, winlimit, customData;
		isSoundAlert = false;

		for (var i = 0, max = data.length; i < max; i++) {
			winlimit = data[i];
			customData = 'data-playerid="' + winlimit.Player.Id + '"' + ' data-playername="' + winlimit.Player.Name + '"' +
				' data-gameid="' + winlimit.Game.Id + '"' + ' data-gamename="' + winlimit.Game.Name + '"';
			cell = [
                { Name: "colNumber", Value: (startIndex + i + 1) },
                { Name: "colUsername", Value: winlimit.Player.Name },
                { Name: "colCurrency", Value: winlimit.Currency.Name },
                { Name: "colGame", Value: winlimit.Game.Name },
                { Name: "colWinLimit", Value: winlimit.WinLimit },
                { Name: "colTotalWinningAmount", Value: winlimit.TotalWinningAmount },
                { Name: "colBlockedTime", Value: winlimit.BlockedTimeDisplay },
                { Name: "colPercentageUsed", Value: getPercentageUsedBox(winlimit.PercentageUsed) },
				{ Name: "colBlocked", Value: renderBlockedRow(customData, winlimit.IsBlocked, winlimit.WinlostDateDisplay) },
                { Name: "colViewHistory", Value: renderViewHistoryRow(customData) }
			];

			if (winlimit.IsNotified) {
				bodyRows.push({ Columns: cell, CssClass: "new-data-row" });
				isSoundAlert = true;
			}
			else {
				bodyRows.push({ Columns: cell });
			}
		}

		return bodyRows;
	}

	function getWinLimitMonitor() {
		CMS.hideInlineMsg($divResult);
		var url = searchUrl + $.param(searchParameters);

		CMS.getJson(url, function (result) {
			gridOptions.titleInfo.customButtons = renderAutoRefresh();

			gridOptions.pagerOption.currentPage = searchParameters.PageIndex;
			gridOptions.pagerOption.itemsPerPage = searchParameters.PageSize;
			gridOptions.pagerOption.totalItem = result.TotalRow;

			gridOptions.bodyRows = buildBodyRows(result.Winlimits);
			$freegrid.removeClass("display-none").fanexGrid(gridOptions);

			if (result.TotalRow > 0) {
				bindAutoRefreshEvents();
				bindSoundEvent();
				bindUnblockEvent();
				bindViewHistory();
			}
		});
	}

	function initDateRangeSelect() {
		$datePickerPlugin = $("#fromtodate-picker").nexDatePicker({
			nexDatePickerType: "daterange",
			datePickerFormat: {
				dateFormat: CMS.formats.DateFormat,
				changeMonth: true,
				changeYear: true,
				showOtherMonths: true,
				selectOtherMonths: true,
				minDate: new Date(minDate),
				maxDate: new Date(currentDate)
			},
			firstDatePicker: {
				Id: "dpk-fromdate",
				Label: winLimitControlLabels.FromDate,
				InitDate: new Date(currentDate),
				LabelCss: "lbl-field col_3",
				TextboxCss: "edit-field col_7"
			},
			secondDatePicker: {
				Id: "dpk-todate",
				Label: winLimitControlLabels.ToDate,
				InitDate: new Date(currentDate),
				CurrentDate: new Date(currentDate),
				LabelCss: "lbl-field col_3",
				TextboxCss: "edit-field col_8"
			}
		});
	}

	function initialize() {
		gridOptions = initGridOptions();

		$username.autocomplete({
			minLength: 3,
			delay: 300,
			source: window.rootUrl + "WinLimitControl/PlayerSetting/AutoSuggestion"
		});

		if (getCookieMuteSound() === null) {
			setCookieMuteSound("false");
		}

		viewHistoryModal = new ViewHistoryModal($divViewHistoryContainer, function () { timer.pausing = false; });
		$("#divModals").empty();

		$formFilter
			.submit(function (e) {
				e.preventDefault();
				searchParameters = getSearchParams();
				getWinLimitMonitor();
			})
			.trigger("submit");

		setTimeout(initDateRangeSelect, 100);
	}

	initialize();
})();
