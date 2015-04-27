define([
    'text!js/DropdownDatepicker/DropdownDatepicker.html',
    'js/Component'
], function (Template, Component) {

    return Component.extend({
        datepickerMonth: null,
        slideDown: null,
        isDisplayCustomDatepicker: null,
        storeObject: null,
        events: {
            'click .eng-datepicker-close': 'closeDropDown',
            'click .eng-datepicker-apply': 'loadCustomDateData',
            'click .eng-init': "selectDropdown",
            'click .eng-option>li:not(.eng-init)': "itemDropDownClick",
            'click .eng-datepicer-blur': 'closeDropDown'
        },

        constructor: function (options) {
            var opts = ENG.$.extend(true, {

            }, options);

            Component.prototype.constructor.call(this, opts);

        },
        render: function (params) {

            Component.prototype.render.call(this);

            this.isDisplayCustomDatepicker = false;

            this.initDropDownListMonth();

            this.$el.html("");
            var compile = _.template(this.template());
            var html = compile({ datepickerMonth: this.datepickerMonth });
            this.$el.append(html);

            this.initDropdownValue();

            return this;
        },
        template: function () {
            return Template;
        },
        initDropdownValue: function () {
            if (this.storeObject != null && this.storeObject != undefined) {
                this.$el.find(".eng-init").html(this.getSelectedTextByValue(this.storeObject.selectedValue));
                this.$el.find("[data-value='" + this.storeObject.selectedValue + "']").addClass('eng-selected');
            }
        },
        initDatepicker: function () {
            var me = this;
            this.$el.find(".eng-datePicker-from").datepicker({
                numberOfMonths: 1,
                onSelect: function (selected) {
                    ENG.$(this).parent().siblings(".eng-datepicker-span-table-view").find(".eng-datePicker-to").datepicker("option", "minDate", selected);
                }


            });

            this.$el.find(".eng-datePicker-to").datepicker({
                numberOfMonths: 1,
                onSelect: function (selected) {
                    ENG.$(this).parent().siblings(".eng-datepicker-span-table-view").find(".eng-datePicker-from").datepicker("option", "maxDate", selected);
                }
            });


            var me = this;
            var defaultEndDate = new Date(this.storeObject.endDate.getFullYear(), this.storeObject.endDate.getMonth(), this.storeObject.endDate.getDate() - 1);
            var defaultStartDate = this.storeObject.startDate;

            this.$el.find(".eng-datepicker-table-view").last().val(ENG.getDateStringMMDDYYYY(defaultStartDate));
            this.$el.find(".eng-datepicker-table-view").first().val(ENG.getDateStringMMDDYYYY(defaultEndDate));
        },
        initDropDownListMonth: function () {
            var me = this;
            this.datepickerMonth = [];
            var currentDate = new Date();
            for (var i = 0; i < 3; i++) {
                var date = new Date(currentDate.getFullYear(), currentDate.getMonth() - i, currentDate.getDate());
                this.datepickerMonth.push({
                    value: (date.getMonth() + 1) + "/" + date.getFullYear(),
                    dateString: me.getMonthStringYear(date)
                });
            }
        },
        itemDropDownClick: function (e) {
            var el = this.$el;
            var dropDown = el.find(".eng-init");
            var options = el.find(".eng-option");
            var allOptions = options.children('li:not(.eng-init)');
            allOptions.removeClass('eng-selected');
            ENG.$(e.target).addClass('eng-selected');
            dropDown.html(ENG.$(e.target).html());
            allOptions.slideUp();
            slideDown = false;

            this.$el.find(".eng-datepicer-blur").css("display", "none");

            var value = options.find(".eng-selected").data("value");
            this.selectDate(value, ENG.$(e.target).html());
        },
        selectDropdown: function () {
            if (this.isDisplayCustomDatepicker) {
                return;
            }
            var el = this.$el;
            var options = el.find(".eng-option");
            var slideDown = this.slideDown;
            if (slideDown) {
                this.slideDown = false;
                options.children('li:not(.eng-init)').slideUp();
                this.$el.find(".eng-datepicer-blur").css("display", "none");
            } else {
                this.slideDown = true;
                options.children('li:not(.eng-init)').slideDown();
                this.$el.find(".eng-datepicer-blur").css("display", "block");
            }
        },
        getMonthStringYear: function (date) {
            var monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
            return monthNames[date.getMonth()] + " " + date.getFullYear();
        },
        selectDate: function (value, text) {
            var me = this;
            if (_.isNumber(value)) {
                if (value === -1) {
                    //custom date
                    this.initDatepicker();
                    this.$el.find(".eng-datepicker-area").css("display", "block");
                    this.isDisplayCustomDatepicker = true;
                    this.$el.find(".eng-datepicer-blur").css("display", "block");

                    this.$el.find(".eng-datepicker-area").draggable({
                        //handle: "h1",
                        cursor: "move",
                        //cursorAt: { top: -200, left: 0 }
                    });

                    return;
                } else {
                    //7,14,..
                    var currentDate = new Date();
                    var endDate = new Date(currentDate.getFullYear(), currentDate.getMonth(), currentDate.getDate() + 1);
                    var startDate = new Date(currentDate.getFullYear(), currentDate.getMonth(), currentDate.getDate() - value);

                }
            } else if (value.indexOf("current") > -1) {
                //today, yesterday,..
                var currentDate = new Date();
                var dateAdd = value.split(".")[1];
                startDate = new Date(currentDate.getFullYear(), currentDate.getMonth(), currentDate.getDate() - parseInt(dateAdd));
                endDate = new Date(currentDate.getFullYear(), currentDate.getMonth(), currentDate.getDate() + 1 - parseInt(dateAdd));
            } else {
                //month
                var arrDate = value.split('/');
                var endDate = new Date(parseInt(arrDate[1]), parseInt(arrDate[0]), 1);
                var startDate = new Date(parseInt(arrDate[1]), parseInt(arrDate[0]) - 1, 1);
            }
            this.triggerEvent(startDate, endDate, value, text);
        },
        triggerEvent: function (startDate, endDate, selectedValue, selectedText) {
            this.trigger("dateChange", { startDate: startDate, endDate: endDate, selectedValue: selectedValue, selectedText: selectedText });
        },
        loadCustomDateData: function (e) {
            this.isDisplayCustomDatepicker = false;
            var me = this;
            var startDate = new Date(this.$el.find(".eng-datePicker-from").val());
            var selectedEndDate = new Date(this.$el.find(".eng-datePicker-to").val());
            var endDate = new Date(selectedEndDate.getFullYear(), selectedEndDate.getMonth(), selectedEndDate.getDate() + 1);

            if (startDate.getTime() > endDate.getTime()) {
                alert("start date must less than end date");
            } else {
                this.$el.find(".eng-datepicker-area").css("display", "none");

                this.$el.find(".eng-init").html(ENG.getDateStringMMDDYYYY(startDate) + " - " + ENG.getDateStringMMDDYYYY(selectedEndDate));
                this.$el.find("[data-value='-1']").addClass('eng-selected');

                this.triggerEvent(startDate, endDate, -1, "");
            }
        },
        closeDropDown: function () {
            this.slideDown = false;
            this.$el.find(".eng-option").children('li:not(.eng-init)').slideUp();
            this.$el.find(".eng-datepicer-blur").css("display", "none");

            this.$el.find(".eng-datepicker-area").css("display", "none");

            this.$el.find(".eng-init").html(this.storeObject.selectedText);
            this.$el.find("[data-value='" + this.storeObject.selectedValue + "']").addClass('eng-selected');


            this.isDisplayCustomDatepicker = false;
        },

        //function for parent component
        initStoreObject: function (searchObject) {
            if (searchObject === undefined) {
                searchObject = {
                    data: null,
                    startDate: null,
                    endDate: null,
                    isNew: false,
                    selectedValue: null,
                    selectedText: null,
                };

                this.initDateValue(searchObject);
            }
            return searchObject;
        },
        initDateValue: function (searchObject) {
            if (searchObject.startDate == null || searchObject.endDate == null) {
                var initDate = this.getDefaultValue();

                //init date value to load
                var defaultEndDate = new Date();
                var defaultStartDate = new Date(defaultEndDate.getFullYear(), defaultEndDate.getMonth(), defaultEndDate.getDate() - initDate);

                searchObject.startDate = defaultStartDate;
                searchObject.endDate = new Date(defaultEndDate.getFullYear(), defaultEndDate.getMonth(), defaultEndDate.getDate() + 1);

                searchObject.selectedValue = initDate;
            }
            return searchObject;
        },
        getDefaultValue: function () {
            return 7;
        },
        getSelectedTextByValue: function (value) {
            var text = "";
            if (_.isNumber(value)) {
                if (value === -1) {
                    //
                } else {
                    //7,14,..
                    text = "Last " + value + " days";
                }
            } else if (value.indexOf("current") > -1) {
                //today, yesterday,..
                var dateAdd = value.split(".")[1];

                switch (dateAdd) {
                    case "0":
                        text = "Today";
                        break;
                    case "1":
                        text = "Yesterday";
                        break;
                    case "2":
                        text = "2 days ago";
                        break;
                    default:
                }
            } else {
                //month
                var arrDate = value.split('/');
                var date = new Date(parseInt(arrDate[1]), parseInt(arrDate[0]), 1);
                text = this.getMonthStringYear(date);
            }
            return text;
        }
    });
});