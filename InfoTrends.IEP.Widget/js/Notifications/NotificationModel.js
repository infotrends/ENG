define([
    'js/Model'
],
function (Model) {
    return Model.extend({
        defaults: {
            title: null,
            message: null,
            id: null,
            time: null,
            seen: null,
            clientId: null
},
        idAttribute: "id"
    });
});