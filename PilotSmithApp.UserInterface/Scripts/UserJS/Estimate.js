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
        BindOrReloadEstimateTable('Init');
        $('#tblEstimate tbody').on('dblclick', 'td', function () {
            if (this.textContent !== "No data available in table")
                EditEstimate(this);
        });
        if ($('#RedirectToDocument').val() != "") {
            if ($('#RedirectToDocument').val() === _emptyGuid) {
                AddEstimate();
            }
            else {
                EditRedirectToDocument($('#RedirectToDocument').val());
            }
        }
    }
    catch (e) {
        console.log(e.message);
    }
      $("#AdvDocumentStatusCode").select2({
        dropdownParent: $(".divboxASearch")
        });

    $('.select2').addClass('form-control newinput');
});

//function bind the Enquiry list checking search and filter
function BindOrReloadEstimateTable(action) {
    try {
        //creating advancesearch object
        EstimateAdvanceSearchViewModel = new Object();
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
                break;
            case 'Search':
                if (($('#SearchTerm').val() == "") && ($('.divboxASearch #AdvFromDate').val() == "") &&($('#AdvToDate').val() == "") && ($('.divboxASearch #AdvAreaCode').val() == "") && ($('.divboxASearch #AdvCustomerID').val() == "") && ($('.divboxASearch #AdvReferencePersonCode').val() == "") && ($('.divboxASearch #AdvBranchCode').val() == "") && ($('.divboxASearch #AdvDocumentStatusCode').val() == "") && ($('.divboxASearch #AdvDocumentOwnerID').val() == "")) {
                    return true;
                }
                break;
            case 'Export':
                DataTablePagingViewModel.Length = -1;
                EstimateAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
                EstimateAdvanceSearchViewModel.SearchTerm = $('#SearchTerm').val() == "" ? null : $('#SearchTerm').val();
                EstimateAdvanceSearchViewModel.AdvFromDate = $('.divboxASearch #AdvFromDate').val() == "" ? null : $('.divboxASearch #AdvFromDate').val();
                EstimateAdvanceSearchViewModel.AdvToDate = $('.divboxASearch #AdvToDate').val() == "" ? null : $('.divboxASearch #AdvToDate').val();
                EstimateAdvanceSearchViewModel.AdvAreaCode = $('.divboxASearch #AdvAreaCode').val() == "" ? null : $('.divboxASearch #AdvAreaCode').val();
                EstimateAdvanceSearchViewModel.AdvCustomerID = $('.divboxASearch #AdvCustomerID').val() == "" ? _emptyGuid : $('.divboxASearch #AdvCustomerID').val();
                EstimateAdvanceSearchViewModel.AdvReferencePersonCode = $('.divboxASearch #AdvReferencePersonCode').val() == "" ? null : $('.divboxASearch #AdvReferencePersonCode').val();
                EstimateAdvanceSearchViewModel.AdvBranchCode = $('.divboxASearch #AdvBranchCode').val() == "" ? null : $('.divboxASearch #AdvBranchCode').val();
                EstimateAdvanceSearchViewModel.AdvDocumentStatusCode = $('.divboxASearch #AdvDocumentStatusCode').val() == "" ? null : $('.divboxASearch #AdvDocumentStatusCode').val();
                EstimateAdvanceSearchViewModel.AdvDocumentOwnerID = $('.divboxASearch #AdvDocumentOwnerID').val() == "" ? _emptyGuid : $('.divboxASearch #AdvDocumentOwnerID').val();
                $('#AdvanceSearch').val(JSON.stringify(EstimateAdvanceSearchViewModel));
                $('#FormExcelExport').submit();
                return true;
                break;
            default:
                break;
        }
        EstimateAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
        EstimateAdvanceSearchViewModel.SearchTerm = $('#SearchTerm').val();
        EstimateAdvanceSearchViewModel.AdvFromDate = $('.divboxASearch #AdvFromDate').val();
        EstimateAdvanceSearchViewModel.AdvToDate = $('.divboxASearch #AdvToDate').val();
        EstimateAdvanceSearchViewModel.AdvAreaCode = $('.divboxASearch #AdvAreaCode').val();
        EstimateAdvanceSearchViewModel.AdvCustomerID = $('.divboxASearch #AdvCustomerID').val();
        EstimateAdvanceSearchViewModel.AdvReferencePersonCode = $('.divboxASearch #AdvReferencePersonCode').val();
        EstimateAdvanceSearchViewModel.AdvBranchCode = $('.divboxASearch #AdvBranchCode').val();
        EstimateAdvanceSearchViewModel.AdvDocumentStatusCode = $('.divboxASearch #AdvDocumentStatusCode').val();
        EstimateAdvanceSearchViewModel.AdvDocumentOwnerID = $('.divboxASearch #AdvDocumentOwnerID').val();
        //apply datatable plugin on Estimate table
        
        _dataTable.EstimateList = $('#tblEstimate').DataTable(
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
                url: "Estimate/GetAllEstimate/",
                data: { "estimateAdvanceSearchVM": EstimateAdvanceSearchViewModel },
                type: 'POST'
            },
            pageLength: 8,
            columns: [
               {
                   "data": "EstimateNo", render: function (data, type, row) {
                       return (data == null ? " " : data) + " <br/>" + "<img src='./Content/images/datePicker.png' height='10px'>" + "&nbsp;" + (row.EstimateDateFormatted == null ? " " : row.EstimateDateFormatted)
                   }, "defaultContent": "<i>-</i>"
               },
               { "data": "Enquiry.EnquiryNo", "defaultContent": "<i>-</i>" },
               {
                   "data": "Customer.CompanyName", render: function (data, type, row) {

                       return "<img src='./Content/images/contact.png' height='10px'>" + "&nbsp;" + (row.Customer.ContactPerson == null ? " " : row.Customer.ContactPerson) + " </br>" + "<img src='./Content/images/organisation.png' height='10px'>" + "&nbsp;" + (data == null ? " " : data);

                   }, "defaultContent": "<i>-</i>"
               },
               { "data": "Area.Description", "defaultContent": "<i>-</i>" },
               { "data": "ReferencePerson.Name", "defaultContent": "<i>-</i>" },
               { "data": "UserName", "defaultContent": "<i>-</i>" },
                {
                    "data": "DocumentStatus.Description", render: function (data, type, row) {

                        return "<b>Document Status-</b>" + (data == null ? " " : data) + " </br>" + "<b>Branch-</b>" + (row.Branch.Description == null ? " " : row.Branch.Description);

                    }, "defaultContent": "<i>-</i>"
                },
               { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink"  onclick="EditEstimate(this)" ><i class="fa fa-pencil-square-o" aria-hidden="true"></i></a>' },
            ],
            columnDefs: [
                    { className: "text-left", "targets": [0,1,2,3,4,5,6] },
                    { className: "text-center", "targets": [7] },
                    { "targets": [ 3, 4, 5], "width": "10%" },
                    { "targets": [0,1], "width": "14%" },
                    { "targets": [2], "width": "20%" },
                    { "targets": [6], "width": "17%" },
                    {"targets": [7], "width": "5%"
        },
            ],
            destroy: true,
            //for performing the import operation after the data loaded
            initComplete: function (settings, json) {
            
                $('.dataTables_wrapper div.bottom div').addClass('col-md-6');
                $('#tblEstimate').fadeIn(100);
                if (action == undefined) {
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
function ResetEstimateList() {
    $(".searchicon").removeClass('filterApplied');
    BindOrReloadEstimateTable('Reset');
}
//function export data to excel
function ExportEstimateData() {
    BindOrReloadEstimateTable('Export');
}

// add Estimate section
function AddEstimate() {
    debugger;
    //this will return form body(html)
    OnServerCallBegin();
    $("#divEstimateForm").load("Estimate/EstimateForm?id=" + _emptyGuid, function (responseTxt, statusTxt, xhr) {
        if (statusTxt == "success") {
            OnServerCallComplete();
            openNav();
            $('#lblEstimateInfo').text('<<Estimate No.>>');
            ChangeButtonPatchView("Estimate", "btnPatchEstimateNew", "Add");
            BindEstimateDetailList(_emptyGuid);
        }
        else {
            console.log("Error: " + xhr.status + ": " + xhr.statusText);
        }
    });
}

function EditEstimate(this_Obj) {
    debugger;
    OnServerCallBegin();
    var Estimate = _dataTable.EstimateList.row($(this_Obj).parents('tr')).data();
    $('#lblEstimateInfo').text(Estimate.EstimateNo);
    //this will return form body(html)
    $("#divEstimateForm").load("Estimate/EstimateForm?id=" + Estimate.ID, function (responseTxt, statusTxt, xhr) {
        if (statusTxt == "success") {
         OnServerCallComplete();
         openNav();
        if ($('#IsDocLocked').val() == "True") {
            ChangeButtonPatchView("Estimate", "btnPatchEstimateNew", "Edit", Estimate.ID);
        }
        else {
            ChangeButtonPatchView("Estimate", "btnPatchEstimateNew", "LockDocument", Estimate.ID);
        }
        BindEstimateDetailList(Estimate.ID);
        $('#divCustomerBasicInfo').load("Customer/CustomerBasicInfo?ID=" + $('#hdnCustomerID').val());
        clearUploadControl();
        PaintImages(Estimate.ID);        
        $("#divEstimateForm #EnquiryID").prop('disabled', true);
        
        }
        else {
            console.log("Error: " + xhr.status + ": " + xhr.statusText);
        }
    });
}

function ResetEstimate() {
    debugger;
    if ($('#IsUpdate').val() == 'False') {
        $('#hdnEnquiryID').val('');
    }
    //this will return form body(html)
    $("#divEstimateForm").load("Estimate/EstimateForm?id=" + $('#EstimateForm #ID').val() , function (responseTxt, statusTxt, xhr) {
        if (statusTxt == "success") {
            if ($('#ID').val() != _emptyGuid && $('#ID').val() != null) {
                //resides in customjs for sliding
               
                    $("#divEstimateForm #EnquiryID").prop('disabled', true);
                    openNav();               
            }
            else {
                debugger;
                $('#hdnCustomerID').val('');
                $("#EstimateForm #CustomerID").prop('disabled', false);
                $('#lblEstimateInfo').text('<<Estimate No.>>');
            }
            BindEstimateDetailList($('#ID').val(), false);
            clearUploadControl();
            PaintImages($('#EstimateForm #ID').val());
            $('#divCustomerBasicInfo').load("Customer/CustomerBasicInfo?ID=" + $('#EstimateForm #hdnCustomerID').val());
        }
        else {
            console.log("Error: " + xhr.status + ": " + xhr.statusText);
        }
        
    });
}
function SaveEstimate() {
    var estimateDetailList = _dataTable.EstimateDetailList.rows().data().toArray();
    $('#DetailJSON').val(JSON.stringify(estimateDetailList));
    $('#btnInsertUpdateEstimate').trigger('click');
}
function ApplyFilterThenSearch() {
    $(".searchicon").addClass('filterApplied');
    CloseAdvanceSearch();
    BindOrReloadEstimateTable('Search');
}

function SaveSuccessEstimate(data, status) {
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
                $("#divEstimateForm").load("Estimate/EstimateForm?id=" + _result.ID+"&enquiryID="+_result.EnquiryID, function () {
                    ChangeButtonPatchView("Estimate", "btnPatchEstimateNew", "Edit", _result.ID);
                    BindEstimateDetailList(_result.ID);
                    clearUploadControl();
                    PaintImages(_result.ID);
                    $('#lblEstimateInfo').text(_result.EstimateNo);

                });
                ChangeButtonPatchView("Estimate", "btnPatchEstimateNew", "Edit", _result.ID);
                BindOrReloadEstimateTable('Init');
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

function DeleteEstimate() {
    debugger;
    notyConfirm('Are you sure to delete?', 'DeleteEstimateItem("' + $('#EstimateForm #ID').val() + '")');
}
function DeleteEstimateItem(id) {
    debugger;
    try {
        if (id) {
            var data = { "id": id };
            _jsonData = GetDataFromServer("Estimate/DeleteEstimate/", data);
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
                    ChangeButtonPatchView("Estimate", "btnPatchEstimateNew", "Add");
                    ResetEstimate();
                    BindOrReloadEstimateTable('Init');
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

function BindEstimateDetailList(id,IsEnquiry) {
    debugger;
    _dataTable.EstimateDetailList = $('#tblEstimateDetails').DataTable(
         {
             dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
             order: [],
             searching: false,
             paging: false,
             ordering: false,
             bInfo: false,
             data: !IsEnquiry ? id == _emptyGuid ? null : GetEstimateDetailListByEstimateID(id,false) : GetEstimateDetailListByEstimateID(id,true),
             language: {
                 search: "_INPUT_",
                 searchPlaceholder: "Search"
             },
             columns: [ 
             {
                 "data": "Product.Code", render: function (data, type, row) {
                     debugger;
                     return row.Product.Name + "<br/>" + '<div style="width:100%" class="show-popover" data-html="true" data-placement="top" data-toggle="popover" data-title="<p align=left>Product Specification" data-content="' +(row.ProductSpec!==null?row.ProductSpec.replace(/"/g, "&quot"):"") + '</p>"/>' + row.ProductModel.Name
                 }, "defaultContent": "<i></i>"
             },
            
             {
                 "data": "Qty", render: function (data, type, row) {
                     return data + " " + row.Unit.Description
                 }, "defaultContent": "<i></i>"
             },
             //{ "data": "Unit.Description", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
             { "data": "CostRate", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
             { "data": "SellingRate", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
             { "data": "DrawingNo", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
             { "data": "TotalCostPrice", render: function (data, type, row) { return parseFloat(row.CostRate) * parseFloat(row.Qty) }, "defaultContent": "<i></i>" },
             { "data": "TotalSeingPrice", render: function (data, type, row) { return parseFloat(row.SellingRate) * parseFloat(row.Qty) }, "defaultContent": "<i></i>" },
            { "data": null, "orderable": false, "defaultContent": ($('#IsDocLocked').val() == "True" || $('#IsUpdate').val() == "False") ? '<a href="#" class="actionLink"  onclick="EditEstimateDetail(this)" ><i class="fa fa-pencil-square-o" aria-hidden="true"></i></a> <a href="#" class="DeleteLink"  onclick="ConfirmDeleteEstimateDetail(this)" ><i class="fa fa-trash-o" aria-hidden="true"></i></a>' : "-" },
             ],
             columnDefs: [
                 { "targets": [0], "width": "21%" },
                 { "targets": [1], "width": "10%" },
                 { "targets": [2, 3, 7], "width": "10%" },
                 { "targets": [5,6], "width": "12%" },
                 { "targets": [4], "width": "15%" },
                 { className: "text-left", "targets": [0,4] },
                 { className: "text-right", "targets": [1, 2, 3,5,6] },
                 { className: "text-center", "targets": [7] }
             ],
             destroy:true
         });
    $('[data-toggle="popover"]').popover({
        html: true,
        'trigger': 'hover',

    });
}

function GetEstimateDetailListByEstimateID(id,IsEnquiry) {
    try {
        debugger;
       
        var estimateDetailList = [];
        if (IsEnquiry)
        {
            var data = { "enquiryID": $('#EstimateForm #hdnEnquiryID').val() };
            _jsonData = GetDataFromServer("Estimate/GetEstimateDetailListByEstimateIDWithEnquiry/", data);
        }
        else {
            var data = { "estimateID": id };
            _jsonData = GetDataFromServer("Estimate/GetEstimateDetailListByEstimateID/", data);
        }
       
        if (_jsonData != '') {
            _jsonData = JSON.parse(_jsonData);
            _message = _jsonData.Message;
            _status = _jsonData.Status;
            estimateDetailList = _jsonData.Records;
        }
        if (_status == "OK") {
            return estimateDetailList;
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

function AddEstimateDetailList() {
    debugger;
    $("#divModelEstimatePopBody").load("Estimate/AddEstimateDetail", function () {
        $('#lblModelPopEstimate').text('Estimate Detail')
        $('#divModelPopEstimate').modal('show');
    });
}

function AddEstimateDetailToList() {
    debugger;
    $("#FormEstimateDetail").submit(function () { });
        debugger;
        if ($('#FormEstimateDetail #IsUpdate').val() == 'True') {
            debugger;
            if (($('#ProductID').val() != "") && ($('#ProductModelID').val() != "") && ($('#ProductSpec').val() != "") && ($('#Qty').val() != "") && ($('#UnitCode').val() != "")) {
                debugger;
                var estimateDetailList = _dataTable.EstimateDetailList.rows().data();
                estimateDetailList[_datatablerowindex].Product.Code = $("#ProductID").val() != "" ? $("#ProductID option:selected").text().split("-")[0].trim() : "";
                estimateDetailList[_datatablerowindex].Product.Name = $("#ProductID").val() != "" ? $("#ProductID option:selected").text().split("-")[1].trim() : "";
                estimateDetailList[_datatablerowindex].ProductID = $("#ProductID").val() != "" ? $("#ProductID").val() : _emptyGuid;
                estimateDetailList[_datatablerowindex].ProductModelID = $("#ProductModelID").val() != "" ? $("#ProductModelID").val() : _emptyGuid;
                ProductModel = new Object;
                Unit = new Object;
                ProductModel.Name = $("#ProductModelID").val() != "" ? $("#ProductModelID option:selected").text() : "";
                estimateDetailList[_datatablerowindex].ProductModel = ProductModel;
                estimateDetailList[_datatablerowindex].ProductSpec = $('#ProductSpec').val();
                estimateDetailList[_datatablerowindex].Qty = $('#Qty').val();
                estimateDetailList[_datatablerowindex].UnitCode = $('#UnitCode').val();
                Unit.Description = $("#UnitCode").val() != "" ? $("#UnitCode option:selected").text().trim() : "";
                estimateDetailList[_datatablerowindex].Unit = Unit;
                estimateDetailList[_datatablerowindex].CostRate = $('#CostRate').val();
                estimateDetailList[_datatablerowindex].SellingRate = $('#SellingRate').val();
                estimateDetailList[_datatablerowindex].DrawingNo = $('#DrawingNo').val();
                _dataTable.EstimateDetailList.clear().rows.add(estimateDetailList).draw(false);
                $('#divModelPopEstimate').modal('hide');
                _datatablerowindex = -1;
            }
        }
        else {
            if (($('#ProductID').val() != "") && ($('#ProductModelID').val() != "") && ($('#ProductSpec').val() != "") && ($('#Qty').val() != "") && ($('#UnitCode').val() != ""))
            {
                if (_dataTable.EstimateDetailList.rows().data().length === 0) {
                    _dataTable.EstimateDetailList.clear().rows.add(GetEstimateDetailListByEstimateID(_emptyGuid)).draw(false);
                    debugger;
                    var estimateDetailList = _dataTable.EstimateDetailList.rows().data();
                    estimateDetailList[0].Product.Code = $("#ProductID").val() != "" ? $("#ProductID option:selected").text().split("-")[0].trim() : "";
                    estimateDetailList[0].Product.Name = $("#ProductID").val() != "" ? $("#ProductID option:selected").text().split("-")[1].trim() : "";
                    estimateDetailList[0].ProductID = $("#ProductID").val() != "" ? $("#ProductID").val() : _emptyGuid;
                    estimateDetailList[0].ProductModelID = $("#ProductModelID").val() != "" ? $("#ProductModelID").val() : _emptyGuid;
                    estimateDetailList[0].ProductModel.Name = $("#ProductModelID").val() != "" ? $("#ProductModelID option:selected").text() : "";
                    estimateDetailList[0].ProductSpec = $('#ProductSpec').val();
                    estimateDetailList[0].Qty = $('#Qty').val();
                    estimateDetailList[0].UnitCode = $('#UnitCode').val();
                    estimateDetailList[0].Unit.Description = $("#UnitCode").val() != "" ? $("#UnitCode option:selected").text().trim() : "";
                    estimateDetailList[0].CostRate = $('#CostRate').val();
                    estimateDetailList[0].SellingRate = $('#SellingRate').val();
                    estimateDetailList[0].DrawingNo = $('#DrawingNo').val();
                    _dataTable.EstimateDetailList.clear().rows.add(estimateDetailList).draw(false);
                    $('#divModelPopEstimate').modal('hide');
                }
                else {
                    debugger;
                    var estimateDetailList = _dataTable.EstimateDetailList.rows().data();
                    if (estimateDetailList.length > 0) {
                        var checkpoint = 0;
                        var productSpec = $('#ProductSpec').val();
                        productSpec = productSpec.replace(/\n/g, ' ');
                        for (var i = 0; i < estimateDetailList.length; i++) {
                            if ((estimateDetailList[i].ProductID == $('#ProductID').val()) && (estimateDetailList[i].ProductModelID == $('#ProductModelID').val()
                                && (estimateDetailList[i].ProductSpec.replace(/\n/g, ' ') == productSpec && (estimateDetailList[i].UnitCode == $('#UnitCode').val())))) {
                                estimateDetailList[i].Qty = parseFloat(estimateDetailList[i].Qty) + parseFloat($('#Qty').val());
                                checkpoint = 1;
                                break;
                            }
                        }
                        if (checkpoint == 1) {
                            debugger;
                            _dataTable.EstimateDetailList.clear().rows.add(estimateDetailList).draw(false);
                            $('#divModelPopEstimate').modal('hide');
                        }
                        else if (checkpoint == 0) {
                            var EstimateDetailVM = new Object();
                            var Product = new Object;
                            var ProductModel = new Object()
                            var Unit = new Object();
                            EstimateDetailVM.ID = _emptyGuid;
                            EstimateDetailVM.ProductID = $("#ProductID").val() != "" ? $("#ProductID").val() : _emptyGuid;
                            Product.Code = $("#ProductID").val() != "" ? $("#ProductID option:selected").text().split("-")[0].trim() : "";
                            Product.Name = $("#ProductID").val() != "" ? $("#ProductID option:selected").text().split("-")[1].trim() : "";
                            EstimateDetailVM.Product = Product;
                            EstimateDetailVM.ProductModelID = $("#ProductModelID").val() != "" ? $("#ProductModelID").val() : _emptyGuid;
                            ProductModel.Name = $("#ProductModelID").val() != "" ? $("#ProductModelID option:selected").text() : "";
                            EstimateDetailVM.ProductModel = ProductModel;
                            EstimateDetailVM.ProductSpec = $('#ProductSpec').val();
                            EstimateDetailVM.Qty = $('#Qty').val();
                            Unit.Description = $("#UnitCode").val() != "" ? $("#UnitCode option:selected").text().trim() : "";
                            EstimateDetailVM.Unit = Unit;
                            EstimateDetailVM.UnitCode = $('#UnitCode').val();
                            EstimateDetailVM.CostRate = $('#CostRate').val();
                            EstimateDetailVM.SellingRate = $('#SellingRate').val();
                            EstimateDetailVM.DrawingNo = $('#DrawingNo').val();
                            _dataTable.EstimateDetailList.row.add(EstimateDetailVM).draw(false);
                            $('#divModelPopEstimate').modal('hide');
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

function EditEstimateDetail(this_Obj) {
    debugger;
    _datatablerowindex = _dataTable.EstimateDetailList.row($(this_Obj).parents('tr')).index();
    var estimateDetail = _dataTable.EstimateDetailList.row($(this_Obj).parents('tr')).data();
    $("#divModelEstimatePopBody").load("Estimate/AddEstimateDetail", function () {
        $('#lblModelPopEstimate').text('Estimate Detail')
        $('#FormEstimateDetail #IsUpdate').val('True');
        $('#FormEstimateDetail #ID').val(estimateDetail.ID);
        $("#FormEstimateDetail #ProductID").val(estimateDetail.ProductID)
        $("#FormEstimateDetail #hdnProductID").val(estimateDetail.ProductID)
        $('#divProductBasicInfo').load("Product/ProductBasicInfo?ID=" + $('#hdnProductID').val(), function () {
        });

        if ($('#hdnProductID').val() != _emptyGuid) {
            $('.divProductModelSelectList').load("ProductModel/ProductModelSelectList?required=required&productID=" + $('#hdnProductID').val())
        }
        else {
            $('.divProductModelSelectList').empty();
            $('.divProductModelSelectList').append('<span class="form-control newinput"><i id="dropLoad" class="fa fa-spinner"></i></span>');
        }
        $("#FormEstimateDetail #ProductModelID").val(estimateDetail.ProductModelID);
        $("#FormEstimateDetail #hdnProductModelID").val(estimateDetail.ProductModelID);
        if ($('#hdnProductModelID').val() != _emptyGuid) {
            $('#divProductBasicInfo').load("ProductModel/ProductModelBasicInfo?ID=" + $('#hdnProductModelID').val(), function () {
            });
        }
        $('#FormEstimateDetail #ProductSpec').val(estimateDetail.ProductSpec);
        $('#FormEstimateDetail #Qty').val(estimateDetail.Qty);
        $('#FormEstimateDetail #UnitCode').val(estimateDetail.UnitCode);
        $('#FormEstimateDetail #hdnUnitCode').val(estimateDetail.UnitCode);
        $('#FormEstimateDetail #CostRate').val(estimateDetail.CostRate);
        $('#FormEstimateDetail #SellingRate').val(estimateDetail.SellingRate);
        $('#FormEstimateDetail #DrawingNo').val(estimateDetail.DrawingNo);
        $('#divModelPopEstimate').modal('show');
    });
}

function ConfirmDeleteEstimateDetail(this_Obj) {
    debugger;
    _datatablerowindex = _dataTable.EstimateDetailList.row($(this_Obj).parents('tr')).index();
    var estimateDetail = _dataTable.EstimateDetailList.row($(this_Obj).parents('tr')).data();
    if (estimateDetail.ID === _emptyGuid) {
        notyConfirm('Are you sure to delete?', 'DeleteCurrentEstimateDetail("' + _datatablerowindex + '")');
    }
    else {
        notyConfirm('Are you sure to delete?', 'DeleteEstimateDetail("' + estimateDetail.ID + '")');

    }
}
function DeleteCurrentEstimateDetail(_datatablerowindex)
{
    var estimateDetailList = _dataTable.EstimateDetailList.rows().data();
    estimateDetailList.splice(_datatablerowindex, 1);
    _dataTable.EstimateDetailList.clear().rows.add(estimateDetailList).draw(false);
    notyAlert('success', 'Detail Row deleted successfully');
}
function DeleteEstimateDetail(ID) {
    debugger;
    if (ID != _emptyGuid && ID != null && ID != '') {
        var data = { "id": ID };
        var ds = {};
        _jsonData = GetDataFromServer("Estimate/DeleteEstimateDetail/", data);
        if (_jsonData != '') {
            _jsonData = JSON.parse(_jsonData);
            _message = _jsonData.Message;
            _status = _jsonData.Status;
            _result = _jsonData.Record;
        }
        if (_status == "OK") {
            notyAlert('success', _result.Message);
            var estimateDetailList = _dataTable.EstimateDetailList.rows().data();
            estimateDetailList.splice(_datatablerowindex, 1);
            _dataTable.EstimateDetailList.clear().rows.add(estimateDetailList).draw(false);
        }
        if (_status == "ERROR") {
            notyAlert('error', _message);
        }
    }
}
function EditRedirectToDocument(id) {

    OnServerCallBegin();

    $("#divEstimateForm").load("Estimate/EstimateForm?id=" + id, function (responseTxt, statusTxt, xhr) {
        if (statusTxt == "success") {
            OnServerCallComplete();
            openNav();
            $('#lblEstimateInfo').text($('#EstimateNo').val());
            if ($('#IsDocLocked').val() == "True") {
                ChangeButtonPatchView("Estimate", "btnPatchEstimateNew", "Edit", id);
            }
            else {
                ChangeButtonPatchView("Estimate", "btnPatchEstimateNew", "LockDocument");
            }
            BindEstimateDetailList(id);
            $('#divCustomerBasicInfo').load("Customer/CustomerBasicInfo?ID=" + $('#hdnCustomerID').val());
            clearUploadControl();
            PaintImages(id);
        }
        else {
            console.log("Error: " + xhr.status + ": " + xhr.statusText);
        }
    });
}