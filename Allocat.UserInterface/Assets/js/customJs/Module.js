var UploadedExcelFile = '';
var UploadedAttachmentName = '';
//var app = angular.module("AllocatApp", ['angularNotify', 'ui.grid', 'lr.upload', 'ui.bootstrap', 'angular.filter', 'angularjs-datetime-picker', 'ngDialog']);
var app = angular.module("AllocatApp", ['angularNotify', 'ui.grid', 'lr.upload', 'ui.bootstrap', 'angular.filter', 'angularjs-datetime-picker', 'ngDialog', 'angularTrix', 'app.photo', 'ngResource']);

app.config(function ($httpProvider) {
    $httpProvider.defaults.headers.common = {};
    $httpProvider.defaults.headers.post = {};
    $httpProvider.defaults.headers.put = {};
    $httpProvider.defaults.headers.patch = {};
});


app.config(['ngDialogProvider', function (ngDialogProvider) {
    ngDialogProvider.setDefaults({
        className: 'ngdialog-theme-default',
        plain: false,
        showClose: false,
        closeByDocument: true,
        closeByEscape: true,
        appendTo: false
    });
}]);
