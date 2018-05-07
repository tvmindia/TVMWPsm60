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
        BindOrReloadProductionOrderTable('Init');
        $('#tblProductionOrder tbody').on('dblclick', 'td', function () {
            EditProductionOrder(this);
        });
    }
    catch (e) {
        console.log(e.message);
    }
});

//function bind the ProductionOrder list checking search and filter
function BindOrReloadProductionOrderTable(action) {
    try {
        debugger;
        //creating advancesearch object
        ProductionOrderAdvanceSearchViewModel = new Object();
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
        ProductionOrderAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
        ProductionOrderAdvanceSearchViewModel.SearchTerm = $('#SearchTerm').val();
        ProductionOrderAdvanceSearchViewModel.FromDate = $('#FromDate').val();
        ProductionOrderAdvanceSearchViewModel.ToDate = $('#ToDate').val();
        //apply datatable plugin on ProductionOrder table
        _dataTable.ProductionOrderList = $('#tblProductionOrder').DataTable(
        {
            dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
            buttons: [{
                extend: 'excel',
                exportOptions:
                             {
                                 columns: [0, 1, 2, 3, 4, 5]
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
                url: "ProductionOrder/GetAllProductionOrder/",
                data: { "ProductionOrderAdvanceSearchVM": ProductionOrderAdvanceSearchViewModel },
                type: 'POST'
            },
            pageLength: 13,
            columns: [
               { "data": "ProdOrderNo", "defaultContent": "<i>-</i>" },
               { "data": "ProdOrderDateFormatted", "defaultContent": "<i>-</i>" },
               { "data": "Customer.CompanyName", "defaultContent": "<i>-</i>" },
               { "data": "Branch.Description", "defaultContent": "<i>-</i>" },
               { "data": "DocumentStatus.Description", "defaultContent": "<i>-</i>" },
               //{ "data": "UserName", "defaultContent": "<i>-</i>" },
               {
                   "data": "IsFinalApproved", render: function (data, type, row) {
                       if (data) {
                           return "Approved ✔";// <br/>📅 " + (row.FinalApprovalDateFormatted !== null ? row.FinalApprovalDateFormatted : "-");
                       }
                       else {
                           return 'Pending';
                       }

                   }, "defaultContent": "<i>-</i>"
               },
               { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink"  onclick="EditProductionOrder(this)" ><i class="fa fa-pencil-square-o" aria-hidden="true"></i></a>' },
            ],
            columnDefs: [
                          { className: "text-left", "targets": [0, 2, 3, 4, 5, 6] },
                          { className: "text-center", "targets": [1] },
                            { "targets": [0], "width": "10%" },
                            { "targets": [2], "width": "20%" },
                            { "targets": [1, 3, 4], "width": "10%" },
                            { "targets": [5], "width": "7%" },
                            { "targets": [6], "width": "7%" },

            ],
            destroy: true,
            //for performing the import operation after the data loaded
            initComplete: function (settings, json) {
                debugger;
                $('.dataTables_wrapper div.bottom div').addClass('col-md-6');
                $('#tblProductionOrder').fadeIn('slow');
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
                    BindOrReloadProductionOrderTable();
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
function ResetProductionOrderList() {
    $(".searchicon").removeClass('filterApplied');
    BindOrReloadProductionOrderTable('Reset');
}
//function export data to excel
function ExportProductionOrderData() {
    debugger;
    $('.excelExport').show();
    OnServerCallBegin();
    BindOrReloadProductionOrderTable('Export');
}
// add ProductionOrder section
function AddProductionOrder() {
    debugger;
    //this will return form body(html)
    //OnServerCallBegin();
    $("#divProductionOrderForm").load("ProductionOrder/ProductionOrderForm?id=" + _emptyGuid ,function () {
        ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "Add");
       // BindProductionOrderDetailList(_emptyGuid);
        //BindProductionOrderOtherChargesDetailList(_emptyGuid)
         OnServerCallComplete();
        setTimeout(function () {
            //resides in customjs for sliding
            openNav();
        }, 100);
    });
}

function EditProductionOrder(this_Obj) {
    debugger;
    OnServerCallBegin();
    var productionOrder = _dataTable.ProductionOrderList.row($(this_Obj).parents('tr')).data();
    //this will return form body(html)
    $("#divProductionOrderForm").load("ProductionOrder/ProductionOrderForm?id=" + productionOrder.ID , function () {

        ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "Edit");
       // BindProductionOrderDetailList(productionOrder.ID);
        $('#divCustomerBasicInfo').load("Customer/CustomerBasicInfo?ID=" + $('#hdnCustomerID').val());
        clearUploadControl();
        PaintImages(productionOrder.ID);
        OnServerCallComplete();
        //resides in customjs for sliding
      
            //$("#divEstimateForm #EnquiryID").prop('disabled', true);
            openNav();
      
    });
}

function ResetProductionOrder() {
    //this will return form body(html)
    $("#divProductionOrderForm").load("ProductionOrder/ProductionOrderForm?id=" + $('#ProductionOrderForm #ID').val() , function () {
        if ($('#ID').val() != _emptyGuid && $('#ID').val() != null) {
            //resides in customjs for sliding
            setTimeout(function () {
                //$("#divEstimateForm #EnquiryID").prop('disabled', true);
                openNav();
            }, 100);
        }
       // BindProductionOrderDetailList($('#ID').val(), false);
        clearUploadControl();
        PaintImages($('#ProductionOrderForm #ID').val());
        $('#divCustomerBasicInfo').load("Customer/CustomerBasicInfo?ID=" + $('#EstimateForm #hdnCustomerID').val());
    });
}

function SaveProductionOrder() {
    var productionOrderDetailList = _dataTable.ProductionOrderList.rows().data().toArray();
    $('#DetailJSON').val(JSON.stringify(productionOrderDetailList));
    $('#btnInsertUpdateProductionOrder').trigger('click');
}

function ApplyFilterThenSearch() {
    $(".searchicon").addClass('filterApplied');
    CloseAdvanceSearch();
    BindOrReloadProductionOrderTable('Search');
}

function SaveSuccessProductionOrder(data, status) {
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
                $("#divProductionOrderForm").load("ProductionOrder/ProductionOrderForm?id=" + _result.ID , function () {
                    ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "Edit");
                   // BindProductionOrderDetailList(_result.ID);
                   // clearUploadControl();
                   // PaintImages(_result.ID);
                });
                ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "Edit");
                BindOrReloadProductionOrderTable('Init');
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

function BindProductionOrderDetailList(id) {
    debugger;
    _dataTable.EnquiryDetailList = $('#tblEnquiryDetails').DataTable(
         {
             dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
             order: [],
             searching: false,
             paging: false,
             ordering: false,
             bInfo: false,
             data: id == _emptyGuid ? null : GetEnquiryDetailListByEnquiryID(id),
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
             { "data": null, "orderable": false, "defaultContent": '<a href="#" class="DeleteLink"  onclick="ConfirmDeleteEnquiryDetail(this)" ><i class="fa fa-trash-o" aria-hidden="true"></i></a> <a href="#" class="actionLink"  onclick="EditEnquiryDetail(this)" ><i class="fa fa-pencil-square-o" aria-hidden="true"></i></a>' },
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

function AddProductionOrderDetailList() {
    debugger;
    $("#divModelProductionOrderPopBody").load("ProductionOrder/AddProductionOrderDetail", function () {
        $('#lblModelPopProductionOrder').text('ProductionOrder Detail')
        $('#divModelPopProductionOrder').modal('show');
    });
}