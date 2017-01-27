(function () {
    'use strict';
    angular.module('angularTrix', []).directive('angularTrix', function () {
        return {
            restrict: 'A',
            require: 'ngModel',
            scope: {
                trixInitialize: '&',
                trixChange: '&'
               
            },
            link: function (scope, element, attrs, ngModel) {


                ngModel.$render = function () {
                    if (element[0].editor) {
                        element[0].editor.loadHTML(ngModel.$modelValue);
                    }

                    element.on('trix-change', function () {
                        ngModel.$setViewValue(element.html());
                    });
                };

                var registerEvents = function (type, method) {
                    element[0].addEventListener(type, function (e) {
                        if (type === 'trix-file-accept' && attrs.preventTrixFileAccept === 'true') {
                            e.preventDefault();
                        }

                        scope[method]({
                            e: e,
                            editor: element[0].editor
                        });
                    });
                };

                registerEvents('trix-initialize', 'trixInitialize');
                registerEvents('trix-change', 'trixChange');
            }
        };
    });

}());