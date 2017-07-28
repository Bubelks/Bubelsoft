require(["knockout"],
    function (ko) {
        ko.components.register("buildings",
        {
            viewModel: function(params) { return params.viewModel},
            template: "home/buildings/buildings"
        });
    });
require(["knockout"],
    function (ko) {
        ko.components.register("notifications",
            {
                viewModel: function(params) { return params.viewModel },
                template: "home/notifications/notifications"
            });
    });