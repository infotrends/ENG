define([
    'js/Model'
],
function (Model) {
    return Model.extend({
        defaults: {
            title: "",
            isAddWicket: false,
            favicon:"fa"
        }
    });
});