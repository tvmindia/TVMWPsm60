var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
var _jsonData = {};
var _message = "";
var _status = "";
var _result = "";
$(document).ready(function () {
    try {
        BindOrReloadCompanyTable('Init');
    }
    catch (e) {
        console.log(e.message);
    }
});
//function bind the Product Specification list and cheking and filter
function BindOrReloadCompanyTable(action) {
    try {
        debugger;
        //creating advancesearch object
        CompanyAdvanceSearchViewModel = new Object();
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
        CompanyAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
        CompanyAdvanceSearchViewModel.SearchTerm = $('#SearchTerm').val();
        //apply datatable plugin on Company table
        DataTables.CompanyList = $('#tblCompany').DataTable(
            {
                dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
                buttons: [{
                    extend: 'excel',
                    exportOptions:
                                 {
                                     columns: [1]
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

                    url: "Company/GetAllCompany",
                    data: { "CompanyAdvanceSearchVM": CompanyAdvanceSearchViewModel },
                    type: 'POST'
                },
                pageLength: 10,
                columns: [
                { "data": "ID", "defaultContent": "<i>-</i>" },
                { "data": "Name", "defaultContent": "<i>-</i>"},
                {
                    "data": null, "orderable": false, "defaultContent": '<a href="#" onclick="EditCompanyMaster(this)"<i class="glyphicon glyphicon-edit" aria-hidden="true"></i></a>  <a href="#" onclick="DeleteCompanyMaster(this)"<i class="glyphicon glyphicon-trash" aria-hidden="true"></i></a>', "width": "10%"
                }
                ],
                columnDefs: [{ "targets": [0], "visible": false, "searchable": false },],
                destroy: true,
                initComplete: function (settings, json) {
                    $('.dataTables_wrapper div.bottom div').addClass('col-md-6');
                    $('#tblCompany').fadeIn('slow');
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
                        BindOrReloadCompanyTable();
                    }
                }
            });
        $('.buttons-excel').hide();
    }
    catch(e)
    {
        console.log(e.message);
    }
}

function ResetCompanyList() {
    BindOrReloadCompanyTable('Reset');
}

function ExportCompanyData() {
    $('.excelExport').show();
    OnServerCallBegin();
    BindOrReloadCompanyTable('Export');
}

function EditCompanyMaster(thisObj) {
    debugger;
    CompanyVM = DataTables.CompanyList.row($(thisObj).parents('tr')).data();
    GetMasterPartial('Company', CompanyVM.ID);
    $('#h3ModelMasterContextLabel').text('Edit Company')
    $('#divModelMasterPopUp').modal('show');
    $('#hdnMasterCall').val('MSTR');
}
function DeleteCompanyMaster(thisObj) {
    debugger;
    CompanyVM = DataTables.CompanyList.row($(thisObj).parents('tr')).data();
    notyConfirm('Are you sure to delete?', 'DeleteCompany("' + CompanyVM.ID + '")');
}

function DeleteCompany(id) {
    debugger;
    try {
        if (id) {
            var data = { "ID": id };
            _jsonData = GetDataFromServer("Company/DeleteCompany/", data);
            if (_jsonData != '') {
                _jsonData = JSON.parse(_jsonData);
                _message = _jsonData.Message;
                _status = _jsonData.Status;
                _result = _jsonData.Record;
            }
            switch (_status) {
                case "OK":
                    notyAlert('success', _result.Message);
                    BindOrReloadCompanyTable('Reset');
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