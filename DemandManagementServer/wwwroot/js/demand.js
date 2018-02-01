$(function () {
    $("#DemandType").select2({ minimumResultsForSearch: Infinity });
    $("#btnAdd").click(function () { add(); });
    $("#btnSave").click(function () { save(); });
    $('.datepicker').datepicker({
        todayBtn: "linked",
        clearBtn: true,
        language: "zh-CN",
        autoclose: true
    });
    loadDemands(1, 15);
    initialUser();
    initialCustomer();
});

var currentUserId = "0";

function initialUser() {
    $.ajax({
        type: "Get",
        url: "/User/GetAllUsers",    //获取数据的ajax请求地址
        async:false,
        success: function (data) {
            $("#User").select2({ minimumResultsForSearch: Infinity});
            var option = "";
            $.each(data,
                function (i, item) {
                    option += "<option value='" + item.id + "'>" + item.name+"("+item.userName+")" + "</option>";
                });
            $("#User").html(option);
        }
    });
    $.ajax({
        type: "Get",
        url: "/User/GetCurrentUser",    //获取数据的ajax请求地址
        success: function (data) {
            currentUserId = data.id.toString();
            $("#User").val(data.id.toString()).trigger('change');
        }
    });
}

function initialCustomer() {
    $.ajax({
        type: "Get",
        url: "/Customer/GetAllCustomers",    //获取数据的ajax请求地址
        success: function (data) {
            $("#Customer").select2({ minimumResultsForSearch: Infinity });
            var option = "";
            $.each(data,
                function (i, item) {
                    option += "<option value='" + item.id + "'>" + item.name + "</option>";
                });
            $("#Customer").html(option);
        }
    });
}

function loadDemands(startPage, pageSize) {
    $("#tableBody").html("");
    $.ajax({
        type: "GET",
        url: "/Demand/GetDemands?startPage=" + startPage + "&pageSize=" + pageSize,
        success: function (data) {
            $.each(data.demands,
                function (i, item) {
                    var tr = "<tr>";
                    tr += "<td>" + item.demandCode + "</td>";
                    tr += "<td>" + item.DemandType + "</td>";
                    tr += "<td>" + item.DemandDetail + "</td>";
                    tr += "<td>" + item.User + "</td>";
                    tr += "<td>" + item.Customer + "</td>";
                    tr += "<td>" + item.CreateTime + "</td>";
                    tr += "<td>" + "<span class='badge " + item.DemandPhase === "完成"
                        ? "badge-success"
                        : (item.DemandPhase === "中止" ? "badge-ignore" : "") + "'>" + item.DemandPhase + "</span>";
                    tr += "<td>" + item.AlignRecords + "</td>";
                    tr += "<td>" + item.AnalyseRecords + "</td>";
                    tr += "<td>" + item.SoftwareVersion + "</td>";
                    tr += "<td>" + item.ReleaseDate + "</td>";
                    tr += "<td>" + item.Remarks + "</td>";
                    var editHtml = "<button class='btn btn-info btn-xs' href='javascript:;' onclick='edit(\"" +
                        item.id +
                        "\")'><i class='fa fa-edit'></i> 编辑 </button>";
                    var deleteHtml =
                        "<button class='btn btn-danger btn-xs' href='javascript:;' onclick='deleteSingle(\"" +
                        item.id +
                        "\")'><i class='fa fa-trash-o'></i> 中止 </button>";

                    tr += "<td>" +  editHtml + deleteHtml+ "</td>";
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
                        loadDemands(newPage, pageSize);
                    }
                }
                elment.bootstrapPaginator(options); //分页插件初始化
            }
        }
    });
}

function add() {
    $("#Title").text("新增需求");
    $("#Action").val("AddDemand");
    $("#Id").val(0);
    $("#DemandCode").val("");
    $("#DemandType").val("0").trigger("change");
    $("#DemandDetail").val("");
    $("#User").val(currentUserId).trigger("change");
    $("#Customer").val("1").trigger("change");
    $("#Remarks").val("");
    //弹出新增窗体
    $("#addDemand").modal("show");
}