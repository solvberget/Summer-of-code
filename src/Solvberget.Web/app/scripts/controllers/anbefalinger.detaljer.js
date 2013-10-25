'use strict';

angular.module('Solvberget.WebApp')
    .controller('AnbefalingerDetaljerCtrl', function ($scope, $rootScope) {

        $rootScope.pageTitle = 'ANBEFALINGER';

        $scope.list = {
            title : "Fredrik's anbefalinger"
        };

        $scope.list.items = [
            {
                id: 1,
                title : "En bok",
                subtext: "Forfatternavn",
                type:'Bok',
                imageUrl : 'http://placehold.it/60x80',
                isFavorite: true},
            {
                id: 2,
                title : "En annen bok",
                subtext: "Forfatternavn",
                type:'Bok',
                imageUrl : 'http://placehold.it/60x80'},
            {
                id: 3,
                title : "En CD",
                subtext: "Forfatternavn",
                type:'Cd',
                imageUrl : 'http://placehold.it/60x80'}];

        $scope.pathFor = function(item){

            var title = item.title.replace(' ','-').toLowerCase();

            return $rootScope.path(item.type + 'Ctrl', {id: item.id, title : title});
        };
    });
