﻿var _dataTable = {};
var _emptyGuid = "00000000-0000-0000-0000-000000000000";
var _datatablerowindex = -1;
var _jsonData = {};
var _message = "";
var _status = "";
var _result = "";
var _isApproval = false;
var _SlNo = 1;
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
        if ($('#RedirectToDocument').val() != "") {
            if ($('#RedirectToDocument').val() === _emptyGuid) {
                AddProductionOrder();
            }
            else {
                EditRedirectToDocument($('#RedirectToDocument').val());
            }
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
        var SerachValue = $('#hdnSearchTerm').val();
        var SearchTerm = $('#SearchTerm').val();
        $('#hdnSearchTerm').val($('#SearchTerm').val())
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
                if ((SearchTerm == SerachValue) && ($('.divboxASearch #AdvFromDate').val() == "") && ($('.divboxASearch #AdvToDate').val() == "") && ($('.divboxASearch #AdvAreaCode').val() == "") && ($('.divboxASearch #AdvCustomerID').val() == "") && ($('.divboxASearch #AdvBranchCode').val() == "") && ($('.divboxASearch #AdvDocumentStatusCode').val() == "") && ($('.divboxASearch #AdvDocumentOwnerID').val() == "") && ($('#AdvEmailSentStatus').val() == "") && ($('#AdvApprovalStatusCode').val() == "")) {
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
            autoWidth: false,
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
                          { className: "text-left", "targets": [0, 1, 2, 3, 4] },
                          { className: "text-center", "targets": [5] },
                            { "targets": [0, 1], "width": "12%" },
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
    $("#divProductionOrderForm").load("ProductionOrder/ProductionOrderForm?id=" + _emptyGuid, function (responseTxt, statusTxt, xhr) {
        if (statusTxt == "success") {
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
	$("#divProductionOrderForm").load("ProductionOrder/ProductionOrderForm?id=" + productionOrder.ID+"&&isDocumentApprover=" + $("#hdnIsDocumentApprover").val(), function (responseTxt, statusTxt, xhr) {
        if (statusTxt == "success") {
            OnServerCallComplete();
            openNav();
           // $('#spanSaleOrderID').text(productionOrder.SaleOrderNo);
            if ($('#IsDocLocked').val() == "True") {
                debugger;
                switch ($('#LatestApprovalStatus').val()) {
                    case "0":
                        _isApproval = false;
                        ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "Draft", productionOrder.ID);
                        break;
                    case "1":
                        
                        //ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "ClosedForApproval", productionOrder.ID);
                        if ($("#hdnIsDocumentApprover").val() == "True") {
                            _isApproval = false;
                            ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "ClosedForApprovalApproverEdit", productionOrder.ID);
                        }
                        else {
                            _isApproval = true;
                            ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "ClosedForApproval", productionOrder.ID);
                        }
                        //if ($('#ApproverLevel').val() > 1) {
                        //    ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "Approved", productionOrder.ID);
                        //}
                        //else {
                        //    ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "Recalled", productionOrder.ID);
                        //}
                        break;
                    case "3":
                        ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "Edit", productionOrder.ID);
                        break;
                    case "4":
                        _isApproval = true;
                        ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "Approved", productionOrder.ID);
                        DisableFields();
                        break;
                    default:
                        //ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "LockDocument", productionOrder.ID);
                        if ($('#LatestApprovalStatus').val() == 9) {

                            if ($("#hdnIsDocumentApprover").val() == "True") {
                                _isApproval = false;
                                ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "DocumentApproverEdit", productionOrder.ID);
                            }
                            else {
                                _isApproval = true;
                                ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "LockDocument", productionOrder.ID);
                            }
                        }
                        else {
                            _isApproval = true;
                            ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "LockDocument", productionOrder.ID);
                        }
						break;

                }
            }
            else {
                //$('.switch-input').prop('disabled', true);
                //$('.switch-label,.switch-handle').addClass('switch-disabled').addClass('disabled');
                //$('.switch-label').attr('title', 'Document Locked');
                debugger;
                //ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "LockDocument", productionOrder.ID);
                switch ($('#LatestApprovalStatus').val()) {
					case "4":
						_isApproval = true;
						if ($('#IsDistributor').val() == "True" && $('#IsMilestoneUpdate').val() == "False")
							ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "DistributorApproveDocument", productionOrder.ID);
						else if ($('#IsMilestoneUpdate').val() == "True" && $('#IsDistributor').val() == "False") {
							ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "MilestoneApproveDocument", productionOrder.ID);
							DisableFields();
					
						}
						else if ($('#IsDistributor').val() == "True" && $('#IsMilestoneUpdate').val() == "True") {
							ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "DistributerMileStoneApprove", productionOrder.ID);
							DisableFields();
						}
						else
							ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "LockDocument", productionOrder.ID);
						break;
					case "1":
					    if ($("#hdnIsDocumentApprover").val() == "True") {
					        _isApproval = false
					        ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "DocumentApproverEdit", productionOrder.ID);
					    }
					    else {
					        _isApproval = true;
					        ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "LockDocument", productionOrder.ID);
					    }
						break;
                        //else
                        //    ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "LockDocument", productionOrder.ID);
                        
					default:	
                        //ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "LockDocument", productionOrder.ID);
					    if ($('#LatestApprovalStatus').val() == 9) {
					        if ($("#hdnIsDocumentApprover").val() == "True") {
					            _isApproval = false
					            ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "DocumentApproverEdit", productionOrder.ID);
					        }
					        else {
					            _isApproval = true;
					            ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "LockDocument", productionOrder.ID);
					        }
					    }
					    else {
					        _isApproval = true;
					        ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "LockDocument", productionOrder.ID);
					    }
						break;
                }
            }
            _SlNo = 1;
            BindProductionOrderDetailList(productionOrder.ID);
            $('#divCustomerBasicInfo').load("Customer/CustomerBasicInfo?ID=" + $('#hdnCustomerID').val());
            clearUploadControl();
            PaintImages(productionOrder.ID, _isApproval);
            $("#divProductionOrderForm #SaleOrderID").prop('disabled', true);
            //if (productionOrder.DocumentStatus.Description == "OPEN") {
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

