var _dataTable = {};
var _emptyGuid = "00000000-0000-0000-0000-000000000000";
var _datatablerowindex = -1;
var _jsonData = {};

//---------------------------------------Docuement Ready--------------------------------------------------//

$(document).ready(function () {
    debugger;
    try {

        BindOrReloadPendingProductionOrderReportTable('Init');
        $(' #AdvDocumentStatusCode,#AdvCustomer,#AdvProductModel,#AdvProduct').select2({
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
//function bind the Pending Production Order  report list checking search and filter
function BindOrReloadPendingProductionOrderReportTable(action) {
    try {
        debugger;
        //creating advancesearch object
        PendingProductionOrderReportViewModel = new Object();
        DataTablePagingViewModel = new Object();
        DataTablePagingViewModel.Length = 0;
        var SearchValue = $('#hdnSearchTerm').val();
        var SearchTerm = $('#SearchTerm').val();
        $('#hdnSearchTerm').val($('#SearchTerm').val())
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
                $('.divboxASearch #AdvDocumentStatusCode').val('7').trigger('change');
                $('.divboxASearch #AdvDocumentOwnerID').val('').trigger('change');
               // $('.divboxASearch #AdvPreparedBy').val('').trigger('change');
                $('.divboxASearch #AdvCountryCode').val('').trigger('change');
                $('.divboxASearch #AdvStateCode').val('').trigger('change');
                $('.divboxASearch #AdvDistrictCode').val('').trigger('change');
                $('.divboxASearch #AdvCustomerCategoryCode').val('').trigger('change');
                $('.divboxASearch #AdvReferencePersonCode').val('').trigger('change');
                //$('.divboxASearch #AdvApprovalStatusCode').val('').trigger('change');
                //$('.divboxASearch #AdvReportType').val('').trigger('change');
                //$('.divboxASearch #AdvDelFromDate').val('');
                //$('.divboxASearch #AdvDelToDate').val('');
                $('#DateFilter').val('').trigger('change');
                $('.divboxASearch #AdvProduct').val('').trigger('change');
                $('.divboxASearch #AdvProductModel').val('').trigger('change');
                $('.divboxASearch #AdvPlantCode').val('').trigger('change');



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
               // $('.divboxASearch #AdvPreparedBy').val('');
                $('.divboxASearch #AdvAmountFrom').val('');
                $('.divboxASearch #AdvAmountTo').val('');
                $('.divboxASearch #AdvCountryCode').val('');
                $('.divboxASearch #AdvStateCode').val('');
                $('.divboxASearch #AdvDistrictCode').val('');
                $('.divboxASearch #AdvCustomerCategoryCode').val('');
                $('.divboxASearch #AdvReferencePersonCode').val('');
                //$('.divboxASearch #AdvApprovalStatusCode').val('');
                //$('.divboxASearch #AdvDelFromDate').val('');
                //$('.divboxASearch #AdvDelToDate').val('');
                //$('.divboxASearch #AdvReportType').val('');
                $('#DateFilter').val('');
                $('.divboxASearch #AdvProduct').val('');
                $('.divboxASearch #AdvProductModel').val('');
                $('.divboxASearch #AdvPlantCode').val('');

                break;
            case 'Search':
                if ((SearchTerm == SearchValue) && ($('.divboxASearch #AdvFromDate').val() == "")
                    && ($('.divboxASearch #AdvToDate').val() == "") &&
                    ($('.divboxASearch #AdvDocumentOwnerID').val() == "") &&
                    ($('.divboxASearch #AdvCustomer').val() == "") &&
                    ($('.divboxASearch #AdvAreaCode').val() == "") &&
                    ($('.divboxASearch #AdvBranchCode').val() == "") &&
                    ($('.divboxASearch #AdvDocumentStatusCode').val() == "7") &&
                   // ($('.divboxASearch #AdvPreparedBy').val() == "") &&
                    ($('.divboxASearch #AdvAmountFrom').val() == "") &&
                    ($('.divboxASearch #AdvAmountTo').val() == "") &&
                     ($('.divboxASearch #AdvCountryCode').val() == "") &&
                    ($('.divboxASearch #AdvStateCode').val() == "") &&
                    ($('.divboxASearch #AdvDistrictCode').val() == "") &&
                    ($('.divboxASearch #AdvCustomerCategoryCode').val() == "") &&
                    ($('.divboxASearch #AdvReferencePersonCode').val() == "") 
                    //($('.divboxASearch #AdvApprovalStatusCode').val() == "")
                    //&& ($('.divboxASearch #AdvDelFromDate').val() == "")
                    //&& ($('.divboxASearch #AdvDelToDate').val() == "")
                    //&& ($('.divboxASearch #AdvReportType').val() == "")
                    && ($('.divboxASearch #AdvProduct').val() == "")
                    && ($('.divboxASearch #AdvProductModel').val() == "")
                    && ($('.divboxASearch #AdvPlantCode').val() == "")

                    ) {
                    return true;
                }
                break;
            case 'Export':
                DataTablePagingViewModel.Length = -1;
                PendingProductionOrderReportViewModel.DataTablePaging = DataTablePagingViewModel;

                PendingProductionOrderReportViewModel.SearchTerm = $('#SearchTerm').val() == "" ? null : $('#SearchTerm').val();
                PendingProductionOrderReportViewModel.AdvFromDate = $('.divboxASearch #AdvFromDate').val() == "" ? null : $('.divboxASearch #AdvFromDate').val();
                PendingProductionOrderReportViewModel.AdvFromDate = $('#AdvFromDate').val() == "" ? null : $('#AdvFromDate').val();
                PendingProductionOrderReportViewModel.AdvToDate = $('.divboxASearch #AdvToDate').val() == "" ? null : $('.divboxASearch #AdvToDate').val();
                PendingProductionOrderReportViewModel.AdvToDate = $('#AdvToDate').val() == "" ? null : $('#AdvToDate').val();

                PendingProductionOrderReportViewModel.AdvDocumentOwnerID = $('.divboxASearch #AdvDocumentOwnerID').val() == "" ? _emptyGuid : $('.divboxASearch #AdvDocumentOwnerID').val();
                PendingProductionOrderReportViewModel.AdvCustomer = $('.divboxASearch #AdvCustomer').val() == "" ? null : $('.divboxASearch #AdvCustomer').val();
                PendingProductionOrderReportViewModel.AdvAreaCode = $('.divboxASearch #AdvAreaCode').val() == "" ? null : $('.divboxASearch #AdvAreaCode').val();
                PendingProductionOrderReportViewModel.AdvBranchCode = $('.divboxASearch #AdvBranchCode').val() == "" ? null : $('.divboxASearch #AdvBranchCode').val();
                PendingProductionOrderReportViewModel.AdvDocumentStatusCode = $('.divboxASearch #AdvDocumentStatusCode').val() == "" ? null : $('.divboxASearch #AdvDocumentStatusCode').val();
                PendingProductionOrderReportViewModel.AdvPreparedBy = $('.divboxASearch #AdvPreparedBy').val() == "" ? _emptyGuid : $('.divboxASearch #AdvPreparedBy').val();

                PendingProductionOrderReportViewModel.AdvAmountFrom = $('.divboxASearch #AdvAmountFrom').val() == "" ? null : $('.divboxASearch #AdvAmountFrom').val();
                PendingProductionOrderReportViewModel.AdvAmountTo = $('.divboxASearch #AdvAmountTo').val() == "" ? null : $('.divboxASearch #AdvAmountTo').val();
                PendingProductionOrderReportViewModel.AdvCountryCode = $('.divboxASearch #AdvCountryCode').val() == "" ? null : $('.divboxASearch #AdvCountryCode').val();
                PendingProductionOrderReportViewModel.AdvStateCode = $('.divboxASearch #AdvStateCode').val() == "" ? null : $('.divboxASearch #AdvStateCode').val();
                PendingProductionOrderReportViewModel.AdvDistrictCode = $('.divboxASearch #AdvDistrictCode').val() == "" ? null : $('.divboxASearch #AdvDistrictCode').val();
                PendingProductionOrderReportViewModel.AdvCustomerCategoryCode = $('.divboxASearch #AdvCustomerCategoryCode').val() == "" ? null : $('.divboxASearch #AdvCustomerCategoryCode').val();
                PendingProductionOrderReportViewModel.AdvReferencePersonCode = $('.divboxASearch #AdvReferencePersonCode').val() == "" ? null : $('.divboxASearch #AdvReferencePersonCode').val();
                PendingProductionOrderReportViewModel.AdvApprovalStatusCode = $('.divboxASearch #AdvApprovalStatusCode').val() == "" ? null : $('.divboxASearch #AdvApprovalStatusCode').val();
                PendingProductionOrderReportViewModel.AdvDelFromDate = $('.divboxASearch #AdvDelFromDate').val() == "" ? null : $('.divboxASearch #AdvDelFromDate').val();
                PendingProductionOrderReportViewModel.AdvDelToDate = $('.divboxASearch #AdvDelToDate').val() == "" ? null : $('.divboxASearch #AdvDelToDate').val();
                PendingProductionOrderReportViewModel.AdvReportType = $('.divboxASearch #AdvReportType').val() == "" ? null : $('.divboxASearch #AdvReportType').val();
                PendingProductionOrderReportViewModel.DateFilter = $('#DateFilter').val() == "" ? null : $('#DateFilter').val();
                PendingProductionOrderReportViewModel.AdvProduct = $('.divboxASearch #AdvProduct').val() == "" ? _emptyGuid : $('.divboxASearch #AdvProduct').val();
                PendingProductionOrderReportViewModel.AdvProductModel = $('.divboxASearch #AdvProductModel').val() == "" ? _emptyGuid : $('.divboxASearch #AdvProductModel').val();
                PendingProductionOrderReportViewModel.AdvPlantCode = $('.divboxASearch #AdvPlantCode').val() == "" ? null : $('.divboxASearch #AdvPlantCode').val();


                $('#AdvanceSearch').val(JSON.stringify(PendingProductionOrderReportViewModel));
                $('#FormExcelExport').submit();
                return true;
                break;
            default:
                break;
        }
        PendingProductionOrderReportViewModel.DataTablePaging = DataTablePagingViewModel;
        PendingProductionOrderReportViewModel.SearchTerm = $('#SearchTerm').val();
        PendingProductionOrderReportViewModel.AdvFromDate = $('.divboxASearch #AdvFromDate').val();
        PendingProductionOrderReportViewModel.AdvToDate = $('.divboxASearch #AdvToDate').val();
        PendingProductionOrderReportViewModel.AdvDocumentOwnerID = $('.divboxASearch #AdvDocumentOwnerID').val();
        PendingProductionOrderReportViewModel.AdvCustomer = $('.divboxASearch #AdvCustomer').val();
        PendingProductionOrderReportViewModel.AdvAreaCode = $('.divboxASearch #AdvAreaCode').val();
        PendingProductionOrderReportViewModel.AdvBranchCode = $('.divboxASearch #AdvBranchCode').val();
        PendingProductionOrderReportViewModel.AdvDocumentStatusCode = $('.divboxASearch #AdvDocumentStatusCode').val();
        PendingProductionOrderReportViewModel.AdvPreparedBy = $('.divboxASearch #AdvPreparedBy').val();
        PendingProductionOrderReportViewModel.AdvAmountFrom = $('.divboxASearch #AdvAmountFrom').val();
        PendingProductionOrderReportViewModel.AdvAmountTo = $('.divboxASearch #AdvAmountTo').val();
        PendingProductionOrderReportViewModel.AdvCountryCode = $('.divboxASearch #AdvCountryCode').val();
        PendingProductionOrderReportViewModel.AdvStateCode = $('.divboxASearch #AdvStateCode').val();
        PendingProductionOrderReportViewModel.AdvDistrictCode = $('.divboxASearch #AdvDistrictCode').val();
        PendingProductionOrderReportViewModel.AdvCustomerCategoryCode = $('.divboxASearch #AdvCustomerCategoryCode').val();
        PendingProductionOrderReportViewModel.AdvReferencePersonCode = $('.divboxASearch #AdvReferencePersonCode').val();
        PendingProductionOrderReportViewModel.AdvApprovalStatusCode = $('.divboxASearch #AdvApprovalStatusCode').val();
        PendingProductionOrderReportViewModel.AdvDelFromDate = $('.divboxASearch #AdvDelFromDate').val();
        PendingProductionOrderReportViewModel.AdvDelToDate = $('.divboxASearch #AdvDelToDate').val();
        PendingProductionOrderReportViewModel.AdvReportType = $('.divboxASearch #AdvReportType').val();
        PendingProductionOrderReportViewModel.DateFilter = $('#DateFilter').val();
        PendingProductionOrderReportViewModel.AdvProduct = $('.divboxASearch #AdvProduct').val();
        PendingProductionOrderReportViewModel.AdvProductModel = $('.divboxASearch #AdvProductModel').val();
        PendingProductionOrderReportViewModel.AdvPlantCode = $('.divboxASearch #AdvPlantCode').val();


        //apply datatable plugin on Production Order standard report table
        _dataTable.productionOrderReportList = $("#tblPendingProductionOrderReport").DataTable(
        {
            dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
            ordering: false,
            searching: false,
            paging: true,
            lengthChange: false,
            autoWidth: false,
            scrollX: "500px",
            fixedHeader: true,
            //scrollCollapse: true,
            processing: true,
            language: {
                "processing": "<div class='spinner'><div class='bounce1'></div><div class='bounce2'></div><div class='bounce3'></div></div>"
            },
            serverSide: true,
            ajax: {
                url: "GetPendingProductionOrderReport/",
                data: { "pendingProductionOrderReportVM": PendingProductionOrderReportViewModel },
                type: 'POST'
            },
            pageLength: 8,
            autoWidth: false,
            columns: [

               { "data": "ProductionOrderNo", "defaultContent": "<i>-</i>" },
               { "data": "ProdOrderDateFormatted", "defaultContent": "<i>-</i>" },
               { "data": "Customer.CompanyName", "defaultContent": "<i>-</i>" },
               { "data": "Customer.ContactPerson", "defaultContent": "<i>-</i>" },
               { "data": "ExpectedDelvDateFormatted", "defaultContent": "<i>-</i>" },
               { "data": "ReferencePerson.Name", "defaultContent": "<i>-</i>" },
               { "data": "Area.Description", "defaultContent": "<i>-</i>" },
               { "data": "Plant.Description", "defaultContent": "<i>-</i>" },
               { "data": "Product.Name", "defaultContent": "<i>-</i>" },
               { "data": "ProductModel.Name", "defaultContent": "<i>-</i>" },
               { "data": "ProductSpec", "defaultContent": "<i>-</i>" },               
                {
                    "data": "Amount", render: function (data, type, row) {
                        return formatCurrency(row.Amount)
                    }, "defaultContent": "<i>-</i>"
                },
               { "data": "Qty", "defaultContent": "<i>-</i>" },
               { "data": "SaleOrdNo", "defaultContent": "<i>-</i>" },
               { "data": "SaleOrderQty", "defaultContent": "<i>-</i>" },
               { "data": "PendingQty", "defaultContent": "<i>-</i>" },
               { "data": "Progress", "defaultContent": "<i>-</i>" },
               { "data": "ForecastDateFormatted", "defaultContent": "<i>-</i>" },

            ],
            columnDefs: [{ className: "text-right", "targets": [11,12,14,15] },
                         { className: "text-left", "targets": [0, 2, 3, 5, 6, 7, 8, 10,, 13,16] },
                         { className: "text-center", "targets": [1,4,17] },
                           { "targets": [0], "width": "8%" },
                           { "targets": [1], "width": "5%" },
                           { "targets": [2], "width": "8%" },
                           { "targets": [3], "width": "5%" },
                           { "targets": [4], "width": "8%" },
                           { "targets": [5], "width": "5%" },
                           { "targets": [6], "width": "5%" },
                           { "targets": [7], "width": "5%" },
                           { "targets": [8], "width": "8%" },
                           { "targets": [9], "width": "8%" },
                           { "targets": [10], "width": "13%" },
                           { "targets": [12], "width": "5%" },
                           { "targets": [13], "width": "5%" },
                           { "targets": [14], "width": "6%" },
                           { "targets": [15], "width": "5%" },
                           { "targets": [16], "width": "6%" },
                           { "targets": [17], "width": "12%" },


            ],
            destroy: true,
            //for performing the import operation after the data loaded
            initComplete: function (settings, json) {
                debugger;
                $('.dataTables_wrapper div.bottom div').addClass('col-md-6');
                $('#divPendingProductionOrderReport').fadeIn('slow');
                if (action == undefined) {
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
    BindOrReloadPendingProductionOrderReportTable('Reset');
}
//function export data to excel
function ExportReportData() {
    debugger;
    //$('.excelExport').show();
    //OnServerCallBegin();       
    BindOrReloadPendingProductionOrderReportTable('Export');
}

function ApplyFilterThenSearch() {
    $(".searchicon").addClass('filterApplied');
    CloseAdvanceSearch();
    BindOrReloadPendingProductionOrderReportTable('Search');
}

function GoToList() {
    window.location.href = "/Report";
}


function DateFilterOnchange() {
    debugger;
    var selectedValue = $("#DateFilter").val().split("||");
    $('.divboxASearch #AdvFromDate').val(selectedValue[0]);
    $('.divboxASearch #AdvToDate').val(selectedValue[1]);
    BindOrReloadPendingProductionOrderReportTable('Search');
}