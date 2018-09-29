﻿var _dataTable = {};
var _emptyGuid = "00000000-0000-0000-0000-000000000000";
var _datatablerowindex = -1;
var _jsonData = {};

//---------------------------------------Docuement Ready--------------------------------------------------//

$(document).ready(function () {
    debugger;
    try {

        BindOrReloadEstimateReportTable('Init');
        $(' #AdvDocumentStatusCode,#AdvPreparedBy,#AdvCustomer').select2({
            dropdownParent: $(".divboxASearch")
        });

        $('.select2').addClass('form-control newinput');

    }
    catch (e) {
        console.log(e.message);
    }
});
//function bind the Enquiry list checking search and filter
function BindOrReloadEstimateReportTable(action) {
    try {
        debugger;
        //creating advancesearch object
        EstimateReportViewModel = new Object();
        DataTablePagingViewModel = new Object();
        DataTablePagingViewModel.Length = 0;
        //switch case to check the operation
        switch (action) {
            case 'Reset':
                $('#SearchTerm').val('');
                $('.divboxASearch #AdvFromDate').val('');
                $('.divboxASearch #AdvToDate').val('');
                $('.divboxASearch #AdvAreaCode').val('').trigger('change');
                $('.divboxASearch #AdvCustomer').val('').trigger('change');
                $('.divboxASearch #AdvBranchCode').val('').trigger('change');
                $('.divboxASearch #AdvDocumentStatusCode').val('').trigger('change');
                $('.divboxASearch #AdvDocumentOwnerID').val('').trigger('change');                
                $('.divboxASearch #AdvPreparedBy').val('').trigger('change');              
                $('.divboxASearch #AdvAmountFrom').val('').trigger('change');
                $('.divboxASearch #AdvAmountTo').val('').trigger('change');
                $('.divboxASearch #AdvCountryCode').val('').trigger('change');
                $('.divboxASearch #AdvStateCode').val('').trigger('change');
                $('.divboxASearch #AdvDistrictCode').val('').trigger('change');
                $('.divboxASearch #AdvCustomerCategoryCode').val('').trigger('change');
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
                    ($('.divboxASearch #AdvAmountTo').val() == "")  &&
                     ($('.divboxASearch #AdvCountryCode').val() == "") &&
                    ($('.divboxASearch #AdvStateCode').val() == "") &&
                    ($('.divboxASearch #AdvDistrictCode').val() == "") &&
                    ($('.divboxASearch #AdvCustomerCategoryCode').val() == "")
                    ) {
                    return true;
                }
                break;
            case 'Export':
                DataTablePagingViewModel.Length = -1;
                EstimateReportViewModel.DataTablePaging = DataTablePagingViewModel;
                EstimateReportViewModel.SearchTerm = $('#SearchTerm').val() == "" ? null : $('#SearchTerm').val();
                EstimateReportViewModel.AdvFromDate = $('.divboxASearch #AdvFromDate').val() == "" ? null : $('.divboxASearch #AdvFromDate').val();
                EstimateReportViewModel.AdvToDate = $('.divboxASearch #AdvToDate').val() == "" ? null : $('.divboxASearch #AdvToDate').val();
                EstimateReportViewModel.AdvDocumentOwnerID = $('.divboxASearch #AdvDocumentOwnerID').val() == "" ? _emptyGuid : $('.divboxASearch #AdvDocumentOwnerID').val();
                EstimateReportViewModel.AdvCustomer = $('.divboxASearch #AdvCustomer').val() == "" ? null : $('.divboxASearch #AdvCustomer').val();
                EstimateReportViewModel.AdvAreaCode = $('.divboxASearch #AdvAreaCode').val() == "" ? null : $('.divboxASearch #AdvAreaCode').val();
                EstimateReportViewModel.AdvBranchCode = $('.divboxASearch #AdvBranchCode').val() == "" ? null : $('.divboxASearch #AdvBranchCode').val();
                EstimateReportViewModel.AdvDocumentStatusCode = $('.divboxASearch #AdvDocumentStatusCode').val() == "" ? null : $('.divboxASearch #AdvDocumentStatusCode').val();
                EstimateReportViewModel.AdvPreparedBy = $('.divboxASearch #AdvPreparedBy').val() == "" ? _emptyGuid : $('.divboxASearch #AdvPreparedBy').val();
               
                EstimateReportViewModel.AdvAmountFrom = $('.divboxASearch #AdvAmountFrom').val() == "" ? null : $('.divboxASearch #AdvAmountFrom').val();
                EstimateReportViewModel.AdvAmountTo = $('.divboxASearch #AdvAmountTo').val() == "" ? null : $('.divboxASearch #AdvAmountTo').val();               
                EstimateReportViewModel.AdvCountryCode = $('.divboxASearch #AdvCountryCode').val() == "" ? null : $('.divboxASearch #AdvCountryCode').val();
                EstimateReportViewModel.AdvStateCode = $('.divboxASearch #AdvStateCode').val() == "" ? null : $('.divboxASearch #AdvStateCode').val();
                EstimateReportViewModel.AdvDistrictCode = $('.divboxASearch #AdvDistrictCode').val() == "" ? null : $('.divboxASearch #AdvDistrictCode').val();
                EstimateReportViewModel.AdvCustomerCategoryCode = $('.divboxASearch #AdvCustomerCategoryCode').val() == "" ? null : $('.divboxASearch #AdvCustomerCategoryCode').val();

                $('#AdvanceSearch').val(JSON.stringify(EstimateReportViewModel));
                $('#FormExcelExport').submit();
                return true;
                break;
            default:
                break;
        }
        EstimateReportViewModel.DataTablePaging = DataTablePagingViewModel;
        EstimateReportViewModel.SearchTerm = $('#SearchTerm').val();
        EstimateReportViewModel.AdvFromDate = $('.divboxASearch #AdvFromDate').val();
        EstimateReportViewModel.AdvToDate = $('.divboxASearch #AdvToDate').val();
        EstimateReportViewModel.AdvDocumentOwnerID = $('.divboxASearch #AdvDocumentOwnerID').val();
        EstimateReportViewModel.AdvCustomer = $('.divboxASearch #AdvCustomer').val();
        EstimateReportViewModel.AdvAreaCode = $('.divboxASearch #AdvAreaCode').val();
        EstimateReportViewModel.AdvBranchCode = $('.divboxASearch #AdvBranchCode').val();
        EstimateReportViewModel.AdvDocumentStatusCode = $('.divboxASearch #AdvDocumentStatusCode').val();
        EstimateReportViewModel.AdvPreparedBy = $('.divboxASearch #AdvPreparedBy').val();
        EstimateReportViewModel.AdvAmountFrom = $('.divboxASearch #AdvAmountFrom').val();
        EstimateReportViewModel.AdvAmountTo = $('.divboxASearch #AdvAmountTo').val();
        EstimateReportViewModel.AdvCountryCode = $('.divboxASearch #AdvCountryCode').val();
        EstimateReportViewModel.AdvStateCode = $('.divboxASearch #AdvStateCode').val();
        EstimateReportViewModel.AdvDistrictCode = $('.divboxASearch #AdvDistrictCode').val();
        EstimateReportViewModel.AdvCustomerCategoryCode = $('.divboxASearch #AdvCustomerCategoryCode').val();


        //apply datatable plugin on enquiry report table
        _dataTable.EstimateReportList = $("#tblEstimateReport").DataTable(
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
                url: "GetEstimateReport/",
                data: { "estimateReportVM": EstimateReportViewModel },
                type: 'POST'
            },
            pageLength: 8,
            autoWidth: false,
            columns: [
              
               { "data": "EstimateNo", "defaultContent": "<i>-</i>" },
               { "data": "EstimateDateFormatted", "defaultContent": "<i>-</i>" },
               { "data": "Customer.CompanyName", "defaultContent": "<i>-</i>" },
               { "data": "Customer.ContactPerson", "defaultContent": "<i>-</i>" },           
         
               { "data": "Area.Description", "defaultContent": "<i>-</i>" },
               { "data": "PreparedBy", "defaultContent": "<i>-</i>" },           
               
               { "data": "DocumentStatus.Description", "defaultContent": "<i>-</i>" },
                { "data": "Branch.Description", "defaultContent": "<i>-</i>" },
               { "data": "PSAUser.LoginName", "defaultContent": "<i>-</i>" },               
               { "data": "Amount", "defaultContent": "<i>-</i>" },
               { "data": "Notes", "defaultContent": "<i>-</i>" },

                

            ],
            columnDefs: [{ className: "text-right", "targets": [9] },
                         { className: "text-left", "targets": [0, 2, 3, 4, 5, 6, 7, 8, 10] },
                         { className: "text-center", "targets": [1] },
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
                           
            ],
            destroy: true,
            //for performing the import operation after the data loaded
            initComplete: function (settings, json) {
                debugger;
                $('.dataTables_wrapper div.bottom div').addClass('col-md-6');
                $('#tblEstimateReport').fadeIn('slow');
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
    BindOrReloadEstimateReportTable('Reset');
}
//function export data to excel
function ExportReportData() {
    debugger;
    //$('.excelExport').show();
    //OnServerCallBegin();
    BindOrReloadEstimateReportTable('Export');
}

function ApplyFilterThenSearch() {
    $(".searchicon").addClass('filterApplied');
    CloseAdvanceSearch();
    BindOrReloadEstimateReportTable('Search');
}

function GoToList() {
    window.location.href = "/Report";
}