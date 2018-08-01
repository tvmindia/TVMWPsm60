var DataTables = {};

function GetApprovalHistory() {
    try {
        debugger;
        var DocumentID = $("#DocumentID").val();
        var DocumentTypeCode = $("#DocumentType").val();
        var data = { "DocumentID": DocumentID, "DocumentTypeCode": DocumentTypeCode };
        var ds = {};
        ds = GetDataFromServer("DocumentApproval/GetApprovalHistory/", data);
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
function GetOwnershipHistory() {
    try {
        debugger;
        var DocumentID = $("#DocumentID").val();
        var DocumentTypeCode = $("#DocumentType").val();
        var data = { "documentID": DocumentID, "documentTypeCode": DocumentTypeCode };
        var ds = {};
        ds = GetDataFromServer("TakeOwnership/GetOwnershipHistory/", data);
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
function BindApprovalHistoryTable() {
    try {
            var result = GetApprovalHistory();
            $("#tbodyApprovalHistory").empty();
            if (result.length > 0) {
                $.each(result, function (index, Records) {
                    $("#tbodyApprovalHistory").append('<tr><td><span><i class="fa fa-circle-o" style="color:green;"/></span></td><td style="font-weight:500;"><span><i class="fa fa-calendar" style="color:grey;"/></span> ' + (Records.ApprovalDate == null ? "" : Records.ApprovalDate) + '</td><td>' + (Records.ApproverName == null ? "" : Records.ApproverName) + '</td><td>' + (Records.ApproverLevel == null ? "" : Records.ApproverLevel) + '</td><td>' + (Records.Remarks == null ? "" : Records.Remarks) + '</td><td style="font-size: 10px;">' + (Records.ApprovalStatus == null ? "" : Records.ApprovalStatus) + '</td></tr>');
                });
            }
            else {
                $("#tbodyApprovalHistory").append('<tr><td colspan="6">No history for the document</td></tr>');
            }
        }
        catch (e) {
            console.log(e.message);
        }
}

function BindOwnershipHistoryTable() {
    try {
        var result=GetOwnershipHistory();
        $("#tbodyOwnershipHistory").empty();
        if (result.length > 0)
        {
            $.each(result, function (index, Records) {
                $("#tbodyOwnershipHistory").append('<tr><td><span><i class="fa fa-circle-o" style="color:green;"/></span></td><td style="font-weight:500;"><span><i class="fa fa-calendar" style="color:grey;"/></span> ' + Records.DateFormatted + '</td><td>' + Records.Type + '</td><td>' + Records.Remarks + '</td></tr>');
            });
        }
        else
        {
            $("#tbodyOwnershipHistory").append('<tr><td colspan="4">No updates in log</td></tr>');
        }
    }
    catch (e) {
        console.log(e.message);
    }
}
