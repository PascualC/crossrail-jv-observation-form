//Time Spinner
//------------
// - Simple implementation that extends jQuery UI spinner as taken from the jQuery UI website: http://jqueryui.com/spinner/#time
// - Relies on Globalize so that times and dates can be correctly parsed and displayed

    $.widget("ui.timespinner", $.ui.spinner, {
        options: {
            // seconds
            step: 60 * 1000,
            // hours
            page: 60
        },

        _parse: function (value) {
            if (typeof value === "string") {
                // already a timestamp
                if (Number(value) == value) {
                    return Number(value);
                }
                return +Globalize.parseDate(value);
            }
            return value;
        },

        _format: function (value) {
            return Globalize.format(new Date(value), "t");
        }
    });