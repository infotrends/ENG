define([
    'js/Collection',
    'js/Feedback/FeedbackModel'
],
function (Collection, FeedbackModel) {
    return Collection.extend({
        model: FeedbackModel
    });
});