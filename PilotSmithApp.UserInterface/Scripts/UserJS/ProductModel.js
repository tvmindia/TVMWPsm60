var _dataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
var _jsonData = {};
var _message = "";
var _status = "";
var _result = "";
$(document).ready(function () {
    try {

        //$('#ProductModelID').select2({});
        //$('.select2').addClass('form-control newinput');
        debugger;
        BindOrReloadProductModelTable('Init');
    }
    catch (e) {
        console.log(e.message);
    }
});

function BindOrReloadProductModelTable(action) {
    try {
        debugger;
        //creating advancesearch object
        ProductModelAdvanceSearchViewModel = new Object();
        DataTablePagingViewModel = new Object();
        DataTablePagingViewModel.Length = 0;
        //switch case to check the operation
        switch (action) {
            case 'Reset':
                $('#SearchTerm').val('');
                break;
            case 'Init':
                break;
            case 'Search':
                if ($('#SearchTerm').val() == '') {
                    return true;
                }
                break;
            case 'Export':
                DataTablePagingViewModel.Length = -1;
                break;
            default:
                break;
        }
        ProductModelAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
        ProductModelAdvanceSearchViewModel.SearchTerm = $('#SearchTerm').val();
        //apply datatable plugin on Product table
        _dataTables.ProductModelList = $('#tblProductModel').DataTable(
            {
                dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
                buttons: [{
                    extend: 'excel',
                    exportOptions:
                                 {
                                     columns: [1, 2, 3, 4, 5, 6,7,8]
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

                    url: "ProductModel/GetAllProductModel",
                    data: { "ProductModelAdvanceSearchVM": ProductModelAdvanceSearchViewModel },
                    type: 'POST'
                },
                pageLength: 10,
                columns: [
                { "data": "ID", "defaultContent": "<i>-</i>" },
                { "data": "Product.Name", "defaultContent": "<i>-</i>" },
                { "data": "Name", "defaultContent": "<i>-</i>" },
                { "data": "UnitCode", "defaultContent": "<i>-</i>" },
                { "data": "Specification", "defaultContent": "<i>-</i>" },
                { "data": "CostPrice", "defaultContent": "<i>-</i>" },
                { "data": "SellingPrice", "defaultContent": "<i>-</i>" },
                { "data": "IntroducedDateFormatted", "defaultContent": "<i>-</i>" },
                { "data": "StockQty", "defaultContent": "<i>-</i>" },
                {
                    "data": null, "orderable": false, "defaultContent": '<a href="#" onclick="EditProductModelMaster(this)"<i class="glyphicon glyphicon-edit" aria-hidden="true"></i></a>  <a href="#" onclick="DeleteProductModelMaster(this)"<i class="glyphicon glyphicon-trash" aria-hidden="true"></i></a>'
                }
                ],
                columnDefs: [{ "targets": [0], "visible": false, "searchable": false },
                { className: "text-center", "targets": [7] },
                { className: "text-right", "targets": [5,6] },
                ],
                destroy: true,
                initComplete: function (settings, json) {
                    $('.dataTables_wrapper div.bottom div').addClass('col-md-6');
                    $('#tblProductModel').fadeIn('slow');
                    if (action == undefined) {
                        $('.excelExport').hide();
                        OnServerCallComplete();
                    }
                    if (action === 'Export') {
                        if (json.data.length > 0) {
                            if (json.data[0].TotalCount > 10000) {
                                MasterAlert("info", 'We are able to download maximum 10000 rows of data, There exist more than 10000 rows of data please filter and download')
                            }
                        }
                        $('.buttons-excel').trigger('click');
                        BindOrReloadProductModelTable();
                    }
                }
            });
        $('.buttons-excel').hide();
    }
    catch (e) {
        console.log(e.message);
    }
}

function ResetProductModelList() {
    try{
        BindOrReloadProductModelTable('Reset');
    }
    catch (e) {
        console.log(e.message);
    }
}

function ExportProductModelData() {
    try{
        $('.excelExport').show();
        OnServerCallBegin();
        BindOrReloadProductModelTable('Export');
    }
    catch (e) {
        console.log(e.message);
    }
}

function EditProductModelMaster(thisObj) {
    try{
        debugger;
        ProductModelVM = _dataTables.ProductModelList.row($(thisObj).parents('tr')).data();

        $("#divMasterBody").load("ProductModel/MasterPartial?masterCode=" + ProductModelVM.ID, function () {
            $('#hdnMasterCall').val('MSTR');
            $('#lblModelMasterContextLabel').text('Edit Product Model Information')
            $('#divModelMasterPopUp').modal('show');
        });
    }
    catch (e) {
        console.log(e.message);
    }
}
function DeleteProductModelMaster(thisObj) {
    try{
        debugger;
        ProductModelVM = _dataTables.ProductModelList.row($(thisObj).parents('tr')).data();
        notyConfirm('Are you sure to delete?', 'DeleteProductModel("' + ProductModelVM.ID + '")');
    }
    catch (e) {
        console.log(e.message);
    }
}

function DeleteProductModel(id) {
    debugger;
    try {
        if (id) {
            var data = { "ID": id };
            _jsonData = GetDataFromServer("ProductModel/DeleteProductModel/", data);
            if (_jsonData != '') {
                _jsonData = JSON.parse(_jsonData);
                _message = _jsonData.Message;
                _status = _jsonData.Status;
                _result = _jsonData.Record;
            }
            switch (_status) {
                case "OK":
                    notyAlert('success', _result.Message);
                    BindOrReloadProductModelTable('Reset');
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
        console.log(e.message);
    }
}