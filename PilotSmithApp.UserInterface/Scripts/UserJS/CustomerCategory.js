var _dataTables = {};
var _emptyGuid = "00000000-0000-0000-0000-000000000000";
var _jsonData = {};
var _message = "";
var _status = "";
var _result = "";

//---------------------------------------Docuement Ready--------------------------------------------------//

$(document).ready(function () {
    try {
        BindOrReloadCustomerCategoryTable('Init');
        $('#tblCustomerCategory tbody').on('dblclick', 'td', function () {
            EditCustomerCategoryMaster(this);
        });
    }
    catch (e) {
        console.log(e.message);
    }
});

//function bind the CustomerCategory list checking search and filter
function BindOrReloadCustomerCategoryTable(action) {
    try {
        debugger;
        //creating advancesearch object
        CustomerCategoryAdvanceSearchViewModel = new Object();
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
        CustomerCategoryAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
        CustomerCategoryAdvanceSearchViewModel.SearchTerm = $('#SearchTerm').val();
        //apply datatable plugin on CustomerCategory table
        _dataTables.CustomerCategoryList = $('#tblCustomerCategory').DataTable(
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

                    url: "CustomerCategory/GetAllCustomerCategory",
                    data: { "CustomerCategoryAdvanceSearchVM": CustomerCategoryAdvanceSearchViewModel },
                    type: 'POST'
                },
                pageLength: 10,
                columns: [
                { "data": "Name", "defaultContent": "<i>-</i>" },
                {
                    "data": null, "orderable": false, "defaultContent": '<a href="#" onclick="EditCustomerCategoryMaster(this)"<i class="fa fa-pencil-square-o" aria-hidden="true"></i></a>  <a href="#" onclick="DeleteCustomerCategoryMaster(this)"<i class="fa fa-trash-o" aria-hidden="true"></i></a>'
                }
                ],
                columnDefs: [{ "targets": [], "visible": false, "searchable": false },
                { className: "text-center", "targets": [ 1] },
                { className: "text-right", "targets": [] },
                { "targets": [0], "width": "60%" },
                { "targets": [1], "width": "40%" },
                ],
                destroy: true,
                initComplete: function (settings, json) {
                    $('.dataTables_wrapper div.bottom div').addClass('col-md-6');
                    $('#tblCustomerCategory').fadeIn(100);
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
                        BindOrReloadCustomerCategoryTable();
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
function ResetCustomerCategoryList() {
    BindOrReloadCustomerCategoryTable('Reset');
}

//function export data to excel
function ExportCustomerCategoryData() {
    $('.excelExport').show();
    OnServerCallBegin();
    BindOrReloadCustomerCategoryTable('Export');
}

function EditCustomerCategoryMaster(thisObj) {
    debugger;
    CustomerCategoryVM = _dataTables.CustomerCategoryList.row($(thisObj).parents('tr')).data();
    $("#divMasterBody").load("CustomerCategory/MasterPartial?masterCode=" + CustomerCategoryVM.Code, function (responseTxt, statusTxt, xhr) {
        if (statusTxt == "success") {
            $('#lblModelMasterContextLabel').text('Edit Customer Category Information')
            $('#divModelMasterPopUp').modal('show');
            $('#hdnMasterCall').val('MSTR');
        }
        else {
            console.log("Error: " + xhr.status + ": " + xhr.statusText);
        }
    });
}
function DeleteCustomerCategoryMaster(thisObj) {
    debugger;
    CustomerCategoryVM = _dataTables.CustomerCategoryList.row($(thisObj).parents('tr')).data();
    notyConfirm('Are you sure to delete?', 'DeleteCustomerCategory("' + CustomerCategoryVM.Code + '")');
}

function DeleteCustomerCategory(code) {
    debugger;
    try {
        if (code) {
            var data = { "code": code };
            _jsonData = GetDataFromServer("CustomerCategory/DeleteCustomerCategory/", data);
            if (_jsonData != '') {
                _jsonData = JSON.parse(_jsonData);
                _message = _jsonData.Message;
                _status = _jsonData.Status;
                _result = _jsonData.Record;
            }
            switch (_status) {
                case "OK":
                    notyAlert('success', _result.Message);
                    BindOrReloadCustomerCategoryTable('Reset');
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