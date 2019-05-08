var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";

$(document).ready(function () {
    debugger;
    try {
       
        BindOrReloadDocumentApprovals('Init');
        $('#tblPendingDocuments tbody').on('dblclick', 'td', function () {
            Edit(this);
        });
        debugger;
        if ($('#DocID').val() != "") {
            EditRedirectFromDocument($('#DocID').val(), $('#ApprLogID').val(), $('#DocType').val());
        }
        $('#DocumentTypeCode,#ShowAll').select2({
            dropdownParent: $(".divboxASearch")
        });

        $('.select2').addClass('form-control newinput');

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
                $('#ShowAll').val('false');
                break;
            case 'Init':
                break;
            case 'Search':
                if ((SearchTerm == SearchValue) && ($('#FromDate').val('') == "") && ($('#ToDate').val('') == "")
                    && ($('#DocumentTypeCode').val('') == "") && ($('#ShowAll').val()=="")) {
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
                    { "data": "DocumentOwner", "defaultContent": "<i>-</i>" },
                   {
                        "data": "null", "orderable": false, render: function (data, type, row) {
                            debugger;
                            return '<a href="#" class="actionLink" onclick="Edit(this)" ><i class="glyphicon glyphicon-share-alt" aria-hidden="true"></i></a>'
                        }, "defaultContent": "<i>-</i>"
                    }
                ],
                columnDefs: [{ "targets": [], "visible": false, "searchable": false },
                    { className: "text-left", "targets": [3] },
                    { className: "text-center", "targets": [2,4,5] }],
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
                                //setTimeout(function () {
                                    MasterAlert("info", 'We are able to download maximum 1000 rows of data, There exist more than 1000 rows of data please filter and download')
                                //}, 10000)
                            }
                        }
                        $(".buttons-excel").trigger('click');
                        BindOrReloadDocumentApprovals();
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

//closes nav and rebinds table
function Close() {
    BindOrReloadDocumentApprovals('Init');
    closeNav();
}

//ApplyFilterThenSearch
function ApplyFilterThenSearch() {
    $(".searchicon").addClass('filterApplied');
    CloseAdvanceSearch();
    BindOrReloadDocumentApprovals('Search');
}

function Edit(curObj) {

    OnServerCallBegin();
    debugger;
    var rowData = DataTables.PurchaseOrderList.row($(curObj).parents('tr')).data();
    $("#divPendingDocumentForm").load("/DocumentApproval/ApproveDocument?ID=" + rowData.ApprovalLogID + "&DocType=" + rowData.DocumentTypeCode + '&DocID=' + rowData.DocumentID, function () {
        ChangeButtonPatchView("DocumentApproval", "btnPatchPendingDocumentNew", "Back"/*, documentApprovalVM.ID*/);
        OnServerCallComplete();
        setTimeout(function () {
            //resides in customjs for sliding
            openNav();
        }, 100);
    });
    //window.location.replace("ApproveDocument?code=APR&ID=" + rowData.ApprovalLogID + '&DocType=' + rowData.DocumentTypeCode + '&DocID=' + rowData.DocumentID);
}

function EditRedirectFromDocument(DocumentID, ApprovalLogID, DocumentType)
{
     OnServerCallBegin();
    debugger;   
    $("#divPendingDocumentForm").load("/DocumentApproval/ApproveDocument?ID=" + ApprovalLogID + "&DocType=" + DocumentType + '&DocID=' + DocumentID, function () {
        ChangeButtonPatchView("DocumentApproval", "btnPatchPendingDocumentNew", "Back"/*, documentApprovalVM.ID*/);
        OnServerCallComplete();
        setTimeout(function () {
            //resides in customjs for sliding
            openNav();
        }, 100);
    });
}
function DocumentPreview(DocumentType, DocumentID) {
	debugger;
	switch (DocumentType) {
		case "QUO":
			$("#divModelPreviewBody").load("/Quotation/PreviewQuotation?id=" + DocumentID, function () {
			debugger;
			$('#lblModelPreview').text('Quotation Preview');
			$('#divModelPreview').modal('show');
		});
			break;
		case "POD":
			$("#divModelPreviewBody").load("/ProductionOrder/PreviewProductionOrder?id=" + DocumentID, function () {
				debugger;
				$('#lblModelPreview').text('ProductionOrder Preview');
				$('#divModelPreview').modal('show');
			});
			break;
		case "SOD":
			$("#divModelPreviewBody").load("/SaleOrder/PreviewSaleOrder?id=" + DocumentID, function () {
				debugger;
				$('#lblModelPreview').text('SaleOrder Preview');
				$('#divModelPreview').modal('show');
			});
			break;
	    case "PIV":
	        $("#divModelPreviewBody").load("/ProformaInvoice/PreviewProformaInvoice?id=" + DocumentID, function () {
	            debugger;
	            $('#lblModelPreview').text('ProformaInvoice Preview');
	            $('#divModelPreview').modal('show');
	        });
	        break;
	}
	
}