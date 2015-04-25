define([
    'text!js/ContentWidget/ContentWidget.html',
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
            this.$el.addClass('eng-content-widget-item');
            var urlCss = ENG.DOMAIN + "/js/ContentWidget/ContentWidget.css";
            ENG.loadCss(urlCss);
        },
        render: function (model) {
            Component.prototype.render.call(this);
            var compile = _.template(this.template());
            console.log(this.opts.data);
            html = compile({ data: this.opts.data});
            this.$el.html(html);
            return this;
        },
        template: function () {
            return Template;
        }
    });
});