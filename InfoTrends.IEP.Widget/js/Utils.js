ENG.Utils = {

    mouseMoveArr: null,
    mouseClickArr: null,
    sampleMoveHeat: [],
    sampleClickHeat: [],
    rangeTimer: null,

    drawHeat: function () {
        //console.time('draw');
        ENG.heat.draw();
        //console.timeEnd('draw');
        ENG.frame = null;
    },


    checkObjExist: function (obj, prop) {
        var parts = prop.split('.');
        for (var i = 0, l = parts.length; i < l; i++) {
            var part = parts[i];
            if (obj !== null && typeof obj === "object" && part in obj) {
                obj = obj[part];
            }
            else {
                return false;
            }
        }
        return true;

    },




    //Cookies
    //Create cookie
    createCookie: function(name, value, days){
        var expires;
        if (days) {
            var date = new Date();
            date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
            expires = "; expires=" + date.toGMTString();
        }
        else {
            expires = "";
        }
        document.cookie = name + "=" + value + expires + "; path=/";
    },
    //Get cookie by name
    getCookie: function(cookieName){
        if (document.cookie.length > 0) {
            indexStart = document.cookie.indexOf(cookieName + "=");
            if (indexStart != -1) {
                indexStart = indexStart + cookieName.length + 1;
                indexEnd = document.cookie.indexOf(";", indexStart);
                if (indexEnd == -1) {
                    indexEnd = document.cookie.length;
                }
                return unescape(document.cookie.substring(indexStart, indexEnd));
            }
        }
        return "";
    },
    //Generate guid
    GUID: function(){
        function s4() {
            return Math.floor((1 + Math.random()) * 0x10000)
              .toString(16)
              .substring(1);
        }
        return s4() + s4() + '-' + s4() + '-' + s4() + '-' +
          s4() + '-' + s4() + s4() + s4();
    },

    toggleHeatMapControl: function (flg) {

        var control = ENG.$(".eng-heatMapControl");
        if (control.length > 0) {
            control.toggle(flg);
            return;
        }

        var heatMapControl = [' <div class="eng-heatMapControl" title="Drag Me Over">',
            '<label>Radius </label><input type="range" id="eng-radius" value="40" min="10" max="50" />',
            '<br/><label>Blur </label><input type="range" id="eng-blur" value="15" min="10" max="50" />',
            '<br/><label>Max Point </label><input type="range" id="eng-datapoint" value="1000" min="200" max="2000" />',
            '<p></p>',
            '<input type="button" id="eng-closeHeatMap" value="Close Heat Map">',
            '</div>'

        ];

        ENG.$("body").append(heatMapControl.join(""));

        control = ENG.$(".eng-heatMapControl");
        if (control.length > 0) {
            control.draggable();

            ENG.$("#eng-closeHeatMap").click(function () { //hide heatmap
                ENG.$("#eng-mouseHeatMap").hide();
                ENG.$(".eng-toolbar").show();
                ENG.$(".eng-leftPanel").show().find(".eng-component").show();
                ENG.$(".eng-add-feedback").show();
                ENG.$(".eng-list-feedback").show();
                control.hide();

            });

        }


        var radius = document.getElementById('eng-radius'),
         blur = document.getElementById('eng-blur'),
         dataPoint = document.getElementById('eng-datapoint'),
    changeType = 'oninput' in radius ? 'oninput' : 'onblur';

        radius[changeType] = blur[changeType] = function (e) {
            ENG.heat.radius(+radius.value, +blur.value);
            //window.requestAnimationFrame(ENG.Utils.drawHeat());

            window.clearTimeout(ENG.rangeTimer);
            ENG.rangeTimer = window.setTimeout(function () {
            window.requestAnimationFrame(ENG.Utils.drawHeat());
            }, 500);

        };

        dataPoint[changeType] = function (e) {
            ENG.heat.max(dataPoint.value);
            //window.requestAnimationFrame(ENG.Utils.drawHeat());
            window.clearTimeout(ENG.rangeTimer);
            ENG.rangeTimer = window.setTimeout(function () {
            window.requestAnimationFrame(ENG.Utils.drawHeat());
            }, 500);
            
        };


    },

    isTrack: function (type) {
        var isTrack = false;
        switch (type) {
            case ENG.enum.trackingSetting.Enable:
                //login -> don't track
                if (sessionStorage.email && !_.isEmpty(sessionStorage.email)) {
                    isTrack = false;
                }
                else {
                    isTrack = true;
                }
                break;
            case ENG.enum.trackingSetting.Disable:
                isTrack = false;
                break;
        }
        return isTrack;
    },

    trackMouse: function () {
        ENG.$(document).mousemove(function (event) {


            ENG.currentItem = event.target;

            if (typeof ENG.oldDivMouseOver !== "undefined") {
                ENG.oldDivMouseOver.removeClass("eng-moveMouseOver");
            }

            //drag and drop hover
            if (typeof ENG.dragWidget !== "undefined" && ENG.dragWidget === true) {
                var currentItem = ENG.$(event.target);
                if (currentItem.parents(".eng-component").length > 0) {
                    return;
                }
                var divParent;
                if (currentItem.is("div")) {
                    divParent = currentItem;
                } else {
                    divParent = ENG.$(ENG.currentItem).parent().closest('div');
                }
                divParent.addClass("eng-moveMouseOver");
                ENG.oldDivMouseOver = divParent;
            }
            
            event = event || window.event;
            event = ENG.$.event.fix(event);
            
            if (!ENG.Utils.isTrack(ENG.trackingSetting.MouseMoveTracking)) {
                return;
            }
                


            var msg = "Handler for .mousemove() called at ";
            msg += event.pageX + ", " + event.pageY;
            msg += " target: " + event.target.tagName;

            var mouseMoveArr = ENG.Utils.mouseMoveArr;
            var mouseMoveObj = new ENG.Model.MouseTrackModel();

            var target = event.target || event.srcElement,
            rect = target.getBoundingClientRect(),
            offsetX = event.clientX - rect.left,
            offsetY = event.clientY - rect.top;


            
            if (offsetX != event.clientX && offsetY != event.clientY) {
            }


            //Get Scroll
            var doc = document.documentElement;
            var left = (window.pageXOffset || doc.scrollLeft) - (doc.clientLeft || 0);
            var top = (window.pageYOffset || doc.scrollTop) - (doc.clientTop || 0);
            
            var gap = 50;

            if (mouseMoveArr.length === 0) {
                mouseMoveObj.set({                    
                    PageUrl_tsd: document.URL,
                    PageX_i: ENG.Utils.getPosition(event.clientX + left, gap),
                    PageY_i: ENG.Utils.getPosition(event.clientY + top, gap),
                    Point_i: 0 // TODO currently set to default value 
                });
            }
            else {
                var prevClientX = mouseMoveArr.at(mouseMoveArr.length - 1).get("PageX_i");
                var prevClientY = mouseMoveArr.at(mouseMoveArr.length - 1).get("PageY_i");
                //Set Data
                if (ENG.Utils.isMouseMove(event.clientX + left, event.clientY + top, prevClientX, prevClientY, gap)) {
                    var myX = ENG.Utils.getPosition(event.clientX + left, gap);
                    var myY = ENG.Utils.getPosition(event.clientY + top, gap);
                    //if (existMouseObj) {
                    //    existMouseObj.set("Point_i", existMouseObj.get("Point_i") + 1);
                    //    return;
                    //}
                    //else {
                        mouseMoveObj.set({
                            PageUrl_tsd: document.URL,
                            PageX_i: myX,
                            PageY_i: myY,
                            Point_i: 0 // TODO currently set to default value 
                        });
                    //}
                }
                else
                    return;
            }           
            
            var eTarget = event.target;

            
            mouseMoveObj.set({
                ClientId_s: ENG.cid,
                TargetName_tsd: eTarget.tagName,
                TargetID_tsd: eTarget.id,
                TargetClassName_tsd: eTarget.className,
                WindowW_i: ENG.$(window).width(),
                WindowH_i: ENG.$(window).height(),
                ScreenW_i: screen.width,
                ScreenH_i: screen.height,
                ActionName_s: 'mousemove'
            });                       
            
            mouseMoveArr.push(mouseMoveObj);
        });


        ENG.$(document).click(function (event) {
            
            if (!ENG.Utils.isTrack(ENG.trackingSetting.MouseClickTracking)) {
                return;
            }
            var mouseClickArr = ENG.Utils.mouseClickArr;

            var mouseClickObj = new ENG.Model.MouseTrackModel();

            var gap = 50;

            //Set Data
            mouseClickObj.set({
                ClientId_s: ENG.cid,
                PageX_i: ENG.Utils.getPosition(event.pageX, gap),
                PageY_i: ENG.Utils.getPosition(event.pageY, gap),
                Point_i: 0 // TODO currently set to default value 
            });

            //mouseClickObj.pageX = event.pageX;
            //mouseClickObj.pageY = event.pageY;

            //ENG.Utils.sampleClickHeat.push([mouseClickObj.pageX, mouseClickObj.pageY, 1]);
            ENG.Utils.sampleClickHeat.push([mouseClickObj.get("PageX_i"), mouseClickObj.get("PageY_i"), 0.1]);

            //mouseClickObj.target = {};

            var eTarget = event.target;

            //mouseClickObj.target.tagName = eTarget.tagName;
            //mouseClickObj.target.className = eTarget.className;
            //mouseClickObj.target.id = eTarget.id;
            //mouseClickObj.type = "mouseclick";

            mouseClickObj.set({
                PageUrl_tsd: document.URL,
                TargetName_tsd: eTarget.tagName,
                TargetClassName_tsd: eTarget.className,
                TargetID_tsd: eTarget.id,
                WindowW_i: ENG.$(window).width(),
                WindowH_i: ENG.$(window).height(),
                ScreenW_i: screen.width,
                ScreenH_i: screen.height,
                ActionName_s: 'mouseclick'
            });

            mouseClickArr.push(mouseClickObj);

        });
       
        ENG.$("a").click(function (event) {
            // collect a click into VisitorLog Model

            if (!ENG.Utils.isTrack()) {
                return;
            }

            var model = new ENG.Model.VisitorLogModel();
            var parent = event.currentTarget.parentNode;
            var parentName = parent.classList.toString().substr(4);
            var parentName_convert;
            if (parentName.indexOf("-") !== -1) {
                parentName_convert = parentName.replace("-", " ");
            } else {
                parentName_convert = parentName;
            }

            if (parent && event.currentTarget.innerText) {
                model.set("ClientId_s", ENG.cid);
                model.set("type_tsd", event.type);
                model.set("parent_tsd", parentName_convert);
                model.set("elementID_tsd", event.currentTarget.id);
                model.set("elementName_tsd", event.currentTarget.innerText);
                model.set("elementHtml_tsd", event.currentTarget.innerHTML);
                model.set("UrlReferrer_tsd", document.referrer);
                         
                model.engSave(this.engHandleData);
            }

        });

    },

    isMouseMove: function (x, y, prevX, prevY, gap) {
        if (Math.floor(x / gap) === Math.floor(prevX / gap) && Math.floor(y / gap) === Math.floor(prevY / gap))
            return false;
        return true;
    },

    getPosition: function (i, gap) {
        return i - (i % gap) + Math.floor(gap / 2);
    },
    
    // extract out the parameters
    getUrlParam: function (n, s) {
        n = n.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
        var p = (new RegExp("[\\?&]" + n + "=([^&#]*)")).exec(s);
        return (p === null) ? "" : p[1];
    },

    flushData: function () {
        var mouseMoveArr = ENG.Utils.mouseMoveArr;
        var mouseClickArr = ENG.Utils.mouseClickArr;
 
        var me = this;
        
        setInterval(function () {

            if (mouseMoveArr && mouseMoveArr.length > 0) {
                mouseMoveArr.save();
            }
            
            if (mouseMoveArr && mouseClickArr.length > 0) {
                mouseClickArr.save();
            }
            mouseMoveArr.reset();
            mouseClickArr.reset();

            //Add cookie data
            var sessionModel = new ENG.Model.SessionModel();
            sessionModel.set({
                ClientId_s: ENG.cid,
                ViewerID_s: me.getCookie("ENGViewerCookie"),
                SessionID_s: me.getCookie("ENGSessionCookie")
            });

            sessionModel.engSave();

        }, 9000);
        
    },


    //loadjQueryUI: function () {

    //    if (typeof $ !== "undefined" && $.ui) {
    //        ENG.$.ui = $.ui;
    //        return;
    //    }

    //    require([ENG.DOMAIN + '/lib/jquery-ui.min.js'], function () {
    //        ENG.loadCss(ENG.DOMAIN + "/lib/jquery-ui.css");

    //    });  // eof
    //},
    
    loadPace: function () {

        
        if (typeof Pace !== "undefined") {
            return;
        }

        require([ENG.DOMAIN + '/lib/pace.min.js'], function (pace) {
            pace.start();
        });  // eof
    },

    loadTextEditor: function () {

        if (typeof $ !== "undefined" && $.fn.jqte) {
            return;
        }

        require([ENG.DOMAIN + '/lib/jquery-te-1.4.0.min.js'], function () {
            ENG.loadCss(ENG.DOMAIN + "/lib/jquery-te-1.4.0.css");
            initTextEditor(ENG.$);
            

        });  // eof
    },

    loadMapLib: function () {

        if (typeof $ !== "undefined" && $.fn.mapael) {
            return;
        }

        require([ENG.DOMAIN + "/lib/raphael-min.js"], function () {


            ENG.loadScript(ENG.DOMAIN + "/lib/jquery.mapael.js", function () {
                initMap(ENG.$);


                ENG.loadScript(ENG.DOMAIN + "/lib/world_countries.js", function () {
                    initWorldMap(ENG.$);


                }, this); // eof


            }, this); // eof


        });  // eof




    },



    detechTechonology: function () {
        var techonologys = [];
        //Check jquery framework
        if (typeof jQuery !== "undefined") {
            var tech = {
                type: "JQuery",
                version: jQuery.fn.jquery
            };
            techonologys.push(tech);
        }
        //Check angular framework
        if (typeof angular !== "undefined") {
            var tech = {
                type: "Angular",
                version: angular.version.full
            };
            techonologys.push(tech);
        }
//        //Check Backbone framework
//        if (typeof Backbone !== "undefined") {
//            var tech = {
//                type: "Backbone",
//                version: Backbone.VERSION
//            };
//            techonologys.push(tech);
//        }
//        //Check Underscore framework
//        if (typeof _ !== "undefined") {
//            var tech = {
//                type: "Underscore",
//                version: _.VERSION
//            };
//            techonologys.push(tech);
//        }
        //Check Extjs framework
        if (typeof Ext !== "undefined") {
            var tech = {
                type: "Extjs",
                version: Ext.versions.extjs.version
            };
            techonologys.push(tech);
        }
        return techonologys;
    },
    dateDiff: function (date) {
        var stringDateDiff = "";
        var dateAtMoment = new Date();
        var oneMinute = 60 * 1000;
        var dateDiff = dateAtMoment.getTime() - date.getTime();
        if (dateDiff < 0) {
            return "now";
        } else {
            if (dateDiff < (oneMinute)) {
                stringDateDiff = Math.floor(dateDiff / 1000) + " seconds ago";
            } else if (dateDiff < (oneMinute * 60)) {
                stringDateDiff = Math.floor(dateDiff / 60 / 1000) + " minutes ago";
            } else if (dateDiff < (oneMinute * 60 * 24)) {
                stringDateDiff = Math.floor(dateDiff / 60 / 60 / 1000) + " hours ago";
            } else if (dateDiff < (oneMinute * 60 * 24 * 7)) {
                stringDateDiff = Math.floor(dateDiff / 60 / 60 / 24 / 1000) + " days ago";
            } else if (dateDiff < (oneMinute * 60 * 24 * 7 * 30)) {
                stringDateDiff = Math.floor(dateDiff / 60 / 24 / 7 / 1000) + " weeks ago";
            } else if (dateDiff < (oneMinute * 60 * 24 * 7 * 30 * 12)) {
                stringDateDiff = Math.floor(dateDiff / 60 / 24 / 7 / 30 / 1000) + " months ago";
            } else {
                stringDateDiff = Math.floor(dateDiff / 60 / 24 / 7 / 30 / 12 / 1000) + " years ago";
            }
            return stringDateDiff;
        }
        
    },
    htmlEncode: function (value) {
        //create a in-memory div, set it's inner text(which jQuery automatically encodes)
        //then grab the encoded contents back out.  The div never exists on the page.
        return $('<div/>').text(value).html();
    },
    htmlDecode: function (value) {
        return $('<div/>').html(value).text();
    },
},
ENG.$.fn.appendToWithIndex = function (to, index) {
    if (!to instanceof jQuery) {
        to = $(to);
    };
    if (index === 0) {
        to.prependTo($(this));
    } else {
        to.insertAfter($(this).children()[index - 1]);
    }
}