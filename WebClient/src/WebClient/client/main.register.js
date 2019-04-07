require(["knockout", "app"], function (ko, app) {
    var viewModel = new app.App();
    app.AppSingleton.setInstance(viewModel);
    var element = $("body")[0];
    ko.applyBindings(viewModel, element);
});