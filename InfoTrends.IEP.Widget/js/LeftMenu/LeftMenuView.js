define([
    "text!js/LeftMenu/LeftMenu.html",
    'js/Container',
    'js/Widget/WidgetView',
    'js/Widget/WidgetModel',
    'js/AnalyticItem/AnalyticItemView',
    'js/AnalyticItem/AnalyticItemModel',
    'js/ExpandMenu/ExpandMenuView',
    'js/ExpandMenu/ExpandMenuModel',
    'js/WidgetContent/WidgetContentView',
    'js/WidgetContent/WidgetContentModel',
    'js/SearchContent/SearchWidgetView',
    'js/SubscriberWidget/SubscriberWidgetView',
    'js/ContactUs/ContactUsView',
    'js/ContentWidget/ContentWidgetView',
    'js/HiddenItem/HiddenItemView'

],

function (
    Template,
    Component,
    WidgetView,
    WidgetModel,
    AnalyticItem,
    AnalyticItemModel,
    ExpandMenu,
    ExpandMenuModel,
    WidgetContentView,
    WidgetContentModel,
    SearchWidgetView,
    SubscribeView,
    ContactUsView,
    ContentWidget, 
    HiddenItemView) {

    return Component.extend({

        events: {
            'click .eng-addWidget': 'addWidget',
            'click #rightPanel': 'showRightPanel',
            'click #engCollapse': 'collapseLeftMenu',

        },
        constructor: function (options) {

            var opts = ENG.$.extend(true, {
                position: {
                    left: "0",
                    right: "auto"
                },
                model: null
            }, options);

            Component.prototype.constructor.call(this, opts);
            this.$el.addClass('eng-leftPanel');
            this.$el.html('');
            var urlCss = ENG.DOMAIN + "/js/LeftMenu/LeftMenu.css";
            ENG.loadCss(urlCss);

        },
        render: function (model) {
            Component.prototype.render.call(this);

            var compile = _.template(this.template());

            var html = "";
            this.$el.html('');
            if (model != undefined) {
                html = compile({ title: model.get("title"), favicon: model.get("favicon") });
            } else {
                html = compile({ title: "Widgets" });
            }

            this.$el.append(html);

            //show hide add widget button
            var addWidgetButton = ENG.$(".eng-addWidget");
            if (model && model.get("title") && model.get("title") === "Widgets") {
                addWidgetButton.show();
            } else {
                addWidgetButton.hide();
            }
            this.changePosition();
            this.showPosition();

            return this;
        },
        //        initScroll: function () {
        //
        //            var windowHeight = ENG.$(window).height();
        //            var topHeaderHeight = ENG.leftMenu.$el.find(".eng-header").height();
        //            var toolbarHeight = ENG.toolbar.$el.height();
        //            var containerHeight = windowHeight - topHeaderHeight - toolbarHeight;
        //
        //            var container = this.$el.find(".eng-container");
        //            var numberItem = container.find(".eng-box-widget").length;
        //            container.slimscroll({
        //                height: containerHeight - ((numberItem+1)*5+10),
        //                alwaysVisible: true,
        //                allowPageScroll : true
        //            });
        //        },
        //        destroyScroll: function() {
        //            var container = this.$el.find(".eng-container");
        //            container.slimscroll({
        //                destroy: true
        //            });
        //            var $elem = ENG.$('.scrollable'),
        //                events = ENG.$._data($elem[0], "events");
        //
        //            if (events) {
        //                ENG.$._removeData($elem[0], "events");
        //            }
        //        },
        template: function () {
            return Template;
        },
        changePosition: function () {
            var me = this;
            var leftButton = this.$el.find(".eng-arrow-leftPanel");
            var rightPanel = this.$el.find(".eng-arrow-rightPanel");
            this.showChangePositionArrow();
            leftButton.on("click", function () {
                var position = {
                    left: "0",
                    right: "auto"
                };
                ENG.isRightLeftPanel = false;
                ENG.rightPanel.showPosition(ENG.isRightLeftPanel);
                me.setPosition(position);
                me.showChangePositionArrow();

                me.calculateCollapseMenu(true);
            });
            rightPanel.on("click", function () {
                var position = {
                    left: "auto",
                    right: "0"
                };
                ENG.isRightLeftPanel = true;
                ENG.rightPanel.showPosition(ENG.isRightLeftPanel);
                me.setPosition(position);
                me.showChangePositionArrow();

                me.calculateCollapseMenu(true);
            });
        },
        loadData: function (collection) {
            var widgets = [];
            collection.forEach(function (item) {
                var w = new WidgetView({ model: item });
                widgets.push(w);
            }, this);
            this.add(widgets);
            if (ENG.hiddenWidget) {
                ENG.hiddenWidget.show();
                    } else {
                var hiddenItem = new HiddenItemView();
                ENG.$("body").append(hiddenItem.render().$el);
                ENG.hiddenWidget = hiddenItem;
                    }

        },
        leftPaneladdWidget: function (obj) {
            var widgetModel = new WidgetModel();
            var widget = new WidgetView();
            if (obj && obj.name)
                widgetModel.set({ name: obj.name });
            widget.model = widgetModel;
            this.add(widget);
            ENG.widgets.push(widgetModel);
        },
        loadDataAnalytic: function (collection) {
            var collection = [];
            var model = new AnalyticItemModel();
            var analytics = [];
            model.set("header", "Mouse Move Heatmap");
            model.set("content", "Mouse Move Heatmap");
            collection.push(model);

            var mouseClickModel = new AnalyticItemModel();
            mouseClickModel.set("header", "Mouse Click Heatmap");
            mouseClickModel.set("content", "Mouse Click Heatmap");
            collection.push(mouseClickModel);

            var mouseNetWorkAnalyticModel = new AnalyticItemModel();
            mouseNetWorkAnalyticModel.set("header", "Network Analytic");
            mouseNetWorkAnalyticModel.set("content", "Network Analytic");
            collection.push(mouseNetWorkAnalyticModel);

            collection.forEach(function (item) {
                var a = new AnalyticItem({ model: item });
                analytics.push(a);
            }, this);
            this.add(analytics);
        },
        loadExpandMenu: function (collection) {
            var collection = [];
            var settingMenuItems = [];

            //Settings
            var settingsMenu = new ExpandMenuModel();
            settingsMenu.set("title", "Settings");
            settingsMenu.set("favicon", "fa fa-cog ");

            var analytics = [];


            settingsMenu.set("childs", settingMenuItems);
            collection.push(settingsMenu);

            //Base Analytics
            var baseAnalyticMenu = new ExpandMenuModel();
            baseAnalyticMenu.set("title", "Base Analytics");
            baseAnalyticMenu.set("favicon", "fa fa-area-chart");

            var childs = [];
            var OverviewMenu = {
                name: "Overview",
                url: ENG.enum.subMenu.overView,
                favicon: "fa fa-area-chart"
            };
            //var AudienceMenu = {
            //    name: "Audience",
            //    url: ENG.enum.subMenu.overView,
            //    favicon: "fa fa-area-chart"
            //};
            var LogsMenu = {
                name: "Logs",
                url: ENG.enum.subMenu.log,
                favicon: "fa fa-file-o"
            };
            var LocationsMenu = {
                name: "Locations",
                url: ENG.enum.subMenu.locations,
                favicon: "fa fa-location-arrow"
            };
            var SettingsMenu = {
                name: "Settings",
                url: ENG.enum.subMenu.settings,
                favicon: "fa fa-cog"
            };
            var DeviceMenu = {
                name: "Devices",
                url: ENG.enum.subMenu.devices,
                favicon: "fa fa-laptop"
            };
            var MoveHeatMap = {
                name: "Mouse Move Heat Map",
                url: ENG.enum.subMenu.mouseMoveHM,
                favicon: "fa fa-arrows"
            };
            var ClickHeatMap = {
                name: "Mouse Click Heat Map",
                url: ENG.enum.subMenu.mouseClickHM,
                favicon: "fa fa-hand-o-up"
            };

            childs.push(OverviewMenu);
            //childs.push(AudienceMenu);
            childs.push(LogsMenu);
            childs.push(LocationsMenu);
            childs.push(SettingsMenu);
            childs.push(DeviceMenu);
            childs.push(MoveHeatMap);
            childs.push(ClickHeatMap);

            baseAnalyticMenu.set("childs", childs);
            collection.push(baseAnalyticMenu);

            //Qualitative Analysis
            var qualitativeAnalysisMenu = new ExpandMenuModel();
            qualitativeAnalysisMenu.set("title", "Qualitative Analysis");
            qualitativeAnalysisMenu.set("favicon", "fa fa-bars ");

            var analytics = [];


            qualitativeAnalysisMenu.set("childs", settingMenuItems);
            collection.push(qualitativeAnalysisMenu);

            collection.forEach(function (item) {
                var a = new ExpandMenu({ model: item });
                analytics.push(a);
            }, this);

            this.add(analytics);
        },


        loadSettingMenu: function (collection) {
            var collection = [];
            var childsUserMenu = [];
            var analytics = [];

            var networkMenu = new ExpandMenuModel();
            var analytics = [];
            networkMenu.set("title", "Account Settings");
            networkMenu.set("favicon", "fa fa-area-chart");

            var childs = [];
            var trackingSetting = {
                name: "Tracking Settings",
                url: ENG.enum.subMenu.trackSetting,
                favicon: "fa fa-cog"
            };

            var systemSetting = {
                name: "System Settings",
                url: ENG.enum.subMenu.systemSetting,
                favicon: "fa fa-cogs"
            };


            childs.push(trackingSetting);
            childs.push(systemSetting);
            networkMenu.set("childs", childs);
            collection.push(networkMenu);
            collection.forEach(function (item) {
                var a = new ExpandMenu({ model: item });
                analytics.push(a);
            }, this);

            this.add(analytics);
        },

        loadUserActMenu: function (collection) {
            //            var childs = [];
            //            var model = new ExpandMenuModel();
            //            var analytics = [];
            //            model.set("title", "Users activities");
            //            model.set("favicon", "fa fa-plus");
            //            
            //            var MoveHeatMap = {
            //                name: "Mouse Move Heat Map",
            //                url: ENG.enum.subMenu.mouseMoveHM,
            //                favicon: "fa fa-plus"
            //            };
            //            childs.push(MoveHeatMap);
            //            var ClickHeatMap = {
            //                name: "Mouse Click Heat Map",
            //                url: ENG.enum.subMenu.mouseClickHM,
            //                favicon: "fa fa-plus"
            //            };
            //            
            //            childs.push(ClickHeatMap);
            //            collection.forEach(function (item) {
            //                var a = new ExpandMenu({ model: item });
            //                analytics.push(a);
            //            }, this);
            //            this.add(analytics);
        },
        getAddressElement: function (address, ele) {
            var parentNode = ele.parent();
            var tagName;
            var index;
            var seperate;
            if (address) {
                seperate = "-";
            }
            if (parentNode) {
                index = parentNode.children().index(ele);
                tagName = parentNode.prop("tagName");
            }
            var complie = _.template("<%= seperate%><%= tagName%>.<%= index%>");
            address = address + complie({
                tagName: tagName,
                index: index,
                seperate: seperate
            });
            if (tagName.toLowerCase() == "body") {
                return address;
            } else {
                return this.getAddressElement(address, parentNode);
            }
        },
        showChangePositionArrow: function () {
            var leftButton = this.$el.find(".eng-arrow-leftPanel");
            var rightButton = this.$el.find(".eng-arrow-rightPanel");
            if (ENG.isRightLeftPanel) {
                leftButton.show();
                rightButton.hide();

                ENG.$("#engCollapse i").removeClass("fa-angle-double-left");
                ENG.$("#engCollapse i").addClass("fa-angle-double-right");


            } else {
                leftButton.hide();
                rightButton.show();

                ENG.$("#engCollapse i").removeClass("fa-angle-double-right");
                ENG.$("#engCollapse i").addClass("fa-angle-double-left");
            }
        },

        collapseLeftMenu: function (isChangePosition) {
            this.calculateCollapseMenu(false);
        },
        calculateCollapseMenu: function (isChangePosition) {
            if (ENG.isRightLeftPanel) {
                this.$el.find(".eng-collapse-btn").css("float", "left");
            } else {
                this.$el.find(".eng-collapse-btn").css("float", "right");
            }
            this.$el.find(".eng-collapse-btn").find("i").removeClass();
            if (ENG.leftMenu.$el.width() === 65) {
                if (isChangePosition) {
                    if (ENG.isRightLeftPanel) {
                        this.$el.find(".eng-collapse-btn").find("i").addClass("fa fa-angle-double-left");
                    } else {
                        this.$el.find(".eng-collapse-btn").find("i").addClass("fa fa-angle-double-right");
                    }
                } else {
                    if (ENG.isRightLeftPanel) {
                        this.$el.find(".eng-collapse-btn").find("i").addClass("fa fa-angle-double-right");
            } else {
                        this.$el.find(".eng-collapse-btn").find("i").addClass("fa fa-angle-double-left");
                    }
                ENG.leftMenu.$el.css("width", "25%");
                ENG.leftMenu.$el.find(".eng-sub-menu-item-text").show();
                }
            } else {
                if (isChangePosition) {
                    if (ENG.isRightLeftPanel) {
                        this.$el.find(".eng-collapse-btn").find("i").addClass("fa fa-angle-double-right");
                    } else {
                        this.$el.find(".eng-collapse-btn").find("i").addClass("fa fa-angle-double-left");
                    }
                } else {
                    if (ENG.isRightLeftPanel) {
                        this.$el.find(".eng-collapse-btn").find("i").addClass("fa fa-angle-double-left");
                    } else {
                        this.$el.find(".eng-collapse-btn").find("i").addClass("fa fa-angle-double-right");
                    }
                    ENG.leftMenu.$el.css("width", "65px");
                    ENG.leftMenu.$el.find(".eng-sub-menu-item-text").hide();
                }
            }
        },

        showPosition: function (isRight) {
            var bottom = ENG.$(".eng-toolbar").height();
            if (isRight) {
                var position = {
                    left: "auto",
                    right: "0",
                    top: "0",
                    bottom: bottom
                };
                this.setPosition(position);
            } else {
                var position = {
                    left: "0",
                    right: "auto",
                    top: "0",
                    bottom: bottom
                };
                this.setPosition(position);
            }
        },
        loadMenuStyle: function () {
            var height = ENG.leftMenu.$el.height() - (ENG.leftMenu.$el.find(".eng-header").height() + 30);
            ENG.leftMenu.$el.find(".eng-menu").height(height);
        },
        addColapseButton: function () {
            var html = "<a href='#' class='eng-collapse-btn' id='engCollapse'>";
            html += "<i class='eng fa fa-angle-double-left'></i>";
            html += "</a>";

            this.$el.find(".eng-menu").append(html);
        },
        setColapseButton: function () {
            var width = 0;
            if (window.scrollbars.visible) {
                $('body').css('overflow', 'hidden');
                width = ENG.$(window).width();
                $('body').css('overflow', 'auto');
            }
            if (width > 768) {
                this.$el.find(".eng-collapse-btn").show();
            } else {
                this.$el.find(".eng-collapse-btn").hide();
            }
        },
        initDragAndDrop: function() {
            var me = this;

            ENG.$("body").droppable({
                drop: function (event, ui) {
                    var address = "";
                    var currentItem = ENG.$(ENG.currentItem);
                    //Check drag drop in tool, not in the content of client page.
                    if (currentItem.parents(".eng-component").length > 0 || ENG.$(ENG.currentItem).prop("tagName") === "HTML") {
                        return;
                    }

                    //Find div clothes to insert widget
                    var divParent;
                    if (currentItem.is("div")) {
                        divParent = currentItem;
                    } else {
                        divParent = ENG.$(ENG.currentItem).parent().closest('div');
                    }

                    /*
                            If ENG.typeWidget === WIDGET
                            Widget in leftMenu
                            IF ENG.isDragReturn === WIDGETCONTENT
                                Widget content in client page,
                                can drag and drop to change position
                    */
                    if (ENG.typeWidget === ENG.enum.dragDropWidget.WIDGET) {

                        var widgetName = ENG.dragInClient.model.get("WidgetTypeName");
                        var widgetContent = new WidgetContentView();
                        widgetContent.opts.model = ENG.dragInClient.model;
                        switch (widgetName) {
                            case ENG.enum.typeWidget.CONTENT:
                                Xdr.ajax({
                                    url: ENG.ApiDomain + '/umbraco/api/WidgetContent/GetAllContent'
                                }, function (response) {
                                    if (response.success) {
                                        var contentItem = new ContentWidget({
                                            data: response.data
                                        });

                                        widgetContent.opts.itemChilds = contentItem;
                                        widgetContent.opts.model = ENG.dragInClient.model;
                                        widgetContent.opts.model.set("Data", response.data);
                                        divParent.append(widgetContent.render().$el);

                                        var itemHasInserted = divParent.find(".eng-widget-content");
                                        var position = me.getAddressElement(address, itemHasInserted);
                                        widgetContent.opts.position = position;


                                    }
                                });

                                break;
                            case ENG.enum.typeWidget.SEARCH:
                                var searchWidget = new SearchWidgetView();

                                widgetContent.opts.itemChilds = searchWidget;
                                widgetContent.opts.model = ENG.dragInClient.model;

                                divParent.append(widgetContent.render().$el);
                                break;
                            case ENG.enum.typeWidget.SUBSCRIBE:
                                var subscribeView = new SubscribeView();
                                widgetContent.opts.itemChilds = subscribeView;
                                widgetContent.opts.nameWidget = widgetName;
                                divParent.append(widgetContent.render().$el);

                                var itemHasInserted = divParent.find(".eng-widget-content");
                                var position = me.getAddressElement(address, itemHasInserted);
                                widgetContent.opts.position = position;
                                break;
                            case ENG.enum.typeWidget.CONTACTUS:
                                var contactView = new ContactUsView();
                                widgetContent.opts.itemChilds = contactView;
                                widgetContent.opts.nameWidget = widgetName;
                                divParent.append(widgetContent.render().$el);

                                var itemHasInserted = divParent.find(".eng-widget-content");
                                var position = me.getAddressElement(address, itemHasInserted);
                                widgetContent.opts.position = position;
                                break;
                        }


                    } else if (ENG.typeWidget === ENG.enum.dragDropWidget.WIDGETCONTENT) {
                        var itemChilds = ENG.dragInClient.opts.itemChilds;
                        ENG.dragInClient.itemChilds = itemChilds;
                        divParent.append(ENG.dragInClient.render().$el);
                    }

                }
            });
        }
    });
});