var contactApp = angular.module('contacts', [
	'ngRoute'
]);

contactApp.config([
	'$routeProvider',
	function ($routeProvider) {
		$routeProvider.
			when('/', {
				templateUrl: "/App/views/list.html",
				controller: "ListController"
			})
		.when('/list', {
			templateUrl: "/App/views/list.html",
			controller: "ListController"
		})
			.when('/create', {
				templateUrl: "/App/views/create.html",
				controller: "CreateController"
			})
			.when('/edit/:id', {
				templateUrl: "/App/views/edit.html",
				controller: "EditController"
			})
			.otherwise({
				redirectTo: "/"
			});
	}
]);

contactApp.controller("ListController", ["$scope", "$http", function ($scope, $http) {

	$scope.deleteItem = function(id) {
		$http.post("/Api/Delete", {
			id: id
		}).success(function (data) {
			$scope.refreshItems();
		});
	}

	$scope.refreshItems = function () {
		$http.get("/Api/Find").success(function(data) {
			$scope.items = data;
		});
	}

	$scope.refreshItems();

}]);

contactApp.controller("CreateController",
	["$scope", "$http", "$location",
		function ($scope, $http, $location) {
			$scope.item = {};

			$scope.saveData = function () {
				$http.post("/Api/Create", $scope.item).success(function (data) {
					$location.path("/list");
				});
			};

		}]);

contactApp.controller("EditController",
	["$scope", "$routeParams", "$http", "$location",
		function ($scope, $routeParams, $http, $location) {

			$http.get("/Api/Get/" + $routeParams.id).success(function (data) {
				$scope.item = data;
			});

			$scope.saveData = function () {
				$http.post("/Api/Update", $scope.item).success(function () {

					var data = new FormData();
					var fileUpload = $("#test")[0];
					for (var x = 0; x < fileUpload.files.length; x++) {
						data.append(x, fileUpload.files[x]);
					}

					var request = {
						method: "POST",
						url: "/Api/UploadFile/" + $routeParams.id,
						data: data,
						transformRequest: angular.identity,
						headers: {
							"Content-Type": undefined
						}
					};
					$http(request).success(function () {
						$location.path("/list");
					});

				});
			};

		}]);