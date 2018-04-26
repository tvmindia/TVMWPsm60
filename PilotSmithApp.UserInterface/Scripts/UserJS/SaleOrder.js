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
        BindOrReloadSaleOrderTable('Init');
        $('#tblSaleOrder tbody').on('dblclick', 'td', function () {
            //EditSaleOrder(this);
        });
    }
    catch (e) {
        console.log(e.message);
    }
});

//function bind the SaleOrder list checking search and filter
function BindOrReloadSaleOrderTable(action) {
    try {
        //creating advancesearch object
        SaleOrderAdvanceSearchViewModel = new Object();
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
        SaleOrderAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
        SaleOrderAdvanceSearchViewModel.SearchTerm = $('#SearchTerm').val();
        SaleOrderAdvanceSearchViewModel.FromDate = $('#FromDate').val();
        SaleOrderAdvanceSearchViewModel.ToDate = $('#ToDate').val();
        //apply datatable plugin on SaleOrder table
        _dataTable.SaleOrderList = $('#tblSaleOrder').DataTable(
        {
            dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
            buttons: [{
                extend: 'excel',
                exportOptions:
                             {
                                 columns: [0, 1, 2, 3, 4, 5, 6]
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
            //ajax: {
            //    url: "SaleOrder/GetAllSaleOrder/",
            //    data: { "SaleOrderAdvanceSearchVM": SaleOrderAdvanceSearchViewModel },
            //    type: 'POST'
            //},
            pageLength: 13,
            columns: [
               { "data": "SaleOrderNo", "defaultContent": "<i>-</i>" },
               { "data": "SaleOrderDateFormatted", "defaultContent": "<i>-</i>" },
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
               { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink"  onclick="EditSaleOrder(this)" ><i class="fa fa-pencil-square-o" aria-hidden="true"></i></a>' },
            ],
            columnDefs: [
                          { className: "text-left", "targets": [2, 3, 5, 6] },
                          { className: "text-center", "targets": [0, 1, 4, 7] },
                            { "targets": [0, 1, 4], "width": "10%" },
                            { "targets": [2, 3], "width": "10%" },
                            { "targets": [5], "width": "30%" },
                            { "targets": [6], "width": "10%" },
                            { "targets": [7], "width": "5%" },
            ],
            destroy: true,
            //for performing the import operation after the data loaded
            initComplete: function (settings, json) {
                debugger;
                $('.dataTables_wrapper div.bottom div').addClass('col-md-6');
                $('#tblSaleOrder').fadeIn('slow');
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
                    BindOrReloadSaleOrderTable();
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
function ResetSaleOrderList() {
    $(".searchicon").removeClass('filterApplied');
    BindOrReloadSaleOrderTable('Reset');
}
//function export data to excel
function ExportSaleOrderData() {
    $('.excelExport').show();
   // OnServerCallBegin();
    BindOrReloadSaleOrderTable('Export');
}
// add SaleOrder section
function AddSaleOrder() {
    debugger;
    //this will return form body(html)
    //OnServerCallBegin();
    $("#divSaleOrderForm").load("SaleOrder/SaleOrderForm?id=" + _emptyGuid + "&quotationID=", function () {
        ChangeButtonPatchView("SaleOrder", "btnPatchSaleOrderNew", "Add");
       // BindSaleOrderDetailList(_emptyGuid);
        //BindSaleOrderOtherChargesDetailList(_emptyGuid)
       // OnServerCallComplete();
        setTimeout(function () {
            //resides in customjs for sliding
            openNav();
        }, 100);
    });
}

function AddSaleOrderDetailList() {
    debugger;
    $("#divModelSaleOrderPopBody").load("SaleOrder/AddSaleOrderDetail", function () {
        $('#lblModelPopSaleOrder').text('SaleOrder Detail')
        $('#divModelPopSaleOrder').modal('show');
    });
}