var _dataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
var _jsonData = {};
var _message = "";
var _status = "";
var _result = "";
$(document).ready(function () {
    try {
        BindOrReloadProductCategoryTable('Init');
        $('#tblProductCategory tbody').on('dblclick', 'td', function () {
            EditProductCategoryMaster(this);
        });
    }
    catch (e) {
        console.log(e.message);
    }
});
//function bind the Product Category list and cheking and filter
function BindOrReloadProductCategoryTable(action)
{
    try {
        //creating advancesearch object
        ProductCategoryAdvanceSearchViewModel = new Object();
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
                if ($('#SearchTerm').val() == '')
                {
                    return true;
                }
                break;
            case 'Export':
                DataTablePagingViewModel.Length = -1;
                break;
            default:
                break;
        }
        ProductCategoryAdvanceSearchViewModel.DataTablePaging = DataTablePagingViewModel;
        ProductCategoryAdvanceSearchViewModel.SearchTerm = $('#SearchTerm').val();
        //apply datatable plugin on bank table
        _dataTables.ProductCategoryList = $('#tblProductCategory').DataTable(
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
                    
                    url: "ProductCategory/GetAllProductCategory",
                    data: { "productCategoryAdvanceSearchVM": ProductCategoryAdvanceSearchViewModel },
                    type: 'POST'
                },
                pageLength: 10,
                columns: [
                { "data": "Description", "defaultContent": "<i>-</i>"},
                { "data": "PSASysCommon.CreatedDateString", "defaultContent": "<i>-</i>"},
                {
                    "data": null, "orderable": false, "defaultContent": '<a href="#" onclick="EditProductCategoryMaster(this)"<i class="fa fa-pencil-square-o" aria-hidden="true"></i></a>  <a href="#" onclick="DeleteProductCategoryMaster(this)"<i class="fa fa-trash-o" aria-hidden="true"></i></a>'
                }
                ],
                columnDefs: [{ "targets": [], "visible": false, "searchable": false },
                { className: "text-center", "targets": [1,2] },
                { "targets": [0], "width": "60%" },
                { "targets": [1], "width": "30%" },
                {"targets":[2],"width":"10%"}
                ],
                destroy: true,
                initComplete: function (settings, json) {
                    $('.dataTables_wrapper div.bottom div').addClass('col-md-6');
                    $('#tblProductCategory').fadeIn(100);
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
                        BindOrReloadProductCategoryTable();
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
function ResetProductCategoryList()
{
    BindOrReloadProductCategoryTable('Reset');
}

function ExportProductCategoryData()
{
    $('.excelExport').show();
    OnServerCallBegin();
    BindOrReloadProductCategoryTable('Export');
}

function EditProductCategoryMaster(thisObj)
{
    debugger;
    productCategoryVM = _dataTables.ProductCategoryList.row($(thisObj).parents('tr')).data();

    $("#divMasterBody").load("ProductCategory/MasterPartial?masterCode=" + productCategoryVM.Code, function (responseTxt, statusTxt, xhr) {
        if (statusTxt == "success") {
            $('#hdnMasterCall').val('MSTR');
            $('#lblModelMasterContextLabel').text('Edit Category Information')
            $('#divModelMasterPopUp').modal('show');
        }
        else {
            console.log("Error: " + xhr.status + ": " + xhr.statusText);
        }
    });

    //GetMasterPartial('ProductCategory', productCategoryVM.Code);
    //$('#h3ModelMasterContextLabel').text('Edit ProductCategory')
    //$('#divModelMasterPopUp').modal('show');
    //$('#hdnMasterCall').val('MSTR');  
}
function DeleteProductCategoryMaster(thisObj) {
    debugger;
    productCategoryVM = _dataTables.ProductCategoryList.row($(thisObj).parents('tr')).data();
    notyConfirm('Are you sure to delete?', 'DeleteProductCategory("' + productCategoryVM.Code + '")');
}

function DeleteProductCategory(code)
{
    debugger;
    try
    {
        if(code)
        {
            var data = { "code": code };           
            _jsonData = GetDataFromServer("ProductCategory/DeleteProductCategory/", data);
            if (_jsonData != '')
            {
                _jsonData = JSON.parse(_jsonData);
                _message = _jsonData.Message;
                _status = _jsonData.Status;
                _result = _jsonData.Record;
            }
            switch (_status)
            {
                case "OK":
                    notyAlert('success', _result.Message);
                    BindOrReloadProductCategoryTable('Reset');
                    break;
                case "ERROR":
                    notyAlert('error', _message);
                    break;
                default:
                    break;
            }
        }
    }
    catch(e)
    {
        console.log(e.message);
    }
}