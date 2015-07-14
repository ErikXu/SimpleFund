define(['data/fund-api', 'knockout', 'shared/date-formatter', 'knockout-mapping', 'bootstrap', 'shared/pager'], function (fundApi, ko, formatter, mapping) {
    ko.mapping = mapping;

    var vm = {
        detail: ko.observable(),
        formatter: formatter,
        activate: function (symbol) {
            return fundApi.detail(symbol).then(function(detail) {
                vm.detail(detail);
            });
        }
    };

    return vm;
});