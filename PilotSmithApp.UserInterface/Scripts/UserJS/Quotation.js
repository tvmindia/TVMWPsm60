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
            EditQuotation(this);
        });
    }
    catch (e) {
        console.log(e.message);
    }
});
//function bind the Quotation list checking search and filter
function BindOrReloadQuotationTable(action) {
    try {
        //creating advancesearch object
        QuotationAdvanceSearchViewModel = new Object();
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
        QuotationAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
        QuotationAdvanceSearchViewModel.SearchTerm = $('#SearchTerm').val();
        QuotationAdvanceSearchViewModel.FromDate = $('#FromDate').val();
        QuotationAdvanceSearchViewModel.ToDate = $('#ToDate').val();
        //apply datatable plugin on Quotation table
        _dataTable.QuotationList = $('#tblQuotation').DataTable(
        {
            dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
            buttons: [{
                extend: 'excel',
                exportOptions:
                             {
                                 columns: [0, 1, 2, 3, 4, 5, 6, 7]
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
                url: "Quotation/GetAllQuotation/",
                data: { "QuotationAdvanceSearchVM": QuotationAdvanceSearchViewModel },
                type: 'POST'
            },
            pageLength: 13,
            columns: [
               { "data": "QuoteNo", "defaultContent": "<i>-</i>" },
               { "data": "QuoteDateFormatted", "defaultContent": "<i>-</i>" },
               { "data": "Customer.CompanyName", "defaultContent": "<i>-</i>" },
               { "data": "Customer.ContactPerson", "defaultContent": "<i>-</i>" },
               { "data": "Customer.Mobile", "defaultContent": "<i>-</i>" },
               { "data": "RequirementSpec", "defaultContent": "<i>-</i>" },
               { "data": "ReferencePerson.Name", "defaultContent": "<i>-</i>" },
               { "data": "DocumentStatus.Description", "defaultContent": "<i>-</i>" },
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
               { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink"  onclick="EditQuotation(this)" ><i class="fa fa-pencil-square-o" aria-hidden="true"></i></a>' },
            ],
            columnDefs: [{ className: "text-right", "targets": [] },
                          { className: "text-left", "targets": [2, 3, 5, 6] },
                          { className: "text-center", "targets": [0, 1, 4, 7, 8] },
                            { "targets": [0, 1, 4], "width": "10%" },
                            { "targets": [2, 3], "width": "10%" },
                            { "targets": [5], "width": "30%" },
                            { "targets": [6], "width": "10%" },
                            { "targets": [7, 8], "width": "5%" },
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
                if (action === 'Export') {
                    if (json.data.length > 0) {
                        if (json.data[0].TotalCount > 1000) {
                            setTimeout(function () {
                                MasterAlert("info", 'We are able to download maximum 1000 rows of data, There exist more than 1000 rows of data please filter and download')
                            }, 10000)
                        }
                    }
                    $(".buttons-excel").trigger('click');
                    BindOrReloadQuotationTable();
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
function ResetQuotationList() {
    $(".searchicon").removeClass('filterApplied');
    BindOrReloadQuotationTable('Reset');
}
//function export data to excel
function ExportQuotationData() {
    $('.excelExport').show();
    OnServerCallBegin();
    BindOrReloadQuotationTable('Export');
}
// add Quotation section
function AddQuotation() {
    //this will return form body(html)
    OnServerCallBegin();
    $("#divQuotationForm").load("Quotation/QuotationForm?id=" + _emptyGuid + "&estimateID=", function () {
        ChangeButtonPatchView("Quotation", "btnPatchQuotationNew", "Add");
        BindQuotationDetailList(_emptyGuid);
        BindQuotationOtherChargesDetailList(_emptyGuid);
        OnServerCallComplete();
        setTimeout(function () {
            //resides in customjs for sliding
            openNav();
        }, 100);
    });
}
function EditQuotation(this_Obj) {
    debugger;
    OnServerCallBegin();
    var Quotation = _dataTable.QuotationList.row($(this_Obj).parents('tr')).data();
    //this will return form body(html)
    $("#divQuotationForm").load("Quotation/QuotationForm?id=" + Quotation.ID + "&estimateID=" + Quotation.EstimateID, function () {
        //$('#CustomerID').trigger('change');
        if ($('#IsDocLocked').val() == "True") {
            ChangeButtonPatchView("Quotation", "btnPatchQuotationNew", "Edit", Quotation.ID);
        }
        else {
            ChangeButtonPatchView("Quotation", "btnPatchQuotationNew", "LockDocument");
        }
        BindQuotationDetailList(Quotation.ID);
        $('#divCustomerBasicInfo').load("Customer/CustomerBasicInfo?ID=" + $('#hdnCustomerID').val());
        clearUploadControl();
        PaintImages(Quotation.ID);
        OnServerCallComplete();
        setTimeout(function () {
            $("#divQuotationForm #EstimateID").prop('disabled', true);
            //resides in customjs for sliding
            openNav();
        }, 100);
    });
}
function ResetQuotation() {
    $("#divQuotationForm").load("Quotation/QuotationForm?id=" + $('#QuotationForm #ID').val() + "&estimateID=" + $('#hdnEstimateID').val(), function () {
        if ($('#ID').val() != _emptyGuid && $('#ID').val() != null)
        {
            setTimeout(function () {
                $("#divQuotationForm #EstimateID").prop('disabled', true);
                //resides in customjs for sliding
                openNav();
            }, 100);
        }
        BindQuotationDetailList($('#ID').val(),false);
        clearUploadControl();
        PaintImages($('#QuotationForm #ID').val());
        $('#divCustomerBasicInfo').load("Customer/CustomerBasicInfo?ID=" + $('#QuotationForm #hdnCustomerID').val());
    });
}
function SaveQuotation() {
    var quotationDetailList = _dataTable.QuotationDetailList.rows().data().toArray();
    $('#DetailJSON').val(JSON.stringify(quotationDetailList));
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
                    ChangeButtonPatchView("Quotation", "btnPatchQuotationNew", "Edit");
                    BindQuotationDetailList(_result.ID);
                    clearUploadControl();
                    PaintImages(_result.ID);
                    $('#divCustomerBasicInfo').load("Customer/CustomerBasicInfo?ID=" + $('#QuotationForm #hdnCustomerID').val());
                });
                ChangeButtonPatchView("Quotation", "btnPatchQuotationNew", "Edit");
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
                     return '<div class="show-popover text-right" data-html="true" data-toggle="popover" data-title="<p align=left>Total GST : ₹ ' + GSTAmt + '" data-content=" SGST ' + SGST + '% : ₹ ' + roundoff(parseFloat(SGSTAmt)) + '<br/>CGST ' + CGST + '% : ₹ ' + roundoff(parseFloat(CGSTAmt)) + '<br/> IGST ' + IGST + '% : ₹ ' + roundoff(parseFloat(IGSTAmt)) + '</p>"/>' + GSTAmt
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
                     return Total
                     //return '<div class="show-popover text-right" data-html="true" data-toggle="popover" data-title="<p align=left>Grand Total : ₹ ' + GrandTotal + '" data-content="Taxable : ₹ ' + TaxableAmt + '<br/>GST : ₹ ' + GSTAmt + '</p>"/>' + GrandTotal
                 }, "defaultContent": "<i></i>"
             },
             { "data": null, "orderable": false, "defaultContent": '<a href="#" class="DeleteLink"  onclick="ConfirmDeleteQuotationDetail(this)" ><i class="fa fa-trash-o" aria-hidden="true"></i></a> <a href="#" class="actionLink"  onclick="EditQuotationDetail(this)" ><i class="fa fa-pencil-square-o" aria-hidden="true"></i></a>' },
             ],             
             columnDefs: [
                 { "targets": [0], "width": "30%" },
                 { "targets": [1, 2], "width": "20%" },
                 { "targets": [3], "width": "20%" },
                 { className: "text-right", "targets": [1, 2, 3] },
                 { className: "text-left", "targets": [0] },
                 { className: "text-center", "targets": [4] }
             ],
             destroy: true,
         });
}
function BindQuotationDetailList(id, IsEstimated) {
    debugger;
    ClearCalculatedFields();
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
                     return row.Product.Name + "<br/>" + '<div style="width:100%" class="show-popover" data-html="true" data-toggle="popover" data-title="<p align=left>Product Specification" data-content="' + row.ProductSpec.replace(/"/g, "&quot") + '</p>"/>' + row.ProductModel.Name
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
                 "data": "Rate", render: function (data, type, row) {
                     var CGST = parseFloat(row.CGSTPerc != "" ? row.CGSTPerc : 0);
                     var SGST = parseFloat(row.SGSTPerc != "" ? row.SGSTPerc : 0);
                     var IGST = parseFloat(row.IGSTPerc != "" ? row.IGSTPerc : 0);
                     var TaxableAmt = roundoff((parseFloat(row.Rate != "" ? row.Rate : 0) * parseInt(row.Qty != "" ? row.Qty : 1)) - parseFloat(row.Discount != "" ? row.Discount : 0))
                     var CGSTAmt = parseFloat(TaxableAmt * CGST / 100);
                     var SGSTAmt = parseFloat(TaxableAmt * SGST / 100)
                     var IGSTAmt = parseFloat(TaxableAmt * IGST / 100)
                     var GSTAmt = roundoff(CGSTAmt + SGSTAmt + IGSTAmt)
                     var GrandTotal = roundoff(((parseFloat(row.Rate != "" ? row.Rate : 0) * parseInt(row.Qty != "" ? row.Qty : 1)) - parseFloat(row.Discount != "" ? row.Discount : 0)) + parseFloat(GSTAmt))
                     return '<div class="show-popover text-right" data-html="true" data-toggle="popover" data-title="<p align=left>Grand Total : ₹ ' + GrandTotal + '" data-content="Taxable : ₹ ' + TaxableAmt + '<br/>GST : ₹ ' + GSTAmt + '</p>"/>' + GrandTotal
                 }, "defaultContent": "<i></i>"
             },
             { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink"  onclick="EditQuotationDetail(this)" ><i class="fa fa-pencil-square-o" aria-hidden="true"></i></a> <a href="#" class="DeleteLink"  onclick="ConfirmDeleteQuotationDetail(this)" ><i class="fa fa-trash-o" aria-hidden="true"></i></a>' },
             ],
             columnDefs: [
                 { "targets": [0], "width": "30%" },
                 { "targets": [1,2,3,4, 5, 6, 7], "width": "10%" },
                 { className: "text-right", "targets": [1,2,3,4, 5, 6] },
                 { className: "text-left", "targets": [3, 0] },
                 { className: "text-center", "targets": [7] }
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
                 var GrossTotalAmt = TaxableAmt + GSTAmt 
                 var TaxTotal = roundoff(parseFloat($('#lblTaxTotal').text()) + GSTAmt)
                 var TaxableTotal = roundoff(parseFloat($('#lblItemTotal').text()) + TaxableAmt)
                 var GrossAmount = roundoff(parseFloat($('#lblGrossAmount').text()) + GrossTotalAmt)
                 var GrandTotal = roundoff(parseFloat($('#lblGrandTotal').text()) + GrossTotalAmt)

                 $('#lblTaxTotal').text(TaxTotal);
                 $('#lblItemTotal').text(TaxableTotal);
                 $('#lblGrossAmount').text(GrossAmount);
                 $('#lblGrandTotal').text(GrandTotal);
             },
             initComplete: function (settings, json) {
                 $('#QuotationForm #Discount').trigger('change');
             },
             destroy:true
         });
    $('[data-toggle="popover"]').popover({
        html: true,
        'trigger': 'hover',
        'placement': 'top'
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
    $("#divModelQuotationPopBody").load("Quotation/AddQuotationDetail", function () {
        $('#lblModelPopQuotation').text('Quotation Detail')
        $('#divModelPopQuotation').modal('show');
    });
}
function AddQuotationDetailToList() {
    debugger;
    $("#FormQuotationDetail").submit(function () { });
        debugger;
        if ($('#FormQuotationDetail #IsUpdate').val() == 'True') {
            if (($('#divModelQuotationPopBody #ProductID').val() != "") && ($('#divModelQuotationPopBody #Rate').val() != "" )&& ($('#divModelQuotationPopBody #Qty').val() != "" )&& ($('#divModelQuotationPopBody #UnitCode').val() != "")) {
                debugger;
                var quotationDetailList = _dataTable.QuotationDetailList.rows().data();
                quotationDetailList[_datatablerowindex].Product.Code = $("#divModelQuotationPopBody #ProductID").val() != "" ? $("#divModelQuotationPopBody #ProductID option:selected").text().split("-")[0].trim() : "";
                quotationDetailList[_datatablerowindex].Product.Name = $("#divModelQuotationPopBody #ProductID").val() != "" ? $("#divModelQuotationPopBody #ProductID option:selected").text().split("-")[1].trim() : "";
                quotationDetailList[_datatablerowindex].ProductID = $("#divModelPopQuotation #ProductID").val() != "" ? $("#divModelPopQuotation #ProductID").val() : _emptyGuid;
                quotationDetailList[_datatablerowindex].ProductModelID = $("#divModelQuotationPopBody #ProductModelID").val() != "" ? $("#divModelQuotationPopBody #ProductModelID").val() : _emptyGuid;
                ProductModel = new Object;
                Unit = new Object;
                TaxType = new Object;
                ProductModel.Name = $("#divModelQuotationPopBody #ProductModelID").val() != "" ? $("#divModelQuotationPopBody #ProductModelID option:selected").text() : "";
                quotationDetailList[_datatablerowindex].ProductModel = ProductModel;
                quotationDetailList[_datatablerowindex].ProductSpec = $('#divModelQuotationPopBody #ProductSpec').val();
                quotationDetailList[_datatablerowindex].Qty = $('#divModelQuotationPopBody #Qty').val();
                quotationDetailList[_datatablerowindex].UnitCode = $('#divModelQuotationPopBody #UnitCode').val();
                Unit.Description = $("#divModelQuotationPopBody #UnitCode").val() != "" ? $("#divModelQuotationPopBody #UnitCode option:selected").text().trim() : "";
                quotationDetailList[_datatablerowindex].Unit = Unit;
                quotationDetailList[_datatablerowindex].Rate = $('#divModelQuotationPopBody #Rate').val();
                quotationDetailList[_datatablerowindex].Discount = $('#Discount').val() != "" ? $('#divModelQuotationPopBody #Discount').val() : 0;
                quotationDetailList[_datatablerowindex].TaxTypeCode = $('#divModelQuotationPopBody #TaxTypeCode').val().split('|')[0];
                TaxType.ValueText = $('#divModelQuotationPopBody #TaxTypeCode').val();
                quotationDetailList[_datatablerowindex].TaxType = TaxType;
                quotationDetailList[_datatablerowindex].CGSTPerc = $('#divModelQuotationPopBody #hdnCGSTPerc').val();
                quotationDetailList[_datatablerowindex].SGSTPerc = $('#divModelQuotationPopBody #hdnSGSTPerc').val();
                quotationDetailList[_datatablerowindex].IGSTPerc = $('#divModelQuotationPopBody #hdnIGSTPerc').val();
                ClearCalculatedFields();
                _dataTable.QuotationDetailList.clear().rows.add(quotationDetailList).draw(false);
                $('#divModelPopQuotation').modal('hide');
                _datatablerowindex = -1;
            }
        }
        else {
            if (($('#divModelQuotationPopBody #ProductID').val() != "") && ($('#divModelQuotationPopBody #Rate').val() != "") && ($('#divModelQuotationPopBody #Qty').val() != "") && ($('#divModelQuotationPopBody #UnitCode').val() != ""))
            {
                debugger;
                if (_dataTable.QuotationDetailList.rows().data().length === 0) {
                    _dataTable.QuotationDetailList.clear().rows.add(GetQuotationDetailListByQuotationID(_emptyGuid,false)).draw(false);
                    debugger;
                    var quotationDetailList = _dataTable.QuotationDetailList.rows().data();
                    quotationDetailList[0].Product.Code = $("#divModelQuotationPopBody #ProductID").val() != "" ? $("#divModelQuotationPopBody #ProductID option:selected").text().split("-")[0].trim() : "";
                    quotationDetailList[0].Product.Name = $("#divModelQuotationPopBody #ProductID").val() != "" ? $("#divModelQuotationPopBody #ProductID option:selected").text().split("-")[1].trim() : "";
                    quotationDetailList[0].ProductID = $("#divModelQuotationPopBody #ProductID").val() != "" ? $("#divModelQuotationPopBody #ProductID").val() : _emptyGuid;
                    quotationDetailList[0].ProductModelID = $("#divModelQuotationPopBody #ProductModelID").val() != "" ? $("#divModelQuotationPopBody #ProductModelID").val() : _emptyGuid;
                    quotationDetailList[0].ProductModel.Name = $("#divModelQuotationPopBody #ProductModelID").val() != "" ? $("#divModelQuotationPopBody #ProductModelID option:selected").text() : "";
                    quotationDetailList[0].ProductSpec = $('#divModelQuotationPopBody #ProductSpec').val();
                    quotationDetailList[0].Qty = $('#divModelQuotationPopBody #Qty').val();
                    quotationDetailList[0].UnitCode = $('#divModelQuotationPopBody #UnitCode').val();
                    quotationDetailList[0].Unit.Description = $("#divModelQuotationPopBody #UnitCode").val() != "" ? $("#divModelQuotationPopBody #UnitCode option:selected").text().trim() : "";
                    quotationDetailList[0].Rate = $('#divModelQuotationPopBody #Rate').val();
                    quotationDetailList[0].Discount = $('#divModelQuotationPopBody #Discount').val() != "" ? $('#divModelQuotationPopBody #Discount').val() : 0;
                    quotationDetailList[0].TaxTypeCode = $('#divModelQuotationPopBody #TaxTypeCode').val().split('|')[0];
                    quotationDetailList[0].TaxType.ValueText = $('#divModelQuotationPopBody #TaxTypeCode').val();
                    quotationDetailList[0].CGSTPerc = $('#divModelQuotationPopBody #hdnCGSTPerc').val();
                    quotationDetailList[0].SGSTPerc = $('#divModelQuotationPopBody #hdnSGSTPerc').val();
                    quotationDetailList[0].IGSTPerc = $('#divModelQuotationPopBody #hdnIGSTPerc').val();
                    ClearCalculatedFields();
                    _dataTable.QuotationDetailList.clear().rows.add(quotationDetailList).draw(false);
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
                            QuotationDetailVM.Product = Product;
                            QuotationDetailVM.ProductModelID = ($("#divModelQuotationPopBody #ProductModelID").val() != "" ? $("#divModelQuotationPopBody #ProductModelID").val() : _emptyGuid);
                            var ProductModel = new Object()
                            ProductModel.Name = ($("#ProductModelID").val() != "" ? $("#ProductModelID option:selected").text() : "");
                            QuotationDetailVM.ProductModel = ProductModel;
                            QuotationDetailVM.ProductSpec = $('#divModelQuotationPopBody #ProductSpec').val();
                            QuotationDetailVM.Qty = $('#divModelQuotationPopBody #Qty').val();
                            var Unit = new Object();
                            Unit.Description = $("#divModelQuotationPopBody #UnitCode").val() != "" ? $("#divModelQuotationPopBody #UnitCode option:selected").text().trim() : "";
                            QuotationDetailVM.Unit = Unit;
                            QuotationDetailVM.UnitCode = $('#divModelQuotationPopBody #UnitCode').val();
                            QuotationDetailVM.Rate = $('#divModelQuotationPopBody #Rate').val();
                            QuotationDetailVM.Discount = $('#divModelQuotationPopBody #Discount').val() != "" ? $('#divModelQuotationPopBody #Discount').val() : 0;
                            QuotationDetailVM.TaxTypeCode = $('#divModelQuotationPopBody #TaxTypeCode').val().split('|')[0];
                            var TaxType = new Object();
                            TaxType.ValueText = $('#divModelQuotationPopBody #TaxTypeCode').val();
                            QuotationDetailVM.TaxType = TaxType;
                            QuotationDetailVM.CGSTPerc = $('#divModelQuotationPopBody #hdnCGSTPerc').val();
                            QuotationDetailVM.SGSTPerc = $('#divModelQuotationPopBody #hdnSGSTPerc').val();
                            QuotationDetailVM.IGSTPerc = $('#divModelQuotationPopBody #hdnIGSTPerc').val();
                            _dataTable.QuotationDetailList.row.add(QuotationDetailVM).draw(true);
                            $('#divModelPopQuotation').modal('hide');
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
function EditQuotationDetail(this_Obj) {
    debugger;
    _datatablerowindex = _dataTable.QuotationDetailList.row($(this_Obj).parents('tr')).index();
    var quotationDetail = _dataTable.QuotationDetailList.row($(this_Obj).parents('tr')).data();
    $("#divModelQuotationPopBody").load("Quotation/AddQuotationDetail", function () {
        $('#lblModelPopQuotation').text('Quotation Detail')
        $('#FormQuotationDetail #IsUpdate').val('True');
        $('#FormQuotationDetail #ID').val(quotationDetail.ID);
        $("#FormQuotationDetail #ProductID").val(quotationDetail.ProductID)
        $("#FormQuotationDetail #hdnProductID").val(quotationDetail.ProductID)
        $('#divProductBasicInfo').load("Product/ProductBasicInfo?ID=" + $('#hdnProductID').val(), function () {
        });

        if ($('#hdnProductID').val() != _emptyGuid) {
            $('.divProductModelSelectList').load("ProductModel/ProductModelSelectList?required=required&productID=" + $('#hdnProductID').val())
        }
        else {
            $('.divProductModelSelectList').empty();
            $('.divProductModelSelectList').append('<span class="form-control newinput"><i id="dropLoad" class="fa fa-spinner"></i></span>');
        }
        $("#FormQuotationDetail #ProductModelID").val(quotationDetail.ProductModelID);
        $("#FormQuotationDetail #hdnProductModelID").val(quotationDetail.ProductModelID);
        if ($('#hdnProductModelID').val() != _emptyGuid) {
            $('#divProductBasicInfo').load("ProductModel/ProductModelBasicInfo?ID=" + $('#hdnProductModelID').val(), function () {
            });
        }
        $('#FormQuotationDetail #ProductSpec').val(quotationDetail.ProductSpec);
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
    });
}
function ConfirmDeleteQuotationDetail(this_Obj) {
    debugger;
    _datatablerowindex = _dataTable.QuotationDetailList.row($(this_Obj).parents('tr')).index();
    var quotationDetail = _dataTable.QuotationDetailList.row($(this_Obj).parents('tr')).data();
    if (quotationDetail.ID === _emptyGuid) {
        var quotationDetailList = _dataTable.QuotationDetailList.rows().data();
        quotationDetailList.splice(_datatablerowindex, 1);
        _dataTable.QuotationDetailList.clear().rows.add(quotationDetailList).draw(false);
        notyAlert('success', 'Detail Row deleted successfully');
    }
    else {
        notyConfirm('Are you sure to delete?', 'DeleteQuotationDetail("' + quotationDetail.ID + '")');

    }
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
}
//=========================================================================================================================
//Email Quotation
//
function EmailQuotation() {
    debugger;
    $("#divModelEmailQuotationBody").load("Quotation/EmailQuotation?ID=" + $('#QuotationForm #ID').val() + "&EmailFlag=True", function () {
        $('#lblModelEmailQuotation').text('Email Quotation')
        $('#divModelEmailQuotation').modal('show');
    });
}
function SendQuotationEmail() {
    $('#hdnQuotationEMailContent').val($('#divQuotationEmailcontainer').html());
    $('#FormQuotationEmailSend #ID').val($('#QuotationForm #ID').val());
}
function UpdateQuotationEmailInfo() {
    $('#hdnMailBodyHeader').val($('#MailBodyHeader').val());
    $('#hdnMailBodyFooter').val($('#MailBodyFooter').val());
    $('#FormUpdateQuotationEmailInfo #ID').val($('#QuotationForm #ID').val());
}
function DownloadQuotation() {
    var bodyContent = $('#divQuotationEmailcontainer').html();
    var headerContent = $('#hdnHeadContent').html();
    $('#hdnContent').val(bodyContent);
    $('#hdnHeadContent').val(headerContent);
    var customerName = $("#QuotationForm #CustomerID option:selected").text();
    $('#hdnCustomerName').val(customerName);
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
                MasterAlert("success", _result.Message)
                $("#divModelEmailQuotationBody").load("Quotation/EmailQuotation?ID=" + $('#QuotationForm #ID').val() + "&EmailFlag=False", function () {
                    $('#lblModelEmailQuotation').text('Send Email Quotation')
                });
                break;
            case "ERROR":
                MasterAlert("success", _message)
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
                break;
            case "ERROR":
                MasterAlert("success", _message)
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
    if ($('#LatestApprovalStatus').val() == 3) {
        var documentID = $('#QuotationForm #ID').val();
        var latestApprovalID = $('QuotationForm #LatestApprovalID').val();
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
        //BindPurchaseOrder($('#ID').val());
        
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
            var quotationOtherExpenseDetailList = _dataTable.QuotationOtherExpenseDetailList.rows().data();
            quotationOtherExpenseDetailList[_datatablerowindex].OtherCharge.Description = $("#divModelQuotationPopBody #OtherChargeCode").val() != "" ? $("#divModelQuotationPopBody #OtherChargeCode option:selected").text().split("-")[0].trim() : "";
            quotationOtherExpenseDetailList[_datatablerowindex].ChargeAmount = $("#divModelQuotationPopBody #ChargeAmount").val();
            TaxType = new Object;
            if ($('#divModelQuotationPopBody #TaxTypeCode').val() != null) {
                quotationDetailList[_datatablerowindex].TaxTypeCode = $('#divModelQuotationPopBody #TaxTypeCode').val().split('|')[0];
                TaxType.ValueText = $('#divModelQuotationPopBody #TaxTypeCode').val();
            }
            else
                quotationDetailList[_datatablerowindex].TaxTypeCode = null;
            quotationOtherExpenseDetailList[_datatablerowindex].TaxType = TaxType;
            quotationOtherExpenseDetailList[_datatablerowindex].CGSTPerc = $('#divModelQuotationPopBody #hdnCGSTPerc').val();
            quotationOtherExpenseDetailList[_datatablerowindex].SGSTPerc = $('#divModelQuotationPopBody #hdnSGSTPerc').val();
            quotationOtherExpenseDetailList[_datatablerowindex].IGSTPerc = $('#divModelQuotationPopBody #hdnIGSTPerc').val();
            ClearCalculatedFields();
            _dataTable.QuotationOtherChargesDetailList.clear().rows.add(quotationOtherExpenseDetailList).draw(false);
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
                quotationOtherExpenseDetailList[0].ChargeAmount = $("#divModelQuotationPopBody #ChargeAmount").val();
                if ($('#divModelQuotationPopBody #TaxTypeCode').val() != null) {
                    quotationOtherExpenseDetailList[0].TaxTypeCode = $('#divModelQuotationPopBody #TaxTypeCode').val().split('|')[0];
                }
                else
                    quotationOtherExpenseDetailList[0].TaxTypeCode = null;
                quotationOtherExpenseDetailList[0].TaxType.ValueText = $('#divModelQuotationPopBody #TaxTypeCode').val();
                quotationOtherExpenseDetailList[0].CGSTPerc = $('#divModelQuotationPopBody #hdnCGSTPerc').val();
                quotationOtherExpenseDetailList[0].SGSTPerc = $('#divModelQuotationPopBody #hdnSGSTPerc').val();
                quotationOtherExpenseDetailList[0].IGSTPerc = $('#divModelQuotationPopBody #hdnIGSTPerc').val();
                ClearCalculatedFields();
                _dataTable.QuotationOtherChargesDetailList.clear().rows.add(quotationOtherExpenseDetailList).draw(false);
                $('#divModelPopQuotation').modal('hide');
            }
            else {
                debugger;
                var quotationOtherExpenseDetailList = _dataTable.QuotationOtherChargesDetailList.rows().data();
                if (quotationOtherExpenseDetailList.length > 0) {
                    var checkpoint = 0;
                    var otherCharge = $('#OtherChargeCode').text();
                    for (var i = 0; i < quotationOtherExpenseDetailList.length; i++) {
                        if ((quotationOtherExpenseDetailList[i].OtherCharge.Description == otherCharge)) {
                            quotationOtherExpenseDetailList[i].ChargeAmount = $('#ChargeAmount').val();
                            checkpoint = 1;
                            break;
                        }
                    }
                    if (checkpoint == 1) {
                        debugger;
                        ClearCalculatedFields();
                        _dataTable.QuotationOtherChargesDetailList.clear().rows.add(quotationOtherExpenseDetailList).draw(false);
                        $('#divModelPopQuotation').modal('hide');
                    }
                    else if (checkpoint == 0) {
                        ClearCalculatedFields();
                        var QuotationOtherChargesDetailVM = new Object();
                        QuotationOtherChargesDetailVM.ID = _emptyGuid;
                        var OtherCharge = new Object;
                        OtherCharge.Description = $("#divModelQuotationPopBody #OtherChargeCode").val() != "" ? $("#divModelQuotationPopBody #OtherChargeCode option:selected").text().split("-")[0].trim() : "";
                        QuotationOtherChargesDetailVM.OtherCharge = OtherCharge;
                        QuotationOtherChargesDetailVM.ChargeAmount = $("#divModelQuotationPopBody #ChargeAmount").val();
                        var TaxType = new Object();
                        if ($('#divModelQuotationPopBody #TaxTypeCode').val() != null) {
                            QuotationOtherChargesDetailVM.TaxTypeCode = $('#divModelQuotationPopBody #TaxTypeCode').val().split('|')[0];
                            TaxType.ValueText = $('#divModelQuotationPopBody #TaxTypeCode').val();
                        }
                        else
                            QuotationOtherChargesDetailVM.TaxTypeCode = null;
                        QuotationOtherChargesDetailVM.TaxType = TaxType;
                        QuotationOtherChargesDetailVM.CGSTPerc = $('#divModelQuotationPopBody #hdnCGSTPerc').val();
                        QuotationOtherChargesDetailVM.SGSTPerc = $('#divModelQuotationPopBody #hdnSGSTPerc').val();
                        QuotationOtherChargesDetailVM.IGSTPerc = $('#divModelQuotationPopBody #hdnIGSTPerc').val();
                        _dataTable.QuotationDetailList.row.add(QuotationOtherChargesDetailVM).draw(true);
                        debugger;
                        $('#divModelPopQuotation').modal('hide');
                    }
                }
            }
        }
    }
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