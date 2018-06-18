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
        BindOrReloadProductionQCTable('Init');       
        $('#tblProductionQC tbody').on('dblclick', 'td', function () {
            if (this.textContent !== "No data available in table")
            EditProductionQC(this);
        });
    }
    catch (e) {
        console.log(e.message);
    }
    $("#AdvDocumentStatusCode,#AdvEmailSentStatus").select2({
        dropdownParent: $(".divboxASearch")
    });

    $('.select2').addClass('form-control newinput');
});
//function bind the ProductionQC list checking search and filter
function BindOrReloadProductionQCTable(action) {
    try {
        debugger;
        //creating advancesearch object
        ProductionQCAdvanceSearchViewModel = new Object();
        DataTablePagingViewModel = new Object();
        DataTablePagingViewModel.Length = 0;
        //switch case to check the operation
        switch (action) {
            case 'Reset':
                $('#SearchTerm').val('');                
                $('.divboxASearch #AdvFromDate').val('').trigger('change');
                $('.divboxASearch #AdvToDate').val('').trigger('change');
                $('.divboxASearch #AdvAreaCode').val('').trigger('change');
                $('.divboxASearch #AdvCustomerID').val('').trigger('change');
                $('.divboxASearch #AdvPlantCode').val('').trigger('change');
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
                $('.divboxASearch #AdvPlantCode').val('');
                $('.divboxASearch #AdvBranchCode').val('');
                $('.divboxASearch #AdvDocumentStatusCode').val('');
                $('.divboxASearch #AdvDocumentOwnerID').val('');
                $('.divboxASearch #AdvApprovalStatusCode').val('');
                $('#AdvEmailSentStatus').val('');
                break;
            case 'Search':
                if (($('#SearchTerm').val() == "") && ($('.divboxASearch #AdvFromDate').val() == "") && ($('.divboxASearch #AdvToDate').val() == "") && ($('.divboxASearch #AdvAreaCode').val() == "") && ($('.divboxASearch #AdvCustomerID').val() == "") && ($('.divboxASearch #AdvPlantCode').val() == "") && ($('.divboxASearch #AdvBranchCode').val() == "") && ($('.divboxASearch #AdvDocumentStatusCode').val() == "") && ($('.divboxASearch #AdvDocumentOwnerID').val() == "") && ($('#AdvEmailSentStatus').val() == "") && ($('#AdvApprovalStatusCode').val() == "")) {
                    return true;
                }
                break;
            case 'Export':
                DataTablePagingViewModel.Length = -1;
                break;
            default:
                break;
        }
        ProductionQCAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
        ProductionQCAdvanceSearchViewModel.SearchTerm = $('#SearchTerm').val();
        ProductionQCAdvanceSearchViewModel.AdvFromDate = $('.divboxASearch #AdvFromDate').val(); 
        ProductionQCAdvanceSearchViewModel.AdvToDate = $('.divboxASearch #AdvToDate').val();
        ProductionQCAdvanceSearchViewModel.AdvAreaCode = $('.divboxASearch #AdvAreaCode').val();
        ProductionQCAdvanceSearchViewModel.AdvCustomerID = $('.divboxASearch #AdvCustomerID').val();
        ProductionQCAdvanceSearchViewModel.AdvPlantCode = $('.divboxASearch #AdvPlantCode').val();
        ProductionQCAdvanceSearchViewModel.AdvBranchCode = $('.divboxASearch #AdvBranchCode').val();
        ProductionQCAdvanceSearchViewModel.AdvDocumentStatusCode = $('.divboxASearch #AdvDocumentStatusCode').val();
        ProductionQCAdvanceSearchViewModel.AdvDocumentOwnerID = $('.divboxASearch #AdvDocumentOwnerID').val();
        ProductionQCAdvanceSearchViewModel.AdvApprovalStatusCode = $('.divboxASearch #AdvApprovalStatusCode').val();
        ProductionQCAdvanceSearchViewModel.AdvEmailSentStatus = $('#AdvEmailSentStatus').val();
        //apply datatable plugin on ProductionQC table
        _dataTable.ProductionQCList = $('#tblProductionQC').DataTable(
       
        {           
            dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
            buttons: [{
                extend: 'excel',
                exportOptions:
                             {
                                 columns: [0, 1, 2, 3, 4, 5,6]
                             }
            }],
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
                url: "ProductionQC/GetAllProductionQC/",
                data: { "ProductionQCAdvanceSearchVM": ProductionQCAdvanceSearchViewModel },
                type: 'POST'
            },
            pageLength: 8,
            columns: [                
            
                {
                    "data": "ProdQCNo", render: function (data, type, row) {
                        return row.ProdQCNo + "</br>" + "<img src='./Content/images/datePicker.png' height='10px'>" + "&nbsp;" + row.ProdQCDateFormatted;
                    }, "defaultContent": "<i>-</i>"
                },
                { "data": "ProdOrderNo", "defaultContent": "<i>-</i>" },
               {
                   "data": "Customer.CompanyName", render: function (data, type, row) {
                       return "<img src='./Content/images/contact.png' height='10px'>" + "&nbsp;" + (row.Customer.ContactPerson == null ? "" : row.Customer.ContactPerson) + "</br>" + "<img src='./Content/images/organisation.png' height='10px'>" + "&nbsp;" + data;
                   }, "defaultContent": "<i>-</i>"
               },
               { "data": "Area.Description", "defaultContent": "<i>-</i>" },
               { "data": "Plant.Description", "defaultContent": "<i>-</i>" },
               {
                   "data": "Branch.Description", render: function (data, type, row) {
                       debugger;
                       return "<b>Doc.Owner-</b>" + row.PSAUser.LoginName + "</br>" + "<b>Branch-</b>" + data;
                   }, "defaultContent": "<i>-</i>"
               },
               {
                   "data": "DocumentStatus.Description", render: function (data, type, row) {
                       debugger;
                       return "<b>Doc.Status-</b>" + data + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + (row.EmailSentYN == true ? "<img src='./Content/images/mailSend.png' height='20px' >" : '') + "</br>" + "<b>Appr.Status-</b>" + row.ApprovalStatus.Description;
                   }, "defaultContent": "<i>-</i>"
               },

               { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink"  onclick="EditProductionQC(this)" ><i class="fa fa-pencil-square-o" aria-hidden="true"></i></a>' },


            ],
            columnDefs: [{ className: "text-right", "targets": [] },
                          { className: "text-left", "targets": [0,1,2, 3,4,5,6] },
                          { className: "text-center", "targets": [6] },
                            { "targets": [0], "width": "11%" },
                            { "targets": [1], "width": "11%" },
                            { "targets": [2], "width": "12%" },
                            { "targets": [3], "width": "9%" },           
                            { "targets": [4], "width": "9%" },
                             { "targets": [5], "width": "14%" },
                             { "targets": [6], "width": "24%" },
                             { "targets": [7], "width": "2%" },

                                              
            ],
            destroy: true,
            //for performing the import operation after the data loaded
            initComplete: function (settings, json) {
                debugger;
                $('.dataTables_wrapper div.bottom div').addClass('col-md-6');
                $('#tblProductionQC').fadeIn(100);
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
                    BindOrReloadProductionQCTable();
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
function ResetProductionQCList() {
    $(".searchicon").removeClass('filterApplied');
    BindOrReloadProductionQCTable('Reset');
}
//function export data to excel
function ExportProductionQCData() {
    $('.excelExport').show();
    OnServerCallBegin();
    BindOrReloadProductionQCTable('Export');
}

// add ProductionQC section
function AddProductionQC() {
    //this will return form body(html)
    OnServerCallBegin();
    $('#lblProductionQCInfo').text("<<ProductionQC No.>>");
    $("#divProductionQCForm").load("ProductionQC/ProductionQCForm?id=" + _emptyGuid + "&productionOrderID=", function (responseTxt, statusTxt, xhr) {
        if (statusTxt == "success") {
            OnServerCallComplete();
            openNav();
            ChangeButtonPatchView("ProductionQC", "btnPatchProductionQCNew", "Add");
            BindProductionQCDetailList(_emptyGuid);
        }
        else {
            console.log("Error: " + xhr.status + ": " + xhr.statusText);
        }
    });
}
function EditProductionQC(this_Obj) {
    OnServerCallBegin();
    var ProductionQC = _dataTable.ProductionQCList.row($(this_Obj).parents('tr')).data();
    $('#lblProductionQCInfo').text(ProductionQC.ProdQCNo);
    //this will return form body(html)
    $("#divProductionQCForm").load("ProductionQC/ProductionQCForm?id=" + ProductionQC.ID + "&productionOrderID=" + ProductionQC.ProdOrderID, function (responseTxt, statusTxt, xhr) {
        if (statusTxt == "success") {
            //$('#CustomerID').trigger('change');
            
            
            OnServerCallComplete();
            openNav();
            if ($('#IsDocLocked').val() == "True") {
                ChangeButtonPatchView("ProductionQC", "btnPatchProductionQCNew", "Edit");
            }
            else {
                ChangeButtonPatchView("ProductionQC", "btnPatchProductionQCNew", "LockDocument");
            }
            BindProductionQCDetailList(ProductionQC.ID);
            $('#divCustomerBasicInfo').load("Customer/CustomerBasicInfo?ID=" + $('#hdnCustomerID').val());
            clearUploadControl();
            PaintImages(ProductionQC.ID);
        }
        else {
            console.log("Error: " + xhr.status + ": " + xhr.statusText);
        }
    });
}
function ResetProductionQC() {
    if ($('#IsUpdate').val() == 'False') {
        $('#hdnProdOrderID').val('');
    }
    $("#divProductionQCForm").load("ProductionQC/ProductionQCForm?id=" + $('#ProductionQCForm #ID').val() + "&productionOrderID=" + $('#hdnProdOrderID').val(), function (responseTxt, statusTxt, xhr) {
        if (statusTxt == "success") {
            if ($('#ID').val() != _emptyGuid && $('#ID').val() != null) {
                //resides in customjs for sliding

                $("#divProductionQCForm #ProdOrderID").prop('disabled', true);
                openNav();
            }
            else {
                debugger;
                $('#hdnCustomerID').val('');
                $("#ProductionQCForm #CustomerID").prop('disabled', false);
                $('#lblProductionQCInfo').text('<<ProductionQC No.>>');
            }            
            BindProductionQCDetailList($('#ID').val());
            clearUploadControl();
            PaintImages($('#ProductionQCForm #ID').val());
            $('#divCustomerBasicInfo').load("Customer/CustomerBasicInfo?ID=" + $('#ProductionQCForm #hdnCustomerID').val());
        }
        else {
            console.log("Error: " + xhr.status + ": " + xhr.statusText);
        }
    });
}
function SaveProductionQC() {
    var productionQCDetailList = _dataTable.ProductionQCDetailList.rows().data().toArray();
    $('#DetailJSON').val(JSON.stringify(productionQCDetailList));
    $('#btnInsertUpdateProductionQC').trigger('click');
}
function ApplyFilterThenSearch() {
    $(".searchicon").addClass('filterApplied');
    CloseAdvanceSearch();
    BindOrReloadProductionQCTable('Search');
}
function SaveSuccessProductionQC(data, status) {
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
                $('#lblProductionQCInfo').text(_result.ProdQCNo);
                $("#divProductionQCForm").load("ProductionQC/ProductionQCForm?id=" + _result.ID, function () {
                    ChangeButtonPatchView("ProductionQC", "btnPatchProductionQCNew", "Edit");
                    BindProductionQCDetailList(_result.ID);
                    clearUploadControl();
                    PaintImages(_result.ID);                   
                    $('#divCustomerBasicInfo').load("Customer/CustomerBasicInfo?ID=" + $('#ProductionQCForm #hdnCustomerID').val());
                });
                ChangeButtonPatchView("ProductionQC", "btnPatchProductionQCNew", "Edit");
                BindOrReloadProductionQCTable('Init');
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

function DeleteProductionQC() {
    notyConfirm('Are you sure to delete?', 'DeleteProductionQCItem("' + $('#ProductionQCForm #ID').val() + '")');
}
function DeleteProductionQCItem(id) {
    try {
        if (id) {
            var data = { "id": id };
            _jsonData = GetDataFromServer("ProductionQC/DeleteProductionQC/", data);
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
                    ChangeButtonPatchView("ProductionQC", "btnPatchProductionQCNew", "Add");
                    ResetProductionQC();
                    BindOrReloadProductionQCTable('Init');
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
function BindProductionQCDetailList(id, IsProductioOrder) {
    debugger;
    _dataTable.ProductionQCDetailList = $('#tblProductionQCDetails').DataTable(
         {
             dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
             order: [],
             searching: false,
             paging: false,
             ordering: false,
             bInfo: false,
             data: !IsProductioOrder ? id == _emptyGuid ? null : GetProductionQCDetailListByProductionQCID(id, false) : GetProductionQCDetailListByProductionQCID(id, true),
             language: {
                 search: "_INPUT_",
                 searchPlaceholder: "Search"
             },
             columns: [
             {
                 "data": "Product.Code", render: function (data, type, row) {
                     return '<div style="width:100%" class="show-popover" data-html="true" data-toggle="popover" data-placement="top" data-title="<p align=left>Product Specification" data-content="' + row.ProductSpec.replace(/"/g, "&quot") + '</p>"/>' + row.Product.Name + "<br/>" + row.ProductModel.Name
                 }, "defaultContent": "<i></i>"
             },
             {
                 "data": "ProducedQty", render: function (data, type, row) {
                     return data + " " + row.Unit.Description
                 }, "defaultContent": "<i></i>"
             },
             {
                 "data": "QCQtyPrevious", render: function (data, type, row) {
                     return data + " " + row.Unit.Description
                 }, "defaultContent": "<i></i>"
             },
             {
                 "data": "QCQty", render: function (data, type, row) {
                     return data + " " + row.Unit.Description
                 }, "defaultContent": "<i></i>"
             },
             { "data": "QCDateFormatted", render: function (data, type, row) { return data }, "defaultContent": "<i>__</i>" },
             { "data": "Employee.Name", render: function (data, type, row) { return data }, "defaultContent": "<i>__</i>" },
             { "data": null, "orderable": false, "defaultContent": ($('#IsDocLocked').val() == "True" || $('#IsUpdate').val() == "False")?'<a href="#" class="actionLink"  onclick="EditProductionQCDetail(this)" ><i class="fa fa-pencil-square-o" aria-hidden="true"></i></a>':'-' },
             ],
             columnDefs: [
                 { "targets": [0], "width": "30%" },
                 { "targets": [1, 2], "width": "15%" },
                 { "targets": [3, 4], "width": "10%" },
                 { "targets": [5], "width": "10%" },
                 { "targets": [6], "width": "10%" },
                 { className: "text-left", "targets": [0, 1, 2, 3,4,5] },
                 { className: "text-center", "targets": [ 6] }
             ]
         });
    $('[data-toggle="popover"]').popover({
        html: true,
        'trigger': 'hover'
    });
}
function GetProductionQCDetailListByProductionQCID(id, IsProductioOrder) {
    try {
        var productionQCDetailList = [];
        if (IsProductioOrder) {
            var data = { "productionOrderID": $('#ProductionQCForm #hdnProdOrderID').val() };
            _jsonData = GetDataFromServer("ProductionQC/GetProductionQCDetailListByProductionOrderIDWithProductionOrder/", data);
        }
        else {
            var data = { "productionQCID": id };
            _jsonData = GetDataFromServer("ProductionQC/GetProductionQCDetailListByProductionQCID/", data);
        }
        if (_jsonData != '') {
            _jsonData = JSON.parse(_jsonData);
            _message = _jsonData.Message;
            _status = _jsonData.Status;
            productionQCDetailList = _jsonData.Records;
        }
        if (_status == "OK") {
            return productionQCDetailList;
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
//function AddProductionQCDetailList() {
//    $("#divModelProductionQCPopBody").load("ProductionQC/AddProductionQCDetail", function () {
//        $('#lblModelPopProductionQC').text('ProductionQC Detail')
//        $('#divModelPopProductionQC').modal('show');
//    });
//}
function AddProductionQCDetailToList() {
    $("#FormProductionQCDetail").submit(function () { });
    if ($('#FormProductionQCDetail #IsUpdate').val() == 'True') {
        if (($('#QCDateFormatted').val() != "") && ($('#QCBy').val() != "")) {
            debugger;
            var productionQCDetailList = _dataTable.ProductionQCDetailList.rows().data();
            productionQCDetailList[_datatablerowindex].QCQty = $('#FormProductionQCDetail #QCQty').val();
            productionQCDetailList[_datatablerowindex].QCDateFormatted = $('#FormProductionQCDetail #QCDateFormatted').val();
            productionQCDetailList[_datatablerowindex].QCBy = $('#FormProductionQCDetail #QCBy').val();
            var Employee = new Object();
            Employee.Name = $("#FormProductionQCDetail #QCBy option:selected").text()
            productionQCDetailList[_datatablerowindex].Employee = Employee;
            _dataTable.ProductionQCDetailList.clear().rows.add(productionQCDetailList).draw(false);
            $('#divModelPopProductionQC').modal('hide');
            _datatablerowindex = -1;
        }
    }
}
function EditProductionQCDetail(this_Obj) {
    debugger;
    _datatablerowindex = _dataTable.ProductionQCDetailList.row($(this_Obj).parents('tr')).index();
    var productionQCDetail = _dataTable.ProductionQCDetailList.row($(this_Obj).parents('tr')).data();
    $("#divModelProductionQCPopBody").load("ProductionQC/AddProductionQCDetail", function () {
        $('#lblModelPopProductionQC').text('ProductionQC Detail')
        $('#FormProductionQCDetail #IsUpdate').val('True');
        $('#FormProductionQCDetail #ID').val(productionQCDetail.ID);
        $("#FormProductionQCDetail #ProductID").val(productionQCDetail.ProductID)
        $("#FormProductionQCDetail #hdnProductID").val(productionQCDetail.ProductID)
        $('#divProductBasicInfo').load("Product/ProductBasicInfo?ID=" + $('#hdnProductID').val(), function () {
        });

        if ($('#hdnProductID').val() != _emptyGuid) {
            $('.divProductModelSelectList').load("ProductModel/ProductModelSelectList?required=required&disabled=true&productID=" + $('#hdnProductID').val())
        }
        else {
            $('.divProductModelSelectList').empty();
            $('.divProductModelSelectList').append('<span class="form-control newinput"><i id="dropLoad" class="fa fa-spinner"></i></span>');
        }
        $("#FormProductionQCDetail #ProductModelID").val(productionQCDetail.ProductModelID);
        $("#FormProductionQCDetail #hdnProductModelID").val(productionQCDetail.ProductModelID);
        if ($('#hdnProductModelID').val() != _emptyGuid) {
            $('#divProductBasicInfo').load("ProductModel/ProductModelBasicInfo?ID=" + $('#hdnProductModelID').val(), function () {
            });
        }
        $('#FormProductionQCDetail #ProductSpec').val(productionQCDetail.ProductSpec);
        $('#FormProductionQCDetail #QCQty').val(productionQCDetail.QCQty);
        $('#FormProductionQCDetail #QCDateFormatted').val(productionQCDetail.QCDateFormatted);
        $('#FormProductionQCDetail #QCBy').val(productionQCDetail.QCBy);
        $('#FormProductionQCDetail #hdnQCBy').val(productionQCDetail.QCBy);
        $('#divModelPopProductionQC').modal('show');
    });
}
function ConfirmDeleteProductionQCDetail(this_Obj) {
    debugger;
    _datatablerowindex = _dataTable.ProductionQCDetailList.row($(this_Obj).parents('tr')).index();
    var productionQCDetail = _dataTable.ProductionQCDetailList.row($(this_Obj).parents('tr')).data();
    if (productionQCDetail.ID === _emptyGuid) {
        var productionQCDetailList = _dataTable.ProductionQCDetailList.rows().data();
        productionQCDetailList.splice(_datatablerowindex, 1);
        _dataTable.ProductionQCDetailList.clear().rows.add(productionQCDetailList).draw(false);
        notyAlert('success', 'Detail Row deleted successfully');
    }
    else {
        notyConfirm('Are you sure to delete?', 'DeleteProductionQCDetail("' + productionQCDetail.ID + '")');

    }
}
function DeleteProductionQCDetail(ID) {
    if (ID != _emptyGuid && ID != null && ID != '') {
        var data = { "id": ID };
        var ds = {};
        _jsonData = GetDataFromServer("ProductionQC/DeleteProductionQCDetail/", data);
        if (_jsonData != '') {
            _jsonData = JSON.parse(_jsonData);
            _message = _jsonData.Message;
            _status = _jsonData.Status;
            _result = _jsonData.Record;
        }
        if (_status == "OK") {
            notyAlert('success', _result.Message);
            var productionQCDetailList = _dataTable.ProductionQCDetailList.rows().data();
            productionQCDetailList.splice(_datatablerowindex, 1);
            _dataTable.ProductionQCDetailList.clear().rows.add(productionQCDetailList).draw(false);
        }
        if (_status == "ERROR") {
            notyAlert('error', _message);
        }
    }
}
