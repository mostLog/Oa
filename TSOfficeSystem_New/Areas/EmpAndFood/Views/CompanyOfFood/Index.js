$(function () {


    //获取服务
    var service = oa.services.companyoffoodservice;
    var btnObj = new ButtonObject(service);
    btnObj.Init();

});

//按钮事件初始化
function ButtonObject(service) {


    return {
        Init: function () {
            this.InitBindEvent();
        },
        InitBindEvent: function () {
            //统计按钮
            $("#btnStat").click(function () {
                if (confirm("你确定要重新获取数据吗？")) {
                    service.statInfo().done(function (data) {
                        if (data == 1) {
                            alert("数据获取成功！");
                            location.href = location.href;
                        }
                    });
                }
            });
            //修改按钮
            $("#heguan").click(function () {
                var pars = $(this).attr("data-parameters");
                var ps = pars.split(",");
                if (confirm("你确定要修改吗？")) {
                    service.updateCompanyOfFood(
                        {
                            f_cID: ps[0],
                            f_breakfast: $("#f_breakfast").val(),
                            f_lunch: $("#f_lunch").val(),
                            f_dinner: $("#f_dinner").val(),
                            f_Midnight: $("#f_Midnight").val(),
                            f_type_tID:ps[1]
                        })
                        .done(function(data) {
                            if (data == 1) {
                                alert("修改成功");
                                location.href = location.href;
                            } else {
                                alert("修改失败！");
                            }
                        });
                }
            });
        }
    };
};
