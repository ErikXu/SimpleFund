define(['data/fund-api', 'knockout', 'shared/date-formatter', 'knockout-mapping', 'bootstrap', 'shared/pager'], function (fundApi, ko, formatter, mapping) {
    ko.mapping = mapping;
    var list = ko.observableArray([]).extend({
        datasource: {
            dataLoader: function () {
                var self = this;
                return fundApi.list(ko.mapping.toJS(self.filter)).then(function (response) {
                    if (response.data.length === 0 && self.pager.page() !== 1) {
                        self.pager.page(1);
                    } else {
                        list(response.data);
                        self.pager.totalCount(response.pager.totalCount);
                    }
                });
            }
        }
    });
    var vm = {
        list: list,
        formatter: formatter,
        activate: function () {
            list.load();
        },
        formatAmount : function(amount) {
            if (amount !== undefined && amount !== null) {
                return amount.toFixed(4);
            }

            return amount;
        }
    };

    return vm;
});