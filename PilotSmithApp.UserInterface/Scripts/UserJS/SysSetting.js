var _dataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
var _jsonData = {};
var _message = "";
var _status = "";
var _result = "";

$(document).ready(function () {
    try {
        BindOrReloadSysSettingTable('Init');
        $('#tblSysSetting tbody').on('dblclick', 'td', function () {
            EditSysSettingMaster(this);
        });
    }
    catch (e) {
        console.log(e.message);
    }
});

function BindOrReloadSysSettingTable(action) {
    try {
        debugger;
        //creating advancesearch object
        SysSettingAdvanceSearchViewModel = new Object();
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
        SysSettingAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
        SysSettingAdvanceSearchViewModel.SearchTerm = $('#SearchTerm').val();
        //apply datatable plugin on SysSetting table
        _dataTables.SysSettingList = $('#tblSysSetting').DataTable(
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

                    url: "SysSetting/GetAllSysSetting",
                    data: { "sysSettingAdvanceSearchVM": SysSettingAdvanceSearchViewModel },
                    type: 'POST'
                },
                pageLength: 10,
                columns: [
                //{ "data": "Slno", "defaultContent": "<i>-</i>" },
                { "data": "Name", "defaultContent": "<i>-</i>" },
                { "data": "Value", "defaultContent": "<i>-</i>" },
                {
                    "data": null, "orderable": false, "defaultContent": '<a href="#" onclick="EditSysSettingMaster(this)"<i class="fa fa-pencil-square-o" aria-hidden="true"></i></a>  <a href="#" onclick="DeleteSysSettingMaster(this)"<i class="fa fa-trash-o" aria-hidden="true"></i></a>'
                }
                ],
                columnDefs: [
                ],
                destroy: true,
                initComplete: function (settings, json) {
                    $('.dataTables_wrapper div.bottom div').addClass('col-md-6');
                    $('#tblSysSetting').fadeIn(100);
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
                        BindOrReloadSysSettingTable();
                    }
                }
            });
        $('.buttons-excel').hide();
    }
    catch (e) {
        console.log(e.message);
    }
}

function ResetSysSettingList() {
    try {
        BindOrReloadSysSettingTable('Reset');
    }
    catch (e) {
        console.log(e.message);
    }
}

function ExportSysSettingData() {
    try {
        $('.excelExport').show();
        OnServerCallBegin();
        BindOrReloadSysSettingTable('Export');
    }
    catch (e) {
        console.log(e.message);
    }
}

function DeleteSysSettingMaster(thisObj) {
    try {
        debugger;
        sysSettingVM = _dataTables.SysSettingList.row($(thisObj).parents('tr')).data();
        notyConfirm('Are you sure to delete?', 'DeleteSysSetting("' + sysSettingVM.ID + '")');
    }
    catch (e) {
        console.log(e.message);
    }
}

function DeleteSysSetting(id) {
    debugger;
    try {
        if (id) {
            var data = { "ID": id };
            _jsonData = GetDataFromServer("SysSetting/DeleteSysSetting/", data);
            if (_jsonData != '') {
                _jsonData = JSON.parse(_jsonData);
                _message = _jsonData.Message;
                _status = _jsonData.Status;
                _result = _jsonData.Record;
            }
            switch (_status) {
                case "OK":
                    notyAlert('success', _result.Message);
                    BindOrReloadSysSettingTable('Reset');
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

function EditSysSettingMaster(thisObj) {
    try {
        debugger;
        sysSettingVM = _dataTables.SysSettingList.row($(thisObj).parents('tr')).data();

        $("#divMasterBody").load("SysSetting/MasterPartial?masterCode=" + sysSettingVM.ID, function (responseTxt, statusTxt, xhr) {
            if (statusTxt == "success") {
                $('#hdnMasterCall').val('MSTR');
                $('#lblModelMasterContextLabel').text('Edit SysSetting Information')
                //clearPopupUploadControl();
                //PaintImages(sysSettingVM.ID);
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
