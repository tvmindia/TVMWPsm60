var _dataTable = {};
var _emptyGuid = "00000000-0000-0000-0000-000000000000";
var _datatablerowindex = -1;
var _jsonData = {};

//---------------------------------------Docuement Ready--------------------------------------------------//

$(document).ready(function () {
    debugger;
    try {

        BindOrReloadPendingSaleOrderReportTable('Init');
        $(' #AdvDocumentStatusCode,#AdvPreparedBy,#AdvCustomer,#AdvProduct,#AdvProductModel').select2({
            dropdownParent: $(".divboxASearch")
        });

        $('.select2').addClass('form-control newinput');

        $('#DateFilter').select2({

        });
        $('.select2').addClass('form-control newinput');


    }
    catch (e) {
        console.log(e.message);
    }
});
//function bind the pending sale order list checking search and filter
function BindOrReloadPendingSaleOrderReportTable(action) {
    try {
        debugger;
        //creating advancesearch object
        PendingSaleOrderReportViewModel = new Object();
        DataTablePagingViewModel = new Object();
        DataTablePagingViewModel.Length = 0;
        //switch case to check the operation
        switch (action) {
            case 'Reset':
                $('#SearchTerm').val('');
                $('.divboxASearch #AdvFromDate').val('');
                $('.divboxASearch #AdvToDate').val('');
                $('.divboxASearch #AdvAmountFrom').val('');
                $('.divboxASearch #AdvAmountTo').val('');
                $('.divboxASearch #AdvAreaCode').val('').trigger('change');
                $('.divboxASearch #AdvCustomer').val('').trigger('change');
                $('.divboxASearch #AdvBranchCode').val('').trigger('change');
                $('.divboxASearch #AdvDocumentStatusCode').val('').trigger('change');
                $('.divboxASearch #AdvDocumentOwnerID').val('').trigger('change');
                $('.divboxASearch #AdvPreparedBy').val('').trigger('change');
                $('.divboxASearch #AdvCountryCode').val('').trigger('change');
                $('.divboxASearch #AdvStateCode').val('').trigger('change');
                $('.divboxASearch #AdvDistrictCode').val('').trigger('change');
                $('.divboxASearch #AdvCustomerCategoryCode').val('').trigger('change');
                $('.divboxASearch #AdvReferencePersonCode').val('').trigger('change');
                $('.divboxASearch #AdvApprovalStatusCode').val('').trigger('change');
                $('.divboxASearch #AdvProduct').val('').trigger('change');
                $('.divboxASearch #AdvProductModel').val('').trigger('change');
                $('#ReportType').val("1").trigger('change');
                $('.divboxASearch #AdvDelFromDate').val('');
                $('.divboxASearch #AdvDelToDate').val('');
                $('#DateFilter').val('').trigger('change');             
                $('#productionOrder')[0].checked = true;
                


                break;
            case 'Init':
                $('#SearchTerm').val('');
                $('.divboxASearch #AdvFromDate').val('');
                $('.divboxASearch #AdvToDate').val('');                
                $('.divboxASearch #AdvAreaCode').val('');
                $('.divboxASearch #AdvCustomer').val('');
                $('.divboxASearch #AdvBranchCode').val('');
                $('.divboxASearch #AdvDocumentStatusCode');
                $('.divboxASearch #AdvDocumentOwnerID').val('');
                $('.divboxASearch #AdvPreparedBy').val('');
                $('.divboxASearch #AdvAmountFrom').val('');
                $('.divboxASearch #AdvAmountTo').val('');
                $('.divboxASearch #AdvCountryCode').val('');
                $('.divboxASearch #AdvStateCode').val('');
                $('.divboxASearch #AdvDistrictCode').val('');
                $('.divboxASearch #AdvCustomerCategoryCode').val('');
                $('.divboxASearch #AdvReferencePersonCode').val('');
                $('.divboxASearch #AdvApprovalStatusCode').val('');
                $('.divboxASearch #AdvDelFromDate').val('');
                $('.divboxASearch #AdvDelToDate').val('');
                $('.divboxASearch #AdvProduct').val('');
                $('.divboxASearch #AdvProductModel').val('');
                $('#ReportType').val('1');
                $('#DateFilter').val('');
                
                break;
            case 'Search':
                if (($('#SearchTerm').val() == "") && ($('.divboxASearch #AdvFromDate').val() == "")
                    && ($('.divboxASearch #AdvToDate').val() == "") &&
                    ($('.divboxASearch #AdvDocumentOwnerID').val() == "") &&
                    ($('.divboxASearch #AdvCustomer').val() == "") &&
                    ($('.divboxASearch #AdvAreaCode').val() == "") &&
                    ($('.divboxASearch #AdvBranchCode').val() == "") &&
                    ($('.divboxASearch #AdvDocumentStatusCode').val() == "") &&
                    ($('.divboxASearch #AdvPreparedBy').val() == "") &&
                    ($('.divboxASearch #AdvAmountFrom').val() == "") &&
                    ($('.divboxASearch #AdvAmountTo').val() == "") &&
                     ($('.divboxASearch #AdvCountryCode').val() == "") &&
                    ($('.divboxASearch #AdvStateCode').val() == "") &&
                    ($('.divboxASearch #AdvDistrictCode').val() == "") &&
                    ($('.divboxASearch #AdvCustomerCategoryCode').val() == "") &&
                    ($('.divboxASearch #AdvReferencePersonCode').val() == "") &&
                    ($('.divboxASearch #AdvApprovalStatusCode').val() == "")
                    && ($('.divboxASearch #AdvDelFromDate').val() == "")
                    && ($('.divboxASearch #AdvDelToDate').val() == "")
                     && ($('.divboxASearch #AdvProduct').val() == "")
                    && ($('.divboxASearch #AdvProductModel').val() == "")                 
                
                    && ($('#ReportType').val() == "1")
                    &&($('#DateFilter').val() == "")
                    
                    ) {
                    return true;
                }
                break;
            case 'Export':
                DataTablePagingViewModel.Length = -1;
                PendingSaleOrderReportViewModel.DataTablePaging = DataTablePagingViewModel;
                PendingSaleOrderReportViewModel.SearchTerm = $('#SearchTerm').val() == "" ? null : $('#SearchTerm').val();
                PendingSaleOrderReportViewModel.AdvFromDate = $('.divboxASearch #AdvFromDate').val() == "" ? null : $('.divboxASearch #AdvFromDate').val();
                PendingSaleOrderReportViewModel.AdvToDate = $('.divboxASearch #AdvToDate').val() == "" ? null : $('.divboxASearch #AdvToDate').val();
                PendingSaleOrderReportViewModel.AdvDocumentOwnerID = $('.divboxASearch #AdvDocumentOwnerID').val() == "" ? _emptyGuid : $('.divboxASearch #AdvDocumentOwnerID').val();
                PendingSaleOrderReportViewModel.AdvCustomer = $('.divboxASearch #AdvCustomer').val() == "" ? null : $('.divboxASearch #AdvCustomer').val();
                PendingSaleOrderReportViewModel.AdvAreaCode = $('.divboxASearch #AdvAreaCode').val() == "" ? null : $('.divboxASearch #AdvAreaCode').val();
                PendingSaleOrderReportViewModel.AdvBranchCode = $('.divboxASearch #AdvBranchCode').val() == "" ? null : $('.divboxASearch #AdvBranchCode').val();
                PendingSaleOrderReportViewModel.AdvDocumentStatusCode = $('.divboxASearch #AdvDocumentStatusCode').val() == "" ? null : $('.divboxASearch #AdvDocumentStatusCode').val();
                PendingSaleOrderReportViewModel.AdvPreparedBy = $('.divboxASearch #AdvPreparedBy').val() == "" ? _emptyGuid : $('.divboxASearch #AdvPreparedBy').val();

                PendingSaleOrderReportViewModel.AdvAmountFrom = $('.divboxASearch #AdvAmountFrom').val() == "" ? null : $('.divboxASearch #AdvAmountFrom').val();
                PendingSaleOrderReportViewModel.AdvAmountTo = $('.divboxASearch #AdvAmountTo').val() == "" ? null : $('.divboxASearch #AdvAmountTo').val();
                PendingSaleOrderReportViewModel.AdvCountryCode = $('.divboxASearch #AdvCountryCode').val() == "" ? null : $('.divboxASearch #AdvCountryCode').val();
                PendingSaleOrderReportViewModel.AdvStateCode = $('.divboxASearch #AdvStateCode').val() == "" ? null : $('.divboxASearch #AdvStateCode').val();
                PendingSaleOrderReportViewModel.AdvDistrictCode = $('.divboxASearch #AdvDistrictCode').val() == "" ? null : $('.divboxASearch #AdvDistrictCode').val();
                PendingSaleOrderReportViewModel.AdvCustomerCategoryCode = $('.divboxASearch #AdvCustomerCategoryCode').val() == "" ? null : $('.divboxASearch #AdvCustomerCategoryCode').val();
                PendingSaleOrderReportViewModel.AdvReferencePersonCode = $('.divboxASearch #AdvReferencePersonCode').val() == "" ? null : $('.divboxASearch #AdvReferencePersonCode').val();
                PendingSaleOrderReportViewModel.AdvApprovalStatusCode = $('.divboxASearch #AdvApprovalStatusCode').val() == "" ? null : $('.divboxASearch #AdvApprovalStatusCode').val();
                PendingSaleOrderReportViewModel.AdvDelFromDate = $('.divboxASearch #AdvDelFromDate').val() == "" ? null : $('.divboxASearch #AdvDelFromDate').val();
                PendingSaleOrderReportViewModel.AdvDelToDate = $('.divboxASearch #AdvDelToDate').val() == "" ? null : $('.divboxASearch #AdvDelToDate').val();
                PendingSaleOrderReportViewModel.AdvReportType = $('#ReportType').val() == "" ? null : $('#ReportType').val();
                PendingSaleOrderReportViewModel.DateFilter = $('#DateFilter').val() == "" ? null : $('#DateFilter').val();
                PendingSaleOrderReportViewModel.AdvProduct = $('.divboxASearch #AdvProduct').val() == "" ? _emptyGuid : $('.divboxASearch #AdvProduct').val();
                PendingSaleOrderReportViewModel.AdvProductModel = $('.divboxASearch #AdvProductModel').val() == "" ? _emptyGuid : $('.divboxASearch #AdvProductModel').val();

                $('#AdvanceSearch').val(JSON.stringify(PendingSaleOrderReportViewModel));
                $('#FormExcelExport').submit();
                return true;
                break;
            default:
                break;
        }
        PendingSaleOrderReportViewModel.DataTablePaging = DataTablePagingViewModel;
        PendingSaleOrderReportViewModel.SearchTerm = $('#SearchTerm').val();
        PendingSaleOrderReportViewModel.AdvFromDate = $('.divboxASearch #AdvFromDate').val();
        PendingSaleOrderReportViewModel.AdvToDate = $('.divboxASearch #AdvToDate').val();
        PendingSaleOrderReportViewModel.AdvDocumentOwnerID = $('.divboxASearch #AdvDocumentOwnerID').val();
        PendingSaleOrderReportViewModel.AdvCustomer = $('.divboxASearch #AdvCustomer').val();
        PendingSaleOrderReportViewModel.AdvAreaCode = $('.divboxASearch #AdvAreaCode').val();
        PendingSaleOrderReportViewModel.AdvBranchCode = $('.divboxASearch #AdvBranchCode').val();
        PendingSaleOrderReportViewModel.AdvDocumentStatusCode = $('.divboxASearch #AdvDocumentStatusCode').val();
        PendingSaleOrderReportViewModel.AdvPreparedBy = $('.divboxASearch #AdvPreparedBy').val();
        PendingSaleOrderReportViewModel.AdvAmountFrom = $('.divboxASearch #AdvAmountFrom').val();
        PendingSaleOrderReportViewModel.AdvAmountTo = $('.divboxASearch #AdvAmountTo').val();
        PendingSaleOrderReportViewModel.AdvCountryCode = $('.divboxASearch #AdvCountryCode').val();
        PendingSaleOrderReportViewModel.AdvStateCode = $('.divboxASearch #AdvStateCode').val();
        PendingSaleOrderReportViewModel.AdvDistrictCode = $('.divboxASearch #AdvDistrictCode').val();
        PendingSaleOrderReportViewModel.AdvCustomerCategoryCode = $('.divboxASearch #AdvCustomerCategoryCode').val();
        PendingSaleOrderReportViewModel.AdvReferencePersonCode = $('.divboxASearch #AdvReferencePersonCode').val();
        PendingSaleOrderReportViewModel.AdvApprovalStatusCode = $('.divboxASearch #AdvApprovalStatusCode').val();
        PendingSaleOrderReportViewModel.AdvDelFromDate = $('.divboxASearch #AdvDelFromDate').val();
        PendingSaleOrderReportViewModel.AdvDelToDate = $('.divboxASearch #AdvDelToDate').val();
        PendingSaleOrderReportViewModel.AdvReportType = $('#ReportType').val();
        PendingSaleOrderReportViewModel.DateFilter = $('#DateFilter').val();
        PendingSaleOrderReportViewModel.AdvProduct = $('.divboxASearch #AdvProduct').val();
        PendingSaleOrderReportViewModel.AdvProductModel = $('.divboxASearch #AdvProductModel').val();
        
        //apply datatable plugin on pending sale order report table
        _dataTable.PendingSaleOrderReportList = $("#tblPendingSaleOrderReport").DataTable(
        {
            dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
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
                url: "GetPendingSaleOrderReport/",
                data: { "pendingSaleOrderReportVM": PendingSaleOrderReportViewModel },
                type: 'POST'
            },
            pageLength: 8,
            autoWidth: false,
            columns: [

               { "data": "SaleOrderNo", "defaultContent": "<i>-</i>" },
               { "data": "SaleOrderDateFormatted", "defaultContent": "<i>-</i>" },
               { "data": "Customer.CompanyName", "defaultContent": "<i>-</i>" },
               { "data": "Customer.ContactPerson", "defaultContent": "<i>-</i>" },
               { "data": "Product.Name", "defaultContent": "<i>-</i>" },
                { "data": "ProductModel.Name", "defaultContent": "<i>-</i>" },
               { "data": "ProductSpec", "defaultContent": "<i>-</i>" },
                { "data": "Qty", "defaultContent": "<i>-</i>" },                
                 { "data": "PendingQty", "defaultContent": "<i>-</i>" },

               { "data": "Unit.Description", "defaultContent": "<i>-</i>" },
                 { "data": "Amount", "defaultContent": "<i>-</i>" },
                 { "data": "Branch.Description", "defaultContent": "<i>-</i>" },
               { "data": "PSAUser.LoginName", "defaultContent": "<i>-</i>" },          
               
            ],
            columnDefs: [{ className: "text-right", "targets": [10] },
                         { className: "text-left", "targets": [0, 1, 2, 3, 4, 5, 6, 7, 8,9, 11,12] },
                         { className: "text-center", "targets": [] },
                           { "targets": [0], "width": "12%" },
                           { "targets": [1], "width": "12%" },
                           { "targets": [2], "width": "12%" },
                           { "targets": [3], "width": "12%" },
                           { "targets": [4], "width": "12%" },
                           { "targets": [5], "width": "12%" },
                           { "targets": [6], "width": "12%" },
                           { "targets": [7], "width": "12%" },
                           { "targets": [8], "width": "12%" },
                           { "targets": [9], "width": "12%" },
                           { "targets": [10], "width": "12%" },
                            { "targets": [11], "width": "12%" },
            ],
            destroy: true,
            //for performing the import operation after the data loaded
            initComplete: function (settings, json) {
                debugger;
                $('.dataTables_wrapper div.bottom div').addClass('col-md-6');
                $('#tblPendingSaleOrderReport').fadeIn('slow');
                if (action == undefined) {
                    $('.excelExport').hide();
                    OnServerCallComplete();
                }               
            }
        });

    }
    catch (e) {
        console.log(e.message);
    }
}

