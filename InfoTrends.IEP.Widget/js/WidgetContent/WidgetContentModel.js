define([
    'js/Model'
],
function (Model) {
    return Model.extend({
        defaults: {
            data: null
},
        idAttribute: "ID"
    });
});