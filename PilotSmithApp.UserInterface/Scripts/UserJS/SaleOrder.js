var _dataTable = {};
var _emptyGuid = "00000000-0000-0000-0000-000000000000";
var _datatablerowindex = -1;
var _jsonData = {};
var _message = "";
var _status = "";
var _result = "";
//---------------------------------------Docuement Ready--------------------------------------------------//
$(document).ready(function () {
    try {
        BindOrReloadSaleOrderTable('Init');
        $('#tblSaleOrder tbody').on('dblclick', 'td', function () {
            //EditSaleOrder(this);
        });
    }
    catch (e) {
        console.log(e.message);
    }
});

//function bind the SaleOrder list checking search and filter
function BindOrReloadSaleOrderTable(action) {
    try {
        debugger;
        //creating advancesearch object
        SaleOrderAdvanceSearchViewModel = new Object();
        DataTablePagingViewModel = new Object();
        DataTablePagingViewModel.Length = 0;
        //switch case to check the operation
        switch (action) {
            case 'Reset':
                $('#SearchTerm').val('');
                $('#FromDate').val('');
                $('#ToDate').val('');
                break;
            case 'Init':
                $('#SearchTerm').val('');
                $('#FromDate').val('');
                $('#ToDate').val('');
                break;
            case 'Search':
                if (($('#SearchTerm').val() == "") && ($('#FromDate').val() == "") && ($('#ToDate').val() == "")) {
                    return true;
                }
                break;
            case 'Export':
                DataTablePagingViewModel.Length = -1;
                break;
            default:
                break;
        }
        SaleOrderAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
        SaleOrderAdvanceSearchViewModel.SearchTerm = $('#SearchTerm').val();
        SaleOrderAdvanceSearchViewModel.FromDate = $('#FromDate').val();
        SaleOrderAdvanceSearchViewModel.ToDate = $('#ToDate').val();
        //apply datatable plugin on SaleOrder table
        _dataTable.SaleOrderList = $('#tblSaleOrder').DataTable(
        {
            dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
            buttons: [{
                extend: 'excel',
                exportOptions:
                             {
                                 columns: [0, 1, 2, 3, 4, 5]
                             }
            }],
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
                url: "SaleOrder/GetAllSaleOrder/",
                data: { "saleOrderAdvanceSearchVM": SaleOrderAdvanceSearchViewModel },
                type: 'POST'
            },
            pageLength: 13,
            columns: [
               { "data": "SaleOrderNo", "defaultContent": "<i>-</i>" },
               { "data": "SaleOrderDateFormatted", "defaultContent": "<i>-</i>" },
               { "data": "Customer.CompanyName", "defaultContent": "<i>-</i>" },
               { "data": "Branch.Description", "defaultContent": "<i>-</i>" },
               { "data": "DocumentStatus.Description", "defaultContent": "<i>-</i>" },
               //{ "data": "UserName", "defaultContent": "<i>-</i>" },
               {
                   "data": "IsFinalApproved", render: function (data, type, row) {
                       if (data) {
                           return "Approved ✔";// <br/>📅 " + (row.FinalApprovalDateFormatted !== null ? row.FinalApprovalDateFormatted : "-");
                       }
                       else {
                           return 'Pending';
                       }

                   }, "defaultContent": "<i>-</i>"
               },
               { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink"  onclick="EditSaleOrder(this)" ><i class="fa fa-pencil-square-o" aria-hidden="true"></i></a>' },
            ],
            columnDefs: [
                          { className: "text-left", "targets": [0, 2, 3, 4, 5, 6] },
                          { className: "text-center", "targets": [1] },
                            { "targets": [0], "width": "10%" },
                            { "targets": [2], "width": "20%" },
                            { "targets": [1, 3, 4], "width": "10%" },
                            { "targets": [5], "width": "7%" },
                            { "targets": [6], "width": "7%" },

            ],
            destroy: true,
            //for performing the import operation after the data loaded
            initComplete: function (settings, json) {
                debugger;
                $('.dataTables_wrapper div.bottom div').addClass('col-md-6');
                $('#tblSaleOrder').fadeIn('slow');
                if (action == undefined) {
                    $('.excelExport').hide();
                    OnServerCallComplete();
                }
                if (action === 'Export') {
                    if (json.data.length > 0) {
                        if (json.data[0].TotalCount > 1000) {
                            setTimeout(function () {
                                MasterAlert("info", 'We are able to download maximum 1000 rows of data, There exist more than 1000 rows of data please filter and download')
                            }, 10000)
                        }
                    }
                    $(".buttons-excel").trigger('click');
                    BindOrReloadSaleOrderTable();
                }
            }
        });
        $(".buttons-excel").hide();
    }
    catch (e) {
        console.log(e.message);
    }
}

//function reset the list to initial
function ResetSaleOrderList() {
    $(".searchicon").removeClass('filterApplied');
    BindOrReloadSaleOrderTable('Reset');
}
//function export data to excel
function ExportSaleOrderData() {
    debugger;
    $('.excelExport').show();
    OnServerCallBegin();
    BindOrReloadSaleOrderTable('Export');
}
// add SaleOrder section
function AddSaleOrder() {
    debugger;
    //this will return form body(html)
    //OnServerCallBegin();
    $("#divSaleOrderForm").load("SaleOrder/SaleOrderForm?id=" + _emptyGuid + "&saleOrderID=", function () {
        $('#lblSaleOrderInfo').text('<<Sale Order No.>>');
        ChangeButtonPatchView("SaleOrder", "btnPatchSaleOrderNew", "Add");
        BindSaleOrderDetailList(_emptyGuid,false,false);
        //BindSaleOrderOtherChargesDetailList(_emptyGuid)
        // OnServerCallComplete();
        setTimeout(function () {
            //resides in customjs for sliding
            openNav();
        }, 100);
    });
}

