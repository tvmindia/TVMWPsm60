var _dataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
var _jsonData = {};
var _message = "";
var _status = "";
var _result = "";

$(document).ready(function () {
    try {
        BindOrReloadSpareTable('Init');
        $('#tblSpare tbody').on('dblclick', 'td', function () {
            EditSpareMaster(this);
        });
    }
    catch (e) {
        console.log(e.message);
    }
});

function BindOrReloadSpareTable(action) {
    try {
        debugger;
        //creating advancesearch object
        SpareAdvanceSearchViewModel = new Object();
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
        SpareAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
        SpareAdvanceSearchViewModel.SearchTerm = $('#SearchTerm').val();
        //apply datatable plugin on Spare table
        _dataTables.SpareList = $('#tblSpare').DataTable(
            {
                dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
                buttons: [{
                    extend: 'excel',
                    exportOptions:
                                 {
                                     columns: [0, 1, 2]
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

                    url: "Spare/GetAllSpare",
                    data: { "SpareAdvanceSearchVM": SpareAdvanceSearchViewModel },
                    type: 'POST'
                },
                pageLength: 10,
                columns: [
                { "data": "Code", "defaultContent": "<i>-</i>" },
                { "data": "Name", "defaultContent": "<i>-</i>" },
                { "data": "HSNCode", "defaultContent": "<i>-</i>" },
                {
                    "data": null, "orderable": false, "defaultContent": '<a href="#" onclick="EditSpareMaster(this)"<i class="fa fa-pencil-square-o" aria-hidden="true"></i></a>  <a href="#" onclick="DeleteSpareMaster(this)"<i class="fa fa-trash-o" aria-hidden="true"></i></a>'
                }
                ],
                columnDefs: [
                ],
                destroy: true,
                initComplete: function (settings, json) {
                    $('.dataTables_wrapper div.bottom div').addClass('col-md-6');
                    $('#tblSpare').fadeIn(100);
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
                        BindOrReloadSpareTable();
                    }
                }
            });
        $('.buttons-excel').hide();
    }
    catch (e) {
        console.log(e.message);
    }
}

function ResetSpareList() {
    try {
        BindOrReloadSpareTable('Reset');
    }
    catch (e) {
        console.log(e.message);
    }
}

function ExportSpareData() {
    try {
        $('.excelExport').show();
        OnServerCallBegin();
        BindOrReloadSpareTable('Export');
    }
    catch (e) {
        console.log(e.message);
    }
}

function DeleteSpareMaster(thisObj) {
    try {
        debugger;
        SpareVM = _dataTables.SpareList.row($(thisObj).parents('tr')).data();
        notyConfirm('Are you sure to delete?', 'DeleteSpare("' + SpareVM.ID + '")');
    }
    catch (e) {
        console.log(e.message);
    }
}

function DeleteSpare(id) {
    debugger;
    try {
        if (id) {
            var data = { "ID": id };
            _jsonData = GetDataFromServer("Spare/DeleteSpare/", data);
            if (_jsonData != '') {
                _jsonData = JSON.parse(_jsonData);
                _message = _jsonData.Message;
                _status = _jsonData.Status;
                _result = _jsonData.Record;
            }
            switch (_status) {
                case "OK":
                    notyAlert('success', _result.Message);
                    BindOrReloadSpareTable('Reset');
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

function EditSpareMaster(thisObj) {
    try {
        debugger;
        SpareVM = _dataTables.SpareList.row($(thisObj).parents('tr')).data();

        $("#divMasterBody").load("Spare/MasterPartial?masterCode=" + SpareVM.ID, function (responseTxt, statusTxt, xhr) {
            if (statusTxt == "success") {
                $('#hdnMasterCall').val('MSTR');
                $('#lblModelMasterContextLabel').text('Edit Spare Information')
                clearPopupUploadControl();
                PaintImages(SpareVM.ID);
                $('#divModelMasterPopUp').modal('show');
            }
            else {
                console.log("Error: " + xhr.status + ": " + xhr.statusText);
            }
        });
    }
    catch (e) {
        console.log(e.message);
    }
}
