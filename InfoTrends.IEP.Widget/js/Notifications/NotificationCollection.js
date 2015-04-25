define([
    'js/Collection',
    'js/Notifications/NotificationModel'
],
function (Collection, NotificationModel) {
    return Collection.extend({
        model: NotificationModel
    });
});