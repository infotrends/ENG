define([
    'text!js/Widget/Widget.css',
    'text!js/Widget/Widget.html',
    'js/Component'

], function (Css, Template, Component) {

    return Component.extend({
        events: {
            "click .eng-under>ul>li>a": "clickWidgetFooter",
            "mousemove .eng-under>ul>li>a": "setBackgound",
            "mouseout .eng-under>ul>li>a": "resetBackgound"
        },
        constructor: function(options) {

            var opts = ENG.$.extend(true, {
                /*backGroundColor: "#ffff",*/
                id: null,
                dragable: true
            }, options);

            Component.prototype.constructor.call(this, opts);
            this.$el.addClass('eng-box-widget');
            this.setBackGroundColor(opts.backGroundColor);
        },
        render: function () {
            Component.prototype.render.call(this);
            
            var compile = _.template(this.template());
            var html;
            var widgetName = "";
            var icon;
            model = this.opts.model;
            if (model && model.attributes)
                widgetName = model.get("WidgetTypeName");
            switch(widgetName) {
                case ENG.enum.typeWidget.CONTENT:
                    icon = "fa fa-newspaper-o";
                    break;
                case ENG.enum.typeWidget.SEARCH:
                    icon = "fa fa-search";
                    break;
                case ENG.enum.typeWidget.SUBSCRIBE:
                    icon = "fa fa-play";
                    break;
                default:
                    icon = "fa fa-chain-broken";
            }
            html = compile({ name: widgetName, icon: icon });
            this.$el.html(html);
            //Drag drop widget
            this.initDragDrop();
            //this.setBackGroundColor("#" + model.get("Color"));
            /*this.$el.find(".eng-top").css("background-color", "#" + model.get("Color"));*/
            this.setBackGroundColor( "#" + model.get("Color"));
            return this;
        },
        template: function() {
            return Template;
        },
        clickWidgetFooter: function (e) {
            var nameTab = ENG.$(e.currentTarget).data("value");
            if (nameTab === "settings") {
                
            }else if (nameTab === "viewDetails") {
                
            }
        },
        initDragDrop: function () {
            var me = this;
            this.$el.draggable({
                revert: true,
                handle: ".eng-top",
                scroll: true,
                cursor: "move",
                cursorAt: { left: -5, top: 0 },
                appendTo: "body",
                start: function (param) {
                    ENG.hightLightItem = true;
                    ENG.typeWidget = ENG.enum.dragDropWidget.WIDGET;
                    ENG.dragInClient = me;
                    
                    
                    var leftMenu = ENG.$(".eng-leftPanel .eng-container");
                    leftMenu.css("overflow-x", "visible");
                    leftMenu.css("overflow-y", "visible");
                },
                stop: function () {
                    
                    ENG.hightLightItem = false;
                    var leftMenu = ENG.$(".eng-leftPanel .eng-container");
                    leftMenu.css("overflow-x", "hidden");
                    leftMenu.css("overflow-y", "auto");
                }
            });
        },
        setBackgound: function (e) {
            ENG.$(e.currentTarget).css("background", "#"+ this.opts.model.get("Color"));
        },
        resetBackgound: function (e) {
            ENG.$(e.currentTarget).css("background", "initial");
        }
});
});