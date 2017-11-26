$(function () {
    $("#checkAll").click(function () { checkAll(this); });
    $("#btnAdd").click(function () { add(); });
    $("#btnSave").click(function () { save(); });
    $("#btnDelete").click(function () { deleteMulti(); });
    loadUsers(1, 15);
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