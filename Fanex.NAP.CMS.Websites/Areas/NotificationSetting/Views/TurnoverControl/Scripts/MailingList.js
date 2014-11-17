/* File Created: Oct 03, 2014 */
/* global Namespace, 
Fanex, 
CMS,
model,
userGroupId
*/
/* jshint strict: false,
camelcase: false,
unused: vars */
// 'use strict';

var mailingList;
$(document).ready(function () {
    mailingList.initPage();
});

Namespace.Register("Fanex.Nap.CMS.NotificationSetting.TurnoverControl");
Fanex.Nap.CMS.NotificationSetting.TurnoverControl = function () {
    var self = this;
    var mailboxArr = ["txtToList", "txtCcList", "txtBccList"];
    var updateMailingListParams = {
        ToList: '',
        CCList: '',
        BCCList: '',
        SiteId: '',
        SiteName: '',
        isUpdate: false
    };
    var cachedBccListData = '';

    var userGroupEnums = {
        ItGroup: 2
    };

    self.initPage = function () {
        initTokenInput();
        initMailingListData();
        bindingEvents();
    };

    var bindingEvents = function () {
        $("#btnSubmit").click(function () {
            updateMailingList();
        });
    };

    var initTokenInput = function () {
        for (var i = 0; i < mailboxArr.length; i++) {
            $('#' + mailboxArr[i] + '').tokenInput("", {
                theme: "facebook",
                allowFreeTagging: true,
                hintText: "",
                noResultsText: "",
                searchingText: "",
                isCheckEmail: true
            });
        }
    };

    var initMailingListData = function () {
        var mailInfo = null;
        $.each(model.MailInfo, function (key, value) {
            mailInfo = value;
            updateMailingListParams.SiteId = value.SiteId;
            updateMailingListParams.SiteName = value.SiteName;
            updateMailingListParams.isUpdate = value.IsMailExist ? true : false;
        });

        if (mailInfo) {
            $("#txtToList").tokenInput("clear").tokenInput("add", { id: mailInfo.ToList, name: mailInfo.ToList });
            $("#txtCcList").tokenInput("clear").tokenInput("add", { id: mailInfo.CCList, name: mailInfo.CCList });
            cachedBccListData = mailInfo.BCCList;
            if (parseInt(userGroupId) === userGroupEnums.ItGroup) {
                $("#txtBccList").tokenInput("clear").tokenInput("add", { id: mailInfo.BCCList, name: mailInfo.BCCList });
            } else {
                $("#txtBccList").tokenInput("clear").tokenInput("add", { id: '', name: '' });
            }
        } else {
            $("#txtToList").tokenInput("clear").tokenInput("add", { id: '', name: '' });
            $("#txtCcList").tokenInput("clear").tokenInput("add", { id: '', name: '' });
            cachedBccListData = '';
            $("#txtBccList").tokenInput("clear").tokenInput("add", { id: '', name: '' });
        }
    };

    var updateMailingList = function () {
        CMS.disableInput();
        getParamsUpdate();
        CMS.callAjax(
        self,
        window.rootUrl + 'NotificationSetting/TurnoverControlNotification/UpdateMailingList',
        updateMailingListParams,
        self.renderUpdateMailingList);
        CMS.enableInput();
    };

    var getParamsUpdate = function () {
        var emailLength = 0;
        var data = [];
        resetUpdateParams();

        emailLength = $("#toList").find("input").length;
        $("#toList").find("input").each(function (index) {
            var value = $(this).val();
            if (validateEmail(value) && index !== emailLength - 1) {
                data.push(value);
            }
        });

        updateMailingListParams.ToList = data.join(";");

        emailLength = $("#ccList").find("input").length;
        data = [];
        $("#ccList").find("input").each(function (index) {
            var value = $(this).val();
            if (validateEmail(value) && index !== emailLength - 1) {
                data.push(value);
            }
        });

        updateMailingListParams.CCList = data.join(";");

        emailLength = $("#bccList").find("input").length;
        data = [];

        $("#bccList").find("input").each(function (index) {
            var value = $(this).val();
            if (validateEmail(value) && index !== emailLength - 1) {
                data.push(value);
            }
        });

        updateMailingListParams.BCCList = data.join(";");

        if (parseInt(userGroupId) !== userGroupEnums.ItGroup) {
            updateMailingListParams.BCCList = cachedBccListData;
        }
    };

    self.renderUpdateMailingList = function (result) {
        if (result.Code) {
            CMS.showInlineErrorMsg(result.Message, $("#div-result-message"));
            return;
        }

        CMS.showInlineSuccessMsg(result.Message, $("#div-result-message"), true, function () {
            $(location).attr('href', window.rootUrl + 'NotificationSetting/TurnoverControlNotification/Index');
        });
    };

    var validateEmail = function (emailAddress) {
        /*jslint maxlen: 200 */
        var emailRegex = new RegExp(/^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/);
        var valid = emailRegex.test($.trim(emailAddress));

        return valid;
    };

    var resetUpdateParams = function () {
        updateMailingListParams.ToList = '';
        updateMailingListParams.CCList = '';
        updateMailingListParams.BCCList = '';
    };
};

mailingList = new Fanex.Nap.CMS.NotificationSetting.TurnoverControl();