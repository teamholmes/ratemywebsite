var appModule = angular.module('reviewmywebsite', []);


appModule.controller('loginController', ['$scope', '$http', function ($scope, $http) {

    var connectionWelcomeText = "Angular connected";

    var errorrTextDefault = "Error "

    var newReview = {};


    function resetAddReviewFrom() {
        newReview = {};
        $scope.form = newReview;
    }


    $scope.init = function () {

        $scope.statusText = connectionWelcomeText;
        resetAddReviewFrom();
        $scope.statusText = "";
        $scope.isLoading = false;
        $scope.loadJson();
        $scope.reviewWebsites = {};

    };


    $scope.selectItem = function (item) {
        $scope.statusText = item.Name;
    };

    $scope.addReview = function (form) {
        newReview = $scope.form;

        $scope.statusText = "Please wait while the data is saved..."
        $scope.isLoading = true;
        $http({
            method: 'POST',
            url: '/api/Public',
            data: newReview
        })
                .success(function (data, status, headers, config) {

                    if (data.Success == true)
                    {
                        $scope.reviewWebsites.push(data.ResultData);
                    }

                    $scope.isLoading = false;
                    $scope.statusText = data.Message;
                    resetAddReviewFrom();
                })
                .error(function (data, status, headers, config) {
                    $scope.statusText = errorrTextDefault + " " + status + " " + data;
                    $scope.isLoading = false;
                });




    };



    $scope.loadJson = function () {
        $scope.statusText = "Please wait while the data loads..."
        $scope.isLoading = true;
        $scope.reviewWebsites = {};
        $http.get('/api/public/', {
            params: {}

        })
                .success(function (data, status, headers, config) {
                    if (data.Success == true) {
                        $scope.reviewWebsites = data.ResultData;
                    }
                    $scope.isLoading = false;
                    $scope.statusText = data.Message;
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

appModule.directive('addreview', function () {
    return {
        templateUrl: '/Scripts/angular/View/AddReview.html'
    };
});