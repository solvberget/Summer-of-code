angular
    .module('globalErrors', [])
    .config(function($provide, $httpProvider, $compileProvider) {

        var showMessage = function(content, cl) {

            var container = $("#appMessageContainer")

            container.hide();
            container.removeClass();
            container.addClass('alert alert-dismissable');
            container.addClass(cl);

            container.children("span").text(content);
            container.fadeIn('fast');
        };

        $httpProvider.responseInterceptors.push(function($timeout, $q) {
            return function(promise) {
                return promise.then(function(successResponse) {
                    if (successResponse.config.method.toUpperCase() != 'GET')
                        showMessage('Success', 'alert-success');
                    return successResponse;

                }, function(errorResponse) {
                    switch (errorResponse.status) {
                        case 401:
                            showMessage('Wrong usename or password', 'alert-warning');
                            break;
                        case 403:
                            showMessage('You don\'t have the right to do this', 'alert-danger');
                            break;
                        case 500:
                            showMessage('Server internal error: ' + errorResponse.data, 'alert-danger');
                            break;
                        default:
                            showMessage('Error ' + errorResponse.status + ': ' + errorResponse.data, 'alert-danger');
                    }
                    return $q.reject(errorResponse);
                });
            };
        });
    });