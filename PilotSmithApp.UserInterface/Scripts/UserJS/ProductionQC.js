var _dataTable = {};
var _emptyGuid = "00000000-0000-0000-0000-000000000000";
var _datatablerowindex = -1;
var _jsonData = {};
var _message = "";
var _status = "";
var _result = "";
//---------------------------------------Docuement Ready--------------------------------------------------//
$(document).ready(function () {
    try {
        BindOrReloadProductionQCTable('Init');
        $('#tblProductionQC tbody').on('dblclick', 'td', function () {
            EditProductionQC(this);
        });
    }
    catch (e) {
        console.log(e.message);
    }
});
//function bind the ProductionQC list checking search and filter
function BindOrReloadProductionQCTable(action) {
    try {
        //creating advancesearch object
        ProductionQCAdvanceSearchViewModel = new Object();
        DataTablePagingViewModel = new Object();
        DataTablePagingViewModel.Length = 0;
        //switch case to check the operation
        switch (action) {
            case 'Reset':
                $('#SearchTerm').val('');
                $('#FromDate').val('');
                $('#ToDate').val('');
                break;
            case 'Init':
                $('#SearchTerm').val('');
                $('#FromDate').val('');
                $('#ToDate').val('');
                break;
            case 'Search':
                if (($('#SearchTerm').val() == "") && ($('#FromDate').val() == "") && ($('#ToDate').val() == "")) {
                    return true;
                }
                break;
            case 'Export':
                DataTablePagingViewModel.Length = -1;
                break;
            default:
                break;
        }
        ProductionQCAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
        ProductionQCAdvanceSearchViewModel.SearchTerm = $('#SearchTerm').val();
        ProductionQCAdvanceSearchViewModel.FromDate = $('#FromDate').val();
        ProductionQCAdvanceSearchViewModel.ToDate = $('#ToDate').val();
        //apply datatable plugin on ProductionQC table
        _dataTable.ProductionQCList = $('#tblProductionQC').DataTable(
        {
            dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
            buttons: [{
                extend: 'excel',
                exportOptions:
                             {
                                 columns: [0, 1, 2, 3, 4, 5, 6, 7]
                             }
            }],
            ordering: false,
            searching: false,
            paging: true,
            lengthChange: false,
            processing: true,
            language: {
                "processing": "<div class='spinner'><div class='bounce1'></div><div class='bounce2'></div><div class='bounce3'></div></div>"
            },
            serverSide: true,
            ajax: {
                url: "ProductionQC/GetAllProductionQC/",
                data: { "ProductionQCAdvanceSearchVM": ProductionQCAdvanceSearchViewModel },
                type: 'POST'
            },
            pageLength: 13,
            columns: [
               { "data": "ProdQCNo", "defaultContent": "<i>-</i>" },
               { "data": "ProdQCDateFormatted", "defaultContent": "<i>-</i>" },
               { "data": "Customer.CompanyName", "defaultContent": "<i>-</i>" },
               { "data": "Customer.ContactPerson", "defaultContent": "<i>-</i>" },
               { "data": "Customer.Mobile", "defaultContent": "<i>-</i>" },
               { "data": "DocumentStatus.Description", "defaultContent": "<i>-</i>" },
               { "data": "PreparedBy", "defaultContent": "<i>-</i>" },
               { "data": "Plant", "defaultContent": "<i>-</i>" },
               { "data": "LatestApprovalStatus", "defaultContent": "<i>-</i>" },
               { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink"  onclick="EditProductionQC(this)" ><i class="fa fa-pencil-square-o" aria-hidden="true"></i></a>' },
            ],
            columnDefs: [{ className: "text-right", "targets": [] },
                          { className: "text-left", "targets": [2, 3, 5, 6] },
                          { className: "text-center", "targets": [0, 1, 4, 7, 8] },
                            { "targets": [0, 1, 4], "width": "10%" },
                            { "targets": [2, 3], "width": "10%" },
                            { "targets": [5], "width": "30%" },
                            { "targets": [6], "width": "10%" },
                            { "targets": [7, 8], "width": "5%" },
            ],
            destroy: true,
            //for performing the import operation after the data loaded
            initComplete: function (settings, json) {
                debugger;
                $('.dataTables_wrapper div.bottom div').addClass('col-md-6');
                $('#tblProductionQC').fadeIn(100);
                if (action == undefined) {
                    $('.excelExport').hide();
                    OnServerCallComplete();
                }
                if (action === 'Export') {
                    if (json.data.length > 0) {
                        if (json.data[0].TotalCount > 1000) {
                            setTimeout(function () {
                                MasterAlert("info", 'We are able to download maximum 1000 rows of data, There exist more than 1000 rows of data please filter and download')
                            }, 10000)
                        }
                    }
                    $(".buttons-excel").trigger('click');
                    BindOrReloadProductionQCTable();
                }
            }
        });
        $(".buttons-excel").hide();
    }
    catch (e) {
        console.log(e.message);
    }
}
//function reset the list to initial
function ResetProductionQCList() {
    $(".searchicon").removeClass('filterApplied');
    BindOrReloadProductionQCTable('Reset');
}
//function export data to excel
function ExportProductionQCData() {
    $('.excelExport').show();
    OnServerCallBegin();
    BindOrReloadProductionQCTable('Export');
}

// add ProductionQC section
function AddProductionQC() {
    //this will return form body(html)
    OnServerCallBegin();
    $("#divProductionQCForm").load("ProductionQC/ProductionQCForm?id=" + _emptyGuid + "&productionOrderID=", function () {
        ChangeButtonPatchView("ProductionQC", "btnPatchProductionQCNew", "Add");
        BindProductionQCDetailList(_emptyGuid);
        OnServerCallComplete();
        //setTimeout(function () {
        //resides in customjs for sliding
        openNav();
        //}, 100);
    });
}
function EditProductionQC(this_Obj) {
    OnServerCallBegin();
    var ProductionQC = _dataTable.ProductionQCList.row($(this_Obj).parents('tr')).data();
    //this will return form body(html)
    $("#divProductionQCForm").load("ProductionQC/ProductionQCForm?id=" + ProductionQC.ID + "&productionOrderID=" + ProductionQC.ProdOrderID, function () {
        //$('#CustomerID').trigger('change');
        ChangeButtonPatchView("ProductionQC", "btnPatchProductionQCNew", "Edit");
        BindProductionQCDetailList(ProductionQC.ID);
        $('#divCustomerBasicInfo').load("Customer/CustomerBasicInfo?ID=" + $('#hdnCustomerID').val());
        clearUploadControl();
        PaintImages(ProductionQC.ID);
        OnServerCallComplete();
            openNav();
    });
}
function ResetProductionQC() {
    $("#divProductionQCForm").load("ProductionQC/ProductionQCForm?id=" + $('#ProductionQCForm #ID').val() + "&productionOrderID=" + $('#hdnProdOrderID').val(), function () {
        BindProductionQCDetailList($('#ID').val());
        clearUploadControl();
        PaintImages($('#ProductionQCForm #ID').val());
        $('#divCustomerBasicInfo').load("Customer/CustomerBasicInfo?ID=" + $('#ProductionQCForm #hdnCustomerID').val());
    });
}
function SaveProductionQC() {
    var productionQCDetailList = _dataTable.ProductionQCDetailList.rows().data().toArray();
    $('#DetailJSON').val(JSON.stringify(productionQCDetailList));
    $('#btnInsertUpdateProductionQC').trigger('click');
}
function ApplyFilterThenSearch() {
    $(".searchicon").addClass('filterApplied');
    CloseAdvanceSearch();
    BindOrReloadProductionQCTable('Search');
}
function SaveSuccessProductionQC(data, status) {
    try {
        debugger;
        var _jsonData = JSON.parse(data)
        //message field will return error msg only
        _message = _jsonData.Message;
        _status = _jsonData.Status;
        _result = _jsonData.Record;
        switch (_status) {
            case "OK":
                $('#IsUpdate').val('True');
                $("#divProductionQCForm").load("ProductionQC/ProductionQCForm?id=" + _result.ID, function () {
                    ChangeButtonPatchView("ProductionQC", "btnPatchProductionQCNew", "Edit");
                    BindProductionQCDetailList(_result.ID);
                    clearUploadControl();
                    PaintImages(_result.ID);
                    $('#divCustomerBasicInfo').load("Customer/CustomerBasicInfo?ID=" + $('#ProductionQCForm #hdnCustomerID').val());
                });
                ChangeButtonPatchView("ProductionQC", "btnPatchProductionQCNew", "Edit");
                BindOrReloadProductionQCTable('Init');
                notyAlert('success', _result.Message);
                break;
            case "ERROR":
                notyAlert('error', _message);
                break;
            default:
                break;
        }
    }
    catch (e) {
        //this will show the error msg in the browser console(F12) 
        console.log(e.message);
    }
}

function DeleteProductionQC() {
    notyConfirm('Are you sure to delete?', 'DeleteProductionQCItem("' + $('#ProductionQCForm #ID').val() + '")');
}
function DeleteProductionQCItem(id) {
    try {
        if (id) {
            var data = { "id": id };
            _jsonData = GetDataFromServer("ProductionQC/DeleteProductionQC/", data);
            if (_jsonData != '') {
                _jsonData = JSON.parse(_jsonData);
                _message = _jsonData.Message;
                _status = _jsonData.Status;
                _result = _jsonData.Record;
            }
            switch (_status) {
                case "OK":
                    notyAlert('success', _result.Message);
                    $('#ID').val(_emptyGuid);
                    ChangeButtonPatchView("ProductionQC", "btnPatchProductionQCNew", "Add");
                    ResetProductionQC();
                    BindOrReloadProductionQCTable('Init');
                    break;
                case "ERROR":
                    notyAlert('error', _message);
                    break;
                default:
                    break;
            }
        }
    }
    catch (e) {
        //this will show the error msg in the browser console(F12) 
        console.log(e.message);
    }
}
function BindProductionQCDetailList(id, IsProductioOrder) {
    debugger;
    _dataTable.ProductionQCDetailList = $('#tblProductionQCDetails').DataTable(
         {
             dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
             order: [],
             searching: false,
             paging: false,
             ordering: false,
             bInfo: false,
             data: !IsProductioOrder ? id == _emptyGuid ? null : GetProductionQCDetailListByProductionQCID(id, false) : GetProductionQCDetailListByProductionQCID(id, true),
             language: {
                 search: "_INPUT_",
                 searchPlaceholder: "Search"
             },
             columns: [
             { "data": "Product.Code", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
             { "data": "Product.Name", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
             { "data": "ProductModel.Name", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
             { "data": "ProductSpec", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
             {
                 "data": "Qty", render: function (data, type, row) {
                     return data + " " + row.Unit.Description
                 }, "defaultContent": "<i></i>"
             },
             { "data": "Rate", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
             { "data": null, "orderable": false, "defaultContent": '<a href="#" class="DeleteLink"  onclick="ConfirmDeleteProductionQCDetail(this)" ><i class="fa fa-trash-o" aria-hidden="true"></i></a> <a href="#" class="actionLink"  onclick="EditProductionQCDetail(this)" ><i class="fa fa-pencil-square-o" aria-hidden="true"></i></a>' },
             ],
             columnDefs: [
                 { "targets": [0], "width": "35%" },
                 { "targets": [1, 2], "width": "15%" },
                 { "targets": [3, 4], "width": "10%" },
                 { "targets": [5], "width": "10%" },
                 { "targets": [6], "width": "5%" },
                 { className: "text-left", "targets": [0, 1, 2, 3,4,5] },
                 { className: "text-center", "targets": [ 6] }
             ]
         });
}
function GetProductionQCDetailListByProductionQCID(id, IsProductioOrder) {
    try {
        var productionQCDetailList = [];
        if (IsProductioOrder) {
            var data = { "productionOrderID": $('#ProductionQCForm #hdnProdOrderID').val() };
            _jsonData = GetDataFromServer("ProductionQC/GetProductionQCDetailListByProductionOrderIDWithProductionOrder/", data);
        }
        else {
            var data = { "productionQCID": id };
            _jsonData = GetDataFromServer("ProductionQC/GetProductionQCDetailListByProductionQCID/", data);
        }
        if (_jsonData != '') {
            _jsonData = JSON.parse(_jsonData);
            _message = _jsonData.Message;
            _status = _jsonData.Status;
            productionQCDetailList = _jsonData.Records;
        }
        if (_status == "OK") {
            return productionQCDetailList;
        }
        if (_status == "ERROR") {
            notyAlert('error', _message);
        }
    }
    catch (e) {
        //this will show the error msg in the browser console(F12) 
        console.log(e.message);

    }
}
function AddProductionQCDetailList() {
    $("#divModelProductionQCPopBody").load("ProductionQC/AddProductionQCDetail", function () {
        $('#lblModelPopProductionQC').text('ProductionQC Detail')
        $('#divModelPopProductionQC').modal('show');
    });
}
function AddProductionQCDetailToList() {
    debugger;
    $("#FormProductionQCDetail").submit(function () { });
    debugger;
    if ($('#FormProductionQCDetail #IsUpdate').val() == 'True') {
        if (($('#ProductID').val() != "") && ($('#Rate').val() != "") && ($('#Qty').val() != "") && ($('#UnitCode').val() != "")) {
            debugger;
            var productionQCDetailList = _dataTable.ProductionQCDetailList.rows().data();
            productionQCDetailList[_datatablerowindex].Product.Code = $("#ProductID").val() != "" ? $("#ProductID option:selected").text().split("-")[0].trim() : "";
            productionQCDetailList[_datatablerowindex].Product.Name = $("#ProductID").val() != "" ? $("#ProductID option:selected").text().split("-")[1].trim() : "";
            productionQCDetailList[_datatablerowindex].ProductID = $("#ProductID").val() != "" ? $("#ProductID").val() : _emptyGuid;
            productionQCDetailList[_datatablerowindex].ProductModelID = $("#ProductModelID").val() != "" ? $("#ProductModelID").val() : _emptyGuid;
            ProductModel = new Object;
            Unit = new Object;
            ProductModel.Name = $("#ProductModelID").val() != "" ? $("#ProductModelID option:selected").text() : "";
            productionQCDetailList[_datatablerowindex].ProductModel = ProductModel;
            productionQCDetailList[_datatablerowindex].ProductSpec = $('#ProductSpec').val();
            productionQCDetailList[_datatablerowindex].Qty = $('#Qty').val();
            productionQCDetailList[_datatablerowindex].UnitCode = $('#UnitCode').val();
            Unit.Description = $("#UnitCode").val() != "" ? $("#UnitCode option:selected").text().trim() : "";
            productionQCDetailList[_datatablerowindex].Unit = Unit;
            productionQCDetailList[_datatablerowindex].Rate = $('#Rate').val();
            _dataTable.ProductionQCDetailList.clear().rows.add(productionQCDetailList).draw(false);
            $('#divModelPopProductionQC').modal('hide');
            _datatablerowindex = -1;
        }
    }
    else {
        if (($('#ProductID').val() != "") && ($('#Rate').val() != "") && ($('#Qty').val() != "") && ($('#UnitCode').val() != "")) {
            if (_dataTable.ProductionQCDetailList.rows().data().length === 0) {
                _dataTable.ProductionQCDetailList.clear().rows.add(GetProductionQCDetailListByProductionQCID(_emptyGuid)).draw(false);
                debugger;
                var productionQCDetailList = _dataTable.ProductionQCDetailList.rows().data();
                productionQCDetailList[0].Product.Code = $("#ProductID").val() != "" ? $("#ProductID option:selected").text().split("-")[0].trim() : "";
                productionQCDetailList[0].Product.Name = $("#ProductID").val() != "" ? $("#ProductID option:selected").text().split("-")[1].trim() : "";
                productionQCDetailList[0].ProductID = $("#ProductID").val() != "" ? $("#ProductID").val() : _emptyGuid;
                productionQCDetailList[0].ProductModelID = $("#ProductModelID").val() != "" ? $("#ProductModelID").val() : _emptyGuid;
                productionQCDetailList[0].ProductModel.Name = $("#ProductModelID").val() != "" ? $("#ProductModelID option:selected").text() : "";
                productionQCDetailList[0].ProductSpec = $('#ProductSpec').val();
                productionQCDetailList[0].Qty = $('#Qty').val();
                productionQCDetailList[0].UnitCode = $('#UnitCode').val();
                productionQCDetailList[0].Unit.Description = $("#UnitCode").val() != "" ? $("#UnitCode option:selected").text().trim() : "";
                productionQCDetailList[0].Rate = $('#Rate').val();
                _dataTable.ProductionQCDetailList.clear().rows.add(productionQCDetailList).draw(false);
                $('#divModelPopProductionQC').modal('hide');
            }
            else {
                debugger;
                var ProductionQCDetailVM = new Object();
                var Product = new Object;
                var ProductModel = new Object()
                var Unit = new Object();
                ProductionQCDetailVM.ID = _emptyGuid;
                ProductionQCDetailVM.ProductID = $("#ProductID").val() != "" ? $("#ProductID").val() : _emptyGuid;
                Product.Code = $("#ProductID").val() != "" ? $("#ProductID option:selected").text().split("-")[0].trim() : "";
                Product.Name = $("#ProductID").val() != "" ? $("#ProductID option:selected").text().split("-")[1].trim() : "";
                ProductionQCDetailVM.Product = Product;
                ProductionQCDetailVM.ProductModelID = $("#ProductModelID").val() != "" ? $("#ProductModelID").val() : _emptyGuid;
                ProductModel.Name = $("#ProductModelID").val() != "" ? $("#ProductModelID option:selected").text() : "";
                ProductionQCDetailVM.ProductModel = ProductModel;
                ProductionQCDetailVM.ProductSpec = $('#ProductSpec').val();
                ProductionQCDetailVM.Qty = $('#Qty').val();
                Unit.Description = $("#UnitCode").val() != "" ? $("#UnitCode option:selected").text().trim() : "";
                ProductionQCDetailVM.Unit = Unit;
                ProductionQCDetailVM.UnitCode = $('#UnitCode').val();
                ProductionQCDetailVM.Rate = $('#Rate').val();
                _dataTable.ProductionQCDetailList.row.add(ProductionQCDetailVM).draw(true);
                $('#divModelPopProductionQC').modal('hide');
            }
        }

    }
}
function EditProductionQCDetail(this_Obj) {
    debugger;
    _datatablerowindex = _dataTable.ProductionQCDetailList.row($(this_Obj).parents('tr')).index();
    var productionQCDetail = _dataTable.ProductionQCDetailList.row($(this_Obj).parents('tr')).data();
    $("#divModelProductionQCPopBody").load("ProductionQC/AddProductionQCDetail", function () {
        $('#lblModelPopProductionQC').text('ProductionQC Detail')
        $('#FormProductionQCDetail #IsUpdate').val('True');
        $('#FormProductionQCDetail #ID').val(productionQCDetail.ID);
        $("#FormProductionQCDetail #ProductID").val(productionQCDetail.ProductID)
        $("#FormProductionQCDetail #hdnProductID").val(productionQCDetail.ProductID)
        $('#divProductBasicInfo').load("Product/ProductBasicInfo?ID=" + $('#hdnProductID').val(), function () {
        });

        if ($('#hdnProductID').val() != _emptyGuid) {
            $('.divProductModelSelectList').load("ProductModel/ProductModelSelectList?required=required&productID=" + $('#hdnProductID').val())
        }
        else {
            $('.divProductModelSelectList').empty();
            $('.divProductModelSelectList').append('<span class="form-control newinput"><i id="dropLoad" class="fa fa-spinner"></i></span>');
        }
        $("#FormProductionQCDetail #ProductModelID").val(productionQCDetail.ProductModelID);
        $("#FormProductionQCDetail #hdnProductModelID").val(productionQCDetail.ProductModelID);
        if ($('#hdnProductModelID').val() != _emptyGuid) {
            $('#divProductBasicInfo').load("ProductModel/ProductModelBasicInfo?ID=" + $('#hdnProductModelID').val(), function () {
            });
        }
        $('#FormProductionQCDetail #ProductSpec').val(productionQCDetail.ProductSpec);
        $('#FormProductionQCDetail #Qty').val(productionQCDetail.Qty);
        $('#FormProductionQCDetail #UnitCode').val(productionQCDetail.UnitCode);
        $('#FormProductionQCDetail #hdnUnitCode').val(productionQCDetail.UnitCode);
        $('#FormProductionQCDetail #Rate').val(productionQCDetail.Rate);
        $('#divModelPopProductionQC').modal('show');
    });
}
function ConfirmDeleteProductionQCDetail(this_Obj) {
    debugger;
    _datatablerowindex = _dataTable.ProductionQCDetailList.row($(this_Obj).parents('tr')).index();
    var productionQCDetail = _dataTable.ProductionQCDetailList.row($(this_Obj).parents('tr')).data();
    if (productionQCDetail.ID === _emptyGuid) {
        var productionQCDetailList = _dataTable.ProductionQCDetailList.rows().data();
        productionQCDetailList.splice(_datatablerowindex, 1);
        _dataTable.ProductionQCDetailList.clear().rows.add(productionQCDetailList).draw(false);
        notyAlert('success', 'Detail Row deleted successfully');
    }
    else {
        notyConfirm('Are you sure to delete?', 'DeleteProductionQCDetail("' + productionQCDetail.ID + '")');

    }
}
function DeleteProductionQCDetail(ID) {
    if (ID != _emptyGuid && ID != null && ID != '') {
        var data = { "id": ID };
        var ds = {};
        _jsonData = GetDataFromServer("ProductionQC/DeleteProductionQCDetail/", data);
        if (_jsonData != '') {
            _jsonData = JSON.parse(_jsonData);
            _message = _jsonData.Message;
            _status = _jsonData.Status;
            _result = _jsonData.Record;
        }
        if (_status == "OK") {
            notyAlert('success', _result.Message);
            var productionQCDetailList = _dataTable.ProductionQCDetailList.rows().data();
            productionQCDetailList.splice(_datatablerowindex, 1);
            _dataTable.ProductionQCDetailList.clear().rows.add(productionQCDetailList).draw(false);
        }
        if (_status == "ERROR") {
            notyAlert('error', _message);
        }
    }
}
