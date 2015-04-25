define([
    'js/Model'
],
function (Model) {
    return Model.extend({
        defaults: {
            title: null,
            data: null
        }
    });
});