$(function () {
    $("#checkAll").click(function () { checkAll(this); });
    $("#btnAdd").click(function () { add(); });
    $("#btnSave").click(function () { save(); });
    $("#btnDelete").click(function () { deleteMulti(); });
    $('.input-group.date').datepicker({
        todayBtn: "linked",
        clearBtn: true,
        language: "zh-CN",
        autoclose: true
    });
    loadSoftwareVersions(1, 15);
});

function loadSoftwareVersions(startPage, pageSize) {
    $("#tableBody").html("");
    $.ajax({
        type: "GET",
        url: "/SoftwareVersion/GetSoftwareVersions?startPage=" + startPage + "&pageSize=" + pageSize,
        success: function (data) {
            $.each(data.customers,
                function (i, item) {
                    var tr = "<tr>";
                    tr += "<td align='center'><input type='checkbox' class='checkboxs' value='" + item.id + "'/></td>";
                    tr += "<td>" + item.versionName + "</td>";
                    tr += "<td>" + item.expectedStartDate + "</td>";
                    tr += "<td>" + item.expectedEndDate + "</td>";
                    tr += "<td>" + item.expectedReleaseDate + "</td>";
                    tr += "<td>" + item.releaseDate + "</td>";
                    tr += "<td>" + (item.versionProgress === 0 ? "计划阶段" : (item.versionProgress === 1 ? "正在实施" : "已发布")) + "</td>";
                    tr += "<td>" + (item.isDeleted === 0 ? "<span class='badge badge-success'>正常</span>" : "<span class='badge badge-warning'>禁用</span>") + "</td>";
                    tr += "<td>" + (item.remarks == null ? "" : item.remarks) + "</td>";
                    tr += "<td><button class='btn btn-info btn-xs' href='javascript:;' onclick='edit(\"" +
                        item.id +
                        "\")'><i class='fa fa-edit'></i> 编辑 </button> <button class='btn btn-danger btn-xs' href='javascript:;' onclick='deleteSingle(\"" +
                        item.id +
                        "\")'><i class='fa fa-trash-o'></i> 删除 </button> </td>";
                    tr += "</tr>";
                    $("#tableBody").append(tr);
                });
            var elment = $("#pagination"); //分页插件的容器id
            if (data.rowCount > 0) {
                var options = { //分页插件配置项
                    bootstrapMajorVersion: 3,
                    currentPage: startPage, //当前页
                    numberOfPages: data.rowsCount, //总数
                    totalPages: data.pageCount, //总页数
                    onPageChanged: function (event, oldPage, newPage) { //页面切换事件
                        loadSoftwareVersions(newPage, pageSize);
                    }
                }
                elment.bootstrapPaginator(options); //分页插件初始化
            }
        }
    });
}

function add() {
    //$("#Title").text("新增角色");
    //$("#Action").val("AddRole");
    //$("#Id").val(0);
    //$("#Name").val("");
    //$("#Menu").val(null).trigger("change");
    //$("#Remarks").val("");
    //弹出新增窗体
    $("#addSoftwareVersion").modal("show");
}