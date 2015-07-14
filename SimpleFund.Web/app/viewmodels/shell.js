define(['plugins/router', 'jquery', 'knockout'], function (router, $, ko) {

    var defaultRoutes = [
       { route: '', title: '基金列表', moduleId: 'viewmodels/list' },
       { route: ['detail/:symbol*'], title: '基金详情', moduleId: 'viewmodels/detail' }
    ];

    var isAjaxing = ko.observable(false);
    $(document).ajaxStart(function () { isAjaxing(true); })
               .ajaxStop(function () { isAjaxing(false); });

    var vm = {
        router: router,
        isLoading: ko.computed(function () {
            return (router.isNavigating() || isAjaxing());
        }),
        activate: function () {
            router.map(defaultRoutes).buildNavigationModel();
            return router.activate();
        }
    };

    return vm;
});