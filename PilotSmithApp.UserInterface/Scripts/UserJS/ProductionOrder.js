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
        debugger;
        BindOrReloadProductionOrderTable('Init');
        $('#tblProductionOrder tbody').on('dblclick', 'td', function () {
            if (this.textContent !== "No data available in table")
            EditProductionOrder(this);
        });
        debugger;
        if ($('#RedirectToDocument').val() != "")
        {
            EditRedirectToDocument($('#RedirectToDocument').val());
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
                $('.divboxASearch #AdvFromDate').val('');
                $('.divboxASearch #AdvToDate').val('');
                $('.divboxASearch #AdvCustomerID').val('').trigger('change');              
                $('.divboxASearch #AdvBranchCode').val('').trigger('change');
                $('.divboxASearch #AdvAreaCode').val('').trigger('change');
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
                $('.divboxASearch #AdvBranchCode').val('');
                $('.divboxASearch #AdvDocumentStatusCode').val('');
                $('.divboxASearch #AdvDocumentOwnerID').val('');
                $('.divboxASearch #AdvApprovalStatusCode').val('');
                $('#AdvEmailSentStatus').val('');
                break;
            case 'Search':
                if (($('#SearchTerm').val() == "") && ($('.divboxASearch #AdvFromDate').val() == "") && ($('.divboxASearch #AdvToDate').val() == "") && ($('.divboxASearch #AdvAreaCode').val() == "") && ($('.divboxASearch #AdvCustomerID').val() == "") && ($('.divboxASearch #AdvBranchCode').val() == "") && ($('.divboxASearch #AdvDocumentStatusCode').val() == "") && ($('.divboxASearch #AdvDocumentOwnerID').val() == "") && ($('#AdvEmailSentStatus').val() == "") && ($('#AdvApprovalStatusCode').val() == "")) {
                    return true;
                }
                break;
            case 'Export':
                debugger;
                DataTablePagingViewModel.Length = -1;
                ProductionOrderAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
                ProductionOrderAdvanceSearchViewModel.SearchTerm = $('#SearchTerm').val() == "" ? null : $('#SearchTerm').val();
                ProductionOrderAdvanceSearchViewModel.AdvFromDate = $('.divboxASearch #AdvFromDate').val() == "" ? null : $('.divboxASearch #AdvFromDate').val();
                ProductionOrderAdvanceSearchViewModel.AdvToDate = $('.divboxASearch #AdvToDate').val() == "" ? null : $('.divboxASearch #AdvToDate').val();
                ProductionOrderAdvanceSearchViewModel.AdvAreaCode = $('.divboxASearch #AdvAreaCode').val() == "" ? null : $('.divboxASearch #AdvAreaCode').val();
                ProductionOrderAdvanceSearchViewModel.AdvCustomerID = $('.divboxASearch #AdvCustomerID').val() == "" ? _emptyGuid : $('.divboxASearch #AdvCustomerID').val();
                ProductionOrderAdvanceSearchViewModel.AdvBranchCode = $('.divboxASearch #AdvBranchCode').val() == "" ? null : $('.divboxASearch #AdvBranchCode').val();
                ProductionOrderAdvanceSearchViewModel.AdvDocumentStatusCode = $('.divboxASearch #AdvDocumentStatusCode').val() == "" ? null : $('.divboxASearch #AdvDocumentStatusCode').val();
                ProductionOrderAdvanceSearchViewModel.AdvDocumentOwnerID = $('.divboxASearch #AdvDocumentOwnerID').val() == "" ? _emptyGuid : $('.divboxASearch #AdvDocumentOwnerID').val();
                ProductionOrderAdvanceSearchViewModel.AdvApprovalStatusCode = $('.divboxASearch #AdvApprovalStatusCode').val() == "" ? null : $('.divboxASearch #AdvApprovalStatusCode').val();
                ProductionOrderAdvanceSearchViewModel.AdvEmailSentStatus = $('#AdvEmailSentStatus').val() == "" ? null : $('#AdvEmailSentStatus').val();
                $('#AdvanceSearch').val(JSON.stringify(ProductionOrderAdvanceSearchViewModel));
                $('#FormExcelExport').submit();
                return true;
                break;
            default:
                break;
        }
        ProductionOrderAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
        ProductionOrderAdvanceSearchViewModel.SearchTerm = $('#SearchTerm').val();        
        ProductionOrderAdvanceSearchViewModel.AdvFromDate = $('.divboxASearch #AdvFromDate').val();
        ProductionOrderAdvanceSearchViewModel.AdvToDate = $('.divboxASearch #AdvToDate').val();
        ProductionOrderAdvanceSearchViewModel.AdvAreaCode = $('.divboxASearch #AdvAreaCode').val();
        ProductionOrderAdvanceSearchViewModel.AdvCustomerID = $('.divboxASearch #AdvCustomerID').val();       
        ProductionOrderAdvanceSearchViewModel.AdvBranchCode = $('.divboxASearch #AdvBranchCode').val();
        ProductionOrderAdvanceSearchViewModel.AdvDocumentStatusCode = $('.divboxASearch #AdvDocumentStatusCode').val();
        ProductionOrderAdvanceSearchViewModel.AdvDocumentOwnerID = $('.divboxASearch #AdvDocumentOwnerID').val();
        ProductionOrderAdvanceSearchViewModel.AdvApprovalStatusCode = $('.divboxASearch #AdvApprovalStatusCode').val();
        ProductionOrderAdvanceSearchViewModel.AdvEmailSentStatus = $('#AdvEmailSentStatus').val();

        //apply datatable plugin on ProductionOrder table
        _dataTable.ProductionOrderList = $('#tblProductionOrder').DataTable(
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
                url: "ProductionOrder/GetAllProductionOrder/",
                data: { "ProductionOrderAdvanceSearchVM": ProductionOrderAdvanceSearchViewModel },
                type: 'POST'
            },
            pageLength: 8,
            columns: [   
                {
                    "data": "ProdOrderNo", render: function (data, type, row) {
                        return row.ProdOrderNo + "</br>" + "<img src='./Content/images/datePicker.png' height='10px'>" + "&nbsp;" + row.ProdOrderDateFormatted;
                    }, "defaultContent": "<i>-</i>"
                },
               {
                   "data": "Customer.CompanyName", render: function (data, type, row) {
                       return "<img src='./Content/images/contact.png' height='10px'>" + "&nbsp;" + (row.Customer.ContactPerson == null ? "" : row.Customer.ContactPerson) + "</br>" + "<img src='./Content/images/organisation.png' height='10px'>" + "&nbsp;" + data;
                   }, "defaultContent": "<i>-</i>"
               },              
               { "data": "Area.Description", "defaultContent": "<i>-</i>" },             
               {
                   "data": "Branch.Description", render: function (data, type, row) {
                       return "<b>Doc.Owner-</b>" + row.PSAUser.LoginName + "</br>" + "<b>Branch-</b>" + row.Branch.Description;
                   }, "defaultContent": "<i>-</i>"
               },
               {
                   "data": "DocumentStatus.Description", render: function (data, type, row) {
                       return "<b>Doc.Status-</b>" + data + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + (row.EmailSentYN == true ? "<img src='./Content/images/mailSend.png' height='20px' >" : '') + "</br>" + "<b>Appr.Status-</b>" + row.ApprovalStatus.Description;
                   }, "defaultContent": "<i>-</i>"
               },




               { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink"  onclick="EditProductionOrder(this)" ><i class="fa fa-pencil-square-o" aria-hidden="true"></i></a>' },
            ],
            columnDefs: [
                          { className: "text-left", "targets": [0,1, 2, 3, 4] },
                          { className: "text-center", "targets": [5] },
                            { "targets": [0,1], "width": "12%" },
                            { "targets": [2], "width": "15%" },
                             { "targets": [3], "width": "15%" },
                            { "targets": [4], "width": "24%" },                      
                            { "targets": [5], "width": "3%" },                         
                           

            ],
            destroy: true,
            //for performing the import operation after the data loaded
            initComplete: function (settings, json) {
                debugger;
                $('.dataTables_wrapper div.bottom div').addClass('col-md-6');
                $('#tblProductionOrder').fadeIn('slow');
                if (action == undefined) {
                    //$('.excelExport').hide();
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
function ResetProductionOrderList() {
    $(".searchicon").removeClass('filterApplied');
    BindOrReloadProductionOrderTable('Reset');
}
//function export data to excel
function ExportProductionOrderData() {   
    BindOrReloadProductionOrderTable('Export');
}
// add ProductionOrder section
function AddProductionOrder() {
    debugger;
    //this will return form body(html)
    OnServerCallBegin();
    $("#divProductionOrderForm").load("ProductionOrder/ProductionOrderForm?id=" + _emptyGuid , function (responseTxt, statusTxt, xhr) {
        if (statusTxt == "success")
        {
            OnServerCallComplete();
            openNav();
            $('#lblProductionOrderInfo').text('<<Production Order No.>>');
            ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "Add");
            BindProductionOrderDetailList(_emptyGuid);
        }
        else {
            console.log("Error: " + xhr.status + ": " + xhr.statusText);
        }

    });
}

function EditProductionOrder(this_Obj) {
    debugger;
    OnServerCallBegin();  
    var productionOrder = _dataTable.ProductionOrderList.row($(this_Obj).parents('tr')).data();
    $('#lblProductionOrderInfo').text(productionOrder.ProdOrderNo);
    //this will return form body(html)
    $("#divProductionOrderForm").load("ProductionOrder/ProductionOrderForm?id=" + productionOrder.ID , function (responseTxt, statusTxt, xhr) {
        if (statusTxt == "success") {
            OnServerCallComplete();
            openNav();
            if ($('#IsDocLocked').val() == "True") {
                debugger;
                switch ($('#LatestApprovalStatus').val()) {
                    case "0":
                        ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "Draft", productionOrder.ID);
                        break;
                    case "3":
                        ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "Edit", productionOrder.ID);
                        break;
                    case "4":
                        ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "Approved", productionOrder.ID);
                        break;
                    default:
                        ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "LockDocument", productionOrder.ID);
                        break;
                }
            }
            else {
                ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "LockDocument", productionOrder.ID);
            }
            BindProductionOrderDetailList(productionOrder.ID);
            $('#divCustomerBasicInfo').load("Customer/CustomerBasicInfo?ID=" + $('#hdnCustomerID').val());
            clearUploadControl();
            PaintImages(productionOrder.ID);
            $("#divProductionOrderForm #SaleOrderID").prop('disabled', true);
        }
        else {
            console.log("Error: " + xhr.status + ": " + xhr.statusText);
        }

    });
}

