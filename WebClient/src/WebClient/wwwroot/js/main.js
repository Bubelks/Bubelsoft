require(["knockout", "app"], function (ko, app) {
    var viewModel = new app.App();
    var element = $("#main-page")[0];
    ko.applyBindings(viewModel, element);
});