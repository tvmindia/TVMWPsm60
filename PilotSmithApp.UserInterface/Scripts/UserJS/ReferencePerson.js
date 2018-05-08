var _dataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
var _jsonData = {};
var _message = "";
var _status = "";
var _result = "";

//---------------------------------------Docuement Ready--------------------------------------------------//
$(document).ready(function () {
    try {
        debugger;
        BindOrReloadReferencePersonTable('Init');
        $('#tblReferencePerson tbody').on('dblclick', 'td', function () {
            EditReferencePersonMaster(this);
        });
    }
    catch (e) {
        console.log(e.message);
    }
});

//function bind the ReferencePerson list checking search and filter
function BindOrReloadReferencePersonTable(action) {
    try {
        debugger;
        //creating advancesearch object
        ReferencePersonAdvanceSearchViewModel = new Object();
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
        ReferencePersonAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
        ReferencePersonAdvanceSearchViewModel.SearchTerm = $('#SearchTerm').val();
        //apply datatable plugin on ReferencePerson table
        _dataTables.ReferencePersonList = $('#tblReferencePerson').DataTable(
            {
               
                dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
                buttons: [{
                    extend: 'excel',
                    exportOptions:
                                 {
                                     columns: [ 1, 2, 3, 4,5,6,7,8,9]
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

                    url: "ReferencePerson/GetAllReferencePerson",
                    data: { "ReferencePersonAdvanceSearchVM": ReferencePersonAdvanceSearchViewModel },
                    type: 'POST'
                },
                pageLength: 10,
                columns: [
                { "data": "Code", "defaultContent": "<i>-</i>" },
                { "data": "Name", "defaultContent": "<i>-</i>" },
                { "data": "ReferenceType.Description", "defaultContent": "<i>-</i>" },
                { "data": "Area.Description", "defaultContent": "<i>-</i>" },
                { "data": "Organization", "defaultContent": "<i>-</i>" },
                { "data": "Address", "defaultContent": "<i>-</i>" },
                { "data": "Email", "defaultContent": "<i>-</i>" },
                { "data": "PhoneNos", "defaultContent": "<i>-</i>" },
                { "data": "FaxNos", "defaultContent": "<i>-</i>" },
                { "data": "GeneralNotes", "defaultContent": "<i>-</i>" },
                {
                    "data": null, "orderable": false, "defaultContent": '<a href="#" onclick="EditReferencePersonMaster(this)"<i class="fa fa-pencil-square-o" aria-hidden="true"></i></a>  <a href="#" onclick="DeleteReferencePersonMaster(this)"<i class="fa fa-trash-o" aria-hidden="true"></i></a>'
                }
                ],
                columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
                { className: "text-center", "targets": [4] },
                { "targets": [0], "width": "10%" },
                { "targets": [1], "width": "10%" },
                { "targets": [2], "width": "10%" },
                { "targets": [3], "width": "10%" },
                { "targets": [4], "width": "10%" },
                { "targets": [5], "width": "10%" },
                { "targets": [6], "width": "10%" },
                { "targets": [7], "width": "10%" },
                { "targets": [8], "width": "10%" },
                { "targets": [9], "width": "10%" },
                { "targets": [10], "width": "10%" },
                ],
                destroy: true,
                //for performing the import operation after the data loaded
                initComplete: function (settings, json) {
                    $('.dataTables_wrapper div.bottom div').addClass('col-md-6');
                    $('#tblReferencePerson').fadeIn(100);
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
                        BindOrReloadReferencePersonTable();
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
function ResetReferencePersonList() {
    BindOrReloadReferencePersonTable('Reset');
}

//function export data to excel
function ExportReferencePersonData() {
    $('.excelExport').show();
    OnServerCallBegin();
    BindOrReloadReferencePersonTable('Export');
}

function EditReferencePersonMaster(thisObj) {
    debugger;
    ReferencePersonVM = _dataTables.ReferencePersonList.row($(thisObj).parents('tr')).data();
    //this will return form body(html)
    $("#divMasterBody").load("ReferencePerson/MasterPartial?masterCode=" + ReferencePersonVM.Code, function () {
        $('#lblModelMasterContextLabel').text('Edit ReferencePerson Information')
        $('#divModelMasterPopUp').modal('show');
        $('#hdnMasterCall').val('MSTR');
    });
}
function DeleteReferencePersonMaster(thisObj) {
    debugger;
    ReferencePersonVM = _dataTables.ReferencePersonList.row($(thisObj).parents('tr')).data();
    notyConfirm('Are you sure to delete?', 'DeleteReferencePerson("' + ReferencePersonVM.Code + '")');
}

function DeleteReferencePerson(code) {
    debugger;
    try {
        if (code) {
            var data = { "code": code };           
            _jsonData = GetDataFromServer("ReferencePerson/DeleteReferencePerson/", data);
            if (_jsonData != '') {
                _jsonData = JSON.parse(_jsonData);
                _message = _jsonData.Message;
                _status = _jsonData.Status;
                _result = _jsonData.Record;
            }
            switch (_status) {
                case "OK":
                    notyAlert('success', _result.Message);
                    BindOrReloadReferencePersonTable('Reset');
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