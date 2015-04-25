define([
],
function () {
    return Backbone.Model.extend({
        engSave: function (engHandleData) {
            //ENG.$.ajax({
            //    type: 'POST',
            //    url: this.url,
            //    data: this.attributes,
            //    async: true
            //}).done(function (data) {
            //    if (engHandleData) engHandleData(data, 'success');
            //}).error(function (jqXHR, textStatus, errorThrown) {
            //    if (engHandleData)  engHandleData(errorThrown, 'error');
            //});
            // Change to use Xdr
            Xdr.ajax({
                //config
                type: 'POST',
                url: this.url,
                data: this.attributes,
                async: true
            }, function (response) {
                // callback
                if (response && response.success) {
                    if (engHandleData) engHandleData(response.data, 'success');
                } else {
                    if (engHandleData) engHandleData(response.data, 'error');
                }
            });

        }
    });
});