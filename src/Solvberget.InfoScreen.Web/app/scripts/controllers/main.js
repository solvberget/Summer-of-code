'use strict';

angular.module('solvbergetinfoScreenwebApp').controller('MainCtrl', function ($scope, $rootScope, $timeout, $routeParams, slides) {
    // Called after first slide retrieval. Starts a loop where slides are rotated every timeOut second.
    // Each slide defines its own timeout interval.
    $scope.onSlidesReceived = function () {
        $scope.template=$scope.slides[0];
        $scope.count=0;

        $scope.nextSlide=function(timeOut) {
            $timeout(function() {
                $scope.template = $scope.slides[$scope.count];
                $scope.templateName = "views/slides/"+$scope.template.template+".html";
                $scope.count+=1;
                if($scope.count>=$scope.slides.length) {
                    $scope.count=0;
                }

                console.log("slides", $scope.slides);
                console.log("template", $scope.template);   

                $scope.nextSlide($scope.template.duration * 1000);
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
                    $scope.reloadSlides(timeOut, screenId);
                }
            );
        }, timeOut);
    };

    // Mapper between slide names and

    var screenId = ($routeParams.id) ? $routeParams.id : "default";
    // Load slides and start slideshow
    
    var testSlides = [
        {
            template: "instagram",
            duration: 600
        }
    ];

    $scope.slides = testSlides; //slides(screenId).query($scope.onSlidesReceived);

    $scope.onSlidesReceived();

    // Start reload rotation of slides
    $scope.reloadSlides(2 * 60 * 1000, screenId);
    $rootScope.title = "Sølvberget";
});
