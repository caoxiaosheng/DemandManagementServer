$(function() {
    $("#checkAll").click(function () { checkAll(this); });
    $("#btnAdd").click(function () { add(); });
    $("#btnSave").click(function () { save(); });
    $("#btnDelete").click(function () { deleteMulti(); });
    initTree();
});

//加载树
function initTree() {
    $.jstree.destroy();
    $.ajax({
        type: "Get",
        url: "/Menu/GetAllMenusOfTree",    //获取数据的ajax请求地址
        success: function (data) {
            $('#menuTree').jstree({ //创建JsTtree
                'core': {
                    'data': data, //绑定JsTree数据
                    "multiple": true //是否多选
                },
                "plugins": ["state", "types", "wholerow", "checkbox",], //配置信息
                "checkbox": {
                    "keep_selected_style": false
                }
            });
            $("#menuTree").on("ready.jstree", function (e, data) {   //树创建完成事件
                data.instance.open_all();    //展开所有节点
            });
        }
    });
    
}

