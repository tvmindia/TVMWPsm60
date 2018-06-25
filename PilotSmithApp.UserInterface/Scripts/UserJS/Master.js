var EmptyGuid = "00000000-0000-0000-0000-000000000000";
//Add Product Category
function AddProductCategoryMaster(flag) {
    OnServerCallBegin();
    $("#divMasterBody").load("ProductCategory/MasterPartial?masterCode=0", function (responseTxt, statusTxt, xhr) {
        if (statusTxt == "success") {
            OnServerCallComplete();
            $('#hdnMasterCall').val(flag);
            $('#lblModelMasterContextLabel').text('Add ProductCategory')
            $('#divModelMasterPopUp').modal('show');
        }
        else {
            console.log("Error: " + xhr.status + ": " + xhr.statusText);
        }
    });

    //GetMasterPartial('ProductCategory', "0");
    //$('#h3ModelMasterContextLabel').text('Add ProductCategory')

}

//onsuccess function for formsubmitt
function SaveSuccessProductCategory(data, status) {
    debugger;
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Status) {
        case "OK":
            if ($('#hdnMasterCall').val() == "MSTR") {
                $('#IsUpdate').val('True');
                BindOrReloadProductCategoryTable('Reset');
            }
            else if ($('#hdnMasterCall').val() == "OTR") {
                $('.divProductCategorySelectList').load('/ProductCategory/ProductCategorySelectList?required=' + $('#hdnProductCategoryRequired').val());
            }
            MasterAlert("success", JsonResult.Record.Message)
            break;
        case "ERROR":
            MasterAlert("danger", JsonResult.Message)
            break;
        default:
            MasterAlert("danger", JsonResult.Message)
            break;
    }
    $('#divModelMasterPopUp').modal('hide');
}

//Add Product Specification
function AddProductSpecificationMaster(flag) {
    OnServerCallBegin();
    $("#divMasterBody").load("ProductSpecification/MasterPartial?masterCode=0", function (responseTxt, statusTxt, xhr) {
        if (statusTxt == "success") {
            OnServerCallComplete();
            $('#hdnMasterCall').val(flag);
            $('#lblModelMasterContextLabel').text('Add Product Specification')
            $('#divModelMasterPopUp').modal('show');
        }
        else {
            console.log("Error: " + xhr.status + ": " + xhr.statusText);
        }
    });

}

//onsuccess function for formsubmitt
function SaveSuccessProductSpecification(data, status) {
    debugger;
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Status) {
        case "OK":
            if ($('#hdnMasterCall').val() == "MSTR") {
                $('#IsUpdate').val('True');
                BindOrReloadProductSpecificationTable('Reset');
            }
            else if ($('#hdnMasterCall').val() == "OTR") {
                $('.divProductSpecificationSelectList').load('/ProductSpecification/ProductSpecificationSelectList?required=' + $('#hdnProductSpecificationRequired').val());
            }
            MasterAlert("success", JsonResult.Record.Message)
            break;
        case "ERROR":
            MasterAlert("danger", JsonResult.Message)
            break;
        default:
            MasterAlert("danger", JsonResult.Message)
            break;
    }
    $('#divModelMasterPopUp').modal('hide');
}

//Add State master
function AddStateMaster(flag) {
    OnServerCallBegin();
    $("#divMasterBody").load("State/MasterPartial?masterCode=0", function (responseTxt, statusTxt, xhr) {
        if (statusTxt == "success") {
            OnServerCallComplete();
            $('#lblModelMasterContextLabel').text('Add State Information')
            $('#divModelMasterPopUp').modal('show');
            $('#hdnMasterCall').val(flag);
        }
        else {
            console.log("Error: " + xhr.status + ": " + xhr.statusText);
        }
    });

}

//onsuccess function for formsubmitt
function SaveSuccessState(data, status) {
    debugger;
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Status) {
        case "OK":
            if ($('#hdnMasterCall').val() == "MSTR") {
                $('#IsUpdate').val('True');
                BindOrReloadStateTable('Reset');
            }
            else if ($('#hdnMasterCall').val() == "OTR") {
                $('.divStateSelectList').load('/State/StateSelectList?required=' + $('#hdnStateRequired').val());
            }
            MasterAlert("success", JsonResult.Record.Message)
            break;
        case "ERROR":
            MasterAlert("danger", JsonResult.Message)
            break;
        default:
            MasterAlert("danger", JsonResult.Message)
            break;
    }
    $('#divModelMasterPopUp').modal('hide');
}

//Add District
function AddDistrictMaster(flag) {
    debugger;
    OnServerCallBegin();
    $("#divMasterBody").load("District/MasterPartial?masterCode=0", function (responseTxt, statusTxt, xhr) {
        if (statusTxt == "success") {
            OnServerCallComplete();
            $('#lblModelMasterContextLabel').text('Add District Information')
            $('#divModelMasterPopUp').modal('show');
            $('#hdnMasterCall').val(flag);
        }
        else {
            console.log("Error: " + xhr.status + ": " + xhr.statusText);
        }
    });
}

//onsuccess function for formsubmitt
function SaveSuccessDistrict(data, status) {
    debugger;
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Status) {
        case "OK":
            if ($('#hdnMasterCall').val() == "MSTR") {
                $('#IsUpdate').val('True');
                BindOrReloadDistrictTable('Reset');
            }
            else if ($('#hdnMasterCall').val() == "OTR") {
                $('.divDistrictSelectList').load('/District/DistrictSelectList?required=' + $('#hdnDistrictRequired').val());
            }
            MasterAlert("success", JsonResult.Record.Message)
            break;
        case "ERROR":
            MasterAlert("danger", JsonResult.Message)
            break;
        default:
            MasterAlert("danger", JsonResult.Message)
            break;
    }
    $('#divModelMasterPopUp').modal('hide');
}

