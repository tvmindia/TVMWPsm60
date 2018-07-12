var _dataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
var _jsonData = {};
var _message = "";
var _status = "";
var _result = "";
$(document).ready(function () {
    try {     
        BindOrReloadProductTable('Init');
        $('#tblProduct tbody').on('dblclick', 'td', function () {
            EditProductMaster(this);
        });
    }
    catch (e) {
        console.log(e.message);
    }
});

function BindOrReloadProductTable(action)
{
    try {
        debugger;
        //creating advancesearch object
        ProductAdvanceSearchViewModel = new Object();
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
        ProductAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
        ProductAdvanceSearchViewModel.SearchTerm = $('#SearchTerm').val();
        //apply datatable plugin on Product table
        _dataTables.ProductList = $('#tblProduct').DataTable(
            {
                dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
                buttons: [{
                    extend: 'excel',
                    exportOptions:
                                 {
                                     columns: [1, 2, 3, 4, 5, 6]
                                 }
                }],
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

                    url: "Product/GetAllProduct",
                    data: { "ProductAdvanceSearchVM": ProductAdvanceSearchViewModel },
                    type: 'POST'
                },
                pageLength: 10,
                columns: [
                { "data": "Code", "defaultContent": "<i>-</i>" },
                { "data": "Name", "defaultContent": "<i>-</i>" },               
                { "data": "ProductCategory.Description", "defaultContent": "<i>-</i>" },
                { "data": "IntroducedDateFormatted", "defaultContent": "<i>-</i>" },
                { "data": "Company.Name", "defaultContent": "<i>-</i>" },
                { "data": "HSNCode", "defaultContent": "<i>-</i>" },
                {
                    "data": null, "orderable": false, "defaultContent": '<a href="#" onclick="EditProductMaster(this)"<i class="fa fa-pencil-square-o" aria-hidden="true"></i></a>  <a href="#" onclick="DeleteProductMaster(this)"<i class="fa fa-trash-o" aria-hidden="true"></i></a>'
                }
                ],
                columnDefs: [
                { className: "text-center", "targets": [6] },
                ],
                destroy: true,
                initComplete: function (settings, json) {
                    $('.dataTables_wrapper div.bottom div').addClass('col-md-6');
                    $('#tblProduct').fadeIn(100);
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
                        BindOrReloadProductTable();
                    }
                }
            });
        $('.buttons-excel').hide();
    }
    catch(e)
    {
        console.log(e.message);
    }
}

function ResetProductList() {
    try{
        BindOrReloadProductTable('Reset');
    }
    catch (e) {
        console.log(e.message);
    }
}

function ExportProductData() {
    try{
        $('.excelExport').show();
        OnServerCallBegin();
        BindOrReloadProductTable('Export');
    }
    catch(e)
    {
        console.log(e.message);
    }
}

function EditProductMaster(thisObj) {
    try{
        debugger;
        ProductVM = _dataTables.ProductList.row($(thisObj).parents('tr')).data();

        $("#divMasterBody3").load("Product/MasterPartial?masterCode=" + ProductVM.ID, function (responseTxt, statusTxt, xhr) {
            if (statusTxt == "success") {
                $('#hdnMasterCall3').val('MSTR');
                $('#lblModelMasterContextLabel3').text('Edit Product Information')
                clearPopupUploadControl();
                PaintImages(ProductVM.ID);
                $('#divModelMasterPopUp3').modal('show');
            }
            else {
                console.log("Error: " + xhr.status + ": " + xhr.statusText);
            }
        });
    }
    catch (e) {
        console.log(e.message);
    }
}
function DeleteProductMaster(thisObj) {
    try{
        debugger;
        ProductVM = _dataTables.ProductList.row($(thisObj).parents('tr')).data();
        notyConfirm('Are you sure to delete?', 'DeleteProduct("' + ProductVM.ID + '")');
    }
    catch (e) {
        console.log(e.message);
    }
}

function DeleteProduct(id) {
    debugger;
    try {
        if (id) {
            var data = { "ID": id };
            _jsonData = GetDataFromServer("Product/DeleteProduct/", data);
            if (_jsonData != '') {
                _jsonData = JSON.parse(_jsonData);
                _message = _jsonData.Message;
                _status = _jsonData.Status;
                _result = _jsonData.Record;
            }
            switch (_status) {
                case "OK":
                    notyAlert('success', _result.Message);
                    BindOrReloadProductTable('Reset');
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