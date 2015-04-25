define([
    'text!js/AnalyticItem/AnalyticItem.html',
    'js/Component',
    'js/RightPanel/RightPanelView',
    'lib/simpleheat',
    'js/NetworkAnalytic/NetworkAnalyticView',
    'js/NetworkAnalytic/NetworkAnalyticModel'
], function (Template, Component, RightPanelView, simpleHeat, NetworkAnalyticView, NetworkAnalyticModel) {

    return Component.extend({

        events: {
            'click .analyticItem': 'showRightPanel'
        },
        constructor: function (options) {



            var opts = ENG.$.extend(true, {
            }, options);

            Component.prototype.constructor.call(this, opts);

            this.$el.addClass('eng-analyticItem');
            var urlCss = ENG.DOMAIN + "/js/AnalyticItem/AnalyticItem.css";
            ENG.loadCss(urlCss);
        },
        render: function (model) {
            Component.prototype.render.call(this);
            var compile = _.template(this.template());

            var html = compile(model.attributes);
            this.$el.append(html);
            return this;
        },
        template: function () {
            return Template;
        },
        showRightPanel: function () {


            var $body = ENG.$('body');
            var rightPanel = new RightPanelView();
            ENG.rightPanel = rightPanel;
            $body.append(rightPanel.render().$el);


            if (this.model.attributes.header == "Mouse Move Heatmap") {
                this.showMouseHeatMap("move");
            }

            if (this.model.attributes.header == "Mouse Click Heatmap") {
                this.showMouseHeatMap("click");

            }
            if (this.model.attributes.header == "Network Analytic") {
                this.displayNetworkAnalytic();
            }
        },

        showMouseHeatMap: function (action) {
            var canvas = document.getElementById("eng-mouseHeatMap");
            if (!canvas) {
                ENG.$("body").append('<canvas id="eng-mouseHeatMap"></canvas>');
                canvas = document.getElementById("eng-mouseHeatMap");
                canvas.width = screen.width;
                canvas.height = screen.height;


                window.requestAnimationFrame = window.requestAnimationFrame || window.mozRequestAnimationFrame ||
                                           window.webkitRequestAnimationFrame || window.msRequestAnimationFrame;

                var data = [];
                var heat = simpleheat('eng-mouseHeatMap').data(data).max(18),
                    frame;

                ENG.heat = heat;
                ENG.frame = frame;

                if (action == "move")
                    ENG.heat._data = ENG.Utils.sampleMoveHeat;

                else if (action == "click")
                    ENG.heat._data = ENG.Utils.sampleClickHeat;

                ENG.heat.radius(+20, +15);
                window.requestAnimationFrame(ENG.Utils.drawHeat);
                ENG.$(canvas).click(function (event) {
                    ENG.$(this).hide();

                });

            }

            else {
                ENG.$(canvas).show();
                if (action == "move")
                    ENG.heat._data = ENG.Utils.sampleMoveHeat;

                else if (action == "click")
                    ENG.heat._data = ENG.Utils.sampleClickHeat;
                window.requestAnimationFrame(ENG.Utils.drawHeat);
            }


        },
        displayNetworkAnalytic: function () {
            ////show Network Analytic
            //var networkAnalytic = new NetworkAnalyticView();
            //var networkModel = new NetworkAnalyticModel();
            //networkAnalytic.model = networkModel;
            //ENG.rightPanel.add(networkAnalytic);

            //networkAnalytic.loadInteractiveChart();
            //networkAnalytic.loadDonutChart();
            //networkAnalytic.loadMapChart();
        }

    });
});