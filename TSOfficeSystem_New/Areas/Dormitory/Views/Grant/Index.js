$(function () {

    
    //获取服务
    var service = oa.services.grantservice;
    var btnObj = new ButtonObject(service);
    btnObj.Init();

});

//按钮事件初始化
function ButtonObject(service) {
    return {
        Init:function () {
            this.InitSubmit();
            this.InitBindEvent();
            this.InitControl();
        },
        InitSubmit:function () {
            var $dialog = $('div.addbg');
            var $form = $dialog.find('form');
            //初始化页面上面的按钮事件
            $form.find('button[type="submit"]').click(function (e) {
                $form.validate();

                e.preventDefault();

                if (!$form.valid()) {
                    return;
                }
                var grant = $form.serializeFormToObject();
                service.addOrEdit(grant).done(function (data) {
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
                    f_chineseName: { required: true },
                    f_eid: { required:true},
                    f_amount: { number: true }
                  
                },
                messages: {
                    f_chineseName: {
                        required: "中文名/护照名/昵称不能为空！"
                    },
                    f_eid: {
                        required:"请选择员工！"
                    }
                }
            });
        },
        InitBindEvent:function () {
            //添加按钮
            $("#btn-add").click(function () {
                //重置表单信息
                $("#formTable").clearForm(true);
                //设置默认值
                $("#GrantDate").val(new Date().format("yyyy-MM-dd"));
                $('#GrantDate').datetimepicker('update');
                $($("#formTable input[type='radio']")[1]).prop("checked", true);
                $("#Amount").val(0);
                OpenADDwindow();
            });
            //导出按钮
            $("#a_export").click(function () {
                //查询条件
                var data = "Name=" + encodeURI($('#Name').val())
                    + "&PaymentStartDate=" + encodeURI($('#date_timepicker_start').val())
                    + "&PaymentEndDate=" + encodeURI($('#date_timepicker_end').val())
                    + "&IsPayment=" + encodeURI($('#IsPayment').val());
                //跳转页面(直接打开 Excel)
                location.href = '/Dormitory/Grant/ExportExcel?' + data;
            });
            $("#generateLastMonthDatas").click(function () {
                if (confirm("确定要生成上月数据吗?")) {
                    service.generateLastMonthDatas()
                        .done(function (data) {
                            if (data > 0) {
                                alert("成功添加数据" + data + "行！");
                                location.href = location.href;
                            } else {
                                alert("可添加数据行数为" + data);
                            }
                        });
                }
            });
            //绑定修改按钮事件
            var uBtnList = $("button[data-mark='update']");
            uBtnList.each(function () {
                var obj = $(this);
                var id = obj.attr("data-id");
                obj.click(function () {
                    clearErrorInfos();
                    service.getGrantById({
                        id: id
                    }).done(function (data) {
                        //初始化表单信息
                        OpenADDwindow();
                        var txtControls = $("#formTable input[type='text'],input[type='hidden']");
                        txtControls.each(function () {
                            var pName = $(this).attr("id");
                            $(this).val(data[pName]);
                        });
                        //是否缴费
                        if (data.IsPayment) {
                            $($("#formTable input[type='radio']")[0]).prop("checked", true);
                        } else {
                            $($("#formTable input[type='radio']")[1]).prop("checked", true);
                        }
                        //收费地点
                        $("#formTable .dropdown ul li a").each(function () {
                            var $a = $(this);
                            if ($a.attr("id") == data.AddressId) {
                                ($("#formTable .dropdown button")[0]).innerHTML = $(this).text() + "<span class='caret'></span>";
                            }
                        });
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
        //初始化日期控件
        InitControl:function () {
            $.datetimepicker.setLocale('zh');
            //搜索框日期
            $('#date_timepicker_start').datetimepicker({
                lang: 'zh',
                format: 'Y-m-d',
                onShow: function (ct) {
                    this.setOptions({
                        maxDate: $('#date_timepicker_end').val() ? $('#date_timepicker_end').val() : false
                    });
                },
                timepicker: false
            });
            $('#date_timepicker_end').datetimepicker({
                lang: 'zh',
                format: 'Y-m-d',
                onShow: function (ct) {
                    this.setOptions({
                        minDate: $('#date_timepicker_start').val() ? $('#date_timepicker_start').val() : false
                    });
                },
                timepicker: false
            });

            /*对话框日期*/
            //缴费日期
            $("#GrantDate").datetimepicker({
                yearOffset: 0,
                lang: 'zh',
                timepicker: false,
                format: 'Y-m-d',
                formatDate: 'Y-m-d'
            });
            var nameId = {}; //姓名对应的id
            $('#ChineseName').typeahead({
                source: function (query, process) {
                    service.getNames({
                        query: query
                    }).done(function (e) {
                        if (e.success) {
                            var array = [];
                            $.each(e.data, function (index, ele) {
                                nameId[ele.name] = ele.id; //键值对保存下来
                                array.push(ele.name);
                            });
                            process(array);
                        }
                    });
                },
                items: 8,
                afterSelect: function (item) {
                    $('#EId').val(nameId[item]);
                },
                delay: 500 //延迟时间

            });
        }
    };
};
