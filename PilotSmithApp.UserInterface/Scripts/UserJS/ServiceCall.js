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
        debugger;
        BindOrReloadServiceCallTable('Init');
        $('#tblServiceCall tbody').on('dblclick', 'td', function () {
            //EditServiceCall(this);
        });
    }
    catch (e) {
        console.log(e.message);
    }
});
function BindOrReloadServiceCallTable(action) {
    try {
        debugger;
        //creating advancesearch object
        ServiceCallAdvanceSearchViewModel = new Object();
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
        ServiceCallAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
        ServiceCallAdvanceSearchViewModel.SearchTerm = $('#SearchTerm').val();
        ServiceCallAdvanceSearchViewModel.FromDate = $('#FromDate').val();
        ServiceCallAdvanceSearchViewModel.ToDate = $('#ToDate').val();
        //apply datatable plugin on ServiceCall table
        _dataTable.ServiceCallList = $('#tblServiceCall').DataTable(
        {
            dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
            buttons: [{
                extend: 'excel',
                exportOptions:
                             {
                                 columns: [0, 1, 2, 3]
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
                url: "ServiceCall/GetAllServiceCall/",
                data: { "serviceCallAdvanceSearchVM": ServiceCallAdvanceSearchViewModel },
                type: 'POST'
            },
            pageLength: 13,
            columns: [
               { "data": "ServiceCallNo", "defaultContent": "<i>-</i>" },
               { "data": "ServiceCallDateFormatted", "defaultContent": "<i>-</i>" },
               { "data": "Customer.CompanyName", "defaultContent": "<i>-</i>" },
               { "data": "Employee.Name", "defaultContent": "<i>-</i>" },
               { "data": "DocumentStatus.Description", "defaultContent": "<i>-</i>" },             
               { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink"  onclick="EditServiceCall(this)" ><i class="fa fa-pencil-square-o" aria-hidden="true"></i></a>' },
            ],
            columnDefs: [
                          { className: "text-left", "targets": [0, 2,3,4] },
                          { className: "text-center", "targets": [1] },                           

            ],
            destroy: true,
            //for performing the import operation after the data loaded
            initComplete: function (settings, json) {
                debugger;
                $('.dataTables_wrapper div.bottom div').addClass('col-md-6');
                $('#tblServiceCall').fadeIn('slow');
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
                    BindOrReloadServiceCallTable();
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
function ResetServiceCallList() {
    $(".searchicon").removeClass('filterApplied');
    BindOrReloadServiceCallTable('Reset');
}
//function export data to excel
function ExportServiceCallData() {
    debugger;
    $('.excelExport').show();
    OnServerCallBegin();
    BindOrReloadServiceCallTable('Export');
}
// add ProductionOrder section
function AddServiceCall() {
    debugger;
    //this will return form body(html)
    //OnServerCallBegin();
    $("#divServiceCallForm").load("ServiceCall/ServiceCallForm?id=" + _emptyGuid, function () {
        ChangeButtonPatchView("ServiceCall", "btnPatchServiceCallNew", "Add");
        BindServiceCallChargeDetailList(_emptyGuid);
        //BindServiceCallDetailList("00000000-0000-0000-0000-000000000000", true);
        // BindProductionOrderDetailList(_emptyGuid);
        //BindProductionOrderOtherChargesDetailList(_emptyGuid)
        // OnServerCallComplete();
        setTimeout(function () {
            //resides in customjs for sliding
            openNav();
        }, 100);
    });
}

function AddServiceCallDetailList() {
    debugger;
    $("#divModelServiceCallPopBody").load("ServiceCall/AddServiceCallDetail", function () {
        $('#lblModelServiceCall').text('ServiceCall Detail')
        $('#divModelPopServiceCall').modal('show');
    });
}

//Bind ServiceCallCharges Table
function BindServiceCallChargeDetailList(id) {
    debugger;
    _dataTable.ServiceCallChargeDetailList = $('#tblServiceCallChargeDetailList').DataTable(
         {
             dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
             order: [],
             searching: false,
             paging: false,
             ordering: false,
             bInfo: false,
             data: id == _emptyGuid ? null : GetQuotationOtherChargesDetailListByServiceCallID(id),
             language: {
                 search: "_INPUT_",
                 searchPlaceholder: "Search"
             },
             columns: [
             { "data": "OtherCharge.Description", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
             { "data": "ChargeAmount", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
             {
                 "data": "ChargeAmount", render: function (data, type, row) {
                     debugger;
                     var CGST = parseFloat(row.CGSTPerc != "" ? row.CGSTPerc : 0);
                     var SGST = parseFloat(row.SGSTPerc != "" ? row.SGSTPerc : 0);
                     var IGST = parseFloat(row.IGSTPerc != "" ? row.IGSTPerc : 0);
                     var CGSTAmt = parseFloat(data * CGST / 100);
                     var SGSTAmt = parseFloat(data * SGST / 100)
                     var IGSTAmt = parseFloat(data * IGST / 100)
                     var GSTAmt = roundoff(parseFloat(CGSTAmt) + parseFloat(SGSTAmt) + parseFloat(IGSTAmt))
                     return '<div class="show-popover text-right" data-html="true" data-toggle="popover" data-title="<p align=left>Total GST : ₹ ' + GSTAmt + '" data-content=" SGST ' + SGST + '% : ₹ ' + roundoff(parseFloat(SGSTAmt)) + '<br/>CGST ' + CGST + '% : ₹ ' + roundoff(parseFloat(CGSTAmt)) + '<br/> IGST ' + IGST + '% : ₹ ' + roundoff(parseFloat(IGSTAmt)) + '</p>"/>' + GSTAmt
                 }, "defaultContent": "<i></i>"
             },
             { "data": "AddlTaxAmt", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
             {
                 "data": "ChargeAmount", render: function (data, type, row) {
                     var CGST = parseFloat(row.CGSTPerc != "" ? row.CGSTPerc : 0);
                     var SGST = parseFloat(row.SGSTPerc != "" ? row.SGSTPerc : 0);
                     var IGST = parseFloat(row.IGSTPerc != "" ? row.IGSTPerc : 0);
                     var CGSTAmt = parseFloat(data * CGST / 100);
                     var SGSTAmt = parseFloat(data * SGST / 100)
                     var IGSTAmt = parseFloat(data * IGST / 100)
                     var GSTAmt = roundoff(parseFloat(CGSTAmt) + parseFloat(SGSTAmt) + parseFloat(IGSTAmt))
                     var Total = roundoff(parseFloat(data) + parseFloat(GSTAmt))
                     //return Total
                     return '<div class="show-popover text-right" data-html="true" data-toggle="popover" data-title="<p align=left>Total : ₹ ' + Total + '" data-content="Charge Amount : ₹ ' + data + '<br/>GST : ₹ ' + GSTAmt + '</p>"/>' + Total
                 }, "defaultContent": "<i></i>"
             },
             { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink"  onclick="EditQuotationOtherChargesDetail(this)" ><i class="fa fa-pencil-square-o" aria-hidden="true"></i></a> <a href="#" class="DeleteLink"  onclick="ConfirmDeleteQuotationOtherChargeDetail(this)" ><i class="fa fa-trash-o" aria-hidden="true"></i></a>' },
             ],
             columnDefs: [
                 //{ "targets": [0], "width": "30%" },
                 //{ "targets": [1, 2], "width": "20%" },
                 //{ "targets": [3], "width": "20%" },
                 { className: "text-right", "targets": [1, 2, 3, 4] },
                 { className: "text-left", "targets": [0] },
                 { className: "text-center", "targets": [5] }
             ],
             destroy: true,
         });
    $('[data-toggle="popover"]').popover({
        html: true,
        'trigger': 'hover',
        'placement': 'top'
    });
}

function AddServiceCallChargeDetail() {
    try {
        debugger;
        $("#divModelCallChargesPopBody").load("ServiceCall/AddServiceCallCharge", function () {
            $('#lblModelPopCallCharges').text('ServiceCall Charges')
            $('#divModelPopCallCharges').modal('show');
        });
    }
    catch (e) {
        console.log(e.message);
    }
}