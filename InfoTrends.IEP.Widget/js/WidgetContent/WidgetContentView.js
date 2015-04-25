define([
    'text!js/WidgetContent/WidgetContent.css',
    'text!js/WidgetContent/WidgetContent.html',
    'js/Container',
    'js/MessageBox/MessageBoxView',
    'js/WidgetSetting/WidgetSettingView'

], function (Css, Template, Container, MessageBox, WidgetSettingView) {
    return Container.extend({
        
        events: {
            "click .eng-close-widget-content": "removeWidgetContent",
            "click .eng-save-widget": "saveWidgetContent",
            "click .eng-settting-widget": "settingWidget"
        },
        constructor: function (options) {
            var opts = ENG.$.extend(true, {
                id: null,
                itemChilds: null,
                model: null,
                nameWidget: null,
                position: null,
                isChangePosition: true
            }, options);

            Container.prototype.constructor.call(this, opts);
            this.$el.addClass('eng-widget-content');
            var urlCss = ENG.DOMAIN + "/js/WidgetContent/WidgetContent.css";
            ENG.loadCss(urlCss);
        },
        setBackground: function (color) {
        },
        render: function (model) {
            Container.prototype.render.call(this);
            var compile = _.template(this.template());
            html = compile({ nameWidget: this.opts.model.get("WidgetTypeName"), id: this.opts.id });
            this.$el.html(html);
            this.initPosition();
            this.initDragDrop();
            this.renderItem();
            return this;
        },
        template: function () {
            return Template;
        },
        initDragDrop: function () {
            var me = this;
            console.log(this.opts.model.get("ID"));
            if (!this.opts.model.get("ID")) {
                console.log("zo zo");
                this.$el.draggable({
                    scroll: true,
                    cursor: "move",
                    handle: ".eng-panel-heading",
                    cursorAt: { left: -5, top: 0 },
                    appendTo: "body",
                    start: function (param) {
                        ENG.dragWidget = true;
                        ENG.typeWidget = ENG.enum.dragDropWidget.WIDGETCONTENT;
                        ENG.dragInClient = me;
                        
                    },
                    stop: function () {
                        ENG.dragWidget = false;
                        me.isChangePosition = false;
                    }
                });
            }
        },
        initPosition: function () {
            this.$el.css({
                'top': 'auto',
                'left': 'auto'
            });
        },
        removeWidgetContent: function (e) {
            var messageBox = new MessageBox();
            messageBox.opts.params = {
                headerText: "Widget Confirm",
                messageText: "Are you sure to remove Widget?"
            };
            messageBox.opts.type = "yes-no";
            messageBox.opts.size = "small";

            messageBox.showMessageBox();
            this.listenTo(messageBox, "ok", function (data) {
                this.remove();
            });
        },
        saveWidgetContent: function (e) {
            var me = this;
            var listContentId = [];
            //listContentId.push(this.opts.model.get("Data")[0].ID);
            if (this.opts.model.get("WidgetTypeName") === ENG.enum.typeWidget.CONTENT) {
                
                _.each(this.opts.model.get("Data"), function (item) {
                    if (listContentId.indexOf(item.WidgetDataTypeID) === -1) {
                        listContentId.push(item.WidgetDataTypeID);
                    }
                });

            }
            Xdr.ajax({
                url: ENG.ApiDomain + '/umbraco/api/WidgetContent/AddWidget',
                data: {
                    URL: window.location.href,
                    Position: me.opts.position,
                    WidgetTypeName: me.opts.model.get("WidgetTypeName"),
                    Color: me.opts.model.get("Color"),
                    Width: parseInt("0"),
                    Height: parseInt("0"),
                    Name: me.opts.model.get("Name"),
                    clientId: ENG.cid,
                    WidgetDataTypeId: { '': listContentId }
                },
                type: "POST"
            }, function(response) {
                if (response.success) {
                    if (response.data.id) {
                        me.opts.id = response.data.id;
                    }
                    var messageBox = new MessageBox();
                    messageBox.opts.params = {
                        headerText: "Infomation",
                        messageText: "Save widget is successful"
                    };
                    messageBox.opts.size = "medium";
                    messageBox.showMessageBox();
                    me.render();
                }
            });
        },
        renderItem: function () {
            this.add(this.opts.itemChilds);
        },
        settingWidget: function () {
            var widgetContentView = new WidgetSettingView();
            var body = ENG.$("body");
            body.append(widgetContentView.render().$el);
        }
    });
});