//https://github.com/CraigCav/ko.datasource
//https://github.com/remcoros/ko.pager

define(['jquery', 'knockout'], function ($, ko) {
    // ReSharper disable once InconsistentNaming
    var PAGE_SLIDE = 2;
    // ReSharper disable once InconsistentNaming
    var DEFAULT_FILTER = { limit: 10 };

    // ReSharper disable once InconsistentNaming

    function Pager(limit) {
        var self = this;
        self.page = ko.observable(1);
        self.totalCount = ko.observable(0);
        self.limit = ko.observable(limit);

        self.totalPages = ko.computed(function () {
            var count = Math.ceil(ko.utils.unwrapObservable(self.totalCount) / ko.utils.unwrapObservable(self.limit));
            return count === 0 ? 1 : count;
        });

        self.pages = ko.computed(function () {
            var pageCount = self.totalPages();
            var pageFrom = Math.max(1, self.page() - PAGE_SLIDE);
            var pageTo = Math.min(pageCount, self.page() + PAGE_SLIDE);
            pageFrom = Math.max(1, Math.min(pageTo - 2 * PAGE_SLIDE, pageFrom));
            pageTo = Math.min(pageCount, Math.max(pageFrom + 2 * PAGE_SLIDE, pageTo));

            return ko.utils.range(pageFrom, pageTo);
        });

        self.firstRecordNo = ko.computed(function () {
            if (self.totalCount() === 0) {
                return 0;
            }
            return self.limit() * (self.page() - 1) + 1;
        });

        self.lastRecordNo = ko.computed(function () {
            return Math.min(self.limit() * self.page(), self.totalCount());
        });

        self.next = function () {
            var currentPage = self.page();
            if (currentPage < self.totalPages()) {
                self.page(currentPage + 1);
            }
        };

        self.previous = function () {
            var currentPage = self.page();
            if (currentPage > 1) {
                self.page(currentPage - 1);
            }
        };

        self.first = function () {
            self.page(1);
        };

        self.last = function () {
            self.page(self.totalPages());
        };
    }

    ko.extenders.datasource = function (target, options) {
        var dataLoader, filter, init = true;
        if ($.isFunction(options)) {
            dataLoader = options;
            filter = {};
        } else {
            dataLoader = options.dataLoader;
            filter = options.filter;
        }

        filter = $.extend({}, DEFAULT_FILTER, filter);

        target.pager = new Pager(filter.limit);

        target.load = function () {
            return dataLoader.call(target);
        };

        target.refresh = function () {
            return dataLoader.call(target);
        };

        target.sort = function (sortBy) {
            var self = this;
            if (sortBy === self.filter.sortBy()) {
                self.filter.desc(!self.filter.desc());
            } else {
                self.filter.sortBy(sortBy);
                self.filter.desc(false);
            }
            target.refresh();
        };

        filter.offset = ko.computed(function () {
            return (target.pager.page() - 1) * target.pager.limit();
        }).extend({ notify: 'always' });

        filter.offset.subscribe(function () {
            if (init) {
                init = false;
            } else {
                filter.limit = target.pager.limit();
                target.refresh();
            }
        });

        target.filter = filter;

        target.subscribe(function (changes) {
            var delta = $.grep(changes, function (item) {
                return item.status === 'added';
            }).length - $.grep(changes, function (item) {
                return item.status === 'deleted';
            }).length;
            target.pager.totalCount(target.pager.totalCount() + delta);

        }, null, "arrayChange");


        return target;
    };
});