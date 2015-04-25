define([
    "text!js/Notifications/Notifications.html",
    'js/Component'
],

function (Template, Component) {

    return Component.extend({
        events: {
            "click .eng-close-notification": "close",
            "click .item-notification": "viewNotification"
        },
        constructor: function (options) {
            var opts = ENG.$.extend(true, {
                model: null
            }, options);

            Component.prototype.constructor.call(this, opts);
            this.$el.addClass('eng-notifications');
            this.$el.html('');
            var urlCss = ENG.DOMAIN + "/js/Notifications/Notifications.css";
            ENG.loadCss(urlCss);
        },
        render: function (model) {
            Component.prototype.render.call(this);
            var html = "";
            var compile = _.template(this.template());
            html = compile({ notifications: ENG.notifications.models });
            this.$el.html(html);
            return this;

        },
        template: function () {
            return Template;
        },
        close: function (e) {
            ENG.$(e.currentTarget).parents(".eng-notifications").hide();
        },
        updateStatusNotification: function (e) {
            var currentNotification = ENG.$(e.currentTarget);
            var id = currentNotification.prop("id");
            var itemUpdate = _.filter(ENG.notifications.models, function (item) {
                return item.id === id;
            })[0];

            if (itemUpdate && !itemUpdate.get("seen")) {

                var apiUrl = url({
                    url: ENG.ApiDomain + "/umbraco/api/EngTrack/CollectNotification",
                    type: "POST",
                    data: {
                        Id_s: itemUpdate.get("id"),
                        Title_s: itemUpdate.get("title"),
                        Message_tsd: itemUpdate.get("message"),
                        Seen_b: true,
                        ClientId_s: itemUpdate.get("clientId")
                    }
                });

                Xdr.ajax({
                    url: apiUrl
                }, function (response) {
                    if (response & response.success) {
                        if (response.data.data.success) {
                            _.each(ENG.notifications.models, function (item) {
                                if (item.id === id) {
                                    item.set("seen", true);
                                }
                            });
                            currentNotification.css("background", "#F6F7F8");
                            currentNotification.css("color", "black");
                            currentNotification.css("box-shadow", "0 0 6px #F6F7F8");
                            var text = ENG.$(".eng-label-notification").text();

                            var notificationsNotSee = _.filter(ENG.notifications.models, function (item) {
                                return item.get("seen") === false;
                            });

                            if (notificationsNotSee.length === 0) {
                                ENG.$(".eng-label-notification").hide();
                            } else {
                                ENG.$(".eng-label-notification").show();
                                ENG.$(".eng-label-notification").text(notificationsNotSee.length);
                            }
                        } else {

                        }
                    }
                });
            }
        },
        viewNotification: function (e) {
            this.updateStatusNotification(e);
        }
    });
});