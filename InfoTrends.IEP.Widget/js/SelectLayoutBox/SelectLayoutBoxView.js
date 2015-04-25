define([
    'text!js/SelectLayoutBox/SelectLayoutBox.css',
    'text!js/SelectLayoutBox/SelectLayoutBox.html',
    'js/MessageBox/MessageBoxView',


], function (Css, Template, MessageBoxView) {
    return MessageBoxView.extend({

        events: {
            "click .eng-message-box-btn-cancel": "cancelClick",
            "click .eng-message-box-btn-ok": "okClick",
            "click .eng-panel-message-box-heading-btn>a": "cancelClick",
            "click .eng-blur": "blurClick"
        },
        constructor: function (options) {
            var opts = ENG.$.extend(true, {
            }, options);

            MessageBoxView.prototype.constructor.call(this, opts);
            //this.$el.addClass('eng-message-box');
            var urlCss = ENG.DOMAIN + "/js/SelectLayoutBox/SelectLayoutBox.css";
            ENG.loadCss(urlCss);
        },
        render: function () {
            MessageBoxView.prototype.render.call(this);
            var compile = _.template(this.template());
            html = compile({
                headerText: this.opts.params.headerText,
                messageText: this.opts.params.messageText
            });
            this.$el.html(html);

            this.setSize();
            this.$el.find("#eng-layout-select").selectable();


            return this;
        },
        template: function () {
            return Template;
        },
    });
});