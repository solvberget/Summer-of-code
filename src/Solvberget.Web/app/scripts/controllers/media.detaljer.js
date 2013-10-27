'use strict';

var mediaDetaljerCtrl = function ($scope, $routeParams, mediaItemRepository) {

    $scope.item = mediaItemRepository.get($routeParams.id);
};

angular.module('Solvberget.WebApp')
    .controller('BokCtrl', mediaDetaljerCtrl)
    .controller('FilmCtrl', mediaDetaljerCtrl)
    .controller('LydbokCtrl', mediaDetaljerCtrl)
    .controller('CdCtrl', mediaDetaljerCtrl)
    .controller('NotehefteCtrl', mediaDetaljerCtrl);
