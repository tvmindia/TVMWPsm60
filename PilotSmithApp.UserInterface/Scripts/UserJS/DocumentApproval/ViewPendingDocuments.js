﻿var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";

$(document).ready(function () {
    debugger;
    try {
       
        BindOrReloadDocumentApprovals('Init');
        $('#tblPendingDocuments tbody').on('dblclick', 'td', function () {
            Edit(this);
        });
    }
    catch (e) {
        console.log(e.message);
    }
});

//bind Pending list
function BindOrReloadDocumentApprovals(action) {
    try {
        //creating advancesearch object
        debugger;
        DocumentApprovalAdvanceSearchViewModel = new Object();
        DataTablePagingViewModel = new Object();
        DocumentTypeViewModel = new Object();

        DataTablePagingViewModel.Length = 0;
        //switch case to check the operation
        switch (action) {
            case 'Reset':
                $('#SearchTerm').val('');
                $('#FromDate').val('');
                $('#ToDate').val('');
                $('#DocumentTypeCode').val('');
                $('#ShowAll').val('false');
                break;
            case 'Init':
                break;
            case 'Apply':
                break;
            case 'Export':
                DataTablePagingViewModel.Length = -1;
                break;
            default:
                break;
        }
        DocumentApprovalAdvanceSearchViewModel.SearchTerm = $('#SearchTerm').val();
        DocumentApprovalAdvanceSearchViewModel.FromDate = $('#FromDate').val();
        DocumentApprovalAdvanceSearchViewModel.ToDate = $('#ToDate').val();
        DocumentTypeViewModel.Code = $('#DocumentTypeCode').val();      
        DocumentApprovalAdvanceSearchViewModel.ShowAll = $('#ShowAll').val();
        DocumentApprovalAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
        DocumentApprovalAdvanceSearchViewModel.DocumentType = DocumentTypeViewModel;
        DataTables.PurchaseOrderList = $('#tblPendingDocuments').DataTable(
            {
                dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
                buttons: [{
                    extend: 'excel',
                    exportOptions:
                    {
                        columns: [1, 2, 3, 4, 5]
                    }
                }],
                order: false,
                ordering: false,
                searching: false,
                paging: true,
                lengthChange: false,
                proccessing: true,
                serverSide: true,
                ajax: {
                    url: "GetAllDocumentApproval/",
                    data: { "documentApprovalAdvanceSearchVM": DocumentApprovalAdvanceSearchViewModel },
                    type: 'POST'
                },
                pageLength: 13,
                columns: [
                    { "data": "DocumentType", "defaultContent": "<i>-</i>" },
                    { "data": "DocumentNo", "defaultContent": "<i>-</i>" },
                    { "data": "DocumentDateFormatted", "defaultContent": "<i>-</i>" },
                    { "data": "ApproverLevel", "defaultContent": "<i>-</i>" },
                    { "data": "DocumentCreatedBy", "defaultContent": "<i>-</i>" },
                    {
                        "data": "ApprovalLogID", "orderable": false, render: function (data, type, row) {
                            debugger;
                            return '<a href="#" class="actionLink" onclick="Edit(this)" ><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>'
                        }, "defaultContent": "<i>-</i>"
                    }
                ],
                columnDefs: [{ "targets": [0,1], "visible": false, "searchable": false },
                    { className: "text-left", "targets": [2,3] },
                    { className: "text-center", "targets": [4,5] }],
                destroy: true,
                //for performing the import operation after the data loaded
                initComplete: function (settings, json) {
                    $('.dataTables_wrapper div.bottom div').addClass('col-md-6');
                    $('#tblPendingDocuments').fadeIn(100);
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
                        BindOrReloadQuotationTable();
                    }
                }
            });
        $(".buttons-excel").hide();
    }
    catch (e) {
        console.log(e.message);
    }
}


function ResetPendingDocList() {
    $(".searchicon").removeClass('filterApplied');
    BindOrReloadDocumentApprovals('Reset');
}
//function export data to excel
function ExportPendingDocs() {
    $('.excelExport').show();
    OnServerCallBegin();
    BindOrReloadDocumentApprovals('Export');
}

function Edit(curObj) {

    OnServerCallBegin();
    debugger;
    var rowData = DataTables.PurchaseOrderList.row($(curObj).parents('tr')).data();
    $("#divPendingDocumentForm").load("/DocumentApproval/ApproveDocument?ID=" + rowData.ApprovalLogID + "&DocType=" + rowData.DocumentTypeCode + '&DocID=' + rowData.DocumentID, function () {
        OnServerCallComplete();
        setTimeout(function () {
            //resides in customjs for sliding
            openNav();
        }, 100);
    });
    //window.location.replace("ApproveDocument?code=APR&ID=" + rowData.ApprovalLogID + '&DocType=' + rowData.DocumentTypeCode + '&DocID=' + rowData.DocumentID);
}
