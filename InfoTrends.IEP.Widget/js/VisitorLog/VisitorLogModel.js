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
            CreateOn_dt: '',
            type_tsd: '',
            Parent_tsd: '',
            ElementID_tsd: '',
            ElementName_tsd: '',
            ElementHtml_tsd: '',
            IPAddress_s: '',
            CountryName_s: '',
            CountryCode_s: '',
            City_s: '',
            Latitude_f: '',
            Longitude_f: '',
            UrlReferrer_tsd: '',
            ViewerID_s: '',
            SessionID_s: ''
        },

        url: ENG.ApiDomain + '/umbraco/api/EngTrack/CollectVisitorLog',
    });
});