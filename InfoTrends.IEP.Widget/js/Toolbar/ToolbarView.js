define([
    'text!js/Toolbar/Toolbar.html',
    'js/Component',
    'js/LeftMenu/LeftMenuModel',
    'js/ListFeedback/ListFeedbackView',
    'js/Notifications/NotificationsView',
    'js/Notifications/NotificationCollection',
    'js/Notifications/NotificationModel'
], function (Template, Component, LeftMenuModel, ListFeedbackView, Notifications, NotificationCollections, NotificationModel) {

    return Component.extend({

        events: {
            'click .eng-toolbar-item': 'showLeftPanelContent',
            'click #AddFeedback': 'showFeedback',
            'click #ListFeedback': 'listFeedback',
        },
        constructor: function (options) {

            var cssLinkVal = ENG.DOMAIN + '/js/Toolbar/Toolbar.css';

            var opts = ENG.$.extend(true, {
                name: name,
                cssLink: cssLinkVal
            }, options);

            Component.prototype.constructor.call(this, opts);

            this.$el.addClass('eng-toolbar');
            var urlCss = ENG.DOMAIN + "/js/Toolbar/Toolbar.css";
            ENG.loadCss(urlCss);

        },
        render: function () {

            Component.prototype.render.call(this);
            var me = this;
            this.handleData(function () {
                var data = _.filter(ENG.notifications.models, function (item) {
                    return item.get("seen") === false;
                });

                var compile = _.template(me.template());
                var html = compile({ numberNotifications: data.length });
                me.$el.append(html);
            });
            return this;
        },
        template: function () {
            return Template;
        },
        showLeftPanelContent: function (e, item) {

            var techonologies = ENG.Utils.detechTechonology();
            var me = this;
            var nameTab = ENG.$(e.currentTarget).data("value");
            ENG.rightPanel.showPosition(ENG.isRightLeftPanel);

            //add Active class
            var toolbarItem = ENG.$(e.target).parents(".eng-toolbar-item");
            var toolbarItemActive = toolbarItem.siblings().children("a.eng-ToolbarItem-Active");
            toolbarItemActive.parents(".eng-toolbar-item").find(".eng-subToolBar").hide();
            toolbarItemActive.removeClass("eng-ToolbarItem-Active");

            toolbarItem.children("a:first-child").addClass("eng-ToolbarItem-Active");
            if (nameTab === "Widgets") {
                //Left button add widget
                var model = new LeftMenuModel();
                model.set({
                    title: nameTab,
                    favicon: "fa fa-th"
                });
                ENG.leftMenu.render(model);

                ENG.leftMenu.setVisibility(true);
                ENG.rightPanel.setVisibility(false);
                ENG.leftMenu.loadData(ENG.widgets);
                
                ENG.leftMenu.initDragAndDrop();

            } else if (nameTab === "Analytics") {
                var model = new LeftMenuModel();
                model.set({
                    title: nameTab,
                    favicon: "fa fa-laptop"
                });
                ENG.leftMenu.render(model);

                ENG.leftMenu.loadExpandMenu();


                ENG.leftMenu.setVisibility(true);
                ENG.rightPanel.setVisibility(false);

                ENG.leftMenu.addColapseButton();
            } else if (nameTab === "Recommendations") {
                var model = new LeftMenuModel();
                model.set({
                    title: nameTab,
                    favicon: "fa fa-laptop"
                });
                ENG.leftMenu.render(model);
                ENG.leftMenu.setVisibility(true);
                ENG.rightPanel.setVisibility(false);
            } else if (nameTab === "Account") {
                if (ENG.$(e.target).data("value") == undefined) {
                    this.showSubMenu(e);
                } else {
                    if (ENG.$(e.target).data("value") == "Settings") {
                        var model = new LeftMenuModel();
                        model.set({
                            title: "Settings",
                            favicon: "fa fa-user"
                        });
                        ENG.leftMenu.render(model);

                        ENG.leftMenu.loadSettingMenu();


                        ENG.leftMenu.setVisibility(true);
                        ENG.rightPanel.setVisibility(false);

                        ENG.$(e.target).parents(".eng-toolbar-item").find(".eng-subToolBar").hide(500);
                        ENG.leftMenu.addColapseButton();
                    } else if (ENG.$(e.target).data("value") == "SignOut") {
                        ENG.Api.Auth.Profile.logout(function () {
                            sessionStorage.clear();
                            window.location.reload();
                        }, this);


                    }
                }

            } else if (nameTab === "Notification") {
                var engNotifications = ENG.$(".eng-notifications");
                if (engNotifications.length > 0) {
                    var display = engNotifications.css("display");
                    if (display === "none") {
                        ENG.notificationsPanel.render();
                        ENG.notificationsPanel.setVisibility(true);
                    } else {
                        ENG.notificationsPanel.setVisibility(false);
                    }
                } else {
                    var notifications = new Notifications();
                    var $body = ENG.$('body');
                    ENG.notificationsPanel = notifications;
                    $body.append(notifications.render().$el);
                }

            } else {
                ENG.rightPanel.setVisibility(false);
                ENG.leftMenu.setVisibility(false);
                this.showSubMenu(e);
            }

            ENG.leftMenu.loadMenuStyle();
        },
        showSubMenu: function (e) {
            var toolbarItem = ENG.$(e.target).parents(".eng-toolbar-item");
            var subMenu = toolbarItem.find(".eng-subToolBar");

            if (typeof (subMenu.css("display")) === "undefined" || subMenu.css("display") === "none") {
                //
                subMenu.show(500);
            } else {
                subMenu.hide(500);
            }
        },
        showFeedback: function () {
            ENG.$("#eng_Feedback").show();
            ENG.$(".eng_blur").show();

            ENG.$("#eng_Feedback").draggable({
                handle: "h2",
                cursor: "move", cursorAt: { top: -150, left: 0 }
            });
            var jqte = ENG.$(".jqte");
            if (jqte.length === 0) {
                ENG.$('#content_TextArea').jqte();
                ENG.$('.jqte_editor').focus();
            } else {
                ENG.$('.jqte_editor').focus();
                return;
            }
        },
        listFeedback: function () {
            ENG.$("#eng-listFeedbackBlur").show();
            ENG.listFB.listFeedback();
        },
        handleData: function (callBack) {
            ENG.notifications = new NotificationCollections();
            this.getDataNotification(function () {
                callBack();
            });
            var me = this;

            //Each 10 minutes, this function is run to update notifications
            setInterval(function () {
                me.getDataNotification(function () {
                    var notificationsNotSee = _.filter(ENG.notifications.models, function (item) {
                        return item.get("seen") === false;
                    });

                    if (notificationsNotSee.length === 0) {
                        ENG.$(".eng-label-notification").hide();
                    } else {
                        ENG.$(".eng-label-notification").show();
                        ENG.$(".eng-label-notification").text(notificationsNotSee.length);
                    }

                    if (ENG.notificationsPanel) {
                        ENG.notificationsPanel.render();
                    }
                });
            }, 10 * 60 * 1000);

        },

        /*
        get Data notifications
        callBack: sync with Toolbar. This function run before toolbar is rendered.
        */
        getDataNotification: function (callBack) {
            Xdr.ajax({
                url: ENG.ApiDomain + '/umbraco/api/EngQuery/GetNotification?clientId=' + ENG.cid,

            }, function (response) {
                if (response && response.success) {
                    if (ENG.Utils.checkObjExist(response, 'data.data.response.docs')) {
                        var listNotifications = _.isArray(response.data.data.response.docs) ? response.data.data.response.docs : [response.data.data.response.docs];
                        _.each(listNotifications, function (item) {
                            var notificationModel = new NotificationModel();
                            notificationModel.set("title", item.Title_s);
                            notificationModel.set("message", item.Message_tsd);
                            notificationModel.set("id", item.Id_s);
                            notificationModel.set("time", ENG.Utils.dateDiff(new Date(item.CreateOn_dt)));
                            notificationModel.set("seen", item.Seen_b);
                            notificationModel.set("clientId", item.ClientId_s);
                            notificationModel.set("dateCreate", new Date(item.CreateOn_dt));
                            ENG.notifications.push(notificationModel);
                        });
                    }
                    ENG.notifications.models.sort(function (a, b) {
                        return a.get("dateCreate").getTime() - b.get("dateCreate").getTime();
                    });
                    ENG.notifications.models.reverse();
                    callBack();
                } else {
                    callBack();
                }
            });
        }
    });
});