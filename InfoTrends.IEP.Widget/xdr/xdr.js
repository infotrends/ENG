var Xdr = {
    _callbacks: null,
    _iframe: null,
    _id: 0,

    ajax: function(config, callback, scope) {
        var me = this;

        // increment id
        config.callbackId = (++me._id);

        // save
        me._callbacks[me._id] = {
            config: config,
            callback: callback,
            scope: scope
        };

        var cfgClone = JSON.parse(JSON.stringify(config));
        me._postMessage("xdr.send", cfgClone);
    },

    _init: function() {
        var me = this;

        me._id = 0;
        me._callbacks = [];

        me._iframe = me._createIframe();
        me._on('message', me._onMessage, me);
    },
    _on: function(eventName, func, scope) {
        var fnEvent = function() {
            func.apply(scope, arguments);
        };
        if (window.addEventListener) {
            var evt = eventName.toLowerCase();
            window.addEventListener(evt, fnEvent, false);
        } else if (window.attachEvent) {
            var evt = 'on' + eventName.toLowerCase();
            window.attachEvent(evt, fnEvent);
        }
    },
    _postMessage: function(eventName, data) {
        var me = this;
        var msg = { eventName: eventName, data: data };
        msg = JSON.stringify(msg);
        me._iframe.contentWindow.postMessage(msg, "*");
    },
    _parseRoot: function() {
        var scripts = document.scripts;
        var key = '/xdr/xdr.js';
        var path = null;

        for (var i = 0; i < scripts.length; i++) {
            var script = scripts[i];
            if (script == null) continue;
            src = script.src;
            if (src == null) continue;
            src = src.toLowerCase();

            if (src.indexOf(key) > -1) {
                path = src;
                break;
            }
        }

        if (path != null) {
            path = path.split(key)[0] + '/xdr/?_bust=' + (new Date()).getTime();
        }

        return path;
    },
    _createIframe: function() {
        //var src = this._parseRoot();
        var src = "//10.16.56.135/engblankpage"; //src for testing
        var iframe = document.createElement('iframe');
        iframe.setAttribute('src', src);
        iframe.setAttribute('width', '200');
        iframe.setAttribute('height', '200');
        iframe.setAttribute('style', 'display:none');
        document.body.appendChild(iframe);
        return iframe;
    },
    _onMessage: function(e) {
        var me = this;

        var msg = JSON.parse(e.data);
        var evt = msg.eventName;
        var result = msg.data;

        if (evt != 'xdr.receive')
            return;

        var o = me._callbacks[result.callbackId];
        if (o != null && o.callback != null) {
            o.callback.call(o.scope, result);
        }
    }
};
Xdr._init();
