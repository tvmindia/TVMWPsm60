var _dataTable = {};
var _emptyGuid = "00000000-0000-0000-0000-000000000000";
//---------------------------------------Docuement Ready--------------------------------------------------//
$(document).ready(function () {
    try {
        BindOrReloadCustomerTable('Init');
    }
    catch (e) {
        console.log(e.message);
    }
});
//function bind the Customer list checking search and filter
function BindOrReloadCustomerTable(action) {
    try {
        //creating advancesearch object
        CustomerAdvanceSearchViewModel = new Object();
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
                break;
            case 'Export':
                DataTablePagingViewModel.Length = -1;
                break;
            default:
                break;
        }
        CustomerAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
        CustomerAdvanceSearchViewModel.SearchTerm = $('#SearchTerm').val();

        //apply datatable plugin on Customer table
        _dataTable.customerList = $('#tblCustomer').DataTable(
        {
            dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
            buttons: [{
                extend: 'excel',
                exportOptions:
                             {
                                 columns: [1,2,3,4,5,6]
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
                url: "Customer/GetAllCustomer/",
                data: { "CustomerAdvanceSearchVM": CustomerAdvanceSearchViewModel },
                type: 'POST'
            },
            pageLength: 10,
            columns: [
               { "data": "ID" },
               {
                   "data": "CompanyName", render: function (data, type, row) {
                       if (row.IsInternalComp) {
                           return data + " ( <label><i><b> Internal </b></i></label> )";
                       }
                       else {
                           return data;
                       }
                   }, "defaultContent": "<i>-</i>"
               },
               { "data": "ContactPerson", "defaultContent": "<i>-</i>" },
               { "data": "Mobile", "defaultContent": "<i>-</i>" },
               { "data": "TaxRegNo", "defaultContent": "<i>-</i>" },
               { "data": "PANNO", "defaultContent": "<i>-</i>" },
                { "data": "OutStanding", "defaultContent": "<i>-</i>" },
               { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink"  onclick="EditCustomer(this)" ><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' }
            ],
            columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
                 { className: "text-right", "targets": [6] },
                  { className: "text-left", "targets": [1, 2] },
            { className: "text-center", "targets": [3, 4, 5] }

            ],
            destroy: true,
            //for performing the import operation after the data loaded
            initComplete: function (settings, json) {
                debugger;
                if (action === 'Export') {
                    if (json.data.length > 0) {
                        if (json.data[0].TotalCount > 10000) {
                            MasterAlert("info", 'We are able to download maximum 10000 rows of data, There exist more than 10000 rows of data please filter and download')
                        }
                    }
                    $(".buttons-excel").trigger('click');
                    BindOrReloadCustomerTable('Search');
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
function ResetCustomerList() {
    BindOrReloadCustomerTable('Reset');
}
//function export data to excel
function ExportCustomerData() {
    BindOrReloadCustomerTable('Export');
}
// add customer section
function AddCustomer() {
    //this will return form body(html)
    $("#divCustomerForm").load("Customer/CustomerForm?id=" + _emptyGuid, function () {
        ChangeButtonPatchView("Customer", "btnPatchCustomerNew", "Add");
        //resides in customjs for sliding
        openNav();
    });
}
function EditCustomer(this_Obj)
{
    var customer = _dataTable.customerList.row($(this_Obj).parents('tr')).data();
    //this will return form body(html)
    $("#divCustomerForm").load("Customer/CustomerForm?id=" + customer.ID, function () {
        ChangeButtonPatchView("Customer", "btnPatchCustomerNew", "Edit");
        //resides in customjs for sliding
        openNav();
    });
}
function ResetCustomer() {
    //this will return form body(html)
    $('#divCustomerForm').load("Customer/CustomerForm?id=" + $('#ID').val());
}
function SaveCustomer() {
    $('#btnInsertUpdateCustomer').trigger('click');
}

function SaveSuccessCustomer(data, status) {
    try {
        var jsonData = JSON.parse(data)
        //message field will return error msg only
        message = jsonData.Message;
        status = jsonData.Status;
        result = jsonData.Record;
        switch (status) {
            case "OK":
                $('#IsUpdate').val('True');
                $('#ID').val(result.ID);
                ChangeButtonPatchView("Customer", "btnPatchCustomerNew", "Edit");
                BindOrReloadCustomerTable('Init');
                notyAlert('success', result.Message);
                break;
            case "ERROR":
                notyAlert('error', message);
                break;
            default:
                break;
        }
    }
    catch (e) {
        //this will show the error msg in the browser console(F12) 
        console.log(e.message);
    }
}

function DeleteCustomer() {
    notyConfirm('Are you sure to delete?', 'DeleteItem("' + $('#ID').val() + '")');
}
function DeleteItem(id) {
    try {
        if (id) {
            var data = { "id": id };
            var jsonData = {};
            var message = "";
            var status = "";
            var result = "";
            jsonData = GetDataFromServer("Customer/DeleteCustomer/", data);
            if (jsonData != '') {
                jsonData = JSON.parse(jsonData);
                message = jsonData.Message;
                status = jsonData.Status;
                result = jsonData.Record;
            }
            switch (status) {
                case "OK":
                    notyAlert('success', result.Message);
                    BindOrReloadCustomerTable('Init');
                    break;
                case "ERROR":
                    notyAlert('error', message);
                    break;
                default:
                    break;
            }
        }
    }
    catch (e) {
        //this will show the error msg in the browser console(F12) 
        console.log(e.message);
    }
}