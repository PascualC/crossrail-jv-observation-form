var Crossrail = Crossrail || {};

//Set-up validation for this form. 

$.validator.setDefaults({

    //Modify highlight / un-highlight such that it can support adding the error class
    //onto jQuery UI widgets that are wrapped around certain elements such as spinner.

    //The jQuery UI widgets use a different error class because something bizarre appears
    //to happen with this and unobtrusive validation and the time picker. Use the same
    //error class and watch the time picker have "display:none" applied?

    highlight: function (element, errorClass, validClass) {

        $(element).addClass(errorClass).removeClass(validClass);
        $(element).parent(".ui-widget").addClass("ui-widget-error").removeClass(validClass);
    },
    unhighlight: function (element, errorClass, validClass) {

        $(element).removeClass(errorClass).addClass(validClass);
        $(element).parent(".ui-widget").removeClass("ui-widget-error").addClass(validClass);
    }
});

Crossrail.Forms = (function () {

    var selectorObservationType = "input:radio[name='ObservationTypeId']";
    var selectorObservationCategory = "input:radio[name='ObservationCategoryId']";
    var selectorObservationEmail = "input:checkbox[name='EmailYesOrNo']";
    var selectorTimeSpinner = '.time-spinner';
    var selectorDatePicker = '.date';

    var instance = {};

    instance.initialise = function () {

        //Rich controls for date picker and time spinner.

        var timeSpinners = $(selectorTimeSpinner);
        var datePickers = $(selectorDatePicker);

        if (!Crossrail.isTouch) {
            if (timeSpinners.length > 0) {
                timeSpinners.timespinner();
            }
            datePickers.datepicker({ "dateFormat": Crossrail.dateFormatUi });
        }
        else {
            //Rely on built-in HTML5 input types for touch screen
            //devices instead of forcing jQuery UI which is hard
            //work for end users.

            timeSpinners.attr("type", "time").removeClass("textbox");
            datePickers.attr("type", "date").removeClass("textbox");
        }

        $("input[type=submit], a, button").button();

        //Set-up dynamic form elements which show other elements based on selection.

        instance.setupRadioClick(selectorObservationType, "2", "#observation-category", true);
        instance.setupRadioClick(selectorObservationCategory, "11", "#observation-area", true);
        instance.setupRadioClick(selectorObservationEmail, "True", "#observation-email");
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