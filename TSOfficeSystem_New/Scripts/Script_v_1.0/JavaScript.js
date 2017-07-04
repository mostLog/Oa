//获取右边窗口高度 --------------start
var winHeight = "";
var bgHeight = ""; //背景遮布高度
var winWidth = "";
var rightw = "";
var clientHeight = "";
//导航栏隐藏及显示JS
$(document).ready(function () {     
    $('[data-toggle="offcanvas"]').click(function () {
        $('#wrapper').toggleClass('toggled');
    });
});
//到这里结束
$(document).ready(function () {
    setHeightWidth();
    bindClickEvent();
    //解決ie9下 disabled之後可以選中
    $("input[type='text']:disabled").on("mouseup", function (evt) {
        if ($(this).attr("disabled") == true) {
            this.blur();
        }
    });
    //设置左边菜单栏选中正确的位置
    setLeftTonavnurrent();
    //设置下拉框事件
    setDropdownChangeValue();
    //设置自动固定表头
    autoSetFixedTop();
    //如果是出勤列表，就设置宽度。(特殊竖着的表)
    if (window.isCqlb) {
        SetCQLBWidth();
    }

    //设置form表单不保存文本框历史记录
    setAutocompleteTOoff();
});

$(document).resize(function () {
    setHeightWidth();
});

function CssParseInt(td, css) {
    return parseInt($(td).css(css), 10);
}

//auto-fixed-top
function autoSetFixedTop() {
    if ($("#auto-fixed-top").length == 0) {
        return;
    }
    ///需要固定的元素 到顶部的距离 - Top页面的高度
    var tH = $("#auto-fixed-top").offset().top - 55;
    //存储原本的宽
    var arrTdW = new Array();
    $("#auto-fixed-top").find("th").each(function (i, td) {
        var c = CssParseInt(td, "border-left-width") + CssParseInt(td, "border-right-width") + CssParseInt(td, "padding-left") + CssParseInt(td, "padding-right") + CssParseInt(td, "margin-left") + CssParseInt(td, "margin-right");
        arrTdW[i] = $(td).width();
    });
    $("#auto-fixed-top2").css({
        "position": "fixed", "top": "55px", "z-index": "88"
    });
    $(document).scroll(function () {
        var scroH = $(this).scrollTop();
        if (scroH >= tH) {
            $("#auto-fixed-top2").show();
            $("#auto-fixed-top2").find("th").each(function (i, td) {
                $(td).width(arrTdW[i]).css("z-index", "88");
            });

        } else if (scroH < tH) {
            $("#auto-fixed-top2").hide();
        }

    });
}

function setHeightWidth() {
    winWidth = $(window).width();
    $(".loginbg").css("width", winWidth);
    //获取窗口高度Height  --start
    winHeight = $(window).height();
    clientHeight = document.body.clientHeight;
    bgHeight = clientHeight > winHeight ? clientHeight : winHeight;
    $(".addbg").css("height", bgHeight);
    $(".sidebar").css("height", winHeight);
    $(".loginbg").css("height", winHeight);


}
function setLeftTonavnurrent() {
    $(".nav li").removeClass("navcurrent");
    if (current) {
        $(".nav li").each(function (i) {
            if ($(this).text().indexOf(current) > 0) {
                $(this).addClass("navcurrent");
            }
        });
    }
}
//日期加上天数得到新的日期  
//dateTemp 需要参加计算的日期，days要添加的天数，返回新的日期，日期格式：YYYY-MM-DD  
function getNewDay(dateTemp, days) {   
    var millSeconds = Math.abs(dateTemp) + (days * 24 * 60 * 60 * 1000);
    var rDate = new Date(millSeconds);
    var year = rDate.getFullYear();
    var month = rDate.getMonth() + 1;
    if (month < 10) month = "0" + month;
    var date = rDate.getDate();
    if (date < 10) date = "0" + date;
    return (year + "-" + month + "-" + date);
}
/** 
 * 时间对象的格式化; 
 */