//Add Area
function AddAreaMaster(flag) {
    debugger;
    OnServerCallBegin();
    $("#divMasterBody").load("Area/MasterPartial?masterCode=0", function (responseTxt, statusTxt, xhr) {
        if (statusTxt == "success") {
            OnServerCallComplete();
            $('#lblModelMasterContextLabel').text('Add Area Information')
            $('#divModelMasterPopUp').modal('show');
            $('#hdnMasterCall').val(flag);
        }
        else {
            console.log("Error: " + xhr.status + ": " + xhr.statusText);
        }
    });
}

//onsuccess function for formsubmitt
function SaveSuccessArea(data, status) {
    debugger;
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Status) {
        case "OK":
            if ($('#hdnMasterCall').val() == "MSTR") {
                $('#IsUpdate').val('True');
                BindOrReloadAreaTable('Reset');
            }
            else if ($('#hdnMasterCall').val() == "OTR") {
                $('.divAreaSelectList').load('/Area/AreaSelectList?required=' + $('#hdnAreaRequired').val());
            }
            MasterAlert("success", JsonResult.Record.Message)
            break;
        case "ERROR":
            MasterAlert("danger", JsonResult.Message)
            break;
        default:
            MasterAlert("danger", JsonResult.Message)
            break;
    }
    $('#divModelMasterPopUp').modal('hide');
}

//Add Product
function AddProductMaster(flag) {
    debugger;
    OnServerCallBegin();
    $("#divMasterBody").load("Product/MasterPartial?masterCode=" + EmptyGuid, function (responseTxt, statusTxt, xhr) {
        if (statusTxt == "success") {
            OnServerCallComplete();
            $('#hdnMasterCall').val(flag);
            $('#lblModelMasterContextLabel').text('Add Product')
            $('#divModelMasterPopUp').modal('show');
        }
        else {
            console.log("Error: " + xhr.status + ": " + xhr.statusText);
        }
    });

    //GetMasterPartial('Product', EmptyGuid);
    //$('#h3ModelMasterContextLabel').text('Add Product')
    //$('#divModelMasterPopUp').modal('show');
    //$('#hdnMasterCall').val(flag);
}

//onsuccess function for formsubmitt
function SaveSuccessProduct(data, status) {
    debugger;
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Status) {
        case "OK":
            if ($('#hdnMasterCall').val() == "MSTR") {
                $('#IsUpdate').val('True');
                BindOrReloadProductTable('Reset');
            }
            else if ($('#hdnMasterCall').val() == "OTR") {
                $('.divProductSelectList').load('/Product/ProductSelectList?required='+$('#hdnProductRequired').val());
            }
            MasterAlert("success", JsonResult.Record.Message)
            break;
        case "ERROR":
            MasterAlert("danger", JsonResult.Message)
            break;
        default:
            MasterAlert("danger", JsonResult.Message)
            break;
    }
    $('#divModelMasterPopUp').modal('hide');
}

//Add Company
function AddCompanyMaster(flag) {
    debugger;
    OnServerCallBegin();
    $("#divMasterBody").load("Company/MasterPartial?masterCode=" + EmptyGuid, function (responseTxt, statusTxt, xhr) {
        if (statusTxt == "success") {
            OnServerCallComplete();
            $('#hdnMasterCall').val(flag);
            $('#lblModelMasterContextLabel').text('Add Company')
            $('#divModelMasterPopUp').modal('show');
        }
        else {
            console.log("Error: " + xhr.status + ": " + xhr.statusText);
        }
    });

}

//onsuccess function for formsubmitt
function SaveSuccessCompany(data, status) {
    debugger;
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Status) {
        case "OK":
            if ($('#hdnMasterCall').val() == "MSTR") {
                $('#IsUpdate').val('True');
                BindOrReloadCompanyTable('Reset');
            }
            else if ($('#hdnMasterCall').val() == "OTR") {
                $('.divCompanySelectList').load('/Company/CompanySelectList?required=' + $('#hdnCompanyRequired').val());
            }
            MasterAlert("success", JsonResult.Record.Message)
            break;
        case "ERROR":
            MasterAlert("danger", JsonResult.Message)
            break;
        default:
            MasterAlert("danger", JsonResult.Message)
            break;
    }
    $('#divModelMasterPopUp').modal('hide');
}

//Add Product Model
function AddProductModelMaster(flag) {
    debugger;
    OnServerCallBegin();
    $("#divMasterBody").load("ProductModel/MasterPartial?masterCode=" + EmptyGuid, function (responseTxt, statusTxt, xhr) {
        if (statusTxt == "success") {
            OnServerCallComplete();
            $('#hdnMasterCall').val(flag);
            $('#lblModelMasterContextLabel').text('Add Product Model')
            $('#divModelMasterPopUp').modal('show');
        }
        else {
            console.log("Error: " + xhr.status + ": " + xhr.statusText);
        }
    });
}