function ResetProductionOrder() {
    debugger;
    //this will return form body(html)
	$("#divProductionOrderForm").load("ProductionOrder/ProductionOrderForm?id=" + $('#ProductionOrderForm #ID').val() + "&&isDocumentApprover=" + $("#hdnIsDocumentApprover").val(), function (responseTxt, statusTxt, xhr) {
        if (statusTxt == "success") {
            //if ($('#hdnDescription').val() == "OPEN") {
            //    $('.switch-input').prop('checked', true);
               
            //} else {
            //    $('.switch-input').prop('checked', false);
               
            //}
            if ($('#ID').val() != _emptyGuid && $('#ID').val() != null) {
                //resides in customjs for sliding
                $("#divProductionOrderForm #SaleOrderID").prop('disabled', true);
                openNav();

            }
            else {
                $('#hdnCustomerID').val('');
                $('#lblProductionOrderInfo').text('<<Production Order No.>>');
            }
			if ($('#IsDocLocked').val() == "True") {
				debugger;
				switch ($('#LatestApprovalStatus').val()) {
				    case "":
				        _isApproval = false;
						ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "Add");
						break;
				    case "0":
				        _isApproval = false;
						ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "Draft", $('#ID').val());
						break;
					case "1":
						debugger;
						
						//ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "ClosedForApproval", $('#ID').val());
						if ($("#hdnIsDocumentApprover").val() == "True") {
						    ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "ClosedForApprovalApproverEdit", $('#ID').val());
						    _isApproval = false;
						}
						else {
						    _isApproval = true;
						    ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "ClosedForApproval", $('#ID').val());
						}
						    //if ($('#ApproverLevel').val() > 1) {
						//    ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "Approved", $('#ID').val());
						//}
						//else {
						//    ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "Recalled", $('#ID').val());
						//}
						break;
				    case "3":
				        _isApproval = false;
						ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "Edit", $('#ID').val());
						break;
					case "4":
						_isApproval = true;
						ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "Approved", $('#ID').val());
						break;
					default:
						//ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "LockDocument", $('#ID').val());
					    if ($('#LatestApprovalStatus').val() == 9) {
					        if ($("#hdnIsDocumentApprover").val() == "True") {
					            ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "DocumentApproverEdit", $('#ID').val());
					            _isApproval = false;
					        }
					        else {
					            ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "LockDocument", $('#ID').val());
					            _isApproval = true;
					        }
					    }
					    else {
					        ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "LockDocument", $('#ID').val());
					        _isApproval = true;
					    }
					    break;
				}
			}
			else {
			    switch ($('#LatestApprovalStatus').val()) {
			        case "":
			            _isApproval = false;
			            ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "Add", $('#ID').val());
			            break;
					case "1":
					    if ($("#hdnIsDocumentApprover").val() == "True") {
					        ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "DocumentApproverEdit", $('#ID').val());
					        _isApproval = false;
					    }
					    else {
					        ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "LockDocument", $('#ID').val());
					        _isApproval = true;
					    }
					    break;
					default:
					    if ($('#LatestApprovalStatus').val() == 9) {
					        if ($("#hdnIsDocumentApprover").val() == "True") {
					            _isApproval = false;
					            ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "DocumentApproverEdit", $('#ID').val());
					        }
					        else {
					            _isApproval = true;
					            ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "LockDocument", $('#ID').val());
					        }
					    }
					    else {
					        _isApproval = true;
					        ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "LockDocument", $('#ID').val());
					    }
						break;
				}
			}
			_SlNo = 1;
            BindProductionOrderDetailList($('#ID').val(), false);
            clearUploadControl();
            PaintImages($('#ProductionOrderForm #ID').val(), _isApproval);
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
                if (_message == "Insertion successfull" && $('#IsUpdate').val()=="False")
                    $('#IsDocLocked').val("True");
                $('#IsUpdate').val('True');
				$("#divProductionOrderForm").load("ProductionOrder/ProductionOrderForm?id=" + _result.ID + "&saleOrderID=" + _result.SaleOrderID + "&&isDocumentApprover=" + $("#hdnIsDocumentApprover").val(), function () {
				    //ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "Edit", _result.ID);
				    _SlNo = 1;
                    BindProductionOrderDetailList(_result.ID);
                    clearUploadControl();
                    PaintImages(_result.ID, _isApproval);
                    $('#lblProductionOrderInfo').text(_result.ProductionOrderNo);
                    //if ($('#hdnDescription').val() == "OPEN") {
                    //    $('.switch-input').prop('checked', true);

                    //} else {
                    //    $('.switch-input').prop('checked', false);

                    //}

                });
                debugger;
				if ($('#IsDocLocked').val() == "True")
				{
					if ($('#LatestApprovalStatus').val() == "4")
					{
					    _isApproval = true;
							ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "Approved", _result.ID);
							DisableFields();
					
				    }
					else if ($('#LatestApprovalStatus').val() == "1" && $("#hdnIsDocumentApprover").val() == "True" || $('#LatestApprovalStatus').val() == "9" && $("#hdnIsDocumentApprover").val() == "True") {
					    _isApproval = false;
					    ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "ClosedForApprovalApproverEdit", _result.ID);

					}

					else {
					    _isApproval = false;
					    if ($('#LatestApprovalStatus').val() == "0" || $('#LatestApprovalStatus').val() == "")
					        ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "Draft", _result.ID);
                        else
					        ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "Edit", _result.ID);
					}
                }
				else
				{
     //               if ($('#LatestApprovalStatus').val() == "4" && $('#IsDistributor').val() == "True" && $('#IsMilestoneUpdate').val() == "False") {
                        
     //                   ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "DistributorApproveDocument", _result.ID);
                        
     //               }
					//else if ($('#IsMilestoneUpdate').val() == "True" && $('#IsDistributor').val() == "False" && $('#LatestApprovalStatus').val() == "4") {
     //                   ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "MilestoneApproveDocument", _result.ID);
     //                   DisableFields();
     //               }
					//else if ($('#IsDistributor').val() == "True" && $('#IsMilestoneUpdate').val() == "True" && $('#LatestApprovalStatus').val() == "4") {
     //                   ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "DistributerMileStoneApprove", _result.ID);
     //                   DisableFields();
     //               }
     //               else
     //                   ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "LockDocument", _result.ID);
					switch ($('#LatestApprovalStatus').val()) {
					    case "4":
					        _isApproval = true;
							if ($('#IsDistributor').val() == "True" && $('#IsMilestoneUpdate').val() == "False") {
								ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "DistributorApproveDocument", _result.ID);
							}
							else if ($('#IsMilestoneUpdate').val() == "True" && $('#IsDistributor').val() == "False") {
								ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "MilestoneApproveDocument", _result.ID);
								DisableFields();
							}
							else if ($('#IsDistributor').val() == "True" && $('#IsMilestoneUpdate').val() == "True") {
								ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "DistributerMileStoneApprove", _result.ID);
								DisableFields();
							}
							break;
						default:
						    debugger;
						    _isApproval = false;
							if (($('#LatestApprovalStatus').val() == "1" && $("#hdnIsDocumentApprover").val() == "True" && $('#IsDocLocked').val() == "False") || ($('#LatestApprovalStatus').val() == "9" && $("#hdnIsDocumentApprover").val() == "True"))
								ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "DocumentApproverEdit", _result.ID);
							else if ($('#LatestApprovalStatus').val() == "1" && $("#hdnIsDocumentApprover").val() == "True" && $('#IsDocLocked').val() == "True")
								ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "ClosedForApprovalApproverEdit", _result.ID);
							else
								ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "Edit", _result.ID);
					}
                }
                
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

