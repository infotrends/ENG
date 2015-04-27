define([
],
function () {
    return Backbone.Model.extend({
        engSave: function (engHandleData) {
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