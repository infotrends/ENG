var ENG = {

    $: null,


    DOMAIN: "//localhost:48967/",
    ApiDomain: "//10.16.56.135",
    buildNum: (new Date().getTime()), //local loading
    SESSION: null,

    CUSTOMERID: null,

    trackingSetting: null,

    init: function (customerId) {
        // Set for global
        this.setVersion(0); //18
        this.CUSTOMERID = customerId;
        this.listCss = [];
        ENG.isRightLeftPanel = false;
        // Load frameworks
        this.loadFrameworks(function () {
            // something more but not likely because you are going to put code into app.js
        }, this);


    },
    loadFrameworks: function (cb, scope) {

        var me = this;

        var domain = this.DOMAIN;
        var rjs = domain + "/lib/require.js";
        var tjs = domain + "/lib/text.js";
        var jjs = domain + "/lib/jquery.js";
        var ujs = domain + "/lib/underscore.js";
        var bjs = domain + "/lib/backbone.js";

        var arr = [
            domain + "/xdr/xdr.js",
            domain + "/js/utils.js",
            domain + "/js/app.js",
            domain + "/lib/isoCountries.js"
        ];

        var css = [
            domain + "/css/InitStyle.css",
            domain + "/css/main.css",
            domain + "/css/fonts/font-awesome.min.css",
            domain + "/js/NetworkAnalytic/NetworkAnalytic.css",
            domain + "/js/Widget/Widget.css"
        ];

        //        //Load global css
        for (var i = 0; i < css.length; i++) {
            me.loadCss(css[i]);
        }

        me.loadScript(rjs, function () {

            // config require js
            require.config({
                baseUrl: domain,
                urlArgs: "bust=" + ENG.buildNum,
                waitSeconds: 15,

                paths: {

                    text: domain + "/lib/text"
                },

                config: {
                    text: {
                        useXhr: function (url, protocol, hostname, port) {
                            // allow cross-domain requests
                            // remote server allows CORS
                            return true;
                        }
                    }
                }


            });





            me.loadScript(jjs, function () {
                if (typeof _ === 'undefined') {
                    require([ujs], function () {
                        if (typeof Backbone === 'undefined') {
                            require([bjs], function (Backbone) {
                                me.loadNonAmdLibs(arr, 0, cb, scope);
                            });  // eof: Load Backbone
                        }
                        else {
                            me.loadNonAmdLibs(arr, 0, cb, scope);
                        }
                    }); // eof: Load underscores
                }
                else {
                    if (typeof Backbone === 'undefined') {
                        require([bjs], function (Backbone) {
                            me.loadNonAmdLibs(arr, 0, cb, scope);
                        });  // eof: Load Backbone
                    }
                    else {
                        me.loadNonAmdLibs(arr, 0, cb, scope);
                    }
                }


            }, this); // eof: Load Jquery.






        }, this); // eof: Load RequireJs


    },


    loadNonAmdLibs: function (arr, pos, cb, scope) {
        var domain = ENG.DOMAIN;


        if ($.plot) {

            if (typeof freewall !== "undefined") {
                ENG.$ = jQuery.noConflict(true); // Set for self-use
                ENG.loadScripts(arr, 0, cb, scope);
            }

            else {
                ENG.loadScript(domain + '/lib/freewall.js', function () {

                    ENG.$ = jQuery.noConflict(true); // Set for self-use
                    ENG.loadScripts(arr, 0, cb, scope);

                }, this);
            }

        }

        else {
            ENG.loadScript(domain + '/lib/jquery.flot.min.js', function () {

                ENG.loadScript(domain + '/lib/jquery.flot.time.min.js', function () {
                    ENG.loadScript(domain + '/lib/jquery.flot.canvas.min.js', function () {
                        ENG.loadScript(domain + '/lib/jquery.flot.pie.min.js', function () {
                            ENG.loadScript(domain + '/lib/jquery.flot.categories.min.js', function () {
                                ENG.loadScript(domain + '/lib/jquery.slimscroll.js', function () {
                                    if (typeof freewall !== "undefined") {
                                        ENG.$ = jQuery.noConflict(true); // Set for self-use
                                        ENG.loadScripts(arr, 0, cb, scope);
                                    }else {
                                        ENG.loadScript(domain + '/lib/freewall.js', function () {

                                            ENG.$ = jQuery.noConflict(true); // Set for self-use
                                            ENG.loadScripts(arr, 0, cb, scope);

                                        }, this);
                                    }
                                }, this);
                            }, this);
                        }, this);
                    }, this);
                }, this);

            }, this);
        }
    },


    loadScripts: function (arr, pos, cb, scope) {

        // Base case
        if (pos >= arr.length) {
            cb.call(scope);
            return;
        }

        var url = arr[pos];

        this.loadScript(url, function () {
            this.loadScripts(arr, pos + 1, cb, scope);
        }, this);

    },
    loadScript: function (url, callback, scope) {
        /* Load script from url and calls callback once it's loaded */
        if (url.indexOf('?') == -1) url += "?";
        url += "&_dc=" + ENG.buildNum;

        var scriptTag = document.createElement('script');
        scriptTag.setAttribute("type", "text/javascript");
        scriptTag.setAttribute("src", url);

        if (typeof callback !== "undefined") {
            if (scriptTag.readyState) {
                /* For old versions of IE */
                scriptTag.onreadystatechange = function () {
                    if (this.readyState === 'complete' || this.readyState === 'loaded') {
                        callback.call(scope);
                    }
                };
            } else {
                scriptTag.onload = function () {
                    callback.call(scope);
                };
            }
        }
        (document.getElementsByTagName("head")[0] || document.documentElement).appendChild(scriptTag);
    },
    addCssToDom: function (cssText) {
        var styleTag = document.createElement('style');
        styleTag.innerHTML = cssText;
        (document.getElementsByTagName("head")[0] || document.documentElement).appendChild(styleTag);
    },
    loadCss: function (url, callback, scope) {
        if (this.listCss.indexOf(url) < 0) {
            this.listCss.push(url);
        } else {
            if (callback)
                callback.call(scope);
            return;
        }
        /* Load script from url and calls callback once it's loaded */
        if (url.indexOf('?') == -1) url += "?";
        url += "&_dc=" + ENG.buildNum;

        var scriptTag = document.createElement('link');
        scriptTag.setAttribute("rel", "stylesheet");
        scriptTag.setAttribute("href", url);

        if (typeof callback !== "undefined") {
            if (scriptTag.readyState) {
                /* For old versions of IE */
                scriptTag.onreadystatechange = function () {
                    if (this.readyState === 'complete' || this.readyState === 'loaded') {
                        callback.call(scope);
                    }
                };
            } else {
                scriptTag.onload = function () {
                    callback.call(scope);
                };
            }
        }

        (document.getElementsByTagName("head")[0] || document.documentElement).appendChild(scriptTag);
    },

    loadJSON: function (json) {
        var jsonData = ENG.$.parseJSON(json);

        var columnHeader = [];
        var data = [];
        ENG.$.each(jsonData, function (k, v) {
            var isFirstTime = true;
            _.each(v, function (item) {

                if (isFirstTime) {
                    //columnHeader.push([item.key, item.value]);
                    ENG.$.each(item, function (key, value) {
                        columnHeader.push({
                            name: key,
                            isShow: true
                        });
                    });

                    isFirstTime = false;
                }

                var item1 = {
                    keyTitle: columnHeader[0].name,
                    valueTitle: columnHeader[1].name,
                    key: item[columnHeader[0].name],
                    value: item[columnHeader[1].name],
                }
                var extra = [];

                for (var i = 2; i < columnHeader.length; i++) {
                    extra.push({
                        dataTitle: columnHeader[i].name,
                        value: item[columnHeader[i].name]
                    });
                }

                item1.extraProperties = extra;

                data.push(item1);
            });

        });

        return retData = {
            columnHeader: columnHeader,
            data: data
        };
    },

    setVersion: function (ver) {

        if (ver == 0) { //local

            ENG.DOMAIN = "//localhost:48967";
            ENG.ApiDomain = "//10.16.56.135";
            ENG.buildNum = (new Date().getTime());
        }



        else { //production
            ENG.DOMAIN = "//engagementdev.infotrends.com";
            ENG.ApiDomain = "//engagementdev.infotrends.com:82";
            ENG.buildNum = ver;

        }

    },

    loadPageViewJson: function (url, columnHeader, handleData) {
        console.log(url);

        Xdr.ajax({
            url: url
        },
        function (success, response) {
            var retData = null;
            if (success && success.success) {
                retData = [];
                if (ENG.Utils.checkObjExist(success, 'data.data.facet_counts.facet_fields')) {

                    var jsonData = "";
                    _.each(success.data.data.facet_counts.facet_fields, function (obj) {
                        jsonData = obj;
                    });

                    for (var i = 0; i < jsonData.length; i += 2) {
                        retData.push({
                            keyTitle: columnHeader[0].name,
                            valueTitle: columnHeader[1].name,
                            key: jsonData[i],
                            value: jsonData[i + 1],
                        });
                    }
                }
            }

            handleData(retData);

        }, this);

    },
    loadPageViewByDateJson: function (startDate, endDate, handleData) {
        var me = this;
        var url = _.template("<%=domain%>/umbraco/api/EngQuery/GetPageviewByDate?clientId=<%=cliectId%>&startDate=<%=startDate%>&endDate=<%=endDate%>");

        var apiUrl = url({
            domain: ENG.ApiDomain,
            cliectId: ENG.cid,
            startDate: ENG.getDateStringYYYYMMDD(startDate),
            endDate: ENG.getDateStringYYYYMMDD(endDate),
        });

        console.log(apiUrl);

        Xdr.ajax({
            url: apiUrl
        },
        function (success, response) {

            if (success && success.success) {

                var pageViewsData = [];

                if (ENG.Utils.checkObjExist(success, 'data.data.facet_counts.facet_dates.CreateOn_dt')) {

                    var tempData = success.data.data.facet_counts.facet_dates.CreateOn_dt;

                    var pageViewsData = [];
                    ENG.$.each(tempData, function (date, pageView) {
                        var dateValue = ENG.getDateValue(date);
                        if (dateValue != "Invalid Date") {
                            pageViewsData.push([dateValue, pageView]);
                        }
                    });
                }
                handleData(pageViewsData);

            }


        }, this);

    },


    Api: {
        request: function (params, callback, scope, contentType) {
            var method = params.method;
            if (method == null) method = 'GET';

            var ctrl = params.ctrl;
            if (ctrl == null) throw "Invalid Ctrl";

            var url = ENG.ApiDomain + ctrl;

            delete params.method;
            delete params.ctrl;

            var cfg = {
                url: url,
                type: method.toUpperCase(),
                data: params,
                headers: {}
            };


            if (contentType == 'json') {
                cfg.data = JSON.stringify(params);
                cfg.headers["Content-Type"] = "application/json";
            }


            // Send
            Xdr.ajax(cfg, function (result) {

                
                console.log("result is: " + result);
                console.log("is callback: " + ENG.$.isFunction(callback));

                if (ENG.$.isFunction(callback)) {

                    console.log("in callback");

                    callback.call(scope, result.success, result.data);
                }
            }, this);

        },
        Auth: {
            Profile: {
                getCurrentSession: function (callback, scope, contentType) {
                    var params = {
                        method: 'GET',
                        ctrl: '/umbraco/Auth/Profile/GetCurrentSession'
                    };
                    ENG.Api.request(params, callback, scope, contentType);
                },
                login: function (username, password, callback, scope, contentType) {
                    var params = {
                        method: 'POST',
                        ctrl: '/umbraco/Auth/Profile/Login',
                        username: username,
                        password: password
                    };
                    ENG.Api.request(params, callback, scope, contentType);
                },
                logout: function (callback, scope, contentType) {
                    var params = {
                        method: 'GET',
                        ctrl: '/umbraco/Auth/Profile/Logout'
                    };
                    ENG.Api.request(params, callback, scope, contentType);
                }
            }
        }
    },


    getDateString: function (date) {
        var monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
        return monthNames[date.getMonth()] + " " + date.getDate();
    },
    getDateStringMMDDYYYY: function (date) {
        return (date.getMonth() + 1) + "/" + date.getDate() + "/" + date.getFullYear();
    },
    getDateStringYYYYMMDD: function (date) {
        return date.getFullYear() + "/" + (date.getMonth() + 1) + "/" + date.getDate();
    },
    getDateValue: function (strDate) {
        return new Date(Date.parse(strDate));
    },
    loadtrackingSetting: function (callBack) {

        var me = this;
        var url = _.template("<%=domain%>/umbraco/api/EngUserSetting/Read?clientId=<%=cliectId%>");

        var apiUrl = url({
            domain: ENG.ApiDomain,
            cliectId: ENG.cid,
        });

        Xdr.ajax({
            url: apiUrl
        }, function (success, response) {
            if (success && success.success) {
                if (success.data != null) {
                    ENG.trackingSetting = {
                        MouseMoveTracking: success.data.MouseMoveTracking,
                        MouseClickTracking: success.data.MouseClickTracking,
                        PageViewsCounter: success.data.PageViewsCounter,
                        PageViewsRankingLow: success.data.PageViewsRankingLow,
                        PageViewsRankingMedium: success.data.PageViewsRankingMedium,
                        PageViewsRankingHigh: success.data.PageViewsRankingHigh,
                        ClientId: ENG.cid
                    }
                }
            }
            callBack();
        });
    },
    validateEmail: function (email) {
        var re = /^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$/i;
        return re.test(email);
    },
    setRightMenuPosition: function () {
        ENG.$(window).resize(function () {
            if (ENG.rightPanel) {
                ENG.rightPanel.showPosition(ENG.isRightLeftPanel);
            }
            if (ENG.leftMenu) {
                ENG.leftMenu.showPosition(ENG.isRightLeftPanel);
            };

            ENG.leftMenu.loadMenuStyle();
            //ENG.leftMenu.setColapseButton();
        });
    },

};

// Start

ENG.init();
