'use strict';

angular.module('solvbergetinfoScreenwebApp').controller('MainCtrl', function ($scope, $timeout, $routeParams, slides) {
    $scope.onSlidesReceived = function (slideConfig) {
        $scope.template=$scope.slides[0];
        $scope.count=0;

        $scope.nextSlide=function(timeOut) {
            $timeout(function() {
                $scope.template = $scope.slides[$scope.count];
                $scope.count+=1;
                if($scope.count>=$scope.slides.length) {
                    $scope.count=0;
                }
                $scope.nextSlide($scope.slides[$scope.count].duration);
            }, timeOut);
        };

        $scope.nextSlide(0);
        console.log($scope.slides);
    };

    $scope.slides = slides.query($scope.onSlidesReceived);
  });