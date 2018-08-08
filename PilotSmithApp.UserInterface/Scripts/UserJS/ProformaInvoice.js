﻿var _dataTable = {};
var _emptyGuid = "00000000-0000-0000-0000-000000000000";
var _datatablerowindex = -1;
var _jsonData = {};
var _message = "";
var _status = "";
var _result = "";
//---------------------------------------Docuement Ready--------------------------------------------------//
$(document).ready(function () {
    try {
        debugger;
        BindOrReloadProformaInvoiceTable('Init');
        $('#tblProformaInvoice tbody').on('dblclick', 'td', function () {
            EditProformaInvoice(this);
        });
        if ($('#RedirectToDocument').val() != "") {
            if ($('#RedirectToDocument').val() === _emptyGuid) {
                AddProformaInvoice();
            }
            else {
                EditRedirectToDocument($('#RedirectToDocument').val());
            }
        }
    }
    catch (e) {
        console.log(e.message);
    }
    $("#AdvDocumentStatusCode,#AdvEmailSentStatus").select2({
        dropdownParent: $(".divboxASearch")
    });

    $('.select2').addClass('form-control newinput');
});
//function bind the ProformaInvoice list checking search and filter
function BindOrReloadProformaInvoiceTable(action) {
    try {
        debugger;
        //creating advancesearch object
        ProformaInvoiceAdvanceSearchViewModel = new Object();
        DataTablePagingViewModel = new Object();
        DataTablePagingViewModel.Length = 0;
        //switch case to check the operation
        switch (action) {
            case 'Reset':
                $('#SearchTerm').val('');
                $('.divboxASearch #AdvFromDate').val('');
                $('.divboxASearch #AdvToDate').val('');
                $('.divboxASearch #AdvCustomerID').val('').trigger('change');
                $('.divboxASearch #AdvBranchCode').val('').trigger('change');
                $('.divboxASearch #AdvAreaCode').val('').trigger('change');
                $('.divboxASearch #AdvDocumentStatusCode').val('').trigger('change');
                $('.divboxASearch #AdvDocumentOwnerID').val('').trigger('change');
                $('.divboxASearch #AdvApprovalStatusCode').val('').trigger('change');
                $('#AdvEmailSentStatus').val('').trigger('change');
                break;
            case 'Init':
                $('#SearchTerm').val('');
                $('.divboxASearch #AdvFromDate').val('');
                $('.divboxASearch #AdvToDate').val('');
                $('.divboxASearch #AdvAreaCode').val('');
                $('.divboxASearch #AdvCustomerID').val('');
                $('.divboxASearch #AdvBranchCode').val('');
                $('.divboxASearch #AdvDocumentStatusCode').val('');
                $('.divboxASearch #AdvDocumentOwnerID').val('');
                $('.divboxASearch #AdvApprovalStatusCode').val('');
                $('#AdvEmailSentStatus').val('');
                break;
            case 'Search':
                if (($('#SearchTerm').val() == "") && ($('.divboxASearch #AdvFromDate').val() == "") && ($('.divboxASearch #AdvToDate').val() == "") && ($('.divboxASearch #AdvAreaCode').val() == "") && ($('.divboxASearch #AdvCustomerID').val() == "") && ($('.divboxASearch #AdvBranchCode').val() == "") && ($('.divboxASearch #AdvDocumentStatusCode').val() == "") && ($('.divboxASearch #AdvDocumentOwnerID').val() == "") && ($('#AdvEmailSentStatus').val() == "") && ($('#AdvApprovalStatusCode').val() == "")) {
                    return true;
                }
                break;
            case 'Export':
                DataTablePagingViewModel.Length = -1;
                ProformaInvoiceAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
                ProformaInvoiceAdvanceSearchViewModel.SearchTerm = $('#SearchTerm').val() == "" ? null : $('#SearchTerm').val();
                ProformaInvoiceAdvanceSearchViewModel.AdvFromDate = $('.divboxASearch #AdvFromDate').val() == "" ? null : $('.divboxASearch #AdvFromDate').val();
                ProformaInvoiceAdvanceSearchViewModel.AdvToDate = $('.divboxASearch #AdvToDate').val() == "" ? null : $('.divboxASearch #AdvToDate').val();
                ProformaInvoiceAdvanceSearchViewModel.AdvAreaCode = $('.divboxASearch #AdvAreaCode').val() == "" ? null : $('.divboxASearch #AdvAreaCode').val();
                ProformaInvoiceAdvanceSearchViewModel.AdvCustomerID = $('.divboxASearch #AdvCustomerID').val() == "" ? _emptyGuid : $('.divboxASearch #AdvCustomerID').val();
                ProformaInvoiceAdvanceSearchViewModel.AdvBranchCode = $('.divboxASearch #AdvBranchCode').val() == "" ? null : $('.divboxASearch #AdvBranchCode').val();
                ProformaInvoiceAdvanceSearchViewModel.AdvDocumentStatusCode = $('.divboxASearch #AdvDocumentStatusCode').val() == "" ? null : $('.divboxASearch #AdvDocumentStatusCode').val();
                ProformaInvoiceAdvanceSearchViewModel.AdvDocumentOwnerID = $('.divboxASearch #AdvDocumentOwnerID').val() == "" ? _emptyGuid : $('.divboxASearch #AdvDocumentOwnerID').val();
                ProformaInvoiceAdvanceSearchViewModel.AdvApprovalStatusCode = $('.divboxASearch #AdvApprovalStatusCode').val() == "" ? null : $('.divboxASearch #AdvApprovalStatusCode').val();
                ProformaInvoiceAdvanceSearchViewModel.AdvEmailSentStatus = $('#AdvEmailSentStatus').val() == "" ? null : $('#AdvEmailSentStatus').val();
                $('#AdvanceSearch').val(JSON.stringify(ProformaInvoiceAdvanceSearchViewModel));
                $('#FormExcelExport').submit();
                return true;
                break;
            default:
                break;
        }
        ProformaInvoiceAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
        ProformaInvoiceAdvanceSearchViewModel.SearchTerm = $('#SearchTerm').val();
        ProformaInvoiceAdvanceSearchViewModel.AdvFromDate = $('.divboxASearch #AdvFromDate').val();
        ProformaInvoiceAdvanceSearchViewModel.AdvToDate = $('.divboxASearch #AdvToDate').val();
        ProformaInvoiceAdvanceSearchViewModel.AdvAreaCode = $('.divboxASearch #AdvAreaCode').val();
        ProformaInvoiceAdvanceSearchViewModel.AdvCustomerID = $('.divboxASearch #AdvCustomerID').val();
        ProformaInvoiceAdvanceSearchViewModel.AdvBranchCode = $('.divboxASearch #AdvBranchCode').val();
        ProformaInvoiceAdvanceSearchViewModel.AdvDocumentStatusCode = $('.divboxASearch #AdvDocumentStatusCode').val();
        ProformaInvoiceAdvanceSearchViewModel.AdvDocumentOwnerID = $('.divboxASearch #AdvDocumentOwnerID').val();
        ProformaInvoiceAdvanceSearchViewModel.AdvApprovalStatusCode = $('.divboxASearch #AdvApprovalStatusCode').val();
        ProformaInvoiceAdvanceSearchViewModel.AdvEmailSentStatus = $('#AdvEmailSentStatus').val();
        //apply datatable plugin on ProformaInvoice table
        _dataTable.ProformaInvoiceList = $('#tblProformaInvoice').DataTable(
        {
            dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
            ordering: false,
            searching: false,
            paging: true,
            lengthChange: false,
            processing: true,
            autoWidth: false,
            language: {
                "processing": "<div class='spinner'><div class='bounce1'></div><div class='bounce2'></div><div class='bounce3'></div></div>"
            },
            serverSide: true,
            ajax: {
                url: "ProformaInvoice/GetAllProformaInvoice/",
                data: { "proformaInvoiceAdvanceSearchVM": ProformaInvoiceAdvanceSearchViewModel },
                type: 'POST'
            },
            pageLength: 8,
            columns: [
                 {
                     "data": "ProfInvNo", render: function (data, type, row) {
                         return row.ProfInvNo + "</br>" + "<img src='./Content/images/datePicker.png' height='10px'>" + "&nbsp;" + row.ProfInvDateFormatted;
                     }, "defaultContent": "<i>-</i>"
                 },
                 {
                     "data": "Customer.CompanyName", render: function (data, type, row) {
                         return "<img src='./Content/images/contact.png' height='10px'>" + "&nbsp;" + (row.Customer.ContactPerson == null ? "" : row.Customer.ContactPerson) + "</br>" + "<img src='./Content/images/organisation.png' height='10px'>" + "&nbsp;" + data;
                     }, "defaultContent": "<i>-</i>"
                 },
                 {
                     "data": "ReferenceNo", render: function (data, type, row) {
                         return (row.SaleOrder.SaleOrderNo == null ? row.Quotation.QuoteNo : row.SaleOrder.SaleOrderNo);
                     }, "defaultContent": "<i>-</i>"
                 },
                 { "data": "Area.Description", "defaultContent": "<i>-</i>" },
                 {
                     "data": "Branch.Description", render: function (data, type, row) {
                         return "<b>Doc.Owner-</b>" + row.PSAUser.LoginName + "</br>" + "<b>Branch-</b>" + row.Branch.Description;
                     }, "defaultContent": "<i>-</i>"
                 },
                 {
                     "data": "DocumentStatus.Description", render: function (data, type, row) {
                         return "<b>Doc.Status-</b>" + data + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + (row.EmailSentYN == true ? "<img src='./Content/images/mailSend.png' height='20px' >" : '') + "</br>" + "<b>Appr.Status-</b>" + row.ApprovalStatus.Description;
                     }, "defaultContent": "<i>-</i>"
                 },
                 { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink"  onclick="EditProformaInvoice(this)" ><i class="fa fa-pencil-square-o" aria-hidden="true"></i></a>' },
            ],
            columnDefs: [{ className: "text-left", "targets": [0, 1, 2, 3, 4, 5] },
                          { className: "text-center", "targets": [6] },
                            { "targets": [0], "width": "12%" },
                            { "targets": [1], "width": "15%" },
                            { "targets": [2], "width": "15%" },
                            { "targets": [3], "width": "10%" },
                            { "targets": [4], "width": "15%" },
                            { "targets": [5], "width": "24%" },
                            { "targets": [6], "width": "3%" },
            ],
            destroy: true,
            //for performing the import operation after the data loaded
            initComplete: function (settings, json) {

                $('.dataTables_wrapper div.bottom div').addClass('col-md-6');
                $('#tblProformaInvoice').fadeIn(100);
                if (action == undefined) {
                    // $('.excelExport').hide();
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
function ResetProformaInvoiceList() {
    $(".searchicon").removeClass('filterApplied');
    BindOrReloadProformaInvoiceTable('Reset');
}
//function export data to excel
function ExportProformaInvoiceData() {
    BindOrReloadProformaInvoiceTable('Export');
}
// add ProformaInvoice section
function AddProformaInvoice() {
    //this will return form body(html)
    OnServerCallBegin();
    $("#divProformaInvoiceForm").load("ProformaInvoice/ProformaInvoiceForm?id=" + _emptyGuid, function (responseTxt, statusTxt, xhr) {
        if (statusTxt == "success") {
            OnServerCallComplete();
            openNav();
            $('#lblProformaInvoiceInfo').text('<<Proforma Invoice No.>>');
            ChangeButtonPatchView("ProformaInvoice", "btnPatchProformaInvoiceNew", "Add");
            BindProformaInvoiceDetailList(_emptyGuid);
            BindProformaInvoiceOtherChargesDetailList(_emptyGuid);
            
        }
        else {
            console.log("Error: " + xhr.status + ": " + xhr.statusText);
        }
    });
}
function EditProformaInvoice(this_Obj) {
    debugger;
    OnServerCallBegin();
    var ProformaInvoice = _dataTable.ProformaInvoiceList.row($(this_Obj).parents('tr')).data();
    $('#lblProformaInvoiceInfo').text(ProformaInvoice.ProfInvNo);
    //this will return form body(html)
    $("#divProformaInvoiceForm").load("ProformaInvoice/ProformaInvoiceForm?id=" + ProformaInvoice.ID, function (responseTxt, statusTxt, xhr) {
        if (statusTxt == "success") {
            OnServerCallComplete();
            openNav();
            //$('#CustomerID').trigger('change');
            debugger;
            if ($('#IsDocLocked').val() == "True") {
                ChangeButtonPatchView("ProformaInvoice", "btnPatchProformaInvoiceNew", "Edit", ProformaInvoice.ID);
            }
            else {
                ChangeButtonPatchView("ProformaInvoice", "btnPatchProformaInvoiceNew", "LockDocument", ProformaInvoice.ID);
            }
            BindProformaInvoiceDetailList(ProformaInvoice.ID);
            BindProformaInvoiceOtherChargesDetailList(ProformaInvoice.ID);           
            CalculateTotal();
            $('#divCustomerBasicInfo').load("Customer/CustomerBasicInfo?ID=" + $('#hdnCustomerID').val());
            clearUploadControl();
            PaintImages(ProformaInvoice.ID);
            
        }
        else {
            console.log("Error: " + xhr.status + ": " + xhr.statusText);
        }
    });
}
function ResetProformaInvoice() {
    $("#divProformaInvoiceForm").load("ProformaInvoice/ProformaInvoiceForm?id=" + $('#ProformaInvoiceForm #ID').val(), function (responseTxt, statusTxt, xhr) {
        if (statusTxt == "success") {
            if ($('#ID').val() != _emptyGuid && $('#ID').val() != null) {
                //resides in customjs for sliding
                openNav();
            }
            debugger;
            if ($('#IsUpdate').val() == "False") {
                ChangeButtonPatchView("ProformaInvoice", "btnPatchProformaInvoiceNew", "Add", $('#ProformaInvoiceForm #ID').val());
            }
            else if ($('#IsDocLocked').val() == "True") {
                ChangeButtonPatchView("ProformaInvoice", "btnPatchProformaInvoiceNew", "Edit", $('#ProformaInvoiceForm #ID').val());
            }
            else {
                ChangeButtonPatchView("ProformaInvoice", "btnPatchProformaInvoiceNew", "LockDocument", $('#ProformaInvoiceForm #ID').val());
            }
            BindProformaInvoiceDetailList($('#ID').val());
            BindProformaInvoiceOtherChargesDetailList($('#ID').val());          
            CalculateTotal();
            clearUploadControl();
            PaintImages($('#ProformaInvoiceForm #ID').val());
            $('#divCustomerBasicInfo').load("Customer/CustomerBasicInfo?ID=" + $('#ProformaInvoiceForm #hdnCustomerID').val());
        }
        else {
            console.log("Error: " + xhr.status + ": " + xhr.statusText);
        }
    });
}
function SaveProformaInvoice() {
    debugger;
    var ProformaInvoiceDetailList = _dataTable.ProformaInvoiceDetailList.rows().data().toArray();
    var ProformaInvoiceOtherChargesDetailList = _dataTable.ProformaInvoiceOtherChargesDetailList.rows().data().toArray();
    $('#DetailJSON').val(JSON.stringify(ProformaInvoiceDetailList));
    $('#OtherChargesDetailJSON').val(JSON.stringify(ProformaInvoiceOtherChargesDetailList));
    $('#btnInsertUpdateProformaInvoice').trigger('click');
}
function ApplyFilterThenSearch() {
    $(".searchicon").addClass('filterApplied');
    CloseAdvanceSearch();
    BindOrReloadProformaInvoiceTable('Search');
}
function SaveSuccessProformaInvoice(data, status) {
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
                $("#divProformaInvoiceForm").load("ProformaInvoice/ProformaInvoiceForm?id=" + _result.ID, function () {
                    ChangeButtonPatchView("ProformaInvoice", "btnPatchProformaInvoiceNew", "Edit", _result.ID);
                    BindProformaInvoiceDetailList(_result.ID);
                    BindProformaInvoiceOtherChargesDetailList(_result.ID);
                    CalculateTotal();
                    clearUploadControl();
                    PaintImages(_result.ID);
                    $('#lblProformaInvoiceInfo').text(_result.ProformaInvoiceNo);
                    $('#divCustomerBasicInfo').load("Customer/CustomerBasicInfo?ID=" + $('#ProformaInvoiceForm #hdnCustomerID').val());
                });
                ChangeButtonPatchView("ProformaInvoice", "btnPatchProformaInvoiceNew", "Edit", _result.ID);
                BindOrReloadProformaInvoiceTable('Init');
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
function DeleteProformaInvoice() {
    notyConfirm('Are you sure to delete?', 'DeleteProformaInvoiceItem("' + $('#ProformaInvoiceForm #ID').val() + '")');
}
function DeleteProformaInvoiceItem(id) {
    try {
        if (id) {
            var data = { "id": id };
            _jsonData = GetDataFromServer("ProformaInvoice/DeleteProformaInvoice/", data);
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
                    ChangeButtonPatchView("ProformaInvoice", "btnPatchProformaInvoiceNew", "Add");
                    ResetProformaInvoice();
                    $('#lblProformaInvoiceInfo').text('<<Proforma Invoice No.>>');
                    BindOrReloadProformaInvoiceTable('Init');
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
function BindProformaInvoiceDetailList(id, IsSaleOrder, IsQuotation) {
    var data;
    if (id == _emptyGuid && !(IsSaleOrder) && !(IsQuotation)) {
        data = null;
    }
    else if (id == _emptyGuid && (IsSaleOrder)) {
        data = GetProformaInvoiceDetailListByProformaInvoiceID(id, IsSaleOrder, IsQuotation)
    }
    else if (id == _emptyGuid && (IsQuotation)) {
        data = GetProformaInvoiceDetailListByProformaInvoiceID(id, IsSaleOrder, IsQuotation)
    }
    else {
        data = GetProformaInvoiceDetailListByProformaInvoiceID(id, IsSaleOrder, IsQuotation)
    }
    _dataTable.ProformaInvoiceDetailList = $('#tblProformaInvoiceDetails').DataTable(
         {
             dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
             order: [],
             searching: false,
             paging: false,
             ordering: false,
             bInfo: false,
             data: data,//id == _emptyGuid ? null : GetProformaInvoiceDetailListByProformaInvoiceID(id),
             language: {
                 search: "_INPUT_",
                 searchPlaceholder: "Search"
             },
             columns: [
             {
                 "data": "Product.Code", render: function (data, type, row) {
                     if (data != "" && data!=undefined) {
                         debugger;
                         return '<div style="width:100%" class="show-popover" data-html="true" data-placement="top" data-toggle="popover" data-title="<p align=left>Product Specification" data-content="' +(row.ProductSpec!==null?row.ProductSpec.replace(/"/g, "&quot"):"") + '</p>"/>' +
                                                 row.Product.Name + '</br>' + row.ProductModel.Name
                     }
                     else {
                         return row.OtherCharge.Description
                     }
                 }, "defaultContent": "<i></i>"
             },
             {
                 "data": "Product.HSNCode", render: function (data, type, row) {
                     debugger;
                     //if ((row.OtherCharge.SACCode == null || row.OtherCharge.SACCode == "") && (row.Product.HSNCode !== null || row.Product.HSNCode=="")) {
                     //    return row.Product.HSNCode;
                     //}
                     //else if (((row.OtherCharge.SACCode !== null) || (row.OtherCharge.SACCode=="")) && ((row.Product.HSNCode == null) || (row.Product.HSNCode == ""))) {
                     //    return row.OtherCharge.SACCode;
                     //}
                     if (data !== null || data == "") {
                         return row.Product.HSNCode;
                     }
                     else if(data!=null || data=="") {
                         return row.OtherCharge.SACCode;
                     }
                 }, "defaultContent": "<i></i>"
             },
             {
                 "data": "Qty", render: function (data, type, row) {
                     return data + " " + row.Unit.Description
                 }, "defaultContent": "<i></i>"
             },
             {
                 "data": "Rate", render: function (data, type, row) { return roundoff(parseFloat(data)) }, "defaultContent": "<i></i>"
             },
             { "data": "Discount", render: function (data, type, row) { return roundoff(parseFloat(data)) }, "defaultContent": "<i></i>" },
             {//Taxable
                 "data": "Rate", render: function (data, type, row) {
                     var Total = roundoff(parseFloat(data != "" ? data : 0) * parseInt(row.Qty != "" ? row.Qty : 1))
                     var Discount = roundoff(parseFloat(row.Discount != "" ? row.Discount : 0))
                     var Taxable = Total - Discount
                     return '<div class="show-popover text-right" data-html="true" data-placement="left" data-toggle="popover" data-title="<p align=left>Taxable : ₹ ' + Taxable + '" data-content="Net Total : ₹ ' + Total + '<br/> Discount : ₹ -' + Discount + '</p>"/>' + roundoff(parseFloat(Taxable))
                 }, "defaultContent": "<i></i>"
             },
             {//GST
                 "data": "Rate", render: function (data, type, row) {
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
                     return '<div class="show-popover text-right" data-html="true" data-placement="left" data-toggle="popover" data-title="<p align=left>Total GST : ₹ ' + GSTAmt + '" data-content=" SGST ' + SGST + '% : ₹ ' + roundoff(SGSTAmt) + '<br/>CGST ' + CGST + '% : ₹ ' + roundoff(parseFloat(CGSTAmt)) + '<br/> IGST ' + IGST + '% : ₹ ' + roundoff(parseFloat(IGSTAmt)) + '</p>"/>' + GSTAmt
                 }, "defaultContent": "<i></i>"
             },
            {//Cess
                "data": "CessAmt", render: function (data, type, row) {
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
                    return '<div class="show-popover text-right" data-html="true" data-placement="left" data-toggle="popover" data-title="<p align=left>Grand Total : ₹ ' + GrandTotal + '" data-content="Taxable : ₹ ' + TaxableAmt + '<br/>GST : ₹ ' + GSTAmt + '</p>"/>' + GrandTotal
                }, "defaultContent": "<i></i>"
            },
            {
                "data": null, "orderable": false, render: function (data, type, row) {
                    if (row.Product.Code != "" && row.Product.Code != null) {
                        return ($('#IsDocLocked').val() == "True" || $('#IsUpdate').val() == "False") ? '<a href="#" class="actionLink"  onclick="EditProformaInvoiceDetail(this)" ><i class="fa fa-pencil-square-o" aria-hidden="true"></i></a> <a href="#" class="DeleteLink"  onclick="ConfirmDeleteProformaInvoiceDetail(this)" ><i class="fa fa-trash-o" aria-hidden="true"></i></a>' : "-"
                    }
                    else {
                        return ($('#IsDocLocked').val() == "True" || $('#IsUpdate').val() == "False") ? '<a href="#" class="actionLink"  onclick="EditProformaInvoiceServiceBill(this)" ><i class="fa fa-pencil-square-o" aria-hidden="true"></i></a> <a href="#" class="DeleteLink"  onclick="ConfirmDeleteProformaInvoiceDetail(this)" ><i class="fa fa-trash-o" aria-hidden="true"></i></a>' : "-"
                    }
                }, "defaultContent": "<i></i>"
            },
             ],
             columnDefs: [
                 { className: "text-right", "targets": [2,3, 4, 5, 6, 7,8] },
                 { className: "text-left", "targets": [0,1] },
                 { className: "text-center", "targets": [ 9] }
             ]
         });
    $('[data-toggle="popover"]').popover({
        html: true,
        'trigger': 'hover'
    });
}

function GetProformaInvoiceDetailListByProformaInvoiceID(id, IsSaleOrder, IsQuotation) {
    try {
        ;
        var ProformaInvoiceDetailList = [];
        if (IsSaleOrder) {
            var data = { "saleOrderID": $('#ProformaInvoiceForm #hdnSaleOrderID').val() };
            _jsonData = GetDataFromServer("ProformaInvoice/GetProformaInvoiceDetailListBySaleOrderIDFromSaleOrder/", data);
        }
        else if (IsQuotation) {
            var data = { "quoteID": $('#ProformaInvoiceForm #hdnQuoteID').val() };
            _jsonData = GetDataFromServer("ProformaInvoice/GetProformaInvoiceDetailListByQuotationIDFromQuotation/", data);
        }
        else {
            var data = { "ProformaInvoiceID": id };
            _jsonData = GetDataFromServer("ProformaInvoice/GetProformaInvoiceDetailListByProformaInvoiceID/", data);
        }

        if (_jsonData != '') {
            _jsonData = JSON.parse(_jsonData);
            _message = _jsonData.Message;
            _status = _jsonData.Status;
            ProformaInvoiceDetailList = _jsonData.Records;
        }
        if (_status == "OK") {
            return ProformaInvoiceDetailList;
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


//Add ProformaInvoice Detail
function AddProformaInvoiceDetailList() {
    $("#divModelProformaInvoicePopBody").load("ProformaInvoice/AddProformaInvoiceDetail", function () {
        $('#lblModelPopProformaInvoice').text('ProformaInvoice Detail')
        $('#divModelPopProformaInvoice').modal('show');
    });
}

function AddProformaInvoiceDetailToList() {
    debugger;
    $("#FormProformaInvoiceDetail").submit(function () { });

    if ($('#FormProformaInvoiceDetail #IsUpdate').val() == 'True') {
        if (($('#ProductID').val() != "") && ($('#ProductModelID').val() != "") && ($('#Rate').val() != "") && ($('#Qty').val() != "") && ($('#UnitCode').val() != "")) {
            var proformaInvoiceDetailList = _dataTable.ProformaInvoiceDetailList.rows().data();
            debugger;
            proformaInvoiceDetailList[_datatablerowindex].Product.Code = $("#ProductID").val() != "" ? $("#ProductID option:selected").text().split("-")[0].trim() : "";
            proformaInvoiceDetailList[_datatablerowindex].Product.Name = $("#ProductID").val() != "" ? $("#ProductID option:selected").text().split("-")[1].trim() : "";
            proformaInvoiceDetailList[_datatablerowindex].Product.HSNCode = $("#hdnProductHSNCode").val();
            proformaInvoiceDetailList[_datatablerowindex].ProductID = $("#ProductID").val() != "" ? $("#ProductID").val() : _emptyGuid;
            proformaInvoiceDetailList[_datatablerowindex].ProductModelID = $("#ProductModelID").val() != "" ? $("#ProductModelID").val() : _emptyGuid;
            ProductModel = new Object;
            Unit = new Object;
            OtherCharge = new Object;
            OtherCharge.SACCode = null;
            ProductModel.Name = $("#ProductModelID").val() != "" ? $("#ProductModelID option:selected").text() : "";
            proformaInvoiceDetailList[_datatablerowindex].ProductModel = ProductModel;
            proformaInvoiceDetailList[_datatablerowindex].ProductSpec = $('#ProductSpec').val();
            proformaInvoiceDetailList[_datatablerowindex].Qty = $('#Qty').val();
            proformaInvoiceDetailList[_datatablerowindex].UnitCode = $('#UnitCode').val();
            Unit.Description = $("#UnitCode").val() != "" ? $("#UnitCode option:selected").text().trim() : "";
            proformaInvoiceDetailList[_datatablerowindex].Unit = Unit;
            proformaInvoiceDetailList[_datatablerowindex].Rate = $('#Rate').val();
            proformaInvoiceDetailList[_datatablerowindex].Discount = $('#divModelProformaInvoicePopBody #Discount').val() != "" ? $('#divModelProformaInvoicePopBody #Discount').val() : 0;
            if ($('#divModelProformaInvoicePopBody #TaxTypeCode').val() != null)
                proformaInvoiceDetailList[_datatablerowindex].TaxTypeCode = $('#divModelProformaInvoicePopBody #TaxTypeCode').val().split('|')[0];
            proformaInvoiceDetailList[_datatablerowindex].TaxType = new Object;
            proformaInvoiceDetailList[_datatablerowindex].TaxType.ValueText = $('#divModelProformaInvoicePopBody #TaxTypeCode').val();
            proformaInvoiceDetailList[_datatablerowindex].CGSTPerc = $('#divModelProformaInvoicePopBody #hdnCGSTPerc').val();
            proformaInvoiceDetailList[_datatablerowindex].SGSTPerc = $('#divModelProformaInvoicePopBody #hdnSGSTPerc').val();
            proformaInvoiceDetailList[_datatablerowindex].IGSTPerc = $('#divModelProformaInvoicePopBody #hdnIGSTPerc').val();
            proformaInvoiceDetailList[_datatablerowindex].CessPerc = $('#divModelProformaInvoicePopBody #CessPerc').val() != "" ? $('#divModelProformaInvoicePopBody #CessPerc').val() : 0;
            proformaInvoiceDetailList[_datatablerowindex].CessAmt = $('#divModelProformaInvoicePopBody #CessAmt').val();

            _dataTable.ProformaInvoiceDetailList.clear().rows.add(proformaInvoiceDetailList).draw(false);
            CalculateTotal();
            $('#divModelPopProformaInvoice').modal('hide');
            _datatablerowindex = -1;
        }
    }
    else {
        if (($('#ProductID').val() != "") && ($('#ProductModelID').val() != "") && ($('#Rate').val() != "") && ($('#Qty').val() != "") && ($('#UnitCode').val() != "")) {
            if (_dataTable.ProformaInvoiceDetailList.rows().data().length === 0) {
                _dataTable.ProformaInvoiceDetailList.clear().rows.add(GetProformaInvoiceDetailListByProformaInvoiceID(_emptyGuid)).draw(false);
                var proformaInvoiceDetailVM = _dataTable.ProformaInvoiceDetailList.rows().data();
                proformaInvoiceDetailVM.Product = new Object;
                proformaInvoiceDetailVM.ProductModel = new Object;
                proformaInvoiceDetailVM.Unit = new Object;
                proformaInvoiceDetailVM.TaxType = new Object;
                proformaInvoiceDetailVM.OtherCharge = new Object;
                proformaInvoiceDetailVM.OtherCharge.SACCode = null;
                proformaInvoiceDetailVM[0].Product.Code = $("#divModelProformaInvoicePopBody #ProductID").val() != "" ? $("#divModelProformaInvoicePopBody #ProductID option:selected").text().split("-")[0].trim() : "";
                proformaInvoiceDetailVM[0].Product.Name = $("#divModelProformaInvoicePopBody #ProductID").val() != "" ? $("#divModelProformaInvoicePopBody #ProductID option:selected").text().split("-")[1].trim() : "";
                proformaInvoiceDetailVM[0].Product.HSNCode = $("#hdnProductHSNCode").val();
                proformaInvoiceDetailVM[0].ProductID = $("#divModelProformaInvoicePopBody #ProductID").val() != "" ? $("#divModelProformaInvoicePopBody #ProductID").val() : _emptyGuid;
                proformaInvoiceDetailVM[0].ProductModelID = $("#divModelProformaInvoicePopBody #ProductModelID").val() != "" ? $("#divModelProformaInvoicePopBody #ProductModelID").val() : _emptyGuid;
                proformaInvoiceDetailVM[0].ProductModel.Name = $("#divModelProformaInvoicePopBody #ProductModelID").val() != "" ? $("#divModelProformaInvoicePopBody #ProductModelID option:selected").text() : "";
                proformaInvoiceDetailVM[0].ProductSpec = $('#divModelProformaInvoicePopBody #ProductSpec').val();
                proformaInvoiceDetailVM[0].Qty = $('#divModelProformaInvoicePopBody #Qty').val();
                proformaInvoiceDetailVM[0].UnitCode = $('#divModelProformaInvoicePopBody #UnitCode').val();
                proformaInvoiceDetailVM[0].Unit.Description = $("#divModelProformaInvoicePopBody #UnitCode").val() != "" ? $("#divModelProformaInvoicePopBody #UnitCode option:selected").text().trim() : "";
                proformaInvoiceDetailVM[0].Rate = $('#divModelProformaInvoicePopBody #Rate').val();
                proformaInvoiceDetailVM[0].Discount = $('#divModelProformaInvoicePopBody #Discount').val() != "" ? $('#divModelProformaInvoicePopBody #Discount').val() : 0;
                proformaInvoiceDetailVM[0].TaxTypeCode =$('#divModelProformaInvoicePopBody #TaxTypeCode').val()!=null? $('#divModelProformaInvoicePopBody #TaxTypeCode').val().split('|')[0]:"";
                proformaInvoiceDetailVM[0].TaxType.ValueText = $('#divModelProformaInvoicePopBody #TaxTypeCode').val();
                proformaInvoiceDetailVM[0].CGSTPerc = $('#divModelProformaInvoicePopBody #hdnCGSTPerc').val();
                proformaInvoiceDetailVM[0].SGSTPerc = $('#divModelProformaInvoicePopBody #hdnSGSTPerc').val();
                proformaInvoiceDetailVM[0].IGSTPerc = $('#divModelProformaInvoicePopBody #hdnIGSTPerc').val();
                proformaInvoiceDetailVM[0].CessPerc = $('#divModelProformaInvoicePopBody #CessPerc').val() != "" ? $('#divModelProformaInvoicePopBody #CessPerc').val() : 0;
                proformaInvoiceDetailVM[0].CessAmt = $('#divModelProformaInvoicePopBody #CessAmt').val();
                ClearCalculatedFields();
                _dataTable.ProformaInvoiceDetailList.clear().rows.add(proformaInvoiceDetailVM).draw(false);
                CalculateTotal();
                $('#divModelPopProformaInvoice').modal('hide');
            }
            else {
                var proformaInvoiceDetailVM = _dataTable.ProformaInvoiceDetailList.rows().data();
                if (proformaInvoiceDetailVM.length > 0) {
                    var checkpoint = 0;
                    var productSpec = $('#ProductSpec').val();
                    productSpec = productSpec.replace(/\n/g, ' ');
                    for (var i = 0; i < proformaInvoiceDetailVM.length; i++) {
                        if ((proformaInvoiceDetailVM[i].ProductID == $('#ProductID').val()) && (proformaInvoiceDetailVM[i].ProductModelID == $('#ProductModelID').val()
                            && (proformaInvoiceDetailVM[i].ProductSpec.replace(/\n/g, ' ') == productSpec && (proformaInvoiceDetailVM[i].UnitCode == $('#UnitCode').val())))) {
                            proformaInvoiceDetailVM[i].Qty = parseFloat(proformaInvoiceDetailVM[i].Qty) + parseFloat($('#Qty').val());
                            checkpoint = 1;
                            break;
                        }
                    }
                    if (checkpoint == 1) {
                        _dataTable.ProformaInvoiceDetailList.clear().rows.add(proformaInvoiceDetailVM).draw(false);
                    }
                    else if (checkpoint == 0) {
                        var ProformaInvoiceDetailVM = new Object();
                        ProformaInvoiceDetailVM.Product = new Object;
                        ProformaInvoiceDetailVM.ProductModel = new Object()
                        ProformaInvoiceDetailVM.Unit = new Object();
                        ProformaInvoiceDetailVM.TaxType = new Object();
                        ProformaInvoiceDetailVM.OtherCharge = new Object();
                        ProformaInvoiceDetailVM.OtherCharge.SACCode = null;

                        ProformaInvoiceDetailVM.ID = _emptyGuid;
                        ProformaInvoiceDetailVM.ProductID = $("#ProductID").val() != "" ? $("#ProductID").val() : _emptyGuid;
                        ProformaInvoiceDetailVM.Product.Code = $("#divModelProformaInvoicePopBody #ProductID").val() != "" ? $("#divModelProformaInvoicePopBody #ProductID option:selected").text().split("-")[0].trim() : "";
                        ProformaInvoiceDetailVM.Product.Name = $("#divModelProformaInvoicePopBody #ProductID").val() != "" ? $("#divModelProformaInvoicePopBody #ProductID option:selected").text().split("-")[1].trim() : "";
                        ProformaInvoiceDetailVM.Product.HSNCode = $("#hdnProductHSNCode").val();
                        ProformaInvoiceDetailVM.ProductID = $("#divModelProformaInvoicePopBody #ProductID").val() != "" ? $("#divModelProformaInvoicePopBody #ProductID").val() : _emptyGuid;
                        ProformaInvoiceDetailVM.ProductModelID = $("#divModelProformaInvoicePopBody #ProductModelID").val() != "" ? $("#divModelProformaInvoicePopBody #ProductModelID").val() : _emptyGuid;
                        ProformaInvoiceDetailVM.ProductModel.Name = $("#divModelProformaInvoicePopBody #ProductModelID").val() != "" ? $("#divModelProformaInvoicePopBody #ProductModelID option:selected").text() : "";
                        ProformaInvoiceDetailVM.ProductSpec = $('#divModelProformaInvoicePopBody #ProductSpec').val();
                        ProformaInvoiceDetailVM.Qty = $('#divModelProformaInvoicePopBody #Qty').val();
                        ProformaInvoiceDetailVM.UnitCode = $('#divModelProformaInvoicePopBody #UnitCode').val();
                        ProformaInvoiceDetailVM.Unit.Description = $("#divModelProformaInvoicePopBody #UnitCode").val() != "" ? $("#divModelProformaInvoicePopBody #UnitCode option:selected").text().trim() : "";
                        ProformaInvoiceDetailVM.Rate = $('#divModelProformaInvoicePopBody #Rate').val();
                        ProformaInvoiceDetailVM.Discount = $('#divModelProformaInvoicePopBody #Discount').val() != "" ? $('#divModelProformaInvoicePopBody #Discount').val() : 0;
                        ProformaInvoiceDetailVM.TaxTypeCode =$('#divModelProformaInvoicePopBody #TaxTypeCode').val()!=null? $('#divModelProformaInvoicePopBody #TaxTypeCode').val().split('|')[0]:"";
                        ProformaInvoiceDetailVM.TaxType.ValueText = $('#divModelProformaInvoicePopBody #TaxTypeCode').val();
                        ProformaInvoiceDetailVM.CGSTPerc = $('#divModelProformaInvoicePopBody #hdnCGSTPerc').val();
                        ProformaInvoiceDetailVM.SGSTPerc = $('#divModelProformaInvoicePopBody #hdnSGSTPerc').val();
                        ProformaInvoiceDetailVM.IGSTPerc = $('#divModelProformaInvoicePopBody #hdnIGSTPerc').val();
                        ProformaInvoiceDetailVM.CessPerc = $('#divModelProformaInvoicePopBody #CessPerc').val() != "" ? $('#divModelProformaInvoicePopBody #CessPerc').val() : 0;
                        ProformaInvoiceDetailVM.CessAmt = $('#divModelProformaInvoicePopBody #CessAmt').val();
                        _dataTable.ProformaInvoiceDetailList.row.add(ProformaInvoiceDetailVM).draw(true);
                    }
                    CalculateTotal();
                    $('#divModelPopProformaInvoice').modal('hide');
                }
            }
        }
    }
    $('[data-toggle="popover"]').popover({
        html: true,
        'trigger': 'hover'

    });
}

//Edit ProformaInvoice Detail
function EditProformaInvoiceDetail(this_Obj) {
    debugger;
    _datatablerowindex = _dataTable.ProformaInvoiceDetailList.row($(this_Obj).parents('tr')).index();
    var proformaInvoiceDetail = _dataTable.ProformaInvoiceDetailList.row($(this_Obj).parents('tr')).data();
    $("#divModelProformaInvoicePopBody").load("ProformaInvoice/AddProformaInvoiceDetail", function () {
        $('#lblModelPopProformaInvoice').text('ProformaInvoice Detail')
        $('#FormProformaInvoiceDetail #IsUpdate').val('True');
        $('#FormProformaInvoiceDetail #ID').val(proformaInvoiceDetail.ID);
        $("#FormProformaInvoiceDetail #ProductID").val(proformaInvoiceDetail.ProductID)
        $("#FormProformaInvoiceDetail #hdnProductID").val(proformaInvoiceDetail.ProductID)
        $('#divProductBasicInfo').load("Product/ProductBasicInfo?ID=" + $('#hdnProductID').val(), function () {
        });

        if ($('#hdnProductID').val() != _emptyGuid) {
            $('.divProductModelSelectList').load("ProductModel/ProductModelSelectList?required=required&productID=" + $('#hdnProductID').val())
        }
        else {
            $('.divProductModelSelectList').empty();
            $('.divProductModelSelectList').append('<span class="form-control newinput"><i id="dropLoad" class="fa fa-spinner"></i></span>');
        }
        $("#FormProformaInvoiceDetail #ProductModelID").val(proformaInvoiceDetail.ProductModelID);
        $("#FormProformaInvoiceDetail #hdnProductModelID").val(proformaInvoiceDetail.ProductModelID);
        if ($('#hdnProductModelID').val() != _emptyGuid) {
            $('#divProductBasicInfo').load("ProductModel/ProductModelBasicInfo?ID=" + $('#hdnProductModelID').val(), function () {
            });
        }
        $('#FormProformaInvoiceDetail #ProductSpec').val(proformaInvoiceDetail.ProductSpec);
        $('#FormProformaInvoiceDetail #Qty').val(proformaInvoiceDetail.Qty);
        $('#FormProformaInvoiceDetail #UnitCode').val(proformaInvoiceDetail.UnitCode);
        $('#FormProformaInvoiceDetail #hdnUnitCode').val(proformaInvoiceDetail.UnitCode);
        $('#FormProformaInvoiceDetail #Rate').val(proformaInvoiceDetail.Rate);
        $('#FormProformaInvoiceDetail #Discount').val(proformaInvoiceDetail.Discount);
        if (proformaInvoiceDetail.TaxTypeCode != 0) {
            $('#FormProformaInvoiceDetail #TaxTypeCode').val(proformaInvoiceDetail.TaxType.ValueText);
            $('#FormProformaInvoiceDetail #hdnTaxTypeCode').val(proformaInvoiceDetail.TaxType.ValueText);
        }
        $('#FormProformaInvoiceDetail #hdnCGSTPerc').val(proformaInvoiceDetail.CGSTPerc);
        $('#FormProformaInvoiceDetail #hdnSGSTPerc').val(proformaInvoiceDetail.SGSTPerc);
        $('#FormProformaInvoiceDetail #hdnIGSTPerc').val(proformaInvoiceDetail.IGSTPerc);
        var TaxableAmt = ((parseFloat(proformaInvoiceDetail.Rate) * parseInt(proformaInvoiceDetail.Qty)) - parseFloat(proformaInvoiceDetail.Discount))
        var CGSTAmt = (TaxableAmt * parseFloat(proformaInvoiceDetail.CGSTPerc)) / 100;
        var SGSTAmt = (TaxableAmt * parseFloat(proformaInvoiceDetail.SGSTPerc)) / 100;
        var IGSTAmt = (TaxableAmt * parseFloat(proformaInvoiceDetail.IGSTPerc)) / 100;
        $('#FormProformaInvoiceDetail #CGSTPerc').val(CGSTAmt);
        $('#FormProformaInvoiceDetail #SGSTPerc').val(SGSTAmt);
        $('#FormProformaInvoiceDetail #IGSTPerc').val(IGSTAmt);
        $('#FormProformaInvoiceDetail #CessPerc').val(proformaInvoiceDetail.CessPerc);
        $('#FormProformaInvoiceDetail #CessAmt').val(proformaInvoiceDetail.CessAmt);

        $('#divModelPopProformaInvoice').modal('show');
    });
}

//Delete ProformaInvoice Detail
function ConfirmDeleteProformaInvoiceDetail(this_Obj) {
    debugger;
    _datatablerowindex = _dataTable.ProformaInvoiceDetailList.row($(this_Obj).parents('tr')).index();
    var proformaInvoiceDetail = _dataTable.ProformaInvoiceDetailList.row($(this_Obj).parents('tr')).data();
    if (proformaInvoiceDetail.ID === _emptyGuid) {
        notyConfirm('Are you sure to delete?', 'DeleteCurrentPerformaInvoiceDetail("' + _datatablerowindex + '")');
    }
    else {
        notyConfirm('Are you sure to delete?', 'DeleteProformaInvoiceDetail("' + proformaInvoiceDetail.ID + '")');

    }
}
function DeleteCurrentPerformaInvoiceDetail(_datatablerowindex) {
    var proformaInvoiceDetail = _dataTable.ProformaInvoiceDetailList.rows().data();
    proformaInvoiceDetail.splice(_datatablerowindex, 1);
    _dataTable.ProformaInvoiceDetailList.clear().rows.add(proformaInvoiceDetail).draw(false);
    CalculateTotal();
    notyAlert('success', 'Detail Row deleted successfully');
}
function DeleteProformaInvoiceDetail(ID) {
    debugger;
    if (ID != _emptyGuid && ID != null && ID != '') {
        var data = { "id": ID };
        var ds = {};
        _jsonData = GetDataFromServer("ProformaInvoice/DeleteProformaInvoiceDetail/", data);
        if (_jsonData != '') {
            _jsonData = JSON.parse(_jsonData);
            _message = _jsonData.Message;
            _status = _jsonData.Status;
            _result = _jsonData.Record;
        }
        if (_status == "OK") {
            notyAlert('success', _result.Message);
            var proformaInvoiceDetailList = _dataTable.ProformaInvoiceDetailList.rows().data();
            proformaInvoiceDetailList.splice(_datatablerowindex, 1);
            _dataTable.ProformaInvoiceDetailList.clear().rows.add(proformaInvoiceDetailList).draw(false);
            CalculateTotal();
        }
        if (_status == "ERROR") {
            notyAlert('error', _message);
        }
    }
}

//OtherExpense------------------
function AddOtherExpenseDetailList() {
    $("#divModelProformaInvoicePopBody").load("ProformaInvoice/ProformaInvoiceOtherChargeDetail", function () {
        $('#lblModelPopProformaInvoice').text('OtherExpense Detail')
        $('#divModelPopProformaInvoice').modal('show');
    });
}

function AddOtherExpenseDetailToList() {
    debugger;
    $("#FormOtherExpenseDetail").submit(function () { });
    if ($('#FormOtherExpenseDetail #IsUpdate').val() == 'True') {
        debugger;
        if (($('#divModelProformaInvoicePopBody #OtherChargeCode').val() != "") && ($('#divModelProformaInvoicePopBody #ChargeAmount').val() != "")) {
            var proformaInvoiceOtherExpenseDetailList = _dataTable.ProformaInvoiceOtherChargesDetailList.rows().data();
            proformaInvoiceOtherExpenseDetailList[_datatablerowindex].OtherCharge.Description = $("#divModelProformaInvoicePopBody #OtherChargeCode").val() != "" ? $("#divModelProformaInvoicePopBody #OtherChargeCode option:selected").text().split("-")[0].trim() : "";
            proformaInvoiceOtherExpenseDetailList[_datatablerowindex].OtherCharge.SACCode = $("#hdnOtherChargeSACCode").val();
            proformaInvoiceOtherExpenseDetailList[_datatablerowindex].ChargeAmount = $("#divModelProformaInvoicePopBody #ChargeAmount").val();
            proformaInvoiceOtherExpenseDetailList[_datatablerowindex].OtherChargeCode = $("#divModelProformaInvoicePopBody #OtherChargeCode").val() != "" ? $("#divModelProformaInvoicePopBody #OtherChargeCode").val() : _emptyGuid;
            TaxType = new Object;
            if ($('#divModelProformaInvoicePopBody #TaxTypeCode').val() != null) {
                proformaInvoiceOtherExpenseDetailList[_datatablerowindex].TaxTypeCode = $('#divModelProformaInvoicePopBody #TaxTypeCode').val().split('|')[0];
                TaxType.ValueText = $('#divModelProformaInvoicePopBody #TaxTypeCode').val();
            }
            proformaInvoiceOtherExpenseDetailList[_datatablerowindex].TaxType = TaxType;
            proformaInvoiceOtherExpenseDetailList[_datatablerowindex].CGSTPerc = $('#divModelProformaInvoicePopBody #hdnCGSTPerc').val();
            proformaInvoiceOtherExpenseDetailList[_datatablerowindex].SGSTPerc = $('#divModelProformaInvoicePopBody #hdnSGSTPerc').val();
            proformaInvoiceOtherExpenseDetailList[_datatablerowindex].IGSTPerc = $('#divModelProformaInvoicePopBody #hdnIGSTPerc').val();
            proformaInvoiceOtherExpenseDetailList[_datatablerowindex].AddlTaxPerc = $('#divModelProformaInvoicePopBody #AddlTaxPerc').val() != "" ? $('#divModelProformaInvoicePopBody #AddlTaxPerc').val() : 0;
            proformaInvoiceOtherExpenseDetailList[_datatablerowindex].AddlTaxAmt = $('#divModelProformaInvoicePopBody #AddlTaxAmt').val() != "" ? $('#divModelProformaInvoicePopBody #AddlTaxAmt').val() : 0.00;
            ClearCalculatedFields();
            _dataTable.ProformaInvoiceOtherChargesDetailList.clear().rows.add(proformaInvoiceOtherExpenseDetailList).draw(false);
            CalculateTotal();
            $('#divModelPopProformaInvoice').modal('hide');
            _datatablerowindex = -1;
        }
    }
    else {
        debugger;
        if (($('#divModelProformaInvoicePopBody #OtherChargeCode').val() != "") && ($('#divModelProformaInvoicePopBody #ChargeAmount').val() != "")) {
            if (_dataTable.ProformaInvoiceOtherChargesDetailList.rows().data().length === 0) {
                _dataTable.ProformaInvoiceOtherChargesDetailList.clear().rows.add(GetProformaInvoiceOtherChargesDetailListBySaleOrderID(_emptyGuid, false)).draw(false);
                var proformaInvoiceOtherExpenseDetailList = _dataTable.ProformaInvoiceOtherChargesDetailList.rows().data();
                //saleInvoiceOtherExpenseDetailList.OtherCharge = new Object;
                //saleInvoiceOtherExpenseDetailList.TaxType = new Object;
                proformaInvoiceOtherExpenseDetailList[0].OtherCharge.Description = $("#divModelProformaInvoicePopBody #OtherChargeCode").val() != "" ? $("#divModelProformaInvoicePopBody #OtherChargeCode option:selected").text().split("-")[0].trim() : "";
                proformaInvoiceOtherExpenseDetailList[0].OtherCharge.SACCode = $("#hdnOtherChargeSACCode").val();
                proformaInvoiceOtherExpenseDetailList[0].OtherChargeCode = $("#divModelProformaInvoicePopBody #OtherChargeCode").val() != "" ? $("#divModelProformaInvoicePopBody #OtherChargeCode").val() : _emptyGuid;
                proformaInvoiceOtherExpenseDetailList[0].ChargeAmount = $("#divModelProformaInvoicePopBody #ChargeAmount").val();
                if ($('#divModelProformaInvoicePopBody #TaxTypeCode').val() != null) {
                    proformaInvoiceOtherExpenseDetailList[0].TaxTypeCode = $('#divModelProformaInvoicePopBody #TaxTypeCode').val().split('|')[0];
                }
                proformaInvoiceOtherExpenseDetailList[0].TaxType.ValueText = $('#divModelProformaInvoicePopBody #TaxTypeCode').val();
                proformaInvoiceOtherExpenseDetailList[0].CGSTPerc = $('#divModelProformaInvoicePopBody #hdnCGSTPerc').val();
                proformaInvoiceOtherExpenseDetailList[0].SGSTPerc = $('#divModelProformaInvoicePopBody #hdnSGSTPerc').val();
                proformaInvoiceOtherExpenseDetailList[0].IGSTPerc = $('#divModelProformaInvoicePopBody #hdnIGSTPerc').val();
                proformaInvoiceOtherExpenseDetailList[0].AddlTaxPerc = $('#divModelProformaInvoicePopBody #AddlTaxPerc').val() != "" ? $('#divModelProformaInvoicePopBody #AddlTaxPerc').val() : 0;
                proformaInvoiceOtherExpenseDetailList[0].AddlTaxAmt = $('#divModelProformaInvoicePopBody #AddlTaxAmt').val() != "" ? $('#divModelProformaInvoicePopBody #AddlTaxAmt').val() : 0.00;
                ClearCalculatedFields();
                _dataTable.ProformaInvoiceOtherChargesDetailList.clear().rows.add(proformaInvoiceOtherExpenseDetailList).draw(false);
                CalculateTotal();
                $('#divModelPopProformaInvoice').modal('hide');
            }
            else {
                debugger;
                var proformaInvoiceOtherExpenseDetailList = _dataTable.ProformaInvoiceOtherChargesDetailList.rows().data();
                if (proformaInvoiceOtherExpenseDetailList.length > 0) {
                    var checkpoint = 0;
                    var otherCharge = $('#OtherChargeCode').val();
                    for (var i = 0; i < proformaInvoiceOtherExpenseDetailList.length; i++) {
                        if ((proformaInvoiceOtherExpenseDetailList[i].OtherChargeCode == otherCharge)) {
                            proformaInvoiceOtherExpenseDetailList[i].ChargeAmount = parseFloat(proformaInvoiceOtherExpenseDetailList[i].ChargeAmount) + parseFloat($('#ChargeAmount').val());
                            checkpoint = 1;
                            break;
                        }
                    }
                    if (checkpoint == 1) {
                        ClearCalculatedFields();
                        _dataTable.ProformaInvoiceOtherChargesDetailList.clear().rows.add(proformaInvoiceOtherExpenseDetailList).draw(false);
                        CalculateTotal();
                        $('#divModelPopProformaInvoice').modal('hide');
                    }
                    else if (checkpoint == 0) {
                        ClearCalculatedFields();
                        var ProformaInvoiceOtherChargesDetailVM = new Object();
                        ProformaInvoiceOtherChargesDetailVM.ID = _emptyGuid;
                        var OtherCharge = new Object;
                        OtherCharge.Description = $("#divModelProformaInvoicePopBody #OtherChargeCode").val() != "" ? $("#divModelProformaInvoicePopBody #OtherChargeCode option:selected").text().split("-")[0].trim() : "";
                        ProformaInvoiceOtherChargesDetailVM.OtherCharge = OtherCharge;
                        ProformaInvoiceOtherChargesDetailVM.OtherChargeCode = $("#divModelProformaInvoicePopBody #OtherChargeCode").val() != "" ? $("#divModelProformaInvoicePopBody #OtherChargeCode").val() : _emptyGuid;
                        ProformaInvoiceOtherChargesDetailVM.OtherCharge.SACCode = $("#hdnOtherChargeSACCode").val();
                        ProformaInvoiceOtherChargesDetailVM.ChargeAmount = $("#divModelProformaInvoicePopBody #ChargeAmount").val();
                        var TaxType = new Object();
                        if ($('#divModelProformaInvoicePopBody #TaxTypeCode').val() != null) {
                            ProformaInvoiceOtherChargesDetailVM.TaxTypeCode = $('#divModelProformaInvoicePopBody #TaxTypeCode').val().split('|')[0];
                            TaxType.ValueText = $('#divModelProformaInvoicePopBody #TaxTypeCode').val();
                        }
                        ProformaInvoiceOtherChargesDetailVM.TaxType = TaxType;
                        ProformaInvoiceOtherChargesDetailVM.CGSTPerc = $('#divModelProformaInvoicePopBody #hdnCGSTPerc').val();
                        ProformaInvoiceOtherChargesDetailVM.SGSTPerc = $('#divModelProformaInvoicePopBody #hdnSGSTPerc').val();
                        ProformaInvoiceOtherChargesDetailVM.IGSTPerc = $('#divModelProformaInvoicePopBody #hdnIGSTPerc').val();
                        ProformaInvoiceOtherChargesDetailVM.AddlTaxPerc = $('#divModelProformaInvoicePopBody #AddlTaxPerc').val() != "" ? $('#divModelProformaInvoicePopBody #AddlTaxPerc').val() : 0.00;
                        ProformaInvoiceOtherChargesDetailVM.AddlTaxAmt = $('#divModelProformaInvoicePopBody #AddlTaxAmt').val() != "" ? $('#divModelProformaInvoicePopBody #AddlTaxAmt').val() : 0.00;
                        _dataTable.ProformaInvoiceOtherChargesDetailList.row.add(ProformaInvoiceOtherChargesDetailVM).draw(true);
                        CalculateTotal();
                        $('#divModelPopProformaInvoice').modal('hide');
                    }
                }
            }
        }
    }
    $('[data-toggle="popover"]').popover({
        html: true,
        'trigger': 'hover'
    });
}

//OtherExpense
function BindProformaInvoiceOtherChargesDetailList(id, IsQuotation, IsSaleOrder) {
    debugger;
    var data;
    if (id == _emptyGuid && !(IsQuotation) && !(IsSaleOrder)) {
        data = null;
    }
    else if (id == _emptyGuid && (IsQuotation)) {
        data = GetProformaInvoiceOtherChargesDetailListBySaleOrderID(id, IsQuotation, IsSaleOrder)
    }
    else if (id == _emptyGuid && (IsSaleOrder)) {
        data = GetProformaInvoiceOtherChargesDetailListBySaleOrderID(id, IsQuotation, IsSaleOrder)
    }
    else {
        data = GetProformaInvoiceOtherChargesDetailListBySaleOrderID(id, IsQuotation, IsSaleOrder)
    }

    _dataTable.ProformaInvoiceOtherChargesDetailList = $('#tblProformaInvoiceOtherChargesDetailList').DataTable(
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
             { "data": "OtherCharge.Description", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
              {
                  "data": "OtherCharge.SACCode", "defaultContent": "<i></i>"
              },
             { "data": "ChargeAmount", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
             {
                 "data": "ChargeAmount", render: function (data, type, row) {
                     var CGST = parseFloat(row.CGSTPerc != "" ? row.CGSTPerc : 0);
                     var SGST = parseFloat(row.SGSTPerc != "" ? row.SGSTPerc : 0);
                     var IGST = parseFloat(row.IGSTPerc != "" ? row.IGSTPerc : 0);
                     var CGSTAmt = parseFloat(data * CGST / 100);
                     var SGSTAmt = parseFloat(data * SGST / 100)
                     var IGSTAmt = parseFloat(data * IGST / 100)
                     var GSTAmt = roundoff(parseFloat(CGSTAmt) + parseFloat(SGSTAmt) + parseFloat(IGSTAmt))
                     return '<div class="show-popover text-right" data-html="true" data-placement="left" data-toggle="popover" data-title="<p align=left>Total GST : ₹ ' + GSTAmt + '" data-content=" SGST ' + SGST + '% : ₹ ' + roundoff(parseFloat(SGSTAmt)) + '<br/>CGST ' + CGST + '% : ₹ ' + roundoff(parseFloat(CGSTAmt)) + '<br/> IGST ' + IGST + '% : ₹ ' + roundoff(parseFloat(IGSTAmt)) + '</p>"/>' + GSTAmt
                 }, "defaultContent": "<i></i>"
             },
             {
                 "data": "ChargeAmount", render: function (data, type, row) {
                     var CGST = parseFloat(row.CGSTPerc != "" ? row.CGSTPerc : 0);
                     var SGST = parseFloat(row.SGSTPerc != "" ? row.SGSTPerc : 0);
                     var IGST = parseFloat(row.IGSTPerc != "" ? row.IGSTPerc : 0);
                     var CGSTAmt = parseFloat(data * CGST / 100);
                     var SGSTAmt = parseFloat(data * SGST / 100)
                     var IGSTAmt = parseFloat(data * IGST / 100)
                     var GSTAmt = roundoff(parseFloat(CGSTAmt) + parseFloat(SGSTAmt) + parseFloat(IGSTAmt))
                     var TaxAmt = parseFloat(GSTAmt)
                     if (row.AddlTaxPerc != undefined || row.AddlTaxPerc != null) {
                         var AddlTax = parseFloat(TaxAmt * row.AddlTaxPerc / 100);
                     }
                     else {
                         AddlTax = 0;
                         row.AddlTaxPerc = 0;
                     }
                     return '<div class="show-popover text-right" data-html="true" data-placement="left" data-toggle="popover" data-title="<p align=left>Additional Tax :' + row.AddlTaxPerc + '%</p>"/>' + roundoff(AddlTax)
                 }, "defaultContent": "<i></i>"
             },
             {
                 "data": "ChargeAmount", render: function (data, type, row) {
                     var CGST = parseFloat(row.CGSTPerc != "" ? row.CGSTPerc : 0);
                     var SGST = parseFloat(row.SGSTPerc != "" ? row.SGSTPerc : 0);
                     var IGST = parseFloat(row.IGSTPerc != "" ? row.IGSTPerc : 0);
                     var CGSTAmt = parseFloat(data * CGST / 100);
                     var SGSTAmt = parseFloat(data * SGST / 100)
                     var IGSTAmt = parseFloat(data * IGST / 100)
                     var GSTAmt = roundoff(parseFloat(CGSTAmt) + parseFloat(SGSTAmt) + parseFloat(IGSTAmt))
                     var Total = roundoff(parseFloat(data) + parseFloat(GSTAmt) + parseFloat(row.AddlTaxAmt))

                     return '<div class="show-popover text-right" data-html="true" data-placement="left" data-toggle="popover" data-title="<p align=left>Total : ₹ ' + Total + '" data-content="Charge Amount : ₹ ' + data + '<br/>GST : ₹ ' + GSTAmt + '<br/>Additional Tax : ₹ ' + row.AddlTaxAmt + '</p>"/>' + Total
                 }, "defaultContent": "<i></i>"
             },
             { "data": null, "orderable": false, "defaultContent": ($('#IsDocLocked').val() == "True" || $('#IsUpdate').val() == "False") ? '<a href="#" class="actionLink"  onclick="EditProformaInvoiceOtherChargesDetail(this)" ><i class="fa fa-pencil-square-o" aria-hidden="true"></i></a> <a href="#" class="DeleteLink"  onclick="ConfirmDeleteProformaInvoiceOtherChargeDetail(this)" ><i class="fa fa-trash-o" aria-hidden="true"></i></a>' : "-" },
             ],
             columnDefs: [
                 //{ "targets": [0], "width": "30%" },
                 //{ "targets": [1, 2, 3, 4], "width": "15%" },
                 //{ "targets": [5], "width": "10%" },
                 { className: "text-left", "targets": [0,1] },
                 { className: "text-right", "targets": [ 2, 3, 4, 5] },
                 { className: "text-center", "targets": [6] }
             ],
             destroy: true,
         });
    $('[data-toggle="popover"]').popover({
        html: true,
        'trigger': 'hover',
        'placement': 'top'
    });
}
function GetProformaInvoiceOtherChargesDetailListBySaleOrderID(id, IsQuotation, IsSaleOrder) {
    try {
        var ProformaInvoiceOtherChargesDetailList = [];
        debugger;
        if (IsQuotation) {
            var data = { "quoteID": $('#ProformaInvoiceForm #hdnQuoteID').val() };
            _jsonData = GetDataFromServer("ProformaInvoice/GetProformaInvoiceOtherChargesDetailListByQuotationIDFromQuotation/", data);
        }
        else if (IsSaleOrder) {
            var data = { "saleOrderID": $('#ProformaInvoiceForm #hdnSaleOrderID').val() };
            _jsonData = GetDataFromServer("ProformaInvoice/GetProformaInvoiceOtherChargesDetailListBySaleOrderIDFromSaleOrder/", data);
        }
        else {
            var data = { "proformaInvoiceID": id };
            _jsonData = GetDataFromServer("ProformaInvoice/GetProformaInvoiceOtherChargesDetailListByProformaInvoiceID/", data);
        }

        if (_jsonData != '') {
            _jsonData = JSON.parse(_jsonData);
            _message = _jsonData.Message;
            _status = _jsonData.Status;
            ProformaInvoiceOtherChargesDetailList = _jsonData.Records;
        }
        if (_status == "OK") {
            return ProformaInvoiceOtherChargesDetailList;
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

function EditProformaInvoiceOtherChargesDetail(this_Obj) {
    debugger;
    _datatablerowindex = _dataTable.ProformaInvoiceOtherChargesDetailList.row($(this_Obj).parents('tr')).index();
    var proformaInvoiceOtherChargesDetail = _dataTable.ProformaInvoiceOtherChargesDetailList.row($(this_Obj).parents('tr')).data();
    $("#divModelProformaInvoicePopBody").load("ProformaInvoice/ProformaInvoiceOtherChargeDetail", function () {
        debugger;
        $('#lblModelPopQuotation').text('OtherCharges Detail')
        $('#FormOtherExpenseDetail #IsUpdate').val('True');
        $('#FormOtherExpenseDetail #ID').val(proformaInvoiceOtherChargesDetail.ID);
        $("#FormOtherExpenseDetail #OtherChargeCode").val(proformaInvoiceOtherChargesDetail.OtherChargeCode);
        $("#FormOtherExpenseDetail #hdnOtherChargeCode").val(proformaInvoiceOtherChargesDetail.OtherChargeCode);
        $("#FormOtherExpenseDetail #ChargeAmount").val(proformaInvoiceOtherChargesDetail.ChargeAmount);
       // if (proformaInvoiceOtherChargesDetail.TaxType.Code != 0) {
            $('#FormOtherExpenseDetail #TaxTypeCode').val(proformaInvoiceOtherChargesDetail.TaxType.ValueText);
            $('#FormOtherExpenseDetail #hdnTaxTypeCode').val(proformaInvoiceOtherChargesDetail.TaxType.ValueText);
        //}
        $('#FormOtherExpenseDetail #hdnCGSTPerc').val(proformaInvoiceOtherChargesDetail.CGSTPerc);
        $('#FormOtherExpenseDetail #hdnSGSTPerc').val(proformaInvoiceOtherChargesDetail.SGSTPerc);
        $('#FormOtherExpenseDetail #hdnIGSTPerc').val(proformaInvoiceOtherChargesDetail.IGSTPerc);
        $('#FormOtherExpenseDetail #hdnAddlTaxPerc').val(proformaInvoiceOtherChargesDetail.AddlTaxPerc);
        $('#FormOtherExpenseDetail #hdnAddlTaxAmt').val(proformaInvoiceOtherChargesDetail.AddlTaxAmt);
        $('#FormOtherExpenseDetail #AddlTaxPerc').val(proformaInvoiceOtherChargesDetail.AddlTaxPerc);
        $('#FormOtherExpenseDetail #AddlTaxAmt').val(proformaInvoiceOtherChargesDetail.AddlTaxAmt);

        var CGSTAmt = (proformaInvoiceOtherChargesDetail.ChargeAmount * parseFloat(proformaInvoiceOtherChargesDetail.CGSTPerc)) / 100;
        var SGSTAmt = (proformaInvoiceOtherChargesDetail.ChargeAmount * parseFloat(proformaInvoiceOtherChargesDetail.SGSTPerc)) / 100;
        var IGSTAmt = (proformaInvoiceOtherChargesDetail.ChargeAmount * parseFloat(proformaInvoiceOtherChargesDetail.IGSTPerc)) / 100;
        $('#FormOtherExpenseDetail #CGSTPerc').val(CGSTAmt);
        $('#FormOtherExpenseDetail #SGSTPerc').val(SGSTAmt);
        $('#FormOtherExpenseDetail #IGSTPerc').val(IGSTAmt);
        $('#divModelPopProformaInvoice').modal('show');
    });
}

function ConfirmDeleteProformaInvoiceOtherChargeDetail(this_Obj) {
    _datatablerowindex = _dataTable.ProformaInvoiceOtherChargesDetailList.row($(this_Obj).parents('tr')).index();
    var proformaInvoiceOtherChargesDetail = _dataTable.ProformaInvoiceOtherChargesDetailList.row($(this_Obj).parents('tr')).data();
    if (proformaInvoiceOtherChargesDetail.ID === _emptyGuid) {
        notyConfirm('Are you sure to delete?', 'DeleteCurrentPerformaInvoiceOtherChargeDetail("' + _datatablerowindex + '")');
    }
    else {
        notyConfirm('Are you sure to delete?', 'DeleteProformaInvoiceOtherChargeDetail("' + proformaInvoiceOtherChargesDetail.ID + '")');

    }
}
function DeleteCurrentPerformaInvoiceOtherChargeDetail(_datatablerowindex) {
    var quotationOtherChargeDetailList = _dataTable.ProformaInvoiceOtherChargesDetailList.rows().data();
    quotationOtherChargeDetailList.splice(_datatablerowindex, 1);
    ClearCalculatedFields();
    _dataTable.ProformaInvoiceOtherChargesDetailList.clear().rows.add(quotationOtherChargeDetailList).draw(false);
    CalculateTotal();
    notyAlert('success', 'Detail Row deleted successfully');
}
function DeleteProformaInvoiceOtherChargeDetail(ID) {
    if (ID != _emptyGuid && ID != null && ID != '') {
        var data = { "id": ID };
        var ds = {};
        _jsonData = GetDataFromServer("ProformaInvoice/DeleteProformaInvoiceOtherChargeDetail/", data);
        if (_jsonData != '') {
            _jsonData = JSON.parse(_jsonData);
            _message = _jsonData.Message;
            _status = _jsonData.Status;
            _result = _jsonData.Record;
        }
        if (_status == "OK") {
            notyAlert('success', _result.Message);
            var proformaInvoiceOtherChargeDetailList = _dataTable.ProformaInvoiceOtherChargesDetailList.rows().data();
            proformaInvoiceOtherChargeDetailList.splice(_datatablerowindex, 1);
            ClearCalculatedFields();
            _dataTable.ProformaInvoiceOtherChargesDetailList.clear().rows.add(proformaInvoiceOtherChargeDetailList).draw(false);
            CalculateTotal();
        }
        if (_status == "ERROR") {
            notyAlert('error', _message);
        }
    }
}

//Calculations Methods
function CalculateTotal() {
    var TaxTotal = 0.00, TaxableTotal = 0.00, GrossAmount = 0.00, GrandTotal = 0.00, OtherChargeAmt = 0.00, CessAmt = 0.00;
    var proformaInvoiceDetail = _dataTable.ProformaInvoiceDetailList.rows().data();
    var proformaInvoiceOtherChargeDetail = _dataTable.ProformaInvoiceOtherChargesDetailList.rows().data();
    for (var i = 0; i < proformaInvoiceDetail.length; i++) {
        var TaxableAmt = (parseFloat(proformaInvoiceDetail[i].Rate != "" ? proformaInvoiceDetail[i].Rate : 0) * parseInt(proformaInvoiceDetail[i].Qty != "" ? proformaInvoiceDetail[i].Qty : 1)) - parseFloat(proformaInvoiceDetail[i].Discount != "" ? proformaInvoiceDetail[i].Discount : 0)
        var CGST = parseFloat(proformaInvoiceDetail[i].CGSTPerc != "" ? proformaInvoiceDetail[i].CGSTPerc : 0);
        var SGST = parseFloat(proformaInvoiceDetail[i].SGSTPerc != "" ? proformaInvoiceDetail[i].SGSTPerc : 0);
        var IGST = parseFloat(proformaInvoiceDetail[i].IGSTPerc != "" ? proformaInvoiceDetail[i].IGSTPerc : 0);
        var CGSTAmt = parseFloat(TaxableAmt * CGST / 100);
        var SGSTAmt = parseFloat(TaxableAmt * SGST / 100);
        var IGSTAmt = parseFloat(TaxableAmt * IGST / 100);
        var GSTAmt = parseFloat(CGSTAmt) + parseFloat(SGSTAmt) + parseFloat(IGSTAmt)
        var TaxAmount = TaxableAmt + parseFloat(GSTAmt);
        var Cess = roundoff(parseFloat(GSTAmt * proformaInvoiceDetail[i].CessPerc / 100));
        CessAmt = roundoff(parseFloat(CessAmt) + parseFloat(Cess));
        var GrossTotalAmt = TaxableAmt + GSTAmt
        TaxTotal = roundoff(parseFloat(TaxTotal) + parseFloat(GSTAmt))
        TaxableTotal = roundoff(parseFloat(TaxableTotal) + parseFloat(TaxableAmt))
        GrossAmount = roundoff(parseFloat(GrossAmount) + parseFloat(GrossTotalAmt))
    }
    for (var i = 0; i < proformaInvoiceOtherChargeDetail.length; i++) {
        var CGST = parseFloat(proformaInvoiceOtherChargeDetail[i].CGSTPerc != "" ? proformaInvoiceOtherChargeDetail[i].CGSTPerc : 0);
        var SGST = parseFloat(proformaInvoiceOtherChargeDetail[i].SGSTPerc != "" ? proformaInvoiceOtherChargeDetail[i].SGSTPerc : 0);
        var IGST = parseFloat(proformaInvoiceOtherChargeDetail[i].IGSTPerc != "" ? proformaInvoiceOtherChargeDetail[i].IGSTPerc : 0);
        var CGSTAmt = parseFloat(proformaInvoiceOtherChargeDetail[i].ChargeAmount * CGST / 100);
        var SGSTAmt = parseFloat(proformaInvoiceOtherChargeDetail[i].ChargeAmount * SGST / 100)
        var IGSTAmt = parseFloat(proformaInvoiceOtherChargeDetail[i].ChargeAmount * IGST / 100)
        var GSTAmt = roundoff(parseFloat(CGSTAmt) + parseFloat(SGSTAmt) + parseFloat(IGSTAmt))
        var TaxAmt = parseFloat(proformaInvoiceOtherChargeDetail[i].ChargeAmount) + parseFloat(GSTAmt)
        var AddlTax = parseFloat(GSTAmt * proformaInvoiceOtherChargeDetail[i].AddlTaxPerc / 100);
        var Total = roundoff(parseFloat(proformaInvoiceOtherChargeDetail[i].ChargeAmount) + parseFloat(GSTAmt) + parseFloat(AddlTax))
        OtherChargeAmt = roundoff(parseFloat(OtherChargeAmt) + parseFloat(Total))
    }
    GrossAmount = roundoff(parseFloat(GrossAmount) + parseFloat(OtherChargeAmt) + parseFloat(CessAmt))
    $('#lblTaxTotal').text(TaxTotal);
    $('#lblItemTotal').text(TaxableTotal);
    $('#lblGrossAmount').text(GrossAmount);
    $('#lblGrandTotal').text(GrossAmount);
    $('#lblOtherChargeAmount').text(OtherChargeAmt);
    $('#Discount').trigger('onchange');
    $('#lblCessAmount').text(CessAmt);
}

function ClearCalculatedFields() {
    $('#lblTaxTotal').text('0.00');
    $('#lblItemTotal').text('0.00');
    $('#lblGrossAmount').text('0.00');
    $('#lblCessAmount').text('0.00');
    $('#lblGrandTotal').text('0.00');
    $('#lblOtherChargeAmount').text('0.00');
}

function CalculateGrandTotal(value) {
    var GrandTotal = roundoff(parseFloat($('#lblGrossAmount').text()) - parseFloat(value != "" ? value : 0))
    $('#lblGrandTotal').text(GrandTotal);
}

//=========================================================================================================================
//Email ProformaInvoice
//
function EmailProformaInvoice() {
    debugger;
    $("#divModelEmailProformaInvoiceBody").load("ProformaInvoice/EmailProformaInvoice?ID=" + $('#ProformaInvoiceForm #ID').val() + "&EmailFlag=True", function () {
        $('#lblModelEmailProformaInvoice').text('Email Proforma Invoice')
        $('#divModelEmailProformaInvoice').modal('show');
    });
}

function SendProformaInvoiceEmail() {
    debugger;
    if ($('#hdnEmailSentTo').val() != null && $('#hdnEmailSentTo').val() != "" && $('#Subject').val() != null) {
        $('#hdnMailBodyHeader').val($('#MailBodyHeader').val());
        $('#hdnMailBodyFooter').val($('#MailBodyFooter').val());
        $('#hdnProformaInvoiceEMailContent').val($('#divProformaInvoiceEmailcontainer').html());
        $('#hdnProfInvNo').val($('#ProfInvNo').val());
        $('#hdnContactPerson').val($('#ContactPerson').text());
        $('#hdnProfInvDateFormatted').val($('#ProfInvDateFormatted').val());
        $('#FormProformaInvoiceEmailSend #ID').val($('#ProformaInvoiceForm #ID').val());
        $('#FormProformaInvoiceEmailSend').submit();
    }
    else {
        if ($('#EmailSentTo').val() == null) {
            $('#sentTolbl').css('color', 'red');
            $("#sentTolbl").attr("title", "Please specify at least one recipient");
            $('#sentTovalidationmsglbl').html('__________________________________');
            $('#sentTovalidationmsglbl').css('color', 'red');
            $("#sentTovalidationmsglbl").attr("title", "Please specify at least one recipient");
        }
    }
}
function UpdateProformaInvoiceEmailInfo() {
    $('#hdnMailBodyHeader').val($('#MailBodyHeader').val());
    $('#hdnMailBodyFooter').val($('#MailBodyFooter').val());
    $('#FormUpdateProformaInvoiceEmailInfo #ID').val($('#ProformaInvoiceForm #ID').val());
}
function DownloadProformaInvoice() {
    var bodyContent = $('#divProformaInvoiceEmailcontainer').html();
    var headerContent = $('#hdnHeadContent').html();
    $('#hdnContent').val(bodyContent);
    $('#hdnHeadContent').val(headerContent);
    //var customerName = $("#ProformaInvoiceForm #CustomerID option:selected").text();
    //$('#hdnCustomerName').val(customerName);
}
function PrintProformaInvoice() {
    debugger;
    $("#divModelPrintProformaInvoiceBody").load("ProformaInvoice/PrintProformaInvoice?ID=" + $('#ProformaInvoiceForm #ID').val(), function () {
        $('#lblModelPrintProformaInvoice').text('Print Proforma Invoice');
        $('#divModelPrintProformaInvoice').modal('show');
    });
}
function SaveSuccessUpdateProformaInvoiceEmailInfo(data, status) {
    try {
        debugger;
        var _jsonData = JSON.parse(data)
        //message field will return error msg only
        _message = _jsonData.Message;
        _status = _jsonData.Status;
        _result = _jsonData.Record;
        switch (_status) {
            case "OK":
                //MasterAlert("success", _result.Message)
                $("#divModelEmailProformaInvoiceBody").load("ProformaInvoice/EmailProformaInvoice?ID=" + $('#ProformaInvoiceForm #ID').val() + "&EmailFlag=False", function () {
                    $('#lblModelEmailProformaInvoice').text('Email Attachment')
                });
                break;
            case "ERROR":
                //MasterAlert("success", _message)
                $('#divModelEmailProformaInvoice').modal('hide');
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
function SaveSuccessProformaInvoiceEmailSend(data, status) {
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
                $('#divModelEmailProformaInvoice').modal('hide');
                ResetProformaInvoice();
                break;
            case "ERROR":
                MasterAlert("success", _message)
                $('#divModelEmailProformaInvoice').modal('hide');
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

//---------------------------------------InvoiCeTypeOnChange-------------------------------
function InvoiceTypeOnChange(curObj) {
    if (curObj == 'SB') {
        $('#btnAddServiceBill').css("display", "block")
        $('#btnAddItems').css("display", "none");
    }
    else {
        $('#btnAddServiceBill').css("display", "none");
        $('#btnAddItems').css("display", "block")
    }
}
function AddProformaInvoiceServiceBillList() {
    $("#divModelProformaInvoicePopBody").load("ProformaInvoice/AddProformaInvoiceServiceBill", function () {
        $('#lblModelPopProformaInvoice').text('Service Invoice Detail')
        $('#divModelPopProformaInvoice').modal('show');
    });
}

function AddProformaInvoiceServiceBillToDetailList() {
    debugger;
    $("#FormProformaInvoiceServiceBill").submit(function () { });

    if ($('#FormProformaInvoiceServiceBill #IsUpdate').val() == 'True') {
        if (($('#OtherChargeCode').val() != "") && ($('#Rate').val() != "") && ($('#Qty').val() != "") && ($('#UnitCode').val() != "")) {
            var proformaInvoiceDetailList = _dataTable.ProformaInvoiceDetailList.rows().data();
            proformaInvoiceDetailList[_datatablerowindex].Product = new Object();
            proformaInvoiceDetailList[_datatablerowindex].OtherCharge.Description = $("#divModelProformaInvoicePopBody #OtherChargeCode").val() != "" ? $("#divModelProformaInvoicePopBody #OtherChargeCode option:selected").text().split("-")[0].trim() : "";
            proformaInvoiceDetailList[_datatablerowindex].OtherChargeCode = $("#OtherChargeCode").val();
            proformaInvoiceDetailList[_datatablerowindex].OtherCharge.SACCode = $("#hdnOtherChargeSACCode").val();
            proformaInvoiceDetailList[_datatablerowindex].Qty = $('#Qty').val();
            proformaInvoiceDetailList[_datatablerowindex].UnitCode = $('#UnitCode').val();
            proformaInvoiceDetailList[_datatablerowindex].Unit.Description = $("#UnitCode").val() != "" ? $("#UnitCode option:selected").text().trim() : "";
            //saleInvoiceDetailList[_datatablerowindex].Unit = Unit;
            proformaInvoiceDetailList[_datatablerowindex].Rate = $('#Rate').val();
            proformaInvoiceDetailList[_datatablerowindex].Discount = $('#divModelProformaInvoicePopBody #Discount').val() != "" ? $('#divModelProformaInvoicePopBody #Discount').val() : 0;
            if ($('#divModelProformaInvoicePopBody #TaxTypeCode').val() != null)
                proformaInvoiceDetailList[_datatablerowindex].TaxTypeCode = $('#divModelProformaInvoicePopBody #TaxTypeCode').val().split('|')[0];
            proformaInvoiceDetailList[_datatablerowindex].TaxType = new Object;
            proformaInvoiceDetailList[_datatablerowindex].TaxType.ValueText = $('#divModelProformaInvoicePopBody #TaxTypeCode').val();
            proformaInvoiceDetailList[_datatablerowindex].CGSTPerc = $('#divModelProformaInvoicePopBody #hdnCGSTPerc').val();
            proformaInvoiceDetailList[_datatablerowindex].SGSTPerc = $('#divModelProformaInvoicePopBody #hdnSGSTPerc').val();
            proformaInvoiceDetailList[_datatablerowindex].IGSTPerc = $('#divModelProformaInvoicePopBody #hdnIGSTPerc').val();
            proformaInvoiceDetailList[_datatablerowindex].CessPerc = $('#divModelProformaInvoicePopBody #CessPerc').val() != "" ? $('#divModelProformaInvoicePopBody #CessPerc').val() : 0;
            proformaInvoiceDetailList[_datatablerowindex].CessAmt = $('#divModelProformaInvoicePopBody #CessAmt').val();

            _dataTable.ProformaInvoiceDetailList.clear().rows.add(proformaInvoiceDetailList).draw(false);
            CalculateTotal();
            $('#divModelPopProformaInvoice').modal('hide');
            _datatablerowindex = -1;
        }
    }
    else {
        if (($('#OtherChargeCode').val() != "") && ($('#Rate').val() != "") && ($('#Qty').val() != "") && ($('#UnitCode').val() != "")) {
            if (_dataTable.ProformaInvoiceDetailList.rows().data().length === 0) {
                _dataTable.ProformaInvoiceDetailList.clear().rows.add(GetProformaInvoiceDetailListByProformaInvoiceID(_emptyGuid)).draw(false);
                var proformaInvoiceDetailVM = _dataTable.ProformaInvoiceDetailList.rows().data();
                proformaInvoiceDetailVM.OtherCharge = new Object;
                proformaInvoiceDetailVM.Unit = new Object;
                proformaInvoiceDetailVM.TaxType = new Object;
                proformaInvoiceDetailVM.Product = new Object;
                proformaInvoiceDetailVM[0].OtherCharge.Description = $("#divModelProformaInvoicePopBody #OtherChargeCode").val() != "" ? $("#divModelProformaInvoicePopBody #OtherChargeCode option:selected").text().split("-")[0].trim() : "";
                proformaInvoiceDetailVM[0].OtherChargeCode = $("#OtherChargeCode").val();
                proformaInvoiceDetailVM[0].OtherCharge.SACCode = $("#hdnOtherChargeSACCode").val();
                proformaInvoiceDetailVM[0].Qty = $('#divModelProformaInvoicePopBody #Qty').val();
                proformaInvoiceDetailVM[0].UnitCode = $('#divModelProformaInvoicePopBody #UnitCode').val();
                proformaInvoiceDetailVM[0].Unit.Description = $("#divModelProformaInvoicePopBody #UnitCode").val() != "" ? $("#divModelProformaInvoicePopBody #UnitCode option:selected").text().trim() : "";
                proformaInvoiceDetailVM[0].Rate = $('#divModelProformaInvoicePopBody #Rate').val();
                proformaInvoiceDetailVM[0].Discount = $('#divModelProformaInvoicePopBody #Discount').val() != "" ? $('#divModelProformaInvoicePopBody #Discount').val() : 0;
                proformaInvoiceDetailVM[0].TaxTypeCode = $('#divModelProformaInvoicePopBody #TaxTypeCode').val().split('|')[0];
                proformaInvoiceDetailVM[0].TaxType.ValueText = $('#divModelProformaInvoicePopBody #TaxTypeCode').val();
                proformaInvoiceDetailVM[0].CGSTPerc = $('#divModelProformaInvoicePopBody #hdnCGSTPerc').val();
                proformaInvoiceDetailVM[0].SGSTPerc = $('#divModelProformaInvoicePopBody #hdnSGSTPerc').val();
                proformaInvoiceDetailVM[0].IGSTPerc = $('#divModelProformaInvoicePopBody #hdnIGSTPerc').val();
                proformaInvoiceDetailVM[0].CessPerc = $('#divModelProformaInvoicePopBody #CessPerc').val() != "" ? $('#divModelProformaInvoicePopBody #CessPerc').val() : 0;
                proformaInvoiceDetailVM[0].CessAmt = $('#divModelProformaInvoicePopBody #CessAmt').val();
                ClearCalculatedFields();
                _dataTable.ProformaInvoiceDetailList.clear().rows.add(proformaInvoiceDetailVM).draw(false);
                CalculateTotal();
                $('#divModelPopProformaInvoice').modal('hide');
            }
            else {
                debugger;
                var proformaInvoiceDetailVM = _dataTable.ProformaInvoiceDetailList.rows().data();
                if (proformaInvoiceDetailVM.length > 0) {
                    var checkpoint = 0;
                    for (var i = 0; i < proformaInvoiceDetailVM.length; i++) {
                        if (proformaInvoiceDetailVM[i].OtherChargeCode == $('#OtherChargeCode').val()) {
                            proformaInvoiceDetailVM[i].Qty = parseFloat(proformaInvoiceDetailVM[i].Qty) + parseFloat($('#Qty').val());
                            checkpoint = 1;
                            break;
                        }
                    }
                    if (checkpoint == 1) {
                        _dataTable.ProformaInvoiceDetailList.clear().rows.add(proformaInvoiceDetailVM).draw(false);
                    }
                    else if (checkpoint == 0) {
                        var ProformaInvoiceDetailVM = new Object();
                        ProformaInvoiceDetailVM.Unit = new Object();
                        ProformaInvoiceDetailVM.TaxType = new Object();
                        ProformaInvoiceDetailVM.OtherCharge = new Object;
                        ProformaInvoiceDetailVM.ID = _emptyGuid;
                        ProformaInvoiceDetailVM.Product = new Object;

                        ProformaInvoiceDetailVM.OtherCharge.Description = $("#divModelProformaInvoicePopBody #OtherChargeCode").val() != "" ? $("#divModelProformaInvoicePopBody #OtherChargeCode option:selected").text().split("-")[0].trim() : "";
                        ProformaInvoiceDetailVM.OtherChargeCode = $("#divModelProformaInvoicePopBody #OtherChargeCode").val();
                        ProformaInvoiceDetailVM.OtherCharge.SACCode = $("#hdnOtherChargeSACCode").val();
                        ProformaInvoiceDetailVM.Qty = $('#divModelProformaInvoicePopBody #Qty').val();
                        ProformaInvoiceDetailVM.UnitCode = $('#divModelProformaInvoicePopBody #UnitCode').val();
                        ProformaInvoiceDetailVM.Unit.Description = $("#divModelProformaInvoicePopBody #UnitCode").val() != "" ? $("#divModelProformaInvoicePopBody #UnitCode option:selected").text().trim() : "";
                        ProformaInvoiceDetailVM.Rate = $('#divModelProformaInvoicePopBody #Rate').val();
                        ProformaInvoiceDetailVM.Discount = $('#divModelProformaInvoicePopBody #Discount').val() != "" ? $('#divModelProformaInvoicePopBody #Discount').val() : 0;
                        ProformaInvoiceDetailVM.TaxTypeCode = $('#divModelProformaInvoicePopBody #TaxTypeCode').val().split('|')[0];
                        ProformaInvoiceDetailVM.TaxType.ValueText = $('#divModelProformaInvoicePopBody #TaxTypeCode').val();
                        ProformaInvoiceDetailVM.CGSTPerc = $('#divModelProformaInvoicePopBody #hdnCGSTPerc').val();
                        ProformaInvoiceDetailVM.SGSTPerc = $('#divModelProformaInvoicePopBody #hdnSGSTPerc').val();
                        ProformaInvoiceDetailVM.IGSTPerc = $('#divModelProformaInvoicePopBody #hdnIGSTPerc').val();
                        ProformaInvoiceDetailVM.CessPerc = $('#divModelProformaInvoicePopBody #CessPerc').val() != "" ? $('#divModelProformaInvoicePopBody #CessPerc').val() : 0;
                        ProformaInvoiceDetailVM.CessAmt = $('#divModelProformaInvoicePopBody #CessAmt').val();
                        _dataTable.ProformaInvoiceDetailList.row.add(ProformaInvoiceDetailVM).draw(true);
                    }
                    CalculateTotal();
                    $('#divModelPopProformaInvoice').modal('hide');
                }
            }
        }
    }
    $('[data-toggle="popover"]').popover({
        html: true,
        'trigger': 'hover'
    });
}

function EditProformaInvoiceServiceBill(this_Obj) {
    _datatablerowindex = _dataTable.ProformaInvoiceDetailList.row($(this_Obj).parents('tr')).index();
    var proformaInvoiceDetail = _dataTable.ProformaInvoiceDetailList.row($(this_Obj).parents('tr')).data();
    $("#divModelProformaInvoicePopBody").load("ProformaInvoice/AddProformaInvoiceServiceBill", function () {
        $('#lblModelPopProformaInvoice').text('ProformaInvoice Detail')
        $('#FormProformaInvoiceServiceBill #IsUpdate').val('True');
        $('#FormProformaInvoiceServiceBill #ID').val(proformaInvoiceDetail.ID);

        $("#FormProformaInvoiceServiceBill #OtherChargeCode").val(proformaInvoiceDetail.OtherChargeCode)
        $("#FormProformaInvoiceServiceBill #hdnOtherChargeCode").val(proformaInvoiceDetail.OtherChargeCode)

        $('#FormProformaInvoiceServiceBill #Qty').val(proformaInvoiceDetail.Qty);
        $('#FormProformaInvoiceServiceBill #UnitCode').val(proformaInvoiceDetail.UnitCode);
        $('#FormProformaInvoiceServiceBill #hdnUnitCode').val(proformaInvoiceDetail.UnitCode);
        $('#FormProformaInvoiceServiceBill #Rate').val(proformaInvoiceDetail.Rate);
        $('#FormProformaInvoiceServiceBill #Discount').val(proformaInvoiceDetail.Discount);
        if (proformaInvoiceDetail.TaxTypeCode != 0) {
            $('#FormProformaInvoiceServiceBill #TaxTypeCode').val(proformaInvoiceDetail.TaxType.ValueText);
            $('#FormProformaInvoiceServiceBill #hdnTaxTypeCode').val(proformaInvoiceDetail.TaxType.ValueText);
        }
        $('#FormProformaInvoiceServiceBill #hdnCGSTPerc').val(proformaInvoiceDetail.CGSTPerc);
        $('#FormProformaInvoiceServiceBill #hdnSGSTPerc').val(proformaInvoiceDetail.SGSTPerc);
        $('#FormProformaInvoiceServiceBill #hdnIGSTPerc').val(proformaInvoiceDetail.IGSTPerc);
        var TaxableAmt = ((parseFloat(proformaInvoiceDetail.Rate) * parseInt(proformaInvoiceDetail.Qty)) - parseFloat(proformaInvoiceDetail.Discount))
        var CGSTAmt = (TaxableAmt * parseFloat(proformaInvoiceDetail.CGSTPerc)) / 100;
        var SGSTAmt = (TaxableAmt * parseFloat(proformaInvoiceDetail.SGSTPerc)) / 100;
        var IGSTAmt = (TaxableAmt * parseFloat(proformaInvoiceDetail.IGSTPerc)) / 100;
        $('#FormProformaInvoiceServiceBill #CGSTPerc').val(CGSTAmt);
        $('#FormProformaInvoiceServiceBill #SGSTPerc').val(SGSTAmt);
        $('#FormProformaInvoiceServiceBill #IGSTPerc').val(IGSTAmt);
        $('#FormProformaInvoiceServiceBill #CessPerc').val(proformaInvoiceDetail.CessPerc);
        $('#FormProformaInvoiceServiceBill #CessAmt').val(proformaInvoiceDetail.CessAmt);

        $('#divModelPopProformaInvoice').modal('show');
    });
}
function EditRedirectToDocument(id) {

    OnServerCallBegin();

    $("#divProformaInvoiceForm").load("ProformaInvoice/ProformaInvoiceForm?id=" + id, function (responseTxt, statusTxt, xhr) {
        if (statusTxt == "success") {
            OnServerCallComplete();
            openNav();
            $('#lblProformaInvoiceInfo').text($('#ProfInvNo').val());
            if ($('#IsDocLocked').val() == "True") {
                ChangeButtonPatchView("ProformaInvoice", "btnPatchProformaInvoiceNew", "Edit", id);
            }
            else {
                ChangeButtonPatchView("ProformaInvoice", "btnPatchProformaInvoiceNew", "LockDocument", id);
            }
            BindProformaInvoiceDetailList(id);
            BindProformaInvoiceOtherChargesDetailList(id);
            CalculateTotal();
            $('#divCustomerBasicInfo').load("Customer/CustomerBasicInfo?ID=" + $('#hdnCustomerID').val());
            clearUploadControl();
            PaintImages(id);
        }
        else {
            console.log("Error: " + xhr.status + ": " + xhr.statusText);
        }
    });
}

//To get SACCode
function GetOtherCharge(value) {
    try {
        debugger;
        var otherCharge;
        var data = { "code": value };
        _jsonData = GetDataFromServer("OtherCharge/GetOtherCharge/", data);
        if (_jsonData != '') {
            _jsonData = JSON.parse(_jsonData);
            _message = _jsonData.Message;
            _status = _jsonData.Status;
            otherCharge = _jsonData.Record;
        }

        if (_status == "OK") {
            return otherCharge;
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