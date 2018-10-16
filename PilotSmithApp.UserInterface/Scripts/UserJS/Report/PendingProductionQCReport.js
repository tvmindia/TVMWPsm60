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
                $('.divboxASearch #AdvDocumentStatusCode').val('').trigger('change');
                $('.divboxASearch #AdvDocumentOwnerID').val('').trigger('change');
                $('.divboxASearch #AdvCountryCode').val('').trigger('change');
                $('.divboxASearch #AdvStateCode').val('').trigger('change');
                $('.divboxASearch #AdvDistrictCode').val('').trigger('change');
                $('.divboxASearch #AdvCustomerCategoryCode').val('').trigger('change');
                $('.divboxASearch #AdvReferencePersonCode').val('').trigger('change');
               // $('.divboxASearch #AdvReportType').val('').trigger('change');
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
                $('.divboxASearch #AdvDocumentStatusCode').val('');
                $('.divboxASearch #AdvDocumentOwnerID').val('');
                $('.divboxASearch #AdvAmountFrom').val('');
                $('.divboxASearch #AdvAmountTo').val('');
                $('.divboxASearch #AdvCountryCode').val('');
                $('.divboxASearch #AdvStateCode').val('');
                $('.divboxASearch #AdvDistrictCode').val('');
                $('.divboxASearch #AdvCustomerCategoryCode').val('');
                $('.divboxASearch #AdvReferencePersonCode').val('');

               // $('.divboxASearch #AdvReportType').val('');
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
                    ($('.divboxASearch #AdvDocumentStatusCode').val() == "") &&
                    //($('.divboxASearch #AdvPreparedBy').val() == "") &&
                    ($('.divboxASearch #AdvAmountFrom').val() == "") &&
                    ($('.divboxASearch #AdvAmountTo').val() == "") &&
                     ($('.divboxASearch #AdvCountryCode').val() == "") &&
                    ($('.divboxASearch #AdvStateCode').val() == "") &&
                    ($('.divboxASearch #AdvDistrictCode').val() == "") &&
                    ($('.divboxASearch #AdvCustomerCategoryCode').val() == "") &&
                    ($('.divboxASearch #AdvReferencePersonCode').val() == "")

                   // && ($('.divboxASearch #AdvReportType').val() == "")
                    && ($('.divboxASearch #AdvProduct').val() == "")
                    && ($('.divboxASearch #AdvProductModel').val() == "")
                    && ($('.divboxASearch #AdvPlantCode').val() == "")

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


        //apply datatable plugin on Pending Production QC report table
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
                {
                    "data": "ProductionOrderNo", render: function (data, type, row) {
                        return "<img src='../Content/images/datePicker.png' height='10px'>" + "&nbsp;" + row.ProdOrderDateFormatted + "</br>" + row.ProductionOrderNo;
                    }, "defaultContent": "<i>-</i>"
                },
               {
                   "data": "Customer.CompanyName", render: function (data, type, row) {
                       return "<img src='../Content/images/contact.png' height='10px'>" + "&nbsp;" + (row.Customer.ContactPerson == null ? "" : row.Customer.ContactPerson) + "</br>" + "<img src='../Content/images/organisation.png' height='10px'>" + "&nbsp;" + data;
                   }, "defaultContent": "<i>-</i>"
               },
               { "data": "ExpectedDelvDateFormatted", "defaultContent": "<i>-</i>" },//, "width": "50px" }4,
               {
                   "data": "Product.Name", render: function (data, type, row) {
                       return data + "</br>" + row.ProductModel.Name;
                   }, "defaultContent": "<i>-</i>"
               },
               {
                   "data": "ProductSpec", render: function (data, type, row) {
                       return '<div class="show-popover" data-html="true" data-toggle="popover" data-content="<p align=left>' + (data === null ? "-" : data.replace(/"/g, '”')) + '</p>"/>' + (data == null ? " " : data.substring(0, 50) + (data.length > 50 ? '...' : ''))

                   }, "defaultContent": "<i>-</i>"
               },
               { "data": "ProdOrdQty", "defaultContent": "<i>-</i>" },//, "width": "50px" }12,
               { "data": "ProdQCQty", "defaultContent": "<i>-</i>" },//, "width": "50px" }14,
               { "data": "PendingQty", "defaultContent": "<i>-</i>" },//, "width": "50px" }15,
               { "data": "ProductionQCNo", "defaultContent": "<i>-</i>" },//, "width": "50px" }13,
               { "data": "Plant.Description", "defaultContent": "<i>-</i>" },//, "width": "50px" }7,
               { "data": "Area.Description", "defaultContent": "<i>-</i>" },//, "width": "50px" }6,
               { "data": "ReferencePerson.Name", "defaultContent": "<i>-</i>"},//, "width": "50px" }5,
               { "data": "DocumentStatus.Description", "defaultContent": "<i>-</i>" },//, "width": "50px" }1,
                {
                    "data": "Amount", render: function (data, type, row) {
                        return formatCurrency(row.Amount)
                    }, "defaultContent": "<i>-</i>"
                },//, "width": "50px" }11,
               { "data": "Branch.Description", "defaultContent": "<i>-</i>" },
               { "data": "PSAUser.LoginName", "defaultContent": "<i>-</i>" },
               {
                   "data": "Remarks", render: function (data, type, row) {
                       return '<div class="show-popover" data-html="true" data-toggle="popover" data-content="<p align=left>' + (data === null ? "-" : data.replace(/"/g, '”')) + '</p>"/>' + (data == null ? " " : data.substring(0, 50) + (data.length > 50 ? '...' : ''))
                   }, "defaultContent": "<i>-</i>"
               },//, "width": "50px" }17,


            ],
            columnDefs: [{ className: "text-right", "targets": [13] },
                         { className: "text-left", "targets": [1,3,4, 8, 10,11,13] },
                         { className: "text-center", "targets": [0, 2,5, 6, 7,12] },
                           { "targets": [0], "width": "8%" },
                           { "targets": [1], "width": "7%" },
                           { "targets": [2], "width": "5%" },
                           { "targets": [3], "width": "6%" },
                           { "targets": [4], "width": "11%" },
                           { "targets": [5], "width": "5%" },
                           { "targets": [6], "width": "5%" },
                           { "targets": [7], "width": "5%" },
                           { "targets": [8], "width": "6%" },
                           { "targets": [9], "width": "5%" },
                           { "targets": [10], "width": "5%" },
                           { "targets": [11], "width": "5%" },
                           { "targets": [12], "width": "5%" },
                           { "targets": [13], "width": "5%" },
                           { "targets": [14], "width": "5%" },
                           { "targets": [15], "width": "5%" },
                           { "targets": [16], "width": "7%" }


            ],
            destroy: true,
            rowCallback: function (row, data) {
                setTimeout(function () {
                    $('[data-toggle="popover"]').popover({
                        html: true,
                        'trigger': 'hover',
                        'placement': 'top'
                    });
                }, 500);
            },
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