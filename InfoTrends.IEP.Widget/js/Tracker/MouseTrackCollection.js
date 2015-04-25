define([
    'js/Collection',
    'js/Tracker/MouseTrackModel'
],
function (Collection, MouseTrackModel) {
    return Collection.extend({
        model: MouseTrackModel,

        url: ENG.ApiDomain + '/umbraco/api/EngTrack/CollectSetOfMouseActionInfo',
                   
        save: function (engHandleData) {
            var data = [];
            _.each(this.models, function (model) {
                data.push(model.attributes);
            });
            
            ENG.$.ajax({
                type: 'POST',
                url: this.url,
                data: {'': data},                
                async: true
            }).done(function (data) {
                 if (engHandleData) engHandleData(data, 'success');
            }).error(function (jqXHR, textStatus, errorThrown) {
                 if (engHandleData) engHandleData(errorThrown, 'error');
            });
        }
    });
});