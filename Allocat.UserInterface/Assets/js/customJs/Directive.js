app.directive('modal', function () {
    return {
        template: '<div class="modal fade">' +
            '<div class="modal-dialog">' +
              '<div class="modal-content" style="height:326px;width: 800px;margin-left: -82px;">' +
                '<div class="modal-header">' +
                  '<button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>' +
                  '<h4 class="modal-title">Details</h4>' +
                '</div>' +
                '<div class="modal-body" ng-transclude></div>' +
              '</div>' +
            '</div>' +
          '</div>',
        restrict: 'E',
        transclude: true,
        replace: true,
        scope: true,
        link: function postLink(scope, element, attrs) {
            scope.$watch(attrs.visible, function (value) {
                if (value == true)
                    $(element).modal('show');
                else
                    $(element).modal('hide');
            });

            $(element).on('shown.bs.modal', function () {
                scope.$apply(function () {
                    scope.$parent[attrs.visible] = true;
                });
            });

            $(element).on('hidden.bs.modal', function () {
                scope.$apply(function () {
                    scope.$parent[attrs.visible] = false;
                });
            });
        }
    };
});

app.directive("fileread", ['$parse', '$window', function ($parse, $window) {
    return {
        scope: {
            opts: '='
        },
        link: function ($scope, $elm, $attrs) {
            var file_uploaded = $parse($attrs.fileread);
            $elm.bind('change', function () {
                $scope.$apply(function () {
                    file_uploaded.assign($scope, $elm[0].files[0]);

                });
            });

            $elm.on('change', function (changeEvent) {
                var reader = new FileReader();

                reader.onload = function (evt) {
                    $scope.$apply(function () {
                        var data = evt.target.result;

                        var workbook = XLSX.read(data, { type: 'binary' });

                        var headerNames = XLSX.utils.sheet_to_json(workbook.Sheets[workbook.SheetNames[1]], { header: 1 })[0];

                        var data = XLSX.utils.sheet_to_json(workbook.Sheets[workbook.SheetNames[1]]);
                        $scope.opts.columnDefs = [];

                        if (data.length > 0) {
                            headerNames.forEach(function (h) {
                                $scope.opts.columnDefs.push({

                                    field: h,
                                    width: 150
                                });
                            });

                            var filledData = [];
                            for (i = 0; i < data.length; i++) {
                                if (data[i].ProductMasterName != null)
                                    filledData.push(data[i]);
                            }
                            $scope.opts.data = filledData;
                            $window.UploadedExcelFile = $scope.myFile;
                        }
                        else
                            alert("no data in selected file");

                        $elm.val(null);

                    });
                };

                reader.readAsBinaryString(changeEvent.target.files[0]);
            });
        }
    }
}]);

app.directive('overwriteEmail', function () {
    var EMAIL_REGEXP = /^[a-z0-9!#$%&'*+/=?^_`{|}~.-]+@example\.com$/i;

    return {
        require: '?ngModel',
        link: function (scope, elm, attrs, ctrl) {
            // only apply the validator if ngModel is present and Angular has added the email validator
            if (ctrl && ctrl.$validators.email) {

                // this will overwrite the default Angular email validator
                ctrl.$validators.email = function (modelValue) {
                    return ctrl.$isEmpty(modelValue) || EMAIL_REGEXP.test(modelValue);
                };
            }
        }
    };
});

app.directive('hideme', ['$http', '$timeout', function ($http, $timeout) {
    return {
        transclude: true,
        restrict: 'E',
        template: '<div id="loaderDiv"  ng-show="showEl"><div class="ajax-loader" ng-transclude></div></div>',

        link: function ($scope, $elm, $attrs) {
            $scope.isLoading = function () {
                return $http.pendingRequests.length > 0;
            };

            $scope.$watch($scope.isLoading, function (v) {
                if (v) {
                    $scope.showEl = true;
                } else {
                    $scope.showEl = false;
                }
            });
        }
    };

}]);

app.directive('confirmOnExit', function () {
    return {
        link: function ($scope, elem, attrs) {
            window.onbeforeunload = function () {
                if ($scope.form_ConfirmOnExit.$dirty) {
                    return "You have some unsaved changes.Do you really want to exit?";
                }
            }
            $scope.$on('$locationChangeStart', function (event, next, current) {
                if ($scope.form_ConfirmOnExit.$dirty) {
                    if (!confirm("TYou have some unsaved changes.Do you really want to exit?")) {
                        event.preventDefault();
                    }
                }
            });
        }
    };
});

app.directive('modal2', function () {
    return {
        template: '<div class="modal fade">' +
            '<div class="modal-dialog">' +
              '<div class="modal-content"  ng-transclude></div>' +
            '</div>' +
          '</div>',
        restrict: 'E',
        transclude: true,
        replace: true,
        scope: true,
        link: function postLink(scope, element, attrs) {
            scope.$watch(attrs.visible, function (value) {
                if (value == true)
                    $(element).modal('show');
                else
                    $(element).modal('hide');
            });

            $(element).on('shown.bs.modal2', function () {
                scope.$apply(function () {
                    scope.$parent[attrs.visible] = true;
                });
            });

            $(element).on('hidden.bs.modal2', function () {
                scope.$apply(function () {
                    scope.$parent[attrs.visible] = false;
                });
            });
        }
    };
});

app.directive("passwordVerify", function () {
    return {
        require: "ngModel",
        scope: {
            passwordVerify: '='
        },
        link: function (scope, element, attrs, ctrl) {
            scope.$watch(function () {
                var combined;

                if (scope.passwordVerify || ctrl.$viewValue) {
                    combined = scope.passwordVerify + '_' + ctrl.$viewValue;
                }
                return combined;
            }, function (value) {
                if (value) {
                    ctrl.$parsers.unshift(function (viewValue) {
                        var origin = scope.passwordVerify;
                        if (origin !== viewValue) {
                            ctrl.$setValidity("passwordVerify", false);
                            return undefined;
                        } else {
                            ctrl.$setValidity("passwordVerify", true);
                            return viewValue;
                        }
                    });
                }
            });
        }
    };
});