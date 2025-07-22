
(function ($) {
    let cachedData = null,
        cachedPeakNote = null,
        cachedLicenseeSites = [],
        cachedCreditSites = [],
        cachedFilter = null,
        ddlSites = null,
        ddlCurrencies = null,
        ddlLeagueGroupBox = $("#ddlLeagueGroupBox"),
        ddlLeagueGroup = null,
        freeGrid = $("#freegrid"),
        cachedAddNewRule = null,
        hasHostPermisson = null,
        isFirstLoad = true;

    let IsLiveOptions = [{ Text: "No", Value: "0" }, { Text: "Yes", Value: "1" }, { Text: "All", Value: "2" }];

    function init() {
        const txtFilterCustomerClass = $("#txtFilterCustomerClass");
        txtFilterCustomerClass.blur(() => txtFilterCustomerClass.val(customerClassHelper.replaceCharsCommaInput(txtFilterCustomerClass.val())));
        txtFilterCustomerClass.keydown((e) => customerClassHelper.handleNumberCommanTextInput(e));
        renderSites();
        renderCurrencies();
        const ddlSportType = $("#ddlSportType")
            .change(renderLeagueGroup)
            .trigger("change");
        $("#formSearch").submit(function (e) {
            e.preventDefault();

            const formData = {
                customerClass: txtFilterCustomerClass.val(),
                site: getSelectedReplaceAllValue("ddlSites", "0", siteList.length),
                sportTypes: ddlSportType.val(),
                currency: getSelectedReplaceAllValue("ddlCurrencies", "0", currencyList.length),
                leagueGroups: ddlLeagueGroup.fanexMultiSelect("getItems", false).length === 0 ? "-1" : ddlLeagueGroup.fanexMultiSelect("getItems", true).join()
            };
            if (formData.sportTypes === "0") {
                formData.sportTypes = sportList.map(sp => sp.Value).join(',');
            }

            if (formData.customerClass === "All") {
                formData.customerClass = "";
            }
            cachedAddNewRule = null;
            cachedFilter = formData;
            setTimeout(search, 0);
        });

        hasHostPermisson = $("#btnAdd").length > 0;
        if (hasHostPermisson) {
            $("#btnAdd").off("click").click(enableRowForAdding);
            adjustPeakHour.init();
            $("#btnEditPeakHour").off("click").click(function () {
                adjustPeakHour.openModal();
            });
        }

    }

    function renderLeagueGroup() {
        const sportType = +this.value;
        if (customerClassHelper.leagueBySport[sportType]) {
            BuildDropDownLeague(sportType);
        } else {
            customerClassHelper.getLeagueGroups(sportType, function () { BuildDropDownLeague(sportType); });
        }
    }

    function BuildDropDownLeague(sportType) {
        let leagueGroups = [];
        if (sportType === 0) {
            leagueGroupList.forEach(function (item) {
                leagueGroups.push({
                    value: item.Id,
                    text: item.Name,
                    checked: true,
                    enable: true
                });
            });
        } else {
            customerClassHelper.leagueBySport[sportType].forEach(function (item) {
                leagueGroups.push({
                    value: item.Id,
                    text: item.Name,
                    checked: true,
                    enable: true
                });
            });
        }

        ddlLeagueGroupBox.html('<div id="ddlLeagueGroup"></div>');
        ddlLeagueGroup = customerClassHelper.renderMultiSelect(
            "#ddlLeagueGroup",
            {
                minWidth: 220,
                listData: leagueGroups,
                showFilter: true
            }
        );

        if (isFirstLoad) {
            $("#formSearch").trigger("submit");
            isFirstLoad = false;
        }
    }

    function buildGroupSiteList(isAllCredit, isAllLicensee, selectedValue) {
        let creditOrLicensee;
        cachedCreditSites = [];
        cachedLicenseeSites = [];
        let selectedValueList = selectedValue.split(",");
        siteList.forEach(function (item) {
            creditOrLicensee = item.IsLicensee ? cachedLicenseeSites : cachedCreditSites;
            let isChecked = selectedValue === "All" || item.IsLicensee && isAllLicensee || !item.IsLicensee && isAllCredit;
            creditOrLicensee.push({
                value: item.Id,
                text: item.Name,
                checked: isChecked || selectedValueList.includes(item.Name.toString()),
                enable: true
            });
        });

        return [
            {
                value: "credit",
                text: "Credit",
                checked: isAllCredit,
                enable: true,
                items: cachedCreditSites
            },
            {
                value: "licensee",
                text: "Licensee",
                checked: isAllLicensee,
                enable: true,
                items: cachedLicenseeSites
            }
        ];
    }


    function renderSites() {
        var listData = buildGroupSiteList(true, true, "All");
        ddlSites = customerClassHelper.renderMultiSelect(
            "#ddlSites",
            {
                minWidth: 180,
                maxHeight: 300,
                listData: listData,
                isCheckAll: true,
                showFilter: true
            }
        );
    }


    function renderCurrencies() {
        let optionList = [];
        currencyList.forEach(item => optionList.push({ value: item.Id, text: item.Name, checked: true, enable: true })
        );

        ddlCurrencies = customerClassHelper.renderMultiSelect(
            "#ddlCurrencies",
            {
                minWidth: 160,
                maxHeight: 300,
                listData: optionList,
                showFilter: true
            }
        );
    }

    function search() {
        cachedData = [];
        CMS.disableInput();
        CMS.callAjax(
            this,
            CMS.resolveRootUrl(`PeakHour/Search`),
            cachedFilter,
            function (result) {
                var parsedResult = typeof result === "string" ? JSON.parse(result) : result;
                if (parsedResult.Message) {
                    CMS.showErrorMessage(parsedResult.Message);
                    cachedAddNewRule = null;
                    cachedData = [];
                    cachedPeakNote = "";
                    renderGrid(new Array());
                } else {
                    cachedData = parsedResult.SpreadRules;
                    cachedPeakNote = parsedResult.PeakNoteInfo;
                    renderGrid(cachedData);
                }
            },
            null,
            function () {
                CMS.enableInput();
            });
    }

    function renderGrid(data) {
        let html = [
            `<table class="tbl-freegrid" style="height: calc(100vh - 200px);"><thead style="z-index: ${data.length + 2}"><tr>`,
            `<th class="width-180">${resources.title}</th>`,
            `<th class="width-150">${resources.customerClass}</th>`,
            `<th class="width-180">${resources.site}</th>`,
            `<th class="width-150">${resources.currency}</th>`,
            `<th class="width-200">${resources.platform}</th>`,
            `<th class="width-200">${resources.sportType}</th>`,
            `<th class="width-200">${resources.leagueGroup}</th>`,
            `<th class="width-80">${resources.isLive}</th>`,
            `<th class="width-150">${resources.peakHourSpread}<span class="peak-tipsy light-note-info-message small-margin-left" original-title="${cachedPeakNote}"></span></th>`,
            `<th class="width-60">${resources.enable}</th>`,
            hasHostPermisson ? `<th class="width-60">${resources.action}</th>` : '',
            '</tr></thead > ',
            '<tbody>'
        ];

        if (cachedAddNewRule === null && data.length === 0) {
            let colspan = hasHostPermisson ? 11 : 10;
            html.push(buildRowNoData(colspan, resources.noData));
            html.push('</tbody></table>');

            freeGrid.removeClass("display-none").html(html.join(""));
            bindEventNotePeak();
        } else {
            if (cachedAddNewRule !== null) {
                html.push(buildAddRow(data.length + 1));
            }
            progressBuildRow(data, 0, html);
        }
    }

    function progressBuildRow(data, nextPos, html) {
        if (nextPos >= data.length) {
            html.push('</tbody></table>');

            freeGrid.removeClass("display-none").html(html.join(""));

            cachedAddNewRule !== null && bindingEventForAddRow();
            bindEventsAfterRenderGridSuccessfully();
        } else {
            var item = data[nextPos];
            if (customerClassHelper.leagueBySport[item.SportType]) {
                html.push(buildRowItem(item, nextPos, data.length - nextPos));
                progressBuildRow(data, nextPos + 1, html);
            } else {
                customerClassHelper.getLeagueGroups(item.SportType, function () { html.push(buildRowItem(item, nextPos, data.length - nextPos)); progressBuildRow(data, nextPos + 1, html); });
            }
        }
    }

    function buildRowNoData(colspan, textNoData) {
        return `<tr><td colspan="${colspan}" class="text-red align-center">${textNoData}</td></tr>`;
    }

    function buildRowItem(rule, index, zindex) {
        let rowHtml = [`<tr style="z-index: ${zindex}" class="position-relative">`,
        `<td class="rule-cell width-180"><div class="view-item">${CMS.htmlEncode(rule.PolicyTitle)}</div > <div class="dropdown-cell edit-item hidden"><input type="text" class="inputTitle" id="editTitle-${rule.PolicyID}" maxlength="500" /></div></td>`,
        `<td class="rule-cell width-150"><div class="view-item text-center cell-customer-class">${editingGridHelper.renderCustomerClassCellWithLink(rule.CustomerClass, customerClassList)}</div><div class="dropdown-cell edit-item hidden"><div id="ddlEditCustomerClass-${rule.PolicyID}" class="select-customer-class"></div></div></td>`,
        `<td class="rule-cell width-180"><div class="view-item text-center">${editingGridHelper.renderSiteCell(rule.Sites, siteList, cachedCreditSites, cachedLicenseeSites, "0", "-1", "-2", true)}</div><div class="dropdown-cell edit-item hidden"><div id="ddlEditSite-${rule.PolicyID}" class="select-customer-class"></div></div></td>`,
        `<td class="rule-cell width-150"><div class="view-item text-center">${renderCurrencyCell(rule)}</div><div class="dropdown-cell edit-item hidden"><div id="ddlEditCurrency-${rule.PolicyID}" class="select-customer-class"></div></div></td>`,
        `<td class="rule-cell width-200"><div class="view-item text-center">${editingGridHelper.renderPlatformCell(rule.Platforms, platforms)}</div><div class="dropdown-cell edit-item hidden"><div id="ddlEditPlatform-${rule.PolicyID}" class="select-customer-class"></div></div></td>`,
        `<td class="rule-cell width-200"><div class="view-item text-center">${renderSportTypeCell(rule)}</div><div class="dropdown-cell edit-item ddlSportType hidden"><select id="ddlEditSportType-${rule.PolicyID}" class="select-customer-class"></select></div></td>`,
        `<td class="rule-cell width-200"><div class="view-item text-center">${renderLeagueGroupCell(rule)}</div><div class="dropdown-cell edit-item hidden"><div id="ddlEditLeagueGroup-${rule.PolicyID}" class="select-customer-class"></div></div></td>`,
            `<td class="rule-cell width-80"><div class="view-item text-center">${editingGridHelper.renderLiveCell(rule.IsLive)}</div><div class="dropdown-cell edit-item ddlIsLive hidden"><select id="ddlEditIsLive-${rule.PolicyID}" class="select-customer-class"></select></div></td>`,
        `<td class="rule-cell width-150 action spread"><div class="view-item text-center">${rule.Spread}</div><input type="text" class="inputSpread edit-item hidden" id="editSpread-${rule.PolicyID}" /></td>`,
        `<td class="rule-cell width-60 text-center">${renderEnableCell(rule.PolicyID, CMS.htmlEncode(rule.PolicyTitle), rule.SportType, rule.Enable)}</td>`,
        hasHostPermisson ? `<td class="width-60 text-center action">${renderActionCell(rule.PolicyID, index, rule.edittingValue && rule.edittingValue !== null)}</td>` : '',
            `</tr>`
        ];

        return rowHtml.join("");
    }

    function buildAddRow(zindex) {
        let rowHtml = [`<tr style="z-index: ${zindex}" class="position-relative">`,
            `<td class="rule-cell width-180 add-cell"><div><input type="text" class="inputTitle" id="addNewTitle" maxlength="500" /></div></td>`,
            `<td class="rule-cell width-150 add-cell"><div><div id="ddlAddNewCustomerClass" class="select-customer-class"></div></div</td>`,
            `<td class="rule-cell width-180 add-cell"><div><div id="ddlAddNewSite" class="select-customer-class"></div></div></td>`,
            `<td class="rule-cell width-150 add-cell"><div><div id="ddlAddNewCurrency" class="select-customer-class"></div></div></td>`,
            `<td class="rule-cell width-200 add-cell"><div><div id="ddlAddNewPlatform" class="select-customer-class"></div></div></td>`,
            `<td class="rule-cell width-200 add-cell"><div class="ddlSportType"><select id="ddlAddNewSportType" class="select-customer-class"></select></div></td>`,
            `<td class="rule-cell width-200 add-cell"><div><div id="ddlAddNewLeagueGroup" class="select-customer-class"></div></div></td>`,
            `<td class="rule-cell width-80 add-cell"><div class="ddlIsLive"><select id="ddlAddNewIsLive" class="select-customer-class"></select></div></td>`,
            `<td class="rule-cell width-150 action spread"><input type="text" class="inputSpread" id="addNewSpread" /></td>`,
            `<td class="rule-cell width-60 text-center"><input type="checkbox" class="ckbEnable" id="addNewEnable" checked /></td>`,
        hasHostPermisson ? `<td class="width-60 text-center action">${renderAddActionCell()}</td>` : '',
            `</tr>`
        ];

        return rowHtml.join("");
    }

    function renderEnableCell(policyId, policyTitle, sportType, enable) {
        return `<input id="ckbEdit-${policyId}" type="checkbox" class="checkbox-enable" data-policyId="${policyId}" data-sportType="${sportType}" data-policyTitle="${policyTitle.replaceAll('"', '&#34;')}" ${enable ? "checked" : ""} ${hasHostPermisson ? "" : "disabled"} />`;
    }

    function renderActionCell(policyId, index, isEditting) {
        return [
            `<div>`,
            `<span class="ico-edit ${isEditting ? "is-editting" : ""} mid-margin-right view-item" title="Edit" data-policyId="${policyId}" data-index="${index}" ></span>`,
            `<span class="ico-trash view-item" title="Delete" data-policyId="${policyId}"></span>`,
            `<span class="ico-save-16 mid-margin-right edit-item hidden" title="Save" data-policyId="${policyId}" data-index="${index}" ></span>`,
            `<span class="ico-cancel edit-item hidden" title="Cancel" data-policyId="${policyId}" data-index="${index}"></span>`,
            `</div>`
        ].join("");
    }

    function renderCurrencyCell(rule) {
        if (rule.Currencies === "All" || rule.Currencies === "0") {
            return `<a href="javascript:;" class="link-currency" data-currencies="${currencyList.map(cur => cur.Name).join()}">All</a>`;
        }

        const currencyArray = rule.Currencies.split(",");
        var currencyNames = currencyList.filter(cur => currencyArray.includes(cur.Id.toString())).map(cur => cur.Name);
        return currencyNames.length <= 1
            ? currencyNames.join()
            : `<a href="javascript:;" class="link-currency" data-currencies="${currencyNames.join()}">${currencyNames.length}</a>`;
    }

    function renderSportTypeCell(rule) {
        var sport = sportList.find(st => st.Value === rule.SportType.toString());
        if (sport) {
            return sport.Text;
        }
    }

    function renderLeagueGroupCell(rule) {
        if (undefined === rule.LeagueGroups) {
            return "";
        }

        if (customerClassHelper.leagueBySport[rule.SportType]) {
            return editingGridHelper.renderLeagueGroupCell(rule.LeagueGroups, customerClassHelper.leagueBySport[rule.SportType]);
        } else {
            return customerClassHelper.getLeagueGroups(rule.SportType, function () { editingGridHelper.renderLeagueGroupCell(rule.LeagueGroups, customerClassHelper.leagueBySport[rule.SportType]); });
        }
        
    }

    function handleViewItems(element, title, dataType) {
        let html = [
            '<div class="form" id="viewCurrenciesForm">',
            '<table class="tbl-freegrid">',
            '<thead><tr><th class="width-60">#</th><th>', title, '</th></tr></thead>',
            '<tbody>'
        ];
        element.data(dataType).split(",").forEach(function (item, index) {
            html.push('<tr><td class="align-center">', index + 1, '</td><td>', item, '</td></tr>');
        });
        html.push('</tbody></table></div>');

        CMS.modal(
            title,
            html.join(""),
            {
                resizable: false,
                position: {
                    my: "center top",
                    at: "center top",
                    of: "#headline"
                }
            });
    }

    function bindEventsAfterRenderGridSuccessfully() {
        freeGrid.find(".ico-edit")
            .off("click")
            .click(showEditItem);
        freeGrid.find(".ico-trash")
            .off("click")
            .click(showDeleteConfirmPopup);
        freeGrid.find(".link-currency")
            .off("click")
            .click(function () { handleViewItems($(this), resources.currency, "currencies"); });
        freeGrid.find(".link-customerclass")
            .off("click")
            .click(function () { handleViewItems($(this), resources.customerClass, "customerclass"); });
        freeGrid.find(".link-site")
            .off("click")
            .click(function () { handleViewItems($(this), resources.site, "sites"); });
        freeGrid.find(".link-platform")
            .off("click")
            .click(function () { handleViewItems($(this), resources.platform, "platform"); });
        freeGrid.find(".link-league-group")
            .off("click")
            .click(function () { handleViewItems($(this), resources.leagueGroup, "league-group"); });
        freeGrid.find(".checkbox-enable")
            .off("click")
            .click(showEnableDisablePopup);
        freeGrid.find(".is-editting").trigger('click');
        bindEventNotePeak();
    }

    function bindEventNotePeak() {
        $('.light-note-info-message').tipsy({
            className: 'custom-grid-colheader-note',
            html: true,
            opacity: 0.9,
            gravity: 'n'
        });
    }

    function enableRowForAdding() {
        clearAllTipsyErrors();
        if ($("#addNewTitle").length > 0) {
            return;
        }

        if (cachedAddNewRule === null) {
            cachedAddNewRule = {
                PolicyTitle: "",
                SportType: "",
                Spread: "",
                CustomerClass: "",
                Currencies: "",
                Sites: "",
                Platforms: "",
                LeagueGroups: "",
                IsLive: 0,
                Enable: true
            };
        }

        renderGrid(cachedData);
        bindEventsAfterRenderGridSuccessfully();
        $(`#addNewTitle`).focus();
    }

    function bindingEventForAddRow() {
        $(`#addNewTitle`).val(cachedAddNewRule.PolicyTitle).change(function () { cachedAddNewRule.PolicyTitle = $(`#addNewTitle`).val(); });
        editingGridHelper.renderAddEditCustomerClass("ddlAddNewCustomerClass", cachedAddNewRule.CustomerClass, function () { onCustomerClassChange("ddlAddNewCustomerClass"); });
        editingGridHelper.renderCombobox("ddlAddNewSportType", sportList, cachedAddNewRule.SportType);
        editingGridHelper.renderAddEditSite("ddlAddNewSite", cachedAddNewRule.Sites, cachedCreditSites, cachedLicenseeSites, cachedAddNewRule.Sites === "0", cachedAddNewRule.Sites.includes("-1"), cachedAddNewRule.Sites.includes("-2"), function () { onSiteChange("ddlAddNewSite"); });
        editingGridHelper.renderAddEditCurrency("ddlAddNewCurrency", cachedAddNewRule.Currencies, function () { onCurrencyChange("ddlAddNewCurrency"); });
        editingGridHelper.renderAddEditPlatform("ddlAddNewPlatform", cachedAddNewRule.Platforms, platforms, function () { onPlatformChange("ddlAddNewPlatform"); });
        
        $("#ddlAddNewSportType").on("change", function () {
            var sportType = +$(this).val();
            cachedAddNewRule.SportType = sportType;
            if (isNaN(sportType)) {
                editingGridHelper.renderAddEditLeagueGroup("ddlAddNewLeagueGroup", cachedAddNewRule.LeagueGroups, "", function () { onLeagueGroupChange("ddlAddNewLeagueGroup"); });
                return;
            }

            if (customerClassHelper.leagueBySport[sportType]) {
                editingGridHelper.renderAddEditLeagueGroup("ddlAddNewLeagueGroup", cachedAddNewRule.LeagueGroups, sportType, function () { onLeagueGroupChange("ddlAddNewLeagueGroup"); });
            } else {
                customerClassHelper.getLeagueGroups(sportType, function () { editingGridHelper.renderAddEditLeagueGroup("ddlAddNewLeagueGroup", cachedAddNewRule.LeagueGroups, sportType, function () { onLeagueGroupChange("ddlAddNewLeagueGroup"); }); });
            }
        });
        $("#ddlAddNewSportType").trigger("change");
        $("#addNewSpread").off("change");
        var newSpread = new AutoNumeric("#addNewSpread", {
            minimumValue: 0,
            maximumValue: 0.5,
            decimalPlaces: 2,
            onInvalidPaste: "ignore"
        });
        editingGridHelper.renderCombobox("ddlAddNewIsLive", IsLiveOptions, cachedAddNewRule.IsLive, false);
        $(`#ddlAddNewIsLive > input.ui-autocomplete-input`).css('width', '38px');
        $(`#addNewSpread`).change(function () { cachedAddNewRule.Spread = newSpread.get(); });
        $(`#addNewEnable`).change(function () { cachedAddNewRule.Enable = $(`#addNewEnable`).is(":checked"); });


        $("#btn-cancel-add").click(cancelAddNew);
        $("#btn-submit-add").click(submitAddNew);
    }

    function onCustomerClassChange(divId, index) {
        if (index !== undefined) {
            cachedData[index].edittingValue.CustomerClass = $(`#${divId}`).fanexMultiSelect("getItems", true).join();
        } else {
            cachedAddNewRule.CustomerClass = $(`#${divId}`).fanexMultiSelect("getItems", true).join();
        }
    }

    function onSiteChange(divId, index) {
        if (index !== undefined) {
            cachedData[index].edittingValue.Sites = editingGridHelper.getSelectedSites(divId, siteList, "0", cachedCreditSites, "-1", cachedLicenseeSites, "-2");
        } else {
            cachedAddNewRule.Sites = editingGridHelper.getSelectedSites(divId, siteList, "0", cachedCreditSites, "-1", cachedLicenseeSites, "-2");
        }
    }

    function onCurrencyChange(divId, index) {
        if (index !== undefined) {
            cachedData[index].edittingValue.Currencies = getSelectedReplaceAllValue(divId, "0", currencyList.length);
        } else {
            cachedAddNewRule.Currencies = getSelectedReplaceAllValue(divId, "0", currencyList.length);
        }
    }

    function onLeagueGroupChange(divId, index) {
        if (index !== undefined) {
            cachedData[index].edittingValue.LeagueGroups = getSelectedReplaceAllValue(divId, "-1", customerClassHelper.leagueBySport[cachedData[index].edittingValue.SportType].length);
        } else {
            cachedAddNewRule.LeagueGroups = getSelectedReplaceAllValue(divId, "-1", customerClassHelper.leagueBySport[cachedAddNewRule.SportType].length);
        }
    }

    function onPlatformChange(divId, index) {
        if (index !== undefined) {
            cachedData[index].edittingValue.Platforms = getSelectedReplaceAllValue(divId, "0", platforms.length);
        } else {
            cachedAddNewRule.Platforms = getSelectedReplaceAllValue(divId, "0", platforms.length);
        }
    }

    function renderAddActionCell(ruleId) {
        return [
            '<div>',
            '<span class="ico-save-16 mid-margin-right" title="Save" id="btn-submit-add"></span>',
            '<span class="ico-cancel" title="Cancel" id="btn-cancel-add"></span>',
            '</div>'
        ].join("");
    }

    function cancelAddNew() {
        clearAllTipsyErrors();
        if ($("#addNewTitle").length <= 0) {
            return;
        }
        cachedAddNewRule = null;
        renderGrid(cachedData);
        bindEventsAfterRenderGridSuccessfully();
    }

    function submitAddNew() {
        clearAllTipsyErrors();
        if (!cachedAddNewRule) {
            return;
        }
        cachedAddNewRule.SportType = $(`#ddlAddNewSportType`).val();
        cachedAddNewRule.IsLive = $(`#ddlAddNewIsLive`).val();
        if (cachedAddNewRule.SportType === "Please select") {
            cachedAddNewRule.SportType = "";
        }
        if (cachedAddNewRule.IsLive === "Please select") {
            cachedAddNewRule.IsLive = "";
        }
        var newRuleTitle = spreadManagementHelper.validateTitle($(`#addNewTitle`));
        spreadManagementHelper.validateRequiredField($(`#addNewSpread`), cachedAddNewRule.Spread);
        spreadManagementHelper.validateRequiredField($(`#ddlAddNewCustomerClass`), cachedAddNewRule.CustomerClass);
        spreadManagementHelper.validateRequiredField($(`#ddlAddNewCurrency`), cachedAddNewRule.Currencies);
        spreadManagementHelper.validateRequiredField($(`#ddlAddNewSite`), cachedAddNewRule.Sites);
        spreadManagementHelper.validateRequiredField($(`#ddlAddNewPlatform`), cachedAddNewRule.Platforms);
        spreadManagementHelper.validateRequiredField($(`#ddlAddNewLeagueGroup`), cachedAddNewRule.LeagueGroups);
        spreadManagementHelper.validateRequiredField($(`span#ddlAddNewSportType`), cachedAddNewRule.SportType);
        spreadManagementHelper.validateRequiredField($(`span#ddlAddNewIsLive`), cachedAddNewRule.IsLive);
        if (spreadManagementHelper.isEmptyValue(newRuleTitle) || !spreadManagementHelper.isValidRule(cachedAddNewRule) || spreadManagementHelper.isEmptyValue(cachedAddNewRule.LeagueGroups)) {
            return;
        }

        CMS.disableInput();
        CMS.callAjax(
            this,
            CMS.resolveRootUrl(`PeakHour/Add`),
            cachedAddNewRule,
            function (result) {
                const resultJson = typeof result === "string" ? JSON.parse(result) : result;
                if (resultJson.IsSuccess) {
                    cachedAddNewRule.PolicyID = resultJson.PolicyID;
                    cachedData.unshift(cachedAddNewRule);
                    cachedAddNewRule = null;
                    CMS.showSuccessMessage(resultJson.Message);
                    renderGrid(cachedData);
                    bindEventsAfterRenderGridSuccessfully();
                } else {
                    CMS.showErrorMessage(resultJson.Message);
                }
            });
    }

    function getSelectedReplaceAllValue(divId, allvalue, allItemsLength) {
        var selectedItems = $(`#${divId}`).fanexMultiSelect("getItems", true);

        if (selectedItems.length === allItemsLength) {
            return allvalue;
        }

        return selectedItems.join();
    }

    function cancelEditItem(div) {
        clearAllTipsyErrors();
        var rowEdit = div.closest('tr');
        var index = div.data("index");
        cachedData[index].edittingValue = null;
        rowEdit.find("div[class*=view-item]").removeClass("hidden");
        rowEdit.find("div[class*=edit-item]").addClass("hidden");
        rowEdit.find("input[class*=edit-item]").addClass("hidden");
        rowEdit.find("span[class*=view-item]").removeClass("hidden");
        rowEdit.find("span[class*=edit-item]").addClass("hidden");
        rowEdit.find("span[class*=ico-save-16]")
            .off("click");
        rowEdit.find("span[class*=ico-cancel]")
            .off("click");
        rowEdit.find(".checkbox-enable")
            .off("change");
    }

    function showEditItem() {
        clearAllTipsyErrors();
        var rowEdit = $(this).closest('tr');
        var policyId = $(this).data("policyid");
        var index = $(this).data("index");
        var editPolicy = cachedData[index].edittingValue || cachedData[index];
        cachedData[index].edittingValue = { ...cachedData[index] };
        $(`#editTitle-${policyId}`).val(editPolicy.PolicyTitle).change(function () { cachedData[index].edittingValue.PolicyTitle = $(`#editTitle-${policyId}`).val(); });
        rowEdit.find("div[class*=view-item]").addClass("hidden");
        rowEdit.find("div[class*=edit-item]").removeClass("hidden");
        rowEdit.find("input[class*=edit-item]").removeClass("hidden");
        rowEdit.find("span[class*=view-item]").addClass("hidden");
        rowEdit.find("span[class*=edit-item]").removeClass("hidden");
        editingGridHelper.renderAddEditCustomerClass(`ddlEditCustomerClass-${policyId}`, editPolicy.CustomerClass, function () { onCustomerClassChange(`ddlEditCustomerClass-${policyId}`, index); });
        editingGridHelper.renderCombobox(`ddlEditSportType-${policyId}`, sportList, editPolicy.SportType);
        editingGridHelper.renderAddEditSite(`ddlEditSite-${policyId}`, editPolicy.Sites, cachedCreditSites, cachedLicenseeSites, editPolicy.Sites === "0", editPolicy.Sites.includes("-1"), editPolicy.Sites.includes("-2"), function () { onSiteChange(`ddlEditSite-${policyId}`, index); });
        editingGridHelper.renderAddEditCurrency(`ddlEditCurrency-${policyId}`, editPolicy.Currencies, function () { onCurrencyChange(`ddlEditCurrency-${policyId}`, index); });
        editingGridHelper.renderAddEditPlatform(`ddlEditPlatform-${policyId}`, editPolicy.Platforms, platforms, function () { onPlatformChange(`ddlEditPlatform-${policyId}`, index); });

        $(`#editSpread-${policyId}`).off("change");
        var spreadInput = new AutoNumeric(`#editSpread-${policyId}`, {
            minimumValue: 0,
            maximumValue: 0.5,
            decimalPlaces: 2,
            onInvalidPaste: 'ignore'
        }).set(editPolicy.Spread);
        $(`#ddlEditSportType-${policyId}`).on("change", function () {
            var sportType = +$(this).val();
            cachedData[index].edittingValue.SportType = sportType;
            if (isNaN(sportType)) return;
            if (customerClassHelper.leagueBySport[sportType]) {
                editingGridHelper.renderAddEditLeagueGroup(`ddlEditLeagueGroup-${policyId}`, editPolicy.LeagueGroups, sportType, function () { onLeagueGroupChange(`ddlEditLeagueGroup-${policyId}`, index); });
            } else {
                customerClassHelper.getLeagueGroups(sportType, function () {
                    editingGridHelper.renderAddEditLeagueGroup(`ddlEditLeagueGroup-${policyId}`, editPolicy.LeagueGroups, sportType, function () {
                        onLeagueGroupChange(`ddlEditLeagueGroup-${policyId}`, index);
                    });
                });
            }
        });
        editingGridHelper.renderCombobox(`ddlEditIsLive-${policyId}`, IsLiveOptions, editPolicy.IsLive, false);
        $(`#ddlEditIsLive-${policyId} > input.ui-autocomplete-input`).css('width', '38px');
        $(`#ddlEditSportType-${policyId}`).trigger("change");
        $(`#ddlEditIsLive-${policyId}`).trigger("change");
        $(`#editSpread-${policyId}`).change(function () { cachedData[index].edittingValue.Spread = spreadInput.get(); });
        $(`#ckbEdit-${policyId}`).change(function () { cachedData[index].edittingValue.Enable = $(`#ckbEdit-${policyId}`).is(":checked"); });


        rowEdit.find("span[class*=ico-save-16]")
            .off("click")
            .click(saveEditItem);
        rowEdit.find("span[class*=ico-cancel]")
            .off("click")
            .click(function () { spreadInput.remove(); cancelEditItem($(this)); });
        $(`#editTitle-${policyId}`).focus();
    }

    function saveEditItem() {
        clearAllTipsyErrors();
        var policyId = $(this).data("policyid");
        var index = cachedData.findIndex(policy => policyId === policy.PolicyID);
        var sportType = $(`#ddlEditSportType-${policyId}`).val();
        var isLive = $(`#ddlEditIsLive-${policyId}`).val();
        if (sportType === "Please select") {
            sportType = "";
        }
        var updateRule = {
            PolicyID: policyId,
            PolicyTitle: spreadManagementHelper.validateTitle($(`#editTitle-${policyId}`)),
            Spread: spreadManagementHelper.validateRequiredField($(`#editSpread-${policyId}`), $(`#editSpread-${policyId}`).val()),
            SportType: spreadManagementHelper.validateRequiredField($(`span#ddlEditSportType-${policyId}`), sportType),
            CustomerClass: spreadManagementHelper.validateRequiredField($(`#ddlEditCustomerClass-${policyId}`), $(`#ddlEditCustomerClass-${policyId}`).fanexMultiSelect("getItems", true).join()),
            Currencies: spreadManagementHelper.validateRequiredField($(`#ddlEditCurrency-${policyId}`), getSelectedReplaceAllValue(`ddlEditCurrency-${policyId}`, "0", currencyList.length)),
            Sites: spreadManagementHelper.validateRequiredField($(`#ddlEditSite-${policyId}`), editingGridHelper.getSelectedSites(`ddlEditSite-${policyId}`, siteList, "0", cachedCreditSites, "-1", cachedLicenseeSites, "-2")),
            Platforms: spreadManagementHelper.validateRequiredField($(`#ddlEditPlatform-${policyId}`), getSelectedReplaceAllValue(`ddlEditPlatform-${policyId}`, "0", platforms.length)),
            Enable: $(`#ckbEdit-${policyId}`).is(":checked"),
            IsLive: spreadManagementHelper.validateRequiredField($(`span#ddlEditIsLive-${policyId}`), isLive),
            LeagueGroups: spreadManagementHelper.validateRequiredField($(`#ddlEditLeagueGroup-${policyId}`), getSelectedReplaceAllValue(`ddlEditLeagueGroup-${policyId}`, "-1", customerClassHelper.leagueBySport[+sportType].length))
        };

        if (!spreadManagementHelper.isValidRule(updateRule) || spreadManagementHelper.isEmptyValue(updateRule.LeagueGroups)) {
            return;
        }

        CMS.disableInput();
        CMS.callAjax(
            this,
            CMS.resolveRootUrl(`PeakHour/Update`),
            updateRule,
            function (result) {
                const resultJson = typeof result === "string" ? JSON.parse(result) : result;
                if (resultJson.IsSuccess) {
                    CMS.showSuccessMessage(resultJson.Message);
                    cachedData[index] = updateRule;
                    renderGrid(cachedData);
                    bindEventsAfterRenderGridSuccessfully();
                } else {
                    CMS.showErrorMessage(resultJson.Message);
                }
            });
    }

    function showDeleteConfirmPopup() {
        clearAllTipsyErrors();
        var policyId = $(this).data("policyid");
        var index = cachedData.findIndex(policy => policyId === policy.PolicyID);
        CMS.showConfirmMessage(`Are you sure you want to <span class="color-red">delete</span> this title "${CMS.htmlEncode(cachedData[index].PolicyTitle)}"?`, function () { callDelete(policyId, cachedData[index].SportType, index); });
    }

    function callDelete(policyID, sportType, index) {
        CMS.disableInput();
        CMS.callAjax(
            this,
            CMS.resolveRootUrl(`PeakHour/Delete?policyID=${policyID}&sportType=${sportType}`),
            null,
            function (result) {
                const resultJson = typeof result === "string" ? JSON.parse(result) : result;
                if (resultJson.IsSuccess) {
                    CMS.showSuccessMessage(resultJson.Message);
                    cachedData.splice(index, 1);
                    renderGrid(cachedData);
                    bindEventsAfterRenderGridSuccessfully();
                } else {
                    CMS.showErrorMessage(resultJson.Message);
                }
            });
    }

    function showEnableDisablePopup() {
        event.preventDefault();
        clearAllTipsyErrors();
        var checkbox = $(this);
        var policyId = checkbox.data("policyid");
        var isEnable = checkbox.is(":checked");
        var action = isEnable ? "ENABLE" : "DISABLE";
        var policyTitle = checkbox.data("policytitle");
        var sportType = checkbox.data("sporttype");
        CMS.showConfirmMessage(`Are you sure you want to <span class="color-red">${action}</span> this title "${CMS.htmlEncode(policyTitle)}"?`, function () { callEnableDisable(policyId, sportType, isEnable, checkbox); });
    }

    function callEnableDisable(policyID, sportType, isEnable, checkbox) {
        CMS.disableInput();
        CMS.callAjax(
            this,
            CMS.resolveRootUrl(`PeakHour/Enable?policyID=${policyID}&sportType=${sportType}&enable=${isEnable}`),
            null,
            function (result) {
                const resultJson = typeof result === "string" ? JSON.parse(result) : result;
                if (resultJson.IsSuccess) {
                    CMS.showSuccessMessage(resultJson.Message);
                    checkbox.prop('checked', isEnable).trigger("change");
                } else {
                    CMS.showErrorMessage(resultJson.Message);
                }
            });
    }

    function clearAllTipsyErrors() {
        $("[original-title]").each(function () {
            if (!$(this).hasClass('peak-tipsy')) {
                CMS.clearTipsyError($(this));
            }
        });
    }

    $(function () {
        init();
    });
})(jQuery);