Date.prototype.format = function (format) {
    /* 
     * eg:format="yyyy-MM-dd hh:mm:ss"; 
     */
    var o = {
        "M+": this.getMonth() + 1, // month  
        "d+": this.getDate(), // day  
        "h+": this.getHours(), // hour  
        "m+": this.getMinutes(), // minute  
        "s+": this.getSeconds(), // second  
        "q+": Math.floor((this.getMonth() + 3) / 3), // quarter  
        "S": this.getMilliseconds()
        // millisecond  
    }

    if (/(y+)/.test(format)) {
        format = format.replace(RegExp.$1, (this.getFullYear() + "").substr(4
                        - RegExp.$1.length));
    }

    for (var k in o) {
        if (new RegExp("(" + k + ")").test(format)) {
            format = format.replace(RegExp.$1, RegExp.$1.length == 1
                            ? o[k]
                            : ("00" + o[k]).substr(("" + o[k]).length));
        }
    }
    return format;
}
function setDropdownChangeValue() {
    $(".dropdown-menu a").each(function () {
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
function bindClickEvent() {
    $("#btn-xinzeng").unbind("click");
    $("#btn-xinzeng").click(function () {
        $("div.addbg").show(200);
    });
    $("#btn-quxiao").unbind("click");
    $("#btn-quxiao").click(function () {
        $("div.addbg").hide();
    });

    $("#caidan").unbind("click");
    $("#caidan").click(function () {
        if ($(".sidebar").css("display") == "none") {
            $(".sidebar").show(200);
            $(this).find("span").removeClass("glyphicon-list").addClass("glyphicon-transfer");
        } else {
            $(".sidebar").hide();
            $(this).find("span").removeClass("glyphicon-transfer").addClass("glyphicon-list");
        }
    });
    $("#searchicon").unbind("click");
    $("#searchicon").click(function () {
        if ($("#searchbox").css("display") == "none") {
            $("#searchbox").show(200);
            $(this).find("span").removeClass("glyphicon-search").addClass("glyphicon-remove-circle");
        } else {
            $("#searchbox").hide();
            $(this).find("span").removeClass("glyphicon-remove-circle").addClass("glyphicon-search");
        }
    });

    $("#xiala").unbind("click");
    $("#xiala").click(function () {
        if ($("#navtop").css("display") == "none") {
            $("#navtop").show(200);
            $(this).find("span").removeClass("glyphicon-chevron-down").addClass("glyphicon-chevron-up");
        } else {
            $("#navtop").hide();
            $(this).find("span").removeClass("glyphicon-chevron-up").addClass("glyphicon-chevron-down");
        }
    });
}

function closeADDwindow() {
    $("div.addbg").hide();
}
function loadingTips() {
    $("div.addtable").html("<div class='loader'>Loading...</div>");
}
function OpenADDwindow() {
    $('html,body').animate({ scrollTop: '0px' }, 100);
    $("div.addbg").show(200);
}

function RefreshF5() {
    $("div.addtable").html("<div class='loader'>Loading...</div>");
    $("div.addbg").show();
    document.location = document.location;
}

function setAutocompleteTOoff() {
    $("form").attr("autocomplete", "off");
}
//验证只能输入汉字
function checkCC(obj) {
    var t = obj.value.replace(/[^\u4E00-\u9FA5]/g, "");
    if (obj.value != t) {
        obj.value = t;
    }
}

//验证只能输入字母+ 【-】
function checkABC(obj) {
    var t = obj.value.replace(/[^A-Za-z-,]/g, "");
    if (obj.value != t) {
        obj.valu = t;
    }
}
//验证只能输入字母+ 【-】+数字
function checkABC123(obj) {
    var t = obj.value.replace(/[^A-Za-z0-9-]/g, "");
    if (obj.value != t) {
        obj.value = t;
    }
}

//验证只能输入【-】+数字
function check123(obj) {
    var t = obj.value.replace(/[^0-9-.]/g, "");
    if (obj.value != t) {
        obj.value = t;
    }
}
//验证号码(简单)
function checknumber(obj) {
    var t = obj.value.replace(/[^0-9-+]/g, "");
    if (obj.value != t) {
        obj.value = t;
    }
}
//验证日期(简单)
function checkDatetime(obj) {
    var t = obj.value.replace(/[^0-9:：-]/g, "");
    if (obj.value != t) {
        obj.value = t;
    }
}
//指定项，需要验证的字符串，指定长度//验证字符串长度
function ValidationLength(sItemName, sChar, iLength) {
    var bIsTrue = false;
    if (sChar) {
        bIsTrue = sChar.length > iLength;
    }
    if (bIsTrue) {
        alert(sItemName + "    不能超过" + iLength + "" + "个字");
    }
    return bIsTrue;
}
//指定项，需要验证的字符串/验证字符串是否为空
function ValidationNull(sItemName, sChar) {
    var bIsTrue = false;
    if (!sChar) {
        bIsTrue = true;
        alert(sItemName + "不能为空！");
    }
    return bIsTrue;
}
//指定项，需要验证的字符串/验证字符串是否为空或者为0
function ValidationNullOr0(sItemName, sChar, sTips) {
    var bIsTrue = false;
    if ((!sChar) || sChar == "0") {
        bIsTrue = true;
        alert(sItemName + (sTips == undefined ? "不能为空！" : sTips));
    }
    return bIsTrue;
}
//指定项，需要验证的字符串/验证金额输入是否正确
function ValidationMoney(sItemName, sChar) {
    var bIsTrue = false;
    var reg = new RegExp("^[0-9]{0,9}$");
    if (!reg.test(sChar)) {
        alert(sItemName + "只能输入最多9位数字的整数");
        bIsTrue = true;
    }
    return bIsTrue;
}


function SetCQLBWidth() {
    //设置出勤列表中的宽
    $(".oacentbox").width($("#divcqlb")[0].offsetWidth + 350);
}
//切换宿舍管理 房间号文本框样式
function RoomNoTextSwitchStyle(data)
{
    if (data == 0)
    {
        $("#T-div").attr("class", "form-group has-error has-feedback");
        $("#Ticon").attr("class", "glyphicon glyphicon-remove form-control-feedback");
    }
    else
    {
        $("#T-div").attr("class", "form-group has-success has-feedback");
        $("#Ticon").attr("class", "glyphicon glyphicon-ok form-control-feedback");
    }

}