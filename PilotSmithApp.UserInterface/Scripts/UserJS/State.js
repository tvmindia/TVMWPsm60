var _dataTables = {};
var _emptyGuid = "00000000-0000-0000-0000-000000000000";
var _jsonData = {};
var _message = "";
var _status = "";
var _result = "";
$(document).ready(function () {
    try {
        BindOrReloadStateTable('Init');
        $('#tblState tbody').on('dblclick', 'td', function () {
            EditStateMaster(this);
        });
    }
    catch (e) {
        console.log(e.message);
    }
});
//function bind the Product Specification list and cheking and filter
function BindOrReloadStateTable(action) {
    try {
        //creating advancesearch object
        StateAdvanceSearchViewModel = new Object();
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
        StateAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
        StateAdvanceSearchViewModel.SearchTerm = $('#SearchTerm').val();
        //apply datatable plugin on bank table
        _dataTables.StateList = $('#tblState').DataTable(
            {
                dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
                buttons: [{
                    extend: 'excel',
                    exportOptions:
                                 {
                                     columns: [ 1, 2,3]
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

                    url: "State/GetAllState",
                    data: { "StateAdvanceSearchVM": StateAdvanceSearchViewModel },
                    type: 'POST'
                },
                pageLength: 10,
                columns: [
                { "data": "Description", "defaultContent": "<i>-</i>" },
                { "data": "Country.Description", "defaultContent": "<i>-</i>" },
                { "data": "PSASysCommon.CreatedDateString", "defaultContent": "<i>-</i>" },
                {
                    "data": null, "orderable": false, "defaultContent": '<a href="#" onclick="EditStateMaster(this)"<i class="fa fa-pencil-square-o" aria-hidden="true"></i></a>  <a href="#" onclick="DeleteStateMaster(this)"<i class="fa fa-trash-o" aria-hidden="true"></i></a>'
                }
                ],
                columnDefs: [
                { className: "text-center", "targets": [ 2,3] },                
                { "targets": [0], "width": "60%" },
                { "targets": [1], "width": "20%" },
                { "targets": [2], "width": "10%" },
                { "targets": [3], "width": "10%" },
                ],
                destroy: true,
                initComplete: function (settings, json) {
                    $('.dataTables_wrapper div.bottom div').addClass('col-md-6');
                    $('#tblState').fadeIn(100);
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
                        BindOrReloadStateTable();
                    }
                }
            });
        $('.buttons-excel').hide();
    }
    catch (e) {
        console.log(e.message);
    }
}

function ResetStateList() {
    BindOrReloadStateTable('Reset');
}

function ExportStateData() {
    $('.excelExport').show();
    OnServerCallBegin();
    BindOrReloadStateTable('Export');
}

function EditStateMaster(thisObj) {
    debugger;
    StateVM = _dataTables.StateList.row($(thisObj).parents('tr')).data();
    $("#divMasterBody").load("State/MasterPartial?masterCode=" + StateVM.Code, function (responseTxt, statusTxt, xhr) {
        if (statusTxt == "success") {
            $('#lblModelMasterContextLabel').text('Edit State Information')
            $('#divModelMasterPopUp').modal('show');
            $('#hdnMasterCall').val('MSTR');
        }
        else {
            console.log("Error: " + xhr.status + ": " + xhr.statusText);
        }
    });
}
function DeleteStateMaster(thisObj) {
    debugger;
    StateVM = _dataTables.StateList.row($(thisObj).parents('tr')).data();
    notyConfirm('Are you sure to delete?', 'DeleteState("' + StateVM.Code + '")');
}

function DeleteState(code) {
    debugger;
    try {
        if (code) {
            var data = { "code": code };        
            _jsonData = GetDataFromServer("State/DeleteState/", data);
            if (_jsonData != '') {
                _jsonData = JSON.parse(_jsonData);
                _message = _jsonData.Message;
                _status = _jsonData.Status;
                _result = _jsonData.Record;
            }
            switch (_status) {
                case "OK":
                    notyAlert('success', _result.Message);
                    BindOrReloadStateTable('Reset');
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