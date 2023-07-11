"use strict";

const ruleManagementAddOrEdit = (function ($) {
    const ADD_NEW = 1,
        EDIT = 2,
        CLONE = 3;
    const SoccerSportType = 1,
        BasketballSportType = 2;
    let addNewOrEditForm = null,
        formMode = null,
        inlineMessage = null,
        txtCustomerClass = null,
        txtRuleTitle = null,
        dynamicCCRuleTitleControl = null,
        ddlSportType = null,
        spanSportType = null,
        ddlLeagueGroupBox = null,
        ddlLeagueGroup = null,
        ddlIndicator = null,
        txtDiffBalanceRatio = null,
        txtBetDelay = null,
        txtNextBetIntervalSec = null,
        txtSuperBetIntervalSec = null,
        txtSiteBetIntervalSec = null,
        txtMinBet = null,
        txtMaxBet = null,
        txtMaxPerMatch = null,
        txtRepeatedBets = null,
        txtSeizeBet = null,
        chkDisableParlay = null,
        //txtPeakHourSpread = null,
        reloadRuleGrid = null;

    function init(reloadGridFunc) {
        reloadRuleGrid = reloadGridFunc;
        addNewOrEditForm = $("#addNewOrEditForm");
        // Empty the #divPopupContent since it's children was cached.
        // It also helps to prevent the 'for' attributes do not work correctly in the popup.
        $("#divPopupContent").remove();
    }

    function openModal(mode, rule) {
        formMode = mode;
        rule = rule || {};
        CMS.modal(
            getModalTitle(rule.RuleTitle),
            addNewOrEditForm.clone(),
            {
                resizable: false,
                position: {
                    my: "center top",
                    at: "center top",
                    of: "#headline"
                }
            });

        cacheElements(rule);
        bindEvents(rule.RuleId);
    }

    function cacheElements(rule) {
        inlineMessage = $("#inlineMessage");
        dynamicCCRuleTitleControl = $("#dynamicCCRuleTitleControl");
        txtCustomerClass = $("#txtCustomerClass");
        txtRuleTitle = $("#txtRuleTitle");

        if (isAddNew()) {
            $(".clone").remove();
        } else if (isEdit()) {
            dynamicCCRuleTitleControl.remove();
            $(".button-add-more").remove();
            $(".clone").remove();
            txtCustomerClass.val(rule.CustomerClass).attr("disabled", "disabled");
            txtRuleTitle.val(rule.RuleTitle);
        } else if (isClone()) {
            $("#txtTargetRuleTitle").val(rule.RuleTitle);
            $("#lblCustomerClass").text(resources.destinationCC);
        }

        ddlSportType = $("#ddlPopupSportType").val(rule.SportType).combobox();
        spanSportType = ddlSportType.next("#ddlPopupSportType");
        ddlLeagueGroupBox = $("#ddlPopupLeagueGroupBox");
        ddlLeagueGroup = renderLeagueGroup(rule.SportType, rule.LeagueGroupIds);
        ddlIndicator = $("#ddlPopupIndicator").val(rule.IsLive).combobox();

        txtDiffBalanceRatio = $("#txtDiffBalanceRatio").val(ruleManagementHelper.formatInteger(rule.DiffBalanceRatio));
        txtBetDelay = $("#txtBetDelay").val(ruleManagementHelper.formatInteger(rule.BetDelay));
        txtNextBetIntervalSec = $("#txtNextBetIntervalSec").val(ruleManagementHelper.formatInteger(rule.NextBetInterval));
        txtSuperBetIntervalSec = $("#txtSuperBetIntervalSec").val(ruleManagementHelper.formatInteger(rule.SuperBetInterval));
        txtSiteBetIntervalSec = $("#txtSiteBetIntervalSec").val(ruleManagementHelper.formatInteger(rule.SiteBetInterval));
        txtMinBet = $("#txtMinBet").val(ruleManagementHelper.formatInteger(rule.MinBet));
        txtMaxBet = $("#txtMaxBet").val(ruleManagementHelper.formatInteger(rule.MaxBet));
        txtMaxPerMatch = $("#txtMaxPerMatch").val(ruleManagementHelper.formatInteger(rule.MaxPerMatch));
        txtRepeatedBets = $("#txtRepeatedBets").val(rule.RepeatedBets);

        txtSeizeBet = $("#txtSeizeBet");
        disableSeizeBetTextbox(rule.SportType);
        txtSeizeBet.val(rule.SeizeBet);

        chkDisableParlay = $("#chkDisableParlay").prop("checked", rule.DisableParlay);
        //txtPeakHourSpread = $("#txtPeakHourSpread").val(ruleManagementHelper.formatInteger(rule.PeakHourSpread));
    }

    function bindEvents(ruleId) {
        let index = 0;
        $("#btnAddMore")
            .off("click")
            .click(function () {
                clearAllTipsyErrors();
                renderDynamicCCRuleTitleControl(++index);
            });

        ddlSportType
            .off("change")
            .change(function () {
                const sportType = +this.value;
                ddlLeagueGroup = renderLeagueGroup(sportType);
                disableSeizeBetTextbox(sportType);
            });

        txtCustomerClass.off().on("keypress", CMS.forceNumberInput);
        [txtDiffBalanceRatio, txtBetDelay, txtNextBetIntervalSec, txtSuperBetIntervalSec, txtSiteBetIntervalSec]
            .forEach(function (item) {
                item.off()
                    .on("keypress", CMS.forceNegativeNumberInput)
                    .on("keyup", CMS.formatDecimal);
            });
        [txtMinBet, txtMaxBet, txtMaxPerMatch, txtRepeatedBets, txtSeizeBet/*, txtPeakHourSpread*/].forEach(function (item) {
            item.off()
                .on("keypress", CMS.forceNumberInput)
                .on("keyup", CMS.formatDecimal);
        });
        const btnSave = $("#btnSave")
            .off("click")
            .click(function () {
                handleOnSave(ruleId);
            });
        isClone() && btnSave.removeClass("blue-button").addClass("green-button").text(resources.clone);
        $("#btnCancel").off("click").click(CMS.closePopup);
    }

    function handleOnSave(ruleId) {
        clearAllTipsyErrors();

        const formData = getFormData();
        if (formData) {
            let action = "Add";
            if (isEdit()) {
                action = "Edit";
                formData.ruleId = ruleId;
            }

            CMS.disableInput();
            CMS.callAjax(
                this,
                CMS.resolveRootUrl(`Rule/${action}`),
                formData,
                function (result) {
                    customerClassHelper.showInlineMessageAndClosePopup(result, inlineMessage, reloadRuleGrid);
                });
        }
    }

    function getFormData() {
        const ccRuleTitles = isEdit()
            ? getCCRuleTitleEdit()
            : getCCRuleTitleAddNew();

        const sportType = validateSportType();
        const leagueGroups = validateLeagueGroups();

        const diffBalanceRatio = validateRangeNumber(txtDiffBalanceRatio);
        const betDelay = validateRangeNumber(txtBetDelay);
        const nextBetIntervalSec = validateRangeNumber(txtNextBetIntervalSec);
        const superBetIntervalSec = validateRangeNumber(txtSuperBetIntervalSec);
        const siteBetIntervalSec = validateRangeNumber(txtSiteBetIntervalSec);
        const minBetMaxBetMaxPerMatch = validateMinBetMaxBetMaxPerMatch();
        const repeatedBets = validateRangeNumber(txtRepeatedBets, resources.pleaseInputPositiveNumber);
        const seizeBet = validateRangeNumber(txtSeizeBet, resources.pleaseInputPositiveNumber);
        //const peakHourSpread = validateRangeNumber(txtPeakHourSpread, resources.pleaseInputPositiveNumber);

        const isValid = ccRuleTitles && ccRuleTitles.length > 0
            && !isUndefined(sportType)
            && !isUndefined(leagueGroups)
            && !isUndefined(diffBalanceRatio)
            && !isUndefined(betDelay)
            && !isUndefined(nextBetIntervalSec)
            && !isUndefined(superBetIntervalSec)
            && !isUndefined(siteBetIntervalSec)
            && !isUndefined(minBetMaxBetMaxPerMatch)
            && !isUndefined(repeatedBets)
            && !isUndefined(seizeBet);
            //&& !isUndefined(peakHourSpread);

        if (isValid) {
            const textToNumber = function (text) {
                return text.length ? +replaceComma(text) : undefined;
            };

            return {
                ccRuleTitles,
                sportType: +ddlSportType.val(),
                leagueGroups,
                isLive: ddlIndicator.val(),
                diffBalanceRatio: textToNumber(diffBalanceRatio),
                betDelay: textToNumber(betDelay),
                nextBetIntervalSec: textToNumber(nextBetIntervalSec),
                superBetIntervalSec: textToNumber(superBetIntervalSec),
                siteBetIntervalSec: textToNumber(siteBetIntervalSec),
                minBet: textToNumber(minBetMaxBetMaxPerMatch.minBet),
                maxBet: textToNumber(minBetMaxBetMaxPerMatch.maxBet),
                maxPerMatch: textToNumber(minBetMaxBetMaxPerMatch.maxPerMatch),
                repeatedBets: textToNumber(repeatedBets),
                seizeBet: textToNumber(seizeBet),
                disableParlay: chkDisableParlay.is(":checked"),
                //peakHourSpread: textToNumber(peakHourSpread)
            };
        }
    }

    function getCCRuleTitleAddNew() {
        let isValid = true;
        let ruleTitles = [];
        let ruleTitleElements = [];
        let ccRuleTitles = [];
        $(".row-cc-ruletitle").each(function () {
            const $this = $(this);
            const id = $this.data("randomid");

            const txtRandomCC = id ? $this.find(`#txtCustomerClass-${id}`) : txtCustomerClass;
            const cc = validateCustomerClass(txtRandomCC);

            const txtRandomRuleTitle = id ? $this.find(`#txtRuleTitle-${id}`) : txtRuleTitle;
            const ruleTitle = validateRuleTitle(txtRandomRuleTitle, "nw");

            if (cc && ruleTitle) {
                ruleTitles.push(ruleTitle);
                ruleTitleElements.push(txtRandomRuleTitle);
                ccRuleTitles.push({ customerClass: cc, ruleTitle });
            } else {
                isValid = false;
            }
        });

        if (isValid) {
            let duplicatedIndexes = [];
            ruleTitles.forEach(function (item, index) {
                if (ruleTitles.indexOf(item) !== index) {
                    duplicatedIndexes.push(index);
                }
            });

            if (duplicatedIndexes.length) {
                duplicatedIndexes.forEach(function (index) {
                    CMS.showTipsyError(ruleTitleElements[index], resources.ruleTitleIsExisted, "nw");
                });

                return;
            }
        }

        return isValid ? ccRuleTitles : [];
    }

    function getCCRuleTitleEdit() {
        const ruleTitle = validateRuleTitle(txtRuleTitle, "nw");

        if (ruleTitle) {
            return [{ customerClass: txtCustomerClass.val(), ruleTitle }];
        }
    }

    function validateSportType() {
        const sportType = ddlSportType.val();
        if (sportType === "0") {
            CMS.showTipsyError(spanSportType, resources.requiredField, "ne");
            return;
        }

        return sportType;
    }

    function validateLeagueGroups() {
        if (ddlLeagueGroup.fanexMultiSelect("isCheckAll")) {
            return "-1";
        }

        const leagueGroups = ddlLeagueGroup.fanexMultiSelect("getItems", true);
        if (!leagueGroups.length) {
            CMS.showTipsyError($("#ddlPopupLeagueGroup"), resources.requiredField, "ne");
            return;
        }

        return leagueGroups.join();
    }

    function validateCustomerClass(inputElement) {
        const textValue = $.trim(inputElement.val());
        if (!textValue.length) {
            CMS.showTipsyError(inputElement, resources.requiredField, "ne");
            return;
        }

        return validateRangeNumber(inputElement, resources.pleaseInputPositiveNumber);
    }

    function validateRuleTitle(inputElement, gravity) {
        const textValue = $.trim(inputElement.val());
        if (!textValue.length) {
            CMS.showTipsyError(inputElement, resources.requiredField, gravity);
            return;
        }

        if (/<|&|"/.test(textValue) || !customerClassHelper.isASCII(textValue)) {
            CMS.showTipsyError(inputElement, `Rule Title cannot contain special characters ('<', '&' and ' " '), symbol and punctuation.`, gravity);
            return;
        }

        return textValue;
    }

    function validateMinBetMaxBetMaxPerMatch() {
        const minBetText = validateRangeNumber(txtMinBet, resources.pleaseInputPositiveNumber);
        const maxBetText = validateRangeNumber(txtMaxBet, resources.pleaseInputPositiveNumber);
        const maxPerMatchText = validateRangeNumber(txtMaxPerMatch, resources.pleaseInputPositiveNumber);

        let isValid = !isUndefined(minBetText)
            && !isUndefined(maxBetText)
            && !isUndefined(maxPerMatchText);

        if (isValid) {
            const minBet = parseInt(replaceComma(minBetText));
            const maxBet = parseInt(replaceComma(maxBetText));
            const maxPerMatch = parseInt(replaceComma(maxPerMatchText));

            if (!isNaN(minBet)) {
                if (!isNaN(maxBet) && minBet > maxBet) {
                    CMS.showTipsyError(txtMinBet, resources.compareMinBetWithMaxBetError, "ne");
                    isValid = false;
                } else if (!isNaN(maxPerMatch) && minBet > maxPerMatch) {
                    CMS.showTipsyError(txtMinBet, resources.compareMinBetWithMaxPerMatchError, "ne");
                    isValid = false;
                }
            }

            if (!isNaN(maxBet) && !isNaN(maxPerMatch) && maxBet > maxPerMatch) {
                CMS.showTipsyError(txtMaxBet, resources.compareMaxBetWithMaxPerMatchError, "ne");
                isValid = false;
            }

            if (isValid) {
                return {
                    minBet: minBetText,
                    maxBet: maxBetText,
                    maxPerMatch: maxPerMatchText
                };
            }
        }
    }

    function validateRangeNumber(textElement, notNumericError) {
        const textValue = $.trim(textElement.val());
        if (textValue.length) {
            if (!CMS.isNumeric(textValue)) {
                CMS.showTipsyError(textElement, notNumericError || resources.pleaseInputNumber, "ne");
                return;
            }

            const intValue = +replaceComma(textValue);
            const range = textElement.data("val-range").split(";");
            const min = +range[0];
            const max = +range[1];
            if (intValue < min || intValue > max) {
                CMS.showTipsyError(
                    textElement,
                    `Please input a value between ${numeral(min).format(CMS.formats.IntegerFormat)} and ${numeral(max).format(CMS.formats.IntegerFormat)}`,
                    "ne");
                return;
            }
        }

        return textValue;
    }

    function renderDynamicCCRuleTitleControl(id) {
        dynamicCCRuleTitleControl.append(renderCCRuleTitle(id));

        $(`#txtCustomerClass-${id}`).off().on("keypress", CMS.forceNumberInput);
        $(`#icoDelete-${id}`).click(function () {
            clearAllTipsyErrors();
            $(this).closest(".row-cc-ruletitle").remove();
        });
    }

    function renderCCRuleTitle(id) {
        return [
            '<div class="form-row row-cc-ruletitle" data-randomid="', id, '">',
            '<div class="lbl-field"><label class="label" for="txtCustomerClass-', id, '">', isAddNew() ? resources.customerClass : resources.destinationCC,
            ' <span class="text-red">*</span></label></div>',
            '<div class="edit-field"><input type="text" class="text" id="txtCustomerClass-', id, '" autocomplete="off" maxlength="5" data-val-range="0;32767" /></div>',
            '<div class="lbl-field"><label class="label" for="txtRuleTitle-', id, '">', resources.ruleTitle, ' <span class="text-red">*</span></label></div>',
            '<div class="edit-field width-430"><input type="text" class="width-100percent" id="txtRuleTitle-', id, '" autocomplete="off" maxlength="500" /></div>',
            '<div class="lbl-field width-30"><span class="ico-delete" id="icoDelete-', id, '" title="Delete"></span></div></div>'
        ].join("");
    }

    function renderLeagueGroup(sportType, selectedLeagueGroups) {
        let isAllLeagueGroups = false;
        let isCheckAll = false;
        let selectedObject = {};
        if (!isUndefined(selectedLeagueGroups)) {
            isAllLeagueGroups = selectedLeagueGroups === "-1";
            isCheckAll = true;
            selectedLeagueGroups.split(",").forEach(function (item) {
                selectedObject[item] = true;
            });
        }

        let leagueGroups = [];
        leagueGroupList.forEach(function (item) {
            if (sportType && (item.SportType === sportType || item.SportType === 0)) {
                const checked = isAllLeagueGroups || selectedObject.hasOwnProperty(item.Id);
                isCheckAll = isCheckAll && checked;
                leagueGroups.push({
                    value: item.Id,
                    text: item.Name,
                    checked: checked,
                    enable: true
                });
            }
        });

        ddlLeagueGroupBox.html('<div id="ddlPopupLeagueGroup"></div>');
        return customerClassHelper.renderMultiSelect(
            "#ddlPopupLeagueGroup",
            {
                minWidth: 248,
                maxHeight: 250,
                showTooltip: false,
                showFilter: true,
                isCheckAll: isCheckAll,
                listData: leagueGroups,
                onCheck: function () {
                    this.checked && CMS.clearTipsyError(ddlLeagueGroup);
                }
            });
    }

    function clearAllTipsyErrors() {
        $("[original-title]").each(function () {
            CMS.clearTipsyError($(this));
        });
    }

    function disableSeizeBetTextbox(sportType) {
        txtSeizeBet.prop("disabled", sportType !== SoccerSportType && sportType !== BasketballSportType).val("");
    }

    function getModalTitle(ruleTitle) {
        if (isAddNew()) {
            return resources.addRuleSetting;
        }

        if (isEdit()) {
            return resources.editRuleSetting;
        }

        if (isClone()) {
            return `${resources.cloneRuleSetting} - ${ruleTitle.length > 100 ? ruleTitle.substring(0, 100) + " ..." : ruleTitle}`;
        }
    }

    function isAddNew() {
        return formMode === ADD_NEW;
    }

    function isEdit() {
        return formMode === EDIT;
    }

    function isClone() {
        return formMode === CLONE;
    }

    function isUndefined(value) {
        return typeof value === "undefined";
    }

    function replaceComma(text) {
        return text.replace(/,/g, "");
    }

    return {
        init: init,
        openModal: openModal,
        ADD_NEW: ADD_NEW,
        EDIT: EDIT,
        CLONE: CLONE
    };
})(jQuery);
