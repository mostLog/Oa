$(function () {
    $(".nav li").click(function () {
        setBusy();
    });
});
function setBusy($model) {
    var loader = $("#loading");
    loader.css({ "display": "block" });
}
function clearBusy(ele) {
    $("#loading").css("display","none");
}
//设置dropdown 默认值
function setDropDownDefault(dropId) {
    $("#"+dropId+">button").html("请选择 <span class=\"caret\" ></span >");
}
