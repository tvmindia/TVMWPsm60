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