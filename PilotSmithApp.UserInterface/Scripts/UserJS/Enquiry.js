﻿var _dataTable = {};
var _emptyGuid = "00000000-0000-0000-0000-000000000000";
var _datatablerowindex = -1;
var _jsonData = {};
var _message = "";
var _status = "";
var _result = "";
var _SlNo = 1;
//---------------------------------------Docuement Ready--------------------------------------------------//
$(document).ready(function () {
    try {
        BindOrReloadEnquiryTable('Init');
        $('#tblEnquiry tbody').on('dblclick', 'td', function () {
            if (this.textContent !== "No data available in table")
            EditEnquiry(this);
        });
        if ($('#RedirectToDocument').val() != "") {
            if ($('#RedirectToDocument').val() === _emptyGuid) {
                AddEnquiry();
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
 function BindOrReloadEnquiryTable(action) {
    try {
        
        //creating advancesearch object
        EnquiryAdvanceSearchViewModel = new Object();
        DataTablePagingViewModel = new Object();
        DataTablePagingViewModel.Length = 0;
        var SerachValue = $('#hdnSearchTerm').val();
        var SearchTerm = $('#SearchTerm').val();
        $('#hdnSearchTerm').val($('#SearchTerm').val())
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
                if ((SearchTerm == SerachValue) && ($('.divboxASearch #AdvFromDate').val() == "") && ($('#AdvToDate').val() == "") && ($('.divboxASearch #AdvAreaCode').val() == "") && ($('.divboxASearch #AdvCustomerID').val() == "") && ($('.divboxASearch #AdvReferencePersonCode').val() == "") && ($('.divboxASearch #AdvBranchCode').val() == "") && ($('.divboxASearch #AdvDocumentStatusCode').val() == "") && ($('.divboxASearch #AdvDocumentOwnerID').val() == "")) {
                    return true;
                }
                break;
            case 'Export':
                DataTablePagingViewModel.Length = -1;
                EnquiryAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
                EnquiryAdvanceSearchViewModel.SearchTerm = $('#SearchTerm').val()==""?null: $('#SearchTerm').val();
                EnquiryAdvanceSearchViewModel.AdvFromDate = $('.divboxASearch #AdvFromDate').val()== "" ? null : $('.divboxASearch #AdvFromDate').val();
                EnquiryAdvanceSearchViewModel.AdvToDate = $('.divboxASearch #AdvToDate').val() == "" ? null : $('.divboxASearch #AdvToDate').val();
                EnquiryAdvanceSearchViewModel.AdvAreaCode = $('.divboxASearch #AdvAreaCode').val();
                EnquiryAdvanceSearchViewModel.AdvCustomerID = $('.divboxASearch #AdvCustomerID').val() == "" ? _emptyGuid : $('.divboxASearch #AdvCustomerID').val();
                EnquiryAdvanceSearchViewModel.AdvReferencePersonCode = $('.divboxASearch #AdvReferencePersonCode').val();
                EnquiryAdvanceSearchViewModel.AdvBranchCode = $('.divboxASearch #AdvBranchCode').val();
                EnquiryAdvanceSearchViewModel.AdvDocumentStatusCode = $('.divboxASearch #AdvDocumentStatusCode').val();
                EnquiryAdvanceSearchViewModel.AdvDocumentOwnerID = $('.divboxASearch #AdvDocumentOwnerID').val() == "" ? _emptyGuid : $('.divboxASearch #AdvDocumentOwnerID').val();
                $('#AdvanceSearch').val(JSON.stringify(EnquiryAdvanceSearchViewModel));
                $('#FormExcelExport').submit();               
                return true;
                break;
            default:
                break;
        }
        EnquiryAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
        EnquiryAdvanceSearchViewModel.SearchTerm = $('#SearchTerm').val();
        EnquiryAdvanceSearchViewModel.AdvFromDate = $('.divboxASearch #AdvFromDate').val();
        EnquiryAdvanceSearchViewModel.AdvToDate = $('.divboxASearch #AdvToDate').val();
        EnquiryAdvanceSearchViewModel.AdvAreaCode = $('.divboxASearch #AdvAreaCode').val();
        EnquiryAdvanceSearchViewModel.AdvCustomerID = $('.divboxASearch #AdvCustomerID').val();
        EnquiryAdvanceSearchViewModel.AdvReferencePersonCode = $('.divboxASearch #AdvReferencePersonCode').val();
        EnquiryAdvanceSearchViewModel.AdvBranchCode = $('.divboxASearch #AdvBranchCode').val();
        EnquiryAdvanceSearchViewModel.AdvDocumentStatusCode = $('.divboxASearch #AdvDocumentStatusCode').val();
        EnquiryAdvanceSearchViewModel.AdvDocumentOwnerID = $('.divboxASearch #AdvDocumentOwnerID').val();
        //apply datatable plugin on Enquiry table
        _dataTable.EnquiryList = $('#tblEnquiry').DataTable(
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
                url: "Enquiry/GetAllEnquiry/",
                data: { "EnquiryAdvanceSearchVM": EnquiryAdvanceSearchViewModel },
                type: 'POST'
            },
            pageLength: 8,
            columns: [
               {
                   "data": "EnquiryNo", render: function (data, type, row) {
                       return (data == null ? " " : data) + "<br/>" + "<img src='./Content/images/datePicker.png' height='10px'>" + "&nbsp;" + (row.EnquiryDateFormatted == null ? " " : row.EnquiryDateFormatted)

                   }, "defaultContent": "<i>-</i>"
               },
               {
                   "data": "Customer.CompanyName", render: function (data, type, row) {
                      
                       return "<img src='./Content/images/contact.png' height='10px'>" + "&nbsp;" + (row.Customer.ContactPerson == null ? " " : row.Customer.ContactPerson) + "</br>" + "<img src='./Content/images/organisation.png' height='10px'>" + "&nbsp;" + (data == null ? " " : data);

                   }, "defaultContent": "<i>-</i>"
               },
               {
                   "data": "RequirementSpec", render: function (data, type, row) {
                       return '<div class="show-popover" data-html="true" data-toggle="popover" data-title="<p align=left>Requirement Specification" data-content="' + data + '</p>' + (data == null ? " " : data.substring(0, 50)+(data.length>50?'...':''))//if (data.length > 30){var newdata = data.substring(0, 30);return newdata + ' <a style="color:rgba(94, 66, 209, 0.8);"> More.. ▼</a>';}else{return data ;}

                   }, "defaultContent": "<i>-</i>"
               },
               { "data": "Area.Description", "defaultContent": "<i>-</i>" },
               { "data": "ReferencePerson.Name", "defaultContent": "<i>-</i>" },
               { "data": "PSAUser.LoginName", "defaultContent": "<i>-</i>" },
               {
                   "data": "DocumentStatus.Description", render: function (data, type, row) {
                       return "<b>Doc.Status-</b>" + (data == null ? " " : data) + "</br>" + "<b>Branch-</b>" + (row.Branch.Description == null ? " " : row.Branch.Description);
                   }, "defaultContent": "<i>-</i>"
               },
               { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink"  onclick="EditEnquiry(this)" ><i class="fa fa-pencil-square-o" aria-hidden="true"></i></a>' },
            ],
            columnDefs: [ { className: "text-right", "targets": [] },
                          { className: "text-left", "targets": [0,1,2,3,4,5,6] },
                          { className: "text-center", "targets": [ 7] },
                            { "targets": [3, 4, 5], "width": "10%" },
                            {"targets":[0],"width":"12%"},
                            { "targets": [1], "width": "13%" },
                            { "targets": [2,6], "width": "20%" },
                            { "targets": [7], "width": "5%" },
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
                
                $('.dataTables_wrapper div.bottom div').addClass('col-md-6');
                $('#tblEnquiry').fadeIn(100);
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
function ResetEnquiryList() {
    $(".searchicon").removeClass('filterApplied');
    BindOrReloadEnquiryTable('Reset');
}
//function export data to excel
function ExportEnquiryData() {
    BindOrReloadEnquiryTable('Export');
}
// add Enquiry section
function AddEnquiry() {
    //this will return form body(html)
    $('#lblEnquiryInfo').text("<<Enquiry No.>>");
    OnServerCallBegin();
    $("#divEnquiryForm").load("Enquiry/EnquiryForm?id=" + _emptyGuid, function (responseTxt, statusTxt, xhr) {
        if (statusTxt == "success")
        {
            OnServerCallComplete();
            openNav();
            ChangeButtonPatchView("Enquiry", "btnPatchEnquiryNew", "Add");
            BindEnquiryDetailList(_emptyGuid);
        }
        else {
            console.log("Error: " + xhr.status + ": " + xhr.statusText);
        }
    });
}

function EditEnquiry(this_Obj) {
    OnServerCallBegin();
    var Enquiry = _dataTable.EnquiryList.row($(this_Obj).parents('tr')).data();
    $('#lblEnquiryInfo').text(Enquiry.EnquiryNo);
    $("#divEnquiryForm").load("Enquiry/EnquiryForm?id=" + Enquiry.ID, function (responseTxt, statusTxt, xhr) {
        if (statusTxt == "success") {
            OnServerCallComplete();
            openNav();
            if ($('#IsDocLocked').val() == "True") {
                ChangeButtonPatchView("Enquiry", "btnPatchEnquiryNew", "Edit", Enquiry.ID);
            }
            else {
                debugger;
                //$('.switch-input').prop('disabled', true);
                //$('.switch-label,.switch-handle').addClass('switch-disabled').addClass('disabled');
                //$('.switch-label').attr('title', 'Document Locked');
                ChangeButtonPatchView("Enquiry", "btnPatchEnquiryNew", "LockDocument", Enquiry.ID);
            }
            _SlNo = 1;
            BindEnquiryDetailList(Enquiry.ID);
            $('#divCustomerBasicInfo').load("Customer/CustomerBasicInfo?ID=" + $('#hdnCustomerID').val());
            clearUploadControl();
            PaintImages(Enquiry.ID);
            //if (Enquiry.DocumentStatus.Description == "OPEN") {
            //    $('.switch-input').prop('checked', true);
            //    //  $('.switch-input').attr(':checked', ':checked');
            //    //$('.switch-input').is(':checked') = true;
            //} else {
            //    $('.switch-input').prop('checked', false);
            //    //  $('.switch-input').removeAttr(':checked', ':checked');
            //    // $('.switch-input').is(':checked') = false;
            //}
        }
        else {
            console.log("Error: " + xhr.status + ": " + xhr.statusText);
        }
    });
}
function ResetEnquiry() {
    $("#divEnquiryForm").load("Enquiry/EnquiryForm?id=" + $('#EnquiryForm #ID').val(), function (responseTxt, statusTxt, xhr) {
        if (statusTxt == "success") {
            //if ($('#hdnDescription').val() == "OPEN") {
            //    $('.switch-input').prop('checked', true);
              
            //} else {
            //    $('.switch-input').prop('checked', false);
              
            //}
            $('#lblEnquiryInfo').text($('#EnquiryNo').val());
            _SlNo = 1;
            BindEnquiryDetailList($('#ID').val());
            clearUploadControl();
            $('#divCustomerBasicInfo').load("Customer/CustomerBasicInfo?ID=" + $('#EnquiryForm #hdnCustomerID').val());
            PaintImages($('#EnquiryForm #ID').val());
           

        }
        else {
            console.log("Error: " + xhr.status + ": " + xhr.statusText);
        }
        
    });
}
function SaveEnquiry() {
    var enquiryDetailList = _dataTable.EnquiryDetailList.rows().data().toArray();
    $('#DetailJSON').val(JSON.stringify(enquiryDetailList));
    $('#btnInsertUpdateEnquiry').trigger('click');
}
function ApplyFilterThenSearch() {
    $(".searchicon").addClass('filterApplied');
    CloseAdvanceSearch();
    BindOrReloadEnquiryTable('Search');
}
function SaveSuccessEnquiry(data, status) {
    try {
        
        var _jsonData = JSON.parse(data)
        //message field will return error msg only
        _message = _jsonData.Message;
        _status = _jsonData.Status;
        _result = _jsonData.Record;
        switch (_status) {
            case "OK":
                $('#IsUpdate').val('True');
                $('#lblEnquiryInfo').text(_result.EnquiryNo);
                $("#divEnquiryForm").load("Enquiry/EnquiryForm?id=" + _result.ID, function () {
                    if ($('#IsDocLocked').val() == "True")
                    {
                        ChangeButtonPatchView("Enquiry", "btnPatchEnquiryNew", "Edit", _result.ID);
                    }
                    else
                    {
                        ChangeButtonPatchView("Enquiry", "btnPatchEnquiryNew", "LockDocument", _result.ID);
                    }
                    _SlNo = 1;
                    BindEnquiryDetailList(_result.ID);
                    clearUploadControl();
                    PaintImages(_result.ID);
                    $('#divCustomerBasicInfo').load("Customer/CustomerBasicInfo?ID=" + $('#EnquiryForm #hdnCustomerID').val());
                    //if ($('#hdnDescription').val() == "OPEN") {
                    //    $('.switch-input').prop('checked', true);

                    //} else {
                    //    $('.switch-input').prop('checked', false);

                    //}
                });
                ChangeButtonPatchView("Enquiry", "btnPatchEnquiryNew", "Edit", _result.ID);
                BindOrReloadEnquiryTable('Init');
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

function DeleteEnquiry() {
    notyConfirm('Are you sure to delete?', 'DeleteEnquiryItem("' + $('#EnquiryForm #ID').val() + '")');
}
function DeleteEnquiryItem(id) {
    try {
        if (id) {
            var data = { "id": id };
            _jsonData = GetDataFromServer("Enquiry/DeleteEnquiry/", data);
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
                    ChangeButtonPatchView("Enquiry", "btnPatchEnquiryNew", "Add");
                    ResetEnquiry();
                    
                    BindOrReloadEnquiryTable('Init');
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
function BindEnquiryDetailList(id) {
    debugger;
    _dataTable.EnquiryDetailList = $('#tblEnquiryDetails').DataTable(
         {
             dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
             order: [],
             searching: false,
             paging: false,
             ordering: false,
             bInfo : false,
             data: id==_emptyGuid?null:GetEnquiryDetailListByEnquiryID(id),
             language: {
                 search: "_INPUT_",
                 searchPlaceholder: "Search"
             },
             columns: [
                 {
                     "data": "", render: function (data, type, row) {
                         debugger;
                         return _SlNo++
                     }, "defaultContent": "<i></i>", "width": "2%"
                 },
             {
                 "data": "Product.Code", render: function (data, type, row) {                   
                     return '<div style="width:100%" class="show-popover" data-html="true" data-toggle="popover" data-placement="top" data-title="<p align=left>Product Specification" data-content="' + (row.ProductSpec !== null ? row.ProductSpec.replace("\n", "<br>").replace(/"/g, "&quot") : "") + '</p>"/>' + row.Product.Name + "<br/>" + row.ProductModel.Name
                 }, "defaultContent": "<i></i>"
             },
             {
                 "data": "Product.HSNCode", "defaultContent": "<i></i>"
             },
             {
                 "data": "Qty", render: function (data, type, row) {
                     return data +" "+ row.Unit.Description
                 }, "defaultContent": "<i></i>"
             },
             { "data": "Rate", render: function (data, type, row) { return formatCurrency(roundoff(data,2)) }, "defaultContent": "<i></i>" },
             {
                 "data": "Rate", render: function (data, type, row) {

                     return formatCurrency(roundoff(parseFloat(data) * parseFloat(row.Qty), 2))
                 }, "defaultContent": "<i></i>"
             },
             { "data": null, "orderable": false, "defaultContent": ($('#IsDocLocked').val() == "True" || $('#IsUpdate').val() == "False") ? '<a href="#" class="actionLink"  onclick="EditEnquiryDetail(this)" ><i class="fa fa-pencil-square-o" aria-hidden="true"></i></a> <a href="#" class="DeleteLink"  onclick="ConfirmDeleteEnquiryDetail(this)" ><i class="fa fa-trash-o" aria-hidden="true"></i></a> ' : "-" },
             ],
             columnDefs: [
                 { "targets": [5,2,3], "width": "10%" },
                 { "targets": [1,4], "width": "20%" },
                 { "targets": [0], "width": "28%" },
                 { className: "text-right", "targets": [3,4,5] },
                 { className: "text-left", "targets": [1, 2] },
                 { className: "text-center", "targets": [0,6] }
             ]
         });
    $('[data-toggle="popover"]').popover({
        html: true,
        'trigger': 'hover'
    });
}
function BindCustomerHistory(id) {
    _SlNo = 1;
    _dataTable.CustomerHistory = $('#tblCustomerHistory').DataTable(
       {
           dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
           order: [],
           searching: false,
           paging: true,
           ordering: false,
           bInfo: false,
           ajax: {
               url: "SaleOrder/GetCustomerHistory/",
               data: { "CustomerID": id },
               type: 'POST'
           },
         pageLength:5,
           language: {
               search: "_INPUT_",
               searchPlaceholder: "Search"
           },
           columns: [
               {
                   "data": "", render: function (data, type, row) {
                       debugger;
                       return _SlNo++;
                       
                   }, "defaultContent": "<i></i>", "width": "2%"
               },
            { "data": "SaleOrderNo", "defaultContent": "<i></i>" },
             { "data": "SaleOrderDate", "defaultContent": "<i></i>" },
              { "data": "CurrencyCode", "defaultContent": "<i></i>" },
             {
                 "data": "SaleOrderValue", render: function (data, type, row) {
                     return roundoff(data)
                 }, "defaultContent": "<i></i>"
             },

           ],
           columnDefs: [             
               { "targets": [1, 4,2,3], "width": "22%" },
               { "targets": [0], "width": "12%" },
               { className: "text-right", "targets": [4] },
               { className: "text-center", "targets": [0,1,2,3] }
           ],
           destroy: true,
           initComplete: function (settings, json) {
               debugger;
               $('#lblTotalSaleOrderCount').text(json.data.length);
               $('#lblTotalSaleOrderValue').text(formatCurrency(roundoff(json.data[0].TotalOrderValue)));
           },
       });
    $('[data-toggle="popover"]').popover({
        html: true,
        'trigger': 'hover'
    });
}


function GetEnquiryDetailListByEnquiryID(id) {
    try {
        
        var data = { "enquiryID": id };
        var enquiryDetailList = [];
        _jsonData = GetDataFromServer("Enquiry/GetEnquiryDetailListByEnquiryID/", data);
        if (_jsonData != '') {
            _jsonData = JSON.parse(_jsonData);
            _message = _jsonData.Message;
            _status = _jsonData.Status;
            enquiryDetailList = _jsonData.Records;
        }
        if (_status == "OK") {
            return enquiryDetailList;
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
function AddEnquiryDetailList()
{
    $("#divModelEnquiryPopBody").load("Enquiry/AddEnquiryDetail?update=false", function () {
        $('#lblModelPopEnquiry').text('Enquiry Detail')
        $('#divModelPopEnquiry').modal('show');
    });
}
function AddEnquiryDetailToList() {
    debugger;
   
    $("#FormEnquiryDetail").submit(function () { });
        
        if($('#FormEnquiryDetail #IsUpdate').val()=='True')
        {
            if (($('#Rate').val() > 0) && ($('#Qty').val() > 0) && ($('#UnitCode').val() != ""))
            {
                
                var enquiryDetailList = _dataTable.EnquiryDetailList.rows().data();
               // enquiryDetailList[_datatablerowindex].Product.Code = $("#ProductID").val() != "" ? $("#ProductID option:selected").text().split("-")[0].trim() : "";
              //  enquiryDetailList[_datatablerowindex].Product.Name = $("#ProductID").val() != "" ? $("#ProductID option:selected").text().split("-")[1].trim() : "";
                //enquiryDetailList[_datatablerowindex].Product.Code = $('#spanProductName').text().split("-")[0].trim();
                enquiryDetailList[_datatablerowindex].Product.Code = $('#spanProductName').text() != "" ? $('#spanProductName').text().split("-")[0].trim() : "";
                enquiryDetailList[_datatablerowindex].Product.Name = $('#spanProductName').text() != "" ? $('#spanProductName').text().split("-")[1].trim() : "";
                    
             
                debugger;
                enquiryDetailList[_datatablerowindex].Product.HSNCode = $("#hdnProductHSNCode").val();
             //   enquiryDetailList[_datatablerowindex].ProductID = $("#ProductID").val() != "" ? $("#ProductID").val() : _emptyGuid;
              //  enquiryDetailList[_datatablerowindex].ProductModelID = $("#ProductModelID").val() != "" ? $("#ProductModelID").val() : _emptyGuid;
                ProductModel = new Object;
                Unit = new Object;
               ProductModel.Name = $('#spanProductModelName').text();
                //    ProductModel.Name = $("#ProductModelID").val() != "" ? $("#ProductModelID option:selected").text() : "";
                enquiryDetailList[_datatablerowindex].ProductModel = ProductModel;
                enquiryDetailList[_datatablerowindex].ProductSpec = $('#ProductSpec').val();
                enquiryDetailList[_datatablerowindex].Qty = $('#Qty').val() !=""? $('#Qty').val() :0 ;
                enquiryDetailList[_datatablerowindex].UnitCode = $('#UnitCode').val();
                Unit.Description=$("#UnitCode").val() != "" ? $("#UnitCode option:selected").text().trim() : "";
                enquiryDetailList[_datatablerowindex].Unit = Unit;
                enquiryDetailList[_datatablerowindex].Rate = $('#Rate').val() != "" ? $('#Rate').val() : 0;
                
                _dataTable.EnquiryDetailList.clear().rows.add(enquiryDetailList).draw(false);
                $('#divModelPopEnquiry').modal('hide');
                _datatablerowindex = -1;
            }
        }
        else
        {
            if (($('#ProductID').val() != "") && ($('#ProductModelID').val() != "") && ($('#Rate').val() > 0) && ($('#Qty').val() > 0) && ($('#UnitCode').val() != ""))
            {
                if (_dataTable.EnquiryDetailList.rows().data().length === 0) {
                    _SlNo = 0;
                    _dataTable.EnquiryDetailList.clear().rows.add(GetEnquiryDetailListByEnquiryID(_emptyGuid)).draw(false);
                    
                    var enquiryDetailList = _dataTable.EnquiryDetailList.rows().data();
                    enquiryDetailList[0].Product.Code = $("#ProductID").val() != "" ? $("#ProductID option:selected").text().split("-")[0].trim() : "";
                    enquiryDetailList[0].Product.Name = $("#ProductID").val() != "" ? $("#ProductID option:selected").text().split("-")[1].trim() : "";
                    debugger;
                    enquiryDetailList[0].Product.HSNCode = $("#hdnProductHSNCode").val();
                    enquiryDetailList[0].ProductID = $("#ProductID").val() != "" ? $("#ProductID").val() : _emptyGuid;
                    enquiryDetailList[0].ProductModelID = $("#ProductModelID").val() != "" ? $("#ProductModelID").val() : _emptyGuid;
                    enquiryDetailList[0].ProductModel.Name = $("#ProductModelID").val() != "" ? $("#ProductModelID option:selected").text() : "";
                    enquiryDetailList[0].ProductSpec = $('#ProductSpec').val();
                    enquiryDetailList[0].Qty = $('#Qty').val() != "" ? $('#Qty').val() : 0;
                    enquiryDetailList[0].UnitCode = $('#UnitCode').val();
                    enquiryDetailList[0].Unit.Description = $("#UnitCode").val() != "" ? $("#UnitCode option:selected").text().trim() : "";
                    enquiryDetailList[0].Rate = $('#Rate').val() != "" ? $('#Rate').val() : 0;
                   
                    _dataTable.EnquiryDetailList.clear().rows.add(enquiryDetailList).draw(false);
                    $('#divModelPopEnquiry').modal('hide');
                }
                else {
                    
                    var enquiryDetailList = _dataTable.EnquiryDetailList.rows().data();
                    if (enquiryDetailList.length > 0) {
                        var checkpoint = 0;
                        var productSpec = $('#ProductSpec').val();
                        productSpec = productSpec.replace(/\n/g, ' ');
                        for (var i = 0; i < enquiryDetailList.length; i++) {
                            if ((enquiryDetailList[i].ProductID == $('#ProductID').val()) && (enquiryDetailList[i].ProductModelID == $('#ProductModelID').val()
                                && (enquiryDetailList[i].ProductSpec == null ? "" : enquiryDetailList[i].ProductSpec.replace(/\n/g, ' ') == productSpec && (enquiryDetailList[i].UnitCode == $('#UnitCode').val())
                                && (enquiryDetailList[i].Rate == $('#Rate').val())))) {
                                enquiryDetailList[i].Qty = parseFloat(enquiryDetailList[i].Qty)+parseFloat($('#Qty').val());
                                checkpoint = 1;
                                break;
                            }
                        }
                        if (checkpoint == 1) {
                            _SlNo = 1;
                            _dataTable.EnquiryDetailList.clear().rows.add(enquiryDetailList).draw(false);
                            $('#divModelPopEnquiry').modal('hide');
                        }
                        else if (checkpoint == 0) {
                            if ($('#EnquiryForm #IsUpdate').val() == 'True')
                                _SlNo = _dataTable.EnquiryDetailList.rows().data().length + 1;
                            var EnquiryDetailVM = new Object();
                            var Product = new Object;
                            var ProductModel = new Object()
                            var Unit = new Object();
                            EnquiryDetailVM.ID = _emptyGuid;
                            EnquiryDetailVM.ProductID = $("#ProductID").val() != "" ? $("#ProductID").val() : _emptyGuid;
                            Product.Code = $("#ProductID").val() != "" ? $("#ProductID option:selected").text().split("-")[0].trim() : "";
                            Product.Name = $("#ProductID").val() != "" ? $("#ProductID option:selected").text().split("-")[1].trim() : "";
                            debugger;
                            Product.HSNCode = $("#hdnProductHSNCode").val();
                            EnquiryDetailVM.Product = Product;
                            EnquiryDetailVM.ProductModelID = $("#ProductModelID").val() != "" ? $("#ProductModelID").val() : _emptyGuid;
                            ProductModel.Name = $("#ProductModelID").val() != "" ? $("#ProductModelID option:selected").text() : "";
                            EnquiryDetailVM.ProductModel = ProductModel;
                            EnquiryDetailVM.ProductSpec = $('#ProductSpec').val();
                            EnquiryDetailVM.Qty = $('#Qty').val() != "" ? $('#Qty').val() : 0;
                            Unit.Description = $("#UnitCode").val() != "" ? $("#UnitCode option:selected").text().trim() : "";
                            EnquiryDetailVM.Unit = Unit;
                            EnquiryDetailVM.UnitCode = $('#UnitCode').val();
                            EnquiryDetailVM.Rate = $('#Rate').val() != "" ? $('#Rate').val() : 0;
                           
                            _dataTable.EnquiryDetailList.row.add(EnquiryDetailVM).draw(true);
                            $('#divModelPopEnquiry').modal('hide');
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
function EditEnquiryDetail(this_Obj)
{
    debugger;
    _SlNo = 1;
    _datatablerowindex = _dataTable.EnquiryDetailList.row($(this_Obj).parents('tr')).index();
    var enquiryDetail = _dataTable.EnquiryDetailList.row($(this_Obj).parents('tr')).data();
    $("#divModelEnquiryPopBody").load("Enquiry/AddEnquiryDetail?update=true", function (responseTxt, statusTxt, xhr) {
        if (statusTxt == 'success') {
            $('#lblModelPopEnquiry').text('Enquiry Detail')
            $('#FormEnquiryDetail #IsUpdate').val('True');
            $('#FormEnquiryDetail #ID').val(enquiryDetail.ID);
            //  $("#FormEnquiryDetail #ProductID").val(enquiryDetail.ProductID)
            $("#FormEnquiryDetail #hdnProductID").val(enquiryDetail.ProductID)
            $('#spanProductName').text(enquiryDetail.Product.Code + "-" + enquiryDetail.Product.Name)
            $('#spanProductModelName').text(enquiryDetail.ProductModel.Name)
            $('#divProductBasicInfo').load("Product/ProductBasicInfo?ID=" + $('#hdnProductID').val(), function (responseTxt, statusTxt, xhr) {
                if (statusTxt == 'success') {
                    debugger;
                    $("#FormEnquiryDetail #hdnProductModelID").val(enquiryDetail.ProductModelID);
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
            //  $("#FormEnquiryDetail #ProductModelID").val(enquiryDetail.ProductModelID);


            $('#FormEnquiryDetail #ProductSpec').val(enquiryDetail.ProductSpec);
            $('#FormEnquiryDetail #Qty').val(enquiryDetail.Qty);
            $('#FormEnquiryDetail #UnitCode').val(enquiryDetail.UnitCode);
            $('#FormEnquiryDetail #hdnUnitCode').val(enquiryDetail.UnitCode);
            $('#FormEnquiryDetail #Rate').val(enquiryDetail.Rate);
            $('#divModelPopEnquiry').modal('show');
        }
        else {
            console.log("Error: " + xhr.status + ": " + xhr.statusText);
        }
    });
}
function ConfirmDeleteEnquiryDetail(this_Obj) {
    _SlNo = 1;
    _datatablerowindex = _dataTable.EnquiryDetailList.row($(this_Obj).parents('tr')).index();
    var enquiryDetail = _dataTable.EnquiryDetailList.row($(this_Obj).parents('tr')).data();
    if (enquiryDetail.ID === _emptyGuid) {
        notyConfirm('Are you sure to delete?', 'DeleteCurrentEnquiryDetail("' +_datatablerowindex + '")');
    }
    else {
        notyConfirm('Are you sure to delete?', 'DeleteEnquiryDetail("' + enquiryDetail.ID + '")');
    }
}
function DeleteCurrentEnquiryDetail(_datatablerowindex) {
    var enquiryDetailList = _dataTable.EnquiryDetailList.rows().data();
    enquiryDetailList.splice(_datatablerowindex, 1);
    _dataTable.EnquiryDetailList.clear().rows.add(enquiryDetailList).draw(false);
    notyAlert('success', 'Detail Row deleted successfully');
}
function DeleteEnquiryDetail(ID) {
    if (ID != _emptyGuid && ID != null && ID !='') {
        var data = { "id": ID };
        var ds = {};
        _jsonData = GetDataFromServer("Enquiry/DeleteEnquiryDetail/", data);
        if (_jsonData != '') {
            _jsonData = JSON.parse(_jsonData);
            _message = _jsonData.Message;
            _status = _jsonData.Status;
            _result = _jsonData.Record;
        }
        if (_status == "OK") {
            notyAlert('success', _result.Message);
            var enquiryDetailList = _dataTable.EnquiryDetailList.rows().data();
            enquiryDetailList.splice(_datatablerowindex, 1);
            _dataTable.EnquiryDetailList.clear().rows.add(enquiryDetailList).draw(false);
        }
        if (_status == "ERROR") {
            notyAlert('error', _message);
        }
    }
}
//================================================================================================
//EnquiryFollowup Section
function AddEnquiryFollowUp()
{
    $("#divModelEnquiryPopBody").load("EnquiryFollowup/AddEnquiryFollowup?id="+_emptyGuid+ "&enquiryID=" + $('#EnquiryForm input[type="hidden"]#ID').val()+"&customerID=" + ($('#EnquiryForm #hdnCustomerID').val()!=""?$('#EnquiryForm #hdnCustomerID').val():_emptyGuid), function () {
        $('#lblModelPopEnquiry').text('Add Enquiry Followup')
        $('#btnresetEnquiryFollowup').trigger('click');
        $('#divModelPopEnquiry').modal('show');

    });
}
function EnquiryFollowUpPaging(start)
{
    $("#divEnquiryFollowupboxbody").load("EnquiryFollowup/GetEnquiryFollowupList?ID=" + _emptyGuid + "&EnquiryID=" + $('#EnquiryForm input[type="hidden"]#ID').val() + "&DataTablePaging.Start=" + start, function () {
       
    });
}
function EditEnquiryFollowup(id)
{
    
    $("#divModelEnquiryPopBody").load("EnquiryFollowup/AddEnquiryFollowup?id=" + id + "&enquiryID=" + $('#EnquiryForm input[type="hidden"]#ID').val()+"&customerID="+_emptyGuid , function () {
        $('#lblModelPopEnquiry').text('Edit Enquiry Followup')
        $('#divModelPopEnquiry').modal('show');
    });
}
function SaveSuccessEnquiryFollowup(data, status) {
    try {
        
        var _jsonData = JSON.parse(data)
        //message field will return error msg only
        _message = _jsonData.Message;
        _status = _jsonData.Status;
        _result = _jsonData.Record;
        switch (_status) {
            case "OK":
                MasterAlert("success", _result.Message)
                $("#divModelEnquiryPopBody").load("EnquiryFollowup/AddEnquiryFollowup?ID=" + _result.ID + "&EnquiryID=" + $('#EnquiryForm input[type="hidden"]#ID').val(),"&customerID="+_emptyGuid, function () {
                    $('#lblModelPopEnquiry').text('Edit Enquiry Followup')
                    
                });
                $("#divFollowupList").load("EnquiryFollowup/GetEnquiryFollowupList?ID=" + _emptyGuid + "&EnquiryID=" + $('#EnquiryForm input[type="hidden"]#ID').val(), function () {
                });
                break;
            case "ERROR":
                MasterAlert("danger", _message)
                break;
            default:
                console.log(_message);
                break;
        }
    }
    catch (e) {
        //this will show the error msg in the browser console(F12) 
        console.log(e.message);
    }
}
function ConfirmDeleteEnquiryFollowup(ID) {
    if (ID != _emptyGuid)
    {
        notyConfirm('Are you sure to delete?', 'DeleteEnquiryFollowup("' + ID + '")');
    }
}
function DeleteEnquiryFollowup(ID) {
    if (ID != _emptyGuid && ID != null && ID != '') {
        var data = { "id": ID };
        var ds = {};
        _jsonData = GetDataFromServer("EnquiryFollowup/DeleteEnquiryFollowup/", data);
        if (_jsonData != '') {
            _jsonData = JSON.parse(_jsonData);
            _message = _jsonData.Message;
            _status = _jsonData.Status;
            _result = _jsonData.Record;
        }
        if (_status == "OK") {
            notyAlert('success', _result.Message);
            $("#divFollowupList").load("EnquiryFollowup/GetEnquiryFollowupList?ID=" + _emptyGuid + "&EnquiryID=" + $('#EnquiryForm input[type="hidden"]#ID').val(), function () {
            });
        }
        if (_status == "ERROR") {
            notyAlert('error', _message);
        }
    }
}
//=====================Disabled functions===================================
//==========================================================================
function EditRedirectToDocument(id)
{
    debugger;
    OnServerCallBegin();
   
    $("#divEnquiryForm").load("Enquiry/EnquiryForm?id=" + id, function (responseTxt, statusTxt, xhr) {
        debugger;
        if (statusTxt == "success") {
            OnServerCallComplete();
            openNav();
            $('#lblEnquiryInfo').text($('#EnquiryNo').val());
            if ($('#IsDocLocked').val() == "True") {
                ChangeButtonPatchView("Enquiry", "btnPatchEnquiryNew", "Edit", id);
            }
            else {
                //$('.switch-label,.switch-handle').addClass('switch-disabled').addClass('disabled');
                //$('.switch-input').prop('disabled', true);
                //$('.switch-label').attr('title', 'Document Locked');
                ChangeButtonPatchView("Enquiry", "btnPatchEnquiryNew", "LockDocument", id);
            }
            _SlNo = 1;
            BindEnquiryDetailList(id);
            $('#divCustomerBasicInfo').load("Customer/CustomerBasicInfo?ID=" + $('#hdnCustomerID').val());
            clearUploadControl();
            PaintImages(id);
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

function ClearEnquiryform() {
    debugger;
    ResetEnquiry();
    $('.showSweetAlert .cancel').click();
}