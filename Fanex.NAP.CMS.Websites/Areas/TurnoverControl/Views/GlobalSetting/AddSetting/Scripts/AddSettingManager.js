/* File Created: Oct 03, 2014 */
/* global Namespace, 
Fanex, 
CMS, 
viewResources,
numeral,
CMSConstants,
maxTurnoverValue */
/* jshint strict: false,
camelcase: false,
unused: vars  */
var addSettingManager;
$(document).ready(function () {
    addSettingManager.initPage();
});

Namespace.Register("Fanex.Nap.CMS.TurnoverControl.GlobalSetting.AddSettingManager");
Fanex.Nap.CMS.TurnoverControl.GlobalSetting.AddSettingManager = function () {
    var self = this;
    self.domElements = {
        btnAdd: null,
        btnCancel: null,
        textboxSMAName: null,
        textboxMaxTurnover: null,
        divResult: null
    };
    self.configurations = null;
    self.params = {
        addGlobalSetting: {
        }
    };
    self.initPage = function () {
        self.cacheDOMElements();
        self.intElements();
        self.configurations = {
            addGlobalSettingLink: window.rootUrl + 'GlobalSetting/AddGlobalSetting',
            css: {
                displayNone: 'display-none'
            }
        };
        self.bindEvents();
    };
    self.cacheDOMElements = function () {
        self.domElements.btnAdd = $('#btn-add');
        self.domElements.btnCancel = $('#btn-cancel');
        self.domElements.textboxSMAName = $('#txt-username');
        self.domElements.textboxMaxTurnover = $('#txt-maxturnover');
        self.domElements.divResult = $("#div-result-message");
    };
    self.intElements = function () {
    };
    self.bindEvents = function () {
        self.domElements.btnAdd.unbind("click");
        self.domElements.btnAdd.click(self.btnAddClick);
        self.domElements.btnCancel.unbind("click");
        self.domElements.btnCancel.click(self.btnCancelClick);

        self.domElements.textboxMaxTurnover.blur(function () {
            self.formatMaxTurnover();
        });

        self.domElements.textboxMaxTurnover.unbind("keypress");
        self.domElements.textboxMaxTurnover.keypress(function (e) {
            var allowInput = CMS.forceNumberInput(e);
            if (!allowInput) {
                return false;
            }
        });

        self.domElements.textboxMaxTurnover.unbind("keyup");
        self.domElements.textboxMaxTurnover.keyup(function (e) {
            setTimeout(function () {
                self.formatMaxTurnover();
            }, 100);
        });

        self.domElements.textboxMaxTurnover.on("paste", function (e) {
            setTimeout(function () {
                self.formatMaxTurnover();
            }, 100);
        });
    };
    self.formatMaxTurnover = function () {
        var textboxValue = self.domElements.textboxMaxTurnover.val();
        var maxTurnover = parseInt(textboxValue.replace(/,/g, ""));
        if (maxTurnover.toString() === textboxValue.replace(/,/g, "")) {
            self.domElements.textboxMaxTurnover.val(numeral(maxTurnover).format(CMSConstants.NumberFormat.IntNumber));
        }
    };
    self.btnAddClick = function () {
        self.params.addGlobalSetting.Name = self.domElements.textboxSMAName.val();
        self.params.addGlobalSetting.MaxTurnover = parseFloat(self.domElements.textboxMaxTurnover.val().replace(/,/g, ""));

        if (!self.checkAddSettingParam(self.params.addGlobalSetting)) {
            return;
        }

        CMS.disableInput();
        CMS.callAjax(self,
             self.configurations.addGlobalSettingLink,
             self.params.addGlobalSetting,
             self.completeAddGlobalSetting);
    };
    self.btnCancelClick = function () {
        parent.popUpManager.closePopup();
    };
    self.checkAddSettingParam = function (addSettingParam) {
        var isSuccess = true;
        if (addSettingParam.Name === "") {
            CMS.showTipsyError(self.domElements.textboxSMAName, viewResources.Error_RequiredField, "w");
            isSuccess = false;
        } else {
            CMS.clearTipsyError(self.domElements.textboxSMAName);
        }

        if (self.domElements.textboxMaxTurnover.val() === "") {
            CMS.showTipsyError(self.domElements.textboxMaxTurnover, viewResources.Error_RequiredField, "w");
            isSuccess = false;
        } else if (addSettingParam.MaxTurnover > maxTurnoverValue) {
            CMS.showTipsyError(self.domElements.textboxMaxTurnover, viewResources.GlobalSetting_MaxTurnover_ExceedMaxTurnover, "w");
            isSuccess = false;
        } else {
            CMS.clearTipsyError(self.domElements.textboxMaxTurnover);
        }

        return isSuccess;
    };
    self.completeAddGlobalSetting = function (result) {
        if (result.Code) {
            CMS.showInlineErrorMsg(result.Message, self.domElements.divResult);
            return;
        }

        CMS.showInlineSuccessMsg(result.Message, self.domElements.divResult, true, function () {
            parent.globalSettingManager.cachedData.shouldReloadGrid = true;
            parent.popUpManager.closePopup();
        });
    };
};

addSettingManager = new Fanex.Nap.CMS.TurnoverControl.GlobalSetting.AddSettingManager();