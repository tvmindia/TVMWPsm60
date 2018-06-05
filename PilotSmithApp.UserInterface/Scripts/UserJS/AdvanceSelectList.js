//---------------------------------------Docuement Ready--------------------------------------------------//
$(document).ready(function () {
    try {
        BindCustomerSelectList();
    }
    catch (e) {
        console.log(e.message);
    }
    try {
        BindAreaSelectList();
    }
    catch (e) {
        console.log(e.message);
    }
    try {
        BindReferenceSelectList();
    }
    catch (e) {
        console.log(e.message);
    }
    try {
        BindBranchSelectList();
    }
    catch (e) {
        console.log(e.message);
    }
    try {
        BindDocumentOwnerSelectList();
    }
    catch (e) {
        console.log(e.message);
    }
    $('.select2').addClass('form-control newinput');
});

// Customer SelectList //
function BindCustomerSelectList() {
    $('#AdvCustomerID').select2({
        ajax: {
            type: 'POST',
            dataType: 'json',
            url: "Customer/GetCustomerForSelectListOnDemand/",
            delay: 50,
            data: function (term) {
                return {
                    'searchTerm': term.term = (term.term == null ? "" : term.term )//search term
                };
            },
            processResults: function (data) {
                return {
                    results: data.items
                };
            },
        }
    });
}
// Area SelectList //
function BindAreaSelectList() {
    $('#AdvAreaCode').select2({
        ajax: {
            type: 'POST',
            dataType: 'json',
            url: "Area/GetAreaForSelectListOnDemand/",
            delay: 50,
            data: function (term) {
                return {
                    'searchTerm': term.term = (term.term == null ? "" : term.term)//search term
                };
            },
            processResults: function (data) {
                return {
                    results: data.items
                };
            },
        }
    });
}
// Reference Person SelectList //
function BindReferenceSelectList() {
    $('#AdvReferencePersonCode').select2({
        ajax: {
            type: 'POST',
            dataType: 'json',
            url: "ReferencePerson/GetReferencePersonForSelectListOnDemand/",
            delay: 50,
            data: function (term) {
                return {
                    'searchTerm': term.term = (term.term == null ? "" : term.term)//search term
                };
            },
            processResults: function (data) {
                return {
                    results: data.items
                };
            },
        }
    });
}
// Branch SelectList //
function BindBranchSelectList() {
    $('#AdvBranchCode').select2({
        ajax: {
            type: 'POST',
            dataType: 'json',
            url: "Branch/GetBranchForSelectListOnDemand/",
            delay: 50,
            data: function (term) {
                return {
                    'searchTerm': term.term = (term.term == null ? "" : term.term)//search term
                };
            },
            processResults: function (data) {
                return {
                    results: data.items
                };
            },
        }
    });
}
// DocumentOwner SelectList //
function BindDocumentOwnerSelectList() {
    $('#AdvDocumentOwnerID').select2({
        ajax: {
            type: 'POST',
            dataType: 'json',
            url: "DocumentOwner/GetDocumentOwnerForSelectListOnDemand/",
            delay: 50,
            data: function (term) {
                return {
                    'searchTerm': term.term = (term.term == null ? "" : term.term)//search term
                };
            },
            processResults: function (data) {
                return {
                    results: data.items
                };
            },
        }
    });
}