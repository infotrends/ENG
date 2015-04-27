define([
    'js/Model'
],
function (myModel) {
    return myModel.extend({
        constructor: function () {
            myModel.apply(this, arguments);
        },
        defaults: {
            ClientId: '123456789',
            Width: 0,
            Height: 0,
            PageUrl: '',
            Referrer: '',
            ViewerId: '',
            SessionId: '',
        },
        url: ENG.ApiDomain + '/umbraco/api/EngTrack/CollectClientInfo'
    });
});