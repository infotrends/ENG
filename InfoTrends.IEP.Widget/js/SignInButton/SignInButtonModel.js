define([
    'js/Model'
],
function (myModel) {
    return myModel.extend({
        constructor: function() {
            myModel.apply(this, arguments);
        },
        defaults: {
            Username: 'Guest',
            Password: '',
            DoRemember: 0,            
        },
        url: ENG.ApiDomain + '/umbraco/auth/profile/englogin'

    });
});