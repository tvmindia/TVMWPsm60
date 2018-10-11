var _dataTables = {};
var _emptyGuid = "00000000-0000-0000-0000-000000000000";

$(document).ready(function () {
    debugger;
    try {
        BindOrReloadApprovalHistory('Init');
        $('#tblApprovalHistoryList tbody').on('dblclick', 'td', function () {
            Edit(this);
        });
        debugger;
        if ($('#DocID').val() != "") {
            EditRedirectFromDocument($('#DocID').val(), $('#ApprLogID').val(), $('#DocType').val());
        }

        $('#DocumentTypeCode,#ApproverLevel,#ApprovalStatus').select2({
            dropdownParent: $(".divboxASearch")
        });

        $('.select2').addClass('form-control newinput');

    }
    catch (e) {
        console.log(e.message);
    }
});


//bind Pending list
function BindOrReloadApprovalHistory(action) {
    try {
        //creating advancesearch object
        debugger;
        DocumentApprovalAdvanceSearchViewModel = new Object();
        DataTablePagingViewModel = new Object();
        DocumentTypeViewModel = new Object();

        DataTablePagingViewModel.Length = 0;
        var SearchValue = $('#hdnSearchTerm').val();
        var SearchTerm = $('#SearchTerm').val();
        $('#hdnSearchTerm').val($('#SearchTerm').val())
        //switch case to check the operation
        switch (action) {
            case 'Reset':
                $('#SearchTerm').val('');
                $('#FromDate').val('');
                $('#ToDate').val('');
                $('#DocumentTypeCode').val('');
                $('#ApprovalStatus').val('');
                $('#ApproverLevel').val('');
                //$('#ShowAll').val('false');
                break;
            case 'Init':
                break;
            case 'Search':
                if((SearchTerm==SearchValue)&&
                  ($('#FromDate').val()=="")&&
                    ($('#ToDate').val()=="")&&
                    ($('#DocumentTypeCode').val()=="")&&
                    ($('#ApprovalStatus').val()=="")&&
                    ($('#ApproverLevel').val() == "")) {

                    return true;
                }
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
        //DocumentApprovalAdvanceSearchViewModel.ShowAll = $('#ShowAll').val();
        DocumentApprovalAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
        DocumentApprovalAdvanceSearchViewModel.DocumentType = DocumentTypeViewModel;
        DocumentApprovalAdvanceSearchViewModel.ApprovalStatus = $('#ApprovalStatus').val();
        DocumentApprovalAdvanceSearchViewModel.ApproverLevel = $('#ApproverLevel').val();
        DataTables.ApprovalHistoryList = $('#tblApprovalHistoryList').DataTable(
            {
                dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
                buttons: [{
                    extend: 'excel',
                    exportOptions:
                    {
                        columns: [1, 2, 3, 4, 5, 6]
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
                    url: "GetAllApprovalHistory/",
                    data: { "documentApprovalAdvanceSearchVM": DocumentApprovalAdvanceSearchViewModel },
                    type: 'POST'
                },
                pageLength: 13,
                columns: [
                    { "data": "DocumentType", "defaultContent": "<i>-</i>" },
                    { "data": "DocumentNo", "defaultContent": "<i>-</i>" },
                    { "data": "DocumentDateFormatted", "defaultContent": "<i>-</i>" },
                    { "data": "ApproverLevel", "defaultContent": "<i>-</i>" },
                    { "data": "DocumentStatus", "defaultContent": "<i>-</i>" },
                    { "data": "DocumentCreatedBy", "defaultContent": "<i>-</i>" },
                    { "data": "DocumentOwner", "defaultContent": "<i>-</i>" },
                   {
                        "data": "null", "orderable": false, render: function (data, type, row) {
                            debugger;
                            return '<a href="#" class="actionLink" onclick="Edit(this)" ><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>'
                        }, "defaultContent": "<i>-</i>"
                    }
                ],
                columnDefs: [{ "targets": [], "visible": false, "searchable": false },
                    { className: "text-left", "targets": [0, 1, 3, 4] },
                    { className: "text-center", "targets": [2, 5, 6] }],
                destroy: true,
                //for performing the import operation after the data loaded
                initComplete: function (settings, json) {
                    $('.dataTables_wrapper div.bottom div').addClass('col-md-6');
                    $('#tblApprovalHistoryList').fadeIn(100);
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
                        BindOrReloadApprovalHistory();
                    }
                }
            });
        $(".buttons-excel").hide();
    }
    catch (e) {
        console.log(e.message);
    }
}

function ResetApprovalHistory() {
    $(".searchicon").removeClass('filterApplied');
    BindOrReloadApprovalHistory('Reset');
}
//function export data to excel
function ExportApprovalHistory() {
    $('.excelExport').show();
    OnServerCallBegin();
    BindOrReloadApprovalHistory('Export');
}

//ApplyFilterThenSearch
function ApplyFilterThenSearch() {
    $(".searchicon").addClass('filterApplied');
    CloseAdvanceSearch();
    BindOrReloadApprovalHistory('Search');
}

function Edit(curObj) {

    OnServerCallBegin();
    debugger;
    var documentApprovalVM = DataTables.ApprovalHistoryList.row($(curObj).parents('tr')).data();
    $("#divApprovalHistoryForm").load("/DocumentApproval/ApproveDocument?ID=" + documentApprovalVM.ApprovalLogID + "&DocType=" + documentApprovalVM.DocumentTypeCode + '&DocID=' + documentApprovalVM.DocumentID, function () {
        OnServerCallComplete();
        debugger;
        ChangeButtonPatchView("DocumentApproval", "btnPatchApprovalHistoryNew", "Close"/*, documentApprovalVM.ID*/);
        setTimeout(function () {
            //resides in customjs for sliding
            openNav();
        }, 100);
    });
}

function EditRedirectFromDocument(DocumentID, ApprovalLogID, DocumentType) {
    OnServerCallBegin();
    debugger;
    $("#divApprovalHistoryForm").load("/DocumentApproval/ApproveDocument?ID=" + ApprovalLogID + "&DocType=" + DocumentType + '&DocID=' + DocumentID, function () {
        OnServerCallComplete();
        ChangeButtonPatchView("DocumentApproval", "btnPatchApprovalHistoryNew", "Close");
        setTimeout(function () {
            //resides in customjs for sliding
            openNav();
        }, 100);
    });
}