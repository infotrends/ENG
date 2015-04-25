define([
    'js/Model'
],
function (myModel) {
    return myModel.extend({
        constructor: function () {
            myModel.apply(this, arguments);
        },

        defaults: {
            'ClientId_s': '',
            'CreateOn_dt' : '',
            'type_tsd': '',
            'parent_tsd': '',
            'elementID_tsd': '',
            'elementName_tsd': '',
            'elementHtml_tsd': '',
            'IPAddress_s': '',
            'CountryName_s': '',
            'CountryCode_s': '',
            'City_s': '',
            'Latitude_f': '',
            'Longitude_f': '',
            'UrlReferrer_tsd': ''
        },

        url: ENG.ApiDomain + '/umbraco/api/EngTrack/CollectVisitorLog',
    });
});