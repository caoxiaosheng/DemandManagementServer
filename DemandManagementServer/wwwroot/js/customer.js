$(function () {
    $("#CustomerType").select2({
        minimumResultsForSearch: Infinity
    });
    $("#CustomerPriority").select2({
        minimumResultsForSearch: Infinity
    });
    $("#checkAll").click(function () { checkAll(this); });
    $("#btnAdd").click(function () { add(); });
    $("#btnSave").click(function () { save(); });
    $("#btnDelete").click(function () { deleteMulti(); });
});

function loadCustomers(startPage, pageSize) {
    $("#tableBody").html("");
    $("#checkAll").prop("checked", false);
    $.ajax({
        type: "GET",
        url: "/Customer/GetCustomers?startPage=" + startPage + "&pageSize=" + pageSize,
        success: function (data) {
            $.each(data.roles,
                function (i, item) {
                    var tr = "<tr>";
                    tr += "<td align='center'><input type='checkbox' class='checkboxs' value='" + item.id + "'/></td>";
                    tr += "<td>" + item.name + "</td>";
                    tr += "<td>" + (item.customerType === 0 ? "免费用户" : "付费用户") + "</td>";
                    tr += "<td>" + (item.customerPriority === 0 ? "低" : (item.customerPriority === 1 ? "正常" :"高")) + "</td>";
                    tr += "<td>" + (item.customerState === 0 ? "<span class='badge badge-success'>正常</span>" : "<span class='badge badge-warning'>禁用</span>") + "</td>";
                    tr += "<td>" + (item.remarks == null ? "" : item.remarks) + "</td>";
                    tr += "<td><button class='btn btn-info btn-xs' href='javascript:;' onclick='edit(\"" +
                        item.id +
                        "\")'><i class='fa fa-edit'></i> 编辑 </button> <button class='btn btn-danger btn-xs' href='javascript:;' onclick='deleteSingle(\"" +
                        item.id +
                        "\")'><i class='fa fa-trash-o'></i> 删除 </button> </td>";
                    tr += "</tr>";
                    $("#tableBody").append(tr);
                });
            var elment = $("#customerPagination"); //分页插件的容器id
            if (data.rowCount > 0) {
                var options = { //分页插件配置项
                    bootstrapMajorVersion: 3,
                    currentPage: startPage, //当前页
                    numberOfPages: data.rowsCount, //总数
                    totalPages: data.pageCount, //总页数
                    onPageChanged: function (event, oldPage, newPage) { //页面切换事件
                        loadCustomers(newPage, pageSize);
                    }
                }
                elment.bootstrapPaginator(options); //分页插件初始化
            }
        }
    });
}

function checkAll(checkBox) {
    $(".checkboxs").each(function (index, elem) { $(elem).prop("checked", checkBox.checked) });
}

function add() {
    $("#Title").text("新增客户");
    $("#Action").val("AddCustomer");
    $("#Id").val(0);
    $("#Name").val("");
    $("#CustomerType").val("0").trigger("change");
    $("#CustomerPriority").val("1").trigger("change");
    $("#Remarks").val("");
    //弹出新增窗体
    $("#addCustomer").modal("show");
}
