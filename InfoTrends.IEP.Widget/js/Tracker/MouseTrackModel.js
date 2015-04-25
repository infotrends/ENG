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
            PageUrl_tsd: '',
            PageX_i: 0,
            PageY_i: 0,
            Point_i: 0,
            TargetName_tsd: '',
            TargetClassName_tsd: '',
            TargetID_tsd: '',
            WindowW_i: 0,
            WindowH_i: 0,
            ScreenW_i: 0,
            ScreenH_i: 0,
            ActionName_s: '',
        },
        url: ENG.ApiDomain + '/umbraco/api/EngTrack/CollectMouseActionInfo'
    });
});