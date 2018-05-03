var _dataTable = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
var _jsonData = {};
var _message = "";
var _status = "";
var _result = "";

$(document).ready(function () {
    try {
        BindOrReloadEmployeeTable('Init');
        $('#tblEmployee tbody').on('dblclick', 'td', function () {
            EditEmployee(this);
        });
    }
    catch (e) {
        console.log(e.message);
    }
});

//function bind the Employee list checking search and filter
function BindOrReloadEmployeeTable(action) {
    try {
        debugger;
        //creating advancesearch object
        EmployeeAdvanceSearchViewModel = new Object();
        DataTablePagingViewModel = new Object();
        DataTablePagingViewModel.Length = 0;
        //switch case to check the operation
        switch (action) {
            case 'Reset':
                $('#SearchTerm').val('');
                $('#FromDate').val('');
                $('#ToDate').val('');
                break;
            case 'Init':
                $('#SearchTerm').val('');
                $('#FromDate').val('');
                $('#ToDate').val('');
                break;
            case 'Search':
                if (($('#SearchTerm').val() == "") && ($('#FromDate').val() == "") && ($('#ToDate').val() == "")) {
                    return true;
                }
                break;
            case 'Export':
                DataTablePagingViewModel.Length = -1;
                break;
            default:
                break;
        }
        EmployeeAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
        EmployeeAdvanceSearchViewModel.SearchTerm = $('#SearchTerm').val();
        EmployeeAdvanceSearchViewModel.FromDate = $('#FromDate').val();
        EmployeeAdvanceSearchViewModel.ToDate = $('#ToDate').val();
        //apply datatable plugin on Employee table
        _dataTable.EmployeeList = $('#tblEmployee').DataTable(
        {
            dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
            buttons: [{
                extend: 'excel',
                exportOptions:
                             {
                                 columns: [0,1,2,3,4]
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
                url: "Employee/GetAllEmployee/",
                data: { "employeeAdvanceSearchVM": EmployeeAdvanceSearchViewModel },
                type: 'POST'
            },
            pageLength: 13,
            columns: [               
               { "data": "Code", "defaultContent": "<i>-</i>" },
               { "data": "Name", "defaultContent": "<i>-</i>" },
               { "data": "MobileNo", "defaultContent": "<i>-</i>" },
               { "data": "DepartmentCode", "defaultContent": "<i>-</i>" },
               { "data": "PositionCode", "defaultContent": "<i>-</i>" },
               { "data": null, "orderable": false, "defaultContent": '<a href="#" onclick="EditEmployeeMaster(this)"<i class="fa fa-pencil-square-o" aria-hidden="true"></i></a>  <a href="#" onclick="DeleteEmployeeMaster(this)"<i class="fa fa-trash-o" aria-hidden="true"></i></a>' },
            ],
            //columnDefs: [
            //      { className: "text-left", "targets": [0, 1] },
            //{ className: "text-center", "targets": [2, 3, 4] }
            //],
            destroy: true,
            //for performing the import operation after the data loaded
            initComplete: function (settings, json) {
                debugger;
                $('.dataTables_wrapper div.bottom div').addClass('col-md-6');
                $('#tblEmployee').fadeIn(100);
                if (action == undefined) {
                    $('.excelExport').hide();
                    OnServerCallComplete();
                }
                if (action === 'Export') {
                    if (json.data.length > 0) {
                        if (json.data[0].TotalCount > 1000) {
                            setTimeout(function () {
                                MasterAlert("info", 'We are able to download maximum 1000 rows of data, There exist more than 1000 rows of data please filter and download')
                            }, 10000)
                        }
                    }
                    $(".buttons-excel").trigger('click');
                    BindOrReloadEmployeeTable();
                }
            }
        });
        $(".buttons-excel").hide();
    }
    catch (e) {
        console.log(e.message);
    }
}

//function reset the list to initial
function ResetEmployeeList() {
    $(".searchicon").removeClass('filterApplied');
    BindOrReloadEmployeeTable('Reset');
}
//function export data to excel
function ExportEmployeeData() {
    $('.excelExport').show();
    OnServerCallBegin();
    BindOrReloadEmployeeTable('Export');
}

function EditEmployeeMaster(this_Obj) {
    var Employee = _dataTable.EmployeeList.row($(this_Obj).parents('tr')).data();
    //this will return form body(html)
    $("#divEmployeeForm").load("Employee/EmployeeForm?id=" + Employee.ID, function () {
        ChangeButtonPatchView("Employee", "btnPatchEmployeeNew", "Edit");
        //resides in customjs for sliding
        openNav();
    });
}
function DeleteEmployeeMaster(thisObj) {
    debugger;
    EmployeeVM = _dataTables.EmployeeList.row($(thisObj).parents('tr')).data();
    notyConfirm('Are you sure to delete?', 'DeleteEmployee("' + EmployeeVM.Code + '")');
}

function DeleteEmployee(code) {
    debugger;
    try {
        if (code) {
            var data = { "code": code };
            _jsonData = GetDataFromServer("Employee/DeleteEmployee/", data);
            if (_jsonData != '') {
                _jsonData = JSON.parse(_jsonData);
                _message = _jsonData.Message;
                _status = _jsonData.Status;
                _result = _jsonData.Record;
            }
            switch (_status) {
                case "OK":
                    notyAlert('success', _result.Message);
                    BindOrReloadEmployeeTable('Reset');
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