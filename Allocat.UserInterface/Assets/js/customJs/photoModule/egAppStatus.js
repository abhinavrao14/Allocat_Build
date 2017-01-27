(function () {
    'use strict';

    angular
        .module('AllocatApp')
        .directive('egAppStatus', egAppStatus);

    egAppStatus.$inject = ['appInfo'];

    function egAppStatus(appInfo) {
        var directive = {
            link: link,
            restrict: 'E',
            templateUrl: '../../Assets/js/customJs/PhotoModule/egAppStatus.html'
        };
        return directive;

        function link(scope, element, attrs) {
            scope.status = appInfo.status;
        }
    }

})();