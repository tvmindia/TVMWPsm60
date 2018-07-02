﻿
var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
function LoadApprovalDocument() {
    debugger;
    ValidateDocumentsApprovalPermission();
    try {
        DataTables.ApprovalHistoryTable = $('#tblApprovalHistory').DataTable(
        {
            dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
            ordering: false,
            searching: false,
            paging: false,
            bInfo: false,
            data: GetApprovalHistory(),//ApprovalHistory.js
            autoWidth: false,
            columns: [
            { "data": "ApproverName", "defaultContent": "<i>-</i>", "width": "20%" },
            { "data": "ApproverLevel", "defaultContent": "<i>-</i>", "width": "5%" },
            { "data": "ApprovalDate", "defaultContent": "<i>-</i>", "width": "20%" },
            { "data": "Remarks", "defaultContent": "<i>-</i>", "width": "35%" },
            { "data": "ApprovalStatus", "defaultContent": "<i>-</i>", "width": "20%" },
            ],
            columnDefs: [
                { className: "text-center", "targets": [2] },
                { className: "text-right", "targets": [] },
                { className: "text-left", "targets": [0, 1, 3] }
            ]
        });
    }
    catch (e) {
        console.log(e.message);
    }
    }
    

function ConfirmApproveDocument() {
    notyConfirm('Are you sure to Approve Document?', 'ApproveDocument()', '', 'Yes, approve it!');
}

    function ApproveDocument() {
        try {
            debugger;
            var DocumentID = $("#DocumentID").val();
            var ApprovalLogID = $("#ID").val();
            var DocumentTypeCode = $("#DocumentType").val();
            var Remarks = $('#Remarks').val();

            var data = { "ApprovalLogID": ApprovalLogID, "DocumentID": DocumentID, "DocumentTypeCode": DocumentTypeCode, "Remarks": Remarks };
            var ds = {};
            ds = GetDataFromServer("DocumentApproval/ApproveDocumentInsert/", data);
            if (ds != '') {
                ds = JSON.parse(ds);
            }
            if (ds.Result == "OK") {
                notyAlert('success', ds.Records.Message);
                DataTables.ApprovalHistoryTable.clear().rows.add(GetApprovalHistory()).draw(false);
                DisableButtons();
                ReloadSummary(DocumentID, DocumentTypeCode);

            }
            if (ds.Result == "ERROR") {
                alert(ds.Message);
            }
        }
        catch (e) {
            notyAlert('error', e.message);
        }
    }
    function ConfirmRejectDocument() {
        notyConfirm('Are you sure to Reject Document?', 'RejectDocument()', '', 'Yes, reject it!');
    }

    function RejectDocument() {
        try {
            debugger;
            var DocumentID = $("#DocumentID").val();
            var ApprovalLogID = $("#ID").val();
            var DocumentTypeCode = $("#DocumentType").val();
            var Remarks = $('#Remarks').val();

            if (Remarks == "")
                notyAlert('warning', 'Remarks Field is Empty');
            else {
                var data = { "ApprovalLogID": ApprovalLogID, "DocumentID": DocumentID, "DocumentTypeCode": DocumentTypeCode, "Remarks": Remarks };
                var ds = {};
                ds = GetDataFromServer("DocumentApproval/RejectDocument/", data);
                if (ds != '') {
                    ds = JSON.parse(ds);
                }
                if (ds.Result == "OK") {
                    notyAlert('success', ds.Records.Message);
                    DataTables.ApprovalHistoryTable.clear().rows.add(GetApprovalHistory()).draw(false);
                    DisableButtons();
                    ReloadSummary(DocumentID, DocumentTypeCode);
                }
                if (ds.Result == "ERROR") {
                    alert(ds.Message);
                }
            }
        }
        catch (e) {
            notyAlert('error', e.message);
        }
    }

    function ReloadSummary(DocumentID, DocumentTypeCode) {
        $("#DocumentSummarydiv").load("./DocumentSummary?DocumentID=" + DocumentID + "&DocumentTypeCode=" + DocumentTypeCode);
    }

    function ValidateDocumentsApprovalPermission() {
        debugger;
        var DocumentID = $("#DocumentID").val();
        var DocumentTypeCode = $("#DocumentType").val();

        var data = { "DocumentID": DocumentID, "DocumentTypeCode": DocumentTypeCode };
        var ds = {};
        ds = GetDataFromServer("DocumentApproval/ValidateDocumentsApprovalPermission/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        debugger;

        if (ds.Records.Status == "False") {
            DisableButtons();
        }
    }

    function DisableButtons() {
        $("#Remarks").attr("disabled", "disabled");
        $("#btnApproveDocument").attr("disabled", "disabled");
        $("#btnApproveDocument").prop("onclick", null);
        $("#btnRejectDocument").attr("disabled", "disabled");
        $("#btnRejectDocument").prop("onclick", null);
    }
