var _dataTable = {};
var _emptyGuid = "00000000-0000-0000-0000-000000000000";
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
        //apply datatable plugin on Enquiry table
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
            //serverSide: true,
            //ajax: {
            //    url: "Estimate/GetAllEstimate/",
            //    data: { "EnquiryAdvanceSearchVM": EnquiryAdvanceSearchViewModel },
            //    type: 'POST'
            //},
            pageLength: 13,
            columns: [
               { "data": "EstimateNo", "defaultContent": "<i>-</i>" },
               { "data": "EstimateRefNo", "defaultContent": "<i>-</i>" },
               { "data": "EstimateDateFormatted", "defaultContent": "<i>-</i>" },
               { "data": "EnquiryID", "defaultContent": "<i>-</i>" },
               { "data": "CustomerID", "defaultContent": "<i>-</i>" },
               { "data": "DocumentStatusCode", "defaultContent": "<i>-</i>" },
               { "data": "ValidUpToDateFormatted", "defaultContent": "<i>-</i>" },
               { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink"  onclick="EditEstimate(this)" ><i class="fa fa-pencil-square-o" aria-hidden="true"></i></a>' },
            ],
            columnDefs: [{ className: "text-right", "targets": [5] },
                  { className: "text-left", "targets": [0, 1] },
            { className: "text-center", "targets": [2, 3, 4] }
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
    $("#divEstimateForm").load("Estimate/EstimateForm?id=" + _emptyGuid, function () {
        ChangeButtonPatchView("Estimate", "btnPatchEstimateNew", "Add");
        BindEstimateDetailList(_emptyGuid);
        //resides in customjs for sliding
        openNav();
    });
}

function EditEstimate(this_Obj) {
    var Estimate = _dataTable.EstimateList.row($(this_Obj).parents('tr')).data();
    //this will return form body(html)
    $("#divEstimateForm").load("Estimate/EstimateForm?id=" + Estimate.ID, function () {
        //$('#CustomerID').trigger('change');
        ChangeButtonPatchView("Estimate", "btnPatchEstimateNew", "Edit");
        BindEstimateDetailList(Estimate.ID);

        //resides in customjs for sliding
        openNav();
    });
}

function ResetEstimate() {
    //this will return form body(html)
    $('#divEstimateForm').load("Estimate/EstimateForm?id=" + $('#ID').val());
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
    notyConfirm('Are you sure to delete?', 'DeleteItem("' + $('#ID').val() + '")');
}
function DeleteItem(id) {
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

function BindEstimateDetailList(id) {
    debugger;
    _dataTable.EstimateDetailList = $('#tblEstimateDetails').DataTable(
         {
             dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
             order: [],
             searching: false,
             paging: false,
             ordering: false,
             bInfo: false,
             data: id == _emptyGuid ? null : GetEstimateDetailListByEstimateID(id),
             language: {
                 search: "_INPUT_",
                 searchPlaceholder: "Search"
             },
             columns: [ //----Dummy names just for information.---//
             { "data": "Product", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },        
             { "data": "Model", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
             { "data": "Spec", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },             
             {
                 "data": "Qty", render: function (data, type, row) {
                     return data + " " + row.UnitCode
                 }, "defaultContent": "<i></i>"
             },
             { "data": "Unit", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
             { "data": "CostPrice", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
             { "data": "SellingPrice", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
            { "data": null, "orderable": false, "defaultContent": '<a href="#" class="DeleteLink"  onclick="DeleteClick(this)" ><i class="glyphicon glyphicon-trash" aria-hidden="true"></i></a> | <a href="#" class="actionLink"  onclick="ProductEdit(this)" ><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' },
             ],
             columnDefs: [
                 //{ "targets": [0, 4], "width": "10%" },
                 //{ "targets": [1, 2], "width": "15%" },
                 //{ "targets": [3], "width": "35%" },
                 //{ "targets": [5], "width": "10%" },
                 //{ "targets": [6], "width": "5%" },
                 //{ className: "text-right", "targets": [4, 5] },
                 //{ className: "text-left", "targets": [1, 2, 3] },
                 { className: "text-center", "targets": [0, 6] }
             ]
         });
}

function GetEstimateDetailListByEstimateID(id) {
    try {
        debugger;
        var data = { "estimateID": id };
        var estimateDetailList = [];
      //  _jsonData = GetDataFromServer("Estimate/GetEstimateDetailListByEstimateID/", data);
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
    $("#divModelEstimatePopBody").load("Estimate/AddEstimateDetail", function () {
        $('#lblModelPopEstimate').text('Estimate Detail')
        $('#divModelPopEstimate').modal('show');
    });
}

function AddEstimateDetailToList() {
    debugger;
    $("#FormEstimateDetail").submit(function () {
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
                estimateDetailList[0].UnitCode = $('#UnitCode').val()!=""?$('#UnitCode option:selected').text():"";
                estimateDetailList[0].CostRate = $('#CostRate').val();
                estimateDetailList[0].SellingRate = $('#SellingRate').val();
                _dataTable.EstimateDetailList.clear().rows.add(estimateDetailList).draw(false);
                $('#divModelPopEstimate').modal('hide');
            }
            else {
                var EstimateDetailVM = new Object();
                var Product = new Object;
                var ProductModel = new Object()
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
                EstimateDetailVM.UnitCode = $('#UnitCode').val();
                EstimateDetailVM.CostRate = $('#CostRate').val();
                EstimateDetailVM.SellingRate = $('#SellingRate').val();
                _dataTable.EstimateDetailList.row.add(EstimateDetailVM).draw(true);
                $('#divModelPopEstimate').modal('hide');
            }
    });
}