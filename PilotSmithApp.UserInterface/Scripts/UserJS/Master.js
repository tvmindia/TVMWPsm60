var EmptyGuid = "00000000-0000-0000-0000-000000000000";
//Add Product Category
function AddProductCategoryMaster(flag)
{
    $("#divMasterBody").load("ProductCategory/MasterPartial?masterCode=0", function () {       
        $('#hdnMasterCall').val(flag);
        $('#lblModelMasterContextLabel').text('Add ProductCategory')
        $('#divModelMasterPopUp').modal('show');
    });

    //GetMasterPartial('ProductCategory', "0");
    //$('#h3ModelMasterContextLabel').text('Add ProductCategory')
   
}

//onsuccess function for formsubmitt
function SaveSuccessProductCategory(data, status)
{
    debugger;
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Status)
    {
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


    $("#divMasterBody").load("ProductSpecification/MasterPartial?masterCode=0", function () {
        $('#hdnMasterCall').val(flag);
        $('#lblModelMasterContextLabel').text('Add Product Specification')
        $('#divModelMasterPopUp').modal('show');
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
    $("#divMasterBody").load("State/MasterPartial?masterCode=0", function () {
        $('#lblModelMasterContextLabel').text('Add State Information')
        $('#divModelMasterPopUp').modal('show');
        $('#hdnMasterCall').val(flag);
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
    $("#divMasterBody").load("District/MasterPartial?masterCode=0", function () {
        $('#lblModelMasterContextLabel').text('Add District Information')
        $('#divModelMasterPopUp').modal('show');
        $('#hdnMasterCall').val(flag);
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
    $("#divMasterBody").load("Area/MasterPartial?masterCode=0", function () {
        $('#lblModelMasterContextLabel').text('Add Area Information')
        $('#divModelMasterPopUp').modal('show');
        $('#hdnMasterCall').val(flag);
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

        $("#divMasterBody").load("Product/MasterPartial?masterCode=" + EmptyGuid, function () {
        $('#hdnMasterCall').val(flag);
        $('#lblModelMasterContextLabel').text('Add Product')
        $('#divModelMasterPopUp').modal('show');
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
                $('.divProductSelectList').load('/Product/ProductSelectList?required=');
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

    $("#divMasterBody").load("Company/MasterPartial?masterCode=" + EmptyGuid, function () {
        $('#hdnMasterCall').val(flag);
        $('#lblModelMasterContextLabel').text('Add Company')
        $('#divModelMasterPopUp').modal('show');
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

    $("#divMasterBody").load("ProductModel/MasterPartial?masterCode=" + EmptyGuid, function () {
        $('#hdnMasterCall').val(flag);
        $('#lblModelMasterContextLabel').text('Add Product Model')
        $('#divModelMasterPopUp').modal('show');
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
                $('.divProductModelSelectList').load('/ProductModel/ProductModelSelectList?required=');
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
    $("#divMasterBody").load("Employee/MasterPartial?masterCode=" + EmptyGuid, function () {
        $('#lblModelMasterContextLabel').text('Add Employee Information')
        $('#divModelMasterPopUp').modal('show');
        $('#hdnMasterCall').val(flag);
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
                $('.divEmployeeSelectList').load('/Employee/EmployeeSelectList?required=');
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
    $("#divMasterBody").load("DynamicUI/PopUpUnderConstruction", function () {
        $('#lblModelMasterContextLabel').text('Add Branch Information')
        $('#divModelMasterPopUp').modal('show');
        $('#hdnMasterCall').val(flag);
    });
    //$("#divMasterBody").load("Employee/MasterPartial?masterCode=" + EmptyGuid, function () {
    //    $('#lblModelMasterContextLabel').text('Add Employee Information')
    //    $('#divModelMasterPopUp').modal('show');
    //    $('#hdnMasterCall').val(flag);
    //});
}
//========================================================================================================
function AddReferredByMaster()
{
    $("#divMasterBody").load("DynamicUI/PopUpUnderConstruction", function () {
        $('#lblModelMasterContextLabel').text('Add Reference Person Information')
        $('#divModelMasterPopUp').modal('show');
        $('#hdnMasterCall').val(flag);
    });
}
//================================================================================================
function AddDocumentStatusMaster()
{
    $("#divMasterBody").load("DynamicUI/PopUpUnderConstruction", function () {
        $('#lblModelMasterContextLabel').text('Add Document Status Information')
        $('#divModelMasterPopUp').modal('show');
        $('#hdnMasterCall').val(flag);
    });
}
//==========================================================================================================
//Add Customer master
function AddCustomerMaster(flag) {
    $("#divMasterBody").load("Customer/AddCustomerPartial", function () {
        $('#lblModelMasterContextLabel').text('Add Customer Information')
        $('#divModelMasterPopUp').modal('show');
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

    $("#divMasterBody").load("Approver/MasterPartial?masterCode=" + EmptyGuid, function () {
        $('#hdnMasterCall').val(flag);
        $('#lblModelMasterContextLabel').text('Add Approver')
        $('#divModelMasterPopUp').modal('show');
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
    $("#divMasterBody").load("ReferencePerson/MasterPartial?masterCode=0", function () {
        $('#lblModelMasterContextLabel').text('Add Reference Person Information')
        $('.modal-dialog').attr('style', 'min-width:60%')
        $('#divModelMasterPopUp').modal('show');
        $('#hdnMasterCall').val(flag);
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
                $('.divAreaSelectList').load('/Area/ReferencePersonSelectList?required=');
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
    $("#divMasterBody").load("PaymentTerm/MasterPartial?masterCode=", function () {
        $('#lblModelMasterContextLabel').text('Add Payment Term Information')
        $('#divModelMasterPopUp').modal('show');

        $('#hdnMasterCall').val(flag);
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
    $("#divMasterBody").load("TaxType/MasterPartial?masterCode=0", function () {
        $('#lblModelMasterContextLabel').text('Add Tax Type Information')
        $('#divModelMasterPopUp').modal('show');

        $('#hdnMasterCall').val(flag);
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

