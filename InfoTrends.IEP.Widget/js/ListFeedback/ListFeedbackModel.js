define([
    'js/Model'
],
function (Model) {
    return Model.extend({
        defaults: {
            name: '',
            email: '',
            feedback: ''
        }
        
    });
});