//function reset the list to initial
function ResetReportList() {
    $(".searchicon").removeClass('filterApplied');
    BindOrReloadPendingSaleOrderReportTable('Reset');
}
//function export data to excel
function ExportReportData() {
    debugger;
    //$('.excelExport').show();
    //OnServerCallBegin();
    BindOrReloadPendingSaleOrderReportTable('Export');
}

function ApplyFilterThenSearch() {
    $(".searchicon").addClass('filterApplied');
    CloseAdvanceSearch();
    BindOrReloadPendingSaleOrderReportTable('Search');
}

function GoToList() {
    window.location.href = "/Report";
}



function DateFilterOnchange() {
    debugger;
    var selectedValue = $("#DateFilter").val().split("||");
    $('.divboxASearch #AdvFromDate').val(selectedValue[0]);
    $('.divboxASearch #AdvToDate').val(selectedValue[1]);
    BindOrReloadPendingSaleOrderReportTable('Search');
}


function RadioButtonOnChange()
{
    debugger;
    if ($("#productionOrder").is(':checked')) {
       var  reportType1 = $("#productionOrder").val();
       $('#ReportType').val(reportType1);
     

    }
    else {
        var reportType2 = $("#saleInvoice").val();
        $('#ReportType').val(reportType2);

    }

    BindOrReloadPendingSaleOrderReportTable('Search');
}