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
        BindOrReloadDeliveryChallanTable('Init');
        $('#tblDeliveryChallan tbody').on('dblclick', 'td', function () {
            EditDeliveryChallan(this);
        });
        if ($('#RedirectToDocument').val() != "") {
            if ($('#RedirectToDocument').val() === _emptyGuid) {
                AddDeliveryChallan();
            }
            else {
                EditRedirectToDocument($('#RedirectToDocument').val());
            }
        }
    }
    catch (e) {
        console.log(e.message);
    }
    $("#AdvEmailSentStatus").select2({
        dropdownParent: $(".divboxASearch")
    });

    $('.select2').addClass('form-control newinput');
});

//function bind the SaleOrder list checking search and filter
function BindOrReloadDeliveryChallanTable(action) {
    try {
        debugger;
        //creating advancesearch object
        DeliveryChallanAdvanceSearchViewModel = new Object();
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
                $('.divboxASearch #AdvPlantCode').val('').trigger('change');
                $('.divboxASearch #AdvBranchCode').val('').trigger('change');                
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
                $('.divboxASearch #AdvDocumentOwnerID').val('');
                $('.divboxASearch #AdvApprovalStatusCode').val('');
                $('#AdvEmailSentStatus').val('');
                break;
            case 'Search':
                if ((SearchTerm == SerachValue) &&
                    ($('.divboxASearch #AdvFromDate').val() == "") &&
                    ($('.divboxASearch #AdvToDate').val() == "") &&
                    ($('.divboxASearch #AdvAreaCode').val() == "") &&
                    ($('.divboxASearch #AdvCustomerID').val() == "") &&
                    ($('.divboxASearch #AdvPlantCode').val() == "") &&
                    ($('.divboxASearch #AdvBranchCode').val() == "") &&
                   // ($('.divboxASearch #AdvDocumentStatusCode').val() == "") &&
                    ($('.divboxASearch #AdvDocumentOwnerID').val() == "") &&
                    //($('#AdvEmailSentStatus').val() == "") &&
                    ($('#AdvApprovalStatusCode').val() == "")) {
                    return true;
                }
                break;
            case 'Export':
                DataTablePagingViewModel.Length = -1;
                DeliveryChallanAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
                DeliveryChallanAdvanceSearchViewModel.SearchTerm = $('#SearchTerm').val() == "" ? null : $('#SearchTerm').val();
                DeliveryChallanAdvanceSearchViewModel.AdvFromDate = $('.divboxASearch #AdvFromDate').val() == "" ? null : $('.divboxASearch #AdvFromDate').val();
                DeliveryChallanAdvanceSearchViewModel.AdvToDate = $('.divboxASearch #AdvToDate').val() == "" ? null : $('.divboxASearch #AdvToDate').val();
                DeliveryChallanAdvanceSearchViewModel.AdvAreaCode = $('.divboxASearch #AdvAreaCode').val() == "" ? null : $('.divboxASearch #AdvAreaCode').val();
                DeliveryChallanAdvanceSearchViewModel.AdvCustomerID = $('.divboxASearch #AdvCustomerID').val() == "" ? _emptyGuid : $('.divboxASearch #AdvCustomerID').val();
                DeliveryChallanAdvanceSearchViewModel.AdvPlantCode = $('.divboxASearch #AdvPlantCode').val() == "" ? null : $('.divboxASearch #AdvPlantCode').val();
                DeliveryChallanAdvanceSearchViewModel.AdvBranchCode = $('.divboxASearch #AdvBranchCode').val() == "" ? null : $('.divboxASearch #AdvBranchCode').val();
                DeliveryChallanAdvanceSearchViewModel.AdvDocumentOwnerID = $('.divboxASearch #AdvDocumentOwnerID').val() == "" ? _emptyGuid : $('.divboxASearch #AdvDocumentOwnerID').val();
                DeliveryChallanAdvanceSearchViewModel.AdvApprovalStatusCode = $('.divboxASearch #AdvApprovalStatusCode').val() == "" ? null : $('.divboxASearch #AdvApprovalStatusCode').val();
                DeliveryChallanAdvanceSearchViewModel.AdvEmailSentStatus = $('#AdvEmailSentStatus').val() == "" ? null : $('#AdvEmailSentStatus').val();
                $('#AdvanceSearch').val(JSON.stringify(DeliveryChallanAdvanceSearchViewModel));
                $('#FormExcelExport').submit();
                return true;
                break;
            default:
                break;
        }
        DeliveryChallanAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
        DeliveryChallanAdvanceSearchViewModel.SearchTerm = $('#SearchTerm').val();
        DeliveryChallanAdvanceSearchViewModel.AdvFromDate = $('.divboxASearch #AdvFromDate').val();
        DeliveryChallanAdvanceSearchViewModel.AdvToDate = $('.divboxASearch #AdvToDate').val();
        DeliveryChallanAdvanceSearchViewModel.AdvAreaCode = $('.divboxASearch #AdvAreaCode').val();
        DeliveryChallanAdvanceSearchViewModel.AdvCustomerID = $('.divboxASearch #AdvCustomerID').val();
        DeliveryChallanAdvanceSearchViewModel.AdvPlantCode = $('.divboxASearch #AdvPlantCode').val();
        DeliveryChallanAdvanceSearchViewModel.AdvBranchCode = $('.divboxASearch #AdvBranchCode').val();       
        DeliveryChallanAdvanceSearchViewModel.AdvDocumentOwnerID = $('.divboxASearch #AdvDocumentOwnerID').val();
        DeliveryChallanAdvanceSearchViewModel.AdvApprovalStatusCode = $('.divboxASearch #AdvApprovalStatusCode').val();
        DeliveryChallanAdvanceSearchViewModel.AdvEmailSentStatus = $('#AdvEmailSentStatus').val();
        //apply datatable plugin on SaleOrder table
        _dataTable.DeliveryChallanList = $('#tblDeliveryChallan').DataTable(
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
                url: "DeliveryChallan/GetAllDeliveryChallan/",
                data: { "deliveryChallanAdvanceSearchVM": DeliveryChallanAdvanceSearchViewModel },
                type: 'POST'
            },
            pageLength: 8,
            columns: [              
                {
                    "data": "DelvChallanNo", render: function (data, type, row) {
                        return row.DelvChallanNo + "</br>" + "<img src='./Content/images/datePicker.png' height='10px'>" + "&nbsp;" + row.DelvChallanDateFormatted;
                    }, "defaultContent": "<i>-</i>"
                },
                 {
                     "data": "Customer.CompanyName", render: function (data, type, row) {
                         return "<img src='./Content/images/contact.png' height='10px'>" + "&nbsp;" + (row.Customer.ContactPerson == null ? "" : row.Customer.ContactPerson) + "</br>" + "<img src='./Content/images/organisation.png' height='10px'>" + "&nbsp;" + data;
                     }, "defaultContent": "<i>-</i>"
                 },              
                 {
                     "data": "ReferenceNo", render: function (data, type, row) {
                         debugger;
                         return (row.SaleOrder.SaleOrderNo == null ? row.ProductionOrder.ProdOrderNo : row.SaleOrder.SaleOrderNo);
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
                   "data": "ApprovalStatus.Description", render: function (data, type, row) {
                       debugger;
                       return "<b>Appr.Status-</b>" + data + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + (row.EmailSentYN == true ? "<img src='./Content/images/mailSend.png' height='20px' >" : '');
                   }, "defaultContent": "<i>-</i>"
               },


               { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink"  onclick="EditDeliveryChallan(this)" ><i class="fa fa-pencil-square-o" aria-hidden="true"></i></a>' },

            ],
            columnDefs: [
                          { className: "text-left", "targets": [0, 1, 3, 4, 5,6] },
                          { className: "text-center", "targets": [7] },
                          { "targets": [0], "width": "12%" },
                          { "targets": [1], "width": "12%" },
                          { "targets": [2], "width": "16%" },
                          { "targets": [3], "width": "9%" },
                          { "targets": [4], "width": "11%" },
                          { "targets": [5], "width": "15%" },
                          { "targets": [6], "width": "24%" },
                          { "targets": [7], "width": "2%" },

            ],
            destroy: true,
            //for performing the import operation after the data loaded
            initComplete: function (settings, json) {
                debugger;
                $('.dataTables_wrapper div.bottom div').addClass('col-md-6');
                $('#tblDeliveryChallan').fadeIn('slow');
                if (action == undefined) {
                   // $('.excelExport').hide();
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
function ResetDeliveryChallanList() {
    $(".searchicon").removeClass('filterApplied');
    BindOrReloadDeliveryChallanTable('Reset');
}
//function export data to excel
function ExportDeliveryChallanData() {    
    BindOrReloadDeliveryChallanTable('Export');
}

// add DeliveryChallan section
function AddDeliveryChallan() {
    debugger;
    //this will return form body(html)
    OnServerCallBegin();
    $("#divDeliveryChallanForm").load("DeliveryChallan/DeliveryChallanForm?id=" + _emptyGuid + "&delvChallanID=", function (responseTxt, statusTxt, xhr) {
        
        if (statusTxt == "success")
        {
            OnServerCallComplete();
            //resides in customjs for sliding       
            openNav();
            $('#lblDeliveryChallanInfo').text('<<Challan No.>>');
            ChangeButtonPatchView("DeliveryChallan", "btnPatchDeliveryChallanNew", "Add");
            BindDeliveryChallanDetailList(_emptyGuid, false, false);
            
        }
        else {
            console.log("Error: " + xhr.status + ": " + xhr.statusText);
        }
    });
}

function EditDeliveryChallan(this_Obj) {
    debugger;
    OnServerCallBegin();
    var DeliveryChallan = _dataTable.DeliveryChallanList.row($(this_Obj).parents('tr')).data();
    $('#lblDeliveryChallanInfo').text(DeliveryChallan.DelvChallanNo);
    //this will return form body(html)
    $("#divDeliveryChallanForm").load("DeliveryChallan/DeliveryChallanForm?id=" + DeliveryChallan.ID + "&saleOrderID=" + DeliveryChallan.SaleOrderID + "&prodOrderID=" + DeliveryChallan.ProdOrderID, function (responseTxt, statusTxt, xhr) {
        if (statusTxt == "success") {
            OnServerCallComplete();
            //resides in customjs for sliding

            //$("#divDeliveryChallanForm #SaleOrderID").prop('disabled', true);
            //$("#divDeliveryChallanForm #ProdOrderID").prop('disabled', true);
            openNav();
           // $('#spanSaleOrderID').text(DeliveryChallan.SaleOrder.SaleOrderNo);
            if ($('#IsDocLocked').val() == "True") {
                ChangeButtonPatchView("DeliveryChallan", "btnPatchDeliveryChallanNew", "Edit", DeliveryChallan.ID);
            }
            else {
                ChangeButtonPatchView("DeliveryChallan", "btnPatchDeliveryChallanNew", "LockDocument", DeliveryChallan.ID);
            }
            _SlNo = 1;
            BindDeliveryChallanDetailList(DeliveryChallan.ID, false, false);
            $('#divCustomerBasicInfo').load("Customer/CustomerBasicInfo?ID=" + $('#hdnCustomerID').val());
            clearUploadControl();
            PaintImages(DeliveryChallan.ID);
           
        }
        else {
            console.log("Error: " + xhr.status + ": " + xhr.statusText);
        }
       
    });
}

function ResetDeliveryChallan() {
    //this will return form body(html)
    $("#divDeliveryChallanForm").load("DeliveryChallan/DeliveryChallanForm?id=" + $('#DeliveryChallanForm #ID').val(), function (responseTxt, statusTxt, xhr) {
        if (statusTxt == "success") {
            if ($('#ID').val() != _emptyGuid && $('#ID').val() != null) {
                $('#divDeliveryChallanForm #DelvChallanNo').prop('disabled', true);
                //resides in customjs for sliding
                openNav();
            }
            _SlNo = 1;
            BindDeliveryChallanDetailList($('#ID').val(), false, false);
            $('#lblDeliveryChallanInfo').text('<<Challan No.>>');
            clearUploadControl();
            PaintImages($('#DeliveryChallanForm #ID').val());
            $('#divCustomerBasicInfo').load("Customer/CustomerBasicInfo?ID=" + $('#DeliveryChallanForm #hdnCustomerID').val(), function () {
            });
        }
        else {
            console.log("Error: " + xhr.status + ": " + xhr.statusText);
        }
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
                    ChangeButtonPatchView("DeliveryChallan", "btnPatchDeliveryChallanNew", "Edit", _result.ID);
                    _SlNo = 1;
                    BindDeliveryChallanDetailList(_result.ID,false,false);
                    clearUploadControl();
                    PaintImages(_result.ID);
                    $('#lblDeliveryChallanInfo').text(_result.DeliveryChallanNo);

                });
                ChangeButtonPatchView("DeliveryChallan", "btnPatchDeliveryChallanNew", "Edit", _result.ID);
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

function BindDeliveryChallanDetailList(id,IsProdOrder,IsSaleOrder) {
    debugger;
    var data;
    if (id == _emptyGuid && !(IsProdOrder) && !(IsSaleOrder))
    {
        data = null;
    }
    else if (id == _emptyGuid && !(IsProdOrder))
    {
        data = GetDeliveryChallanDetailListByDeliveryChallanID(id, IsProdOrder, IsSaleOrder)
    }
    else if (id == _emptyGuid && !(IsSaleOrder)) {
        data = GetDeliveryChallanDetailListByDeliveryChallanID(id, IsProdOrder, IsSaleOrder)
    }
    else {
        data = GetDeliveryChallanDetailListByDeliveryChallanID(id, IsProdOrder, IsSaleOrder)
    }
    _dataTable.DeliveryChallanDetailList = $('#tblDeliveryChallanDetails').DataTable(
         {
             dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
             order: [],
             searching: false,
             paging: false,
             ordering: false,
             bInfo: false,
             data:data,
                 //id == _emptyGuid ? null : GetDeliveryChallanDetailListByDeliveryChallanID(id),
                 //!IsSaleOrder ? id == _emptyGuid ? null : GetDeliveryChallanDetailListByDeliveryChallanID(id, false) : GetDeliveryChallanDetailListByDeliveryChallanID(id, true),         
                 //(!(IsProdOrder||IsSaleOrder)) ? id == _emptyGuid ? null : GetDeliveryChallanDetailListByDeliveryChallanID(id, IsProdOrder,IsSaleOrder) : GetDeliveryChallanDetailListByDeliveryChallanID(id, IsProdOrder,IsSaleOrder),         

             language: {
                 search: "_INPUT_",
                 searchPlaceholder: "Search"
             },
             columns: [
                  {
                      "data": "", render: function (data, type, row) {
                          return _SlNo++
                      }, "defaultContent": "<i></i>", "width": "2%"
                  },
             {
                 "data": "Product.Code", render: function (data, type, row) {
                     return row.Product.Name + "<br/>" + '<div style="width:100%" class="show-popover" data-placement="top" data-html="true" data-toggle="popover" data-title="<p align=left>Product Specification" data-content="' + (row.ProductSpec !== null ? row.ProductSpec.replace("\n", "<br>").replace(/"/g, "&quot") : "") + '</p>"/>' + row.ProductModel.Name
                 }, "defaultContent": "<i></i>"
             },
             { "data": "Product.HSNCode", "defaultContent": "<i></i>" },
             {
                 "data": "OrderQty", render: function (data, type, row) {
                     return data + " " + row.Unit.Description
                 }, "defaultContent": "<i></i>"
             },
             //{ "data": "Unit.Description", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
            {
                "data": "PrevDelQty", render: function (data, type, row) {
                    if (row.PrevDelQty > 0)
                    {
                        return data + " " + row.Unit.Description
                    }
                    else {
                        return 0 + " " + row.Unit.Description
                    }
                }, "defaultContent": "<i></i>"
            },
             {
                 "data": "DelvQty", render: function (data, type, row) {
                     //if (row.DelvQty > 0)
                     //{
                     //return row.OrderQty-row.PrevDelQty
                         return data + " " + row.Unit.Description
                     //}
                     //else
                     //   return 0 + " " + row.Unit.Description
                     //return  data + " " + row.Unit.Description
                 }, "defaultContent": "<i></i>"
             },
            { "data": null, "orderable": false, "defaultContent": ($('#IsDocLocked').val() == "True" || $('#IsUpdate').val() == "False") ? '<a href="#" class="actionLink"  onclick="EditDeliveryChallanDetail(this)" ><i class="fa fa-pencil-square-o" aria-hidden="true"></i></a> <a href="#" class="DeleteLink"  onclick="ConfirmDeleteDeliveryChallanDetail(this)" ><i class="fa fa-trash-o" aria-hidden="true"></i></a>' : '-' },
             ],
             columnDefs: [
                  { className: "text-left", "targets": [1,2] },
                 { className: "text-right", "targets": [3, 4,5] },
                 { className: "text-center", "targets": [6,0] },
                 { "targets": [1], "width": "30%" },
                 { "targets": [2,3,4,5], "width": "10%" },                 
                 { "targets": [6], "width": "10%" },                                
             ],
             destroy: true
         });
    $('[data-toggle="popover"]').popover({
        html: true,
        'trigger': 'hover',

    });
}

function GetDeliveryChallanDetailListByDeliveryChallanID(id, IsProdOrder, IsSaleOrder) {
    try {
        debugger;
        if ((IsSaleOrder == undefined || IsProdOrder == undefined) && id != _emptyGuid)
            _SlNo = 1;
        else if ((IsSaleOrder == undefined || IsProdOrder == undefined) && id == _emptyGuid)
            _SlNo = 0;
        else
            _SlNo = 1;
        var deliveryChallanDetailList = [];
        if (IsProdOrder) {
            debugger;
            var data = { "prodOrderID": $('#DeliveryChallanForm #hdnProdOrderID').val() };
            _jsonData = GetDataFromServer("DeliveryChallan/GetDeliveryChallanDetailListByDeliveryChallanIDWithProductionOrder/", data);
        }
        else if(IsSaleOrder)
        {
            debugger;
            var data={"saleOrderID":$('#DeliveryChallanForm #hdnSaleOrderID').val()};
            _jsonData = GetDataFromServer("DeliveryChallan/GetDeliveryChallanDetailListByDeliveryChallanIDWithSaleOrder/", data);
        }
        else {
            var data = { "deliveryChallanID": id };
            _jsonData = GetDataFromServer("DeliveryChallan/GetDeliveryChallanDetailListByDeliveryChallanID/", data);
        }

        if (_jsonData != '') {
            _jsonData = JSON.parse(_jsonData);
            _message = _jsonData.Message;
            _status = _jsonData.Status;
            deliveryChallanDetailList = _jsonData.Records;
        }
        if (_status == "OK") {
            return deliveryChallanDetailList;
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
        $('#lblModelPopDeliveryChallan').text('CancellationChallan Detail')
        $('#divModelPopDeliveryChallan').modal('show');
    });
}

function AddDeliveryChallanDetailToList() {
    debugger;
    //$("#FormProductionOrderDetail").submit(function () { });
    if ($('#FormDeliveryChallanDetail #IsUpdate').val() == 'True') {
        _SlNo = 1;
        if (($('#ProductID').val() != "") && ($('#ProductModelID').val() != "") && ($('#ProductSpec').val() != "") && ($('#UnitCode').val() != "")) {
            if (parseInt($('#DelvQty').val()) <= parseInt($('#OrderQty').val())) {
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
            else {
                $('#msgDelvQty').append('Cancelled Qty cannot be grater than Order Qty: ' + $('#OrderQty').val())
            }
    }
    }
    else {
        if (($('#ProductID').val() != "") && ($('#ProductModelID').val() != "") && ($('#ProductSpec').val() != "") && ($('#UnitCode').val() != "")) {
            if (parseInt($('#DelvQty').val()) <= parseInt($('#OrderQty').val())) {
                debugger;
                if (_dataTable.DeliveryChallanDetailList.rows().data().length === 0) {
                    _dataTable.DeliveryChallanDetailList.clear().rows.add(GetDeliveryChallanDetailListByDeliveryChallanID(_emptyGuid)).draw(false);
                    _SlNo = 1;
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
                        var productSpec = $('#ProductSpec').val();
                        productSpec = productSpec.replace(/\n/g, ' ');
                        for (var i = 0; i < deliveryChallanDetailList.length; i++) {
                            if ((deliveryChallanDetailList[i].ProductID == $('#ProductID').val()) && (deliveryChallanDetailList[i].ProductModelID == $('#ProductModelID').val()
                                && (deliveryChallanDetailList[i].ProductSpec.replace(/\n/g, ' ') == productSpec && (deliveryChallanDetailList[i].OrderQty == $('#OrderQty').val())))) {
                                deliveryChallanDetailList[i].DelvQty = parseFloat(deliveryChallanDetailList[i].DelvQty) + parseFloat($('#DelvQty').val());
                                checkpoint = 1;
                                break;
                            }
                        }
                        if (checkpoint == 1) {
                            _SlNo = 1;
                            debugger;
                            _dataTable.DeliveryChallanDetailList.clear().rows.add(deliveryChallanDetailList).draw(false);
                            $('#divModelPopDeliveryChallan').modal('hide');
                        }
                        else if (checkpoint == 0) {
                            _SlNo = _dataTable.DeliveryChallanDetailList.rows().data().length + 1;
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
            else {
                $('#msgDelvQty').append('Cancelled Qty cannot be grater than Order Qty: ' + $('#OrderQty').val())
            }
        }
    }
    $('[data-toggle="popover"]').popover({
        html: true,
        'trigger': 'hover',

    });

}

function EditDeliveryChallanDetail(this_Obj) {
    debugger;
    _datatablerowindex = _dataTable.DeliveryChallanDetailList.row($(this_Obj).parents('tr')).index();
    var deliveryChallanDetail = _dataTable.DeliveryChallanDetailList.row($(this_Obj).parents('tr')).data();
    $("#divModelDeliveryChallanPopBody").load("DeliveryChallan/AddDeliveryChallanDetail", function (responseTxt, statusTxt, xhr) {
        if (statusTxt == 'success') {
            $('#lblModelPopDeliveryChallan').text('CancellationChallan Detail')
            $('#FormDeliveryChallanDetail #IsUpdate').val('True');
            $('#FormDeliveryChallanDetail #ID').val(deliveryChallanDetail.ID);
            $("#FormDeliveryChallanDetail #ProductID").val(deliveryChallanDetail.ProductID)
            $("#FormDeliveryChallanDetail #hdnProductID").val(deliveryChallanDetail.ProductID)

            $("#FormDeliveryChallanDetail #ProductModelID").val(deliveryChallanDetail.ProductModelID);


            $('#divProductBasicInfo').load("Product/ProductBasicInfo?ID=" + $('#hdnProductID').val(), function (responseTxt, statusTxt, xhr) {
                if (statusTxt == 'success') {
                    debugger;
                    $("#FormDeliveryChallanDetail #hdnProductModelID").val(deliveryChallanDetail.ProductModelID);
                    if ($('#hdnProductModelID').val() != _emptyGuid) {

                        $('#divProductBasicInfo').load("ProductModel/ProductModelBasicInfo?ID=" + $('#hdnProductModelID').val() + "&rate=1", function () {
                        });
                    }
                }
                else {
                    console.log("Error: " + xhr.status + ": " + xhr.statusText);
                }
            });



            if ($('#hdnProductID').val() != _emptyGuid) {
                $('.divProductModelSelectList').load("ProductModel/ProductModelSelectList?required=required&productID=" + $('#hdnProductID').val())
            }
            else {
                $('.divProductModelSelectList').empty();
                $('.divProductModelSelectList').append('<span class="form-control newinput"><i id="dropLoad" class="fa fa-spinner"></i></span>');
            }


            $('#FormDeliveryChallanDetail #ProductSpec').val(deliveryChallanDetail.ProductSpec);
            $('#FormDeliveryChallanDetail #OrderQty').val(deliveryChallanDetail.OrderQty);
            $('#FormDeliveryChallanDetail #DelvQty').val(deliveryChallanDetail.DelvQty);
            $('#FormDeliveryChallanDetail #UnitCode').val(deliveryChallanDetail.UnitCode);
            $('#FormDeliveryChallanDetail #hdnUnitCode').val(deliveryChallanDetail.UnitCode);
            $('#divModelPopDeliveryChallan').modal('show');
        }
        else {
            console.log("Error: " + xhr.status + ": " + xhr.statusText);
        }
    });
}

function ConfirmDeleteDeliveryChallanDetail(this_Obj) {
    debugger;
    _SlNo = 1;
    _datatablerowindex = _dataTable.DeliveryChallanDetailList.row($(this_Obj).parents('tr')).index();
    var deliveryChallanDetail = _dataTable.DeliveryChallanDetailList.row($(this_Obj).parents('tr')).data();
    if (deliveryChallanDetail.ID === _emptyGuid) {
        notyConfirm('Are you sure to delete?', 'DeleteCurrentDeliveryChallanDetail("' + _datatablerowindex + '")');
    }
    else {
        notyConfirm('Are you sure to delete?', 'DeleteDeliveryChallanDetail("' + deliveryChallanDetail.ID + '")');

    }
}
function DeleteCurrentDeliveryChallanDetail(_datatablerowindex) {
    var deliveryChallanDetailList = _dataTable.DeliveryChallanDetailList.rows().data();
    deliveryChallanDetailList.splice(_datatablerowindex, 1);
    _dataTable.DeliveryChallanDetailList.clear().rows.add(deliveryChallanDetailList).draw(false);
    notyAlert('success', 'Detail Row deleted successfully');
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
function EditRedirectToDocument(id) {

    OnServerCallBegin();

    $("#divDeliveryChallanForm").load("DeliveryChallan/DeliveryChallanForm?id=" + id, function (responseTxt, statusTxt, xhr) {
        if (statusTxt == "success") {
            OnServerCallComplete();
            openNav();
            $('#lblDeliveryChallanInfo').text($('#DelvChallanNo').val());
            if ($('#IsDocLocked').val() == "True") {
                ChangeButtonPatchView("DeliveryChallan", "btnPatchDeliveryChallanNew", "Edit", id);
            }
            else {
                ChangeButtonPatchView("DeliveryChallan", "btnPatchDeliveryChallanNew", "LockDocument", id);
            }
            _SlNo = 1;
            BindDeliveryChallanDetailList(id, false, false);
            $('#divCustomerBasicInfo').load("Customer/CustomerBasicInfo?ID=" + $('#hdnCustomerID').val());
            clearUploadControl();
            PaintImages(id);
        }
        else {
            console.log("Error: " + xhr.status + ": " + xhr.statusText);
        }
    });
}