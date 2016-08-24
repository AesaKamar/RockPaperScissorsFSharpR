(function (app) {
    "use strict";
    //Factory to build new matches
    app.factory('Match', function ($http, $q) {
        function Match() {
            this.Id = null;
            this.PlayerName = null;
            this.P1Choice = null;
            this.P2Choice = null;
            this.Winner = 0;
            this.Timestamp = null;
            this.$post = function () {
                var defer = $q.defer();
                $http.post('http://localhost:1337/api/matches', this).then(
                    function (res) {
                        defer.resolve(res.data)
                    },
                    function (err) {
                        defer.reject(err)
                    });
                return defer.promise;
            }
        };
        return Match;
    });


    //Use Angular's moduleAPI to define new controller
    app.controller('IndexController', IndexController);

    //Make our dependencies EXPLICIT
    IndexController.$inject = ['$scope', '$http', '$q', 'Match'];

    //This is our controller
    function IndexController($scope, $http, $q, Match) {

        //Initialization
        function init() {
            $scope.hand_signs = [
                 { name: 'rock', enum_code: 0 },
                 { name: 'paper', enum_code: 1 },
                 { name: 'scissors', enum_code: 2 }
            ];
            $scope.PlayerName;
            $scope.P1Choice;
            $scope.this_match;
            $scope.ctx = document.getElementById("WinningsChart");
        };
        init();

        //Server Interaction
        $scope.createMatch = function (sign) {
            var new_match = new Match();
            new_match.PlayerName = $scope.PlayerName;
            new_match.P1Choice = sign.enum_code;

            new_match.$post().then(
                function (res) {
                    console.log(res);
                    $scope.this_match = res;
                    $scope.pieChart.data.datasets[0].data[res.Winner] += 1;
                    $scope.pieChart.update();
                },
                function (err) {
                    console.log(err);
                });
        };

        //Charting and Analytics
        $scope.chartData = {
            labels: ["Ties", "You", "AI"],
            datasets: [
                {
                    data: [0,0,0],
                    backgroundColor: [
                        "#000066",
                        "#990099",
                        "#99cc00"
                    ],
                    hoverBackgroundColor: [
                        "#000066",
                        "#990099",
                        "#99cc00"
                    ]
                }
            ]
        };
        $scope.pieChart = new Chart($scope.ctx, {
            type: 'doughnut',
            data: $scope.chartData
        });
    }

})(angular.module('RPS'));