var appModule = angular.module('reviewmywebsite', []);


appModule.controller('loginController', ['$scope', '$http', function ($scope, $http) {

    var connectionWelcomeText = "Angular connected";


    $scope.init = function () {

        $scope.statusText = connectionWelcomeText;

        $scope.statusText = "";
        $scope.isLoading = false;
        $scope.loadJson();

    };


    $scope.selectItem = function (item) {
        $scope.statusText = item.Name;
    };

    $scope.loadJson = function () {
        $scope.statusText = "Please wait while the data loads..."
            $scope.isLoading = true;
            $scope.reviewWebsites = {};
        $http.get('/api/public', {
                params: {  }

            })
                .success(function (data, status, headers, config) {
                $scope.reviewWebsites = data;
                $scope.isLoading = false;
                $scope.statusText = "";
                })
                .error(function (data, status, headers, config) {
                    $scope.statusText = errorrTextDefault + " " + status + " " + data;
                $scope.isLoading = false;
            });
        };

    $scope.init();

}]);

appModule.directive('websitelist', function () {
    return {
        templateUrl: '/Scripts/angular/View/websitelist.html'
    };
});