//onsuccess function for formsubmitt
function SaveSuccessProductModel(data, status) {
    debugger;
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Status) {
        case "OK":
            if ($('#hdnMasterCall').val() == "MSTR") {
                $('#IsUpdate').val('True');
                BindOrReloadProductModelTable('Reset');
            }
            else if ($('#hdnMasterCall').val() == "OTR") {
                $('.divProductModelSelectList').load('/ProductModel/ProductModelSelectList?required=' + $('#hdnProductModelRequired').val()+'&productID='+$('#hdnProductID').val());
            }
            MasterAlert("success", JsonResult.Record.Message)
            break;
        case "ERROR":
            MasterAlert("danger", JsonResult.Message)
            break;
        default:
            MasterAlert("danger", JsonResult.Message)
            break;
    }
    $('#divModelMasterPopUp').modal('hide');
}
//=========================================================================================================
//Add Employee master
function AddEmployeeMaster(flag) {
    //$("#divMasterBody").load("DynamicUI/PopUpUnderConstruction", function () {
    //    $('#lblModelMasterContextLabel').text('Add Employee Information')
    //    $('#divModelMasterPopUp').modal('show');
    //    $('#hdnMasterCall').val(flag);
    //});
    debugger;
    OnServerCallBegin();
    $("#divMasterBody").load("Employee/MasterPartial?masterCode=" + EmptyGuid, function (responseTxt, statusTxt, xhr) {
        if (statusTxt == "success") {
            OnServerCallComplete();
            $('#lblModelMasterContextLabel').text('Add Employee Information')
            $('#divModelMasterPopUp').modal('show');
            $('#hdnMasterCall').val(flag);
        }
        else {
            console.log("Error: " + xhr.status + ": " + xhr.statusText);
        }
    });
}

//onsuccess function for formsubmitt
function SaveSuccessEmployeeMaster(data, status) {
    debugger;
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Status) {
        case "OK":
            if ($('#hdnMasterCall').val() == "MSTR") {
                $('#IsUpdate').val('True');
                BindOrReloadEmployeeTable('Reset');
            }
            else if ($('#hdnMasterCall').val() == "OTR") {
                if ($(".divEmployeeSelectList")[0])
                    $('.divEmployeeSelectList').load('/Employee/EmployeeSelectList?required='+$('#hdnEmployeeRequired').val());
                if ($(".divResponsiblePersonSelectList")[0])
                    $('.divResponsiblePersonSelectList').load('/Employee/ResponsiblePersonSelectList?required=' + $('$hdnResponsiblePersonRequired').val());
                if ($(".divAttendedBySelectList")[0])
                    $('.divAttendedBySelectList').load('/Employee/AttendedBySelectList?required=' + $('#hdnAttendedByRequired').val());
                if ($(".divServicedBySelectList")[0])
                    $('.divServicedBySelectList').load('/Employee/ServicedBySelectList?required=' + $('#hdnServicedByRequired').val());
                if ($(".divPreparedBySelectList")[0])
                    $('.divPreparedBySelectList').load('/Employee/PreparedBySelectList?required=' + $('#hdnPreparedByRequired').val());
                if ($(".divQCBySelectList")[0])
                    $('.divQCBySelectList').load('/Employee/QCBySelectList?required=' + $('#hdnQCByRequired').val());
            }
            MasterAlert("success", JsonResult.Record.Message)
            break;
        case "ERROR":
            MasterAlert("danger", JsonResult.Message)
            break;
        default:
            MasterAlert("danger", JsonResult.Message)
            break;
    }
    $('#divModelMasterPopUp').modal('hide');
}
//=========================================================================================================
function AddBranchMaster(flag) {
    OnServerCallBegin();
    $("#divMasterBody").load("DynamicUI/PopUpUnderConstruction", function (responseTxt, statusTxt, xhr) {
        if (statusTxt == "success") {
            OnServerCallComplete();
            $('#lblModelMasterContextLabel').text('Add Branch Information')
            $('#divModelMasterPopUp').modal('show');
            $('#hdnMasterCall').val(flag);
        }
        else {
            console.log("Error: " + xhr.status + ": " + xhr.statusText);
        }
    });
    //$("#divMasterBody").load("Employee/MasterPartial?masterCode=" + EmptyGuid, function () {
    //    $('#lblModelMasterContextLabel').text('Add Employee Information')
    //    $('#divModelMasterPopUp').modal('show');
    //    $('#hdnMasterCall').val(flag);
    //});
}
//========================================================================================================
function AddReferredByMaster(flag) {
    OnServerCallBegin();
    $("#divMasterBody").load("ReferencePerson/MasterPartial?masterCode=0", function (responseTxt, statusTxt, xhr) {
        if (statusTxt == "success") {
            OnServerCallComplete();
            $('#lblModelMasterContextLabel').text('Add Reference Person Information')
            $('#divModelMasterPopUp').modal('show');
            $('#hdnMasterCall').val(flag);
        }
        else {
            console.log("Error: " + xhr.status + ": " + xhr.statusText);
        }
    });
}
//================================================================================================
function AddDocumentStatusMaster() {
    OnServerCallBegin();
    $("#divMasterBody").load("DynamicUI/PopUpUnderConstruction", function (responseTxt, statusTxt, xhr) {
        if (statusTxt == "success") {
            OnServerCallComplete();
            $('#lblModelMasterContextLabel').text('Add Document Status Information')
            $('#divModelMasterPopUp').modal('show');
            $('#hdnMasterCall').val(flag);
        }
        else {
            console.log("Error: " + xhr.status + ": " + xhr.statusText);
        }
    });
}
//==========================================================================================================
//Add Customer master
function AddCustomerMaster(flag) {
    OnServerCallBegin();
    $("#divMasterBody").load("Customer/AddCustomerPartial", function (responseTxt, statusTxt, xhr) {
        if (statusTxt == "success") {
            OnServerCallComplete();
            $('#lblModelMasterContextLabel').text('Add Customer Information')
            $('#divModelMasterPopUp').modal('show');
        }
        else {
            console.log("Error: " + xhr.status + ": " + xhr.statusText);
        }
    });
}
//==========================================================================================================
//Add OtherCharge master
function AddOtherChargeMaster(flag) {
    OnServerCallBegin();
    $("#divMasterBody").load("OtherCharge/AddOtherChargePartial", function (responseTxt, statusTxt, xhr) {
        if (statusTxt == "success") {
            OnServerCallComplete();
            $('#lblModelMasterContextLabel').text('Add Other Charge Information')
            $('#divModelMasterPopUp').modal('show');
        }
        else {
            console.log("Error: " + xhr.status + ": " + xhr.statusText);
        }
    });
}

