var Crossrail = Crossrail || {};

Crossrail = (function () {

    var culture = 'en-GB';

    //Date format includes jQuery UI + RFC33939 format for HTML5 date input.
    //Likely will need changing when moving to another culture / language.

    var dateFormats = ['dd/MM/yyyy', 'yyyy-MM-dd'];
    var dateFormatUi = 'dd/mm/yy';

    //Set-up culture and change validation to match new culture.
    $.culture = Globalize.culture(culture);

    $.validator.methods.date = function (value, element) {
        //Rely on globalize to parse the date so that we can support multiple cultures.
        var date = Globalize.parseDate(value, dateFormats, culture);
        return this.optional(element) || date != null;
    };

    return {
        isTouch: (('ontouchstart' in window) || (navigator.msMaxTouchPoints > 0)),
        culture: culture,
        dateFormat: dateFormats,
        dateFormatUi: dateFormatUi,
    };

})();