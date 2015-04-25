define([
    'js/Model'
],

function (myModel) {
    return myModel.extend({
        constructor: function() {
            myModel.apply(this, arguments);
        },

        defaults: {
            'name_tsd' : '',
            'email_tsd': '',
            'category_tsd': '',
            'feedback_tsd': ''
        },

        url: ENG.ApiDomain + '/umbraco/api/EngTrack/CollectFeedback',

    });
});