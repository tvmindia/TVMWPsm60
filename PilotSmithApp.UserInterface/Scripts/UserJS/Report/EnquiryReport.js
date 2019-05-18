var _dataTable = {};
var _emptyGuid = "00000000-0000-0000-0000-000000000000";
var _datatablerowindex = -1;
var _jsonData = {};

//---------------------------------------Docuement Ready--------------------------------------------------//

$(document).ready(function () {
    debugger;
    try {
    
        BindOrReloadEnquiryReportTable('Init');
        $(' #AdvDocumentStatusCode,#AdvReferenceTypeCode,#AdvEnquiryGradeCode,#AdvAttendedByID,#AdvCustomer').select2({
            dropdownParent: $(".divboxASearch")
        });

        $('.select2').addClass('form-control newinput');

    }
    catch (e) {
        console.log(e.message);
    }
});
//function bind the Enquiry list checking search and filter
function BindOrReloadEnquiryReportTable(action) {
    try {
        debugger;
        //creating advancesearch object
        EnquiryReportViewModel = new Object();
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
                $('.divboxASearch #AdvAreaCode').val('').trigger('change');
                $('.divboxASearch #AdvCustomer').val('').trigger('change');
                $('.divboxASearch #AdvBranchCode').val('').trigger('change');
                $('.divboxASearch #AdvDocumentStatusCode').val('').trigger('change');
                $('.divboxASearch #AdvDocumentOwnerID').val('').trigger('change');
                $('.divboxASearch #AdvReferencePersonCode').val('').trigger('change');
                $('.divboxASearch #AdvReferenceTypeCode').val('').trigger('change');
                $('.divboxASearch #AdvEnquiryGradeCode').val('').trigger('change');
                $('.divboxASearch #AdvAmountFrom').val('').trigger('change');
                $('.divboxASearch #AdvAmountTo').val('').trigger('change');
                $('.divboxASearch #AdvAttendedByID').val('').trigger('change');
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
                $('.divboxASearch #AdvDocumentStatusCode').val('');
                $('.divboxASearch #AdvDocumentOwnerID').val('');
                $('.divboxASearch #AdvReferencePersonCode').val('');
                $('.divboxASearch #AdvReferenceTypeCode').val('');
                $('.divboxASearch #AdvEnquiryGradeCode').val('');
                $('.divboxASearch #AdvAmountFrom').val('');
                $('.divboxASearch #AdvAmountTo').val('');
                $('.divboxASearch #AdvAttendedByID').val('');
                $('.divboxASearch #AdvCountryCode').val('');
                $('.divboxASearch #AdvStateCode').val('');
                $('.divboxASearch #AdvDistrictCode').val('');
                $('.divboxASearch #AdvCustomerCategoryCode').val('');
                               
                break;
            case 'Search':
                if ((SearchTerm==SearchValue) && ($('.divboxASearch #AdvFromDate').val() == "")
                    && ($('.divboxASearch #AdvToDate').val() == "") &&
                    ($('.divboxASearch #AdvDocumentOwnerID').val() == "") &&
                    ($('.divboxASearch #AdvCustomer').val() == "") &&
                    ($('.divboxASearch #AdvAreaCode').val() == "")&&
                    ($('.divboxASearch #AdvBranchCode').val() == "") &&
                    ($('.divboxASearch #AdvDocumentStatusCode').val() == "") &&
                    ($('.divboxASearch #AdvReferencePersonCode').val() == "")&&
                    ($('.divboxASearch #AdvReferenceTypeCode').val() == "") &&
                    ($('.divboxASearch #AdvEnquiryGradeCode').val() == "")&&
                    ($('.divboxASearch #AdvAmountFrom').val() == "") &&
                    ($('.divboxASearch #AdvAmountTo').val() == "")&&
                    ($('.divboxASearch #AdvAttendedByID').val() == "")&&
                    ($('.divboxASearch #AdvCountryCode').val() == "") &&
                    ($('.divboxASearch #AdvStateCode').val() == "") &&
                    ($('.divboxASearch #AdvDistrictCode').val() == "")&&
                    ($('.divboxASearch #AdvCustomerCategoryCode').val() == "")                    
                    ) {
                    return true;
                }
                break;
            case 'Export':
                DataTablePagingViewModel.Length = -1;
                EnquiryReportViewModel.DataTablePaging = DataTablePagingViewModel;
                EnquiryReportViewModel.SearchTerm = $('#SearchTerm').val() == "" ? null : $('#SearchTerm').val();
                EnquiryReportViewModel.AdvFromDate = $('.divboxASearch #AdvFromDate').val() == "" ? null : $('.divboxASearch #AdvFromDate').val();
                EnquiryReportViewModel.AdvToDate = $('.divboxASearch #AdvToDate').val() == "" ? null : $('.divboxASearch #AdvToDate').val();
                EnquiryReportViewModel.AdvDocumentOwnerID = $('.divboxASearch #AdvDocumentOwnerID').val() == "" ? _emptyGuid : $('.divboxASearch #AdvDocumentOwnerID').val();
                EnquiryReportViewModel.AdvCustomer = $('.divboxASearch #AdvCustomer').val() == "" ? null : $('.divboxASearch #AdvCustomer').val();
                EnquiryReportViewModel.AdvAreaCode = $('.divboxASearch #AdvAreaCode').val() == "" ? null : $('.divboxASearch #AdvAreaCode').val();
                EnquiryReportViewModel.AdvBranchCode = $('.divboxASearch #AdvBranchCode').val() == "" ? null : $('.divboxASearch #AdvBranchCode').val();
                EnquiryReportViewModel.AdvDocumentStatusCode = $('.divboxASearch #AdvDocumentStatusCode').val() == "" ? null : $('.divboxASearch #AdvDocumentStatusCode').val();
                EnquiryReportViewModel.AdvReferencePersonCode = $('.divboxASearch #AdvReferencePersonCode').val() == "" ? null : $('.divboxASearch #AdvReferencePersonCode').val();
               // EnquiryReportViewModel.AdvReferencePersonCode = $('.divboxASearch #AdvReferencePersonCode').val() == "" ? null : $('.divboxASearch #AdvReferencePersonCode').val();
                EnquiryReportViewModel.AdvReferenceTypeCode = $('.divboxASearch #AdvReferenceTypeCode').val() == "" ? null : $('.divboxASearch #AdvReferenceTypeCode').val();
                EnquiryReportViewModel.AdvEnquiryGradeCode = $('.divboxASearch #AdvEnquiryGradeCode').val() == "" ? null : $('.divboxASearch #AdvEnquiryGradeCode').val();
                EnquiryReportViewModel.AdvAmountFrom = $('.divboxASearch #AdvAmountFrom').val() == "" ? null : $('.divboxASearch #AdvAmountFrom').val();
                EnquiryReportViewModel.AdvAmountTo = $('.divboxASearch #AdvAmountTo').val() == "" ? null : $('.divboxASearch #AdvAmountTo').val();
                EnquiryReportViewModel.AdvAttendedByID = $('.divboxASearch #AdvAttendedByID').val() == "" ? _emptyGuid : $('.divboxASearch #AdvAttendedByID').val();
                EnquiryReportViewModel.AdvCountryCode = $('.divboxASearch #AdvCountryCode').val() == "" ? null : $('.divboxASearch #AdvCountryCode').val();
                EnquiryReportViewModel.AdvStateCode = $('.divboxASearch #AdvStateCode').val() == "" ? null : $('.divboxASearch #AdvStateCode').val();
                EnquiryReportViewModel.AdvDistrictCode = $('.divboxASearch #AdvDistrictCode').val() == "" ? null : $('.divboxASearch #AdvDistrictCode').val();
                EnquiryReportViewModel.AdvCustomerCategoryCode = $('.divboxASearch #AdvCustomerCategoryCode').val() == "" ? null : $('.divboxASearch #AdvCustomerCategoryCode').val();

                $('#AdvanceSearch').val(JSON.stringify(EnquiryReportViewModel));
                $('#FormExcelExport').submit();
                return true;
                break;
            default:
                break;
        }
        EnquiryReportViewModel.DataTablePaging = DataTablePagingViewModel;
        EnquiryReportViewModel.SearchTerm = $('#SearchTerm').val();
        EnquiryReportViewModel.AdvFromDate = $('.divboxASearch #AdvFromDate').val();
        EnquiryReportViewModel.AdvToDate = $('.divboxASearch #AdvToDate').val();
        EnquiryReportViewModel.AdvDocumentOwnerID = $('.divboxASearch #AdvDocumentOwnerID').val();
        EnquiryReportViewModel.AdvCustomer = $('.divboxASearch #AdvCustomer').val();
        EnquiryReportViewModel.AdvAreaCode = $('.divboxASearch #AdvAreaCode').val();
        EnquiryReportViewModel.AdvBranchCode = $('.divboxASearch #AdvBranchCode').val();
        EnquiryReportViewModel.AdvDocumentStatusCode = $('.divboxASearch #AdvDocumentStatusCode').val();
        EnquiryReportViewModel.AdvReferencePersonCode = $('.divboxASearch #AdvReferencePersonCode').val();
        EnquiryReportViewModel.AdvAmountFrom = $('.divboxASearch #AdvAmountFrom').val();
        EnquiryReportViewModel.AdvAmountTo = $('.divboxASearch #AdvAmountTo').val();
        EnquiryReportViewModel.AdvEnquiryGradeCode = $('.divboxASearch #AdvEnquiryGradeCode').val();
        EnquiryReportViewModel.AdvReferenceTypeCode = $('.divboxASearch #AdvReferenceTypeCode').val();
        EnquiryReportViewModel.AdvAttendedByID = $('.divboxASearch #AdvAttendedByID').val();
        EnquiryReportViewModel.AdvCountryCode = $('.divboxASearch #AdvCountryCode').val();
        EnquiryReportViewModel.AdvStateCode = $('.divboxASearch #AdvStateCode').val();
        EnquiryReportViewModel.AdvDistrictCode = $('.divboxASearch #AdvDistrictCode').val();
        EnquiryReportViewModel.AdvCustomerCategoryCode = $('.divboxASearch #AdvCustomerCategoryCode').val();



        //apply datatable plugin on enquiry report table
        _dataTable.EnquiryReportList = $("#tblEnquiryReport").DataTable(
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
                url: "GetEnquiryReport/",
                data: { "enquiryReportVM": EnquiryReportViewModel },
                type: 'POST'
            },
            pageLength: 8,
            autoWidth: false,
            columns: [
               {
                   "data": "EnquiryNo", render: function (data, type, row) {
                       return "<img src='../Content/images/datePicker.png' height='10px'>" + "&nbsp;" + row.EnquiryDateFormatted + "</br>" + row.EnquiryNo;
                   }, "defaultContent": "<i>-</i>"
               },
               {
                   "data": "Customer.CompanyName", render: function (data, type, row) {
                       return "<img src='../Content/images/contact.png' height='10px'>" + "&nbsp;" + (row.Customer.ContactPerson == null ? "" : row.Customer.ContactPerson) + "</br>" + "<img src='../Content/images/organisation.png' height='10px'>" + "&nbsp;" + data;
                   }, "defaultContent": "<i>-</i>"
               },
               {
                   "data": "RequirementSpec", render: function (data, type, row) {
                       return '<div class="show-popover" data-html="true" data-toggle="popover" data-content="<p align=left>' + (data === null ? "-" : data.replace(/"/g, '”')) + '</p>"/>' + (data == null ? " " : data.substring(0, 50) + (data.length > 50 ? '...' : ''))
                   }, "defaultContent": "<i>-</i>"
               },
               { "data": "EnquiryGrade.Description", "defaultContent": "<i>-</i>" },
               {
                   "data": "Area.Description", "defaultContent": "<i>-</i>"
               },
               { "data": "ReferencePerson.Name", "defaultContent": "<i>-</i>" },
               { "data": "Employee.Name", "defaultContent": "<i>-</i>" },
               { "data": "DocumentStatus.Description", "defaultContent": "<i>-</i>" },
               { "data": "Branch.Description", "defaultContent": "<i>-</i>" },
               { "data": "PSAUser.LoginName", "defaultContent": "<i>-</i>" },
               {
                   "data": "Amount", render: function (data, type, row) {
                       return formatCurrency(roundoff(row.Amount))
                   }, "defaultContent": "<i>-</i>"
               },
            ],
            columnDefs: [{ className: "text-right", "targets": [10] },
                         { className: "text-left", "targets": [1,2,3,4,5,6,9,8] },
                         { className: "text-center", "targets": [0,7] },
                           { "targets": [0], "width": "17%" },
                           { "targets": [1], "width": "18%" },
                           { "targets": [2], "width": "20%" },
                           { "targets": [4], "width": "5%" },
                           { "targets": [8], "width": "5%" },
                           { "targets": [5], "width": "5%" },
                           { "targets": [7], "width": "5%" },
                           { "targets": [3], "width": "5%" },
                           { "targets": [6], "width": "5%" },
                           { "targets": [9], "width": "5%" },
                           { "targets": [10], "width": "10%" }
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
                $('#tblEnquiryReport').fadeIn('slow');
                if (action == undefined) {
                    $('.excelExport').hide();
                    OnServerCallComplete();
                }
                if (json.data[0] != undefined && json.data[0] != null)
                    $('#lblTotalAmount').text(formatCurrency(roundoff(json.data[0].TotalAmount)));
                else
                    $('#lblTotalAmount').text("0.00");
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
    BindOrReloadEnquiryReportTable('Reset');
}
//function export data to excel
function ExportReportData() {
    debugger;
    //$('.excelExport').show();
    //OnServerCallBegin();
    BindOrReloadEnquiryReportTable('Export');
}

function ApplyFilterThenSearch() {
    $(".searchicon").addClass('filterApplied');
    CloseAdvanceSearch();
    BindOrReloadEnquiryReportTable('Search');
}

function GoToList()
{
    window.location.href = "/Report";
}