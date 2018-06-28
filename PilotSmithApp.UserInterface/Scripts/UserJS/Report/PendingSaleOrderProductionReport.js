//var _dataTable = {};
//var _emptyGuid = "00000000-0000-0000-0000-000000000000";
//var _datatablerowindex = -1;
//var _jsonData = {};
//var _message = "";
//var _status = "";
//var _result = "";
////---------------------------------------Docuement Ready--------------------------------------------------//

//$(document).ready(function () {
//    debugger;
//    try {
//        $("#AdvProductID").select2({
//            dropdownParent: $(".divboxASearch")
//        });

//        $('.select2').addClass('form-control newinput');
//        BindOrReloadSaleOrderPendingProductionTable('Init');

//    }
//    catch (e) {
//        console.log(e.message);
//    }
//});
////function bind the SaleOrder list checking search and filter
//function BindOrReloadSaleOrderPendingProductionTable(action) {
//    try {
//        debugger;
//        //creating advancesearch object
//        PendingSaleOrderProductionReportViewModel = new Object();
//        DataTablePagingViewModel = new Object();
//        DataTablePagingViewModel.Length = 0;
//        //switch case to check the operation
//        switch (action) {
//            case 'Reset':
//                $('#SearchTerm').val('');
//                $('.divboxASearch #AdvFromDate').val('');
//                $('.divboxASearch #AdvToDate').val('');
//                $('.divboxASearch #AdvProductID').val('').trigger('change');
//                $('.divboxASearch #AdvCustomerID').val('').trigger('change');                
//                break;
//            case 'Init':
//                $('#SearchTerm').val('');
//                $('.divboxASearch #AdvFromDate').val('');
//                $('.divboxASearch #AdvToDate').val('');
//                $('.divboxASearch #AdvProductID').val('');
//                $('.divboxASearch #AdvCustomerID').val('');              
//                break;
//            case 'Search':
//                if (($('#SearchTerm').val() == "") && ($('.divboxASearch #AdvFromDate').val() == "") && ($('.divboxASearch #AdvToDate').val() == "") && ($('.divboxASearch #AdvProductID').val() == "") && ($('.divboxASearch #AdvCustomerID').val() == "")) {
//                    return true;
//                }
//                break;
//            case 'Export':
//                DataTablePagingViewModel.Length = -1;
//                PendingSaleOrderProductionReportViewModel.DataTablePaging = DataTablePagingViewModel;
//                PendingSaleOrderProductionReportViewModel.SearchTerm = $('#SearchTerm').val() == "" ? null : $('#SearchTerm').val();
//                PendingSaleOrderProductionReportViewModel.AdvFromDate = $('.divboxASearch #AdvFromDate').val() == "" ? null : $('.divboxASearch #AdvFromDate').val();
//                PendingSaleOrderProductionReportViewModel.AdvToDate = $('.divboxASearch #AdvToDate').val() == "" ? null : $('.divboxASearch #AdvToDate').val();
//                PendingSaleOrderProductionReportViewModel.AdvProductID = $('.divboxASearch #AdvProductID').val() == "" ? _emptyGuid : $('.divboxASearch #AdvProductID').val();
//                PendingSaleOrderProductionReportViewModel.AdvCustomerID = $('.divboxASearch #AdvCustomerID').val() == "" ? _emptyGuid : $('.divboxASearch #AdvCustomerID').val();
              
//                $('#AdvanceSearch').val(JSON.stringify(PendingSaleOrderProductionReportViewModel));
//                $('#FormExcelExport').submit();
//                return true;
//                break;
//            default:
//                break;
//        }
//        PendingSaleOrderProductionReportViewModel.DataTablePaging = DataTablePagingViewModel;
//        PendingSaleOrderProductionReportViewModel.SearchTerm = $('#SearchTerm').val();
//        PendingSaleOrderProductionReportViewModel.AdvFromDate = $('.divboxASearch #AdvFromDate').val();
//        PendingSaleOrderProductionReportViewModel.AdvToDate = $('.divboxASearch #AdvToDate').val();
//        PendingSaleOrderProductionReportViewModel.AdvProductID = $('.divboxASearch #AdvProductID').val();
//        PendingSaleOrderProductionReportViewModel.AdvCustomerID = $('.divboxASearch #AdvCustomerID').val();      
      
