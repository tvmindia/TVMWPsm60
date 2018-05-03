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
        BindOrReloadEnquiryTable('Init');
        $('#tblEnquiry tbody').on('dblclick', 'td', function () {
            EditEnquiry(this);
        });
    }
    catch (e) {
        console.log(e.message);
    }
});
//function bind the Enquiry list checking search and filter
function BindOrReloadEnquiryTable(action) {
    try {
        //creating advancesearch object
        EnquiryAdvanceSearchViewModel = new Object();
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
        EnquiryAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
        EnquiryAdvanceSearchViewModel.SearchTerm = $('#SearchTerm').val();
        EnquiryAdvanceSearchViewModel.FromDate = $('#FromDate').val();
        EnquiryAdvanceSearchViewModel.ToDate = $('#ToDate').val();
        //apply datatable plugin on Enquiry table
        _dataTable.EnquiryList = $('#tblEnquiry').DataTable(
        {
            dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
            buttons: [{
                extend: 'excel',
                exportOptions:
                             {
                                 columns: [0,1, 2, 3, 4, 5, 6,7]
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
                url: "Enquiry/GetAllEnquiry/",
                data: { "EnquiryAdvanceSearchVM": EnquiryAdvanceSearchViewModel },
                type: 'POST'
            },
            pageLength: 13,
            columns: [
               { "data": "EnquiryNo", "defaultContent": "<i>-</i>" },
               { "data": "EnquiryDateFormatted", "defaultContent": "<i>-</i>" },
               { "data": "Customer.CompanyName", "defaultContent": "<i>-</i>" },
               { "data": "Customer.ContactPerson", "defaultContent": "<i>-</i>" },
               { "data": "Customer.Mobile", "defaultContent": "<i>-</i>" },
               { "data": "RequirementSpec", "defaultContent": "<i>-</i>" },
               { "data": "ReferencePerson.Name", "defaultContent": "<i>-</i>" },
               { "data": "DocumentStatus.Description", "defaultContent": "<i>-</i>" },
               { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink"  onclick="EditEnquiry(this)" ><i class="fa fa-pencil-square-o" aria-hidden="true"></i></a>' },
            ],
            columnDefs: [ { className: "text-right", "targets": [] },
                          { className: "text-left", "targets": [2,3,5,6] },
                          { className: "text-center", "targets": [0, 1, 4, 7, 8] },
                            { "targets": [0,1,4], "width": "10%" },
                            { "targets": [2,3], "width": "10%" },
                            { "targets": [5], "width": "30%" },
                            { "targets": [6], "width": "10%" },
                            { "targets": [7,8], "width": "5%" },
                        ],
            destroy: true,
            //for performing the import operation after the data loaded
            initComplete: function (settings, json) {
                debugger;
                $('.dataTables_wrapper div.bottom div').addClass('col-md-6');
                $('#tblEnquiry').fadeIn(100);
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
                    BindOrReloadEnquiryTable();
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
function ResetEnquiryList() {
    $(".searchicon").removeClass('filterApplied');
    BindOrReloadEnquiryTable('Reset');
}
//function export data to excel
function ExportEnquiryData() {
    $('.excelExport').show();
    OnServerCallBegin();
    BindOrReloadEnquiryTable('Export');
}
// add Enquiry section
function AddEnquiry() {
    //this will return form body(html)
    OnServerCallBegin();
    $("#divEnquiryForm").load("Enquiry/EnquiryForm?id=" + _emptyGuid, function () {
        ChangeButtonPatchView("Enquiry", "btnPatchEnquiryNew", "Add");
        BindEnquiryDetailList(_emptyGuid);
        OnServerCallComplete();
        //setTimeout(function () {
            //resides in customjs for sliding
            openNav();
        //}, 100);
    });
}
function EditEnquiry(this_Obj) {
    OnServerCallBegin();
    var Enquiry = _dataTable.EnquiryList.row($(this_Obj).parents('tr')).data();
    //this will return form body(html)
    $("#divEnquiryForm").load("Enquiry/EnquiryForm?id=" + Enquiry.ID, function () {
        //$('#CustomerID').trigger('change');
        ChangeButtonPatchView("Enquiry", "btnPatchEnquiryNew", "Edit");
        BindEnquiryDetailList(Enquiry.ID);
        $('#divCustomerBasicInfo').load("Customer/CustomerBasicInfo?ID=" + $('#hdnCustomerID').val());
        clearUploadControl();
        PaintImages(Enquiry.ID);
        OnServerCallComplete();
        setTimeout(function () {
            //resides in customjs for sliding
            openNav();
        }, 100);
    });
}
function ResetEnquiry() {
    $("#divEnquiryForm").load("Enquiry/EnquiryForm?id=" + $('#EnquiryForm #ID').val(), function () {
        BindEnquiryDetailList($('#ID').val());
        clearUploadControl();
        PaintImages($('#EnquiryForm #ID').val());
        $('#divCustomerBasicInfo').load("Customer/CustomerBasicInfo?ID=" + $('#EnquiryForm #hdnCustomerID').val());
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
        debugger;
        var _jsonData = JSON.parse(data)
        //message field will return error msg only
        _message = _jsonData.Message;
        _status = _jsonData.Status;
        _result = _jsonData.Record;
        switch (_status) {
            case "OK":
                $('#IsUpdate').val('True');
                $("#divEnquiryForm").load("Enquiry/EnquiryForm?id=" + _result.ID, function () {
                    ChangeButtonPatchView("Enquiry", "btnPatchEnquiryNew", "Edit");
                    BindEnquiryDetailList(_result.ID);
                    clearUploadControl();
                    PaintImages(_result.ID);
                    $('#divCustomerBasicInfo').load("Customer/CustomerBasicInfo?ID=" + $('#EnquiryForm #hdnCustomerID').val());
                });
                ChangeButtonPatchView("Enquiry", "btnPatchEnquiryNew", "Edit");
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
             { "data": "Product.Code", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
             { "data": "Product.Name", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
             { "data": "ProductModel.Name", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
             { "data": "ProductSpec", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
             {
                 "data": "Qty", render: function (data, type, row) {
                     return data +" "+ row.Unit.Description
                 }, "defaultContent": "<i></i>"
             },
             { "data": "Rate", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
             { "data": null, "orderable": false, "defaultContent": '<a href="#" class="DeleteLink"  onclick="ConfirmDeleteEnquiryDetail(this)" ><i class="fa fa-trash-o" aria-hidden="true"></i></a> <a href="#" class="actionLink"  onclick="EditEnquiryDetail(this)" ><i class="fa fa-pencil-square-o" aria-hidden="true"></i></a>' },
             ],
             columnDefs: [
                 { "targets": [0, 4], "width": "10%" },
                 { "targets": [1, 2], "width": "15%" },
                 { "targets": [3], "width": "35%" },
                 { "targets": [5], "width": "10%" },
                 { "targets": [6], "width": "5%" },
                 { className: "text-right", "targets": [4,5] },
                 { className: "text-left", "targets": [1,2, 3] },
                 { className: "text-center", "targets": [0, 6] }
             ]
         });
}
function GetEnquiryDetailListByEnquiryID(id) {
    try {
        debugger;
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
    $("#divModelEnquiryPopBody").load("Enquiry/AddEnquiryDetail", function () {
        $('#lblModelPopEnquiry').text('Enquiry Detail')
        $('#divModelPopEnquiry').modal('show');
    });
}
function AddEnquiryDetailToList() {
    debugger;
    $("#FormEnquiryDetail").submit(function () { });
        debugger;
        if($('#FormEnquiryDetail #IsUpdate').val()=='True')
        {
            if (($('#ProductID').val() != "" )&& ($('#Rate').val() != "" )&& ($('#Qty').val() != "" )&& ($('#UnitCode').val() != ""))
            {
                debugger;
                var enquiryDetailList = _dataTable.EnquiryDetailList.rows().data();
                enquiryDetailList[_datatablerowindex].Product.Code = $("#ProductID").val() != "" ? $("#ProductID option:selected").text().split("-")[0].trim() : "";
                enquiryDetailList[_datatablerowindex].Product.Name = $("#ProductID").val() != "" ? $("#ProductID option:selected").text().split("-")[1].trim() : "";
                enquiryDetailList[_datatablerowindex].ProductID = $("#ProductID").val() != "" ? $("#ProductID").val() : _emptyGuid;
                enquiryDetailList[_datatablerowindex].ProductModelID = $("#ProductModelID").val() != "" ? $("#ProductModelID").val() : _emptyGuid;
                ProductModel = new Object;
                Unit = new Object;
                ProductModel.Name = $("#ProductModelID").val() != "" ? $("#ProductModelID option:selected").text() : "";
                enquiryDetailList[_datatablerowindex].ProductModel = ProductModel;
                enquiryDetailList[_datatablerowindex].ProductSpec = $('#ProductSpec').val();
                enquiryDetailList[_datatablerowindex].Qty = $('#Qty').val();
                enquiryDetailList[_datatablerowindex].UnitCode = $('#UnitCode').val();
                Unit.Description=$("#UnitCode").val() != "" ? $("#UnitCode option:selected").text().trim() : "";
                enquiryDetailList[_datatablerowindex].Unit = Unit;
                enquiryDetailList[_datatablerowindex].Rate = $('#Rate').val();
                _dataTable.EnquiryDetailList.clear().rows.add(enquiryDetailList).draw(false);
                $('#divModelPopEnquiry').modal('hide');
                _datatablerowindex = -1;
            }
        }
        else
        {
            if (($('#ProductID').val() != "") && ($('#Rate').val() != "") && ($('#Qty').val() != "") && ($('#UnitCode').val() != ""))
            {
                if (_dataTable.EnquiryDetailList.rows().data().length === 0) {
                    _dataTable.EnquiryDetailList.clear().rows.add(GetEnquiryDetailListByEnquiryID(_emptyGuid)).draw(false);
                    debugger;
                    var enquiryDetailList = _dataTable.EnquiryDetailList.rows().data();
                    enquiryDetailList[0].Product.Code = $("#ProductID").val() != "" ? $("#ProductID option:selected").text().split("-")[0].trim() : "";
                    enquiryDetailList[0].Product.Name = $("#ProductID").val() != "" ? $("#ProductID option:selected").text().split("-")[1].trim() : "";
                    enquiryDetailList[0].ProductID = $("#ProductID").val() != "" ? $("#ProductID").val() : _emptyGuid;
                    enquiryDetailList[0].ProductModelID = $("#ProductModelID").val() != "" ? $("#ProductModelID").val() : _emptyGuid;
                    enquiryDetailList[0].ProductModel.Name = $("#ProductModelID").val() != "" ? $("#ProductModelID option:selected").text() : "";
                    enquiryDetailList[0].ProductSpec = $('#ProductSpec').val();
                    enquiryDetailList[0].Qty = $('#Qty').val();
                    enquiryDetailList[0].UnitCode = $('#UnitCode').val();
                    enquiryDetailList[0].Unit.Description = $("#UnitCode").val() != "" ? $("#UnitCode option:selected").text().trim() : "";
                    enquiryDetailList[0].Rate = $('#Rate').val();
                    _dataTable.EnquiryDetailList.clear().rows.add(enquiryDetailList).draw(false);
                    $('#divModelPopEnquiry').modal('hide');
                }
                else {
                    debugger;
                    var EnquiryDetailVM = new Object();
                    var Product = new Object;
                    var ProductModel = new Object()
                    var Unit = new Object();
                    EnquiryDetailVM.ID = _emptyGuid;
                    EnquiryDetailVM.ProductID = $("#ProductID").val() != "" ? $("#ProductID").val() : _emptyGuid;
                    Product.Code = $("#ProductID").val() != "" ? $("#ProductID option:selected").text().split("-")[0].trim() : "";
                    Product.Name = $("#ProductID").val() != "" ? $("#ProductID option:selected").text().split("-")[1].trim() : "";
                    EnquiryDetailVM.Product = Product;
                    EnquiryDetailVM.ProductModelID = $("#ProductModelID").val() != "" ? $("#ProductModelID").val() : _emptyGuid;
                    ProductModel.Name = $("#ProductModelID").val() != "" ? $("#ProductModelID option:selected").text() : "";
                    EnquiryDetailVM.ProductModel = ProductModel;
                    EnquiryDetailVM.ProductSpec = $('#ProductSpec').val();
                    EnquiryDetailVM.Qty = $('#Qty').val();
                    Unit.Description = $("#UnitCode").val() != "" ? $("#UnitCode option:selected").text().trim() : "";
                    EnquiryDetailVM.Unit = Unit;
                    EnquiryDetailVM.UnitCode = $('#UnitCode').val();
                    EnquiryDetailVM.Rate = $('#Rate').val();
                    _dataTable.EnquiryDetailList.row.add(EnquiryDetailVM).draw(true);
                    $('#divModelPopEnquiry').modal('hide');
                }
            }
                
        }
}
function EditEnquiryDetail(this_Obj)
{
    debugger;
    _datatablerowindex = _dataTable.EnquiryDetailList.row($(this_Obj).parents('tr')).index();
    var enquiryDetail = _dataTable.EnquiryDetailList.row($(this_Obj).parents('tr')).data();
    $("#divModelEnquiryPopBody").load("Enquiry/AddEnquiryDetail", function () {
        $('#lblModelPopEnquiry').text('Enquiry Detail')
        $('#FormEnquiryDetail #IsUpdate').val('True');
        $('#FormEnquiryDetail #ID').val(enquiryDetail.ID);
        $("#FormEnquiryDetail #ProductID").val(enquiryDetail.ProductID)
        $("#FormEnquiryDetail #hdnProductID").val(enquiryDetail.ProductID)
        $('#divProductBasicInfo').load("Product/ProductBasicInfo?ID="+$('#hdnProductID').val(), function () {
        });
        
        if ($('#hdnProductID').val() != _emptyGuid) {
            $('.divProductModelSelectList').load("ProductModel/ProductModelSelectList?required=required&productID=" + $('#hdnProductID').val())
        }
        else {
            $('.divProductModelSelectList').empty();
            $('.divProductModelSelectList').append('<span class="form-control newinput"><i id="dropLoad" class="fa fa-spinner"></i></span>');
        }
        $("#FormEnquiryDetail #ProductModelID").val(enquiryDetail.ProductModelID);
        $("#FormEnquiryDetail #hdnProductModelID").val(enquiryDetail.ProductModelID);
        if($('#hdnProductModelID').val()!=_emptyGuid)
        {
            $('#divProductBasicInfo').load("ProductModel/ProductModelBasicInfo?ID=" + $('#hdnProductModelID').val(), function () {
            });
        }
        $('#FormEnquiryDetail #ProductSpec').val(enquiryDetail.ProductSpec);
        $('#FormEnquiryDetail #Qty').val(enquiryDetail.Qty);
        $('#FormEnquiryDetail #UnitCode').val(enquiryDetail.UnitCode);
        $('#FormEnquiryDetail #hdnUnitCode').val(enquiryDetail.UnitCode);
        $('#FormEnquiryDetail #Rate').val(enquiryDetail.Rate);
        $('#divModelPopEnquiry').modal('show');
    });
}
function ConfirmDeleteEnquiryDetail(this_Obj) {
    debugger;
    _datatablerowindex = _dataTable.EnquiryDetailList.row($(this_Obj).parents('tr')).index();
    var enquiryDetail = _dataTable.EnquiryDetailList.row($(this_Obj).parents('tr')).data();
    if (enquiryDetail.ID === _emptyGuid) {
        var enquiryDetailList = _dataTable.EnquiryDetailList.rows().data();
        enquiryDetailList.splice(_datatablerowindex, 1);
        _dataTable.EnquiryDetailList.clear().rows.add(enquiryDetailList).draw(false);
        notyAlert('success', 'Detail Row deleted successfully');
    }
    else {
        notyConfirm('Are you sure to delete?', 'DeleteEnquiryDetail("' + enquiryDetail.ID + '")');

    }
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
    debugger;
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
    debugger;
    $("#divModelEnquiryPopBody").load("EnquiryFollowup/AddEnquiryFollowup?id=" + id + "&enquiryID=" + $('#EnquiryForm input[type="hidden"]#ID').val()+"&customerID="+_emptyGuid , function () {
        $('#lblModelPopEnquiry').text('Edit Enquiry Followup')
        $('#divModelPopEnquiry').modal('show');
    });
}
function SaveSuccessEnquiryFollowup(data, status) {
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