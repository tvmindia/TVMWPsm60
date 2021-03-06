﻿var _dataTable = {};
var _emptyGuid = "00000000-0000-0000-0000-000000000000";
var _datatablerowindex = -1;
var _jsonData = {};
var _message = "";
var _status = "";
var _result = "";
var _SlNo = 1;
var _SlNoOtherCharge = 1;
//---------------------------------------Docuement Ready--------------------------------------------------//
$(document).ready(function () {
    try {
        BindOrReloadSaleInvoiceTable('Init');
        $('#tblSaleInvoice tbody').on('dblclick', 'td', function () {
            EditSaleInvoice(this);
        });
        if ($('#RedirectToDocument').val() != "") {
            if ($('#RedirectToDocument').val() === _emptyGuid) {
                AddSaleInvoice();
            }
            else {
                EditRedirectToDocument($('#RedirectToDocument').val());
            }
        }
        //function GetSaleInvoiceXmlFile()
        //{
        //    try{
        //        var saleInvoiceList = [];
        //        _jsonData = GetDataFromServer("SaleInvoice/GetSaleInvoiceXmlFile/");
        //        if (_jsonData != '') {
        //            _jsonData = JSON.parse(_jsonData);
        //            _message = _jsonData.Message;
        //            _status = _jsonData.Status;
        //            saleInvoiceList = _jsonData.Records;
        //        }
        //        if (_status == "OK") {
        //            return saleInvoiceList;
        //        }
        //        if (_status == "ERROR") {
        //            notyAlert('error', _message);
        //        }
        //    }
        //    catch (e) {
        //        console.log(e.message);
        //    }
        //}
    }
    catch (e) {
        console.log(e.message);
    }
    $("#AdvDocumentStatusCode,#AdvEmailSentStatus").select2({
        dropdownParent: $(".divboxASearch")
    });

    $('.select2').addClass('form-control newinput');
});
//function bind the SaleInvoice list checking search and filter
function BindOrReloadSaleInvoiceTable(action) {
    try {
        //creating advancesearch object
        SaleInvoiceAdvanceSearchViewModel = new Object();
        DataTablePagingViewModel = new Object();
        DataTablePagingViewModel.Length = 0;
        var SearchValue = $('#hdnSearchTerm').val();
        var SearchTerm = $('#SearchTerm').val();
        $('#hdnSearchTerm').val($('#SearchTerm').val());
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
               // $('.divboxASearch #AdvApprovalStatusCode').val('').trigger('change');
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
               // $('.divboxASearch #AdvApprovalStatusCode').val('');
                $('#AdvEmailSentStatus').val('');
                break;
            case 'Search':
                if ((SearchTerm == SearchValue) && ($('.divboxASearch #AdvFromDate').val() == "") && ($('.divboxASearch #AdvToDate').val() == "") && ($('.divboxASearch #AdvAreaCode').val() == "") && ($('.divboxASearch #AdvCustomerID').val() == "") && ($('.divboxASearch #AdvBranchCode').val() == "") && ($('.divboxASearch #AdvDocumentStatusCode').val() == "") && ($('.divboxASearch #AdvDocumentOwnerID').val() == "") && ($('#AdvEmailSentStatus').val() == "")) {
                    return true;
                }
                break;
            case 'Export':
                DataTablePagingViewModel.Length = -1;
                SaleInvoiceAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
                SaleInvoiceAdvanceSearchViewModel.SearchTerm = $('#SearchTerm').val() == "" ? null : $('#SearchTerm').val();
                SaleInvoiceAdvanceSearchViewModel.AdvFromDate = $('.divboxASearch #AdvFromDate').val() == "" ? null : $('.divboxASearch #AdvFromDate').val();
                SaleInvoiceAdvanceSearchViewModel.AdvToDate = $('.divboxASearch #AdvToDate').val() == "" ? null : $('.divboxASearch #AdvToDate').val();
                SaleInvoiceAdvanceSearchViewModel.AdvAreaCode = $('.divboxASearch #AdvAreaCode').val() == "" ? null : $('.divboxASearch #AdvAreaCode').val();
                SaleInvoiceAdvanceSearchViewModel.AdvCustomerID = $('.divboxASearch #AdvCustomerID').val() == "" ? _emptyGuid : $('.divboxASearch #AdvCustomerID').val();
                SaleInvoiceAdvanceSearchViewModel.AdvBranchCode = $('.divboxASearch #AdvBranchCode').val() == "" ? null : $('.divboxASearch #AdvBranchCode').val();
                SaleInvoiceAdvanceSearchViewModel.AdvDocumentStatusCode = $('.divboxASearch #AdvDocumentStatusCode').val() == "" ? null : $('.divboxASearch #AdvDocumentStatusCode').val();
                SaleInvoiceAdvanceSearchViewModel.AdvDocumentOwnerID = $('.divboxASearch #AdvDocumentOwnerID').val() == "" ? _emptyGuid : $('.divboxASearch #AdvDocumentOwnerID').val();
               // SaleInvoiceAdvanceSearchViewModel.AdvApprovalStatusCode = $('.divboxASearch #AdvApprovalStatusCode').val() == "" ? null : $('.divboxASearch #AdvApprovalStatusCode').val();
                SaleInvoiceAdvanceSearchViewModel.AdvEmailSentStatus = $('#AdvEmailSentStatus').val() == "" ? null : $('#AdvEmailSentStatus').val();
                $('#AdvanceSearch').val(JSON.stringify(SaleInvoiceAdvanceSearchViewModel));
                $('#FormExcelExport').submit();
                return true;
                break;
            default:
                break;
        }
        SaleInvoiceAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
        SaleInvoiceAdvanceSearchViewModel.SearchTerm = $('#SearchTerm').val();
        SaleInvoiceAdvanceSearchViewModel.AdvFromDate = $('.divboxASearch #AdvFromDate').val();
        SaleInvoiceAdvanceSearchViewModel.AdvToDate = $('.divboxASearch #AdvToDate').val();
        SaleInvoiceAdvanceSearchViewModel.AdvAreaCode = $('.divboxASearch #AdvAreaCode').val();
        SaleInvoiceAdvanceSearchViewModel.AdvCustomerID = $('.divboxASearch #AdvCustomerID').val();
        SaleInvoiceAdvanceSearchViewModel.AdvBranchCode = $('.divboxASearch #AdvBranchCode').val();
        SaleInvoiceAdvanceSearchViewModel.AdvDocumentStatusCode = $('.divboxASearch #AdvDocumentStatusCode').val();
        SaleInvoiceAdvanceSearchViewModel.AdvDocumentOwnerID = $('.divboxASearch #AdvDocumentOwnerID').val();
       // SaleInvoiceAdvanceSearchViewModel.AdvApprovalStatusCode = $('.divboxASearch #AdvApprovalStatusCode').val();
        SaleInvoiceAdvanceSearchViewModel.AdvEmailSentStatus = $('#AdvEmailSentStatus').val();
        //apply datatable plugin on SaleInvoice table
        _dataTable.SaleInvoiceList = $('#tblSaleInvoice').DataTable(
        {
            dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',           
            ordering: false,
            searching: false,
            paging: true,
            lengthChange: false,
            processing: true,
            autoWidth:false,
            language: {
                "processing": "<div class='spinner'><div class='bounce1'></div><div class='bounce2'></div><div class='bounce3'></div></div>"
            },
            serverSide: true,
            ajax: {
                url: "SaleInvoice/GetAllSaleInvoice/",
                data: { "SaleInvoiceAdvanceSearchVM": SaleInvoiceAdvanceSearchViewModel },
                type: 'POST'
            },
            pageLength: 8,
            columns: [
                 { "data": "SaleInvNo", render: function (data, type, row) {
                         return row.SaleInvNo + "</br>" + "<img src='./Content/images/datePicker.png' height='10px'>" + "&nbsp;" + row.SaleInvDateFormatted;
                     }, "defaultContent": "<i>-</i>"
                 },
                 {
                   "data": "Customer.CompanyName", render: function (data, type, row) {
                       return "<img src='./Content/images/contact.png' height='10px'>" + "&nbsp;" + (row.Customer.ContactPerson == null ? "" : row.Customer.ContactPerson) + "</br>" + "<img src='./Content/images/organisation.png' height='10px'>" + "&nbsp;" + data;
                   }, "defaultContent": "<i>-</i>"
                 },
                 {
                     "data": "ReferenceNo", render: function (data, type, row) {
                         debugger;
                         if(row.SaleOrder.SaleOrderNo!=null)
                         {
                             return row.SaleOrder.SaleOrderNo;
                         }
                         else if(row.Quotation.QuoteNo!=null){
                             return row.Quotation.QuoteNo;
                         }
                         else if (row.ProformaInvoice.ProfInvNo != null) {
                             return row.ProformaInvoice.ProfInvNo;
                         }
                     //return (row.SaleOrder.SaleOrderNo == null ? row.Quotation.QuoteNo : row.SaleOrder.SaleOrderNo);
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
                 { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink"  onclick="EditSaleInvoice(this)" ><i class="fa fa-pencil-square-o" aria-hidden="true"></i></a>' },
            ],
            columnDefs: [{ className: "text-left", "targets": [0, 1, 2, 3, 4,5] },
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
                $('#tblSaleInvoice').fadeIn(100);
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
function ResetSaleInvoiceList() {
    $(".searchicon").removeClass('filterApplied');
    BindOrReloadSaleInvoiceTable('Reset');
}
//function export data to excel
function ExportSaleInvoiceData() {   
    BindOrReloadSaleInvoiceTable('Export');
}
// add SaleInvoice section
function AddSaleInvoice() {
    debugger;
    //this will return form body(html)
    OnServerCallBegin();
    $("#divSaleInvoiceForm").load("SaleInvoice/SaleInvoiceForm?id=" + _emptyGuid, function (responseTxt, statusTxt, xhr) {
        if (statusTxt == "success") {
            OnServerCallComplete();
            openNav();
            $('#lblSaleInvoiceInfo').text('<<Sale Invoice No.>>');
            ChangeButtonPatchView("SaleInvoice", "btnPatchSaleInvoiceNew", "Add");
            BindSaleInvoiceDetailList(_emptyGuid);
            BindSaleInvoiceOtherChargesDetailList(_emptyGuid);           
            
        }
        else {
            console.log("Error: " + xhr.status + ": " + xhr.statusText);
        }
    });
}


function EditSaleInvoice(this_Obj) {
    debugger;
    OnServerCallBegin();
    var SaleInvoice = _dataTable.SaleInvoiceList.row($(this_Obj).parents('tr')).data();
    //this will return form body(html)
    $("#divSaleInvoiceForm").load("SaleInvoice/SaleInvoiceForm?id=" + SaleInvoice.ID, function (responseTxt, statusTxt, xhr) {
        if (statusTxt == "success") {
            OnServerCallComplete();
            openNav();
            //$('#CustomerID').trigger('change');
            debugger;
            if ($('#IsDocLocked').val() == "True") {
                ChangeButtonPatchView("SaleInvoice", "btnPatchSaleInvoiceNew", "Edit", SaleInvoice.ID);
            }
            else {
                //$('.switch-input').prop('disabled', true);
                //$('.switch-label,.switch-handle').addClass('switch-disabled').addClass('disabled');
                //$('.switch-label').attr('title', 'Document Locked');
                ChangeButtonPatchView("SaleInvoice", "btnPatchSaleInvoiceNew", "LockDocument", SaleInvoice.ID);
            }
            _SlNo = 1;
            BindSaleInvoiceDetailList(SaleInvoice.ID);
            _SlNoOtherCharge = 1;
            BindSaleInvoiceOtherChargesDetailList(SaleInvoice.ID);
            $('#lblSaleInvoiceInfo').text(SaleInvoice.SaleInvNo);
            CalculateTotal();
            $('#divCustomerBasicInfo').load("Customer/CustomerBasicInfo?ID=" + $('#hdnCustomerID').val());
            clearUploadControl();
            PaintImages(SaleInvoice.ID);
            debugger;
            if (SaleInvoice.InvoiceType == "SB") {
                $('#btnAddItems').css("display", "none");
                $('#btnAddOtherExpenses').css("display", "none");
                $('#divSaleInvoiceOtherChargesDetailList').hide();
               
            }
            //if (SaleInvoice.DocumentStatus.Description == "OPEN") {
            //    $('.switch-input').prop('checked', true);

            //} else {
            //    $('.switch-input').prop('checked', false);

            //}
        }
        else {
            console.log("Error: " + xhr.status + ": " + xhr.statusText);
        }
    });
}
function ResetSaleInvoice(event) {
    $("#divSaleInvoiceForm").load("SaleInvoice/SaleInvoiceForm?id=" + $('#SaleInvoiceForm #ID').val(), function (responseTxt, statusTxt, xhr) {
        if (statusTxt == "success") {
            //if ($('#hdnDescription').val() == "OPEN") {
            //    $('.switch-input').prop('checked', true);

            //} else {
            //    $('.switch-input').prop('checked', false);

            //}
            if ($('#IsUpdate').val() == "False") {
                ChangeButtonPatchView("SaleInvoice", "btnPatchSaleInvoiceNew", "Add", $('#SaleInvoiceForm #ID').val());
            }
            else if ($('#IsDocLocked').val() == "True") {
                ChangeButtonPatchView("SaleInvoice", "btnPatchSaleInvoiceNew", "Edit", $('#SaleInvoiceForm #ID').val());
            }
            else {
                ChangeButtonPatchView("SaleInvoice", "btnPatchSaleInvoiceNew", "LockDocument", $('#SaleInvoiceForm #ID').val());
            }
            _SlNo = 1;
            BindSaleInvoiceDetailList($('#ID').val());
            _SlNoOtherCharge = 1;
            BindSaleInvoiceOtherChargesDetailList($('#ID').val());           
            CalculateTotal();
            clearUploadControl();
            PaintImages($('#SaleInvoiceForm #ID').val());
            $('#divCustomerBasicInfo').load("Customer/CustomerBasicInfo?ID=" + $('#SaleInvoiceForm #hdnCustomerID').val());
        }
        else {
            console.log("Error: " + xhr.status + ": " + xhr.statusText);
        }
        if ($('#hdnInvoiceType').val() == "SB") {
            $('#divSaleInvoiceOtherChargesDetailList').hide();
        }

        if (event.value == "Quotation") {
            $('#RadioQuoteID').attr('checked', true);
            $('#divSaleOrderSelectList').hide();
            $('#divQuotationSelectList').show();
            $('#divProformaInvoiceSelectList').hide();
            $('#QuoteID').prop('disabled', false);
            $('#SaleOrderID').prop('disabled', true);
            $('#ProfInvID').prop('disabled', true);
        }
        else if (event.value == "SaleOrder") {
            $('#RadioSaleOrderID').attr('checked', true);
            $('#divSaleOrderSelectList').show();
            $('#divQuotationSelectList').hide();
            $('#divProformaInvoiceSelectList').hide();
            $('#QuoteID').prop('disabled', true);
            $('#SaleOrderID').prop('disabled', false);
            $('#ProfInvID').prop('disabled', true);
        }
        else if (event.value == "ProformaInvoice") {
            $('#RadioProfInvID').attr('checked', true);
            $('#divSaleOrderSelectList').hide();
            $('#divQuotationSelectList').hide();
            $('#divProformaInvoiceSelectList').show();
            $('#QuoteID').prop('disabled', true);
            $('#SaleOrderID').prop('disabled', true);
            $('#ProfInvID').prop('disabled', false);
        }
        
    });    
}
function SaveSaleInvoice() {
    debugger;
    var saleInvoiceDetailList = _dataTable.SaleInvoiceDetailList.rows().data().toArray();
    $('#DetailJSON').val(JSON.stringify(saleInvoiceDetailList));
    if ($('#hdnInvoiceType').val() != "SB")
    {
        var saleInvoiceOtherChargesDetailList = _dataTable.SaleInvoiceOtherChargesDetailList.rows().data().toArray();
        $('#OtherChargesDetailJSON').val(JSON.stringify(saleInvoiceOtherChargesDetailList));
    }
   // var saleInvoiceOtherChargesDetailList = _dataTable.SaleInvoiceOtherChargesDetailList.rows().data().toArray();  
   // $('#OtherChargesDetailJSON').val(JSON.stringify(saleInvoiceOtherChargesDetailList));
    $('#btnInsertUpdateSaleInvoice').trigger('click');
}
function ApplyFilterThenSearch() {
    $(".searchicon").addClass('filterApplied');
    CloseAdvanceSearch();
    BindOrReloadSaleInvoiceTable('Search');
}
function SaveSuccessSaleInvoice(data, status) {
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
                $("#divSaleInvoiceForm").load("SaleInvoice/SaleInvoiceForm?id=" + _result.ID, function () {
                    ChangeButtonPatchView("SaleInvoice", "btnPatchSaleInvoiceNew", "Edit", _result.ID);
                    _SlNo = 1;
                    BindSaleInvoiceDetailList(_result.ID);
                    if ($("#hdnInvoiceType").val() == "RB") {
                        _SlNoOtherCharge = 1;
                        BindSaleInvoiceOtherChargesDetailList(_result.ID);
                    }
                    CalculateTotal();
                    clearUploadControl();
                    PaintImages(_result.ID);
                    $('#lblSaleInvoiceInfo').text(_result.SaleInvoiceNo);
                    $('#divCustomerBasicInfo').load("Customer/CustomerBasicInfo?ID=" + $('#SaleInvoiceForm #hdnCustomerID').val());
                    if ($("#hdnInvoiceType").val() == "SB") {
                        $('#btnAddItems').css("display", "none");
                        $('#btnAddOtherExpenses').css("display", "none");
                        $('#divSaleInvoiceOtherChargesDetailList').hide();
                        $("#divInvoiceType").hide();

                    }
                    //if ($('#hdnDescription').val() == "OPEN") {
                    //    $('.switch-input').prop('checked', true);

                    //} else {
                    //    $('.switch-input').prop('checked', false);
                    //}


                });
                ChangeButtonPatchView("SaleInvoice", "btnPatchSaleInvoiceNew", "Edit", _result.ID);
                BindOrReloadSaleInvoiceTable('Init');
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

function DeleteSaleInvoice() {
    notyConfirm('Are you sure to delete?', 'DeleteSaleInvoiceItem("' + $('#SaleInvoiceForm #ID').val() + '")');
}
function DeleteSaleInvoiceItem(id) {
    try {
        if (id) {
            var data = { "id": id };
            _jsonData = GetDataFromServer("SaleInvoice/DeleteSaleInvoice/", data);
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
                    ChangeButtonPatchView("SaleInvoice", "btnPatchSaleInvoiceNew", "Add");
                    ResetSaleInvoice();
                    $('#lblSaleInvoiceInfo').text('<<Sale Invoice No.>>');
                    BindOrReloadSaleInvoiceTable('Init');
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
function BindSaleInvoiceDetailList(id, IsSaleOrder, IsQuotation, IsProformaInvoice) {
    debugger;
    var data;
    if (id == _emptyGuid && !(IsSaleOrder) && !(IsQuotation) && !(IsProformaInvoice)) {
        data = null;
    }
    else if (id == _emptyGuid && (IsSaleOrder)) {
        data = GetSaleInvoiceDetailListBySaleInvoiceID(id, IsSaleOrder, IsQuotation, IsProformaInvoice)
    }
    else if (id == _emptyGuid && (IsQuotation)) {
        data = GetSaleInvoiceDetailListBySaleInvoiceID(id, IsSaleOrder, IsQuotation, IsProformaInvoice)
    }
    else if (id == _emptyGuid && (IsProformaInvoice)) {
        data = GetSaleInvoiceDetailListBySaleInvoiceID(id, IsSaleOrder, IsQuotation, IsProformaInvoice)
    }
    else {
        data = GetSaleInvoiceDetailListBySaleInvoiceID(id, IsSaleOrder, IsQuotation, IsProformaInvoice)
    }
    _dataTable.SaleInvoiceDetailList = $('#tblSaleInvoiceDetails').DataTable(
         {
             dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
             order: [],
             searching: false,
             paging: false,
             ordering: false,
             bInfo: false,
             data: data,//id == _emptyGuid ? null : GetSaleInvoiceDetailListBySaleInvoiceID(id),
             language: {
                 search: "_INPUT_",
                 searchPlaceholder: "Search"
             },
             columns: [
                  {
                      "data": "", render: function (data, type, row) {
                          return _SlNo++
                      }, "defaultContent": "<i></i>", "width": "2%"
                  },
             {
                 "data": "Product.Code", render: function (data, type, row) {
                     if (data != "" && data!=undefined) {
                         debugger;
                         return '<div style="width:100%" class="show-popover" data-html="true" data-placement="top" data-toggle="popover" data-title="<p align=left>Product Specification" data-content="' + (row.ProductSpec !== null ? row.ProductSpec.replace("\n", "<br>").replace(/"/g, "&quot") : "") + '</p>"/>' +
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
                     //if ((row.OtherCharge.SACCode == null||row.OtherCharge.SACCode=="") && row.Product.HSNCode !== null) {
                     //    return row.Product.HSNCode;
                     //}
                     //else if (row.OtherCharge.SACCode !== null && (row.Product.HSNCode == null || row.Product.HSNCode=="")) {
                     //    return row.OtherCharge.SACCode;
                     //}
                     if (data != null || data == "") {
                         return row.Product.HSNCode;
                     }
                     else if(data!=null || data==""){
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
                 "data": "Rate", render: function (data, type, row) { return formatCurrency(roundoff(parseFloat(data))) }, "defaultContent": "<i></i>"
             },
             { "data": "Discount", render: function (data, type, row) { return formatCurrency(roundoff(parseFloat(data))) }, "defaultContent": "<i></i>" },
             {//Taxable
                 "data": "Rate", render: function (data, type, row) {
                     var Total = roundoff(parseFloat(data != "" ? data : 0) * parseInt(row.Qty != "" ? row.Qty : 1))
                     var Discount = roundoff(parseFloat(row.Discount != "" ? row.Discount : 0))
                     var Taxable = Total - Discount
                     return '<div class="show-popover text-right" data-html="true" data-placement="left" data-toggle="popover" data-title="<p align=left>Taxable :  ' + formatCurrency(Taxable) + '" data-content="Net Total :  ' + formatCurrency(Total) + '<br/> Discount :  -' + formatCurrency(Discount) + '</p>"/>' + formatCurrency(roundoff(parseFloat(Taxable)))
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
                     var GSTPerc = CGST + SGST + IGST
                     return '<div class="show-popover text-right" data-html="true" data-placement="left" data-toggle="popover" data-title="<p align=left>Total GST :  ' + formatCurrency(GSTAmt) + '" data-content=" SGST ' + SGST + '% :  ' + formatCurrency(roundoff(SGSTAmt)) + '<br/>CGST ' + CGST + '% :  ' + formatCurrency(roundoff(parseFloat(CGSTAmt))) + '<br/> IGST ' + IGST + '% :  ' + formatCurrency(roundoff(parseFloat(IGSTAmt))) + '</p>"/>' + formatCurrency(GSTAmt) + '<br/><i style="font-size:10px;color:brown">GST(%) -</i>' + GSTPerc
                 }, "defaultContent": "<i></i>"
             },
            {//Cess
                "data": "CessAmt", render: function (data, type, row) {
                    return formatCurrency(data)+'<br/><i style="font-size:10px;color:brown">Cess(%) -</i>' + row.CessPerc 
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
                    return '<div class="show-popover text-right" data-html="true" data-placement="left" data-toggle="popover" data-title="<p align=left>Grand Total :  ' + formatCurrency(GrandTotal) + '" data-content="Taxable :  ' + formatCurrency(TaxableAmt) + '<br/>GST :  ' + formatCurrency(GSTAmt) + '</p>"/>' + formatCurrency(GrandTotal)
                }, "defaultContent": "<i></i>"
            },
            {
                "data": null, "orderable": false,render: function(data,type,row){
                    if (row.Product.Code != "" && row.Product.Code != null)
                    {
                        return ($('#IsDocLocked').val() == "True" || $('#IsUpdate').val() == "False") ? '<a href="#" class="actionLink"  onclick="EditSaleInvoiceDetail(this)" ><i class="fa fa-pencil-square-o" aria-hidden="true"></i></a> <a href="#" class="DeleteLink"  onclick="ConfirmDeleteSaleInvoiceDetail(this)" ><i class="fa fa-trash-o" aria-hidden="true"></i></a>':"-"
                    }
                    else
                    {
                        return ($('#IsDocLocked').val() == "True" || $('#IsUpdate').val() == "False") ? '<a href="#" class="actionLink"  onclick="EditSaleInvoiceServiceBill(this)" ><i class="fa fa-pencil-square-o" aria-hidden="true"></i></a> <a href="#" class="DeleteLink"  onclick="ConfirmDeleteSaleInvoiceDetail(this)" ><i class="fa fa-trash-o" aria-hidden="true"></i></a>':"-"
                    }
                },"defaultContent": "<i></i>"
             },
             ],
             columnDefs: [
                 { className: "text-right", "targets": [3, 4, 5, 6, 7, 8,9] },
                 { className: "text-left", "targets": [1,2] },
                 { className: "text-center", "targets": [0,3,10] }
             ]
         });
    $('[data-toggle="popover"]').popover({
        html: true,
        'trigger': 'hover'        
    });
}
function GetSaleInvoiceDetailListBySaleInvoiceID(id, IsSaleOrder, IsQuotation, IsProformaInvoice) {
    try {
        debugger;
        if ((IsSaleOrder == undefined || IsQuotation == undefined || IsProformaInvoice==undefined) && id != _emptyGuid)
            _SlNo = 1;
        else if ((IsSaleOrder == undefined || IsQuotation == undefined || IsProformaInvoice == undefined) && id == _emptyGuid)
            _SlNo = 0;
        else
            _SlNo = 1;
        var saleInvoiceDetailList = [];
        if (IsSaleOrder) {
            var data = { "saleOrderID": $('#SaleInvoiceForm #hdnSaleOrderID').val() };
            _jsonData = GetDataFromServer("SaleInvoice/GetSaleInvoiceDetailListBySaleOrderIDFromSaleOrder/", data);
        }
        else if (IsQuotation) {
            var data = { "quoteID": $('#SaleInvoiceForm #hdnQuoteID').val() };
            _jsonData = GetDataFromServer("SaleInvoice/GetSaleInvoiceDetailListByQuotationIDFromQuotation/", data);
        }
        else if (IsProformaInvoice) {
            var data = { "proformaInvoiceID": $('#SaleInvoiceForm #hdnProfInvID').val() };
            _jsonData = GetDataFromServer("SaleInvoice/GetSaleInvoiceDetailListByProfInvIDFromProformaInvoice/", data);
            if ($('#hdnInvoiceType').val() == "SB") {
                $('#divSaleInvoiceOtherChargesDetailList').hide();
            }
            
        }
        else {
            var data = { "saleInvoiceID": id };
            _jsonData = GetDataFromServer("SaleInvoice/GetSaleInvoiceDetailListBySaleInvoiceID/", data);
        }

        if (_jsonData != '') {
            _jsonData = JSON.parse(_jsonData);
            _message = _jsonData.Message;
            _status = _jsonData.Status;
            saleInvoiceDetailList = _jsonData.Records;
          
        }
        if (_status == "OK") {
            return saleInvoiceDetailList;
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


//Add SaleInvoice Detail
function AddSaleInvoiceDetailList() {
    $("#divModelSaleInvoicePopBody").load("SaleInvoice/AddSaleInvoiceDetail?update=false", function () {
        $('#lblModelPopSaleInvoice').text('SaleInvoice Detail')
        $('#divModelPopSaleInvoice').modal('show');
    });
}
function AddSaleInvoiceDetailToList() {
    debugger;
    $("#FormSaleInvoiceDetail").submit(function () { });
    if ($('#FormSaleInvoiceDetail #IsUpdate').val() == 'True') {
        _SlNo = 1;
        if (($('#ProductID').val() != "") && ($('#ProductModelID').val() != "") && ($('#Rate').val() > 0) && ($('#Qty').val() > 0) && ($('#UnitCode').val() != "")) {
            var saleInvoiceDetailList = _dataTable.SaleInvoiceDetailList.rows().data();
            //saleInvoiceDetailList[_datatablerowindex].Product.Code = $("#ProductID").val() != "" ? $("#ProductID option:selected").text().split("-")[0].trim() : "";
            //saleInvoiceDetailList[_datatablerowindex].Product.Name = $("#ProductID").val() != "" ? $("#ProductID option:selected").text().split("-")[1].trim() : "";
            saleInvoiceDetailList[_datatablerowindex].Product.Code = $('#spanProductName').text() != "" ? $('#spanProductName').text().split("-")[0].trim() : "";
            saleInvoiceDetailList[_datatablerowindex].Product.Name = $('#spanProductName').text() != "" ? $('#spanProductName').text().split("-")[1].trim() : "";

            saleInvoiceDetailList[_datatablerowindex].Product.HSNCode = $("#hdnProductHSNCode").val();
           // saleInvoiceDetailList[_datatablerowindex].ProductID = $("#ProductID").val() != "" ? $("#ProductID").val() : _emptyGuid;
         //   saleInvoiceDetailList[_datatablerowindex].ProductModelID = $("#ProductModelID").val() != "" ? $("#ProductModelID").val() : _emptyGuid;
            ProductModel = new Object;
            Unit = new Object;
            OtherCharge = new Object();
            OtherCharge.SACCode = null;
            OtherCharge.Description = null;
           // ProductModel.Name = $("#ProductModelID").val() != "" ? $("#ProductModelID option:selected").text() : "";
            ProductModel.Name= $('#spanProductModelName').text();
            saleInvoiceDetailList[_datatablerowindex].ProductModel = ProductModel;
            saleInvoiceDetailList[_datatablerowindex].ProductSpec = $('#ProductSpec').val();
            saleInvoiceDetailList[_datatablerowindex].Qty = $('#Qty').val();
            saleInvoiceDetailList[_datatablerowindex].UnitCode = $('#UnitCode').val();
            Unit.Description = $("#UnitCode").val() != "" ? $("#UnitCode option:selected").text().trim() : "";
            saleInvoiceDetailList[_datatablerowindex].Unit = Unit;
            saleInvoiceDetailList[_datatablerowindex].Rate = $('#Rate').val();
            saleInvoiceDetailList[_datatablerowindex].Discount = $('#divModelSaleInvoicePopBody #Discount').val() != "" ? $('#divModelSaleInvoicePopBody #Discount').val() : 0;
            if ($('#divModelSaleInvoicePopBody #TaxTypeCode').val()!=null)
                saleInvoiceDetailList[_datatablerowindex].TaxTypeCode = $('#divModelSaleInvoicePopBody #TaxTypeCode').val().split('|')[0];
            saleInvoiceDetailList[_datatablerowindex].TaxType.ValueText = $('#divModelSaleInvoicePopBody #TaxTypeCode').val();
            saleInvoiceDetailList[_datatablerowindex].CGSTPerc = $('#divModelSaleInvoicePopBody #hdnCGSTPerc').val();
            saleInvoiceDetailList[_datatablerowindex].SGSTPerc = $('#divModelSaleInvoicePopBody #hdnSGSTPerc').val();
            saleInvoiceDetailList[_datatablerowindex].IGSTPerc = $('#divModelSaleInvoicePopBody #hdnIGSTPerc').val();
            saleInvoiceDetailList[_datatablerowindex].CessPerc = $('#divModelSaleInvoicePopBody #CessPerc').val() != "" ? $('#divModelSaleInvoicePopBody #CessPerc').val() : 0;
            saleInvoiceDetailList[_datatablerowindex].CessAmt = $('#divModelSaleInvoicePopBody #CessAmt').val();

            _dataTable.SaleInvoiceDetailList.clear().rows.add(saleInvoiceDetailList).draw(false);
            CalculateTotal();
            $('#divModelPopSaleInvoice').modal('hide');
            _datatablerowindex = -1;
        }
    }
    else {
        if (($('#ProductID').val() != "") && ($('#ProductModelID').val() != "") && ($('#Rate').val() > 0) && ($('#Qty').val() > 0) && ($('#UnitCode').val() != "")) {
            if (_dataTable.SaleInvoiceDetailList.rows().data().length === 0) {
                _dataTable.SaleInvoiceDetailList.clear().rows.add(GetSaleInvoiceDetailListBySaleInvoiceID(_emptyGuid)).draw(false);
                _SlNo = 1;
                var saleInvoiceDetailVM = _dataTable.SaleInvoiceDetailList.rows().data();
                saleInvoiceDetailVM.Product = new Object;
                saleInvoiceDetailVM.ProductModel = new Object;
                saleInvoiceDetailVM.Unit = new Object;
                saleInvoiceDetailVM.TaxType = new Object;
                saleInvoiceDetailVM.OtherCharge = new Object();
                saleInvoiceDetailVM.OtherCharge.SACCode = null;
                saleInvoiceDetailVM.OtherCharge.Description = null;
                saleInvoiceDetailVM[0].Product.Code = $("#divModelSaleInvoicePopBody #ProductID").val() != "" ? $("#divModelSaleInvoicePopBody #ProductID option:selected").text().split("-")[0].trim() : "";
                saleInvoiceDetailVM[0].Product.Name = $("#divModelSaleInvoicePopBody #ProductID").val() != "" ? $("#divModelSaleInvoicePopBody #ProductID option:selected").text().split("-")[1].trim() : "";
                saleInvoiceDetailVM[0].Product.HSNCode = $("#hdnProductHSNCode").val();
                saleInvoiceDetailVM[0].ProductID = $("#divModelSaleInvoicePopBody #ProductID").val() != "" ? $("#divModelSaleInvoicePopBody #ProductID").val() : _emptyGuid;
                saleInvoiceDetailVM[0].ProductModelID = $("#divModelSaleInvoicePopBody #ProductModelID").val() != "" ? $("#divModelSaleInvoicePopBody #ProductModelID").val() : _emptyGuid;
                saleInvoiceDetailVM[0].ProductModel.Name = $("#divModelSaleInvoicePopBody #ProductModelID").val() != "" ? $("#divModelSaleInvoicePopBody #ProductModelID option:selected").text() : "";
                saleInvoiceDetailVM[0].ProductSpec = $('#divModelSaleInvoicePopBody #ProductSpec').val();
                saleInvoiceDetailVM[0].Qty = $('#divModelSaleInvoicePopBody #Qty').val();
                saleInvoiceDetailVM[0].UnitCode = $('#divModelSaleInvoicePopBody #UnitCode').val();
                saleInvoiceDetailVM[0].Unit.Description = $("#divModelSaleInvoicePopBody #UnitCode").val() != "" ? $("#divModelSaleInvoicePopBody #UnitCode option:selected").text().trim() : "";
                saleInvoiceDetailVM[0].Rate = $('#divModelSaleInvoicePopBody #Rate').val()!=""?$('#divModelSaleInvoicePopBody #Rate').val():0;
                saleInvoiceDetailVM[0].Discount = $('#divModelSaleInvoicePopBody #Discount').val() != "" ? $('#divModelSaleInvoicePopBody #Discount').val() : 0;
                saleInvoiceDetailVM[0].TaxTypeCode =$('#divModelSaleInvoicePopBody #TaxTypeCode').val()!=null? $('#divModelSaleInvoicePopBody #TaxTypeCode').val().split('|')[0]:"";
                saleInvoiceDetailVM[0].TaxType.ValueText = $('#divModelSaleInvoicePopBody #TaxTypeCode').val();
                saleInvoiceDetailVM[0].CGSTPerc = $('#divModelSaleInvoicePopBody #hdnCGSTPerc').val();
                saleInvoiceDetailVM[0].SGSTPerc = $('#divModelSaleInvoicePopBody #hdnSGSTPerc').val();
                saleInvoiceDetailVM[0].IGSTPerc = $('#divModelSaleInvoicePopBody #hdnIGSTPerc').val();
                saleInvoiceDetailVM[0].CessPerc = $('#divModelSaleInvoicePopBody #CessPerc').val() != "" ? $('#divModelSaleInvoicePopBody #CessPerc').val() : 0;
                saleInvoiceDetailVM[0].CessAmt = $('#divModelSaleInvoicePopBody #CessAmt').val();
                ClearCalculatedFields();
                _dataTable.SaleInvoiceDetailList.clear().rows.add(saleInvoiceDetailVM).draw(false);
                CalculateTotal();
                $('#divModelPopSaleInvoice').modal('hide');
            }
            else {
                var saleInvoiceDetailVM = _dataTable.SaleInvoiceDetailList.rows().data();
                if (saleInvoiceDetailVM.length > 0) {
                    var checkpoint = 0;
                    var productSpec = $('#ProductSpec').val();
                    productSpec = productSpec.replace(/\n/g, ' ');
                    for (var i = 0; i < saleInvoiceDetailVM.length; i++) {
                        if ((saleInvoiceDetailVM[i].ProductID == $('#ProductID').val()) && (saleInvoiceDetailVM[i].ProductModelID == $('#ProductModelID').val()
                            && (saleInvoiceDetailVM[i].ProductSpec == null ? "" : saleInvoiceDetailVM[i].ProductSpec.replace(/\n/g, ' ') == productSpec && (saleInvoiceDetailVM[i].UnitCode == $('#UnitCode').val())
                            && (saleInvoiceDetailVM[i].Rate == $('#divModelSaleInvoicePopBody #Rate').val())
                            )))
                        {
                            saleInvoiceDetailVM[i].Qty = parseFloat(saleInvoiceDetailVM[i].Qty) + parseFloat($('#Qty').val());
                            checkpoint = 1;
                            break;
                        }
                    }
                    if (checkpoint == 1) {
                        _SlNo = 1;
                        _dataTable.SaleInvoiceDetailList.clear().rows.add(saleInvoiceDetailVM).draw(false);
                    }
                    else if (checkpoint == 0) {
                        _SlNo = _dataTable.SaleInvoiceDetailList.rows().data().length + 1;
                        var SaleInvoiceDetailVM = new Object();
                        SaleInvoiceDetailVM.Product = new Object;
                        SaleInvoiceDetailVM.ProductModel = new Object()
                        SaleInvoiceDetailVM.Unit = new Object();
                        SaleInvoiceDetailVM.TaxType = new Object();
                        SaleInvoiceDetailVM.OtherCharge = new Object();
                        SaleInvoiceDetailVM.OtherCharge.SACCode = null;
                        SaleInvoiceDetailVM.OtherCharge.Description = null;

                        SaleInvoiceDetailVM.ID = _emptyGuid;
                        SaleInvoiceDetailVM.ProductID = $("#ProductID").val() != "" ? $("#ProductID").val() : _emptyGuid;
                        SaleInvoiceDetailVM.Product.Code = $("#divModelSaleInvoicePopBody #ProductID").val() != "" ? $("#divModelSaleInvoicePopBody #ProductID option:selected").text().split("-")[0].trim() : "";
                        SaleInvoiceDetailVM.Product.Name = $("#divModelSaleInvoicePopBody #ProductID").val() != "" ? $("#divModelSaleInvoicePopBody #ProductID option:selected").text().split("-")[1].trim() : "";
                        SaleInvoiceDetailVM.Product.HSNCode = $("#hdnProductHSNCode").val();
                        SaleInvoiceDetailVM.ProductID = $("#divModelSaleInvoicePopBody #ProductID").val() != "" ? $("#divModelSaleInvoicePopBody #ProductID").val() : _emptyGuid;
                        SaleInvoiceDetailVM.ProductModelID = $("#divModelSaleInvoicePopBody #ProductModelID").val() != "" ? $("#divModelSaleInvoicePopBody #ProductModelID").val() : _emptyGuid;
                        SaleInvoiceDetailVM.ProductModel.Name = $("#divModelSaleInvoicePopBody #ProductModelID").val() != "" ? $("#divModelSaleInvoicePopBody #ProductModelID option:selected").text() : "";
                        SaleInvoiceDetailVM.ProductSpec = $('#divModelSaleInvoicePopBody #ProductSpec').val();
                        SaleInvoiceDetailVM.Qty = $('#divModelSaleInvoicePopBody #Qty').val();
                        SaleInvoiceDetailVM.UnitCode = $('#divModelSaleInvoicePopBody #UnitCode').val();
                        SaleInvoiceDetailVM.Unit.Description = $("#divModelSaleInvoicePopBody #UnitCode").val() != "" ? $("#divModelSaleInvoicePopBody #UnitCode option:selected").text().trim() : "";
                        SaleInvoiceDetailVM.Rate = $('#divModelSaleInvoicePopBody #Rate').val();
                        SaleInvoiceDetailVM.Discount = $('#divModelSaleInvoicePopBody #Discount').val() != "" ? $('#divModelSaleInvoicePopBody #Discount').val() : 0;
                        SaleInvoiceDetailVM.TaxTypeCode =$('#divModelSaleInvoicePopBody #TaxTypeCode').val()!=null? $('#divModelSaleInvoicePopBody #TaxTypeCode').val().split('|')[0]:"";
                        SaleInvoiceDetailVM.TaxType.ValueText = $('#divModelSaleInvoicePopBody #TaxTypeCode').val();
                        SaleInvoiceDetailVM.CGSTPerc = $('#divModelSaleInvoicePopBody #hdnCGSTPerc').val();
                        SaleInvoiceDetailVM.SGSTPerc = $('#divModelSaleInvoicePopBody #hdnSGSTPerc').val();
                        SaleInvoiceDetailVM.IGSTPerc = $('#divModelSaleInvoicePopBody #hdnIGSTPerc').val();
                        SaleInvoiceDetailVM.CessPerc = $('#divModelSaleInvoicePopBody #CessPerc').val() != "" ? $('#divModelSaleInvoicePopBody #CessPerc').val() : 0;
                        SaleInvoiceDetailVM.CessAmt = $('#divModelSaleInvoicePopBody #CessAmt').val();
                        _dataTable.SaleInvoiceDetailList.row.add(SaleInvoiceDetailVM).draw(true);
                    }
                    CalculateTotal();
                    $('#divModelPopSaleInvoice').modal('hide');
                }
            }
        }
    }
    $('[data-toggle="popover"]').popover({
        html: true,
        'trigger': 'hover'
        
    });
}
//Edit SaleInvoice Detail
function EditSaleInvoiceDetail(this_Obj) {
    debugger;
    _datatablerowindex = _dataTable.SaleInvoiceDetailList.row($(this_Obj).parents('tr')).index();
    var saleInvoiceDetail = _dataTable.SaleInvoiceDetailList.row($(this_Obj).parents('tr')).data();
    $("#divModelSaleInvoicePopBody").load("SaleInvoice/AddSaleInvoiceDetail?update=true", function (responseTxt, statusTxt, xhr) {
        if (statusTxt == 'success') {
            $('#lblModelPopSaleInvoice').text('SaleInvoice Detail')
            $('#FormSaleInvoiceDetail #IsUpdate').val('True');
            $('#FormSaleInvoiceDetail #ID').val(saleInvoiceDetail.ID);
            // $("#FormSaleInvoiceDetail #ProductID").val(saleInvoiceDetail.ProductID)
            $("#FormSaleInvoiceDetail #hdnProductID").val(saleInvoiceDetail.ProductID)
            $('#spanProductName').text(saleInvoiceDetail.Product.Code + "-" + saleInvoiceDetail.Product.Name)
            $('#spanProductModelName').text(saleInvoiceDetail.ProductModel.Name)
            $('#divProductBasicInfo').load("Product/ProductBasicInfo?ID=" + $('#hdnProductID').val(), function (responseTxt, statusTxt, xhr) {
                if (statusTxt == 'success') {
                    debugger;
                    $("#FormSaleInvoiceDetail #hdnProductModelID").val(saleInvoiceDetail.ProductModelID);
                    if ($('#hdnProductModelID').val() != _emptyGuid) {
                        var curRate = $('#hdnCurrencyRate').val() == undefined ? 0 : $('#hdnCurrencyRate').val();
                        $('#divProductBasicInfo').load("ProductModel/ProductModelBasicInfo?ID=" + $('#hdnProductModelID').val() + "&rate=" + curRate, function () {
                        });
                    }
                }
                else {
                    console.log("Error: " + xhr.status + ": " + xhr.statusText);
                }
            });


            //if ($('#hdnProductID').val() != _emptyGuid) {
            //    $('.divProductModelSelectList').load("ProductModel/ProductModelSelectList?required=required&productID=" + $('#hdnProductID').val())
            //}
            //else {
            //    $('.divProductModelSelectList').empty();
            //    $('.divProductModelSelectList').append('<span class="form-control newinput"><i id="dropLoad" class="fa fa-spinner"></i></span>');
            //}
            //$("#FormSaleInvoiceDetail #ProductModelID").val(saleInvoiceDetail.ProductModelID);


            $('#FormSaleInvoiceDetail #ProductSpec').val(saleInvoiceDetail.ProductSpec);
            $('#FormSaleInvoiceDetail #Qty').val(saleInvoiceDetail.Qty);
            $('#FormSaleInvoiceDetail #UnitCode').val(saleInvoiceDetail.UnitCode);
            $('#FormSaleInvoiceDetail #hdnUnitCode').val(saleInvoiceDetail.UnitCode);
            $('#FormSaleInvoiceDetail #Rate').val(saleInvoiceDetail.Rate);
            $('#FormSaleInvoiceDetail #Discount').val(saleInvoiceDetail.Discount);
            if (saleInvoiceDetail.TaxTypeCode != 0) {
                $('#FormSaleInvoiceDetail #TaxTypeCode').val(saleInvoiceDetail.TaxType.ValueText);
                $('#FormSaleInvoiceDetail #hdnTaxTypeCode').val(saleInvoiceDetail.TaxType.ValueText);
            }
            $('#FormSaleInvoiceDetail #hdnCGSTPerc').val(saleInvoiceDetail.CGSTPerc);
            $('#FormSaleInvoiceDetail #hdnSGSTPerc').val(saleInvoiceDetail.SGSTPerc);
            $('#FormSaleInvoiceDetail #hdnIGSTPerc').val(saleInvoiceDetail.IGSTPerc);
            var TaxableAmt = ((parseFloat(saleInvoiceDetail.Rate) * parseInt(saleInvoiceDetail.Qty)) - parseFloat(saleInvoiceDetail.Discount))
            var CGSTAmt = (TaxableAmt * parseFloat(saleInvoiceDetail.CGSTPerc)) / 100;
            var SGSTAmt = (TaxableAmt * parseFloat(saleInvoiceDetail.SGSTPerc)) / 100;
            var IGSTAmt = (TaxableAmt * parseFloat(saleInvoiceDetail.IGSTPerc)) / 100;
            $('#FormSaleInvoiceDetail #CGSTPerc').val(CGSTAmt);
            $('#FormSaleInvoiceDetail #SGSTPerc').val(SGSTAmt);
            $('#FormSaleInvoiceDetail #IGSTPerc').val(IGSTAmt);
            $('#FormSaleInvoiceDetail #CessPerc').val(saleInvoiceDetail.CessPerc);
            $('#FormSaleInvoiceDetail #CessAmt').val(saleInvoiceDetail.CessAmt);

            $('#divModelPopSaleInvoice').modal('show');
        }
        else {
            console.log("Error: " + xhr.status + ": " + xhr.statusText);
        }
    });
}

//Delete SaleInvoice Detail
function ConfirmDeleteSaleInvoiceDetail(this_Obj) {
    _SlNo = 1;
    _datatablerowindex = _dataTable.SaleInvoiceDetailList.row($(this_Obj).parents('tr')).index();
    var saleInvoiceDetail = _dataTable.SaleInvoiceDetailList.row($(this_Obj).parents('tr')).data();
    if (saleInvoiceDetail.ID === _emptyGuid) {
        notyConfirm('Are you sure to delete?', 'DeleteCurrentSaleInvoiceDetail("' + _datatablerowindex + '")');
    }
    else {
        notyConfirm('Are you sure to delete?', 'DeleteSaleInvoiceDetail("' + saleInvoiceDetail.ID + '")');

    }
}
function DeleteCurrentSaleInvoiceDetail(_datatablerowindex) {
    var saleInvoiceDetailList = _dataTable.SaleInvoiceDetailList.rows().data();
    saleInvoiceDetailList.splice(_datatablerowindex, 1);
    _dataTable.SaleInvoiceDetailList.clear().rows.add(saleInvoiceDetailList).draw(false);
    CalculateTotal();
    notyAlert('success', 'Detail Row deleted successfully');
}
function DeleteSaleInvoiceDetail(ID) {
    if (ID != _emptyGuid && ID != null && ID != '') {
        var data = { "id": ID };
        var ds = {};
        _jsonData = GetDataFromServer("SaleInvoice/DeleteSaleInvoiceDetail/", data);
        if (_jsonData != '') {
            _jsonData = JSON.parse(_jsonData);
            _message = _jsonData.Message;
            _status = _jsonData.Status;
            _result = _jsonData.Record;
        }
        if (_status == "OK") {
            notyAlert('success', _result.Message);
            var saleInvoiceDetailList = _dataTable.SaleInvoiceDetailList.rows().data();
            saleInvoiceDetailList.splice(_datatablerowindex, 1);
            _dataTable.SaleInvoiceDetailList.clear().rows.add(saleInvoiceDetailList).draw(false);
            CalculateTotal();
        }
        if (_status == "ERROR") {
            notyAlert('error', _message);
        }
    }
}
//OtherExpense------------------
function AddOtherExpenseDetailList() {
    $("#divModelSaleInvoicePopBody").load("SaleInvoice/SaleInvoiceOtherChargeDetail?update=false", function () {
        $('#lblModelPopSaleInvoice').text('OtherExpense Detail')
        $('#divModelPopSaleInvoice').modal('show');
    });
}
function AddOtherExpenseDetailToList() {
    debugger;
    $("#FormOtherExpenseDetail").submit(function () { });
    if ($('#FormOtherExpenseDetail #IsUpdate').val() == 'True') {
        _SlNoOtherCharge = 1;
        if (($('#divModelSaleInvoicePopBody #OtherChargeCode').val() != "") && ($('#divModelSaleInvoicePopBody #ChargeAmount').val() != "")) {
            var saleInvoiceOtherExpenseDetailList = _dataTable.SaleInvoiceOtherChargesDetailList.rows().data();
          //  saleInvoiceOtherExpenseDetailList[_datatablerowindex].OtherCharge.Description = $("#divModelSaleInvoicePopBody #OtherChargeCode").val() != "" ? $("#divModelSaleInvoicePopBody #OtherChargeCode option:selected").text().split("-")[0].trim() : "";
            saleInvoiceOtherExpenseDetailList[_datatablerowindex].OtherCharge.Description = $('#spanOtherCharge').text() != "" ? $('#spanOtherCharge').text().split("-")[0].trim() : "";
            saleInvoiceOtherExpenseDetailList[_datatablerowindex].OtherCharge.SACCode = $("#hdnOtherChargeSACCode").val();
            saleInvoiceOtherExpenseDetailList[_datatablerowindex].ChargeAmount = $("#divModelSaleInvoicePopBody #ChargeAmount").val();
            saleInvoiceOtherExpenseDetailList[_datatablerowindex].OtherChargeCode = $("#divModelSaleInvoicePopBody #hdnOtherChargeCode").val() != "" ? $("#divModelSaleInvoicePopBody #hdnOtherChargeCode").val() : _emptyGuid;
            TaxType = new Object;
            if ($('#divModelSaleInvoicePopBody #TaxTypeCode').val() != null) {
                saleInvoiceOtherExpenseDetailList[_datatablerowindex].TaxTypeCode = $('#divModelSaleInvoicePopBody #TaxTypeCode').val().split('|')[0];
                TaxType.ValueText = $('#divModelSaleInvoicePopBody #TaxTypeCode').val();
            }
            saleInvoiceOtherExpenseDetailList[_datatablerowindex].TaxType = TaxType;
            saleInvoiceOtherExpenseDetailList[_datatablerowindex].CGSTPerc = $('#divModelSaleInvoicePopBody #hdnCGSTPerc').val();
            saleInvoiceOtherExpenseDetailList[_datatablerowindex].SGSTPerc = $('#divModelSaleInvoicePopBody #hdnSGSTPerc').val();
            saleInvoiceOtherExpenseDetailList[_datatablerowindex].IGSTPerc = $('#divModelSaleInvoicePopBody #hdnIGSTPerc').val();
            saleInvoiceOtherExpenseDetailList[_datatablerowindex].AddlTaxPerc = $('#divModelSaleInvoicePopBody #AddlTaxPerc').val() != "" ? $('#divModelSaleInvoicePopBody #AddlTaxPerc').val() : 0;
            saleInvoiceOtherExpenseDetailList[_datatablerowindex].AddlTaxAmt = $('#divModelSaleInvoicePopBody #AddlTaxAmt').val() != "" ? $('#divModelSaleInvoicePopBody #AddlTaxAmt').val() : 0.00;
            ClearCalculatedFields();
            _dataTable.SaleInvoiceOtherChargesDetailList.clear().rows.add(saleInvoiceOtherExpenseDetailList).draw(false);
            CalculateTotal();
            $('#divModelPopSaleInvoice').modal('hide');
            _datatablerowindex = -1;
        }
    }
    else {
        if (($('#divModelSaleInvoicePopBody #OtherChargeCode').val() != "") && ($('#divModelSaleInvoicePopBody #ChargeAmount').val() != "")) {
            if (_dataTable.SaleInvoiceOtherChargesDetailList.rows().data().length === 0) {
                _dataTable.SaleInvoiceOtherChargesDetailList.clear().rows.add(GetSaleInvoiceOtherChargesDetailListBySaleOrderID(_emptyGuid, false)).draw(false);
                _SlNoOtherCharge = 1;
                var saleInvoiceOtherExpenseDetailList = _dataTable.SaleInvoiceOtherChargesDetailList.rows().data();
                //saleInvoiceOtherExpenseDetailList.OtherCharge = new Object;
                //saleInvoiceOtherExpenseDetailList.TaxType = new Object;
                saleInvoiceOtherExpenseDetailList[0].OtherCharge.Description = $("#divModelSaleInvoicePopBody #OtherChargeCode").val() != "" ? $("#divModelSaleInvoicePopBody #OtherChargeCode option:selected").text().split("-")[0].trim() : "";
                saleInvoiceOtherExpenseDetailList[0].OtherChargeCode = $("#divModelSaleInvoicePopBody #OtherChargeCode").val() != "" ? $("#divModelSaleInvoicePopBody #OtherChargeCode").val() : _emptyGuid;
                saleInvoiceOtherExpenseDetailList[0].OtherCharge.SACCode = $("#hdnOtherChargeSACCode").val();
                saleInvoiceOtherExpenseDetailList[0].ChargeAmount = $("#divModelSaleInvoicePopBody #ChargeAmount").val();
                if ($('#divModelSaleInvoicePopBody #TaxTypeCode').val() != null) {
                    saleInvoiceOtherExpenseDetailList[0].TaxTypeCode = $('#divModelSaleInvoicePopBody #TaxTypeCode').val().split('|')[0];
                }
                saleInvoiceOtherExpenseDetailList[0].TaxType.ValueText = $('#divModelSaleInvoicePopBody #TaxTypeCode').val();
                saleInvoiceOtherExpenseDetailList[0].CGSTPerc = $('#divModelSaleInvoicePopBody #hdnCGSTPerc').val();
                saleInvoiceOtherExpenseDetailList[0].SGSTPerc = $('#divModelSaleInvoicePopBody #hdnSGSTPerc').val();
                saleInvoiceOtherExpenseDetailList[0].IGSTPerc = $('#divModelSaleInvoicePopBody #hdnIGSTPerc').val();
                saleInvoiceOtherExpenseDetailList[0].AddlTaxPerc = $('#divModelSaleInvoicePopBody #AddlTaxPerc').val() != "" ? $('#divModelSaleInvoicePopBody #AddlTaxPerc').val() : 0;
                saleInvoiceOtherExpenseDetailList[0].AddlTaxAmt = $('#divModelSaleInvoicePopBody #AddlTaxAmt').val() != "" ? $('#divModelSaleInvoicePopBody #AddlTaxAmt').val() : 0.00;
                ClearCalculatedFields();
                _dataTable.SaleInvoiceOtherChargesDetailList.clear().rows.add(saleInvoiceOtherExpenseDetailList).draw(false);
                CalculateTotal();
                $('#divModelPopSaleInvoice').modal('hide');
            }
            else {
                var saleInvoiceOtherExpenseDetailList = _dataTable.SaleInvoiceOtherChargesDetailList.rows().data();
                if (saleInvoiceOtherExpenseDetailList.length > 0) {
                    var checkpoint = 0;
                    var otherCharge = $('#OtherChargeCode').val();
                    for (var i = 0; i < saleInvoiceOtherExpenseDetailList.length; i++) {
                        if ((saleInvoiceOtherExpenseDetailList[i].OtherChargeCode == otherCharge)) {
                            saleInvoiceOtherExpenseDetailList[i].ChargeAmount = parseFloat(saleInvoiceOtherExpenseDetailList[i].ChargeAmount) + parseFloat($('#ChargeAmount').val());
                            checkpoint = 1;
                            break;
                        }
                    }
                    if (checkpoint == 1) {
                        _SlNoOtherCharge = 1;
                        ClearCalculatedFields();
                        _dataTable.SaleInvoiceOtherChargesDetailList.clear().rows.add(saleInvoiceOtherExpenseDetailList).draw(false);
                        CalculateTotal();
                        $('#divModelPopSaleInvoice').modal('hide');
                    }
                    else if (checkpoint == 0) {
                        _SlNoOtherCharge = _dataTable.SaleInvoiceOtherChargesDetailList.rows().data().length + 1;
                        ClearCalculatedFields();
                        var SaleInvoiceOtherChargesDetailVM = new Object();
                        SaleInvoiceOtherChargesDetailVM.ID = _emptyGuid;
                        var OtherCharge = new Object;
                        OtherCharge.Description = $("#divModelSaleInvoicePopBody #OtherChargeCode").val() != "" ? $("#divModelSaleInvoicePopBody #OtherChargeCode option:selected").text().split("-")[0].trim() : "";
                        SaleInvoiceOtherChargesDetailVM.OtherCharge = OtherCharge;
                        SaleInvoiceOtherChargesDetailVM.OtherChargeCode = $("#divModelSaleInvoicePopBody #OtherChargeCode").val() != "" ? $("#divModelSaleInvoicePopBody #OtherChargeCode").val() : _emptyGuid;
                        SaleInvoiceOtherChargesDetailVM.OtherCharge.SACCode = $("#hdnOtherChargeSACCode").val();
                        SaleInvoiceOtherChargesDetailVM.ChargeAmount = $("#divModelSaleInvoicePopBody #ChargeAmount").val();
                        var TaxType = new Object();
                        if ($('#divModelSaleInvoicePopBody #TaxTypeCode').val() != null) {
                            SaleInvoiceOtherChargesDetailVM.TaxTypeCode = $('#divModelSaleInvoicePopBody #TaxTypeCode').val().split('|')[0];
                            TaxType.ValueText = $('#divModelSaleInvoicePopBody #TaxTypeCode').val();
                        }
                        SaleInvoiceOtherChargesDetailVM.TaxType = TaxType;
                        SaleInvoiceOtherChargesDetailVM.CGSTPerc = $('#divModelSaleInvoicePopBody #hdnCGSTPerc').val();
                        SaleInvoiceOtherChargesDetailVM.SGSTPerc = $('#divModelSaleInvoicePopBody #hdnSGSTPerc').val();
                        SaleInvoiceOtherChargesDetailVM.IGSTPerc = $('#divModelSaleInvoicePopBody #hdnIGSTPerc').val();
                        SaleInvoiceOtherChargesDetailVM.AddlTaxPerc = $('#divModelSaleInvoicePopBody #AddlTaxPerc').val() != "" ? $('#divModelSaleInvoicePopBody #AddlTaxPerc').val() : 0.00;
                        SaleInvoiceOtherChargesDetailVM.AddlTaxAmt = $('#divModelSaleInvoicePopBody #AddlTaxAmt').val() != "" ? $('#divModelSaleInvoicePopBody #AddlTaxAmt').val() : 0.00;
                        _dataTable.SaleInvoiceOtherChargesDetailList.row.add(SaleInvoiceOtherChargesDetailVM).draw(true);
                        CalculateTotal();
                        $('#divModelPopSaleInvoice').modal('hide');
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
function BindSaleInvoiceOtherChargesDetailList(id, IsQuotation, IsSaleOrder, IsProformaInvoice) {
    var data;
    if (id == _emptyGuid && !(IsQuotation) && !(IsSaleOrder) && !(IsProformaInvoice)) {
        data = null;
    }
    else if (id == _emptyGuid && (IsQuotation)) {
        data = GetSaleInvoiceOtherChargesDetailListBySaleOrderID(id, IsQuotation, IsSaleOrder, IsProformaInvoice)
    }
    else if (id == _emptyGuid && (IsSaleOrder)) {
        data = GetSaleInvoiceOtherChargesDetailListBySaleOrderID(id, IsQuotation, IsSaleOrder, IsProformaInvoice)
    }
    else if (id == _emptyGuid && (IsProformaInvoice)) {
        data = GetSaleInvoiceOtherChargesDetailListBySaleOrderID(id, IsQuotation, IsSaleOrder, IsProformaInvoice)
    }
    else {
        data = GetSaleInvoiceOtherChargesDetailListBySaleOrderID(id, IsQuotation, IsSaleOrder, IsProformaInvoice)
    }

    _dataTable.SaleInvoiceOtherChargesDetailList = $('#tblSaleInvoiceOtherChargesDetailList').DataTable(
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
                      "data": "", render: function (data, type, row) {
                          return _SlNoOtherCharge++
                      }, "defaultContent": "<i></i>", "width": "2%"
                  },
             { "data": "OtherCharge.Description", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
             {
                 "data": "OtherCharge.SACCode", "defaultContent": "<i></i>"
             },
             { "data": "ChargeAmount", render: function (data, type, row) { return formatCurrency(data) }, "defaultContent": "<i></i>" },
             {
                 "data": "ChargeAmount", render: function (data, type, row) {
                     var CGST = parseFloat(row.CGSTPerc != "" ? row.CGSTPerc : 0);
                     var SGST = parseFloat(row.SGSTPerc != "" ? row.SGSTPerc : 0);
                     var IGST = parseFloat(row.IGSTPerc != "" ? row.IGSTPerc : 0);
                     var CGSTAmt = parseFloat(data * CGST / 100);
                     var SGSTAmt = parseFloat(data * SGST / 100)
                     var IGSTAmt = parseFloat(data * IGST / 100)
                     var GSTAmt = roundoff(parseFloat(CGSTAmt) + parseFloat(SGSTAmt) + parseFloat(IGSTAmt))
                     var GSTPerc = CGST + SGST + IGST
                     return '<div class="show-popover text-right" data-html="true" data-placement="left" data-toggle="popover" data-title="<p align=left>Total GST : ₹ ' + formatCurrency(GSTAmt) + '" data-content=" SGST ' + SGST + '% : ₹ ' + formatCurrency(roundoff(parseFloat(SGSTAmt))) + '<br/>CGST ' + CGST + '% : ₹ ' + formatCurrency(roundoff(parseFloat(CGSTAmt))) + '<br/> IGST ' + IGST + '% : ₹ ' + formatCurrency(roundoff(parseFloat(IGSTAmt))) + '</p>"/>' + formatCurrency(GSTAmt) + '<br/><i style="font-size:10px;color:brown">GST(%) -</i>' + GSTPerc
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
                     return '<div class="show-popover text-right" data-html="true" data-placement="left" data-toggle="popover" data-title="<p align=left>Additional Tax :' + row.AddlTaxPerc + '%</p>"/>' + formatCurrency(roundoff(AddlTax))
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

                     return '<div class="show-popover text-right" data-html="true" data-placement="left" data-toggle="popover" data-title="<p align=left>Total :  ' + formatCurrency(Total) + '" data-content="Charge Amount :  ' + formatCurrency(data) + '<br/>GST :  ' + formatCurrency(GSTAmt) + '<br/>Additional Tax :  ' + formatCurrency(row.AddlTaxAmt) + '</p>"/>' + formatCurrency(Total)
                 }, "defaultContent": "<i></i>"
             },
             { "data": null, "orderable": false, "defaultContent": ($('#IsDocLocked').val() == "True" || $('#IsUpdate').val() == "False") ? '<a href="#" class="actionLink"  onclick="EditSaleInvoiceOtherChargesDetail(this)" ><i class="fa fa-pencil-square-o" aria-hidden="true"></i></a> <a href="#" class="DeleteLink"  onclick="ConfirmDeleteSaleInvoiceOtherChargeDetail(this)" ><i class="fa fa-trash-o" aria-hidden="true"></i></a>':"-" },
             ],
             columnDefs: [
                 //{ "targets": [0], "width": "30%" },
                 //{ "targets": [1, 2, 3, 4], "width": "15%" },
                 //{ "targets": [5], "width": "10%" },
                 { className: "text-left", "targets": [1,2] },
                 { className: "text-right", "targets": [ 3, 4, 5, 6] },
                 { className: "text-center", "targets": [0,7] }
             ],
             destroy: true,
         });
    $('[data-toggle="popover"]').popover({
        html: true,
        'trigger': 'hover',
        'placement': 'top'
    });
}

function GetSaleInvoiceOtherChargesDetailListBySaleOrderID(id, IsQuotation, IsSaleOrder, IsProformaInvoice) {
    debugger;
    try {
        if ((IsSaleOrder == undefined || IsQuotation == undefined || IsProformaInvoice==undefined) && id != _emptyGuid)
            _SlNoOtherCharge = 1;
        else if ((IsSaleOrder == undefined || IsQuotation == undefined || IsProformaInvoice == undefined) && id == _emptyGuid)
            _SlNoOtherCharge = 0;
        else
            _SlNoOtherCharge = 1;
        var SaleInvoiceOtherChargesDetailList = [];
        debugger;
        if (IsQuotation) {
            var data = { "quoteID": $('#SaleInvoiceForm #hdnQuoteID').val() };
            _jsonData = GetDataFromServer("SaleInvoice/GetSaleInvoiceOtherChargesDetailListByQuotationIDFromQuotation/", data);
        }
        else if (IsSaleOrder) {
            var data = { "saleOrderID": $('#SaleInvoiceForm #hdnSaleOrderID').val() };
            _jsonData = GetDataFromServer("SaleInvoice/GetSaleInvoiceOtherChargesDetailListBySaleOrderIDFromSaleOrder/", data);
        }
        else if (IsProformaInvoice) {
            var data = { "proformaInvoiceID": $('#SaleInvoiceForm #hdnProfInvID').val() };
            _jsonData = GetDataFromServer("SaleInvoice/GetProformaInvoiceOtherChargesDetailListByProfInvIDFromProformaInvoice/", data);
        }
        else {
            var data = { "saleInvoiceID": id };
            _jsonData = GetDataFromServer("SaleInvoice/GetSaleInvoiceOtherChargesDetailListBySaleInvoiceID/", data);
        }

        if (_jsonData != '') {
            _jsonData = JSON.parse(_jsonData);
            _message = _jsonData.Message;
            _status = _jsonData.Status;
            SaleInvoiceOtherChargesDetailList = _jsonData.Records;
        }
        if (_status == "OK") {
            return SaleInvoiceOtherChargesDetailList;
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

function EditSaleInvoiceOtherChargesDetail(this_Obj) {
    debugger;
    _datatablerowindex = _dataTable.SaleInvoiceOtherChargesDetailList.row($(this_Obj).parents('tr')).index();
    var saleInvoiceOtherChargesDetail = _dataTable.SaleInvoiceOtherChargesDetailList.row($(this_Obj).parents('tr')).data();
    $("#divModelSaleInvoicePopBody").load("SaleInvoice/SaleInvoiceOtherChargeDetail?update=true", function () {
        $('#lblModelPopQuotation').text('OtherCharges Detail')
        $('#FormOtherExpenseDetail #IsUpdate').val('True');
        $('#FormOtherExpenseDetail #ID').val(saleInvoiceOtherChargesDetail.ID);
        $('#spanOtherCharge').text(saleInvoiceOtherChargesDetail.OtherCharge.Description)
        $("#FormOtherExpenseDetail #OtherChargeCode").val(saleInvoiceOtherChargesDetail.OtherChargeCode);
        $("#FormOtherExpenseDetail #hdnOtherChargeCode").val(saleInvoiceOtherChargesDetail.OtherChargeCode);
        $("#FormOtherExpenseDetail #ChargeAmount").val(saleInvoiceOtherChargesDetail.ChargeAmount);
       // if (saleInvoiceOtherChargesDetail.TaxType.Code != 0) {
            $('#FormOtherExpenseDetail #TaxTypeCode').val(saleInvoiceOtherChargesDetail.TaxType.ValueText);
            $('#FormOtherExpenseDetail #hdnTaxTypeCode').val(saleInvoiceOtherChargesDetail.TaxType.ValueText);
       // }
        $('#FormOtherExpenseDetail #hdnCGSTPerc').val(saleInvoiceOtherChargesDetail.CGSTPerc);
        $('#FormOtherExpenseDetail #hdnSGSTPerc').val(saleInvoiceOtherChargesDetail.SGSTPerc);
        $('#FormOtherExpenseDetail #hdnIGSTPerc').val(saleInvoiceOtherChargesDetail.IGSTPerc);
        $('#FormOtherExpenseDetail #hdnAddlTaxPerc').val(saleInvoiceOtherChargesDetail.AddlTaxPerc);
        $('#FormOtherExpenseDetail #hdnAddlTaxAmt').val(saleInvoiceOtherChargesDetail.AddlTaxAmt);
        $('#FormOtherExpenseDetail #AddlTaxPerc').val(saleInvoiceOtherChargesDetail.AddlTaxPerc);
        $('#FormOtherExpenseDetail #AddlTaxAmt').val(saleInvoiceOtherChargesDetail.AddlTaxAmt);

        var CGSTAmt = (saleInvoiceOtherChargesDetail.ChargeAmount * parseFloat(saleInvoiceOtherChargesDetail.CGSTPerc)) / 100;
        var SGSTAmt = (saleInvoiceOtherChargesDetail.ChargeAmount * parseFloat(saleInvoiceOtherChargesDetail.SGSTPerc)) / 100;
        var IGSTAmt = (saleInvoiceOtherChargesDetail.ChargeAmount * parseFloat(saleInvoiceOtherChargesDetail.IGSTPerc)) / 100;
        $('#FormOtherExpenseDetail #CGSTPerc').val(CGSTAmt);
        $('#FormOtherExpenseDetail #SGSTPerc').val(SGSTAmt);
        $('#FormOtherExpenseDetail #IGSTPerc').val(IGSTAmt);
        $('#divModelPopSaleInvoice').modal('show');
    });
}

function ConfirmDeleteSaleInvoiceOtherChargeDetail(this_Obj) {
    _SlNoOtherCharge = 1;
    _datatablerowindex = _dataTable.SaleInvoiceOtherChargesDetailList.row($(this_Obj).parents('tr')).index();
    var saleInvoiceOtherChargeDetail = _dataTable.SaleInvoiceOtherChargesDetailList.row($(this_Obj).parents('tr')).data();
    if (saleInvoiceOtherChargeDetail.ID === _emptyGuid) {
        notyConfirm('Are you sure to delete?', 'DeleteCurrentSaleInvoiceOtherChargeDetail("' + _datatablerowindex + '")');
    }
    else {
        notyConfirm('Are you sure to delete?', 'DeleteSaleInvoiceOtherChargeDetail("' + saleInvoiceOtherChargeDetail.ID + '")');

    }
}
function DeleteCurrentSaleInvoiceOtherChargeDetail(_datatablerowindex) {
    var quotationOtherChargeDetailList = _dataTable.SaleInvoiceOtherChargesDetailList.rows().data();
    quotationOtherChargeDetailList.splice(_datatablerowindex, 1);
    ClearCalculatedFields();
    _dataTable.SaleInvoiceOtherChargesDetailList.clear().rows.add(quotationOtherChargeDetailList).draw(false);
    CalculateTotal();
    notyAlert('success', 'Detail Row deleted successfully');
}
function DeleteSaleInvoiceOtherChargeDetail(ID) {
    if (ID != _emptyGuid && ID != null && ID != '') {
        var data = { "id": ID };
        var ds = {};
        _jsonData = GetDataFromServer("SaleInvoice/DeleteSaleInvoiceOtherChargeDetail/", data);
        if (_jsonData != '') {
            _jsonData = JSON.parse(_jsonData);
            _message = _jsonData.Message;
            _status = _jsonData.Status;
            _result = _jsonData.Record;
        }
        if (_status == "OK") {
            notyAlert('success', _result.Message);
            var saleInvoiceOtherChargeDetailList = _dataTable.SaleInvoiceOtherChargesDetailList.rows().data();
            saleInvoiceOtherChargeDetailList.splice(_datatablerowindex, 1);
            ClearCalculatedFields();
            _dataTable.SaleInvoiceOtherChargesDetailList.clear().rows.add(saleInvoiceOtherChargeDetailList).draw(false);
            CalculateTotal();
        }
        if (_status == "ERROR") {
            notyAlert('error', _message);
        }
    }
}

////================================================================================================
////SaleInvoiceFollowup Section
//function AddSaleInvoiceFollowUp() {
//    $("#divModelSaleInvoicePopBody").load("SaleInvoiceFollowup/AddSaleInvoiceFollowup?id=" + _emptyGuid + "&saleInvoiceID=" + $('#SaleInvoiceForm input[type="hidden"]#ID').val() + "&customerID=" + ($('#SaleInvoiceForm #hdnCustomerID').val() != "" ? $('#SaleInvoiceForm #hdnCustomerID').val() : _emptyGuid), function () {
//        $('#lblModelPopSaleInvoice').text('Add SaleInvoice Followup')
//        $('#btnresetSaleInvoiceFollowup').trigger('click');
//        $('#divModelPopSaleInvoice').modal('show');

//    });
//}
//function SaleInvoiceFollowUpPaging(start) {
//    $("#divSaleInvoiceFollowupboxbody").load("SaleInvoiceFollowup/GetSaleInvoiceFollowupList?ID=" + _emptyGuid + "&SaleInvoiceID=" + $('#SaleInvoiceForm input[type="hidden"]#ID').val() + "&DataTablePaging.Start=" + start, function () {

//    });
//}
//function EditSaleInvoiceFollowup(id) {
//    $("#divModelSaleInvoicePopBody").load("SaleInvoiceFollowup/AddSaleInvoiceFollowup?id=" + id + "&saleInvoiceID=" + $('#SaleInvoiceForm input[type="hidden"]#ID').val() + "&customerID=" + _emptyGuid, function () {
//        $('#lblModelPopSaleInvoice').text('Edit SaleInvoice Followup')
//        $('#divModelPopSaleInvoice').modal('show');
//    });
//}
//function SaveSuccessSaleInvoiceFollowup(data, status) {
//    try {
//        var _jsonData = JSON.parse(data)
//        //message field will return error msg only
//        _message = _jsonData.Message;
//        _status = _jsonData.Status;
//        _result = _jsonData.Record;
//        switch (_status) {
//            case "OK":
//                MasterAlert("success", _result.Message)
//                $("#divModelSaleInvoicePopBody").load("SaleInvoiceFollowup/AddSaleInvoiceFollowup?ID=" + _result.ID + "&SaleInvoiceID=" + $('#SaleInvoiceForm input[type="hidden"]#ID').val(), "&customerID=" + _emptyGuid, function () {
//                    $('#lblModelPopSaleInvoice').text('Edit SaleInvoice Followup')

//                });
//                $("#divFollowupList").load("SaleInvoiceFollowup/GetSaleInvoiceFollowupList?ID=" + _emptyGuid + "&SaleInvoiceID=" + $('#SaleInvoiceForm input[type="hidden"]#ID').val(), function () {
//                });
//                break;
//            case "ERROR":
//                MasterAlert("danger", _message)
//                break;
//            default:
//                console.log(_message);
//                break;
//        }
//    }
//    catch (e) {
//        //this will show the error msg in the browser console(F12) 
//        console.log(e.message);
//    }
//}
//function ConfirmDeleteSaleInvoiceFollowup(ID) {
//    if (ID != _emptyGuid) {
//        notyConfirm('Are you sure to delete?', 'DeleteSaleInvoiceFollowup("' + ID + '")');
//    }
//}
//function DeleteSaleInvoiceFollowup(ID) {
//    if (ID != _emptyGuid && ID != null && ID != '') {
//        var data = { "id": ID };
//        var ds = {};
//        _jsonData = GetDataFromServer("SaleInvoiceFollowup/DeleteSaleInvoiceFollowup/", data);
//        if (_jsonData != '') {
//            _jsonData = JSON.parse(_jsonData);
//            _message = _jsonData.Message;
//            _status = _jsonData.Status;
//            _result = _jsonData.Record;
//        }
//        if (_status == "OK") {
//            notyAlert('success', _result.Message);
//            $("#divFollowupList").load("SaleInvoiceFollowup/GetSaleInvoiceFollowupList?ID=" + _emptyGuid + "&SaleInvoiceID=" + $('#SaleInvoiceForm input[type="hidden"]#ID').val(), function () {
//            });
//        }
//        if (_status == "ERROR") {
//            notyAlert('error', _message);
//        }
//    }
//}
////================================================================================================

//Calculations Methods
function CalculateTotal() {
    var TaxTotal = 0.00, TaxableTotal = 0.00, GrossAmount = 0.00, GrandTotal = 0.00, OtherChargeAmt = 0.00, CessAmt = 0.00, TotalDiscount = 0.00, TotalItemAmt = 0.00;
    var saleInvoiceDetail = _dataTable.SaleInvoiceDetailList.rows().data();
    var saleInvoiceOtherChargeDetail = _dataTable.SaleInvoiceOtherChargesDetailList.rows().data();
    for (var i = 0; i < saleInvoiceDetail.length; i++) {
        var ItemDiscount = (parseFloat(saleInvoiceDetail[i].Discount != "" ? saleInvoiceDetail[i].Discount : 0));
        var itemTotal = parseFloat(saleInvoiceDetail[i].Rate != "" ? saleInvoiceDetail[i].Rate : 0) * parseInt(saleInvoiceDetail[i].Qty != "" ? saleInvoiceDetail[i].Qty : 0);
        var TaxableAmt = (parseFloat(saleInvoiceDetail[i].Rate != "" ? saleInvoiceDetail[i].Rate : 0) * parseInt(saleInvoiceDetail[i].Qty != "" ? saleInvoiceDetail[i].Qty : 1)) - parseFloat(saleInvoiceDetail[i].Discount != "" ? saleInvoiceDetail[i].Discount : 0)
        var CGST = parseFloat(saleInvoiceDetail[i].CGSTPerc != "" ? saleInvoiceDetail[i].CGSTPerc : 0);
        var SGST = parseFloat(saleInvoiceDetail[i].SGSTPerc != "" ? saleInvoiceDetail[i].SGSTPerc : 0);
        var IGST = parseFloat(saleInvoiceDetail[i].IGSTPerc != "" ? saleInvoiceDetail[i].IGSTPerc : 0);
        var CGSTAmt = parseFloat(TaxableAmt * CGST / 100);
        var SGSTAmt = parseFloat(TaxableAmt * SGST / 100);
        var IGSTAmt = parseFloat(TaxableAmt * IGST / 100);
        var GSTAmt = parseFloat(CGSTAmt) + parseFloat(SGSTAmt) + parseFloat(IGSTAmt)
        var TaxAmount = TaxableAmt + parseFloat(GSTAmt);
        var Cess = roundoff(parseFloat(GSTAmt * saleInvoiceDetail[i].CessPerc / 100));
        CessAmt = roundoff(parseFloat(CessAmt) + parseFloat(Cess));
        var GrossTotalAmt = TaxableAmt + GSTAmt
        TotalItemAmt = roundoff(parseFloat(TotalItemAmt) + parseFloat(itemTotal));
        TotalDiscount = roundoff(parseFloat(TotalDiscount) + parseFloat(ItemDiscount));
        TaxTotal = roundoff(parseFloat(TaxTotal) + parseFloat(GSTAmt))
        TaxableTotal = roundoff(parseFloat(TaxableTotal) + parseFloat(TaxableAmt))
        GrossAmount = roundoff(parseFloat(GrossAmount) + parseFloat(GrossTotalAmt))
    }
    for (var i = 0; i < saleInvoiceOtherChargeDetail.length; i++) {
        var CGST = parseFloat(saleInvoiceOtherChargeDetail[i].CGSTPerc != "" ? saleInvoiceOtherChargeDetail[i].CGSTPerc : 0);
        var SGST = parseFloat(saleInvoiceOtherChargeDetail[i].SGSTPerc != "" ? saleInvoiceOtherChargeDetail[i].SGSTPerc : 0);
        var IGST = parseFloat(saleInvoiceOtherChargeDetail[i].IGSTPerc != "" ? saleInvoiceOtherChargeDetail[i].IGSTPerc : 0);
        var CGSTAmt = parseFloat(saleInvoiceOtherChargeDetail[i].ChargeAmount * CGST / 100);
        var SGSTAmt = parseFloat(saleInvoiceOtherChargeDetail[i].ChargeAmount * SGST / 100)
        var IGSTAmt = parseFloat(saleInvoiceOtherChargeDetail[i].ChargeAmount * IGST / 100)
        var GSTAmt = roundoff(parseFloat(CGSTAmt) + parseFloat(SGSTAmt) + parseFloat(IGSTAmt))
        var TaxAmt = parseFloat(saleInvoiceOtherChargeDetail[i].ChargeAmount) + parseFloat(GSTAmt)
        var AddlTax = parseFloat(GSTAmt * saleInvoiceOtherChargeDetail[i].AddlTaxPerc / 100);
        var Total = roundoff(parseFloat(saleInvoiceOtherChargeDetail[i].ChargeAmount) + parseFloat(GSTAmt)+ parseFloat(AddlTax))
        OtherChargeAmt = roundoff(parseFloat(OtherChargeAmt) + parseFloat(Total))
    }
    GrossAmount = roundoff(parseFloat(GrossAmount) + parseFloat(OtherChargeAmt) + parseFloat(CessAmt))
    $('#lblTotalItemAmount').text(roundoff(TotalItemAmt));
    $('#lblDiscountTotal').text(roundoff(TotalDiscount));
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
    $('#Discount').val('0.00');
}

function CalculateGrandTotal(value) {
    var GrandTotal = roundoff(parseFloat($('#lblGrossAmount').text()) - parseFloat(value != "" ? value : 0))
    $('#lblGrandTotal').text(formatCurrency(GrandTotal));
}


//=========================================================================================================================
//Email SaleInvoice
//
function EmailSaleInvoice() {
    $("#divModelEmailSaleInvoiceBody").load("SaleInvoice/EmailSaleInvoice?ID=" + $('#SaleInvoiceForm #ID').val() + "&EmailFlag=True", function () {
        $('#lblModelEmailSaleInvoice').text('Email Sale Invoice')
        $('#divModelEmailSaleInvoice').modal('show');
    });
}

function SendSaleInvoiceEmail() {
    if ($('#hdnEmailSentTo').val() != null && $('#hdnEmailSentTo').val() != "" && $('#Subject').val() != null) {
        $('#hdnMailBodyHeader').val($('#MailBodyHeader').val());
        $('#hdnMailBodyFooter').val($('#MailBodyFooter').val());
        $('#hdnSaleInvoiceEMailContent').val($('#divSaleInvoiceEmailcontainer').html());
        $('#hdnSaleInvNo').val($('#SaleInvNo').val());
        $('#hdnContactPerson').val($('#ContactPerson').text());
        $('#hdnSaleInvDateFormatted').val($('#SaleInvDateFormatted').val());
        $('#FormSaleInvoiceEmailSend #ID').val($('#SaleInvoiceForm #ID').val());
        $('#FormSaleInvoiceEmailSend').submit();
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
function UpdateSaleInvoiceEmailInfo() {
    $('#hdnMailBodyHeader').val($('#MailBodyHeader').val());
    $('#hdnMailBodyFooter').val($('#MailBodyFooter').val());
    $('#FormUpdateSaleInvoiceEmailInfo #ID').val($('#SaleInvoiceForm #ID').val());
}
function DownloadSaleInvoice() {
    var bodyContent = $('#divSaleInvoiceEmailcontainer').html();
    var headerContent = $('#hdnHeadContent').html();
    $('#hdnContent').val(bodyContent);
    $('#hdnHeadContent').val(headerContent);
    //var customerName = $("#SaleInvoiceForm #CustomerID option:selected").text();
    //$('#hdnCustomerName').val(customerName);
}
function PrintSaleInvoice() {
    debugger;
    $("#divModelPrintSaleInvoiceBody").load("SaleInvoice/PrintSaleInvoice?ID=" + $('#SaleInvoiceForm #ID').val(), function () {
        $('#lblModelPrintSaleInvoice').text('Print Sale Invoice');
        $('#divModelPrintSaleInvoice').modal('show');
    });
}
function SaveSuccessUpdateSaleInvoiceEmailInfo(data, status) {
    try {
        var _jsonData = JSON.parse(data)
        //message field will return error msg only
        _message = _jsonData.Message;
        _status = _jsonData.Status;
        _result = _jsonData.Record;
        switch (_status) {
            case "OK":
                //MasterAlert("success", _result.Message)
                $("#divModelEmailSaleInvoiceBody").load("SaleInvoice/EmailSaleInvoice?ID=" + $('#SaleInvoiceForm #ID').val() + "&EmailFlag=False", function () {
                    $('#lblModelEmailSaleInvoice').text('Email Attachment')
                });
                break;
            case "ERROR":
                //MasterAlert("success", _message)
                $('#divModelEmailSaleInvoice').modal('hide');
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
function SaveSuccessSaleInvoiceEmailSend(data, status) {
    try {
        var _jsonData = JSON.parse(data)
        //message field will return error msg only
        _message = _jsonData.Message;
        _status = _jsonData.Status;
        _result = _jsonData.Record;
        switch (_status) {
            case "OK":
                MasterAlert("success", _message)
                $('#divModelEmailSaleInvoice').modal('hide');
                ResetSaleInvoice();
                break;
            case "ERROR":
                MasterAlert("success", _message)
                $('#divModelEmailSaleInvoice').modal('hide');
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
function InvoiceTypeOnChange(curObj)
{
    debugger;
    if (curObj == 'SB') {
        debugger;
        notyConfirm('Are you sure?', 'ClearSaleInvoiceDetailList();', 'This will clear the detail section and will allow to add service items only !', "Continue", 0);

    }
    else {

        $('#btnAddServiceBill').css("display", "none");
        $('#btnAddItems').css("display", "block")
        $('#btnAddOtherExpenses').css("display", "block");
        $('#divSaleInvoiceOtherChargesDetailList').show();

    }
    $(".cancel").click(function () {
        $('#InvoiceType').val('RB').trigger('change');
    });
    $('#hdnInvoiceType').val($('#InvoiceType').val());
    //if(curObj=='SB')
    //{
    //    $('#btnAddServiceBill').css("display", "block")
    //    $('#btnAddItems').css("display", "none");
    //}
    //else
    //{
    //    $('#btnAddServiceBill').css("display", "none");
    //    $('#btnAddItems').css("display", "block")
    //}
}

function ClearSaleInvoiceDetailList() {
    debugger;

    $(".sweet-alert.showSweetAlert").hide();
    $(".sweet-overlay").hide();
    $("#divServiceBill").append("<span class='form-control newinput' style='background-color:#eeeeee'>" + "Service Bill" + '</span>');
    // $('#InvoiceType').attr("disabled", "disabled");
    $("#divInvoiceType").hide();
    $('#btnAddServiceBill').css("display", "block")
    $('#btnAddItems').css("display", "none");
    $('#btnAddOtherExpenses').css("display", "none");
    var SaleInvoiceDetailList = [];
    _dataTable.SaleInvoiceDetailList.clear().rows.add(SaleInvoiceDetailList).draw(false);
    $('#divSaleInvoiceOtherChargesDetailList').hide();
    $('#hdnInvoiceType').val($('#InvoiceType').val());
    ClearCalculatedFields();
}

function AddSaleInvoiceServiceBillList()
{
    $("#divModelSaleInvoicePopBody").load("SaleInvoice/AddSaleInvoiceServiceBill?update=false", function () {
        $('#lblModelPopSaleInvoice').text('Service Invoice Detail')
        $('#divModelPopSaleInvoice').modal('show');
    });
}

 
function AddSaleInvoiceServiceBillToDetailList() {
    $("#FormSaleInvoiceServiceBill").submit(function () { });

    if ($('#FormSaleInvoiceServiceBill #IsUpdate').val() == 'True') {
        if (($('#OtherChargeCode').val() != "") && ($('#Rate').val() >0) && ($('#Qty').val() >0) && ($('#UnitCode').val() != "")) {
            var saleInvoiceDetailList = _dataTable.SaleInvoiceDetailList.rows().data();

          //  saleInvoiceDetailList[_datatablerowindex].OtherCharge.Description = $("#divModelSaleInvoicePopBody #OtherChargeCode").val() != "" ? $("#divModelSaleInvoicePopBody #OtherChargeCode option:selected").text().split("-")[0].trim() : "";
            saleInvoiceDetailList[_datatablerowindex].OtherCharge.Description = $('#spanOtherCharge').text() != "" ? $('#spanOtherCharge').text().split("-")[0].trim() : "";
            saleInvoiceDetailList[_datatablerowindex].OtherChargeCode = $("#hdnOtherChargeCode").val();
            saleInvoiceDetailList[_datatablerowindex].OtherCharge.SACCode = $("#hdnOtherChargeSACCode").val();
            saleInvoiceDetailList[_datatablerowindex].Qty = $('#Qty').val();
            saleInvoiceDetailList[_datatablerowindex].UnitCode = $('#UnitCode').val();
            saleInvoiceDetailList[_datatablerowindex].Unit.Description = $("#UnitCode").val() != "" ? $("#UnitCode option:selected").text().trim() : "";
            //saleInvoiceDetailList[_datatablerowindex].Unit = Unit;
            saleInvoiceDetailList[_datatablerowindex].Rate = $('#Rate').val();
            saleInvoiceDetailList[_datatablerowindex].Discount = $('#divModelSaleInvoicePopBody #Discount').val() != "" ? $('#divModelSaleInvoicePopBody #Discount').val() : 0;
            if ($('#divModelSaleInvoicePopBody #TaxTypeCode').val() != null)
                saleInvoiceDetailList[_datatablerowindex].TaxTypeCode = $('#divModelSaleInvoicePopBody #TaxTypeCode').val().split('|')[0];
            saleInvoiceDetailList[_datatablerowindex].TaxType = new Object;
            saleInvoiceDetailList[_datatablerowindex].TaxType.ValueText = $('#divModelSaleInvoicePopBody #TaxTypeCode').val();
            saleInvoiceDetailList[_datatablerowindex].CGSTPerc = $('#divModelSaleInvoicePopBody #hdnCGSTPerc').val();
            saleInvoiceDetailList[_datatablerowindex].SGSTPerc = $('#divModelSaleInvoicePopBody #hdnSGSTPerc').val();
            saleInvoiceDetailList[_datatablerowindex].IGSTPerc = $('#divModelSaleInvoicePopBody #hdnIGSTPerc').val();
            saleInvoiceDetailList[_datatablerowindex].CessPerc = $('#divModelSaleInvoicePopBody #CessPerc').val() != "" ? $('#divModelSaleInvoicePopBody #CessPerc').val() : 0;
            saleInvoiceDetailList[_datatablerowindex].CessAmt = $('#divModelSaleInvoicePopBody #CessAmt').val();

            _dataTable.SaleInvoiceDetailList.clear().rows.add(saleInvoiceDetailList).draw(false);
            CalculateTotal();
            $('#divModelPopSaleInvoice').modal('hide');
            _datatablerowindex = -1;
        }
    }
    else {
        if (($('#OtherChargeCode').val() != "") && ($('#Rate').val() >0) && ($('#Qty').val() >0) && ($('#UnitCode').val() != "")) {
            if (_dataTable.SaleInvoiceDetailList.rows().data().length === 0) {
                _dataTable.SaleInvoiceDetailList.clear().rows.add(GetSaleInvoiceDetailListBySaleInvoiceID(_emptyGuid)).draw(false);
                var saleInvoiceDetailVM = _dataTable.SaleInvoiceDetailList.rows().data();
                saleInvoiceDetailVM.OtherCharge = new Object;
                saleInvoiceDetailVM.Unit = new Object;
                saleInvoiceDetailVM.TaxType = new Object;
                saleInvoiceDetailVM.Product = new Object;
                saleInvoiceDetailVM[0].OtherCharge.Description = $("#divModelSaleInvoicePopBody #OtherChargeCode").val() != "" ? $("#divModelSaleInvoicePopBody #OtherChargeCode option:selected").text().split("-")[0].trim() : "";
                saleInvoiceDetailVM[0].OtherChargeCode = $("#OtherChargeCode").val();
                saleInvoiceDetailVM[0].OtherCharge.SACCode = $("#hdnOtherChargeSACCode").val();
                saleInvoiceDetailVM[0].Qty = $('#divModelSaleInvoicePopBody #Qty').val();
                saleInvoiceDetailVM[0].UnitCode = $('#divModelSaleInvoicePopBody #UnitCode').val();
                saleInvoiceDetailVM[0].Unit.Description = $("#divModelSaleInvoicePopBody #UnitCode").val() != "" ? $("#divModelSaleInvoicePopBody #UnitCode option:selected").text().trim() : "";
                saleInvoiceDetailVM[0].Rate = $('#divModelSaleInvoicePopBody #Rate').val();
                saleInvoiceDetailVM[0].Discount = $('#divModelSaleInvoicePopBody #Discount').val() != "" ? $('#divModelSaleInvoicePopBody #Discount').val() : 0;
                saleInvoiceDetailVM[0].TaxTypeCode = $('#divModelSaleInvoicePopBody #TaxTypeCode').val().split('|')[0];
                saleInvoiceDetailVM[0].TaxType.ValueText = $('#divModelSaleInvoicePopBody #TaxTypeCode').val();
                saleInvoiceDetailVM[0].CGSTPerc = $('#divModelSaleInvoicePopBody #hdnCGSTPerc').val();
                saleInvoiceDetailVM[0].SGSTPerc = $('#divModelSaleInvoicePopBody #hdnSGSTPerc').val();
                saleInvoiceDetailVM[0].IGSTPerc = $('#divModelSaleInvoicePopBody #hdnIGSTPerc').val();
                saleInvoiceDetailVM[0].CessPerc = $('#divModelSaleInvoicePopBody #CessPerc').val() != "" ? $('#divModelSaleInvoicePopBody #CessPerc').val() : 0;
                saleInvoiceDetailVM[0].CessAmt = $('#divModelSaleInvoicePopBody #CessAmt').val();
                ClearCalculatedFields();
                _dataTable.SaleInvoiceDetailList.clear().rows.add(saleInvoiceDetailVM).draw(false);
                CalculateTotal();
                $('#divModelPopSaleInvoice').modal('hide');
            }
            else {
                var saleInvoiceDetailVM = _dataTable.SaleInvoiceDetailList.rows().data();
                if (saleInvoiceDetailVM.length > 0) {
                    var checkpoint = 0;
                    for (var i = 0; i < saleInvoiceDetailVM.length; i++) {
                        if (saleInvoiceDetailVM[i].OtherChargeCode == $('#OtherChargeCode').val()) {
                            saleInvoiceDetailVM[i].Qty = parseFloat(saleInvoiceDetailVM[i].Qty) + parseFloat($('#Qty').val());
                            checkpoint = 1;
                            break;
                        }
                    }
                    if (checkpoint == 1) {
                        _dataTable.SaleInvoiceDetailList.clear().rows.add(saleInvoiceDetailVM).draw(false);
                    }
                    else if (checkpoint == 0) {
                        var SaleInvoiceDetailVM = new Object();
                        SaleInvoiceDetailVM.Unit = new Object();
                        SaleInvoiceDetailVM.TaxType = new Object();
                        SaleInvoiceDetailVM.OtherCharge = new Object;
                        SaleInvoiceDetailVM.ID = _emptyGuid;
                        SaleInvoiceDetailVM.Product = new Object;

                        SaleInvoiceDetailVM.OtherCharge.Description = $("#divModelSaleInvoicePopBody #OtherChargeCode").val() != "" ? $("#divModelSaleInvoicePopBody #OtherChargeCode option:selected").text().split("-")[0].trim() : "";
                        SaleInvoiceDetailVM.OtherChargeCode = $("#divModelSaleInvoicePopBody #OtherChargeCode").val();
                        SaleInvoiceDetailVM.OtherCharge.SACCode = $("#hdnOtherChargeSACCode").val();
                        SaleInvoiceDetailVM.Qty = $('#divModelSaleInvoicePopBody #Qty').val();
                        SaleInvoiceDetailVM.UnitCode = $('#divModelSaleInvoicePopBody #UnitCode').val();
                        SaleInvoiceDetailVM.Unit.Description = $("#divModelSaleInvoicePopBody #UnitCode").val() != "" ? $("#divModelSaleInvoicePopBody #UnitCode option:selected").text().trim() : "";
                        SaleInvoiceDetailVM.Rate = $('#divModelSaleInvoicePopBody #Rate').val();
                        SaleInvoiceDetailVM.Discount = $('#divModelSaleInvoicePopBody #Discount').val() != "" ? $('#divModelSaleInvoicePopBody #Discount').val() : 0;
                        SaleInvoiceDetailVM.TaxTypeCode = $('#divModelSaleInvoicePopBody #TaxTypeCode').val().split('|')[0];
                        SaleInvoiceDetailVM.TaxType.ValueText = $('#divModelSaleInvoicePopBody #TaxTypeCode').val();
                        SaleInvoiceDetailVM.CGSTPerc = $('#divModelSaleInvoicePopBody #hdnCGSTPerc').val();
                        SaleInvoiceDetailVM.SGSTPerc = $('#divModelSaleInvoicePopBody #hdnSGSTPerc').val();
                        SaleInvoiceDetailVM.IGSTPerc = $('#divModelSaleInvoicePopBody #hdnIGSTPerc').val();
                        SaleInvoiceDetailVM.CessPerc = $('#divModelSaleInvoicePopBody #CessPerc').val() != "" ? $('#divModelSaleInvoicePopBody #CessPerc').val() : 0;
                        SaleInvoiceDetailVM.CessAmt = $('#divModelSaleInvoicePopBody #CessAmt').val();
                        _dataTable.SaleInvoiceDetailList.row.add(SaleInvoiceDetailVM).draw(true);
                    }
                    CalculateTotal();
                    $('#divModelPopSaleInvoice').modal('hide');
                }
            }
        }
    }
    $('[data-toggle="popover"]').popover({
        html: true,
        'trigger': 'hover'
    });
}

function EditSaleInvoiceServiceBill(this_Obj)
{
    _datatablerowindex = _dataTable.SaleInvoiceDetailList.row($(this_Obj).parents('tr')).index();
    var saleInvoiceDetail = _dataTable.SaleInvoiceDetailList.row($(this_Obj).parents('tr')).data();
    $("#divModelSaleInvoicePopBody").load("SaleInvoice/AddSaleInvoiceServiceBill?update=true", function () {
        $('#lblModelPopSaleInvoice').text('SaleInvoice Detail')
        $('#FormSaleInvoiceServiceBill #IsUpdate').val('True');
        $('#FormSaleInvoiceServiceBill #ID').val(saleInvoiceDetail.ID);
        $('#spanOtherCharge').text(saleInvoiceDetail.OtherCharge.Description)
        $("#FormSaleInvoiceServiceBill #OtherChargeCode").val(saleInvoiceDetail.OtherChargeCode)
        $("#FormSaleInvoiceServiceBill #hdnOtherChargeCode").val(saleInvoiceDetail.OtherChargeCode)
       
        $('#FormSaleInvoiceServiceBill #Qty').val(saleInvoiceDetail.Qty);
        $('#FormSaleInvoiceServiceBill #UnitCode').val(saleInvoiceDetail.UnitCode);
        $('#FormSaleInvoiceServiceBill #hdnUnitCode').val(saleInvoiceDetail.UnitCode);
        $('#FormSaleInvoiceServiceBill #Rate').val(saleInvoiceDetail.Rate);
        $('#FormSaleInvoiceServiceBill #Discount').val(saleInvoiceDetail.Discount);
        if (saleInvoiceDetail.TaxTypeCode != 0) {
            $('#FormSaleInvoiceServiceBill #TaxTypeCode').val(saleInvoiceDetail.TaxType.ValueText);
            $('#FormSaleInvoiceServiceBill #hdnTaxTypeCode').val(saleInvoiceDetail.TaxType.ValueText);
        }
        $('#FormSaleInvoiceServiceBill #hdnCGSTPerc').val(saleInvoiceDetail.CGSTPerc);
        $('#FormSaleInvoiceServiceBill #hdnSGSTPerc').val(saleInvoiceDetail.SGSTPerc);
        $('#FormSaleInvoiceServiceBill #hdnIGSTPerc').val(saleInvoiceDetail.IGSTPerc);
        var TaxableAmt = ((parseFloat(saleInvoiceDetail.Rate) * parseInt(saleInvoiceDetail.Qty)) - parseFloat(saleInvoiceDetail.Discount))
        var CGSTAmt = (TaxableAmt * parseFloat(saleInvoiceDetail.CGSTPerc)) / 100;
        var SGSTAmt = (TaxableAmt * parseFloat(saleInvoiceDetail.SGSTPerc)) / 100;
        var IGSTAmt = (TaxableAmt * parseFloat(saleInvoiceDetail.IGSTPerc)) / 100;
        $('#FormSaleInvoiceServiceBill #CGSTPerc').val(CGSTAmt);
        $('#FormSaleInvoiceServiceBill #SGSTPerc').val(SGSTAmt);
        $('#FormSaleInvoiceServiceBill #IGSTPerc').val(IGSTAmt);
        $('#FormSaleInvoiceServiceBill #CessPerc').val(saleInvoiceDetail.CessPerc);
        $('#FormSaleInvoiceServiceBill #CessAmt').val(saleInvoiceDetail.CessAmt);

        $('#divModelPopSaleInvoice').modal('show');
    });
}
function EditRedirectToDocument(id) {
    debugger;
    OnServerCallBegin();

    $("#divSaleInvoiceForm").load("SaleInvoice/SaleInvoiceForm?id=" + id, function (responseTxt, statusTxt, xhr) {
        if (statusTxt == "success") {
            OnServerCallComplete();
            openNav();
            $('#lblSaleInvoiceInfo').text($('#SaleInvNo').val());
            if ($('#IsDocLocked').val() == "True") {
                ChangeButtonPatchView("SaleInvoice", "btnPatchSaleInvoiceNew", "Edit", id);
            }
            else {
                //$('.switch-label,.switch-handle').addClass('switch-disabled').addClass('disabled');
                //$('.switch-input').prop('disabled', true);
                //$('.switch-label').attr('title', 'Document Locked');
                ChangeButtonPatchView("SaleInvoice", "btnPatchSaleInvoiceNew", "LockDocument", id);
            }
            _SlNo = 1;
            BindSaleInvoiceDetailList(id);
            _SlNoOtherCharge = 1;
            BindSaleInvoiceOtherChargesDetailList(id);
            CalculateTotal();
            $('#divCustomerBasicInfo').load("Customer/CustomerBasicInfo?ID=" + $('#hdnCustomerID').val());
            clearUploadControl();
            PaintImages(id);
            debugger;
            if ($("#hdnInvoiceType").val() == "SB") {
                $('#btnAddItems').css("display", "none");
                $('#btnAddOtherExpenses').css("display", "none");
                $('#divSaleInvoiceOtherChargesDetailList').hide();

            }
            //if ($('#hdnDescription').val() == "OPEN") {
            //    $('.switch-input').prop('checked', true);

            //} else {
            //    $('.switch-input').prop('checked', false);

            //}
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
function GetSaleInvoiceXML()
{
    $('#lblModelXMLSaleInvoice').text('Generate Invoice XML For Tally')
    $('#divModelXMLSaleInvoice').modal('show');
    bindSaleInvoiceTblForXML();
}
function GetXML()
{
    debugger;
    var ids;
    // save timestamp in hidden field
    $('#cookieValue').val(Date.now().toString());
    // start timer to wait for cookie
    _tmr = window.setInterval(function () {
        var _str = 'dlc=' + $('#cookieValue').val();
        if (document.cookie.indexOf(_str) !== -1) {

        $('#cookieValue').val('jk');//Setting cookie value to come out of the loop//
        $.ajax({
            url: '/' + 'SaleInvoice' + '/UpdateSaleInvoiceTallyStatus',
            type: "POST",
            data: JSON.stringify({ 'ids': ids }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                bindSaleInvoiceTblForXML();
                clearInterval(_tmr);
            },
            error: function (err) {
                alert(err.statusText);
            }
        });

    }
}, 100);
    ids = selectedInvoiceIDs();//identify the selected rows 
    if (ids != "") {
        $('#ids').val(ids);
        var saleInvoiceDetailsList = GetSaleInvoiceDetailByIDs();
        var flag = 0;
        var saleInvoiceVM = _dataTable.XMLSaleInvoiceList.rows().data();
        debugger;
        for (var r = 0; r < saleInvoiceDetailsList.length; r++) {
            if(saleInvoiceDetailsList[r].Customer.TallyName == null || saleInvoiceDetailsList[r].Customer.TallyName=="")
            {
                for (i = 0; i < saleInvoiceVM.length; i++)
                {
                    if (saleInvoiceVM[i].ID == saleInvoiceDetailsList[r].ID)
                        break;
                }
                
                var row=_dataTable.XMLSaleInvoiceList.row(i).node();
                $('td', row).css('color', 'red');
                flag = 1;
            }
            else
            {
                debugger;
                for (var t = 0; t < saleInvoiceDetailsList[r].SaleInvoiceDetailList.length; t++)
                {
                    if (saleInvoiceDetailsList[r].SaleInvoiceDetailList[t].Product.TallyName == null || saleInvoiceDetailsList[r].SaleInvoiceDetailList[t].Product.TallyName == "" || (saleInvoiceDetailsList[r].SaleInvoiceDetailList[t].TaxTypeCode != 0 && (saleInvoiceDetailsList[r].SaleInvoiceDetailList[t].TaxType.TallyName == null || saleInvoiceDetailsList[r].SaleInvoiceDetailList[t].TaxType.TallyName == "")))
                    {
                        for (i = 0; i < saleInvoiceVM.length; i++) {
                            if (saleInvoiceVM[i].ID == saleInvoiceDetailsList[r].ID)
                                break;
                        }

                        var row = _dataTable.XMLSaleInvoiceList.row(i).node();
                        $('td', row).css('color', 'red');
                        flag = 1;
                    }
                }
            }
        }
        if (flag == 0) {
            _dataTable.XMLSaleInvoiceList.destroy();
            $('#FormXMLExport').submit();
            $('#divModelXMLSaleInvoice').modal('hide');
        }
        else {
            document.getElementById('Errormsglbl').style.display = 'inline';
        }
    }
    else {
        notyAlert('error','Select invoices to generate XML');
    }
}
function GetSaleInvoiceList() {
    try{
        debugger;
        var data = { "invoiceType": $('#ddlInvoiceFilter').val() };
        _jsonData = GetDataFromServer("SaleInvoice/GetSaleInvoiceListForXMLGeneration/", data);
        if (_jsonData != '') {
            debugger;
            _jsonData = JSON.parse(_jsonData);
            _message = _jsonData.Message;
            _status = _jsonData.Status;
            SaleInvoiceList = _jsonData.Records;
        }
        if (_status == "OK") {
            return SaleInvoiceList;
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
function bindSaleInvoiceTblForXML()
{
    _dataTable.XMLSaleInvoiceList = $('#tblXMLSaleInvoice').DataTable(
         {
             dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
             order: [],
             searching: false,
             paging: false,
             autoWidth: false,
             ordering: false,
             bInfo: false,
             //data: data,
             language: {
                 search: "_INPUT_",
                 searchPlaceholder: "Search"
             },
             serverSide: false,
             data: GetSaleInvoiceList()
             ,
             pageLength: 8,
             columns: [
                   { "data": "Checkbox", "defaultContent": "", "width": "5%" },
                   { "data": "SaleInvNo", "defaultContent": "<i>-</i>" },
                   { "data": "SaleInvDateFormatted", "defaultContent": "<i>-</i>" },
                   { "data": "Customer.CompanyName", "defaultContent": "<i>-</i>" },
                   {
                       "data": "TallyStatus", render: function (data, type, row) {
                           if (data==0)
                               return '<span class="label label-danger">Not Exported</span>'
                           else if(data==1)
                               return '<span class="label label-success">Exported</span>'     
                           else 
                               return '<span class="label label-primary">Modified</span>'

                       }, "defaultContent": "<i>-</i>"
                   },
             ],
             columnDefs: [{ orderable: false, className: 'select-checkbox', "targets": 0 }
                 , { className: "text-left", "targets": [1,3,4] }
                 , { className: "text-center", "targets": [2] }
                 , { "targets": [], "bSortable": false }
                , { "targets": [1], "width": "20%" }
                , { "targets": [4], "width": "15%" }
                , { "targets": [3], "width": "40%" }
                , { "targets": [2], "width": "20%" }],
             select: { style: 'multi', selector: 'td:first-child' },
             destroy: true
         });
}
//Selected Rows
function selectedInvoiceIDs() {
    debugger;
    var saleInvoiceVM = _dataTable.XMLSaleInvoiceList.rows(".selected").data();
    var saleInvoiceIDs = [];
    for (var r = 0; r < saleInvoiceVM.length; r++) {
        saleInvoiceIDs.push(saleInvoiceVM[r].ID);
    }
    return saleInvoiceIDs.toString();
}
function GetSaleInvoiceDetailByIDs(ids) {
    try{
        debugger;
        var data = { "ids": $('#ids').val() };
        _jsonData = GetDataFromServer("SaleInvoice/GetSaleInvoiceBySaleInvoiceIDs/", data);
        if (_jsonData != '') {
            debugger;
            _jsonData = JSON.parse(_jsonData);
            _message = _jsonData.Message;
            _status = _jsonData.Status;
            SaleInvoiceList = _jsonData.Records;
        }
        if (_status == "OK") {
            return SaleInvoiceList;
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

function ClearSaleInvoiceform() {
    debugger;
    ResetSaleInvoice();
    $('.showSweetAlert .cancel').click();
}