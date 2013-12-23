var Crossrail = Crossrail || {};

Crossrail.Forms = (function () {

    var selectorObservationType = "input:radio[name='observation-type']";
    var selectorObservationCategory = "input:radio[name='category']";
    var selectorObservationEmail = "input:checkbox[name='email']";
    var selectorTimeSpinner = '.time-spinner';
    var selectorDatePicker = '.date';

    var instance = {};

    instance.initialise = function () {

        //Rich controls for date picker and time spinner.

        var timeSpinners = $(selectorTimeSpinner);
        var datePickers = $(selectorDatePicker);

        if (!Crossrail.isTouch) {
            timeSpinners.timespinner();
            datePickers.datepicker({ "dateFormat": Crossrail.dateFormatUi });
        }
        else {
            //Rely on built-in HTML5 input types for touch screen
            //devices instead of forcing jQuery UI which is hard
            //work for end users.

            timeSpinners.attr("type", "time").removeClass("textbox");
            datePickers.attr("type", "date").removeClass("textbox");
        }

        //Set-up validation for this form. 

        $("form").validate({
            errorPlacement: function (error, element) {
                error.appendTo(element.parents(".field-value"));
            },

            messages: {
                date: "The date is required.",
                time: "The time is required."
            },

            //Modify highlight / un-highlight such that it can support adding the error class
            //onto jQuery UI widgets that are wrapped around certain elements such as spinner.

            highlight: function (element, errorClass, validClass) {
                $(element).addClass(errorClass).removeClass(validClass);
                $(element).parent(".ui-widget").addClass(errorClass).removeClass(validClass);
            },
            unhighlight: function (element, errorClass, validClass) {
                $(element).removeClass(errorClass).addClass(validClass);
                $(element).parent(".ui-widget").removeClass(errorClass).addClass(validClass);
            }
        });

        //Set-up dynamic form elements which show other elements based on selection.

        instance.setupRadioClick(selectorObservationType, "UnsafeCondition", "#observation-category", true);
        instance.setupRadioClick(selectorObservationCategory, "Other catagory not mentioned e.g paperwork", "#observation-area", true);
        instance.setupRadioClick(selectorObservationEmail, "Yes", "#observation-email");
    };

    //Helper to enable a specific radio button group value to show or hide
    //a target selector based on the value passed in.

    instance.setupRadioClick = function (selector, selctorValue, selectorDisplay, showOnEmpty) {

        $(selector).click(function () {
            updateDisplay();
        });

        updateDisplay();

        //Helper for setupRadioClick function

        function updateDisplay() {

            var selectorChecked = selector + ":checked";
            var value = $(selectorChecked).val();
            var isValid = value == selctorValue;
            var $display = $(selectorDisplay);

            if (isValid || (showOnEmpty && !value)) {
                $display.slideDown(750);
            }
            else {
                $display.slideUp(750);
            }
        }
    };

    $(function () {
        instance.initialise();
    });

})();