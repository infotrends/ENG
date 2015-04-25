define([
    'text!js/SignInButton/SignInButton.html',
    'js/Component',
    'js/SignInButton/SignInButtonModel',
    'js/Toolbar/ToolbarView',
    'js/LeftMenu/LeftMenuView',
    'js/LeftMenu/LeftMenuModel',
    'js/FeedBack/FeedBackView',
    'js/RightPanel/RightPanelView',
    'js/Tracker/ClientInfoModel',
    'js/ListFeedBack/ListFeedBackView'

        
        
        

],
function (Template, Component, SignInButtonModel, ToolbarView, LeftMenuView, LeftMenuModel, FeedBackView, RightPanelView, ClientInfoModel, ListFeedBackView) {
    return Component.extend({
        constructor: function (options) {
            var cssLinkVal = ENG.DOMAIN + '/js/SignInButton/SignInButton.css';
            var opts = ENG.$.extend(true, {

                cssLink: cssLinkVal

            }, options);

            Component.prototype.constructor.call(this, opts);

            this.$el.addClass('eng-sign-in');
            var urlCss = ENG.DOMAIN + "/js/SignInButton/SignInButton.css";
            ENG.loadCss(urlCss);

        },


        events: {
            'click button#eng-sign-in-button': 'showLoginForm',
            'click #eng-signin-submit': 'signin',
            'click .eng_blur': 'hideLoginForm',
            'click #eng-register-button': 'showRegisterForm',
            'click .eng-signin-close': 'hideLoginForm',
            'click .close': 'hideRegisterForm',
            'click #signup_Submit': 'checkValidateMail'

        },


        render: function () {

            Component.prototype.render.call(this);
            var compile = _.template(this.template());
            var html = compile({});
            this.$el.append(html);
            this.$el.find("#eng-sign-in-form").hide();
            this.$el.find("#eng_Register").hide();
            this.$el.find(".eng_blur").hide();

            //Handle when user press enter
            var me = this;
            this.$el.find(".eng-sign-in-form-container").keypress(function (e) {
                if (e.keyCode == 13) {
                    me.$el.find("#eng-signin-submit").click();
                    return false;
                }
            });

            return this;
        },
        template: function () {
            return Template;
        },


        signin: function () {
            var userName = this.$el.find("input[type='email']").val();
            var password = this.$el.find("input[type='password']").val();
            var validationText = this.$el.find(".eng-sign-in-validation-text");

            if (userName === '') {
                validationText.html("Please enter your email!");
                validationText.show();
                return;
            }

            if (password === '') {
                validationText.html("Please enter your password!");
                validationText.show();
                return;
            }

            var filter = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;

            if (!filter.test(userName)) {
                validationText.html("Incorrect E-mail format!");
                validationText.show();
                return;
            }
            ENG.Api.Auth.Profile.login(userName, password, this.engHandleData);
            
            //this.$el.hide();
            //ENG.toolbar.setVisibility(true);

        },

        showLoginForm: function () {
            this.$el.find("#eng-sign-in-form").show();
            this.$el.find(".eng-sign-in-validation-text").hide();
            this.$el.find(".eng_blur").show();

            this.$el.find("#eng-sign-in-form").draggable({
                handle: "h1",
                cursor: "move",
                cursorAt: { top: -200, left: 0 }
            });
        },

        hideLoginForm: function () {

            this.$el.find("#eng-sign-in-form").hide();
            this.$el.find(".eng_blur").hide();
        },

        showRegisterForm: function () {
            this.$el.find("#eng-sign-in-form").hide();
            this.$el.find("#eng_Register").show();
            this.$el.find(".eng_blur").show();
        },
        hideRegisterForm: function () {
            this.$el.find("#eng_Register").hide();
            this.$el.find(".eng_blur").hide();
        },
        checkValidateMail: function () {
            var email = this.$("#mail");
            var pwd = this.$("#pass");
            var filter = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;

            if (!filter.test(email.val())) {
                alert('Invalid E-mail Format!');
                email.focus;
                return false;
            }

            if (pwd.val().length < 6) {
                alert("Password must contain at least six characters!");
                pwd.focus();
                return false;
            }
            if (pwd.val() == email.val()) {
                alert("Password must be different from Email!");
                pwd.focus();
                return false;
            }
        },
        engHandleData: function (success, data) {            
            var me = this;
            if (success) {
                
                sessionStorage.email = data.UserProfile.Email;
                sessionStorage.userName = data.UserProfile.First + " " + data.UserProfile.Last;

                //handle login success here
                ENG.signInButton.$el.hide();
                var $body = ENG.$('body');

                //Add Right Panel
                var rightPanel = new RightPanelView();
                $body.append(rightPanel.render().$el);
                ENG.rightPanel = rightPanel;

                //Add Toolbar
                var toolbar = new ToolbarView();
                $body.append(toolbar.render().$el);
                ENG.toolbar = toolbar;
                ENG.loadCss(toolbar.opts.cssLink);

                //Add Left Menu
                var leftMenu = new LeftMenuView();

                var leftMenuModel = new LeftMenuModel();
                $body.append(leftMenu.render(leftMenuModel).$el);
                ENG.leftMenu = leftMenu;

                var feedBack = new FeedBackView();
                $body.append(feedBack.render().$el);
                //ENG.leftMenu = leftMenu;

                var listFB = new ListFeedBackView();
                $body.append(listFB.render().$el);
                ENG.listFB = listFB;

                toolbar.setVisibility(false);
                ENG.leftMenu.setVisibility(false);
                ENG.rightPanel.setVisibility(false);
                ENG.toolbar.setVisibility(true);

            }
            else {
                var validationText = ENG.$(".eng-sign-in-validation-text");
                validationText.html("Incorrect Username or Password!");
                validationText.show();
            }
        },
        

    });
});
