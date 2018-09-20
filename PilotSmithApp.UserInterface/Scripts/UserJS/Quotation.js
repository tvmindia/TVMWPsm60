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
        BindOrReloadQuotationTable('Init');
        $('#tblQuotation tbody').on('dblclick', 'td', function () {
            if (this.textContent !== "No data available in table")
                EditQuotation(this);
        });
        if ($('#RedirectToDocument').val() != "") {
            if ($('#RedirectToDocument').val() === _emptyGuid) {
                AddQuotation();
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
//function bind the Quotation list checking search and filter
function BindOrReloadQuotationTable(action) {
    try {
        debugger;
        //creating advancesearch object
        QuotationAdvanceSearchViewModel = new Object();
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
                //if (($('#SearchTerm').val() == "") && ($('#FromDate').val() == "") && ($('#ToDate').val() == "")) {
                //    return true;
                //}
                if (($('#SearchTerm').val() == "") && ($('.divboxASearch #AdvFromDate').val() == "") && ($('#AdvToDate').val() == "") && ($('.divboxASearch #AdvAreaCode').val() == "") && ($('.divboxASearch #AdvCustomerID').val() == "") && ($('.divboxASearch #AdvReferencePersonCode').val() == "") && ($('.divboxASearch #AdvBranchCode').val() == "") && ($('.divboxASearch #AdvDocumentStatusCode').val() == "") && ($('.divboxASearch #AdvDocumentOwnerID').val() == "") && ($('#AdvEmailSentStatus').val() == "") && ($('#AdvApprovalStatusCode').val()=="")) {
                    return true;
                }
                break;
            case 'Export':
                DataTablePagingViewModel.Length = -1;
                QuotationAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
                QuotationAdvanceSearchViewModel.SearchTerm = $('#SearchTerm').val() == "" ? null : $('#SearchTerm').val();
                QuotationAdvanceSearchViewModel.AdvFromDate = $('.divboxASearch #AdvFromDate').val() == "" ? null : $('.divboxASearch #AdvFromDate').val();
                QuotationAdvanceSearchViewModel.AdvToDate = $('.divboxASearch #AdvToDate').val() == "" ? null : $('.divboxASearch #AdvToDate').val();
                QuotationAdvanceSearchViewModel.AdvAreaCode = $('.divboxASearch #AdvAreaCode').val() == "" ? null : $('.divboxASearch #AdvAreaCode').val();
                QuotationAdvanceSearchViewModel.AdvCustomerID = $('.divboxASearch #AdvCustomerID').val() == "" ? _emptyGuid : $('.divboxASearch #AdvCustomerID').val();
                QuotationAdvanceSearchViewModel.AdvReferencePersonCode = $('.divboxASearch #AdvReferencePersonCode').val() == "" ? null : $('.divboxASearch #AdvReferencePersonCode').val();
                QuotationAdvanceSearchViewModel.AdvBranchCode = $('.divboxASearch #AdvBranchCode').val() == "" ? null : $('.divboxASearch #AdvBranchCode').val();
                QuotationAdvanceSearchViewModel.AdvDocumentStatusCode = $('.divboxASearch #AdvDocumentStatusCode').val() == "" ? null : $('.divboxASearch #AdvDocumentStatusCode').val();
                QuotationAdvanceSearchViewModel.AdvDocumentOwnerID = $('.divboxASearch #AdvDocumentOwnerID').val() == "" ? _emptyGuid : $('.divboxASearch #AdvDocumentOwnerID').val();
                QuotationAdvanceSearchViewModel.AdvApprovalStatusCode = $('.divboxASearch #AdvApprovalStatusCode').val() == "" ? null : $('.divboxASearch #AdvApprovalStatusCode').val();
                QuotationAdvanceSearchViewModel.AdvEmailSentStatus = $('#AdvEmailSentStatus').val() == "" ? null : $('#AdvEmailSentStatus').val();
                $('#AdvanceSearch').val(JSON.stringify(QuotationAdvanceSearchViewModel));
                $('#FormExcelExport').submit();
                return true;
                break;
            default:
                break;
        }
        QuotationAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
        QuotationAdvanceSearchViewModel.SearchTerm = $('#SearchTerm').val();       
        QuotationAdvanceSearchViewModel.AdvFromDate = $('.divboxASearch #AdvFromDate').val();
        QuotationAdvanceSearchViewModel.AdvToDate = $('.divboxASearch #AdvToDate').val();
        QuotationAdvanceSearchViewModel.AdvAreaCode = $('.divboxASearch #AdvAreaCode').val();
        QuotationAdvanceSearchViewModel.AdvCustomerID = $('.divboxASearch #AdvCustomerID').val();
        QuotationAdvanceSearchViewModel.AdvReferencePersonCode = $('.divboxASearch #AdvReferencePersonCode').val();
        QuotationAdvanceSearchViewModel.AdvBranchCode = $('.divboxASearch #AdvBranchCode').val();
        QuotationAdvanceSearchViewModel.AdvDocumentStatusCode = $('.divboxASearch #AdvDocumentStatusCode').val();
        QuotationAdvanceSearchViewModel.AdvDocumentOwnerID = $('.divboxASearch #AdvDocumentOwnerID').val();
        QuotationAdvanceSearchViewModel.AdvApprovalStatusCode = $('.divboxASearch #AdvApprovalStatusCode').val();
        QuotationAdvanceSearchViewModel.AdvEmailSentStatus = $('#AdvEmailSentStatus').val();
        //apply datatable plugin on Quotation table
        _dataTable.QuotationList = $('#tblQuotation').DataTable(
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
                url: "Quotation/GetAllQuotation/",
                data: { "QuotationAdvanceSearchVM": QuotationAdvanceSearchViewModel },
                type: 'POST'
            },
            pageLength: 8,
            columns: [             

               {"data": "QuoteNo", render: function (data, type, row) {
                       return row.QuoteNo + "</br>" + "<img src='./Content/images/datePicker.png' height='10px'>"+"&nbsp;" + row.QuoteDateFormatted;
                   }, "defaultContent": "<i>-</i>"
               },
               {"data": "Customer.CompanyName", render: function (data, type, row)
                   {
                        return "<img src='./Content/images/contact.png' height='10px'>" + "&nbsp;" + (row.Customer.ContactPerson == null ? "" : row.Customer.ContactPerson) + "</br>" + "<img src='./Content/images/organisation.png' height='10px'>" + "&nbsp;" + data;
                    }, "defaultContent": "<i>-</i>"
               },             
               //{ "data": "RequirementSpec", "defaultContent": "<i>-</i>" },
               { "data": "Area.Description", "defaultContent": "<i>-</i>" },
               { "data": "ReferencePerson.Name", "defaultContent": "<i>-</i>" },              
               { "data": "Branch.Description", render: function (data, type, row)
                   {
                        return  "<b>Doc.Owner-</b>" + row.PSAUser.LoginName + "</br>" + "<b>Branch-</b>" + row.Branch.Description;
                    }, "defaultContent": "<i>-</i>"
               },               
               {"data": "DocumentStatus.Description", render: function (data, type, row) {
                   return "<b>Doc.Status-</b>" + data + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + (row.EmailSentYN == true ? "<img src='./Content/images/mailSend.png' height='20px'  >" : '')
                     
                       
                       + "</br>" + "<b>Appr.Status-</b>" + row.ApprovalStatus.Description;
                    }, "defaultContent": "<i>-</i>"
               },

               { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink"  onclick="EditQuotation(this)" ><i class="fa fa-pencil-square-o" aria-hidden="true"></i></a>' },
            ],
            columnDefs: [{ className: "text-right", "targets": [] },
                          { className: "text-left", "targets": [0,1,2,3, 4, 5] },
                          { className: "text-center", "targets": [6] },
                            { "targets": [0], "width": "10%" },
                            { "targets": [1], "width": "12%" },
                            { "targets": [2, 3], "width": "10%" },
                            { "targets": [4], "width": "15%" },
                            { "targets": [5], "width": "22%" },
                            { "targets": [6], "width": "3%" },                         
            ],
            destroy: true,
            //for performing the import operation after the data loaded
            initComplete: function (settings, json) {
                debugger;
                $('.dataTables_wrapper div.bottom div').addClass('col-md-6');
                $('#tblQuotation').fadeIn(100);
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
function ResetQuotationList() {
    $(".searchicon").removeClass('filterApplied');
    BindOrReloadQuotationTable('Reset');
}
//function export data to excel
function ExportQuotationData() {
    BindOrReloadQuotationTable('Export');
}
// add Quotation section
function AddQuotation() {
    //this will return form body(html)
    OnServerCallBegin();
    $("#divQuotationForm").load("Quotation/QuotationForm?id=" + _emptyGuid , function (responseTxt, statusTxt, xhr) {
        if (statusTxt == "success") {
            OnServerCallComplete();
            openNav();
            $('#lblQuotationInfo').text('<<Quotation No.>>');
            ChangeButtonPatchView("Quotation", "btnPatchQuotationNew", "Add");
            BindQuotationDetailList(_emptyGuid);
            BindQuotationOtherChargesDetailList(_emptyGuid);
        }
        else {
            console.log("Error: " + xhr.status + ": " + xhr.statusText);
        }
    });
}
function EditQuotation(this_Obj) {
    debugger;
    OnServerCallBegin();
    var Quotation = _dataTable.QuotationList.row($(this_Obj).parents('tr')).data();
    //this will return form body(html)
    $("#divQuotationForm").load("Quotation/QuotationForm?id=" + Quotation.ID , function (responseTxt, statusTxt, xhr) {
        if (statusTxt == "success") {
            debugger;
            OnServerCallComplete();
            openNav();
            $('#lblQuotationInfo').text(Quotation.QuoteNo);
            //$('#CustomerID').trigger('change');
            if ($('#IsDocLocked').val() == "True") {

                debugger;
                switch ($('#LatestApprovalStatus').val()) {
                    case "0":
                        ChangeButtonPatchView("Quotation", "btnPatchQuotationNew", "Draft", Quotation.ID);
                        break;
                    case "1":                      
                        if ($('#ApproverLevel').val() > 1) {
                            ChangeButtonPatchView("Quotation", "btnPatchQuotationNew", "Approved", Quotation.ID);
                        }
                        else {
                            ChangeButtonPatchView("Quotation", "btnPatchQuotationNew", "Recalled", Quotation.ID);
                        }
                       
                        break;
                    case "3":
                        ChangeButtonPatchView("Quotation", "btnPatchQuotationNew", "Edit", Quotation.ID);
                        break;
                    case "4":
                        ChangeButtonPatchView("Quotation", "btnPatchQuotationNew", "Approved", Quotation.ID);
                        break;
                    default:
                        ChangeButtonPatchView("Quotation", "btnPatchQuotationNew", "LockDocument", Quotation.ID);
                        break;
                }
            }
            else {
                ChangeButtonPatchView("Quotation", "btnPatchQuotationNew", "LockDocument", Quotation.ID);
            }
            BindQuotationDetailList(Quotation.ID);
            BindQuotationOtherChargesDetailList(Quotation.ID);
            CalculateTotal();
            $('#divCustomerBasicInfo').load("Customer/CustomerBasicInfo?ID=" + $('#hdnCustomerID').val());
            clearUploadControl();
            PaintImages(Quotation.ID);
            $("#divQuotationForm #EstimateID").prop('disabled', true);
        }
        else {
            console.log("Error: " + xhr.status + ": " + xhr.statusText);
        }
    });
}
function ResetQuotation() {
    $("#divQuotationForm").load("Quotation/QuotationForm?id=" + $('#QuotationForm #ID').val() , function (responseTxt, statusTxt, xhr) {
        if (statusTxt == "success") {
            if ($('#ID').val() != _emptyGuid && $('#ID').val() != null) {
                    $("#divQuotationForm #EstimateID").prop('disabled', true);
                    //resides in customjs for sliding
                    openNav();
            }
            else {
                debugger;
                $('#hdnCustomerID').val('');
                $("#QuotationForm #CustomerID").prop('disabled', false);
                $('#lblQuotationInfo').text('<<Quotation No.>>');
            }

            switch ($('#LatestApprovalStatus').val()) {
                case "":
                    ChangeButtonPatchView("Quotation", "btnPatchQuotationNew", "Add");
                    break;
                case "0":
                    ChangeButtonPatchView("Quotation", "btnPatchQuotationNew", "Draft", $('#ID').val());
                    break;
                case "1":
                    if ($('#ApproverLevel').val() > 1) {
                        ChangeButtonPatchView("Quotation", "btnPatchQuotationNew", "Approved", $('#ID').val());
                    }
                    else {
                        ChangeButtonPatchView("Quotation", "btnPatchQuotationNew", "Recalled", $('#ID').val());
                    }
                    break;
                case "3":
                    ChangeButtonPatchView("Quotation", "btnPatchQuotationNew", "Edit", $('#ID').val());
                    break;
                case "4":
                    ChangeButtonPatchView("Quotation", "btnPatchQuotationNew", "Approved", $('#ID').val());
                    break;
                default:
                    ChangeButtonPatchView("Quotation", "btnPatchQuotationNew", "LockDocument", $('#ID').val());
                    break;
            }
            BindQuotationDetailList($('#ID').val(), false);
            BindQuotationOtherChargesDetailList($('#ID').val());
            CalculateTotal();
            clearUploadControl();
            PaintImages($('#QuotationForm #ID').val());
            $('#divCustomerBasicInfo').load("Customer/CustomerBasicInfo?ID=" + $('#QuotationForm #hdnCustomerID').val());
        }
        else {
            console.log("Error: " + xhr.status + ": " + xhr.statusText);
        }
    });
}
function SaveQuotation() {
    var quotationDetailList = _dataTable.QuotationDetailList.rows().data().toArray();
    $('#DetailJSON').val(JSON.stringify(quotationDetailList));
    var otherChargesDetailList = _dataTable.QuotationOtherChargesDetailList.rows().data().toArray();
    $('#OtherChargesDetailJSON').val(JSON.stringify(otherChargesDetailList));
    $('#btnInsertUpdateQuotation').trigger('click');
}
function ApplyFilterThenSearch() {
    $(".searchicon").addClass('filterApplied');
    CloseAdvanceSearch();
    BindOrReloadQuotationTable('Search');
}
function SaveSuccessQuotation(data, status) {
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
                $("#divQuotationForm").load("Quotation/QuotationForm?id=" + _result.ID + "&estimateID="+ _result.EstimateID, function () {
                    ChangeButtonPatchView("Quotation", "btnPatchQuotationNew", "Edit", _result.ID);
                    $('#lblQuotationInfo').text(_result.QuotationNo);
                    BindQuotationDetailList(_result.ID);
                    BindQuotationOtherChargesDetailList(_result.ID);
                    CalculateTotal();
                    clearUploadControl();
                    PaintImages(_result.ID);
                    $('#divCustomerBasicInfo').load("Customer/CustomerBasicInfo?ID=" + $('#QuotationForm #hdnCustomerID').val());
                });
                ChangeButtonPatchView("Quotation", "btnPatchQuotationNew", "Edit", _result.ID);
                BindOrReloadQuotationTable('Init');
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

function DeleteQuotation() {
    notyConfirm('Are you sure to delete?', 'DeleteQuotationItem("' + $('#QuotationForm #ID').val() + '")');
}
function DeleteQuotationItem(id) {
    try {
        if (id) {
            var data = { "id": id };
            _jsonData = GetDataFromServer("Quotation/DeleteQuotation/", data);
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
                    ChangeButtonPatchView("Quotation", "btnPatchQuotationNew", "Add");
                    ResetQuotation();
                    BindOrReloadQuotationTable('Init');
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
function BindQuotationOtherChargesDetailList(id) {
    debugger;
    _dataTable.QuotationOtherChargesDetailList = $('#tblQuotationOtherChargesDetailList').DataTable(
         {
             dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
             order: [],
             searching: false,
             paging: false,
             ordering: false,
             bInfo: false,
             data: id == _emptyGuid ? null : GetQuotationOtherChargesDetailListByQuotationID(id),
             language: {
                 search: "_INPUT_",
                 searchPlaceholder: "Search"
             },
             columns: [
             { "data": "OtherCharge.Description", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
             { "data": "OtherCharge.SACCode", "defaultContent": "<i></i>" },
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
                     var Total = roundoff(parseFloat(data) + parseFloat(GSTAmt))
                     //return Total
                     return '<div class="show-popover text-right" data-html="true" data-toggle="popover" data-placement="left" data-title="<p align=left>Total : ₹ ' + Total + '" data-content="Charge Amount : ₹ ' + data + '<br/>GST : ₹ ' + GSTAmt + '</p>"/>' + Total
                 }, "defaultContent": "<i></i>"
             },
             {
                 "data": "ChargeAmount", "orderable": false,render:function(data,type,row){
                     if (($('#IsDocLocked').val() == "False" || $('#IsUpdate').val() == "False" || $('#LatestApprovalStatus').val() == "1" || $('#LatestApprovalStatus').val() == "9" || $('#LatestApprovalStatus').val() == "4") && $('#LatestApprovalStatus').val()!="") {
                         return "-"
                     }
                     else {
                         return '<a href="#" class="actionLink"  onclick="EditQuotationOtherChargesDetail(this)" ><i class="fa fa-pencil-square-o" aria-hidden="true"></i></a> <a href="#" class="DeleteLink"  onclick="ConfirmDeleteQuotationOtherChargeDetail(this)" ><i class="fa fa-trash-o" aria-hidden="true"></i></a>'
                     }
                 },
                 "defaultContent": "<i></i>"
             },
             ],             
             columnDefs: [
                 { "targets": [0], "width": "30%" },
                 { "targets": [1, 2,4], "width": "20%" },
                 { "targets": [3,5], "width": "10%" },
                 { className: "text-right", "targets": [ 2, 3,4] },
                 { className: "text-left", "targets": [0,1] },
                 { className: "text-center", "targets": [5] }
             ],
             destroy: true,
         });
    $('[data-toggle="popover"]').popover({
        html: true,
        'trigger': 'hover',
    });
}
function BindQuotationDetailList(id, IsEstimated) {
    debugger;
    //ClearCalculatedFields();
    _dataTable.QuotationDetailList = $('#tblQuotationDetails').DataTable(
         {
             dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
             order: [],
             searching: false,
             paging: false,
             ordering: false,
             bInfo: false,
             data: !IsEstimated ? id == _emptyGuid ? null : GetQuotationDetailListByQuotationID(id, false) : GetQuotationDetailListByQuotationID(id, true),
             language: {
                 search: "_INPUT_",
                 searchPlaceholder: "Search"
             },
             columns: [
             {
                 "data": "Product.Code", render: function (data, type, row) {
                     debugger;
                     if (row.ProductModel.ImageURL!==null)
                     {
                         return row.Product.Name + "<br/>" + '<div style="width:100%" class="show-popover" data-placement="top" data-html="true" data-toggle="popover" data-placement="top" data-title="<p align=left>Product Specification" data-content="' + "<div><table><tr><td><img height='80px' src='" + row.ProductModel.ImageURL + "'></td><td>&nbsp;</td><td>" + (row.ProductSpecHtml !== null ? row.ProductSpecHtml.replace(/"/g, "&quot") : "") + '</td></tr></table></div></p>"/>' + row.ProductModel.Name
                     }
                     else {
                         return row.Product.Name + "<br/>" + '<div style="width:100%" class="show-popover" data-placement="top" data-html="true" data-toggle="popover" data-placement="top" data-title="<p align=left>Product Specification" data-content="' + (row.ProductSpecHtml !== null ? row.ProductSpecHtml.replace(/"/g, "&quot") : "") + '</p>"/>' + row.ProductModel.Name
                     }
                 }, "defaultContent": "<i></i>"
             },
             { "data": "Product.HSNCode", "defaultContent": "<i></i>" },
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
                     return '<div class="show-popover text-right" data-html="true" data-placement="left" data-toggle="popover" data-placement="left" data-title="<p align=left>Taxable : ₹ ' + Taxable + '" data-content="Net Total : ₹ ' + Total + '<br/> Discount : ₹ -' + Discount + '</p>"/>' + Taxable
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
                 "data": "Rate", render: function (data, type, row) {
                     debugger;
                     var CGST = parseFloat(row.CGSTPerc != "" ? row.CGSTPerc : 0);
                     var SGST = parseFloat(row.SGSTPerc != "" ? row.SGSTPerc : 0);
                     var IGST = parseFloat(row.IGSTPerc != "" ? row.IGSTPerc : 0);
                     var TaxableAmt = roundoff((parseFloat(row.Rate != "" ? row.Rate : 0) * parseInt(row.Qty != "" ? row.Qty : 1)) - parseFloat(row.Discount != "" ? row.Discount : 0))
                     var CGSTAmt = parseFloat(TaxableAmt * CGST / 100);
                     var SGSTAmt = parseFloat(TaxableAmt * SGST / 100)
                     var IGSTAmt = parseFloat(TaxableAmt * IGST / 100)
                     var GSTAmt = roundoff(CGSTAmt + SGSTAmt + IGSTAmt)
                     var GrandTotal = roundoff(((parseFloat(row.Rate != "" ? row.Rate : 0) * parseInt(row.Qty != "" ? row.Qty : 1)) - parseFloat(row.Discount != "" ? row.Discount : 0)) + parseFloat(GSTAmt))
                     return '<div class="show-popover text-right" data-html="true" data-toggle="popover" data-placement="left" data-title="<p align=left>Grand Total : ₹ ' + GrandTotal + '" data-content="Taxable : ₹ ' + TaxableAmt + '<br/>GST : ₹ ' + GSTAmt + '</p>"/>' + GrandTotal
                 }, "defaultContent": "<i></i>"
             },
             {
                 "data": "Rate", "orderable": false, render: function (data, type, row) {
                     debugger;
                     if (($('#IsDocLocked').val() == "False" || $('#IsUpdate').val() == "False" || $('#LatestApprovalStatus').val() == "1" || $('#LatestApprovalStatus').val() == "9" || $('#LatestApprovalStatus').val() == "4") && $('#LatestApprovalStatus').val()!="") {
                         return "-"
                     }
                     else {
                         return '<a href="#" class="actionLink"  onclick="EditQuotationDetail(this)" ><i class="fa fa-pencil-square-o" aria-hidden="true"></i></a> <a href="#" class="DeleteLink"  onclick="ConfirmDeleteQuotationDetail(this)" ><i class="fa fa-trash-o" aria-hidden="true"></i></a>'
                     }
                 }, "defaultContent": "<i></i>"
                     //($('#IsDocLocked').val() == "True" || $('#IsUpdate').val() == "False" ) ? '<a href="#" class="actionLink"  onclick="EditQuotationDetail(this)" ><i class="fa fa-pencil-square-o" aria-hidden="true"></i></a> <a href="#" class="DeleteLink"  onclick="ConfirmDeleteQuotationDetail(this)" ><i class="fa fa-trash-o" aria-hidden="true"></i></a>' : "-"
             },
             ],
             columnDefs: [
                 { "targets": [0], "width": "23%" },
                 { "targets": [1, 2, 3, 4, 5, 6, 7], "width": "10%" },
                 { "targets": [8], "width": "7%" },
                 { className: "text-right", "targets": [2,3,4, 5, 6,7] },
                 { className: "text-left", "targets": [1,4, 0] },
                 { className: "text-center", "targets": [8] }
             ],
             initComplete: function (settings, json) {
                 $('#QuotationForm #Discount').trigger('change');
             },
             destroy:true
         });
    $('[data-toggle="popover"]').popover({
        html: true,
        'trigger': 'hover'       
    });
}
function GetQuotationDetailListByQuotationID(id, IsEstimated) {
    try {
        debugger;
        
        var quotationDetailList = [];
        if (IsEstimated) {
            var data = { "estimateID": $('#QuotationForm #hdnEstimateID').val() };
            _jsonData = GetDataFromServer("Quotation/GetQuotationDetailListByQuotationIDWithEstimate/", data);
        }
        else {
            var data = { "quotationID": id };
            _jsonData = GetDataFromServer("Quotation/GetQuotationDetailListByQuotationID/", data);
        }

        if (_jsonData != '') {
            _jsonData = JSON.parse(_jsonData);
            _message = _jsonData.Message;
            _status = _jsonData.Status;
            quotationDetailList = _jsonData.Records;
        }
        if (_status == "OK") {
            return quotationDetailList;
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
function AddQuotationDetailList() {
    $("#divModelQuotationPopBody").load("Quotation/AddQuotationDetail?update=false", function () {
        $('#lblModelPopQuotation').text('Quotation Detail')
        $('#divModelPopQuotation').modal('show');
    });
}
function AddQuotationDetailToList() {
    debugger;
    $("#FormQuotationDetail").submit(function () { });
        debugger;
        if ($('#FormQuotationDetail #IsUpdate').val() == 'True') {
            if (($('#divModelQuotationPopBody #Rate').val() != "") && ($('#divModelQuotationPopBody #Qty').val() != "") && ($('#divModelQuotationPopBody #UnitCode').val() != "")) {
                debugger;
                var quotationDetailList = _dataTable.QuotationDetailList.rows().data();
                //quotationDetailList[_datatablerowindex].Product.Code = $("#divModelQuotationPopBody #ProductID").val() != "" ? $("#divModelQuotationPopBody #ProductID option:selected").text().split("-")[0].trim() : "";
                //quotationDetailList[_datatablerowindex].Product.Name = $("#divModelQuotationPopBody #ProductID").val() != "" ? $("#divModelQuotationPopBody #ProductID option:selected").text().split("-")[1].trim() : "";
                quotationDetailList[_datatablerowindex].Product.Code = $('#spanProductName').text() != "" ? $('#spanProductName').text().split("-")[0].trim() : "";
                quotationDetailList[_datatablerowindex].Product.Name = $('#spanProductName').text() != "" ? $('#spanProductName').text().split("-")[1].trim() : "";


                quotationDetailList[_datatablerowindex].Product.HSNCode = $("#hdnProductHSNCode").val();
                //quotationDetailList[_datatablerowindex].ProductID = $("#divModelPopQuotation #ProductID").val() != "" ? $("#divModelPopQuotation #ProductID").val() : _emptyGuid;
                //quotationDetailList[_datatablerowindex].ProductModelID = $("#divModelQuotationPopBody #ProductModelID").val() != "" ? $("#divModelQuotationPopBody #ProductModelID").val() : _emptyGuid;
                ProductModel = new Object;
                Unit = new Object;
                TaxType = new Object;
                ProductModel.Name = $('#spanProductModelName').text();
               // ProductModel.Name = $("#divModelQuotationPopBody #ProductModelID").val() != "" ? $("#divModelQuotationPopBody #ProductModelID option:selected").text() : "";
                quotationDetailList[_datatablerowindex].ProductModel = ProductModel;
                quotationDetailList[_datatablerowindex].ProductModel.ImageURL = $('#hdnProductModelImage').val();
                quotationDetailList[_datatablerowindex].ProductSpecHtml = $('#divModelQuotationPopBody #ProductSpec').val();
                quotationDetailList[_datatablerowindex].Qty = $('#divModelQuotationPopBody #Qty').val();
                quotationDetailList[_datatablerowindex].UnitCode = $('#divModelQuotationPopBody #UnitCode').val();
                Unit.Description = $("#divModelQuotationPopBody #UnitCode").val() != "" ? $("#divModelQuotationPopBody #UnitCode option:selected").text().trim() : "";
                quotationDetailList[_datatablerowindex].Unit = Unit;
                quotationDetailList[_datatablerowindex].Rate = $('#divModelQuotationPopBody #Rate').val();
                quotationDetailList[_datatablerowindex].Discount = $('#divModelQuotationPopBody #Discount').val() != "" ? $('#divModelQuotationPopBody #Discount').val() : 0;
                quotationDetailList[_datatablerowindex].TaxTypeCode = $('#divModelQuotationPopBody #TaxTypeCode').val()!=null? $('#divModelQuotationPopBody #TaxTypeCode').val().split('|')[0]:"";
                TaxType.ValueText = $('#divModelQuotationPopBody #TaxTypeCode').val();
                quotationDetailList[_datatablerowindex].TaxType = TaxType;
                quotationDetailList[_datatablerowindex].CGSTPerc = $('#divModelQuotationPopBody #hdnCGSTPerc').val();
                quotationDetailList[_datatablerowindex].SGSTPerc = $('#divModelQuotationPopBody #hdnSGSTPerc').val();
                quotationDetailList[_datatablerowindex].IGSTPerc = $('#divModelQuotationPopBody #hdnIGSTPerc').val();
                ClearCalculatedFields();
                _dataTable.QuotationDetailList.clear().rows.add(quotationDetailList).draw(false);
                CalculateTotal();
                $('#divModelPopQuotation').modal('hide');
                _datatablerowindex = -1;
            }
        }
        else {
            if (($('#divModelQuotationPopBody #ProductID').val() != "") && ($('#divModelQuotationPopBody #ProductModelID').val() != "") && ($('#divModelQuotationPopBody #Rate').val() != "") && ($('#divModelQuotationPopBody #Qty').val() != "") && ($('#divModelQuotationPopBody #UnitCode').val() != ""))
            {
                debugger;
                if (_dataTable.QuotationDetailList.rows().data().length === 0) {
                    _dataTable.QuotationDetailList.clear().rows.add(GetQuotationDetailListByQuotationID(_emptyGuid,false)).draw(false);
                    debugger;
                    var quotationDetailList = _dataTable.QuotationDetailList.rows().data();
                    quotationDetailList[0].Product.Code = $("#divModelQuotationPopBody #ProductID").val() != "" ? $("#divModelQuotationPopBody #ProductID option:selected").text().split("-")[0].trim() : "";
                    quotationDetailList[0].Product.Name = $("#divModelQuotationPopBody #ProductID").val() != "" ? $("#divModelQuotationPopBody #ProductID option:selected").text().split("-")[1].trim() : "";
                    quotationDetailList[0].Product.HSNCode = $("#hdnProductHSNCode").val();
                    quotationDetailList[0].ProductID = $("#divModelQuotationPopBody #ProductID").val() != "" ? $("#divModelQuotationPopBody #ProductID").val() : _emptyGuid;
                    quotationDetailList[0].ProductModelID = $("#divModelQuotationPopBody #ProductModelID").val() != "" ? $("#divModelQuotationPopBody #ProductModelID").val() : _emptyGuid;
                    quotationDetailList[0].ProductModel.Name = $("#divModelQuotationPopBody #ProductModelID").val() != "" ? $("#divModelQuotationPopBody #ProductModelID option:selected").text() : "";
                    quotationDetailList[0].ProductModel.ImageURL = $('#hdnProductModelImage').val();
                    quotationDetailList[0].ProductSpecHtml = $('#divModelQuotationPopBody #ProductSpec').val();
                    quotationDetailList[0].Qty = $('#divModelQuotationPopBody #Qty').val();
                    quotationDetailList[0].UnitCode = $('#divModelQuotationPopBody #UnitCode').val();
                    quotationDetailList[0].Unit.Description = $("#divModelQuotationPopBody #UnitCode").val() != "" ? $("#divModelQuotationPopBody #UnitCode option:selected").text().trim() : "";
                    quotationDetailList[0].Rate = $('#divModelQuotationPopBody #Rate').val();
                    quotationDetailList[0].Discount = $('#divModelQuotationPopBody #Discount').val() != "" ? $('#divModelQuotationPopBody #Discount').val() : 0;
                    quotationDetailList[0].TaxTypeCode =$('#divModelQuotationPopBody #TaxTypeCode').val()!=null? $('#divModelQuotationPopBody #TaxTypeCode').val().split('|')[0]:"";
                    quotationDetailList[0].TaxType.ValueText = $('#divModelQuotationPopBody #TaxTypeCode').val();
                    quotationDetailList[0].CGSTPerc = $('#divModelQuotationPopBody #hdnCGSTPerc').val();
                    quotationDetailList[0].SGSTPerc = $('#divModelQuotationPopBody #hdnSGSTPerc').val();
                    quotationDetailList[0].IGSTPerc = $('#divModelQuotationPopBody #hdnIGSTPerc').val();
                    ClearCalculatedFields();
                    _dataTable.QuotationDetailList.clear().rows.add(quotationDetailList).draw(false);
                    CalculateTotal();
                    $('#divModelPopQuotation').modal('hide');
                }
                else {
                    debugger;
                    var quotationDetailList = _dataTable.QuotationDetailList.rows().data();
                    if (quotationDetailList.length > 0) {
                        var checkpoint = 0;
                        var productSpec = $('#ProductSpec').val();
                        productSpec = productSpec.replace(/\n/g, ' ');
                        for (var i = 0; i < quotationDetailList.length; i++) {
                            if ((quotationDetailList[i].ProductID == $('#ProductID').val()) && (quotationDetailList[i].ProductModelID == $('#ProductModelID').val()
                                && (quotationDetailList[i].ProductSpec.replace(/\n/g, ' ') == productSpec && (quotationDetailList[i].UnitCode == $('#UnitCode').val())))) {
                                quotationDetailList[i].Qty = parseFloat(quotationDetailList[i].Qty) + parseFloat($('#Qty').val());
                                checkpoint = 1;
                                break;
                            }
                        }
                        if (checkpoint == 1) {
                            debugger;
                            ClearCalculatedFields();
                            _dataTable.QuotationDetailList.clear().rows.add(quotationDetailList).draw(false);
                            CalculateTotal();
                            $('#divModelPopQuotation').modal('hide');
                        }
                        else if (checkpoint == 0) {
                            ClearCalculatedFields();
                            var QuotationDetailVM = new Object();
                            QuotationDetailVM.ID = _emptyGuid;
                            QuotationDetailVM.ProductID = ($("#divModelQuotationPopBody #ProductID").val() != "" ? $("#divModelQuotationPopBody #ProductID").val() : _emptyGuid);
                            var Product = new Object;
                            Product.Code = ($("#divModelQuotationPopBody #ProductID").val() != "" ? $("#divModelQuotationPopBody #ProductID option:selected").text().split("-")[0].trim() : "");
                            Product.Name = ($("#divModelQuotationPopBody #ProductID").val() != "" ? $("#divModelQuotationPopBody #ProductID option:selected").text().split("-")[1].trim() : "");
                            Product.HSNCode = $("#hdnProductHSNCode").val();
                            QuotationDetailVM.Product = Product;
                            QuotationDetailVM.ProductModelID = ($("#divModelQuotationPopBody #ProductModelID").val() != "" ? $("#divModelQuotationPopBody #ProductModelID").val() : _emptyGuid);
                            var ProductModel = new Object()
                            ProductModel.Name = ($("#ProductModelID").val() != "" ? $("#ProductModelID option:selected").text() : "");
                            QuotationDetailVM.ProductModel = ProductModel;
                            QuotationDetailVM.ProductModel.ImageURL = $('#hdnProductModelImage').val();
                            QuotationDetailVM.ProductSpecHtml = $('#divModelQuotationPopBody #ProductSpec').val();
                            QuotationDetailVM.Qty = $('#divModelQuotationPopBody #Qty').val();
                            var Unit = new Object();
                            Unit.Description = $("#divModelQuotationPopBody #UnitCode").val() != "" ? $("#divModelQuotationPopBody #UnitCode option:selected").text().trim() : "";
                            QuotationDetailVM.Unit = Unit;
                            QuotationDetailVM.UnitCode = $('#divModelQuotationPopBody #UnitCode').val();
                            QuotationDetailVM.Rate = $('#divModelQuotationPopBody #Rate').val();
                            QuotationDetailVM.Discount = $('#divModelQuotationPopBody #Discount').val() != "" ? $('#divModelQuotationPopBody #Discount').val() : 0;
                            QuotationDetailVM.TaxTypeCode =$('#divModelQuotationPopBody #TaxTypeCode').val()!=null? $('#divModelQuotationPopBody #TaxTypeCode').val().split('|')[0]:"";
                            var TaxType = new Object();
                            TaxType.ValueText = $('#divModelQuotationPopBody #TaxTypeCode').val();
                            QuotationDetailVM.TaxType = TaxType;
                            QuotationDetailVM.CGSTPerc = $('#divModelQuotationPopBody #hdnCGSTPerc').val();
                            QuotationDetailVM.SGSTPerc = $('#divModelQuotationPopBody #hdnSGSTPerc').val();
                            QuotationDetailVM.IGSTPerc = $('#divModelQuotationPopBody #hdnIGSTPerc').val();
                            _dataTable.QuotationDetailList.row.add(QuotationDetailVM).draw(true);
                            CalculateTotal();
                            $('#divModelPopQuotation').modal('hide');
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
function EditQuotationDetail(this_Obj) {
    debugger;
    _datatablerowindex = _dataTable.QuotationDetailList.row($(this_Obj).parents('tr')).index();
    var quotationDetail = _dataTable.QuotationDetailList.row($(this_Obj).parents('tr')).data();
    $("#divModelQuotationPopBody").load("Quotation/AddQuotationDetail?update=true", function () {
        $('#lblModelPopQuotation').text('Quotation Detail')
        $('#FormQuotationDetail #IsUpdate').val('True');
        $('#FormQuotationDetail #ID').val(quotationDetail.ID);
        $("#FormQuotationDetail #ProductID").val(quotationDetail.ProductID)
        $("#FormQuotationDetail #hdnProductID").val(quotationDetail.ProductID)
        $('#spanProductName').text(quotationDetail.Product.Code + "-" + quotationDetail.Product.Name)
        $('#spanProductModelName').text(quotationDetail.ProductModel.Name)
        $('#divProductBasicInfo').load("Product/ProductBasicInfo?ID=" + $('#hdnProductID').val(), function () {
        });

        //if ($('#hdnProductID').val() != _emptyGuid) {
        //    $('.divProductModelSelectList').load("ProductModel/ProductModelSelectList?required=required&productID=" + $('#hdnProductID').val())
        //}
        //else {
        //    $('.divProductModelSelectList').empty();
        //    $('.divProductModelSelectList').append('<span class="form-control newinput"><i id="dropLoad" class="fa fa-spinner"></i></span>');
        //}
        $("#FormQuotationDetail #ProductModelID").val(quotationDetail.ProductModelID);
        $("#FormQuotationDetail #hdnProductModelID").val(quotationDetail.ProductModelID);
        //if ($('#hdnProductModelID').val() != _emptyGuid) {
        //    $('#divProductBasicInfo').load("ProductModel/ProductModelBasicInfo?ID=" + $('#hdnProductModelID').val(), function () {
        //    });
        //}
        $('#FormQuotationDetail #ProductSpec').val(quotationDetail.ProductSpecHtml);
        $('#FormQuotationDetail #Qty').val(quotationDetail.Qty);
        $('#FormQuotationDetail #UnitCode').val(quotationDetail.UnitCode);
        $('#FormQuotationDetail #hdnUnitCode').val(quotationDetail.UnitCode);
        $('#FormQuotationDetail #Rate').val(quotationDetail.Rate);
        $('#FormQuotationDetail #Discount').val(quotationDetail.Discount);
        $('#FormQuotationDetail #TaxTypeCode').val(quotationDetail.TaxType.ValueText);
        $('#FormQuotationDetail #hdnTaxTypeCode').val(quotationDetail.TaxType.ValueText);
        $('#FormQuotationDetail #hdnCGSTPerc').val(quotationDetail.CGSTPerc);
        $('#FormQuotationDetail #hdnSGSTPerc').val(quotationDetail.SGSTPerc);
        $('#FormQuotationDetail #hdnIGSTPerc').val(quotationDetail.IGSTPerc);
        var TaxableAmt = ((parseFloat(quotationDetail.Rate) * parseInt(quotationDetail.Qty)) - parseFloat(quotationDetail.Discount))
        var CGSTAmt = (TaxableAmt * parseFloat(quotationDetail.CGSTPerc)) / 100;
        var SGSTAmt = (TaxableAmt * parseFloat(quotationDetail.SGSTPerc)) / 100;
        var IGSTAmt = (TaxableAmt * parseFloat(quotationDetail.IGSTPerc)) / 100;
        $('#FormQuotationDetail #CGSTPerc').val(CGSTAmt);
        $('#FormQuotationDetail #SGSTPerc').val(SGSTAmt);
        $('#FormQuotationDetail #IGSTPerc').val(IGSTAmt);
        $('#divModelPopQuotation').modal('show');
        var editor = new wysihtml5.Editor("ProductSpec", {
            toolbar: "toolbar",
            //stylesheets: "css/stylesheet.css",
            parserRules: wysihtml5ParserRules
        });
    });
}
function ConfirmDeleteQuotationDetail(this_Obj) {
    debugger;
    _datatablerowindex = _dataTable.QuotationDetailList.row($(this_Obj).parents('tr')).index();
    var quotationDetail = _dataTable.QuotationDetailList.row($(this_Obj).parents('tr')).data();
    if (quotationDetail.ID === _emptyGuid) {
        notyConfirm('Are you sure to delete?', 'DeleteCurrentQuotationDetail("' + _datatablerowindex + '")');
    }
    else {
        notyConfirm('Are you sure to delete?', 'DeleteQuotationDetail("' + quotationDetail.ID + '")');

    }
}
function DeleteCurrentQuotationDetail(_datatablerowindex)
{
    var quotationDetailList = _dataTable.QuotationDetailList.rows().data();
    quotationDetailList.splice(_datatablerowindex, 1);
    ClearCalculatedFields();
    _dataTable.QuotationDetailList.clear().rows.add(quotationDetailList).draw(false);
    CalculateTotal();
    notyAlert('success', 'Detail Row deleted successfully');
}
function DeleteQuotationDetail(ID) {
    if (ID != _emptyGuid && ID != null && ID != '') {
        var data = { "id": ID };
        var ds = {};
        _jsonData = GetDataFromServer("Quotation/DeleteQuotationDetail/", data);
        if (_jsonData != '') {
            _jsonData = JSON.parse(_jsonData);
            _message = _jsonData.Message;
            _status = _jsonData.Status;
            _result = _jsonData.Record;
        }
        if (_status == "OK") {
            notyAlert('success', _result.Message);
            var quotationDetailList = _dataTable.QuotationDetailList.rows().data();
            quotationDetailList.splice(_datatablerowindex, 1);
            ClearCalculatedFields();
            _dataTable.QuotationDetailList.clear().rows.add(quotationDetailList).draw(false);
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
function ClearCalculatedFields() {
    $('#lblTaxTotal').text('0.00');
    $('#lblItemTotal').text('0.00');
    $('#lblGrossAmount').text('0.00');
    $('#lblGrandTotal').text('0.00');
    $('#lblOtherChargeAmount').text('0.00');
}
//=========================================================================================================================
//Email Quotation
//
function EmailQuotation() {
    debugger;
    $("#divModelEmailQuotationBody").load("Quotation/EmailQuotation?ID=" + $('#QuotationForm #ID').val() + "&EmailFlag=True&ImageCheck=True", function () {
        $('#lblModelEmailQuotation').text('Email Quotation');
        //var mailheader = $('#MailBodyHeader').val();
        //mailheader = mailheader.replace(/\n/g, "<br />");
        //$('#lblmailheader').html(mailheader);
        //var mailfooter = $('#MailBodyFooter').val();
        //mailfooter = mailfooter.replace(/\n/g, "<br />");
        //$('#lblmailfooter').html(mailfooter);
        $('#divModelEmailQuotation').modal('show');
    });
}
function PrintQuotation() {
    debugger;
    $("#divModelPrintQuotationBody").load("Quotation/PrintQuotation?ID=" + $('#QuotationForm #ID').val(), function () {
        $('#lblModelPrintQuotation').text('Print Quotation');
        //var mailheader = $('#MailBodyHeader').val();
        //mailheader = mailheader.replace(/\n/g, "<br />");
        //$('#lblmailheader').html(mailheader);
        //var mailfooter = $('#MailBodyFooter').val();
        //mailfooter = mailfooter.replace(/\n/g, "<br />");
        //$('#lblmailfooter').html(mailfooter);
        $('#divModelPrintQuotation').modal('show');
    });
}
function SendQuotationEmail() {
    debugger;
    if ($('#hdnEmailSentTo').val() != null && $('#hdnEmailSentTo').val() != "" && $('#Subject').val() != null) {
        var bodyContent = $('#divQuotationEmailcontainer').html();
        //var headerContent = $('#hdnHeadContent').html();
        $('#hdnContentEmail').val(bodyContent);
        $('#hdnQuotationEMailContent').val($('#divQuotationEmailcontainer').html());
        $('#hdnQuoteNo').val($('#QuoteNo').val());
        $('#hdnContactPerson').val($('#ContactPerson').text());
        $('#hdnQuoteDate').val($('#QuoteDateFormatted').val());
        $('#FormQuotationEmailSend #ID').val($('#QuotationForm #ID').val());
        $('#FormQuotationEmailSend').submit();
    }
    else {
        if ($('#EmailSentTo').val() == null)
        {
            $('#sentTolbl').css('color', 'red'); 
            $("#sentTolbl").attr("title", "Please specify at least one recipient");
            $('#sentTovalidationmsglbl').html('__________________________________');
            $('#sentTovalidationmsglbl').css('color', 'red');
            $("#sentTovalidationmsglbl").attr("title", "Please specify at least one recipient");
        }
        //if ($('#Subject').val() == null) {
        //    $('#subjectlbl').css('color', 'red');
        //}
    }
}
function UpdateQuotationEmailInfo() {
    debugger;
    //$('#hdnMailBodyHeader').val($('#MailBodyHeader').val());
    //$('#hdnMailBodyFooter').val($('#MailBodyFooter').val());
    //$('#hdnEmailSentTo').val($('#EmailSentTo').val());  
    //if ($('#imagecheck').prop("checked")) {
    //    alert('1')
    //}
    //else {
    //    alert('2')
    //}
    $('#imagecheck').prop("checked")
    $('#FormUpdateQuotationEmailInfo #ID').val($('#QuotationForm #ID').val());
}

//function ChangeValueCheckBox(element) {
//    debugger;
//    if (element.checked) {
//        element.value = true;
//        $('#FormUpdateQuotationEmailInfo #imagecheck').val($('#QuotationForm #imagecheck').val());
//    }
//    else {
//        element.value = false;
//        $('#FormUpdateQuotationEmailInfo #imagecheck').val($('#QuotationForm #imagecheck').val());
//    }
//}

function DownloadQuotation() {
    debugger;
    var bodyContent = $('#divQuotationEmailcontainer').html();
    var headerContent = $('#hdnHeadContent').html();
    $('#hdnContent').val(bodyContent);
    $('#hdnHeadContent').val(headerContent);
    //var customerName = $("#QuotationForm #CustomerID option:selected").text();
    //$('#hdnCustomerName').val(customerName);   
}

function SaveSuccessUpdateQuotationEmailInfo(data, status) {
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
                $("#divModelEmailQuotationBody").load("Quotation/EmailQuotation?ID=" + $('#QuotationForm #ID').val() + "&EmailFlag=False" + "&ImageCheck=" + $('#imagecheck').prop("checked"), function () {
                    $('#lblModelEmailQuotation').text('Email Attachment')
                });
                break;
            case "ERROR":
                //MasterAlert("success", _message)
                $('#divModelEmailQuotation').modal('hide');
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
function SaveSuccessQuotationEmailSend(data, status) {
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
                $('#divModelEmailQuotation').modal('hide');
                ResetQuotation();
                break;
            case "ERROR":
                MasterAlert("danger", _message)
                $('#divModelEmailQuotation').modal('hide');
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
        debugger;
    if ($('#LatestApprovalStatus').val() == 3) {
        var documentID = $('#QuotationForm #ID').val();
        var latestApprovalID = $('#LatestApprovalID').val();
        ReSendDocForApproval(documentID, documentTypeCode, latestApprovalID);
        //SendForApproval('QUO')
        //BindPurchaseOrder($('#ID').val());
        ResetQuotation();
        
    }
    else {
        $('#SendApprovalModal').modal('show');
    }
    });
}



function SendForApproval(documentTypeCode) {
    debugger;
    
    var documentID = $('#QuotationForm #ID').val();
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
        ResetQuotation();
        //BindPurchaseOrder($('#ID').val());
        
}
function RecallDocumentItem(documentTypeCode) {
    debugger;
    notyConfirmRecall('Are you sure to recall document?', 'RecallDoc("' + documentTypeCode + '")');
}
function RecallDoc(documentTypeCode) {
    debugger;    
    try{
        if (documentTypeCode) { 
            $('#QuotationForm').load("DocumentApproval/RecallDocument?documentID=" + $('#QuotationForm #ID').val() + "&documentTypeCode=" + "QUO" + "&documentNo=" + $('#QuotationForm #QuoteNo').val(), function () {
            });
            var data = { "documentID": $('#QuotationForm #ID').val() };
            _jsonData = GetDataFromServer("DocumentApproval/RecallDocument/", data);
            if (_jsonData != '') {
                _jsonData = JSON.parse(_jsonData);
                _message = _jsonData.Message;
                _status = _jsonData.Status;
                _result = _jsonData.Record;
            }
            switch (_status) {
                case "OK":
                    notyAlert('success', _message);
                    ResetQuotation();
                    debugger;
                    $("#SendApprovalModalBody").load("DocumentApproval/GetApprovers?documentTypeCode=QUO", function () {
                        if (($('#ApproverLevel').val()) > 1) {
                            ChangeButtonPatchView("Quotation", "btnPatchQuotationNew", "Approved", Quotation.ID);
                        }
                        else {
                            ChangeButtonPatchView("Quotation", "btnPatchQuotationNew", "Recalled", Quotation.ID);
                        }
                    });
                    break;
                case "ERROR":
                    notyAlert('error', _message);
                    ResetQuotation();
                    break;
                default:
                    break;
            }
        }
        
    }
    catch (e) {
        console.log(e.message);
    }
}
//OtherExpense------------------
function AddOtherExpenseDetailList() {
    $("#divModelQuotationPopBody").load("Quotation/QuotationOtherChargeDetail", function () {
        $('#lblModelPopQuotation').text('OtherExpense Detail')
        $('#divModelPopQuotation').modal('show');
    });
}
function AddOtherExpenseDetailToList() {
    debugger;
    $("#FormOtherExpenseDetail").submit(function () { });
    debugger;
    if ($('#FormOtherExpenseDetail #IsUpdate').val() == 'True') {
        if (($('#divModelQuotationPopBody #OtherChargeCode').val() != "") && ($('#divModelQuotationPopBody #ChargeAmount').val() != "")) {
            debugger;
            var quotationOtherExpenseDetailList = _dataTable.QuotationOtherChargesDetailList.rows().data();
            quotationOtherExpenseDetailList[_datatablerowindex].OtherCharge.Description = $("#divModelQuotationPopBody #OtherChargeCode").val() != "" ? $("#divModelQuotationPopBody #OtherChargeCode option:selected").text().split("-")[0].trim() : "";
            quotationOtherExpenseDetailList[_datatablerowindex].OtherCharge.SACCode = $("#hdnOtherChargeSACCode").val();
            quotationOtherExpenseDetailList[_datatablerowindex].ChargeAmount = $("#divModelQuotationPopBody #ChargeAmount").val();
            quotationOtherExpenseDetailList[_datatablerowindex].OtherChargeCode = $("#divModelQuotationPopBody #OtherChargeCode").val() != "" ? $("#divModelQuotationPopBody #OtherChargeCode").val() : _emptyGuid;
            TaxType = new Object;
            if ($('#divModelQuotationPopBody #TaxTypeCode').val() != null) {
                quotationOtherExpenseDetailList[_datatablerowindex].TaxTypeCode = $('#divModelQuotationPopBody #TaxTypeCode').val().split('|')[0];
                TaxType.ValueText = $('#divModelQuotationPopBody #TaxTypeCode').val();
            }
            quotationOtherExpenseDetailList[_datatablerowindex].TaxType = TaxType;
            quotationOtherExpenseDetailList[_datatablerowindex].CGSTPerc = $('#divModelQuotationPopBody #hdnCGSTPerc').val();
            quotationOtherExpenseDetailList[_datatablerowindex].SGSTPerc = $('#divModelQuotationPopBody #hdnSGSTPerc').val();
            quotationOtherExpenseDetailList[_datatablerowindex].IGSTPerc = $('#divModelQuotationPopBody #hdnIGSTPerc').val();
            ClearCalculatedFields();
            _dataTable.QuotationOtherChargesDetailList.clear().rows.add(quotationOtherExpenseDetailList).draw(false);
            CalculateTotal();
            $('#divModelPopQuotation').modal('hide');
            _datatablerowindex = -1;
        }
    }
    else {
        if (($('#divModelQuotationPopBody #OtherChargeCode').val() != "") && ($('#divModelQuotationPopBody #ChargeAmount').val() != "")) {
            debugger;
            if (_dataTable.QuotationOtherChargesDetailList.rows().data().length === 0) {
                _dataTable.QuotationOtherChargesDetailList.clear().rows.add(GetQuotationOtherChargesDetailListByQuotationID(_emptyGuid, false)).draw(false);
                debugger;
                var quotationOtherExpenseDetailList = _dataTable.QuotationOtherChargesDetailList.rows().data();
                quotationOtherExpenseDetailList[0].OtherCharge.Description = $("#divModelQuotationPopBody #OtherChargeCode").val() != "" ? $("#divModelQuotationPopBody #OtherChargeCode option:selected").text().split("-")[0].trim() : "";
                quotationOtherExpenseDetailList[0].OtherChargeCode = $("#divModelQuotationPopBody #OtherChargeCode").val() != "" ? $("#divModelQuotationPopBody #OtherChargeCode").val() : _emptyGuid;
                quotationOtherExpenseDetailList[0].OtherCharge.SACCode = $("#hdnOtherChargeSACCode").val();
                quotationOtherExpenseDetailList[0].ChargeAmount = $("#divModelQuotationPopBody #ChargeAmount").val();
                if ($('#divModelQuotationPopBody #TaxTypeCode').val() != null) {
                    quotationOtherExpenseDetailList[0].TaxTypeCode = $('#divModelQuotationPopBody #TaxTypeCode').val().split('|')[0];
                }
                quotationOtherExpenseDetailList[0].TaxType.ValueText = $('#divModelQuotationPopBody #TaxTypeCode').val();
                quotationOtherExpenseDetailList[0].CGSTPerc = $('#divModelQuotationPopBody #hdnCGSTPerc').val();
                quotationOtherExpenseDetailList[0].SGSTPerc = $('#divModelQuotationPopBody #hdnSGSTPerc').val();
                quotationOtherExpenseDetailList[0].IGSTPerc = $('#divModelQuotationPopBody #hdnIGSTPerc').val();
                ClearCalculatedFields();
                _dataTable.QuotationOtherChargesDetailList.clear().rows.add(quotationOtherExpenseDetailList).draw(false);
                CalculateTotal();
                $('#divModelPopQuotation').modal('hide');
            }
            else {
                debugger;
                var quotationOtherExpenseDetailList = _dataTable.QuotationOtherChargesDetailList.rows().data();
                if (quotationOtherExpenseDetailList.length > 0) {
                    var checkpoint = 0;
                    var otherCharge = $('#OtherChargeCode').val();
                    for (var i = 0; i < quotationOtherExpenseDetailList.length; i++) {
                        if ((quotationOtherExpenseDetailList[i].OtherChargeCode == otherCharge)) {
                            quotationOtherExpenseDetailList[i].ChargeAmount = parseFloat(quotationOtherExpenseDetailList[i].ChargeAmount) + parseFloat($('#ChargeAmount').val());
                            checkpoint = 1;
                            break;
                        }
                    }
                    if (checkpoint == 1) {
                        debugger;
                        ClearCalculatedFields();
                        _dataTable.QuotationOtherChargesDetailList.clear().rows.add(quotationOtherExpenseDetailList).draw(false);
                        CalculateTotal();
                        $('#divModelPopQuotation').modal('hide');
                    }
                    else if (checkpoint == 0) {
                        ClearCalculatedFields();
                        var QuotationOtherChargesDetailVM = new Object();
                        QuotationOtherChargesDetailVM.ID = _emptyGuid;
                        var OtherCharge = new Object;
                        OtherCharge.Description = $("#divModelQuotationPopBody #OtherChargeCode").val() != "" ? $("#divModelQuotationPopBody #OtherChargeCode option:selected").text().split("-")[0].trim() : "";
                        QuotationOtherChargesDetailVM.OtherCharge = OtherCharge;
                        QuotationOtherChargesDetailVM.OtherChargeCode = $("#divModelQuotationPopBody #OtherChargeCode").val() != "" ? $("#divModelQuotationPopBody #OtherChargeCode").val() : _emptyGuid;
                        QuotationOtherChargesDetailVM.OtherCharge.SACCode = $("#hdnOtherChargeSACCode").val();
                        QuotationOtherChargesDetailVM.ChargeAmount = $("#divModelQuotationPopBody #ChargeAmount").val();
                        var TaxType = new Object();
                        if ($('#divModelQuotationPopBody #TaxTypeCode').val() != null) {
                            QuotationOtherChargesDetailVM.TaxTypeCode = $('#divModelQuotationPopBody #TaxTypeCode').val().split('|')[0];
                            TaxType.ValueText = $('#divModelQuotationPopBody #TaxTypeCode').val();
                        }
                        QuotationOtherChargesDetailVM.TaxType = TaxType;
                        QuotationOtherChargesDetailVM.CGSTPerc = $('#divModelQuotationPopBody #hdnCGSTPerc').val();
                        QuotationOtherChargesDetailVM.SGSTPerc = $('#divModelQuotationPopBody #hdnSGSTPerc').val();
                        QuotationOtherChargesDetailVM.IGSTPerc = $('#divModelQuotationPopBody #hdnIGSTPerc').val();
                        _dataTable.QuotationOtherChargesDetailList.row.add(QuotationOtherChargesDetailVM).draw(true);
                        CalculateTotal();
                        $('#divModelPopQuotation').modal('hide');
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

function GetQuotationOtherChargesDetailListByQuotationID(id) {
    try {
        debugger;

        var quotationOtherChargesDetailList = [];
        
        var data = { "quotationID": id };
        _jsonData = GetDataFromServer("Quotation/GetQuotationOtherChargesDetailListByQuotationID/", data);
        

        if (_jsonData != '') {
            _jsonData = JSON.parse(_jsonData);
            _message = _jsonData.Message;
            _status = _jsonData.Status;
            quotationOtherChargesDetailList = _jsonData.Records;
        }
        if (_status == "OK") {
            return quotationOtherChargesDetailList;
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

function EditQuotationOtherChargesDetail(this_Obj) {
    debugger;
    _datatablerowindex = _dataTable.QuotationOtherChargesDetailList.row($(this_Obj).parents('tr')).index();
    var quotationOtherChargesDetail = _dataTable.QuotationOtherChargesDetailList.row($(this_Obj).parents('tr')).data();
    $("#divModelQuotationPopBody").load("Quotation/QuotationOtherChargeDetail", function () {
        debugger;
        $('#lblModelPopQuotation').text('OtherCharges Detail')
        $('#FormOtherExpenseDetail #IsUpdate').val('True');
        $('#FormOtherExpenseDetail #ID').val(quotationOtherChargesDetail.ID);
        $("#FormOtherExpenseDetail #OtherChargeCode").val(quotationOtherChargesDetail.OtherChargeCode);
        $("#FormOtherExpenseDetail #hdnOtherChargeCode").val(quotationOtherChargesDetail.OtherChargeCode);
        $("#FormOtherExpenseDetail #ChargeAmount").val(quotationOtherChargesDetail.ChargeAmount);
        
        $('#FormOtherExpenseDetail #TaxTypeCode').val(quotationOtherChargesDetail.TaxType.ValueText);
        $('#FormOtherExpenseDetail #hdnTaxTypeCode').val(quotationOtherChargesDetail.TaxType.ValueText);
        $('#FormOtherExpenseDetail #hdnCGSTPerc').val(quotationOtherChargesDetail.CGSTPerc);
        $('#FormOtherExpenseDetail #hdnSGSTPerc').val(quotationOtherChargesDetail.SGSTPerc);
        $('#FormOtherExpenseDetail #hdnIGSTPerc').val(quotationOtherChargesDetail.IGSTPerc);
        
        var CGSTAmt = (quotationOtherChargesDetail.ChargeAmount * parseFloat(quotationOtherChargesDetail.CGSTPerc)) / 100;
        var SGSTAmt = (quotationOtherChargesDetail.ChargeAmount * parseFloat(quotationOtherChargesDetail.SGSTPerc)) / 100;
        var IGSTAmt = (quotationOtherChargesDetail.ChargeAmount * parseFloat(quotationOtherChargesDetail.IGSTPerc)) / 100;
        $('#FormOtherExpenseDetail #CGSTPerc').val(CGSTAmt);
        $('#FormOtherExpenseDetail #SGSTPerc').val(SGSTAmt);
        $('#FormOtherExpenseDetail #IGSTPerc').val(IGSTAmt);
        $('#divModelPopQuotation').modal('show');
    });
}
function ConfirmDeleteQuotationOtherChargeDetail(this_Obj) {
    debugger;
    _datatablerowindex = _dataTable.QuotationOtherChargesDetailList.row($(this_Obj).parents('tr')).index();
    var quotationOtherChargeDetail = _dataTable.QuotationOtherChargesDetailList.row($(this_Obj).parents('tr')).data();
    if (quotationOtherChargeDetail.ID === _emptyGuid) {
        notyConfirm('Are you sure to delete?', 'DeleteCurrentQuotationOtherChargeDetail("' + _datatablerowindex + '")');
    }
    else {
        notyConfirm('Are you sure to delete?', 'DeleteQuotationOtherChargeDetail("' + quotationOtherChargeDetail.ID + '")');

    }
}
function DeleteCurrentQuotationOtherChargeDetail(_datatablerowindex)
{
    var quotationOtherChargeDetailList = _dataTable.QuotationOtherChargesDetailList.rows().data();
    quotationOtherChargeDetailList.splice(_datatablerowindex, 1);
    ClearCalculatedFields();
    _dataTable.QuotationOtherChargesDetailList.clear().rows.add(quotationOtherChargeDetailList).draw(false);
    CalculateTotal();
    notyAlert('success', 'Detail Row deleted successfully');
}
function DeleteQuotationOtherChargeDetail(ID) {
    if (ID != _emptyGuid && ID != null && ID != '') {
        var data = { "id": ID };
        var ds = {};
        _jsonData = GetDataFromServer("Quotation/DeleteQuotationOtherChargeDetail/", data);
        if (_jsonData != '') {
            _jsonData = JSON.parse(_jsonData);
            _message = _jsonData.Message;
            _status = _jsonData.Status;
            _result = _jsonData.Record;
        }
        if (_status == "OK") {
            notyAlert('success', _result.Message);
            var quotationOtherChargeDetailList = _dataTable.QuotationOtherChargesDetailList.rows().data();
            quotationOtherChargeDetailList.splice(_datatablerowindex, 1);
            ClearCalculatedFields();
            _dataTable.QuotationOtherChargesDetailList.clear().rows.add(quotationOtherChargeDetailList).draw(false);
            CalculateTotal();
        }
        if (_status == "ERROR") {
            notyAlert('error', _message);
        }
    }
}
function CalculateTotal()
{
    var TaxTotal = 0.00, TaxableTotal = 0.00, GrossAmount = 0.00, GrandTotal = 0.00, OtherChargeAmt=0.00;
    var quotationDetail= _dataTable.QuotationDetailList.rows().data();
    var quotationOtherChargeDetail = _dataTable.QuotationOtherChargesDetailList.rows().data();
    for (var i = 0; i < quotationDetail.length; i++) {
        var TaxableAmt = (parseFloat(quotationDetail[i].Rate != "" ? quotationDetail[i].Rate : 0) * parseInt(quotationDetail[i].Qty != "" ? quotationDetail[i].Qty : 1)) - parseFloat(quotationDetail[i].Discount != "" ? quotationDetail[i].Discount : 0)
        var CGST = parseFloat(quotationDetail[i].CGSTPerc != "" ? quotationDetail[i].CGSTPerc : 0);
        var SGST = parseFloat(quotationDetail[i].SGSTPerc != "" ? quotationDetail[i].SGSTPerc : 0);
        var IGST = parseFloat(quotationDetail[i].IGSTPerc != "" ? quotationDetail[i].IGSTPerc : 0);
        var CGSTAmt = parseFloat(TaxableAmt * CGST / 100);
        var SGSTAmt = parseFloat(TaxableAmt * SGST / 100);
        var IGSTAmt = parseFloat(TaxableAmt * IGST / 100);
        var GSTAmt = parseFloat(CGSTAmt) + parseFloat(SGSTAmt) + parseFloat(IGSTAmt)
        var GrossTotalAmt = TaxableAmt + GSTAmt
        TaxTotal = roundoff(parseFloat(TaxTotal) + parseFloat(GSTAmt))
        TaxableTotal = roundoff(parseFloat(TaxableTotal) + parseFloat(TaxableAmt))
        GrossAmount = roundoff(parseFloat(GrossAmount) + GrossTotalAmt)
    }
    for (var i = 0; i < quotationOtherChargeDetail.length; i++) {
        var CGST = parseFloat(quotationOtherChargeDetail[i].CGSTPerc != "" ? quotationOtherChargeDetail[i].CGSTPerc : 0);
        var SGST = parseFloat(quotationOtherChargeDetail[i].SGSTPerc != "" ? quotationOtherChargeDetail[i].SGSTPerc : 0);
        var IGST = parseFloat(quotationOtherChargeDetail[i].IGSTPerc != "" ? quotationOtherChargeDetail[i].IGSTPerc : 0);
        var CGSTAmt = parseFloat(quotationOtherChargeDetail[i].ChargeAmount * CGST / 100);
        var SGSTAmt = parseFloat(quotationOtherChargeDetail[i].ChargeAmount * SGST / 100)
        var IGSTAmt = parseFloat(quotationOtherChargeDetail[i].ChargeAmount * IGST / 100)
        var GSTAmt = roundoff(parseFloat(CGSTAmt) + parseFloat(SGSTAmt) + parseFloat(IGSTAmt))
        var Total = roundoff(parseFloat(quotationOtherChargeDetail[i].ChargeAmount) + parseFloat(GSTAmt))
        OtherChargeAmt = roundoff(parseFloat(OtherChargeAmt) + parseFloat(Total))
    }
    GrossAmount = roundoff(parseFloat(GrossAmount) + parseFloat(OtherChargeAmt))
    $('#lblTaxTotal').text(roundoff(TaxTotal));
    $('#lblItemTotal').text(roundoff(TaxableTotal));
    $('#lblGrossAmount').text(GrossAmount);
    $('#lblGrandTotal').text(GrossAmount);
    $('#lblOtherChargeAmount').text(roundoff(OtherChargeAmt));
    $('#Discount').trigger('onchange');
}
function EditRedirectToDocument(id) {
    debugger;
    OnServerCallBegin();

    //this will return form body(html)
    $("#divQuotationForm").load("Quotation/QuotationForm?id=" + id, function (responseTxt, statusTxt, xhr) {
        if (statusTxt == "success") {
            OnServerCallComplete();
            openNav();
            $('#lblQuotationInfo').text($('#QuoteNo').val());
            if ($('#IsDocLocked').val() == "True") {

                debugger;
                switch ($('#LatestApprovalStatus').val()) {
                    case "0":
                        ChangeButtonPatchView("Quotation", "btnPatchQuotationNew", "Draft", id);
                        break;
                    case "1":
                        if ($('#ApproverLevel').val() > 1) {
                            ChangeButtonPatchView("Quotation", "btnPatchQuotationNew", "Approved", id);
                        }
                        else {
                            ChangeButtonPatchView("Quotation", "btnPatchQuotationNew", "Recalled", id);
                        }
                        break;
                    case "3":
                        ChangeButtonPatchView("Quotation", "btnPatchQuotationNew", "Edit", id);
                        break;
                    case "4":
                        ChangeButtonPatchView("Quotation", "btnPatchQuotationNew", "Approved", id);
                        break;
                    default:
                        ChangeButtonPatchView("Quotation", "btnPatchQuotationNew", "LockDocument", id);
                        break;
                }
            }
            else {
                ChangeButtonPatchView("Quotation", "btnPatchQuotationNew", "LockDocument", id);
            }
            BindQuotationDetailList(id);
            BindQuotationOtherChargesDetailList(id);
            CalculateTotal();
            $('#divCustomerBasicInfo').load("Customer/CustomerBasicInfo?ID=" + $('#hdnCustomerID').val());
            clearUploadControl();
            PaintImages(id);
            $("#divQuotationForm #EstimateID").prop('disabled', true);

        }
        else {
            console.log("Error: " + xhr.status + ": " + xhr.statusText);
        }
    });
}

//To get SACCode
function GetOtherCharge(value)
{
    try{
        debugger;
        var otherCharge;
        var data = { "code": value };
        _jsonData = GetDataFromServer("OtherCharge/GetOtherCharge/",data);
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

