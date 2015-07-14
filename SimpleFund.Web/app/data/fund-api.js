define(['plugins/http'], function (http) {
    // ReSharper disable once InconsistentNaming
    var URL = "api/funds/";

    return {
        list: function (filter) {
            return http.get(URL, filter);
        },
        detail:function(symbol) {
            return http.get(URL + symbol);
        }
    };
});