define([
    'text!js/MessageBox/MessageBox.css',
    'text!js/MessageBox/MessageBox.html',
    'js/Container'

], function (Css, Template, Container) {
    return Container.extend({

        events: {
            "click .eng-message-box-btn-cancel": "cancelClick",
            "click .eng-message-box-btn-ok": "okClick",
            "click .eng-panel-message-box-heading-btn>a": "cancelClick",
            "click .eng-blur": "blurClick"
        },
        constructor: function (options) {
            var opts = ENG.$.extend(true, {
                id: null,
                params: null,
                data: null,
                type: null,
                size: null
            }, options);

            Container.prototype.constructor.call(this, opts);
            this.$el.addClass('eng-message-box');
            var urlCss = ENG.DOMAIN + "/js/MessageBox/MessageBox.css";
            ENG.loadCss(urlCss);
        },
        render: function () {
            Container.prototype.render.call(this);
            var compile = _.template(this.template());
            html = compile({
                headerText: this.opts.params.headerText,
                messageText: this.opts.params.messageText
            });
            this.$el.html(html);

            this.showButton();
            //this.initDrag();

            this.setSize();
            return this;
        },
        template: function () {
            return Template;
        },
        cancelClick: function (e) {
            if ("yes-no" === this.opts.type) {
                this.trigger("cancel", this.opts.data);
            }
            this.remove();
        },
        okClick: function () {
            if ("yes-no" === this.opts.type) {
                this.trigger("ok", this.opts.data);
            }
            this.remove();
        },
        addCustomContent: function (contentCustom) {
            this.$el.find(".eng-message-box-custom").html(contentCustom);
        },
        showMessageBox: function () {
            var body = ENG.$("body");
            body.append(this.render().$el);
        },
        showButton: function () {
            if ("yes-no" !== this.opts.type) {
                this.$el.find(".eng-message-box-btn-cancel").hide();
            }
        },
        setSize: function () {
            if (this.opts.size == null) {
                this.opts.size = "medium";
            }

            var width = "";

            if (window.scrollbars.visible) {
                $('body').css('overflow', 'hidden');
                width = ENG.$(window).width();
                $('body').css('overflow', 'auto');
            }
            if (width > 768) {
                switch (this.opts.size) {
                    case "small":
                        width = "30%";
                        break;
                    case "medium":
                        width = "50%";
                        break;
                    case "large":
                        width = "80%";
                        break;
                    default:
                }

            } else {
                width = "100%";
            }

            this.$el.find(".eng-message-box-model").css("width", width);
        },
        blurClick: function (e) {
            this.remove();
        },
        initDrag: function () {
            this.$el.find(".eng-message-box-model").draggable({
                handle: ".eng-panel-message-box-heading",
                cursor: "move"
            });
        }

    });
});