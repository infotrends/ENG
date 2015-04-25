define([
    'text!js/SubscriberWidget/SubscriberWidget.html',
    'js/Component',
    'js/MessageBox/MessageBoxView'
],
function (Template, Component, MessageBox) {
    return Component.extend({
        events: {
            "click .eng-search-button": "addSubscriber",
            "click .eng-close":"closeErrorMessage"
        },
        constructor: function(options) {
            var opts = ENG.$.extend(true, {
                id: null,
                model: null,
                data: null,
                messageTemplateError: '<div class="eng-alert eng-alert-danger eng-alert-dismissable"><button class="eng-close" type="button">×</button><%=message%></div>',
                messageTemplateSuccess: '<div class="eng-alert eng-alert-success eng-alert-dismissable"><button class="eng-close" type="button">×</button><%=message%></div>'
            }, options);

            Component.prototype.constructor.call(this, opts);
            this.$el.addClass('eng-subscriber-widget');

            var urlCss = ENG.DOMAIN + "/js/SubscriberWidget/SubscriberWidget.css";
            ENG.loadCss(urlCss);
        },
        render: function(model) {
            Component.prototype.render.call(this);
            var compile = _.template(this.template());
            html = compile({ data: this.opts.data });
            this.$el.html(html);
            return this;
        },
        template: function() {
            return Template;
        },
        addSubscriber: function() {
            var email = this.$el.find(".eng-email-subscribe");
            var emailVal = email.val();
            
            var valid = ENG.validateEmail(emailVal);
            if (valid) {
                var me = this;
                Xdr.ajax({
                        url: ENG.ApiDomain + "/umbraco/api/WidgetContent/Subscribe",
                        data: {
                            Email: emailVal,
                            ClientID: ENG.cid
                        },
                        type: "POST"
                    }, function(response) {
                        if (response.success) {
                            email.val("");
                            me.showSuccessMessage("Subscribe is Successful");
                        }
                    });
            } else {
                this.showErrorMessage("Email is not valid!");
            }
        },
        showErrorMessage: function(message) {
            var container = this.$el.find(".eng-ibox-content");
            var errorBox = _.template(this.opts.messageTemplateError);
            var html = errorBox({ message: message });
            container.append(html);
        },
        showSuccessMessage: function (message) {
            var container = this.$el.find(".eng-ibox-content");
            var successBox = _.template(this.opts.messageTemplateSuccess);
            var html = successBox({ message: message });
            container.append(html);
        },
        closeErrorMessage: function(e) {
            var boxError = ENG.$(e.currentTarget).parents(".eng-alert");
            boxError.remove();
        }
    });
});