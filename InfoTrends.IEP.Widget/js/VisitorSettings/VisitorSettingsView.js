define([
    'text!js/VisitorSettings/VisitorSettings.html',
    'js/Component',
    'js/Table/TableView',
    'js/DropdownDatepicker/DropdownDatepickerView',
],

function (Template, Component, TableView, DropdownDatepickerView) {
    return Component.extend({
        data: [],
        events: {
            'click .eng-chooseReport': 'showReport'
        },

        constructor: function (options) {
            var opts = ENG.$.extend(true, {
                data: [],
            }, options);


            Component.prototype.constructor.call(this, opts);
            this.$el.addClass('eng-visitor-settings');
            this.$el.html('');

            var urlCss = ENG.DOMAIN + "/js/VisitorSettings/VisitorSettings.css";
            ENG.loadCss(urlCss);
        },


        render: function (params) {
            Component.prototype.render.call(this);

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
            this.loadSettingsPlugin("eng-settings-plugin");
            this.loadSettingsConfiguration("eng-settings-resolution");
            this.loadSettingsResolution("eng-settings-configuration");

            this.opts.data["searchObject"].isNew = false;
        },
        loadSettingsPlugin: function (id) {
            var area = this.opts.parent.find("#" + id);
            area.html("");

            var pluginData = ENG.loadJSON('{"data":[{"PLUGIN": "Cookie", "% VISITS": "100%","VISITS": "1118"},{"PLUGIN": "Flash", "% VISITS": "89%","VISITS": "1000"},{"PLUGIN": "Java", "% VISITS": "85%","VISITS": "955"}]}');
            var table = new TableView();
            table.setVisibility(true);
            table.data = pluginData.data;
            table.title = "Plugins";
            table.isShowChooseArea = false;
            table.columnHeader = pluginData.columnHeader;

            table.opts.searchObject = this.opts.data["searchObject"];
            table.opts.buttonType = this.opts.buttonType;

            area.append(table.$el);
            table.render();
        },
        loadSettingsResolution: function (id) {
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
        loadSettingsConfiguration: function (id) {
            var area = this.opts.parent.find("#" + id);
            area.html("");

            var configurationData = ENG.loadJSON('{"data":[{"Configuration": "Windows Phone / IE Mobile / 480x800", "Unique Visitors": "289"},{"Configuration": "Windows / Vivaldi / 1680x1050", "Unique Visitors": "87"},{"Configuration": "Windows / Safari / 1680x1050", "Unique Visitors": "181"}]}');
            var table = new TableView();
            table.setVisibility(true);
            table.title = "Configuration";
            table.data = configurationData.data;
            table.columnHeader = configurationData.columnHeader;
            table.isShowChooseArea = true;

            table.opts.searchObject = this.opts.data["searchObject"];
            table.opts.buttonType = this.opts.buttonType;

            area.append(table.$el);
            table.render();
        },

        loadDatepicker: function () {
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