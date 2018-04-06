//Add Product Category
function AddProductCategoryMaster(flag)
{
    GetMasterPartial('ProductCategory', "0");
    $('#h3ModelMasterContextLabel').text('Add ProductCategory')
    $('#divModelMasterPopUp').modal('show');
    $('#hdnMasterCall').val(flag);
}

//onsuccess function for formsubmitt
function SaveSuccessProductCategory(data, status)
{
    debugger;
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Result)
    {
        case "OK":
            if ($('#hdnMasterCall').val() == "MSTR") {
                $('#IsUpdate').val('True');
                BindOrReloadProductCategoryTable('Reset');
            }
            else if ($('#hdnMasterCall').val() == "OTR") {
                $('#divProductCategory').load('/ProductCategory/ProductCategoryDropDown');
            }
            MasterAlert("success", JsonResult.Records.Message)
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
    GetMasterPartial('ProductSpecification', '0');
    $('#h3ModelMasterContextLabel').text('Add ProductSpecification')
    $('#divModelMasterPopUp').modal('show');
    $('#hdnMasterCall').val(flag);
}

//onsuccess function for formsubmitt
function SaveSuccessProductSpecification(data, status) {
    debugger;
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Result) {
        case "OK":
            if ($('#hdnMasterCall').val() == "MSTR") {
                $('#IsUpdate').val('True');
                BindOrReloadProductSpecificationTable('Reset');
            }
            else if ($('#hdnMasterCall').val() == "OTR") {
                $('#divProductSpecification').load('/ProductSpecification/ProductSpecificationDropDown');
            }
            MasterAlert("success", JsonResult.Records.Message)
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

//Add State
function AddStateMaster(flag) {
    GetMasterPartial('State', '0');
    $('#h3ModelMasterContextLabel').text('Add State')
    $('#divModelMasterPopUp').modal('show');
    $('#hdnMasterCall').val(flag);
}

//onsuccess function for formsubmitt
function SaveSuccessState(data, status) {
    debugger;
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Result) {
        case "OK":
            if ($('#hdnMasterCall').val() == "MSTR") {
                $('#IsUpdate').val('True');
                BindOrReloadStateTable('Reset');
            }
            else if ($('#hdnMasterCall').val() == "OTR") {
                $('#divState').load('/State/StateDropDown');
            }
            MasterAlert("success", JsonResult.Records.Message)
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
    GetMasterPartial('District', '0'); 
    $('#h3ModelMasterContextLabel').text('Add District')
    $('#divModelMasterPopUp').modal('show');
    $('#hdnMasterCall').val(flag);
}

//onsuccess function for formsubmitt
function SaveSuccessDistrict(data, status) {
    debugger;
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Result) {
        case "OK":
            if ($('#hdnMasterCall').val() == "MSTR") {
                $('#IsUpdate').val('True');
                BindOrReloadDistrictTable('Reset');
            }
            else if ($('#hdnMasterCall').val() == "OTR") {
                $('#divDistrict').load('/District/DistrictDropDown');
            }
            MasterAlert("success", JsonResult.Records.Message)
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
    GetMasterPartial('Area', '0');
    $('#h3ModelMasterContextLabel').text('Add Area')
    $('#divModelMasterPopUp').modal('show');
    $('#hdnMasterCall').val(flag);
}

//onsuccess function for formsubmitt
function SaveSuccessArea(data, status) {
    debugger;
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Result) {
        case "OK":
            if ($('#hdnMasterCall').val() == "MSTR") {
                $('#IsUpdate').val('True');
                BindOrReloadAreaTable('Reset');
            }
            else if ($('#hdnMasterCall').val() == "OTR") {
                $('#divArea').load('/Area/AreaDropDown');
            }
            MasterAlert("success", JsonResult.Records.Message)
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
//==========================================================================================================
//Add Customer master
function AddCustomerMaster(flag) {
    $("#divMasterBody").load("Customer/AddCustomerPartial", function () {
        $('#divModelMasterPopUp').modal('show');
    });    
}

//onsuccess function for formsubmitt customer master
function SaveSuccessCustomerMaster(data, status) {
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Status) {
        case "OK":
            $('#divCustomerSelectList').load('/Customer/CustomerSelectList?required=required');
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