define([
    'js/Collection',
    'js/VisitorLog/VisitorLogModel'
],
function (Collection, VisitorLogModel) {
    return Collection.extend({
        model: VisitorLogModel
    });
});