function ResetProductionOrder() {
    debugger;
    //this will return form body(html)
    $("#divProductionOrderForm").load("ProductionOrder/ProductionOrderForm?id=" + $('#ProductionOrderForm #ID').val() , function (responseTxt, statusTxt, xhr) {
        if (statusTxt == "success") {
            if ($('#ID').val() != _emptyGuid && $('#ID').val() != null) {
                //resides in customjs for sliding
                $("#divProductionOrderForm #SaleOrderID").prop('disabled', true);
                openNav();

            }
            else {
                $('#hdnCustomerID').val('');
                $('#lblProductionOrderInfo').text('<<Production Order No.>>');
            }
            if ($('#LatestApprovalStatus').val() == "") {
                ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "Add");
            }
            else if ($('#LatestApprovalStatus').val() == 3 || $('#LatestApprovalStatus').val() == 0) {
                ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "Edit", $('#ID').val());
            }
            else if ($('#LatestApprovalStatus').val() == 4) {
                ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "Approved", $('#ID').val());
            }
            else {
                ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "LockDocument", $('#ID').val());
            }
            BindProductionOrderDetailList($('#ID').val(), false);
            clearUploadControl();
            PaintImages($('#ProductionOrderForm #ID').val());
            $('#divCustomerBasicInfo').load("Customer/CustomerBasicInfo?ID=" + $('#ProductionOrderForm #hdnCustomerID').val());
        }
        else {
            console.log("Error: " + xhr.status + ": " + xhr.statusText);
        }
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
                $("#divProductionOrderForm").load("ProductionOrder/ProductionOrderForm?id=" + _result.ID+"&saleOrderID="+_result.SaleOrderID, function () {
                    ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "Edit", _result.ID);
                    BindProductionOrderDetailList(_result.ID);                   
                    clearUploadControl();
                    PaintImages(_result.ID);
                    $('#lblProductionOrderInfo').text(_result.ProductionOrderNo);
                });
                ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "Edit", _result.ID);
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