//onsuccess function for formsubmitt customer master
function SaveSuccessCustomerMaster(data, status) {
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Status) {
        case "OK":
            $('.divCustomerSelectList').load('/Customer/CustomerSelectList?required=' + $('#hdnCustomerRequired').val());
            MasterAlert("success", JsonResult.Record.Message)
            break;
        case "ERROR":
            MasterAlert("danger", JsonResult.Message)
            break;
        default:
            MasterAlert("danger", JsonResult.Message)
            break;
    }
    $('#divModelMasterPopUp').modal('hide');
}


//-- add Approver--//
function AddApproverMaster(flag) {
    debugger;
    OnServerCallBegin();
    $("#divMasterBody").load("Approver/MasterPartial?masterCode=" + EmptyGuid, function (responseTxt, statusTxt, xhr) {
        if (statusTxt == "success") {
            OnServerCallComplete();
            $('#hdnMasterCall').val(flag);
            $('#lblModelMasterContextLabel').text('Add Approver')
            $('#divModelMasterPopUp').modal('show');
        }
        else {
            console.log("Error: " + xhr.status + ": " + xhr.statusText);
        }
    });

    //GetMasterPartial("Approver", "");
    //$('#h3ModelMasterContextLabel').text('Add Approver')
    //$('#divModelMasterPopUp').modal('show');
    //$('#hdnMasterCall').val(flag);
}

//onsuccess function for formsubmitt
function SaveSuccessApprover(data, status) {
    debugger;
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Status) {
        case "OK":
            if ($('#hdnMasterCall').val() == "MSTR") {
                $('#IsUpdate').val('True');
                BindOrReloadApproverTable('Reset');
            }
            else if ($('#hdnMasterCall').val() == "OTR") {
                $('.divApproverSelectList').load('/Approver/ApproverSelectList?required=');
            }
            MasterAlert("success", JsonResult.Record.Message)
            break;
        case "ERROR":
            MasterAlert("danger", JsonResult.Message)
            break;
        default:
            MasterAlert("danger", JsonResult.Message)
            break;
    }
    $('#divModelMasterPopUp').modal('hide');
}

//Add ReferencePerson
function AddReferencePersonMaster(flag) {
    debugger;
    OnServerCallBegin();
    $("#divMasterBody").load("ReferencePerson/MasterPartial?masterCode=0", function (responseTxt, statusTxt, xhr) {
        if (statusTxt == "success") {
            OnServerCallComplete();
            $('#lblModelMasterContextLabel').text('Add Reference Person Information')
            $('.modal-dialog').attr('style', 'min-width:60%')
            $('#divModelMasterPopUp').modal('show');
            $('#hdnMasterCall').val(flag);
        }
        else {
            console.log("Error: " + xhr.status + ": " + xhr.statusText);
        }
    });
}

//onsuccess function for formsubmitt
function SaveSuccessReferencePerson(data, status) {
    debugger;
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Status) {
        case "OK":
            if ($('#hdnMasterCall').val() == "MSTR") {
                $('#IsUpdate').val('True');
                BindOrReloadReferencePersonTable('Reset');
            }
            else if ($('#hdnMasterCall').val() == "OTR") {
                $('.divReferredByCodeSelectList').load('/ReferencePerson/ReferencePersonSelectList?required=' + $('#hdnReferredByRequired').val());
            }
            MasterAlert("success", JsonResult.Record.Message)
            break;
        case "ERROR":
            MasterAlert("danger", JsonResult.Message)
            break;
        default:
            MasterAlert("danger", JsonResult.Message)
            break;
    }
    $('#divModelMasterPopUp').modal('hide');
}


//Add PaymentTerm
function AddPaymentTermMaster(flag) {
    debugger;
    OnServerCallBegin();
    $("#divMasterBody").load("PaymentTerm/MasterPartial?masterCode=", function (responseTxt, statusTxt, xhr) {
        if (statusTxt == "success") {
            OnServerCallComplete();
            $('#lblModelMasterContextLabel').text('Add Payment Term Information')
            $('#divModelMasterPopUp').modal('show');

            $('#hdnMasterCall').val(flag);
        }
        else {
            console.log("Error: " + xhr.status + ": " + xhr.statusText);
        }
    });
}

//onsuccess function for formsubmitt
function SaveSuccessPaymentTerm(data, status) {
    debugger;
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Status) {
        case "OK":
            if ($('#hdnMasterCall').val() == "MSTR") {
                $('#IsUpdate').val('True');
                BindOrReloadPaymentTermTable('Reset');
            }
            else if ($('#hdnMasterCall').val() == "OTR") {
                $('.divPaymentTermSelectList').load('/PaymentTerm/PaymentTermSelectList?required=' + $('#hdnPaymentTermRequired').val());
            }
            MasterAlert("success", JsonResult.Record.Message)
            break;
        case "ERROR":
            MasterAlert("danger", JsonResult.Message)
            break;
        default:
            MasterAlert("danger", JsonResult.Message)
            break;
    }
    $('#divModelMasterPopUp').modal('hide');
}

