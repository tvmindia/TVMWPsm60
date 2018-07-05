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
            if (this.textContent !== "No data available in table")
                EditSaleOrder(this);
        });
        if ($('#RedirectToDocument').val() != "") {
            if ($('#RedirectToDocument').val() === _emptyGuid) {
                AddSaleOrder();
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
                $('.divboxASearch #AdvFromDate').val('');
                $('.divboxASearch #AdvToDate').val('');
                $('.divboxASearch #AdvAreaCode').val('').trigger('change');
                $('.divboxASearch #AdvCustomerID').val('').trigger('change');
                $('.divboxASearch #AdvReferencePersonCode').val('').trigger('change');
                $('.divboxASearch #AdvBranchCode').val('').trigger('change');
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
                $('.divboxASearch #AdvReferencePersonCode').val('');
                $('.divboxASearch #AdvBranchCode').val('');
                $('.divboxASearch #AdvDocumentStatusCode').val('');
                $('.divboxASearch #AdvDocumentOwnerID').val('');
                $('.divboxASearch #AdvApprovalStatusCode').val('');
                $('#AdvEmailSentStatus').val('');
                break;
            case 'Search':
                if (($('#SearchTerm').val() == "") && ($('.divboxASearch #AdvFromDate').val() == "") && ($('#AdvToDate').val() == "") && ($('.divboxASearch #AdvAreaCode').val() == "") && ($('.divboxASearch #AdvCustomerID').val() == "") && ($('.divboxASearch #AdvReferencePersonCode').val() == "") && ($('.divboxASearch #AdvBranchCode').val() == "") && ($('.divboxASearch #AdvDocumentStatusCode').val() == "") && ($('.divboxASearch #AdvDocumentOwnerID').val() == "") && ($('#AdvEmailSentStatus').val() == "") && ($('#AdvApprovalStatusCode').val() == "")) {
                    return true;
                }
                break;
            case 'Export':
                DataTablePagingViewModel.Length = -1;
                SaleOrderAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
                SaleOrderAdvanceSearchViewModel.SearchTerm = $('#SearchTerm').val() == "" ? null : $('#SearchTerm').val();
                SaleOrderAdvanceSearchViewModel.AdvFromDate = $('.divboxASearch #AdvFromDate').val() == "" ? null : $('.divboxASearch #AdvFromDate').val();
                SaleOrderAdvanceSearchViewModel.AdvToDate = $('.divboxASearch #AdvToDate').val() == "" ? null : $('.divboxASearch #AdvToDate').val();
                SaleOrderAdvanceSearchViewModel.AdvAreaCode = $('.divboxASearch #AdvAreaCode').val() == "" ? null : $('.divboxASearch #AdvAreaCode').val();
                SaleOrderAdvanceSearchViewModel.AdvCustomerID = $('.divboxASearch #AdvCustomerID').val() == "" ? _emptyGuid : $('.divboxASearch #AdvCustomerID').val();
                SaleOrderAdvanceSearchViewModel.AdvReferencePersonCode = $('.divboxASearch #AdvReferencePersonCode').val() == "" ? null : $('.divboxASearch #AdvReferencePersonCode').val();
                SaleOrderAdvanceSearchViewModel.AdvBranchCode = $('.divboxASearch #AdvBranchCode').val() == "" ? null : $('.divboxASearch #AdvBranchCode').val();
                SaleOrderAdvanceSearchViewModel.AdvDocumentStatusCode = $('.divboxASearch #AdvDocumentStatusCode').val() == "" ? null : $('.divboxASearch #AdvDocumentStatusCode').val();
                SaleOrderAdvanceSearchViewModel.AdvDocumentOwnerID = $('.divboxASearch #AdvDocumentOwnerID').val() == "" ? _emptyGuid : $('.divboxASearch #AdvDocumentOwnerID').val();
                SaleOrderAdvanceSearchViewModel.AdvApprovalStatusCode = $('#AdvApprovalStatusCode').val() == "" ? null : $('#AdvApprovalStatusCode').val();
                SaleOrderAdvanceSearchViewModel.AdvEmailSentStatus = $('#AdvEmailSentStatus').val() == "" ? null : $('#AdvEmailSentStatus').val();
                $('#AdvanceSearch').val(JSON.stringify(SaleOrderAdvanceSearchViewModel));
                $('#FormExcelExport').submit();
                return true;
                break;
            default:
                break;
        }
        SaleOrderAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
        SaleOrderAdvanceSearchViewModel.SearchTerm = $('#SearchTerm').val();
        SaleOrderAdvanceSearchViewModel.AdvFromDate = $('.divboxASearch #AdvFromDate').val();
        SaleOrderAdvanceSearchViewModel.AdvToDate = $('.divboxASearch #AdvToDate').val();
        SaleOrderAdvanceSearchViewModel.AdvAreaCode = $('.divboxASearch #AdvAreaCode').val();
        SaleOrderAdvanceSearchViewModel.AdvCustomerID = $('.divboxASearch #AdvCustomerID').val();
        SaleOrderAdvanceSearchViewModel.AdvReferencePersonCode = $('.divboxASearch #AdvReferencePersonCode').val();
        SaleOrderAdvanceSearchViewModel.AdvBranchCode = $('.divboxASearch #AdvBranchCode').val();
        SaleOrderAdvanceSearchViewModel.AdvDocumentStatusCode = $('.divboxASearch #AdvDocumentStatusCode').val();
        SaleOrderAdvanceSearchViewModel.AdvDocumentOwnerID = $('.divboxASearch #AdvDocumentOwnerID').val();
        SaleOrderAdvanceSearchViewModel.AdvApprovalStatusCode = $('.divboxASearch #AdvApprovalStatusCode').val();
        SaleOrderAdvanceSearchViewModel.AdvEmailSentStatus = $('#AdvEmailSentStatus').val();
        //apply datatable plugin on SaleOrder table
        _dataTable.SaleOrderList = $('#tblSaleOrder').DataTable(
        {
            dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
            //buttons: [{
            //    extend: 'excel',
            //    exportOptions:
            //                 {
            //                     columns: [0, 1, 2, 3, 4, 5]
            //                 }
            //}],
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
            pageLength: 8,
            autoWidth:false,
            columns: [
               {
                   "data": "SaleOrderNo", render: function (data, type, row) {
                       return row.SaleOrderNo + "</br>" + "<img src='./Content/images/datePicker.png' height='10px'>" + "&nbsp;" + row.SaleOrderDateFormatted;
                   }, "defaultContent": "<i>-</i>"
               },
               {
                   "data": "Customer.CompanyName", render: function (data, type, row) {
                       return "<img src='./Content/images/contact.png' height='10px'>" + "&nbsp;" + (row.Customer.ContactPerson == null ? "" : row.Customer.ContactPerson) + "</br>" + "<img src='./Content/images/organisation.png' height='10px'>" + "&nbsp;" + data;
                   }, "defaultContent": "<i>-</i>"
               },
               {
                   "data": "Quotation.QuoteNo", render: function (data, type, row) {
                       return (data == null ? row.Enquiry.EnquiryNo : data);

               }, "defaultContent": "<i>-</i>"
               },
               { "data": "Area.Description", "defaultContent": "<i>-</i>" },
               { "data": "ReferencePerson.Name", "defaultContent": "<i>-</i>" },
               {
                   "data": "Branch.Description", render: function (data, type, row) {
                       return "<b>Doc.Owner-</b>" + row.PSAUser.LoginName + "</br>" + "<b>Branch-</b>" + row.Branch.Description;
                   }, "defaultContent": "<i>-</i>"
               },
               //{ "data": "UserName", "defaultContent": "<i>-</i>" },
               {
                   "data": "DocumentStatus.Description", render: function (data, type, row) {
                       return "<b>Doc.Status-</b>" + data + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + (row.EmailSentYN == true ? "<img src='./Content/images/mailSend.png' height='20px'  >" : '')


                           + "</br>" + "<b>Appr.Status-</b>" + row.ApprovalStatus.Description;
                   }, "defaultContent": "<i>-</i>"
               },
               { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink"  onclick="EditSaleOrder(this)" ><i class="fa fa-pencil-square-o" aria-hidden="true"></i></a>' },
            ],
            columnDefs: [{ className: "text-right", "targets": [] },
                         { className: "text-left", "targets": [0, 1, 2, 3, 4, 5, 6] },
                         { className: "text-center", "targets": [7] },
                           { "targets": [0], "width": "12%" },
                           { "targets": [1], "width": "12%" },
                           { "targets": [2], "width": "12%" },
                           { "targets": [3], "width": "9%" },
                           { "targets": [4], "width": "9%" },
                           { "targets": [5], "width": "13%" },
                           { "targets": [6], "width": "22%" },
                           { "targets": [7], "width": "5%" },
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
                //if (action === 'Export') {
                //    if (json.data.length > 0) {
                //        if (json.data[0].TotalCount > 1000) {
                //            setTimeout(function () {
                //                MasterAlert("info", 'We are able to download maximum 1000 rows of data, There exist more than 1000 rows of data please filter and download')
                //            }, 10000)
                //        }
                //    }
                //    $(".buttons-excel").trigger('click');
                //    BindOrReloadSaleOrderTable();
                //}
            }
        });
       // $(".buttons-excel").hide();
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
    //$('.excelExport').show();
    //OnServerCallBegin();
    BindOrReloadSaleOrderTable('Export');
}
// add SaleOrder section
function AddSaleOrder() {
    debugger;
    //this will return form body(html)
    OnServerCallBegin();
    $("#divSaleOrderForm").load("SaleOrder/SaleOrderForm?id=" + _emptyGuid , function (responseTxt, statusTxt, xhr) {
        if (statusTxt == "success") {
            OnServerCallComplete();
            openNav();
            $('#lblSaleOrderInfo').text('<<Sale Order No.>>');
            ChangeButtonPatchView("SaleOrder", "btnPatchSaleOrderNew", "Add");
            BindSaleOrderDetailList(_emptyGuid, false, false);
            BindSaleOrderOtherChargesDetailList(_emptyGuid, false);
        }
        else {
            console.log("Error: " + xhr.status + ": " + xhr.statusText);
        }
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
    $("#divSaleOrderForm").load("SaleOrder/SaleOrderForm?id=" + SaleOrder.ID , function (responseTxt, statusTxt, xhr) {
        if (statusTxt == "success") {
            OnServerCallComplete();
            openNav();
            $('#lblSaleOrderInfo').text(SaleOrder.SaleOrderNo);
            //$('#CustomerID').trigger('change');
            if ($('#IsDocLocked').val() == "True") {
                debugger;
                switch ($('#LatestApprovalStatus').val()) {
                    case "0":
                        ChangeButtonPatchView("SaleOrder", "btnPatchSaleOrderNew", "Draft", SaleOrder.ID);
                        break;
                    case "1":
                        ChangeButtonPatchView("SaleOrder", "btnPatchSaleOrderNew", "Approved", SaleOrder.ID);
                        break;
                    case "3":
                        ChangeButtonPatchView("SaleOrder", "btnPatchSaleOrderNew", "Edit", SaleOrder.ID);
                        break;
                    case "4":
                        ChangeButtonPatchView("SaleOrder", "btnPatchSaleOrderNew", "Approved", SaleOrder.ID);
                        break;
                    default:
                        ChangeButtonPatchView("SaleOrder", "btnPatchSaleOrderNew", "LockDocument", SaleOrder.ID);
                        break;
                }
            }
            else {
                ChangeButtonPatchView("SaleOrder", "btnPatchSaleOrderNew", "LockDocument", SaleOrder.ID);
            }
            BindSaleOrderDetailList(SaleOrder.ID, false, false);
            BindSaleOrderOtherChargesDetailList(SaleOrder.ID, false);
            CalculateTotal();
            $('#divCustomerBasicInfo').load("Customer/CustomerBasicInfo?ID=" + $('#hdnCustomerID').val(), function () {
            });
            clearUploadControl();
            PaintImages(SaleOrder.ID);
        }
        else {
            console.log("Error: " + xhr.status + ": " + xhr.statusText);
        }
    });
}
function ResetSaleOrder() {
    $("#divSaleOrderForm").load("SaleOrder/SaleOrderForm?id=" + $('#SaleOrderForm #ID').val() , function (responseTxt, statusTxt, xhr) {
        if (statusTxt == "success") {
            if ($('#ID').val() != _emptyGuid && $('#ID').val() != null) {
                    //resides in customjs for sliding
                    openNav();
            }
            debugger;
            switch ($('#LatestApprovalStatus').val()) {
                case "":
                    ChangeButtonPatchView("SaleOrder", "btnPatchSaleOrderNew", "Add");
                    break;
                case "0":
                    ChangeButtonPatchView("SaleOrder", "btnPatchSaleOrderNew", "Draft", $('#ID').val());
                    break;
                case "1":
                    ChangeButtonPatchView("SaleOrder", "btnPatchSaleOrderNew", "Approved", $('#ID').val());
                    break;
                case "3":
                    ChangeButtonPatchView("SaleOrder", "btnPatchSaleOrderNew", "Edit", $('#ID').val());
                    break;
                case "4":
                    ChangeButtonPatchView("SaleOrder", "btnPatchSaleOrderNew", "Approved", $('#ID').val());
                    break;
                default:
                    ChangeButtonPatchView("SaleOrder", "btnPatchSaleOrderNew", "LockDocument", $('#ID').val());
                    break;
            }
            //if ($('#LatestApprovalStatus').val() == "") {
            //    ChangeButtonPatchView("SaleOrder", "btnPatchSaleOrderNew", "Add");
            //}
            //else if ($('#LatestApprovalStatus').val() == 3 || $('#LatestApprovalStatus').val() == 0) {
            //    ChangeButtonPatchView("SaleOrder", "btnPatchSaleOrderNew", "Edit", $('#ID').val());
            //}
            //else if ($('#LatestApprovalStatus').val() == 4) {
            //    ChangeButtonPatchView("SaleOrder", "btnPatchSaleOrderNew", "Approved", $('#ID').val());
            //}
            //else {
            //    ChangeButtonPatchView("SaleOrder", "btnPatchSaleOrderNew", "LockDocument", $('#ID').val());
            //}
            BindSaleOrderDetailList($('#ID').val(), false, false);
            BindSaleOrderOtherChargesDetailList($('#ID').val(), false);
            CalculateTotal();
            clearUploadControl();
            PaintImages($('#SaleOrderForm #ID').val());
            $('#divCustomerBasicInfo').load("Customer/CustomerBasicInfo?ID=" + $('#SaleOrderForm #hdnCustomerID').val(), function () {
            });
        }
        else {
            console.log("Error: " + xhr.status + ": " + xhr.statusText);
        }
    });
}
function SaveSaleOrder() {
    var saleOrderDetailList = _dataTable.SaleOrderDetailList.rows().data().toArray();
    $('#DetailJSON').val(JSON.stringify(saleOrderDetailList));
    var otherChargesDetailList = _dataTable.SaleOrderOtherChargesDetailList.rows().data().toArray();
    $('#OtherChargesDetailJSON').val(JSON.stringify(otherChargesDetailList));
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
                    ChangeButtonPatchView("SaleOrder", "btnPatchSaleOrderNew", "Edit",_result.ID);
                    BindSaleOrderDetailList(_result.ID, false, false);
                    BindSaleOrderOtherChargesDetailList(_result.ID, false);
                    CalculateTotal();
                    clearUploadControl();
                    PaintImages(_result.ID);
                    $('#lblSaleOrderInfo').text(_result.SaleOrderNo);
                    $('#divCustomerBasicInfo').load("Customer/CustomerBasicInfo?ID=" + $('#SaleOrderForm #hdnCustomerID').val());
                });
                ChangeButtonPatchView("SaleOrder", "btnPatchSaleOrderNew", "Edit",_result.ID);
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
                    $('#lblSaleOrderInfo').text('<<Sale Order No.>>');
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
function BindSaleOrderOtherChargesDetailList(id, IsQuotation) {
    debugger;
    var data;
    if (id == _emptyGuid && !(IsQuotation)) {
        data = null;
    }
    else if (id == _emptyGuid && (IsQuotation)) {
        data = GetSaleOrderOtherChargesDetailListBySaleOrderID(id, IsQuotation)
    }
    else {
        data = GetSaleOrderOtherChargesDetailListBySaleOrderID(id, IsQuotation)
    }
    _dataTable.SaleOrderOtherChargesDetailList = $('#tblSaleOrderOtherChargesDetailList').DataTable(
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
             { "data": "ChargeAmount", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
             {
                 "data": "ChargeAmount", render: function (data, type, row) {
                     debugger;
                     var CGST = parseFloat(row.CGSTPerc != "" ? row.CGSTPerc : 0);
                     var SGST = parseFloat(row.SGSTPerc != "" ? row.SGSTPerc : 0);
                     var IGST = parseFloat(row.IGSTPerc != "" ? row.IGSTPerc : 0);
                     var CGSTAmt = parseFloat(data * CGST / 100);
                     var SGSTAmt = parseFloat(data * SGST / 100)
                     var IGSTAmt = parseFloat(data * IGST / 100)
                     var GSTAmt = roundoff(parseFloat(CGSTAmt) + parseFloat(SGSTAmt) + parseFloat(IGSTAmt))
                     return '<div class="show-popover text-right" data-html="true" data-toggle="popover" data-placement="left" data-title="<p align=left>Total GST : ₹ ' + GSTAmt + '" data-content=" SGST ' + SGST + '% : ₹ ' + roundoff(parseFloat(SGSTAmt)) + '<br/>CGST ' + CGST + '% : ₹ ' + roundoff(parseFloat(CGSTAmt)) + '<br/> IGST ' + IGST + '% : ₹ ' + roundoff(parseFloat(IGSTAmt)) + '</p>"/>' + GSTAmt
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
                     //var TaxAmt = parseFloat(data) + parseFloat(GSTAmt)
                     var AddlTax = parseFloat(GSTAmt * row.AddlTaxPerc / 100);
                     return '<div class="show-popover text-right" data-html="true" data-toggle="popover" data-placement="left" data-title="<p align=left>Additional Tax : % ' + row.AddlTaxPerc + '</p>"/>' + roundoff(AddlTax)
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
                     var TaxAmt = parseFloat(data) + parseFloat(GSTAmt)
                     var AddlTax = parseFloat(GSTAmt * row.AddlTaxPerc / 100);
                     var Total = roundoff(parseFloat(data) + parseFloat(GSTAmt) + parseFloat(AddlTax))
                     //return Total
                     return '<div class="show-popover text-right" data-html="true" data-toggle="popover" data-placement="left" data-title="<p align=left>Total : ₹ ' + Total + '" data-content="Charge Amount : ₹ ' + data + '<br/>GST : ₹ ' + GSTAmt + '<br/>Additional Tax : ₹ ' + row.AddlTaxAmt + '</p>"/>' + Total
                 }, "defaultContent": "<i></i>"
             },
             { "data": null, "orderable": false, "defaultContent": ($('#IsDocLocked').val() == "True" || $('#IsUpdate').val() == "False") ? '<a href="#" class="actionLink"  onclick="EditSaleOrderOtherChargesDetail(this)" ><i class="fa fa-pencil-square-o" aria-hidden="true"></i></a> <a href="#" class="DeleteLink"  onclick="ConfirmDeleteSaleOrderOtherChargeDetail(this)" ><i class="fa fa-trash-o" aria-hidden="true"></i></a>' : "-" },
             ],
             columnDefs: [
                 { "targets": [0], "width": "30%" },
                 { "targets": [1, 2, 3,4], "width": "15%" },
                 { "targets": [5], "width": "10%" },
                 { className: "text-left", "targets": [0] },
                 { className: "text-right", "targets": [1, 2, 3,4] },
                 { className: "text-center", "targets": [5] }
             ],
             destroy: true,
         });
    $('[data-toggle="popover"]').popover({
        html: true,
        'trigger': 'hover',
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
                     return '<div style="width:100%" class="show-popover" data-html="true" data-placement="top" data-toggle="popover" data-title="<p align=left>Product Specification" data-content="' + row.ProductSpec.replace(/"/g, "&quot") + '</p>"/>' + row.Product.Name + "<br/>" + row.ProductModel.Name
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
                     return '<div class="show-popover text-right" data-html="true" data-toggle="popover" data-placement="left" data-title="<p align=left>Taxable : ₹ ' + Taxable + '" data-content="Net Total : ₹ ' + Total + '<br/> Discount : ₹ -' + Discount + '</p>"/>' + Taxable
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
                     return '<div class="show-popover text-right" data-html="true" data-toggle="popover" data-placement="left" data-title="<p align=left>Total GST : ₹ ' + GSTAmt + '" data-content=" SGST ' + SGST + '% : ₹ ' + roundoff(SGSTAmt) + '<br/>CGST ' + CGST + '% : ₹ ' + roundoff(parseFloat(CGSTAmt)) + '<br/> IGST ' + IGST + '% : ₹ ' + roundoff(parseFloat(IGSTAmt)) + '</p>"/>' + GSTAmt
                 }, "defaultContent": "<i></i>"
             },
             {
                 "data": "CessAmt", render: function (data, type, row) {
                     debugger;
                     var CGST = parseFloat(row.CGSTPerc != "" ? row.CGSTPerc : 0);
                     var SGST = parseFloat(row.SGSTPerc != "" ? row.SGSTPerc : 0);
                     var IGST = parseFloat(row.IGSTPerc != "" ? row.IGSTPerc : 0);
                     var Total = roundoff(parseFloat(row.Rate != "" ? row.Rate : 0) * parseInt(row.Qty != "" ? row.Qty : 1))
                     var Discount = roundoff(parseFloat(row.Discount != "" ? row.Discount : 0))
                     var Taxable = Total - Discount
                     var CGSTAmt = parseFloat(Taxable * CGST / 100);
                     var SGSTAmt = parseFloat(Taxable * SGST / 100)
                     var IGSTAmt = parseFloat(Taxable * IGST / 100)
                     var GSTAmt = CGSTAmt + SGSTAmt + IGSTAmt;
                     //var TaxAmount = Taxable + parseFloat(GSTAmt);
                     var Cess = roundoff(parseFloat(GSTAmt * row.CessPerc / 100));
                     return '<i style="font-size:10px;color:brown">Cess(%) -</i>' + row.CessPerc + '<br/><i style="font-size:10px;color:brown">Cess(₹) -</i>' + Cess
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
                     var TaxAmount = TaxableAmt + parseFloat(GSTAmt);
                     var Cess = roundoff(parseFloat(GSTAmt * row.CessPerc / 100));
                     var GrandTotal = roundoff(((parseFloat(row.Rate != "" ? row.Rate : 0) * parseInt(row.Qty != "" ? row.Qty : 1)) - parseFloat(row.Discount != "" ? row.Discount : 0)) + parseFloat(GSTAmt) + parseFloat(Cess))
                     return '<div class="show-popover text-right" data-html="true" data-toggle="popover" data-placement="left" data-title="<p align=left>Grand Total : ₹ ' + GrandTotal + '" data-content="Taxable : ₹ ' + TaxableAmt + '<br/>GST : ₹ ' + GSTAmt + '<br/>Cess : ₹ ' + row.CessAmt + '</p>"/>' + GrandTotal
                 }, "defaultContent": "<i></i>"
             },
             { "data": null, "orderable": false, "defaultContent": ($('#IsDocLocked').val() == "True" || $('#IsUpdate').val() == "False") ? '<a href="#" class="actionLink"  onclick="EditSaleOrderDetail(this)" ><i class="fa fa-pencil-square-o" aria-hidden="true"></i></a> <a href="#" class="DeleteLink"  onclick="ConfirmDeleteSaleOrderDetail(this)" ><i class="fa fa-trash-o" aria-hidden="true"></i></a>' : "-" },
             ],
             columnDefs: [
                 { "targets": [0], "width": "35%" },
                 { "targets": [2, 4, 5, 6, 7], "width": "10%" },
                 { "targets": [1, 3, 8], "width": "5%" },
                 { className: "text-right", "targets": [2, 3, 4, 5, 6, 7] },
                 { className: "text-left", "targets": [0] },
                 { className: "text-center", "targets": [1, 8] }
             ],
             //rowCallback: function (row, data, index) {
             //    debugger;
             //    var TaxableAmt = (parseFloat(data.Rate != "" ? data.Rate : 0) * parseInt(data.Qty != "" ? data.Qty : 1)) - parseFloat(data.Discount != "" ? data.Discount : 0)
             //    var CGST = parseFloat(data.CGSTPerc != "" ? data.CGSTPerc : 0);
             //    var SGST = parseFloat(data.SGSTPerc != "" ? data.SGSTPerc : 0);
             //    var IGST = parseFloat(data.IGSTPerc != "" ? data.IGSTPerc : 0);
             //    var CGSTAmt = parseFloat(TaxableAmt * CGST / 100);
             //    var SGSTAmt = parseFloat(TaxableAmt * SGST / 100);
             //    var IGSTAmt = parseFloat(TaxableAmt * IGST / 100);
             //    var GSTAmt = parseFloat(CGSTAmt) + parseFloat(SGSTAmt) + parseFloat(IGSTAmt)
             //    var GrossTotalAmt = TaxableAmt + GSTAmt + parseFloat(data.CessAmt)
             //    var CessAmt = roundoff(parseFloat($('#lblCessAmount').text()) + parseFloat(data.CessAmt))
             //    var TaxTotal = roundoff(parseFloat($('#lblTaxTotal').text()) + GSTAmt)
             //    var TaxableTotal = roundoff(parseFloat($('#lblItemTotal').text()) + TaxableAmt)
             //    var GrossAmount = roundoff(parseFloat($('#lblGrossAmount').text()) + GrossTotalAmt)
             //    var GrandTotal = roundoff(parseFloat($('#lblGrandTotal').text()) + GrossTotalAmt)

             //    $('#lblTaxTotal').text(TaxTotal);
             //    $('#lblItemTotal').text(TaxableTotal);
             //    $('#lblGrossAmount').text(GrossAmount);
             //    $('#lblCessAmount').text(CessAmt);
             //    $('#lblGrandTotal').text(GrandTotal);
             //},
             initComplete: function (settings, json) {
                 $('#SaleOrderForm #Discount').trigger('change');
             },
             destroy: true
         });
    $('[data-toggle="popover"]').popover({
        html: true,
        'trigger': 'hover'
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
        if (($('#divModelSaleOrderPopBody #ProductID').val() != "") && ($('#divModelSaleOrderPopBody #ProductModelID').val() != "") && ($('#divModelSaleOrderPopBody #Rate').val() != "") && ($('#divModelSaleOrderPopBody #Qty').val() != "") && ($('#divModelSaleOrderPopBody #UnitCode').val() != "")) {
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
            saleOrderDetailList[_datatablerowindex].TaxTypeCode =$('#divModelSaleOrderPopBody #TaxTypeCode').val()!=null? $('#divModelSaleOrderPopBody #TaxTypeCode').val().split('|')[0]:"";
            TaxType.ValueText = $('#divModelSaleOrderPopBody #TaxTypeCode').val();
            saleOrderDetailList[_datatablerowindex].TaxType = TaxType;
            saleOrderDetailList[_datatablerowindex].CGSTPerc = $('#divModelSaleOrderPopBody #hdnCGSTPerc').val();
            saleOrderDetailList[_datatablerowindex].SGSTPerc = $('#divModelSaleOrderPopBody #hdnSGSTPerc').val();
            saleOrderDetailList[_datatablerowindex].IGSTPerc = $('#divModelSaleOrderPopBody #hdnIGSTPerc').val();
            saleOrderDetailList[_datatablerowindex].CessPerc = $('#divModelSaleOrderPopBody #CessPerc').val() != "" ? $('#divModelSaleOrderPopBody #CessPerc').val() : 0;
            saleOrderDetailList[_datatablerowindex].CessAmt = $('#divModelSaleOrderPopBody #CessAmt').val();
            ClearCalculatedFields();
            _dataTable.SaleOrderDetailList.clear().rows.add(saleOrderDetailList).draw(false);
            CalculateTotal();
            $('#divModelPopSaleOrder').modal('hide');
            _datatablerowindex = -1;
        }
    }
    else {
        if (($('#divModelSaleOrderPopBody #ProductID').val() != "") && ($('#divModelSaleOrderPopBody #ProductModelID').val() != "") && ($('#divModelSaleOrderPopBody #Rate').val() != "") && ($('#divModelSaleOrderPopBody #Qty').val() != "") && ($('#divModelSaleOrderPopBody #UnitCode').val() != "")) {
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
                saleOrderDetailList[0].TaxTypeCode =$('#divModelSaleOrderPopBody #TaxTypeCode').val()!=null? $('#divModelSaleOrderPopBody #TaxTypeCode').val().split('|')[0]:"";
                saleOrderDetailList[0].TaxType.ValueText = $('#divModelSaleOrderPopBody #TaxTypeCode').val();
                saleOrderDetailList[0].CGSTPerc = $('#divModelSaleOrderPopBody #hdnCGSTPerc').val();
                saleOrderDetailList[0].SGSTPerc = $('#divModelSaleOrderPopBody #hdnSGSTPerc').val();
                saleOrderDetailList[0].IGSTPerc = $('#divModelSaleOrderPopBody #hdnIGSTPerc').val();
                saleOrderDetailList[0].CessPerc = $('#divModelSaleOrderPopBody #CessPerc').val() != "" ? $('#divModelSaleOrderPopBody #CessPerc').val() : 0;
                saleOrderDetailList[0].CessAmt = $('#divModelSaleOrderPopBody #CessAmt').val();
                ClearCalculatedFields();
                _dataTable.SaleOrderDetailList.clear().rows.add(saleOrderDetailList).draw(false);
                CalculateTotal();
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
                        CalculateTotal();
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
                        SaleOrderDetailVM.TaxTypeCode =$('#divModelSaleOrderPopBody #TaxTypeCode').val()!=null? $('#divModelSaleOrderPopBody #TaxTypeCode').val().split('|')[0]:"";
                        var TaxType = new Object();
                        TaxType.ValueText = $('#divModelSaleOrderPopBody #TaxTypeCode').val();
                        SaleOrderDetailVM.TaxType = TaxType;
                        SaleOrderDetailVM.CGSTPerc = $('#divModelSaleOrderPopBody #hdnCGSTPerc').val();
                        SaleOrderDetailVM.SGSTPerc = $('#divModelSaleOrderPopBody #hdnSGSTPerc').val();
                        SaleOrderDetailVM.IGSTPerc = $('#divModelSaleOrderPopBody #hdnIGSTPerc').val();
                        SaleOrderDetailVM.CessPerc = $('#divModelSaleOrderPopBody #CessPerc').val() != "" ? $('#divModelSaleOrderPopBody #CessPerc').val() : 0;
                        SaleOrderDetailVM.CessAmt = $('#divModelSaleOrderPopBody #CessAmt').val();
                        _dataTable.SaleOrderDetailList.row.add(SaleOrderDetailVM).draw(true);
                        CalculateTotal();
                        $('#divModelPopSaleOrder').modal('hide');
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
        if (saleOrderDetail.TaxType.Code != 0) {
            $('#FormSaleOrderDetail #TaxTypeCode').val(saleOrderDetail.TaxType.ValueText);
            $('#FormSaleOrderDetail #hdnTaxTypeCode').val(saleOrderDetail.TaxType.ValueText);
        }
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
        notyConfirm('Are you sure to delete?', 'DeleteCurrentSaleOrderDetail("' + _datatablerowindex + '")');
    }
    else {
        notyConfirm('Are you sure to delete?', 'DeleteSaleOrderDetail("' + saleOrderDetail.ID + '")');

    }
}
function DeleteCurrentSaleOrderDetail(_datatablerowindex) {
    var saleOrderDetailList = _dataTable.SaleOrderDetailList.rows().data();
    saleOrderDetailList.splice(_datatablerowindex, 1);
    ClearCalculatedFields();
    _dataTable.SaleOrderDetailList.clear().rows.add(saleOrderDetailList).draw(false);
    CalculateTotal();
    notyAlert('success', 'Detail Row deleted successfully');
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
            CalculateTotal();
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
    debugger;
    $("#divModelEmailSaleOrderBody").load("SaleOrder/EmailSaleOrder?ID=" + $('#SaleOrderForm #ID').val() + "&EmailFlag=True", function () {
        $('#lblModelEmailSaleOrder').text('Email Attachment')
        $('#divModelEmailSaleOrder').modal('show');
    });
}
function SendSaleOrderEmail() {
    $('#hdnMailBodyHeader').val($('#MailBodyHeader').val());
    $('#hdnMailBodyFooter').val($('#MailBodyFooter').val());
    $('#hdnSaleOrderEMailContent').val($('#divSaleOrderEmailcontainer').html());
    $('#hdnSaleOrderNo').val($('#SaleOrderNo').val());
    $('#hdnContactPerson').val($('#ContactPerson').text());
    $('#hdnSaleOrderDateFormatted').val($('#SaleOrderDateFormatted').val());
    $('#FormSaleOrderEmailSend #ID').val($('#SaleOrderForm #ID').val());
}
function UpdateSaleOrderEmailInfo() {
    $('#hdnMailBodyHeader').val($('#MailBodyHeader').val());
    $('#hdnMailBodyFooter').val($('#MailBodyFooter').val());
    $('#FormUpdateSaleOrderEmailInfo #ID').val($('#SaleOrderForm #ID').val());
}
function DownloadSaleOrder() {
    debugger;
    var bodyContent = $('#divSaleOrderEmailcontainer').html();
    var headerContent = $('#hdnHeadContent').html();
    $('#hdnContent').val(bodyContent);
    $('#hdnHeadContent').val(headerContent);
    var customerName = $("#SaleOrderForm #CustomerID option:selected").text();
    $('#hdnCustomerName').val(customerName);
}
function PrintSaleOrder() {
    debugger;
    $("#divModelPrintSaleOrderBody").load("SaleOrder/PrintSaleOrder?ID=" + $('#SaleOrderForm #ID').val(), function () {
        $('#lblModelPrintSaleOrder').text('Print SaleOrder');
        $('#divModelPrintSaleOrder').modal('show');
    });
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
                    $('#lblModelEmailSaleOrder').text('Email Attachment')
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
                ResetSaleOrder();
                break;
            case "ERROR":
                MasterAlert("danger", _message)
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
    $("#SendApprovalModalBody").load("DocumentApproval/GetApprovers?documentTypeCode=SOD", function () {
        debugger;
        if ($('#LatestApprovalStatus').val() == 3) {
            var documentID = $('#SaleOrderForm #ID').val();
            var latestApprovalID = $('#LatestApprovalID').val();
            ReSendDocForApproval(documentID, documentTypeCode, latestApprovalID);
            //SendForApproval('QUO')
            //BindPurchaseOrder($('#ID').val());
            ResetSaleOrder();
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
    ResetSaleOrder();
    //BindPurchaseOrder($('#ID').val());

}
function ClearCalculatedFields()
{
    $('#lblTaxTotal').text('0.00');
    $('#lblItemTotal').text('0.00');
    $('#lblGrossAmount').text('0.00');
    $('#lblCessAmount').text('0.00');
    $('#lblGrandTotal').text('0.00');
    $('#lblOtherChargeAmount').text('0.00');
}
//OtherExpense------------------
function AddOtherExpenseDetailList() {
    $("#divModelSaleOrderPopBody").load("SaleOrder/SaleOrderOtherChargeDetail", function () {
        $('#lblModelPopSaleOrder').text('OtherExpense Detail')
        $('#divModelPopSaleOrder').modal('show');
    });
}
function AddOtherExpenseDetailToList() {
    debugger;
    $("#FormOtherExpenseDetail").submit(function () { });
    debugger;
    if ($('#FormOtherExpenseDetail #IsUpdate').val() == 'True') {
        if (($('#divModelSaleOrderPopBody #OtherChargeCode').val() != "") && ($('#divModelSaleOrderPopBody #ChargeAmount').val() != "")) {
            debugger;
            var saleOrderOtherExpenseDetailList = _dataTable.SaleOrderOtherChargesDetailList.rows().data();
            saleOrderOtherExpenseDetailList[_datatablerowindex].OtherCharge.Description = $("#divModelSaleOrderPopBody #OtherChargeCode").val() != "" ? $("#divModelSaleOrderPopBody #OtherChargeCode option:selected").text().split("-")[0].trim() : "";
            saleOrderOtherExpenseDetailList[_datatablerowindex].ChargeAmount = $("#divModelSaleOrderPopBody #ChargeAmount").val();
            saleOrderOtherExpenseDetailList[_datatablerowindex].OtherChargeCode = $("#divModelSaleOrderPopBody #OtherChargeCode").val() != "" ? $("#divModelSaleOrderPopBody #OtherChargeCode").val() : _emptyGuid;
            TaxType = new Object;
            if ($('#divModelSaleOrderPopBody #TaxTypeCode').val() != null) {
                saleOrderOtherExpenseDetailList[_datatablerowindex].TaxTypeCode = $('#divModelSaleOrderPopBody #TaxTypeCode').val().split('|')[0];
                TaxType.ValueText = $('#divModelSaleOrderPopBody #TaxTypeCode').val();
            }
            saleOrderOtherExpenseDetailList[_datatablerowindex].TaxType = TaxType;
            saleOrderOtherExpenseDetailList[_datatablerowindex].CGSTPerc = $('#divModelSaleOrderPopBody #hdnCGSTPerc').val();
            saleOrderOtherExpenseDetailList[_datatablerowindex].SGSTPerc = $('#divModelSaleOrderPopBody #hdnSGSTPerc').val();
            saleOrderOtherExpenseDetailList[_datatablerowindex].IGSTPerc = $('#divModelSaleOrderPopBody #hdnIGSTPerc').val();
            saleOrderOtherExpenseDetailList[_datatablerowindex].AddlTaxPerc = $('#divModelSaleOrderPopBody #AddlTaxPerc').val() != "" ? $('#divModelSaleOrderPopBody #AddlTaxPerc').val() : 0;
            saleOrderOtherExpenseDetailList[_datatablerowindex].AddlTaxAmt = $('#divModelSaleOrderPopBody #AddlTaxAmt').val() != "" ? $('#divModelSaleOrderPopBody #AddlTaxAmt').val() : 0.00;
            ClearCalculatedFields();
            _dataTable.SaleOrderOtherChargesDetailList.clear().rows.add(saleOrderOtherExpenseDetailList).draw(false);
            CalculateTotal();
            $('#divModelPopSaleOrder').modal('hide');
            _datatablerowindex = -1;
        }
    }
    else {
        if (($('#divModelSaleOrderPopBody #OtherChargeCode').val() != "") && ($('#divModelSaleOrderPopBody #ChargeAmount').val() != "")) {
            debugger;
            if (_dataTable.SaleOrderOtherChargesDetailList.rows().data().length === 0) {
                _dataTable.SaleOrderOtherChargesDetailList.clear().rows.add(GetSaleOrderOtherChargesDetailListBySaleOrderID(_emptyGuid, false)).draw(false);
                debugger;
                var saleOrderOtherExpenseDetailList = _dataTable.SaleOrderOtherChargesDetailList.rows().data();
                saleOrderOtherExpenseDetailList[0].OtherCharge.Description = $("#divModelSaleOrderPopBody #OtherChargeCode").val() != "" ? $("#divModelSaleOrderPopBody #OtherChargeCode option:selected").text().split("-")[0].trim() : "";
                saleOrderOtherExpenseDetailList[0].OtherChargeCode = $("#divModelSaleOrderPopBody #OtherChargeCode").val() != "" ? $("#divModelSaleOrderPopBody #OtherChargeCode").val() : _emptyGuid;
                saleOrderOtherExpenseDetailList[0].ChargeAmount = $("#divModelSaleOrderPopBody #ChargeAmount").val();
                if ($('#divModelSaleOrderPopBody #TaxTypeCode').val() != null) {
                    saleOrderOtherExpenseDetailList[0].TaxTypeCode = $('#divModelSaleOrderPopBody #TaxTypeCode').val().split('|')[0];
                }
                saleOrderOtherExpenseDetailList[0].TaxType.ValueText = $('#divModelSaleOrderPopBody #TaxTypeCode').val();
                saleOrderOtherExpenseDetailList[0].CGSTPerc = $('#divModelSaleOrderPopBody #hdnCGSTPerc').val();
                saleOrderOtherExpenseDetailList[0].SGSTPerc = $('#divModelSaleOrderPopBody #hdnSGSTPerc').val();
                saleOrderOtherExpenseDetailList[0].IGSTPerc = $('#divModelSaleOrderPopBody #hdnIGSTPerc').val();
                saleOrderOtherExpenseDetailList[0].AddlTaxPerc = $('#divModelSaleOrderPopBody #AddlTaxPerc').val() != "" ? $('#divModelSaleOrderPopBody #AddlTaxPerc').val() : 0;
                saleOrderOtherExpenseDetailList[0].AddlTaxAmt = $('#divModelSaleOrderPopBody #AddlTaxAmt').val() != "" ? $('#divModelSaleOrderPopBody #AddlTaxAmt').val() : 0.00;
                ClearCalculatedFields();
                _dataTable.SaleOrderOtherChargesDetailList.clear().rows.add(saleOrderOtherExpenseDetailList).draw(false);
                CalculateTotal();
                $('#divModelPopSaleOrder').modal('hide');
            }
            else {
                debugger;
                var saleOrderOtherExpenseDetailList = _dataTable.SaleOrderOtherChargesDetailList.rows().data();
                if (saleOrderOtherExpenseDetailList.length > 0) {
                    var checkpoint = 0;
                    var otherCharge = $('#OtherChargeCode').val();
                    for (var i = 0; i < saleOrderOtherExpenseDetailList.length; i++) {
                        if ((saleOrderOtherExpenseDetailList[i].OtherChargeCode == otherCharge)) {
                            saleOrderOtherExpenseDetailList[i].ChargeAmount = parseFloat(saleOrderOtherExpenseDetailList[i].ChargeAmount) + parseFloat($('#ChargeAmount').val());
                            checkpoint = 1;
                            break;
                        }
                    }
                    if (checkpoint == 1) {
                        debugger;
                        ClearCalculatedFields();
                        _dataTable.SaleOrderOtherChargesDetailList.clear().rows.add(saleOrderOtherExpenseDetailList).draw(false);
                        CalculateTotal();
                        $('#divModelPopSaleOrder').modal('hide');
                    }
                    else if (checkpoint == 0) {
                        ClearCalculatedFields();
                        var SaleOrderOtherChargesDetailVM = new Object();
                        SaleOrderOtherChargesDetailVM.ID = _emptyGuid;
                        var OtherCharge = new Object;
                        OtherCharge.Description = $("#divModelSaleOrderPopBody #OtherChargeCode").val() != "" ? $("#divModelSaleOrderPopBody #OtherChargeCode option:selected").text().split("-")[0].trim() : "";
                        SaleOrderOtherChargesDetailVM.OtherCharge = OtherCharge;
                        SaleOrderOtherChargesDetailVM.OtherChargeCode = $("#divModelSaleOrderPopBody #OtherChargeCode").val() != "" ? $("#divModelSaleOrderPopBody #OtherChargeCode").val() : _emptyGuid;
                        SaleOrderOtherChargesDetailVM.ChargeAmount = $("#divModelSaleOrderPopBody #ChargeAmount").val();
                        var TaxType = new Object();
                        if ($('#divModelSaleOrderPopBody #TaxTypeCode').val() != null) {
                            SaleOrderOtherChargesDetailVM.TaxTypeCode = $('#divModelSaleOrderPopBody #TaxTypeCode').val().split('|')[0];
                            TaxType.ValueText = $('#divModelSaleOrderPopBody #TaxTypeCode').val();
                        }
                        SaleOrderOtherChargesDetailVM.TaxType = TaxType;
                        SaleOrderOtherChargesDetailVM.CGSTPerc = $('#divModelSaleOrderPopBody #hdnCGSTPerc').val();
                        SaleOrderOtherChargesDetailVM.SGSTPerc = $('#divModelSaleOrderPopBody #hdnSGSTPerc').val();
                        SaleOrderOtherChargesDetailVM.IGSTPerc = $('#divModelSaleOrderPopBody #hdnIGSTPerc').val();
                        SaleOrderOtherChargesDetailVM.AddlTaxPerc = $('#divModelSaleOrderPopBody #AddlTaxPerc').val() != "" ? $('#divModelSaleOrderPopBody #AddlTaxPerc').val() : 0.00;
                        SaleOrderOtherChargesDetailVM.AddlTaxAmt = $('#divModelSaleOrderPopBody #AddlTaxAmt').val() != "" ? $('#divModelSaleOrderPopBody #AddlTaxAmt').val() : 0.00;
                        _dataTable.SaleOrderOtherChargesDetailList.row.add(SaleOrderOtherChargesDetailVM).draw(true);
                        CalculateTotal();
                        $('#divModelPopSaleOrder').modal('hide');
                    }
                }
            }
        }
    }
    $('[data-toggle="popover"]').popover({
        html: true,
        'trigger': 'hover',
    });
}
function GetSaleOrderOtherChargesDetailListBySaleOrderID(id, IsQuotation) {
    try {
        debugger;

        var SaleOrderOtherChargesDetailList = [];
        if (IsQuotation) {
            var data = { "quoteID": $('#SaleOrderForm #hdnQuoteID').val() };
            _jsonData = GetDataFromServer("SaleOrder/GetSaleOrderOtherChargesDetailListByQuotationIDFromQuotation/", data);
        }
        else {
            var data = { "saleOrderID": id };
            _jsonData = GetDataFromServer("SaleOrder/GetSaleOrderOtherChargesDetailListBySaleOrderID/", data);
        }

        if (_jsonData != '') {
            _jsonData = JSON.parse(_jsonData);
            _message = _jsonData.Message;
            _status = _jsonData.Status;
            SaleOrderOtherChargesDetailList = _jsonData.Records;
        }
        if (_status == "OK") {
            return SaleOrderOtherChargesDetailList;
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
function EditSaleOrderOtherChargesDetail(this_Obj) {
    debugger;
    _datatablerowindex = _dataTable.SaleOrderOtherChargesDetailList.row($(this_Obj).parents('tr')).index();
    var saleOrderOtherChargesDetail = _dataTable.SaleOrderOtherChargesDetailList.row($(this_Obj).parents('tr')).data();
    $("#divModelSaleOrderPopBody").load("SaleOrder/SaleOrderOtherChargeDetail", function () {
        debugger;
        $('#lblModelPopQuotation').text('OtherCharges Detail')
        $('#FormOtherExpenseDetail #IsUpdate').val('True');
        $('#FormOtherExpenseDetail #ID').val(saleOrderOtherChargesDetail.ID);
        $("#FormOtherExpenseDetail #OtherChargeCode").val(saleOrderOtherChargesDetail.OtherChargeCode);
        $("#FormOtherExpenseDetail #hdnOtherChargeCode").val(saleOrderOtherChargesDetail.OtherChargeCode);
        $("#FormOtherExpenseDetail #ChargeAmount").val(saleOrderOtherChargesDetail.ChargeAmount);
        if (saleOrderOtherChargesDetail.TaxType.Code != 0) {
            $('#FormOtherExpenseDetail #TaxTypeCode').val(saleOrderOtherChargesDetail.TaxType.ValueText);
            $('#FormOtherExpenseDetail #hdnTaxTypeCode').val(saleOrderOtherChargesDetail.TaxType.ValueText);
        }
        $('#FormOtherExpenseDetail #hdnCGSTPerc').val(saleOrderOtherChargesDetail.CGSTPerc);
        $('#FormOtherExpenseDetail #hdnSGSTPerc').val(saleOrderOtherChargesDetail.SGSTPerc);
        $('#FormOtherExpenseDetail #hdnIGSTPerc').val(saleOrderOtherChargesDetail.IGSTPerc);
        $('#FormOtherExpenseDetail #hdnAddlTaxPerc').val(saleOrderOtherChargesDetail.AddlTaxPerc);
        $('#FormOtherExpenseDetail #hdnAddlTaxAmt').val(saleOrderOtherChargesDetail.AddlTaxAmt);
        $('#FormOtherExpenseDetail #AddlTaxPerc').val(saleOrderOtherChargesDetail.AddlTaxPerc);
        $('#FormOtherExpenseDetail #AddlTaxAmt').val(saleOrderOtherChargesDetail.AddlTaxAmt);

        var CGSTAmt = (saleOrderOtherChargesDetail.ChargeAmount * parseFloat(saleOrderOtherChargesDetail.CGSTPerc)) / 100;
        var SGSTAmt = (saleOrderOtherChargesDetail.ChargeAmount * parseFloat(saleOrderOtherChargesDetail.SGSTPerc)) / 100;
        var IGSTAmt = (saleOrderOtherChargesDetail.ChargeAmount * parseFloat(saleOrderOtherChargesDetail.IGSTPerc)) / 100;
        $('#FormOtherExpenseDetail #CGSTPerc').val(CGSTAmt);
        $('#FormOtherExpenseDetail #SGSTPerc').val(SGSTAmt);
        $('#FormOtherExpenseDetail #IGSTPerc').val(IGSTAmt);
        $('#divModelPopSaleOrder').modal('show');
    });
}
function ConfirmDeleteSaleOrderOtherChargeDetail(this_Obj) {
    debugger;
    _datatablerowindex = _dataTable.SaleOrderOtherChargesDetailList.row($(this_Obj).parents('tr')).index();
    var saleOrderOtherChargeDetail = _dataTable.SaleOrderOtherChargesDetailList.row($(this_Obj).parents('tr')).data();
    if (saleOrderOtherChargeDetail.ID === _emptyGuid) {
        notyConfirm('Are you sure to delete?', 'DeleteCurrentSaleOrderOtherChargeDetail("' + _datatablerowindex + '")');
    }
    else {
        notyConfirm('Are you sure to delete?', 'DeleteSaleOrderOtherChargeDetail("' + saleOrderOtherChargeDetail.ID + '")');

    }
}
function DeleteCurrentSaleOrderOtherChargeDetail(_datatablerowindex) {
    var quotationOtherChargeDetailList = _dataTable.SaleOrderOtherChargesDetailList.rows().data();
    quotationOtherChargeDetailList.splice(_datatablerowindex, 1);
    ClearCalculatedFields();
    _dataTable.SaleOrderOtherChargesDetailList.clear().rows.add(quotationOtherChargeDetailList).draw(false);
    CalculateTotal();
    notyAlert('success', 'Detail Row deleted successfully');
}
function DeleteSaleOrderOtherChargeDetail(ID) {
    if (ID != _emptyGuid && ID != null && ID != '') {
        var data = { "id": ID };
        var ds = {};
        _jsonData = GetDataFromServer("SaleOrder/DeleteSaleOrderOtherChargeDetail/", data);
        if (_jsonData != '') {
            _jsonData = JSON.parse(_jsonData);
            _message = _jsonData.Message;
            _status = _jsonData.Status;
            _result = _jsonData.Record;
        }
        if (_status == "OK") {
            notyAlert('success', _result.Message);
            var saleOrderOtherChargeDetailList = _dataTable.SaleOrderOtherChargesDetailList.rows().data();
            saleOrderOtherChargeDetailList.splice(_datatablerowindex, 1);
            ClearCalculatedFields();
            _dataTable.SaleOrderOtherChargesDetailList.clear().rows.add(saleOrderOtherChargeDetailList).draw(false);
            CalculateTotal();
        }
        if (_status == "ERROR") {
            notyAlert('error', _message);
        }
    }
}
function CalculateTotal() {
    var TaxTotal = 0.00, TaxableTotal = 0.00, GrossAmount = 0.00, GrandTotal = 0.00, OtherChargeAmt = 0.00, CessAmt=0.00;
    var saleOrderDetail = _dataTable.SaleOrderDetailList.rows().data();
    var saleOrderOtherChargeDetail = _dataTable.SaleOrderOtherChargesDetailList.rows().data();
    for (var i = 0; i < saleOrderDetail.length; i++) {
        var TaxableAmt = (parseFloat(saleOrderDetail[i].Rate != "" ? saleOrderDetail[i].Rate : 0) * parseInt(saleOrderDetail[i].Qty != "" ? saleOrderDetail[i].Qty : 1)) - parseFloat(saleOrderDetail[i].Discount != "" ? saleOrderDetail[i].Discount : 0)
        var CGST = parseFloat(saleOrderDetail[i].CGSTPerc != "" ? saleOrderDetail[i].CGSTPerc : 0);
        var SGST = parseFloat(saleOrderDetail[i].SGSTPerc != "" ? saleOrderDetail[i].SGSTPerc : 0);
        var IGST = parseFloat(saleOrderDetail[i].IGSTPerc != "" ? saleOrderDetail[i].IGSTPerc : 0);
        var CGSTAmt = parseFloat(TaxableAmt * CGST / 100);
        var SGSTAmt = parseFloat(TaxableAmt * SGST / 100);
        var IGSTAmt = parseFloat(TaxableAmt * IGST / 100);
        var GSTAmt = parseFloat(CGSTAmt) + parseFloat(SGSTAmt) + parseFloat(IGSTAmt)
        var TaxAmount = TaxableAmt + parseFloat(GSTAmt);
        var Cess = roundoff(parseFloat(GSTAmt * saleOrderDetail[i].CessPerc / 100));
        CessAmt = roundoff(parseFloat(CessAmt) + parseFloat(Cess));
        var GrossTotalAmt = TaxableAmt + GSTAmt
        TaxTotal = roundoff(parseFloat(TaxTotal) + parseFloat(GSTAmt))
        TaxableTotal = roundoff(parseFloat(TaxableTotal) + parseFloat(TaxableAmt))
        GrossAmount = roundoff(parseFloat(GrossAmount) + parseFloat(GrossTotalAmt))
    }
    for (var i = 0; i < saleOrderOtherChargeDetail.length; i++) {
        var CGST = parseFloat(saleOrderOtherChargeDetail[i].CGSTPerc != "" ? saleOrderOtherChargeDetail[i].CGSTPerc : 0);
        var SGST = parseFloat(saleOrderOtherChargeDetail[i].SGSTPerc != "" ? saleOrderOtherChargeDetail[i].SGSTPerc : 0);
        var IGST = parseFloat(saleOrderOtherChargeDetail[i].IGSTPerc != "" ? saleOrderOtherChargeDetail[i].IGSTPerc : 0);
        var CGSTAmt = parseFloat(saleOrderOtherChargeDetail[i].ChargeAmount * CGST / 100);
        var SGSTAmt = parseFloat(saleOrderOtherChargeDetail[i].ChargeAmount * SGST / 100)
        var IGSTAmt = parseFloat(saleOrderOtherChargeDetail[i].ChargeAmount * IGST / 100)
        var GSTAmt = roundoff(parseFloat(CGSTAmt) + parseFloat(SGSTAmt) + parseFloat(IGSTAmt))
        var TaxAmt = parseFloat(saleOrderOtherChargeDetail[i].ChargeAmount) + parseFloat(GSTAmt)
        var AddlTax = parseFloat(GSTAmt * saleOrderOtherChargeDetail[i].AddlTaxPerc / 100);
        var Total = roundoff(parseFloat(saleOrderOtherChargeDetail[i].ChargeAmount) + parseFloat(GSTAmt) + parseFloat(AddlTax))
        OtherChargeAmt = roundoff(parseFloat(OtherChargeAmt) + parseFloat(Total))
    }
    GrossAmount = roundoff(parseFloat(GrossAmount) + parseFloat(OtherChargeAmt) + parseFloat(CessAmt))
    $('#lblTaxTotal').text(roundoff(TaxTotal));
    $('#lblItemTotal').text(roundoff(TaxableTotal));
    $('#lblGrossAmount').text(GrossAmount);
    $('#lblGrandTotal').text(GrossAmount);
    $('#lblOtherChargeAmount').text(roundoff(OtherChargeAmt));
    $('#Discount').trigger('onchange');
    $('#lblCessAmount').text(roundoff(CessAmt));
}
function EditRedirectToDocument(id) {
    debugger;
    OnServerCallBegin();

    //this will return form body(html)
    $("#divSaleOrderForm").load("SaleOrder/SaleOrderForm?id=" + id, function (responseTxt, statusTxt, xhr) {
        if (statusTxt == "success") {
            OnServerCallComplete();
            openNav();
            $('#lblSaleOrderInfo').text($('#SaleOrderNo').val());
            if ($('#IsDocLocked').val() == "True") {
                debugger;
                switch ($('#LatestApprovalStatus').val()) {
                    case "0":
                        ChangeButtonPatchView("SaleOrder", "btnPatchSaleOrderNew", "Draft", id);
                        break;
                    case "1":
                        ChangeButtonPatchView("SaleOrder", "btnPatchSaleOrderNew", "Approved", id);
                        break;
                    case "3":
                        ChangeButtonPatchView("SaleOrder", "btnPatchSaleOrderNew", "Edit", id);
                        break;
                    case "4":
                        ChangeButtonPatchView("SaleOrder", "btnPatchSaleOrderNew", "Approved", id);
                        break;
                    default:
                        ChangeButtonPatchView("SaleOrder", "btnPatchSaleOrderNew", "LockDocument", id);
                        break;
                }
            }
            else {
                ChangeButtonPatchView("SaleOrder", "btnPatchSaleOrderNew", "LockDocument", id);
            }
            BindSaleOrderDetailList(id, false, false);
            BindSaleOrderOtherChargesDetailList(id, false);
            CalculateTotal();
            $('#divCustomerBasicInfo').load("Customer/CustomerBasicInfo?ID=" + $('#hdnCustomerID').val(), function () {
            });
            clearUploadControl();
            PaintImages(id);
        }
        else {
            console.log("Error: " + xhr.status + ": " + xhr.statusText);
        }
    });
}