define([
    "text!js/RightPanel/RightPanel.html",
    'js/Container',
    'js/NetworkAnalytic/NetworkAnalyticView'
],

function (Template, Container, NetworkAnalyticView) {

    return Container.extend({


        events: {
            'click .eng-close': 'closeAll',
        },
        constructor: function (options) {

            var opts = ENG.$.extend(true, {
                position: {
                    right: "0"
                },
                model: null
            }, options);

            Container.prototype.constructor.call(this, opts);

            this.$el.addClass('eng-rightPanel');
            this.$el.html('');
            var urlCss = ENG.DOMAIN + "/js/RightPanel/RightPanel.css";
            ENG.loadCss(urlCss);

        },
        render: function (model) {
            Container.prototype.render.call(this);
            var html = "";
            var compile = _.template(this.template());
            html = compile({});
            this.$el.append(html);
            this.showPosition();
            return this;
        },
        template: function () {
            return Template;
        },
        showPosition: function (isRight) {

            var size = ENG.$(".eng-leftPanel").width();
            var bottom = ENG.$(".eng-toolbar").height();
            if (isRight) {
                var position = {
                    left: "0",
                    right: size,
                    top: "0",
                    bottom: bottom
                };
                this.setPosition(position);
            } else {
                var position = {
                    left: size,
                    right: "0",
                    top: "0",
                    bottom: bottom
                };
                this.setPosition(position);
            }

        },
        closeAll: function () {
            this.clear();
            ENG.$(".eng-close").css("opacity", "0");
            ENG.rightPanel.setVisibility(false);
        },

    });
});