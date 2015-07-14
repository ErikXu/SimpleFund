requirejs.config({
    paths: {
        'text': '../lib/require/text',
        'durandal': '../lib/durandal/js',
        'plugins': '../lib/durandal/js/plugins',
        'plugins/http': 'shared/http',
        'transitions': '../lib/durandal/js/transitions',
        'knockout': '../lib/knockout/knockout',
        'knockout-mapping': '../lib/knockout/mapping/knockout.mapping-latest',
        'bootstrap': '../lib/bootstrap/js/bootstrap.min',
        'toastr': '../lib/toastr/js/toastr',
        'moment': '../lib/moment/moment.min',
        'jquery': '../lib/jquery/jquery.min'
    },
    shim: {
        'bootstrap': {
            deps: ['jquery'],
            exports: 'jQuery'
        },
        'knockout-mapping': {
            deps: ['knockout'],
            exports: 'knockout-mapping'
        }
    }
});

define(['durandal/system', 'durandal/app', 'durandal/viewLocator'], function (system, app, viewLocator) {

    system.debug(false);

    app.title = '基金研究';

    app.configurePlugins({
        router: true,
        dialog: true
    });

    app.start().then(function () {
        //Replace 'viewmodels' in the moduleId with 'views' to locate the view.
        //Look for partial views in a 'views' folder in the root.
        viewLocator.useConvention();

        //Show the app by setting the root view model for our application with a transition.
        app.setRoot('viewmodels/shell', 'entrance');
    });
});