var _dataTable = {};
var _emptyGuid = "00000000-0000-0000-0000-000000000000";
var _datatablerowindex = -1;
var _jsonData = {};

//---------------------------------------Docuement Ready--------------------------------------------------//

$(document).ready(function () {
    debugger;
    try {

        BindOrReloadEnquiryDetailReportTable('Init');
        $(' #AdvDocumentStatusCode,#AdvAttendedBy,#AdvCustomer,#AdvProductModel,#AdvProduct,#AdvEnquiryGradeCode,#AdvResponsibleBy').select2({
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
function BindOrReloadEnquiryDetailReportTable(action) {
    try {
        debugger;
        //creating advancesearch object
        EnquiryDetailReportViewModel = new Object();
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
                $('.divboxASearch #AdvAttendedBy').val('').trigger('change');
                $('.divboxASearch #AdvCountryCode').val('').trigger('change');
                $('.divboxASearch #AdvStateCode').val('').trigger('change');
                $('.divboxASearch #AdvDistrictCode').val('').trigger('change');
                $('.divboxASearch #AdvCustomerCategoryCode').val('').trigger('change');
                $('.divboxASearch #AdvReferencePersonCode').val('').trigger('change');
                $('.divboxASearch #AdvReportType').val('').trigger('change');
                $('.divboxASearch #AdvProduct').val('').trigger('change');
                $('.divboxASearch #AdvProductModel').val('').trigger('change');
                $('.divboxASearch #AdvEnquiryGradeCode').val('').trigger('change');
                $('.divboxASearch #AdvResponsibleBy').val('').trigger('change');
                

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
                $('.divboxASearch #AdvAttendedBy').val('');
                $('.divboxASearch #AdvAmountFrom').val('');
                $('.divboxASearch #AdvAmountTo').val('');
                $('.divboxASearch #AdvCountryCode').val('');
                $('.divboxASearch #AdvStateCode').val('');
                $('.divboxASearch #AdvDistrictCode').val('');
                $('.divboxASearch #AdvCustomerCategoryCode').val('');
                $('.divboxASearch #AdvReferencePersonCode').val('');
                $('.divboxASearch #AdvReportType').val('');
                $('.divboxASearch #AdvProduct').val('');
                $('.divboxASearch #AdvProductModel').val('');
                $('.divboxASearch #AdvEnquiryGradeCode').val('').trigger('');
                $('.divboxASearch #AdvResponsibleBy').val('').trigger('');
                break;
            case 'Search':
                if ((SearchTerm == SearchValue) && ($('.divboxASearch #AdvFromDate').val() == "")
                    && ($('.divboxASearch #AdvToDate').val() == "") &&
                    ($('.divboxASearch #AdvDocumentOwnerID').val() == "") &&
                    ($('.divboxASearch #AdvCustomer').val() == "") &&
                    ($('.divboxASearch #AdvAreaCode').val() == "") &&
                    ($('.divboxASearch #AdvBranchCode').val() == "") &&
                    ($('.divboxASearch #AdvDocumentStatusCode').val() == "") &&
                    ($('.divboxASearch #AdvAttendedBy').val() == "") &&
                    ($('.divboxASearch #AdvAmountFrom').val() == "") &&
                    ($('.divboxASearch #AdvAmountTo').val() == "") &&
                     ($('.divboxASearch #AdvCountryCode').val() == "") &&
                    ($('.divboxASearch #AdvStateCode').val() == "") &&
                    ($('.divboxASearch #AdvDistrictCode').val() == "") &&
                    ($('.divboxASearch #AdvCustomerCategoryCode').val() == "") &&
                    ($('.divboxASearch #AdvReferencePersonCode').val() == "") &&
                    //&& ($('.divboxASearch #AdvReportType').val() == "")                  
                    ($('.divboxASearch #AdvProduct').val() == "")
                    && ($('.divboxASearch #AdvProductModel').val() == "")
                    && ($('.divboxASearch #AdvResponsibleBy').val() == "")
                    && ($('.divboxASearch #AdvEnquiryGradeCode').val() == "")
                    ) {
                    return true;
                }
                break;
            case 'Export':
                DataTablePagingViewModel.Length = -1;
                EnquiryDetailReportViewModel.DataTablePaging = DataTablePagingViewModel;

                EnquiryDetailReportViewModel.SearchTerm = $('#SearchTerm').val() == "" ? null : $('#SearchTerm').val();
                EnquiryDetailReportViewModel.AdvFromDate = $('.divboxASearch #AdvFromDate').val() == "" ? null : $('.divboxASearch #AdvFromDate').val();
                EnquiryDetailReportViewModel.AdvFromDate = $('#AdvFromDate').val() == "" ? null : $('#AdvFromDate').val();
                EnquiryDetailReportViewModel.AdvToDate = $('.divboxASearch #AdvToDate').val() == "" ? null : $('.divboxASearch #AdvToDate').val();
                EnquiryDetailReportViewModel.AdvToDate = $('#AdvToDate').val() == "" ? null : $('#AdvToDate').val();

                EnquiryDetailReportViewModel.AdvDocumentOwnerID = $('.divboxASearch #AdvDocumentOwnerID').val() == "" ? _emptyGuid : $('.divboxASearch #AdvDocumentOwnerID').val();
                EnquiryDetailReportViewModel.AdvCustomer = $('.divboxASearch #AdvCustomer').val() == "" ? null : $('.divboxASearch #AdvCustomer').val();
                EnquiryDetailReportViewModel.AdvAreaCode = $('.divboxASearch #AdvAreaCode').val() == "" ? null : $('.divboxASearch #AdvAreaCode').val();
                EnquiryDetailReportViewModel.AdvBranchCode = $('.divboxASearch #AdvBranchCode').val() == "" ? null : $('.divboxASearch #AdvBranchCode').val();
                EnquiryDetailReportViewModel.AdvDocumentStatusCode = $('.divboxASearch #AdvDocumentStatusCode').val() == "" ? null : $('.divboxASearch #AdvDocumentStatusCode').val();
                EnquiryDetailReportViewModel.AdvAttendedBy = $('.divboxASearch #AdvAttendedBy').val() == "" ? _emptyGuid : $('.divboxASearch #AdvAttendedBy').val();
                EnquiryDetailReportViewModel.AdvResponsibleBy = $('.divboxASearch #AdvResponsibleBy').val() == "" ? _emptyGuid : $('.divboxASearch #AdvResponsibleBy').val();

                EnquiryDetailReportViewModel.AdvAmountFrom = $('.divboxASearch #AdvAmountFrom').val() == "" ? null : $('.divboxASearch #AdvAmountFrom').val();
                EnquiryDetailReportViewModel.AdvAmountTo = $('.divboxASearch #AdvAmountTo').val() == "" ? null : $('.divboxASearch #AdvAmountTo').val();
                EnquiryDetailReportViewModel.AdvCountryCode = $('.divboxASearch #AdvCountryCode').val() == "" ? null : $('.divboxASearch #AdvCountryCode').val();
                EnquiryDetailReportViewModel.AdvStateCode = $('.divboxASearch #AdvStateCode').val() == "" ? null : $('.divboxASearch #AdvStateCode').val();
                EnquiryDetailReportViewModel.AdvDistrictCode = $('.divboxASearch #AdvDistrictCode').val() == "" ? null : $('.divboxASearch #AdvDistrictCode').val();
                EnquiryDetailReportViewModel.AdvCustomerCategoryCode = $('.divboxASearch #AdvCustomerCategoryCode').val() == "" ? null : $('.divboxASearch #AdvCustomerCategoryCode').val();
                EnquiryDetailReportViewModel.AdvReferencePersonCode = $('.divboxASearch #AdvReferencePersonCode').val() == "" ? null : $('.divboxASearch #AdvReferencePersonCode').val();
                EnquiryDetailReportViewModel.AdvApprovalStatusCode = $('.divboxASearch #AdvApprovalStatusCode').val() == "" ? null : $('.divboxASearch #AdvApprovalStatusCode').val();
                EnquiryDetailReportViewModel.AdvReportType = $('.divboxASearch #AdvReportType').val() == "" ? null : $('.divboxASearch #AdvReportType').val();
                EnquiryDetailReportViewModel.AdvProduct = $('.divboxASearch #AdvProduct').val() == "" ? _emptyGuid : $('.divboxASearch #AdvProduct').val();
                EnquiryDetailReportViewModel.AdvProductModel = $('.divboxASearch #AdvProductModel').val() == "" ? _emptyGuid : $('.divboxASearch #AdvProductModel').val();
                EnquiryDetailReportViewModel.AdvEnquiryGradeCode = $('.divboxASearch #AdvEnquiryGradeCode').val() == "" ? null : $('.divboxASearch #AdvEnquiryGradeCode').val();

                $('#AdvanceSearch').val(JSON.stringify(EnquiryDetailReportViewModel));
                $('#FormExcelExport').submit();
                return true;
                break;
            default:
                break;
        }
        EnquiryDetailReportViewModel.DataTablePaging = DataTablePagingViewModel;
        EnquiryDetailReportViewModel.SearchTerm = $('#SearchTerm').val();
        EnquiryDetailReportViewModel.AdvFromDate = $('.divboxASearch #AdvFromDate').val();
        EnquiryDetailReportViewModel.AdvToDate = $('.divboxASearch #AdvToDate').val();
        EnquiryDetailReportViewModel.AdvDocumentOwnerID = $('.divboxASearch #AdvDocumentOwnerID').val();
        EnquiryDetailReportViewModel.AdvCustomer = $('.divboxASearch #AdvCustomer').val();
        EnquiryDetailReportViewModel.AdvAreaCode = $('.divboxASearch #AdvAreaCode').val();
        EnquiryDetailReportViewModel.AdvBranchCode = $('.divboxASearch #AdvBranchCode').val();
        EnquiryDetailReportViewModel.AdvDocumentStatusCode = $('.divboxASearch #AdvDocumentStatusCode').val();
        EnquiryDetailReportViewModel.AdvAttendedBy = $('.divboxASearch #AdvAttendedBy').val();
        EnquiryDetailReportViewModel.AdvAmountFrom = $('.divboxASearch #AdvAmountFrom').val();
        EnquiryDetailReportViewModel.AdvAmountTo = $('.divboxASearch #AdvAmountTo').val();
        EnquiryDetailReportViewModel.AdvCountryCode = $('.divboxASearch #AdvCountryCode').val();
        EnquiryDetailReportViewModel.AdvStateCode = $('.divboxASearch #AdvStateCode').val();
        EnquiryDetailReportViewModel.AdvDistrictCode = $('.divboxASearch #AdvDistrictCode').val();
        EnquiryDetailReportViewModel.AdvCustomerCategoryCode = $('.divboxASearch #AdvCustomerCategoryCode').val();
        EnquiryDetailReportViewModel.AdvReferencePersonCode = $('.divboxASearch #AdvReferencePersonCode').val();
        EnquiryDetailReportViewModel.AdvApprovalStatusCode = $('.divboxASearch #AdvApprovalStatusCode').val();
        EnquiryDetailReportViewModel.AdvDelFromDate = $('.divboxASearch #AdvDelFromDate').val();
        EnquiryDetailReportViewModel.AdvDelToDate = $('.divboxASearch #AdvDelToDate').val();
        EnquiryDetailReportViewModel.AdvReportType = $('.divboxASearch #AdvReportType').val();
        EnquiryDetailReportViewModel.AdvProduct = $('.divboxASearch #AdvProduct').val();
        EnquiryDetailReportViewModel.AdvProductModel = $('.divboxASearch #AdvProductModel').val();
        EnquiryDetailReportViewModel.AdvEnquiryGradeCode = $('.divboxASearch #AdvEnquiryGradeCode').val();
        EnquiryDetailReportViewModel.AdvResponsibleBy = $('.divboxASearch #AdvResponsibleBy').val();

        //apply datatable plugin on sale order standard report table
        _dataTable.EnquiryDetailReportList = $("#tblEnquiryDetailReport").DataTable(
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
                url: "GetEnquiryDetailReport/",
                data: { "enquiryDetailReportVM": EnquiryDetailReportViewModel },
                type: 'POST'
            },
            pageLength: 8,
            autoWidth: false,
            columns: [
                {
                    "data": "EnqNo", render: function (data, type, row) {
                        return "<img src='../Content/images/datePicker.png' height='10px'>" + "&nbsp;" + row.EnquiryDateFormatted + "</br>" + row.EnqNo;
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
                   "data": "Amount", render: function (data, type, row) {
                       return formatCurrency(row.Amount)
                   }, "defaultContent": "<i>-</i>"
               },
            ],
            columnDefs: [{ className: "text-right", "targets": [7] },
                         { className: "text-left", "targets": [1, 3, 4, 6, 5] },
                         { className: "text-center", "targets": [0] },
                           { "targets": [0], "width": "15%" },
                           { "targets": [1], "width": "17%" },
                           { "targets": [2], "width": "18%" },
                           { "targets": [3], "width": "22%" },
                           { "targets": [4], "width": "5%" },
                           { "targets": [6], "width": "8%" },
                           { "targets": [5], "width": "5%" },
                           { "targets": [7], "width": "10%" },
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
                $('#tblEnquiryDetailReport').fadeIn('slow');
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
    BindOrReloadEnquiryDetailReportTable('Reset');
}
//function export data to excel
function ExportReportData() {
    debugger;
    //$('.excelExport').show();
    //OnServerCallBegin();       
    BindOrReloadEnquiryDetailReportTable('Export');
}

function ApplyFilterThenSearch() {
    $(".searchicon").addClass('filterApplied');
    CloseAdvanceSearch();
    BindOrReloadEnquiryDetailReportTable('Search');
}

function GoToList() {
    window.location.href = "/Report";
}


