define([
    'text!js/VisitorLog/VisitorLog.html',
    'js/Component',
    'js/Table/TableView',
    'js/VisitorLog/VisitorLogCollection',
    'js/VisitorLog/VisitorLogModel',
    'js/DropdownDatepicker/DropdownDatepickerView',
],
function (Template, Component, TableView, VisitorLogCollection, VisitorLogModel, DropdownDatepickerView) {
    return Component.extend({
        events: {
            'click .eng-visitlog-previous': 'previousPage',
            'click .eng-visitlog-next': 'nextPage',
        },
        data: null,
        filterData: null,
        constructor: function (options) {
            var opts = ENG.$.extend(true, {
            }, options);


            Component.prototype.constructor.call(this, opts);
            this.$el.addClass('eng-visitor-log');
            this.$el.html('');

            var urlCss = ENG.DOMAIN + "/js/VisitorLog/VisitorLog.css";
            ENG.loadCss(urlCss);
        },

        render: function () {
            Component.prototype.render.call(this);
            
            this.initDateValue();

            this.initPageData();

            this.calculateData();

            return this;
        },
        template: function () {
            return Template;
        },
        initPageData: function () {
            this.filterData = {
                pageCount: 0,
                pageSize: 10,
                currentPage: 1
            };



        },
        calculateData: function () {
            this.loadData();
        },

        loadData: function (callBack) {
            var me = this;
            ENG.logCollection.fetch({
                url: ENG.ApiDomain + '/umbraco/api/EngQuery/GetVisitorLog?clientId=' + ENG.cid + '&pageNumber=' + this.filterData.currentPage + '&startDate=' + ENG.getDateStringYYYYMMDD(me.data.startDate) + '&endDate=' + ENG.getDateStringYYYYMMDD(me.data.endDate),
                success: function (model, data) {
                    if (ENG.logCollection.length !== 0) {
                        ENG.logCollection.reset();
                        if (ENG.Utils.checkObjExist(data, 'data.response.docs')) {
                            var items = data.data.response.docs;

                            if (me.filterData.pageCount == 0) {
                                var itemsCount = data.data.response.numFound;
                                me.filterData.pageCount = Math.ceil(itemsCount / me.filterData.pageSize);
                            }
                            items = _.isArray(items) ? items : [items];
                            _.each(items, function (item) {
                                if (item) {
                                    var logModel = new VisitorLogModel();
                                    logModel.set("type_tsd", item.type_tsd);
                                    logModel.set("CreateOn_dt", new Date(item.CreateOn_dt).toDateString() + " - " + new Date(item.CreateOn_dt).toLocaleTimeString());
                                    if (item.parent_tsd) {
                                        logModel.set("parent_tsd", "in " + item.parent_tsd);
                                    } else {
                                        logModel.set("parent_tsd", "");
                                    }

                                    logModel.set("elementID_tsd", item.elementID_tsd);
                                    logModel.set("elementName_tsd", item.elementName_tsd);
                                    logModel.set("elementHtml_tsd", item.elementHtml_tsd);
                                    logModel.set("IPAddress_s", item.IPAddress_s);
                                    logModel.set("CountryName_s", item.CountryName_s);
                                    logModel.set("CountryCode_s", item.CountryCode_s);
                                    logModel.set("City_s", item.City_s);
                                    logModel.set("Latitude_f", item.Latitude_f);
                                    logModel.set("Longitude_f", item.Longitude_f);
                                    logModel.set("UrlReferrer_tsd", item.UrlReferrer_tsd);

                                    ENG.logCollection.push(logModel);
                                }

                            });
                        }
                        
                    }

                    me.$el.html("");
                    var compile = _.template(me.template());
                    var html = compile({ logCollection: ENG.logCollection.models, index: (me.filterData.currentPage - 1) * me.filterData.pageSize, totalRecord: data.data.response.numFound });
                    me.$el.append(html);
                    if (me.filterData.currentPage == 1) {
                        me.$el.find('.eng-visitlog-previous').hide();
                    }
                    if (me.filterData.currentPage == me.filterData.pageCount) {
                        me.$el.find('.eng-visitlog-next').hide();
                    }
                    if (me.filterData.pageCount == 1 || me.filterData.pageCount == 0) {
                        me.$el.find('.eng-visitlog-previous').hide();
                        me.$el.find('.eng-visitlog-next').hide();
                    }

                    me.initDatepicker();
                }
            });
        },
        previousPage: function (e) {
            if (this.filterData.currentPage != 1) {
                this.filterData.currentPage--;
            }
            this.calculateData();
        },

        nextPage: function (e) {
            if (this.filterData.currentPage != this.filterData.pageCount) {
                this.filterData.currentPage++;
            } 
            this.calculateData();

        },

        initDateValue: function () {
            var me = this;

            var defaultEndDate = new Date();
            var defaultStartDate = new Date(defaultEndDate.getFullYear(), defaultEndDate.getMonth(), defaultEndDate.getDate() - 7);

            me.data = {
                startDate: defaultStartDate,
                endDate: new Date(defaultEndDate.getFullYear(), defaultEndDate.getMonth(), defaultEndDate.getDate() + 1),
                isNew: true,
                selectedValue: 7,
                selectedText: "Last 7 days",
                data: null,
                dropdownDatepicker: null
            };
        },

        initDatepicker: function () {
            var me = this;
            me.data.dropdownDatepicker = new DropdownDatepickerView();
            me.data.dropdownDatepicker.storeObject = me.data;

            //add event listener
            this.listenTo(me.data.dropdownDatepicker, "dateChange", function (obj) {
                me.data.startDate = obj.startDate;
                me.data.endDate = obj.endDate;

                me.data.selectedValue = obj.selectedValue;
                me.data.selectedText = obj.selectedText;
                if (obj.selectedValue == -1) {
                    me.data.selectedText = ENG.getDateStringMMDDYYYY(obj.startDate) + " - " + ENG.getDateStringMMDDYYYY(obj.endDate);
                }

                me.filterData.currentPage = 1
                me.filterData.pageCount = 0;

                me.calculateData();
            });
            this.$el.find('.eng-visitorLog-datePicker').append(me.data.dropdownDatepicker.render().$el);
        }
    });
});


