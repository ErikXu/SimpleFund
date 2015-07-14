define(['moment'], function (moment) {
    var min = moment('0001-01-01T00:00:00Z');

    function parse(time) {
        if (time) {
            var t = moment(time);
            if (!t.isSame(min)) {
                return t;
            }
        }
        return null;
    }

    var vm = {
        simpleDate: function (time) {
            var t = parse(time);
            return t && t.format('YYYY-MM-DD');
        },
        format: function (time, foramt) {
            var t = parse(time);
            return t && t.format(foramt);
        }
    };
    return vm;
});