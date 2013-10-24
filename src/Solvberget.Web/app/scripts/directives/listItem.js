angular.module('Solvberget.WebApp')
    .directive('listItem', function() {
        return {
            scope : {
                title:"=title",
                imageUrl:"=imageUrl",
                url:"=url"
            },
            controller: function($scope, $location) {

                $scope.open = function(){
                    $location.path(this.url);
                }
            },
            replace:true,
            template: "<li class='media' ng-click='open()'>" +
                        "<span><img class='media-object' src='{{imageUrl}}' alt='{{title}}'></span>" +
                        "<div class='media-body'>" +
                            "<h4 class='media-heading'>{{title}}</h4>" +
                        "</div>" +
                      "</li>"
        };
    });