var oa = oa || {};
oa.services = oa.services || {};
oa.util = oa.util || {};
(function ($) {
    oa.util.createService = function (serviceName) {
        var service = oa.services[serviceName];
        if (service == undefined) {
            oa.services[serviceName]= {};
        } else
        {
            return service;
        }
        return oa.services[serviceName];
    };
})(jQuery);