define([
    'text!js/HiddenItem/HiddenItem.html',
    'js/Component'

], function (Template, Component) {
    return Component.extend({

        events: {
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
        }
    });
});