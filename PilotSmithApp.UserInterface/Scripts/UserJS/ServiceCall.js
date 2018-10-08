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
        BindOrReloadServiceCallTable('Init');
        $('#tblServiceCall tbody').on('dblclick', 'td', function () {
            EditServiceCall(this);
        });

        if ($('#RedirectToDocument').val() != "") {
            if ($('#RedirectToDocument').val() === _emptyGuid) {
                AddServiceCall();
            }
            else {
                EditRedirectToDocument($('#RedirectToDocument').val());
            }
        }
    }
    catch (e) {
        console.log(e.message);
    }
    $("#AdvAreaCode,#AdvCustomerID,#AdvBranchCode,#AdvDocumentStatusCode,#AdvServicedBy,#AdvAttendedBy,#AdvServiceTypeCode").select2({
        dropdownParent: $(".divboxASearch")
    });

    $('.select2').addClass('form-control newinput');
});

function BindOrReloadServiceCallTable(action) {
    try {
        debugger;
        //creating advancesearch object
        ServiceCallAdvanceSearchViewModel = new Object();
        DataTablePagingViewModel = new Object();
        DataTablePagingViewModel.Length = 0;
        var SearchValue = $('#hdnSearchTerm').val();
        var SearchTerm = $('#SearchTerm').val();
        $('#hdnSearchTerm').val($('#SearchTerm').val());
        //switch case to check the operation
        switch (action) {
            case 'Reset':
                $('#SearchTerm').val('');
                $('.divboxASearch #AdvFromDate').val('');
                $('.divboxASearch #AdvToDate').val('');
                $('.divboxASearch #AdvAreaCode').val('').trigger('change');
                $('.divboxASearch #AdvCustomerID').val('').trigger('change');
                $('.divboxASearch #AdvBranchCode').val('').trigger('change');
                $('.divboxASearch #AdvDocumentStatusCode').val('').trigger('change');
                $('.divboxASearch #AdvServicedBy').val('').trigger('change');
                $('.divboxASearch #AdvAttendedBy').val('').trigger('change');
                $('.divboxASearch #AdvServiceTypeCode').val('').trigger('change');
                break;
            case 'Init':
                $('#SearchTerm').val('');
                $('.divboxASearch #AdvFromDate').val('');
                $('.divboxASearch #AdvToDate').val('');
                $('.divboxASearch #AdvAreaCode').val('');
                $('.divboxASearch #AdvCustomerID').val('');
                $('.divboxASearch #AdvBranchCode').val('');
                $('.divboxASearch #AdvDocumentStatusCode').val('');
                $('.divboxASearch #AdvServicedBy').val('');
                $('.divboxASearch #AdvAttendedBy').val('');
                $('.divboxASearch #AdvServiceTypeCode').val('');
                break;
            case 'Search':
                if ((SearchTerm == SearchValue) && ($('.divboxASearch #AdvFromDate').val() == "") && ($('#AdvToDate').val() == "") && ($('.divboxASearch #AdvAreaCode').val() == "") && ($('.divboxASearch #AdvCustomerID').val() == "") && ($('.divboxASearch #AdvBranchCode').val() == "") && ($('.divboxASearch #AdvDocumentStatusCode').val() == "") && ($('.divboxASearch #AdvAttendedBy').val() == "") && ($('.divboxASearch #AdvServicedBy').val() == "") && ($('.divboxASearch #AdvServiceTypeCode').val() == "")) {
                    return true;
                }
                break;
            case 'Export':
                DataTablePagingViewModel.Length = -1;
                ServiceCallAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
                ServiceCallAdvanceSearchViewModel.SearchTerm = $('#SearchTerm').val() == "" ? null : $('#SearchTerm').val();
                ServiceCallAdvanceSearchViewModel.AdvFromDate = $('.divboxASearch #AdvFromDate').val() == "" ? null : $('.divboxASearch #AdvFromDate').val();
                ServiceCallAdvanceSearchViewModel.AdvToDate = $('.divboxASearch #AdvToDate').val() == "" ? null : $('.divboxASearch #AdvToDate').val();
                ServiceCallAdvanceSearchViewModel.AdvAreaCode = $('.divboxASearch #AdvAreaCode').val() == "" ? null : $('.divboxASearch #AdvAreaCode').val();
                ServiceCallAdvanceSearchViewModel.AdvCustomerID = $('.divboxASearch #AdvCustomerID').val() == "" ? _emptyGuid : $('.divboxASearch #AdvCustomerID').val();
                ServiceCallAdvanceSearchViewModel.AdvBranchCode = $('.divboxASearch #AdvBranchCode').val() == "" ? null : $('.divboxASearch #AdvBranchCode').val();
                ServiceCallAdvanceSearchViewModel.AdvDocumentStatusCode = $('.divboxASearch #AdvDocumentStatusCode').val() == "" ? null : $('.divboxASearch #AdvDocumentStatusCode').val();
                ServiceCallAdvanceSearchViewModel.AdvServicedBy = $('.divboxASearch #AdvServicedBy').val() == "" ? _emptyGuid : $('.divboxASearch #AdvServicedBy').val();
                ServiceCallAdvanceSearchViewModel.AdvAttendedBy = $('.divboxASearch #AdvAttendedBy').val() == "" ? _emptyGuid : $('.divboxASearch #AdvAttendedBy').val();
                ServiceCallAdvanceSearchViewModel.AdvServiceTypeCode = $('.divboxASearch #AdvServiceTypeCode').val() == "" ? null : $('.divboxASearch #AdvServiceTypeCode').val();
                $('#AdvanceSearch').val(JSON.stringify(ServiceCallAdvanceSearchViewModel));
                $('#FormExcelExport').submit();
                return true;
                break;
            default:
                break;
        }
        ServiceCallAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
        ServiceCallAdvanceSearchViewModel.SearchTerm = $('#SearchTerm').val();
        ServiceCallAdvanceSearchViewModel.AdvFromDate = $('.divboxASearch #AdvFromDate').val();
        ServiceCallAdvanceSearchViewModel.AdvToDate = $('.divboxASearch #AdvToDate').val();
        ServiceCallAdvanceSearchViewModel.AdvAreaCode = $('.divboxASearch #AdvAreaCode').val();
        ServiceCallAdvanceSearchViewModel.AdvCustomerID = $('.divboxASearch #AdvCustomerID').val();
        ServiceCallAdvanceSearchViewModel.AdvBranchCode = $('.divboxASearch #AdvBranchCode').val();
        ServiceCallAdvanceSearchViewModel.AdvDocumentStatusCode = $('.divboxASearch #AdvDocumentStatusCode').val();
        ServiceCallAdvanceSearchViewModel.AdvServicedBy = $('.divboxASearch #AdvServicedBy').val();
        ServiceCallAdvanceSearchViewModel.AdvAttendedBy = $('.divboxASearch #AdvAttendedBy').val();
        ServiceCallAdvanceSearchViewModel.AdvServiceTypeCode = $('.divboxASearch #AdvServiceTypeCode').val();
        //apply datatable plugin on ServiceCall table
        _dataTable.ServiceCallList = $('#tblServiceCall').DataTable(
        {
            dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
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
                url: "ServiceCall/GetAllServiceCall/",
                data: { "serviceCallAdvanceSearchVM": ServiceCallAdvanceSearchViewModel },
                type: 'POST'
            },
            pageLength: 8,
            columns: [
               {
                   "data": "ServiceCallNo", render: function (data, type, row) {
                       return data + "</br>" + "<img src='./Content/images/datePicker.png' height='10px'>" + "&nbsp;" + row.ServiceCallDateFormatted;
                   }, "defaultContent": "<i>-</i>"
               },
               {
                   "data": "Customer", render: function (data, type, row) {
                       return "<img src='./Content/images/contact.png' height='10px'>" + "&nbsp;" + (data.ContactPerson == null ? " " : row.Customer.ContactPerson) + "</br>" + "<img src='./Content/images/organisation.png' height='10px'>" + "&nbsp;" + data.CompanyName;
                   }, "defaultContent": "<i>-</i>"
               },
               { "data": "Area.Description", "defaultContent": "<i>-</i>" },
               { "data": "Employee.Name", "defaultContent": "<i>-</i>" },
               {
                   "data": "ServiceDateFormatted", render: function (data, type, row) {
                       return "<img src='./Content/images/contact.png' height='10px'>" + "&nbsp;" + (row.ServicedBy == null ? " " : row.ServicedByName) + "</br>" + "<img src='./Content/images/datePicker.png' height='10px'>" + "&nbsp;" + (data === null ? " " : data);
                   }, "defaultContent": "<i>-</i>"
               },//4
               {
                   "data": "DocumentStatus.Description", render: function (data, type, row) {
                       return "<b>Doc.Status-</b>" + (data == null ? " " : data) + " </br>" + "<b>Branch-</b>" + (row.Branch.Description == null ? " " : row.Branch.Description);
                   }, "defaultContent": "<i>-</i>"
               },
               { "data": "ServiceType.Name", "defaultContent": "<i>-</i>" },
               { "data": null, "orderable": false, "defaultContent": '<a href="#" class="actionLink"  onclick="EditServiceCall(this)" ><i class="fa fa-pencil-square-o" aria-hidden="true"></i></a>' },
            ],
            columnDefs: [
                          { className: "text-left", "targets": [0, 3, 4, 5, 6] },
                          { className: "text-center", "targets": [6] },
            ],
            destroy: true,
            //for performing the import operation after the data loaded
            initComplete: function (settings, json) {
                debugger;
                $('.dataTables_wrapper div.bottom div').addClass('col-md-6');
                $('#tblServiceCall').fadeIn('slow');
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
function ResetServiceCallList() {
    $(".searchicon").removeClass('filterApplied');
    BindOrReloadServiceCallTable('Reset');
}

//function export data to excel
function ExportServiceCallData() {   
    BindOrReloadServiceCallTable('Export');
}

//ApplyFilterThenSearch
function ApplyFilterThenSearch() {
    $(".searchicon").addClass('filterApplied');
    CloseAdvanceSearch();
    BindOrReloadServiceCallTable('Search');
}
//===============Form==============//

//Bind ServiceCallDetail Table
function AddServiceCall() {
    debugger;
    //this will return form body(html)
    OnServerCallBegin();
    $("#divServiceCallForm").load("ServiceCall/ServiceCallForm?id=" + _emptyGuid, function (responseTxt, statusTxt, xhr) {
        $('#lblServiceCallInfo').text("Service Call Information");
        if (statusTxt == "success") {
            OnServerCallComplete();
            openNav();
            ChangeButtonPatchView("ServiceCall", "btnPatchServiceCallNew", "Add");
            BindServiceCallChargeDetailList(_emptyGuid);
            BindServiceCallDetailList(_emptyGuid);
        }
        else {
            console.log("Error: " + xhr.status + ": " + xhr.statusText);
        }
    });
}

function EditServiceCall(this_Obj) {
    OnServerCallBegin();
    var ServiceCall = _dataTable.ServiceCallList.row($(this_Obj).parents('tr')).data();
    $('#lblServiceCallInfo').text(ServiceCall.ServiceCallNo);
    $("#divServiceCallForm").load("ServiceCall/ServiceCallForm?id=" + ServiceCall.ID, function (responseTxt, statusTxt, xhr) {
        if (statusTxt == "success") {
            OnServerCallComplete();
            openNav();
            if ($('#IsDocLocked').val() == "True") {
                ChangeButtonPatchView("ServiceCall", "btnPatchServiceCallNew", "Edit", ServiceCall.ID);
            }
            else {
                ChangeButtonPatchView("ServiceCall", "btnPatchServiceCallNew", "LockDocument",ServiceCall.ID);
            }
            BindServiceCallDetailList(ServiceCall.ID);
            BindServiceCallChargeDetailList(ServiceCall.ID)
            $('#divCustomerBasicInfo').load("Customer/CustomerBasicInfo?ID=" + $('#hdnCustomerID').val());
            clearUploadControl();
            PaintImages(ServiceCall.ID);
            CalculateTotal();
            
        }
        else {
            console.log("Error: " + xhr.status + ": " + xhr.statusText);
        }
    });
}

function ResetServiceCall() {
    $("#divServiceCallForm").load("ServiceCall/ServiceCallForm?id=" + $('#ServiceCallForm #ID').val(), function (responseTxt, statusTxt, xhr) {
        if (statusTxt == "success") {
            $('#lblServiceCallInfo').text(($('#ServiceCallNo').val() !== "") ? $('#ServiceCallNo').val() : "Service Call Information");
            BindServiceCallDetailList($('#ID').val());
            BindServiceCallChargeDetailList($('#ID').val());
            clearUploadControl();
            CalculateTotal();
            $('#divCustomerBasicInfo').load("Customer/CustomerBasicInfo?ID=" + $('#ServiceCallForm #hdnCustomerID').val());
            PaintImages($('#ServiceCallForm #ID').val());
        }
        else {
            console.log("Error: " + xhr.status + ": " + xhr.statusText);
        }

    });
}

function EditServiceCallDetail(this_Obj) {
    try{
        debugger;
        _datatablerowindex = _dataTable.ServiceCallDetailList.row($(this_Obj).parents('tr')).index();
        var serviceCallDetail = _dataTable.ServiceCallDetailList.row($(this_Obj).parents('tr')).data();
        $("#divModelServiceCallPopBody").load("ServiceCall/AddServiceCallDetail?update=true", function () {
            debugger;
            $('#lblModelPopServiceCall').text('Service Call Detail (Product)')
            $('#FormServiceCallDetail #IsUpdate').val('True');
            $('#FormServiceCallDetail #ID').val(serviceCallDetail.ID);
            $("#FormServiceCallDetail #ProductID").val(serviceCallDetail.ProductID)
            $("#FormServiceCallDetail #hdnProductID").val(serviceCallDetail.ProductID)
            $('#spanProductName').text(serviceCallDetail.Product.Code + "-" + serviceCallDetail.Product.Name)
            $('#spanProductModelName').text(serviceCallDetail.ProductModel.Name)
            $('#divProductBasicInfo').load("Product/ProductBasicInfo?ID=" + $('#hdnProductID').val(), function () {
            });

            if ($('#hdnProductID').val() != _emptyGuid) {
                $('.divProductModelSelectList').load("ProductModel/ProductModelSelectList?required=required&productID=" + $('#hdnProductID').val())
            }
            else {
                $('.divProductModelSelectList').empty();
                $('.divProductModelSelectList').append('<span class="form-control newinput"><i id="dropLoad" class="fa fa-spinner"></i></span>');
            }
            $("#FormServiceCallDetail #ProductModelID").val(serviceCallDetail.ProductModelID);
            $("#FormServiceCallDetail #hdnProductModelID").val(serviceCallDetail.ProductModelID);
            if ($('#hdnProductModelID').val() != _emptyGuid) {
                $('#divProductBasicInfo').load("ProductModel/ProductModelBasicInfo?ID=" + $('#hdnProductModelID').val(), function () {
                });
            }
            $('#FormServiceCallDetail #ProductSpec').val(serviceCallDetail.ProductSpec);
            switch (serviceCallDetail.GuaranteeYN) {
                case true:
                    serviceCallDetail.GuaranteeYN = 'True'
                    break;
                case false:
                    serviceCallDetail.GuaranteeYN = 'False'
                    break;
                default:
                    serviceCallDetail.GuaranteeYN = ''
                    break;
            }
            $('#FormServiceCallDetail #GuaranteeYN').val(serviceCallDetail.GuaranteeYN);
            $('#FormServiceCallDetail #ServiceStatusCode').val(serviceCallDetail.ServiceStatusCode);
            $('#FormServiceCallDetail #hdnServiceStatusCode').val(serviceCallDetail.ServiceStatusCode);
            $('#FormServiceCallDetail #InstalledDate').val(serviceCallDetail.InstalledDateFormatted);
            $('#divModelPopServiceCall').modal('show');
        });
    }
    catch (e) {
        console.log(e.message);
    }
} 

function EditServiceCallChargeDetail(this_Obj) {
    try {
        debugger;
        _datatablerowindex = _dataTable.ServiceCallChargeDetailList.row($(this_Obj).parents('tr')).index();
        var serviceCallChargeDetail = _dataTable.ServiceCallChargeDetailList.row($(this_Obj).parents('tr')).data();
        $("#divModelCallChargesPopBody").load("ServiceCall/AddServiceCallCharge?update=true", function () {
            debugger;
            $('#lblModelPopCallCharges').text('Service Call Charges Detail')
            $('#spanOtherCharge').text(serviceCallChargeDetail.OtherCharge.Description)
            $('#FormServiceCallChargeDetail #IsUpdate').val('True');
            $('#FormServiceCallChargeDetail #ID').val(serviceCallChargeDetail.ID);
            $("#FormServiceCallChargeDetail #OtherChargeCode").val(serviceCallChargeDetail.OtherChargeCode);
            $("#FormServiceCallChargeDetail #hdnOtherChargeCode").val(serviceCallChargeDetail.OtherChargeCode);
            $("#FormServiceCallChargeDetail #ChargeAmount").val(serviceCallChargeDetail.ChargeAmount);

            $('#FormServiceCallChargeDetail #TaxTypeCode').val(serviceCallChargeDetail.TaxType.ValueText);
            $('#FormServiceCallChargeDetail #hdnTaxTypeCode').val(serviceCallChargeDetail.TaxType.ValueText);
            $('#FormServiceCallChargeDetail #hdnCGSTPerc').val(serviceCallChargeDetail.CGSTPerc);
            $('#FormServiceCallChargeDetail #hdnSGSTPerc').val(serviceCallChargeDetail.SGSTPerc);
            $('#FormServiceCallChargeDetail #hdnIGSTPerc').val(serviceCallChargeDetail.IGSTPerc);

            var CGSTAmt = (serviceCallChargeDetail.ChargeAmount * parseFloat(serviceCallChargeDetail.CGSTPerc)) / 100;
            var SGSTAmt = (serviceCallChargeDetail.ChargeAmount * parseFloat(serviceCallChargeDetail.SGSTPerc)) / 100;
            var IGSTAmt = (serviceCallChargeDetail.ChargeAmount * parseFloat(serviceCallChargeDetail.IGSTPerc)) / 100;
            $('#FormServiceCallChargeDetail #CGSTPerc').val(CGSTAmt);
            $('#FormServiceCallChargeDetail #SGSTPerc').val(SGSTAmt);
            $('#FormServiceCallChargeDetail #IGSTPerc').val(IGSTAmt);
            $('#FormServiceCallChargeDetail #AddlTaxPerc').val(serviceCallChargeDetail.AddlTaxPerc);
            $('#FormServiceCallChargeDetail #AddlTaxAmt').val(serviceCallChargeDetail.AddlTaxAmt);
            $('#divModelPopCallCharges').modal('show');
        });
    }
    catch (e) {
        console.log(e.message);
    }
}

function ClearCalculatedFields() {
    $('#lblTaxTotal').text('0.00');
    //$('#lblItemTotal').text('0.00');
    $('#lblAddlTaxTotal').text('0.00');
    $('#lblGrandTotal').text('0.00');
    $('#lblOtherChargeAmount').text('0.00');
}

function CalculateTotal() {
    var TaxTotal = 0.00, GrandTotal = 0.00, OtherChargeAmt = 0.00, TotalAddlTax = 0.00;
    var serviceCallChargeDetailList = _dataTable.ServiceCallChargeDetailList.rows().data();
    for (var i = 0; i < serviceCallChargeDetailList.length; i++) {
        var CGST = parseFloat(serviceCallChargeDetailList[i].CGSTPerc != "" ? serviceCallChargeDetailList[i].CGSTPerc : 0);
        var SGST = parseFloat(serviceCallChargeDetailList[i].SGSTPerc != "" ? serviceCallChargeDetailList[i].SGSTPerc : 0);
        var IGST = parseFloat(serviceCallChargeDetailList[i].IGSTPerc != "" ? serviceCallChargeDetailList[i].IGSTPerc : 0);
        var CGSTAmt = parseFloat(serviceCallChargeDetailList[i].ChargeAmount * CGST / 100);
        var SGSTAmt = parseFloat(serviceCallChargeDetailList[i].ChargeAmount * SGST / 100);
        var IGSTAmt = parseFloat(serviceCallChargeDetailList[i].ChargeAmount * IGST / 100);
        var AddlTaxAmt = parseFloat(serviceCallChargeDetailList[i].AddlTaxAmt);
        var GSTAmt = roundoff(parseFloat(CGSTAmt) + parseFloat(SGSTAmt) + parseFloat(IGSTAmt));
        var Total = roundoff(parseFloat(serviceCallChargeDetailList[i].ChargeAmount) + parseFloat(GSTAmt) + parseFloat(AddlTaxAmt));
        GrandTotal = roundoff(parseFloat(GrandTotal) + parseFloat(Total));
        TaxTotal = roundoff(parseFloat(TaxTotal) + parseFloat(GSTAmt));
        TotalAddlTax = roundoff(parseFloat(TotalAddlTax) + parseFloat(AddlTaxAmt));
        OtherChargeAmt = roundoff(parseFloat(OtherChargeAmt) + parseFloat(serviceCallChargeDetailList[i].ChargeAmount))
    }
    $('#lblAddlTaxTotal').text(TotalAddlTax);
    $('#lblTaxTotal').text(TaxTotal);
    $('#lblGrandTotal').text(GrandTotal);
    $('#lblOtherChargeAmount').text(OtherChargeAmt);
}

//======Add ServiceCallDetail======//
function BindServiceCallDetailList(id) {

    _dataTable.ServiceCallDetailList = $('#tblServiceCallDetails').DataTable(
         {
             dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
             order: [],
             searching: false,
             paging: false,
             ordering: false,
             bInfo: false,
             data: id == _emptyGuid ? null : GetServiceCallDetailListByServiceCallID(id),
             language: {
                 search: "_INPUT_",
                 searchPlaceholder: "Search"
             },
             columns: [
             {
                 "data": null, render: function (data, type, row) {
                     debugger;
                     if (row.Product !== undefined && row.Product !== null && row.Product.Code !== "") {
                         return '<div style="width:100%" class="show-popover" data-html="true" data-toggle="popover" data-title="<p align=left>Product Specification" data-content="' + ((row.ProductSpec!==null&&row.ProductSpec!=='')? row.ProductSpec.replace(/"/g, "&quot"):'') + '</p>"/>' + row.Product.Name + "<br/>" + row.ProductModel.Name;
                     }
                     if (row.Spare !== undefined && row.Spare !== null && row.Spare.Code !== "") {
                         return '<span>' + row.Spare.Name + '</span>';
                     }
                     else {
                         return '<i></i>';
                     }
                 }, "defaultContent": "<i></i>"
             },
             { "data": "Product.HSNCode", "defaultContent": "<i></i>" },
             { "data": "GuaranteeYN", render: function (data, type, row) { if (data === "True" || data === "true" || data === true) { return "Yes" } else if (data === "False" || data === "false" || data === false) { return "No" } else { return "Not Set" } }, "defaultContent": "<i></i>" },
             { "data": "InstalledDateFormatted", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
             { "data": "DocumentStatus.Description", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
             { "data": null, "orderable": false, render: function (data, type, row) { debugger; return ($('#IsDocLocked').val() == "True" || $('#IsUpdate').val() == "False") ? '<a href="#" class="actionLink"  onclick="' + (row.Spare.Code !== "" ? 'EditServiceCallDetailSpare(this)' : 'EditServiceCallDetail(this)') + '" ><i class="fa fa-pencil-square-o" aria-hidden="true"></i></a> <a href="#" class="DeleteLink"  onclick="ConfirmDeleteServiceCallDetail(this)" ><i class="fa fa-trash-o" aria-hidden="true"></i></a> ' : "-"; }, "defaultContent": "<i></i>" },
             ],
             columnDefs: [
                 { "targets": [1,4,3,2], "width": "15%" },
                 { "targets": [5], "width": "5%" },
                 { "targets": [0], "width": "35%" },

                 { className: "text-left", "targets": [0, 1,2,4] },
                 { className: "text-center", "targets": [5,3] }
             ]
         });
    $('[data-toggle="popover"]').popover({
        html: true,
        'trigger': 'hover',
        'placement': 'top'
    });
}

function AddServiceCallDetailList() {
    debugger;
    $("#divModelServiceCallPopBody").load("ServiceCall/AddServiceCallDetail?update=false", function () {
        $('#lblModelPopServiceCall').text('Service Call Detail (Product)');
        $('#divModelPopServiceCall').modal('show');
    });
}

function AddServiceCallDetailToList() {
    try {
        debugger;

        $("#FormServiceCallDetail").submit(function () { });

        if ($('#FormServiceCallDetail #IsUpdate').val() == 'True') {
            if (($('#spanProductName').text() != "") && ($('#spanProductModelName').text() != "") && ($('#InstalledDate').val() != "") && ($('#ProductModelID')[0].length <= 1 || ($('#spanProductModelName')[0].length > 1 && $('#spanProductModelName').val() != "")))
            {

                var serviceCallDetailList = _dataTable.ServiceCallDetailList.rows().data();
                serviceCallDetailList[_datatablerowindex].Product = new Object();
               // serviceCallDetailList[_datatablerowindex].Product.Code = $("#ProductID").val() != "" ? $("#ProductID option:selected").text().split("-")[0].trim() : "";
              //  serviceCallDetailList[_datatablerowindex].Product.Name = $("#ProductID").val() != "" ? $("#ProductID option:selected").text().split("-")[1].trim() : "";
                serviceCallDetailList[_datatablerowindex].Product.Code = $('#spanProductName').text() != "" ? $('#spanProductName').text().split("-")[0].trim() : "";
                serviceCallDetailList[_datatablerowindex].Product.Name = $('#spanProductName').text() != "" ? $('#spanProductName').text().split("-")[1].trim() : "";
                serviceCallDetailList[_datatablerowindex].Product.HSNCode = $("#hdnProductHSNCode").val();
             //   serviceCallDetailList[_datatablerowindex].ProductID = $("#ProductID").val() != "" ? $("#ProductID").val() : _emptyGuid;
              //  serviceCallDetailList[_datatablerowindex].ProductModelID = $("#ProductModelID").val() != "" ? $("#ProductModelID").val() : _emptyGuid;
                var ProductModel = new Object;
                var DocumentStatus = new Object;
                var Spare = new Object();
                Spare.Code = "";
                serviceCallDetailList[_datatablerowindex].SpareID = _emptyGuid;
                serviceCallDetailList[_datatablerowindex].Spare = Spare;
            //    ProductModel.Name = $("#ProductModelID").val() != "" ? $("#ProductModelID option:selected").text() : "";
                ProductModel.Name = $('#spanProductModelName').text();
                serviceCallDetailList[_datatablerowindex].ProductModel = ProductModel;
                serviceCallDetailList[_datatablerowindex].ProductSpec = $('#ProductSpec').val();
                serviceCallDetailList[_datatablerowindex].GuaranteeYN = $('#divModelServiceCallPopBody #GuaranteeYN').val();
                serviceCallDetailList[_datatablerowindex].ServiceStatusCode = $('#divModelServiceCallPopBody #ServiceStatusCode').val();
                DocumentStatus.Description = $("#divModelServiceCallPopBody #ServiceStatusCode").val() != "" ? $("#divModelServiceCallPopBody #ServiceStatusCode option:selected").text().trim() : "";
                serviceCallDetailList[_datatablerowindex].DocumentStatus = DocumentStatus;
                serviceCallDetailList[_datatablerowindex].InstalledDate = $('#InstalledDate').val();
                serviceCallDetailList[_datatablerowindex].InstalledDateFormatted = $('#InstalledDate').val();
                _dataTable.ServiceCallDetailList.clear().rows.add(serviceCallDetailList).draw(false);
                $('#divModelPopServiceCall').modal('hide');
                _datatablerowindex = -1;
            }
            else {
                if (($('#InstalledDate').val() != "")) {
                    $('#msgInstalledDate').show();
                    $('#msgInstalledDate').change(function () {
                        if ($('#InstalledDate').val !== "") { $('#msgInstalledDate').hide(); }
                    });
                }
                if ($('#ProductModelID').length > 1 && $('#ProductModelID').val() != "") {
                    $('#msgProductModel').show();
                    $('#msgProductModel').change(function () {
                        if ($('#ProductModelID').val !== "") { $('#msgProductModel').hide(); }
                    });
                }
            }
        }
        else {
            if (($('#ProductID').val() != "") && ($('#ProductModelID').val() != "") && ($('#InstalledDate').val() != "") && ($('#ProductModelID')[0].length <= 1 || ($('#ProductModelID')[0].length > 1 && $('#ProductModelID').val() != ""))) {
                    if (_dataTable.ServiceCallDetailList.rows().data().length === 0) {
                    _dataTable.ServiceCallDetailList.clear().rows.add(GetServiceCallDetailListByServiceCallID(_emptyGuid)).draw(false);
                    debugger;
                    var serviceCallDetailList = _dataTable.ServiceCallDetailList.rows().data();
                    serviceCallDetailList[0].Product = new Object();
                    serviceCallDetailList[0].Product.Code = $("#ProductID").val() != "" ? $("#ProductID option:selected").text().split("-")[0].trim() : "";
                    serviceCallDetailList[0].Product.Name = $("#ProductID").val() != "" ? $("#ProductID option:selected").text().split("-")[1].trim() : "";
                    serviceCallDetailList[0].Product.HSNCode = $("#hdnProductHSNCode").val();
                    serviceCallDetailList[0].ProductID = $("#ProductID").val() != "" ? $("#ProductID").val() : _emptyGuid;
                    serviceCallDetailList[0].ProductModelID = $("#ProductModelID").val() != "" ? $("#ProductModelID").val() : _emptyGuid;
                    ProductModel = new Object;
                    DocumentStatus = new Object;
                    ProductModel.Name = $("#ProductModelID").val() != "" ? $("#ProductModelID option:selected").text() : "";
                    serviceCallDetailList[0].ProductModel = ProductModel;
                    serviceCallDetailList[0].ProductSpec = $('#ProductSpec').val();
                    serviceCallDetailList[0].GuaranteeYN = $('#divModelServiceCallPopBody #GuaranteeYN').val();
                    serviceCallDetailList[0].ServiceStatusCode = $('#divModelServiceCallPopBody #ServiceStatusCode').val();
                    DocumentStatus.Description = $("#divModelServiceCallPopBody #ServiceStatusCode").val() != "" ? $("#divModelServiceCallPopBody #ServiceStatusCode option:selected").text().trim() : "";
                    serviceCallDetailList[0].DocumentStatus = DocumentStatus;
                    serviceCallDetailList[0].InstalledDate = $('#InstalledDate').val();
                    serviceCallDetailList[0].InstalledDateFormatted = $('#InstalledDate').val();
                    serviceCallDetailList[0].SpareID = _emptyGuid;
                    _dataTable.ServiceCallDetailList.clear().rows.add(serviceCallDetailList).draw(false);
                    $('#divModelPopServiceCall').modal('hide');
                }
                else {
                    var serviceCallDetailList = _dataTable.ServiceCallDetailList.rows().data();
                    if (serviceCallDetailList.length > 0) {
                        var checkpoint = 0;
                        var productSpec = $('#ProductSpec').val();
                        productSpec = productSpec.replace(/\n/g, ' ');
                        for (var i = 0; i < serviceCallDetailList.length; i++) {
                            if ((serviceCallDetailList[i].ProductID == $('#ProductID').val()) && (serviceCallDetailList[i].ProductModelID == $('#ProductModelID').val()
                                && (serviceCallDetailList[i].ProductSpec.replace(/\n/g, ' ') == productSpec))) {
                               
                                checkpoint = 1;
                                break;
                            }
                        }
                        if (checkpoint == 1) {
                            _dataTable.ServiceCallDetailList.clear().rows.add(serviceCallDetailList).draw(false);
                        }
                        else if (checkpoint == 0) {
                            var serviceCallDetailVM = new Object();
                            var Product = new Object;
                            var ProductModel = new Object();
                            var DocumentStatus = new Object;
                            var Spare = new Object();
                            serviceCallDetailVM.ID = _emptyGuid;
                            serviceCallDetailVM.ProductID = $("#ProductID").val() != "" ? $("#ProductID").val() : _emptyGuid;
                            Product.Code = $("#ProductID").val() != "" ? $("#ProductID option:selected").text().split("-")[0].trim() : "";
                            Product.Name = $("#ProductID").val() != "" ? $("#ProductID option:selected").text().split("-")[1].trim() : "";
                            Product.HSNCode = $("#hdnProductHSNCode").val();
                            serviceCallDetailVM.Product = Product;
                            serviceCallDetailVM.ProductModelID = $("#ProductModelID").val() != "" ? $("#ProductModelID").val() : _emptyGuid;
                            ProductModel.Name = $("#ProductModelID").val() != "" ? $("#ProductModelID option:selected").text() : "";
                            serviceCallDetailVM.ProductModel = ProductModel;
                            serviceCallDetailVM.ProductSpec = $('#ProductSpec').val();
                            serviceCallDetailVM.GuaranteeYN = $('#divModelServiceCallPopBody #GuaranteeYN').val();
                            serviceCallDetailVM.ServiceStatusCode = $('#divModelServiceCallPopBody #ServiceStatusCode').val();
                            DocumentStatus.Description = $("#divModelServiceCallPopBody #ServiceStatusCode").val() != "" ? $("#divModelServiceCallPopBody #ServiceStatusCode option:selected").text().trim() : "";
                            serviceCallDetailVM.DocumentStatus = DocumentStatus;
                            serviceCallDetailVM.InstalledDate = $('#InstalledDate').val();
                            serviceCallDetailVM.InstalledDateFormatted = $('#InstalledDate').val();
                            Spare.Code = "";
                            serviceCallDetailVM.Spare = Spare;
                            serviceCallDetailVM.SpareID = _emptyGuid;
                            _dataTable.ServiceCallDetailList.row.add(serviceCallDetailVM).draw(true);
                        }
                        $('#divModelPopServiceCall').modal('hide');
                    }
                }
            }
            else {
                if (($('#InstalledDate').val() != "")) {
                    $('#msgInstalledDate').show();
                    $('#msgInstalledDate').change(function () {
                        if ($('#InstalledDate').val !== "") { $('#msgInstalledDate').hide(); }
                    });
                }
                if ($('#ProductModelID').length > 1 && $('#ProductModelID').val() != "") {
                    $('#msgProductModel').show();
                    $('#msgProductModel').change(function () {
                        if ($('#ProductModelID').val !== "") { $('#msgProductModel').hide(); }
                    });
                }
            }
        }
        $('[data-toggle="popover"]').popover({
            html: true,
            'trigger': 'hover',
            'placement': 'top'
        });
    }
    catch (e) {
        console.log(e.message);
    }
}

//Get ServiceCallDetailList By ServiceCallID
function GetServiceCallDetailListByServiceCallID(id) {
    try {
        debugger;
            var data = { "serviceCallID": id };
            var serviceCallDetailList = [];
            _jsonData = GetDataFromServer("ServiceCall/GetServiceCallDetailListByServiceCallID/", data);
            if (_jsonData != '') {
                _jsonData = JSON.parse(_jsonData);
                _message = _jsonData.Message;
                _status = _jsonData.Status;
                serviceCallDetailList = _jsonData.Records;
            }
            if (_status == "OK") {
                return serviceCallDetailList;
            }
            if (_status == "ERROR") {
                notyAlert('error', _message);
            }
        }
    catch (e) {
        console.log(e.message);
    }
}

//======Add ServiceCallCharge======//
//Bind ServiceCallCharges Table
function BindServiceCallChargeDetailList(id) {
    debugger;
    _dataTable.ServiceCallChargeDetailList = $('#tblServiceCallChargeDetailList').DataTable(
         {
             dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
             order: [],
             searching: false,
             paging: false,
             ordering: false,
             bInfo: false,
             data: id == _emptyGuid ? null : GetServiceCallChargeDetailListByServiceCallID(id),
             language: {
                 search: "_INPUT_",
                 searchPlaceholder: "Search"
             },
             columns: [
             { "data": "OtherCharge.Description", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
             { "data":"OtherCharge.SACCode","defaultContent": "<i></i>" },
             { "data": "ChargeAmount", render: function (data, type, row) { return data }, "defaultContent": "<i></i>" },
             {
                 "data": "ChargeAmount", render: function (data, type, row) {
                     debugger;
                     var CGST = parseFloat(row.CGSTPerc != "" ? row.CGSTPerc : 0);
                     var SGST = parseFloat(row.SGSTPerc != "" ? row.SGSTPerc : 0);
                     var IGST = parseFloat(row.IGSTPerc != "" ? row.IGSTPerc : 0);
                     var CGSTAmt = parseFloat(data * CGST / 100);
                     var SGSTAmt = parseFloat(data * SGST / 100)
                     var IGSTAmt = parseFloat(data * IGST / 100)
                     var GSTAmt = roundoff(parseFloat(CGSTAmt) + parseFloat(SGSTAmt) + parseFloat(IGSTAmt))
                     return '<div class="show-popover text-right" data-html="true" data-toggle="popover" data-title="<p align=left>Total GST : ₹ ' + GSTAmt + '" data-content=" SGST ' + SGST + '% : ₹ ' + roundoff(parseFloat(SGSTAmt)) + '<br/>CGST ' + CGST + '% : ₹ ' + roundoff(parseFloat(CGSTAmt)) + '<br/> IGST ' + IGST + '% : ₹ ' + roundoff(parseFloat(IGSTAmt)) + '</p>"/>' + GSTAmt
                 }, "defaultContent": "<i></i>"
             },
             { "data": "AddlTaxAmt", render: function (data, type, row) { return roundoff(parseFloat(data)) }, "defaultContent": "<i></i>" },
             {
                 "data": "ChargeAmount", render: function (data, type, row) {
                     var CGST = parseFloat(row.CGSTPerc != "" ? row.CGSTPerc : 0);
                     var SGST = parseFloat(row.SGSTPerc != "" ? row.SGSTPerc : 0);
                     var IGST = parseFloat(row.IGSTPerc != "" ? row.IGSTPerc : 0);
                     var CGSTAmt = parseFloat(data * CGST / 100);
                     var SGSTAmt = parseFloat(data * SGST / 100)
                     var IGSTAmt = parseFloat(data * IGST / 100)
                     var GSTAmt = roundoff(parseFloat(CGSTAmt) + parseFloat(SGSTAmt) + parseFloat(IGSTAmt))
                     var AddlTaxAmt = parseFloat(row.AddlTaxAmt);
                     var Total = roundoff(parseFloat(data) + parseFloat(GSTAmt) + parseFloat(AddlTaxAmt))
                     //return Total
                     return '<div class="show-popover text-right" data-html="true" data-toggle="popover" data-title="<p align=left>Total : ₹ ' + Total + '" data-content="Charge Amount : ₹ ' + data + '<br/>GST : ₹ ' + GSTAmt + '</p>"/>' + Total
                 }, "defaultContent": "<i></i>"
             },
             { "data": null, "orderable": false, "defaultContent": ($('#IsDocLocked').val() == "True" || $('#IsUpdate').val() == "False") ? '<a href="#" class="actionLink"  onclick="EditServiceCallChargeDetail(this)" ><i class="fa fa-pencil-square-o" aria-hidden="true"></i></a> <a href="#" class="DeleteLink"  onclick="ConfirmDeleteServiceCallChargeDetail(this)" ><i class="fa fa-trash-o" aria-hidden="true"></i></a>' : "-"},
             ],
             columnDefs: [
                 //{ "targets": [0], "width": "30%" },
                 //{ "targets": [1, 2], "width": "20%" },
                 //{ "targets": [3], "width": "20%" },
                 { className: "text-right", "targets": [ 2, 3, 4, 5] },
                 { className: "text-left", "targets": [0,1] },
                 { className: "text-center", "targets": [6] }
             ],
             destroy: true,
         });
    $('[data-toggle="popover"]').popover({
        html: true,
        'trigger': 'hover',
        'placement': 'top'
    });
}

function AddServiceCallChargeDetailList() {
    try {
        debugger;
        $("#divModelCallChargesPopBody").load("ServiceCall/AddServiceCallCharge?update=false", function () {
            $('#lblModelPopCallCharges').text('ServiceCall Charges')
            $('#divModelPopCallCharges').modal('show');
        });
    }
    catch (e) {
        console.log(e.message);
    }
}

function AddServiceCallChargeDetailToList() {
    try {
        debugger;
        $("#FormServiceCallChargeDetail").submit(function () { });
        debugger;
        if ($('#FormServiceCallChargeDetail #IsUpdate').val() == 'True') {
            if (($('#divModelCallChargesPopBody #OtherChargeCode').val() != "") && ($('#divModelCallChargesPopBody #ChargeAmount').val() != "")) {
                debugger;
                var serviceCallChargeDetailList = _dataTable.ServiceCallChargeDetailList.rows().data();
              //  serviceCallChargeDetailList[_datatablerowindex].OtherCharge.Description = $("#divModelCallChargesPopBody #OtherChargeCode").val() != "" ? $("#divModelCallChargesPopBody #OtherChargeCode option:selected").text().split("-")[0].trim() : "";
                serviceCallChargeDetailList[_datatablerowindex].OtherCharge.Description = $('#spanOtherCharge').text() != "" ? $('#spanOtherCharge').text().split("-")[0].trim() : "";
                serviceCallChargeDetailList[_datatablerowindex].ChargeAmount = $("#divModelCallChargesPopBody #ChargeAmount").val();
                serviceCallChargeDetailList[_datatablerowindex].OtherChargeCode = $("#divModelCallChargesPopBody #hdnOtherChargeCode").val() != "" ? $("#divModelCallChargesPopBody #hdnOtherChargeCode").val() : _emptyGuid;
                serviceCallChargeDetailList[_datatablerowindex].OtherCharge.SACCode = $("#hdnOtherChargeSACCode").val();
                TaxType = new Object;
                if ($('#divModelCallChargesPopBody #TaxTypeCode').val() != null) {
                    serviceCallChargeDetailList[_datatablerowindex].TaxTypeCode = $('#divModelCallChargesPopBody #TaxTypeCode').val().split('|')[0];
                    TaxType.ValueText = $('#divModelCallChargesPopBody #TaxTypeCode').val();
                }
                serviceCallChargeDetailList[_datatablerowindex].TaxType = TaxType;
                serviceCallChargeDetailList[_datatablerowindex].CGSTPerc = $('#divModelCallChargesPopBody #hdnCGSTPerc').val();
                serviceCallChargeDetailList[_datatablerowindex].SGSTPerc = $('#divModelCallChargesPopBody #hdnSGSTPerc').val();
                serviceCallChargeDetailList[_datatablerowindex].IGSTPerc = $('#divModelCallChargesPopBody #hdnIGSTPerc').val();
                serviceCallChargeDetailList[_datatablerowindex].AddlTaxPerc = $('#divModelCallChargesPopBody #AddlTaxPerc').val();
                serviceCallChargeDetailList[_datatablerowindex].AddlTaxAmt = $('#divModelCallChargesPopBody #AddlTaxAmt').val();
                ClearCalculatedFields();
                _dataTable.ServiceCallChargeDetailList.clear().rows.add(serviceCallChargeDetailList).draw(false);
                CalculateTotal();
                $('#divModelPopCallCharges').modal('hide');
                _datatablerowindex = -1;
            }
        }
        else {
            if (($('#divModelCallChargesPopBody #OtherChargeCode').val() != "") && ($('#divModelCallChargesPopBody #ChargeAmount').val() != "")) {
                debugger;
                if (_dataTable.ServiceCallChargeDetailList.rows().data().length === 0) {
                    _dataTable.ServiceCallChargeDetailList.clear().rows.add(GetServiceCallChargeDetailListByServiceCallID(_emptyGuid, false)).draw(false);
                    debugger;
                    var serviceCallChargeDetailList = _dataTable.ServiceCallChargeDetailList.rows().data();
                    serviceCallChargeDetailList[0].OtherCharge.Description = $("#divModelCallChargesPopBody #OtherChargeCode").val() != "" ? $("#divModelCallChargesPopBody #OtherChargeCode option:selected").text().split("-")[0].trim() : "";
                    serviceCallChargeDetailList[0].OtherChargeCode = $("#divModelCallChargesPopBody #OtherChargeCode").val() != "" ? $("#divModelCallChargesPopBody #OtherChargeCode").val() : _emptyGuid;
                    serviceCallChargeDetailList[0].ChargeAmount = $("#divModelCallChargesPopBody #ChargeAmount").val();
                    serviceCallChargeDetailList[0].OtherCharge.SACCode = $("#hdnOtherChargeSACCode").val();
                    if ($('#divModelCallChargesPopBody #TaxTypeCode').val() != null) {
                        serviceCallChargeDetailList[0].TaxTypeCode = $('#divModelCallChargesPopBody #TaxTypeCode').val().split('|')[0];
                    }
                    serviceCallChargeDetailList[0].TaxType.ValueText = $('#divModelCallChargesPopBody #TaxTypeCode').val();
                    serviceCallChargeDetailList[0].CGSTPerc = $('#divModelCallChargesPopBody #hdnCGSTPerc').val();
                    serviceCallChargeDetailList[0].SGSTPerc = $('#divModelCallChargesPopBody #hdnSGSTPerc').val();
                    serviceCallChargeDetailList[0].IGSTPerc = $('#divModelCallChargesPopBody #hdnIGSTPerc').val();
                    var AddlTaxPerc = ($('#divModelCallChargesPopBody #AddlTaxPerc').val() === "") ? 0 : $('#divModelCallChargesPopBody #AddlTaxPerc').val();
                    serviceCallChargeDetailList[0].AddlTaxPerc = AddlTaxPerc;
                    serviceCallChargeDetailList[0].AddlTaxAmt = $('#divModelCallChargesPopBody #AddlTaxAmt').val();
                    ClearCalculatedFields();
                    _dataTable.ServiceCallChargeDetailList.clear().rows.add(serviceCallChargeDetailList).draw(false);
                    CalculateTotal();
                    $('#divModelPopCallCharges').modal('hide');
                }
                else {
                    debugger;
                    var serviceCallChargeDetailList = _dataTable.ServiceCallChargeDetailList.rows().data();
                    if (serviceCallChargeDetailList.length > 0) {
                        var checkpoint = 0;
                        var otherCharge = $('#OtherChargeCode').val();
                        for (var i = 0; i < serviceCallChargeDetailList.length; i++) {
                            if ((serviceCallChargeDetailList[i].OtherChargeCode == otherCharge)) {
                                serviceCallChargeDetailList[i].ChargeAmount = parseFloat(serviceCallChargeDetailList[i].ChargeAmount) + parseFloat($('#ChargeAmount').val());
                                checkpoint = 1;
                                break;
                            }
                        }
                        if (checkpoint == 1) {
                            debugger;
                            ClearCalculatedFields();
                            _dataTable.ServiceCallChargeDetailList.clear().rows.add(serviceCallChargeDetailList).draw(false);
                            CalculateTotal();
                            $('#divModelPopCallCharges').modal('hide');
                        }
                        else if (checkpoint == 0) {
                            ClearCalculatedFields();
                            var ServiceCallChargeDetailVM = new Object();
                            ServiceCallChargeDetailVM.ID = _emptyGuid;
                            ServiceCallChargeDetailVM.ServiceCallID = _emptyGuid;
                            var OtherCharge = new Object;
                            OtherCharge.Description = $("#divModelCallChargesPopBody #OtherChargeCode").val() != "" ? $("#divModelCallChargesPopBody #OtherChargeCode option:selected").text().split("-")[0].trim() : "";
                            ServiceCallChargeDetailVM.OtherCharge = OtherCharge;
                            ServiceCallChargeDetailVM.OtherChargeCode = $("#divModelCallChargesPopBody #OtherChargeCode").val() != "" ? $("#divModelCallChargesPopBody #OtherChargeCode").val() : _emptyGuid;
                            ServiceCallChargeDetailVM.ChargeAmount = $("#divModelCallChargesPopBody #ChargeAmount").val();
                            ServiceCallChargeDetailVM.OtherCharge.SACCode = $("#hdnOtherChargeSACCode").val();
                            var TaxType = new Object();
                            if ($('#divModelCallChargesPopBody #TaxTypeCode').val() != null) {
                                ServiceCallChargeDetailVM.TaxTypeCode = $('#divModelCallChargesPopBody #TaxTypeCode').val().split('|')[0];
                                TaxType.ValueText = $('#divModelCallChargesPopBody #TaxTypeCode').val();
                            }
                            ServiceCallChargeDetailVM.TaxType = TaxType;
                            ServiceCallChargeDetailVM.CGSTPerc = $('#divModelCallChargesPopBody #hdnCGSTPerc').val();
                            ServiceCallChargeDetailVM.SGSTPerc = $('#divModelCallChargesPopBody #hdnSGSTPerc').val();
                            ServiceCallChargeDetailVM.IGSTPerc = $('#divModelCallChargesPopBody #hdnIGSTPerc').val();
                            var AddlTaxPerc = ($('#divModelCallChargesPopBody #AddlTaxPerc').val() === "") ? 0 : $('#divModelCallChargesPopBody #AddlTaxPerc').val();
                            ServiceCallChargeDetailVM.AddlTaxPerc = AddlTaxPerc;
                            ServiceCallChargeDetailVM.AddlTaxAmt = $('#divModelCallChargesPopBody #AddlTaxAmt').val();
                            _dataTable.ServiceCallChargeDetailList.row.add(ServiceCallChargeDetailVM).draw(true);
                            CalculateTotal();
                            $('#divModelPopCallCharges').modal('hide');
                        }
                    }
                }
            }
        }
        $('[data-toggle="popover"]').popover({
            html: true,
            'trigger': 'hover',
            'placement': 'left'
        });
    }
    catch (e) {
        console.log(e.message);
    }
}

//Get ServiceCallChargeList By ServiceCallID
function GetServiceCallChargeDetailListByServiceCallID(id) {
    try {
        debugger;
        var data = { "serviceCallID": id };
        var serviceCallDetailList = [];
        _jsonData = GetDataFromServer("ServiceCall/GetServiceCallChargeListByServiceCallID/", data);
        if (_jsonData != '') {
            _jsonData = JSON.parse(_jsonData);
            _message = _jsonData.Message;
            _status = _jsonData.Status;
            serviceCallDetailList = _jsonData.Records;
        }
        if (_status == "OK") {
            return serviceCallDetailList;
        }
        if (_status == "ERROR") {
            notyAlert('error', _message);
        }
    }
    catch (e) {
        console.log(e.message);
    }
}

//===============Save==============//

function SaveServiceCall() {
    debugger;
    var serviceCallList = _dataTable.ServiceCallDetailList.rows().data().toArray();
    $('#DetailJSON').val(JSON.stringify(serviceCallList));
    var callChargeDetailList = _dataTable.ServiceCallChargeDetailList.rows().data().toArray();
    $('#CallChargeJSON').val(JSON.stringify(callChargeDetailList));
    $('#btnInsertUpdateServiceCall').trigger('click');
}
function SaveSuccessServiceCall(data, status) {
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
                $("#divServiceCallForm").load("ServiceCall/ServiceCallForm?id=" + _result.ID + "&estimateID=" + _result.EstimateID, function () {
                    ChangeButtonPatchView("ServiceCall", "btnPatchServiceCallNew", "Edit");
                    $('#lblServiceCallInfo').text(_result.ServiceCallNo);
                    BindServiceCallChargeDetailList(_result.ID);
                    BindServiceCallDetailList(_result.ID);
                    CalculateTotal();
                    clearUploadControl();
                    PaintImages(_result.ID);
                    $('#divCustomerBasicInfo').load("Customer/CustomerBasicInfo?ID=" + $('#ServiceCallForm #hdnCustomerID').val());
                });
                ChangeButtonPatchView("ServiceCall", "btnPatchServiceCallNew", "Edit");
                BindOrReloadServiceCallTable('Init');
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

//==============Delete=============//

function DeleteServiceCall() {
    notyConfirm('Are you sure to delete?', 'DeleteServiceCallItem("' + $('#ServiceCallForm #ID').val() + '")');
}
function DeleteServiceCallItem(id) {
    try {
        if (id) {
            var data = { "id": id };
            _jsonData = GetDataFromServer("ServiceCall/DeleteServiceCall/", data);
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
                    ChangeButtonPatchView("ServiceCall", "btnPatchServiceCallNew", "Add");
                    ResetServiceCall();
                    BindOrReloadServiceCallTable('Init');
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

function ConfirmDeleteServiceCallDetail(this_Obj) {

    _datatablerowindex = _dataTable.ServiceCallDetailList.row($(this_Obj).parents('tr')).index();
    var serviceCallDetail = _dataTable.ServiceCallDetailList.row($(this_Obj).parents('tr')).data();
    if (serviceCallDetail.ID === _emptyGuid) {
        notyConfirm('Are you sure to delete?', 'DeleteCurrentServiceCallDetail("' + _datatablerowindex + '")');
    }
    else {
        notyConfirm('Are you sure to delete?', 'DeleteServiceCallDetail("' + serviceCallDetail.ID + '")');

    }
}
function DeleteCurrentServiceCallDetail(_datatablerowindex) {
    var serviceCallDetailList = _dataTable.ServiceCallDetailList.rows().data();
    serviceCallDetailList.splice(_datatablerowindex, 1);
    _dataTable.ServiceCallDetailList.clear().rows.add(serviceCallDetailList).draw(false);
    notyAlert('success', 'Detail Row deleted successfully');
}
function DeleteServiceCallDetail(ID) {
    if (ID != _emptyGuid && ID != null && ID != '') {
        var data = { "id": ID };
        var ds = {};
        _jsonData = GetDataFromServer("ServiceCall/DeleteServiceCallDetail/", data);
        if (_jsonData != '') {
            _jsonData = JSON.parse(_jsonData);
            _message = _jsonData.Message;
            _status = _jsonData.Status;
            _result = _jsonData.Record;
        }
        if (_status == "OK") {
            notyAlert('success', _result.Message);
            var serviceCallDetailList = _dataTable.ServiceCallDetailList.rows().data();
            serviceCallDetailList.splice(_datatablerowindex, 1);
            _dataTable.ServiceCallDetailList.clear().rows.add(serviceCallDetailList).draw(false);
        }
        if (_status == "ERROR") {
            notyAlert('error', _message);
        }
    }
}

function ConfirmDeleteServiceCallChargeDetail(this_Obj) {
    debugger;
    _datatablerowindex = _dataTable.ServiceCallChargeDetailList.row($(this_Obj).parents('tr')).index();
    var serviceCallChargeDetail = _dataTable.ServiceCallChargeDetailList.row($(this_Obj).parents('tr')).data();
    if (serviceCallChargeDetail.ID === _emptyGuid) {
        notyConfirm('Are you sure to delete?', 'DeleteCurrentServiceCallOtherChargeDetail("' + _datatablerowindex + '")');
    }
    else {
        notyConfirm('Are you sure to delete?', 'DeleteServiceCallChargeDetail("' + serviceCallChargeDetail.ID + '")');

    }
}
function DeleteCurrentServiceCallOtherChargeDetail(_datatablerowindex) {
    var serviceCallChargeDetailList = _dataTable.ServiceCallChargeDetailList.rows().data();
    serviceCallChargeDetailList.splice(_datatablerowindex, 1);
    ClearCalculatedFields();
    _dataTable.ServiceCallChargeDetailList.clear().rows.add(serviceCallChargeDetailList).draw(false);
    CalculateTotal();
    notyAlert('success', 'Detail Row deleted successfully');
}
function DeleteServiceCallChargeDetail(ID) {
    if (ID != _emptyGuid && ID != null && ID != '') {
        var data = { "id": ID };
        var ds = {};
        _jsonData = GetDataFromServer("ServiceCall/DeleteServiceCallChargeDetail/", data);
        if (_jsonData != '') {
            _jsonData = JSON.parse(_jsonData);
            _message = _jsonData.Message;
            _status = _jsonData.Status;
            _result = _jsonData.Record;
        }
        if (_status == "OK") {
            notyAlert('success', _result.Message);
            var serviceCallChargeDetailList = _dataTable.ServiceCallChargeDetailList.rows().data();
            serviceCallChargeDetailList.splice(_datatablerowindex, 1);
            ClearCalculatedFields();
            _dataTable.ServiceCallChargeDetailList.clear().rows.add(serviceCallChargeDetailList).draw(false);
            CalculateTotal();
        }
        if (_status == "ERROR") {
            notyAlert('error', _message);
        }
    }
}

//==============Redirect to nav=================//
function EditRedirectToDocument(id) {

    OnServerCallBegin();

    $("#divServiceCallForm").load("ServiceCall/ServiceCallForm?id=" + id, function (responseTxt, statusTxt, xhr) {
        if (statusTxt == "success") {
            OnServerCallComplete();
            openNav();
            $('#lblServiceCallInfo').text($('#ServiceCallNo').val());
            if ($('#IsDocLocked').val() == "True") {
                ChangeButtonPatchView("ServiceCall", "btnPatchServiceCallNew", "Edit", id);
            }
            else {
                ChangeButtonPatchView("ServiceCall", "btnPatchServiceCallNew", "LockDocument",id);
            }
            BindServiceCallDetailList(id);
            BindServiceCallChargeDetailList(id)
            $('#divCustomerBasicInfo').load("Customer/CustomerBasicInfo?ID=" + $('#hdnCustomerID').val());
            clearUploadControl();
            PaintImages(id);
            CalculateTotal();
        }
        else {
            console.log("Error: " + xhr.status + ": " + xhr.statusText);
        }
    });
}

//=============== Sale Invoice List By Customer ID ==================//
function GetSaleInvoiceByCustomerID() {
    try {
        debugger;
        $('#lblModelPopInvoices').text('Sale Invoices for Customer');
        $('#lblModelPopCustomer').text($("#CustomerID").val() != "" ? $("#CustomerID option:selected").text().trim() : "");
        $('#divModelPopInvoices').modal('show');
    }
    catch (e) {
        console.log(e.message);
    }
}

function AddServiceCallDetailSpare() {
    debugger;
    _datatablerowindex = -1;
    $("#divModelServiceCallPopBody").load("ServiceCall/AddServiceCallDetailSpare?update=false", function () {
        $('#lblModelPopServiceCall').text('Service Call Detail (Spare)')
        $('#divModelPopServiceCall').modal('show');
    });
}

function AddServiceCallDetailSpareToList() {
    try {
        debugger;

        $("#FormServiceCallDetailSpare").submit(function () { });

        if ($('#FormServiceCallDetailSpare #IsUpdate').val() == 'True') {
            if ($('#SpareID').val() != "") {

                var serviceCallDetailList = _dataTable.ServiceCallDetailList.rows().data();

                var Product = new Object;
                var ProductModel = new Object();
                Product.Code = "";
                serviceCallDetailList[_datatablerowindex].Product = Product;
                serviceCallDetailList[_datatablerowindex].ProductID = _emptyGuid;
                serviceCallDetailList[_datatablerowindex].ProductModelID = _emptyGuid;
                serviceCallDetailList[_datatablerowindex].Spare = new Object();
             //   serviceCallDetailList[_datatablerowindex].Spare.Code = $("#SpareID").val() != "" ? $("#SpareID option:selected").text().split("-")[0].trim() : "";
                //   serviceCallDetailList[_datatablerowindex].Spare.Name = $("#SpareID").val() != "" ? $("#SpareID option:selected").text().split("-")[1].trim() : "";
                serviceCallDetailList[_datatablerowindex].Spare.Code = $('#spanSpare').text() != "" ? $('#spanSpare').text().split("-")[0].trim() : "";
                serviceCallDetailList[_datatablerowindex].Spare.Name = $('#spanSpare').text() != "" ? $('#spanSpare').text().split("-")[1].trim() : "";
                serviceCallDetailList[_datatablerowindex].SpareID = $("#hdnSpareID").val() != "" ? $("#hdnSpareID").val() : _emptyGuid;
                DocumentStatus = new Object;
                serviceCallDetailList[_datatablerowindex].GuaranteeYN = $('#divModelServiceCallPopBody #GuaranteeYN').val();
                serviceCallDetailList[_datatablerowindex].ServiceStatusCode = $('#divModelServiceCallPopBody #ServiceStatusCode').val();
                DocumentStatus.Description = $("#divModelServiceCallPopBody #ServiceStatusCode").val() != "" ? $("#divModelServiceCallPopBody #ServiceStatusCode option:selected").text().trim() : "";
                serviceCallDetailList[_datatablerowindex].DocumentStatus = DocumentStatus;
                serviceCallDetailList[_datatablerowindex].InstalledDate = $('#InstalledDate').val();
                serviceCallDetailList[_datatablerowindex].InstalledDateFormatted = $('#InstalledDate').val();
                _dataTable.ServiceCallDetailList.clear().rows.add(serviceCallDetailList).draw(false);
                $('#divModelPopServiceCall').modal('hide');
                _datatablerowindex = -1;
            }
            else {
                if (($('#InstalledDate').val() != "")) {
                    $('#msgInstalledDate').show();
                    $('#msgInstalledDate').change(function () {
                        if ($('#InstalledDate').val !== "") { $('#msgInstalledDate').hide(); }
                    });
                }
            }
        }
        else {
            if ($('#SpareID').val() != "") {
                if (_dataTable.ServiceCallDetailList.rows().data().length === 0) {
                    _dataTable.ServiceCallDetailList.clear().rows.add(GetServiceCallDetailListByServiceCallID(_emptyGuid)).draw(false);
                    var serviceCallDetailList = _dataTable.ServiceCallDetailList.rows().data();
                    serviceCallDetailList[0].Spare = new Object();
                    serviceCallDetailList[0].Spare.Code = $("#SpareID").val() != "" ? $("#SpareID option:selected").text().split("-")[0].trim() : "";
                    serviceCallDetailList[0].Spare.Name = $("#SpareID").val() != "" ? $("#SpareID option:selected").text().split("-")[1].trim() : "";
                    serviceCallDetailList[0].SpareID = $("#SpareID").val() != "" ? $("#SpareID").val() : _emptyGuid;
                    serviceCallDetailList[0].GuaranteeYN = $('#divModelServiceCallPopBody #GuaranteeYN').val();
                    serviceCallDetailList[0].ServiceStatusCode = $('#divModelServiceCallPopBody #ServiceStatusCode').val();
                    DocumentStatus = new Object;
                    DocumentStatus.Description = $("#divModelServiceCallPopBody #ServiceStatusCode").val() != "" ? $("#divModelServiceCallPopBody #ServiceStatusCode option:selected").text().trim() : "";
                    serviceCallDetailList[0].DocumentStatus = DocumentStatus;
                    serviceCallDetailList[0].InstalledDate = $('#InstalledDate').val();
                    serviceCallDetailList[0].InstalledDateFormatted = $('#InstalledDate').val();
                    serviceCallDetailList[0].ProductID = _emptyGuid;
                    serviceCallDetailList[0].ProductModelID = _emptyGuid;
                    _dataTable.ServiceCallDetailList.clear().rows.add(serviceCallDetailList).draw(false);
                    $('#divModelPopServiceCall').modal('hide');
                }
                else {
                    var serviceCallDetailList = _dataTable.ServiceCallDetailList.rows().data();
                    if (serviceCallDetailList.length > 0) {
                        var checkpoint = 0;
                        var spare = $('#SpareID').val();

                        for (var i = 0; i < serviceCallDetailList.length; i++) {
                            if (serviceCallDetailList[i].SpareID == $('#SpareID').val())
                            {
                                checkpoint = 1;
                            }
                        }
                        if (checkpoint == 1) {
                            _dataTable.ServiceCallDetailList.clear().rows.add(serviceCallDetailList).draw(false);
                        }
                        else if (checkpoint == 0) {
                            var serviceCallDetailVM = new Object();
                            var Spare = new Object;
                            var DocumentStatus = new Object;
                            var Product = new Object;
                            var ProductModel = new Object();
                            Product.Code = "";
                            serviceCallDetailVM.Product = Product;
                            serviceCallDetailVM.ID = _emptyGuid;
                            serviceCallDetailVM.ProductID = _emptyGuid;
                            serviceCallDetailVM.ProductModelID = _emptyGuid;
                            serviceCallDetailVM.SpareID = $("#hdnSpareID").val() != "" ? $("#hdnSpareID").val() : _emptyGuid;
                            Spare.Code = $("#SpareID").val() != "" ? $("#SpareID option:selected").text().split("-")[0].trim() : "";
                            Spare.Name = $("#SpareID").val() != "" ? $("#SpareID option:selected").text().split("-")[1].trim() : "";
                            serviceCallDetailVM.Spare = Spare;
                            serviceCallDetailVM.GuaranteeYN = $('#divModelServiceCallPopBody #GuaranteeYN').val();
                            serviceCallDetailVM.ServiceStatusCode = $('#divModelServiceCallPopBody #ServiceStatusCode').val();
                            DocumentStatus.Description = $("#divModelServiceCallPopBody #ServiceStatusCode").val() != "" ? $("#divModelServiceCallPopBody #ServiceStatusCode option:selected").text().trim() : "";
                            serviceCallDetailVM.DocumentStatus = DocumentStatus;
                            serviceCallDetailVM.InstalledDate = $('#InstalledDate').val();
                            serviceCallDetailVM.InstalledDateFormatted = $('#InstalledDate').val();
                            _dataTable.ServiceCallDetailList.row.add(serviceCallDetailVM).draw(true);
                        }                                              
                        $('#divModelPopServiceCall').modal('hide');
                    }
                }
            }
            else {
                if (($('#InstalledDate').val() != "")) {
                    $('#msgInstalledDate').show();
                    $('#msgInstalledDate').change(function () {
                        if ($('#InstalledDate').val !== "") { $('#msgInstalledDate').hide(); }
                    });
                }
            }
        }
        $('[data-toggle="popover"]').popover({
            html: true,
            'trigger': 'hover',
            'placement': 'top'
        });
    }
    catch (e) {
        console.log(e.message);
    }
}

function EditServiceCallDetailSpare(this_Obj) {
    try {
        debugger;
        _datatablerowindex = _dataTable.ServiceCallDetailList.row($(this_Obj).parents('tr')).index();
        var serviceCallDetail = _dataTable.ServiceCallDetailList.row($(this_Obj).parents('tr')).data();
        $("#divModelServiceCallPopBody").load("ServiceCall/AddServiceCallDetailSpare?update=true", function () {
            debugger;
            $('#lblModelPopServiceCall').text('Service Call Detail (Spare)')
            $('#FormServiceCallDetailSpare #IsUpdate').val('True');
            $('#FormServiceCallDetailSpare #ID').val(serviceCallDetail.ID);
            $('#spanSpare').text(serviceCallDetail.Spare.Code + "-" + serviceCallDetail.Spare.Name)
            $("#FormServiceCallDetailSpare #SpareID").val(serviceCallDetail.SpareID).trigger('change');
            $("#FormServiceCallDetailSpare #hdnSpareID").val(serviceCallDetail.SpareID);
            switch (serviceCallDetail.GuaranteeYN) {
                case true:
                    serviceCallDetail.GuaranteeYN = 'True'
                    break;
                case false:
                    serviceCallDetail.GuaranteeYN = 'False'
                    break;
                default:
                    serviceCallDetail.GuaranteeYN = ''
                    break;
            }
            $('#FormServiceCallDetailSpare #GuaranteeYN').val(serviceCallDetail.GuaranteeYN);
            $('#FormServiceCallDetailSpare #ServiceStatusCode').val(serviceCallDetail.ServiceStatusCode);
            $('#FormServiceCallDetailSpare #hdnServiceStatusCode').val(serviceCallDetail.ServiceStatusCode);
            $('#FormServiceCallDetailSpare #InstalledDate').val(serviceCallDetail.InstalledDateFormatted);
            $('#divModelPopServiceCall').modal('show');
        });
    }
    catch (e) {
        console.log(e.message);
    }
}

//To get SACCode
function GetOtherCharge(value) {
    try {
        debugger;
        var otherCharge;
        if (value != "")
         {
        var data = { "code": value };
        _jsonData = GetDataFromServer("OtherCharge/GetOtherCharge/", data);
        if (_jsonData != '') {
            _jsonData = JSON.parse(_jsonData);
            _message = _jsonData.Message;
            _status = _jsonData.Status;
            otherCharge = _jsonData.Record;
        }

        if (_status == "OK") {
            return otherCharge;
        }
        if (_status == "ERROR") {
            notyAlert('error', _message);
        }
        }
    }
    catch (e) {
        //this will show the error msg in the browser console(F12) 
        console.log(e.message);

    }

}