function DeleteProductionOrder() {
    notyConfirm('Are you sure to delete?', 'DeleteProductionOrderItem("' + $('#ProductionOrderForm #ID').val() + '")');
}

function DeleteProductionOrderItem(id) {
    try {
        if (id) {
            var data = { "id": id };
            _jsonData = GetDataFromServer("ProductionOrder/DeleteProductionOrder/", data);
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
                    ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "Add");
                    ResetProductionOrder();
                    BindOrReloadProductionOrderTable('Init');
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

function BindProductionOrderDetailList(id,IsSaleOrder) {
    debugger;
    _dataTable.ProductionOrderDetailList = $('#tblProductionOrderDetails').DataTable(
         {
             dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
             order: [],
             searching: false,
             paging: false,
             ordering: false,
             bInfo: false,
             data: //id == _emptyGuid ? null : GetProductionOrderDetailListByProductionOrderID(id),
                    !IsSaleOrder ? id==_emptyGuid?null: GetProductionOrderDetailListByProductionOrderID(id,false) : GetProductionOrderDetailListByProductionOrderID(id,true),

             language: {
                 search: "_INPUT_",
                 searchPlaceholder: "Search"
             },
             columns: [
             {
                 "data": "Product.Code", render: function (data, type, row) {
                     debugger;                    
                     return row.Product.Name + "<br/>" + '<div style="width:100%" class="show-popover" data-placement="top" data-html="true" data-toggle="popover" data-title="<p align=left>Product Specification" data-content="' + row.ProductSpec.replace(/"/g, "&quot") + '</p>"/>' + row.ProductModel.Name
                 }, "defaultContent": "<i></i>"
             },
             {
                 "data": "SaleOrderQty", render: function (data, type, row) {
                     return data + " " + row.Unit.Description
                 }, "defaultContent": "<i></i>"
             },
             {
                 "data": "PrevProducedQty", render: function (data, type, row) {
                     if (row.PrevProducedQty != null) {
                         return data + " " + row.Unit.Description
                     }
                     else
                         return 0 + " " + row.Unit.Description
                 }, "defaultContent": "<i></i>"
             },
             {
                 "data": "OrderQty", render: function (data, type, row) {
                     debugger;
                      //roundoff(parseFloat(row.SaleOrderQty) - parseFloat(row.PrevProducedQty));
                      if (data > 0) {
                         return data + " " + row.Unit.Description
                     }
                     else
                         return 0 + " " + row.Unit.Description
                     //return data + " " + row.Unit.Description
                 }, "defaultContent": "<i></i>"
             },
             {
                 "data": "ProducedQty", render: function (data, type, row) {
                     return data + " " + row.Unit.Description
                 }, "defaultContent": "<i></i>"
             },
             {
                 "data": "Rate", render: function (data, type, row) {
                     if (row.Rate != null) {
                         return data
                     }
                     else
                         return 0
                 }, "defaultContent": "<i></i>"
             },
             {
                 "data": "Amount", render: function (data, type, row) {
                     if (row.Rate != null) {
                         debugger;
                         if (row.SaleOrderQty != 0) {
                             var Amount = roundoff(parseFloat(row.SaleOrderQty) * parseFloat(row.Rate));
                             return Amount
                         }
                         else if (row.OrderQty != 0) {
                             var Amount = roundoff(parseFloat(row.OrderQty) * parseFloat(row.Rate));
                             return Amount
                         }
                         else 
                             return 0
                     }
                     else
                         return 0
                 }, "defaultContent": "<i></i>"
             },
             { "data": "Plant.Description", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
             {
                 "data": "result", render: function (data, type, row) {
                     debugger;
                     var result = "";
                     if ((((row.MileStone1FcFinishDtFormatted != null) && (row.MileStone1FcFinishDtFormatted!="")) && ((row.MileStone1AcTFinishDtFormatted != null) && (row.MileStone1AcTFinishDtFormatted!=""))))
                     {
                         var M1 = '25%'
                         result = '<div class="show-popover text-right" data-html="true" data-toggle="popover" data-placement="top" data-title="<p align=left> Milestone Reach : ' + M1 + '" data-content="1) ⌚ Forecast :  ' + row.MileStone1FcFinishDtFormatted + ' ⌚ Actual :  ' + row.MileStone1AcTFinishDtFormatted + '</p>"/>' + M1
                     }                    
                     if (((row.MileStone2FcFinishDtFormatted != null && (row.MileStone2FcFinishDtFormatted != "")) && (row.MileStone2AcTFinishDtFormatted != null && (row.MileStone2AcTFinishDtFormatted != ""))))
                     {
                         var M2 = '50%'
                         result = '<div class="show-popover text-right" data-html="true" data-toggle="popover" data-placement="top" data-title="<p align=left>Milestone Reach : ' + M2 + '" data-content="2) ⌚ Forecast :  ' + row.MileStone2FcFinishDtFormatted + '⌚ Actual :  ' + row.MileStone2AcTFinishDtFormatted + '<br/>1) ⌚ Forecast :  ' + row.MileStone1FcFinishDtFormatted + ' ⌚ Actual :  ' + row.MileStone1AcTFinishDtFormatted + '</p>"/>' + M2
                     }
                     if (((row.MileStone3FcFinishDtFormatted != null && (row.MileStone3FcFinishDtFormatted != "")) && (row.MileStone3AcTFinishDtFormatted != null && (row.MileStone3AcTFinishDtFormatted != ""))))
                     {
                         var M3 = '75%'
                         result = '<div class="show-popover text-right" data-html="true" data-toggle="popover" data-placement="top" data-title="<p align=left>Milestone Reach : ' + M3 + '" data-content="3) ⌚ Forecast :  ' + row.MileStone3FcFinishDtFormatted + '⌚ Actual :  ' + row.MileStone3AcTFinishDtFormatted + '<br/>2) ⌚ Forecast :  ' + row.MileStone2FcFinishDtFormatted + '⌚ Actual :  ' + row.MileStone2AcTFinishDtFormatted + '<br/>1) ⌚ Forecast :  ' + row.MileStone1FcFinishDtFormatted + ' ⌚ Actual :  ' + row.MileStone1AcTFinishDtFormatted + '</p>"/>' + M3
                     }
                     if (((row.MileStone4FcFinishDtFormatted != null && (row.MileStone4FcFinishDtFormatted != "")) && (row.MileStone4AcTFinishDtFormatted != null && (row.MileStone4AcTFinishDtFormatted != ""))))
                     {
                         var M4 = '100%'
                         result = '<div class="show-popover text-right" data-html="true" data-toggle="popover" data-placement="top" data-title="<p align=left>Milestone Reach : ' + M4 + '" data-content="4) ⌚ Forecast :  ' + row.MileStone4FcFinishDtFormatted + '⌚ Actual :  ' + row.MileStone4AcTFinishDtFormatted + '<br/>3) ⌚ Forecast :  ' + row.MileStone3FcFinishDtFormatted + '⌚ Actual :  ' + row.MileStone3AcTFinishDtFormatted + '<br/>2) ⌚ Forecast :  ' + row.MileStone2FcFinishDtFormatted + '⌚ Actual :  ' + row.MileStone2AcTFinishDtFormatted + '<br/>1) ⌚ Forecast :  ' + row.MileStone1FcFinishDtFormatted + ' ⌚ Actual :  ' + row.MileStone1AcTFinishDtFormatted + '</p>"/>' + M4
                     }
                     //else
                     //{
                     //    result= '<div class="show-popover text-right" data-html="true" data-toggle="popover" data-title="<p align=left>Milestone : % ' + 0 + '" data-content="Forecast Date :  ' + 0 + '<br/>Actual Date :  ' + 0;
                     //}
                     
                     //return '<div class="show-popover text-right" data-html="true" data-toggle="popover" data-title="<p align=left>Grand Total : ₹ ' + GrandTotal + '" data-content="Taxable : ₹ ' + TaxableAmt + '<br/>GST : ₹ ' + GSTAmt + '</p>"/>' + GrandTotal
                     return result;
                 }, "defaultContent": "<i></i>"
             },
             { "data": null, "orderable": false, "defaultContent": ($('#IsDocLocked').val() == "True" || $('#IsUpdate').val() == "False")?'<a href="#" class="actionLink"  onclick="EditProductionOrderDetail(this)" ><i class="fa fa-pencil-square-o" aria-hidden="true"></i></a> <a href="#" class="DeleteLink"  onclick="ConfirmDeleteProductionOrderDetail(this)" ><i class="fa fa-trash-o" aria-hidden="true"></i></a>':'-' },
             ],
             columnDefs: [
                 { className: "text-right", "targets": [1, 2, 3, 4, 5, 6] },
                 { className: "text-left", "targets": [0,7] },
                 {className:"text-center","targets":[8,9]},
                 { "targets": [0], "width": "20%" },
                 { "targets": [1, 2, 3, 4, 5, 6, 7, 8,9], "width": "9%" },                
             ]
         });
    $('[data-toggle="popover"]').popover({
        html: true,
        'trigger': 'hover',
        
    });
   
}

function GetProductionOrderDetailListByProductionOrderID(id,IsSaleOrder) {
    try {
        debugger;

        var productionOrderDetailList = [];
        if (IsSaleOrder) {
            var data = { "saleOrderID": $('#ProductionOrderForm #hdnSaleOrderID').val() };
            _jsonData = GetDataFromServer("ProductionOrder/GetProductionOrderDetailListByProductionOrderIDWithSaleOrder/", data);
        }
        else {
        var data = { "productionOrderID": id };
        _jsonData = GetDataFromServer("ProductionOrder/GetProductionOrderDetailListByProductionOrderID/", data);
        }

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
    if ($('#FormProductionOrderDetail #IsUpdate').val() == 'True') {
        if (($('#ProductID').val() != "") && ($('#ProductModelID').val() != "") && ($('#ProductSpec').val() != "") && ($('#UnitCode').val()!="")) {
            debugger;
            var productionOrderDetailList = _dataTable.ProductionOrderDetailList.rows().data();
            productionOrderDetailList[_datatablerowindex].Product.Code = $("#ProductID").val() != "" ? $("#ProductID option:selected").text().split("-")[0].trim() : "";
            productionOrderDetailList[_datatablerowindex].Product.Name = $("#ProductID").val() != "" ? $("#ProductID option:selected").text().split("-")[1].trim() : "";
            productionOrderDetailList[_datatablerowindex].ProductID = $("#ProductID").val() != "" ? $("#ProductID").val() : _emptyGuid;
            productionOrderDetailList[_datatablerowindex].ProductModelID = $("#ProductModelID").val() != "" ? $("#ProductModelID").val() : _emptyGuid;
            ProductModel = new Object;
            Unit = new Object;
            Plant = new Object;
            ProductModel.Name = $("#ProductModelID").val() != "" ? $("#ProductModelID option:selected").text() : "";
            productionOrderDetailList[_datatablerowindex].ProductModel = ProductModel;
            productionOrderDetailList[_datatablerowindex].ProductSpec = $('#ProductSpec').val();
            productionOrderDetailList[_datatablerowindex].SaleOrderQty = 0;
            productionOrderDetailList[_datatablerowindex].OrderQty = $('#OrderQty').val();
            productionOrderDetailList[_datatablerowindex].ProducedQty = $('#ProducedQty').val();
            productionOrderDetailList[_datatablerowindex].UnitCode = $('#UnitCode').val();
            Unit.Description = $("#UnitCode").val() != "" ? $("#UnitCode option:selected").text().trim() : "";
            productionOrderDetailList[_datatablerowindex].Unit = Unit;
            productionOrderDetailList[_datatablerowindex].Rate = $('#Rate').val();
            productionOrderDetailList[_datatablerowindex].PlantCode = $('#PlantCode').val();
            Plant.Description = $("#PlantCode").val() != "" ? $("#PlantCode option:selected").text().trim() : "";
            productionOrderDetailList[_datatablerowindex].Plant = Plant;
            productionOrderDetailList[_datatablerowindex].MileStone1FcFinishDtFormatted = $('#MileStone1FcFinishDtFormatted').val();
            productionOrderDetailList[_datatablerowindex].MileStone1AcTFinishDtFormatted = $('#MileStone1AcTFinishDtFormatted').val();
            productionOrderDetailList[_datatablerowindex].MileStone2FcFinishDtFormatted = $('#MileStone2FcFinishDtFormatted').val();
            productionOrderDetailList[_datatablerowindex].MileStone2AcTFinishDtFormatted = $('#MileStone2AcTFinishDtFormatted').val();
            productionOrderDetailList[_datatablerowindex].MileStone3FcFinishDtFormatted = $('#MileStone3FcFinishDtFormatted').val();
            productionOrderDetailList[_datatablerowindex].MileStone3AcTFinishDtFormatted = $('#MileStone3AcTFinishDtFormatted').val();
            productionOrderDetailList[_datatablerowindex].MileStone4FcFinishDtFormatted = $('#MileStone4FcFinishDtFormatted').val();
            productionOrderDetailList[_datatablerowindex].MileStone4AcTFinishDtFormatted = $('#MileStone4AcTFinishDtFormatted').val();
            _dataTable.ProductionOrderDetailList.clear().rows.add(productionOrderDetailList).draw(false);
            $('#divModelPopProductionOrder').modal('hide');
            _datatablerowindex = -1;
        }
    }
    else {
        if (($('#ProductID').val() != "") && ($('#ProductModelID').val() != "") && ($('#ProductSpec').val() != "") && ($('#UnitCode').val() != "")) {
            debugger;
            if (_dataTable.ProductionOrderDetailList.rows().data().length === 0)
            {
                _dataTable.ProductionOrderDetailList.clear().rows.add(GetProductionOrderDetailListByProductionOrderID(_emptyGuid)).draw(false);
                debugger;
                var productionOrderDetailList = _dataTable.ProductionOrderDetailList.rows().data();
                productionOrderDetailList[0].Product.Code = $("#ProductID").val() != "" ? $("#ProductID option:selected").text().split("-")[0].trim() : "";
                productionOrderDetailList[0].Product.Name = $("#ProductID").val() != "" ? $("#ProductID option:selected").text().split("-")[1].trim() : "";
                productionOrderDetailList[0].ProductID = $("#ProductID").val() != "" ? $("#ProductID").val() : _emptyGuid;
                productionOrderDetailList[0].ProductModelID = $("#ProductModelID").val() != "" ? $("#ProductModelID").val() : _emptyGuid;
                ProductModel = new Object;
                Unit = new Object;
                Plant = new Object;
                ProductModel.Name = $("#ProductModelID").val() != "" ? $("#ProductModelID option:selected").text() : "";
                productionOrderDetailList[0].ProductModel = ProductModel;
                productionOrderDetailList[0].ProductSpec = $('#ProductSpec').val();
                productionOrderDetailList[0].SaleOrderQty = 0;
                productionOrderDetailList[0].OrderQty = $('#OrderQty').val();
                productionOrderDetailList[0].ProducedQty = $('#ProducedQty').val();
                productionOrderDetailList[0].UnitCode = $('#UnitCode').val();
                Unit.Description = $("#UnitCode").val() != "" ? $("#UnitCode option:selected").text().trim() : "";
                productionOrderDetailList[0].Unit = Unit;
                productionOrderDetailList[0].Rate = $('#Rate').val();
                productionOrderDetailList[0].PlantCode = $('#PlantCode').val();
                Plant.Description = $("#PlantCode").val() != "" ? $("#PlantCode option:selected").text().trim() : "";
                productionOrderDetailList[0].Plant = Plant;
                productionOrderDetailList[0].MileStone1FcFinishDtFormatted = $('#MileStone1FcFinishDtFormatted').val();
                productionOrderDetailList[0].MileStone1AcTFinishDtFormatted = $('#MileStone1AcTFinishDtFormatted').val();
                productionOrderDetailList[0].MileStone2FcFinishDtFormatted = $('#MileStone2FcFinishDtFormatted').val();
                productionOrderDetailList[0].MileStone2AcTFinishDtFormatted = $('#MileStone2AcTFinishDtFormatted').val();
                productionOrderDetailList[0].MileStone3FcFinishDtFormatted = $('#MileStone3FcFinishDtFormatted').val();
                productionOrderDetailList[0].MileStone3AcTFinishDtFormatted = $('#MileStone3AcTFinishDtFormatted').val();
                productionOrderDetailList[0].MileStone4FcFinishDtFormatted = $('#MileStone4FcFinishDtFormatted').val();
                productionOrderDetailList[0].MileStone4AcTFinishDtFormatted = $('#MileStone4AcTFinishDtFormatted').val();
                _dataTable.ProductionOrderDetailList.clear().rows.add(productionOrderDetailList).draw(false);
                $('#divModelPopProductionOrder').modal('hide');
            }
            else {
                //if ($('#ProductID').val() != "") {
                    debugger;
                    //if (ProductionOrderDetailVM != null) {
                    var productionOrderDetailList = _dataTable.ProductionOrderDetailList.rows().data();
                    if (productionOrderDetailList.length > 0) {
                        var checkpoint = 0;
                        for (var i = 0; i < productionOrderDetailList.length; i++) {
                            if ((productionOrderDetailList[i].ProductID == $('#ProductID').val()) && (productionOrderDetailList[i].ProductModelID == $('#ProductModelID').val()
                                && (productionOrderDetailList[i].ProductSpec == $('#ProductSpec').val() && (productionOrderDetailList[i].OrderQty == $('#OrderQty').val())))) {
                                productionOrderDetailList[i].ProducedQty = parseFloat(productionOrderDetailList[i].ProducedQty)+parseFloat($('#ProducedQty').val());
                                checkpoint = 1;
                                break;
                            }
                        }
                        if (checkpoint == 1) {
                            debugger;
                            _dataTable.ProductionOrderDetailList.clear().rows.add(productionOrderDetailList).draw(false);
                            $('#divModelPopProductionOrder').modal('hide');
                        }
                        else if (checkpoint == 0) {
                            var ProductionOrderDetailVM = new Object();
                            var Product = new Object;
                            var ProductModel = new Object()
                            var Unit = new Object();
                            var Plant = new Object();
                            ProductionOrderDetailVM.ID = _emptyGuid;
                            ProductionOrderDetailVM.ProductID = $("#ProductID").val() != "" ? $("#ProductID").val() : _emptyGuid;
                            Product.Code = $("#ProductID").val() != "" ? $("#ProductID option:selected").text().split("-")[0].trim() : "";
                            Product.Name = $("#ProductID").val() != "" ? $("#ProductID option:selected").text().split("-")[1].trim() : "";
                            ProductionOrderDetailVM.Product = Product;
                            ProductionOrderDetailVM.ProductModelID = $("#ProductModelID").val() != "" ? $("#ProductModelID").val() : _emptyGuid;
                            ProductModel.Name = $("#ProductModelID").val() != "" ? $("#ProductModelID option:selected").text() : "";
                            ProductionOrderDetailVM.ProductModel = ProductModel;
                            ProductionOrderDetailVM.ProductSpec = $('#ProductSpec').val();
                            ProductionOrderDetailVM.SaleOrderQty = 0;
                            ProductionOrderDetailVM.OrderQty = $('#OrderQty').val();
                            ProductionOrderDetailVM.ProducedQty = $('#ProducedQty').val();
                            Unit.Description = $("#UnitCode").val() != "" ? $("#UnitCode option:selected").text().trim() : "";
                            ProductionOrderDetailVM.Unit = Unit;
                            ProductionOrderDetailVM.UnitCode = $('#UnitCode').val();
                            ProductionOrderDetailVM.Rate = $('#Rate').val();
                            Plant.Description = $("#PlantCode").val() != "" ? $("#PlantCode option:selected").text().trim() : "";
                            ProductionOrderDetailVM.Plant = Plant;
                            ProductionOrderDetailVM.PlantCode = $('#PlantCode').val();
                            ProductionOrderDetailVM.MileStone1FcFinishDtFormatted = $('#MileStone1FcFinishDtFormatted').val();
                            ProductionOrderDetailVM.MileStone1AcTFinishDtFormatted = $('#MileStone1AcTFinishDtFormatted').val();
                            ProductionOrderDetailVM.MileStone2FcFinishDtFormatted = $('#MileStone2FcFinishDtFormatted').val();
                            ProductionOrderDetailVM.MileStone2AcTFinishDtFormatted = $('#MileStone2AcTFinishDtFormatted').val();
                            ProductionOrderDetailVM.MileStone3FcFinishDtFormatted = $('#MileStone3FcFinishDtFormatted').val();
                            ProductionOrderDetailVM.MileStone3AcTFinishDtFormatted = $('#MileStone3AcTFinishDtFormatted').val();
                            ProductionOrderDetailVM.MileStone4FcFinishDtFormatted = $('#MileStone4FcFinishDtFormatted').val();
                            ProductionOrderDetailVM.MileStone4AcTFinishDtFormatted = $('#MileStone4AcTFinishDtFormatted').val();
                            _dataTable.ProductionOrderDetailList.row.add(ProductionOrderDetailVM).draw(false);
                            $('#divModelPopProductionOrder').modal('hide');
                        }
                    }
                //}

            }
        }
    }
    $('[data-toggle="popover"]').popover({
        html: true,
        'trigger': 'hover',
       
    });
   
}


function EditProductionOrderDetail(this_Obj) {
    debugger;
    _datatablerowindex = _dataTable.ProductionOrderDetailList.row($(this_Obj).parents('tr')).index();
    var productionOrderDetail = _dataTable.ProductionOrderDetailList.row($(this_Obj).parents('tr')).data();
    $("#divModelProductionOrderPopBody").load("ProductionOrder/AddProductionOrderDetail", function () {
        $('#lblModelPopProductionOrder').text('Production Order Detail')
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
        $('#FormProductionOrderDetail #hdnPlantCode').val(productionOrderDetail.PlantCode);
        $('#FormProductionOrderDetail #MileStone1FcFinishDtFormatted').val(productionOrderDetail.MileStone1FcFinishDtFormatted);
        $('#FormProductionOrderDetail #MileStone1AcTFinishDtFormatted').val(productionOrderDetail.MileStone1AcTFinishDtFormatted);
        $('#FormProductionOrderDetail #MileStone2FcFinishDtFormatted').val(productionOrderDetail.MileStone2FcFinishDtFormatted);
        $('#FormProductionOrderDetail #MileStone2AcTFinishDtFormatted').val(productionOrderDetail.MileStone2AcTFinishDtFormatted);
        $('#FormProductionOrderDetail #MileStone3FcFinishDtFormatted').val(productionOrderDetail.MileStone3FcFinishDtFormatted);
        $('#FormProductionOrderDetail #MileStone3AcTFinishDtFormatted').val(productionOrderDetail.MileStone3AcTFinishDtFormatted);
        $('#FormProductionOrderDetail #MileStone4FcFinishDtFormatted').val(productionOrderDetail.MileStone4FcFinishDtFormatted);
        $('#FormProductionOrderDetail #MileStone4AcTFinishDtFormatted').val(productionOrderDetail.MileStone4AcTFinishDtFormatted);
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

//=======================Email=======================================================================

function EmailProductionOrder() {
    debugger;
    $("#divModelEmailProductionOrderBody").load("ProductionOrder/EmailProductionOrder?ID=" + $('#ProductionOrderForm #ID').val() + "&EmailFlag=True", function () {
        $('#lblModelEmailProductionOrder').text('Email Attachment')
        $('#divModelEmailProductionOrder').modal('show');
    });
}
function SendProductionOrderEmail() {
    debugger;
    $('#hdnProductionOrderEMailContent').val($('#divProductionOrderEmailcontainer').html());
    $('#hdnProdOrderNo').val($('#ProdOrderNo').val());
    $('#hdnContactPerson').val($('#ContactPerson').text());
    $('#hdnProdOrderDateFormatted').val($('#ProdOrderDateFormatted').val());
    $('#FormProductionOrderEmailSend #ID').val($('#ProductionOrderForm #ID').val());
}
function UpdateProductionOrderEmailInfo() {
    debugger;    
    $('#FormUpdateProductionOrderEmailInfo #ID').val($('#ProductionOrderForm #ID').val());
}
function DownloadProductionOrder() {
    debugger;
    var bodyContent = $('#divProductionOrderEmailcontainer').html();
    var headerContent = $('#hdnHeadContent').html();
    $('#hdnContent').val(bodyContent);
    $('#hdnHeadContent').val(headerContent);
    var customerName = $("#ProductionOrderForm #CustomerID option:selected").text();
    $('#hdnCustomerName').val(customerName);
}
function SaveSuccessUpdateProductionOrderEmailInfo(data, status) {
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
                $("#divModelEmailProductionOrderBody").load("ProductionOrder/EmailProductionOrder?ID=" + $('#ProductionOrderForm #ID').val() + "&EmailFlag=False", function () {
                    $('#lblModelEmailProductionOrder').text('Email Attachment')
                });
                break;
            case "ERROR":
                MasterAlert("success", _message)
                $('#divModelEmailProductionOrder').modal('hide');
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
function SaveSuccessProductionOrderEmailSend(data, status) {
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
                $('#divModelEmailProductionOrder').modal('hide');
                ResetProductionOrder();
                break;
            case "ERROR":
                MasterAlert("danger", _message)
                $('#divModelEmailProductionOrder').modal('hide');
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
    $("#SendApprovalModalBody").load("DocumentApproval/GetApprovers?documentTypeCode=POD", function () {
        if ($('#LatestApprovalStatus').val() == 3) {
            var documentID = $('#ProductionOrderForm #ID').val();
            var latestApprovalID = $('#ProductionOrderForm #LatestApprovalID').val();
            ReSendDocForApproval(documentID, documentTypeCode, latestApprovalID);
           // SendForApproval('POD')
            //BindPurchaseOrder($('#ID').val());
            ResetProductionOrder();
        }
        else {
            $('#SendApprovalModal').modal('show');
        }
    });
}

function SendForApproval(documentTypeCode) {
    debugger;

    var documentID = $('#ProductionOrderForm #ID').val();
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
    ResetProductionOrder();
}

function EditRedirectToDocument(id)
{
    debugger;
    OnServerCallBegin();  
  
    //this will return form body(html)
    $("#divProductionOrderForm").load("ProductionOrder/ProductionOrderForm?id=" + id, function (responseTxt, statusTxt, xhr) {
        if (statusTxt == "success") {
            OnServerCallComplete();
            openNav();
            $('#lblProductionOrderInfo').text($('#ProdOrderNo').val());
            if ($('#IsDocLocked').val() == "True") {
                ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "Edit", id);
            }
            else {
                ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "LockDocument", id);
            }
            BindProductionOrderDetailList(id);
            $('#divCustomerBasicInfo').load("Customer/CustomerBasicInfo?ID=" + $('#hdnCustomerID').val());
            clearUploadControl();
            PaintImages(id);
            
            //resides in customjs for sliding
            $("#divProductionOrderForm #SaleOrderID").prop('disabled', true);
            
        }
        else {
            console.log("Error: " + xhr.status + ": " + xhr.statusText);
        }
    });
}