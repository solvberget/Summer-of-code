angular.module('Solvberget.WebApp')
    .directive('listItem', function() {
        return {
            scope : {
                title:"=listItem",
                imageUrl:"=imageUrl",
                url:"=url",
                subtext:"=subtext",
                showChevron:"=showChevron",
                showFavorite:"=showFavorite",
                isFavorite:"=isFavorite"
            },
            controller: function($scope, $location) {

                $scope.toggleFavorite = function(){
                    $scope.isFavorite = !$scope.isFavorite;
                }
            },
            replace:true,
            templateUrl: 'views/listItem.html'
        };
    });