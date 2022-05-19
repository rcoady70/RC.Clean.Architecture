//
// Form validation extensions to unobtrusive validation. Useful site for regex patterns https://www.html5pattern.com/Miscs
//

//Initialize
//
$(document).ready(function () {
    rcForm.addCustomValidation();
});


var rcForm = {
    addCustomValidation: function () {
        $.validator.addMethod('isValidEircodeIE',
            function (value,element,parameters) {
                return /^([A-Za-z]{2}[\d]{1,2}[A-Za-z]?)[\s]+([\d][A-Za-z]{2})$/.test(value)
            },
            'Enter a valid eircode');
        $.validator.addMethod('isValidZipcodeUS',
            function (value, element, parameters) {
                return /(\d{5}([\-]\d{4})?)/.test(value)
            },
            'Enter a valid zipcode');
        $.validator.addMethod('isValidPostCodeUK',
            function (value, element, parameters) {
                return /(?:^[AC-FHKNPRTV-Y][0-9]{2}|D6W)[ -]?[0-9AC-FHKNPRTV-Y]{4}$/.test(value)
            },
            'Enter a valid postcode');
    },
    //Re-bind jquery form validators
    unBindFormValidators(formId) {
        rcState.debugLog("unBindFormValidators", "unbind form validation");
        var $form = $("#" + formId);
        $form.unbind();
        $form.data("validator", null);
    },
    //Re-bind jquery form validators
    reBindFormValidators(formId) {
        rcState.debugLog("reBindFormValidators", "rebind form validation");
        var $form = $("#" + formId);
        $.validator.unobtrusive.parse($form[0]);
        $form.validate($form.data("unobtrusiveValidation").options);
    }
};


