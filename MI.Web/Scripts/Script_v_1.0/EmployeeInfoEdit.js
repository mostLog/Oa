//点击是否宿舍订餐
function chickIsOrderingEmployees(e) {
    if (e.value != "false") {
        if (e.id == "IsOrderingEmployeesTrue" && $("#EmployeeInfo_t_newEmployeeInfo_f_flightArrivalTime").val() == "")
        {
            alert("新人订餐需先选择航班抵达时间");
            $(e).prop("checked", "false");
            $("#IsOrderingEmployees").prop("checked", "true");
            $(".trOrderingEmployees").hide();
            return;
        }
        $(".trOrderingEmployees").show();
    } else {
        $(".trOrderingEmployees").hide();
    }
}
var pImgSrc = ""; //护照大图片
var eImgSrc = ""; //入境章大图片
var cImgSrc = ""; //证件照大图片

$(document).ready(function () {
    /*点击关闭大图片*/
    $(".closephoto").click(function () {
        $(".photobig-staff").hide();
    });

    //点击略缩图 显示加载/隐藏大图片
    $(".photos").click(function () {
        if ($(".photobig-staff").css("display") == "none") {
            $(".photobig-staff").show();
            var oldVal = $("#getImageForm #HZorRJZtype").val();
            if ($(this).hasClass("hz") && oldVal != "p") {
                SwitchingPorE("p");//切换为护照样式值
            } else if ($(this).hasClass("rjz") && oldVal != "e") {
                SwitchingPorE("e");//切换为入境章样式值
            } else if ($(this).hasClass("zjz") && oldVal != "c") {
                SwitchingPorE("c");//切换为证件照样式值
            }
            var newVal = $("#getImageForm #HZorRJZtype").val();
            //只加载一次
            if ($("#PassportImg").attr("src").indexOf("photo1.png") != -1
                || newVal != oldVal) {
                //显示 正在加载图片
                $("#PassportImg").attr({ "src": "/images/loading.gif" });
                if (newVal == "p" && pImgSrc != "") {
                    window.setTimeout("loadBigImg(pImgSrc)", 1000);//已有的直接显示(延迟1s,解决闪图)     
                    $("#repeat").show();
                    $("#downloadbtn").show();
                } else if (newVal == "e" && eImgSrc != "") {
                    window.setTimeout("loadBigImg(eImgSrc)", 1000);//已有的直接显示   
                    $("#repeat").show();
                    $("#downloadbtn").show();
                }
                else if (newVal == "c" && cImgSrc != "") {
                    window.setTimeout("loadBigImg(cImgSrc)", 1000);//已有的直接显示   
                    $("#repeat").show();
                    $("#downloadbtn").show();
                }
                else {
                    $("#getImageForm").ajaxSubmit({
                        dataType: "text",    // html（默认）、xml、script、json接受服务器端返回的类型
                        success: function (data) {
                            if (data != null && data != "") {
                                loadBigImg(data);
                                if (newVal == "p") {
                                    pImgSrc = data;
                                } else if (newVal == "e") {
                                    eImgSrc = data;
                                } else if (newVal == "c") {
                                    cImgSrc = data;
                                }
                            } else {
                                //alert("读取图片失败");
                                $("#PassportImg").attr({ "src": "" });
                            }
                        }    // 提交后的回调函数
                    });
                }
            }
        } else {
            $(".photobig-staff").hide();
        }
    });
    var jiaodu = 90;
    //旋转图片
    $("#repeat").click(function () {
        $(".photobig-staff").animate({ rotate: jiaodu });
        jiaodu = jiaodu + 90;
        if (jiaodu > 360) {
            jiaodu = 90;
        }
    });
    //选中图片提交
    $("#imgFile").change(function (e) {
        var $file = $(this);
        var fileObj = $file[0];
        var windowURL = window.URL || window.webkitURL;
        var dataURL;

        if (fileObj && fileObj.files && fileObj.files[0]) {
            dataURL = windowURL.createObjectURL(fileObj.files[0]);
        } else {
            dataURL = $file.val();
        }
        fileSumbit(dataURL);
    });
    //订餐选择按钮
    $("input[name='LDM_tID']").click(function () {
        if ($(this).prop("checked")) {            
            if ($(this).parent().find("input[name='LDM_Quantity']").val() == 0) {
                $(this).parent().find("input[name='LDM_Quantity']").val(1);
            }
            $(this).parent().find("input[name='LDM_Quantity']").prop("disabled", false);
            $(this).parent().find(".jiahaoBtn,.jianhaoBtn").prop("disabled", false);
            index = $(this).attr("index");
            date = $("#observationDate" + index).text();
            html = "<input type='hidden' value='" + date + "' name='observationDate'/>";
            $(this.parentElement).append(html);
        } else {            
            $(this).parent().find("input[name='LDM_Quantity']").val(0);
            $(this).parent().find("input[name='LDM_Quantity']").prop("disabled", true);
            $(this).parent().find(".jiahaoBtn,.jianhaoBtn").prop("disabled", true);
            index = $(this).attr("index");
            date = $("#observationDate" + index).text();
            $(this.parentElement).find("input[name='observationDate']").remove();
        }
    });

    //订餐份量加号按钮
    $(".jiahaoBtn").click(function () {
        if ($(this).parent().find("input[name='LDM_Quantity']").prop("disabled") == true) {
            return false;
        }
        var oV = $(this).parent().find("input[name='LDM_Quantity']").val();
        if (!isNaN(oV)) {
            oV = parseInt(oV, 10)
            $(this).parent().find("input[name='LDM_Quantity']").val(oV == 99 ? 99 : oV + 1);
        } else {
            $(this).parent().find("input[name='LDM_Quantity']").val(1);
        }
    });
    //订餐份量加号按钮
    $(".jianhaoBtn").click(function () {
        if ($(this).parent().find("input[name='LDM_Quantity']").prop("disabled") == true) {
            return false;
        }
        var oV = $(this).parent().find("input[name='LDM_Quantity']").val();
        if (!isNaN(oV)) {
            oV = parseInt(oV, 10)
            $(this).parent().find("input[name='LDM_Quantity']").val(oV == 0 ? 0 : oV - 1);
        } else {
            $(this).parent().find("input[name='LDM_Quantity']").val(0);
        }
    });
});

