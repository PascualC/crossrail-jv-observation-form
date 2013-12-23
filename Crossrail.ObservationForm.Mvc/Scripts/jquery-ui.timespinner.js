//Time Spinner
//------------
// - Simple implementation that extends jQuery UI spinner as taken from the jQuery UI website: http://jqueryui.com/spinner/#time
// - Relies on Globalize so that times and dates can be correctly parsed and displayed

(function ( $ ) {

    $.widget("ui.timespinner", $.ui.spinner, {
        options: {
            // seconds
            step: 60 * 1000,
            // hours
            page: 60
        },

        //_keydown: function (event) {
        //    var options = this.options,
        //        keyCode = $.ui.keyCode;

        //    switch (event.keyCode) {
        //        case keyCode.UP:
        //            this._repeat(null, 1, event);
        //            return true;
        //        case keyCode.DOWN:
        //            this._repeat(null, -1, event);
        //            return true;
        //        case keyCode.PAGE_UP:
        //            this._repeat(null, options.page, event);
        //            return true;
        //        case keyCode.PAGE_DOWN:
        //            this._repeat(null, -options.page, event);
        //            return true;
        //    }

        //    return false;
        //},

        _parse: function (value) {
            if (typeof value === "string") {
                // NOTE: already a timestamp seems to accept plain ol' numbers like 12.5, etc.
                //if (Number(value) == value) {
                //    return Number(value);
                //}

                var date = +Globalize.parseDate(value, "HH:mm");

                //If we don't have a valid value, set the textbox
                //back to the current time.

                if (date == 0) {
                    this.value(this._format(new Date().getTime()));
                }

                return date;
            }
            return value;
        },

        _format: function (value) {
            return Globalize.format(new Date(value), "HH:mm");
        }
    });

}(jQuery));