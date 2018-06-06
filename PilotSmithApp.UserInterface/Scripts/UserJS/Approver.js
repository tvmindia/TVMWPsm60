
//*****************************************************************************
//*****************************************************************************
//Author: Thomson
//CreatedDate: 20-Apr-2018 
//LastModified: 24-May-2018 
//FileName: Approver.js
//Description: Client side coding for Approver
//******************************************************************************
//******************************************************************************

//--Global Declaration--//
var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";

//--Loading DOM--//
$(document).ready(function () {
    try {
        debugger;
        BindOrReloadApproverTable('Init');
        
    }
    catch (e) {
        console.log(e.message);
    }
    $("#DocumentTypeCode").select2({
        dropdownParent: $(".divboxASearch")
    });
    $('.select2').addClass('form-control newinput');
    
    
});


//--function bind the Approver list checking search and filter--//
function BindOrReloadApproverTable(action) {
    try {
        debugger;
        //creating advancesearch object
        ApproverAdvanceSearchViewModel = new Object();
        DataTablePagingViewModel = new Object();
        DocumentTypeViewModel = new Object();
        ApproverViewModel = new Object();
        DataTablePagingViewModel.Length = 0;
        //switch case to check the operation
        switch (action) {
            case 'Reset':
                $('#SearchTerm').val('');
                $('#DocumentTypeCode').val('').trigger('change');
                break;
            case 'Init':
                $('#SearchTerm').val('');
                $('#DocumentTypeCode').val('');
                break;
            case 'Search':
                if (($('#SearchTerm').val() == "") && ($('#DocumentTypeCode').val() == "") ) {
                    return true;
                }
                break;
            case 'Export':
                DataTablePagingViewModel.Length = -1;
                break;
            default:
                break;
        }
        ApproverAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
        ApproverAdvanceSearchViewModel.DocumentTypeCode = $('#DocumentTypeCode').val();
        ApproverAdvanceSearchViewModel.SearchTerm = $('#SearchTerm').val();

        //apply datatable plugin on Approver table
        DataTables.approverList = $('#tblApprover').DataTable(
        {
            dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
            buttons: [{
                extend: 'excel',
                exportOptions:
                             {
                                 columns: [0,1, 2,3, 4]
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
                url: "Approver/GetAllApprover/",
                data: { "approverAdvanceSearchVM": ApproverAdvanceSearchViewModel },
                type: 'POST'
            },
            pageLength: 13,
            columns: [
            //{ "data": "ID", "defaultContent": "<i>-</i>" },
            { "data": "DocumentType.Description", "defaultContent": "<i>-</i>", "width": "10%" },
            { "data": "Level", "defaultContent": "<i>-</i>", "width": "10%" },
            //{ "data": "UserID", "defaultContent": "<i>-<i>", "width": "10%" },
            { "data": "PSAUser.LoginName", "defaultContent": "<i>-<i>", "width": "10%" },
            { "data": "IsDefault", "defaultContent": "<i>-<i>", "width": "10%" },
            { "data": "IsActive", "defaultContent": "<i>-<i>", "width": "10%" },
            { "data": null, "orderable": false, "defaultContent": '<a href="#" onclick="EditApproverMaster(this)"<i class="fa fa-pencil-square-o" aria-hidden="true"></i></a> <a href="#" onclick="DeleteApproverMaster(this)"<i class="fa fa-trash-o" aria-hidden="true"></i></a>  ', "width": "4%" }
            ],
            columnDefs: [{ "targets": [], "visible": false, "searchable": false },
                { className: "text-right", "targets": [ ] },
                { className: "text-left", "targets": [0,1, 2,3, 4] },
                { className: "text-center", "targets": [5] }],
            destroy: true,
            //for performing the import operation after the data loaded
            initComplete: function (settings, json) {
                $('.dataTables_wrapper div.bottom div').addClass('col-md-6');
                $('#tblApprover').fadeIn(100);
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
                    BindOrReloadApproverTable();
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
function ResetApproverList() {
    $(".searchicon").removeClass('filterApplied');
    BindOrReloadApproverTable('Reset');
}

//--function export data to excel--//
function ImportApproverData() {
    BindOrReloadApproverTable('Export');
}

//Advance filter//
function ApplyFilterThenSearch() {
    debugger;
    $(".searchicon").addClass('filterApplied');
    CloseAdvanceSearch();
    BindOrReloadApproverTable('Search');
}
//--edit Approver--//
function EditApproverMaster(this_obj) {
    debugger;


    //rowData = DataTables.approverList.row($(this_obj).parents('tr')).data();
    //GetMasterPartial("Approver", rowData.ID);
    //$('#h3ModelMasterContextLabel').text('Edit Approver')
    //$('#divModelMasterPopUp').modal('show');
    //$('#hdnMasterCall').val('MSTR');
    //if ($('#IsDefault').is(":checked")) {
    //    $('#IsDefault').prop("disabled", true);
    //}
    //else {
    //    $('#IsDefault').prop("disabled", false);
    //}



    approverVM = DataTables.approverList.row($(this_obj).parents('tr')).data();
    //GetMasterPartial("Approver", rowData.ID);
    //$('#h3ModelMasterContextLabel').text('Edit Approver')
    //$('#divModelMasterPopUp').modal('show');
    //$('#hdnMasterCall').val('MSTR');
    $("#divMasterBody").load("Approver/MasterPartial?masterCode=" + approverVM.ID, function (responseTxt, statusTxt, xhr) {
        if (statusTxt == "success") {
            $('#hdnMasterCall').val('MSTR');
            $('#lblModelMasterContextLabel').text('Edit Approval Information')
            $('#divModelMasterPopUp').modal('show');

            if ($('#IsDefault').is(":checked")) {
                $('#IsDefault').prop("disabled", true);
            }
            else {
                $('#IsDefault').prop("disabled", false);
            }
        }
        else {
            console.log("Error: " + xhr.status + ": " + xhr.statusText);
        }
    });
   
}

//--Function To Confirm Approver Deletion 
function DeleteApproverMaster(this_obj) {
    debugger;
    approverVM = DataTables.approverList.row($(this_obj).parents('tr')).data();
    if (approverVM.IsDefault==true) {
        notyAlert('error', 'Cannot Delete Default Approver');
    } else {
        notyConfirm('Are you sure to delete?', 'DeleteApprover("' + approverVM.ID + '")');
    }
}

//--Function To Delete Approver
function DeleteApprover(id) {
    debugger;
    try {
        if (id) {
            var data = { "id": id};
            var jsonData = {};
            var message = "";
            var status = "";
            var result = "";
            jsonData = GetDataFromServer("Approver/DeleteApprover/", data);
            if (jsonData != '') {
                jsonData = JSON.parse(jsonData);
                message = jsonData.Message;
                status = jsonData.Status;
                result = jsonData.Record;
            }
            switch (status) {
                case "OK":
                    notyAlert('success', result.Message);
                    BindOrReloadApproverTable();
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
