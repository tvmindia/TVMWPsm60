/*****************************************************************************/
//AUTHOR: ARUL VYAS
//DATE  : 25-JUN-2018
//FILE  : RecentDocument.js
/*****************************************************************************/

$(document).ready(function () {
    try {
        debugger;

        $('#SearchTerm').keydown(function (event) {
            if (event.which === 13) {
                debugger;
                event.preventDefault();
                SearchDocument();
            }
        });

    } catch (ex) {
        console.log(ex.message);
    }
});


function SearchDocument() {
    try {
        debugger;
        var searchTerm = $("#SearchTerm").val().replace(/ /g, "");
        $('#divSearchResults').load("DashBoard/SearchDocument?searchTerm=" + searchTerm, function () {
            $('#msgSearchValue').append($("#SearchTerm").val());
        });
    }
    catch (ex) {
        console.log(ex);
    }
}
