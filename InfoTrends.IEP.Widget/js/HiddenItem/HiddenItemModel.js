define([
    'js/Model'
],
function (Model) {
    return Model.extend({
        defaults: {
            Position: null,
            Id: "",
            URL: "",
            Type: ""
}
    });
});