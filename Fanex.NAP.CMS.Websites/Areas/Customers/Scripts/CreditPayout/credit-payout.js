"use strict";

(function ($) {
    let cachedSearchUrlWithQueryString = null,
        ddlPromotion = $("#ddlPromotion"),
        ddlPromotionBox = null,
        ddlMatchBox = null,
        ddlStatusBox = null,
        cachedGridOptions = null,
        freeGrid = $("#freegrid"),
        ddlMatch = null,
        cachedPayouts = [];


    function init() {
        ddlPromotionBox = $("#ddlPromotionBox");
        ddlMatchBox = $("#ddlMatchBox");
        ddlStatusBox = $("#ddlStatusBox");
        ddlPromotion.change(renderMatch).trigger("change");

        $("#btnFilterSubmit").off("click").click(function (e) {
            const formData = {
                TypeId: ddlPromotion.val(),
                MatchID: ddlMatch.val(),
                Username: $("#txtFilterUsername").val(),
                VerificationCode: $("#txtFilterCode").val(),
                Status: $("#ddlStatus").val() === "All" ? "" : $("#ddlStatus").val(),
                IsExpired: $("#ckbIsExpired").prop("checked"),
                IsTest: $("#ckbIsTest").prop("checked")
            };

            if (ddlPromotionBox.find(".error-tipsy").length) {
                return;
            }
            if (ddlMatchBox.find(".error-tipsy").length) {
                return;
            }
            if (ddlStatusBox.find(".error-tipsy").length) {
                return;
            }
            if (containsHTML(formData.Username) || containsHTML(formData.VerificationCode)) {
                CMS.showErrorMessage("Invalid Filter Values!");
                return;
            }
            cachedSearchUrlWithQueryString = CMS.resolveRootUrl(`CreditPayout/Search?${$.param(formData)}`);
            search(0, getPageSize());
        });

        $("#btnFilterVerify").off("click").click(function () {
            creditPayoutVerify.openModal(creditPayoutVerify.VERIFY_SEARCH, $("#ddlPromotion option:selected").text());
        });
        $("#btnBulkUpdate").off('click').click(function () {
            var params = getCheckedInfo();
            if (params.payoutCheckedList.length === 1) {
                creditPayoutBulkUpdate.openModal(params, params.payoutCheckedList[0]);
            } else if (params.payoutCheckedList.length) {
                creditPayoutBulkUpdate.openModal(params);
            }
        });

        creditPayoutBulkUpdate.init(reloadGrid);
        creditPayoutVerify.init(reloadGrid);
    }

    function reloadGrid() {
        search(0, getPageSize());
    }

    function renderMatch() {
        const promotonType = +this.value;
        CMS.getJson(
            CMS.resolveRootUrl(`CreditPayout/Match?typeId=${promotonType}`),
            function (data) {
                var html = [];
                html.push(`<option value="0" selected >All</option>`);
                var matches = JSON.parse(data);
                matches.forEach(function (item) {
                    html.push('<option value="');
                    html.push(item.Value);
                    html.push('"');

                    html.push('>');
                    html.push(item.Text);
                    html.push('</option>');
                });
                ddlMatchBox.html(`<select id="ddlMatch" class="combobox-autocomplete">${html.join("")}</select>`);
                ddlMatch = $("#ddlMatch").combobox();
            });
    }

    function getPageSize() {
        return cachedGridOptions ? cachedGridOptions.pagerOption.itemsPerPage : 50;
    }

    function search(pageIndex, pageSize) {
        CMS.getJson(
            `${cachedSearchUrlWithQueryString}&pageIndex=${pageIndex + 1}&pageSize=${pageSize}`,
            function (data) {
                var result = typeof data === "string" ? JSON.parse(data) : data;
                if (result.Message) {
                    CMS.showErrorMessage(result.Message);
                    assignCachedGridOptions({ TotalItems: 0, Payouts: [] }, 0, 50);
                    freeGrid.removeClass("display-none").fanexGrid(cachedGridOptions);
                } else {
                    assignCachedGridOptions(result, pageIndex, pageSize);
                    freeGrid.removeClass("display-none").fanexGrid(cachedGridOptions);
                    bindEventsAfterRenderGridSuccessfully();
                }
            },
            function (error) {
                assignCachedGridOptions({ TotalItems: 0, Payouts: [] }, 0, 50);
                freeGrid.removeClass("display-none").fanexGrid(cachedGridOptions);
            });
    }

    function assignCachedGridOptions(data, pageIndex, pageSize) {
        cachedGridOptions = cachedGridOptions || defaultGridOptions();

        cachedGridOptions.pagerOption.currentPage = pageIndex;
        cachedGridOptions.pagerOption.itemsPerPage = pageSize;
        cachedGridOptions.pagerOption.totalItem = data.TotalItems;

        cachedGridOptions.bodyRows = buildBodyRows(data.Payouts, pageSize * pageIndex);
    }

    function defaultGridOptions() {
        let columns = [
            { HeaderText: resources.no, Width: 30, Name: "colNo", HeaderAlign: "center", CellAlign: "center" }
        ];

        if (hasHostPermission) {
            columns.push(...[
                { HeaderText: renderHeaderCheckBoxAll(), Width: 30, Name: "colCheckBox", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: resources.action, Width: 60, Name: "colAction", HeaderAlign: "center", CellAlign: "left" }
            ]);
        }

        columns.push(...[
            { HeaderText: resources.username, Width: 200, Name: "colUsername", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: resources.site, Width: 100, Name: "colSite", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.ammount, Width: 60, Name: "colAmount", HeaderAlign: "center", CellAlign: "right" },
            { HeaderText: resources.currency, Width: 60, Name: "colCurrency", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.matchId, Width: 60, Name: "colMatchId", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.match, Width: 250, Name: "colMatch", HeaderAlign: "center", CellAlign: "center", Allowtruncate: true },
            { HeaderText: resources.status, Width: 80, Name: "colStatus", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.createdDate, Width: 100, Name: "colCreatedDate", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.expiredDate, Width: 100, Name: "colExpiredDate", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.paymentDate, Width: 100, Name: "colPaymentDate", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.lastUpdatedDate, Width: 160, Name: "colLastUpdatedDate", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.updatedBy, Width: 60, Name: "colUpdatedBy", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: resources.remark, Width: 100, Name: "colRemark", HeaderAlign: "center", CellAlign: "center" }
        ]);

        return {
            showTitle: false,
            fullWidth: true,
            fullHeight: true,
            showDistinctRow: false,
            showHighlightRow: true,
            gridExpandHeight: 195,
            columns: columns,
            showPager: true,
            customPager: true,
            pagerOption: {
                itemsPerPageArray: CMS.pagerRanges,
                pagerSelectCallBack: search
            }
        };
    }

    function renderHeaderCheckBoxAll() {
        return `<input type="checkbox" id="ckbGridSelectedAll"/>`;
    }

    function buildBodyRows(data, startIndex) {
        cachedPayouts = data;
        return data.map(function (payout, index) {
            let columns = [
                { Name: "colNo", Value: startIndex + index + 1 }
            ];

            if (hasHostPermission) {
                columns.push(...[
                    { Name: "colCheckBox", Value: renderCheckBox(payout, index) },
                    { Name: "colAction", Value: renderActionCell(payout, index) }
                ]);
            }
            columns.push(...[
                { Name: "colUsername", Value: payout.Username },
                { Name: "colSite", Value: payout.Site },
                { Name: "colAmount", Value: payout.Amount, CssClass: "amount-cell" },
                { Name: "colCurrency", Value: payout.Currency },
                { Name: "colMatchId", Value: payout.MatchID },
                { Name: "colMatch", Value: payout.Match },
                { Name: "colStatus", Value: payout.Status },
                { Name: "colCreatedDate", Value: payout.CreatedDate },
                { Name: "colExpiredDate", Value: payout.ExpiredDate, CssClass: payout.IsExpired ? "expired" : "" },
                { Name: "colPaymentDate", Value: payout.PaymentDate },
                { Name: "colLastUpdatedDate", Value: payout.LastModifiedDate },
                { Name: "colUpdatedBy", Value: payout.LastModifiedUser },
                { Name: "colRemark", Value: payout.Remark }
            ]);
            
            return { Columns: columns };
        });
    }

    function renderCheckBox(payout, index) {
        return `<input type="checkbox" class="ckbGridItem" data-index="${index}"/>`;
    }

    function renderActionCell(payout, index) {
        return `<span class="btn-verify-item ico-verified" title="Verify" data-index="${index}"></span><span class="btn-edit-item btn-edit ico-edit" title="Edit" data-index="${index}"></span>`;
    }

    function bindEventsAfterRenderGridSuccessfully() {
        $(".btn-verify-item").each(function () {
            $(this).off("click").click(function () {
                var index = +$(this).data("index");
                if (cachedPayouts[index]) {
                    openVerifyPopup(cachedPayouts[index]);
                }
            });
        });
        $(".btn-edit-item").each(function () {
            $(this).off("click").click(function () {
                var index = +$(this).data("index");
                if (cachedPayouts[index]) {
                    openEditPopup(cachedPayouts[index]);
                }
            });
        });

        $("#ckbGridSelectedAll").off("change").change(function () {
            var isChecked = $(this).prop("checked");
            $(".ckbGridItem").each(function () {
                $(this).prop("checked", isChecked);
            });
            if (isChecked) {
                $("#btnBulkUpdate").prop("disabled", false);
            } else {
                $("#btnBulkUpdate").prop("disabled", true);
            }
        });
        $(".ckbGridItem").each(function () {
            $(this).off("change").change(function () {
                var isCheckedAll = true;
                var isUnCheckedAll = true;
                $(".ckbGridItem").each(function () {
                    if (!$(this).prop("checked")) {
                        isCheckedAll = false;
                    } else {
                        isUnCheckedAll = false;
                    }
                });
                $("#ckbGridSelectedAll").prop("checked", isCheckedAll);
                if (isUnCheckedAll) {
                    $("#btnBulkUpdate").prop("disabled", true);
                } else {
                    $("#btnBulkUpdate").prop("disabled", false);
                }
            });
        });
    }

    function openVerifyPopup(payout) {
        var params = {
            Username: payout.Username,
            DrawID: payout.DrawID,
            CustID: payout.CustID,
            Amount: payout.Amount,
            Currency: payout.Currency,
            Match: payout.Match,
            Site: payout.Site
        };

        creditPayoutVerify.openModal(creditPayoutVerify.VERIFY_MEMBER, $("#ddlPromotion option:selected").text(), params);
    }

    function openEditPopup(payout) {
        var params = {
            usernameList: [{ CustId: payout.CustID, Username: payout.Username }],
            custIDList: BuildCustIdList([payout]),
            usernames: [payout.Username]
        };

        creditPayoutBulkUpdate.openModal(params, payout);
    }

    function BuildCustIdList(payoutList) {
        var promotion = "";
        payoutList.forEach(function (payout) {
            promotion += `<Promotion DrawID="${payout.DrawID}" CustID="${payout.CustID}"/>`;
        });

        return `<Root>${promotion}</Root>`;
    }

    function getCheckedInfo() {
        var payoutCheckedList = [];
        var usernames = [];
        var usernameList = [];
        $(".ckbGridItem").each(function () {
            if ($(this).prop("checked")) {
                var index = +$(this).data("index");
                var payout = cachedPayouts[index];
                if (payout) {
                    payoutCheckedList.push(payout);
                    usernames.push(payout.Username);
                    usernameList.push({ CustId: payout.CustID, Username: payout.Username });
                }
            }
        });

        return {
            custIDList: BuildCustIdList(payoutCheckedList), usernames, usernameList, payoutCheckedList
        };
    }

    function containsHTML(str) {
        return str.match(/^<(?:\"[^\"]*\"['\"]*|'[^']*'['\"]*|[^'\">])+>/);
    }

    $(function () {
        init();
    });
})(jQuery);