define([
    'text!js/HiddenItem/HiddenItem.html',
    'js/Component'

], function (Template, Component) {
    return Component.extend({

        events: {
            "click .eng-open-hidden-item": "showMenuHidden",
            "click .eng-close": "closePopup",
            "click .eng-hidden-toggle" : "addItemHidden"
        },
        constructor: function (options) {
            var opts = ENG.$.extend(true, {
                id: null
            }, options);
            Component.prototype.constructor.call(this, opts);
            this.$el.addClass('eng-hidden-item');
            var urlCss = ENG.DOMAIN + "/js/HiddenItem/HiddenItem.css";
            ENG.loadCss(urlCss);
        },
        render: function (params) {
            Component.prototype.render.call(this);
            var compile = _.template(this.template());
            var html = compile();
            this.$el.append(html);
            return this;
        },
        template: function () {
            return Template;
        },
        showMenuHidden: function (e) {
            var currentTarget = ENG.$(e.currentTarget);
            currentTarget.hide();
            var options = this.$el.find(".eng-option-hidden");
            options.show();
        },
        closePopup: function(e) {
            this.$el.find(".eng-option-hidden").hide();
            this.$el.find(".eng-open-hidden-item").show();
        },
        addItemHidden: function(e) {
            var itemAdd = ENG.$(e.currentTarget);
            if (itemAdd.hasClass("eng-active-hidden")) {
                itemAdd.removeClass("eng-active-hidden");
            } else {
                itemAdd.addClass("eng-active-hidden");
            }
        }
    });
});