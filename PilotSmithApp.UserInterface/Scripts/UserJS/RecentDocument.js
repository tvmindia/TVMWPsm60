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

        $('#zoomBtn').click(function () {
            $('.zoom-btn-sm').toggleClass('scale-out');
            $('.zoom-fab-label').toggleClass('scale-out');
            $('#zoomBtn>i').toggleClass("fa-plus");
            $('#zoomBtn>i').toggleClass("fa-close");
            $(".box").toggleClass('blur');        
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
