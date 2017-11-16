$(function () {
    $("#checkAll").click(function () { checkAll(this); });
    $("#btnAdd").click(function () { add(); });
    $("#btnSave").click(function () { save(); });
    $("#btnDelete").click(function() { deleteMulti(); });
    loadMenus(1, 15);
});

function loadMenus(startPage, pageSize) {
    $("#tableBody").html("");
    $("#checkAll").prop("checkd",false);
    $.ajax({
        type: "GET",
        url: "/Menu/GetMenus?" +
            "startPage=" +
            startPage +
            "&pageSize=" +
            pageSize,
        success: function(data) {
            $.each(data.menus,
                function(i, item) {
                    var tr = "<tr>";
                    tr += "<td align='center'><input type='checkbox' class='checkboxs' value='" + item.id + "'/></td>";
                    tr += "<td>" + item.name + "</td>";
                    tr += "<td>" + (item.code == null ? "" : item.code) + "</td>";
                    tr += "<td>" + (item.url == null ? "" : item.url) + "</td>";
                    tr += "<td>" + "<i class='" + item.icon + "'></i>" + "</td>";
                    tr += "<td>" + (item.remarks == null ? "" : item.remarks) + "</td>";
                    tr += "<td><button class='btn btn-info btn-xs' href='javascript:;' onclick='edit(\"" +
                        item.id +
                        "\")'><i class='fa fa-edit'></i> 编辑 </button> <button class='btn btn-danger btn-xs' href='javascript:;' onclick='deleteSingle(\"" +
                        item.id +
                        "\")'><i class='fa fa-trash-o'></i> 删除 </button> </td>";
                    tr += "</tr>";
                    $("#tableBody").append(tr);
                });
            var elment = $("#menuPagination"); //分页插件的容器id
            if (data.rowsCount > 0) {
                var options = { //分页插件配置项
                    bootstrapMajorVersion: 3,
                    currentPage: startPage, //当前页
                    numberOfPages: data.rowsCount, //总数
                    totalPages: data.pageCount, //总页数
                    onPageChanged: function(event, oldPage, newPage) { //页面切换事件
                        loadMenus(newPage, pageSize);
                    }
                };
                elment.bootstrapPaginator(options); //分页插件初始化
            }
        }
    });
}

function checkAll(checkBox) {
    $(".checkboxs").each(function (index, elem) { $(elem).prop("checked", checkBox.checked) });
}

function add() {
    $("#Title").text("新增功能");
    $("#Action").val("AddMenu");
    $("#Code").val("");
    $("#Name").val("");
    $("#Url").val("");
    $("#Icon").val("");
    $("#Remarks").val("");
    //弹出新增窗体
    $("#addMenu").modal("show");
}

function edit(id) {
    $("#Title").text("编辑功能");
    $("#Action").val("EditMenu");
    $.ajax({
        type: "post",
        url: "/Menu/GetMenuById?id=" + id,
        success:function(data) {
            $("#Id").val(data.id);
            $("#Code").val(data.code);
            $("#Name").val(data.name);
            $("#Url").val(data.url);
            $("#Icon").val(data.icon);
            $("#Remarks").val(data.remarks);
            //弹出新增窗体
            $("#addMenu").modal("show");
        }
    });
}

function save() {
    var postData = {
        "menuViewModel": {
            "Id": $("#Id").val(),
            "Name": $("#Name").val(),
            "Code": $("#Code").val(),
            "Url": $("#Url").val(),
            "Icon": $("#Icon").val(),
            "Remarks": $("#Remarks").val()
        }
    };
    $.ajax({
        type: "Post",
        url: "/Menu/"+$("#Action").val(),
        data: postData,
        success: function (data) {
            if (data.result ===true) {
                loadMenus(1, 15);
                $("#addMenu").modal("hide");
            } else {
                layer.tips(data.reason, "#btnSave");
            };
        }
    });
}

function deleteSingle(id) {
    
}

function deleteMulti() {
    
}