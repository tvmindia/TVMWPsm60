var _dataTable = {};
var _emptyGuid = "00000000-0000-0000-0000-000000000000";
var _datatablerowindex = -1;
var _jsonData = {};

//---------------------------------------Docuement Ready--------------------------------------------------//

$(document).ready(function () {
    debugger;
    try {

        BindOrReloadPendingProductionQCReportTable('Init');
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
//function bind the  Production QC  report list checking search and filter
function BindOrReloadPendingProductionQCReportTable(action) {
    try {
        debugger;
        //creating advancesearch object
        PendingProductionQCReportViewModel = new Object();
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
                $('.divboxASearch #AdvCountryCode').val('').trigger('change');
                $('.divboxASearch #AdvStateCode').val('').trigger('change');
                $('.divboxASearch #AdvDistrictCode').val('').trigger('change');
                $('.divboxASearch #AdvCustomerCategoryCode').val('').trigger('change');
                $('.divboxASearch #AdvReferencePersonCode').val('').trigger('change');
                $('.divboxASearch #AdvReportType').val('').trigger('change');
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
                $('.divboxASearch #AdvAmountFrom').val('');
                $('.divboxASearch #AdvAmountTo').val('');
                $('.divboxASearch #AdvCountryCode').val('');
                $('.divboxASearch #AdvStateCode').val('');
                $('.divboxASearch #AdvDistrictCode').val('');
                $('.divboxASearch #AdvCustomerCategoryCode').val('');
                $('.divboxASearch #AdvReferencePersonCode').val('');

                $('.divboxASearch #AdvReportType').val('');
                $('#DateFilter').val('');
                $('.divboxASearch #AdvProduct').val('');
                $('.divboxASearch #AdvProductModel').val('');
                $('.divboxASearch #AdvPlantCode').val('');

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
                    ($('.divboxASearch #AdvReferencePersonCode').val() == "")

                    && ($('.divboxASearch #AdvReportType').val() == "")
                    && ($('.divboxASearch #AdvProduct').val() == "")
                    && ($('.divboxASearch #AdvProductModel').val() == "")
                    && ($('.divboxASearch #AdvPlantCode').val('') == "")

                    ) {
                    return true;
                }
                break;
            case 'Export':
                DataTablePagingViewModel.Length = -1;
                PendingProductionQCReportViewModel.DataTablePaging = DataTablePagingViewModel;

                PendingProductionQCReportViewModel.SearchTerm = $('#SearchTerm').val() == "" ? null : $('#SearchTerm').val();
                PendingProductionQCReportViewModel.AdvFromDate = $('.divboxASearch #AdvFromDate').val() == "" ? null : $('.divboxASearch #AdvFromDate').val();
                PendingProductionQCReportViewModel.AdvFromDate = $('#AdvFromDate').val() == "" ? null : $('#AdvFromDate').val();
                PendingProductionQCReportViewModel.AdvToDate = $('.divboxASearch #AdvToDate').val() == "" ? null : $('.divboxASearch #AdvToDate').val();
                PendingProductionQCReportViewModel.AdvToDate = $('#AdvToDate').val() == "" ? null : $('#AdvToDate').val();

                PendingProductionQCReportViewModel.AdvDocumentOwnerID = $('.divboxASearch #AdvDocumentOwnerID').val() == "" ? _emptyGuid : $('.divboxASearch #AdvDocumentOwnerID').val();
                PendingProductionQCReportViewModel.AdvCustomer = $('.divboxASearch #AdvCustomer').val() == "" ? null : $('.divboxASearch #AdvCustomer').val();
                PendingProductionQCReportViewModel.AdvAreaCode = $('.divboxASearch #AdvAreaCode').val() == "" ? null : $('.divboxASearch #AdvAreaCode').val();
                PendingProductionQCReportViewModel.AdvBranchCode = $('.divboxASearch #AdvBranchCode').val() == "" ? null : $('.divboxASearch #AdvBranchCode').val();
                PendingProductionQCReportViewModel.AdvDocumentStatusCode = $('.divboxASearch #AdvDocumentStatusCode').val() == "" ? null : $('.divboxASearch #AdvDocumentStatusCode').val();

                PendingProductionQCReportViewModel.AdvAmountFrom = $('.divboxASearch #AdvAmountFrom').val() == "" ? null : $('.divboxASearch #AdvAmountFrom').val();
                PendingProductionQCReportViewModel.AdvAmountTo = $('.divboxASearch #AdvAmountTo').val() == "" ? null : $('.divboxASearch #AdvAmountTo').val();
                PendingProductionQCReportViewModel.AdvCountryCode = $('.divboxASearch #AdvCountryCode').val() == "" ? null : $('.divboxASearch #AdvCountryCode').val();
                PendingProductionQCReportViewModel.AdvStateCode = $('.divboxASearch #AdvStateCode').val() == "" ? null : $('.divboxASearch #AdvStateCode').val();
                PendingProductionQCReportViewModel.AdvDistrictCode = $('.divboxASearch #AdvDistrictCode').val() == "" ? null : $('.divboxASearch #AdvDistrictCode').val();
                PendingProductionQCReportViewModel.AdvCustomerCategoryCode = $('.divboxASearch #AdvCustomerCategoryCode').val() == "" ? null : $('.divboxASearch #AdvCustomerCategoryCode').val();
                PendingProductionQCReportViewModel.AdvReferencePersonCode = $('.divboxASearch #AdvReferencePersonCode').val() == "" ? null : $('.divboxASearch #AdvReferencePersonCode').val();

                PendingProductionQCReportViewModel.AdvReportType = $('.divboxASearch #AdvReportType').val() == "" ? null : $('.divboxASearch #AdvReportType').val();
                PendingProductionQCReportViewModel.DateFilter = $('#DateFilter').val() == "" ? null : $('#DateFilter').val();
                PendingProductionQCReportViewModel.AdvProduct = $('.divboxASearch #AdvProduct').val() == "" ? _emptyGuid : $('.divboxASearch #AdvProduct').val();
                PendingProductionQCReportViewModel.AdvProductModel = $('.divboxASearch #AdvProductModel').val() == "" ? _emptyGuid : $('.divboxASearch #AdvProductModel').val();
                PendingProductionQCReportViewModel.AdvPlantCode = $('.divboxASearch #AdvPlantCode').val() == "" ? null : $('.divboxASearch #AdvPlantCode').val();


                $('#AdvanceSearch').val(JSON.stringify(PendingProductionQCReportViewModel));
                $('#FormExcelExport').submit();
                return true;
                break;
            default:
                break;
        }
        PendingProductionQCReportViewModel.DataTablePaging = DataTablePagingViewModel;
        PendingProductionQCReportViewModel.SearchTerm = $('#SearchTerm').val();
        PendingProductionQCReportViewModel.AdvFromDate = $('.divboxASearch #AdvFromDate').val();
        PendingProductionQCReportViewModel.AdvToDate = $('.divboxASearch #AdvToDate').val();
        PendingProductionQCReportViewModel.AdvDocumentOwnerID = $('.divboxASearch #AdvDocumentOwnerID').val();
        PendingProductionQCReportViewModel.AdvCustomer = $('.divboxASearch #AdvCustomer').val();
        PendingProductionQCReportViewModel.AdvAreaCode = $('.divboxASearch #AdvAreaCode').val();
        PendingProductionQCReportViewModel.AdvBranchCode = $('.divboxASearch #AdvBranchCode').val();
        PendingProductionQCReportViewModel.AdvDocumentStatusCode = $('.divboxASearch #AdvDocumentStatusCode').val();
        PendingProductionQCReportViewModel.AdvAmountFrom = $('.divboxASearch #AdvAmountFrom').val();
        PendingProductionQCReportViewModel.AdvAmountTo = $('.divboxASearch #AdvAmountTo').val();
        PendingProductionQCReportViewModel.AdvCountryCode = $('.divboxASearch #AdvCountryCode').val();
        PendingProductionQCReportViewModel.AdvStateCode = $('.divboxASearch #AdvStateCode').val();
        PendingProductionQCReportViewModel.AdvDistrictCode = $('.divboxASearch #AdvDistrictCode').val();
        PendingProductionQCReportViewModel.AdvCustomerCategoryCode = $('.divboxASearch #AdvCustomerCategoryCode').val();
        PendingProductionQCReportViewModel.AdvReferencePersonCode = $('.divboxASearch #AdvReferencePersonCode').val();

        PendingProductionQCReportViewModel.AdvReportType = $('.divboxASearch #AdvReportType').val();
        PendingProductionQCReportViewModel.DateFilter = $('#DateFilter').val();
        PendingProductionQCReportViewModel.AdvProduct = $('.divboxASearch #AdvProduct').val();
        PendingProductionQCReportViewModel.AdvProductModel = $('.divboxASearch #AdvProductModel').val();
        PendingProductionQCReportViewModel.AdvPlantCode = $('.divboxASearch #AdvPlantCode').val();


        //apply datatable plugin on Production Order standard report table
        _dataTable.pendingProductionQCReportList = $("#tblPendingProductionQCReport").DataTable(
        {
            dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
            ordering: false,
            searching: false,
            paging: true,
            lengthChange: false,
            autoWidth: false,         
            processing: true,
            language: {
                "processing": "<div class='spinner'><div class='bounce1'></div><div class='bounce2'></div><div class='bounce3'></div></div>"
            },
            serverSide: true,
            ajax: {
                url: "GetPendingProductionQCReport/",
                data: { "pendingProductionQCReportVM": PendingProductionQCReportViewModel },
                type: 'POST'
            },
            pageLength: 8,        
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
               { "data": "Amount", "defaultContent": "<i>-</i>" },
               { "data": "ProdOrdQty", "defaultContent": "<i>-</i>" },
               { "data": "ProductionQCNo", "defaultContent": "<i>-</i>" },
               { "data": "ProdQCQty", "defaultContent": "<i>-</i>" },
               { "data": "PendingQty", "defaultContent": "<i>-</i>" },
               { "data": "DocumentStatus.Description", "defaultContent": "<i>-</i>" },
               { "data": "Remarks", "defaultContent": "<i>-</i>" },


            ],
            columnDefs: [{ className: "text-right", "targets": [11] },
                         { className: "text-left", "targets": [0, 1, 2, 3, 4, 5, 6, 7, 8, 10, 12, 13, 14, 15, 16,17] },
                         { className: "text-center", "targets": [] },
                           { "targets": [0], "width": "12%" },
                           { "targets": [1], "width": "12%" },
                           { "targets": [2], "width": "12%" },
                           { "targets": [3], "width": "10%" },
                           { "targets": [4], "width": "10%" },
                           { "targets": [5], "width": "10%" },
                           { "targets": [6], "width": "10%" },
                           { "targets": [7], "width": "9%" },
                           { "targets": [8], "width": "10%" },
                           { "targets": [9], "width": "10%" },
                           { "targets": [10], "width": "14%" },
                           { "targets": [11], "width": "9%" },
                           { "targets": [12], "width": "9%" },
                           { "targets": [13], "width": "9%" },
                           { "targets": [14], "width": "9%" },
                           { "targets": [15], "width": "9%" },
                           { "targets": [16], "width": "9%" },
                           { "targets": [17], "width": "12%" },


            ],
            destroy: true,
            //for performing the import operation after the data loaded
            initComplete: function (settings, json) {
                debugger;
                $('.dataTables_wrapper div.bottom div').addClass('col-md-6');
                $('#tblPendingProductionQCReport').fadeIn('slow');
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
    BindOrReloadPendingProductionQCReportTable('Reset');
}
//function export data to excel
function ExportReportData() {
    debugger;
    //$('.excelExport').show();
    //OnServerCallBegin();       
    BindOrReloadPendingProductionQCReportTable('Export');
}

function ApplyFilterThenSearch() {
    $(".searchicon").addClass('filterApplied');
    CloseAdvanceSearch();
    BindOrReloadPendingProductionQCReportTable('Search');
}

function GoToList() {
    window.location.href = "/Report";
}


function DateFilterOnchange() {
    debugger;
    var selectedValue = $("#DateFilter").val().split("||");
    $('.divboxASearch #AdvFromDate').val(selectedValue[0]);
    $('.divboxASearch #AdvToDate').val(selectedValue[1]);
    BindOrReloadPendingProductionQCReportTable('Search');
}