//显示文件框
function showFile(t) {
    SwitchingPorE(t);
    $("#imgFile").click();
}
//上传图片
function fileSumbit(objUrl) {
    $("#PassportImg").attr({ "src": "/images/loading.gif" });
    var t = $("#imgFileForm input[name='HZorRJZtype']").val();
    var imgSize = document.getElementById("imgFile").files[0].size;
    if (parseInt(imgSize / 1024 / 1204) < 6) {
        $("#imgFileForm").ajaxSubmit({
            dataType: "json",    // html（默认）、xml、script、json接受服务器端返回的类型
            success: function (data) {
                if (data == "5") {
                    alert("图片格式只能为png或jpg/jpeg");
                } else if (data == "7") {
                    alert("图片宽高不可超过1920*1080");
                } else if (data != null && data != "" && data != "500") {
                    $("#PassportImg").attr({ "src": objUrl });
                    $("#repeat").show();
                    $("#downloadbtn").show();
                    SwitchImgSuccess(t, data, objUrl);
                } else {
                    alert("上传失败");
                    $("#PassportImg").attr({ "src": "" });
                }
            }    // 提交后的回调函数
        });
    } else
    {
        alert('图片大小不能超过6M');
    }
   
}
//提交表单
function btnSumbit() {
    debugger
    ///修正上下班时间 忽略分钟，只取小时。日期年月日忽略不用
    var strTime1 = "2016/6/14 " + $.trim($("#EmployeeInfo_f_rideWorkTime").val());
    var strTime2 = "2016/6/14 " + $.trim($("#EmployeeInfo_f_rideOffWorkTime").val());
    var date1 = new Date(strTime1);
    if (date1.getMinutes() > 0) {
        $("#EmployeeInfo_f_rideWorkTime").val(date1.getHours() + ":00");
    }

    var date2 = new Date(strTime2);
    if (date2.getMinutes() > 0) {
        $("#EmployeeInfo_f_rideOffWorkTime").val(date2.getHours() + ":00");
    }

    $("#employeeInfoform").ajaxSubmit(options);
}
var options = {
    beforeSubmit: showRequest,    // 提交前的回调函数
    success: showResponse,    // 提交后的回调函数
    dataType: "json",    // html（默认）、xml、script、json接受服务器端返回的类型
    timeout: 3000    // 限制请求的时间，当请求大于3秒后，跳出请求
}
function showRequest(formData, jqForm, options) {
    if ($.trim($("#dropdownMenu7").text()) == "離職" || $.trim($("#dropdownMenu7").text()) == "离职") {
        if (ValidationNull("离职状态下，离职日期", $.trim($("#EmployeeInfo_f_SeparationDate").val()))) {
            return false;
        }
    }
    var reg = new RegExp("^[0-9]{0,13}[.]{0,1}[0-9]{0,2}$");
    if (!reg.test($("#EmployeeInfo_f_Salary").val()))
    {
        alert("菲员工的薪资输入不正确");
        return false;
    } 
    if ($("#IsOrderingEmployeesTrue").prop("checked") && (($("#onchangeCommunity").val() == "" || $("#onchangeCommunity").val() == "0" || $("#onchangeCommunity").val() == "请选择社区")
        || ($("#onchangeBuilding").val() == "" || $("#onchangeBuilding").val() == "0" || $("#onchangeBuilding").val() == "请先选择社区")
        || ($("#onchangeRoomNo").val() == "" || $("#onchangeRoomNo").val() == "0" || $("#onchangeRoomNo").val() == "请先选择楼栋") || $("#EmployeeInfo_f_DormitoryId").val() == ""))
    {
        alert("若要订餐，必须选择社区、楼栋、房间号");
        return false;
    }
    var newData = $("#employeeInfoform").formSerialize();
    formData = newData;
    return true; // 只要不返回false，表单都会提交，在这里可以对表单元素进行验证 EmployeeInfo
}
function showResponse(responseText, statusText) {
    debugger
    if (responseText.result == 1) {
        if (booIsCreate && confirm("添加成功,是否继续添加?")) {
            $("#employeeInfoform").resetForm();
            ResetPassportImage();
            $(".trOrderingEmployees").hide();
            $("#btn-quxiao").click(function () {
                closeADDwindow();
                RefreshF5();
            });
        }
        else {
            if (!booIsCreate) {
                alert(responseText.tips);
            }
            RefreshF5();
        }
    }
    else {
        alert(responseText.tips);
    }

}
//新增继续的时候 重置护照图片信息
function ResetPassportImage() {
    $("#EmployeeInfo_f_PassportURL,#EmployeeInfo_f_EntrystampURL,#EmployeeInfo_f_IDCardURL").val("");//清空护照url值
    $(".photo-add").show();//新增图片显示
    $(".photos").hide();//修改图片隐藏
    $("#PassportImg").attr("src", "~/images/photo1.png");//图片改为默认
    $(".downloadbtn").hide();//下载按钮隐藏
    $("#repeat").hide();//旋转按钮隐藏
    var timestamp = new Date().getTime();
    $("#imgFileForm").find("input[name='imgName']").val(timestamp + "x");
}
//加载大图片
function loadBigImg(src) {
    $("#PassportImg").attr("src", src);
    $(".downloadbtn").show();
    $("#repeat").show();
}
//护照 和入境章 上传图片成功之后切换
function SwitchImgSuccess(t, data, obj) {
    debugger;
    if (t == "e") {
        $(".photo-staff .rjz").css({ "background-image": "url('" + obj + "')", "background-size": "cover" }).show();
        $("#EmployeeInfo_f_EntrystampURL").val(data);
        $(".photo-staff .rjz").parent().find(".photo-add").hide();
        eImgSrc = obj;
    } else if (t == "p") {
        $(".photo-staff .hz").css({ "background-image": "url('" + obj + "')", "background-size": "cover" }).show();
        $("#EmployeeInfo_f_PassportURL").val(data);
        $(".photo-staff .hz").parent().find(".photo-add").hide();
        pImgSrc = obj;
    } else if (t == "c") {
        $(".photo-staff .zjz").css({ "background-image": "url('" + obj + "')", "background-size": "cover" }).show();
        $("#EmployeeInfo_f_IDCardURL").val(data);
        $(".photo-staff .zjz").parent().find(".photo-add").hide();
        cImgSrc = obj;
    }
}
//护照 和入境章 样式/值切换
function SwitchingPorE(t) {
    $(".photobig-staff").css({ "top": "200px", "left": "auto" });   //初始位置
    var timestamp = new Date().getTime();
    if (t == "e") {
        $(".downloadbtn").attr("href", xiazURL + "&t=e&n=" + timestamp);
        $("input[name='HZorRJZtype']").val("e");
    } else if (t == "p") {
        $(".downloadbtn").attr("href", xiazURL + "&t=p&n=" + timestamp);
        $("input[name='HZorRJZtype']").val("p");
    } else if (t == "c") {
        $(".downloadbtn").attr("href", xiazURL + "&t=c&n=" + timestamp);
        $("input[name='HZorRJZtype']").val("c");
    }
    $(".downloadbtn").hide();//下载按钮隐藏
    $("#repeat").hide(); //旋转按钮隐藏
}