define([
    'text!js/HiddenItem/HiddenItem.html',
    'js/Component',
    'js/MessageBox/MessageBoxView',
    'text!js/HiddenItem/ShowHideMessageBox.html',
    'js/HiddenItem/HiddenItemModel'

], function (Template, Component, MessageBoxView, TemplateMessageBox, HiddenItemModel) {
    console.log(TemplateMessageBox);
    return Component.extend({

        events: {
            "click .eng-open-hidden-item": "showMenuHidden",
            "click .eng-close": "closePopup",
            "click .eng-hidden-toggle": "addItemHidden" ,
            "click .eng-toogle-showAll": "changeToggleShowAll"
        },
        constructor: function (options) {
            var opts = ENG.$.extend(true, {
                id: null
            }, options);
            Component.prototype.constructor.call(this, opts);
            this.$el.addClass('eng-hidden-item');
            var urlCss = ENG.DOMAIN + "/js/HiddenItem/HiddenItem.css";
            ENG.loadCss(urlCss);
        },
        render: function (params) {
            Component.prototype.render.call(this);
            var compile = _.template(this.template());
            var html = compile();
            this.$el.append(html);
            /*this.$el.find(".eng-hidden-item").draggable();*/
            return this;
        },
        template: function () {
            return Template;
        },
        showMenuHidden: function (e) {
            var currentTarget = ENG.$(e.currentTarget);
            currentTarget.hide();
            var options = this.$el.find(".eng-option-hidden");
            ENG.hiddenWidget.$el.find(".eng-option-hidden").draggable();
            options.show();
        },
        closePopup: function (e) {
            this.$el.find(".eng-option-hidden").hide();
            this.$el.find(".eng-open-hidden-item").show();
            var itemAdd = this.$el.find(".eng-hidden-toggle");
            if (itemAdd.hasClass("eng-active-hidden")) {
                itemAdd.removeClass("eng-active-hidden");
                ENG.hightLightItem = false;
                ENG.hiddenMode = false;
            }
        },
        addItemHidden: function (e) {
            var itemAdd = ENG.$(e.currentTarget);
            if (itemAdd.hasClass("eng-active-hidden")) {
                itemAdd.removeClass("eng-active-hidden");
                ENG.hightLightItem = false;
                ENG.hiddenMode = false;
            } else {
                itemAdd.addClass("eng-active-hidden");
                ENG.hightLightItem = true;
                ENG.hiddenMode = true;
            }
        },
        addToolbarHidden: function () {
            var currentItem = ENG.$(ENG.clickItem);
            var messageBox = new MessageBoxView();
            messageBox.opts.params = {
                headerText: "Notice",
                messageText: ""
            };
            messageBox.opts.type = "yes-no";
            messageBox.showMessageBox();
            messageBox.addCustomContent(TemplateMessageBox);
            this.listenTo(messageBox, "ok", function (data) {
                var value = messageBox.$el.find("input[type='radio']:checked").val();
                var divParent;
                var address = "";
                if (currentItem.is("div")) {
                    divParent = currentItem;
                } else {
                    divParent = ENG.$(currentItem).parent().closest('div');
                }
                var position = ENG.Utils.getAddressElement(address, divParent);
                switch (value) {
                    case 'show':
                        currentItem.show();
                        break;
                    case 'hideShrink':
                        currentItem.hide();
                        var model = new HiddenItemModel();
                        model.set("Position", position);
                        model.set("URL", window.location.href);
                        model.set("Type", "hideShrink");
                        ENG.lstHiddenItem.push(model);
                        break;
                    case 'hideKeep':
                        debugger;
                        currentItem.css("visibility", "hidden");
                        var model = new HiddenItemModel();
                        model.set("Position", position);
                        model.set("URL", window.location.href);
                        model.set("Type", "hideKeep");
                        ENG.lstHiddenItem.push(model);
                        break;
                    default:
                }
            });
        },
        changeToggleShowAll: function (e) {
            var buttonShowAll = ENG.$(e.currentTarget);
            var span = buttonShowAll.parent().find("span");
            if (buttonShowAll.hasClass("fa-toggle-off")) {
                buttonShowAll.removeClass("fa-toggle-off");
                buttonShowAll.addClass("fa-toggle-on");
                span.text("Show all");
                this.showHideAllItemHidden(true);
            } else {
                buttonShowAll.removeClass("fa-toggle-on");
                buttonShowAll.addClass("fa-toggle-off");
                span.text("Hidden all");
                this.showHideAllItemHidden(false);
            }
        },
        showHideAllItemHidden: function (isShow) {
            debugger;
            if (ENG.lstHiddenItem.length > 0) {
                _.each(ENG.lstHiddenItem, function (item) {
                    var postionWidget = item.get('Position');
                    var itemArray = postionWidget.split("-");
                    var parentWidget;
                    var positionToInsert;
                    for (var i = itemArray.length; i--; i > 0) {
                        var node = itemArray[i].split(".");
                        var tagName = node[0];
                        var index = node[1];
                        if (i === 0) {
                            positionToInsert = index;
                            switch (item.get("Type")) {
                                case "hideShrink":
                                    var item = parentWidget.children().eq(index);
                                    if (isShow) {
                                        item.show();
                                        item.css("opacity", "0.8");
                                    } else {
                                        item.hide();
                                    }
                                break;
                                case "hideKeep":
                                    var item = parentWidget.children().eq(index);
                                    if (isShow) {
                                        item.css("visibility", "visible");
                                        item.css("opacity", "0.8");
                                    } else {
                                        item.css("visibility", "hidden");
                                        item.css("opacity", "0.8");
                                    }
                                break;
                            }
                        }
                        if (parentWidget) {
                            parentWidget = ENG.$(parentWidget.children()[index]);
                        } else {
                            parentWidget = ENG.$(ENG.$(tagName).children()[index]);
                        }
                    }
                });
            }
        }
    });
});
