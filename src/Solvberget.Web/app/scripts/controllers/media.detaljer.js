'use strict';

var mediaDetaljerCtrl = function ($scope, $rootScope, $routeParams, documents, documentRating, documentReview) {

    $scope.document = documents.get({id : $routeParams.id}, function(){
        $rootScope.breadcrumb.push($scope.document.title);
        $scope.imageUrl = $$config.apiPrefix + 'documents/' + $scope.document.id + '/thumbnail';
    });

    $scope.rating = documentRating.get({id : $routeParams.id});
    $scope.review = documentReview.get({id : $routeParams.id});
};

angular.module('Solvberget.WebApp')
    .controller('BookCtrl', mediaDetaljerCtrl)
    .controller('FilmCtrl', mediaDetaljerCtrl)
    .controller('AudioBookCtrl', mediaDetaljerCtrl)
    .controller('CdCtrl', mediaDetaljerCtrl)
    .controller('SheetMusicCtrl', mediaDetaljerCtrl)
    .controller('OtherMediaCtrl', mediaDetaljerCtrl)
