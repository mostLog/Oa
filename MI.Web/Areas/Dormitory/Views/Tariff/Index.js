$(function () {


    //获取服务
    var service = oa.services.tariffservice;

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
                loadingTips();
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
                    Community: {
                        required:true
                    },
                    Building: {
                        required:true
                    },
                    roomNo: {
                        required: true,
                        remote: {
                            url: "/Dormitory/Tariff/RoomIsExist",
                            type: "post",
                            data: {
                                community: function () {
                                    return $("#Community").val();
                                },
                                building: function () {
                                    return $("#Building").val();
                                }
                            },
                            dataFilter: function (d) {
                                var error = document.getElementById("RoomNoError");
                                $(error).remove();
                                if (d!=0) {
                                    $("#DormitoryId").val(d);
                                } else
                                {
                                    $("#RoomNo").after("<label id='RoomNoError' class='error' for='RoomNo'>当前房间号不存在！</label>");   
                                }
                            }
                        }
                    },
                    f_DormitoryId: {
                        required: true
                    },
                    f_TariffStandard: { number: true },
                    f_RealBill: { number: true },
                    f_Overruns:{number:true}
                },
                messages: {
                    Community: {
                        required:"请选择社区！"
                    },
                    Building: {
                        required:"请选择楼栋！"
                    },
                    roomNo: {
                        required: "请输入房间号！",
                        remote:"房间号不存在！"
                    },
                    f_DormitoryId: {
                        required: "房间号输入有误，请确认"
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
                $("#TariffDate").val(new Date().format("yyyy-MM-dd"));
                $('#TariffDate').datetimepicker('update');
                $($("#formTable input[type='radio']")[1]).prop("checked", true);
                $("#TariffStandard").val(0);
                $("#RealBill").val(0);
                $("#Overruns").val(0);
                OpenADDwindow();
            });
            //导出按钮
            $("#a_export").click(function () {
                //查询条件
                var data = "Community=" + encodeURI($('#sCommunity').val())
                    + "&Building=" + encodeURI($('#sBuilding').val())
                    + "&RoomNo=" + encodeURI($('#sRoomNo').val())
                    + "&TariffStartDate=" + encodeURI($('#date_timepicker_start').val())
                    + "&TariffEndDate=" + encodeURI($('#date_timepicker_end').val())
                    + "&IsPayment=" + encodeURI($('#IsPayment').val());
                //跳转页面(直接打开 Excel)
                location.href = '/Dormitory/Tariff/ExportExcel?' + data;
            });
            //绑定修改按钮事件
            var uBtnList = $("button[data-mark='update']");
            uBtnList.each(function () {
                var obj = $(this);
                var id = obj.attr("data-id");
                obj.click(function () {
                    clearErrorInfos();
                    service.getTariffById({
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
                        //resetCommunityDrop(service);
                        $("#BuildingLaundry").empty();
                        service.getBuildingsByCommunity({
                            community: data.Community
                        }).done(function (data) {
                            var shtml = "";
                            for (var i = 0; i < data.length; i++) {
                                shtml += "<li><a id='" + data[i] + "'>" + data[i] + "</a></li>"
                            }
                            $("#BuildingLaundry").html(shtml);
                            //重新绑定楼栋下拉事件
                            resetBuildingDrop();
                            ($("#formTable .dropdown button")[1]).innerHTML = $("#Building").val() + "<span class='caret'></span>";
                            
                        });
                        //收费地点
                        $("#formTable .dropdown ul li a").each(function () {
                            var $a = $(this);
                            if ($a.attr("id") == data.Community) {

                                ($("#formTable .dropdown button")[0]).innerHTML = $(this).text() + "<span class='caret'></span>";
                            }
                            
                            if ($a.attr("id") == data.AddressId) {

                                ($("#formTable .dropdown button")[2]).innerHTML = $(this).text() + "<span class='caret'></span>";
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
           
            resetCommunityDrop(service);
        },
        //初始化控件
        InitControl: function () {
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
            $('#TariffDate').datetimepicker({
                lang: 'zh',
                format: 'Y-m-d',
                timepicker: false
            });
            
        }
    };
}

function resetCommunityDrop(service) {
    //绑定社区下拉列表
    var communityList = $("#CommunityLaundry li a");
    console.log(communityList);
    //communityList.unbind("click");
    communityList.click(function () {
        var $this = $(this);
        var community = $this.attr("id");
        //清空子标签
        $("#BuildingLaundry").empty();
        service.getBuildingsByCommunity({
            community: community
        }).done(function (data) {
            var shtml = "";
            for (var i = 0; i < data.length; i++) {
                shtml += "<li><a id='" + data[i] + "'>" + data[i] + "</a></li>"
            }
            $("#BuildingLaundry").html(shtml);
            //setDropdownChangeValue();
            resetBuildingDrop();
        });
    });
}

function resetBuildingDrop()
{
    $("#BuildingLaundry li a").each(function () {
        if (this.id != null && this.id != "") {
            var v = $(this).parents(".dropdown-menu").attr("aria-labelledby");
            if (v != null) {
                ///先解绑事件，(防止重复绑定执行多次事件)
                $(this).unbind("click");
                $(this).click(function () {
                    $("input[type=hidden][correspond='" + v + "']").val(this.id);
                    $("#" + v + "").html($(this).text() + "<span class='caret'></span>");
                });
            }
        }
    });
}