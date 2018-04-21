﻿var _dataTable = {};
var _emptyGuid = "00000000-0000-0000-0000-000000000000";
var _datatablerowindex = -1;
var _jsonData = {};
var _message = "";
var _status = "";
var _result = "";
//---------------------------------------Docuement Ready--------------------------------------------------//
$(document).ready(function () {
    try {
        BindOrReloadEstimateTable('Init');
        $('#tblEstimate tbody').on('dblclick', 'td', function () {
            EditEstimate(this);
        });
    }
    catch (e) {
        console.log(e.message);
    }
});

//function bind the Enquiry list checking search and filter
function BindOrReloadEstimateTable(action) {
    try {
        //creating advancesearch object
        EstimateAdvanceSearchViewModel = new Object();
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
        EstimateAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
        EstimateAdvanceSearchViewModel.SearchTerm = $('#SearchTerm').val();
        EstimateAdvanceSearchViewModel.FromDate = $('#FromDate').val();
        EstimateAdvanceSearchViewModel.ToDate = $('#ToDate').val();
        //apply datatable plugin on Estimate table
        debugger;
        _dataTable.EstimateList = $('#tblEstimate').DataTable(
        {
            dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
            buttons: [{
                extend: 'excel',
                exportOptions:
                             {
                                 columns: [1, 2, 3, 4, 5, 6]
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
                url: "Estimate/GetAllEstimate/",
                data: { "estimateAdvanceSearchVM": EstimateAdvanceSearchViewModel },
                type: 'POST'
            },
            pageLength: 13,
            columns: [
               { "data": "EstimateNo", "defaultContent": "<i>-</i>" },
               { "data": "EstimateRefNo", "defaultContent": "<i>-</i>" },
               { "data": "EstimateDateFormatted", "defaultContent": "<i>-</i>" },
               { "data": "Enquiry.EnquiryNo", "defaultContent": "<i>-</i>" },
               { "data": "Customer.CompanyName", "defaultContent": "<i>-</i>" },
               { "data": "Branch.Description", "defaultContent": "<i>-</i>" },
               { "data": "UserName", "defaultContent": "<i>-</i>" },
               { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink"  onclick="EditEstimate(this)" ><i class="fa fa-pencil-square-o" aria-hidden="true"></i></a>' },
            ],
            columnDefs: [
                    { className: "text-left", "targets": [0, 1] },
                    { className: "text-center", "targets": [2] }
            ],
            destroy: true,
            //for performing the import operation after the data loaded
            initComplete: function (settings, json) {
                debugger;
                $('.dataTables_wrapper div.bottom div').addClass('col-md-6');
                $('#tblEstimate').fadeIn('slow');
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
                    BindOrReloadEstimateTable();
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
function ResetEstimateList() {
    $(".searchicon").removeClass('filterApplied');
    BindOrReloadEstimateTable('Reset');
}
//function export data to excel
function ExportEstimateData() {
    $('.excelExport').show();
    OnServerCallBegin();
    BindOrReloadEstimateTable('Export');
}

// add Estimate section
function AddEstimate() {
    debugger;
    //this will return form body(html)
    OnServerCallBegin();
    $("#divEstimateForm").load("Estimate/EstimateForm?id=" + _emptyGuid+"&enquiryID="+_emptyGuid, function () {
        ChangeButtonPatchView("Estimate", "btnPatchEstimateNew", "Add");
        BindEstimateDetailList(_emptyGuid);
        OnServerCallComplete();
        //resides in customjs for sliding
        setTimeout(function () {
            openNav();
        }, 100);
    });
}

function EditEstimate(this_Obj) {
    OnServerCallBegin();
    var Estimate = _dataTable.EstimateList.row($(this_Obj).parents('tr')).data();
    //this will return form body(html)
    $("#divEstimateForm").load("Estimate/EstimateForm?id=" + Estimate.ID+"&enquiryID="+_emptyGuid, function () {
        
        ChangeButtonPatchView("Estimate", "btnPatchEstimateNew", "Edit");
        BindEstimateDetailList(Estimate.ID);
        clearUploadControl();       
        PaintImages(Estimate.ID);        
        OnServerCallComplete();
        //resides in customjs for sliding
        setTimeout(function () {
           $("#divEstimateForm #EnquiryID").prop('disabled', true);
            openNav();
        }, 100);
    });
}

function ResetEstimate() {
    //this will return form body(html)
    $("#divEstimateForm").load("Estimate/EstimateForm?id=" + $('#EstimateForm #ID').val() + "&enquiryID=" + _emptyGuid, function () {
        BindEstimateDetailList($('#ID').val());
        clearUploadControl();
        PaintImages($('#EstimateForm #ID').val());
       
    });
}
function SaveEstimate() {
    var estimateDetailList = _dataTable.EstimateDetailList.rows().data().toArray();
    $('#DetailJSON').val(JSON.stringify(estimateDetailList));
    $('#btnInsertUpdateEstimate').trigger('click');
}
function ApplyFilterThenSearch() {
    $(".searchicon").addClass('filterApplied');
    CloseAdvanceSearch();
    BindOrReloadEstimateTable('Search');
}

function SaveSuccessEstimate(data, status) {
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
                $("#divEstimateForm").load("Estimate/EstimateForm?id=" + _result.ID, function () {
                    ChangeButtonPatchView("Estimate", "btnPatchEstimateNew", "Edit");
                    BindEstimateDetailList(_result.ID);
                    clearUploadControl();
                    PaintImages(_result.ID);
                });
                ChangeButtonPatchView("Estimate", "btnPatchEstimateNew", "Edit");
                BindOrReloadEstimateTable('Init');
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

function DeleteEstimate() {
    debugger;
    notyConfirm('Are you sure to delete?', 'DeleteEstimateItem("' + $('#EstimateForm #ID').val() + '")');
}
function DeleteEstimateItem(id) {
    debugger;
    try {
        if (id) {
            var data = { "id": id };
            _jsonData = GetDataFromServer("Estimate/DeleteEstimate/", data);
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
                    ChangeButtonPatchView("Estimate", "btnPatchEstimateNew", "Add");
                    ResetEstimate();
                    BindOrReloadEstimateTable('Init');
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

function BindEstimateDetailList(id,IsEnquiry) {
    debugger;
    _dataTable.EstimateDetailList = $('#tblEstimateDetails').DataTable(
         {
             dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
             order: [],
             searching: false,
             paging: false,
             ordering: false,
             bInfo: false,
             data: !IsEnquiry ? id == _emptyGuid ? null : GetEstimateDetailListByEstimateID(id,false) : GetEstimateDetailListByEstimateID(id,true),
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
             //{ "data": "Unit.Description", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
             { "data": "CostRate", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
             { "data": "SellingRate", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
            { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink"  onclick="EditEstimateDetail(this)" ><i class="fa fa-pencil-square-o" aria-hidden="true"></i></a> <a href="#" class="DeleteLink"  onclick="ConfirmDeleteEstimateDetail(this)" ><i class="fa fa-trash-o" aria-hidden="true"></i></a>' },
             ],
             columnDefs: [
                 { "targets": [0, 1], "width": "10%" },
                 { "targets": [3], "width": "30%" },
                 { "targets": [,5,6], "width": "10%" },
                 { className: "text-left", "targets": [0,1,2,3,4] },
                 { className: "text-right", "targets": [5,6] }
             ],
             destroy:true
         });
}

function GetEstimateDetailListByEstimateID(id,IsEnquiry) {
    try {
        debugger;
       
        var estimateDetailList = [];
        if (IsEnquiry)
        {
            var data = { "enquiryID": $('#EstimateForm #hdnEnquiryID').val() };
            _jsonData = GetDataFromServer("Estimate/GetEstimateDetailListByEstimateIDWithEnquiry/", data);
        }
        else {
            var data = { "estimateID": id };
            _jsonData = GetDataFromServer("Estimate/GetEstimateDetailListByEstimateID/", data);
        }
       
        if (_jsonData != '') {
            _jsonData = JSON.parse(_jsonData);
            _message = _jsonData.Message;
            _status = _jsonData.Status;
            estimateDetailList = _jsonData.Records;
        }
        if (_status == "OK") {
            return estimateDetailList;
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

function AddEstimateDetailList() {
    debugger;
    $("#divModelEstimatePopBody").load("Estimate/AddEstimateDetail", function () {
        $('#lblModelPopEstimate').text('Estimate Detail')
        $('#divModelPopEstimate').modal('show');
    });
}

function AddEstimateDetailToList() {
    debugger;
    $("#FormEstimateDetail").submit(function () {
        debugger;
        if ($('#FormEstimateDetail #IsUpdate').val() == 'True') {
            debugger;
            if ($('#ProductID').val() != "") {
                debugger;
                var estimateDetailList = _dataTable.EstimateDetailList.rows().data();
                estimateDetailList[_datatablerowindex].Product.Code = $("#ProductID").val() != "" ? $("#ProductID option:selected").text().split("-")[0].trim() : "";
                estimateDetailList[_datatablerowindex].Product.Name = $("#ProductID").val() != "" ? $("#ProductID option:selected").text().split("-")[1].trim() : "";
                estimateDetailList[_datatablerowindex].ProductID = $("#ProductID").val() != "" ? $("#ProductID").val() : _emptyGuid;
                estimateDetailList[_datatablerowindex].ProductModelID = $("#ProductModelID").val() != "" ? $("#ProductModelID").val() : _emptyGuid;
                ProductModel = new Object;
                Unit = new Object;
                ProductModel.Name = $("#ProductModelID").val() != "" ? $("#ProductModelID option:selected").text() : "";
                estimateDetailList[_datatablerowindex].ProductModel = ProductModel;
                estimateDetailList[_datatablerowindex].ProductSpec = $('#ProductSpec').val();
                estimateDetailList[_datatablerowindex].Qty = $('#Qty').val();
                estimateDetailList[_datatablerowindex].UnitCode = $('#UnitCode').val();
                Unit.Description = $("#UnitCode").val() != "" ? $("#UnitCode option:selected").text().trim() : "";
                estimateDetailList[_datatablerowindex].Unit = Unit;
                estimateDetailList[_datatablerowindex].CostRate = $('#CostRate').val();
                estimateDetailList[_datatablerowindex].SellingRate = $('#SellingRate').val();
                _dataTable.EstimateDetailList.clear().rows.add(estimateDetailList).draw(false);
                $('#divModelPopEstimate').modal('hide');
                _datatablerowindex = -1;
            }
        }
        else {
            if ($('#ProductID').val() != "")
                if (_dataTable.EstimateDetailList.rows().data().length === 0) {
                    _dataTable.EstimateDetailList.clear().rows.add(GetEstimateDetailListByEstimateID(_emptyGuid)).draw(false);
                    debugger;
                    var estimateDetailList = _dataTable.EstimateDetailList.rows().data();
                    estimateDetailList[0].Product.Code = $("#ProductID").val() != "" ? $("#ProductID option:selected").text().split("-")[0].trim() : "";
                    estimateDetailList[0].Product.Name = $("#ProductID").val() != "" ? $("#ProductID option:selected").text().split("-")[1].trim() : "";
                    estimateDetailList[0].ProductID = $("#ProductID").val() != "" ? $("#ProductID").val() : _emptyGuid;
                    estimateDetailList[0].ProductModelID = $("#ProductModelID").val() != "" ? $("#ProductModelID").val() : _emptyGuid;
                    estimateDetailList[0].ProductModel.Name = $("#ProductModelID").val() != "" ? $("#ProductModelID option:selected").text() : "";
                    estimateDetailList[0].ProductSpec = $('#ProductSpec').val();
                    estimateDetailList[0].Qty = $('#Qty').val();
                    estimateDetailList[0].UnitCode = $('#UnitCode').val();
                    estimateDetailList[0].Unit.Description = $("#UnitCode").val() != "" ? $("#UnitCode option:selected").text().trim() : "";
                    estimateDetailList[0].CostRate = $('#CostRate').val();
                    estimateDetailList[0].SellingRate = $('#SellingRate').val();
                    _dataTable.EstimateDetailList.clear().rows.add(estimateDetailList).draw(false);
                    $('#divModelPopEstimate').modal('hide');
                }
                else {
                    debugger;
                    var EstimateDetailVM = new Object();
                    var Product = new Object;
                    var ProductModel = new Object()
                    var Unit = new Object();
                    EstimateDetailVM.ID = _emptyGuid;
                    EstimateDetailVM.ProductID = $("#ProductID").val() != "" ? $("#ProductID").val() : _emptyGuid;
                    Product.Code = $("#ProductID").val() != "" ? $("#ProductID option:selected").text().split("-")[0].trim() : "";
                    Product.Name = $("#ProductID").val() != "" ? $("#ProductID option:selected").text().split("-")[1].trim() : "";
                    EstimateDetailVM.Product = Product;
                    EstimateDetailVM.ProductModelID = $("#ProductModelID").val() != "" ? $("#ProductModelID").val() : _emptyGuid;
                    ProductModel.Name = $("#ProductModelID").val() != "" ? $("#ProductModelID option:selected").text() : "";
                    EstimateDetailVM.ProductModel = ProductModel;
                    EstimateDetailVM.ProductSpec = $('#ProductSpec').val();
                    EstimateDetailVM.Qty = $('#Qty').val();
                    Unit.Description = $("#UnitCode").val() != "" ? $("#UnitCode option:selected").text().trim() : "";
                    EstimateDetailVM.Unit = Unit;
                    EstimateDetailVM.UnitCode = $('#UnitCode').val();
                    EstimateDetailVM.CostRate = $('#CostRate').val();
                    EstimateDetailVM.SellingRate = $('#SellingRate').val();
                    _dataTable.EstimateDetailList.row.add(EstimateDetailVM).draw(true);
                    $('#divModelPopEstimate').modal('hide');
                }
        }

    });
}

function EditEstimateDetail(this_Obj) {
    debugger;
    _datatablerowindex = _dataTable.EstimateDetailList.row($(this_Obj).parents('tr')).index();
    var estimateDetail = _dataTable.EstimateDetailList.row($(this_Obj).parents('tr')).data();
    $("#divModelEstimatePopBody").load("Estimate/AddEstimateDetail", function () {
        $('#lblModelPopEstimate').text('Estimate Detail')
        $('#FormEstimateDetail #IsUpdate').val('True');
        $('#FormEstimateDetail #ID').val(estimateDetail.ID);
        $("#FormEstimateDetail #ProductID").val(estimateDetail.ProductID)
        $("#FormEstimateDetail #hdnProductID").val(estimateDetail.ProductID)
        $('#divProductBasicInfo').load("Product/ProductBasicInfo?ID=" + $('#hdnProductID').val(), function () {
        });

        if ($('#hdnProductID').val() != _emptyGuid) {
            $('.divProductModelSelectList').load("ProductModel/ProductModelSelectList?required=required&productID=" + $('#hdnProductID').val())
        }
        else {
            $('.divProductModelSelectList').empty();
            $('.divProductModelSelectList').append('<span class="form-control newinput"><i id="dropLoad" class="fa fa-spinner"></i></span>');
        }
        $("#FormEstimateDetail #ProductModelID").val(estimateDetail.ProductModelID);
        $("#FormEstimateDetail #hdnProductModelID").val(estimateDetail.ProductModelID);
        if ($('#hdnProductModelID').val() != _emptyGuid) {
            $('#divProductBasicInfo').load("ProductModel/ProductModelBasicInfo?ID=" + $('#hdnProductModelID').val(), function () {
            });
        }
        $('#FormEstimateDetail #ProductSpec').val(estimateDetail.ProductSpec);
        $('#FormEstimateDetail #Qty').val(estimateDetail.Qty);
        $('#FormEstimateDetail #UnitCode').val(estimateDetail.UnitCode);
        $('#FormEstimateDetail #hdnUnitCode').val(estimateDetail.UnitCode);
        $('#FormEstimateDetail #CostRate').val(estimateDetail.CostRate);
        $('#FormEstimateDetail #SellingRate').val(estimateDetail.SellingRate);
        $('#divModelPopEstimate').modal('show');
    });
}

function ConfirmDeleteEstimateDetail(this_Obj) {
    debugger;
    _datatablerowindex = _dataTable.EstimateDetailList.row($(this_Obj).parents('tr')).index();
    var estimateDetail = _dataTable.EstimateDetailList.row($(this_Obj).parents('tr')).data();
    if (estimateDetail.ID === _emptyGuid) {
        var estimateDetailList = _dataTable.EstimateDetailList.rows().data();
        estimateDetailList.splice(_datatablerowindex, 1);
        _dataTable.EstimateDetailList.clear().rows.add(estimateDetailList).draw(false);
        notyAlert('success', 'Detail Row deleted successfully');
    }
    else {
        notyConfirm('Are you sure to delete?', 'DeleteEstimateDetail("' + estimateDetail.ID + '")');

    }
}
function DeleteEstimateDetail(ID) {
    debugger;
    if (ID != _emptyGuid && ID != null && ID != '') {
        var data = { "id": ID };
        var ds = {};
        _jsonData = GetDataFromServer("Estimate/DeleteEstimateDetail/", data);
        if (_jsonData != '') {
            _jsonData = JSON.parse(_jsonData);
            _message = _jsonData.Message;
            _status = _jsonData.Status;
            _result = _jsonData.Record;
        }
        if (_status == "OK") {
            notyAlert('success', _result.Message);
            var estimateDetailList = _dataTable.EstimateDetailList.rows().data();
            estimateDetailList.splice(_datatablerowindex, 1);
            _dataTable.EstimateDetailList.clear().rows.add(estimateDetailList).draw(false);
        }
        if (_status == "ERROR") {
            notyAlert('error', _message);
        }
    }
}