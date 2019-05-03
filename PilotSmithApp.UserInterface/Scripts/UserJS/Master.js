var EmptyGuid = "00000000-0000-0000-0000-000000000000";
var _parentFormID = "";
//Add Product Category
function AddProductCategoryMaster(flag) {
    OnServerCallBegin();
    $("#divMasterBody4").load("ProductCategory/MasterPartial?masterCode=0", function (responseTxt, statusTxt, xhr) {
        if (statusTxt == "success") {
            OnServerCallComplete();
            $('#hdnMasterCall4').val(flag);
            $('#lblModelMasterContextLabel4').text('Add Product Category')
            $('#divModelMasterPopUp4').modal('show');
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
            if ($('#hdnMasterCall4').val() == "MSTR") {
                $('#IsUpdate').val('True');
                BindOrReloadProductCategoryTable('Reset');
            }
            else if ($('#hdnMasterCall4').val() == "OTR") {
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
    $('#divModelMasterPopUp4').modal('hide');
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
    debugger;
    //To specify the current master form is #FormState when select list binding
    _parentFormID = "FormState";
    //when close button clicked
    $('#divModelMasterPopUp3 .close').click(function () {
        //Clearing the select lists so that they're not taken in select list rebind in .each(function() { }) given below in savesuccess
        $("#divMasterBody3").html('');
        if ($('#FormDistrict')[0]) {
            _parentFormID = "FormDistrict";
        } else if ($('#FormArea')[0]) {
            _parentFormID = "FormArea";
        } else {
            if ($('#CustomerForm')[0]) {
                _parentFormID = "CustomerForm";
            } else {
                _parentFormID = $('#AreaCode').closest('form').attr('id');
            }
        }
        $('#' + _parentFormID + '#CountryCode').val($('#' + _parentFormID + '#hdnCountryCode').val()).trigger('change');

    });

    OnServerCallBegin();
    $("#divMasterBody3").load("State/MasterPartial?masterCode=0", function (responseTxt, statusTxt, xhr) {
        debugger;
        if (statusTxt == "success") {
            OnServerCallComplete();
            $('#lblModelMasterContextLabel3').text('Add State Information')
            $('#divModelMasterPopUp3').modal('show');
            $('#hdnMasterCall3').val(flag);
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
            if ($('#hdnMasterCall3').val() == "MSTR") {
                $('#IsUpdate').val('True');
                BindOrReloadStateTable('Reset');
            }
            else if ($('#hdnMasterCall3').val() == "OTR") {
                //Clearing the select lists so that they're not taken in select list rebind in .each(function() { }) given below
                $("#divMasterBody3").html('');
                //to specifically select each select lists of state
                $('.divStateSelectList').each(function () {
                    debugger;
                    //to set parent Form ID for rebind
                    _parentFormID = $(this).closest('form').attr('id');//.closest('form') returns closest form from parents or children and attr('id') return its id
                    var parent = "#" + $(this).closest('form').attr('id') + " ";
                    var countryCode = $(parent + ' #CountryCode').val();
                    $(this).load('/State/StateSelectList?required=' + $('#hdnStateRequired').val() + '&countryCode=' + countryCode
                        , function () {
                            debugger;

                            $(parent + '#StateCode').val($(parent + ' #hdnStateCode').val());
                            $(parent + '#StateCode').select2({
                                dropdownParent: $(parent + '.divCountrySelectList')
                            });
                            $('.select2').addClass('form-control newinput');
                            //applicable only for select list in #CustomerForm
                            if ($('CustomerForm')[0]) {
                                $('#CustomerForm #StateCode').change(function () {
                                    if (this.value !== "")
                                        StateCodeOnChange();
                                });
                            }
                            if ($('FormCustomerMaster')[0]) {
                                $('#FormCustomerMaster #StateCode').change(function () {
                                    if (this.value !== "")
                                        StateCodeOnChange();
                                });
                            }
                    });
                });
                $('#' + _parentFormID + '#CountryCode').val($('#' + _parentFormID + '#hdnCountryCode').val()).trigger('change');
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
    $('#divModelMasterPopUp3').modal('hide');
}

//Add District
function AddDistrictMaster(flag) {
    debugger;
    //--- info on what the below code does is given in AddStateMaster
    _parentFormID = "FormDistrict";
    $('#divModelMasterPopUp2 .close').click(function () {
        $("#divMasterBody2").html('');
        if ($('#FormArea')[0]) {
            _parentFormID = "FormArea";
        } else {
            if ($('#CustomerForm')[0]) {
                _parentFormID = "CustomerForm";
            } else {
                _parentFormID = $('#AreaCode').closest('form').attr('id');
            }
        }
        $('#' + _parentFormID + '#CountryCode').val($('#' + _parentFormID + '#hdnCountryCode').val()).trigger('change');
    });
    //---

    OnServerCallBegin();
    $("#divMasterBody2").load("District/MasterPartial?masterCode=0", function (responseTxt, statusTxt, xhr) {
        if (statusTxt == "success") {
            OnServerCallComplete();
            $('#lblModelMasterContextLabel2').text('Add District Information')
            $('#divModelMasterPopUp2').modal('show');
            $('#hdnMasterCall2').val(flag);
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
            if ($('#hdnMasterCall2').val() == "MSTR") {
                $('#IsUpdate').val('True');
                BindOrReloadDistrictTable('Reset');
            }
            else if ($('#hdnMasterCall2').val() == "OTR") {

                $("#divMasterBody2").html('');
                $('.divDistrictSelectList').each(function () {
                    debugger;
                    _parentFormID = $(this).closest('form').attr('id');
                    var parent = "#" + $(this).closest('form').attr('id') + " ";

                    var countryCode = $(parent + '#CountryCode').val();
                    var stateCode = $(parent + '#StateCode').val();
                    $(this).load('/District/DistrictSelectList?required=' + $('#hdnDistrictRequired').val()
                        + '&stateCode=' + stateCode + '&countryCode=' + countryCode
                        , function () {
                            debugger;
                            $(parent + '#DistrictCode').val($(parent + '#hdnDistrictCode').val());
                            $(parent + '#DistrictCode').select2({
                                dropdownParent: $(parent + '.divDistrictSelectList')
                            });
                            $('.select2').addClass('form-control newinput');
                            if ($('CustomerForm')[0]) {
                                $('#CustomerForm #DistrictCode').change(function () {
                                if (this.value !== "")
                                    DistrictCodeOnChange();
                            });
                            }
                            if ($('FormCustomerMaster')[0]) {
                                $('#FormCustomerMaster #DistrictCode').change(function () {
                                    if (this.value !== "")
                                        DistrictCodeOnChange();
                                });
                            }
                        });
                });
                $('#' + _parentFormID + '#CountryCode').val($('#' + _parentFormID + '#hdnCountryCode').val()).trigger('change');

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
    $('#divModelMasterPopUp2').modal('hide');
}

//Add Area
function AddAreaMaster(flag) {
    debugger;
    _parentFormID = "FormArea";
    $('#divModelMasterPopUp1 .close').click(function () {
        $("#divMasterBody1").html('');
        if ($('#CustomerForm')[0]) {
            _parentFormID = "CustomerForm";
        } else {
            _parentFormID = $('#AreaCode').closest('form').attr('id');
        }
        $('#' + _parentFormID + '#CountryCode').val($('#' + _parentFormID + '#hdnCountryCode').val()).trigger('change');
    });

    OnServerCallBegin();
    $("#divMasterBody1").load("Area/MasterPartial?masterCode=0", function (responseTxt, statusTxt, xhr) {
        if (statusTxt == "success") {
            OnServerCallComplete();
            $('#lblModelMasterContextLabel1').text('Add Area Information')
            $('#divModelMasterPopUp1').modal('show');
            $('#hdnMasterCall1').val(flag);
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
            if ($('#hdnMasterCall1').val() == "MSTR") {
                $('#IsUpdate').val('True');
                BindOrReloadAreaTable('Reset');
            }
            else if ($('#hdnMasterCall1').val() == "OTR") {
                $("#divMasterBody1").html('');
                if ($('#CustomerForm')[0]) {
                    _parentFormID = "CustomerForm";
                    var countryCode = $('#CustomerForm #CountryCode').val();
                    var stateCode = $('#CustomerForm #StateCode').val();
                    var districtCode = $('#CustomerForm #DistrictCode').val();
                    $('#CustomerForm .divAreaSelectList').load('/Area/AreaSelectList?required=' + $('#hdnAreaRequired').val() +
                        '&districtCode=' + districtCode + '&stateCode=' + stateCode + '&countryCode=' + countryCode
                        , function () {
                            debugger;
                            $('#CustomerForm #AreaCode').change(function () {
                                if (this.value !== "")
                                    AreaCodeOnChange();
                            });
                        });
                }
                else if ($('#FormCustomerMaster')[0]) {
                    _parentFormID = "FormCustomerMaster";
                    var countryCode = $('#FormCustomerMaster #CountryCode').val();
                    var stateCode = $('#FormCustomerMaster #StateCode').val();
                    var districtCode = $('#FormCustomerMaster #DistrictCode').val();
                    $('#FormCustomerMaster .divAreaSelectList').load('/Area/AreaSelectList?required=' + $('#hdnAreaRequired').val() +
                        '&districtCode=' + districtCode + '&stateCode=' + stateCode + '&countryCode=' + countryCode
                        , function () {
                            debugger;
                            $('#FormCustomerMaster #AreaCode').change(function () {
                                if (this.value !== "")
                                    AreaCodeOnChange();
                            });
                        });
                }
                else {
                    $('.divAreaSelectList').load('/Area/AreaSelectList?required=' + $('#hdnAreaRequired').val());
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
    $('#divModelMasterPopUp1').modal('hide');
}

//Add Product
function AddProductMaster(flag) {
    debugger;
    OnServerCallBegin();
    $("#divMasterBody3").load("Product/MasterPartial?masterCode=" + EmptyGuid, function (responseTxt, statusTxt, xhr) {
        if (statusTxt == "success") {
            OnServerCallComplete();
            $('#hdnMasterCall3').val(flag);
            $('#lblModelMasterContextLabel3').text('Add Product')
            $('#divModelMasterPopUp3').modal('show');
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
            if ($('#hdnMasterCall3').val() == "MSTR") {
                $('#IsUpdate').val('True');
                BindOrReloadProductTable('Reset');
            }
            else if ($('#hdnMasterCall3').val() == "OTR") {
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
    $('#divModelMasterPopUp3').modal('hide');
}

//Add Company
function AddCompanyMaster(flag) {
    debugger;
    OnServerCallBegin();
    $("#divMasterBody4").load("Company/MasterPartial?masterCode=" + EmptyGuid, function (responseTxt, statusTxt, xhr) {
        if (statusTxt == "success") {
            OnServerCallComplete();
            $('#hdnMasterCall4').val(flag);
            $('#lblModelMasterContextLabel4').text('Add Company')
            $('#divModelMasterPopUp4').modal('show');
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
            if ($('#hdnMasterCall4').val() == "MSTR") {
                $('#IsUpdate').val('True');
                BindOrReloadCompanyTable('Reset');
            }
            else if ($('#hdnMasterCall4').val() == "OTR") {
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
    $('#divModelMasterPopUp4').modal('hide');
}

//Add Product Model
function AddProductModelMaster(flag) {
    debugger;
    OnServerCallBegin();
    $("#divMasterBody2 .close").click(function () {
        _parentFormID = "";
    });
    $("#divMasterBody2").load("ProductModel/MasterPartial?masterCode=" + EmptyGuid, function (responseTxt, statusTxt, xhr) {
        if (statusTxt == "success") {
            OnServerCallComplete();
            $('#hdnMasterCall2').val(flag);
            $('#lblModelMasterContextLabel2').text('Add Product Model')
            //if(flag=="OTR")
            //$('#divModelMasterPopUp2 #divimageUpload').hide();
            $('#divModelMasterPopUp2').modal('show');
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
            if ($('#hdnMasterCall2').val() == "MSTR") {
                $('#IsUpdate').val('True');
                BindOrReloadProductModelTable('Reset');
            }
            else if ($('#hdnMasterCall2').val() == "OTR") {
                _parentFormID = "";
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
    $('#divModelMasterPopUp2').modal('hide');
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
                    $('.divResponsiblePersonSelectList').load('/Employee/ResponsiblePersonSelectList?required=' + $('hdnResponsiblePersonRequired').val());
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
                if ($('#divModelPopServiceCall')[0]) {
                    $('.divTaxTypeSelectList').load('/TaxType/TaxTypeSelectList?required=' + $('#hdnTaxTypeRequired').val(), function () {
                        $('#divModelPopCallCharges .CalculateGST').change(function () {
                            debugger;
                            var ChargeAmount = parseFloat($('#divModelPopCallCharges #ChargeAmount').val() != "" ? $('#divModelPopCallCharges #ChargeAmount').val() : 0);
                            var CGST = parseFloat($('#divModelPopCallCharges #TaxTypeCode').val() != "" ? $('#divModelPopCallCharges #TaxTypeCode').val().split('|')[1].split(',')[0].split('-')[1] : 0);
                            var SGST = parseFloat($('#divModelPopCallCharges #TaxTypeCode').val() != "" ? $('#divModelPopCallCharges #TaxTypeCode').val().split('|')[1].split(',')[1].split('-')[1] : 0);
                            var IGST = parseFloat($('#divModelPopCallCharges #TaxTypeCode').val() != "" ? $('#divModelPopCallCharges #TaxTypeCode').val().split('|')[1].split(',')[2].split('-')[1] : 0);
                            $('#divModelPopCallCharges #hdnCGSTPerc').val(CGST);
                            $('#divModelPopCallCharges #hdnSGSTPerc').val(SGST);
                            $('#divModelPopCallCharges #hdnIGSTPerc').val(IGST);
                            $('#divModelPopCallCharges #CGSTPerc').val(ChargeAmount * CGST / 100);
                            $('#divModelPopCallCharges #SGSTPerc').val(ChargeAmount * SGST / 100);
                            $('#divModelPopCallCharges #IGSTPerc').val(ChargeAmount * IGST / 100);
                        });
                    });
                }
                if ($('#divModelPopProformaInvoice')[0]) {
                    if ($('.divOtherChargeSelectList')[0])
                        $('.divTaxTypeSelectList').load('/TaxType/TaxTypeSelectList?required=' + $('#hdnTaxTypeRequired').val(), function () {
                            $('#divModelPopProformaInvoice .CalculateGST').change(function () {
                                debugger;
                                var CGST = 0, SGST = 0, IGST = 0;
                                var ChargeAmount = parseFloat($('#divModelPopProformaInvoice #ChargeAmount').val() != "" ? $('#divModelPopProformaInvoice #ChargeAmount').val() : 0);
                                if ($('#divModelPopProformaInvoice #TaxTypeCode').val() != null) {
                                    CGST = parseFloat($('#divModelPopProformaInvoice #TaxTypeCode').val() != "" ? $('#divModelPopProformaInvoice #TaxTypeCode').val().split('|')[1].split(',')[0].split('-')[1] : 0);
                                    SGST = parseFloat($('#divModelPopProformaInvoice #TaxTypeCode').val() != "" ? $('#divModelPopProformaInvoice #TaxTypeCode').val().split('|')[1].split(',')[1].split('-')[1] : 0);
                                    IGST = parseFloat($('#divModelPopProformaInvoice #TaxTypeCode').val() != "" ? $('#divModelPopProformaInvoice #TaxTypeCode').val().split('|')[1].split(',')[2].split('-')[1] : 0);
                                }
                                var AddlTaxPerc = parseFloat($('#divModelPopProformaInvoice #AddlTaxPerc').val() != "" ? $('#divModelPopProformaInvoice #AddlTaxPerc').val() : 0);
                                $('#divModelPopProformaInvoice #hdnCGSTPerc').val(CGST);
                                $('#divModelPopProformaInvoice #hdnSGSTPerc').val(SGST);
                                $('#divModelPopProformaInvoice #hdnIGSTPerc').val(IGST);
                                $('#divModelPopProformaInvoice #hdnAddlTaxPerc').val(AddlTaxPerc);
                                $('#divModelPopProformaInvoice #CGSTPerc').val(ChargeAmount * CGST / 100);
                                $('#divModelPopProformaInvoice #SGSTPerc').val(ChargeAmount * SGST / 100);
                                $('#divModelPopProformaInvoice #IGSTPerc').val(ChargeAmount * IGST / 100);
                                var TaxAmount = parseFloat($('#divModelPopProformaInvoice #CGSTPerc').val()) + parseFloat($('#divModelPopProformaInvoice #SGSTPerc').val()) + parseFloat($('#divModelPopProformaInvoice #IGSTPerc').val())
                                $('#divModelPopProformaInvoice #AddlTaxAmt').val(TaxAmount * AddlTaxPerc / 100);
                                $('#divModelPopProformaInvoice #hdnAddlTaxAmt').val(TaxAmount * AddlTaxPerc / 100);
                            });
                        });
                    else
                        $('.divTaxTypeSelectList').load('/TaxType/TaxTypeSelectList?required=' + $('#hdnTaxTypeRequired').val(), function () {
                            $('#divModelPopProformaInvoice .CalculateGST').change(function () {
                                debugger;
                                var CGST = 0, SGST = 0, IGST = 0;
                                var qty = parseInt($('#divModelPopProformaInvoice #Qty').val() != "" ? $('#divModelPopProformaInvoice #Qty').val() : 0);
                                var rate = parseFloat($('#divModelPopProformaInvoice #Rate').val() != "" ? $('#divModelPopProformaInvoice #Rate').val() : 0);
                                var discount = parseFloat($('#divModelPopProformaInvoice #Discount').val() != "" ? $('#divModelPopProformaInvoice #Discount').val() : 0);
                                if ($('#divModelPopProformaInvoice #TaxTypeCode').val() != null) {
                                    CGST = parseFloat($('#divModelPopProformaInvoice #TaxTypeCode').val() != "" ? $('#divModelPopProformaInvoice #TaxTypeCode').val().split('|')[1].split(',')[0].split('-')[1] : 0);
                                    SGST = parseFloat($('#divModelPopProformaInvoice #TaxTypeCode').val() != "" ? $('#divModelPopProformaInvoice #TaxTypeCode').val().split('|')[1].split(',')[1].split('-')[1] : 0);
                                    IGST = parseFloat($('#divModelPopProformaInvoice #TaxTypeCode').val() != "" ? $('#divModelPopProformaInvoice #TaxTypeCode').val().split('|')[1].split(',')[2].split('-')[1] : 0);
                                }
                                var CessPerc = parseFloat($('#divModelPopProformaInvoice #CessPerc').val() != "" ? $('#divModelPopProformaInvoice #CessPerc').val() : 0);
                                var taxableAmount = (rate * qty) - discount;
                                $('#divModelPopProformaInvoice #hdnCGSTPerc').val(CGST);
                                $('#divModelPopProformaInvoice #hdnSGSTPerc').val(SGST);
                                $('#divModelPopProformaInvoice #hdnIGSTPerc').val(IGST);
                                $('#divModelPopProformaInvoice #CGSTPerc').val(taxableAmount * CGST / 100);
                                $('#divModelPopProformaInvoice #SGSTPerc').val(taxableAmount * SGST / 100);
                                $('#divModelPopProformaInvoice #IGSTPerc').val(taxableAmount * IGST / 100);
                                var TaxAmount = parseFloat(taxableAmount * CGST / 100) + parseFloat(taxableAmount * SGST / 100) + parseFloat(taxableAmount * IGST / 100)
                                $('#divModelPopProformaInvoice #CessAmt').val((TaxAmount * CessPerc) / 100);
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
    $('#divModelMasterPopUp4 .close').click(function () {
        if ($('#FormState')[0]) {
            _parentFormID = "FormState";
        } else if ($('#FormDistrict')[0]) {
            _parentFormID = "FormDistrict";
        } else if ($('#FormArea')[0]) {
            _parentFormID = "FormArea";
        } else {
            if ($('#CustomerForm')[0]) {
                _parentFormID = "CustomerForm";
            } else {
                _parentFormID = $('#AreaCode').closest('form').attr('id');
            }
        }
        $("#divMasterBody4").html('');
        $('#' + _parentFormID + ' #CountryCode').load('/Country/CountrySelectList?required=' + $('#hdnCountryRequired').val()).trigger('change');
    });

    OnServerCallBegin();
    $("#divMasterBody4").load("Country/MasterPartial?masterCode=0", function (responseTxt, statusTxt, xhr) {
        if (statusTxt == "success") {
            OnServerCallComplete();
            $('#lblModelMasterContextLabel4').text('Add Country Information')
            $('#divModelMasterPopUp4').modal('show');

            $('#hdnMasterCall4').val(flag);
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
            if ($('#hdnMasterCall4').val() == "MSTR") {
                $('#IsUpdate').val('True');
                BindOrReloadCountryTable('Reset');
            }
            else if ($('#hdnMasterCall4').val() == "OTR") {
                $('.divCountrySelectList').each(function () {
                    debugger;
                    _parentFormID = $(this).closest('form').attr('id');
                    var parent = '#' + $(this).closest('form').attr('id') + " ";
                    $(this).load('/Country/CountrySelectList?required=' + $('#hdnCountryRequired').val(), function () {
                        debugger;
                        $(parent + '#CountryCode').val($(parent + '#hdnCountryCode').val());
                        $(parent + '#CountryCode').select2({
                            dropdownParent: $(parent + '.divCountrySelectList')
                        });
                        $('.select2').addClass('form-control newinput');
                    });
                });
                //if ($('#CustomerForm')[0])
                //    $('.divCountrySelectList').load('/Country/CountrySelectList?required=' + $('#hdnCountryRequired').val(), function () {
                //        $('#divCustomerForm #CountryCode').change(function () {
                //            debugger;
                //            if ($('.divStateSelectList') != undefined) {
                //                $('#dropLoad').addClass('fa fa-spinner fa-spin');
                //                if (this.value != "") {
                //                    $('.divStateSelectList').load('State/StateSelectList?countryCode=' + this.value)
                //                }
                //                else {
                //                    $('.divStateSelectList').empty();
                //                    $('.divStateSelectList').append('<span class="form-control newinput"><i id="dropLoad" class="fa fa-spinner"></i></span>');

                //                }
                //            }
                //        });
                //    });

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
    $('#divModelMasterPopUp4').modal('hide');
}

//Add Bank
function AddBankMaster(flag) {
    debugger;
    OnServerCallBegin();
    $("#divMasterBody").load("Bank/MasterPartial?masterCode=0", function (responseTxt, statusTxt, xhr) {
        if (statusTxt == "success") {
            OnServerCallComplete();
            $('#lblModelMasterContextLabel').text('Add Bank Information')
            $('#divModelMasterPopUp').modal('show');

            $('#hdnMasterCall').val(flag);
        }
        else {
            console.log("Error: " + xhr.status + ": " + xhr.statusText);
        }
    });
}

//onsuccess function for formsubmitt
function SaveSuccessBank(data, status) {
    debugger;
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Status) {
        case "OK":
            if ($('#hdnMasterCall').val() == "MSTR") {
                $('#IsUpdate').val('True');
                BindOrReloadBankTable('Reset');
            }
            else if ($('#hdnMasterCall').val() == "OTR") {
                $('.divBankSelectList').load('/Bank/BankSelectList?required=' + $('#hdnBankRequired').val());
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

//Add Spare
function AddSpareMaster(flag) {
    debugger;
    OnServerCallBegin();
    $("#divMasterBody").load("Spare/MasterPartial?masterCode=" + EmptyGuid, function (responseTxt, statusTxt, xhr) {
        if (statusTxt == "success") {
            OnServerCallComplete();
            $('#hdnMasterCall').val(flag);
            $('#lblModelMasterContextLabel').text('Add Spare')
            $('#divModelMasterPopUp').modal('show');
        }
        else {
            console.log("Error: " + xhr.status + ": " + xhr.statusText);
        }
    });

}

//onsuccess function for formsubmitt
function SaveSuccessSpare(data, status) {
    debugger;
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Status) {
        case "OK":
            if ($('#hdnMasterCall').val() == "MSTR") {
                $('#IsUpdate').val('True');
                BindOrReloadSpareTable('Reset');
            }
            else if ($('#hdnMasterCall').val() == "OTR") {
                $('.divSpareSelectList').load('/Spare/SpareSelectList?required=' + $('#hdnSpareRequired').val());
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

//Add SysSetting
function AddSysSettingMaster(flag) {
    debugger;
    OnServerCallBegin();
    $("#divMasterBody").load("SysSetting/MasterPartial?masterCode=" + EmptyGuid, function (responseTxt, statusTxt, xhr) {
        if (statusTxt == "success") {
            OnServerCallComplete();
            $('#hdnMasterCall').val(flag);
            $('#lblModelMasterContextLabel').text('Add Tally Setting')
            $('#divModelMasterPopUp').modal('show');
        }
        else {
            console.log("Error: " + xhr.status + ": " + xhr.statusText);
        }
    });

}

//onsuccess function for formsubmit
function SaveSuccessSysSetting(data, status) {
    debugger;
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Status) {
        case "OK":
            if ($('#hdnMasterCall').val() == "MSTR") {
                $('#IsUpdate').val('True');
                BindOrReloadSysSettingTable('Reset');
            }
            //else if ($('#hdnMasterCall').val() == "OTR") {
            //    $('.divSysSettingSelectList').load('/SysSetting/SysSettingSelectList?required=' + $('#hdnSysSettingRequired').val());
            //}
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
//image upload
function imageUpload() {
    debugger;
    if (window.FormData !== undefined) {
        debugger;
        var fileUpload = $("#fileUpload").get(0);
        var files = fileUpload.files;
        if (files.length > 0) {
            // Create FormData object
            var fileData = new FormData();
            // Looping over all files and add it to FormData object
            for (var i = 0; i < files.length; i++) {
                fileData.append(files[i].name, files[i]);
            }

            $.ajax({
                url: '/' + 'ProductModel' + '/UploadImages',
                type: "POST",
                contentType: false, // Not to set any content header
                processData: false, // Not to process data
                data: fileData,
                success: function (result) {
                    debugger;
                    result = JSON.parse(result)
                    if (result.Result == "OK") {
                        debugger;
                        $('#ImageURL').val(result.Record.AttachmentURL);
                        $('#FormProductModel').submit();
                    }
                },
                error: function (err) {
                    alert(err.statusText);
                }
            });
        }
        else {
            $('#FormProductModel').submit();
        }
    }
}