function AddSaleOrderDetailList() {
    debugger;
    $("#divModelSaleOrderPopBody").load("SaleOrder/AddSaleOrderDetail", function () {
        $('#lblModelPopSaleOrder').text('SaleOrder Detail')
        $('#divModelPopSaleOrder').modal('show');
    });
}
function EditSaleOrder(this_Obj) {
    debugger;
    OnServerCallBegin();
    var SaleOrder = _dataTable.SaleOrderList.row($(this_Obj).parents('tr')).data();
    //this will return form body(html)
    $("#divSaleOrderForm").load("SaleOrder/SaleOrderForm?id=" + SaleOrder.ID + "&quoteID=" + SaleOrder.QuoteID + "&enquiryID=" + SaleOrder.EnquiryID, function () {
        $('#lblSaleOrderInfo').text(SaleOrder.SaleOrderNo);
        //$('#CustomerID').trigger('change');
        if ($('#IsDocLocked').val() == "True") {
            ChangeButtonPatchView("SaleOrder", "btnPatchSaleOrderNew", "Edit", SaleOrder.ID);
        }
        else {
            ChangeButtonPatchView("SaleOrder", "btnPatchSaleOrderNew", "LockDocument");
        }
        BindSaleOrderDetailList(SaleOrder.ID,false,false);
        $('#divCustomerBasicInfo').load("Customer/CustomerBasicInfo?ID=" + $('#hdnCustomerID').val(), function () {
            $('#MailingAddress').val($('#hdnCustomerBillingAddress').val());
            $('#ShippingAddress').val($('#hdnCustomerShippingAddress').val());
        });
        clearUploadControl();
        PaintImages(SaleOrder.ID);
        OnServerCallComplete();
        setTimeout(function () {
            //$("#divSaleOrderForm #EstimateID").prop('disabled', true);
            //resides in customjs for sliding
            openNav();
        }, 100);
    });
}
function ResetSaleOrder() {
    $("#divSaleOrderForm").load("SaleOrder/SaleOrderForm?id=" + $('#SaleOrderForm #ID').val() + "&estimateID=" + $('#hdnEstimateID').val(), function () {
        if ($('#ID').val() != _emptyGuid && $('#ID').val() != null) {
            setTimeout(function () {
                $("#divSaleOrderForm #EstimateID").prop('disabled', true);
                //resides in customjs for sliding
                openNav();
            }, 100);
        }
        BindSaleOrderDetailList($('#ID').val(), false,false);
        clearUploadControl();
        PaintImages($('#SaleOrderForm #ID').val());
        $('#divCustomerBasicInfo').load("Customer/CustomerBasicInfo?ID=" + $('#SaleOrderForm #hdnCustomerID').val(), function () {
            $('#MailingAddress').val($('#hdnCustomerBillingAddress').val());
            $('#ShippingAddress').val($('#hdnCustomerShippingAddress').val());
        });
    });
}
function SaveSaleOrder() {
    var saleOrderDetailList = _dataTable.SaleOrderDetailList.rows().data().toArray();
    $('#DetailJSON').val(JSON.stringify(saleOrderDetailList));
    $('#btnInsertUpdateSaleOrder').trigger('click');
}
function ApplyFilterThenSearch() {
    $(".searchicon").addClass('filterApplied');
    CloseAdvanceSearch();
    BindOrReloadSaleOrderTable('Search');
}
function SaveSuccessSaleOrder(data, status) {
    try {
        debugger;
        var _jsonData = JSON.parse(data)
        //message field will return error msg only
        _message = _jsonData.Message;
        _status = _jsonData.Status;
        _result = _jsonData.Record;
        switch (_status) {
            case "OK":
                $('#IsUpdate').val('True');
                $("#divSaleOrderForm").load("SaleOrder/SaleOrderForm?id=" + _result.ID + "&estimateID=" + _result.EstimateID, function () {
                    ChangeButtonPatchView("SaleOrder", "btnPatchSaleOrderNew", "Edit");
                    BindSaleOrderDetailList(_result.ID,false,false);
                    clearUploadControl();
                    PaintImages(_result.ID);
                    $('#lblSaleOrderInfo').text(_result.SaleOrderNo);
                    $('#divCustomerBasicInfo').load("Customer/CustomerBasicInfo?ID=" + $('#SaleOrderForm #hdnCustomerID').val());
                });
                ChangeButtonPatchView("SaleOrder", "btnPatchSaleOrderNew", "Edit");
                BindOrReloadSaleOrderTable('Init');
                notyAlert('success', _result.Message);
                break;
            case "ERROR":
                notyAlert('error', _message);
                break;
            default:
                break;
        }
    }
    catch (e) {
        //this will show the error msg in the browser console(F12) 
        console.log(e.message);
    }
}

