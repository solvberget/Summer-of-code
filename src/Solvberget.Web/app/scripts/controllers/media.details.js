'use strict';

var mediaDetaljerCtrl = function ($scope, $rootScope, $routeParams, documents, reservations, documentRating, documentReview, favorites) {

    $scope.document = documents.get({id : $routeParams.id}, function(){

        $rootScope.breadcrumb.push($scope.document.title);

        $scope.imageUrl = $$config.apiPrefix + 'documents/' + $scope.document.id + '/thumbnail';

        var ogTypes = {
            'AudioBook' : 'book',
            'Book' : 'book',
            'Game' : 'object',
            'Journal' : 'book',
            'LanguageCourse' : 'event',
            'SheetMusic' : 'book'
        }

        $scope.ogUrl = $$config.appUrlPrefix + $rootScope.pathForDocument($scope.document);

        $scope.ogType = ogTypes[$scope.document.type];
        if(!$scope.ogType) $scope.ogType = 'object';

    });

    $scope.rating = documentRating.get({id : $routeParams.id});
    $scope.review = documentReview.get({id : $routeParams.id});

    $scope.toggleFavorite = function(){

        if($scope.document.isFavorite) favorites.remove({documentId : $scope.document.id});
        else favorites.add({documentId : $scope.document.id});

        $scope.document.isFavorite = !$scope.document.isFavorite; // todo: handle failure
    };

    $scope.toggleReservation = function(){

        if($scope.document.isReserved) reservations.remove({documentId : $scope.document.id});
        else reservations.add({documentId : $scope.document.id});

        $scope.document.isReserved = !$scope.document.isReserved; // todo: handle failure
    };
};

angular.module('Solvberget.WebApp')
    .controller('BookCtrl', mediaDetaljerCtrl)
    .controller('FilmCtrl', mediaDetaljerCtrl)
    .controller('AudioBookCtrl', mediaDetaljerCtrl)
    .controller('CdCtrl', mediaDetaljerCtrl)
    .controller('SheetMusicCtrl', mediaDetaljerCtrl)
    .controller('OtherMediaCtrl', mediaDetaljerCtrl)
    .controller('GameCtrl', mediaDetaljerCtrl)
    .controller('JournalCtrl', mediaDetaljerCtrl)
