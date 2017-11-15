$(function() {
    $("#btnAdd").click(add());
    loadMenus(1, 10);
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

function add() {
    
}