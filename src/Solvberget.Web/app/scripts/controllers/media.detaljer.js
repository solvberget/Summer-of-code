'use strict';

var mediaDetaljerCtrl = function ($scope, $rootScope, $routeParams, documents, documentRating) {

    $scope.document = documents.get({id : $routeParams.id}, function(){
        $rootScope.breadcrumb.push($scope.document.title);
        $scope.imageUrl = $rootScope.apiPrefix + 'documents/' + $scope.document.id + '/thumbnail';
    });

    $scope.rating = documentRating.get({id : $routeParams.id}, function(){
        console.log("rating:", $scope.rating);
    });
};

angular.module('Solvberget.WebApp')
    .controller('BookCtrl', mediaDetaljerCtrl)
    .controller('FilmCtrl', mediaDetaljerCtrl)
    .controller('LydbokCtrl', mediaDetaljerCtrl)
    .controller('CdCtrl', mediaDetaljerCtrl)
    .controller('NotehefteCtrl', mediaDetaljerCtrl);
