/// <reference path="_global.js" />


define([
        


],
function () {
    return Backbone.View.extend({
        tagName: 'div',
        className: 'eng-component',

        id: null,
        opts: null,

        constructor: function (options) {

            /* Put the code will be executed before Backbone */

            Backbone.View.prototype.constructor.call(this, options);

            /* Put the code will be executed after Backbone */

            this.opts = ENG.$.extend(true, {
                css: {},
                visible: true
            }, options);

            // Make sure to use our jquery version
            this.$el = ENG.$(this.el);

            this.id = this.opts.id;
            this.$el.attr('id', this.id);
            this.$el.css(this.opts.css);
            
//            this.setWidth(this.opts.width);
//            this.setHeight(this.opts.height);
            this.setVisibility(this.opts.visible);

        },
        render: function () {
            Backbone.View.prototype.render.call(this);
            return this;
        },
        setWidth: function(value){
            if (_.isString(value)) {
                if (value.toLowerCase().indexOf('%') == -1) {
                    value = parseFloat(value);
                }
            }
            
            if (_.isString(value)) {
                this.$el.css('width', value);
            } else {
                this.$el.css('width', value + 'px');
            }
        },
        setHeight: function (value) {
            if (_.isString(value)) {
                if (value.toLowerCase().indexOf('%') == -1) {
                    value = parseFloat(value);
                }
            }

            if (_.isString(value)) {
                this.$el.css('height', value);
            } else {
                this.$el.css('height', value + 'px');
            }
        },
        setVisibility: function(value) {
            value = _.isBoolean(value) ? value : true;
            if (value) {
                this.$el.css("display", "block");
            } else {
                this.$el.css("display", "none");
            }
        },
        //input: json
        setPosition: function(value) {
            if (value.top) {
                this.$el.css("top", value.top);
            }
            if (value.right) {
                this.$el.css("right", value.right);
            }
            if (value.bottom) {
                this.$el.css("bottom", value.bottom);
            }
            if (value.left) {
                this.$el.css("left", value.left);
            }
        },
        setBackGroundColor: function (value) {
            if(value) {
                this.$el.css("background-color", value);
            }
        },
        setDragable: function(value) {
            var isDrag = _.isBoolean(value) ? value : false;
            if (isDrag) {
                this.$el.draggable();
            }
            
        }
    });
});