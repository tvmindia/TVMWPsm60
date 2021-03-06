﻿//---------------------------------------Docuement Ready--------------------------------------------------//
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
    try {

        BindApprovalStatusSelectList();
    }
    catch (e) {
        console.log(e.message);
    }

    try {
        debugger;
        BindPlantSelectList()
    }
    catch (e) {
        console.log(e.message);
    }

    try {
        debugger;
        BindDepartmentSelectList()
    }
    catch (e) {
        console.log(e.message);
    }

    try {
        debugger;
        BindPositionSelectList()
    }
    catch (e) {
        console.log(e.message);
    }
    try {
        debugger;
        BindCountrySelectList()
    }
    catch (e) {
        console.log(e.message);
    }
    try {
        debugger;
        BindCustomerCategorySelectList()
    }
    catch (e) {
        console.log(e.message);
    }
    try {
        debugger;
        BindDistrictSelectList()
    }
    catch (e) {
        console.log(e.message);
    }
    try {
        debugger;
        BindStateSelectList()
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
            url: "/Customer/GetCustomerForSelectListOnDemand/",
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
// Area SelectList //
function BindAreaSelectList() {
    debugger;
    $('#AdvAreaCode').select2({
        ajax: {
            type: 'POST',
            dataType: 'json',
            url: "/Area/GetAreaForSelectListOnDemand/",
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
            url: "/ReferencePerson/GetReferencePersonForSelectListOnDemand/",
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
            url: "/Branch/GetBranchForSelectListOnDemand/",
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
        if (window.location.search.split('DocumentOwnerID=')[1]==undefined){
        $('#AdvDocumentOwnerID').select2({
            ajax: {
                type: 'POST',
                dataType: 'json',
                url: "/DocumentOwner/GetDocumentOwnerForSelectListOnDemand/",
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
}

// ApprovalStatus SelectList //
function BindApprovalStatusSelectList() {
    debugger;
    $('#AdvApprovalStatusCode').select2({
        ajax: {
            type: 'POST',
            dataType: 'json',
            url: "/ApprovalStatus/GetApprovalStatusSelectListOnDemand/",
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

// Plant SelectList //
function BindPlantSelectList() {
    debugger;
    $('#AdvPlantCode').select2({
        ajax: {
            type: 'POST',
            dataType: 'json',
            url: "/Plant/GetPlantSelectListOnDemand/",
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

//Department SelectList//

function BindDepartmentSelectList() {
    debugger;
    $('#AdvDepartmentCode').select2({
        ajax: {
            type: 'POST',
            dataType: 'json',
            url: "/Department/GetDepartmentForSelectListOnDemand/",
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

//Position SelectList//
function BindPositionSelectList() {
    debugger;
    $('#AdvPositionCode').select2({
        ajax: {
            type: 'POST',
            dataType: 'json',
            url: "/Position/GetPositionForSelectListOnDemand/",
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

function BindCountrySelectList()
{
    $('#AdvCountryCode').select2({
        ajax: {
            type: 'POST',
            dataType: 'json',
            url: "/Country/GetCountryForSelectListOnDemand/",
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

function BindStateSelectList()
{  
    $('#AdvStateCode').select2({
            ajax: {
                type: 'POST',
                dataType: 'json',
                url: "/State/GetStateForSelectListOnDemand/",
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


function BindDistrictSelectList() {
    $('#AdvDistrictCode').select2({
        ajax: {
            type: 'POST',
            dataType: 'json',
            url: "/District/GetDistrictForSelectListOnDemand/",
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



function BindCustomerCategorySelectList() {
    $('#AdvCustomerCategoryCode').select2({
        ajax: {
            type: 'POST',
            dataType: 'json',
            url: "/CustomerCategory/GetCustomerCategoryForSelectListOnDemand/",
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

