$(function () {
    $("#VersionProgress").select2({
        minimumResultsForSearch: Infinity
    });
    $("#btnAdd").click(function () { add(); });
    $("#btnSave").click(function () { save(); });
    $('.datepicker').datepicker({
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
            $.each(data.softwareVersions,
                function (i, item) {
                    var tr = "<tr>";
                    tr += "<td>" + item.versionName + "</td>";
                    tr += "<td>" + item.expectedStartDate + "</td>";
                    tr += "<td>" + item.expectedEndDate + "</td>";
                    tr += "<td>" + item.expectedReleaseDate + "</td>";
                    tr += "<td>" + (item.versionProgress === 2 ? item.releaseDate:"") + "</td>";
                    tr += "<td>" + (item.versionProgress === 0 ? "计划阶段" : (item.versionProgress === 1 ? "正在实施" : "已发布")) + "</td>";
                    tr += "<td>" + (item.isDeleted === 0 ? "<span class='badge badge-success'>正常</span>" : "<span class='badge badge-warning'>禁用</span>") + "</td>";
                    tr += "<td>" + (item.remarks == null ? "" : item.remarks) + "</td>";
                    var editHtml = "<button class='btn btn-info btn-xs' href='javascript:;' onclick='edit(\"" +
                        item.id +
                        "\")'><i class='fa fa-edit'></i> 编辑 </button>";
                    var deleteHtml =
                        "<button class='btn btn-danger btn-xs' href='javascript:;' onclick='deleteSingle(\"" +
                            item.id +
                        "\")'><i class='fa fa-trash-o'></i> 删除 </button>";
                    var releaseHtml = "<button class='btn btn-success btn-xs' href='javascript:;' onclick='release(\"" +
                        item.id +
                        "\")'><i class='fa fa-send'></i> 发布 </button>";
                    tr += "<td>" + (item.versionProgress === 2 ? deleteHtml : editHtml + deleteHtml + releaseHtml)+ "</td>";
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
    $("#Title").text("新增版本");
    $("#Action").val("AddSoftwareVersion");
    $("#Id").val(0);
    $("#VersionName").val("");
    $(".datepicker").datepicker('clearDates');
    $("#VersionProgress").val(0).trigger("change");
    $("#Remarks").val("");
    //弹出新增窗体
    $("#addSoftwareVersion").modal("show");
}

function edit(id) {
    $("#Title").text("编辑版本");
    $("#Action").val("EditSoftwareVersion");
    $.ajax({
        type: "post",
        url: "/SoftwareVersion/GetSoftwareVersionById?id=" + id,
        success: function (data) {
            $("#Id").val(data.id);
            $("#VersionName").val(data.versionName);
            $("#ExpectedStartDate").datepicker('setDate', data.expectedStartDate);
            $("#ExpectedEndDate").datepicker('setDate', data.expectedEndDate);
            $("#ExpectedReleaseDate").datepicker('setDate', data.expectedReleaseDate);
            //$("#ReleaseDate").datepicker('setDate', data.releaseDate);
            $("#VersionProgress").val(data.versionProgress).trigger('change');
            $("#Remarks").val(data.remarks);
            //弹出新增窗体
            $("#addSoftwareVersion").modal("show");
        }
    });
}

function save() {
    var postData = {
        "softwareVersionView": {
            "Id": $("#Id").val(),
            "VersionName": $("#VersionName").val(),
            "ExpectedStartDate": $("#ExpectedStartDate").datepicker('getDate') == null ? "" : $("#ExpectedStartDate").datepicker('getDate').toLocaleDateString(),
            "ExpectedEndDate": $("#ExpectedEndDate").datepicker('getDate') == null ? "" : $("#ExpectedEndDate").datepicker('getDate').toLocaleDateString(),
            "ExpectedReleaseDate": $("#ExpectedReleaseDate").datepicker('getDate') == null ? "" : $("#ExpectedReleaseDate").datepicker('getDate').toLocaleDateString(),
            //"ReleaseDate": $("#ReleaseDate").datepicker('getDate') == null ? "" : $("#ReleaseDate").datepicker('getDate').toLocaleDateString(),
            "VersionProgress": $("#VersionProgress").select2("val"),
            "Remarks": $("#Remarks").val()
        }
    };
    $.ajax({
        type: "Post",
        url: "/SoftwareVersion/" + $("#Action").val(),
        data: postData,
        success: function (data) {
            if (data.result === true) {
                loadSoftwareVersions(1, 15);
                $("#addSoftwareVersion").modal("hide");
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
                url: "/SoftwareVersion/DeleteSingle",
                data: { "id": id },
                success: function () {
                    loadSoftwareVersions(1, 15);
                    layer.closeAll();
                }
            });
        });
};

function release(id) {
    layer.confirm("是否发布",
        { btn: ["是", "否"] },
        function () {
            $.ajax({
                type: "Post",
                url: "/SoftwareVersion/Release",
                data: { "id": id },
                success: function () {
                    loadSoftwareVersions(1, 15);
                    layer.closeAll();
                }
            });
        });
};