//Add TaxType
function AddTaxTypeMaster(flag) {
    debugger;
    OnServerCallBegin();
    $("#divMasterBody").load("TaxType/MasterPartial?masterCode=0", function (responseTxt, statusTxt, xhr) {
        if (statusTxt == "success") {
            OnServerCallComplete();
            $('#lblModelMasterContextLabel').text('Add Tax Type Information')
            $('#divModelMasterPopUp').modal('show');

            $('#hdnMasterCall').val(flag);
        }
        else {
            console.log("Error: " + xhr.status + ": " + xhr.statusText);
        }
    });
}

//onsuccess function for formsubmitt
function SaveSuccessTaxType(data, status) {
    debugger;
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Status) {
        case "OK":
            if ($('#hdnMasterCall').val() == "MSTR") {
                $('#IsUpdate').val('True');
                BindOrReloadTaxTypeTable('Reset');
            }
            else if ($('#hdnMasterCall').val() == "OTR") {
                if ($('#divModelPopQuotation')[0]) {
                    if ($('.divOtherChargeSelectList')[0])
                    $('.divTaxTypeSelectList').load('/TaxType/TaxTypeSelectList?required=' + $('#hdnTaxTypeRequired').val(), function () {
                        $('#divModelPopQuotation .CalculateGST').change(function () {
                            debugger;
                            var ChargeAmount = parseFloat($('#divModelPopQuotation #ChargeAmount').val() != "" ? $('#divModelPopQuotation #ChargeAmount').val() : 0);
                            var CGST = parseFloat($('#divModelPopQuotation #TaxTypeCode').val() != "" ? $('#divModelPopQuotation #TaxTypeCode').val().split('|')[1].split(',')[0].split('-')[1] : 0);
                            var SGST = parseFloat($('#divModelPopQuotation #TaxTypeCode').val() != "" ? $('#divModelPopQuotation #TaxTypeCode').val().split('|')[1].split(',')[1].split('-')[1] : 0);
                            var IGST = parseFloat($('#divModelPopQuotation #TaxTypeCode').val() != "" ? $('#divModelPopQuotation #TaxTypeCode').val().split('|')[1].split(',')[2].split('-')[1] : 0);
                            $('#divModelPopQuotation #hdnCGSTPerc').val(CGST);
                            $('#divModelPopQuotation #hdnSGSTPerc').val(SGST);
                            $('#divModelPopQuotation #hdnIGSTPerc').val(IGST);
                            $('#divModelPopQuotation #CGSTPerc').val(ChargeAmount * CGST / 100);
                            $('#divModelPopQuotation #SGSTPerc').val(ChargeAmount * SGST / 100);
                            $('#divModelPopQuotation #IGSTPerc').val(ChargeAmount * IGST / 100);
                        });
                    });
                    else
                        $('.divTaxTypeSelectList').load('/TaxType/TaxTypeSelectList?required=' + $('#hdnTaxTypeRequired').val(), function () {
                            $('#divModelPopQuotation .CalculateGST').change(function () {
                                debugger;
                                var qty = parseInt($('#divModelPopQuotation #Qty').val() != "" ? $('#divModelPopQuotation #Qty').val() : 1);
                                var rate = parseFloat($('#divModelPopQuotation #Rate').val() != "" ? $('#divModelPopQuotation #Rate').val() : 0);
                                var discount = parseFloat($('#divModelPopQuotation #Discount').val() != "" ? $('#divModelPopQuotation #Discount').val() : 0);
                                var CGST = parseFloat($('#divModelPopQuotation #TaxTypeCode').val() != "" ? $('#divModelPopQuotation #TaxTypeCode').val().split('|')[1].split(',')[0].split('-')[1] : 0);
                                var SGST = parseFloat($('#divModelPopQuotation #TaxTypeCode').val() != "" ? $('#divModelPopQuotation #TaxTypeCode').val().split('|')[1].split(',')[1].split('-')[1] : 0);
                                var IGST = parseFloat($('#divModelPopQuotation #TaxTypeCode').val() != "" ? $('#divModelPopQuotation #TaxTypeCode').val().split('|')[1].split(',')[2].split('-')[1] : 0);
                                var taxableAmount = rate * qty - discount;
                                $('#divModelPopQuotation #hdnCGSTPerc').val(CGST);
                                $('#divModelPopQuotation #hdnSGSTPerc').val(SGST);
                                $('#divModelPopQuotation #hdnIGSTPerc').val(IGST);
                                $('#divModelPopQuotation #CGSTPerc').val(taxableAmount * CGST / 100);
                                $('#divModelPopQuotation #SGSTPerc').val(taxableAmount * SGST / 100);
                                $('#divModelPopQuotation #IGSTPerc').val(taxableAmount * IGST / 100);
                            });
                        })
                }
                if ($('#divModelPopSaleOrder')[0]) {
                    if ($('.divOtherChargeSelectList')[0])
                    $('.divTaxTypeSelectList').load('/TaxType/TaxTypeSelectList?required=' + $('#hdnTaxTypeRequired').val(), function () {
                        $('#divModelPopSaleOrder .CalculateGST').change(function () {
                            debugger;
                            var ChargeAmount = parseFloat($('#divModelPopSaleOrder #ChargeAmount').val() != "" ? $('#divModelPopSaleOrder #ChargeAmount').val() : 0);
                            var CGST = parseFloat($('#divModelPopSaleOrder #TaxTypeCode').val() != "" ? $('#divModelPopSaleOrder #TaxTypeCode').val().split('|')[1].split(',')[0].split('-')[1] : 0);
                            var SGST = parseFloat($('#divModelPopSaleOrder #TaxTypeCode').val() != "" ? $('#divModelPopSaleOrder #TaxTypeCode').val().split('|')[1].split(',')[1].split('-')[1] : 0);
                            var IGST = parseFloat($('#divModelPopSaleOrder #TaxTypeCode').val() != "" ? $('#divModelPopSaleOrder #TaxTypeCode').val().split('|')[1].split(',')[2].split('-')[1] : 0);
                            var AddlTaxPerc = parseFloat($('#divModelPopSaleOrder #AddlTaxPerc').val() != "" ? $('#divModelPopSaleOrder #AddlTaxPerc').val() : 0);
                            $('#divModelPopSaleOrder #hdnCGSTPerc').val(CGST);
                            $('#divModelPopSaleOrder #hdnSGSTPerc').val(SGST);
                            $('#divModelPopSaleOrder #hdnIGSTPerc').val(IGST);
                            $('#divModelPopSaleOrder #hdnAddlTaxPerc').val(AddlTaxPerc);
                            $('#divModelPopSaleOrder #CGSTPerc').val(ChargeAmount * CGST / 100);
                            $('#divModelPopSaleOrder #SGSTPerc').val(ChargeAmount * SGST / 100);
                            $('#divModelPopSaleOrder #IGSTPerc').val(ChargeAmount * IGST / 100);
                            var TaxAmount = parseFloat($('#divModelPopSaleOrder #CGSTPerc').val()) + parseFloat($('#divModelPopSaleOrder #SGSTPerc').val()) + parseFloat($('#divModelPopSaleOrder #IGSTPerc').val())
                            $('#divModelPopSaleOrder #AddlTaxAmt').val(TaxAmount * AddlTaxPerc / 100);
                            $('#divModelPopSaleOrder #hdnAddlTaxAmt').val(TaxAmount * AddlTaxPerc / 100);
                        });
                    });
                    else
                        $('.divTaxTypeSelectList').load('/TaxType/TaxTypeSelectList?required=' + $('#hdnTaxTypeRequired').val(), function () {
                            $('#divModelPopSaleOrder .CalculateGST').change(function () {
                                debugger;
                                var qty = parseInt($('#divModelPopSaleOrder #Qty').val() != "" ? $('#divModelPopSaleOrder #Qty').val() : 1);
                                var rate = parseFloat($('#divModelPopSaleOrder #Rate').val() != "" ? $('#divModelPopSaleOrder #Rate').val() : 0);
                                var discount = parseFloat($('#divModelPopSaleOrder #Discount').val() != "" ? $('#divModelPopSaleOrder #Discount').val() : 0);
                                var CGST = parseFloat($('#divModelPopSaleOrder #TaxTypeCode').val() != "" ? $('#divModelPopSaleOrder #TaxTypeCode').val().split('|')[1].split(',')[0].split('-')[1] : 0);
                                var SGST = parseFloat($('#divModelPopSaleOrder #TaxTypeCode').val() != "" ? $('#divModelPopSaleOrder #TaxTypeCode').val().split('|')[1].split(',')[1].split('-')[1] : 0);
                                var IGST = parseFloat($('#divModelPopSaleOrder #TaxTypeCode').val() != "" ? $('#divModelPopSaleOrder #TaxTypeCode').val().split('|')[1].split(',')[2].split('-')[1] : 0);
                                var CessPerc = parseFloat($('#divModelPopSaleOrder #CessPerc').val() != "" ? $('#divModelPopSaleOrder #CessPerc').val() : 0);
                                var taxableAmount = rate * qty - discount;
                                $('#divModelPopSaleOrder #hdnCGSTPerc').val(CGST);
                                $('#divModelPopSaleOrder #hdnSGSTPerc').val(SGST);
                                $('#divModelPopSaleOrder #hdnIGSTPerc').val(IGST);

                                $('#divModelPopSaleOrder #CGSTPerc').val(taxableAmount * CGST / 100);
                                $('#divModelPopSaleOrder #SGSTPerc').val(taxableAmount * SGST / 100);
                                $('#divModelPopSaleOrder #IGSTPerc').val(taxableAmount * IGST / 100);
                                var TaxAmount = parseFloat(taxableAmount * CGST / 100) + parseFloat(taxableAmount * SGST / 100) + parseFloat(taxableAmount * IGST / 100)
                                $('#divModelPopSaleOrder #CessAmt').val((TaxAmount * CessPerc) / 100);
                            });
                        });
                }
                if ($('#divModelPopSaleInvoice')[0]) {
                    if ($('.divOtherChargeSelectList')[0])
                    $('.divTaxTypeSelectList').load('/TaxType/TaxTypeSelectList?required=' + $('#hdnTaxTypeRequired').val(), function () {
                        $('#divModelPopSaleInvoice .CalculateGST').change(function () {
                            debugger;
                            var CGST = 0, SGST = 0, IGST = 0;
                            var ChargeAmount = parseFloat($('#divModelPopSaleInvoice #ChargeAmount').val() != "" ? $('#divModelPopSaleInvoice #ChargeAmount').val() : 0);
                            if ($('#divModelPopSaleInvoice #TaxTypeCode').val() != null) {
                                CGST = parseFloat($('#divModelPopSaleInvoice #TaxTypeCode').val() != "" ? $('#divModelPopSaleInvoice #TaxTypeCode').val().split('|')[1].split(',')[0].split('-')[1] : 0);
                                SGST = parseFloat($('#divModelPopSaleInvoice #TaxTypeCode').val() != "" ? $('#divModelPopSaleInvoice #TaxTypeCode').val().split('|')[1].split(',')[1].split('-')[1] : 0);
                                IGST = parseFloat($('#divModelPopSaleInvoice #TaxTypeCode').val() != "" ? $('#divModelPopSaleInvoice #TaxTypeCode').val().split('|')[1].split(',')[2].split('-')[1] : 0);
                            }
                            var AddlTaxPerc = parseFloat($('#divModelPopSaleInvoice #AddlTaxPerc').val() != "" ? $('#divModelPopSaleInvoice #AddlTaxPerc').val() : 0);
                            $('#divModelPopSaleInvoice #hdnCGSTPerc').val(CGST);
                            $('#divModelPopSaleInvoice #hdnSGSTPerc').val(SGST);
                            $('#divModelPopSaleInvoice #hdnIGSTPerc').val(IGST);
                            $('#divModelPopSaleInvoice #hdnAddlTaxPerc').val(AddlTaxPerc);
                            $('#divModelPopSaleInvoice #CGSTPerc').val(ChargeAmount * CGST / 100);
                            $('#divModelPopSaleInvoice #SGSTPerc').val(ChargeAmount * SGST / 100);
                            $('#divModelPopSaleInvoice #IGSTPerc').val(ChargeAmount * IGST / 100);
                            var TaxAmount = parseFloat($('#divModelPopSaleInvoice #CGSTPerc').val()) + parseFloat($('#divModelPopSaleInvoice #SGSTPerc').val()) + parseFloat($('#divModelPopSaleInvoice #IGSTPerc').val())
                            $('#divModelPopSaleInvoice #AddlTaxAmt').val(TaxAmount * AddlTaxPerc / 100);
                            $('#divModelPopSaleInvoice #hdnAddlTaxAmt').val(TaxAmount * AddlTaxPerc / 100);
                        });
                    });
                    else
                        $('.divTaxTypeSelectList').load('/TaxType/TaxTypeSelectList?required=' + $('#hdnTaxTypeRequired').val(), function () {
                            $('#divModelPopSaleInvoice .CalculateGST').change(function () {
                                debugger;
                                var CGST = 0, SGST = 0, IGST = 0;
                                var qty = parseInt($('#divModelPopSaleInvoice #Qty').val() != "" ? $('#divModelPopSaleInvoice #Qty').val() : 0);
                                var rate = parseFloat($('#divModelPopSaleInvoice #Rate').val() != "" ? $('#divModelPopSaleInvoice #Rate').val() : 0);
                                var discount = parseFloat($('#divModelPopSaleInvoice #Discount').val() != "" ? $('#divModelPopSaleInvoice #Discount').val() : 0);
                                if ($('#divModelPopSaleInvoice #TaxTypeCode').val() != null) {
                                    CGST = parseFloat($('#divModelPopSaleInvoice #TaxTypeCode').val() != "" ? $('#divModelPopSaleInvoice #TaxTypeCode').val().split('|')[1].split(',')[0].split('-')[1] : 0);
                                    SGST = parseFloat($('#divModelPopSaleInvoice #TaxTypeCode').val() != "" ? $('#divModelPopSaleInvoice #TaxTypeCode').val().split('|')[1].split(',')[1].split('-')[1] : 0);
                                    IGST = parseFloat($('#divModelPopSaleInvoice #TaxTypeCode').val() != "" ? $('#divModelPopSaleInvoice #TaxTypeCode').val().split('|')[1].split(',')[2].split('-')[1] : 0);
                                }
                                var CessPerc = parseFloat($('#divModelPopSaleInvoice #CessPerc').val() != "" ? $('#divModelPopSaleInvoice #CessPerc').val() : 0);
                                var taxableAmount = (rate * qty) - discount;
                                $('#divModelPopSaleInvoice #hdnCGSTPerc').val(CGST);
                                $('#divModelPopSaleInvoice #hdnSGSTPerc').val(SGST);
                                $('#divModelPopSaleInvoice #hdnIGSTPerc').val(IGST);
                                $('#divModelPopSaleInvoice #CGSTPerc').val(taxableAmount * CGST / 100);
                                $('#divModelPopSaleInvoice #SGSTPerc').val(taxableAmount * SGST / 100);
                                $('#divModelPopSaleInvoice #IGSTPerc').val(taxableAmount * IGST / 100);
                                var TaxAmount = parseFloat(taxableAmount * CGST / 100) + parseFloat(taxableAmount * SGST / 100) + parseFloat(taxableAmount * IGST / 100)
                                $('#divModelPopSaleInvoice #CessAmt').val((TaxAmount * CessPerc) / 100);
                            });
                        });
                }
            }
            MasterAlert("success", JsonResult.Record.Message)
            break;
        case "ERROR":
            MasterAlert("danger", JsonResult.Message)
            break;
        default:
            MasterAlert("danger", JsonResult.Message)
            break;
    }
    $('#divModelMasterPopUp').modal('hide');
}

