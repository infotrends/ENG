define([
    'js/Component'
], function (Component) {
    return Component.extend({
        
        items: null,

        constructor: function (options) {
            var opts = ENG.$.extend(true, {

            }, options);
            
            Component.prototype.constructor.call(this, opts);

            this.items = [];
            
            var tpl = ['<div class="eng-container"></div>'];
            this.$el.append(tpl.join());
         
        },
            
        add: function (components) {

            components = _.isArray(components) ? components : [components];
            var $content = this.$el.find('> .eng-container');
            _.each(components, function (item) {
                // check if the component already exist the skip
                this.items.push(item);
                $content.append(item.render(item.model).$el);
            }, this);


            return this;
        },
        clear: function() {
            this.$el.find('> .eng-container').html('');
        }

    });
})