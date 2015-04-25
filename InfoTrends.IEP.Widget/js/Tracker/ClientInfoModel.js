define([
    'js/Model'
],
function (myModel) {
    return myModel.extend({
        constructor: function () {
            myModel.apply(this, arguments);
        },
        defaults: {
            clientId: '123456789',
            width: 0,
            height: 0,
            pageUrl: '',
            referrer: '',
            ViewerID_s: '',
            SessionID_s: '',
        },
        url: ENG.ApiDomain + '/umbraco/api/EngTrack/CollectClientInfo'
    });
});