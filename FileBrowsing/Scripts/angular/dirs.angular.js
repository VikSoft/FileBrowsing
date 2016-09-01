angular
    .module("dirApp", [])
    .controller("dirCtrl", function ($scope, $http) {
    $scope.Actions = {
        NULL: 0,
        GetRoot: 1,
        GetItems: 2,
        CountFiles: 3
    }
    $scope.sizes = {
        Small: 0,
        Medium: 0,
        Large: 0,
        Done: false
    }
    $scope.Drives = true;

    $scope.resetSizes = function() {
        $scope.sizes.Small = 0;
        $scope.sizes.Medium = 0;
        $scope.sizes.Large = 0;
        $scope.sizes.Done = false;
    }

    $scope.load = function () {
        $scope.items = [];
        $scope.postdata = { Path: ""}
        $scope.getItems();
    }

    $scope.getItems = function() {
        $http.post("/api/Directory", $scope.postdata).success(function (data) {
            $scope.current = ($scope.postdata.Path === "") ? "My lovely drives" : $scope.postdata.Path;
            $scope.Drives = ($scope.postdata.Path === "");
            $scope.items = data;
        });
    }

    $scope.getSizes = function(path, last) {
        $http.post("/api/Directory", { Type: $scope.Actions.CountFiles, Path: path + "|" + last }).success(function (data) {
            if (data.Path !== $scope.current) return;

            $scope.sizes.Small += data.Small;
            $scope.sizes.Medium += data.Medium;
            $scope.sizes.Large += data.Large;

            if (!data.Done) {
                $scope.getSizes(data.Path, data.LastPath);
            } else {
                $scope.sizes.Done = true;
            }
        });
    }

    $scope.go = function (index) {
        if (!$scope.items[index].IsDir) return;
        $scope.postdata = { Path: $scope.items[index].Path }
        $scope.getItems();
        $scope.resetSizes();
        $scope.getSizes($scope.items[index].Path, "");
    }

    $scope.load();
});