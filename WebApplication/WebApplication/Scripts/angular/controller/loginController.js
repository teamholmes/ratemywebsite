var appModule = angular.module('reviewmywebsite', []);


appModule.controller('loginController', ['$scope', '$http', function ($scope, $http) {

    var connectionWelcomeText = "Angular connected";


    $scope.init = function () {

        $scope.loadingText = connectionWelcomeText;

    };



    $scope.init();

}]);