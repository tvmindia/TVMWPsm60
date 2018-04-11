var _dataTable = {};
var _emptyGuid = "00000000-0000-0000-0000-000000000000";
var _jsonData = {};
var _message = "";
var _status = "";
var _result = "";
//---------------------------------------Docuement Ready--------------------------------------------------//
$(document).ready(function () {
    try {
        BindOrReloadEnquiryTable('Init');
      
    }
    catch (e) {
        console.log(e.message);
    }
});
//function bind the Enquiry list checking search and filter
function BindOrReloadEnquiryTable(action) {
    try {
        //creating advancesearch object
        EnquiryAdvanceSearchViewModel = new Object();
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
        EnquiryAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
        EnquiryAdvanceSearchViewModel.SearchTerm = $('#SearchTerm').val();
        EnquiryAdvanceSearchViewModel.FromDate = $('#FromDate').val();
        EnquiryAdvanceSearchViewModel.ToDate = $('#ToDate').val();
        //apply datatable plugin on Enquiry table
        _dataTable.EnquiryList = $('#tblEnquiry').DataTable(
        {
            dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
            buttons: [{
                extend: 'excel',
                exportOptions:
                             {
                                 columns: [1, 2, 3, 4, 5, 6]
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
                url: "Enquiry/GetAllEnquiry/",
                data: { "EnquiryAdvanceSearchVM": EnquiryAdvanceSearchViewModel },
                type: 'POST'
            },
            pageLength: 13,
            columns: [
               { "data": "EnquiryNo", "defaultContent": "<i>-</i>" },
               { "data": "EnquiryDate", "defaultContent": "<i>-</i>" },
               { "data": "Customer.CompanyName", "defaultContent": "<i>-</i>" },
               { "data": "RequirementSpec", "defaultContent": "<i>-</i>" },
               { "data": "ReferredByCode", "defaultContent": "<i>-</i>" },
               { "data": "ResponsiblePersonID", "defaultContent": "<i>-</i>" },
               { "data": "AttendedByID", "defaultContent": "<i>-</i>" },
               { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink"  onclick="EditEnquiry(this)" ><i class="fa fa-pencil-square-o" aria-hidden="true"></i></a>' },
            ],
            columnDefs: [{ className: "text-right", "targets": [5] },
                  { className: "text-left", "targets": [0, 1] },
            { className: "text-center", "targets": [2, 3, 4] }
            ],
            destroy: true,
            //for performing the import operation after the data loaded
            initComplete: function (settings, json) {
                debugger;
                $('.dataTables_wrapper div.bottom div').addClass('col-md-6');
                $('#tblEnquiry').fadeIn('slow');
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
                    BindOrReloadEnquiryTable();
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
function ResetEnquiryList() {
    $(".searchicon").removeClass('filterApplied');
    BindOrReloadEnquiryTable('Reset');
}
//function export data to excel
function ExportEnquiryData() {
    $('.excelExport').show();
    OnServerCallBegin();
    BindOrReloadEnquiryTable('Export');
}
// add Enquiry section
function AddEnquiry() {
    //this will return form body(html)
    $("#divEnquiryForm").load("Enquiry/EnquiryForm?id=" + _emptyGuid, function () {
        ChangeButtonPatchView("Enquiry", "btnPatchEnquiryNew", "Add");
        BindEnquiryDetailList(_emptyGuid);
        //resides in customjs for sliding
        openNav();
    });
}
function EditEnquiry(this_Obj) {
    var Enquiry = _dataTable.EnquiryList.row($(this_Obj).parents('tr')).data();
    //this will return form body(html)
    $("#divEnquiryForm").load("Enquiry/EnquiryForm?id=" + Enquiry.ID, function () {
        ChangeButtonPatchView("Enquiry", "btnPatchEnquiryNew", "Edit");
        //resides in customjs for sliding
        openNav();
    });
}
function ResetEnquiry() {
    //this will return form body(html)
    $('#divEnquiryForm').load("Enquiry/EnquiryForm?id=" + $('#ID').val());
}
function SaveEnquiry() {
    $('#btnInsertUpdateEnquiry').trigger('click');
}
function ApplyFilterThenSearch() {
    $(".searchicon").addClass('filterApplied');
    CloseAdvanceSearch();
    BindOrReloadEnquiryTable('Search');
}
function SaveSuccessEnquiry(data, status) {
    try {
        debugger;
        var _jsonData = JSON.parse(data)
        //message field will return error msg only
        _message = _jsonData.Message;
        _status = _jsonData.Status;
        _result = _jsonData.Record;
        switch (_status) {
            case "OK":
                $('#IsUpdate').val('True');
                $('#ID').val(_result.ID);
                ChangeButtonPatchView("Enquiry", "btnPatchEnquiryNew", "Edit");
                BindOrReloadEnquiryTable('Init');
                notyAlert('success', _result.Message);
                break;
            case "ERROR":
                notyAlert('error', _message);
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

function DeleteEnquiry() {
    notyConfirm('Are you sure to delete?', 'DeleteItem("' + $('#ID').val() + '")');
}
function DeleteItem(id) {
    try {
        if (id) {
            var data = { "id": id };
            _jsonData = GetDataFromServer("Enquiry/DeleteEnquiry/", data);
            if (_jsonData != '') {
                _jsonData = JSON.parse(_jsonData);
                _message = _jsonData.Message;
                _status = _jsonData.Status;
                _result = _jsonData.Record;
            }
            switch (_status) {
                case "OK":
                    notyAlert('success', _result.Message);
                    $('#ID').val(_emptyGuid);
                    ChangeButtonPatchView("Enquiry", "btnPatchEnquiryNew", "Add");
                    ResetEnquiry();
                    BindOrReloadEnquiryTable('Init');
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
        //this will show the error msg in the browser console(F12) 
        console.log(e.message);
    }
}
function BindEnquiryDetailList(id) {
    _dataTable.EnquiryDetailList = $('#tblEnquiryDetails').DataTable(
         {
             dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
             order: [],
             searching: false,
             paging: true,
             data: id=_emptyGuid?null:GetEnquiryDetailListByEnquiryID(id),
             language: {
                 search: "_INPUT_",
                 searchPlaceholder: "Search"
             },
             columns: [
             { "data": "ID", "defaultContent": "<i></i>" },
             { "data": "ProductID", "defaultContent": "<i></i>" },
             { "data": "ProductCode", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
             { "data": "OldProductCode", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
             { "data": "Rate", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
             { "data": "TaxPerc", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
             { "data": "ProductDescription", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
             { "data": null, "orderable": false, "defaultContent": '<a href="#" class="DeleteLink"  onclick="DeleteClick(this)" ><i class="glyphicon glyphicon-trash" aria-hidden="true"></i></a> | <a href="#" class="actionLink"  onclick="ProductEdit(this)" ><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>' },
             ],
             columnDefs: [{ "targets": [0, 1], "visible": false, "searchable": false },
                 { "targets": [2, 3], "width": "15%" },
                 { "targets": [4, 5, 6], "width": "20%" },
                  { className: "text-right", "targets": [] },
                   { className: "text-left", "targets": [2, 3, 4] },
             { className: "text-center", "targets": [5] }
             ]
         });
}
function GetEnquiryDetailListByEnquiryID(id) {
    debugger;
    try {

        var data = { "ID": id };
        var enquiryDetailList = [];
        _jsonData = GetDataFromServer("Enquiry/GetEnquiryDetailListByEnquiryID/", data);
        if (_jsonData != '') {
            _jsonData = JSON.parse(_jsonData);
            _message = _jsonData.Message;
            _status = _jsonData.Status;
            enquiryDetailList = _jsonData.Records;
        }
        if (_status == "OK") {
            return enquiryDetailList;
        }
        if (_status == "ERROR") {
            notyAlert('error', _message);
        }
    }
    catch (e) {
        //this will show the error msg in the browser console(F12) 
        console.log(e.message);

    }
}