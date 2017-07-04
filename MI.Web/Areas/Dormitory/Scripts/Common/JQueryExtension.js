(function ($) {
    //序列化表单
    $.fn.serializeFormToObject = function () {
        var data = $(this).serializeArray();
        $(':disabled[name]', this).each(function () {
            data.push({ name: this.name, value: $(this).val() });
        });

        //映射为对象
        var obj = {};
        data.map(function (x) { obj[x.name] = x.value; });

        return obj;
    };
})($);