//Add CustomerCategory
function AddCustomerCategoryMaster(flag) {
    debugger;
    OnServerCallBegin();
    $("#divMasterBody").load("CustomerCategory/MasterPartial?masterCode=0", function (responseTxt, statusTxt, xhr) {
        if (statusTxt == "success") {
            OnServerCallComplete();
            $('#lblModelMasterContextLabel').text('Add Customer Category Information')
            $('#divModelMasterPopUp').modal('show');

            $('#hdnMasterCall').val(flag);
        }
        else {
            console.log("Error: " + xhr.status + ": " + xhr.statusText);
        }
    });
}

//onsuccess function for formsubmitt
function SaveSuccessCustomerCategory(data, status) {
    debugger;
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Status) {
        case "OK":
            if ($('#hdnMasterCall').val() == "MSTR") {
                $('#IsUpdate').val('True');
                BindOrReloadCustomerCategoryTable('Reset');
            }
            else if ($('#hdnMasterCall').val() == "OTR") {
                $('.divCustomerCategorySelectList').load('/CustomerCategory/CustomerCategorySelectList?required=');
            }
            MasterAlert("success", JsonResult.Record.Message)
            break;
        case "ERROR":
            MasterAlert("danger", JsonResult.Message)
            break;
        default:
            MasterAlert("danger", JsonResult.Message)
            break;
    }
    $('#divModelMasterPopUp').modal('hide');
}

