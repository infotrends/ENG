define([
    'text!js/Loading/Loading.html',
    'js/Component',
],

function (Template, Component, TableView) {
    return Component.extend({
        data: [],
        constructor: function (options) {
            var opts = ENG.$.extend(true, {
            }, options);


            Component.prototype.constructor.call(this, opts);
            this.$el.html('');
            var urlCss = ENG.DOMAIN + "/js/Loading/Loading.css";
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
    });
});