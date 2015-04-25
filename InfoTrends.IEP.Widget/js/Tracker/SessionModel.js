define([
    'js/Model'
],
function (myModel) {
    return myModel.extend({
        constructor: function () {
            myModel.apply(this, arguments);
        },
        defaults: {
            ClientId_s: '',
            IPAddress_s: '',
            ViewerID_s: '',
            SessionID_s: ''
        },
        url: ENG.ApiDomain + '/umbraco/api/EngTrack/CollectSessionInfo'
    });
});