//        //apply datatable plugin on SaleOrder table
//        _dataTable.PendingSaleOrderProductionList = $('#tblPendingSaleOrderProduction').DataTable(
//        {
//            dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
//            //buttons: [{
//            //    extend: 'excel',
//            //    exportOptions:
//            //                 {
//            //                     columns: [0, 1, 2, 3, 4, 5]
//            //                 }
//            //}],
//            ordering: false,
//            searching: false,
//            paging: true,
//            lengthChange: false,
//            processing: true,
//            language: {
//                "processing": "<div class='spinner'><div class='bounce1'></div><div class='bounce2'></div><div class='bounce3'></div></div>"
//            },
//            serverSide: true,
//            ajax: {
//                url: "Report/GetPendingSaleOrderProductionReport/",
//                data: { "pendingSaleOrderProductionReportVM": PendingSaleOrderProductionReportViewModel },
//                type: 'POST'
//            },
//            pageLength: 8,
//            autoWidth: false,
//            columns: [
//               {
//                   "data": "SaleOrderNo", render: function (data, type, row) {
//                       return row.SaleOrderNo + "</br>" + "<img src='./Content/images/datePicker.png' height='10px'>" + "&nbsp;" + row.SaleOrderDateFormatted;
//                   }, "defaultContent": "<i>-</i>"
//               },
//               {
//                   "data": "Customer.CompanyName", render: function (data, type, row) {
//                       return "<img src='./Content/images/contact.png' height='10px'>" + "&nbsp;" + (row.Customer.ContactPerson == null ? "" : row.Customer.ContactPerson) + "</br>" + "<img src='./Content/images/organisation.png' height='10px'>" + "&nbsp;" + data;
//                   }, "defaultContent": "<i>-</i>"
//               },
//               { "data": "Product.Name", "defaultContent": "<i>-</i>" },
//               {
//                   "data": "SaleOrderQty", "defaultContent": "<i>-</i>"
//               },
//               { "data": "OrderQty", "defaultContent": "<i>-</i>" },
//               { "data": "ProducedQty", "defaultContent": "<i>-</i>" },
//               { "data": "PendingQty", "defaultContent": "<i>-</i>" },             

               
//            ],
//            columnDefs: [{ className: "text-right", "targets": [] },
//                         { className: "text-left", "targets": [0, 1, 2] },
//                         { className: "text-center", "targets": [3,4,5,6] },
//                           { "targets": [0], "width": "12%" },
//                           { "targets": [1], "width": "12%" },
//                           { "targets": [2], "width": "12%" },
//                           { "targets": [3], "width": "12%" },
//                           { "targets": [4], "width": "12%" },
//                           { "targets": [5], "width": "12%" },
//                           { "targets": [6], "width": "12%" },
                           
//            ],
//            destroy: true,
//            //for performing the import operation after the data loaded
//            initComplete: function (settings, json) {
//                debugger;
//                $('.dataTables_wrapper div.bottom div').addClass('col-md-6');
//                $('#tblPendingSaleOrderProduction').fadeIn('slow');
//                if (action == undefined) {
//                    $('.excelExport').hide();
//                    OnServerCallComplete();
//                }
                
//            }
//        });
      
//    }
//    catch (e) {
//        console.log(e.message);
//    }
//}

////function reset the list to initial
//function ResetReportList() {
//    $(".searchicon").removeClass('filterApplied');
//    BindOrReloadSaleOrderPendingProductionTable('Reset');
//}
////function export data to excel
//function ExportReportData() {
//    debugger;
//    //$('.excelExport').show();
//    //OnServerCallBegin();
//    BindOrReloadSaleOrderPendingProductionTable('Export');
//}

//function ApplyFilterThenSearch() {
//    $(".searchicon").addClass('filterApplied');
//    CloseAdvanceSearch();
//    BindOrReloadSaleOrderPendingProductionTable('Search');
//}