//Add Plant
function AddPlantMaster(flag) {
    debugger;
    OnServerCallBegin();
    $("#divMasterBody").load("Plant/MasterPartial?masterCode=0", function (responseTxt, statusTxt, xhr) {
        if (statusTxt == "success") {
            OnServerCallComplete();
            $('#lblModelMasterContextLabel').text('Add Plant Information')
            $('#divModelMasterPopUp').modal('show');

            $('#hdnMasterCall').val(flag);
        }
        else {
            console.log("Error: " + xhr.status + ": " + xhr.statusText);
        }
    });
}

//onsuccess function for formsubmitt
function SaveSuccessPlant(data, status) {
    debugger;
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Status) {
        case "OK":
            if ($('#hdnMasterCall').val() == "MSTR") {
                $('#IsUpdate').val('True');
                BindOrReloadPlantTable('Reset');
            }
            else if ($('#hdnMasterCall').val() == "OTR") {
                $('.divPlantSelectList').load('/Plant/PlantSelectList?required=' + $('#hdnPlantRequired').val());
            }
            MasterAlert("success", JsonResult.Record.Message)
            break;
        case "ERROR":
            MasterAlert("danger", JsonResult.Message)
            break;
        default:
            MasterAlert("danger", JsonResult.Message)
            break;
    }
    $('#divModelMasterPopUp').modal('hide');
}


