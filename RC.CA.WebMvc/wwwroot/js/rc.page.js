//
// Script to manage various page functions
//
'use strict'
var rcPage = {
    //Initialize button spinner for submit <buttons>
    initButtonSpinner: function () {
        $("button[data-spinning-button]").on("click.button", function (e) {
            let $this = $(this);
            let formId = $this.data("spinning-button");
            let $form = formId ? $("#" + formId) : $this.parents("form");
            if ($form.length) {
                rcState.debugLog("ButtonSpinner", "trigger button spinner");
                //form.valid() will be applicable If you are using jQuery validate https://jqueryvalidation.org/
                //asp.net mvc used it by default with jQuery Unobtrusive Validation
                //you need to check the form before it goes into the if statement
                if ($form.valid()) {
                    $this.append("&nbsp;<span class='spinner-border spinner-border-sm' role='status' aria-hidden='true'></span>")
                        .attr("disabled", "");
                    $form.submit();
                }
            }
        });
    },
    //Show toast error
    toastError(message,sticky) {
        $.toast({
            heading: 'Error',
            text: message,
            hideAfter: sticky,
            bgColor: '#FF1356',
            textColor: 'white',
            position: 'bottom-right',
            loader: false
        })
    },
    //Display errors returned by api standard base response errors
    displayAjaxErrors(response) {
        debugger;
        if (!response.isSuccess) {
            $(response.validationErrors).each(function (key, value) {
                rcPage.toastError("Upload failed: " + value.errorMessage, true);
            });
        }
    },
}

$(document).ready(function () {
    //Get AntiForgeryToken in case it is needed for ajax-calls
    if ($('input[name="AntiForgeryToken"]').length > 0)
        rcState.antiForgeryToken = $('input[name="AntiForgeryToken"]')[0].value;
    rcState.hostUrl = window.location.origin;

    //Init button spinner submit buttons when clicked
    rcPage.initButtonSpinner();

    //rcAjaxServices.testCookies();
});

