var _dataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
var _jsonData = {};
var _message = "";
var _status = "";
var _result = "";
$(document).ready(function () {
    try {

        //$("#StateCode").select2({});

        BindOrReloadDistrictTable('Init');
        $('#tblDistrict tbody').on('dblclick', 'td', function () {
            EditDistrictMaster(this);
        });
    }
    catch (e) {
        console.log(e.message);
    }
});

function BindOrReloadDistrictTable(action) {
    try {
        debugger;
        //creating advancesearch object
        DistrictAdvanceSearchViewModel = new Object();
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
        DistrictAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
        DistrictAdvanceSearchViewModel.SearchTerm = $('#SearchTerm').val();
        //apply datatable plugin on bank table
        _dataTables.DistrictList = $('#tblDistrict').DataTable(
            {
                dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
                buttons: [{
                    extend: 'excel',
                    exportOptions:
                                 {
                                     columns: [ 1, 2, 3]
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

                    url: "District/GetAllDistrict",
                    data: { "DistrictAdvanceSearchVM": DistrictAdvanceSearchViewModel },
                    type: 'POST'
                },
                pageLength: 10,
                columns: [
                { "data": "Description", "defaultContent": "<i>-</i>" },
                { "data": "Country.Description", "defaultContent": "<i>-</i>" },
                {"data" :"State.Description","defaultContent":"<i>-</i>"},
                { "data": "PSASysCommon.CreatedDateString", "defaultContent": "<i>-</i>" },
                {
                    "data": null, "orderable": false, "defaultContent": '<a href="#" onclick="EditDistrictMaster(this)"<i class="fa fa-pencil-square-o" aria-hidden="true"></i></a>  <a href="#" onclick="DeleteDistrictMaster(this)"<i class="fa fa-trash-o" aria-hidden="true"></i></a>'
                }
                ],
                columnDefs: [
                { className: "text-center", "targets": [3,4] },
                { "targets": [0], "width": "30%" },
                {"targets":  [1],"width":"20%"},
                { "targets": [2], "width": "20%" },
                { "targets": [3], "width": "20%" },
                { "targets": [4], "width": "10%" }
                ],
                destroy: true,
                initComplete: function (settings, json) {
                    $('.dataTables_wrapper div.bottom div').addClass('col-md-6');
                    $('#tblDistrict').fadeIn(100);
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
                        BindOrReloadDistrictTable();
                    }
                }
            });
        $('.buttons-excel').hide();
    }
    catch (e) {
        console.log(e.message);
    }
}

function ResetDistrictList() {
    BindOrReloadDistrictTable('Reset');
}

function ExportDistrictData() {
    $('.excelExport').show();
    OnServerCallBegin();
    BindOrReloadDistrictTable('Export');
}

function EditDistrictMaster(thisObj) {
    debugger;
    _parentFormID = "FormDistrict";
    DistrictVM = _dataTables.DistrictList.row($(thisObj).parents('tr')).data();
    $("#divMasterBody2").load("District/MasterPartial?masterCode=" + DistrictVM.Code, function (responseTxt, statusTxt, xhr) {
        if (statusTxt == "success") {
            $('#lblModelMasterContextLabel2').text('Edit District Information')
            $('#divModelMasterPopUp2').modal('show');
            $('#hdnMasterCall2').val('MSTR');
            $('#CountryCode').val(DistrictVM.CountryCode).trigger('change');
            $('#StateCode').val(DistrictVM.StateCode).trigger('change');
        }
        else {
            console.log("Error: " + xhr.status + ": " + xhr.statusText);
        }
    });
}
function DeleteDistrictMaster(thisObj) {
    debugger;
    DistrictVM = _dataTables.DistrictList.row($(thisObj).parents('tr')).data();
    notyConfirm('Are you sure to delete?', 'DeleteDistrict("' + DistrictVM.Code + '")');
}

function DeleteDistrict(code) {
    debugger;
    try {
        if (code) {
            var data = { "code": code };         
            _jsonData = GetDataFromServer("District/DeleteDistrict/", data);
            if (_jsonData != '') {
                _jsonData = JSON.parse(_jsonData);
                _message = _jsonData.Message;
                _status = _jsonData.Status;
                _result = _jsonData.Record;
            }
            switch (_status) {
                case "OK":
                    notyAlert('success', _result.Message);
                    BindOrReloadDistrictTable('Reset');
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