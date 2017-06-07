var ko = require(["knockout"],
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
require(["knockout", "home/buildings/buildings"],
    function (ko, buildings) {
        ko.components.register("buildings",
        {
            viewModel: buildings.Buildings,
            template: "home/buildings/buildings"
        });
    });