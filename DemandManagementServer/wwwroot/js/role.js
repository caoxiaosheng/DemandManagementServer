$(function () {
    $("#checkAll").click(function () { checkAll(this); });
    $("#btnAdd").click(function () { add(); });
    $("#btnSave").click(function () { save(); });
    $("#btnDelete").click(function () { deleteMulti(); });
    initTree();
    loadRoles(1, 15);
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
                "plugins": ["types", "wholerow", "checkbox",], //配置信息
                "checkbox": {
                    "keep_selected_style": false
                }
            });
            $("#menuTree").on("ready.jstree", function (e, data) {   //树创建完成事件
                data.instance.open_all();    //展开所有节点
            });
            initialRoleSelect(data);
        }
    });
}

//加载列表数据
function loadRoles(startPage, pageSize) {
    $("#tableBody").html("");
    $("#checkAll").prop("checked", false);
    $.ajax({
        type: "GET",
        url: "/Role/GetRoles?startPage=" + startPage + "&pageSize=" + pageSize,
        success: function (data) {
            $.each(data.roles,
                function (i, item) {
                    var tr = "<tr>";
                    tr += "<td align='center'><input type='checkbox' class='checkboxs' value='" + item.id + "'/></td>";
                    tr += "<td>" + item.name + "</td>";
                    tr += "<td>" + (item.remarks == null ? "" : item.remarks) + "</td>";
                    tr += "<td><button class='btn btn-info btn-xs' href='javascript:;' onclick='edit(\"" +
                        item.id +
                        "\")'><i class='fa fa-edit'></i> 编辑 </button> <button class='btn btn-danger btn-xs' href='javascript:;' onclick='deleteSingle(\"" +
                        item.id +
                        "\")'><i class='fa fa-trash-o'></i> 删除 </button> </td>";
                    tr += "</tr>";
                    $("#tableBody").append(tr);
                });
            var elment = $("#rolePagination"); //分页插件的容器id
            if (data.rowCount > 0) {
                var options = { //分页插件配置项
                    bootstrapMajorVersion: 3,
                    currentPage: startPage, //当前页
                    numberOfPages: data.rowsCount, //总数
                    totalPages: data.pageCount, //总页数
                    onPageChanged: function (event, oldPage, newPage) { //页面切换事件
                        loadRoles(newPage, pageSize);
                    }
                }
                elment.bootstrapPaginator(options); //分页插件初始化
            }
            $("table > tbody > tr").click(function () {
                $("table > tbody > tr").removeAttr("style");
                $(this).attr("style", "background-color:#beebff");
                var selectedRoleId = $(this).find("input").val();
                loadMenusByRoleId(selectedRoleId);
            });
        }
    });
}

//根据选中角色加载功能权限
function loadMenusByRoleId(selectedRoleId) {
    $.ajax({
        type: "Get",
        url: "/Role/GetMenuIdsByRoleId?roleId=" + selectedRoleId,
        success: function (data) {
            $("#menuTree").find("li").each(function () {
                if (data.indexOf(Number($(this).attr("id"))) === -1) {
                    $("#menuTree").jstree("uncheck_node", $(this));
                } else {
                    $("#menuTree").jstree("check_node", $(this));
                }
            });
        }
    });
};

function initialRoleSelect(data) {
    $("#Menu").select2();
    var option = "";
    $.each(data,
        function(i, item) {
            option += "<option value='" + item.id + "'>" + item.text + "</option>";
        });
    $("#Menu").html(option);
}

function checkAll(checkBox) {
    $(".checkboxs").each(function (index, elem) { $(elem).prop("checked", checkBox.checked) });
}

function add() {
    $("#Title").text("新增角色");
    $("#Action").val("AddRole");
    $("#Id").val(0);
    $("#Name").val("");
    $("#Menu").val(null).trigger("change");
    $("#Remarks").val("");
    //弹出新增窗体
    $("#addRole").modal("show");
}

function edit(id) {
    $("#Title").text("编辑角色");
    $("#Action").val("EditRole");
    $.ajax({
        type: "post",
        url: "/Role/GetRoleById?id=" + id,
        success: function (data) {
            $("#Id").val(data.id);
            $("#Name").val(data.name);
            $("#Remarks").val(data.remarks);
            if (data.roleMenus.length > 0) {
                var menuIds = [];
                $.each(data.roleMenus, function (i, item) {
                    menuIds.push(item.menuId.toString());
                });
                $("#Menu").val(menuIds).trigger('change');
            }
            //弹出新增窗体
            $("#addRole").modal("show");
        }
    });
}

function save() {
    var postData = {
        "roleViewModel": {
            "Id": $("#Id").val(),
            "Name": $("#Name").val(),
            "Remarks": $("#Remarks").val()
        },
        "menuIds": $("#Menu").val().toString()
    };
    $.ajax({
        type: "Post",
        url: "/Role/" + $("#Action").val(),
        data: postData,
        success: function (data) {
            if (data.result === true) {
                loadRoles(1, 15);
                $("#addRole").modal("hide");
            } else {
                layer.tips(data.reason, "#btnSave");
            };
        }
    });
}

function deleteSingle(id) {
    layer.confirm("是否删除",
        { btn: ["是", "否"] },
        function () {
            $.ajax({
                type: "Post",
                url: "/Role/DeleteSingle",
                data: { "id": id },
                success: function () {
                    loadRoles(1, 15);
                    layer.closeAll();
                }
            });
        });
};

function deleteMulti() {
    var ids = new Array();
    $(".checkboxs").each(function (index, elem) {
        if ($(elem).prop("checked") === true) {
            ids.push($(elem).val());
        }
    });
    if (ids.length === 0) {
        layer.alert("请先选择删除项");
        return;
    }
    layer.confirm("是否删除",
        { btn: ["是", "否"] },
        function () {
            $.ajax({
                type: "Post",
                url: "/Role/DeleteMulti",
                data: { "ids": ids },
                success: function () {
                    loadRoles(1, 15);
                    layer.closeAll();
                }
            });
        });
}