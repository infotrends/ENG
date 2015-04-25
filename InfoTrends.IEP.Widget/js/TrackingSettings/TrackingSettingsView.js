define([
    'text!js/TrackingSettings/TrackingSettings.html',
    'js/Component',
    'js/TrackingSettings/TrackingSettingsModel',
],
function (Template, Component, TrackingSettingsModel) {
    return Component.extend({
        data: null,
        events: {
            'change .setting': 'settingChange',
            'click .eng-setting-save': 'save',
            'keyup .eng-view-rank-textbox input': 'numberChange'
        },
        constructor: function (options) {
            var opts = ENG.$.extend(true, {

            }, options);

            Component.prototype.constructor.call(this, opts);

            this.$el.addClass('eng-table');
            var urlCss = ENG.DOMAIN + "/js/TrackingSettings/TrackingSettings.css";
            ENG.loadCss(urlCss);
        },

        render: function (model) {
            Component.prototype.render.call(this);

            this.$el.html("");
            var compile = _.template(this.template());
            var html = compile();
            this.$el.append(html);

            this.loadData();

            return this;

        },
        template: function () {
            return Template;
        },
        settingChange: function (e) {
            var settingType = e.target.id;
            var settingValue = e.target.value;
        },
        loadData: function () {
            this.$el.find("#eng-mouse-move-tracking").val(ENG.trackingSetting.MouseMoveTracking);
            this.$el.find("#eng-mouse-click-tracking").val(ENG.trackingSetting.MouseClickTracking);
            this.$el.find("#eng-page-views-tracking").val(ENG.trackingSetting.PageViewsCounter);

            this.$el.find("#eng-low-to").val(ENG.trackingSetting.PageViewsRankingLow);

            this.$el.find("#eng-medium-from").html(ENG.trackingSetting.PageViewsRankingLow + 1);
            this.$el.find("#eng-medium-to").html(ENG.trackingSetting.PageViewsRankingMedium - 1);

            this.$el.find("#eng-high-from").val(parseInt(ENG.trackingSetting.PageViewsRankingMedium));

            data = {
                low: ENG.trackingSetting.PageViewsRankingLow,
                med: ENG.trackingSetting.PageViewsRankingMedium
            }
        },
        save: function () {
            var model = new TrackingSettingsModel();

            var url = _.template("<%=domain%>/umbraco/api/EngUserSetting/Update");

            model.url = url({
                domain: ENG.ApiDomain,
            });

            model.attributes["MouseMoveTracking"] = this.$el.find("#eng-mouse-move-tracking").val();
            model.attributes["MouseClickTracking"] = this.$el.find("#eng-mouse-click-tracking").val();
            model.attributes["PageViewsCounter"] = this.$el.find("#eng-page-views-tracking").val();

            model.attributes["PageViewsRankingLow"] = this.$el.find("#eng-low-to").val();
            model.attributes["PageViewsRankingMedium"] = this.$el.find("#eng-high-from").val();

            model.attributes["ClientId"] = ENG.cid;

            model.engSave(function (data, msg) {
                ENG.loadtrackingSetting();
                alert(msg);
            });
        },
        numberChange: function (e) {
            if (e.target.value === "") {
                e.target.value = 0;
                this.$el.find("#eng-medium-from").html(parseInt(e.target.value) + 1);
            } else {
                if (e.target.id === "eng-low-to") {

                    if (parseInt(e.target.value) == e.target.value && parseInt(e.target.value) < data.med) {
                        this.$el.find("#eng-medium-from").html(parseInt(e.target.value) + 1);
                        data.low = parseInt(e.target.value);
                    } else {
                        this.$el.find("#eng-low-to").val(data.low);
                    }
                } else {
                    if (parseInt(e.target.value) == e.target.value && parseInt(e.target.value) > data.low) {
                        this.$el.find("#eng-medium-to").html(parseInt(e.target.value) - 1);
                        data.med = parseInt(e.target.value);
                    } else {
                        this.$el.find("#eng-high-from").val(data.med);
                    }
                }
            }

        },
    });
});