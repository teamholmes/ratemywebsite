var appModule = angular.module('reviewmywebsite', []);


appModule.controller('loginController', ['$scope', '$http', function ($scope, $http) {

    var connectionWelcomeText = "Angular connected";


    $scope.init = function () {

        $scope.loadingText = connectionWelcomeText;

        $scope.loadingtext = "";
        $scope.isLoading = false;
        $scope.loadJson();

    };


    $scope.loadJson = function () {
        alert("loading");
            $scope.loadingtext = "Please wait while the data loads..."
            $scope.isLoading = true;
            $scope.reviewWebsites = {};
        $http.get('/api/public', {
                params: {  }

            })
                .success(function (data, status, headers, config) {
                $scope.reviewWebsites = data;
                $scope.isLoading = false;
                $scope.loadingtext = "";
                })
                .error(function (data, status, headers, config) {
                $scope.loadingtext = errorrTextDefault + " " + status + " " + data;
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