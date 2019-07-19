function WeeklyPicker() {

    this.options = {
        $: null,
        fromDateId: "dpk-fromdate-weekly",
        minFromDate: null,
        maxFromDate: null,

        toDateId: "dpk-todate-weekly",
        minToDate: null,
        maxToDate: null,
        maxRange: null
    };

    this.$fromDate = null;
    this.$toDate = null;

    this.getDateRange = () => {
        return {
            fromDate: this.$fromDate.val(),
            toDate: this.$toDate.val()
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
            minDate: this.options.minFromDate,
            maxDate: this.options.maxFromDate,
            onSelect: this.fromDateChanged,
            beforeShowDay: function (date) {
                return [date.getDay() == 6];
            }
        });

        this.$toDate.datepicker({
            dateFormat: "M dd, yy",
            changeMonth: true,
            changeYear: true,
            showOtherMonths: true,
            selectOtherMonths: true,
            minDate: this.options.minToDate,
            maxDate: this.options.maxToDate,
            onSelect: this.toDateChanged,
            beforeShowDay: function (date) {
                return [date.getDay() == 5];
            }
        });

        var self = this;
    };
    this.fromDateChanged = (dateText) => {
        var fromDate = this.$fromDate.datepicker("getDate");
        var toDate = this.$toDate.datepicker("getDate");
        if (toDate < fromDate) {
            this.$toDate.datepicker("setDate", this.getLastDateOfWeek(fromDate));
        }
    };
    this.toDateChanged = (dateText) => {
        var fromDate = this.$fromDate.datepicker("getDate");
        var toDate = this.$toDate.datepicker("getDate");
        if (toDate < fromDate) {
            this.$fromDate.datepicker("setDate", this.getFirstDateOfWeek(toDate));
        }
    };

    this.getLastDateOfWeek = (currentDate) => {
        var d = new Date(currentDate);
        var day = d.getDay(),
            diff = d.getDate() - day + (day == 0 ? -6 : 1); // adjust when day is sunday

        return new Date(d.setDate(diff + 11));
    };

    this.getFirstDateOfWeek = (currentDate) => {
        var d = new Date(currentDate);
        var day = d.getDay(),
            diff = d.getDate() - day + (day == 0 ? -6 : 1); // adjust when day is sunday

        return new Date(d.setDate(diff - 2));
    };

    this.isValidRange = () => {
        var fromDate = this.$fromDate.datepicker("getDate");
        var toDate = this.$toDate.datepicker("getDate");

        var diff = (toDate.getTime() - fromDate.getTime()) / 1000;
        diff /= 60 * 60 * 24;
        diff += 1;

        var weekDiff = Math.round(diff / 7);
        return this.options.maxRange == null || weekDiff <= this.options.maxRange;
    };
}