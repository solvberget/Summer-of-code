'use strict';

angular.module('solvbergetinfoScreenwebApp').controller('MainCtrl', function ($scope, $timeout) {
    $scope.slides =  [
      //{slideUrl: 'views/screen_news.html', duration: 3000},
      {slideUrl: 'views/screen_events.html', duration: 3000}
    ];

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
          },timeOut);
      };

    $scope.nextSlide(0);
  });