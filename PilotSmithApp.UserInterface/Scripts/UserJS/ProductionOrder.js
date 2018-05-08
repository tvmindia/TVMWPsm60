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
        BindOrReloadProductionOrderTable('Init');
        $('#tblProductionOrder tbody').on('dblclick', 'td', function () {
            EditProductionOrder(this);
        });
    }
    catch (e) {
        console.log(e.message);
    }
});

//function bind the ProductionOrder list checking search and filter
function BindOrReloadProductionOrderTable(action) {
    try {
        debugger;
        //creating advancesearch object
        ProductionOrderAdvanceSearchViewModel = new Object();
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
        ProductionOrderAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
        ProductionOrderAdvanceSearchViewModel.SearchTerm = $('#SearchTerm').val();
        ProductionOrderAdvanceSearchViewModel.FromDate = $('#FromDate').val();
        ProductionOrderAdvanceSearchViewModel.ToDate = $('#ToDate').val();
        //apply datatable plugin on ProductionOrder table
        _dataTable.ProductionOrderList = $('#tblProductionOrder').DataTable(
        {
            dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
            buttons: [{
                extend: 'excel',
                exportOptions:
                             {
                                 columns: [0, 1, 2, 3, 4, 5]
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
                url: "ProductionOrder/GetAllProductionOrder/",
                data: { "ProductionOrderAdvanceSearchVM": ProductionOrderAdvanceSearchViewModel },
                type: 'POST'
            },
            pageLength: 13,
            columns: [
               { "data": "ProdOrderNo", "defaultContent": "<i>-</i>" },
               { "data": "ProdOrderDateFormatted", "defaultContent": "<i>-</i>" },
               { "data": "Customer.CompanyName", "defaultContent": "<i>-</i>" },
               { "data": "Branch.Description", "defaultContent": "<i>-</i>" },
               { "data": "DocumentStatus.Description", "defaultContent": "<i>-</i>" },
               //{ "data": "UserName", "defaultContent": "<i>-</i>" },
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
               { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink"  onclick="EditProductionOrder(this)" ><i class="fa fa-pencil-square-o" aria-hidden="true"></i></a>' },
            ],
            columnDefs: [
                          { className: "text-left", "targets": [0, 2, 3, 4, 5, 6] },
                          { className: "text-center", "targets": [1] },
                            { "targets": [0], "width": "10%" },
                            { "targets": [2], "width": "20%" },
                            { "targets": [1, 3, 4], "width": "10%" },
                            { "targets": [5], "width": "7%" },
                            { "targets": [6], "width": "7%" },

            ],
            destroy: true,
            //for performing the import operation after the data loaded
            initComplete: function (settings, json) {
                debugger;
                $('.dataTables_wrapper div.bottom div').addClass('col-md-6');
                $('#tblProductionOrder').fadeIn('slow');
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
                    BindOrReloadProductionOrderTable();
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
function ResetProductionOrderList() {
    $(".searchicon").removeClass('filterApplied');
    BindOrReloadProductionOrderTable('Reset');
}
//function export data to excel
function ExportProductionOrderData() {
    debugger;
    $('.excelExport').show();
    OnServerCallBegin();
    BindOrReloadProductionOrderTable('Export');
}
// add ProductionOrder section
function AddProductionOrder() {
    debugger;
    //this will return form body(html)
    OnServerCallBegin();
    $("#divProductionOrderForm").load("ProductionOrder/ProductionOrderForm?id=" + _emptyGuid ,function () {
        ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "Add");
       BindProductionOrderDetailList(_emptyGuid);
        //BindProductionOrderOtherChargesDetailList(_emptyGuid)
         OnServerCallComplete();
      
            //resides in customjs for sliding
            openNav();
    
    });
}

function EditProductionOrder(this_Obj) {
    debugger;
    OnServerCallBegin();
    var productionOrder = _dataTable.ProductionOrderList.row($(this_Obj).parents('tr')).data();
    //this will return form body(html)
    $("#divProductionOrderForm").load("ProductionOrder/ProductionOrderForm?id=" + productionOrder.ID , function () {

        ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "Edit");
       BindProductionOrderDetailList(productionOrder.ID);
        $('#divCustomerBasicInfo').load("Customer/CustomerBasicInfo?ID=" + $('#hdnCustomerID').val());
        clearUploadControl();
        PaintImages(productionOrder.ID);
        OnServerCallComplete();
        //resides in customjs for sliding
      
            //$("#divEstimateForm #EnquiryID").prop('disabled', true);
            openNav();
      
    });
}

function ResetProductionOrder() {
    //this will return form body(html)
    $("#divProductionOrderForm").load("ProductionOrder/ProductionOrderForm?id=" + $('#ProductionOrderForm #ID').val() , function () {
        if ($('#ID').val() != _emptyGuid && $('#ID').val() != null) {
            //resides in customjs for sliding
         
                //$("#divEstimateForm #EnquiryID").prop('disabled', true);
                openNav();
         
        }
       BindProductionOrderDetailList($('#ID').val(), false);
        clearUploadControl();
        PaintImages($('#ProductionOrderForm #ID').val());
        $('#divCustomerBasicInfo').load("Customer/CustomerBasicInfo?ID=" + $('#EstimateForm #hdnCustomerID').val());
    });
}

function SaveProductionOrder() {
    debugger;
    var productionOrderDetailList = _dataTable.ProductionOrderDetailList.rows().data().toArray();
    $('#DetailJSON').val(JSON.stringify(productionOrderDetailList));
    $('#btnInsertUpdateProductionOrder').trigger('click');
}

function ApplyFilterThenSearch() {
    $(".searchicon").addClass('filterApplied');
    CloseAdvanceSearch();
    BindOrReloadProductionOrderTable('Search');
}

function SaveSuccessProductionOrder(data, status) {
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
                $("#divProductionOrderForm").load("ProductionOrder/ProductionOrderForm?id=" + _result.ID , function () {
                    ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "Edit");
                   BindProductionOrderDetailList(_result.ID);
                   clearUploadControl();
                   PaintImages(_result.ID);
                });
                ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "Edit");
                BindOrReloadProductionOrderTable('Init');
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

function BindProductionOrderDetailList(id) {
    debugger;
    _dataTable.ProductionOrderDetailList = $('#tblProductionOrderDetails').DataTable(
         {
             dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
             order: [],
             searching: false,
             paging: false,
             ordering: false,
             bInfo: false,
             data: id == _emptyGuid ? null : GetProductionOrderDetailListByProductionOrderID(id),
             language: {
                 search: "_INPUT_",
                 searchPlaceholder: "Search"
             },
             columns: [
             {
                 "data": "Product.Code", render: function (data, type, row) {
                     debugger;
                        return row.Product.Code + "<br/>" + row.Product.Name + "<br/>" + row.ProductModel.Name + "<br/>" + row.ProductSpec                                                                           
                 }, "defaultContent": "<i></i>"
             },
             {
                 "data": "OrderQty", render: function (data, type, row) {
                     return data + " " + row.Unit.Description
                 }, "defaultContent": "<i></i>"
             },
             {
                 "data": "OrderQty", render: function (data, type, row) {
                     return data + " " + row.Unit.Description
                 }, "defaultContent": "<i></i>"
             },
             {
                 "data": "OrderQty", render: function (data, type, row) {
                     var CurProducedQty = roundoff(parseFloat(row.OrderQty) - parseFloat(row.OrderQty));
                     return CurProducedQty
                     //return data + " " + row.Unit.Description
                 }, "defaultContent": "<i></i>"
             },
             {
                 "data": "ProducedQty", render: function (data, type, row) {
                     return data + " " + row.Unit.Description
                 }, "defaultContent": "<i></i>"
             },
             { "data": "Rate", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
             {
                 "data": "Amount", render: function (data, type, row) {
                     var Amount = roundoff(parseFloat(row.OrderQty) * parseFloat(row.Rate));
                     return Amount
                 }, "defaultContent": "<i></i>"
             },
             { "data": "PlantCode", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
             {
                 "data": "OrderQty", render: function (data, type, row) {
                     return data
                 }, "defaultContent": "<i></i>"
             },
             { "data": null, "orderable": false, "defaultContent":'<a href="#" class="actionLink"  onclick="EditProductionOrderDetail(this)" ><i class="fa fa-pencil-square-o" aria-hidden="true"></i></a> <a href="#" class="DeleteLink"  onclick="ConfirmDeleteProductionOrderDetail(this)" ><i class="fa fa-trash-o" aria-hidden="true"></i></a>' },
             ],
             columnDefs: [                 
                 { className: "text-right", "targets": [1, 2, 3, 4, 5, 6] },
                 { "targets": [0], "width": "20%" },
                 { "targets": [1, 2, 3, 4, 5, 6, 7, 8], "width": "10%" },
                 {"targets":[9],"width":"7%"}
             ]
         });
}

function GetProductionOrderDetailListByProductionOrderID(id) {
    try {
        debugger;

        var productionOrderDetailList = [];
        //if (IsSaleOrder) {
        //    var data = { "enquiryID": $('#EstimateForm #hdnEnquiryID').val() };
        //    _jsonData = GetDataFromServer("Estimate/GetEstimateDetailListByEstimateIDWithEnquiry/", data);
        //}
        //else {
        var data = { "productionOrderID": id };
        _jsonData = GetDataFromServer("ProductionOrder/GetProductionOrderDetailListByProductionOrderID/", data);
        //}

        if (_jsonData != '') {
            _jsonData = JSON.parse(_jsonData);
            _message = _jsonData.Message;
            _status = _jsonData.Status;
            productionOrderDetailList = _jsonData.Records;
        }
        if (_status == "OK") {
            return productionOrderDetailList;
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

function AddProductionOrderDetailList() {
    debugger;
    $("#divModelProductionOrderPopBody").load("ProductionOrder/AddProductionOrderDetail", function () {
        $('#lblModelPopProductionOrder').text('ProductionOrder Detail')
        $('#divModelPopProductionOrder').modal('show');
    });
}

function AddProductionOrderDetailToList() {
    debugger;
    //$("#FormProductionOrderDetail").submit(function () { });
    if ($('#FormProductionOrderDetail #IsUpdate').val() == 'True')
    {
        if ($('#ProductID').val() != "") {
            debugger;
            var productionOrderDetailList = _dataTable.ProductionOrderDetailList.rows().data();
            productionOrderDetailList[_datatablerowindex].Product.Code = $("#ProductID").val() != "" ? $("#ProductID option:selected").text().split("-")[0].trim() : "";
            productionOrderDetailList[_datatablerowindex].Product.Name = $("#ProductID").val() != "" ? $("#ProductID option:selected").text().split("-")[1].trim() : "";
            productionOrderDetailList[_datatablerowindex].ProductID = $("#ProductID").val() != "" ? $("#ProductID").val() : _emptyGuid;
            productionOrderDetailList[_datatablerowindex].ProductModelID = $("#ProductModelID").val() != "" ? $("#ProductModelID").val() : _emptyGuid;
            ProductModel = new Object;
            Unit = new Object;
            ProductModel.Name = $("#ProductModelID").val() != "" ? $("#ProductModelID option:selected").text() : "";
            productionOrderDetailList[_datatablerowindex].ProductModel = ProductModel;
            productionOrderDetailList[_datatablerowindex].ProductSpec = $('#ProductSpec').val();
            productionOrderDetailList[_datatablerowindex].OrderQty = $('#OrderQty').val();
            productionOrderDetailList[_datatablerowindex].ProducedQty = $('#ProducedQty').val();
            productionOrderDetailList[_datatablerowindex].UnitCode = $('#UnitCode').val();
            Unit.Description = $("#UnitCode").val() != "" ? $("#UnitCode option:selected").text().trim() : "";
            productionOrderDetailList[_datatablerowindex].Unit = Unit;
            productionOrderDetailList[_datatablerowindex].Rate = $('#Rate').val();
            productionOrderDetailList[_datatablerowindex].PlantCode = $('#PlantCode').val();
            //productionOrderDetailList[_datatablerowindex].MileStone1FcFinishDt = $('#MileStone1FcFinishDt').val();
            //productionOrderDetailList[_datatablerowindex].MileStone1AcTFinishDt = $('#MileStone1AcTFinishDt').val();
            //productionOrderDetailList[_datatablerowindex].MileStone2FcFinishDt = $('#MileStone2FcFinishDt').val();
            //productionOrderDetailList[_datatablerowindex].MileStone2AcTFinishDt = $('#MileStone2AcTFinishDt').val();
            //productionOrderDetailList[_datatablerowindex].MileStone3FcFinishDt = $('#MileStone3FcFinishDt').val();
            //productionOrderDetailList[_datatablerowindex].MileStone3AcTFinishDt = $('#MileStone3AcTFinishDt').val();
            //productionOrderDetailList[_datatablerowindex].MileStone4FcFinishDt = $('#MileStone4FcFinishDt').val();
            //productionOrderDetailList[_datatablerowindex].MileStone4AcTFinishDt = $('#MileStone4AcTFinishDt').val();
            _dataTable.ProductionOrderDetailList.clear().rows.add(productionOrderDetailList).draw(false);
            $('#divModelPopProductionOrder').modal('hide');
            _datatablerowindex = -1;
        }
    }
    else
    {
        if ($('#ProductID').val() != "") {
            debugger;
            if (_dataTable.ProductionOrderDetailList.rows().data().length === 0) {
                _dataTable.ProductionOrderDetailList.clear().rows.add(GetProductionOrderDetailListByProductionOrderID(_emptyGuid)).draw(false);
                debugger;
                var productionOrderDetailList = _dataTable.ProductionOrderDetailList.rows().data();
                productionOrderDetailList[0].Product.Code = $("#ProductID").val() != "" ? $("#ProductID option:selected").text().split("-")[0].trim() : "";
                productionOrderDetailList[0].Product.Name = $("#ProductID").val() != "" ? $("#ProductID option:selected").text().split("-")[1].trim() : "";
                productionOrderDetailList[0].ProductID = $("#ProductID").val() != "" ? $("#ProductID").val() : _emptyGuid;
                productionOrderDetailList[0].ProductModelID = $("#ProductModelID").val() != "" ? $("#ProductModelID").val() : _emptyGuid;
                ProductModel = new Object;
                Unit = new Object;
                ProductModel.Name = $("#ProductModelID").val() != "" ? $("#ProductModelID option:selected").text() : "";
                productionOrderDetailList[0].ProductModel = ProductModel;
                productionOrderDetailList[0].ProductSpec = $('#ProductSpec').val();
                productionOrderDetailList[0].OrderQty = $('#OrderQty').val();
                productionOrderDetailList[0].ProducedQty = $('#ProducedQty').val();
                productionOrderDetailList[0].UnitCode = $('#UnitCode').val();
                Unit.Description = $("#UnitCode").val() != "" ? $("#UnitCode option:selected").text().trim() : "";
                productionOrderDetailList[0].Unit = Unit;
                productionOrderDetailList[0].Rate = $('#Rate').val();
                productionOrderDetailList[0].PlantCode = $('#PlantCode').val();
                //productionOrderDetailList[0].MileStone1FcFinishDt = $('#MileStone1FcFinishDt').val();
                //productionOrderDetailList[0].MileStone1AcTFinishDt = $('#MileStone1AcTFinishDt').val();
                //productionOrderDetailList[0].MileStone2FcFinishDt = $('#MileStone2FcFinishDt').val();
                //productionOrderDetailList[0].MileStone2AcTFinishDt = $('#MileStone2AcTFinishDt').val();
                //productionOrderDetailList[0].MileStone3FcFinishDt = $('#MileStone3FcFinishDt').val();
                //productionOrderDetailList[0].MileStone3AcTFinishDt = $('#MileStone3AcTFinishDt').val();
                //productionOrderDetailList[0].MileStone4FcFinishDt = $('#MileStone4FcFinishDt').val();
                //productionOrderDetailList[0].MileStone4AcTFinishDt = $('#MileStone4AcTFinishDt').val();
                _dataTable.ProductionOrderDetailList.clear().rows.add(productionOrderDetailList).draw(false);
                $('#divModelPopProductionOrder').modal('hide');
            }
            else {
                debugger;
                var ProductionOrderDetailVM = new Object();
                var Product = new Object;
                var ProductModel = new Object()
                var Unit = new Object();
                ProductionOrderDetailVM.ID = _emptyGuid;
                ProductionOrderDetailVM.ProductID = $("#ProductID").val() != "" ? $("#ProductID").val() : _emptyGuid;
                Product.Code = $("#ProductID").val() != "" ? $("#ProductID option:selected").text().split("-")[0].trim() : "";
                Product.Name = $("#ProductID").val() != "" ? $("#ProductID option:selected").text().split("-")[1].trim() : "";
                ProductionOrderDetailVM.Product = Product;
                ProductionOrderDetailVM.ProductModelID = $("#ProductModelID").val() != "" ? $("#ProductModelID").val() : _emptyGuid;
                ProductModel.Name = $("#ProductModelID").val() != "" ? $("#ProductModelID option:selected").text() : "";
                ProductionOrderDetailVM.ProductModel = ProductModel;
                ProductionOrderDetailVM.ProductSpec = $('#ProductSpec').val();
                ProductionOrderDetailVM.OrderQty = $('#OrderQty').val();
                ProductionOrderDetailVM.ProducedQty = $('#ProducedQty').val();
                Unit.Description = $("#UnitCode").val() != "" ? $("#UnitCode option:selected").text().trim() : "";
                ProductionOrderDetailVM.Unit = Unit;
                ProductionOrderDetailVM.UnitCode = $('#UnitCode').val();
                ProductionOrderDetailVM.Rate = $('#Rate').val();
                ProductionOrderDetailVM.PlantCode = $('#PlantCode').val();
                //productionOrderDetailVM.MileStone1FcFinishDt = $('#MileStone1FcFinishDt').val();
                //ProductionOrderDetailVM.MileStone1AcTFinishDt = $('#MileStone1AcTFinishDt').val();
                //ProductionOrderDetailVM.MileStone2FcFinishDt = $('#MileStone2FcFinishDt').val();
                //ProductionOrderDetailVM.MileStone2AcTFinishDt = $('#MileStone2AcTFinishDt').val();
                //ProductionOrderDetailVM.MileStone3FcFinishDt = $('#MileStone3FcFinishDt').val();
                //ProductionOrderDetailVM.MileStone3AcTFinishDt = $('#MileStone3AcTFinishDt').val();
                //ProductionOrderDetailVM.MileStone4FcFinishDt = $('#MileStone4FcFinishDt').val();
                //ProductionOrderDetailVM.MileStone4AcTFinishDt = $('#MileStone4AcTFinishDt').val();
                _dataTable.ProductionOrderDetailList.row.add(ProductionOrderDetailVM).draw(true);
                $('#divModelPopProductionOrder').modal('hide');
            }
        }
    }
}

function EditProductionOrderDetail(this_Obj)
{
    debugger;
    _datatablerowindex = _dataTable.ProductionOrderDetailList.row($(this_Obj).parents('tr')).index();
    var productionOrderDetail = _dataTable.ProductionOrderDetailList.row($(this_Obj).parents('tr')).data();
    $("#divModelProductionOrderPopBody").load("ProductionOrder/AddProductionOrderDetail", function () {
        $('#lblModelPopProductionOrder').text('ProductionOrder Detail')
        $('#FormProductionOrderDetail #IsUpdate').val('True');
        $('#FormProductionOrderDetail #ID').val(productionOrderDetail.ID);
        $("#FormProductionOrderDetail #ProductID").val(productionOrderDetail.ProductID)
        $("#FormProductionOrderDetail #hdnProductID").val(productionOrderDetail.ProductID)
        $('#divProductBasicInfo').load("Product/ProductBasicInfo?ID=" + $('#hdnProductID').val(), function () {
        });

        if ($('#hdnProductID').val() != _emptyGuid) {
            $('.divProductModelSelectList').load("ProductModel/ProductModelSelectList?required=required&productID=" + $('#hdnProductID').val())
        }
        else {
            $('.divProductModelSelectList').empty();
            $('.divProductModelSelectList').append('<span class="form-control newinput"><i id="dropLoad" class="fa fa-spinner"></i></span>');
        }
        $("#FormProductionOrderDetail #ProductModelID").val(productionOrderDetail.ProductModelID);
        $("#FormProductionOrderDetail #hdnProductModelID").val(productionOrderDetail.ProductModelID);
        if ($('#hdnProductModelID').val() != _emptyGuid) {
            $('#divProductBasicInfo').load("ProductModel/ProductModelBasicInfo?ID=" + $('#hdnProductModelID').val(), function () {
            });
        }
        $('#FormProductionOrderDetail #ProductSpec').val(productionOrderDetail.ProductSpec);
        $('#FormProductionOrderDetail #OrderQty').val(productionOrderDetail.OrderQty);
        $('#FormProductionOrderDetail #ProducedQty').val(productionOrderDetail.ProducedQty);      
        $('#FormProductionOrderDetail #UnitCode').val(productionOrderDetail.UnitCode);
        $('#FormProductionOrderDetail #hdnUnitCode').val(productionOrderDetail.UnitCode);
        $('#FormProductionOrderDetail #Rate').val(productionOrderDetail.Rate);
        $('#FormProductionOrderDetail #PlantCode').val(productionOrderDetail.PlantCode);
        $('#FormProductionOrderDetail #MileStone1FcFinishDt').val(productionOrderDetail.MileStone1FcFinishDt);
        $('#FormProductionOrderDetail #MileStone1AcTFinishDt').val(productionOrderDetail.MileStone1AcTFinishDt);
        $('#FormProductionOrderDetail #MileStone2FcFinishDt').val(productionOrderDetail.MileStone2FcFinishDt);
        $('#FormProductionOrderDetail #MileStone2AcTFinishDt').val(productionOrderDetail.MileStone2AcTFinishDt);
        $('#FormProductionOrderDetail #MileStone3FcFinishDt').val(productionOrderDetail.MileStone3FcFinishDt);
        $('#FormProductionOrderDetail #MileStone3AcTFinishDt').val(productionOrderDetail.MileStone3AcTFinishDt);
        $('#FormProductionOrderDetail #MileStone4FcFinishDt').val(productionOrderDetail.MileStone4FcFinishDt);
        $('#FormProductionOrderDetail #MileStone4AcTFinishDt').val(productionOrderDetail.MileStone4AcTFinishDt);
        $('#divModelPopProductionOrder').modal('show');
    });
}

function ConfirmDeleteProductionOrderDetail(this_Obj) {
    debugger;
    _datatablerowindex = _dataTable.ProductionOrderDetailList.row($(this_Obj).parents('tr')).index();
    var productionOrderDetail = _dataTable.ProductionOrderDetailList.row($(this_Obj).parents('tr')).data();
    if (productionOrderDetail.ID === _emptyGuid) {
        var productionOrderDetailList = _dataTable.ProductionOrderDetailList.rows().data();
        productionOrderDetailList.splice(_datatablerowindex, 1);
        _dataTable.ProductionOrderDetailList.clear().rows.add(productionOrderDetailList).draw(false);
        notyAlert('success', 'Detail Row deleted successfully');
    }
    else {
        notyConfirm('Are you sure to delete?', 'DeleteProductionOrderDetail("' + productionOrderDetail.ID + '")');

    }
}

function DeleteProductionOrderDetail(ID) {
    if (ID != _emptyGuid && ID != null && ID != '') {
        var data = { "id": ID };
        var ds = {};
        _jsonData = GetDataFromServer("ProductionOrder/DeleteProductionOrderDetail/", data);
        if (_jsonData != '') {
            _jsonData = JSON.parse(_jsonData);
            _message = _jsonData.Message;
            _status = _jsonData.Status;
            _result = _jsonData.Record;
        }
        if (_status == "OK") {
            notyAlert('success', _result.Message);
            var productionOrderDetailList = _dataTable.ProductionOrderDetailList.rows().data();
            productionOrderDetailList.splice(_datatablerowindex, 1);
            _dataTable.ProductionOrderDetailList.clear().rows.add(productionOrderDetailList).draw(false);
        }
        if (_status == "ERROR") {
            notyAlert('error', _message);
        }
    }
}