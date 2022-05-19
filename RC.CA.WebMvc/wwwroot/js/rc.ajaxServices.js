//Consolidate all ajax services 
//
$(document).ready(function () {
    //Add converter to remove the d from data object if it exists
    $.ajaxSetup({
        converters: {
            "json": function(data) {
                return data && data.hasOwnProperty("d") ? data.d : data;
            }
        }
    });
});

var rcAjaxServices = {
    authBearerToken : "",
    getImages: function (data) {
        return $.ajax({
                    type: "GET",
                    url: "/Cdn/Image/ImageListRefresh",
                    contentType: "application/json",
            headers: { 'Authorization': rcAjaxServices .authBearerToken},
                    data: data,
                    dataType: "html"
                });
    },
    testCookies: function () {
        $.ajax({
            url: "https://localhost:7285/api/cdn/Image/TestCookies",
            type: "POST",
            xhrFields: {
                withCredentials: true
            },
            success: function (data, textStatus, jqXHR) {
                alert(data);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                debugger;
                alert("Error");
            }
        });
    }
}   