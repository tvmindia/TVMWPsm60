var _dataTables = {};
var _emptyGuid = "00000000-0000-0000-0000-000000000000";
var _jsonData = {};
var _message = "";
var _status = "";
var _result = "";

//---------------------------------------Docuement Ready--------------------------------------------------//

$(document).ready(function () {
    try {
        BindOrReloadPlantTable('Init');
        $('#tblPlant tbody').on('dblclick', 'td', function () {
            EditPlantMaster(this);
        });
    }
    catch (e) {
        console.log(e.message);
    }
});

//function bind the Plant list checking search and filter
function BindOrReloadPlantTable(action) {
    try {
        debugger;
        //creating advancesearch object
        PlantAdvanceSearchViewModel = new Object();
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
        PlantAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
        PlantAdvanceSearchViewModel.SearchTerm = $('#SearchTerm').val();
        //apply datatable plugin on Plant table
        _dataTables.PlantList = $('#tblPlant').DataTable(
            {
                dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
                buttons: [{
                    extend: 'excel',
                    exportOptions:
                                 {
                                     columns: [0]
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

                    url: "Plant/GetAllPlant",
                    data: { "PlantAdvanceSearchVM": PlantAdvanceSearchViewModel },
                    type: 'POST'
                },
                pageLength: 10,
                columns: [
                { "data": "Description", "defaultContent": "<i>-</i>" },
                {
                    "data": null, "orderable": false, "defaultContent": '<a href="#" onclick="EditPlantMaster(this)"<i class="fa fa-pencil-square-o" aria-hidden="true"></i></a>  <a href="#" onclick="DeletePlantMaster(this)"<i class="fa fa-trash-o" aria-hidden="true"></i></a>'
                }
                ],
                columnDefs: [{ "targets": [], "visible": false, "searchable": false },
                { className: "text-center", "targets": [1] },
                { className: "text-right", "targets": [] },
                { "targets": [0], "width": "70%" },
                { "targets": [1], "width": "30%" },
                ],
                destroy: true,
                initComplete: function (settings, json) {
                    $('.dataTables_wrapper div.bottom div').addClass('col-md-6');
                    $('#tblPlant').fadeIn(100);
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
                        BindOrReloadPlantTable();
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
function ResetPlantList() {
    BindOrReloadPlantTable('Reset');
}

//function export data to excel
function ExportPlantData() {
    $('.excelExport').show();
    OnServerCallBegin();
    BindOrReloadPlantTable('Export');
}

function EditPlantMaster(thisObj) {
    debugger;
    PlantVM = _dataTables.PlantList.row($(thisObj).parents('tr')).data();
    $("#divMasterBody").load("Plant/MasterPartial?masterCode=" + PlantVM.Code, function (responseTxt, statusTxt, xhr) {
        if (statusTxt == "success") {
            $('#lblModelMasterContextLabel').text('Edit Plant Information')
            $('#divModelMasterPopUp').modal('show');
            $('#hdnMasterCall').val('MSTR');
        }
        else {
            console.log("Error: " + xhr.status + ": " + xhr.statusText);
        }
    });
}
function DeletePlantMaster(thisObj) {
    debugger;
    PlantVM = _dataTables.PlantList.row($(thisObj).parents('tr')).data();
    notyConfirm('Are you sure to delete?', 'DeletePlant("' + PlantVM.Code + '")');
}

function DeletePlant(code) {
    debugger;
    try {
        if (code) {
            var data = { "code": code };
            _jsonData = GetDataFromServer("Plant/DeletePlant/", data);
            if (_jsonData != '') {
                _jsonData = JSON.parse(_jsonData);
                _message = _jsonData.Message;
                _status = _jsonData.Status;
                _result = _jsonData.Record;
            }
            switch (_status) {
                case "OK":
                    notyAlert('success', _result.Message);
                    BindOrReloadPlantTable('Reset');
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