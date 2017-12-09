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
            { path: "login/login", tag: "login" },
            { path: "home/home", tag: "home" }
        ];

        components.forEach(c => {
            ko.components.register(c.tag,
                {
                    viewModel: function(params) { return params.viewModel; },
                    template: c.path
                });
        });
    });