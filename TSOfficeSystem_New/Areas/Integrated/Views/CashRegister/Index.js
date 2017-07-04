$(function () {

    //获取服务
    var service = oa.services.cashregisterservice;
    var btnObj = new ButtonObject(service);
    btnObj.Init();

});
//
function ButtonObject(service) {
    return {
        Init: function () {
            this.InitSubmit();
            this.InitBindEvent();
            this.InitControl();
        },
        InitSubmit: function () {
            var $dialog = $('div.addbg');
            var $form = $dialog.find('form');
            //初始化页面上面的按钮事件
            $form.find('button[type="submit"]').click(function (e) {
                $form.validate();

                e.preventDefault();

                if (!$form.valid()) {
                    return;
                }
                var model = $form.serializeFormToObject();
                service.addOrEdit(model).done(function (data) {
                    if (data != null) {
                        if (data == "1") {
                            alert("操作成功");
                            location.href = location.href;
                        } else if (data == "500") {
                            alert("操作异常");
                        }
                    }
                });
            });

            //初始化验证配置信息
            $form.validate({
                ignore: [],//必须设置否则无法验证隐藏域
                rules: {
                    f_Income: {
                        number: true
                    },
                    f_Expenses: {
                        number:true
                    },
                    f_Items: {
                        required:true
                    }
                },
                messages: {
                    f_Items: {
                        required:"品项不能为空！"
                    }
                }
            });
        },
        InitBindEvent: function () {
            //添加按钮
            $("#btn-add").click(function () {
                //重置表单信息
                $("#formTable").clearForm(true);
                //设置默认值
                $("#Date").val(new Date().format("yyyy-MM-dd"));
                $('#Date').datetimepicker('update');
                $("#Income").val("0");
                $("#Expenses").val("0");
                $($("#formTable input[type='radio']")[1]).prop("checked", true);
                OpenADDwindow();
            });
            $("#btn-cancel").click(function () {
                closeADDwindow();
            });
            //导出按钮
            $("#a_export").click(function () {
                //查询条件
                var data = "Name=" + encodeURI($('#sName').val())
                    + "&StartDate=" + encodeURI($('#sStartDate').val())
                    + "&EndDate=" + encodeURI($('#sEndDate').val())
                //跳转页面(直接打开 Excel)
                location.href = '/Integrated/CashRegister/ExportExcel?' + data;
            });
            //保存汇率
            $("#btn-Rate").click(function () {
                var p = {
                    Dollar: $("#Dollar").val(),
                    RMB: $("#RMB").val(),
                    Taiwan: $("#Taiwan").val()
                };
                service.updateRate(p).done(function (data) {
                    if (data==1) {
                        alert("保持汇率成功！");
                        location.href = location.href;
                    } else
                    {
                        alert("保持汇率失败！");
                    }
                });
            });
            //绑定修改按钮事件
            var uBtnList = $("button[data-mark='update']");
            uBtnList.each(function () {
                var obj = $(this);
                var id = obj.attr("data-id");
                obj.click(function () {
                    clearErrorInfos();
                    service.getCashRegisterById({
                        id: id
                    }).done(function (data) {
                        //初始化表单信息
                        OpenADDwindow();
                        var txtControls = $("#formTable input[type='text'],input[type='hidden']");
                        txtControls.each(function () {
                            var pName = $(this).attr("id");
                            $(this).val(data[pName]);
                        });
                        //
                        //是否有收据
                        if (data.HasReceipt) {
                            $($("#formTable input[type='radio']")[0]).prop("checked", true);
                        } else {
                            $($("#formTable input[type='radio']")[1]).prop("checked", true);
                        }
                    });
                });
            });
            //绑定删除按钮事件
            var uBtnList = $("button[data-mark='delete']");
            uBtnList.each(function () {
                var obj = $(this);
                var id = obj.attr("data-id");
                obj.click(function () {
                    if (confirm("是否确定删除？")) {
                        service.delete({
                            id: id
                        }).done(function (data) {
                            if (data != null) {
                                if (data == "1") {
                                    alert("删除成功");
                                    location.href = location.href;
                                } else if (data == "500") {
                                    alert("删除异常");
                                }
                            }
                        });
                    }

                });
            });
          
        },
        //初始化控件
        InitControl: function () {
            $.datetimepicker.setLocale('zh');
            //搜索框日期
            $('#sStarDate').datetimepicker({
                lang: 'zh',
                format: 'Y-m-d',
                onShow: function (ct) {
                    this.setOptions({
                        maxDate: $('#sEndDate').val() ? $('#sEndDate').val() : false
                    });
                },
                timepicker: false
            });
            $('#sEndDate').datetimepicker({
                lang: 'zh',
                format: 'Y-m-d',
                onShow: function (ct) {
                    this.setOptions({
                        minDate: $('#sStarDate').val() ? $('#sStarDate').val() : false
                    });
                },
                timepicker: false
            });
            $('#Date').datetimepicker({
                lang: 'zh',
                format: 'Y-m-d',
                timepicker: false
            });

        }
    };
}