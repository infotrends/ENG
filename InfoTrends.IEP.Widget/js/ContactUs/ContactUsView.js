define([
    'text!js/ContactUs/ContactUs.html',
    'js/Component'
],
function (Template, Component) {
    return Component.extend({

        events: {

        },
        constructor: function (options) {
            var opts = ENG.$.extend(true, {
                id: null,
                model: null,
                data: null
            }, options);

            Component.prototype.constructor.call(this, opts);
            this.$el.addClass('eng-contactUs-form');

            var urlCss = ENG.DOMAIN + "/js/ContactUs/ContactUs.css";
            ENG.loadCss(urlCss);
        },
        render: function (model) {
            Component.prototype.render.call(this);
            var compile = _.template(this.template());
            html = compile({ data: this.opts.data });
            this.$el.html(html);
            return this;
        },
        template: function () {
            return Template;
        }
    });
});