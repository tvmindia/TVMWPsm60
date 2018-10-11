var _dataTable = {};
var _emptyGuid = "00000000-0000-0000-0000-000000000000";
var _datatablerowindex = -1;
var _jsonData = {};

//---------------------------------------Docuement Ready--------------------------------------------------//

$(document).ready(function () {
    debugger;
    try {

        BindOrReloadEnquiryFollowupReportTable('Init');
        $('#AdvStatus,#AdvFollowupPriority,#AdvCustomer').select2({
            dropdownParent: $(".divboxASearch")
        });

        $('.select2').addClass('form-control newinput');

    }
    catch (e) {
        console.log(e.message);
    }
});
//function bind the Enquiry Followup list checking search and filter
function BindOrReloadEnquiryFollowupReportTable(action) {
    try {
        debugger;
        //creating advancesearch object
        EnquiryFollowupReportViewModel = new Object();
        DataTablePagingViewModel = new Object();
        DataTablePagingViewModel.Length = 0;
        var SerachValue = $('#hdnSearchTerm').val();
        var SearchTerm = $('#SearchTerm').val();
        $('#hdnSearchTerm').val($('#SearchTerm').val())
        //switch case to check the operation
        switch (action) {
            case 'Reset':
                $('#SearchTerm').val('');
                $('.divboxASearch #AdvFromDate').val('');
                $('.divboxASearch #AdvToDate').val('');               
                $('.divboxASearch #AdvCustomer').val('').trigger('change');              
                $('.divboxASearch #AdvStatus').val('').trigger('change');              
                $('.divboxASearch #AdvFollowupPriority').val('').trigger('change');
               



                break;
            case 'Init':
                $('#SearchTerm').val('');
                $('.divboxASearch #AdvFromDate').val('');
                $('.divboxASearch #AdvToDate').val('');               
                $('.divboxASearch #AdvCustomer').val('');              
                $('.divboxASearch #AdvStatus');              
                $('.divboxASearch #AdvFollowupPriority').val('');

                break;
            case 'Search':
                if ((SearchTerm == SerachValue) && ($('.divboxASearch #AdvFromDate').val() == "")
                    && ($('.divboxASearch #AdvToDate').val() == "") &&                   
                    ($('.divboxASearch #AdvCustomer').val() == "") &&                   
                    ($('.divboxASearch #AdvStatus').val() == "") &&
                    ($('.divboxASearch #AdvFollowupPriority').val() == "")

                    ) {
                    return true;
                }
                break;
            case 'Export':
                DataTablePagingViewModel.Length = -1;
                EnquiryFollowupReportViewModel.DataTablePaging = DataTablePagingViewModel;
                EnquiryFollowupReportViewModel.SearchTerm = $('#SearchTerm').val() == "" ? null : $('#SearchTerm').val();
                EnquiryFollowupReportViewModel.AdvFromDate = $('.divboxASearch #AdvFromDate').val() == "" ? null : $('.divboxASearch #AdvFromDate').val();
                EnquiryFollowupReportViewModel.AdvToDate = $('.divboxASearch #AdvToDate').val() == "" ? null : $('.divboxASearch #AdvToDate').val();               
                EnquiryFollowupReportViewModel.AdvCustomer = $('.divboxASearch #AdvCustomer').val() == "" ? null : $('.divboxASearch #AdvCustomer').val();
                EnquiryFollowupReportViewModel.AdvStatus = $('.divboxASearch #AdvStatus').val() == "" ? null : $('.divboxASearch #AdvStatus').val();          
                EnquiryFollowupReportViewModel.AdvFollowupPriority = $('.divboxASearch #AdvFollowupPriority').val() == "" ? null : $('.divboxASearch #AdvFollowupPriority').val();               
                $('#AdvanceSearch').val(JSON.stringify(EnquiryFollowupReportViewModel));
                $('#FormExcelExport').submit();
                return true;
                break;
            default:
                break;
        }
        EnquiryFollowupReportViewModel.DataTablePaging = DataTablePagingViewModel;
        EnquiryFollowupReportViewModel.SearchTerm = $('#SearchTerm').val();
        EnquiryFollowupReportViewModel.AdvFromDate = $('.divboxASearch #AdvFromDate').val();
        EnquiryFollowupReportViewModel.AdvToDate = $('.divboxASearch #AdvToDate').val();        
        EnquiryFollowupReportViewModel.AdvCustomer = $('.divboxASearch #AdvCustomer').val();       
        EnquiryFollowupReportViewModel.AdvStatus = $('.divboxASearch #AdvStatus').val();
        EnquiryFollowupReportViewModel.AdvFollowupPriority = $('.divboxASearch #AdvFollowupPriority').val();


        debugger;
        //apply datatable plugin on enquiry followup report table
        _dataTable.EnquiryFollowupReportList = $("#tblEnquiryFollowupReport").DataTable(
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
                url: "GetEnquiryFollowupReport/",
                data: { "enquiryFollowupReportVM": EnquiryFollowupReportViewModel },
                type: 'POST'
            },
            pageLength: 8,
            autoWidth: false,
            columns: [
               {
                   "data": "FollowupDateFormatted", render: function (data, type, row) {
                       return "<img src='../Content/images/datePicker.png' height='10px'>" + "&nbsp;" + data + "&nbsp;&nbsp;<img src='../Content/images/time.png' height='10px'>" + "&nbsp;" + row.FollowupTimeFormatted + "</br>" + row.EnquiryNo;
                   }, "defaultContent": "<i>-</i>"
               },
               { "data": "Priority", "defaultContent": "<i>-</i>" },
               { "data": "EnquiryDateFormatted", "defaultContent": "<i>-</i>" },
               { "data": "Customer.CompanyName", "defaultContent": "<i>-</i>" },
               {
                   "data": "Customer.ContactPerson", render: function (data, type, row) {
                       return "<img src='../Content/images/contact.png' height='10px'>" + "&nbsp;" + (row.Customer.ContactPerson == null ? "" : row.Customer.ContactPerson) + "</br>" + "<img src='../Content/images/phone.png' height='10px'>" + "&nbsp;" + row.ContactNo;
                   }, "defaultContent": "<i>-</i>"
               },
               { "data": "Status", "defaultContent": "<i>-</i>" },
               {
                   "data": "FollowupRemarks", render: function (data, type, row) {
                       return '<div class="show-popover" data-html="true" data-toggle="popover" data-content="<p align=left>' + (data === null ? "-" : data.replace(/"/g, '”')) + '</p>"/>' + (data == null ? " " : data.substring(0, 50) + (data.length > 50 ? '...' : ''))
                   }, "defaultContent": "<i>-</i>"
               },
            ],
            columnDefs: [{ className: "text-right", "targets": [] },
                         { className: "text-left", "targets": [ 3,4,6] },
                         { className: "text-center", "targets": [0,1,5,2] },
                           { "targets": [0], "width": "20%" },
                           { "targets": [1], "width": "10%" },
                           { "targets": [5], "width": "10%" },
                           { "targets": [4], "width": "13%" },
                           { "targets": [3], "width": "17%" },
                           { "targets": [2], "width": "10%" },
                           { "targets": [6], "width": "20%" },
                           //{ "targets": [7], "width": "10%" },
                           //{ "targets": [8], "width": "17%" }
                          //{
                          //    targets: [2],
                          //    render: function (data, type, row) {
                          //        switch (data) {
                          //            case '1': return 'High'; break;
                          //            case '2': return 'Medium'; break;
                          //            case '3': return 'Low'; break;
                          //            default: return '';
                          //        }
                          //    }
                          //}
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
                $('#tblEnquiryFollowupReport').fadeIn('slow');
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
    BindOrReloadEnquiryFollowupReportTable('Reset');
}
//function export data to excel
function ExportReportData() {
    debugger;
    //$('.excelExport').show();
    //OnServerCallBegin();
    BindOrReloadEnquiryFollowupReportTable('Export');
}

function ApplyFilterThenSearch() {
    $(".searchicon").addClass('filterApplied');
    CloseAdvanceSearch();
    BindOrReloadEnquiryFollowupReportTable('Search');
}

function GoToList() {
    window.location.href = "/Report";
}