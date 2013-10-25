'use strict';

var mediaDetaljerCtrl = function ($scope, $rootScope) {

    $scope.item = {

        id : 1,
        title : 'Tittel'
    };

};

angular.module('Solvberget.WebApp')
    .controller('BokCtrl', mediaDetaljerCtrl)
    .controller('FilmCtrl', mediaDetaljerCtrl)
    .controller('LydbokCtrl', mediaDetaljerCtrl)
    .controller('CdCtrl', mediaDetaljerCtrl)
    .controller('NotehefteCtrl', mediaDetaljerCtrl);
