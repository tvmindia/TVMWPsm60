var _dataTable = {};
var _emptyGuid = "00000000-0000-0000-0000-000000000000";
var _datatablerowindex = -1;
var _jsonData = {};

//---------------------------------------Docuement Ready--------------------------------------------------//
$(document).ready(function () {
    debugger;
    try {

        BindOrReloadProductionOrderDetailForecastDateExceededReportTable('Init');
        debugger;
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
function BindOrReloadProductionOrderDetailForecastDateExceededReportTable(action) {
    try {
        debugger;
        //creating advancesearch object
        ProductionOrderDetailForecastDateExceededReportViewModel = new Object();
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
                $('#DateFilter').val('').trigger('change');
                $('#ReportValue').val("2").trigger('change');             
                $('.divboxASearch #AdvProduct').val('').trigger('change');
                $('.divboxASearch #AdvProductModel').val('').trigger('change');
                $('.divboxASearch #AdvPlantCode').val('').trigger('change');
                $('#ForecastIsNotNull')[0].checked = true;



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
                $('#DateFilter').val('');
                $('#ReportValue').val('2');
                $('.divboxASearch #AdvProduct').val('');
                $('.divboxASearch #AdvProductModel').val('');
                $('.divboxASearch #AdvPlantCode').val('');

                break;
            case 'Search':
                if ((SearchTerm == SearchValue)
                    && ($('.divboxASearch #AdvFromDate').val() == "")
                    && ($('.divboxASearch #AdvToDate').val() == "") &&
                    ($('.divboxASearch #AdvDocumentOwnerID').val() == "") &&
                    ($('.divboxASearch #AdvCustomer').val() == "") &&
                    ($('.divboxASearch #AdvAreaCode').val() == "") &&
                    ($('.divboxASearch #AdvBranchCode').val() == "") &&
                    ($('.divboxASearch #AdvDocumentStatusCode').val() == "") &&                 
                    ($('.divboxASearch #AdvAmountFrom').val() == "") &&
                    ($('.divboxASearch #AdvAmountTo').val() == "") &&
                     ($('.divboxASearch #AdvCountryCode').val() == "") &&
                    ($('.divboxASearch #AdvStateCode').val() == "") &&
                    ($('.divboxASearch #AdvDistrictCode').val() == "") &&
                    ($('.divboxASearch #AdvCustomerCategoryCode').val() == "") &&
                    ($('.divboxASearch #AdvReferencePersonCode').val() == "")                  
                    && ($('.divboxASearch #AdvProduct').val() == "")
                    && ($('.divboxASearch #AdvProductModel').val() == "")
                    && ($('.divboxASearch #AdvPlantCode').val() == "")
                    &&(($('#ReportValue').val() == "1")|| ($('#ReportValue').val() == "2"))                
                    )
                {
                    return true;
                }
                break;
            case 'Export':
                DataTablePagingViewModel.Length = -1;
                ProductionOrderDetailForecastDateExceededReportViewModel.DataTablePaging = DataTablePagingViewModel;

                ProductionOrderDetailForecastDateExceededReportViewModel.SearchTerm = $('#SearchTerm').val() == "" ? null : $('#SearchTerm').val();
                ProductionOrderDetailForecastDateExceededReportViewModel.AdvFromDate = $('.divboxASearch #AdvFromDate').val() == "" ? null : $('.divboxASearch #AdvFromDate').val();
                ProductionOrderDetailForecastDateExceededReportViewModel.AdvFromDate = $('#AdvFromDate').val() == "" ? null : $('#AdvFromDate').val();
                ProductionOrderDetailForecastDateExceededReportViewModel.AdvToDate = $('.divboxASearch #AdvToDate').val() == "" ? null : $('.divboxASearch #AdvToDate').val();
                ProductionOrderDetailForecastDateExceededReportViewModel.AdvToDate = $('#AdvToDate').val() == "" ? null : $('#AdvToDate').val();
                ProductionOrderDetailForecastDateExceededReportViewModel.AdvDocumentOwnerID = $('.divboxASearch #AdvDocumentOwnerID').val() == "" ? _emptyGuid : $('.divboxASearch #AdvDocumentOwnerID').val();
                ProductionOrderDetailForecastDateExceededReportViewModel.AdvCustomer = $('.divboxASearch #AdvCustomer').val() == "" ? null : $('.divboxASearch #AdvCustomer').val();
                ProductionOrderDetailForecastDateExceededReportViewModel.AdvAreaCode = $('.divboxASearch #AdvAreaCode').val() == "" ? null : $('.divboxASearch #AdvAreaCode').val();
                ProductionOrderDetailForecastDateExceededReportViewModel.AdvBranchCode = $('.divboxASearch #AdvBranchCode').val() == "" ? null : $('.divboxASearch #AdvBranchCode').val();
                ProductionOrderDetailForecastDateExceededReportViewModel.AdvDocumentStatusCode = $('.divboxASearch #AdvDocumentStatusCode').val() == "" ? null : $('.divboxASearch #AdvDocumentStatusCode').val();
                ProductionOrderDetailForecastDateExceededReportViewModel.AdvPreparedBy = $('.divboxASearch #AdvPreparedBy').val() == "" ? _emptyGuid : $('.divboxASearch #AdvPreparedBy').val();
                ProductionOrderDetailForecastDateExceededReportViewModel.ReportValue = $('#ReportValue').val() == "" ? null : $('#ReportValue').val();
                ProductionOrderDetailForecastDateExceededReportViewModel.AdvAmountFrom = $('.divboxASearch #AdvAmountFrom').val() == "" ? null : $('.divboxASearch #AdvAmountFrom').val();
                ProductionOrderDetailForecastDateExceededReportViewModel.AdvAmountTo = $('.divboxASearch #AdvAmountTo').val() == "" ? null : $('.divboxASearch #AdvAmountTo').val();
                ProductionOrderDetailForecastDateExceededReportViewModel.AdvCountryCode = $('.divboxASearch #AdvCountryCode').val() == "" ? null : $('.divboxASearch #AdvCountryCode').val();
                ProductionOrderDetailForecastDateExceededReportViewModel.AdvStateCode = $('.divboxASearch #AdvStateCode').val() == "" ? null : $('.divboxASearch #AdvStateCode').val();
                ProductionOrderDetailForecastDateExceededReportViewModel.AdvDistrictCode = $('.divboxASearch #AdvDistrictCode').val() == "" ? null : $('.divboxASearch #AdvDistrictCode').val();
                ProductionOrderDetailForecastDateExceededReportViewModel.AdvCustomerCategoryCode = $('.divboxASearch #AdvCustomerCategoryCode').val() == "" ? null : $('.divboxASearch #AdvCustomerCategoryCode').val();
                ProductionOrderDetailForecastDateExceededReportViewModel.AdvReferencePersonCode = $('.divboxASearch #AdvReferencePersonCode').val() == "" ? null : $('.divboxASearch #AdvReferencePersonCode').val();
                ProductionOrderDetailForecastDateExceededReportViewModel.AdvApprovalStatusCode = $('.divboxASearch #AdvApprovalStatusCode').val() == "" ? null : $('.divboxASearch #AdvApprovalStatusCode').val();
                ProductionOrderDetailForecastDateExceededReportViewModel.AdvDelFromDate = $('.divboxASearch #AdvDelFromDate').val() == "" ? null : $('.divboxASearch #AdvDelFromDate').val();
                ProductionOrderDetailForecastDateExceededReportViewModel.AdvDelToDate = $('.divboxASearch #AdvDelToDate').val() == "" ? null : $('.divboxASearch #AdvDelToDate').val();
                ProductionOrderDetailForecastDateExceededReportViewModel.AdvReportType = $('.divboxASearch #AdvReportType').val() == "" ? null : $('.divboxASearch #AdvReportType').val();
                ProductionOrderDetailForecastDateExceededReportViewModel.DateFilter = $('#DateFilter').val() == "" ? null : $('#DateFilter').val();
                ProductionOrderDetailForecastDateExceededReportViewModel.AdvProduct = $('.divboxASearch #AdvProduct').val() == "" ? _emptyGuid : $('.divboxASearch #AdvProduct').val();
                ProductionOrderDetailForecastDateExceededReportViewModel.AdvProductModel = $('.divboxASearch #AdvProductModel').val() == "" ? _emptyGuid : $('.divboxASearch #AdvProductModel').val();
                ProductionOrderDetailForecastDateExceededReportViewModel.AdvPlantCode = $('.divboxASearch #AdvPlantCode').val() == "" ? null : $('.divboxASearch #AdvPlantCode').val();
                $('#OptionValue').val($('#ReportValue').val());
                $('#AdvanceSearch').val(JSON.stringify(ProductionOrderDetailForecastDateExceededReportViewModel));
                $('#FormExcelExport').submit();
                return true;
                break;
            default:
                break;
        }
        ProductionOrderDetailForecastDateExceededReportViewModel.DataTablePaging = DataTablePagingViewModel;
        ProductionOrderDetailForecastDateExceededReportViewModel.SearchTerm = $('#SearchTerm').val();
        ProductionOrderDetailForecastDateExceededReportViewModel.AdvFromDate = $('.divboxASearch #AdvFromDate').val();
        ProductionOrderDetailForecastDateExceededReportViewModel.AdvToDate = $('.divboxASearch #AdvToDate').val();
        ProductionOrderDetailForecastDateExceededReportViewModel.AdvDocumentOwnerID = $('.divboxASearch #AdvDocumentOwnerID').val();
        ProductionOrderDetailForecastDateExceededReportViewModel.AdvCustomer = $('.divboxASearch #AdvCustomer').val();
        ProductionOrderDetailForecastDateExceededReportViewModel.AdvAreaCode = $('.divboxASearch #AdvAreaCode').val();
        ProductionOrderDetailForecastDateExceededReportViewModel.AdvBranchCode = $('.divboxASearch #AdvBranchCode').val();
        ProductionOrderDetailForecastDateExceededReportViewModel.AdvDocumentStatusCode = $('.divboxASearch #AdvDocumentStatusCode').val();
        ProductionOrderDetailForecastDateExceededReportViewModel.AdvPreparedBy = $('.divboxASearch #AdvPreparedBy').val();
        ProductionOrderDetailForecastDateExceededReportViewModel.AdvAmountFrom = $('.divboxASearch #AdvAmountFrom').val();
        ProductionOrderDetailForecastDateExceededReportViewModel.AdvAmountTo = $('.divboxASearch #AdvAmountTo').val();
        ProductionOrderDetailForecastDateExceededReportViewModel.AdvCountryCode = $('.divboxASearch #AdvCountryCode').val();
        ProductionOrderDetailForecastDateExceededReportViewModel.AdvStateCode = $('.divboxASearch #AdvStateCode').val();
        ProductionOrderDetailForecastDateExceededReportViewModel.AdvDistrictCode = $('.divboxASearch #AdvDistrictCode').val();
        ProductionOrderDetailForecastDateExceededReportViewModel.AdvCustomerCategoryCode = $('.divboxASearch #AdvCustomerCategoryCode').val();
        ProductionOrderDetailForecastDateExceededReportViewModel.AdvReferencePersonCode = $('.divboxASearch #AdvReferencePersonCode').val();
        ProductionOrderDetailForecastDateExceededReportViewModel.AdvApprovalStatusCode = $('.divboxASearch #AdvApprovalStatusCode').val();
        ProductionOrderDetailForecastDateExceededReportViewModel.AdvDelFromDate = $('.divboxASearch #AdvDelFromDate').val();
        ProductionOrderDetailForecastDateExceededReportViewModel.AdvDelToDate = $('.divboxASearch #AdvDelToDate').val();
        ProductionOrderDetailForecastDateExceededReportViewModel.AdvReportType = $('.divboxASearch #AdvReportType').val();
        ProductionOrderDetailForecastDateExceededReportViewModel.DateFilter = $('#DateFilter').val();
        ProductionOrderDetailForecastDateExceededReportViewModel.AdvProduct = $('.divboxASearch #AdvProduct').val();
        ProductionOrderDetailForecastDateExceededReportViewModel.AdvProductModel = $('.divboxASearch #AdvProductModel').val();
        ProductionOrderDetailForecastDateExceededReportViewModel.AdvPlantCode = $('.divboxASearch #AdvPlantCode').val();
        ProductionOrderDetailForecastDateExceededReportViewModel.ReportValue = $('#ReportValue').val();


        //apply datatable plugin on Production Order standard report table
      
            _dataTable.productionOrderDetailForecastDateExceededReportList = $("#tblProductionOrderDetailForecastDateExceededReport").DataTable(
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
                      url: "GetProductionOrderDetailForecastDateExceededReport/",
                      data: { "productionOrderReportDetailForecastDateExceededVM": ProductionOrderDetailForecastDateExceededReportViewModel },
                      type: 'POST'
                  },
                  pageLength: 8,
                  autoWidth: false,
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
                     { "data": "ExpectedDelvDateFormatted", "defaultContent": "<i>-</i>" },
                       { "data": "Progress", "defaultContent": "<i>-</i>" },
                         { "data": "ExptProgress", "defaultContent": "<i>-</i>" },
                           { "data": "ExptCompletionDate", "defaultContent": "<i>-</i>" },
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


                     { "data": "Qty", "defaultContent": "<i>-</i>" },
                     { "data": "ProducedQty", "defaultContent": "<i>-</i>" },
                     {
                         "data": "Amount", render: function (data, type, row) {
                             return formatCurrency(roundoff(row.Amount))
                         }, "defaultContent": "<i>-</i>"
                     },
                     { "data": "Plant.Description", "defaultContent": "<i>-</i>" },
                     { "data": "Area.Description", "defaultContent": "<i>-</i>" },
                     { "data": "DocumentStatus.Description", "defaultContent": "<i>-</i>" },
                     { "data": "Branch.Description", "defaultContent": "<i>-</i>" },
                     { "data": "PSAUser.LoginName", "defaultContent": "<i>-</i>" },
                     {
                         "data": "Remarks", render: function (data, type, row) {
                             return '<div class="show-popover" data-html="true" data-toggle="popover" data-content="<p align=left>' + (data === null ? "-" : data.replace(/"/g, '”')) + '</p>"/>' + (data == null ? " " : data.substring(0, 50) + (data.length > 50 ? '...' : ''))
                         }, "defaultContent": "<i>-</i>"
                     },


                  ],
                  columnDefs: [{ className: "text-right", "targets": [10] },
                               { className: "text-left", "targets": [1, 6, 7, 11, 12, 13, 14, 15, 16] },
                               { className: "text-center", "targets": [0, 2, 3, 4, 5, 8, 9] },
                                 { "targets": [0], "width": "8%" },
                                 { "targets": [1], "width": "8%" },
                                 { "targets": [2], "width": "5%" },
                                 { "targets": [3], "width": "5%" },
                                 { "targets": [4], "width": "5%" },
                                 { "targets": [5], "width": "5%" },
                                 { "targets": [6], "width": "10%" },
                                 { "targets": [7], "width": "10%" },
                                 { "targets": [8], "width": "5%" },
                                 { "targets": [9], "width": "5%" },
                                 { "targets": [10], "width": "8%" },
                                 { "targets": [11], "width": "5%" },
                                 { "targets": [12], "width": "5%" },
                                 { "targets": [13], "width": "5%" },
                                 { "targets": [14], "width": "7%" },
                                 { "targets": [15], "width": "7%" },
                                 { "targets": [16], "width": "7%" },



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
                      $('#tblProductionOrderDetailForecastDateExceededReport').fadeIn('slow');
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
    BindOrReloadProductionOrderDetailForecastDateExceededReportTable('Reset');
}
//function export data to excel
function ExportReportData() {
    debugger;
    //$('.excelExport').show();
    //OnServerCallBegin();       
    BindOrReloadProductionOrderDetailForecastDateExceededReportTable('Export');
}

function ApplyFilterThenSearch() {
    debugger;
    $(".searchicon").addClass('filterApplied');
    CloseAdvanceSearch();
    BindOrReloadProductionOrderDetailForecastDateExceededReportTable('Search');
}

function GoToList(){
    debugger;
    window.location.href = "/Report";
}


function DateFilterOnchange() {
    debugger;
    var selectedValue = $("#DateFilter").val().split("||");
    $('.divboxASearch #AdvFromDate').val(selectedValue[0]);
    $('.divboxASearch #AdvToDate').val(selectedValue[1]);
    BindOrReloadProductionOrderDetailForecastDateExceededReportTable('Search');
}
function RadioButtonOnChange() {
    debugger;
    if ($("#ForecastIsNotNull").is(':checked')) {
        var reportValue2 = $("#ForecastIsNotNull").val();
        $('#ReportValue').val(reportValue2);
    }
    else {
        var reportValue1 = $("#ForecastIsNull").val();
        $('#ReportValue').val(reportValue1);

    }
    BindOrReloadProductionOrderDetailForecastDateExceededReportTable('Apply');

    if ($('#ReportValue').val() == "1") {
        _dataTable.productionOrderDetailForecastDateExceededReportList.column(4).visible(false);
        _dataTable.productionOrderDetailForecastDateExceededReportList.column(5).visible(false);
    }
    else {
        _dataTable.productionOrderDetailForecastDateExceededReportList.column(4).visible(true);
        _dataTable.productionOrderDetailForecastDateExceededReportList.column(5).visible(true);
    }
}