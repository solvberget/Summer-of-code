'use strict';

var mediaDetaljerCtrl = function ($scope, $rootScope, $routeParams, $http, documents, reservations, documentRating, documentReview, favorites) {

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

    $scope.toggleReservation = function(branch){

        if ($scope.document.isReserved) {
            reservations.remove({ documentId: $scope.document.id });
            $scope.document.isReserved = false;
        }
        else {
            $http({
                method: 'PUT',
                url: $$config.apiPrefix + '/reservations/' + $scope.document.id,
                data: $.param({ branch: branch }),
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
            })
            .success(function (data, status, headers, config) {
                $scope.document.isReserved = true;
            });
        }
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
