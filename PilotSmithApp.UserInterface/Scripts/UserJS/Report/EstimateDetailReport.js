var _dataTable = {};
var _emptyGuid = "00000000-0000-0000-0000-000000000000";
var _datatablerowindex = -1;
var _jsonData = {};

//---------------------------------------Docuement Ready--------------------------------------------------//

$(document).ready(function () {
    debugger;
    try {

        BindOrReloadEstimateDetailReportTable('Init');
        $(' #AdvDocumentStatusCode,#AdvPreparedBy,#AdvCustomer,#AdvProductModel,#AdvProduct').select2({
            dropdownParent: $(".divboxASearch")
        });

        $('.select2').addClass('form-control newinput');

        $('.select2').addClass('form-control newinput');



    }
    catch (e) {
        console.log(e.message);
    }
});
//function bind the SaleOrder standard report list checking search and filter
function BindOrReloadEstimateDetailReportTable(action) {
    try {
        debugger;
        //creating advancesearch object
        EstimateDetailReportViewModel = new Object();
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
                $('.divboxASearch #AdvValidFromDate').val('');
                $('.divboxASearch #AdvValidToDate').val('');
                $('.divboxASearch #AdvTotalCostRateFrom').val('');
                $('.divboxASearch #AdvTotalCostRateTo').val('');
                $('.divboxASearch #AdvTotalSellingRateFrom').val('');
                $('.divboxASearch #AdvTotalSellingRateTo').val('');
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
                $('.divboxASearch #AdvReportType').val('').trigger('change');
                $('.divboxASearch #AdvProduct').val('').trigger('change');
                $('.divboxASearch #AdvProductModel').val('').trigger('change');

                break;
            case 'Init':
                $('#SearchTerm').val('');
                $('.divboxASearch #AdvFromDate').val('');
                $('.divboxASearch #AdvToDate').val('');
                $('.divboxASearch #AdvValidFromDate').val('');
                $('.divboxASearch #AdvValidToDate').val('');
                $('.divboxASearch #AdvAreaCode').val('');
                $('.divboxASearch #AdvCustomer').val('');
                $('.divboxASearch #AdvBranchCode').val('');
                $('.divboxASearch #AdvDocumentStatusCode').val('');
                $('.divboxASearch #AdvDocumentOwnerID').val('');
                $('.divboxASearch #AdvPreparedBy').val('');
                $('.divboxASearch #AdvTotalCostRateFrom').val('');
                $('.divboxASearch #AdvTotalCostRateTo').val('');
                $('.divboxASearch #AdvTotalSellingRateFrom').val('');
                $('.divboxASearch #AdvTotalSellingRateTo').val('');
                $('.divboxASearch #AdvCountryCode').val('');
                $('.divboxASearch #AdvStateCode').val('');
                $('.divboxASearch #AdvDistrictCode').val('');
                $('.divboxASearch #AdvCustomerCategoryCode').val('');
                $('.divboxASearch #AdvReportType').val('');
                $('.divboxASearch #AdvProduct').val('');
                $('.divboxASearch #AdvProductModel').val('');
                break;
            case 'Search':
                if ((SearchTerm == SearchValue) && ($('.divboxASearch #AdvFromDate').val() == "")
                    && ($('.divboxASearch #AdvToDate').val() == "") &&
                    ($('.divboxASearch #AdvValidFromDate').val() == "") &&
                    ($('.divboxASearch #AdvValidToDate').val() == "") &&
                    ($('.divboxASearch #AdvDocumentOwnerID').val() == "") &&
                    ($('.divboxASearch #AdvCustomer').val() == "") &&
                    ($('.divboxASearch #AdvAreaCode').val() == "") &&
                    ($('.divboxASearch #AdvBranchCode').val() == "") &&
                    ($('.divboxASearch #AdvDocumentStatusCode').val() == "") &&
                    ($('.divboxASearch #AdvPreparedBy').val() == "") &&
                    ($('.divboxASearch #AdvTotalCostRateFrom').val() == "") &&
                    ($('.divboxASearch #AdvTotalCostRateTo').val() == "") &&
                     ($('.divboxASearch #AdvCountryCode').val() == "") &&
                    ($('.divboxASearch #AdvStateCode').val() == "") &&
                    ($('.divboxASearch #AdvDistrictCode').val() == "") &&
                    ($('.divboxASearch #AdvCustomerCategoryCode').val() == "") &&
                    ($('.divboxASearch #AdvProduct').val() == "")
                    && ($('.divboxASearch #AdvProductModel').val() == "")
                    && ($('.divboxASearch #AdvTotalSellingRateFrom').val() == "")
                    && ($('.divboxASearch #AdvTotalSellingRateTo').val() == "")
                    ) {
                    return true;
                }
                break;
            case 'Export':
                DataTablePagingViewModel.Length = -1;
                EstimateDetailReportViewModel.DataTablePaging = DataTablePagingViewModel;

                EstimateDetailReportViewModel.SearchTerm = $('#SearchTerm').val() == "" ? null : $('#SearchTerm').val();
                EstimateDetailReportViewModel.AdvFromDate = $('.divboxASearch #AdvFromDate').val() == "" ? null : $('.divboxASearch #AdvFromDate').val();
                EstimateDetailReportViewModel.AdvFromDate = $('#AdvFromDate').val() == "" ? null : $('#AdvFromDate').val();
                EstimateDetailReportViewModel.AdvToDate = $('.divboxASearch #AdvToDate').val() == "" ? null : $('.divboxASearch #AdvToDate').val();
                EstimateDetailReportViewModel.AdvToDate = $('#AdvToDate').val() == "" ? null : $('#AdvToDate').val();

                EstimateDetailReportViewModel.AdvValidFromDate = $('.divboxASearch #AdvValidFromDate').val() == "" ? null : $('.divboxASearch #AdvValidFromDate').val();
                EstimateDetailReportViewModel.AdvValidFromDate = $('#AdvValidFromDate').val() == "" ? null : $('#AdvValidFromDate').val();
                EstimateDetailReportViewModel.AdvValidToDate = $('.divboxASearch #AdvValidToDate').val() == "" ? null : $('.divboxASearch #AdvValidToDate').val();
                EstimateDetailReportViewModel.AdvValidToDate = $('#AdvValidToDate').val() == "" ? null : $('#AdvValidToDate').val();

                EstimateDetailReportViewModel.AdvDocumentOwnerID = $('.divboxASearch #AdvDocumentOwnerID').val() == "" ? _emptyGuid : $('.divboxASearch #AdvDocumentOwnerID').val();
                EstimateDetailReportViewModel.AdvCustomer = $('.divboxASearch #AdvCustomer').val() == "" ? null : $('.divboxASearch #AdvCustomer').val();
                EstimateDetailReportViewModel.AdvAreaCode = $('.divboxASearch #AdvAreaCode').val() == "" ? null : $('.divboxASearch #AdvAreaCode').val();
                EstimateDetailReportViewModel.AdvBranchCode = $('.divboxASearch #AdvBranchCode').val() == "" ? null : $('.divboxASearch #AdvBranchCode').val();
                EstimateDetailReportViewModel.AdvDocumentStatusCode = $('.divboxASearch #AdvDocumentStatusCode').val() == "" ? null : $('.divboxASearch #AdvDocumentStatusCode').val();
                EstimateDetailReportViewModel.AdvPreparedBy = $('.divboxASearch #AdvPreparedBy').val() == "" ? _emptyGuid : $('.divboxASearch #AdvPreparedBy').val();

                EstimateDetailReportViewModel.AdvTotalCostRateFrom = $('.divboxASearch #AdvTotalCostRateFrom').val() == "" ? null : $('.divboxASearch #AdvTotalCostRateFrom').val();
                EstimateDetailReportViewModel.AdvTotalCostRateTo = $('.divboxASearch #AdvTotalCostRateTo').val() == "" ? null : $('.divboxASearch #AdvTotalCostRateTo').val();
                EstimateDetailReportViewModel.AdvTotalSellingRateFrom = $('.divboxASearch #AdvTotalSellingRateFrom').val() == "" ? null : $('.divboxASearch #AdvTotalSellingRateFrom').val();
                EstimateDetailReportViewModel.AdvTotalSellingRateTo = $('.divboxASearch #AdvTotalSellingRateTo').val() == "" ? null : $('.divboxASearch #AdvTotalSellingRateTo').val();
                EstimateDetailReportViewModel.AdvCountryCode = $('.divboxASearch #AdvCountryCode').val() == "" ? null : $('.divboxASearch #AdvCountryCode').val();
                EstimateDetailReportViewModel.AdvStateCode = $('.divboxASearch #AdvStateCode').val() == "" ? null : $('.divboxASearch #AdvStateCode').val();
                EstimateDetailReportViewModel.AdvDistrictCode = $('.divboxASearch #AdvDistrictCode').val() == "" ? null : $('.divboxASearch #AdvDistrictCode').val();
                EstimateDetailReportViewModel.AdvCustomerCategoryCode = $('.divboxASearch #AdvCustomerCategoryCode').val() == "" ? null : $('.divboxASearch #AdvCustomerCategoryCode').val();
                EstimateDetailReportViewModel.AdvReportType = $('.divboxASearch #AdvReportType').val() == "" ? null : $('.divboxASearch #AdvReportType').val();
                EstimateDetailReportViewModel.AdvProduct = $('.divboxASearch #AdvProduct').val() == "" ? _emptyGuid : $('.divboxASearch #AdvProduct').val();
                EstimateDetailReportViewModel.AdvProductModel = $('.divboxASearch #AdvProductModel').val() == "" ? _emptyGuid : $('.divboxASearch #AdvProductModel').val();

                $('#AdvanceSearch').val(JSON.stringify(EstimateDetailReportViewModel));
                $('#FormExcelExport').submit();
                return true;
                break;
            default:
                break;
        }
        EstimateDetailReportViewModel.DataTablePaging = DataTablePagingViewModel;
        EstimateDetailReportViewModel.SearchTerm = $('#SearchTerm').val();
        EstimateDetailReportViewModel.AdvFromDate = $('.divboxASearch #AdvFromDate').val();
        EstimateDetailReportViewModel.AdvToDate = $('.divboxASearch #AdvToDate').val();
        EstimateDetailReportViewModel.AdvValidFromDate = $('.divboxASearch #AdvValidFromDate').val();
        EstimateDetailReportViewModel.AdvValidToDate = $('.divboxASearch #AdvValidToDate').val();
        EstimateDetailReportViewModel.AdvDocumentOwnerID = $('.divboxASearch #AdvDocumentOwnerID').val();
        EstimateDetailReportViewModel.AdvCustomer = $('.divboxASearch #AdvCustomer').val();
        EstimateDetailReportViewModel.AdvAreaCode = $('.divboxASearch #AdvAreaCode').val();
        EstimateDetailReportViewModel.AdvBranchCode = $('.divboxASearch #AdvBranchCode').val();
        EstimateDetailReportViewModel.AdvDocumentStatusCode = $('.divboxASearch #AdvDocumentStatusCode').val();
        EstimateDetailReportViewModel.AdvPreparedBy = $('.divboxASearch #AdvPreparedBy').val();
        EstimateDetailReportViewModel.AdvTotalCostRateFrom = $('.divboxASearch #AdvTotalCostRateFrom').val();
        EstimateDetailReportViewModel.AdvTotalCostRateTo = $('.divboxASearch #AdvTotalCostRateTo').val();
        EstimateDetailReportViewModel.AdvTotalSellingRateFrom = $('.divboxASearch #AdvTotalSellingRateFrom').val();
        EstimateDetailReportViewModel.AdvTotalSellingRateTo = $('.divboxASearch #AdvTotalSellingRateTo').val();
        EstimateDetailReportViewModel.AdvCountryCode = $('.divboxASearch #AdvCountryCode').val();
        EstimateDetailReportViewModel.AdvStateCode = $('.divboxASearch #AdvStateCode').val();
        EstimateDetailReportViewModel.AdvDistrictCode = $('.divboxASearch #AdvDistrictCode').val();
        EstimateDetailReportViewModel.AdvCustomerCategoryCode = $('.divboxASearch #AdvCustomerCategoryCode').val();
        EstimateDetailReportViewModel.AdvApprovalStatusCode = $('.divboxASearch #AdvApprovalStatusCode').val();
        EstimateDetailReportViewModel.AdvDelFromDate = $('.divboxASearch #AdvDelFromDate').val();
        EstimateDetailReportViewModel.AdvDelToDate = $('.divboxASearch #AdvDelToDate').val();
        EstimateDetailReportViewModel.AdvReportType = $('.divboxASearch #AdvReportType').val();
        EstimateDetailReportViewModel.AdvProduct = $('.divboxASearch #AdvProduct').val();
        EstimateDetailReportViewModel.AdvProductModel = $('.divboxASearch #AdvProductModel').val();

        //apply datatable plugin on sale order standard report table
        _dataTable.EstimateDetailReportList = $("#tblEstimateDetailReport").DataTable(
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
                url: "GetEstimateDetailReport/",
                data: { "estimateDetailReportVM": EstimateDetailReportViewModel },
                type: 'POST'
            },
            pageLength: 8,
            autoWidth: false,
            columns: [
                {
                    "data": "EstNo", render: function (data, type, row) {
                        return "<img src='../Content/images/datePicker.png' height='10px'>" + "&nbsp;" + row.EstimateDateFormatted + "</br>" + row.EstNo;
                    }, "defaultContent": "<i>-</i>"
                },
               {
                   "data": "Customer.CompanyName", render: function (data, type, row) {
                       return "<img src='../Content/images/contact.png' height='10px'>" + "&nbsp;" + (row.Customer.ContactPerson == null ? "" : row.Customer.ContactPerson) + "</br>" + "<img src='../Content/images/organisation.png' height='10px'>" + "&nbsp;" + data;
                   }, "defaultContent": "<i>-</i>"
               },
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
               {
                   "data": "Qty", render: function (data, type, row) {
                       return data + "&nbsp;" + row.Unit.Description;
                   }, "defaultContent": "<i>-</i>"
               },
               { "data": "Branch.Description", "defaultContent": "<i>-</i>" },
               { "data": "PSAUser.LoginName", "defaultContent": "<i>-</i>" },
               {
                   "data": "TotalCostRate", render: function (data, type, row) {
                       if ($('#hdnCostPriceHasAccess').val() == "True")
                           return formatCurrency(roundoff(row.TotalCostRate))
                       else
                           return "****";
                   }, "defaultContent": "<i>-</i>"
               },
               {
                   "data": "TotalSellingRate", render: function (data, type, row) {
                       return formatCurrency(roundoff(row.TotalSellingRate))
                   }, "defaultContent": "<i>-</i>"
               },
            ],
            columnDefs: [{ className: "text-right", "targets": [7,8] },
                         { className: "text-left", "targets": [1, 3, 4, 6, 5,2] },
                         { className: "text-center", "targets": [0] },
                           { "targets": [0], "width": "15%" },
                           { "targets": [1], "width": "14%" },
                           { "targets": [2], "width": "16%" },
                           { "targets": [3], "width": "17%" },
                           { "targets": [4], "width": "5%" },
                           { "targets": [6], "width": "8%" },
                           { "targets": [5], "width": "5%" },
                           { "targets": [7], "width": "10%" },
                           { "targets": [8], "width": "10%" },
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
                $('#tblEstimateDetailReport').fadeIn('slow');
                if (action == undefined) {
                    $('.excelExport').hide();
                    OnServerCallComplete();
                }
                if (json.data[0] != undefined && json.data[0] != null) {
                    $('#lblTotalCostAmount').text(formatCurrency(roundoff(json.data[0].TotalCostAmount)));
                    $('#lblTotalSellingAmount').text(formatCurrency(roundoff(json.data[0].TotalSellingAmount)));
                }
                else {
                    $('#lblTotalCostAmount').text("0.00");
                    $('#lblTotalSellingAmount').text("0.00");
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
    BindOrReloadEstimateDetailReportTable('Reset');
}
//function export data to excel
function ExportReportData() {
    debugger;
    //$('.excelExport').show();
    //OnServerCallBegin();       
    BindOrReloadEstimateDetailReportTable('Export');
}

function ApplyFilterThenSearch() {
    $(".searchicon").addClass('filterApplied');
    CloseAdvanceSearch();
    BindOrReloadEstimateDetailReportTable('Search');
}

function GoToList() {
    window.location.href = "/Report";
}


