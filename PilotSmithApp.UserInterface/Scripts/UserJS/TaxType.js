var _dataTables = {};
var _emptyGuid = "00000000-0000-0000-0000-000000000000";
var _jsonData = {};
var _message = "";
var _status = "";
var _result = "";

//---------------------------------------Docuement Ready--------------------------------------------------//

$(document).ready(function () {
    try {
        BindOrReloadTaxTypeTable('Init');
        $('#tblTaxType tbody').on('dblclick', 'td', function () {
            EditTaxTypeMaster(this);
        });
    }
    catch (e) {
        console.log(e.message);
    }
});

//function bind the TaxType list checking search and filter
function BindOrReloadTaxTypeTable(action) {
    try {
        debugger;
        //creating advancesearch object
        TaxTypeAdvanceSearchViewModel = new Object();
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
        TaxTypeAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
        TaxTypeAdvanceSearchViewModel.SearchTerm = $('#SearchTerm').val();
        //apply datatable plugin on TaxType table
        _dataTables.TaxTypeList = $('#tblTaxType').DataTable(
            {
                dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
                buttons: [{
                    extend: 'excel',
                    exportOptions:
                                 {
                                     columns: [0, 1, 2,3]
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

                    url: "TaxType/GetAllTaxType",
                    data: { "TaxTypeAdvanceSearchVM": TaxTypeAdvanceSearchViewModel },
                    type: 'POST'
                },
                pageLength: 10,
                columns: [
                //{ "data": "Code", "defaultContent": "<i>-</i>" },
                { "data": "Description", "defaultContent": "<i>-</i>" },
                { "data": "CGSTPercentage", "defaultContent": "<i>-</i>" },
                { "data": "SGSTPercentage", "defaultContent": "<i>-</i>" },
                { "data": "IGSTPercentage", "defaultContent": "<i>-</i>" },
                {
                    "data": null, "orderable": false, "defaultContent": '<a href="#" onclick="EditTaxTypeMaster(this)"<i class="fa fa-pencil-square-o" aria-hidden="true"></i></a>  <a href="#" onclick="DeleteTaxTypeMaster(this)"<i class="fa fa-trash-o" aria-hidden="true"></i></a>'
                }
                ],
                columnDefs: [{ "targets": [], "visible": false, "searchable": false },
                { className: "text-center", "targets": [1,2, 3,4] },
                { className: "text-right", "targets": [] },
                { "targets": [0], "width": "30%" },
                { "targets": [1], "width": "20%" },
                { "targets": [2], "width": "20%" },
                { "targets": [3], "width": "20%" },
                { "targets": [4], "width": "10%" },
                //{ "targets": [5], "width": "10%" },
                ],
                destroy: true,
                initComplete: function (settings, json) {
                    $('.dataTables_wrapper div.bottom div').addClass('col-md-6');
                    $('#tblTaxType').fadeIn(100);
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
                        BindOrReloadTaxTypeTable();
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
function ResetTaxTypeList() {
    BindOrReloadTaxTypeTable('Reset');
}

//function export data to excel
function ExportTaxTypeData() {
    $('.excelExport').show();
    OnServerCallBegin();
    BindOrReloadTaxTypeTable('Export');
}

function EditTaxTypeMaster(thisObj) {
    debugger;
    TaxTypeVM = _dataTables.TaxTypeList.row($(thisObj).parents('tr')).data();
    $("#divMasterBody").load("TaxType/MasterPartial?masterCode=0" + TaxTypeVM.Code, function () {
        $('#lblModelMasterContextLabel').text('Edit Tax Type Information')
        $('#divModelMasterPopUp').modal('show');
        $('#hdnMasterCall').val('MSTR');
    });
}
function DeleteTaxTypeMaster(thisObj) {
    debugger;
    TaxTypeVM = _dataTables.TaxTypeList.row($(thisObj).parents('tr')).data();
    notyConfirm('Are you sure to delete?', 'DeleteTaxType("' + TaxTypeVM.Code + '")');
}

function DeleteTaxType(code) {
    debugger;
    try {
        if (code) {
            var data = { "code": code };
            _jsonData = GetDataFromServer("TaxType/DeleteTaxType/", data);
            if (_jsonData != '') {
                _jsonData = JSON.parse(_jsonData);
                _message = _jsonData.Message;
                _status = _jsonData.Status;
                _result = _jsonData.Record;
            }
            switch (_status) {
                case "OK":
                    notyAlert('success', _result.Message);
                    BindOrReloadTaxTypeTable('Reset');
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