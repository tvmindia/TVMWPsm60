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
                if (($('#SearchTerm').val() == "") && ($('.divboxASearch #AdvFromDate').val() == "")
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
             
               { "data": "FollowupDateFormatted", "defaultContent": "<i>-</i>" },
               { "data": "FollowupTimeFormatted", "defaultContent": "<i>-</i>" },
               { "data": "Priority", "defaultContent": "<i>-</i>" },
               { "data": "Status", "defaultContent": "<i>-</i>" },
               { "data": "EnquiryNo", "defaultContent": "<i>-</i>" },
               { "data": "EnquiryDateFormatted", "defaultContent": "<i>-</i>" },
               { "data": "Customer.CompanyName", "defaultContent": "<i>-</i>" },
               { "data": "Customer.ContactPerson", "defaultContent": "<i>-</i>" },         
               { "data": "ContactNo", "defaultContent": "<i>-</i>" },

               { "data": "FollowupRemarks", "defaultContent": "<i>-</i>" },
              
   

            ],
            columnDefs: [{ className: "text-right", "targets": [] },
                         { className: "text-left", "targets": [ 0,1,2,3,4,5, 6, 7,8,9] },
                         { className: "text-center", "targets": [] },
                           { "targets": [0], "width": "12%" },
                           { "targets": [1], "width": "12%" },
                           { "targets": [2], "width": "10%" },
                           { "targets": [3], "width": "10%" },
                           { "targets": [4], "width": "12%" },
                           { "targets": [5], "width": "12%" },
                           { "targets": [6], "width": "12%" },
                           { "targets": [7], "width": "10%" },
                           { "targets": [8], "width": "12%" },

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