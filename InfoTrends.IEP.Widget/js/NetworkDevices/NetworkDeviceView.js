define([
    'text!js/NetworkDevices/NetworkDevice.html',
    'js/Container',
    'js/Table/TableView',
    'js/DropdownDatepicker/DropdownDatepickerView',


],
function (Template, Container, TableView, DropdownDatepickerView) {
    return Container.extend({
        events: {
            'click .close': 'closeNetworkAnalyticForm',
        },

        constructor: function (options) {
            var opts = ENG.$.extend(true, {
                data: [],
            }, options);

            Container.prototype.constructor.call(this, opts);

            this.$el.addClass('eng-network-device');
            this.$el.html('');

            var urlCss = ENG.DOMAIN + "/js/NetworkDevices/NetworkDevice.css";
            ENG.loadCss(urlCss);
        },

        render: function (model) {
            Container.prototype.render.call(this);

            var compile = _.template(this.template());

            var html = compile();
            this.$el.append(html);

            if (this.opts.parent === undefined) {
                this.opts.parent = this.$el;
            }

            this.loadDatepicker();

            this.loadAllTable();

            return this;
        },
        template: function () {
            return Template;
        },

        loadAllTable: function () {
            this.loadDeviceType("eng-device-type");
            this.loadDeviceModel("eng-device-model");
            this.loadDevicebrand("eng-device-brand");
            //this.loadResolutions("eng-device-resolutions");
            this.loadOs("eng-device-os");
            this.loadBrowser("eng-device-browser");

            this.opts.data["searchObject"].isNew = false;
        },
        loadDeviceType: function (id) {
            var area = this.opts.parent.find("#" + id);
            area.html("");

            var pluginData = ENG.loadJSON('{"data":[{"Type": "Desktop", "Unique Visitors": "1281"},{"Type": "Smartphone", "Unique Visitors": "36"},{"Type": "Tablet", "Unique Visitors": "21"}]}');

            var table = new TableView();
            table.setVisibility(true);
            table.data = pluginData.data;
            table.title = "Type";
            table.columnHeader = pluginData.columnHeader;
            table.isShowChooseArea = true;

            table.opts.searchObject = this.opts.data["searchObject"];
            table.opts.buttonType = this.opts.buttonType;

            area.append(table.$el);
            table.render();
        },
        loadDeviceModel: function (id) {
            var area = this.opts.parent.find("#" + id);
            area.html("");

            var me = this;
            var table = new TableView();
            table.setVisibility(true);

            table.title = "Model";
            table.name = "Model";
            table.columnHeader = [{
                name: "Model",
                isShow: true
            }, {
                name: "PageView",
                isShow: true
            }];
            table.isShowChooseArea = true;

            table.baseApiUrl = "/umbraco/api/EngQuery/GetPageviewByDevice";

            table.opts.searchObject = this.opts.data["searchObject"];
            table.opts.buttonType = this.opts.buttonType;

            area.append(table.$el);
            table.render();
        },
        loadDevicebrand: function (id) {
            var area = this.opts.parent.find("#" + id);
            area.html("");
            var pluginData = ENG.loadJSON('{"data":[{"Brand": "English", "Unique Visitors": "143"},{"Brand": "German", "Unique Visitors": "11"},{"Brand": "French", "Unique Visitors": "8"}]}');

            var table = new TableView();
            table.setVisibility(true);
            table.data = pluginData.data;
            table.title = "Brand";
            table.columnHeader = pluginData.columnHeader;
            table.isShowChooseArea = true;

            table.opts.searchObject = this.opts.data["searchObject"];
            table.opts.buttonType = this.opts.buttonType;

            area.append(table.$el);
            table.render();
        },
        loadResolutions: function (id) {
            var area = this.opts.parent.find("#" + id);
            area.html("");

            var me = this;
            var table = new TableView();
            table.setVisibility(true);

            table.title = "Resolutions";
            table.name = "Resolutions";
            table.columnHeader = [{
                name: "Resolution",
                isShow: true
            }, {
                name: "PageView",
                isShow: true
            }];
            table.isShowChooseArea = true;

            table.baseApiUrl = "/umbraco/api/EngQuery/GetPageviewByScreenResolution";

            table.opts.searchObject = this.opts.data["searchObject"];
            table.opts.buttonType = this.opts.buttonType;

            area.append(table.$el);
            table.render();
        },
        loadOs: function (id) {
            var area = this.opts.parent.find("#" + id);
            area.html("");

            var me = this;
            var table = new TableView();
            table.setVisibility(true);

            table.title = "Operating systems";
            table.name = "OperatingSystem";
            table.columnHeader = [{
                name: "Operating system version",
                isShow: true
            }, {
                name: "PageView",
                isShow: true
            }];
            table.isShowChooseArea = true;

            table.baseApiUrl = "/umbraco/api/EngQuery/GetPageviewByOs";

            table.opts.searchObject = this.opts.data["searchObject"];
            table.opts.buttonType = this.opts.buttonType;

            area.append(table.$el);
            table.render();
        },
        loadBrowser: function (id) {
            var area = this.opts.parent.find("#" + id);
            area.html("");

            var me = this;
            var table = new TableView();
            table.setVisibility(true);

            table.title = "Browsers";
            table.name = "Browsers";
            table.columnHeader = [{
                name: "Browser",
                isShow: true
            }, {
                name: "PageView",
                isShow: true
            }];
            table.isShowChooseArea = true;

            table.baseApiUrl = "/umbraco/api/EngQuery/GetPageviewByBrowser";

            table.opts.searchObject = this.opts.data["searchObject"];
            table.opts.buttonType = this.opts.buttonType;

            area.append(table.$el);
            table.render();
        },

        loadDatepicker: function () {
            //this.$el.find(".eng-datepicker-area").
            var me = this;
            var dropdownDatepicker = new DropdownDatepickerView();
            this.opts.data["searchObject"] = dropdownDatepicker.initStoreObject(this.opts.data["searchObject"]);

            this.listenTo(dropdownDatepicker, "dateChange", function (obj) {
                me.opts.data["searchObject"].startDate = obj.startDate;
                me.opts.data["searchObject"].endDate = new Date(obj.endDate.getFullYear(), obj.endDate.getMonth(), obj.endDate.getDate());

                me.opts.data["searchObject"].selectedValue = obj.selectedValue;
                me.opts.data["searchObject"].selectedText = obj.selectedText;

                me.opts.data["searchObject"].isNew = true;
                if (obj.selectedValue == -1) {
                    me.opts.data["searchObject"].selectedText = ENG.getDateStringMMDDYYYY(obj.startDate) + " - " + ENG.getDateStringMMDDYYYY(obj.endDate);
                }

                me.loadAllTable();

            });

            dropdownDatepicker.storeObject = this.opts.data["searchObject"];
            this.$el.find('.eng-datepicker-all-report').append(dropdownDatepicker.render().$el);
        },
    });
});