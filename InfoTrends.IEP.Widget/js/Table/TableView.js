define([
    'text!js/Table/Table.html',
    'js/DropdownDatepicker/DropdownDatepickerView',
    'js/Component'
], function (Template, DropdownDatepickerView, Component) {

    return Component.extend({
        columnHeader: null,
        data: [],
        title: null,
        displayData: null,
        isShowTable: null,
        name: null,
        baseApiUrl: null,
        filterData: null,
        dropdownDatepicker: null,
        events: {
            'click .eng-chooseReport': 'showReport',
            'click .eng-sort-header': 'sort',
            'click .eng-table-paging a': 'pageChange',
            'keyup .eng-search': 'search',
            'click .eng-btn': 'headerBtnClick',
        },

        constructor: function (options) {
            var opts = ENG.$.extend(true, {
                searchObject: null,
                chartType: null,
                buttonType: null,
                isShowChartIcon: null,
                parentView: null,
            }, options);

            Component.prototype.constructor.call(this, opts);

            this.$el.addClass('eng-table');
            var urlCss = ENG.DOMAIN + "/js/Table/Table.css";
            ENG.loadCss(urlCss);
        },
        render: function (params) {
            Component.prototype.render.call(this);

            var me = this;
            //set default data
            if (this.opts.chartType === null) {
                this.opts.chartType = [ENG.enum.chartType.table, ENG.enum.chartType.donut, ENG.enum.chartType.vertical];
                //this.opts.chartType = [ENG.enum.chartType.donut, ENG.enum.chartType.vertical];
                //this.opts.chartType = [ENG.enum.chartType.interactive];
                //this.opts.chartType = [ENG.enum.chartType.quickStats];
                //this.opts.chartType = [ENG.enum.chartType.map];
                //this.opts.chartType = [ENG.enum.chartType.dynamicColumnTable];
            }

            if (this.opts.buttonType === undefined || this.opts.buttonType === null) {
                this.opts.buttonType = [ENG.enum.buttonType.collapse];
            }

            if (this.opts.isShowChartIcon == null) {
                this.opts.isShowChartIcon = true;
            }


            switch (me.opts.chartType[0]) {
                case ENG.enum.chartType.interactive:
                    me.reloadTable();
                    //me.drawInteractiveChart();
                    var interactiveChart = me.$el.find(".eng-interactive-chart");
                    interactiveChart.show();
                    me.drawInteractiveChart(interactiveChart);

                    me.drawChartIcon();
                    me.drawActionButton();

                    ENG.$(window).bind('resize.interactive_' + me.name, function (event) {
                        me.drawInteractiveChart(interactiveChart);
                    });
                    break;

                case ENG.enum.chartType.quickStats:
                    me.reloadTable();

                    var quickStats = me.$el.find(".eng-quick-stats");
                    quickStats.show();

                    me.drawAnalyticDetail();

                    me.drawChartIcon();
                    me.drawActionButton();
                    break;

                case ENG.enum.chartType.dynamicColumnTable:
                    me.isShowTable = true;
                    me.initPageData();
                    me.calculateData();

                    break;
                default:
                    me.drawAllOtherChart();
                    break;
            }

            return this;
        },
        template: function () {
            return Template;
        },

        //drawAllOtherChart
        drawAllOtherChart: function () {
            var me = this;
            this.loadData(function () {
                if (me.opts.chartType[0] === ENG.enum.chartType.table) {
                    me.isShowTable = true;

                    me.initPageData();
                    me.calculateData();

                    //me.drawPageButton();
                    //me.$el.find(".eng-search").val(me.filterData.searchKey);
                } else {
                    me.isShowTable = false;

                    me.reloadTable();
                    switch (me.opts.chartType[0]) {
                        case ENG.enum.chartType.donut:
                            var pieChart = me.$el.find(".eng-pie-chart");
                            pieChart.show();
                            me.drawPieChart(pieChart);
                            window.onresize = function (event) {
                                me.drawPieChart(pieChart)
                            }
                            break;
                        case ENG.enum.chartType.vertical:
                            var verticalChart = me.$el.find(".eng-vertical-bar");
                            verticalChart.show();
                            me.drawVerticalChart(verticalChart);
                            window.onresize = function (event) {
                                me.drawVerticalChart(verticalChart)
                            }
                            break;
                        case ENG.enum.chartType.map:
                            var mapChart = me.$el.find(".eng-map-chart");
                            mapChart.show();
                            me.drawMapChart(mapChart);

                            ENG.$(window).bind('resize.map_' + me.name, function (event) {
                                me.drawMapChart(mapChart)
                            });
                            break;
                        default:
                    }
                    me.drawChartIcon();
                    me.drawActionButton();
                }

            });
        },
        showReport: function (e) {
            var me = this;

            var chart = ENG.$(e.target).prop("title");
            me.hideAll();

            switch (chart) {
                case ENG.enum.chartType.table:
                    me.isShowTable = true;

                    me.initPageData();
                    me.calculateData();

                    //me.drawPageButton();
                    //me.drawChartIcon();
                    //me.drawActionButton();

                    me.$el.find(".eng-search").val(this.filterData.searchKey);
                    break;
                case ENG.enum.chartType.donut:
                    var pieChart = me.$el.find(".eng-pie-chart");
                    pieChart.show();
                    me.drawPieChart(pieChart);

                    ENG.$(window).bind('resize.donut_' + me.name, function (event) {
                        me.drawPieChart(pieChart)
                    });
                    break;
                case ENG.enum.chartType.vertical:
                    var verticalChart = me.$el.find(".eng-vertical-bar");
                    verticalChart.show();
                    me.drawVerticalChart(verticalChart);

                    ENG.$(window).bind('resize.vertical_' + me.name, function (event) {
                        me.drawVerticalChart(verticalChart)
                    });
                    break;
                case ENG.enum.chartType.map:
                    var mapChart = me.$el.find(".eng-map-chart");
                    mapChart.show();
                    me.drawMapChart(mapChart);

                    ENG.$(window).bind('resize.map_' + me.name, function (event) {
                        me.drawMapChart(mapChart)
                    });
                    break;
                default:
            }
        },
        drawPieChart: function (pieChart) {
            if (pieChart.css("display") === "none") {
                return;
            }

            if (this.data[this.name].data.length > 0) {

                var pieData = [];
                ENG.$.each(this.data[this.name].data, function (index, dataObject) {
                    pieData.push({ label: dataObject.key, data: dataObject.value });
                });

                ENG.$.plot(pieChart, pieData, {
                    series: {
                        pie: {
                            show: true,
                            innerRadius: 0.5,
                            label: {
                                show: true,
                                formatter: function (label, point) {
                                    return (point.percent.toFixed(2) + '%');
                                },
                                threshold: 0.015,
                            }
                        }
                    },
                    legend: {
                        show: true
                    }
                });
            }
        },
        drawVerticalChart: function (verticalChart) {
            if (verticalChart.css("display") === "none") {
                return;
            }

            if (this.data[this.name].data.length > 0) {

                var verticalChartData = [];
                ENG.$.each(this.data[this.name].data, function (index, dataObject) {
                    verticalChartData.push([dataObject.key, dataObject.value]);
                });

                ENG.$.plot(verticalChart, [verticalChartData], {
                    series: {
                        bars: {
                            show: true,
                            barWidth: 0.6,
                            align: "center"
                        }
                    },
                    xaxis: {
                        mode: "categories",
                        tickLength: 0
                    }
                });
            }
        },

        //drawMapChart
        drawMapChart: function (mapChart) {
            var me = this;

            if (me.data[me.name].areas === undefined || me.data[me.name].areas === null) {

                me.data[me.name].areas = [];

                _.each(me.data[me.name].data, function (obj) {
                    me.data[me.name].areas[obj.key] = {
                        value: obj.value,
                        tooltip: {
                            content: "<span style=\"font-weight:bold;\">" + getCountryName(obj.key) + " :</span> " + obj.value
                        },
                        attrs: {
                            fill: "#1993e4",
                        }
                    };
                });
            }

            mapChart.mapael({
                map: {
                    name: "world_countries",
                },
                areas: me.data[me.name].areas
            });


        },

        drawChartIcon: function () {
            var me = this;
            if (me.opts.isShowChartIcon && me.opts.chartType.length > 0) {

                var chooseArea = this.$el.find(".eng-choose-chart");
                _.each(me.opts.chartType, function (type) {
                    switch (type) {
                        case ENG.enum.chartType.table:
                            chooseArea.append("<i title='" + type + "' class='eng-chooseReport fa fa-table'></i>");
                            break;
                        case ENG.enum.chartType.donut:
                            chooseArea.append("<i title='" + type + "' class='eng-chooseReport fa fa-pie-chart'></i>");
                            break;
                        case ENG.enum.chartType.vertical:
                            chooseArea.append("<i title='" + type + "' class='eng-chooseReport fa fa-bar-chart'></i>");
                            break;
                        case ENG.enum.chartType.map:
                            chooseArea.append("<i title='" + type + "' class='eng-chooseReport fa fa-map-marker'></i>");
                            break;
                            //case ENG.enum.chartType.interactive:
                            //    chooseArea.append("<i title='" + type + "' class='eng-chooseReport fa fa-line-chart'></i>");
                            //    break;
                            //case ENG.enum.chartType.quickStats:
                            //    chooseArea.append("<i title='" + type + "' class='eng-chooseReport fa fa-area-chart'></i>");
                            //    break;
                        default:
                    }
                });
            }
        },
        hideAll: function () {
            this.$el.find(".eng-pie-chart").hide();
            this.$el.find(".eng-vertical-bar").hide();
            this.$el.find(".eng-interactive-chart").hide();
            this.$el.find(".eng-map-chart").hide();
            this.$el.find(".eng-quick-stats").hide();
            this.$el.find(".eng-table-chart-area").hide();
            this.isShowTable = false;
        },

        //drawInteractiveChart
        drawInteractiveChart: function (interactiveChart) {
            interactiveChart.html("");
            var me = this;
            if (me.data[me.name] === null || me.data[me.name] === undefined || me.opts.searchObject.isNew) {
                var startDate = me.opts.searchObject.startDate;
                var endDate = me.opts.searchObject.endDate;

                ENG.loadPageViewByDateJson(startDate, endDate, function (pageViewsData) {
                    var ticks = [];
                    ticks = me.calculateDate(pageViewsData);

                    var data = [{
                        data: pageViewsData,
                        label: "Page Views",
                        color: "#348fe2"
                    }];

                    me.data[me.name] = {
                        data: data,
                        ticks: ticks
                    };

                    me.setInteraciveChartData(interactiveChart);

                });
            } else {
                me.setInteraciveChartData(interactiveChart);
            }
        },
        setInteraciveChartData: function (interactiveChart) {
            var me = this;
            var options = {
                canvas: true,
                xaxes: [{
                    mode: "time",
                    timeformat: "%m/%y"
                }],
                xaxis: {
                    ticks: me.data[me.name].ticks
                },
                series: {
                    lines: {
                        show: true
                    },
                    points: {
                        show: true
                    }
                },
                grid: {
                    hoverable: true,
                    clickable: true
                },
            };

            ENG.$.plot(interactiveChart, me.data[me.name].data, options);

            ENG.$("<div id='eng_tooltip'></div>").css({
                position: "fixed",
                display: "none",
                border: "1px solid #fdd",
                "background-color": "black",
                color: "white",
                padding: "10px",
                "border-radius": "5px",
                "font-size": "12px",
                opacity: 0.70,
                "z-index": 10000
            }).appendTo(interactiveChart);

            interactiveChart.bind("plothover", function (event, pos, item) {

                if (item) {
                    ENG.$("#eng_tooltip").html("Page Views " + item.datapoint[1])
                    .css({ top: item.pageY - 50, left: item.pageX - 45 })
                    .fadeIn(200);
                } else {
                    ENG.$("#eng_tooltip").hide();
                }
            });
        },
        calculateDate: function (pageViewsData) {

            var me = this;

            var ticks = [];
            var count = pageViewsData.length;
            var step = Math.ceil(pageViewsData.length / 10);

            if (pageViewsData.length > 0) {

                ticks.push([ENG.getDateValue(pageViewsData[0][0]), ENG.getDateString(ENG.getDateValue(pageViewsData[0][0]))]);
                for (var i = 1; i < count - 1; i++) {
                    if (i % step == 0) {
                        ticks.push([ENG.getDateValue(pageViewsData[i][0]), ENG.getDateString(ENG.getDateValue(pageViewsData[i][0]))]);
                    } else {
                        ticks.push([ENG.getDateValue(pageViewsData[i][0]), ""]);
                    }
                }
                ticks.push([ENG.getDateValue(pageViewsData[count - 1][0]), ENG.getDateString(ENG.getDateValue(pageViewsData[count - 1][0]))]);
            }

            return ticks;
        },

        //drawAnalyticDetail
        drawAnalyticDetail: function () {
            var me = this;

            if (me.data[me.name] == undefined || me.data[me.name] == null) {
                me.data[me.name] = {
                    today: 0,
                    yesterday: 0,
                    month: 0,
                    all: 0,
                    userOnline: 0,
                    mostVisited: 0
                };

                var currentDate = new Date();
                //today
                var todayStartDate = currentDate;
                var todayEndDate = new Date(currentDate.getFullYear(), currentDate.getMonth(), currentDate.getDate() + 1);

                ENG.loadPageViewByDateJson(todayStartDate, todayEndDate, function (data) {
                    me.data[me.name].today = 0;
                    _.each(data, function (arr) {
                        me.data[me.name].today += arr[1];
                    });

                    me.$el.find("#eng-pageview-today").text(me.data[me.name].today);
                });

                //yesterday
                var yesterdayStartDate = new Date(currentDate.getFullYear(), currentDate.getMonth(), currentDate.getDate() - 1);
                var yesterdayEndDate = currentDate;

                ENG.loadPageViewByDateJson(yesterdayStartDate, yesterdayEndDate, function (data) {
                    me.data[me.name].yesterday = 0;
                    _.each(data, function (arr) {
                        me.data[me.name].yesterday += arr[1];
                    });

                    me.$el.find("#eng-pageview-yesterday").text(me.data[me.name].yesterday);

                });

                //last month
                var lastMonthStartDate = new Date(currentDate.getFullYear(), currentDate.getMonth() - 1, 1);
                var lastMonthEndDate = new Date(currentDate.getFullYear(), currentDate.getMonth(), 1);

                ENG.loadPageViewByDateJson(lastMonthStartDate, lastMonthEndDate, function (data) {
                    me.data[me.name].month = 0;
                    _.each(data, function (arr) {
                        me.data[me.name].month += arr[1];
                    });

                    me.$el.find("#eng-pageview-last-month").text(me.data[me.name].month);
                });

                //all time
                var allTimeStartDate = new Date(2015, 0, 1);
                var allTimeEndDate = todayEndDate;

                ENG.loadPageViewByDateJson(allTimeStartDate, allTimeEndDate, function (data) {
                    me.data[me.name].all = 0;
                    _.each(data, function (arr) {
                        me.data[me.name].all += arr[1];
                    });

                    me.$el.find("#eng-pageview-all-time").text(me.data[me.name].all);
                });

                //user online
                me.getUserOnline(function (userOnline) {
                    me.data[me.name].userOnline = userOnline;
                    me.$el.find("#eng-user-online").text(me.data[me.name].userOnline);
                });

                //most visited
                me.getMostVisited(function (mostVisited) {
                    me.data[me.name].mostVisited = mostVisited;
                    me.$el.find("#eng-most-visited").text(me.data[me.name].mostVisited);
                });

            } else {
                me.$el.find("#eng-pageview-today").text(me.data[me.name].today);
                me.$el.find("#eng-pageview-yesterday").text(me.data[me.name].yesterday);
                me.$el.find("#eng-pageview-last-month").text(me.data[me.name].month);
                me.$el.find("#eng-pageview-all-time").text(me.data[me.name].all);
                me.$el.find("#eng-user-online").text(me.data[me.name].userOnline);
                me.$el.find("#eng-most-visited").text(me.data[me.name].mostVisited);
            }
        },
        getUserOnline: function (callBack) {
            var url = _.template("<%=domain%>/umbraco/api/EngQuery/GetUsersOnline?clientId=<%=cliectId%>");

            var apiUrl = url({
                domain: ENG.ApiDomain,
                cliectId: ENG.cid,
            });

            Xdr.ajax({
                url: apiUrl
            },
            function (success, response) {
                var usersOnline = 0;
                if (success && success.success) {
                    if (ENG.Utils.checkObjExist(success.data, 'data.grouped.IPAddress_s.groups'))
                        usersOnline = success.data.data.grouped.IPAddress_s.groups.length;
                    else usersOnline = 0;

                }

                callBack(usersOnline);
            }, this);
        },
        getMostVisited: function (callBack) {
            var url = _.template("<%=domain%>/umbraco/api/EngQuery/GetMostView?clientId=<%=cliectId%>");

            var apiUrl = url({
                domain: ENG.ApiDomain,
                cliectId: ENG.cid,
            });

            Xdr.ajax({
                url: apiUrl
            },
            function (success, response) {
                var mostVisited = 0;
                if (success && success.success) {

                    if (ENG.Utils.checkObjExist(success.data, 'data')) {
                        mostVisited = success.data.data;
                    }

                    var splitStr = mostVisited.split("Z: ");
                    mostVisited = splitStr[1];
                }
                callBack(mostVisited);
            }, this);

        },

        //header button
        drawActionButton: function () {
            var me = this;
            if (me.opts.buttonType.length > 0) {

                var buttonArea = this.$el.find(".eng-panel-heading-btn");
                _.each(me.opts.buttonType, function (type) {
                    switch (type) {
                        case ENG.enum.buttonType.collapse:
                            buttonArea.append("<a href='javascript:;' class='eng-btn btn-xs eng-btn-icon eng-btn-circle eng-btn-warning' data-click='" + type + "'><i class='fa fa-minus'></i></a>");
                            break;
                        case ENG.enum.buttonType.close:
                            buttonArea.append("<a href='javascript:;' class='eng-btn btn-xs eng-btn-icon eng-btn-circle eng-btn-danger' data-click='" + type + "'><i class='fa fa-times'></i></a>");
                            break;
                        case ENG.enum.buttonType.add:
                            buttonArea.append("<a href='javascript:;' class='eng-btn btn-xs eng-btn-icon eng-btn-circle eng-btn-default' data-click='" + type + "'><i class='fa fa-plus'></i></a>");
                            break;
                        default:
                    }
                });
            }
        },
        headerBtnClick: function (e) {
            var type = "";
            var me = this;
            if (ENG.$(e.target).parents(".eng-btn").length === 0) {
                type = ENG.$(e.target).data("click");
            } else {
                type = ENG.$(e.target).parents(".eng-btn").data("click");
            }
            switch (type) {
                case ENG.enum.buttonType.collapse:
                    ENG.$(e.target).parents(".eng-tableTitle-bar").siblings(".eng-panel-body").toggle();

                    if (ENG.$(e.target).parents(".eng-dashboard-content").length) {
                        //collapse in dashboard -> save data
                        ENG.loadDashboard(function (data) {
                            var reportName = me.$el.parent()[0].id.replace("eng-report-", "").split("-").join(" ");
                            _.each(data.columns, function (column) {
                                _.each(column.reports, function (report) {
                                    if (report.name === reportName) {
                                        report.collapse = !report.collapse;
                                    }
                                });
                            });

                            ENG.saveDashboard(data);
                        });
                    }

                    break;
                case ENG.enum.buttonType.close:
                    var reportName = me.$el.parent()[0].id.replace("eng-report-", "").split("-").join(" ");
                    me.opts.parentView.trigger("removeTable", reportName);

                    ENG.loadDashboard(function (data) {
                        _.each(data.columns, function (column) {
                            for (var i = 0; i < column.reports.length; i++) {
                                if (column.reports[i].name === reportName) {
                                    column.reports.splice(i, 1);
                                }
                            }
                        });

                        ENG.saveDashboard(data);
                    });

                    ENG.$(e.target).parents(".eng-panel-table").remove();
                    break;
                default:
            }
        },

        //paging, sort, search
        loadData: function (handleData) {
            var me = this;
            if (me.data[me.name] === null || me.data[me.name] === undefined || me.opts.searchObject.isNew) {

                var url = _.template("<%=domain%><%=baseApiUrl%>?clientId=<%=cliectId%>&startDate=<%=startDate%>&endDate=<%=endDate%>");

                var apiUrl = url({
                    domain: ENG.ApiDomain,
                    cliectId: ENG.cid,
                    startDate: ENG.getDateStringYYYYMMDD(me.opts.searchObject.startDate),
                    endDate: ENG.getDateStringYYYYMMDD(me.opts.searchObject.endDate),
                    baseApiUrl: me.baseApiUrl
                });

                ENG.loadPageViewJson(apiUrl, me.columnHeader, function (data) {
                    if (data != null) {
                        data = me.rateData(data);
                        me.data[me.name] = {
                            data: me.rateData(data)
                        };

                        switch (me.name) {
                            case ENG.enum.reportType.country:
                                me.handleCountryData(data);
                                break;
                            case ENG.enum.reportType.continent:
                                me.handleContinentData(data);
                                break;
                            case ENG.enum.reportType.deviceBrand:
                                me.handleDeviceBrandData(data);
                                break;
                            default:
                        }
                    } else {
                        me.data[me.name] = {
                            data: null,
                        }
                    }
                    handleData();
                });
            } else {
                handleData();
            }
        },
        initPageData: function () {
            this.filterData = {
                count: 0,
                pageCount: 0,
                pageSize: 10,
                currentPage: 1,
                searchKey: "",
                sortColumn: "",
                order: null,
                data: null
            };

            this.filterData.data = this.data[this.name].data;
        },
        calculateData: function () {
            var me = this;

            if (this.filterData.searchKey === "") {
                //default data with order
                this.reloadFilterData();
            } else {
                //get data with seach key
                this.filterData.data = _.filter(me.data[me.name].data, function (item) {
                    return (item.key.indexOf(me.filterData.searchKey) > -1) || (item.value.toString().indexOf(me.filterData.searchKey) > -1)
                });
            }
            if (this.filterData.sortColumn != "") {
                if (this.filterData.sortColumn.toLocaleLowerCase().trim() === this.columnHeader[0].name.toLocaleLowerCase()) {
                    this.filterData.data = _.sortBy(this.filterData.data, "key");
                } else {
                    this.filterData.data = _.sortBy(this.filterData.data, "value");
                }

                if (this.filterData.order) {
                    this.filterData.data.reverse();
                }
            }

            if (this.filterData.data != null) {
                this.filterData.count = this.filterData.data.length;

                this.filterData.pageCount = Math.ceil(this.filterData.count / this.filterData.pageSize);

                this.displayData = _.chain(this.filterData.data)
                                .rest(this.filterData.pageSize * (this.filterData.currentPage - 1))
                                .first(this.filterData.pageSize)
                                .value();

                if (this.displayData.length === 0) {
                    this.reloadFilterData();
                }
            }

            this.reloadTable();

            this.drawPageButton();
            this.$el.find(".eng-search").val(me.filterData.searchKey);
            this.drawChartIcon();
            this.drawActionButton();
        },
        drawPageButton: function () {
            for (var i = 1; i <= this.filterData.pageCount; i++) {
                if (i === this.filterData.currentPage) {
                    this.$el.find(".eng-table-page-number").append("<a class='eng-page active'>" + i + "</a>")
                } else {
                    this.$el.find(".eng-table-page-number").append("<a class='eng-page'>" + i + "</a>");
                }
            }
        },
        pageChange: function (e) {
            switch (ENG.$(e.target).attr("class")) {
                case "eng-page eng-first":
                    this.filterData.currentPage = 1;
                    break;
                case "eng-page eng-back":
                    if (this.filterData.currentPage != 1) {
                        this.filterData.currentPage--;
                    }
                    break;
                case "eng-page eng-next":
                    if (this.filterData.currentPage != this.filterData.pageCount) {
                        this.filterData.currentPage++;
                    }
                    break;
                case "eng-page eng-last":
                    this.filterData.currentPage = this.filterData.pageCount;
                    break;
                default:
                    this.filterData.currentPage = ENG.$(e.target).text();
            }

            //this.filterData.currentPage = this.filterData.pageCount;
            this.calculateData();
        },
        search: function (e) {
            this.filterData.currentPage = 1;
            this.filterData.searchKey = ENG.$(e.target).val();
            this.calculateData();
            this.$el.find(".eng-search").focus();
        },
        sort: function (e) {
            this.filterData.currentPage = 1;
            this.filterData.sortColumn = ENG.$(e.target).text();
            if (this.filterData.order === null) {
                this.filterData.order = true;
            }

            this.filterData.order = !this.filterData.order;
            this.calculateData();
        },
        reloadFilterData: function () {
            var me = this;
            var order = this.filterData.order,
                searchKey = this.filterData.searchKey,
                sortColumn = this.filterData.sortColumn,
                currentPage = this.filterData.currentPage;

            this.filterData = {
                count: 0,
                pageCount: 0,
                pageSize: 10,
                currentPage: currentPage,
                searchKey: searchKey,
                sortColumn: sortColumn,
                order: order,
                data: me.data[me.name].data
            };
        },
        initStoreObject: function () {
            if (this.opts.searchObject === null || this.opts.searchObject === undefined) {
                this.opts.searchObject = {
                    data: null,
                    startDate: null,
                    endDate: null,
                    isNew: true,
                    selectedValue: 7,
                    selectedText: "Last 7 days",
                };
            }
        },
        initDateValue: function () {
            if (this.opts.searchObject.startDate === null || this.opts.searchObject.endDate) {
                var initDate = 7;

                //init date value to load
                var defaultEndDate = new Date();
                var defaultStartDate = new Date(defaultEndDate.getFullYear(), defaultEndDate.getMonth(), defaultEndDate.getDate() - initDate);

                this.opts.searchObject.startDate = defaultStartDate;
                this.opts.searchObject.endDate = new Date(defaultEndDate.getFullYear(), defaultEndDate.getMonth(), defaultEndDate.getDate() + 1);

            }
        },
        rateData: function (data) {
            //var low = ENG.trackingSetting.PageViewsRankingLow;
            //var med = ENG.trackingSetting.PageViewsRankingMedium;

            //_.each(data, function (obj) {
            //    if (obj.value < low) {
            //        obj.rate = "Low";
            //    } else if (obj.value > low && obj.value < med) {
            //        obj.rate = "Medium";
            //    } else {
            //        obj.rate = "High";
            //    }
            //});
            return data;
        },

        //handle some special table data
        handleCountryData: function (data) {
            _.each(data, function (obj) {
                obj.key = getCountryName(obj.key);
            });
            this.data[this.name].data = data;
        },

        handleContinentData: function (data) {
            var me = this;
            me.data[me.name].data = [];

            _.each(data, function (obj) {
                var continent = getContinent(obj.key);
                if (!me.checkExistedContinent(continent, obj.value)) {
                    me.data[me.name].data.push({
                        keyTitle: "Continent",
                        key: continent,
                        valueTitle: "PageView",
                        value: obj.value
                    });
                }
            });
        },
        checkExistedContinent: function (continentName, value) {
            var isExisted = false;
            _.each(this.data[this.name].data, function (obj) {
                if (obj.key === continentName) {
                    obj.value += value;
                    isExisted = true;
                }
            });
            return isExisted;
        },

        handleDeviceBrandData: function (data) {
            var me = this;
            me.data[me.name].data = [];

            _.each(data, function (obj) {
                var brand = "";
                var arr = obj.key.split('-');
                if (arr.length > 1) {
                    brand = arr[0].trim();
                } else {
                    brand = "Unknown";
                }

                if (!me.checkExistedBrand(brand, obj.value)) {
                    me.data[me.name].data.push({
                        keyTitle: "Brand",
                        key: brand,
                        valueTitle: "PageView",
                        value: obj.value
                    });
                }
            });
        },
        checkExistedBrand: function (brandName, value) {
            var isExisted = false;
            _.each(this.data[this.name].data, function (obj) {
                if (obj.key === brandName) {
                    obj.value += value;
                    isExisted = true;
                }
            });
            return isExisted;
        },

        //load html
        reloadTable: function () {
            this.$el.html("");
            var compile = _.template(this.template());
            var html = compile({ isShowTable: this.isShowTable, title: this.title, columnHeader: this.columnHeader, data: this.displayData, filterData: this.filterData, datepickerMonth: this.datepickerMonth });
            this.$el.append(html);
        },

        //flow : 
        //Page Load//////////////////////////////////////////////
        //initStoreObject -> init object to store datepicker data

        //initDateValue -> init date value if not existed, default: last 7 days
        //loadData -> load data to table base on StoreObject 
        //initDropDownListMonth -> init month value for drop down list datepicker

        //initPageData -> init page value in filterData object
        //calculateData -> caculate paging, search, sort base on filterData object 
        //reloadTable   -> render html 
        //              -> call drawPageButton
        //              -> fill search key, selected Date base on filterData object
        //              -> fill add hightlight to dropdownrow, text to dropdown button base on StoreObject object

        //Date change//////////////////////////////////////////////
        //itemDropDownClick -> get value from dropdownlist
        //selectDate -> check value then handle data
        //initPageData
        //calculateData

    });
});