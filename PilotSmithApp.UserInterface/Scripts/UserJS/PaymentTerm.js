var _dataTables = {};
var _emptyGuid = "00000000-0000-0000-0000-000000000000";
var _jsonData = {};
var _message = "";
var _status = "";
var _result = "";

//---------------------------------------Docuement Ready--------------------------------------------------//

$(document).ready(function () {
    try {
        BindOrReloadPaymentTermTable('Init');
        $('#tblPaymentTerm tbody').on('dblclick', 'td', function () {
            EditPaymentTermMaster(this);
        });
    }
    catch (e) {
        console.log(e.message);
    }
});

//function bind the PaymentTerm list checking search and filter
function BindOrReloadPaymentTermTable(action) {
    try {
        debugger;
        //creating advancesearch object
        PaymentTermAdvanceSearchViewModel = new Object();
        DataTablePagingViewModel = new Object();
        DataTablePagingViewModel.Length = 0;
        var SearchValue = $('#hdnSearchTerm').val();
        var SearchTerm = $('#SearchTerm').val();
        $('#hdnSearchTerm').val($('#SearchTerm').val())
        //switch case to check the operation
        switch (action) {
            case 'Reset':
                $('#SearchTerm').val('');
                break;
            case 'Init':
                break;
            case 'Search':
                if (SearchTerm == SearchValue) {
                    return false;
                }
                break;
                break;
            case 'Export':
                DataTablePagingViewModel.Length = -1;
                break;
            default:
                break;
        }
        PaymentTermAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
        PaymentTermAdvanceSearchViewModel.SearchTerm = $('#SearchTerm').val();
        //apply datatable plugin on PaymentTerm table
        _dataTables.PaymentTermList = $('#tblPaymentTerm').DataTable(
            {
                dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
                buttons: [{
                    extend: 'excel',
                    exportOptions:
                                 {
                                     columns: [0,1,2]
                                 }
                }],
                ordering: false,
                searching: false,
                paging: true,
                lengthChange: false,
                processing: true,
                autoWidth: false,
                language: {

                    "processing": "<div class='spinner'><div class='bounce1'></div><div class='bounce2'></div><div class='bounce3'></div></div>"
                },
                serverSide: true,
                ajax: {

                    url: "PaymentTerm/GetAllPaymentTerm",
                    data: { "PaymentTermAdvanceSearchVM": PaymentTermAdvanceSearchViewModel },
                    type: 'POST'
                },
                pageLength: 10,
                columns: [
                { "data": "Description", "defaultContent": "<i>-</i>" },
                { "data": "NoOfDays", "defaultContent": "<i>-</i>" },
                {
                    "data": null, "orderable": false, "defaultContent": '<a href="#" onclick="EditPaymentTermMaster(this)"<i class="fa fa-pencil-square-o" aria-hidden="true"></i></a>  <a href="#" onclick="DeletePaymentTermMaster(this)"<i class="fa fa-trash-o" aria-hidden="true"></i></a>'
                }
                ],
                columnDefs: [
                { className: "text-center", "targets": [1,2] },
                { className: "text-right", "targets": [] },
                { "targets": [0], "width": "60%" },
                { "targets": [1], "width": "10%" },
                { "targets": [2], "width": "10%" },
                ],
                destroy: true,
                initComplete: function (settings, json) {
                    $('.dataTables_wrapper div.bottom div').addClass('col-md-6');
                    $('#tblPaymentTerm').fadeIn(100);
                    if (action == undefined) {
                        $('.excelExport').hide();
                        OnServerCallComplete();
                    }
                    if (action === 'Export') {
                        if (json.data.length > 0) {
                            if (json.data[0].TotalCount > 10000) {
                                MasterAlert("info", 'We are able to download maximum 10000 rows of data, There exist more than 10000 rows of data please filter and download')
                            }
                        }
                        $('.buttons-excel').trigger('click');
                        BindOrReloadPaymentTermTable();
                    }
                }
            });
        $('.buttons-excel').hide();
    }
    catch (e) {
        console.log(e.message);
    }
}

//function reset the list to initial
function ResetPaymentTermList() {
    BindOrReloadPaymentTermTable('Reset');
}

//function export data to excel
function ExportPaymentTermData() {
    $('.excelExport').show();
    OnServerCallBegin();
    BindOrReloadPaymentTermTable('Export');
}

function EditPaymentTermMaster(thisObj) {
    debugger;
    PaymentTermVM = _dataTables.PaymentTermList.row($(thisObj).parents('tr')).data();
    $("#divMasterBody").load("PaymentTerm/MasterPartial?masterCode=" + PaymentTermVM.Code, function (responseTxt, statusTxt, xhr) {
        if (statusTxt == "success") {
            $('#lblModelMasterContextLabel').text('Edit Payment Term Information')
            $('#divModelMasterPopUp').modal('show');
            $('#hdnMasterCall').val('MSTR');
        }
        else {
            console.log("Error: " + xhr.status + ": " + xhr.statusText);
        }
    });
}
function DeletePaymentTermMaster(thisObj) {
    debugger;
    PaymentTermVM = _dataTables.PaymentTermList.row($(thisObj).parents('tr')).data();
    notyConfirm('Are you sure to delete?', 'DeletePaymentTerm("' + PaymentTermVM.Code + '")');
}

function DeletePaymentTerm(code) {
    debugger;
    try {
        if (code) {
            var data = { "code": code };
            _jsonData = GetDataFromServer("PaymentTerm/DeletePaymentTerm/", data);
            if (_jsonData != '') {
                _jsonData = JSON.parse(_jsonData);
                _message = _jsonData.Message;
                _status = _jsonData.Status;
                _result = _jsonData.Record;
            }
            switch (_status) {
                case "OK":
                    notyAlert('success', _result.Message);
                    BindOrReloadPaymentTermTable('Reset');
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
        console.log(e.message);
    }
}