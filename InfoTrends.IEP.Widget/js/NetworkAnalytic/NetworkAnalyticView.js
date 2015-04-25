define([
    'text!js/NetworkAnalytic/NetworkAnalytic.html',
    'js/Component',
    'js/NetworkAnalytic/NetworkAnalyticModel',
    'js/DropdownDatepicker/DropdownDatepickerView',
    'js/Table/TableView',
    'js/MessageBox/MessageBoxView',
    'js/VisitorLocations/VisitorLocationsView',
    'js/VisitorSettings/VisitorSettingsView',
    'js/NetworkDevices/NetworkDeviceView',
    'js/SelectLayoutBox/SelectLayoutBoxView',

    //'lib/jquery.flot.min'


],
function (Template, Component, NetworkAnalyticModel, DropdownDatepickerView, TableView, MessageBox, VisitorLocationsView, VisitorSettingsView, NetworkDevicesView, SelectLayoutBoxView) {
    return Component.extend({
        data: [],
        events: {
            'change #eng-dashboard-config': 'dropDownChange',
        },
        constructor: function (options) {
            var opts = ENG.$.extend(true, {
                data: [],
                defaultButtonType: null
            }, options);

            Component.prototype.constructor.call(this, opts);
            this.$el.addClass('eng-network-analytic');
        },

        render: function (model) {
            var me = this;

            Component.prototype.render.call(this);

            var compile = _.template(this.template());

            var html = compile();
            this.$el.append(html);


            this.loadDatepicker();

            //load Manage Dashboard dropDown
            this.loadManageDashboardDropDown();

            //load table
            this.opts.defaultButtonType = [ENG.enum.buttonType.collapse, ENG.enum.buttonType.close];
            this.loadAllTable();

            this.loadDragDrop();

            return this;

        },
        template: function () {
            return Template;
        },

        loadAllTable: function () {
            var data = {
                columns: [{
                    order: 1,
                    size: 6,
                    reports: [{
                        order: 1,
                        name: ENG.enum.reportType.websiteActivity,
                        collapse: false,
                    }, {
                        order: 2,
                        name: ENG.enum.reportType.visitorUserAgent,
                        collapse: false,
                    }]
                }, {
                    order: 2,
                    size: 6,
                    reports: [{
                        order: 1,
                        name: ENG.enum.reportType.quickStats,
                        collapse: false,
                    }, {
                        order: 2,
                        name: ENG.enum.reportType.networkActivity,
                        collapse: false,
                    }]
                }]
            };

            //sort column by order
            data.columns = _.sortBy(data.columns, "order");

            //sort report by order
            _.each(data.columns, function (obj) {
                obj.reports = _.sortBy(obj.reports, "order");
            });

            this.drawReport(data);


            //this.loadInteractiveChart();
            //this.loadDonutChart();
            //this.loadQuickStats();
            //this.loadMapChart();

            this.opts.data["searchObject"].isNew = false;
        },
        drawReport: function (data) {
            var me = this;
            _.each(data.columns, function (column) {
                //add column
                var colId = "eng-col-id-" + column.order;
                var colClass = "eng-col eng-col-" + column.size;
                var columnHtml = "<div class='" + colClass + "' id='" + colId + "'></div>"
                me.$el.find(".eng-dashboard-content").append(columnHtml);

                //add report
                _.each(column.reports, function (report) {
                    //add div wrapper
                    var rpId = "eng-report-" + report.name.trim().split(' ').join('-');
                    var reportDiv = "<div class='eng-panel-table' id='" + rpId + "'></div>";
                    me.$el.find("#" + colId).append(reportDiv);

                    var visitorLocations = new VisitorLocationsView();
                    var visitorSettings = new VisitorSettingsView();
                    var networkDevices = new NetworkDevicesView();

                    visitorLocations.opts.data["searchObject"] = me.opts.data["searchObject"];
                    visitorLocations.opts.parent = me.$el;
                    visitorLocations.opts.buttonType = me.opts.defaultButtonType;

                    visitorSettings.opts.data["searchObject"] = me.opts.data["searchObject"];
                    visitorSettings.opts.parent = me.$el;
                    visitorSettings.opts.buttonType = me.opts.defaultButtonType;

                    networkDevices.opts.data["searchObject"] = me.opts.data["searchObject"];
                    networkDevices.opts.parent = me.$el;
                    networkDevices.opts.buttonType = me.opts.defaultButtonType;

                    switch (report.name) {
                        //network analytic
                        case ENG.enum.reportType.websiteActivity:
                            me.loadInteractiveChart(rpId);
                            break;
                        case ENG.enum.reportType.visitorUserAgent:
                            me.loadDonutChart(rpId);
                            break;
                        case ENG.enum.reportType.quickStats:
                            me.loadQuickStats(rpId);
                            break;
                        case ENG.enum.reportType.networkActivity:
                            me.loadMapChart(rpId);
                            break;

                            //visitorLocations
                        case ENG.enum.reportType.country:
                            visitorLocations.loadCountry(rpId);
                            break;
                        case ENG.enum.reportType.city:
                            visitorLocations.loadCity(rpId);
                            break;
                        case ENG.enum.reportType.region:
                            visitorLocations.loadLocationRegion(rpId);
                            break;
                        case ENG.enum.reportType.browserLanguag:
                            visitorLocations.loadbrowserLanguage(rpId);
                            break;
                        case ENG.enum.reportType.continent:
                            visitorLocations.loadContinent(rpId);
                            break;
                        case ENG.enum.reportType.provider:
                            visitorLocations.loadProvider(rpId);
                            break;

                            //visitorSettings
                        case ENG.enum.reportType.plugin:
                            visitorSettings.loadSettingsPlugin(rpId);
                            break;
                        case ENG.enum.reportType.configuration:
                            visitorSettings.loadSettingsConfiguration(rpId);
                            break;
                        case ENG.enum.reportType.resolution:
                            visitorSettings.loadSettingsResolution(rpId);
                            break;

                            //networkDevices
                        case ENG.enum.reportType.deviceType:
                            networkDevices.loadDeviceType(rpId);
                            break;
                        case ENG.enum.reportType.deviceModel:
                            networkDevices.loadDeviceModel(rpId);
                            break;
                        case ENG.enum.reportType.deviceBrand:
                            networkDevices.loadDevicebrand(rpId);
                            break;
                        case ENG.enum.reportType.operatingSystem:
                            networkDevices.loadOs(rpId);
                            break;
                        case ENG.enum.reportType.browser:
                            networkDevices.loadBrowser(rpId);
                            break;
                        default:
                    }

                    if (report.collapse) {
                        me.$el.find("#" + rpId).find(".eng-panel-body").toggle();
                    }
                });
            });
        },

        //dropdown
        loadManageDashboardDropDown: function () {
            var html = "<select id='eng-dashboard-config'>";
            html += "   <optgroup label='Add report'>";

            _.each(ENG.enum.reportType, function (v, k) {
                html += "       <option value='" + v + "'>" + v + "</option>";
            });

            html += "   </optgroup>";
            html += "   <optgroup label='Congiguration'>";
            html += "       <option value='reset'>Reset dashboard</option>";
            html += "       <option value='change'>Change dashboard layout</option>";
            html += "   </optgroup>";
            html += "</select>";

            this.$el.find(".eng-dashboard-dropdown-wrapper").append(html);
        },

        saveData: function () {
            var data = {
                columns: []
            };
            for (var i = 0; i < this.$el.find(".eng-col").length; i++) {
                var str = ENG.$(this.$el.find(".eng-col")[0]).attr("class").split(" ")[1];

                var column = {
                    clientId: ENG.cid,
                    order: (i + 1),
                    size: str[str.length - 1],
                    reports: []
                };
                for (var j = 0; j < this.$el.find("#eng-col-id-" + i).children().length; j++) {
                    var name = this.$el.find("#eng-col-id-" + i).children()[0].id.replace("eng-report-", "").split("-").join(" ");
                    var isCollapse = ENG.$(this.$el.find("#eng-col-id-" + j).children()[0]).find(".eng-panel-body").css("display") === "none";
                    column.reports.push({
                        order: j,
                        name: name,
                        collapse: isCollapse,
                    });
                }

                data.columns.push(column);
            }
        },

        dropDownChange: function (e) {
            var me = this;
            if (e.target.value === "change") {
                var selectLayoutBoxView = new SelectLayoutBoxView();

                selectLayoutBoxView.opts.params = {
                    headerText: "Select your dashboard layout",
                    messageText: ""
                };
                selectLayoutBoxView.opts.size = "small";
                selectLayoutBoxView.opts.type = "yes-no";
                selectLayoutBoxView.opts.data = selectLayoutBoxView;

                selectLayoutBoxView.showMessageBox();
                this.listenTo(selectLayoutBoxView, "cancel", function (data) {
                });
                this.listenTo(selectLayoutBoxView, "ok", function (data) {
                    debugger;
                    var selected = data.$el.find("#eng-layout-select").children(".ui-selected");
                    if (selected.length > 0) {
                        var arr = (data.$el.find("#eng-layout-select").children(".ui-selected").data("value") + "").split("_");

                    }

                    //save new layout

                    //reload table

                });
            } else if (e.target.value === "reset") {

            } else {

            }
        },

        //datepicker area
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
                me.loadAllChart();

            });

            dropdownDatepicker.storeObject = this.opts.data["searchObject"];
            this.$el.find('.eng-datepicker-all-report').append(dropdownDatepicker.render().$el);

        },

        //dragdrop area
        loadDragDrop: function () {
            var me = this;
            this.$el.find(".eng-col").sortable({
                connectWith: ".eng-col",
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
                    me.saveData();
                }
            });
        },

        //chart
        loadInteractiveChart: function (id) {
            var area = this.$el.find("#" + id);
            area.html("");

            var me = this;
            var table = new TableView();
            table.setVisibility(true);

            table.title = "Website Activity";
            table.name = "Interactive";

            table.opts.searchObject = this.opts.data["searchObject"];
            table.opts.chartType = [ENG.enum.chartType.interactive];
            table.opts.isShowChartIcon = false;
            table.opts.buttonType = this.opts.defaultButtonType;

            area.append(table.$el);
            table.render();
        },
        loadDonutChart: function (id) {
            var area = this.$el.find("#" + id);
            area.html("");

            var me = this;
            var table = new TableView();
            table.setVisibility(true);

            table.title = "Visitor User Agent";
            table.name = "DonutChart";
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
            table.opts.chartType = [ENG.enum.chartType.donut];
            table.opts.isShowChartIcon = false;
            area.append(table.$el);
            table.render();

        },
        loadQuickStats: function (id) {
            var area = this.$el.find("#" + id);
            area.html("");

            var me = this;
            var table = new TableView();
            table.setVisibility(true);

            table.title = "Quick Stats";
            table.name = "QuickStats";

            table.opts.searchObject = this.opts.data["searchObject"];
            table.opts.chartType = [ENG.enum.chartType.quickStats];
            table.opts.isShowChartIcon = false;
            area.append(table.$el);
            table.render();
        },
        loadMapChart: function (id) {
            var area = this.$el.find("#" + id);
            area.html("");

            var me = this;
            var table = new TableView();
            table.setVisibility(true);

            table.title = "Network activity";
            table.name = "MapChart";

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
            table.opts.isShowChartIcon = false;
            area.append(table.$el);
            table.render();
        },
    });
});