function DeleteSaleOrder() {
    notyConfirm('Are you sure to delete?', 'DeleteSaleOrderItem("' + $('#SaleOrderForm #ID').val() + '")');
}
function DeleteSaleOrderItem(id) {
    try {
        if (id) {
            var data = { "id": id };
            _jsonData = GetDataFromServer("SaleOrder/DeleteSaleOrder/", data);
            if (_jsonData != '') {
                _jsonData = JSON.parse(_jsonData);
                _message = _jsonData.Message;
                _status = _jsonData.Status;
                _result = _jsonData.Record;
            }
            switch (_status) {
                case "OK":
                    notyAlert('success', _result.Message);
                    $('#ID').val(_emptyGuid);
                    ChangeButtonPatchView("SaleOrder", "btnPatchSaleOrderNew", "Add");
                    ResetSaleOrder();
                    BindOrReloadSaleOrderTable('Init');
                    break;
                case "ERROR":
                    notyAlert('error', _message);
                    break;
                default:
                    break;
            }
        }
    }
    catch (e) {
        //this will show the error msg in the browser console(F12) 
        console.log(e.message);
    }
}
//
function BindSaleOrderOtherChargesDetailList(id) {
    debugger;
    _dataTable.SaleOrderOtherChargesDetailList = $('#tblSaleOrderOtherChargesDetailList').DataTable(
         {
             dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
             order: [],
             searching: false,
             paging: false,
             ordering: false,
             bInfo: false,
             data: id == _emptyGuid ? null : GetSaleOrderOtherChargesDetailListBySaleOrderID(id),
             language: {
                 search: "_INPUT_",
                 searchPlaceholder: "Search"
             },
             columns: [
             { "data": "OtherCharge.Description", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
             { "data": "Charge Amount", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
             {
                 "data": "CGSTAmt", render: function (data, type, row) {
                     debugger;
                     return ""
                     //var CGST = parseFloat(row.TaxType.ValueText != "" ? row.TaxType.ValueText.split('|')[1].split(',')[0].split('-')[1] : 0);
                     //var SGST = parseFloat(row.TaxType.ValueText != "" ? row.TaxType.ValueText.split('|')[1].split(',')[1].split('-')[1] : 0);
                     //var IGST = parseFloat(row.TaxType.ValueText != "" ? row.TaxType.ValueText.split('|')[1].split(',')[2].split('-')[1] : 0);
                     //var GSTAmt = roundoff(parseFloat(data) + parseFloat(row.SGSTAmt) + parseFloat(row.IGSTAmt))
                     //return '<div class="show-popover text-right" data-html="true" data-toggle="popover" data-title="<p align=left>Total GST : ₹ ' + GSTAmt + '" data-content=" SGST ' + SGST + '% : ₹ ' + roundoff(parseFloat(row.SGSTAmt)) + '<br/>CGST ' + CGST + '% : ₹ ' + roundoff(parseFloat(data)) + '<br/> IGST ' + IGST + '% : ₹ ' + roundoff(parseFloat(row.IGSTAmt)) + '</p>"/>' + GSTAmt
                 }, "defaultContent": "<i></i>"
             },
             {
                 "data": "Charge Amount", render: function (data, type, row) {
                     return ""
                     //var TaxableAmt = roundoff((parseFloat(row.Rate != "" ? row.Rate : 0) * parseInt(row.Qty != "" ? row.Qty : 1)) - parseFloat(row.Discount != "" ? row.Discount : 0))
                     //var GSTAmt = roundoff(parseFloat(row.CGSTAmt) + parseFloat(row.SGSTAmt) + parseFloat(row.IGSTAmt))
                     //var GrandTotal = roundoff(((parseFloat(row.Rate != "" ? row.Rate : 0) * parseInt(row.Qty != "" ? row.Qty : 1)) - parseFloat(row.Discount != "" ? row.Discount : 0)) + parseFloat(row.SGSTAmt) + parseFloat(row.IGSTAmt) + parseFloat(row.CGSTAmt))
                     //return '<div class="show-popover text-right" data-html="true" data-toggle="popover" data-title="<p align=left>Grand Total : ₹ ' + GrandTotal + '" data-content="Taxable : ₹ ' + TaxableAmt + '<br/>GST : ₹ ' + GSTAmt + '</p>"/>' + GrandTotal
                 }, "defaultContent": "<i></i>"
             },
             { "data": null, "orderable": false, "defaultContent": '<a href="#" class="DeleteLink"  onclick="ConfirmDeleteSaleOrderDetail(this)" ><i class="fa fa-trash-o" aria-hidden="true"></i></a> <a href="#" class="actionLink"  onclick="EditSaleOrderDetail(this)" ><i class="fa fa-pencil-square-o" aria-hidden="true"></i></a>' },
             ],
             columnDefs: [
                 { "targets": [0], "width": "20%" },
                 { "targets": [1, 2], "width": "20%" },
                 { "targets": [3], "width": "20%" },
                 { className: "text-left", "targets": [3] },
                 { className: "text-center", "targets": [0, 1, 2] }
             ],
             destroy: true,
         });
}
function BindSaleOrderDetailList(id, IsEnquiry, IsQuotation) {
    debugger;
    ClearCalculatedFields();
    var data;
    if (id == _emptyGuid && !(IsEnquiry) && !(IsQuotation)) {
        data = null;
    }
    else if (id == _emptyGuid && (IsEnquiry)) {
        data = GetSaleOrderDetailListBySaleOrderID(id, IsEnquiry, IsQuotation)
    }
    else if (id == _emptyGuid && (IsQuotation)) {
        data = GetSaleOrderDetailListBySaleOrderID(id, IsEnquiry, IsQuotation)
    }
    else {
        data = GetSaleOrderDetailListBySaleOrderID(id, IsEnquiry, IsQuotation)
    }
    _dataTable.SaleOrderDetailList = $('#tblSaleOrderDetails').DataTable(
         {
             dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
             order: [],
             searching: false,
             paging: false,
             ordering: false,
             bInfo: false,
             data: data,
             language: {
                 search: "_INPUT_",
                 searchPlaceholder: "Search"
             },
             columns: [
             {
                 "data": "Product.Code", render: function (data, type, row) {
                     debugger;
                     return '<div style="width:100%" class="show-popover" data-html="true" data-toggle="popover" data-title="<p align=left>Product Specification" data-content="' + row.ProductSpec.replace(/"/g, "&quot") + '</p>"/>' + row.Product.Name + "<br/>" + row.ProductModel.Name
                 }, "defaultContent": "<i></i>"
             },
             {
                 "data": "Qty", render: function (data, type, row) {
                     return data + " " + row.Unit.Description
                 }, "defaultContent": "<i></i>"
             },
             { "data": "Rate", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
             { "data": "Discount", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
             {
                 "data": "Rate", render: function (data, type, row) {
                     var Total = roundoff(parseFloat(data != "" ? data : 0) * parseInt(row.Qty != "" ? row.Qty : 1))
                     var Discount = roundoff(parseFloat(row.Discount != "" ? row.Discount : 0))
                     var Taxable = Total - Discount
                     return '<div class="show-popover text-right" data-html="true" data-toggle="popover" data-title="<p align=left>Taxable : ₹ ' + Taxable + '" data-content="Net Total : ₹ ' + Total + '<br/> Discount : ₹ -' + Discount + '</p>"/>' + Taxable
                 }, "defaultContent": "<i></i>"
             },
             {
                 "data": "Rate", render: function (data, type, row) {
                     debugger;
                     var CGST = parseFloat(row.CGSTPerc != "" ? row.CGSTPerc : 0);
                     var SGST = parseFloat(row.SGSTPerc != "" ? row.SGSTPerc : 0);
                     var IGST = parseFloat(row.IGSTPerc != "" ? row.IGSTPerc : 0);
                     var Total = roundoff(parseFloat(data != "" ? data : 0) * parseInt(row.Qty != "" ? row.Qty : 1))
                     var Discount = roundoff(parseFloat(row.Discount != "" ? row.Discount : 0))
                     var Taxable = Total - Discount
                     var CGSTAmt = parseFloat(Taxable * CGST / 100);
                     var SGSTAmt = parseFloat(Taxable * SGST / 100)
                     var IGSTAmt = parseFloat(Taxable * IGST / 100)
                     var GSTAmt = roundoff(CGSTAmt + SGSTAmt + IGSTAmt)
                     return '<div class="show-popover text-right" data-html="true" data-toggle="popover" data-title="<p align=left>Total GST : ₹ ' + GSTAmt + '" data-content=" SGST ' + SGST + '% : ₹ ' + roundoff(SGSTAmt) + '<br/>CGST ' + CGST + '% : ₹ ' + roundoff(parseFloat(CGSTAmt)) + '<br/> IGST ' + IGST + '% : ₹ ' + roundoff(parseFloat(IGSTAmt)) + '</p>"/>' + GSTAmt
                 }, "defaultContent": "<i></i>"
             },
             {
                 "data": "CessAmt", render: function (data, type, row) {
                     debugger;
                     return '<i style="font-size:10px;color:brown">Cess(%) -</i>' + row.CessPerc + '<br/><i style="font-size:10px;color:brown">Cess(₹) -</i>' + data
                 }, "defaultContent": "<i></i>"
             },
             {
                 "data": "Rate", render: function (data, type, row) {
                     var CGST = parseFloat(row.CGSTPerc != "" ? row.CGSTPerc : 0);
                     var SGST = parseFloat(row.SGSTPerc != "" ? row.SGSTPerc : 0);
                     var IGST = parseFloat(row.IGSTPerc != "" ? row.IGSTPerc : 0);
                     var TaxableAmt = roundoff((parseFloat(row.Rate != "" ? row.Rate : 0) * parseInt(row.Qty != "" ? row.Qty : 1)) - parseFloat(row.Discount != "" ? row.Discount : 0))
                     var CGSTAmt = parseFloat(TaxableAmt * CGST / 100);
                     var SGSTAmt = parseFloat(TaxableAmt * SGST / 100)
                     var IGSTAmt = parseFloat(TaxableAmt * IGST / 100)
                     var GSTAmt = roundoff(CGSTAmt + SGSTAmt + IGSTAmt)
                     var GrandTotal = roundoff(((parseFloat(row.Rate != "" ? row.Rate : 0) * parseInt(row.Qty != "" ? row.Qty : 1)) - parseFloat(row.Discount != "" ? row.Discount : 0)) + parseFloat(GSTAmt) + parseFloat(row.CessAmt))
                     return '<div class="show-popover text-right" data-html="true" data-toggle="popover" data-title="<p align=left>Grand Total : ₹ ' + GrandTotal + '" data-content="Taxable : ₹ ' + TaxableAmt + '<br/>GST : ₹ ' + GSTAmt + '</p>"/>' + GrandTotal
                 }, "defaultContent": "<i></i>"
             },
             { "data": null, "orderable": false, "defaultContent": '<a href="#" class="DeleteLink"  onclick="ConfirmDeleteSaleOrderDetail(this)" ><i class="fa fa-trash-o" aria-hidden="true"></i></a> <a href="#" class="actionLink"  onclick="EditSaleOrderDetail(this)" ><i class="fa fa-pencil-square-o" aria-hidden="true"></i></a>' },
             ],
             columnDefs: [
                 { "targets": [0], "width": "35%" },
                 { "targets": [2, 4, 5, 6, 7], "width": "10%" },
                 { "targets": [1, 3, 8], "width": "5%" },
                 { className: "text-right", "targets": [2, 3, 4, 5, 6, 7] },
                 { className: "text-left", "targets": [0] },
                 { className: "text-center", "targets": [1, 8] }
             ],
             rowCallback: function (row, data, index) {
                 debugger;
                 var TaxableAmt = (parseFloat(data.Rate != "" ? data.Rate : 0) * parseInt(data.Qty != "" ? data.Qty : 1)) - parseFloat(data.Discount != "" ? data.Discount : 0)
                 var CGST = parseFloat(data.CGSTPerc != "" ? data.CGSTPerc : 0);
                 var SGST = parseFloat(data.SGSTPerc != "" ? data.SGSTPerc : 0);
                 var IGST = parseFloat(data.IGSTPerc != "" ? data.IGSTPerc : 0);
                 var CGSTAmt = parseFloat(TaxableAmt * CGST / 100);
                 var SGSTAmt = parseFloat(TaxableAmt * SGST / 100);
                 var IGSTAmt = parseFloat(TaxableAmt * IGST / 100);
                 var GSTAmt = parseFloat(CGSTAmt) + parseFloat(SGSTAmt) + parseFloat(IGSTAmt)
                 var GrossTotalAmt = TaxableAmt + GSTAmt + parseFloat(data.CessAmt)
                 var CessAmt = roundoff(parseFloat($('#lblCessAmount').text()) + parseFloat(data.CessAmt))
                 var TaxTotal = roundoff(parseFloat($('#lblTaxTotal').text()) + GSTAmt)
                 var TaxableTotal = roundoff(parseFloat($('#lblItemTotal').text()) + TaxableAmt)
                 var GrossAmount = roundoff(parseFloat($('#lblGrossAmount').text()) + GrossTotalAmt)
                 var GrandTotal = roundoff(parseFloat($('#lblGrandTotal').text()) + GrossTotalAmt)

                 $('#lblTaxTotal').text(TaxTotal);
                 $('#lblItemTotal').text(TaxableTotal);
                 $('#lblGrossAmount').text(GrossAmount);
                 $('#lblCessAmount').text(CessAmt);
                 $('#lblGrandTotal').text(GrandTotal);
             },
             initComplete: function (settings, json) {
                 $('#SaleOrderForm #Discount').trigger('change');
             },
             destroy: true
         });
    $('[data-toggle="popover"]').popover({
        html: true,
        'trigger': 'hover',
        'placement': 'left'
    });
}
function GetSaleOrderDetailListBySaleOrderID(id, IsEnquiry, IsQuotation) {
    try {
        debugger;

        var saleOrderDetailList = [];
        if (IsEnquiry) {
            var data = { "enquiryID": $('#SaleOrderForm #hdnEnquiryID').val() };
            _jsonData = GetDataFromServer("SaleOrder/GetSaleOrderDetailListByEnquiryIDFromEnquiry/", data);
        }
        else if (IsQuotation)
        {
            var data = { "quoteID": $('#SaleOrderForm #hdnQuoteID').val() };
            _jsonData = GetDataFromServer("SaleOrder/GetSaleOrderDetailListByQuotationIDFromQuotation/", data);
        }
        else {
            var data = { "saleOrderID": id };
            _jsonData = GetDataFromServer("SaleOrder/GetSaleOrderDetailListBySaleOrderID/", data);
        }

        if (_jsonData != '') {
            _jsonData = JSON.parse(_jsonData);
            _message = _jsonData.Message;
            _status = _jsonData.Status;
            saleOrderDetailList = _jsonData.Records;
        }
        if (_status == "OK") {
            return saleOrderDetailList;
        }
        if (_status == "ERROR") {
            notyAlert('error', _message);
        }
    }
    catch (e) {
        //this will show the error msg in the browser console(F12) 
        console.log(e.message);

    }
}
function AddSaleOrderDetailList() {
    $("#divModelSaleOrderPopBody").load("SaleOrder/AddSaleOrderDetail", function () {
        $('#lblModelPopSaleOrder').text('SaleOrder Detail')
        $('#divModelPopSaleOrder').modal('show');
    });
}
function AddSaleOrderDetailToList() {
    debugger;
    $("#FormSaleOrderDetail").submit(function () { });
    debugger;
    if ($('#FormSaleOrderDetail #IsUpdate').val() == 'True') {
        if (($('#divModelSaleOrderPopBody #ProductID').val() != "") && ($('#divModelSaleOrderPopBody #Rate').val() != "") && ($('#divModelSaleOrderPopBody #Qty').val() != "") && ($('#divModelSaleOrderPopBody #UnitCode').val() != "")) {
            debugger;
            var saleOrderDetailList = _dataTable.SaleOrderDetailList.rows().data();
            saleOrderDetailList[_datatablerowindex].Product.Code = $("#divModelSaleOrderPopBody #ProductID").val() != "" ? $("#divModelSaleOrderPopBody #ProductID option:selected").text().split("-")[0].trim() : "";
            saleOrderDetailList[_datatablerowindex].Product.Name = $("#divModelSaleOrderPopBody #ProductID").val() != "" ? $("#divModelSaleOrderPopBody #ProductID option:selected").text().split("-")[1].trim() : "";
            saleOrderDetailList[_datatablerowindex].ProductID = $("#divModelPopSaleOrder #ProductID").val() != "" ? $("#divModelPopSaleOrder #ProductID").val() : _emptyGuid;
            saleOrderDetailList[_datatablerowindex].ProductModelID = $("#divModelSaleOrderPopBody #ProductModelID").val() != "" ? $("#divModelSaleOrderPopBody #ProductModelID").val() : _emptyGuid;
            ProductModel = new Object;
            Unit = new Object;
            TaxType = new Object;
            ProductModel.Name = $("#divModelSaleOrderPopBody #ProductModelID").val() != "" ? $("#divModelSaleOrderPopBody #ProductModelID option:selected").text() : "";
            saleOrderDetailList[_datatablerowindex].ProductModel = ProductModel;
            saleOrderDetailList[_datatablerowindex].ProductSpec = $('#divModelSaleOrderPopBody #ProductSpec').val();
            saleOrderDetailList[_datatablerowindex].Qty = $('#divModelSaleOrderPopBody #Qty').val();
            saleOrderDetailList[_datatablerowindex].UnitCode = $('#divModelSaleOrderPopBody #UnitCode').val();
            Unit.Description = $("#divModelSaleOrderPopBody #UnitCode").val() != "" ? $("#divModelSaleOrderPopBody #UnitCode option:selected").text().trim() : "";
            saleOrderDetailList[_datatablerowindex].Unit = Unit;
            saleOrderDetailList[_datatablerowindex].Rate = $('#divModelSaleOrderPopBody #Rate').val();
            saleOrderDetailList[_datatablerowindex].Discount = $('#divModelSaleOrderPopBody #Discount').val() != "" ? $('#divModelSaleOrderPopBody #Discount').val() : 0;
            saleOrderDetailList[_datatablerowindex].TaxTypeCode = $('#divModelSaleOrderPopBody #TaxTypeCode').val().split('|')[0];
            TaxType.ValueText = $('#divModelSaleOrderPopBody #TaxTypeCode').val();
            saleOrderDetailList[_datatablerowindex].TaxType = TaxType;
            saleOrderDetailList[_datatablerowindex].CGSTPerc = $('#divModelSaleOrderPopBody #hdnCGSTPerc').val();
            saleOrderDetailList[_datatablerowindex].SGSTPerc = $('#divModelSaleOrderPopBody #hdnSGSTPerc').val();
            saleOrderDetailList[_datatablerowindex].IGSTPerc = $('#divModelSaleOrderPopBody #hdnIGSTPerc').val();
            saleOrderDetailList[_datatablerowindex].CessPerc = $('#divModelSaleOrderPopBody #CessPerc').val();
            saleOrderDetailList[_datatablerowindex].CessAmt = $('#divModelSaleOrderPopBody #CessAmt').val();
            ClearCalculatedFields();
            _dataTable.SaleOrderDetailList.clear().rows.add(saleOrderDetailList).draw(false);
            $('#divModelPopSaleOrder').modal('hide');
            _datatablerowindex = -1;
        }
    }
    else {
        if (($('#divModelSaleOrderPopBody #ProductID').val() != "") && ($('#divModelSaleOrderPopBody #Rate').val() != "") && ($('#divModelSaleOrderPopBody #Qty').val() != "") && ($('#divModelSaleOrderPopBody #UnitCode').val() != "")) {
            debugger;
            if (_dataTable.SaleOrderDetailList.rows().data().length === 0) {
                _dataTable.SaleOrderDetailList.clear().rows.add(GetSaleOrderDetailListBySaleOrderID(_emptyGuid, false)).draw(false);
                debugger;
                var saleOrderDetailList = _dataTable.SaleOrderDetailList.rows().data();
                saleOrderDetailList[0].Product.Code = $("#divModelSaleOrderPopBody #ProductID").val() != "" ? $("#divModelSaleOrderPopBody #ProductID option:selected").text().split("-")[0].trim() : "";
                saleOrderDetailList[0].Product.Name = $("#divModelSaleOrderPopBody #ProductID").val() != "" ? $("#divModelSaleOrderPopBody #ProductID option:selected").text().split("-")[1].trim() : "";
                saleOrderDetailList[0].ProductID = $("#divModelSaleOrderPopBody #ProductID").val() != "" ? $("#divModelSaleOrderPopBody #ProductID").val() : _emptyGuid;
                saleOrderDetailList[0].ProductModelID = $("#divModelSaleOrderPopBody #ProductModelID").val() != "" ? $("#divModelSaleOrderPopBody #ProductModelID").val() : _emptyGuid;
                saleOrderDetailList[0].ProductModel.Name = $("#divModelSaleOrderPopBody #ProductModelID").val() != "" ? $("#divModelSaleOrderPopBody #ProductModelID option:selected").text() : "";
                saleOrderDetailList[0].ProductSpec = $('#divModelSaleOrderPopBody #ProductSpec').val();
                saleOrderDetailList[0].Qty = $('#divModelSaleOrderPopBody #Qty').val();
                saleOrderDetailList[0].UnitCode = $('#divModelSaleOrderPopBody #UnitCode').val();
                saleOrderDetailList[0].Unit.Description = $("#divModelSaleOrderPopBody #UnitCode").val() != "" ? $("#divModelSaleOrderPopBody #UnitCode option:selected").text().trim() : "";
                saleOrderDetailList[0].Rate = $('#divModelSaleOrderPopBody #Rate').val();
                saleOrderDetailList[0].Discount = $('#divModelSaleOrderPopBody #Discount').val() != "" ? $('#divModelSaleOrderPopBody #Discount').val() : 0;
                saleOrderDetailList[0].TaxTypeCode = $('#divModelSaleOrderPopBody #TaxTypeCode').val().split('|')[0];
                saleOrderDetailList[0].TaxType.ValueText = $('#divModelSaleOrderPopBody #TaxTypeCode').val();
                saleOrderDetailList[0].CGSTPerc = $('#divModelSaleOrderPopBody #hdnCGSTPerc').val();
                saleOrderDetailList[0].SGSTPerc = $('#divModelSaleOrderPopBody #hdnSGSTPerc').val();
                saleOrderDetailList[0].IGSTPerc = $('#divModelSaleOrderPopBody #hdnIGSTPerc').val();
                saleOrderDetailList[0].CessPerc = $('#divModelSaleOrderPopBody #CessPerc').val();
                saleOrderDetailList[0].CessAmt = $('#divModelSaleOrderPopBody #CessAmt').val();
                ClearCalculatedFields();
                _dataTable.SaleOrderDetailList.clear().rows.add(saleOrderDetailList).draw(false);
                $('#divModelPopSaleOrder').modal('hide');               
            }
            else {
                var saleOrderDetailList = _dataTable.SaleOrderDetailList.rows().data();
                if (saleOrderDetailList.length > 0) {
                    var checkpoint = 0;
                    var productSpec = $('#ProductSpec').val();
                    productSpec = productSpec.replace(/\n/g, ' ');
                    for (var i = 0; i < saleOrderDetailList.length; i++) {
                        if ((saleOrderDetailList[i].ProductID == $('#ProductID').val()) && (saleOrderDetailList[i].ProductModelID == $('#ProductModelID').val()
                            && (saleOrderDetailList[i].ProductSpec.replace(/\n/g, ' ') == productSpec && (saleOrderDetailList[i].UnitCode == $('#UnitCode').val())))) {
                            saleOrderDetailList[i].Qty = parseInt(saleOrderDetailList[i].Qty) + parseInt($('#Qty').val());
                            checkpoint = 1;
                            break;
                        }
                    }
                    if (checkpoint == 1) {
                        debugger;
                        ClearCalculatedFields();
                        _dataTable.SaleOrderDetailList.clear().rows.add(saleOrderDetailList).draw(false);
                        $('#divModelPopSaleOrder').modal('hide');
                    }
                    else if (checkpoint == 0) {
                        ClearCalculatedFields();
                        var SaleOrderDetailVM = new Object();
                        SaleOrderDetailVM.ID = _emptyGuid;
                        SaleOrderDetailVM.ProductID = ($("#divModelSaleOrderPopBody #ProductID").val() != "" ? $("#divModelSaleOrderPopBody #ProductID").val() : _emptyGuid);
                        var Product = new Object;
                        Product.Code = ($("#divModelSaleOrderPopBody #ProductID").val() != "" ? $("#divModelSaleOrderPopBody #ProductID option:selected").text().split("-")[0].trim() : "");
                        Product.Name = ($("#divModelSaleOrderPopBody #ProductID").val() != "" ? $("#divModelSaleOrderPopBody #ProductID option:selected").text().split("-")[1].trim() : "");
                        SaleOrderDetailVM.Product = Product;
                        SaleOrderDetailVM.ProductModelID = ($("#divModelSaleOrderPopBody #ProductModelID").val() != "" ? $("#divModelSaleOrderPopBody #ProductModelID").val() : _emptyGuid);
                        var ProductModel = new Object()
                        ProductModel.Name = ($("#ProductModelID").val() != "" ? $("#ProductModelID option:selected").text() : "");
                        SaleOrderDetailVM.ProductModel = ProductModel;
                        SaleOrderDetailVM.ProductSpec = $('#divModelSaleOrderPopBody #ProductSpec').val();
                        SaleOrderDetailVM.Qty = $('#divModelSaleOrderPopBody #Qty').val();
                        var Unit = new Object();
                        Unit.Description = $("#divModelSaleOrderPopBody #UnitCode").val() != "" ? $("#divModelSaleOrderPopBody #UnitCode option:selected").text().trim() : "";
                        SaleOrderDetailVM.Unit = Unit;
                        SaleOrderDetailVM.UnitCode = $('#divModelSaleOrderPopBody #UnitCode').val();
                        SaleOrderDetailVM.Rate = $('#divModelSaleOrderPopBody #Rate').val();
                        SaleOrderDetailVM.Discount = $('#divModelSaleOrderPopBody #Discount').val() != "" ? $('#divModelSaleOrderPopBody #Discount').val() : 0;
                        SaleOrderDetailVM.TaxTypeCode = $('#divModelSaleOrderPopBody #TaxTypeCode').val().split('|')[0];
                        var TaxType = new Object();
                        TaxType.ValueText = $('#divModelSaleOrderPopBody #TaxTypeCode').val();
                        SaleOrderDetailVM.TaxType = TaxType;
                        SaleOrderDetailVM.CGSTPerc = $('#divModelSaleOrderPopBody #hdnCGSTPerc').val();
                        SaleOrderDetailVM.SGSTPerc = $('#divModelSaleOrderPopBody #hdnSGSTPerc').val();
                        SaleOrderDetailVM.IGSTPerc = $('#divModelSaleOrderPopBody #hdnIGSTPerc').val();
                        SaleOrderDetailVM.CessPerc = $('#divModelSaleOrderPopBody #CessPerc').val();
                        SaleOrderDetailVM.CessAmt = $('#divModelSaleOrderPopBody #CessAmt').val();
                        _dataTable.SaleOrderDetailList.row.add(SaleOrderDetailVM).draw(true);
                        $('#divModelPopSaleOrder').modal('hide');
                    }
                }
            }
        }
    }
    $('[data-toggle="popover"]').popover({
        html: true,
        'trigger': 'hover',
        'placement': 'left'
    });
}
function EditSaleOrderDetail(this_Obj) {
    debugger;
    _datatablerowindex = _dataTable.SaleOrderDetailList.row($(this_Obj).parents('tr')).index();
    var saleOrderDetail = _dataTable.SaleOrderDetailList.row($(this_Obj).parents('tr')).data();
    $("#divModelSaleOrderPopBody").load("SaleOrder/AddSaleOrderDetail", function () {
        debugger;
        $('#lblModelPopSaleOrder').text('SaleOrder Detail')
        $('#FormSaleOrderDetail #IsUpdate').val('True');
        $('#FormSaleOrderDetail #ID').val(saleOrderDetail.ID);
        $("#FormSaleOrderDetail #ProductID").val(saleOrderDetail.ProductID)
        $("#FormSaleOrderDetail #hdnProductID").val(saleOrderDetail.ProductID)
        $('#divProductBasicInfo').load("Product/ProductBasicInfo?ID=" + $('#hdnProductID').val(), function () {
        });

        if ($('#hdnProductID').val() != _emptyGuid) {
            $('.divProductModelSelectList').load("ProductModel/ProductModelSelectList?required=required&productID=" + $('#hdnProductID').val())
        }
        else {
            $('.divProductModelSelectList').empty();
            $('.divProductModelSelectList').append('<span class="form-control newinput"><i id="dropLoad" class="fa fa-spinner"></i></span>');
        }
        $("#FormSaleOrderDetail #ProductModelID").val(saleOrderDetail.ProductModelID);
        $("#FormSaleOrderDetail #hdnProductModelID").val(saleOrderDetail.ProductModelID);
        if ($('#hdnProductModelID').val() != _emptyGuid) {
            $('#divProductBasicInfo').load("ProductModel/ProductModelBasicInfo?ID=" + $('#hdnProductModelID').val(), function () {
            });
        }
        $('#FormSaleOrderDetail #ProductSpec').val(saleOrderDetail.ProductSpec);
        $('#FormSaleOrderDetail #Qty').val(saleOrderDetail.Qty);
        $('#FormSaleOrderDetail #UnitCode').val(saleOrderDetail.UnitCode);
        $('#FormSaleOrderDetail #hdnUnitCode').val(saleOrderDetail.UnitCode);
        $('#FormSaleOrderDetail #Rate').val(saleOrderDetail.Rate);
        $('#FormSaleOrderDetail #Discount').val(saleOrderDetail.Discount);
        $('#FormSaleOrderDetail #TaxTypeCode').val(saleOrderDetail.TaxType.Code);
        $('#FormSaleOrderDetail #hdnTaxTypeCode').val(saleOrderDetail.TaxType.ValueText);
        $('#FormSaleOrderDetail #hdnCGSTPerc').val(saleOrderDetail.CGSTPerc);
        $('#FormSaleOrderDetail #hdnSGSTPerc').val(saleOrderDetail.SGSTPerc);
        $('#FormSaleOrderDetail #hdnIGSTPerc').val(saleOrderDetail.IGSTPerc);
        var TaxableAmt = ((parseFloat(saleOrderDetail.Rate) * parseInt(saleOrderDetail.Qty)) - parseFloat(saleOrderDetail.Discount))
        var CGSTAmt = (TaxableAmt * parseFloat(saleOrderDetail.CGSTPerc)) / 100;
        var SGSTAmt = (TaxableAmt * parseFloat(saleOrderDetail.SGSTPerc)) / 100;
        var IGSTAmt = (TaxableAmt * parseFloat(saleOrderDetail.IGSTPerc)) / 100;
        $('#FormSaleOrderDetail #CGSTPerc').val(CGSTAmt);
        $('#FormSaleOrderDetail #SGSTPerc').val(SGSTAmt);
        $('#FormSaleOrderDetail #IGSTPerc').val(IGSTAmt);
        $('#FormSaleOrderDetail #CessPerc').val(saleOrderDetail.CessPerc);
        $('#FormSaleOrderDetail #CessAmt').val(saleOrderDetail.CessAmt);
        $('#divModelPopSaleOrder').modal('show');
    });
}
function ConfirmDeleteSaleOrderDetail(this_Obj) {
    debugger;
    _datatablerowindex = _dataTable.SaleOrderDetailList.row($(this_Obj).parents('tr')).index();
    var saleOrderDetail = _dataTable.SaleOrderDetailList.row($(this_Obj).parents('tr')).data();
    if (saleOrderDetail.ID === _emptyGuid) {
        var saleOrderDetailList = _dataTable.SaleOrderDetailList.rows().data();
        saleOrderDetailList.splice(_datatablerowindex, 1);
        ClearCalculatedFields();
        _dataTable.SaleOrderDetailList.clear().rows.add(saleOrderDetailList).draw(false);
        notyAlert('success', 'Detail Row deleted successfully');
    }
    else {
        notyConfirm('Are you sure to delete?', 'DeleteSaleOrderDetail("' + saleOrderDetail.ID + '")');

    }
}
function DeleteSaleOrderDetail(ID) {
    if (ID != _emptyGuid && ID != null && ID != '') {
        var data = { "id": ID };
        var ds = {};
        _jsonData = GetDataFromServer("SaleOrder/DeleteSaleOrderDetail/", data);
        if (_jsonData != '') {
            _jsonData = JSON.parse(_jsonData);
            _message = _jsonData.Message;
            _status = _jsonData.Status;
            _result = _jsonData.Record;
        }
        if (_status == "OK") {
            notyAlert('success', _result.Message);
            var saleOrderDetailList = _dataTable.SaleOrderDetailList.rows().data();
            saleOrderDetailList.splice(_datatablerowindex, 1);
            ClearCalculatedFields();
            _dataTable.SaleOrderDetailList.clear().rows.add(saleOrderDetailList).draw(false);
        }
        if (_status == "ERROR") {
            notyAlert('error', _message);
        }
    }
}
function CalculateGrandTotal(value) {
    var GrandTotal = roundoff(parseFloat($('#lblGrossAmount').text()) - parseFloat(value != "" ? value : 0))
    $('#lblGrandTotal').text(GrandTotal);
}
//=========================================================================================================================
//Email SaleOrder
//
function EmailSaleOrder() {
    $("#divModelEmailSaleOrderBody").load("SaleOrder/EmailSaleOrder?ID=" + $('#SaleOrderForm #ID').val() + "&EmailFlag=True", function () {
        $('#lblModelEmailSaleOrder').text('Email SaleOrder')
        $('#divModelEmailSaleOrder').modal('show');
    });
}
function SendSaleOrderEmail() {
    $('#hdnSaleOrderEMailContent').val($('#divSaleOrderEmailcontainer').html());
    $('#FormSaleOrderEmailSend #ID').val($('#SaleOrderForm #ID').val());
}
function UpdateSaleOrderEmailInfo() {
    $('#hdnMailBodyHeader').val($('#MailBodyHeader').val());
    $('#hdnMailBodyFooter').val($('#MailBodyFooter').val());
    $('#FormUpdateSaleOrderEmailInfo #ID').val($('#SaleOrderForm #ID').val());
}
function DownloadSaleOrder() {
    var bodyContent = $('#divSaleOrderEmailcontainer').html();
    var headerContent = $('#hdnHeadContent').html();
    $('#hdnContent').val(bodyContent);
    $('#hdnHeadContent').val(headerContent);
    var customerName = $("#SaleOrderForm #CustomerID option:selected").text();
    $('#hdnCustomerName').val(customerName);
}

function SaveSuccessUpdateSaleOrderEmailInfo(data, status) {
    try {
        debugger;
        var _jsonData = JSON.parse(data)
        //message field will return error msg only
        _message = _jsonData.Message;
        _status = _jsonData.Status;
        _result = _jsonData.Record;
        switch (_status) {
            case "OK":
                MasterAlert("success", _result.Message)
                $("#divModelEmailSaleOrderBody").load("SaleOrder/EmailSaleOrder?ID=" + $('#SaleOrderForm #ID').val() + "&EmailFlag=False", function () {
                    $('#lblModelEmailSaleOrder').text('Send Email SaleOrder')
                });
                break;
            case "ERROR":
                MasterAlert("success", _message)
                $('#divModelEmailSaleOrder').modal('hide');
                break;
            default:
                break;
        }
    }
    catch (e) {
        //this will show the error msg in the browser console(F12) 
        console.log(e.message);
    }
}
function SaveSuccessSaleOrderEmailSend(data, status) {
    try {
        debugger;
        var _jsonData = JSON.parse(data)
        //message field will return error msg only
        _message = _jsonData.Message;
        _status = _jsonData.Status;
        _result = _jsonData.Record;
        switch (_status) {
            case "OK":
                MasterAlert("success", _message)
                $('#divModelEmailSaleOrder').modal('hide');
                break;
            case "ERROR":
                MasterAlert("success", _message)
                $('#divModelEmailSaleOrder').modal('hide');
                break;
            default:
                break;
        }
    }
    catch (e) {
        //this will show the error msg in the browser console(F12) 
        console.log(e.message);
    }
}
function ShowSendForApproval(documentTypeCode) {
    debugger;
    $("#SendApprovalModalBody").load("DocumentApproval/GetApprovers?documentTypeCode=QUO", function () {
        if ($('#LatestApprovalStatus').val() == 3) {
            var documentID = $('#SaleOrderForm #ID').val();
            var latestApprovalID = $('SaleOrderForm #LatestApprovalID').val();
            ReSendDocForApproval(documentID, documentTypeCode, latestApprovalID);
            SendForApproval('QUO')
            //BindPurchaseOrder($('#ID').val());

        }
        else {
            $('#SendApprovalModal').modal('show');
        }
    });
}

function SendForApproval(documentTypeCode) {
    debugger;

    var documentID = $('#SaleOrderForm #ID').val();
    var approversCSV;
    var count = $('#ApproversCount').val();

    for (i = 0; i < count; i++) {
        if (i == 0)
            approversCSV = $('#ApproverLevel' + i).val();
        else
            approversCSV = approversCSV + ',' + $('#ApproverLevel' + i).val();
    }
    SendDocForApproval(documentID, documentTypeCode, approversCSV);
    $('#SendApprovalModal').modal('hide');
    //BindPurchaseOrder($('#ID').val());

}
function ClearCalculatedFields()
{
    $('#lblTaxTotal').text('0.00');
    $('#lblItemTotal').text('0.00');
    $('#lblGrossAmount').text('0.00');
    $('#lblCessAmount').text('0.00');
    $('#lblGrandTotal').text('0.00');
}