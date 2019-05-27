function MonthlyPicker() {

    this.options = {
        $: null,
        fromDateId: "dpk-fromdate-monthly",
        toDateId: "dpk-todate-monthly",
        minDate: null,
        maxDate: null,
        maxRange: null
    };

    this.$fromDate = null;
    this.$toDate = null;

    this.render = function (options) {
        this.$ = options.$;
        this.options = this.$.extend({}, this.options, options);

        this.$fromDate = this.$("#" + this.options.fromDateId);
        this.$toDate = this.$("#" + this.options.toDateId);

        this.$fromDate.off("click.on-x-icon").removeClass("clearable");
        this.$toDate.off("click.on-x-icon").removeClass("clearable");

        this.$fromDate.datepicker({
            dateFormat: "M dd, yy",
            changeMonth: true,
            changeYear: true,
            showOtherMonths: true,
            selectOtherMonths: true,
            minDate: this.options.minDate,
            maxDate: this.options.maxDate,
            onSelect: this.fromDateChanged,
            beforeShowDay: function (date) {
                return [date.getDate() == 1];
            }
        });

        this.$toDate.datepicker({
            dateFormat: "M dd, yy",
            changeMonth: true,
            changeYear: true,
            showOtherMonths: true,
            selectOtherMonths: true,
            minDate: this.options.minDate,
            maxDate: this.options.maxDate,
            onSelect: this.toDateChanged,
            beforeShowDay: this.isEndOfMonth
        });

        var self = this;
    };

    this.getLastDateOfMonth = (currentDate) => {
        return new Date(currentDate.getFullYear(), currentDate.getMonth() + 1, 0);
    };
    this.getFirstDateOfMonth = (currentDate) => {
        return new Date(currentDate.getFullYear(), currentDate.getMonth(), 1);
    };
    this.isEndOfMonth = (currentDate) => {
        var lastDate = this.getLastDateOfMonth(currentDate);
        return [currentDate.getDate() == lastDate.getDate()];
    };
    this.isValidRange = () => {
        var fromDate = this.$fromDate.datepicker("getDate");
        var toDate = this.$toDate.datepicker("getDate");
        var monthDiff =
            toDate.getMonth() - fromDate.getMonth() +
            (toDate.getFullYear() - fromDate.getFullYear()) * 12;

        return this.options.maxRange == null || monthDiff < this.options.maxRange;
    };
    this.fromDateChanged = (dateText) => {
        var fromDate = this.$fromDate.datepicker("getDate");
        var toDate = this.$toDate.datepicker("getDate");
        if (toDate < fromDate) {
            this.$toDate.datepicker("setDate", this.getLastDateOfMonth(fromDate));
        }
    };
    this.toDateChanged = (dateText) => {
        var fromDate = this.$fromDate.datepicker("getDate");
        var toDate = this.$toDate.datepicker("getDate");
        if (toDate < fromDate) {
            this.$fromDate.datepicker("setDate", this.getFirstDateOfMonth(toDate));
        }
    };
    this.getDateRange = () => {
        return {
            fromDate: this.$fromDate.datepicker("getDate"),
            toDate: this.$toDate.datepicker("getDate")
        };
    };
    this.hide = () => {
        this.$fromDate.addClass("display-none");
        this.$toDate.addClass("display-none");
    };
    this.show = () => {
        this.$fromDate.removeClass("display-none");
        this.$toDate.removeClass("display-none");
    };
}