define([
    'js/Model'
],
function (Model) {
    return Model.extend({
        defaults: {
            usersOnline: "",
            today: "",
            yesterday: "",
            month:"",
            all: "",
            mostVisited: ""
        }
    });
});