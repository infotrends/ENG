define([
    'js/Collection',
    'js/Widget/WidgetModel'
],
function (Collection, WidgetModel) {
    return Collection.extend({
        model: WidgetModel
    });
});