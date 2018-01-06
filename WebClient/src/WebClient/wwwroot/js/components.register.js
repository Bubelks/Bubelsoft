require(["knockout"],
    function (ko) {
        var myTemplateLoader = {
            loadTemplate: function (name, templateName, callback) {
                var fullUrl = "../views/" + templateName + ".html";
                $.get(fullUrl, function (markupString) {
                    ko.components.defaultLoader.loadTemplate(name, markupString, callback);
                });
            }
        };
        ko.components.loaders.unshift(myTemplateLoader);
    });
require(["knockout"],
    function(ko) {

        var components = [
            { path: "home/buildings/buildings", tag: "buildings" },
            { path: "home/buildings/building", tag: "building" },
            { path: "home/notifications/notifications", tag: "notifications" },
            { path: "home/notifications/notification", tag: "notification" },
            { path: "company/company", tag: "company" },
            { path: "company/companyApp", tag: "company-app" },
            { path: "company/create/companyCreate", tag: "company-create" },
            { path: "company/info/companyInfo", tag: "company-info" },
            { path: "company/user/create", tag: "user-create" },
            { path: "login/login", tag: "login" },
            { path: "home/home", tag: "home" },
            { path: "buildings/create/create", tag: "building-create" },
            { path: "buildings/create/steps/building", tag: "building-step" },
            { path: "buildings/create/steps/estimate", tag: "estimate-step" },
            { path: "buildings/buildingsApp", tag: "buildings-app" }
        ];

        components.forEach(c => {
            ko.components.register(c.tag,
                {
                    viewModel: function(params) { return params.viewModel; },
                    template: c.path
                });
        });
    });