var _dataTable = {};
var _emptyGuid = "00000000-0000-0000-0000-000000000000";
var _datatablerowindex = -1;
var _jsonData = {};

//---------------------------------------Docuement Ready--------------------------------------------------//

$(document).ready(function () {
    debugger;
    try {

        BindOrReloadProductionOrderReportTable('Init');
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
//function bind the Production Order standard report list checking search and filter
function BindOrReloadProductionOrderReportTable(action) {
    try {
        debugger;
        //creating advancesearch object
        ProductionOrderReportViewModel = new Object();
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
                    //($('.divboxASearch #AdvPreparedBy').val() == "") &&
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
                ProductionOrderReportViewModel.DataTablePaging = DataTablePagingViewModel;

                ProductionOrderReportViewModel.SearchTerm = $('#SearchTerm').val() == "" ? null : $('#SearchTerm').val();
                ProductionOrderReportViewModel.AdvFromDate = $('.divboxASearch #AdvFromDate').val() == "" ? null : $('.divboxASearch #AdvFromDate').val();
                ProductionOrderReportViewModel.AdvFromDate = $('#AdvFromDate').val() == "" ? null : $('#AdvFromDate').val();
                ProductionOrderReportViewModel.AdvToDate = $('.divboxASearch #AdvToDate').val() == "" ? null : $('.divboxASearch #AdvToDate').val();
                ProductionOrderReportViewModel.AdvToDate = $('#AdvToDate').val() == "" ? null : $('#AdvToDate').val();

                ProductionOrderReportViewModel.AdvDocumentOwnerID = $('.divboxASearch #AdvDocumentOwnerID').val() == "" ? _emptyGuid : $('.divboxASearch #AdvDocumentOwnerID').val();
                ProductionOrderReportViewModel.AdvCustomer = $('.divboxASearch #AdvCustomer').val() == "" ? null : $('.divboxASearch #AdvCustomer').val();
                ProductionOrderReportViewModel.AdvAreaCode = $('.divboxASearch #AdvAreaCode').val() == "" ? null : $('.divboxASearch #AdvAreaCode').val();
                ProductionOrderReportViewModel.AdvBranchCode = $('.divboxASearch #AdvBranchCode').val() == "" ? null : $('.divboxASearch #AdvBranchCode').val();
                ProductionOrderReportViewModel.AdvDocumentStatusCode = $('.divboxASearch #AdvDocumentStatusCode').val() == "" ? null : $('.divboxASearch #AdvDocumentStatusCode').val();
                ProductionOrderReportViewModel.AdvPreparedBy = $('.divboxASearch #AdvPreparedBy').val() == "" ? _emptyGuid : $('.divboxASearch #AdvPreparedBy').val();

                ProductionOrderReportViewModel.AdvAmountFrom = $('.divboxASearch #AdvAmountFrom').val() == "" ? null : $('.divboxASearch #AdvAmountFrom').val();
                ProductionOrderReportViewModel.AdvAmountTo = $('.divboxASearch #AdvAmountTo').val() == "" ? null : $('.divboxASearch #AdvAmountTo').val();
                ProductionOrderReportViewModel.AdvCountryCode = $('.divboxASearch #AdvCountryCode').val() == "" ? null : $('.divboxASearch #AdvCountryCode').val();
                ProductionOrderReportViewModel.AdvStateCode = $('.divboxASearch #AdvStateCode').val() == "" ? null : $('.divboxASearch #AdvStateCode').val();
                ProductionOrderReportViewModel.AdvDistrictCode = $('.divboxASearch #AdvDistrictCode').val() == "" ? null : $('.divboxASearch #AdvDistrictCode').val();
                ProductionOrderReportViewModel.AdvCustomerCategoryCode = $('.divboxASearch #AdvCustomerCategoryCode').val() == "" ? null : $('.divboxASearch #AdvCustomerCategoryCode').val();
                ProductionOrderReportViewModel.AdvReferencePersonCode = $('.divboxASearch #AdvReferencePersonCode').val() == "" ? null : $('.divboxASearch #AdvReferencePersonCode').val();
                ProductionOrderReportViewModel.AdvApprovalStatusCode = $('.divboxASearch #AdvApprovalStatusCode').val() == "" ? null : $('.divboxASearch #AdvApprovalStatusCode').val();
                ProductionOrderReportViewModel.AdvDelFromDate = $('.divboxASearch #AdvDelFromDate').val() == "" ? null : $('.divboxASearch #AdvDelFromDate').val();
                ProductionOrderReportViewModel.AdvDelToDate = $('.divboxASearch #AdvDelToDate').val() == "" ? null : $('.divboxASearch #AdvDelToDate').val();
                ProductionOrderReportViewModel.AdvReportType = $('.divboxASearch #AdvReportType').val() == "" ? null : $('.divboxASearch #AdvReportType').val();
                ProductionOrderReportViewModel.DateFilter = $('#DateFilter').val() == "" ? null : $('#DateFilter').val();
                ProductionOrderReportViewModel.AdvProduct = $('.divboxASearch #AdvProduct').val() == "" ? _emptyGuid : $('.divboxASearch #AdvProduct').val();
                ProductionOrderReportViewModel.AdvProductModel = $('.divboxASearch #AdvProductModel').val() == "" ? _emptyGuid : $('.divboxASearch #AdvProductModel').val();
                ProductionOrderReportViewModel.AdvPlantCode = $('.divboxASearch #AdvPlantCode').val() == "" ? null : $('.divboxASearch #AdvPlantCode').val();

                
                $('#AdvanceSearch').val(JSON.stringify(ProductionOrderReportViewModel));
                $('#FormExcelExport').submit();
                return true;
                break;
            default:
                break;
        }
        ProductionOrderReportViewModel.DataTablePaging = DataTablePagingViewModel;
        ProductionOrderReportViewModel.SearchTerm = $('#SearchTerm').val();
        ProductionOrderReportViewModel.AdvFromDate = $('.divboxASearch #AdvFromDate').val();
        ProductionOrderReportViewModel.AdvToDate = $('.divboxASearch #AdvToDate').val();
        ProductionOrderReportViewModel.AdvDocumentOwnerID = $('.divboxASearch #AdvDocumentOwnerID').val();
        ProductionOrderReportViewModel.AdvCustomer = $('.divboxASearch #AdvCustomer').val();
        ProductionOrderReportViewModel.AdvAreaCode = $('.divboxASearch #AdvAreaCode').val();
        ProductionOrderReportViewModel.AdvBranchCode = $('.divboxASearch #AdvBranchCode').val();
        ProductionOrderReportViewModel.AdvDocumentStatusCode = $('.divboxASearch #AdvDocumentStatusCode').val();
        ProductionOrderReportViewModel.AdvPreparedBy = $('.divboxASearch #AdvPreparedBy').val();
        ProductionOrderReportViewModel.AdvAmountFrom = $('.divboxASearch #AdvAmountFrom').val();
        ProductionOrderReportViewModel.AdvAmountTo = $('.divboxASearch #AdvAmountTo').val();
        ProductionOrderReportViewModel.AdvCountryCode = $('.divboxASearch #AdvCountryCode').val();
        ProductionOrderReportViewModel.AdvStateCode = $('.divboxASearch #AdvStateCode').val();
        ProductionOrderReportViewModel.AdvDistrictCode = $('.divboxASearch #AdvDistrictCode').val();
        ProductionOrderReportViewModel.AdvCustomerCategoryCode = $('.divboxASearch #AdvCustomerCategoryCode').val();
        ProductionOrderReportViewModel.AdvReferencePersonCode = $('.divboxASearch #AdvReferencePersonCode').val();
        ProductionOrderReportViewModel.AdvApprovalStatusCode = $('.divboxASearch #AdvApprovalStatusCode').val();
        ProductionOrderReportViewModel.AdvDelFromDate = $('.divboxASearch #AdvDelFromDate').val();
        ProductionOrderReportViewModel.AdvDelToDate = $('.divboxASearch #AdvDelToDate').val();
        ProductionOrderReportViewModel.AdvReportType = $('.divboxASearch #AdvReportType').val();
        ProductionOrderReportViewModel.DateFilter = $('#DateFilter').val();
        ProductionOrderReportViewModel.AdvProduct = $('.divboxASearch #AdvProduct').val();
        ProductionOrderReportViewModel.AdvProductModel = $('.divboxASearch #AdvProductModel').val();
        ProductionOrderReportViewModel.AdvPlantCode = $('.divboxASearch #AdvPlantCode').val();


        //apply datatable plugin on Production Order standard report table
        _dataTable.productionOrderReportList = $("#tblProductionOrderReport").DataTable(
        {
            dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
            ordering: false,
            searching: false,
            paging: true,
            lengthChange: false,
            autoWidth: false,
            scrollX: "500px",
            fixedHeader: true,
            processing: true,
            language: {
                "processing": "<div class='spinner'><div class='bounce1'></div><div class='bounce2'></div><div class='bounce3'></div></div>"
            },
            serverSide: true,
            ajax: {
                url: "GetProductionOrderStandardReport/",
                data: { "productionOrderReportVM": ProductionOrderReportViewModel },
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
                 { "data": "Qty", "defaultContent": "<i>-</i>" },
                  {
                      "data": "Amount", render: function (data, type, row) {
                          return formatCurrency(row.Amount)
                      }, "defaultContent": "<i>-</i>"
                  },
               { "data": "SaleOrdNo", "defaultContent": "<i>-</i>" },
             
               { "data": "DocumentStatus.Description", "defaultContent": "<i>-</i>" },
               { "data": "Remarks", "defaultContent": "<i>-</i>" },


            ],
            columnDefs: [{ className: "text-right", "targets": [11,12] },
                         { className: "text-left", "targets": [0, 2, 3, 5, 6,7, 8, 10,13,14,15] },
                         { className: "text-center", "targets": [1,4] },
                           { "targets": [0], "width": "8%" },
                           { "targets": [1], "width": "5%" },
                           { "targets": [2], "width": "8%" },
                           { "targets": [3], "width": "8%" },
                           { "targets": [4], "width": "8%" },
                           { "targets": [5], "width": "5%" },
                           { "targets": [6], "width": "8%" },
                           { "targets": [7], "width": "8%" },
                           { "targets": [8], "width": "8%" },
                           { "targets": [9], "width": "8%" },
                           { "targets": [10], "width": "15%" },
                           { "targets": [11], "width": "3%" },
                           { "targets": [12], "width": "5%" },
                           { "targets": [13], "width": "8%" },
                           { "targets": [14], "width": "10%" },
                            { "targets": [15], "width": "10%" },


            ],
            destroy: true,
            //for performing the import operation after the data loaded
            initComplete: function (settings, json) {
                debugger;
                $('.dataTables_wrapper div.bottom div').addClass('col-md-6');
                $('#tblProductionOrderReport').fadeIn('slow');
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
    BindOrReloadProductionOrderReportTable('Reset');
}
//function export data to excel
function ExportReportData() {
    debugger;
    //$('.excelExport').show();
    //OnServerCallBegin();       
    BindOrReloadProductionOrderReportTable('Export');
}

function ApplyFilterThenSearch() {
    $(".searchicon").addClass('filterApplied');
    CloseAdvanceSearch();
    BindOrReloadProductionOrderReportTable('Search');
}

function GoToList() {
    window.location.href = "/Report";
}


function DateFilterOnchange() {
    debugger;
    var selectedValue = $("#DateFilter").val().split("||");
    $('.divboxASearch #AdvFromDate').val(selectedValue[0]);
    $('.divboxASearch #AdvToDate').val(selectedValue[1]);
    BindOrReloadProductionOrderReportTable('Search');
}