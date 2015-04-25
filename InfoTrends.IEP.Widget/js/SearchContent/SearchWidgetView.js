define([
    'text!js/SearchContent/SearchWidget.css',
    'text!js/SearchContent/SearchWidget.html',
    'js/Component'

], function (Css, Template, Component) {
    return Component.extend({

        events: {
            "click .eng-search-button": "searchContent"
        },
        constructor: function (options) {
            var opts = ENG.$.extend(true, {
                id: null,
                dragable: true,
                model: null,
                data: null
            }, options);

            Component.prototype.constructor.call(this, opts);
            this.$el.addClass('eng-search-content');
            var urlCss = ENG.DOMAIN + "/js/SearchContent/SearchWidget.css";
            ENG.loadCss(urlCss);
        },
        setBackground: function (color) {
        },
        render: function (model) {
            Component.prototype.render.call(this);
            var compile = _.template(this.template());
            html = compile({ data: this.opts.data});
            this.$el.html(html);
            return this;
        },
        template: function () {
            return Template;
        },
        searchContent: function () {
            
        }
    });
});