require([
    'js/Widget/WidgetCollection',
    'js/SignInButton/SignInButtonView',
    'js/Toolbar/ToolbarView',
    'js/LeftMenu/LeftMenuView',
    'js/Widget/WidgetView',
    'js/Widget/WidgetModel',
    'js/Feedback/FeedbackView',
    'js/ListFeedback/ListFeedbackView',
    'lib/simpleheat',
    'js/RightPanel/RightPanelView',
    'js/Tracker/ClientInfoModel',
    'js/NetworkAnalytic/NetworkAnalyticView',
    'js/Tracker/MouseTrackCollection',
    'js/Tracker/MouseTrackModel',
    'js/Tracker/SessionModel',
    'js/ListFeedback/ListFeedbackCollection',
    'js/ListFeedback/ListFeedbackModel',
    'js/Feedback/FeedbackModel',
    'js/VisitorLog/VisitorLogModel',
    'js/VisitorLog/VisitorLogCollection',
    'js/Table/TableView',
    'js/MessageBox/MessageBoxView',
    'js/WidgetContent/WidgetContentView',
    'js/SubscriberWidget/SubscriberWidgetView',
    'js/SearchContent/SearchWidgetView',
    'js/ContentWidget/ContentWidgetView'



], function (
    WidgetCollection,
    SignInButton,
    ToolbarView,
    LeftMenuView,
    WidgetView,
    WidgetModel,
    FeedBackView,
    ListFeedbackView,
    simpleHeat,
    RightPanelView,
    ClientInfoModel,
    NetworkAnalyticView,
    MouseTrackCollection,
    MouseTrackModel,
    SessionModel,
    ListFeedbackCollection,
    ListFeedbackModel,
    FeedbackModel,
    VisitorLogModel,
    VisitorLogCollection,
    Table,
    MessageBoxView,
    WidgetContent,
    SubscriberWidget,
    SearchWidget,
    ContentWidget
) {

    var $body = ENG.$('body');

    var scriptSrc = ENG.$("script[src*='eng.js']").attr('src');
    var cid = ENG.Utils.getUrlParam('cid', scriptSrc);

    ENG.cid = cid;



    ENG.Model = {};
    ENG.Collection = {};
    ENG.Model.MouseTrackModel = MouseTrackModel;
    ENG.Model.VisitorLogModel = VisitorLogModel;
    ENG.Model.ClientInfoModel = ClientInfoModel;
    ENG.Model.SessionModel = SessionModel;

    ENG.Collection.MouseTrackCollection = MouseTrackCollection;
    ENG.enum = {
        subMenu: {
            mouseMoveHM: "MOUSE_MOVE_HEAD_MAP",
            mouseClickHM: "MOUSE_CLICK_HEAD_MAP",
            overView: "OVER_VIEW",
            audience: "AUDIENCE",
            log: "LOGS",
            locations: "LOCATIONS",
            settings: "SETTINGS",
            devices: "DEVICES",
            trackSetting: "TRACKSETTING",
            systemSetting: "SYSSETTING",
            territoryBuilder: "TERRITORYBUILDER"
        },
        trackingSetting: {
            Enable: 1,
            Disable: 2,
        },
        dragDropWidget: {
            WIDGET: "WIDGET",
            WIDGETCONTENT: "WIDGETCONTENT"
        },
        typeWidget: {
            CONTENT: "Content",
            SUBSCRIBE: "Subscribe",
            SEARCH: "Search",
            CONTACTUS: "Contact Us"
        },
        chartType: {
            table: "table",
            donut: "donut",
            vertical: "vertical",
            map: "map",
            interactive: "interactive",
            quickStats: "quickstats",
            dynamicColumnTable: "dynamicColumnTable"
        },
        buttonType: {
            collapse: "collapse",
            close: "close",
            add: "add",
        },
        reportType: {
            websiteActivity: "Website Activity",
            visitorUserAgent: "Visitor User Agent",
            quickStats: "Quick Stats",
            networkActivity: "Network Activity",
            country: "Country",
            city: "City",
            //region: "Region",
            browserLanguage: "Browser Language",
            continent: "Continent",
            provider: "Provider",
            plugin: "Plugin",
            configuration: "Configuration",
            resolution: "Resolution",
            deviceType: "Device Type",
            deviceModel: "Device Model",
            deviceBrand: "Device Brand",
            operatingSystem: "Operating System",
            browser: "Browser",
        },
    };

    var ux = {

        cmp: {
            loginView: null,
            toolbarView: null,
        },

        collections: {
            widgets: null
        },


        initLoginButton: function () {

            //login form
            this.cmp.loginView = new SignInButton();
            ENG.signInButton = this.cmp.loginView;
            $body.append(this.cmp.loginView.render().$el);

        },

        init: function () {

            //set cookie
            this.initCookie();






        },

        initState: function () {
            var me = this;



            ENG.Api.Auth.Profile.getCurrentSession(function (success, data) {
                me.initLoginButton();
                ENG.ddata = data;
                me.loadData();
                
                ENG.loadtrackingSetting(function () {
                    if (ENG.Utils.isTrack(ENG.trackingSetting.PageViewsCounter)) {
                        var clientInfo = new ClientInfoModel();

                        clientInfo.set({
                            ClientId: ENG.cid,
                            Width: screen.width,
                            Height: screen.height,
                            PageUrl: window.location.href,
                            Referrer: document.referrer,
                            ViewerId: ENG.Utils.getCookie('ENGViewerCookie'),
                            SessionId: ENG.Utils.getCookie('ENGSessionCookie')

                        });
                        clientInfo.engSave();
                    }
                    ENG.Utils.trackMouse();
                });


            }, this);
        },

        initAuthState: function (data) {
            var singinButton = new SignInButton();
            var success = true;
            singinButton.engHandleData(success, data);

            // set for later use
            ENG.SESSION = data;
            // hide
            this.cmp.loginView.$el.hide();

        },

        initUnauthState: function () {
            // clear session
            ENG.SESSION = null;
            // show
            this.cmp.loginView.$el.show();

        },

        loadData: function (callback, scope) {

            ENG.setRightMenuPosition();
            ENG.lstHiddenItem = [];

            ENG.listFeedback = new ListFeedbackCollection();
            ENG.logCollection = new VisitorLogCollection();

            //Init Mouse Tracking Objects
            ENG.Utils.mouseMoveArr = new MouseTrackCollection();
            ENG.Utils.mouseClickArr = new MouseTrackCollection();
            ENG.Utils.flushData();

            var scriptSrc = ENG.$("script[src*='eng.js']").attr('src');
            var cid = ENG.Utils.getUrlParam('cid', scriptSrc);
            ENG.cid = cid;
            // load widgets
            // load preferences
            ENG.widgets = new WidgetCollection();
            Xdr.ajax({
                url: ENG.ApiDomain + '/umbraco/api/WidgetContent/GetWidget?ClientId=' + ENG.cid
            }, function (response) {
                if (response.success) {
                    _.each(response.data, function (item) {
                        if (item.WidgetTypeName !== ENG.enum.typeWidget.CONTENT) {
                            var widgetModel = new WidgetModel();
                            widgetModel.set("Color", item.Color);
                            widgetModel.set("Name", item.Name);
                            widgetModel.set("Url", item.Url);
                            widgetModel.set("WidgetTypeName", item.WidgetTypeName);
                            ENG.widgets.push(widgetModel);
                        }
                    });

                    var contentWidget = _.filter(response.data, function (item) {
                        return item.WidgetTypeName === ENG.enum.typeWidget.CONTENT;
                    });

                    if (contentWidget.length > 0) {
                        var widgetModel = new WidgetModel();
                        widgetModel.set("Color", contentWidget[0].Color);
                        widgetModel.set("Name", contentWidget[0].Name);
                        widgetModel.set("Url", contentWidget[0].Url);
                        widgetModel.set("WidgetTypeName", contentWidget[0].WidgetTypeName);
                        var data = [];
                        _.each(contentWidget, function (item) {
                            var itemData = {
                                Content: item.Content,
                                Title: item.Title
                            };
                            data.push(itemData);
                        });
                        widgetModel.set("Data", data);
                        ENG.widgets.push(widgetModel);
                    }
                }
            });


            ENG.analyticItem = [];

            this.loadInnerWidget();

            //load tracking setting
            var data = ENG.ddata;
            if (data == null) this.initUnauthState();
            else this.initAuthState(data);


        },
        loadjQueryUI: function () {

            if (typeof $ !== "undefined" && $.ui) {
                ENG.$.ui = $.ui;
                return;
            }

            require([ENG.DOMAIN + '/lib/jquery-ui.min.js'], function () {
                ENG.loadCss(ENG.DOMAIN + "/lib/jquery-ui.css");
                // ux.initSignInButton();
            });  // eof
        },

        initCookie: function () {
            //Create Viewer Cookie
            if (ENG.Utils.getCookie("ENGViewerCookie") === "") {
                var viewerID = ENG.Utils.GUID();
                //Create Cookie never expire
                ENG.Utils.createCookie("ENGViewerCookie", viewerID, 10000);
            }

            //Create Session Cookie
            var sessionID = ENG.Utils.GUID();
            ENG.Utils.createCookie("ENGSessionCookie", sessionID);

            var me = this;


            setTimeout(function () {
                me.initState();
            }, 2000);

        },

        loadInnerWidget: function (listWidget) {
            //_.isArray(listWidget)
            Xdr.ajax({
                url: ENG.ApiDomain + '/umbraco/api/WidgetContent/GetAllWidget?ClientId=' + ENG.cid
            }, function (response) {
                if (response.success) {
                    var widgetIsLoaded = _.filter(response.data, function (item) {
                        return item.URL === window.location.href;
                    });

                    var widgetContentModel = _.filter(widgetIsLoaded, function (item) {
                        return item.WidgetTypeName === ENG.enum.typeWidget.CONTENT;
                    });
                    widgetContentModel = _.groupBy(widgetContentModel, function (item) {
                        return item.ID;
                    });
                    if (widgetContentModel) {

                        for (x in widgetContentModel) {
                            var widgetContent = new WidgetContent();
                            var item = widgetContentModel[x][0];
                            var position = item.Position;
                            var itemArray = position.split("-");
                            var parentWidget;
                            var positionToInsert;
                            for (var i = itemArray.length; i--; i > 0) {
                                var node = itemArray[i].split(".");
                                var tagName = node[0];
                                var index = node[1];
                                if (i === 0) {
                                    positionToInsert = index;

                                    widgetContent.opts.data = widgetContentModel[x];
                                    var widgetModel = new WidgetModel();
                                    widgetModel.set("ID", item.ID);
                                    widgetModel.set("Color", item.Color);
                                    widgetModel.set("Name", item.Name);
                                    widgetModel.set("URL", item.URL);
                                    widgetModel.set("WidgetTypeName", item.WidgetTypeName);
                                    widgetModel.set("Data", item);
                                    var content = new ContentWidget();
                                    widgetContent.opts.model = widgetModel;
                                    widgetContent.opts.itemChilds = content;

                                    content.opts.data = widgetContentModel[x];

                                    positionToInsert = _.isNumber(positionToInsert) ? positionToInsert : parseInt(positionToInsert);
                                    parentWidget.appendToWithIndex(widgetContent.render().$el, positionToInsert);
                                }
                                if (parentWidget) {
                                    parentWidget = ENG.$(parentWidget.children()[index]);
                                } else {
                                    parentWidget = ENG.$(ENG.$(tagName).children()[index]);
                                }
                            }
                        }
                    }


                    _.each(widgetIsLoaded, function (item) {
                        var postionWidget = item.Position;
                        var itemArray = postionWidget.split("-");
                        var parentWidget;
                        var positionToInsert;
                        for (var i = itemArray.length; i--; i > 0) {
                            var node = itemArray[i].split(".");
                            var tagName = node[0];
                            var index = node[1];
                            if (i === 0) {
                                positionToInsert = index;
                                var widgetContent = new WidgetContent();
                                var widgetModel = new WidgetModel();
                                widgetModel.set("ID", item.ID);
                                widgetModel.set("Color", item.Color);
                                widgetModel.set("Name", item.Name);
                                widgetModel.set("URL", item.Url);
                                widgetModel.set("WidgetTypeName", item.WidgetTypeName);
                                switch (item.WidgetTypeName) {
                                    case ENG.enum.typeWidget.SEARCH:
                                        var search = new SearchWidget();
                                        widgetContent.opts.itemChilds = search;
                                        widgetContent.opts.model = widgetModel;
                                        widgetContent.opts.id = item.ID;
                                        positionToInsert = _.isNumber(positionToInsert) ? positionToInsert : parseInt(positionToInsert);
                                        parentWidget.appendToWithIndex(widgetContent.render().$el, positionToInsert);
                                        break;
                                    case ENG.enum.typeWidget.SUBSCRIBE:
                                        var subscribe = new SubscriberWidget();
                                        widgetContent.opts.itemChilds = subscribe;
                                        widgetContent.opts.model = widgetModel;
                                        widgetContent.opts.id = item.ID;
                                        positionToInsert = _.isNumber(positionToInsert) ? positionToInsert : parseInt(positionToInsert);
                                        parentWidget.appendToWithIndex(widgetContent.render().$el, positionToInsert);
                                        break;
                                }
                            }
                            if (parentWidget) {
                                parentWidget = ENG.$(parentWidget.children()[index]);
                            } else {
                                parentWidget = ENG.$(ENG.$(tagName).children()[index]);
                            }
                        }
                    });
                }
            });


        }
    };

    // Start

    //load jqueyUi and signin button
    ux.loadjQueryUI();

    ux.init();

    //ENG.Utils.flushData();

    ENG.Utils.loadMapLib();
    ENG.Utils.loadPace();
    ENG.Utils.loadTextEditor();



    //window.requestAnimationFrame = window.requestAnimationFrame || window.mozRequestAnimationFrame ||
    //                           window.webkitRequestAnimationFrame || window.msRequestAnimationFrame;


    //var data = [];



    //var heat = simpleheat('eng-mouseHeatMap').data(data).max(18),
    //    frame;

    //ENG.heat = heat;






});


//widget workflow
//  +   app.js
//      -   loadData(); -> load all data from database, store in ENG.widgets(cache for reuse)
//  +   ToolbarView.js
//      -   ENG.leftMenu.loadData(ENG.widgets); ->  load data from ENG.widgets above then add into LeftMenuView
//  +   LeftMenuView.js
//      -   loadData -> Add widget into LeftMenuView
//          * Drag workflow
//              +   WidgetView.js
//                  -   initDragDrop -> init drag event
//                          -> draggable -> start -> set typeWidget = ENG.enum.dragDropWidget.WIDGET
//          *   Drop workflow
//                   -> ENG.$("body").droppable ->  define drop event of WidgetView component
//                      .   Check div has eng-component -> never drop to our system
//                      .   Find wrapper div
//                      .   ENG.typeWidget === ENG.enum.dragDropWidget.WIDGET 
//                          -> New widget content 
//                              -> set typeWidget = ENG.enum.dragDropWidget.WIDGETCONTENT
//                          -> Append to div
//                      .   ENG.typeWidget === ENG.enum.dragDropWidget.WIDGETCONTENT -> append current widget content to div
