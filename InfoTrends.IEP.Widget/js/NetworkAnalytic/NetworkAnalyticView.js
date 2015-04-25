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
                defaultButtonType: null,
                columnsData: null
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

            this.getColumnsData(function () {
                //load Manage Dashboard dropDown
                me.loadManageDashboardDropDown();

                //load table
                me.opts.defaultButtonType = [ENG.enum.buttonType.collapse, ENG.enum.buttonType.close];
                me.loadAllTable();
            });

            return this;

        },
        template: function () {
            return Template;
        },

        getColumnsData: function (callBack) {
            //this.setDefaultDashboard();
            //get data from api
            var me = this;
            var url = _.template("<%=domain%>/umbraco/api/DashboardSetting/GetDashboard?ClientId=<%=cliectId%>");

            var apiUrl = url({
                domain: ENG.ApiDomain,
                cliectId: ENG.cid,
            });

            Xdr.ajax({
                url: apiUrl
            }, function (success, response) {
                if (success && success.success) {
                    if (ENG.Utils.checkObjExist(success, 'data.data')) {
                        var data = {
                            columns: [],
                            columnsSize: ""
                        }
                        _.each(success.data.data, function (obj) {
                            debugger;
                            var index = me.getColumnIndex(obj.ColumnOrder, data.columns);
                            if (index == -1) {
                                //no column existed
                                data.columns.push({
                                    order: obj.ColumnOrder,
                                    size: obj.ColumnSize,
                                    reports: [{
                                        order: obj.ReportOrder,
                                        name: obj.ReportName,
                                        collapse: obj.ReportCollapse,
                                    }]
                                });
                            } else {
                                data.columns[index].reports.push({
                                    order: obj.ReportOrder,
                                    name: obj.ReportName,
                                    collapse: obj.ReportCollapse,
                                });
                            }
                        });
                        me.opts.columnsData = data;
                    } else {
                        me.setDefaultDashboard();
                    }

                    var size = "";
                    _.each(me.opts.columnsData.columns, function (obj) {
                        size += obj.size + "_";
                    });
                    me.opts.columnsData.columnsSize = size.substring(0, size.length - 1);

                    callBack();
                }
            });


        },
        getColumnIndex: function (columnOrder, columns) {
            var index = -1;
            for (var i = 0; i < columns.length; i++) {
                if (columns[i].order == columnOrder) {
                    index = i;
                    break;
                }
            }
            return index;
        },
        setDefaultDashboard: function () {
            this.opts.columnsData = {
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
                }],
                columnsSize: ""
            };
        },
        loadAllTable: function () {
            //sort column by order
            this.opts.columnsData.columns = _.sortBy(this.opts.columnsData.columns, "order");

            //sort report by order
            _.each(this.opts.columnsData.columns, function (obj) {
                obj.reports = _.sortBy(obj.reports, "order");
            });

            this.drawReport(this.opts.columnsData);

            this.loadDragDrop();

            this.opts.data["searchObject"].isNew = false;
        },
        drawReport: function (data) {
            var me = this;
            me.$el.find(".eng-dashboard-content").html('');
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
            html += "   <option value='default'>Manage dashboard</option>";

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
                var str = ENG.$(this.$el.find(".eng-col")[i]).attr("class").split(" ")[1];

                var column = {
                    clientId: ENG.cid,
                    order: (i + 1),
                    size: str[str.length - 1],
                    reports: []
                };
                for (var j = 0; j < this.$el.find("#eng-col-id-" + (i + 1)).children().length; j++) {
                    var name = this.$el.find("#eng-col-id-" + (i + 1)).children()[j].id.replace("eng-report-", "").split("-").join(" ");
                    var isCollapse = ENG.$(this.$el.find("#eng-col-id-" + j).children()[j]).find(".eng-panel-body").css("display") === "none";
                    column.reports.push({
                        order: j + 1,
                        name: name,
                        collapse: isCollapse,
                    });
                }

                data.columns.push(column);
            }

            this.opts.columnsData = data;

            //save
            this.saveToDb();
        },
        saveToDb: function (data) {
            //Xdr.ajax({
            //    url: ENG.ApiDomain + '/umbraco/api/DashboardSetting/SaveDashboard',
            //    data: { '': this.opts.columnsData.columns },
            //    type: "POST"
            //}, function (response) {
            //});
        },

        dropDownChange: function (e) {
            var me = this;
            if (e.target.value === "change") {
                //add message box
                var selectLayoutBoxView = new SelectLayoutBoxView();

                selectLayoutBoxView.opts.params = {
                    headerText: "Select your dashboard layout",
                    messageText: ""
                };
                selectLayoutBoxView.opts.size = "small";
                selectLayoutBoxView.opts.type = "yes-no";
                selectLayoutBoxView.opts.data = selectLayoutBoxView;

                selectLayoutBoxView.showMessageBox();

                //set selected layout
                selectLayoutBoxView.$el.find("[data-value=" + this.opts.columnsData.columnsSize + "]").addClass("ui-selected");

                //event
                this.listenTo(selectLayoutBoxView, "cancel", function (data) {
                });
                this.listenTo(selectLayoutBoxView, "ok", function (data) {
                    var selected = data.$el.find("#eng-layout-select").children(".ui-selected");
                    if (selected.length > 0) {
                        var arr = (selected.data("value") + "").split("_");
                        var oldColumnsCount = this.opts.columnsData.columns.length;

                        if (oldColumnsCount < arr.length) {
                            //increase column count -> add new column
                            for (var i = oldColumnsCount; i < arr.length; i++) {
                                this.opts.columnsData.columns.push({
                                    order: i + 1,
                                    size: 0,
                                    reports: []
                                });
                            }


                            for (var i = 0; i < arr.length; i++) {
                                this.opts.columnsData.columns[i].size = arr[i];
                            }
                        } else if (oldColumnsCount > arr.length) {
                            //decrease column count -> move report to first column
                            for (var i = arr.length; i < oldColumnsCount; i++) {
                                for (var j = 0; j < this.opts.columnsData.columns[i].reports.length; j++) {
                                    var report = this.opts.columnsData.columns[i].reports[j];
                                    this.opts.columnsData.columns[0].reports.push(report);
                                }
                            }
                            //remove columns  
                            this.opts.columnsData.columns.splice(arr.length, oldColumnsCount - arr.length);

                        }

                        //change column size
                        for (var i = 0; i < arr.length; i++) {
                            this.opts.columnsData.columns[i].size = arr[i];
                        }
                        this.opts.columnsData.columnsSize = selected.data("value") + "";
                    }
                    //save new layout
                    me.saveToDb();

                    //reload table
                    me.loadAllTable();
                });
            } else if (e.target.value === "reset") {
                me.loadAllTable();
                me.saveToDb();
                me.loadAllTable();
            } else {
                me.opts.columnsData.columns[0].reports.push({
                    order: this.opts.columnsData.columns[0].reports.length + 1,
                    name: e.target.value,
                    collapse: false,
                });
                me.saveToDb();
                me.loadAllTable();
            }

            e.target.value = "default";
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
            table.opts.buttonType = this.opts.defaultButtonType;

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
            table.opts.buttonType = this.opts.defaultButtonType;

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
            table.opts.buttonType = this.opts.defaultButtonType;

            area.append(table.$el);
            table.render();
        },
    });
});