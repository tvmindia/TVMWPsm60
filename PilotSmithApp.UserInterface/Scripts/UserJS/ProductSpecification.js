var _dataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
var _jsonData = {};
var _message = "";
var _status = "";
var _result = "";
$(document).ready(function () {
    try {
        BindOrReloadProductSpecificationTable('Init');
        $('#tblProductSpecification tbody').on('dblclick', 'td', function () {
            EditProductSpecificationMaster(this);
        });
    }
    catch (e) {
        console.log(e.message);
    }
});
//function bind the Product Specification list and cheking and filter
function BindOrReloadProductSpecificationTable(action) {
    try {
        //creating advancesearch object
        ProductSpecificationAdvanceSearchViewModel = new Object();
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
        ProductSpecificationAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
        ProductSpecificationAdvanceSearchViewModel.SearchTerm = $('#SearchTerm').val();
        //apply datatable plugin on bank table
        _dataTables.ProductSpecificationList = $('#tblProductSpecification').DataTable(
            {
                dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
                buttons: [{
                    extend: 'excel',
                    exportOptions:
                                 {
                                     columns: [0, 1, 2]
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

                    url: "ProductSpecification/GetAllProductSpecification",
                    data: { "productSpecificationAdvanceSearchVM": ProductSpecificationAdvanceSearchViewModel },
                    type: 'POST'
                },
                pageLength: 10,
                columns: [
                { "data": "Description", "defaultContent": "<i>-</i>" },
                { "data": "PSASysCommon.CreatedDateString", "defaultContent": "<i>-</i>" },
                {
                    "data": null, "orderable": false, "defaultContent": '<a href="#" onclick="EditProductSpecificationMaster(this)"<i class="fa fa-pencil-square-o" aria-hidden="true"></i></a>  <a href="#" onclick="DeleteProductSpecificationMaster(this)"<i class="fa fa-trash-o" aria-hidden="true"></i></a>'
                }
                ],
                columnDefs: [{ "targets": [], "visible": false, "searchable": false },
                { className: "text-center", "targets": [1,2] },
                { "targets": [0], "width": "60%" },
                { "targets": [1], "width": "30%" },
                { "targets": [2], "width": "10%" }
                ],
                destroy: true,
                initComplete: function (settings, json) {
                    $('.dataTables_wrapper div.bottom div').addClass('col-md-6');
                    $('#tblProductSpecification').fadeIn(100);
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
                        BindOrReloadProductSpecificationTable();
                    }
                }
            });
        $('.buttons-excel').hide();
    }
    catch (e) {
        console.log(e.message);
    }
}

function ResetProductSpecificationList() {
    BindOrReloadProductSpecificationTable('Reset');
}

function ExportProductSpecificationData() {
    $('.excelExport').show();
    OnServerCallBegin();
    BindOrReloadProductSpecificationTable('Export');
}

function EditProductSpecificationMaster(thisObj) {
    debugger;
    productSpecificationVM = _dataTables.ProductSpecificationList.row($(thisObj).parents('tr')).data();
    GetMasterPartial('ProductSpecification', productSpecificationVM.Code);
    $('#h3ModelMasterContextLabel').text('Edit ProductSpecification')
    $('#divModelMasterPopUp').modal('show');
    $('#hdnMasterCall').val('MSTR');
}
function DeleteProductSpecificationMaster(thisObj) {
    debugger;
    productSpecificationVM = _dataTables.ProductSpecificationList.row($(thisObj).parents('tr')).data();
    notyConfirm('Are you sure to delete?', 'DeleteProductSpecification("' + productSpecificationVM.Code + '")');
}

function DeleteProductSpecification(code) {
    debugger;
    try {
        if (code) {
            var data = { "code": code };         
            _jsonData = GetDataFromServer("ProductSpecification/DeleteProductSpecification/", data);
            if (_jsonData != '') {
                _jsonData = JSON.parse(_jsonData);
                _message = _jsonData.Message;
                _status = _jsonData.Status;
                _result = _jsonData.Record;
            }
            switch (_status) {
                case "OK":
                    notyAlert('success', _result.Message);
                    BindOrReloadProductSpecificationTable('Reset');
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