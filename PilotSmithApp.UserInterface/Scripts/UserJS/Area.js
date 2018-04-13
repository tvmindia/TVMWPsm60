﻿var _dataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
var _jsonData = {};
var _message = "";
var _status = "";
var _result = "";
$(document).ready(function () {
    try {      
        BindOrReloadAreaTable('Init');
        $('#tblArea tbody').on('dblclick', 'td', function () {
            EditAreaMaster(this);
        });
    }
    catch (e) {
        console.log(e.message);
    }
});

function BindOrReloadAreaTable(action) {
    try {
        debugger;
        //creating advancesearch object
        AreaAdvanceSearchViewModel = new Object();
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
        AreaAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
        AreaAdvanceSearchViewModel.SearchTerm = $('#SearchTerm').val();
        //apply datatable plugin on Area table
        _dataTables.AreaList = $('#tblArea').DataTable(
            {
                dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
                buttons: [{
                    extend: 'excel',
                    exportOptions:
                                 {
                                     columns: [0, 1, 2, 3, 4]
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

                    url: "Area/GetAllArea",
                    data: { "AreaAdvanceSearchVM": AreaAdvanceSearchViewModel },
                    type: 'POST'
                },
                pageLength: 10,
                columns: [
                { "data": "Code", "defaultContent": "<i>-</i>" },
                { "data": "State.Description", "defaultContent": "<i>-</i>" },
                {"data" :"District.Description","defaultContent":"<i>-</i>"},
                { "data": "Description", "defaultContent": "<i>-</i>" },
                { "data": "PSASysCommon.CreatedDateString", "defaultContent": "<i>-</i>" },
                {
                    "data": null, "orderable": false, "defaultContent": '<a href="#" onclick="EditAreaMaster(this)"<i class="fa fa-pencil-square-o" aria-hidden="true"></i></a>  <a href="#" onclick="DeleteAreaMaster(this)"<i class="fa fa-trash-o" aria-hidden="true"></i></a>'
                }
                ],
                columnDefs: [{ "targets": [], "visible": false, "searchable": false },
                { className: "text-center", "targets": [4] },
                { "targets": [0], "width": "10%" },
                { "targets": [1], "width": "10%" },
                { "targets": [2], "width": "10%" },
                { "targets": [3], "width": "40%" },
                { "targets": [4], "width": "20%" },
                { "targets": [5], "width": "10%" }
                ],
                destroy: true,
                initComplete: function (settings, json) {
                    $('.dataTables_wrapper div.bottom div').addClass('col-md-6');
                    $('#tblArea').fadeIn('slow');
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
                        BindOrReloadAreaTable();
                    }
                }
            });
        $('.buttons-excel').hide();
    }
    catch (e) {
        console.log(e.message);
    }
}
function ResetAreaList() {
    BindOrReloadAreaTable('Reset');
}

function ExportAreaData() {
    $('.excelExport').show();
    OnServerCallBegin();
    BindOrReloadAreaTable('Export');
}

function EditAreaMaster(thisObj) {
    debugger;
    AreaVM = _dataTables.AreaList.row($(thisObj).parents('tr')).data();
    $("#divMasterBody").load("Area/MasterPartial?masterCode=" + AreaVM.Code, function () {
        $('#lblModelMasterContextLabel').text('Edit Area Information')
        $('#divModelMasterPopUp').modal('show');
        $('#hdnMasterCall').val('MSTR');
    });
}
function DeleteAreaMaster(thisObj) {
    debugger;
    AreaVM = _dataTables.AreaList.row($(thisObj).parents('tr')).data();
    notyConfirm('Are you sure to delete?', 'DeleteArea("' + AreaVM.Code + '")');
}

function DeleteArea(code) {
    debugger;
    try {
        if (code) {
            var data = { "code": code };           
            _jsonData = GetDataFromServer("Area/DeleteArea/", data);
            if (_jsonData != '') {
                _jsonData = JSON.parse(_jsonData);
                _message = _jsonData.Message;
                _status = _jsonData.Status;
                _result = _jsonData.Record;
            }
            switch (_status) {
                case "OK":
                    notyAlert('success', _result.Message);
                    BindOrReloadAreaTable('Reset');
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