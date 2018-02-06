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
            { path: "user/userApp", tag: "user-app" },
            { path: "user/register/register", tag: "user-register" },
            { path: "home/home", tag: "home" },
            { path: "buildings/create/create", tag: "building-create" },
            { path: "buildings/list/list", tag: "buildings-list" },
            { path: "buildings/list/listItem", tag: "buildings-list-item" },
            { path: "buildings/building/building", tag: "building-home" },
            { path: "buildings/building/calendar/calendar", tag: "building-calendar" },
            { path: "buildings/building/estimation/estimation", tag: "building-estimation" },
            { path: "buildings/building/companies/companies", tag: "building-companies" },
            { path: "buildings/building/companies/subContractor/subContractor", tag: "building-subcontractor" },
            { path: "buildings/create/steps/building", tag: "building-step" },
            { path: "buildings/create/steps/estimate", tag: "estimate-step" },
            { path: "buildings/building/report/report", tag: "report" },
            { path: "buildings/building/reports/reports", tag: "building-reports" },
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