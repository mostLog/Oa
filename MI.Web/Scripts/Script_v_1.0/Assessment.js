var allcheck = false;
//客服考核班次切换
$(function () {
    $('#periodDropDown').on('hidden.bs.dropdown', function () {
        getEmp();
    });
    $('#groupDropDown').on('hidden.bs.dropdown', function () {
        getEmp();
    });
    $('#directDropDown').on('hidden.bs.dropdown', function () {
        getEmp();
    });
    $('#queryYearDropdown').on('hidden.bs.dropdown', function () {
        $('.form-inline').submit();
    });
});
//全选按钮联动
$(function () {
    $("input[name='empIDs']").each(function (index, obj) {
        if (obj.id == 'allEmpCheckbox') {
            return true;
        }
        allcheck = $(obj).prop('checked');
        if (!allcheck) {
            return false;
        }
    });
    $('#allEmpCheckbox').prop('checked', allcheck);
    bindCheckbox();
});
$(function () {
    $("#btn-quxiao").click(function () {
        $("div.addbg").hide();
    });  //截止日期
    setHeightWidth();
});
function bindCheckbox() {
    $("input[name='empIDs']").click(function () {
        var allcheck;
        $("input[name='empIDs']").each(function (index, obj) {
            if (obj.id == 'allEmpCheckbox') {
                return true;
            }
            allcheck = $(obj).prop('checked');
            if (!allcheck) {
                return false;
            }
        });
        $('#allEmpCheckbox').prop('checked', allcheck);
    })
}
//全选按钮
function SelectAllCheckBox(checkbox) {
    var allcheck = $('#allEmpCheckbox').prop('checked');
    $("input[name='empIDs']").prop('checked', allcheck);
}
//查詢員工
function getEmp() {
    var periodId = $('#periodTypeId').val();
    var deptId = $('#departmentId').val();
    var groupId = $('#groupID').val();
    var directID = $('#directId').val();
    var sName = $('#sName').val();
    var isDirect = $('#isDirect').val();
    $.ajax({
        type: "POST",
        url: "/Assessment/Assessment/GetEmp",
        data: { groupId: groupId, periodId: periodId, deptId: deptId, directID: directID, isDirect: isDirect, sName: sName },
        success: function (data) {
            var emplist = JSON.parse(data);
            var html = '无员工';
            if (emplist.length != 0) {
                html = '<b>选择员工：</b> <label style="margin-right:10px;"><input type="checkbox" id="allEmpCheckbox" name="empIDs" value="0") onclick="SelectAllCheckBox(this)">全选</label>';
                for (var i = 0; i < emplist.length; i++) {
                    html += '<label style="margin-right:10px"><input type="checkbox" name="empIDs" value=" ' + emplist[i].f_eid + '">' + emplist[i].f_chineseName + '</label>';
                }
            }
            $('.checkbox').html(html);
            bindCheckbox();
        }
    });
}
function update(url,id, itemType, groupNum, queryYear) {
    OpenADDwindow();
    loadingTips();
    $.ajax({
        type: "POST",
        url: url,
        data: { eId: id, itemType: itemType, groupNum: groupNum, queryYear: queryYear },
        error: function (data) {
        },
        success: function (data) {
            if (data != null) {
                $(".addtable").html(data);
                //ajax请求的html 需要重新绑定下拉框的动作事件
                setDropdownChangeValue();
            }
        }
    });
}
function UpdatePercent(itemId) {
    if ($('#f_Remark').css("background-color") == 'rgb(255, 0, 0)') {
        alert("占比設置不正確");
        return;
    }
    var f_Remark = $('#f_Remark').val();
    var isDirect = $('#isDirect').val();
    $.ajax({
        type: "POST",
        url: '/Assessment/Assessment/UpdatePercent',
        data: { itemId: itemId, f_Remark: f_Remark, isDirect: isDirect },
        error: function (data) {
        },
        success: function (data) {
            if (data.result == 1) {
                alert("操作成功");
            } else {
                alert(data.tips);
            }
        }
    });
}
//验证只能输入小于100的【-】+数字
function checkValue(obj) {
    if (obj.value.indexOf(' ') != -1) {
        obj.value = obj.value.replace(' ', '');
    }
    if (obj.value.trim() == '') {
        $(obj).css("background-color", "");
        return;
    }
    var reg = new RegExp("^[\-]{0,1}[0-9]{1,3}[.]{0,1}[0-9]{0,2}$");
    if (!reg.test(obj.value) || obj.value.length > 6 || (obj.id == 'f_Remark' && obj.value.indexOf('-') != -1)) {
        $(obj).css("background-color", "red");
    } else {
        $(obj).css("background-color", "");
    }
}
//获取员工
function removeSelect(value){
    $("input[name='empIDs']").prop('checked', false);
    $("input[name='empIDs']").each(function (index, obj) {
        if (obj.id == 'allEmpCheckbox') {
            return true;
        }
        name = obj.parentNode.innerText;
        if (name == value)
        {
            $(obj).prop('checked', true);            
        }
    });
}
//客服考核导出
function Export(tableName) {
    //收集列头(属性名)数据
    var array = new Array();
    var table = $('#dataTable')[0];
    for (var i = 0; i < 1; i++) {
        for (var j = 0; j < table.rows[0].cells.length; j++) {
            var colName = table.rows[0].cells[j].innerText;
            array.push(colName);
        }
    }
    //收集数据
    var dataArray = new Array();
    for (var i = 1; i < table.rows.length; i++) {
        var value = {};
        for (var j = 0; j < table.rows[i].cells.length; j++) {
            if (table.rows[i].cells[j].innerText == undefined || table.rows[i].cells[j].innerText == "") {
                value[array[j]] = "";
            } else {
                value[array[j]] = table.rows[i].cells[j].innerText;
            }
        }
        dataArray.push(value);
    }

    var postUrl = "/Assessment/Assessment/Export";//提交地址
    var postData = JSON.stringify(dataArray);//第一个数据
    var postData2 = JSON.stringify(array);//第一个数据
    var ExportForm = document.createElement("FORM");
    document.body.appendChild(ExportForm);
    ExportForm.method = "POST";
    var newElement = document.createElement("input");
    newElement.setAttribute("name", "data");
    newElement.setAttribute("type", "hidden");
    ExportForm.appendChild(newElement);
    var newElement2 = document.createElement("input");
    newElement2.setAttribute("name", "colNames");
    newElement2.setAttribute("type", "hidden");
    ExportForm.appendChild(newElement2);
    var newElement3 = document.createElement("input");
    newElement3.setAttribute("name", "tableName");
    newElement3.setAttribute("type", "hidden");
    ExportForm.appendChild(newElement3);
    newElement3.value = tableName;
    newElement2.value = postData2;
    newElement.value = postData;
    ExportForm.action = postUrl;
    ExportForm.submit();
}