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
                $('#tblQuotation').fadeIn('slow');
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
        //BindQuotationOtherChargesDetailList(_emptyGuid)
        OnServerCallComplete();
        setTimeout(function () {
            //resides in customjs for sliding
            openNav();
        }, 100);
    });
}
function EditQuotation(this_Obj) {
    OnServerCallBegin();
    var Quotation = _dataTable.QuotationList.row($(this_Obj).parents('tr')).data();
    //this will return form body(html)
    $("#divQuotationForm").load("Quotation/QuotationForm?id=" + Quotation.ID + "&estimateID=" + Quotation.EstimateID, function () {
        //$('#CustomerID').trigger('change');
        ChangeButtonPatchView("Quotation", "btnPatchQuotationNew", "Edit");
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
    $("#divQuotationForm").load("Quotation/QuotationForm?id=" + $('#QuotationForm #ID').val() + "&estimateID=", function () {
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
                $("#divQuotationForm").load("Quotation/QuotationForm?id=" + _result.ID + "&estimateID="+ result.EstimateID, function () {
                    ChangeButtonPatchView("Quotation", "btnPatchQuotationNew", "Edit");
                    BindQuotationDetailList(_result.ID);
                    clearUploadControl();
                    PaintImages(_result.ID);
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
             { "data": null, "orderable": false, "defaultContent": '<a href="#" class="DeleteLink"  onclick="ConfirmDeleteQuotationDetail(this)" ><i class="fa fa-trash-o" aria-hidden="true"></i></a> <a href="#" class="actionLink"  onclick="EditQuotationDetail(this)" ><i class="fa fa-pencil-square-o" aria-hidden="true"></i></a>' },
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
function BindQuotationDetailList(id, IsEstimated) {
    debugger;

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
             { "data": "Product.Code", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
             { "data": "Product.Name", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
             { "data": "ProductModel.Name", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
             { "data": "ProductSpec", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
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
                 "data": "CGSTAmt", render: function (data, type, row) {
                     debugger;
                     var CGST = parseFloat(row.TaxType.ValueText != "" ? row.TaxType.ValueText.split('|')[1].split(',')[0].split('-')[1] : 0);
                     var SGST = parseFloat(row.TaxType.ValueText != "" ? row.TaxType.ValueText.split('|')[1].split(',')[1].split('-')[1] : 0);
                     var IGST = parseFloat(row.TaxType.ValueText != "" ? row.TaxType.ValueText.split('|')[1].split(',')[2].split('-')[1] : 0);
                     var GSTAmt = roundoff(parseFloat(data) + parseFloat(row.SGSTAmt) + parseFloat(row.IGSTAmt))
                     return '<div class="show-popover text-right" data-html="true" data-toggle="popover" data-title="<p align=left>Total GST : ₹ ' + GSTAmt + '" data-content=" SGST ' + SGST + '% : ₹ ' + roundoff(parseFloat(row.SGSTAmt)) + '<br/>CGST ' + CGST + '% : ₹ ' + roundoff(parseFloat(data)) + '<br/> IGST ' + IGST + '% : ₹ ' + roundoff(parseFloat(row.IGSTAmt)) + '</p>"/>' + GSTAmt
                 }, "defaultContent": "<i></i>"
             },
             {
                 "data": "Rate", render: function (data, type, row) {
                     var TaxableAmt = roundoff((parseFloat(row.Rate != "" ? row.Rate : 0) * parseInt(row.Qty != "" ? row.Qty : 1)) - parseFloat(row.Discount != "" ? row.Discount : 0))
                     var GSTAmt = roundoff(parseFloat(row.CGSTAmt) + parseFloat(row.SGSTAmt) + parseFloat(row.IGSTAmt))
                     var GrandTotal = roundoff(((parseFloat(row.Rate != "" ? row.Rate : 0) * parseInt(row.Qty != "" ? row.Qty : 1)) - parseFloat(row.Discount != "" ? row.Discount : 0)) + parseFloat(row.SGSTAmt) + parseFloat(row.IGSTAmt) + parseFloat(row.CGSTAmt))
                     return '<div class="show-popover text-right" data-html="true" data-toggle="popover" data-title="<p align=left>Grand Total : ₹ ' + GrandTotal + '" data-content="Taxable : ₹ ' + TaxableAmt + '<br/>GST : ₹ ' + GSTAmt + '</p>"/>' + GrandTotal
                 }, "defaultContent": "<i></i>"
             },
             { "data": null, "orderable": false, "defaultContent": '<a href="#" class="DeleteLink"  onclick="ConfirmDeleteQuotationDetail(this)" ><i class="fa fa-trash-o" aria-hidden="true"></i></a> <a href="#" class="actionLink"  onclick="EditQuotationDetail(this)" ><i class="fa fa-pencil-square-o" aria-hidden="true"></i></a>' },
             ],
             columnDefs: [
                 { "targets": [0], "width": "15%" },
                 { "targets": [1, 2], "width": "10%" },
                 { "targets": [3], "width": "30%" },
                 { "targets": [4, 5, 6, 7, 8, 9, 10], "width": "5%" },
                 { className: "text-right", "targets": [4, 5, 6, 7, 8, 9] },
                 { className: "text-left", "targets": [3] },
                 { className: "text-center", "targets": [0, 1, 2, 10] }
             ],
             rowCallback: function (row, data, index) {
                 debugger;
                 var TaxableAmt = (parseFloat(data.Rate != "" ? data.Rate : 0) * parseInt(data.Qty != "" ? data.Qty : 1)) - parseFloat(data.Discount != "" ? data.Discount : 0)
                 var GSTAmt = parseFloat(data.CGSTAmt) + parseFloat(data.SGSTAmt) + parseFloat(data.IGSTAmt)
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
        'placement': 'left'
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
    $("#FormQuotationDetail").submit(function () {
        debugger;
        if ($('#FormQuotationDetail #IsUpdate').val() == 'True') {
            debugger;
            if ($('#divModelPopQuotation #ProductID').val() != "") {
                debugger;
                var quotationDetailList = _dataTable.QuotationDetailList.rows().data();
                quotationDetailList[_datatablerowindex].Product.Code = $("#divModelPopQuotation #ProductID").val() != "" ? $("#divModelPopQuotation #ProductID option:selected").text().split("-")[0].trim() : "";
                quotationDetailList[_datatablerowindex].Product.Name = $("#divModelPopQuotation #ProductID").val() != "" ? $("#divModelPopQuotation #ProductID option:selected").text().split("-")[1].trim() : "";
                quotationDetailList[_datatablerowindex].ProductID = $("#divModelPopQuotation #ProductID").val() != "" ? $("#divModelPopQuotation #ProductID").val() : _emptyGuid;
                quotationDetailList[_datatablerowindex].ProductModelID = $("#ProductModelID").val() != "" ? $("#ProductModelID").val() : _emptyGuid;
                ProductModel = new Object;
                Unit = new Object;
                TaxType = new Object;
                ProductModel.Name = $("#divModelPopQuotation #ProductModelID").val() != "" ? $("#divModelPopQuotation #ProductModelID option:selected").text() : "";
                quotationDetailList[_datatablerowindex].ProductModel = ProductModel;
                quotationDetailList[_datatablerowindex].ProductSpec = $('#divModelPopQuotation #ProductSpec').val();
                quotationDetailList[_datatablerowindex].Qty = $('#divModelPopQuotation #Qty').val();
                quotationDetailList[_datatablerowindex].UnitCode = $('#divModelPopQuotation #UnitCode').val();
                Unit.Description = $("#divModelPopQuotation #UnitCode").val() != "" ? $("#divModelPopQuotation #UnitCode option:selected").text().trim() : "";
                quotationDetailList[_datatablerowindex].Unit = Unit;
                quotationDetailList[_datatablerowindex].Rate = $('#divModelPopQuotation #Rate').val();
                quotationDetailList[_datatablerowindex].Discount = $('#Discount').val() != "" ? $('#divModelPopQuotation #Discount').val() : 0;
                quotationDetailList[_datatablerowindex].TaxTypeCode = $('#divModelPopQuotation #TaxTypeCode').val().split('|')[0];
                TaxType.ValueText = $('#divModelPopQuotation #TaxTypeCode').val();
                quotationDetailList[_datatablerowindex].TaxType = TaxType;
                quotationDetailList[_datatablerowindex].CGSTAmt = $('#divModelPopQuotation #CGSTAmt').val();
                quotationDetailList[_datatablerowindex].SGSTAmt = $('#divModelPopQuotation #SGSTAmt').val();
                quotationDetailList[_datatablerowindex].IGSTAmt = $('#divModelPopQuotation #IGSTAmt').val();
                _dataTable.QuotationDetailList.clear().rows.add(quotationDetailList).draw(false);
                $('#divModelPopQuotation').modal('hide');
                _datatablerowindex = -1;
            }
        }
        else {
            if ($('#divModelPopQuotation #ProductID').val() != "")
                if (_dataTable.QuotationDetailList.rows().data().length === 0) {
                    _dataTable.QuotationDetailList.clear().rows.add(GetQuotationDetailListByQuotationID(_emptyGuid)).draw(false);
                    debugger;
                    var quotationDetailList = _dataTable.QuotationDetailList.rows().data();
                    quotationDetailList[0].Product.Code = $("#divModelPopQuotation #ProductID").val() != "" ? $("#divModelPopQuotation #ProductID option:selected").text().split("-")[0].trim() : "";
                    quotationDetailList[0].Product.Name = $("#divModelPopQuotation #ProductID").val() != "" ? $("#ProductID option:selected").text().split("-")[1].trim() : "";
                    quotationDetailList[0].ProductID = $("#divModelPopQuotation #ProductID").val() != "" ? $("#divModelPopQuotation #ProductID").val() : _emptyGuid;
                    quotationDetailList[0].ProductModelID = $("#divModelPopQuotation #ProductModelID").val() != "" ? $("#divModelPopQuotation #ProductModelID").val() : _emptyGuid;
                    quotationDetailList[0].ProductModel.Name = $("#divModelPopQuotation #ProductModelID").val() != "" ? $("#divModelPopQuotation #ProductModelID option:selected").text() : "";
                    quotationDetailList[0].ProductSpec = $('#divModelPopQuotation #ProductSpec').val();
                    quotationDetailList[0].Qty = $('#divModelPopQuotation #Qty').val();
                    quotationDetailList[0].UnitCode = $('#UnitCode').val();
                    quotationDetailList[0].Unit.Description = $("#divModelPopQuotation #UnitCode").val() != "" ? $("#divModelPopQuotation #UnitCode option:selected").text().trim() : "";
                    quotationDetailList[0].Rate = $('#divModelPopQuotation #Rate').val();
                    quotationDetailList[0].Discount = $('#divModelPopQuotation #Discount').val() != "" ? $('#divModelPopQuotation #Discount').val() : 0;
                    quotationDetailList[0].TaxTypeCode = $('#divModelPopQuotation #TaxTypeCode').val().split('|')[0];
                    quotationDetailList[0].TaxType.ValueText = $('#divModelPopQuotation #TaxTypeCode').val();
                    quotationDetailList[0].CGSTAmt = $('#divModelPopQuotation #CGSTAmt').val();
                    quotationDetailList[0].SGSTAmt = $('#divModelPopQuotation #SGSTAmt').val();
                    quotationDetailList[0].IGSTAmt = $('#divModelPopQuotation #IGSTAmt').val();
                    _dataTable.QuotationDetailList.clear().rows.add(quotationDetailList).draw(false);
                    $('#divModelPopQuotation').modal('hide');
                }
                else {
                    debugger;
                    var QuotationDetailVM = new Object();
                    QuotationDetailVM.ID = _emptyGuid;
                    QuotationDetailVM.ProductID = ($("#divModelPopQuotation #ProductID").val() != "" ? $("#divModelPopQuotation #ProductID").val() : _emptyGuid);
                    var Product = new Object;
                    Product.Code = ($("#divModelPopQuotation #ProductID").val() != "" ? $("#divModelPopQuotation #ProductID option:selected").text().split("-")[0].trim() : "");
                    Product.Name = ($("#divModelPopQuotation #ProductID").val() != "" ? $("#divModelPopQuotation #ProductID option:selected").text().split("-")[1].trim() : "");
                    QuotationDetailVM.Product = Product;
                    QuotationDetailVM.ProductModelID = ($("#divModelPopQuotation #ProductModelID").val() != "" ? $("#divModelPopQuotation #ProductModelID").val() : _emptyGuid);
                    var ProductModel = new Object()
                    ProductModel.Name = ($("#ProductModelID").val() != "" ? $("#ProductModelID option:selected").text() : "");
                    QuotationDetailVM.ProductModel = ProductModel;
                    QuotationDetailVM.ProductSpec = $('#divModelPopQuotation #ProductSpec').val();
                    QuotationDetailVM.Qty = $('#divModelPopQuotation #Qty').val();
                    var Unit = new Object();
                    Unit.Description = $("#divModelPopQuotation #UnitCode").val() != "" ? $("#divModelPopQuotation #UnitCode option:selected").text().trim() : "";
                    QuotationDetailVM.Unit = Unit;
                    QuotationDetailVM.UnitCode = $('#divModelPopQuotation #UnitCode').val();
                    QuotationDetailVM.Rate = $('#divModelPopQuotation #Rate').val();
                    QuotationDetailVM.Discount = $('#divModelPopQuotation #Discount').val() != "" ? $('#divModelPopQuotation #Discount').val() : 0;
                    QuotationDetailVM.TaxTypeCode = $('#divModelPopQuotation #TaxTypeCode').val().split('|')[0];
                    var TaxType = new Object();
                    TaxType.ValueText = $('#divModelPopQuotation #TaxTypeCode').val();
                    QuotationDetailVM.TaxType = TaxType;
                    QuotationDetailVM.CGSTAmt = $('#divModelPopQuotation #CGSTAmt').val();
                    QuotationDetailVM.SGSTAmt = $('#divModelPopQuotation #SGSTAmt').val();
                    QuotationDetailVM.IGSTAmt = $('#divModelPopQuotation #IGSTAmt').val();
                    _dataTable.QuotationDetailList.row.add(QuotationDetailVM).draw(true);
                    $('#divModelPopQuotation').modal('hide');
                }
        }
        $('[data-toggle="popover"]').popover({
            html: true,
            'trigger': 'hover',
            'placement': 'left'
        });
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
        $('#FormQuotationDetail #CGSTAmt').val(quotationDetail.CGSTAmt);
        $('#FormQuotationDetail #SGSTAmt').val(quotationDetail.SGSTAmt);
        $('#FormQuotationDetail #IGSTAmt').val(quotationDetail.IGSTAmt);
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
            _dataTable.QuotationDetailList.clear().rows.add(quotationDetailList).draw(false);
        }
        if (_status == "ERROR") {
            notyAlert('error', _message);
        }
    }
}
function CalculateGrandTotal(value) {
    var GrandTotal = roundoff(parseFloat($('#lblGrandTotal').text()) - parseFloat(value!=""?value:0))
    $('#lblGrandTotal').text(GrandTotal);
}
//=========================================================================================================================
//Email Quotation
//
function EmailQuotation() {
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