//Add OtherCharge
function AddOtherChargeMaster(flag) {
    debugger;
    OnServerCallBegin();
    $("#divMasterBody").load("OtherCharge/MasterPartial?masterCode=0", function (responseTxt, statusTxt, xhr) {
        if (statusTxt == "success") {
            OnServerCallComplete();
            $('#lblModelMasterContextLabel').text('Add Other Charge Information')
            $('#divModelMasterPopUp').modal('show');

            $('#hdnMasterCall').val(flag);
        }
        else {
            console.log("Error: " + xhr.status + ": " + xhr.statusText);
        }
    });
}

//onsuccess function for formsubmitt
function SaveSuccessOtherCharge(data, status) {
    debugger;
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Status) {
        case "OK":
            if ($('#hdnMasterCall').val() == "MSTR") {
                $('#IsUpdate').val('True');
                BindOrReloadOtherChargeTable('Reset');
            }
            else if ($('#hdnMasterCall').val() == "OTR") {
                $('.divOtherChargeSelectList').load('/OtherCharge/OtherChargeSelectList?required=' + $('#hdnOtherChargeRequired').val());
            }
            MasterAlert("success", JsonResult.Record.Message)
            break;
        case "ERROR":
            MasterAlert("danger", JsonResult.Message)
            break;
        default:
            MasterAlert("danger", JsonResult.Message)
            break;
    }
    $('#divModelMasterPopUp').modal('hide');
}


//Add Country
function AddCountryMaster(flag) {
    debugger;
    OnServerCallBegin();
    $("#divMasterBody").load("Country/MasterPartial?masterCode=0", function (responseTxt, statusTxt, xhr) {
        if (statusTxt == "success") {
            OnServerCallComplete();
            $('#lblModelMasterContextLabel').text('Add Country Information')
            $('#divModelMasterPopUp').modal('show');

            $('#hdnMasterCall').val(flag);
        }
        else {
            console.log("Error: " + xhr.status + ": " + xhr.statusText);
        }
    });
}

//onsuccess function for formsubmitt
function SaveSuccessCountry(data, status) {
    debugger;
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Status) {
        case "OK":
            if ($('#hdnMasterCall').val() == "MSTR") {
                $('#IsUpdate').val('True');
                BindOrReloadCountryTable('Reset');
            }
            else if ($('#hdnMasterCall').val() == "OTR") {
                $('.divCountrySelectList').load('/Country/CountrySelectList?required=' + $('#hdnCountryRequired').val());
                if ($('#CustomerForm')[0])
                    $('.divCountrySelectList').load('/Country/CountrySelectList?required=' + $('#hdnCountryRequired').val(), function () {
                        $('#divCustomerForm #CountryCode').change(function () {
                            debugger;
                            if ($('.divStateSelectList') != undefined) {
                                $('#dropLoad').addClass('fa fa-spinner fa-spin');
                                if (this.value != "") {
                                    $('.divStateSelectList').load('State/StateSelectList?countryCode=' + this.value)
                                }
                                else {
                                    $('.divStateSelectList').empty();
                                    $('.divStateSelectList').append('<span class="form-control newinput"><i id="dropLoad" class="fa fa-spinner"></i></span>');

                                }
                            }
                        });
                    });
            }
            MasterAlert("success", JsonResult.Record.Message)
            break;
        case "ERROR":
            MasterAlert("danger", JsonResult.Message)
            break;
        default:
            MasterAlert("danger", JsonResult.Message)
            break;
    }
    $('#divModelMasterPopUp').modal('hide');
}