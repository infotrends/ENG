define([
    'text!js/WidgetSetting/WidgetSetting.html',
    'js/Container'

], function (Template, Container) {
    return Container.extend({

        events: {
            "click .eng-close": "closePopup",
            "click .eng-btn-cancel": "closePopup"
        },
        
        constructor: function (options) {
            var opts = ENG.$.extend(true, {
                id: null
            }, options);

            Container.prototype.constructor.call(this, opts);
            this.$el.addClass('eng-widget-setting');
            var urlCss = ENG.DOMAIN + "/js/WidgetSetting/WidgetSetting.css";
            ENG.loadCss(urlCss);
        },
        
        render: function () {
            Container.prototype.render.call(this);
            var compile = _.template(this.template());
            html = compile({});
            this.$el.html(html);
            return this;
        },
        
        template: function () {
            return Template;
        },
        closePopup: function(e) {
            this.remove();
        }
    });
});