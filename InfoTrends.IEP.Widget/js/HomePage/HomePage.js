define([
    'text!js/HomePage/HomePage.html',
    'js/Component'

    

],
function (Template, Component) {
    return Component.extend({

        


        constructor: function (options) {
            var cssLinkVal = ENG.DOMAIN + '/js/HomePage/HomePage.css';
            var opts = ENG.$.extend(true, {

                cssLink: cssLinkVal

            }, options);

            Component.prototype.constructor.call(this, opts);

            debugger;

            var urlCss = ENG.DOMAIN + "/js/HomePage/HomePage.css";
            ENG.loadCss(urlCss);
            var tabCss = ENG.DOMAIN + "/js/HomePage/easy-responsive-tabs.css";
            ENG.loadCss(tabCss);
            this.$el.addClass('eng-homePageWrapper');
            this.$el.removeClass('eng-component');

        },


        events: {
            'click .eng-hideHome': 'hideHomePage'
        },


        render: function () {

            Component.prototype.render.call(this);
            var compile = _.template(this.template());
            var html = compile({});
            this.$el.append(html);


            return this;
        },
        template: function () {
            return Template;
        },

        hideHomePage: function () {
            this.$el.hide();
        }




 

    });
});
