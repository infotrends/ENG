define([
    'text!js/ListFeedback/ListFeedback.html',
    'js/Feedback/FeedbackModel',
    'js/Component'
],
function (Template, FeedbackModel, Component) {
    return Component.extend({
        events: {
            "click #eng-listFeedbackBlur": "hideList",
            "click .eng-listFeedback-close": "hideList"
        },

        constructor: function (options) {

            var opts = ENG.$.extend(true, {

            }, options);


            Component.prototype.constructor.call(this, opts);
            this.$el.addClass('eng-list-feedback');
            var urlCss = ENG.DOMAIN + "/js/ListFeedback/ListFeedback.css";
            ENG.loadCss(urlCss);
        },
        render: function () {
            Component.prototype.render.call(this);
            var compile = _.template(this.template());
            var html = compile({ listFeedback: ENG.listFeedback.models, index: 1 });
            this.$el.append(html);
            this.$el.find("#eng_listFeedback").hide();
            this.$el.find("#eng-listFeedbackBlur").hide();



            var me = this;
            // for chaining
            return this;

        },
        template: function () {
            return Template;
        },
        hideList: function () {
            var list = this.$el.find("#eng_listFeedback");
            var listFB = ENG.$("#eng_listFeedback");
            _.each(list, function () {
                if (list) {
                    list.css("display", "none");
                    listFB.css("display", "none");
                }

            });
            this.$el.find('.eng-list-feedback').css("display", "none");
            ENG.$(".eng-list-feedback").css("display", "none");
            this.$el.find('#eng-listFeedbackBlur').css("display", "none");
            ENG.$("#eng-listFeedbackBlur").css("display", "none");
        },

        listFeedback: function () {
            var listFBComponent = ENG.$('body').find('.eng-list-feedback');
            var listFB = listFBComponent.find('#eng_listFeedback');
            if (listFB.css('display') == 'none') {
                var me = this;
                me.$el.find("#eng-listFeedback-loading").css("display", "block");

                this.loadData(function () {
                    me.$el.find("#eng-listFeedback-loading").css("display", "none");
                    ENG.$(".eng-list-feedback").css("display", "block");
                    ENG.$("#eng_listFeedback").css("display", "block");
                    ENG.$("#eng-listFeedbackBlur").css("display", "block");

                    me.$el.find("#eng_listFeedback").draggable({
                        handle: "h2",
                        cursor: "move",
                        cursorAt: { top: -350, left: 0 }
                    });
                });


            } else {
                return false;
            }
        },
        loadData: function (callBack) {
            var me = this;
            ENG.listFeedback.fetch({
                url: ENG.ApiDomain + '/umbraco/api/EngQuery/GetFeedback?clientid=' + ENG.cid,
                success: function (model, data){
                    if (ENG.listFeedback.length !== 0) {
                        ENG.listFeedback.reset();
                        if (ENG.Utils.checkObjExist(data, 'data.response.docs')) {

                            var items = data.data.response.docs;
                            items = _.isArray(items) ? items : [items];
                            _.each(items, function (item) {
                                if (item) {
                                    var listFeedbackModel = new FeedbackModel();
                                    listFeedbackModel.set("name_tsd", item.name_tsd);
                                    listFeedbackModel.set("email_tsd", item.email_tsd);
                                    listFeedbackModel.set("category_tsd", item.category_tsd);
                                    listFeedbackModel.set("feedback_tsd", ENG.Utils.htmlDecode(item.feedback_tsd));
                                    ENG.listFeedback.push(listFeedbackModel);
                                }

                            });

                        }
                    }

                    me.$el.html("");
                    var compile = _.template(me.template());
                    var html = compile({ listFeedback: ENG.listFeedback.models, index: 1 });
                    me.$el.append(html);

                    callBack();
                }
            });
        }
    });
});