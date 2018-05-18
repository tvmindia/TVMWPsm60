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
        BindOrReloadDeliveryChallanTable('Init');
        $('#tblDeliveryChallan tbody').on('dblclick', 'td', function () {
            EditDeliveryChallan(this);
        });
    }
    catch (e) {
        console.log(e.message);
    }
});

//function bind the SaleOrder list checking search and filter
function BindOrReloadDeliveryChallanTable(action) {
    try {
        debugger;
        //creating advancesearch object
        DeliveryChallanAdvanceSearchViewModel = new Object();
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
        DeliveryChallanAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
        DeliveryChallanAdvanceSearchViewModel.SearchTerm = $('#SearchTerm').val();
        DeliveryChallanAdvanceSearchViewModel.FromDate = $('#FromDate').val();
        DeliveryChallanAdvanceSearchViewModel.ToDate = $('#ToDate').val();
        //apply datatable plugin on SaleOrder table
        _dataTable.DeliveryChallanList = $('#tblDeliveryChallan').DataTable(
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
                url: "DeliveryChallan/GetAllDeliveryChallan/",
                data: { "deliveryChallanAdvanceSearchVM": DeliveryChallanAdvanceSearchViewModel },
                type: 'POST'
            },
            pageLength: 13,
            columns: [
               { "data": "DelvChallanNo", "defaultContent": "<i>-</i>" },
               { "data": "DelvChallanRefNo", "defaultContent": "<i>-</i>" },
               { "data": "DelvChallanDateFormatted", "defaultContent": "<i>-</i>" },
               { "data": "Customer.CompanyName", "defaultContent": "<i>-</i>" },
               { "data": "Branch.Description", "defaultContent": "<i>-</i>" },               
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
               { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink"  onclick="EditDeliveryChallan(this)" ><i class="fa fa-pencil-square-o" aria-hidden="true"></i></a>' },
            ],
            columnDefs: [
                          { className: "text-left", "targets": [0, 1, 3, 4, 5] },
                          { className: "text-center", "targets": [2,6] },
                            { "targets": [0, 1, 2, 5], "width": "10%" },
                            {"targets": [3,4],"width":"20%"},
                            { "targets": [6], "width": "7%" },

            ],
            destroy: true,
            //for performing the import operation after the data loaded
            initComplete: function (settings, json) {
                debugger;
                $('.dataTables_wrapper div.bottom div').addClass('col-md-6');
                $('#tblDeliveryChallan').fadeIn('slow');
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
                    BindOrReloadDeliveryChallanTable();
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
function ResetDeliveryChallanList() {
    $(".searchicon").removeClass('filterApplied');
    BindOrReloadDeliveryChallanTable('Reset');
}
//function export data to excel
function ExportDeliveryChallanData() {
    $('.excelExport').show();
    OnServerCallBegin();
    BindOrReloadDeliveryChallanTable('Export');
}

// add DeliveryChallan section
function AddDeliveryChallan() {
    debugger;
    //this will return form body(html)
    OnServerCallBegin();
    $("#divDeliveryChallanForm").load("DeliveryChallan/DeliveryChallanForm?id=" + _emptyGuid + "&prodOrder=", function () {
        $('#lblDeliveryChallanInfo').text('<<DeliveryChallan No.>>');
        ChangeButtonPatchView("DeliveryChallan", "btnPatchDeliveryChallanNew", "Add");
        BindDeliveryChallanDetailList(_emptyGuid);
        OnServerCallComplete();
        //resides in customjs for sliding       
        openNav();        
    });
}

function EditDeliveryChallan(this_Obj) {
    debugger;
    OnServerCallBegin();
    var DeliveryChallan = _dataTable.DeliveryChallanList.row($(this_Obj).parents('tr')).data();
    $('#lblDeliveryChallanInfo').text(DeliveryChallan.DelvChallanNo);
    //this will return form body(html)
    $("#divDeliveryChallanForm").load("DeliveryChallan/DeliveryChallanForm?id=" + "&prodOrderID=" + DeliveryChallan.ProdOrderID, function () {

        ChangeButtonPatchView("DeliveryChallan", "btnPatchDeliveryChallanNew", "Edit");
        BindDeliveryChallanDetailList(DeliveryChallan.ID);
        $('#divCustomerBasicInfo').load("Customer/CustomerBasicInfo?ID=" + $('#hdnCustomerID').val());
        clearUploadControl();
        PaintImages(DeliveryChallan.ID);
        OnServerCallComplete();
        //resides in customjs for sliding
        setTimeout(function () {
            $("#divDeliveryChallanForm #SaleOrderID #ProdOrderID").prop('disabled', true);
            openNav();
        }, 100);
    });
}

function ResetDeliveryChallan() {
    //this will return form body(html)
    $("#divDeliveryChallanForm").load("DeliveryChallan/DeliveryChallanForm?id=" + $('#DeliveryChallanForm #ID').val() + "&prodOrderID=" + $('#hdnProdOrderID').val(), function () {
        if ($('#ID').val() != _emptyGuid && $('#ID').val() != null) {
            //resides in customjs for sliding

            openNav();
        }
        BindDeliveryChallanDetailList($('#ID').val(), false);
        clearUploadControl();
        PaintImages($('#DeliveryChallanForm #ID').val());
        $('#divCustomerBasicInfo').load("Customer/CustomerBasicInfo?ID=" + $('#DeliveryChallanForm #hdnCustomerID').val());
    });
}
function SaveDeliveryChallan() {
    var deliveryChallanDetailList = _dataTable.DeliveryChallanDetailList.rows().data().toArray();
    $('#DetailJSON').val(JSON.stringify(deliveryChallanDetailList));
    $('#btnInsertUpdateDeliveryChallan').trigger('click');
}
function ApplyFilterThenSearch() {
    $(".searchicon").addClass('filterApplied');
    CloseAdvanceSearch();
    BindOrReloadDeliveryChallanTable('Search');
}

function SaveSuccessDeliveryChallan(data, status) {
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
                $("#divDeliveryChallanForm").load("DeliveryChallan/DeliveryChallanForm?id=" + _result.ID +"&prodOrderID="+_result.ProdOrderID, function () {
                    ChangeButtonPatchView("DeliveryChallan", "btnPatchDeliveryChallanNew", "Edit");
                    BindDeliveryChallanDetailList(_result.ID);
                    clearUploadControl();
                    PaintImages(_result.ID);
                    $('#lblDeliveryChallanInfo').text(_result.DeliveryChallanNo);

                });
                ChangeButtonPatchView("DeliveryChallan", "btnPatchDeliveryChallanNew", "Edit");
                BindOrReloadDeliveryChallanTable('Init');
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

function DeleteDeliveryChallan() {
    debugger;
    notyConfirm('Are you sure to delete?', 'DeleteDeliveryChallanItem("' + $('#DeliveryChallanForm #ID').val() + '")');
}
function DeleteDeliveryChallanItem(id) {
    debugger;
    try {
        if (id) {
            var data = { "id": id };
            _jsonData = GetDataFromServer("DeliveryChallan/DeleteDeliveryChallan/", data);
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
                    ChangeButtonPatchView("DeliveryChallan", "btnPatchDeliveryChallanNew", "Add");
                    ResetDeliveryChallan();
                    BindOrReloadDeliveryChallanTable('Init');
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

function BindDeliveryChallanDetailList(id,IsProdOrder) {
    debugger;
    _dataTable.DeliveryChallanDetailList = $('#tblDeliveryChallanDetails').DataTable(
         {
             dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
             order: [],
             searching: false,
             paging: false,
             ordering: false,
             bInfo: false,
             data: //id==_emptyGuid?null: GetDeliveryChallanDetailListByDeliveryChallanID(id),
                 
                 !IsProdOrder ? id == _emptyGuid ? null : GetDeliveryChallanDetailListByDeliveryChallanID(id, false) : GetDeliveryChallanDetailListByDeliveryChallanID(id, true),
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
                 "data": "OrderQty", render: function (data, type, row) {
                     return data + " " + row.Unit.Description
                 }, "defaultContent": "<i></i>"
             },
             //{ "data": "Unit.Description", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
            {
                "data": "DelvQty", render: function (data, type, row) {
                    return data + " " + row.Unit.Description
                }, "defaultContent": "<i></i>"
            },
             {
                 "data": "DelvQty", render: function (data, type, row) {
                     var curDelQty = roundoff(parseFloat(row.OrderQty) - parseFloat(row.DelvQty));
                     if (curDelQty >= 0)
                     { return curDelQty; }
                     else
                         return 0;
                     
                     //return  data + " " + row.Unit.Description
                 }, "defaultContent": "<i></i>"
             },
            { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink"  onclick="EditDeliveryChallanDetail(this)" ><i class="fa fa-pencil-square-o" aria-hidden="true"></i></a> <a href="#" class="DeleteLink"  onclick="ConfirmDeleteDeliveryChallanDetail(this)" ><i class="fa fa-trash-o" aria-hidden="true"></i></a>' },
             ],
             columnDefs: [
                  { className: "text-left", "targets": [0] },
                 { className: "text-right", "targets": [1, 2, 3] },
                 { className: "text-center", "targets": [4] },
                 { "targets": [0], "width": "30%" },
                 { "targets": [1,2,3], "width": "10%" },                 
                 { "targets": [4], "width": "7%" },                                
             ],
             destroy: true
         });
    $('[data-toggle="popover"]').popover({
        html: true,
        'trigger': 'hover',
        'placement': 'top',

    });
}

function GetDeliveryChallanDetailListByDeliveryChallanID(id, IsProdOrder) {
    try {
        debugger;

        var deliveryChallanDetailList = [];
        if (IsProdOrder) {
            debugger;
            var data = { "prodOrderID": $('#DeliveryChallanForm #hdnProdOrderID').val() };
            _jsonData = GetDataFromServer("DeliveryChallan/GetDeliveryChallanDetailListByDeliveryChallanIDWithProductionOrder/", data);
        }
        else {
            var data = { "deliveryChallanID": id };
            _jsonData = GetDataFromServer("DeliveryChallan/GetDeliveryChallanDetailListByDeliveryChallanID/", data);
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

function AddDeliveryChallanDetailList() {
    debugger;
    $("#divModelDeliveryChallanPopBody").load("DeliveryChallan/AddDeliveryChallanDetail", function () {
        $('#lblModelPopDeliveryChallan').text('DeliveryChallan Detail')
        $('#divModelPopDeliveryChallan').modal('show');
    });
}

function AddDeliveryChallanDetailToList() {
    debugger;
    //$("#FormProductionOrderDetail").submit(function () { });
    if ($('#FormDeliveryChallanDetail #IsUpdate').val() == 'True') {
        if (($('#ProductID').val() != "") && ($('#ProductModelID').val() != "") && ($('#ProductSpec').val() != "") && ($('#UnitCode').val() != "")) {
            debugger;
            var deliveryChallanDetailList = _dataTable.DeliveryChallanDetailList.rows().data();
            deliveryChallanDetailList[_datatablerowindex].Product.Code = $("#ProductID").val() != "" ? $("#ProductID option:selected").text().split("-")[0].trim() : "";
            deliveryChallanDetailList[_datatablerowindex].Product.Name = $("#ProductID").val() != "" ? $("#ProductID option:selected").text().split("-")[1].trim() : "";
            deliveryChallanDetailList[_datatablerowindex].ProductID = $("#ProductID").val() != "" ? $("#ProductID").val() : _emptyGuid;
            deliveryChallanDetailList[_datatablerowindex].ProductModelID = $("#ProductModelID").val() != "" ? $("#ProductModelID").val() : _emptyGuid;
            ProductModel = new Object;
            Unit = new Object;
            Plant = new Object;
            ProductModel.Name = $("#ProductModelID").val() != "" ? $("#ProductModelID option:selected").text() : "";
            deliveryChallanDetailList[_datatablerowindex].ProductModel = ProductModel;
            deliveryChallanDetailList[_datatablerowindex].ProductSpec = $('#ProductSpec').val();
            deliveryChallanDetailList[_datatablerowindex].OrderQty = $('#OrderQty').val();
            deliveryChallanDetailList[_datatablerowindex].DelvQty = $('#DelvQty').val();
            deliveryChallanDetailList[_datatablerowindex].UnitCode = $('#UnitCode').val();
            Unit.Description = $("#UnitCode").val() != "" ? $("#UnitCode option:selected").text().trim() : "";
            deliveryChallanDetailList[_datatablerowindex].Unit = Unit;
            
            _dataTable.DeliveryChallanDetailList.clear().rows.add(deliveryChallanDetailList).draw(false);
            $('#divModelPopDeliveryChallan').modal('hide');
            _datatablerowindex = -1;
        }
    }
    else {
        if (($('#ProductID').val() != "") && ($('#ProductModelID').val() != "") && ($('#ProductSpec').val() != "") && ($('#UnitCode').val() != "")) {
            debugger;
            if (_dataTable.DeliveryChallanDetailList.rows().data().length === 0) {
                _dataTable.DeliveryChallanDetailList.clear().rows.add(GetDeliveryChallanDetailListByDeliveryChallanID(_emptyGuid)).draw(false);
                debugger;
                var deliveryChallanDetailList = _dataTable.DeliveryChallanDetailList.rows().data();
                deliveryChallanDetailList[0].Product.Code = $("#ProductID").val() != "" ? $("#ProductID option:selected").text().split("-")[0].trim() : "";
                deliveryChallanDetailList[0].Product.Name = $("#ProductID").val() != "" ? $("#ProductID option:selected").text().split("-")[1].trim() : "";
                deliveryChallanDetailList[0].ProductID = $("#ProductID").val() != "" ? $("#ProductID").val() : _emptyGuid;
                deliveryChallanDetailList[0].ProductModelID = $("#ProductModelID").val() != "" ? $("#ProductModelID").val() : _emptyGuid;
                ProductModel = new Object;
                Unit = new Object;
                Plant = new Object;
                ProductModel.Name = $("#ProductModelID").val() != "" ? $("#ProductModelID option:selected").text() : "";
                deliveryChallanDetailList[0].ProductModel = ProductModel;
                deliveryChallanDetailList[0].ProductSpec = $('#ProductSpec').val();
                deliveryChallanDetailList[0].OrderQty = $('#OrderQty').val();
                deliveryChallanDetailList[0].DelvQty = $('#DelvQty').val();
                deliveryChallanDetailList[0].UnitCode = $('#UnitCode').val();
                Unit.Description = $("#UnitCode").val() != "" ? $("#UnitCode option:selected").text().trim() : "";
                deliveryChallanDetailList[0].Unit = Unit;               
                _dataTable.DeliveryChallanDetailList.clear().rows.add(deliveryChallanDetailList).draw(false);
                $('#divModelPopDeliveryChallan').modal('hide');
            }
            else {
                //if ($('#ProductID').val() != "") {
                debugger;
                //if (ProductionOrderDetailVM != null) {
                var deliveryChallanDetailList = _dataTable.DeliveryChallanDetailList.rows().data();
                if (deliveryChallanDetailList.length > 0) {
                    var checkpoint = 0;
                    for (var i = 0; i < deliveryChallanDetailList.length; i++) {
                        if ((deliveryChallanDetailList[i].ProductID == $('#ProductID').val()) && (deliveryChallanDetailList[i].ProductModelID == $('#ProductModelID').val()
                            && (deliveryChallanDetailList[i].ProductSpec == $('#ProductSpec').val() && (deliveryChallanDetailList[i].OrderQty == $('#OrderQty').val())))) {
                            deliveryChallanDetailList[i].DelvQty = parseFloat(deliveryChallanDetailList[i].DelvQty) + parseFloat($('#DelvQty').val());
                            checkpoint = 1;
                            break;
                        }
                    }
                    if (checkpoint == 1) {
                        debugger;
                        _dataTable.DeliveryChallanDetailList.clear().rows.add(deliveryChallanDetailList).draw(false);
                        $('#divModelPopDeliveryChallan').modal('hide');
                    }
                    else if (checkpoint == 0) {
                        var DeliveryChallanDetailVM = new Object();
                        var Product = new Object;
                        var ProductModel = new Object()
                        var Unit = new Object();
                     
                        DeliveryChallanDetailVM.ID = _emptyGuid;
                        DeliveryChallanDetailVM.ProductID = $("#ProductID").val() != "" ? $("#ProductID").val() : _emptyGuid;
                        Product.Code = $("#ProductID").val() != "" ? $("#ProductID option:selected").text().split("-")[0].trim() : "";
                        Product.Name = $("#ProductID").val() != "" ? $("#ProductID option:selected").text().split("-")[1].trim() : "";
                        DeliveryChallanDetailVM.Product = Product;
                        DeliveryChallanDetailVM.ProductModelID = $("#ProductModelID").val() != "" ? $("#ProductModelID").val() : _emptyGuid;
                        ProductModel.Name = $("#ProductModelID").val() != "" ? $("#ProductModelID option:selected").text() : "";
                        DeliveryChallanDetailVM.ProductModel = ProductModel;
                        DeliveryChallanDetailVM.ProductSpec = $('#ProductSpec').val();
                        DeliveryChallanDetailVM.OrderQty = $('#OrderQty').val();
                        DeliveryChallanDetailVM.DelvQty = $('#DelvQty').val();
                        Unit.Description = $("#UnitCode").val() != "" ? $("#UnitCode option:selected").text().trim() : "";
                        DeliveryChallanDetailVM.Unit = Unit;
                        DeliveryChallanDetailVM.UnitCode = $('#UnitCode').val();                      
                        _dataTable.DeliveryChallanDetailList.row.add(DeliveryChallanDetailVM).draw(false);
                        $('#divModelPopDeliveryChallan').modal('hide');
                    }
                }
                //}

            }
        }
    }
    $('[data-toggle="popover"]').popover({
        html: true,
        'trigger': 'hover',
        'placement': 'top',

    });

}

function EditDeliveryChallanDetail(this_Obj) {
    debugger;
    _datatablerowindex = _dataTable.DeliveryChallanDetailList.row($(this_Obj).parents('tr')).index();
    var deliveryChallanDetail = _dataTable.DeliveryChallanDetailList.row($(this_Obj).parents('tr')).data();
    $("#divModelDeliveryChallanPopBody").load("DeliveryChallan/AddDeliveryChallanDetail", function () {
        $('#lblModelPopDeliveryChallan').text('DeliveryChallan Detail')
        $('#FormDeliveryChallanDetail #IsUpdate').val('True');
        $('#FormDeliveryChallanDetail #ID').val(deliveryChallanDetail.ID);
        $("#FormDeliveryChallanDetail #ProductID").val(deliveryChallanDetail.ProductID)
        $("#FormDeliveryChallanDetail #hdnProductID").val(deliveryChallanDetail.ProductID)
        $('#divProductBasicInfo').load("Product/ProductBasicInfo?ID=" + $('#hdnProductID').val(), function () {
        });

        if ($('#hdnProductID').val() != _emptyGuid) {
            $('.divProductModelSelectList').load("ProductModel/ProductModelSelectList?required=required&productID=" + $('#hdnProductID').val())
        }
        else {
            $('.divProductModelSelectList').empty();
            $('.divProductModelSelectList').append('<span class="form-control newinput"><i id="dropLoad" class="fa fa-spinner"></i></span>');
        }
        $("#FormDeliveryChallanDetail #ProductModelID").val(deliveryChallanDetail.ProductModelID);
        $("#FormDeliveryChallanDetail #hdnProductModelID").val(deliveryChallanDetail.ProductModelID);
        if ($('#hdnProductModelID').val() != _emptyGuid) {
            $('#divProductBasicInfo').load("ProductModel/ProductModelBasicInfo?ID=" + $('#hdnProductModelID').val(), function () {
            });
        }
        $('#FormDeliveryChallanDetail #ProductSpec').val(deliveryChallanDetail.ProductSpec);
        $('#FormDeliveryChallanDetail #OrderQty').val(deliveryChallanDetail.OrderQty);
        $('#FormDeliveryChallanDetail #DelvQty').val(deliveryChallanDetail.DelvQty);
        $('#FormDeliveryChallanDetail #UnitCode').val(deliveryChallanDetail.UnitCode);
        $('#FormDeliveryChallanDetail #hdnUnitCode').val(deliveryChallanDetail.UnitCode);
        $('#divModelPopDeliveryChallan').modal('show');
    });
}

function ConfirmDeleteDeliveryChallanDetail(this_Obj) {
    debugger;
    _datatablerowindex = _dataTable.DeliveryChallanDetailList.row($(this_Obj).parents('tr')).index();
    var deliveryChallanDetail = _dataTable.DeliveryChallanDetailList.row($(this_Obj).parents('tr')).data();
    if (deliveryChallanDetail.ID === _emptyGuid) {
        var deliveryChallanDetailList = _dataTable.DeliveryChallanDetailList.rows().data();
        deliveryChallanDetailList.splice(_datatablerowindex, 1);
        _dataTable.DeliveryChallanDetailList.clear().rows.add(deliveryChallanDetailList).draw(false);
        notyAlert('success', 'Detail Row deleted successfully');
    }
    else {
        notyConfirm('Are you sure to delete?', 'DeleteDeliveryChallanDetail("' + deliveryChallanDetail.ID + '")');

    }
}

function DeleteDeliveryChallanDetail(ID) {
    if (ID != _emptyGuid && ID != null && ID != '') {
        var data = { "id": ID };
        var ds = {};
        _jsonData = GetDataFromServer("DeliveryChallan/DeleteDeliveryChallanDetail/", data);
        if (_jsonData != '') {
            _jsonData = JSON.parse(_jsonData);
            _message = _jsonData.Message;
            _status = _jsonData.Status;
            _result = _jsonData.Record;
        }
        if (_status == "OK") {
            notyAlert('success', _result.Message);
            var deliveryChallanDetailList = _dataTable.DeliveryChallanDetailList.rows().data();
            deliveryChallanDetailList.splice(_datatablerowindex, 1);
            _dataTable.DeliveryChallanDetailList.clear().rows.add(deliveryChallanDetailList).draw(false);
        }
        if (_status == "ERROR") {
            notyAlert('error', _message);
        }
    }
}