function BindProductionOrderDetailList(id, IsSaleOrder) {
    debugger;
    if ($('#hdnShowRate').val() == "True") {
        _dataTable.ProductionOrderDetailList = $('#tblProductionOrderDetails').DataTable(
             {
                 dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
                 order: [],
                 searching: false,
                 paging: false,
                 ordering: false,
                 bInfo: false,
                 data: //id == _emptyGuid ? null : GetProductionOrderDetailListByProductionOrderID(id),
                        !IsSaleOrder ? id == _emptyGuid ? null : GetProductionOrderDetailListByProductionOrderID(id, false) : GetProductionOrderDetailListByProductionOrderID(id, true),

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
                         debugger;
                         return row.Product.Name + "<br/>" + '<div style="width:100%" class="show-popover" data-placement="top" data-html="true" data-toggle="popover" data-title="<p align=left>Product Specification" data-content="' + (row.ProductSpec !== null ? row.ProductSpec.replace("\n", "<br>").replace(/"/g, "&quot") : "") + '</p>"/>' + row.ProductModel.Name
                     }, "defaultContent": "<i></i>"
                 },
                 { "data": "Product.HSNCode", "defaultContent": "<i></i>" },
                 {
                     "data": "SaleOrderQty", render: function (data, type, row) {
                         return data + " " + row.Unit.Description
                     }, "defaultContent": "<i></i>"
                 },
                 {
                     "data": "PrevProdOrderQty", render: function (data, type, row) {
                         if (row.PrevProdOrderQty != null) {
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
                             return formatCurrency(data)
                         }
                         else
                             return 0
                     }, "defaultContent": "<i></i>"
                 },
                 {
                     "data": "Amount", render: function (data, type, row) {
                         if (row.Rate != null) {
                             debugger;
                             //if (row.SaleOrderQty != 0) {
                             //    var Amount = roundoff(parseFloat(row.SaleOrderQty) * parseFloat(row.Rate));
                             //    return Amount
                             //}
                             if (row.ProducedQty != 0) {
                                 var Amount = roundoff(parseFloat(row.ProducedQty) * parseFloat(row.Rate));
                                 return formatCurrency(Amount)
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
                         if ((((row.MileStone1FcFinishDtFormatted != null) && (row.MileStone1FcFinishDtFormatted != "")) && ((row.MileStone1AcTFinishDtFormatted != null) && (row.MileStone1AcTFinishDtFormatted != "")))) {
                             var M1 = '25%'
                             result = '<div class="show-popover text-right" data-html="true" data-toggle="popover" data-placement="top" data-title="<p align=left> Milestone Reach : ' + M1 + '" data-content="1) ⌚ Forecast :  ' + row.MileStone1FcFinishDtFormatted + ' ⌚ Actual :  ' + row.MileStone1AcTFinishDtFormatted + '</p>"/>' + M1
                         }
                         if (((row.MileStone2FcFinishDtFormatted != null && (row.MileStone2FcFinishDtFormatted != "")) && (row.MileStone2AcTFinishDtFormatted != null && (row.MileStone2AcTFinishDtFormatted != "")))) {
                             var M2 = '50%'
                             result = '<div class="show-popover text-right" data-html="true" data-toggle="popover" data-placement="top" data-title="<p align=left>Milestone Reach : ' + M2 + '" data-content="2) ⌚ Forecast :  ' + row.MileStone2FcFinishDtFormatted + '⌚ Actual :  ' + row.MileStone2AcTFinishDtFormatted + '<br/>1) ⌚ Forecast :  ' + row.MileStone1FcFinishDtFormatted + ' ⌚ Actual :  ' + row.MileStone1AcTFinishDtFormatted + '</p>"/>' + M2
                         }
                         if (((row.MileStone3FcFinishDtFormatted != null && (row.MileStone3FcFinishDtFormatted != "")) && (row.MileStone3AcTFinishDtFormatted != null && (row.MileStone3AcTFinishDtFormatted != "")))) {
                             var M3 = '75%'
                             result = '<div class="show-popover text-right" data-html="true" data-toggle="popover" data-placement="top" data-title="<p align=left>Milestone Reach : ' + M3 + '" data-content="3) ⌚ Forecast :  ' + row.MileStone3FcFinishDtFormatted + '⌚ Actual :  ' + row.MileStone3AcTFinishDtFormatted + '<br/>2) ⌚ Forecast :  ' + row.MileStone2FcFinishDtFormatted + '⌚ Actual :  ' + row.MileStone2AcTFinishDtFormatted + '<br/>1) ⌚ Forecast :  ' + row.MileStone1FcFinishDtFormatted + ' ⌚ Actual :  ' + row.MileStone1AcTFinishDtFormatted + '</p>"/>' + M3
                         }
                         if (((row.MileStone4FcFinishDtFormatted != null && (row.MileStone4FcFinishDtFormatted != "")) && (row.MileStone4AcTFinishDtFormatted != null && (row.MileStone4AcTFinishDtFormatted != "")))) {
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
                 { //"data": null, "orderable": false, "defaultContent": ($('#IsDocLocked').val() == "True" || $('#IsUpdate').val() == "False")?'<a href="#" class="actionLink"  onclick="EditProductionOrderDetail(this)" ><i class="fa fa-pencil-square-o" aria-hidden="true"></i></a> <a href="#" class="DeleteLink"  onclick="ConfirmDeleteProductionOrderDetail(this)" ><i class="fa fa-trash-o" aria-hidden="true"></i></a>':'-' },
                     "data": "OrderQty", "orderable": false, render: function (data, type, row) {
                         debugger;
                         //if (($('#IsDocLocked').val() == "False" || $('#IsUpdate').val() == "False" || $('#LatestApprovalStatus').val() == "1" || $('#LatestApprovalStatus').val() == "9" || $('#LatestApprovalStatus').val() == "4") && $('#LatestApprovalStatus').val() != "") {
						 if (($('#LatestApprovalStatus').val() == "1" && $("#hdnIsDocumentApprover").val() == "True") || ($('#LatestApprovalStatus').val() == "9" && $("#hdnIsDocumentApprover").val() == "True")) {
							 return '<a href="#" class="actionLink"  onclick="EditProductionOrderDetail(this)" ><i class="fa fa-pencil-square-o" aria-hidden="true"></i></a> <a href="#" class="DeleteLink"  onclick="ConfirmDeleteProductionOrderDetail(this)" ><i class="fa fa-trash-o" aria-hidden="true"></i></a>'
						 }
						
						 
                         else if ((($('#IsDocLocked').val() == "False" && $('#IsMilestoneUpdate').val() == "False") || $('#IsUpdate').val() == "False" || $('#LatestApprovalStatus').val() == "1" || $('#LatestApprovalStatus').val() == "9") && $('#LatestApprovalStatus').val() != "") {
                             return "-"
                         }
                         else {
                             if ($('#LatestApprovalStatus').val() == "4")
                                 return '<a href="#" class="actionLink"  onclick="EditProductionOrderDetail(this)" ><i class="fa fa-pencil-square-o" aria-hidden="true"></i></a>'
                             else
                                 return '<a href="#" class="actionLink"  onclick="EditProductionOrderDetail(this)" ><i class="fa fa-pencil-square-o" aria-hidden="true"></i></a> <a href="#" class="DeleteLink"  onclick="ConfirmDeleteProductionOrderDetail(this)" ><i class="fa fa-trash-o" aria-hidden="true"></i></a>'
                         }
                     }, "defaultContent": "<i></i>"
                 },
                 ],
                 columnDefs: [
                     { className: "text-right", "targets": [3, 4, 5, 6, 7, 8] },
                     { className: "text-left", "targets": [1, 9, 2] },
                     { className: "text-center", "targets": [10, 11,0] },
                     { "targets": [1], "width": "20%" },
                     { "targets": [2, 3, 4, 5, 6, 7, 8, 9, 10, 11], "width": "8%" },
                 ]
             });
    }
    else {
        _dataTable.ProductionOrderDetailList = $('#tblProductionOrderDetails').DataTable(
            {
                dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
                order: [],
                searching: false,
                paging: false,
                ordering: false,
                bInfo: false,
                data: //id == _emptyGuid ? null : GetProductionOrderDetailListByProductionOrderID(id),
                       !IsSaleOrder ? id == _emptyGuid ? null : GetProductionOrderDetailListByProductionOrderID(id, false) : GetProductionOrderDetailListByProductionOrderID(id, true),

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
                        debugger;
                        return row.Product.Name + "<br/>" + '<div style="width:100%" class="show-popover" data-placement="top" data-html="true" data-toggle="popover" data-title="<p align=left>Product Specification" data-content="' + (row.ProductSpec !== null ? row.ProductSpec.replace("\n", "<br>").replace(/"/g, "&quot") : "") + '</p>"/>' + row.ProductModel.Name
                    }, "defaultContent": "<i></i>"
                },
                { "data": "Product.HSNCode", "defaultContent": "<i></i>" },
                {
                    "data": "SaleOrderQty", render: function (data, type, row) {
                        return data + " " + row.Unit.Description
                    }, "defaultContent": "<i></i>"
                },
                {
                    "data": "PrevProdOrderQty", render: function (data, type, row) {
                        if (row.PrevProdOrderQty != null) {
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
                { "data": "Plant.Description", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
                {
                    "data": "result", render: function (data, type, row) {
                        debugger;
                        var result = "";
                        if ((((row.MileStone1FcFinishDtFormatted != null) && (row.MileStone1FcFinishDtFormatted != "")) && ((row.MileStone1AcTFinishDtFormatted != null) && (row.MileStone1AcTFinishDtFormatted != "")))) {
                            var M1 = '25%'
                            result = '<div class="show-popover text-right" data-html="true" data-toggle="popover" data-placement="top" data-title="<p align=left> Milestone Reach : ' + M1 + '" data-content="1) ⌚ Forecast :  ' + row.MileStone1FcFinishDtFormatted + ' ⌚ Actual :  ' + row.MileStone1AcTFinishDtFormatted + '</p>"/>' + M1
                        }
                        if (((row.MileStone2FcFinishDtFormatted != null && (row.MileStone2FcFinishDtFormatted != "")) && (row.MileStone2AcTFinishDtFormatted != null && (row.MileStone2AcTFinishDtFormatted != "")))) {
                            var M2 = '50%'
                            result = '<div class="show-popover text-right" data-html="true" data-toggle="popover" data-placement="top" data-title="<p align=left>Milestone Reach : ' + M2 + '" data-content="2) ⌚ Forecast :  ' + row.MileStone2FcFinishDtFormatted + '⌚ Actual :  ' + row.MileStone2AcTFinishDtFormatted + '<br/>1) ⌚ Forecast :  ' + row.MileStone1FcFinishDtFormatted + ' ⌚ Actual :  ' + row.MileStone1AcTFinishDtFormatted + '</p>"/>' + M2
                        }
                        if (((row.MileStone3FcFinishDtFormatted != null && (row.MileStone3FcFinishDtFormatted != "")) && (row.MileStone3AcTFinishDtFormatted != null && (row.MileStone3AcTFinishDtFormatted != "")))) {
                            var M3 = '75%'
                            result = '<div class="show-popover text-right" data-html="true" data-toggle="popover" data-placement="top" data-title="<p align=left>Milestone Reach : ' + M3 + '" data-content="3) ⌚ Forecast :  ' + row.MileStone3FcFinishDtFormatted + '⌚ Actual :  ' + row.MileStone3AcTFinishDtFormatted + '<br/>2) ⌚ Forecast :  ' + row.MileStone2FcFinishDtFormatted + '⌚ Actual :  ' + row.MileStone2AcTFinishDtFormatted + '<br/>1) ⌚ Forecast :  ' + row.MileStone1FcFinishDtFormatted + ' ⌚ Actual :  ' + row.MileStone1AcTFinishDtFormatted + '</p>"/>' + M3
                        }
                        if (((row.MileStone4FcFinishDtFormatted != null && (row.MileStone4FcFinishDtFormatted != "")) && (row.MileStone4AcTFinishDtFormatted != null && (row.MileStone4AcTFinishDtFormatted != "")))) {
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
                { //"data": null, "orderable": false, "defaultContent": ($('#IsDocLocked').val() == "True" || $('#IsUpdate').val() == "False")?'<a href="#" class="actionLink"  onclick="EditProductionOrderDetail(this)" ><i class="fa fa-pencil-square-o" aria-hidden="true"></i></a> <a href="#" class="DeleteLink"  onclick="ConfirmDeleteProductionOrderDetail(this)" ><i class="fa fa-trash-o" aria-hidden="true"></i></a>':'-' },
                    "data": "OrderQty", "orderable": false, render: function (data, type, row) {
                        debugger;
						if (($('#LatestApprovalStatus').val() == "1" && $("#hdnIsDocumentApprover").val() == "True") || ($('#LatestApprovalStatus').val() == "9" && $("#hdnIsDocumentApprover").val() == "True")) {
							return '<a href="#" class="actionLink"  onclick="EditProductionOrderDetail(this)" ><i class="fa fa-pencil-square-o" aria-hidden="true"></i></a> <a href="#" class="DeleteLink"  onclick="ConfirmDeleteProductionOrderDetail(this)" ><i class="fa fa-trash-o" aria-hidden="true"></i></a>'
						}
						
                       else if ((($('#IsDocLocked').val() == "False" && $('#IsMilestoneUpdate').val() == "False") || $('#IsUpdate').val() == "False" || $('#LatestApprovalStatus').val() == "1" || $('#LatestApprovalStatus').val() == "9") && $('#LatestApprovalStatus').val() != "") {
                            return "-"
                        }
                        else {
                            if ($('#LatestApprovalStatus').val() == "4")
                                return '<a href="#" class="actionLink"  onclick="EditProductionOrderDetail(this)" ><i class="fa fa-pencil-square-o" aria-hidden="true"></i></a>'
                            else
                                return '<a href="#" class="actionLink"  onclick="EditProductionOrderDetail(this)" ><i class="fa fa-pencil-square-o" aria-hidden="true"></i></a> <a href="#" class="DeleteLink"  onclick="ConfirmDeleteProductionOrderDetail(this)" ><i class="fa fa-trash-o" aria-hidden="true"></i></a>'
                        }
                    }, "defaultContent": "<i></i>"
                },
                ],
                columnDefs: [
                    { className: "text-right", "targets": [3, 4, 5, 6] },
                    { className: "text-left", "targets": [1, 7, 2] },
                    { className: "text-center", "targets": [8, 9,0] },
                    { "targets": [1], "width": "20%" },
                    { "targets": [2, 3, 4, 5, 6, 7, 8, 9], "width": "8%" },
                ]
            });
    }
    $('[data-toggle="popover"]').popover({
        html: true,
        'trigger': 'hover',

    });

}

function GetProductionOrderDetailListByProductionOrderID(id, IsSaleOrder) {
    try {
        debugger;
        if (IsSaleOrder == undefined)
            _SlNo = 0;
        else
            _SlNo = 1;
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
    $("#divModelProductionOrderPopBody").load("ProductionOrder/AddProductionOrderDetail?update=false", function () {    
        $('#lblModelPopProductionOrder').text('ProductionOrder Detail')
        if (($('#FormProductionOrderDetail #SaleOrderQty').val() == "0") || ($('#FormProductionOrderDetail #SaleOrderQty').val() == 0)) {
            $('#FormProductionOrderDetail #OrderQty').attr('disabled', 'disabled');
        }
        $('#divModelPopProductionOrder').modal('show');
    });   
}

function AddProductionOrderDetailToList() {
    debugger;    
    //$('#FormProductionOrderDetail').submit();  // you submit form
    if ($('#FormProductionOrderDetail #IsUpdate').val() == 'True') {
        _SlNo = 1;
            if (($('#ProductSpec').val().trim() != "") && ($('#UnitCode').val() != "") && ($('#orderQtyID span').text() == "") && ($('#producedQtyID span').text() == "") && ((parseFloat($('#SaleOrderQty').val()) == 0) || (parseFloat($('#ProducedQty').val()) <= parseFloat($('#OrderQty').val())))) {
                debugger;
                var productionOrderDetailList = _dataTable.ProductionOrderDetailList.rows().data();
                if (!(($("#IsMilestoneUpdate").val() == "True" && $('#LatestApprovalStatus').val() == '4') || ($('#IsDocLocked').val() == "True") && $('#LatestApprovalStatus').val() == '4')) {
                    //productionOrderDetailList[_datatablerowindex].Product.Code = $("#ProductID").val() != "" ? $("#ProductID option:selected").text().split("-")[0].trim() : "";
                    //productionOrderDetailList[_datatablerowindex].Product.Name = $("#ProductID").val() != "" ? $("#ProductID option:selected").text().split("-")[1].trim() : "";
                    productionOrderDetailList[_datatablerowindex].Product.Code = $("#productName").text() != "" ? $("#productName").text().split("-")[0].trim() : "";
                    productionOrderDetailList[_datatablerowindex].Product.Name = $("#productName").text() != "" ? $("#productName").text().split("-")[1].trim() : "";
                    productionOrderDetailList[_datatablerowindex].Product.HSNCode = $("#hdnProductHSNCode").val();
                    productionOrderDetailList[_datatablerowindex].ProductID = $("#hdnProductID").val() != "" ? $("#hdnProductID").val() : _emptyGuid;
                    productionOrderDetailList[_datatablerowindex].ProductModelID = $("#hdnProductModelID").val() != "" ? $("#hdnProductModelID").val() : _emptyGuid;
                    ProductModel = new Object;
                    Unit = new Object;
                    Plant = new Object;
                    // ProductModel.Name = $("#ProductModelID").val() != "" ? $("#ProductModelID option:selected").text() : "";
                    ProductModel.Name = $('#productModelName').text();
                    productionOrderDetailList[_datatablerowindex].ProductModel = ProductModel;
                    productionOrderDetailList[_datatablerowindex].ProductSpec = $('#ProductSpec').val();
                    productionOrderDetailList[_datatablerowindex].SaleOrderQty = $('#SaleOrderQty').val();
                    productionOrderDetailList[_datatablerowindex].OrderQty = $('#OrderQty').val();

                    productionOrderDetailList[_datatablerowindex].UnitCode = $('#UnitCode').val();
                    Unit.Description = $("#UnitCode").val() != "" ? $("#UnitCode option:selected").text().trim() : "";
                    productionOrderDetailList[_datatablerowindex].Unit = Unit;
                    if (($('#Rate').val() !== undefined && $('#Rate').val() !== null && $('#Rate').val() !== "")) {
                        productionOrderDetailList[_datatablerowindex].Rate = parseFloat($('#Rate').val());
                    }
                    else {
                        productionOrderDetailList[_datatablerowindex].Rate = $('#hdnSaleOrderRate').val();
                    }
                    productionOrderDetailList[_datatablerowindex].PlantCode = $('#PlantCode').val();
                    Plant.Description = $("#PlantCode").val() != "" ? $("#PlantCode option:selected").text().trim() : "";
                    productionOrderDetailList[_datatablerowindex].Plant = Plant;
                }
                productionOrderDetailList[_datatablerowindex].ProducedQty = $('#ProducedQty').val();
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
            if (($('#ProductID').val() != "") && ($('#ProductModelID').val() != "") && ($('#ProductSpec').val().trim() != "") && ($('#UnitCode').val() != "") && ((parseFloat($('#SaleOrderQty').val()) == 0) || (parseFloat($('#ProducedQty').val()) <= parseFloat($('#OrderQty').val())))) {
                debugger;
                if (_dataTable.ProductionOrderDetailList.rows().data().length === 0) {
                    _SlNo = 0;
                    _dataTable.ProductionOrderDetailList.clear().rows.add(GetProductionOrderDetailListByProductionOrderID(_emptyGuid)).draw(false);

                    debugger;
                    var productionOrderDetailList = _dataTable.ProductionOrderDetailList.rows().data();
                    productionOrderDetailList[0].Product.Code = $("#ProductID").val() != "" ? $("#ProductID option:selected").text().split("-")[0].trim() : "";
                    productionOrderDetailList[0].Product.Name = $("#ProductID").val() != "" ? $("#ProductID option:selected").text().split("-")[1].trim() : "";
                    productionOrderDetailList[0].Product.HSNCode = $("#hdnProductHSNCode").val();
                    productionOrderDetailList[0].ProductID = $("#ProductID").val() != "" ? $("#ProductID").val() : _emptyGuid;
                    productionOrderDetailList[0].ProductModelID = $("#ProductModelID").val() != "" ? $("#ProductModelID").val() : _emptyGuid;
                    ProductModel = new Object;
                    Unit = new Object;
                    Plant = new Object;
                    ProductModel.Name = $("#ProductModelID").val() != "" ? $("#ProductModelID option:selected").text() : "";
                    productionOrderDetailList[0].ProductModel = ProductModel;
                    productionOrderDetailList[0].ProductSpec = $('#ProductSpec').val();
                    productionOrderDetailList[0].SaleOrderQty = $('#SaleOrderQty').val();
                    productionOrderDetailList[0].OrderQty = $('#OrderQty').val();
                    productionOrderDetailList[0].ProducedQty = $('#ProducedQty').val();
                    productionOrderDetailList[0].UnitCode = $('#UnitCode').val();
                    Unit.Description = $("#UnitCode").val() != "" ? $("#UnitCode option:selected").text().trim() : "";
                    productionOrderDetailList[0].Unit = Unit;
                    if (($('#Rate').val() !== undefined && $('#Rate').val() !== null && $('#Rate').val() !== "")) {
                        productionOrderDetailList[0].Rate = parseFloat($('#Rate').val());
                    }
                    else {
                        productionOrderDetailList[0].Rate = $('#hdnSellingPrice').val();
                    }
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
                                && (productionOrderDetailList[i].ProductSpec == $('#ProductSpec').val()) && (productionOrderDetailList[i].UnitCode == $('#UnitCode').val()
                                && (productionOrderDetailList[i].Rate == parseFloat($('#divModelProductionOrderPopBody #Rate').val()))))) {
                                productionOrderDetailList[i].ProducedQty = parseFloat(productionOrderDetailList[i].ProducedQty) + parseFloat($('#ProducedQty').val());
                                checkpoint = 1;
                                break;
                            }
                        }
                        if (checkpoint == 1) {
                            _SlNo = 1;
                            debugger;
                            _dataTable.ProductionOrderDetailList.clear().rows.add(productionOrderDetailList).draw(false);
                            $('#divModelPopProductionOrder').modal('hide');
                        }
                        else if (checkpoint == 0) {
                            _SlNo = _dataTable.ProductionOrderDetailList.rows().data().length + 1;
                            var ProductionOrderDetailVM = new Object();
                            var Product = new Object;
                            var ProductModel = new Object()
                            var Unit = new Object();
                            var Plant = new Object();
                            ProductionOrderDetailVM.ID = _emptyGuid;
                            ProductionOrderDetailVM.ProductID = $("#ProductID").val() != "" ? $("#ProductID").val() : _emptyGuid;
                            Product.Code = $("#ProductID").val() != "" ? $("#ProductID option:selected").text().split("-")[0].trim() : "";
                            Product.Name = $("#ProductID").val() != "" ? $("#ProductID option:selected").text().split("-")[1].trim() : "";
                            Product.HSNCode = $("#hdnProductHSNCode").val();
                            ProductionOrderDetailVM.Product = Product;
                            ProductionOrderDetailVM.ProductModelID = $("#ProductModelID").val() != "" ? $("#ProductModelID").val() : _emptyGuid;
                            ProductModel.Name = $("#ProductModelID").val() != "" ? $("#ProductModelID option:selected").text() : "";
                            ProductionOrderDetailVM.ProductModel = ProductModel;
                            ProductionOrderDetailVM.ProductSpec = $('#ProductSpec').val();
                            ProductionOrderDetailVM.SaleOrderQty = $('#SaleOrderQty').val();
                            ProductionOrderDetailVM.OrderQty = $('#OrderQty').val();
                            ProductionOrderDetailVM.ProducedQty = $('#ProducedQty').val();
                            Unit.Description = $("#UnitCode").val() != "" ? $("#UnitCode option:selected").text().trim() : "";
                            ProductionOrderDetailVM.Unit = Unit;
                            ProductionOrderDetailVM.UnitCode = $('#UnitCode').val();
                            if (($('#Rate').val() !== undefined && $('#Rate').val() !== null && $('#Rate').val() !== "")) {
                                ProductionOrderDetailVM.Rate = parseFloat($('#Rate').val());
                            }
                            else {
                                ProductionOrderDetailVM.Rate = $('#hdnSellingPrice').val();
                            }
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
                //}
                //else {
                //    $('#msgQty').append('SaleOrder Qty cannot be less than Production or Curr.Prod Order Qty')
                //}
            }
        }
    
    $('[data-toggle="popover"]').popover({
        html: true,
        'trigger': 'hover',

    });
   // $.session.remove('Prod');
}


function EditProductionOrderDetail(this_Obj) {
    debugger;
    _datatablerowindex = _dataTable.ProductionOrderDetailList.row($(this_Obj).parents('tr')).index();
    var productionOrderDetail = _dataTable.ProductionOrderDetailList.row($(this_Obj).parents('tr')).data();
    var ismilestoneupdate = 0;
    if (($("#IsMilestoneUpdate").val() == "True" && $('#LatestApprovalStatus').val() == '4') || ($('#IsDocLocked').val() == "True" && $('#LatestApprovalStatus').val() == '4'))
        ismilestoneupdate = 1
    $("#divModelProductionOrderPopBody").load("ProductionOrder/AddProductionOrderDetail?update=true&&IsMilestoneUpdate=" + ismilestoneupdate, function (responseTxt, statusTxt, xhr) {
        $('#divModelPopProductionOrder').modal('show');
        if (statusTxt == 'success') {
            $('#lblModelPopProductionOrder').text('Production Order Detail')
            $('#FormProductionOrderDetail #IsUpdate').val('True');
            $('#FormProductionOrderDetail #SaleOrderQty').val(productionOrderDetail.SaleOrderQty);
            $('#FormProductionOrderDetail #hdnSaleOrderIDForDetail').val($('#hdnSaleOrderID').val());
            $('#FormProductionOrderDetail #hdnSaleOrderDetailID').val(productionOrderDetail.SaleOrderDetailID);
            $('#FormProductionOrderDetail #hdnProductionOrderDetailID').val(productionOrderDetail.ID);
            //$('#FormProductionOrderDetail #hdnPrevProducedQty').val(productionOrderDetail.PrevProducedQty);
            //$('#FormProductionOrderDetail #hdnTotalProducedQty').val(productionOrderDetail.TotalProducedQty);
            $('#FormProductionOrderDetail #ID').val(productionOrderDetail.ID);
            //$("#FormProductionOrderDetail #ProductID").val(productionOrderDetail.ProductID)
            $("#FormProductionOrderDetail #hdnProductID").val(productionOrderDetail.ProductID)
            $('#productName').text(productionOrderDetail.Product.Code + "-" + productionOrderDetail.Product.Name)
            $('#productModelName').text(productionOrderDetail.ProductModel.Name)
            $('#divProductBasicInfo').load("Product/ProductBasicInfo?ID=" + $('#hdnProductID').val(), function (responseTxt, statusTxt, xhr) {
                if (statusTxt == 'success') {
                    
                    //$("#FormProductionOrderDetail #ProductModelID").val(productionOrderDetail.ProductModelID);
                    $("#FormProductionOrderDetail #hdnProductModelID").val(productionOrderDetail.ProductModelID);
                    if ($('#hdnProductModelID').val() != _emptyGuid) {
                        debugger;
                        var curRate = $('#hdnCurrencyRate').val() == undefined ? 0 : $('#hdnCurrencyRate').val();
                        $('#divProductBasicInfo').load("ProductModel/ProductModelBasicInfo?ID=" + $('#hdnProductModelID').val() + "&rate=" + curRate, function () {
                        });
                    }
                }
                
            });           
            $('#FormProductionOrderDetail #ProductSpec').val(productionOrderDetail.ProductSpec);
            debugger;
            if (productionOrderDetail.SaleOrderQty == 0) {
                debugger;
                // $('#FormProductionOrderDetail #OrderQty').val(productionOrderDetail.OrderQty);
                $('#FormProductionOrderDetail #OrderQty').prop("disabled", true);
            }
            else {
                $('#FormProductionOrderDetail #OrderQty').val(productionOrderDetail.OrderQty);
            }
            // $('#FormProductionOrderDetail #OrderQty').val(productionOrderDetail.OrderQty);
            $('#FormProductionOrderDetail #ProducedQty').val(productionOrderDetail.ProducedQty);
            $('#FormProductionOrderDetail #SaleOrderQty').val(productionOrderDetail.SaleOrderQty);
            $('#FormProductionOrderDetail #UnitCode').val(productionOrderDetail.UnitCode);
            $('#FormProductionOrderDetail #hdnUnitCode').val(productionOrderDetail.UnitCode);
            $('#FormProductionOrderDetail #Rate').val(productionOrderDetail.Rate);
            $('#FormProductionOrderDetail #hdnSaleOrderRate').val(productionOrderDetail.Rate);
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
            debugger;
            if (($("#IsMilestoneUpdate").val() == "True" && $('#LatestApprovalStatus').val() == '4') || ($('#IsDocLocked').val() == "True") && $('#LatestApprovalStatus').val() == '4') {
                $('#OrderQty').attr("readonly", true);
                $('#PlantCode').attr("readonly", true);
                $('#ProductSpec').attr("readonly", true);
                $('#UnitCode').attr("disabled", true);
                $('#Rate').attr("readonly", true); 
                $('#spnUnitCode').text(productionOrderDetail.Unit.Description);
                $('#spnPlantCode').text(productionOrderDetail.Plant.Description);
            }
        }
        else {
            console.log("Error: " + xhr.status + ": " + xhr.statusText);
        }
        
    });
}

function ConfirmDeleteProductionOrderDetail(this_Obj) {
    debugger;
    _SlNo = 1;
    _datatablerowindex = _dataTable.ProductionOrderDetailList.row($(this_Obj).parents('tr')).index();
    var productionOrderDetail = _dataTable.ProductionOrderDetailList.row($(this_Obj).parents('tr')).data();
    if (productionOrderDetail.ID === _emptyGuid) {
        notyConfirm('Are you sure to delete?', 'DeleteCurrentProductionOrderDetail("' + _datatablerowindex + '")');
    }
    else {
        notyConfirm('Are you sure to delete?', 'DeleteProductionOrderDetail("' + productionOrderDetail.ID + '")');

    }
}
function DeleteCurrentProductionOrderDetail(_datatablerowindex) {
    var productionOrderDetailList = _dataTable.ProductionOrderDetailList.rows().data();
    productionOrderDetailList.splice(_datatablerowindex, 1);
    _dataTable.ProductionOrderDetailList.clear().rows.add(productionOrderDetailList).draw(false);
    notyAlert('success', 'Detail Row deleted successfully');
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
        $('#lblModelEmailProductionOrder').text('Email Production Order')
        $('#divModelEmailProductionOrder').modal('show');
    });
};  
function SendProductionOrderEmail() {
    debugger;
    if ($('#hdnEmailSentTo').val() != null && $('#hdnEmailSentTo').val() != "" && $('#Subject').val() != null) {
        var filteredFlag = 0;
        var selectedprodindex=[];
        $("table td input.SelectProduct").each(function () {
            if ($(this).is(':checked')) {
                selectedprodindex.push($(this).closest('tr').find('td:eq(1)').text());
                $(this).parent('td').remove();
            }
            else{
                $(this).parent('td').parent('tr').remove();
                filteredFlag = 1;
            }
            //$(this).is(':checked') ? $(this).parent('td').remove() : $(this).parent('td').parent('tr').remove();
        });
        $('table').find('th:first-child').remove();
        //var i = 0;
        if (filteredFlag == "1") {
            $('#spnFilterNote').text("Filtered Document: Showing Item(s) " + selectedprodindex + " only");
            $('#EmailItemList').val(selectedprodindex.toString());
            //$('#ItemDetailsTable tr').each(function () {
            //    $(this).find('td:eq(0)').html(i);
            //    i = i + 1;
            //});
        }
        $('#hdnProductionOrderEMailContent').val($('#divProductionOrderEmailcontainer').html());
        $('#hdnProdOrderNo').val($('#ProdOrderNo').val());
        $('#hdnContactPerson').val($('#ContactPerson').text());
        $('#hdnProdOrderDateFormatted').val($('#ProdOrderDateFormatted').val());
        $('#FormProductionOrderEmailSend #ID').val($('#ProductionOrderForm #ID').val());
        $('#FormProductionOrderEmailSend').submit();
    }
    else {
        if ($('#EmailSentTo').val() == null) {
            $('#sentTolbl').css('color', 'red');
            $("#sentTolbl").attr("title", "Please specify at least one recipient");
            $('#sentTovalidationmsglbl').html('__________________________________');
            $('#sentTovalidationmsglbl').css('color', 'red');
            $("#sentTovalidationmsglbl").attr("title", "Please specify at least one recipient");
        }
    }
}
function UpdateProductionOrderEmailInfo() {
    debugger;
    $('#FormUpdateProductionOrderEmailInfo #ID').val($('#ProductionOrderForm #ID').val());
}
function DownloadProductionOrder() {
    debugger;
    var filteredFlag = 0;
    var selectedprodindex = [];
    $("table td input.SelectProduct").each(function () {
        if ($(this).is(':checked')) {
            selectedprodindex.push($(this).closest('tr').find('td:eq(1)').text());
            $(this).parent('td').remove();
        }
        else{
            $(this).parent('td').parent('tr').remove();
            filteredFlag = 1;
        }
    });
    $('table').find('th:first-child').remove();
    if (filteredFlag == "1") {
        $('#spnFilterNote').text("Filtered Document: Showing Item(s) " + selectedprodindex + " only");
    }
    var bodyContent = $('#divProductionOrderEmailcontainer').html();
    var headerContent = $('#hdnHeadContent').html();
    $('#hdnContent').val(bodyContent);
    $('#hdnHeadContent').val(headerContent);
    if ($('#LatestApprovalStatus').val() == "0")
        $('#hdnWaterMarkFlag').val(true);
    else
        $('#hdnWaterMarkFlag').val(false);
    //var customerName = $("#ProductionOrderForm #CustomerID option:selected").text();
    //$('#hdnCustomerName').val(customerName);
}
function PrintProductionOrder() {
    debugger;
    $("#divModelPrintProductionOrderBody").load("ProductionOrder/PrintProductionOrder?ID=" + $('#ProductionOrderForm #ID').val(), function () {
        $('#lblModelPrintProductionOrder').text('Print ProductionOrder');
        $('#divModelPrintProductionOrder').modal('show');
    });
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
                //MasterAlert("success", _result.Message)
                $("#divModelEmailProductionOrderBody").load("ProductionOrder/EmailProductionOrder?ID=" + $('#ProductionOrderForm #ID').val() + "&EmailFlag=False", function () {
                    $('#lblModelEmailProductionOrder').text('Email Attachment')
                });
                break;
            case "ERROR":
                //MasterAlert("success", _message)
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

//Document Recall
function RecallDocumentItem(documentTypeCode) {
    debugger;
    notyConfirmRecall('Are you sure to recall document?', 'RecallDoc("' + documentTypeCode + '")');
}
function RecallDoc(documentTypeCode) {
    debugger;
    try {
        if (documentTypeCode) {
            $('#ProductionOrderForm').load("DocumentApproval/RecallDocument?documentID=" + $('#ProductionOrderForm #ID').val() + "&documentTypeCode=" + "POD" + "&documentNo=" + $('#ProductionOrderForm #ProdOrderNo').val(), function () {
            });
            var data = { "documentID": $('#ProductionOrderForm #ID').val() };
            _jsonData = GetDataFromServer("DocumentApproval/RecallDocument/", data);
            if (_jsonData != '') {
                _jsonData = JSON.parse(_jsonData);
                _message = _jsonData.Message;
                _status = _jsonData.Status;
                _result = _jsonData.Record;
            }
            switch (_status) {
                case "OK":
                    notyAlert('success', _message);
                    _isApproval = false;
                    ResetProductionOrder();
                    break;
                case "ERROR":
                    notyAlert('error', _message);
                    ResetProductionOrder();
                    break;
                default:
                    break;
            }
        }

    }
    catch (e) {
        console.log(e.message);
    }
}


function EditRedirectToDocument(id) {
    debugger;
    OnServerCallBegin();

    //this will return form body(html)
	$("#divProductionOrderForm").load("ProductionOrder/ProductionOrderForm?id=" + id + "&&isDocumentApprover=" + $("#hdnIsDocumentApprover").val(), function (responseTxt, statusTxt, xhr) {
        if (statusTxt == "success") {
            OnServerCallComplete();
            openNav();
            $('#lblProductionOrderInfo').text($('#ProdOrderNo').val());
            if ($('#IsDocLocked').val() == "True") {
                debugger;
                switch ($('#LatestApprovalStatus').val()) {
                    case "0":
                        _isApproval = false;
                        ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "Draft", id);
                        break;
                    case "1":
                        
                        //ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "ClosedForApproval", id);
                        if ($("#hdnIsDocumentApprover").val() == "True") {
                            ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "ClosedForApprovalApproverEdit", id);
                            _isApproval = false;
                        }
                        else {
                            _isApproval = true;
                            ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "ClosedForApproval", id);
                        }
                        //if ($('#ApproverLevel').val() > 1) {
                        //    ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "Approved", id);
                        //}
                        //else {
                        //    ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "Recalled", id);
                        //}
                        break;
                    case "3":
                        _isApproval = false;
                        ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "Edit", id);
                        break;
                    case "4":
                        _isApproval = true;
                        ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "Approved", id);
                        break;
                    default:
                        //ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "LockDocument", id);
                        //break;
                        if ($('#LatestApprovalStatus').val() == 9) {
                            if ($("#hdnIsDocumentApprover").val() == "True") {
                                _isApproval = false;
                                ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "DocumentApproverEdit", id);
                            }
                            else {
                                ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "LockDocument", id);
                                _isApproval = true;
                            }
                        }
                        else {
                            _isApproval = true;
                            ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "LockDocument", id);
                        }
						break;
                }
            }
            else {
                //$('.switch-label,.switch-handle').addClass('switch-disabled').addClass('disabled');
                //$('.switch-input').prop('disabled', true);
                //$('.switch-label').attr('title', 'Document Locked');
                //ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "LockDocument", id);
                switch ($('#LatestApprovalStatus').val()) {
                    case "4":
                        _isApproval = true;
                        if ($('#IsDistributor').val() == "True" && $('#IsMilestoneUpdate').val() == "False")
                            ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "DistributorApproveDocument", id);
                        else if ($('#IsMilestoneUpdate').val() == "True" && $('#IsDistributor').val() == "False") {
                            ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "MilestoneApproveDocument", id);
                            DisableFields();
                        }
                        else if ($('#IsDistributor').val() == "True" && $('#IsMilestoneUpdate').val() == "True") {
                            ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "DistributerMileStoneApprove", id);
                            DisableFields();
                        }
                        else
                            ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "LockDocument", id);
						break;
					case "1":
					    if ($("#hdnIsDocumentApprover").val() == "True") {
					        _isApproval = false;
					        ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "DocumentApproverEdit", id);
					    }
					    else {
					        _isApproval = true;
					        ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "LockDocument", id);
					    }
						break;
                    default:
                        //ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "LockDocument", id);
                        //break;
                        if ($('#LatestApprovalStatus').val() == 9) {
                            if ($("#hdnIsDocumentApprover").val() == "True") {
                                _isApproval = false;
                                ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "DocumentApproverEdit", id);
                            }
                            else {
                                _isApproval = true;
                                ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "LockDocument", id);
                            }
                        }
                        else {
                            _isApproval = true;
                            ChangeButtonPatchView("ProductionOrder", "btnPatchProductionOrderNew", "LockDocument", id);
                        }
						break;
						
                }
            }
            _SlNo = 1;
            BindProductionOrderDetailList(id);
            $('#divCustomerBasicInfo').load("Customer/CustomerBasicInfo?ID=" + $('#hdnCustomerID').val());
            clearUploadControl();
            PaintImages(id, _isApproval);
            $("#divProductionOrderForm #SaleOrderID").prop('disabled', true);
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

function ClearProdform() {
    debugger;
    ResetProductionOrder();
    $('.showSweetAlert .cancel').click();
}
function DisableFields()
{
    $('#ProdOrderRefNo').attr("readonly", true);
    $('#ProdOrderDateFormatted').attr("readonly", true)
    $('#ExpectedDelvDateFormatted').attr("readonly", true)
    //$('#PreparedBy').attr("disabled", true)
    $('#GeneralNotes').attr("readonly", true);
}