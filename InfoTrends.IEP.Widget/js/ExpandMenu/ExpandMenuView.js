define([
    'text!js/ExpandMenu/ExpandMenu.html',
    'js/Container',
    'js/NetworkAnalytic/NetworkAnalyticView',
    'js/NetworkAnalytic/NetworkAnalyticModel',
    'js/Table/TableView',
    'js/VisitorSettings/VisitorSettingsView',
    'js/VisitorSettings/VisitorSettingsModel',
    'js/VisitorLocations/VisitorLocationsView',
    'js/VisitorLocations/VisitorLocationsModel',
    'js/VisitorLog/VisitorLogView',
    'js/VisitorLog/VisitorLogModel',
    'js/NetworkDevices/NetworkDeviceView',
    'js/Loading/LoadingView',
    'js/TrackingSettings/TrackingSettingsView',

], function (Template, Container, NetworkAnalyticView, NetworkAnalyticModel, Table, VisitorSettingsView, VisitorSettingsModel, VisitorLocationsView, VisitorLocationsModel, VisitorLogView, VisitorLogModel, NetworkDevice, LoadingView, TrackingSettings) {

    return Container.extend({
        events: {
            'click a.eng-headerExpand': 'expandMenu',
            'click .eng_subMenuItem': "subMenuClick"
        },
        constructor: function (options) {

            var opts = ENG.$.extend(true, {
            }, options);

            Container.prototype.constructor.call(this, opts);


        },
        render: function (model) {
            Container.prototype.render.call(this);
            var me = this;
            var urlCss = ENG.DOMAIN + "/js/ExpandMenu/ExpandMenu.css";
            ENG.loadCss(urlCss, function () {
                var compile = _.template(me.template());
                var html = compile({
                    title: model.get("title"),
                    favicon: model.get("favicon"),
                    childs: model.get("childs")
                });
                me.$el.append(html);
            });
            return this;
        },
        template: function () {
            return Template;
        },
        expandMenu: function (e) {
            var headerTag;
            var itemCurrent = ENG.$(e.target).parents(".eng-headerExpand");
            if (itemCurrent.length > 0) {
                headerTag = itemCurrent;
            } else {
                headerTag = ENG.$(e.target);
            }
            if (headerTag.hasClass("eng_ExpanseMenuItem_Active")) {
                headerTag.removeClass("eng_ExpanseMenuItem_Active");
                headerTag.next().slideUp();
            } else {
                headerTag.addClass("eng_ExpanseMenuItem_Active");
                headerTag.next().slideDown();
            }
        },
        subMenuClick: function (e) {
            //clear onresize event
            //window.onresize = null;
            ENG.$(window).unbind('resize');
            ENG.setRightMenuPosition();

            var itemSubMenu = ENG.$(e.target).parents(".eng_subMenuItem")
            var itemSubMenuParent = ENG.$(e.target).parents();
            itemSubMenuParent.find(".eng_subMenuItem").removeClass("eng-subItemActive");
            itemSubMenu.addClass("eng-subItemActive");

            var url = ENG.$(e.target).parents(".eng_subMenuItem").find("input").val()
            ENG.rightPanel.clear();



            if (url === ENG.enum.subMenu.mouseMoveHM) {
                this.showMouseHeatMap("move");
            }

            if (url === ENG.enum.subMenu.devices) {
                this.showDevices();
            }

            if (url === ENG.enum.subMenu.settings) {
                this.showSettings();
            }

            if (url === ENG.enum.subMenu.locations) {
                this.showLocation();
            }


            if (url === ENG.enum.subMenu.mouseClickHM) {
                this.showMouseHeatMap("click");
            }
            if (url === ENG.enum.subMenu.log) {
                this.showLogs();
            }
            if (url === ENG.enum.subMenu.overView) {

                //ENG.rightPanel.setVisibility(true);
                //var settingsView = new NetworkDevice();
                //ENG.rightPanel.add(settingsView);


                //show Network Analytic
                ENG.rightPanel.setVisibility(true);
                var networkAnalytic = new NetworkAnalyticView();
                var networkModel = new NetworkAnalyticModel();


                networkAnalytic.model = networkModel;
                ENG.rightPanel.$el.find(".eng-container").append(networkAnalytic.$el);
                networkAnalytic.render(networkAnalytic.model);
                //ENG.rightPanel.add(networkAnalytic);
            }

            if (url === ENG.enum.subMenu.trackSetting) {
                this.showtrackSetting();
            }

            if (url === ENG.enum.subMenu.systemSetting) {
            }
            if (url === ENG.enum.subMenu.territoryBuilder) {
                this.showTerritoryBuilder();
            }

            ENG.$(".eng-close").css("opacity", "1");
            ENG.rightPanel.setVisibility(true);

        },

        showtrackSetting: function () {
            ENG.rightPanel.setVisibility(true);
            var trackingSettingsView = new TrackingSettings();
            ENG.rightPanel.add(trackingSettingsView);
        },

        showTerritoryBuilder: function () {
            ENG.rightPanel.clear();
            ENG.rightPanel.setVisibility(true);

            var url = _.template("<%=domain%>/js/TerritoryBuilder/TerritoryBuilder.html");
            var apiUrl = url({
                domain: ENG.DOMAIN,
            });
            var frame = _.template("<iframe style='width:100%; height:100%' id='eng-territoryBuilder' src='<%=apiUrl%>'></iframe>");
            var src = frame({
                apiUrl: apiUrl,
            });
            ENG.rightPanel.$el.find(".eng-container").append(src)
        },


        showMouseHeatMap: function (action) {
            var me = this;
            var mouseAction;
            if (action == "move")
                mouseAction = "GetMouseTrack";
            else if (action == "click")
                mouseAction = "GetMouseClick";

            ENG.Utils.sampleMoveHeat.length = 0;

            var url = _.template("<%=domain%>/umbraco/api/EngQuery/<%=ctrAction%>?clientId=<%=cliectId%>&endX=<%=endX%>&endY=<%=endY%>&pageUrl=<%=pageUrl%>");
            var apiUrl = url({
                domain: ENG.ApiDomain,
                ctrAction: mouseAction,
                cliectId: ENG.cid,
                endX: screen.width,
                endY: document.body.scrollHeight,
                pageUrl: document.URL,
            });

            Xdr.ajax({
                url: apiUrl,
            },
            function (success, response) {
                var retData = null;
                if (success && success.success) {
                    me.drawMouseHeatMap(success, action);
                }

            }, this);



        },
        drawMouseHeatMap: function (mouseMoveData, action) {

            ENG.$(".eng-component").hide();

            var canvas = document.getElementById("eng-mouseHeatMap");
            if (ENG.Utils.checkObjExist(mouseMoveData, 'data.data.facet_counts.facet_fields.Position_s')) {



                var dataArr = mouseMoveData.data.data.facet_counts.facet_fields.Position_s;

                for (i = 0; i < dataArr.length; i++) {

                    if (_.isString(dataArr[i])) {

                        var xAndY = dataArr[i].split(",");
                        ENG.Utils.sampleMoveHeat.push([parseInt(xAndY[0]), parseInt(xAndY[1]), dataArr[i + 1]]);
                    }


                }
            }


            if (!canvas) {
                ENG.$("body").append('<canvas id="eng-mouseHeatMap"></canvas>');
                canvas = document.getElementById("eng-mouseHeatMap");
                canvas.width = screen.width;
                canvas.height = document.body.scrollHeight;


                window.requestAnimationFrame = window.requestAnimationFrame || window.mozRequestAnimationFrame ||
                                           window.webkitRequestAnimationFrame || window.msRequestAnimationFrame;

                var data = [];
                var heat = simpleheat('eng-mouseHeatMap').data(data).max(1000),
                    frame;

                ENG.heat = heat;
                ENG.frame = frame;

                ENG.heat._data = ENG.Utils.sampleMoveHeat;


                ENG.heat.radius(+40, +15);
                window.requestAnimationFrame(ENG.Utils.drawHeat);
                ENG.$(canvas).click(function (event) {
                    //ENG.$(this).hide();
                    //ENG.$(".eng-toolbar").show();
                    //ENG.$(".eng-leftPanel").show().find(".eng-component").show();
                    //ENG.$(".eng-add-feedback").show();
                    //ENG.$(".eng-list-feedback").show();
                });

            }

            else {
                ENG.$(canvas).show();
                ENG.heat._data = ENG.Utils.sampleMoveHeat;
                window.requestAnimationFrame(ENG.Utils.drawHeat);
            }

            ENG.Utils.toggleHeatMapControl(true);
        },
        showDevices: function () {
            ENG.rightPanel.setVisibility(true);
            var networkDevice = new NetworkDevice();

            ENG.rightPanel.$el.find(".eng-container").append(networkDevice.$el);
            networkDevice.render();

        },
        showSettings: function () {
            ENG.rightPanel.clear();
            ENG.rightPanel.setVisibility(true);
            var settingsView = new VisitorSettingsView();
            var settingsModel = new VisitorSettingsModel();

            ENG.rightPanel.$el.find(".eng-container").append(settingsView.$el);
            settingsView.render(settingsView.model);

        },
        showLocation: function () {

            ENG.rightPanel.clear();
            ENG.rightPanel.setVisibility(true);

            //var loading = new LoadingView();
            //ENG.rightPanel.add(loading);

            var locationsView = new VisitorLocationsView();
            var locationsModel = new VisitorLocationsModel();

            locationsView.model = locationsModel;

            ENG.rightPanel.$el.find(".eng-container").append(locationsView.$el);
            locationsView.render(locationsView.model);

        },
        showLogs: function () {
            ENG.rightPanel.clear();
            ENG.rightPanel.setVisibility(true);
            var logView = new VisitorLogView();
            var logModel = new VisitorLogModel();

            logView.model = logModel;
            ENG.rightPanel.add(logView);
        },
    });
});

