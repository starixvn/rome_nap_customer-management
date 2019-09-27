Namespace.Register("Fanex.CMS.Customer.PossessStatistics.Index");
Fanex.CMS.Customer.PossessStatistics.Index = function () {
    var me = this;
    var dataGrid = new Fanex.CMS.Customer.PossessStatistics.DataGrid();
    var filter = new Fanex.CMS.Customer.PossessStatistics.Filter(dataGrid);
    var elements = {
        btnUpdateDangerLevel: null,
        btnUpdateMonitorStatus: null,
        formUpdateDangerLevel: null,
        formUpdateMonitorStatus: null
    };

    var cacheDOMElements = function () {
        elements.btnUpdateDangerLevel = $("#btnUpdateDangerLevel");
        elements.btnUpdateMonitorStatus = $("#btnUpdateMonitorStatus");
        elements.formUpdateDangerLevel = $("#formUpdateDangerLevel");
        elements.formUpdateMonitorStatus = $("#formUpdateMonitorStatus");
    };

    var handleAction = function (callback) {
        var selectedCustomers = dataGrid.getSelectedCustomers();
        selectedCustomers.length > 0 && callback(selectedCustomers);
    };

    var updateDangerLevel = function (selectedCustomers) {
        var updateDangerLv = new Fanex.CMS.Customer.PossessStatistics.UpdateDangerLevel(elements.formUpdateDangerLevel.clone(), filter.reloadData);
        updateDangerLv.init(resources.UpdateDangerLevel, selectedCustomers);
    };

    var updateMonitorStatus = function (selectedCustomers) {
        var updateMonitorStt = new Fanex.CMS.Customer.PossessStatistics.UpdateMonitorStatus(elements.formUpdateMonitorStatus.clone(), filter.reloadData);
        updateMonitorStt.init(resources.UpdateMonitorStatus, selectedCustomers);
    };

    var bindEvents = function () {
        elements.btnUpdateDangerLevel.click(function () { handleAction(updateDangerLevel); });
        elements.btnUpdateMonitorStatus.click(function () { handleAction(updateMonitorStatus); });
    };

    me.init = function () {
        cacheDOMElements();
        filter.init();
        bindEvents();
        $("#divPopupContent").remove();
    }
};

$(document).ready(function () {
    var index = new Fanex.CMS.Customer.PossessStatistics.Index();
    index.init();
});