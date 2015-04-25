define([
    'js/Model'
],
function (Model) {
    return Model.extend({
        defaults: {
            ContentId: null,
            Color: null,
            Data: null,
            Name: null,
            Type: null,
            WidgetId: null,
            Url: null,
            WidgetTypeName: null
        },
        idAttribute: "WidgetId"
    });
});