define([
    'js/Model'
],
function (Model) {
    return Model.extend({
        defaults: {
            title: null,
            favicon: null,
            childs: null
}
    });
});