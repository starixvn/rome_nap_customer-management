Namespace.Register("Fanex.CMS.Customer.PossessStatistics.Filter");
Fanex.CMS.Customer.PossessStatistics.Filter = function (dataGrid) {
    var me = this;
    var defaultPageSize = 50;
    var cachedParams = null;
    me.DataGrid = dataGrid;

    var pagerOptions = {
        pageIndex: 0,
        pageSize: defaultPageSize,
        totalItems: 0,
        pagerSelectCallBack: null
    };

    var elements = {
        formSearch: null,
        ddlPTConditionLogic: null,
        ddlAgentStatus: null,
        ddlDangerLevelOperator: null,
        ddlDangerLevelValue: null,
        ddlMonitored: null,
        ddlSortBy: null,
        txtSBPercentFrom: null,
        txtSBPercentTo: null,
        txtNSPercentFrom: null,
        txtNSPercentTo: null,
        txtUsername: null,
    };

    var cacheDOMElements = function () {
        elements.formSearch = $("#formSearch");
        elements.ddlPTConditionLogic = $("#ddlPTConditionLogic");
        elements.ddlAgentStatus = $("#ddlAgentStatus");
        elements.ddlDangerLevelOperator = $("#ddlDangerLevelOperator");
        elements.ddlDangerLevelValue = $("#ddlDangerLevelValue");
        elements.ddlMonitored = $("#ddlMonitored");
        elements.ddlSortBy = $("#ddlSortBy");
        elements.txtSBPercentFrom = $("#txtSBPercentFrom");
        elements.txtSBPercentTo = $("#txtSBPercentTo");
        elements.txtNSPercentFrom = $("#txtNSPercentFrom");
        elements.txtNSPercentTo = $("#txtNSPercentTo");
        elements.txtUsername = $("#txtUsername");
    };

    var registerEvents = function () {
        elements.txtSBPercentFrom.keypress(CMS.forceNegativeNumberInput);
        elements.txtSBPercentTo.keypress(CMS.forceNegativeNumberInput);
        elements.txtNSPercentFrom.keypress(CMS.forceNegativeNumberInput);
        elements.txtNSPercentTo.keypress(CMS.forceNegativeNumberInput);
        elements.ddlAgentStatus.fanexMultiSelect({
            minWidth: 170,
            checkAllText: "All",
            listData: agentStatus.map(function (item) {
                return { id: item.Value, value: item.Value, text: item.Text, checked: item.Selected, enable: true };
            })
        });
        elements.formSearch.submit(function (e) {
            e.preventDefault();
            var pageSize = cachedParams ? cachedParams.pageSize : defaultPageSize;
            getData(0, pageSize);
        });
    };

    var getParams = function () {
        return {
            TotalSBPercentFrom: elements.txtSBPercentFrom.val(),
            TotalSBPercentTo: elements.txtSBPercentTo.val(),
            PTConditionLogic: elements.ddlPTConditionLogic.val(),
            TotalNSPercentFrom: elements.txtNSPercentFrom.val(),
            TotalNSPercentTo: elements.txtNSPercentTo.val(),
            Username: $.trim(elements.txtUsername.val()),
            AgentStatus: elements.ddlAgentStatus.fanexMultiSelect("getItems", true).join(","),
            DangerLevelOperator: elements.ddlDangerLevelOperator.val(),
            DangerLevelValue: elements.ddlDangerLevelValue.val(),
            Monitored: elements.ddlMonitored.val(),
            SortBy: elements.ddlSortBy.val()
        };
    };

    var getData = function (pageIndex, pageSize) {
        pagerOptions.pageIndex = pageIndex;
        pagerOptions.pageSize = pageSize;

        var url = elements.formSearch.attr("action") + "?";
        cachedParams = getParams();
        cachedParams.pageIndex = pageIndex + 1;
        cachedParams.pageSize = pageSize;

        CMS.getJson(url + $.param(cachedParams), function (data) {
            var jsonData = $.parseJSON(data);
            pagerOptions.totalItems = jsonData.TotalItems;
            pagerOptions.pagerSelectCallBack = getData;

            me.DataGrid.render(jsonData.PossessStatistics, pagerOptions, exportExcel);
        });
    };

    var exportExcel = function () {
        CMS.downloadFile(CMS.resolveRootUrl("Customers/PossessStatistics/ExportExcel"), cachedParams);
    };

    me.init = function () {
        cacheDOMElements();
        registerEvents();
    };

    me.reloadData = function () {
        getData(0, cachedParams.pageSize);
    };
}