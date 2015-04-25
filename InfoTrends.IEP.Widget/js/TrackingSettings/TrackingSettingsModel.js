define([
    'js/Model'
],
function (Model) {
    return Model.extend({
        defaults: {
            MouseMoveTracking: null,
            MouseClickTracking: null,
            PageViewsCounter: null,
            PageViewsRankingLow: null,
            PageViewsRankingMedium: null,
            PageViewsRankingHigh: null,
            ClientId : null
        }
    });
});