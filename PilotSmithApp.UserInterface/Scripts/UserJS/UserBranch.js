var _dataTables = {};
var _emptyGuid = "00000000-0000-0000-0000-000000000000";
//---------------------------------------Docuement Ready--------------------------------------------------//

$(document).ready(function () {
    try {
        _dataTables.UserInBranchTable = $('#tblUserInBranch').DataTable(
         {
             dom: '<"pull-right"Bf>rt<"bottom"ip><"clear">',
             order: [],
             searching: false,
             paging: false,
             data:null,
             pageLength: 15,
             language: {
                 search: "_INPUT_",
                 searchPlaceholder: "Search"
             },
             columns: [
             
               { "data": "Branch.Description", "defaultContent": "<i>-</i>" },
               {
                   "data": "HasAccess", render: function (data, type, row) {
                       debugger;
                   if (data)
                       return '<input type="checkbox" name="HasAccess" value=' + row.BranchCode + ' onclick="HasAccessCheckBoxChange(this)" checked>'
                   else
                       return '<input type="checkbox" name="HasAccess" value=' + row.BranchCode + ' onclick="HasAccessCheckBoxChange(this)" >'
               }, "defaultContent": "<i>-<i>"
               },
               {
                   "data": "IsDefault", render: function (data, type, row) {
                       debugger;
                       if (data)
                           return '<input type="checkbox" name="IsDefault" class="radio" value=' + row.BranchCode + ' checked onclick="SetDefaultUserInBranch(this)">'
                       else
                           return '<input type="checkbox" name="IsDefault" class="radio" value=' + row.BranchCode + ' onclick="SetDefaultUserInBranch(this)">'
                   }, "defaultContent": "<i>-<i>"
               },
             ],
             columnDefs: [{ "targets": [], "visible": false, "searchable": false },
                  { className: "text-right", "targets": [] },
                   { className: "text-left", "targets": [0,1] },
             { className: "text-center", "targets": [2] }

             ]
         });

    } catch (x) {

        notyAlert('error', x.message);

    }

});

//To Bind the table on user dropdown change
function UserOnChange(this_obj) {
    debugger;
    if ($('#UserID').val() != "") {
        _dataTables.UserInBranchTable.clear().rows.add(GetAllUserInBranchByUserId(this_obj.value)).draw(false);

    }
    else {
        var array = [];
        _dataTables.UserInBranchTable.clear().rows.add(array).draw(false);
    }
}

//Function To GetAllUserInBranchByUserId 
function GetAllUserInBranchByUserId(id) {
    try {
        
        debugger;
        var data = { "userId": id};
        ds = GetDataFromServer("UserInBranch/GetAllUserInBranchByUserId/", data);
        debugger;
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {
            return ds.Records;
        }
        if (ds.Result == "ERROR") {
            alert(ds.Message);
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}


// the selector will match all input controls of type :checkbox
// and attach a click event handler 
function SetDefaultUserInBranch(this_Obj) {
    debugger;
    // in the handler, 'this_Obj' refers to the box clicked on
    var $box = $(this_Obj);
    if ($box.is(":checked")) {
        // the name of the box is retrieved using the .attr() method
        // as it is assumed and expected to be immutable
        var group = "input:checkbox[name='" + $box.attr("name") + "']";
        // the checked state of the group/box on the other hand will change
        // and the current value is retrieved using .prop() method
        $(group).prop("checked", false);
        $box.prop("checked", true);
        $('input:checkbox[name=HasAccess][value=' + this_Obj.value + ']').prop('checked', true);
    } else {
        $box.prop("checked", true);
    }
};

//Function On Save 
function SaveChanges() {
    debugger;
    $("#btnSave").trigger('click');
    if ($('#UserID').val() != "") {

        var hasAccessBranchCode = [];
        $.each($("input[name='HasAccess']:checked"), function () {
            hasAccessBranchCode.push($(this).val());
        });
        var userId = $('#UserID').val();
        var defaultBranchCode = $("input[name='IsDefault']:checked").val();
        var data = { "userId": userId, "hasAccess": hasAccessBranchCode.toString(), "isDefault": defaultBranchCode };
        _jsonData = GetDataFromServer("UserInBranch/InserUpdateUserInBranch/", data);
        debugger;
        if (_jsonData != '') {
            _jsonData = JSON.parse(_jsonData);
            _message = _jsonData.Message;
            _status = _jsonData.Status;
            _result = _jsonData.Record;
        }
        switch (_status) {
            case "OK":
                notyAlert('success', _result.Message);
                break;
            case "ERROR":
                notyAlert('error', _message);
                break;
            default:
                break;
        }
    }
}

//To check the branch isdefault and to set its  HasAccess on HasAccessCheckBox click
function HasAccessCheckBoxChange(this_Obj) {
    debugger;
    if ($('input:checkbox[name=IsDefault][value=' + this_Obj.value + ']').is(':checked')) {
        $(this_Obj).prop('checked', true)
    }
}