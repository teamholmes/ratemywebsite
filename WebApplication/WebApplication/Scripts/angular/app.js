








//var appModule = angular.module('appName', []);


//appModule.controller('certificateControllerController', ['$scope', '$http', function ($scope, $http) {

//    var loadingTextDefault = "Loading data... Please wait.";
//    var errorrTextDefault = "Error loading data.";



//    function setStateOfIncludeAdditionalTeamMembers() {
//        if ($scope.certificateForEachTeamMember == false) {
//            $scope.includeAdditionalSupportTeamMembers = false;
//        }
//    }

//    //var url = @Url.Action("GetEventTeamsByPhaseApproved", "DevTool")
//    //alert(url);


//    $scope.loadJson = function () {

//        $scope.isLoading = true;
//        $scope.eventteams = {};
//        $scope.loadingtext = loadingTextDefault;
//        $http.get('http://localhost:12186/DevTools/GetEventTeamsByPhaseApproved', {
//            params: { phaseId: $scope.phaseobject }

//        }).success(function (data, status, headers, config) {
//            $scope.loadingtext = "";
//            $scope.eventteams = data.result;
//            $scope.isLoading = false;
//        }).error(function (data, status, headers, config) {
//            $scope.loadingtext = errorrTextDefault + " " + status + " " + data;
//            $scope.isLoading = false;
//        });
//    };//    $scope.checkboxClick = function (itm) {
//        //alert(itm.Selected);
//        //alert("hello");
//    };//    $scope.selectAll = function () {
//        //alert(itm.Selected);
//        for (var i = 0, l = $scope.eventteams.length; i < l; i++) {
//            $scope.eventteams[i].Selected = true;
//        }
//        //alert("hello");
//    };//    $scope.deSelectAll = function () {
//        //alert(itm.Selected);
//        for (var i = 0, l = $scope.eventteams.length; i < l; i++) {
//            $scope.eventteams[i].Selected = false;
//        }
//       // alert("bhello");
//    };//    $scope.teamOnlyChange = function () {
//        //alert(itm.Selected);
//        setStateOfIncludeAdditionalTeamMembers()
//        // alert("bhello");
//    };//    //$scope.sendSchool = function (input) {

//    //    var postData = { schoolname: '"' + input + '"'}
//    //    $http.post('http://localhost:15940/Home/ShowInsitutionspost',postData, {
//    //        params: { schoolname: input }
//    //    }).success(function (data, status, headers, config) {
//    //        $scope.schools = data.result;
//    //    }).error(function (data, status, headers, config) {
//    //        alert("there has been an error " + status + " " + data);
//    //    });
//    //};
//    //$scope.parseInt = function (value) {
//    //        return parseInt(value);
//    //    }



//    //$scope.reset = function () {
//    //    $scope.init();
//    //};





//    $scope.init = function () {
//        $scope.phaseobject = 2;
//        $scope.isLoading = false;
//        $scope.certificateForEachTeamMember = false;
//        $scope.includeAdditionalSupportTeamMembers = false;
//        $scope.loadJson();
//        //$scope.lastName = "Doeey";
//        //$scope.newschoolname = "";
//        //$scope.sex = "Oh no - we are british so dont take the bus shelters with us";
//    };


//    $scope.init();

//}]);

