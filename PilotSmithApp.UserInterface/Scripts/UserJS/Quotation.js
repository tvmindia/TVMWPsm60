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
    $("#divQuotationForm").load("Quotation/QuotationForm?id=" + _emptyGuid, function () {
        ChangeButtonPatchView("Quotation", "btnPatchQuotationNew", "Add");
        BindQuotationDetailList(_emptyGuid);
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
    $("#divQuotationForm").load("Quotation/QuotationForm?id=" + Quotation.ID, function () {
        //$('#CustomerID').trigger('change');
        ChangeButtonPatchView("Quotation", "btnPatchQuotationNew", "Edit");
        //BindQuotationDetailList(Quotation.ID);
        $('#divCustomerBasicInfo').load("Customer/CustomerBasicInfo?ID=" + $('#hdnCustomerID').val());
        clearUploadControl();
        PaintImages(Quotation.ID);
        OnServerCallComplete();
        setTimeout(function () {
            //resides in customjs for sliding
            openNav();
        }, 100);
    });
}
function ResetQuotation() {
    $("#divQuotationForm").load("Quotation/QuotationForm?id=" + $('#QuotationForm #ID').val(), function () {
        BindQuotationDetailList($('#ID').val());
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
                $("#divQuotationForm").load("Quotation/QuotationForm?id=" + _result.ID, function () {
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
function BindQuotationDetailList(id) {
    debugger;
    _dataTable.QuotationDetailList = $('#tblQuotationDetails').DataTable(
         {
             dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
             order: [],
             searching: false,
             paging: false,
             ordering: false,
             bInfo: false,
             data: id == _emptyGuid ? null : GetQuotationDetailListByQuotationID(id),
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
             { "data": null, "orderable": false, "defaultContent": '<a href="#" class="DeleteLink"  onclick="ConfirmDeleteQuotationDetail(this)" ><i class="fa fa-trash-o" aria-hidden="true"></i></a> <a href="#" class="actionLink"  onclick="EditQuotationDetail(this)" ><i class="fa fa-pencil-square-o" aria-hidden="true"></i></a>' },
             ],
             columnDefs: [
                 { "targets": [0, 4], "width": "10%" },
                 { "targets": [1, 2], "width": "15%" },
                 { "targets": [3], "width": "35%" },
                 { "targets": [5], "width": "10%" },
                 { "targets": [6], "width": "5%" },
                 { className: "text-right", "targets": [4, 5] },
                 { className: "text-left", "targets": [1, 2, 3] },
                 { className: "text-center", "targets": [0, 6] }
             ]
         });
}
function GetQuotationDetailListByQuotationID(id) {
    try {
        debugger;
        var data = { "quotationID": id };
        var quotationDetailList = [];
        _jsonData = GetDataFromServer("Quotation/GetQuotationDetailListByQuotationID/", data);
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
            if ($('#ProductID').val() != "") {
                debugger;
                var quotationDetailList = _dataTable.QuotationDetailList.rows().data();
                quotationDetailList[_datatablerowindex].Product.Code = $("#ProductID").val() != "" ? $("#ProductID option:selected").text().split("-")[0].trim() : "";
                quotationDetailList[_datatablerowindex].Product.Name = $("#ProductID").val() != "" ? $("#ProductID option:selected").text().split("-")[1].trim() : "";
                quotationDetailList[_datatablerowindex].ProductID = $("#ProductID").val() != "" ? $("#ProductID").val() : _emptyGuid;
                quotationDetailList[_datatablerowindex].ProductModelID = $("#ProductModelID").val() != "" ? $("#ProductModelID").val() : _emptyGuid;
                ProductModel = new Object;
                Unit = new Object;
                ProductModel.Name = $("#ProductModelID").val() != "" ? $("#ProductModelID option:selected").text() : "";
                quotationDetailList[_datatablerowindex].ProductModel = ProductModel;
                quotationDetailList[_datatablerowindex].ProductSpec = $('#ProductSpec').val();
                quotationDetailList[_datatablerowindex].Qty = $('#Qty').val();
                quotationDetailList[_datatablerowindex].UnitCode = $('#UnitCode').val();
                Unit.Description = $("#UnitCode").val() != "" ? $("#UnitCode option:selected").text().trim() : "";
                quotationDetailList[_datatablerowindex].Unit = Unit;
                quotationDetailList[_datatablerowindex].Rate = $('#Rate').val();
                _dataTable.QuotationDetailList.clear().rows.add(quotationDetailList).draw(false);
                $('#divModelPopQuotation').modal('hide');
                _datatablerowindex = -1;
            }
        }
        else {
            if ($('#ProductID').val() != "")
                if (_dataTable.QuotationDetailList.rows().data().length === 0) {
                    _dataTable.QuotationDetailList.clear().rows.add(GetQuotationDetailListByQuotationID(_emptyGuid)).draw(false);
                    debugger;
                    var quotationDetailList = _dataTable.QuotationDetailList.rows().data();
                    quotationDetailList[0].Product.Code = $("#ProductID").val() != "" ? $("#ProductID option:selected").text().split("-")[0].trim() : "";
                    quotationDetailList[0].Product.Name = $("#ProductID").val() != "" ? $("#ProductID option:selected").text().split("-")[1].trim() : "";
                    quotationDetailList[0].ProductID = $("#ProductID").val() != "" ? $("#ProductID").val() : _emptyGuid;
                    quotationDetailList[0].ProductModelID = $("#ProductModelID").val() != "" ? $("#ProductModelID").val() : _emptyGuid;
                    quotationDetailList[0].ProductModel.Name = $("#ProductModelID").val() != "" ? $("#ProductModelID option:selected").text() : "";
                    quotationDetailList[0].ProductSpec = $('#ProductSpec').val();
                    quotationDetailList[0].Qty = $('#Qty').val();
                    quotationDetailList[0].UnitCode = $('#UnitCode').val();
                    quotationDetailList[0].Unit.Description = $("#UnitCode").val() != "" ? $("#UnitCode option:selected").text().trim() : "";
                    quotationDetailList[0].Rate = $('#Rate').val();
                    _dataTable.QuotationDetailList.clear().rows.add(quotationDetailList).draw(false);
                    $('#divModelPopQuotation').modal('hide');
                }
                else {
                    debugger;
                    var QuotationDetailVM = new Object();
                    var Product = new Object;
                    var ProductModel = new Object()
                    var Unit = new Object();
                    QuotationDetailVM.ID = _emptyGuid;
                    QuotationDetailVM.ProductID = $("#ProductID").val() != "" ? $("#ProductID").val() : _emptyGuid;
                    Product.Code = $("#ProductID").val() != "" ? $("#ProductID option:selected").text().split("-")[0].trim() : "";
                    Product.Name = $("#ProductID").val() != "" ? $("#ProductID option:selected").text().split("-")[1].trim() : "";
                    QuotationDetailVM.Product = Product;
                    QuotationDetailVM.ProductModelID = $("#ProductModelID").val() != "" ? $("#ProductModelID").val() : _emptyGuid;
                    ProductModel.Name = $("#ProductModelID").val() != "" ? $("#ProductModelID option:selected").text() : "";
                    QuotationDetailVM.ProductModel = ProductModel;
                    QuotationDetailVM.ProductSpec = $('#ProductSpec').val();
                    QuotationDetailVM.Qty = $('#Qty').val();
                    Unit.Description = $("#UnitCode").val() != "" ? $("#UnitCode option:selected").text().trim() : "";
                    QuotationDetailVM.Unit = Unit;
                    QuotationDetailVM.UnitCode = $('#UnitCode').val();
                    QuotationDetailVM.Rate = $('#Rate').val();
                    _dataTable.QuotationDetailList.row.add(QuotationDetailVM).draw(true);
                    $('#divModelPopQuotation').modal('hide');
                }
        }

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
//================================================================================================
//QuotationFollowup Section
function AddQuotationFollowUp() {
    debugger;
    $("#divModelQuotationPopBody").load("QuotationFollowup/AddQuotationFollowup?ID=" + _emptyGuid + "&QuotationID=" + $('#QuotationForm input[type="hidden"]#ID').val(), function () {
        $('#lblModelPopQuotation').text('Add Quotation Followup')
        $('#btnresetQuotationFollowup').trigger('click');
        $('#divModelPopQuotation').modal('show');

    });
}
function QuotationFollowUpPaging(start) {
    $("#divQuotationFollowupboxbody").load("QuotationFollowup/GetQuotationFollowupList?ID=" + _emptyGuid + "&QuotationID=" + $('#QuotationForm input[type="hidden"]#ID').val() + "&DataTablePaging.Start=" + start, function () {

    });
}
function EditQuotationFollowup(id) {
    debugger;
    $("#divModelQuotationPopBody").load("QuotationFollowup/AddQuotationFollowup?ID=" + id + "&QuotationID=" + $('#QuotationForm input[type="hidden"]#ID').val(), function () {
        $('#lblModelPopQuotation').text('Edit Quotation Followup')
        $('#divModelPopQuotation').modal('show');
    });
}
function SaveSuccessQuotationFollowup(data, status) {
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
                $("#divModelQuotationPopBody").load("QuotationFollowup/AddQuotationFollowup?ID=" + _result.ID + "&QuotationID=" + $('#QuotationForm input[type="hidden"]#ID').val(), function () {
                    $('#lblModelPopQuotation').text('Edit Quotation Followup')
                });
                $("#divFollowupList").load("QuotationFollowup/GetQuotationFollowupList?ID=" + _emptyGuid + "&QuotationID=" + $('#QuotationForm input[type="hidden"]#ID').val(), function () {
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
function ConfirmDeleteQuotationFollowup(ID) {
    if (ID != _emptyGuid) {
        notyConfirm('Are you sure to delete?', 'DeleteQuotationFollowup("' + ID + '")');
    }
}
function DeleteQuotationFollowup(ID) {
    if (ID != _emptyGuid && ID != null && ID != '') {
        var data = { "id": ID };
        var ds = {};
        _jsonData = GetDataFromServer("QuotationFollowup/DeleteQuotationFollowup/", data);
        if (_jsonData != '') {
            _jsonData = JSON.parse(_jsonData);
            _message = _jsonData.Message;
            _status = _jsonData.Status;
            _result = _jsonData.Record;
        }
        if (_status == "OK") {
            notyAlert('success', _result.Message);
            $("#divFollowupList").load("QuotationFollowup/GetQuotationFollowupList?ID=" + _emptyGuid + "&QuotationID=" + $('#QuotationForm input[type="hidden"]#ID').val(), function () {
            });
        }
        if (_status == "ERROR") {
            notyAlert('error', _message);
        }
    }
}