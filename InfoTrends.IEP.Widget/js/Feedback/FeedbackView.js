define([
    'text!js/Feedback/Feedback.html',
    'js/Component',
    'js/Feedback/FeedbackModel'
],
function (Template, Component, FeedbackModel) {
    return Component.extend({
        events: {
            "click #btnFeedback": "showFeedback",
            "click #cancelFeedback": "cancelFeedback",
            "click #addFeedback": "addFeedback",
            "click .eng_blur": "cancelFeedback"
        },

        constructor: function (options) {
            var opts = ENG.$.extend(true, {

            }, options);


            Component.prototype.constructor.call(this, opts);

            this.$el.addClass('eng-add-feedback');
            var urlCss = ENG.DOMAIN + "/js/Feedback/Feedback.css";
            ENG.loadCss(urlCss);
        },
        render: function () {
            Component.prototype.render.call(this);
            var compile = _.template(this.template());
            var html = compile({});
            this.$el.append(html);
            this.$el.find("#eng_Feedback").hide();
            this.$el.find(".eng_blur").hide();
            //this.$el.
            //this.setVisibility();
            
            var me = this;

            // for chaining
            return this;
        },
        template: function () {
            return Template;
        },

        showFeedback: function () {
            this.$el.find("#btnFeedback").hide();
            this.$el.find("#eng_Feedback").show();
            this.$el.find(".eng_blur").show();
        },

        cancelFeedback: function () {
            var text = document.getElementById('content_TextArea');
            text.value = "";
            ENG.$('.jqte_editor').text("");
            this.$el.find("#eng_Feedback").hide();
            this.$el.find(".eng_blur").hide();
            this.$el.find("#btnFeedback").show();
        },

        addFeedback: function (e) {
            var text = document.getElementById('content_TextArea');
            var model = new FeedbackModel();
            var categoryValue = document.getElementById('eng_Feedback_Category_Select').value
            var feedbackValue = this.$('#content_TextArea').val();
            var feedbackEncode = ENG.Utils.htmlEncode(feedbackValue);
            var email = this.$("#eng-feedback-mail");
            var feedback = this.$("#content_TextArea");
            var filter = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
            if (feedbackValue !== "") {
                if (feedbackValue.length > 1000) {
                    alert('Feedback Content must be less than 1000 characters !!!');
                    feedback.focus;
                    return false;
                } else {
                    model.set("ClientId_s", ENG.cid);
                    model.set("name_tsd", sessionStorage.userName);
                    model.set("email_tsd", sessionStorage.email);
                    model.set("category_tsd", categoryValue);
                    model.set("feedback_tsd", feedbackEncode);

                    model.engSave(this.engHandleData);
                    this.$el.find("#eng_Feedback").hide();
                    this.$el.find("#btnFeedback").show();
                    text.value = "";
                    ENG.$('.jqte_editor').text("");
                    name.value = "";
                    mail.value = "";
                    alert("Thanks for feedback !");
                    this.$el.find(".eng_blur").hide();
                }

            } else {
                alert("Content field are required");
            }
        },

    });

});