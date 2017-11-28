$(function () {
    $("#checkAll").click(function () { checkAll(this); });
    $("#btnAdd").click(function () { add(); });
    $("#btnSave").click(function () { save(); });
    $("#btnDelete").click(function () { deleteMulti(); });
    loadUsers(1, 15);
    initialRoleSelect();
});

function loadUsers(startPage, pageSize) {
    $("#tableBody").html("");
    $("#checkAll").prop("checked", false);
    $.ajax({
        type: "GET",
        url: "/User/GetUsers?" +
        "startPage=" +
        startPage +
        "&pageSize=" +
        pageSize,
        success: function (data) {
            $.each(data.users,
                function (i, item) {
                    var tr = "<tr>";
                    tr += "<td align='center'><input type='checkbox' class='checkboxs' value='" + item.id + "'/></td>";
                    tr += "<td>" + item.userName + "</td>";
                    tr += "<td>" + (item.name == null ? "" : item.name) + "</td>";
                    tr += "<td>" + (item.email == null ? "" : item.email) + "</td>";
                    tr += "<td>" + (item.mobileNumber == null ? "" : item.mobileNumber) + "</td>";
                    tr += "<td>" + (item.remarks == null ? "" : item.remarks) + "</td>";
                    tr += "<td>" + (item.roles == null ? "" : item.roles) + "</td>";
                    tr += "<td>" + (item.lastLoginTime == null ? "" : item.lastLoginTime) + "</td>";
                    tr += "<td>" + (item.isDeleted === 0 ? "<span class='badge badge-success'>正常</span>" : "<span class='badge badge-warning'>禁用</span>") + "</td>";
                    tr += "<td><button class='btn btn-info btn-xs' href='javascript:;' onclick='edit(\"" +
                        item.id +
                        "\")'><i class='fa fa-edit'></i> 编辑 </button> <button class='btn btn-danger btn-xs' href='javascript:;' onclick='deleteSingle(\"" +
                        item.id +
                        "\")'><i class='fa fa-trash-o'></i> 删除 </button> </td>";
                    tr += "</tr>";
                    $("#tableBody").append(tr);
                });
            var elment = $("#userPagination"); //分页插件的容器id
            if (data.rowsCount > 0) {
                var options = { //分页插件配置项
                    bootstrapMajorVersion: 3,
                    currentPage: startPage, //当前页
                    numberOfPages: data.rowsCount, //总数
                    totalPages: data.pageCount, //总页数
                    onPageChanged: function (event, oldPage, newPage) { //页面切换事件
                        loadUsers(newPage, pageSize);
                    }
                };
                elment.bootstrapPaginator(options); //分页插件初始化
            }
        }
    });
}

function initialRoleSelect() {
    $.ajax({
        type: "Get",
        url: "/Role/GetAllRoles",    //获取数据的ajax请求地址
        success: function (data) {
            $("#Role").select2();
            var option = "";
            $.each(data,
                function (i, item) {
                    option += "<option value='" + item.id + "'>" + item.name + "</option>";
                });
            $("#Role").html(option);
        }
    });
}

function checkAll(checkBox) {
    $(".checkboxs").each(function (index, elem) { $(elem).prop("checked", checkBox.checked) });
}

function add() {
    $("#Title").text("新增用户");
    $("#Action").val("AddUser");
    $("#Id").val(0);
    $("#UserName").val("");
    $("#Name").val("");
    $("#Email").val("");
    $("#MobileNumber").val("");
    $("#Role").val(null).trigger("change");
    $("#Remarks").val("");
    //弹出新增窗体
    $("#addUser").modal("show");
}

function edit(id) {
    $("#Title").text("编辑用户");
    $("#Action").val("EditUser");
    $.ajax({
        type: "post",
        url: "/User/GetUserById?id=" + id,
        success: function (data) {
            $("#Id").val(data.id);
            $("#UserName").val(data.userName);
            $("#Name").val(data.name);
            $("#Email").val(data.email);
            $("#MobileNumber").val(data.mobileNumber);
            $("#Remarks").val(data.remarks);
            if (data.userRoles.length > 0) {
                var roleIds = [];
                $.each(data.userRoles, function (i, item) {
                    roleIds.push(item.roleId.toString());
                });
                $("#Role").val(roleIds).trigger('change');
            }
            //弹出新增窗体
            $("#addUser").modal("show");
        }
    });
}

function save() {
    var postData = {
        "userViewModel": {
            "Id": $("#Id").val(),
            "UserName": $("#UserName").val(),
            "Name": $("#Name").val(),
            "Email": $("#Email").val(),
            "MobileNumber": $("#MobileNumber").val(),
            "Remarks": $("#Remarks").val()
        },
        "roleIds": $("#Role").val().toString()
    };
    $.ajax({
        type: "Post",
        url: "/User/" + $("#Action").val(),
        data: postData,
        success: function (data) {
            if (data.result === true) {
                loadUsers(1, 15);
                $("#addUser").modal("hide");
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
                url: "/User/DeleteSingle",
                data: { "id": id },
                success: function () {
                    loadUsers(1, 15);
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
                url: "/User/DeleteMulti",
                data: { "ids": ids },
                success: function () {
                    loadUsers(1, 15);
                    layer.closeAll();
                }
            });
        });
}