define([
    'text!js/VisitorLocations/VisitorLocations.html',
    'js/Component',
    'js/Table/TableView',
    'js/Loading/LoadingView',
    'js/DropdownDatepicker/DropdownDatepickerView',

],

function (Template, Component, TableView, LoadingView, DropdownDatepickerView) {
    return Component.extend({
        data: [],
        constructor: function (options) {
            var opts = ENG.$.extend(true, {
                data: [],
                parentView: null,
            }, options);


            Component.prototype.constructor.call(this, opts);
            this.$el.addClass('eng-visitor-locations');
            this.$el.html('');
            var urlCss = ENG.DOMAIN + "/js/VisitorLocations/VisitorLocations.css";
            ENG.loadCss(urlCss);
        },
        render: function (params) {
            Component.prototype.render.call(this);

            var compile = _.template(this.template());

            var html = compile();
            this.$el.append(html);

            if (this.opts.parentView === undefined || this.opts.parentView === null) {
                this.opts.parentView = this;
            }

            this.loadDatepicker();

            this.loadAllTable();

            this.loadDragDrop();

            return this;
        },
        template: function () {
            return Template;
        },
        loadAllTable: function () {
            this.loadVisitorMap("eng-map");
            this.loadCountry("eng-location-country");
            //this.loadLocationRegion("eng-location-regions");
            this.loadContinent("eng-location-continent");
            this.loadCity("eng-location-city");
            this.loadbrowserLanguage("eng-location-browser-language");
            this.loadProvider("eng-location-provider");

            this.opts.data["searchObject"].isNew = false;
        },

        loadVisitorMap: function (id) {
            var area = this.opts.parentView.$el.find("#" + id);
            area.html("");

            var me = this;
            var table = new TableView();
            table.setVisibility(true);

            table.title = "CountryMap";
            table.name = "CountryMap";
            table.columnHeader = [{
                name: "Country",
                isShow: true
            }, {
                name: "PageView",
                isShow: true
            }];

            table.baseApiUrl = "/umbraco/api/EngQuery/GetPageviewByCountry";

            table.opts.searchObject = this.opts.data["searchObject"];
            table.opts.chartType = [ENG.enum.chartType.map];
            table.opts.buttonType = this.opts.buttonType;
            table.opts.isShowChartIcon = false;

            area.append(table.$el);
            table.render();
        },
        loadCountry: function (id) {
            var area = this.opts.parentView.$el.find("#" + id);
            area.html("");

            var me = this;
            var table = new TableView();
            table.setVisibility(true);

            table.title = "Country";
            table.name = ENG.enum.reportType.country;
            table.columnHeader = [{
                name: "Country",
                isShow: true
            }, {
                name: "PageView",
                isShow: true
            }];

            table.baseApiUrl = "/umbraco/api/EngQuery/GetPageviewByCountry";

            table.opts.searchObject = this.opts.data["searchObject"];
            table.opts.buttonType = this.opts.buttonType;
            table.opts.parentView = this.opts.parentView;

            area.append(table.$el);
            table.render();
        },
        loadLocationRegion: function (id) {
            var area = this.opts.parentView.$el.find("#" + id);
            area.html("");

            var pluginData = ENG.loadJSON('{"data":[{"Region": "Unknow", "Unique Visitors": "143"},{"Region": "United States", "Unique Visitors": "27"},{"Region": "India", "Unique Visitors": "17"}]}');

            var table = new TableView();
            table.setVisibility(true);
            table.data = pluginData.data;
            table.title = "Region";
            table.name = ENG.enum.reportType.region;
            table.columnHeader = pluginData.columnHeader;

            table.opts.searchObject = this.opts.data["searchObject"];
            table.opts.buttonType = this.opts.buttonType;
            table.opts.parentView = this.opts.parentView;

            area.append(table.$el);
            table.render();
        },
        loadContinent: function (id) {
            var area = this.opts.parentView.$el.find("#" + id);
            area.html("");

            var me = this;
            var table = new TableView();
            table.setVisibility(true);

            table.title = ENG.enum.reportType.continent;
            table.name = ENG.enum.reportType.continent;
            table.columnHeader = [{
                name: "Continent",
                isShow: true
            }, {
                name: "PageView",
                isShow: true
            }];

            table.baseApiUrl = "/umbraco/api/EngQuery/GetPageviewByCountry";

            table.opts.searchObject = this.opts.data["searchObject"];
            table.opts.buttonType = this.opts.buttonType;
            table.opts.parentView = this.opts.parentView;

            area.append(table.$el);
            table.render();
        },
        loadCity: function (id) {
            var area = this.opts.parentView.$el.find("#" + id);
            area.html("");

            var me = this;
            var table = new TableView();
            table.setVisibility(true);

            table.title = "City";
            table.name = ENG.enum.reportType.city;
            table.columnHeader = [{
                name: "City",
                isShow: true
            }, {
                name: "PageView",
                isShow: true
            }];

            table.baseApiUrl = "/umbraco/api/EngQuery/GetPageviewByCity";
            table.opts.buttonType = this.opts.buttonType;
            table.opts.parentView = this.opts.parentView;

            table.opts.searchObject = this.opts.data["searchObject"];

            area.append(table.$el);
            table.render();
        },
        loadbrowserLanguage: function (id) {
            var area = this.opts.parentView.$el.find("#" + id);
            area.html("");

            var pluginData = ENG.loadJSON('{"data":[{"Language": "English", "Unique Visitors": "143"},{"Language": "German", "Unique Visitors": "11"},{"Language": "French", "Unique Visitors": "8"}]}');

            var table = new TableView();
            table.setVisibility(true);
            table.data = pluginData.data;
            table.title = "Browser Language";
            table.name = ENG.enum.reportType.browserLanguage;
            table.columnHeader = pluginData.columnHeader;


            table.opts.searchObject = this.opts.data["searchObject"];
            table.opts.buttonType = this.opts.buttonType;
            table.opts.parentView = this.opts.parentView;

            area.append(table.$el);
            table.render();
        },
        loadProvider: function (id) {
            var area = this.opts.parentView.$el.find("#" + id);
            area.html("");

            var pluginData = ENG.loadJSON('{"data":[{"Provider": "Deutsche Telecom AG", "Unique Visitors": "421"},{"Provider": "Comcat cable", "Unique Visitors": "124"},{"Provider": "Free SAS", "Unique Visitors": "54"}]}');

            var table = new TableView();
            table.setVisibility(true);
            table.data = pluginData.data;
            table.title = "Provider";
            table.name = ENG.enum.reportType.provider;
            table.columnHeader = pluginData.columnHeader;


            table.opts.searchObject = this.opts.data["searchObject"];
            table.opts.buttonType = this.opts.buttonType;
            table.opts.parentView = this.opts.parentView;

            area.append(table.$el);
            table.render();
        },

        loadDatepicker: function () {
            //this.$el.find(".eng-datepicker-area").
            var me = this;
            var dropdownDatepicker = new DropdownDatepickerView();
            if (this.opts.data["searchObject"] === undefined) {
                this.opts.data["searchObject"] = dropdownDatepicker.initStoreObject(this.opts.data["searchObject"]);
            }

            this.listenTo(dropdownDatepicker, "dateChange", function (obj) {
                me.opts.data["searchObject"].startDate = obj.startDate;
                me.opts.data["searchObject"].endDate = new Date(obj.endDate.getFullYear(), obj.endDate.getMonth(), obj.endDate.getDate());

                me.opts.data["searchObject"].selectedValue = obj.selectedValue;
                me.opts.data["searchObject"].selectedText = obj.selectedText;

                me.opts.data["searchObject"].isNew = true;
                if (obj.selectedValue === -1) {
                    me.opts.data["searchObject"].selectedText = ENG.getDateStringMMDDYYYY(obj.startDate) + " - " + ENG.getDateStringMMDDYYYY(obj.endDate);
                }

                me.loadAllTable();

            });

            dropdownDatepicker.storeObject = this.opts.data["searchObject"];
            this.$el.find('.eng-datepicker-all-report').append(dropdownDatepicker.render().$el);
        },
        loadDragDrop: function () {
            this.$el.find(".eng-haft-screen").sortable({
                connectWith: ".eng-haft-screen",
                handle: ".eng-tableTitle-bar",
                placeholder: "eng-placeholder",
                start: function (event, ui) {
                    ENG.$("div.eng-placeholder").css("height", ui.item.height() + 8);
                    if (ENG.$(window).width() > 768) {
                        ui.item.css("width", "49%");
                    } else {
                        ui.item.css("width", "98%");
                    }
                },
                stop: function (event, ui) {
                    saveData();
                }
            });
        },
    });
});