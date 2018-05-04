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
        BindOrReloadSaleInvoiceTable('Init');
        $('#tblSaleInvoice tbody').on('dblclick', 'td', function () {
            EditSaleInvoice(this);
        });
    }
    catch (e) {
        console.log(e.message);
    }
});
//function bind the SaleInvoice list checking search and filter
function BindOrReloadSaleInvoiceTable(action) {
    try {
        //creating advancesearch object
        SaleInvoiceAdvanceSearchViewModel = new Object();
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
        SaleInvoiceAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
        SaleInvoiceAdvanceSearchViewModel.SearchTerm = $('#SearchTerm').val();
        SaleInvoiceAdvanceSearchViewModel.FromDate = $('#FromDate').val();
        SaleInvoiceAdvanceSearchViewModel.ToDate = $('#ToDate').val();
        //apply datatable plugin on SaleInvoice table
        _dataTable.SaleInvoiceList = $('#tblSaleInvoice').DataTable(
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
                url: "SaleInvoice/GetAllSaleInvoice/",
                data: { "SaleInvoiceAdvanceSearchVM": SaleInvoiceAdvanceSearchViewModel },
                type: 'POST'
            },
            pageLength: 13,
            columns: [
               { "data": "SaleInvoiceNo", "defaultContent": "<i>-</i>" },
               { "data": "SaleInvoiceDateFormatted", "defaultContent": "<i>-</i>" },
               { "data": "Customer.CompanyName", "defaultContent": "<i>-</i>" },
               { "data": "Customer.ContactPerson", "defaultContent": "<i>-</i>" },
               { "data": "Customer.Mobile", "defaultContent": "<i>-</i>" },
               { "data": "RequirementSpec", "defaultContent": "<i>-</i>" },
               { "data": "ReferencePerson.Name", "defaultContent": "<i>-</i>" },
               { "data": "DocumentStatus.Description", "defaultContent": "<i>-</i>" },
               { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink"  onclick="EditSaleInvoice(this)" ><i class="fa fa-pencil-square-o" aria-hidden="true"></i></a>' },
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
                $('#tblSaleInvoice').fadeIn(100);
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
                    BindOrReloadSaleInvoiceTable();
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
function ResetSaleInvoiceList() {
    $(".searchicon").removeClass('filterApplied');
    BindOrReloadSaleInvoiceTable('Reset');
}
//function export data to excel
function ExportSaleInvoiceData() {
    $('.excelExport').show();
    OnServerCallBegin();
    BindOrReloadSaleInvoiceTable('Export');
}
// add SaleInvoice section
function AddSaleInvoice() {
    //this will return form body(html)
    OnServerCallBegin();
    $("#divSaleInvoiceForm").load("SaleInvoice/SaleInvoiceForm?id=" + _emptyGuid, function () {
        ChangeButtonPatchView("SaleInvoice", "btnPatchSaleInvoiceNew", "Add");
        BindSaleInvoiceDetailList(_emptyGuid);
        OnServerCallComplete();
        //setTimeout(function () {
        //resides in customjs for sliding
        openNav();
        //}, 100);
    });
}
function EditSaleInvoice(this_Obj) {
    OnServerCallBegin();
    var SaleInvoice = _dataTable.SaleInvoiceList.row($(this_Obj).parents('tr')).data();
    //this will return form body(html)
    $("#divSaleInvoiceForm").load("SaleInvoice/SaleInvoiceForm?id=" + SaleInvoice.ID, function () {
        //$('#CustomerID').trigger('change');
        ChangeButtonPatchView("SaleInvoice", "btnPatchSaleInvoiceNew", "Edit");
        BindSaleInvoiceDetailList(SaleInvoice.ID);
        $('#divCustomerBasicInfo').load("Customer/CustomerBasicInfo?ID=" + $('#hdnCustomerID').val());
        clearUploadControl();
        PaintImages(SaleInvoice.ID);
        OnServerCallComplete();
        setTimeout(function () {
            //resides in customjs for sliding
            openNav();
        }, 100);
    });
}
function ResetSaleInvoice() {
    $("#divSaleInvoiceForm").load("SaleInvoice/SaleInvoiceForm?id=" + $('#SaleInvoiceForm #ID').val(), function () {
        BindSaleInvoiceDetailList($('#ID').val());
        clearUploadControl();
        PaintImages($('#SaleInvoiceForm #ID').val());
        $('#divCustomerBasicInfo').load("Customer/CustomerBasicInfo?ID=" + $('#SaleInvoiceForm #hdnCustomerID').val());
    });
}
function SaveSaleInvoice() {
    var saleInvoiceDetailList = _dataTable.SaleInvoiceDetailList.rows().data().toArray();
    $('#DetailJSON').val(JSON.stringify(saleInvoiceDetailList));
    $('#btnInsertUpdateSaleInvoice').trigger('click');
}
function ApplyFilterThenSearch() {
    $(".searchicon").addClass('filterApplied');
    CloseAdvanceSearch();
    BindOrReloadSaleInvoiceTable('Search');
}
function SaveSuccessSaleInvoice(data, status) {
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
                $("#divSaleInvoiceForm").load("SaleInvoice/SaleInvoiceForm?id=" + _result.ID, function () {
                    ChangeButtonPatchView("SaleInvoice", "btnPatchSaleInvoiceNew", "Edit");
                    BindSaleInvoiceDetailList(_result.ID);
                    clearUploadControl();
                    PaintImages(_result.ID);
                    $('#divCustomerBasicInfo').load("Customer/CustomerBasicInfo?ID=" + $('#SaleInvoiceForm #hdnCustomerID').val());
                });
                ChangeButtonPatchView("SaleInvoice", "btnPatchSaleInvoiceNew", "Edit");
                BindOrReloadSaleInvoiceTable('Init');
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

function DeleteSaleInvoice() {
    notyConfirm('Are you sure to delete?', 'DeleteSaleInvoiceItem("' + $('#SaleInvoiceForm #ID').val() + '")');
}
function DeleteSaleInvoiceItem(id) {
    try {
        if (id) {
            var data = { "id": id };
            _jsonData = GetDataFromServer("SaleInvoice/DeleteSaleInvoice/", data);
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
                    ChangeButtonPatchView("SaleInvoice", "btnPatchSaleInvoiceNew", "Add");
                    ResetSaleInvoice();
                    BindOrReloadSaleInvoiceTable('Init');
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
function BindSaleInvoiceDetailList(id) {
    debugger;
    _dataTable.SaleInvoiceDetailList = $('#tblSaleInvoiceDetails').DataTable(
         {
             dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
             order: [],
             searching: false,
             paging: false,
             ordering: false,
             bInfo: false,
             data: id == _emptyGuid ? null : GetSaleInvoiceDetailListBySaleInvoiceID(id),
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
             { "data": null, "orderable": false, "defaultContent": '<a href="#" class="DeleteLink"  onclick="ConfirmDeleteSaleInvoiceDetail(this)" ><i class="fa fa-trash-o" aria-hidden="true"></i></a> <a href="#" class="actionLink"  onclick="EditSaleInvoiceDetail(this)" ><i class="fa fa-pencil-square-o" aria-hidden="true"></i></a>' },
             ],
             columnDefs: [
                 { "targets": [0, 4], "width": "10%" },
                 { "targets": [1, 2], "width": "15%" },
                 { "targets": [3], "width": "35%" },
                 { "targets": [5], "width": "10%" },
                 { "targets": [6], "width": "5%" },
                 { className: "text-right", "targets": [4, 5] },
                 { className: "text-left", "targets": [1, 2, 3] },
                 { className: "text-center", "targets": [0, 6] }
             ]
         });
}
function GetSaleInvoiceDetailListBySaleInvoiceID(id) {
    try {
        debugger;
        var data = { "saleInvoiceID": id };
        var saleInvoiceDetailList = [];
        _jsonData = GetDataFromServer("SaleInvoice/GetSaleInvoiceDetailListBySaleInvoiceID/", data);
        if (_jsonData != '') {
            _jsonData = JSON.parse(_jsonData);
            _message = _jsonData.Message;
            _status = _jsonData.Status;
            saleInvoiceDetailList = _jsonData.Records;
        }
        if (_status == "OK") {
            return saleInvoiceDetailList;
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
function AddSaleInvoiceDetailList() {
    $("#divModelSaleInvoicePopBody").load("SaleInvoice/AddSaleInvoiceDetail", function () {
        $('#lblModelPopSaleInvoice').text('SaleInvoice Detail')
        $('#divModelPopSaleInvoice').modal('show');
    });
}
function AddSaleInvoiceDetailToList() {
    debugger;
    $("#FormSaleInvoiceDetail").submit(function () { });
    debugger;
    if ($('#FormSaleInvoiceDetail #IsUpdate').val() == 'True') {
        if (($('#ProductID').val() != "") && ($('#Rate').val() != "") && ($('#Qty').val() != "") && ($('#UnitCode').val() != "")) {
            debugger;
            var saleInvoiceDetailList = _dataTable.SaleInvoiceDetailList.rows().data();
            saleInvoiceDetailList[_datatablerowindex].Product.Code = $("#ProductID").val() != "" ? $("#ProductID option:selected").text().split("-")[0].trim() : "";
            saleInvoiceDetailList[_datatablerowindex].Product.Name = $("#ProductID").val() != "" ? $("#ProductID option:selected").text().split("-")[1].trim() : "";
            saleInvoiceDetailList[_datatablerowindex].ProductID = $("#ProductID").val() != "" ? $("#ProductID").val() : _emptyGuid;
            saleInvoiceDetailList[_datatablerowindex].ProductModelID = $("#ProductModelID").val() != "" ? $("#ProductModelID").val() : _emptyGuid;
            ProductModel = new Object;
            Unit = new Object;
            ProductModel.Name = $("#ProductModelID").val() != "" ? $("#ProductModelID option:selected").text() : "";
            saleInvoiceDetailList[_datatablerowindex].ProductModel = ProductModel;
            saleInvoiceDetailList[_datatablerowindex].ProductSpec = $('#ProductSpec').val();
            saleInvoiceDetailList[_datatablerowindex].Qty = $('#Qty').val();
            saleInvoiceDetailList[_datatablerowindex].UnitCode = $('#UnitCode').val();
            Unit.Description = $("#UnitCode").val() != "" ? $("#UnitCode option:selected").text().trim() : "";
            saleInvoiceDetailList[_datatablerowindex].Unit = Unit;
            saleInvoiceDetailList[_datatablerowindex].Rate = $('#Rate').val();
            _dataTable.SaleInvoiceDetailList.clear().rows.add(saleInvoiceDetailList).draw(false);
            $('#divModelPopSaleInvoice').modal('hide');
            _datatablerowindex = -1;
        }
    }
    else {
        if (($('#ProductID').val() != "") && ($('#Rate').val() != "") && ($('#Qty').val() != "") && ($('#UnitCode').val() != "")) {
            if (_dataTable.SaleInvoiceDetailList.rows().data().length === 0) {
                _dataTable.SaleInvoiceDetailList.clear().rows.add(GetSaleInvoiceDetailListBySaleInvoiceID(_emptyGuid)).draw(false);
                debugger;
                var saleInvoiceDetailList = _dataTable.SaleInvoiceDetailList.rows().data();
                saleInvoiceDetailList[0].Product.Code = $("#ProductID").val() != "" ? $("#ProductID option:selected").text().split("-")[0].trim() : "";
                saleInvoiceDetailList[0].Product.Name = $("#ProductID").val() != "" ? $("#ProductID option:selected").text().split("-")[1].trim() : "";
                saleInvoiceDetailList[0].ProductID = $("#ProductID").val() != "" ? $("#ProductID").val() : _emptyGuid;
                saleInvoiceDetailList[0].ProductModelID = $("#ProductModelID").val() != "" ? $("#ProductModelID").val() : _emptyGuid;
                saleInvoiceDetailList[0].ProductModel.Name = $("#ProductModelID").val() != "" ? $("#ProductModelID option:selected").text() : "";
                saleInvoiceDetailList[0].ProductSpec = $('#ProductSpec').val();
                saleInvoiceDetailList[0].Qty = $('#Qty').val();
                saleInvoiceDetailList[0].UnitCode = $('#UnitCode').val();
                saleInvoiceDetailList[0].Unit.Description = $("#UnitCode").val() != "" ? $("#UnitCode option:selected").text().trim() : "";
                saleInvoiceDetailList[0].Rate = $('#Rate').val();
                _dataTable.SaleInvoiceDetailList.clear().rows.add(saleInvoiceDetailList).draw(false);
                $('#divModelPopSaleInvoice').modal('hide');
            }
            else {
                debugger;
                var SaleInvoiceDetailVM = new Object();
                var Product = new Object;
                var ProductModel = new Object()
                var Unit = new Object();
                SaleInvoiceDetailVM.ID = _emptyGuid;
                SaleInvoiceDetailVM.ProductID = $("#ProductID").val() != "" ? $("#ProductID").val() : _emptyGuid;
                Product.Code = $("#ProductID").val() != "" ? $("#ProductID option:selected").text().split("-")[0].trim() : "";
                Product.Name = $("#ProductID").val() != "" ? $("#ProductID option:selected").text().split("-")[1].trim() : "";
                SaleInvoiceDetailVM.Product = Product;
                SaleInvoiceDetailVM.ProductModelID = $("#ProductModelID").val() != "" ? $("#ProductModelID").val() : _emptyGuid;
                ProductModel.Name = $("#ProductModelID").val() != "" ? $("#ProductModelID option:selected").text() : "";
                SaleInvoiceDetailVM.ProductModel = ProductModel;
                SaleInvoiceDetailVM.ProductSpec = $('#ProductSpec').val();
                SaleInvoiceDetailVM.Qty = $('#Qty').val();
                Unit.Description = $("#UnitCode").val() != "" ? $("#UnitCode option:selected").text().trim() : "";
                SaleInvoiceDetailVM.Unit = Unit;
                SaleInvoiceDetailVM.UnitCode = $('#UnitCode').val();
                SaleInvoiceDetailVM.Rate = $('#Rate').val();
                _dataTable.SaleInvoiceDetailList.row.add(SaleInvoiceDetailVM).draw(true);
                $('#divModelPopSaleInvoice').modal('hide');
            }
        }

    }
}
function EditSaleInvoiceDetail(this_Obj) {
    debugger;
    _datatablerowindex = _dataTable.SaleInvoiceDetailList.row($(this_Obj).parents('tr')).index();
    var saleInvoiceDetail = _dataTable.SaleInvoiceDetailList.row($(this_Obj).parents('tr')).data();
    $("#divModelSaleInvoicePopBody").load("SaleInvoice/AddSaleInvoiceDetail", function () {
        $('#lblModelPopSaleInvoice').text('SaleInvoice Detail')
        $('#FormSaleInvoiceDetail #IsUpdate').val('True');
        $('#FormSaleInvoiceDetail #ID').val(saleInvoiceDetail.ID);
        $("#FormSaleInvoiceDetail #ProductID").val(saleInvoiceDetail.ProductID)
        $("#FormSaleInvoiceDetail #hdnProductID").val(saleInvoiceDetail.ProductID)
        $('#divProductBasicInfo').load("Product/ProductBasicInfo?ID=" + $('#hdnProductID').val(), function () {
        });

        if ($('#hdnProductID').val() != _emptyGuid) {
            $('.divProductModelSelectList').load("ProductModel/ProductModelSelectList?required=required&productID=" + $('#hdnProductID').val())
        }
        else {
            $('.divProductModelSelectList').empty();
            $('.divProductModelSelectList').append('<span class="form-control newinput"><i id="dropLoad" class="fa fa-spinner"></i></span>');
        }
        $("#FormSaleInvoiceDetail #ProductModelID").val(saleInvoiceDetail.ProductModelID);
        $("#FormSaleInvoiceDetail #hdnProductModelID").val(saleInvoiceDetail.ProductModelID);
        if ($('#hdnProductModelID').val() != _emptyGuid) {
            $('#divProductBasicInfo').load("ProductModel/ProductModelBasicInfo?ID=" + $('#hdnProductModelID').val(), function () {
            });
        }
        $('#FormSaleInvoiceDetail #ProductSpec').val(saleInvoiceDetail.ProductSpec);
        $('#FormSaleInvoiceDetail #Qty').val(saleInvoiceDetail.Qty);
        $('#FormSaleInvoiceDetail #UnitCode').val(saleInvoiceDetail.UnitCode);
        $('#FormSaleInvoiceDetail #hdnUnitCode').val(saleInvoiceDetail.UnitCode);
        $('#FormSaleInvoiceDetail #Rate').val(saleInvoiceDetail.Rate);
        $('#divModelPopSaleInvoice').modal('show');
    });
}
function ConfirmDeleteSaleInvoiceDetail(this_Obj) {
    debugger;
    _datatablerowindex = _dataTable.SaleInvoiceDetailList.row($(this_Obj).parents('tr')).index();
    var saleInvoiceDetail = _dataTable.SaleInvoiceDetailList.row($(this_Obj).parents('tr')).data();
    if (saleInvoiceDetail.ID === _emptyGuid) {
        var saleInvoiceDetailList = _dataTable.SaleInvoiceDetailList.rows().data();
        saleInvoiceDetailList.splice(_datatablerowindex, 1);
        _dataTable.SaleInvoiceDetailList.clear().rows.add(saleInvoiceDetailList).draw(false);
        notyAlert('success', 'Detail Row deleted successfully');
    }
    else {
        notyConfirm('Are you sure to delete?', 'DeleteSaleInvoiceDetail("' + saleInvoiceDetail.ID + '")');

    }
}
function DeleteSaleInvoiceDetail(ID) {
    if (ID != _emptyGuid && ID != null && ID != '') {
        var data = { "id": ID };
        var ds = {};
        _jsonData = GetDataFromServer("SaleInvoice/DeleteSaleInvoiceDetail/", data);
        if (_jsonData != '') {
            _jsonData = JSON.parse(_jsonData);
            _message = _jsonData.Message;
            _status = _jsonData.Status;
            _result = _jsonData.Record;
        }
        if (_status == "OK") {
            notyAlert('success', _result.Message);
            var saleInvoiceDetailList = _dataTable.SaleInvoiceDetailList.rows().data();
            saleInvoiceDetailList.splice(_datatablerowindex, 1);
            _dataTable.SaleInvoiceDetailList.clear().rows.add(saleInvoiceDetailList).draw(false);
        }
        if (_status == "ERROR") {
            notyAlert('error', _message);
        }
    }
}
//================================================================================================
//SaleInvoiceFollowup Section
function AddSaleInvoiceFollowUp() {
    debugger;
    $("#divModelSaleInvoicePopBody").load("SaleInvoiceFollowup/AddSaleInvoiceFollowup?id=" + _emptyGuid + "&saleInvoiceID=" + $('#SaleInvoiceForm input[type="hidden"]#ID').val() + "&customerID=" + ($('#SaleInvoiceForm #hdnCustomerID').val() != "" ? $('#SaleInvoiceForm #hdnCustomerID').val() : _emptyGuid), function () {
        $('#lblModelPopSaleInvoice').text('Add SaleInvoice Followup')
        $('#btnresetSaleInvoiceFollowup').trigger('click');
        $('#divModelPopSaleInvoice').modal('show');

    });
}
function SaleInvoiceFollowUpPaging(start) {
    $("#divSaleInvoiceFollowupboxbody").load("SaleInvoiceFollowup/GetSaleInvoiceFollowupList?ID=" + _emptyGuid + "&SaleInvoiceID=" + $('#SaleInvoiceForm input[type="hidden"]#ID').val() + "&DataTablePaging.Start=" + start, function () {

    });
}
function EditSaleInvoiceFollowup(id) {
    debugger;
    $("#divModelSaleInvoicePopBody").load("SaleInvoiceFollowup/AddSaleInvoiceFollowup?id=" + id + "&saleInvoiceID=" + $('#SaleInvoiceForm input[type="hidden"]#ID').val() + "&customerID=" + _emptyGuid, function () {
        $('#lblModelPopSaleInvoice').text('Edit SaleInvoice Followup')
        $('#divModelPopSaleInvoice').modal('show');
    });
}
function SaveSuccessSaleInvoiceFollowup(data, status) {
    try {
        debugger;
        var _jsonData = JSON.parse(data)
        //message field will return error msg only
        _message = _jsonData.Message;
        _status = _jsonData.Status;
        _result = _jsonData.Record;
        switch (_status) {
            case "OK":
                MasterAlert("success", _result.Message)
                $("#divModelSaleInvoicePopBody").load("SaleInvoiceFollowup/AddSaleInvoiceFollowup?ID=" + _result.ID + "&SaleInvoiceID=" + $('#SaleInvoiceForm input[type="hidden"]#ID').val(), "&customerID=" + _emptyGuid, function () {
                    $('#lblModelPopSaleInvoice').text('Edit SaleInvoice Followup')

                });
                $("#divFollowupList").load("SaleInvoiceFollowup/GetSaleInvoiceFollowupList?ID=" + _emptyGuid + "&SaleInvoiceID=" + $('#SaleInvoiceForm input[type="hidden"]#ID').val(), function () {
                });
                break;
            case "ERROR":
                MasterAlert("danger", _message)
                break;
            default:
                console.log(_message);
                break;
        }
    }
    catch (e) {
        //this will show the error msg in the browser console(F12) 
        console.log(e.message);
    }
}
function ConfirmDeleteSaleInvoiceFollowup(ID) {
    if (ID != _emptyGuid) {
        notyConfirm('Are you sure to delete?', 'DeleteSaleInvoiceFollowup("' + ID + '")');
    }
}
function DeleteSaleInvoiceFollowup(ID) {
    if (ID != _emptyGuid && ID != null && ID != '') {
        var data = { "id": ID };
        var ds = {};
        _jsonData = GetDataFromServer("SaleInvoiceFollowup/DeleteSaleInvoiceFollowup/", data);
        if (_jsonData != '') {
            _jsonData = JSON.parse(_jsonData);
            _message = _jsonData.Message;
            _status = _jsonData.Status;
            _result = _jsonData.Record;
        }
        if (_status == "OK") {
            notyAlert('success', _result.Message);
            $("#divFollowupList").load("SaleInvoiceFollowup/GetSaleInvoiceFollowupList?ID=" + _emptyGuid + "&SaleInvoiceID=" + $('#SaleInvoiceForm input[type="hidden"]#ID').val(), function () {
            });
        }
        if (_status == "ERROR") {
            notyAlert('error', _message);
        }
    }
}