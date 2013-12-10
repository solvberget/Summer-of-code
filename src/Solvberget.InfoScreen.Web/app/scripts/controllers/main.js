'use strict';

angular.module('solvbergetinfoScreenwebApp').controller('MainCtrl', function ($scope, $timeout, $routeParams, slides) {
    // Called after first slide retrieval. Starts a loop where slides are rotated every timeOut second.
    // Each slide defines its own timeout intervall.
    $scope.onSlidesReceived = function () {
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
    };

    // Reloads slides every timeOut milliseconds
    $scope.reloadSlides = function (timeOut, screenId) {
        $timeout(function() {
            slides(screenId).query(
                function (data) {
                    $scope.slides = data;
                    console.log("New slides loaded");
                    $scope.reloadSlides(timeOut);
                }
            );
        }, timeOut);
    };

    var screenId = ($routeParams.id) ? $routeParams.id : "default";
    // Load slides and start slideshow
    $scope.slides = slides(screenId).query($scope.onSlidesReceived);
    // Start reload rotation of slides
    $scope.reloadSlides(60 * 1000);
  });