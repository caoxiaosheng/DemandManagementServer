﻿$(function () {
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
    loadCustomers(1, 15);
});

function loadCustomers(startPage, pageSize) {
    $("#tableBody").html("");
    $("#checkAll").prop("checked", false);
    $.ajax({
        type: "GET",
        url: "/Customer/GetCustomers?startPage=" + startPage + "&pageSize=" + pageSize,
        success: function (data) {
            $.each(data.customers,
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

function edit(id) {
    $("#Title").text("编辑角色");
    $("#Action").val("EditCustomer");
    $.ajax({
        type: "post",
        url: "/Customer/GetCustomerById?id=" + id,
        success: function (data) {
            $("#Id").val(data.id);
            $("#Name").val(data.name);
            $("#CustomerType").val(data.customerType).trigger('change');
            $("#CustomerPriority").val(data.customerPriority).trigger('change');
            $("#Remarks").val(data.remarks);
            //弹出新增窗体
            $("#addCustomer").modal("show");
        }
    });
}

function save() {
    var postData = {
        "customerViewModel": {
            "Id": $("#Id").val(),
            "Name": $("#Name").val(),
            "CustomerType": $("#CustomerType").select2("val"),
            "CustomerPriority": $("#CustomerPriority").select2("val"),
            "Remarks": $("#Remarks").val()
        }
    };
    $.ajax({
        type: "Post",
        url: "/Customer/" + $("#Action").val(),
        data: postData,
        success: function (data) {
            if (data.result === true) {
                loadCustomers(1, 15);
                $("#addCustomer").modal("hide");
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
                url: "/Customer/DeleteSingle",
                data: { "id": id },
                success: function () {
                    loadCustomers(1, 15);
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
                url: "/Customer/DeleteMulti",
                data: { "ids": ids },
                success: function () {
                    loadCustomers(1, 15);
                    layer.closeAll();
                }
            });
        });
}