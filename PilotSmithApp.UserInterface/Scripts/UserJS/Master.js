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
                $('.divProductCategorySelectList').load('/ProductCategory/ProductCategorySelectList?required=');
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
                $('.divProductSpecificationSelectList').load('/ProductSpecification/ProductSpecificationSelectList?required=');
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
                $('.divStateSelectList').load('/State/StatSelectList?required=');
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
                $('.divDistrictSelectList').load('/District/DistrictSelectList?required=');
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
                $('.divAreaSelectList').load('/Area/AreaSelectList?required=');
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
                $('.divCompanySelectList').load('/Company/CompanySelectList?required=');
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
                    $('.divEmployeeSelectList').load('/Employee/EmployeeSelectList?required=');
                if ($(".divResponsiblePersonSelectList")[0])
                    $('.divResponsiblePersonSelectList').load('/Employee/ResponsiblePersonSelectList?required=');
                if ($(".divAttendedBySelectList")[0])
                    $('.divAttendedBySelectList').load('/Employee/AttendedBySelectList?required=');
                if ($(".divServicedBySelectList")[0])
                    $('.divServicedBySelectList').load('/Employee/ServicedBySelectList?required=');
                if ($(".divPreparedBySelectList")[0])
                    $('.divPreparedBySelectList').load('/Employee/PreparedBySelectList?required=');
                if ($(".divQCBySelectList")[0])
                    $('.divQCBySelectList').load('/Employee/QCBySelectList?required=');
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
                $('.divReferredByCodeSelectList').load('/ReferencePerson/ReferencePersonSelectList?required=');
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
                $('.divPaymentTermSelectList').load('/PaymentTerm/PaymentTermSelectList?required=');
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
                $('.divTaxTypeSelectList').load('/TaxType/TaxTypeSelectList?required=');
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
                $('.divPlantSelectList').load('/Plant/PlantSelectList?required=');
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
                $('.divOtherChargeSelectList').load('/OtherCharge/OtherChargeSelectList?required=');
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


