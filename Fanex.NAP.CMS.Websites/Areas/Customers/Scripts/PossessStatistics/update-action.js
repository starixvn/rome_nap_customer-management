Namespace.Register("Fanex.CMS.Customer.PossessStatistics.UpdateAction");
Fanex.CMS.Customer.PossessStatistics.UpdateAction = function (form, reloadData) {
    var me = this;
    me.form = form;
    var elements = {
        selectedCustIds: null,
        spanSelectedAccounts: null,
        ddlCategory: null,
        ddlDangerLevel: null,
        chboxApplyForAllMember: null,
        inlineMessage: null,
        btnCancel: null
    };

    me.cacheDOMElements = function () {
        elements.inlineMessage = form.find("#div-inlineMessage");
        elements.btnCancel = form.find("#btn-cancel");
    };

    me.bindEvents = function () {
        elements.btnCancel.click(CMS.closePopup);
        form.submit(function (event) {
            event.preventDefault();
            me.validate() && onSubmit();
        });
    };

    var onSubmit = function () {
        CMS.disableInput();
        CMS.callAjax(this, form.attr("action"), me.getParams(), submitSuccess);
    };

    var submitSuccess = function (data) {
        var jsonData = $.parseJSON(data);

        if (jsonData.Code === 0) {
            CMS.showInlineSuccessMsg(jsonData.Message, elements.inlineMessage, true, function () {
                CMS.closePopup();
                reloadData();
            });
        } else {
            CMS.showInlineErrorMsg(jsonData.Message, elements.inlineMessage);
        }
    };
};

Fanex.CMS.Customer.PossessStatistics.UpdateAction.prototype.init = function (title, selectedCustomers) {
    var me = this;
    CMS.modal(title, me.form, { height: "auto" });
    me.cacheDOMElements();
    me.form.find("#spanSelectedAccounts").text(selectedCustomers.map(c => c.Value).join(", "));
    me.bindEvents();
};

Fanex.CMS.Customer.PossessStatistics.UpdateAction.prototype.getParams = function () {
    return new {};
};

Fanex.CMS.Customer.PossessStatistics.UpdateAction.prototype.validate = function () {
    return true;
};

// Update Danger Level
Namespace.Register("Fanex.CMS.Customer.PossessStatistics.UpdateDangerLevel");
Fanex.CMS.Customer.PossessStatistics.UpdateDangerLevel = function (form, reloadData) {
    var me = this;
    Fanex.CMS.Customer.PossessStatistics.UpdateAction.call(me, form, reloadData);
};

Fanex.CMS.Customer.PossessStatistics.UpdateDangerLevel.prototype = new Fanex.CMS.Customer.PossessStatistics.UpdateAction();
Fanex.CMS.Customer.PossessStatistics.UpdateDangerLevel.prototype.init = function (title, selectedCustomers) {
    var me = this;
    Fanex.CMS.Customer.PossessStatistics.UpdateAction.prototype.init.call(me, title, selectedCustomers);
    me.form.find("#SelectedCustomers").val(JSON.stringify(selectedCustomers));
    me.form.find("#ddlCategory").combobox();
    me.ddlDangerLevel = me.form.find("#ddlDangerLevel").combobox();
    me.spanDangerLevel = me.form.find('span[id="ddlDangerLevel"]');
};

Fanex.CMS.Customer.PossessStatistics.UpdateDangerLevel.prototype.getParams = function () {
    var me = this;

    return {
        SelectedCustomers: JSON.parse(me.form.find("#SelectedCustomers").val()),
        Category: me.form.find("#ddlCategory").val(),
        DangerLevel: me.form.find("#ddlDangerLevel").val(),
        ApplyForAllMembers: me.form.find("#ApplyForAllMembers").is(":checked")
    };
};

Fanex.CMS.Customer.PossessStatistics.UpdateDangerLevel.prototype.validate = function () {
    var me = this;

    if (me.ddlDangerLevel.val() === "-1") {
        CMS.showTipsyError(me.spanDangerLevel, resources.requiredFieldError, "w");
        return false;
    }

    CMS.clearTipsyError(me.spanDangerLevel);
    return true;
};

// Update Monitor Status
Namespace.Register("Fanex.CMS.Customer.PossessStatistics.UpdateMonitorStatus");
Fanex.CMS.Customer.PossessStatistics.UpdateMonitorStatus = function (form, reloadData) {
    var me = this;
    Fanex.CMS.Customer.PossessStatistics.UpdateAction.call(me, form, reloadData);
};

Fanex.CMS.Customer.PossessStatistics.UpdateMonitorStatus.prototype = new Fanex.CMS.Customer.PossessStatistics.UpdateAction();
Fanex.CMS.Customer.PossessStatistics.UpdateMonitorStatus.prototype.init = function (title, selectedCustomers) {
    var me = this;
    Fanex.CMS.Customer.PossessStatistics.UpdateAction.prototype.init.call(me, title, selectedCustomers);
    me.form.find("#SelectedCustIds").val(selectedCustomers.map(x => x.Key).join(","));
    me.form.find("#ddlMonitorStatus").combobox();
};

Fanex.CMS.Customer.PossessStatistics.UpdateMonitorStatus.prototype.getParams = function () {
    var me = this;

    return {
        SelectedCustIds: me.form.find("#SelectedCustIds").val(),
        MonitorStatus: me.form.find("#ddlMonitorStatus").val()
    };
};