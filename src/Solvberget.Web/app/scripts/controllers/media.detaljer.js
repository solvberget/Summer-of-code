'use strict';

var mediaDetaljerCtrl = function ($scope, $rootScope, $routeParams, documents) {

    $scope.document = documents.get({id : $routeParams.id}, function(){

        $rootScope.breadcrumb.push($scope.document.Title);
    });
};

angular.module('Solvberget.WebApp')
    .controller('BokCtrl', mediaDetaljerCtrl)
    .controller('FilmCtrl', mediaDetaljerCtrl)
    .controller('LydbokCtrl', mediaDetaljerCtrl)
    .controller('CdCtrl', mediaDetaljerCtrl)
    .controller('NotehefteCtrl', mediaDetaljerCtrl);
