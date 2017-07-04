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
            //打印按钮事件绑定
            $(".taiojianli_right a").click(function () {
                var t = $(this);
                var d=t.attr("data-date");
                window.open('/EmpAndFood/CompanyOfFood/GetEmployeeOfFood' + "?date=" + d, '宿舍员工订餐', 'status=no,toolbar=no,menubar=no,scrollbars=yes,resizable=yes,width=1200,height=800');
            });
        }
    };
};