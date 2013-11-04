'use strict';

angular.module('Solvberget.WebApp')
    .controller('SearchCtrl', function ($scope, $rootScope, $filter, documentSearch) {

        $rootScope.breadcrumb.clear();
        $rootScope.breadcrumb.push('Søk');

        $scope.selectedType = null;

        $scope.selectType = function(type){
            $scope.selectedType = type;

            if(null == type) delete $scope.resultFilter;
            else if(type == 'Other') {
                $scope.resultFilter = function(item){
                    return item.type != 'Book' && item.type != 'Film' && item.type != 'AudioBook' && item.type != 'CD' && item.type != 'SheetMusic';
                }
            }
            else $scope.resultFilter = {type : type};
        }

        $scope.search = function(){

            $scope.isSearching = true;

            $scope.results = documentSearch.query({query : $scope.searchQuery}, function(){

                $scope.allCount = $scope.results.length;
                $scope.bookCount = $scope.results.filter(function(item){ return item.type == 'Book'}).length;
                $scope.filmCount = $scope.results.filter(function(item){ return item.type == 'Film'}).length;
                $scope.audioBookCount = $scope.results.filter(function(item){ return item.type == 'AudioBook'}).length;
                $scope.cdCount = $scope.results.filter(function(item){ return item.type == 'CD'}).length;
                $scope.sheetMusicCount = $scope.results.filter(function(item){ return item.type == 'SheetMusic'}).length;
                $scope.otherCount = $scope.allCount - $scope.bookCount - $scope.filmCount - $scope.audioBookCount - $scope.cdCount - $scope.sheetMusicCount;

                $scope.isSearching = false;
            });
        }

        $scope.pathFor = function(item){
            var title = encodeURIComponent(item.title.replace(' ','-').toLowerCase());
            return $rootScope.path(item.type + 'Ctrl', {id: item.id, title : title});
        };

    });
