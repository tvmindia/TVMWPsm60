﻿var _dataTable = {};
var _emptyGuid = "00000000-0000-0000-0000-000000000000";
var _datatablerowindex = -1;
var _jsonData = {};

//---------------------------------------Docuement Ready--------------------------------------------------//

$(document).ready(function () {
    debugger;
    try {

        BindOrReloadSaleOrderReportTable('Init');
        $(' #AdvDocumentStatusCode,#AdvPreparedBy,#AdvCustomer,#AdvProductModel,#AdvProduct').select2({
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
//function bind the SaleOrder standard report list checking search and filter
function BindOrReloadSaleOrderReportTable(action) {
    try {
        debugger;
        //creating advancesearch object
        SaleOrderReportViewModel = new Object();
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
                $('.divboxASearch #AdvPreparedBy').val('').trigger('change');
                $('.divboxASearch #AdvCountryCode').val('').trigger('change');
                $('.divboxASearch #AdvStateCode').val('').trigger('change');
                $('.divboxASearch #AdvDistrictCode').val('').trigger('change');
                $('.divboxASearch #AdvCustomerCategoryCode').val('').trigger('change');
                $('.divboxASearch #AdvReferencePersonCode').val('').trigger('change');
                $('.divboxASearch #AdvApprovalStatusCode').val('').trigger('change');
                $('.divboxASearch #AdvReportType').val('').trigger('change');
                $('.divboxASearch #AdvDelFromDate').val('');
                $('.divboxASearch #AdvDelToDate').val('');
                $('#DateFilter').val('').trigger('change');
                $('.divboxASearch #AdvProduct').val('').trigger('change');
                $('.divboxASearch #AdvProductModel').val('').trigger('change');

               
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
                $('.divboxASearch #AdvPreparedBy').val('');
                $('.divboxASearch #AdvAmountFrom').val('');
                $('.divboxASearch #AdvAmountTo').val('');
                $('.divboxASearch #AdvCountryCode').val('');
                $('.divboxASearch #AdvStateCode').val('');
                $('.divboxASearch #AdvDistrictCode').val('');
                $('.divboxASearch #AdvCustomerCategoryCode').val('');
                $('.divboxASearch #AdvReferencePersonCode').val('');
                $('.divboxASearch #AdvApprovalStatusCode').val('');
                $('.divboxASearch #AdvDelFromDate').val('');
                $('.divboxASearch #AdvDelToDate').val('');
                $('.divboxASearch #AdvReportType').val('');
                $('#DateFilter').val('');
                $('.divboxASearch #AdvProduct').val('');
                $('.divboxASearch #AdvProductModel').val('');
               
                break;
            case 'Search':
                if ((SearchTerm == SearchValue) && ($('.divboxASearch #AdvFromDate').val() == "")
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
                    ($('.divboxASearch #AdvReferencePersonCode').val() == "") &&
                    ($('.divboxASearch #AdvApprovalStatusCode').val() == "")
                    && ($('.divboxASearch #AdvDelFromDate').val() == "")
                    && ($('.divboxASearch #AdvDelToDate').val() == "")
                    //&& ($('.divboxASearch #AdvReportType').val() == "")                  
                    && ($('.divboxASearch #AdvProduct').val() == "")
                    && ($('.divboxASearch #AdvProductModel').val() == "")
                    ) {
                    return true;
                }
                break;
            case 'Export':
                DataTablePagingViewModel.Length = -1;
                SaleOrderReportViewModel.DataTablePaging = DataTablePagingViewModel;           
               
                SaleOrderReportViewModel.SearchTerm = $('#SearchTerm').val() == "" ? null : $('#SearchTerm').val();
                SaleOrderReportViewModel.AdvFromDate = $('.divboxASearch #AdvFromDate').val() == "" ? null : $('.divboxASearch #AdvFromDate').val();
                SaleOrderReportViewModel.AdvFromDate = $('#AdvFromDate').val() == "" ? null : $('#AdvFromDate').val();
                SaleOrderReportViewModel.AdvToDate = $('.divboxASearch #AdvToDate').val() == "" ? null : $('.divboxASearch #AdvToDate').val();
                SaleOrderReportViewModel.AdvToDate = $('#AdvToDate').val() == "" ? null : $('#AdvToDate').val();

                SaleOrderReportViewModel.AdvDocumentOwnerID = $('.divboxASearch #AdvDocumentOwnerID').val() == "" ? _emptyGuid : $('.divboxASearch #AdvDocumentOwnerID').val();
                SaleOrderReportViewModel.AdvCustomer = $('.divboxASearch #AdvCustomer').val() == "" ? null : $('.divboxASearch #AdvCustomer').val();
                SaleOrderReportViewModel.AdvAreaCode = $('.divboxASearch #AdvAreaCode').val() == "" ? null : $('.divboxASearch #AdvAreaCode').val();
                SaleOrderReportViewModel.AdvBranchCode = $('.divboxASearch #AdvBranchCode').val() == "" ? null : $('.divboxASearch #AdvBranchCode').val();
                SaleOrderReportViewModel.AdvDocumentStatusCode = $('.divboxASearch #AdvDocumentStatusCode').val() == "" ? null : $('.divboxASearch #AdvDocumentStatusCode').val();
                SaleOrderReportViewModel.AdvPreparedBy = $('.divboxASearch #AdvPreparedBy').val() == "" ? _emptyGuid : $('.divboxASearch #AdvPreparedBy').val();

                SaleOrderReportViewModel.AdvAmountFrom = $('.divboxASearch #AdvAmountFrom').val() == "" ? null : $('.divboxASearch #AdvAmountFrom').val();
                SaleOrderReportViewModel.AdvAmountTo = $('.divboxASearch #AdvAmountTo').val() == "" ? null : $('.divboxASearch #AdvAmountTo').val();
                SaleOrderReportViewModel.AdvCountryCode = $('.divboxASearch #AdvCountryCode').val() == "" ? null : $('.divboxASearch #AdvCountryCode').val();
                SaleOrderReportViewModel.AdvStateCode = $('.divboxASearch #AdvStateCode').val() == "" ? null : $('.divboxASearch #AdvStateCode').val();
                SaleOrderReportViewModel.AdvDistrictCode = $('.divboxASearch #AdvDistrictCode').val() == "" ? null : $('.divboxASearch #AdvDistrictCode').val();
                SaleOrderReportViewModel.AdvCustomerCategoryCode = $('.divboxASearch #AdvCustomerCategoryCode').val() == "" ? null : $('.divboxASearch #AdvCustomerCategoryCode').val();
                SaleOrderReportViewModel.AdvReferencePersonCode = $('.divboxASearch #AdvReferencePersonCode').val() == "" ? null : $('.divboxASearch #AdvReferencePersonCode').val();
                SaleOrderReportViewModel.AdvApprovalStatusCode = $('.divboxASearch #AdvApprovalStatusCode').val() == "" ? null : $('.divboxASearch #AdvApprovalStatusCode').val();
                SaleOrderReportViewModel.AdvDelFromDate = $('.divboxASearch #AdvDelFromDate').val() == "" ? null : $('.divboxASearch #AdvDelFromDate').val();
                SaleOrderReportViewModel.AdvDelToDate = $('.divboxASearch #AdvDelToDate').val() == "" ? null : $('.divboxASearch #AdvDelToDate').val();
                SaleOrderReportViewModel.AdvReportType = $('.divboxASearch #AdvReportType').val() == "" ? null : $('.divboxASearch #AdvReportType').val();
                SaleOrderReportViewModel.DateFilter = $('#DateFilter').val() == "" ? null : $('#DateFilter').val();
                SaleOrderReportViewModel.AdvProduct = $('.divboxASearch #AdvProduct').val() == "" ? _emptyGuid : $('.divboxASearch #AdvProduct').val();
                SaleOrderReportViewModel.AdvProductModel = $('.divboxASearch #AdvProductModel').val() == "" ? _emptyGuid : $('.divboxASearch #AdvProductModel').val();
             
                $('#AdvanceSearch').val(JSON.stringify(SaleOrderReportViewModel));
                $('#FormExcelExport').submit();
                return true;
                break;
            default:
                break;
        }
        SaleOrderReportViewModel.DataTablePaging = DataTablePagingViewModel;
        SaleOrderReportViewModel.SearchTerm = $('#SearchTerm').val();
        SaleOrderReportViewModel.AdvFromDate = $('.divboxASearch #AdvFromDate').val();
        SaleOrderReportViewModel.AdvToDate = $('.divboxASearch #AdvToDate').val();
        SaleOrderReportViewModel.AdvDocumentOwnerID = $('.divboxASearch #AdvDocumentOwnerID').val();
        SaleOrderReportViewModel.AdvCustomer = $('.divboxASearch #AdvCustomer').val();
        SaleOrderReportViewModel.AdvAreaCode = $('.divboxASearch #AdvAreaCode').val();
        SaleOrderReportViewModel.AdvBranchCode = $('.divboxASearch #AdvBranchCode').val();
        SaleOrderReportViewModel.AdvDocumentStatusCode = $('.divboxASearch #AdvDocumentStatusCode').val();
        SaleOrderReportViewModel.AdvPreparedBy = $('.divboxASearch #AdvPreparedBy').val();
        SaleOrderReportViewModel.AdvAmountFrom = $('.divboxASearch #AdvAmountFrom').val();
        SaleOrderReportViewModel.AdvAmountTo = $('.divboxASearch #AdvAmountTo').val();
        SaleOrderReportViewModel.AdvCountryCode = $('.divboxASearch #AdvCountryCode').val();
        SaleOrderReportViewModel.AdvStateCode = $('.divboxASearch #AdvStateCode').val();
        SaleOrderReportViewModel.AdvDistrictCode = $('.divboxASearch #AdvDistrictCode').val();
        SaleOrderReportViewModel.AdvCustomerCategoryCode = $('.divboxASearch #AdvCustomerCategoryCode').val();
        SaleOrderReportViewModel.AdvReferencePersonCode = $('.divboxASearch #AdvReferencePersonCode').val();
        SaleOrderReportViewModel.AdvApprovalStatusCode = $('.divboxASearch #AdvApprovalStatusCode').val();
        SaleOrderReportViewModel.AdvDelFromDate = $('.divboxASearch #AdvDelFromDate').val();
        SaleOrderReportViewModel.AdvDelToDate = $('.divboxASearch #AdvDelToDate').val();
        SaleOrderReportViewModel.AdvReportType = $('.divboxASearch #AdvReportType').val();
        SaleOrderReportViewModel.DateFilter = $('#DateFilter').val();
        SaleOrderReportViewModel.AdvProduct = $('.divboxASearch #AdvProduct').val();
        SaleOrderReportViewModel.AdvProductModel = $('.divboxASearch #AdvProductModel').val();
       

        //apply datatable plugin on sale order standard report table
        _dataTable.SaleOrderReportList = $("#tblSaleOrderReport").DataTable(
        {
            dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
            ordering: false,
            searching: false,
            paging: true,
            lengthChange: false,
            autoWidth:false,
            processing: true,
            language: {
                "processing": "<div class='spinner'><div class='bounce1'></div><div class='bounce2'></div><div class='bounce3'></div></div>"
            },
            serverSide: true,
            ajax: {
                url: "GetSaleOrderStandardReport/",
                data: { "saleOrderReportVM": SaleOrderReportViewModel },
                type: 'POST'
            },
            pageLength: 8,
            autoWidth: false,
            columns: [
                {
                    "data": "SaleOrderNo", render: function (data, type, row) {
                        return "<img src='../Content/images/datePicker.png' height='10px'>" + "&nbsp;" + row.SaleOrderDateFormatted + "</br>" + row.SaleOrderNo;
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
                    "data": "TaxableAmount", render: function (data, type, row) {
                        return formatCurrency(roundoff(row.TaxableAmount))
                    }, "defaultContent": "<i>-</i>"
                },
               {
                     "data": "Amount", render: function (data, type, row) {
                         return formatCurrency(roundoff(row.Amount))
                     }, "defaultContent": "<i>-</i>"
                },
            ],
            columnDefs: [{ className: "text-right", "targets": [7,8] },
                         { className: "text-left", "targets": [1, 3, 4, 6, 5] },
                         { className: "text-center", "targets": [0,] },
                           { "targets": [0], "width": "15%" },
                           { "targets": [1], "width": "15%" },
                           { "targets": [2], "width": "17%" },
                           { "targets": [3], "width": "25%" },
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
                $('#tblSaleOrderReport').fadeIn('slow');
                if (action == undefined) {
                    $('.excelExport').hide();
                    OnServerCallComplete();
                }
                if (json.data[0] != undefined && json.data[0] != null) {
                    $('#lblTotalAmount').text(formatCurrency(roundoff(json.data[0].TotalAmount)));
                    $('#lblTotalTaxableAmount').text(formatCurrency(roundoff(json.data[0].TotalTaxableAmount)));
                }
                else {
                    $('#lblTotalAmount').text("0.00");
                    $('#lblTotalTaxableAmount').text("0.00");
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
    BindOrReloadSaleOrderReportTable('Reset');
}
//function export data to excel
function ExportReportData() {
    debugger;
    //$('.excelExport').show();
    //OnServerCallBegin();       
    BindOrReloadSaleOrderReportTable('Export');
}

function ApplyFilterThenSearch() {
    $(".searchicon").addClass('filterApplied');
    CloseAdvanceSearch();
    BindOrReloadSaleOrderReportTable('Search');
}

function GoToList() {
    window.location.href = "/Report";
}


function DateFilterOnchange() {
    debugger;
    var selectedValue = $("#DateFilter").val().split("||");
    $('.divboxASearch #AdvFromDate').val(selectedValue[0]);
    $('.divboxASearch #AdvToDate').val(selectedValue[1]);
    BindOrReloadSaleOrderReportTable('Search');
}