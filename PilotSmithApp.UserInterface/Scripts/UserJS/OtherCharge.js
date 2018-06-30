var _dataTables = {};
var _emptyGuid = "00000000-0000-0000-0000-000000000000";
var _jsonData = {};
var _message = "";
var _status = "";
var _result = "";

//---------------------------------------Docuement Ready--------------------------------------------------//

$(document).ready(function () {
    try {
        BindOrReloadOtherChargeTable('Init');
        $('#tblOtherCharge tbody').on('dblclick', 'td', function () {
            EditOtherChargeMaster(this);
        });
    }
    catch (e) {
        console.log(e.message);
    }
});

//function bind the OtherCharge list checking search and filter
function BindOrReloadOtherChargeTable(action) {
    try {
        debugger;
        //creating advancesearch object
        OtherChargeAdvanceSearchViewModel = new Object();
        DataTablePagingViewModel = new Object();
        DataTablePagingViewModel.Length = 0;
        //switch case to check the operation
        switch (action) {
            case 'Reset':
                $('#SearchTerm').val('');
                break;
            case 'Init':
                break;
            case 'Search':
                if ($('#SearchTerm').val() == '') {
                    return true;
                }
                break;
            case 'Export':
                DataTablePagingViewModel.Length = -1;
                break;
            default:
                break;
        }
        OtherChargeAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
        OtherChargeAdvanceSearchViewModel.SearchTerm = $('#SearchTerm').val();
        //apply datatable plugin on OtherCharge table
        _dataTables.OtherChargeList = $('#tblOtherCharge').DataTable(
            {
                dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
                buttons: [{
                    extend: 'excel',
                    exportOptions:
                                 {
                                     columns: [0, 1]
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

                    url: "OtherCharge/GetAllOtherCharge",
                    data: { "OtherChargeAdvanceSearchVM": OtherChargeAdvanceSearchViewModel },
                    type: 'POST'
                },
                pageLength: 10,
                columns: [
                //{ "data": "Code", "defaultContent": "<i>-</i>" },
                { "data": "Description", "defaultContent": "<i>-</i>" },
                { "data": "SACCode", "defaultContent": "<i>-</i>" },
                {
                    "data": null, "orderable": false, "defaultContent": '<a href="#" onclick="EditOtherChargeMaster(this)"<i class="fa fa-pencil-square-o" aria-hidden="true"></i></a>  <a href="#" onclick="DeleteOtherChargeMaster(this)"<i class="fa fa-trash-o" aria-hidden="true"></i></a>'
                }
                ],
                columnDefs: [{ "targets": [], "visible": false, "searchable": false },
                { className: "text-center", "targets": [2] },
                { className: "text-right", "targets": [] },
                { "targets": [0], "width": "80%" },
                { "targets": [1], "width": "10%" },
                { "targets": [2], "width": "10%" },
                ],
                destroy: true,
                initComplete: function (settings, json) {
                    $('.dataTables_wrapper div.bottom div').addClass('col-md-6');
                    $('#tblOtherCharge').fadeIn(100);
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
                        BindOrReloadOtherChargeTable();
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
function ResetOtherChargeList() {
    BindOrReloadOtherChargeTable('Reset');
}

//function export data to excel
function ExportOtherChargeData() {
    $('.excelExport').show();
    OnServerCallBegin();
    BindOrReloadOtherChargeTable('Export');
}

function EditOtherChargeMaster(thisObj) {
    debugger;
    OtherChargeVM = _dataTables.OtherChargeList.row($(thisObj).parents('tr')).data();
    $("#divMasterBody").load("OtherCharge/MasterPartial?masterCode=" + OtherChargeVM.Code, function () {
        $('#lblModelMasterContextLabel').text('Edit Other Charge Information')
        $('#divModelMasterPopUp').modal('show');
        $('#hdnMasterCall').val('MSTR');
    });
}
function DeleteOtherChargeMaster(thisObj) {
    debugger;
    OtherChargeVM = _dataTables.OtherChargeList.row($(thisObj).parents('tr')).data();
    notyConfirm('Are you sure to delete?', 'DeleteOtherCharge("' + OtherChargeVM.Code + '")');
}

function DeleteOtherCharge(code) {
    debugger;
    try {
        if (code) {
            var data = { "code": code };
            _jsonData = GetDataFromServer("OtherCharge/DeleteOtherCharge/", data);
            if (_jsonData != '') {
                _jsonData = JSON.parse(_jsonData);
                _message = _jsonData.Message;
                _status = _jsonData.Status;
                _result = _jsonData.Record;
            }
            switch (_status) {
                case "OK":
                    notyAlert('success', _result.Message);
                    BindOrReloadOtherChargeTable('Reset');
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