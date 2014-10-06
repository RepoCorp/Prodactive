ko.extenders.required = function (target, overrideMessage) {
    //add some sub-observables to our observable
    target.hasError = ko.observable();
    target.validationMessage = ko.observable();

    //define a function to do validation
    function validate(newValue) {
        target.hasError(newValue ? false : true);
        target.validationMessage(newValue ? "" : overrideMessage || "This field is required");
    }

    //initial validation
    validate(target());

    //validate whenever the value changes
    target.subscribe(validate);

    //return the original observable
    return target;
};

ko.bindingHandlers.dateRange = {
    init: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
        var val = valueAccessor();

        var min = allBindingsAccessor.get("min"); //es un observable 
        var max = allBindingsAccessor.get("max");
        var update = allBindingsAccessor.get("update");
        var isUpdating = false;
        $(element).dateRangeSlider(
        {
            bounds: {
                min: new Date(2014, 7, 15),
                max: new Date()
            },
            defaultValues: {
                min: min(),
                max: max()
            },
            step: {
                days: 1
            }
        }
        );
        $(element).on("valuesChanging", function (e, data) {
                min(data.values.min);
                max(data.values.max);
                update();
        });

    }
    ,
    update: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
        var val = valueAccessor();
        var date = moment(ko.utils.unwrapObservable(val));
    }
};


ko.bindingHandlers.moment = {
    init: function (element, valueAccessor, allBindingsAccessor, viewModel,bindingContext) {
        var val = valueAccessor();
        var date = moment(ko.utils.unwrapObservable(val));
        var format = allBindingsAccessor().format || 'YYYY-MM-DD';
        $(element).change(function () {
            var value = valueAccessor();
            value($(element).val());
        });
        element.value = date.format(format);
    }
    ,
    update: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
        var val = valueAccessor();
        var date = moment(ko.utils.unwrapObservable(val));
        var format = allBindingsAccessor().format || 'YYYY-MM-DD';
        element.value = date.format(format);

    }
};

ko.bindingHandlers.date = {
    init:  function (element, valueAccessor, allBindingsAccessor) {
        var value = valueAccessor();
        var allBindings = allBindingsAccessor();
        var valueUnwrapped = ko.utils.unwrapObservable(value);

        // Date formats: http://momentjs.com/docs/#/displaying/format/
        var pattern = allBindings.format || 'YYYY-MM-DD';

        var output = "-";
        if (valueUnwrapped !== null && valueUnwrapped !== undefined && (valueUnwrapped.length > 0 || typeof (valueUnwrapped) === typeof (new Date()))) {
            output = moment(valueUnwrapped).format(pattern);
        }

        if ($(element).is("input") === true) {
            $(element).val(output);
        } else {
            $(element).text(output);
        }
    },
    update: function (element, valueAccessor, allBindingsAccessor) {
        var value = valueAccessor();
        var allBindings = allBindingsAccessor();
        var valueUnwrapped = ko.utils.unwrapObservable(value);

        // Date formats: http://momentjs.com/docs/#/displaying/format/
        var pattern = allBindings.format || 'DD/MM/YYYY';

        var output = "-";
        if (valueUnwrapped !== null && valueUnwrapped !== undefined && valueUnwrapped.length > 0) {
            output = moment(valueUnwrapped).format(pattern);
        }

        if ($(element).is("input") === true) {
            $(element).val(output);
        } else {
            $(element).text(output);
        }
    }
};


//
var formatNumber = function (element, valueAccessor, allBindingsAccessor, format) {
    // Provide a custom text value
    var value = valueAccessor(), allBindings = allBindingsAccessor();
    var numeralFormat = allBindingsAccessor.numeralFormat || format;
    var strNumber = ko.utils.unwrapObservable(value);
    if (strNumber) {
        return numeral(strNumber).format(numeralFormat);
    }
    return '';
};

ko.bindingHandlers.numeraltext = {
    init: function (element, valueAccessor, allBindingsAccessor) {
        //$(element).text(formatNumber(element, valueAccessor, allBindingsAccessor, "(0,0.00)"));
        $(element).text(formatNumber(element, valueAccessor, allBindingsAccessor, "(0,0)"));
    },
    update: function (element, valueAccessor, allBindingsAccessor) {
        //$(element).text(formatNumber(element, valueAccessor, allBindingsAccessor, "(0,0.00)"));
        $(element).text(formatNumber(element, valueAccessor, allBindingsAccessor, "(0,0)"));
    }
};

ko.bindingHandlers.numeralvalue = {
    init: function (element, valueAccessor, allBindingsAccessor) {
        $(element).val(formatNumber(element, valueAccessor, allBindingsAccessor, "(0,0)"));

        //handle the field changing
        ko.utils.registerEventHandler(element, "change", function () {
            var observable = valueAccessor();
            observable($(element).val());
        });
    },
    update: function (element, valueAccessor, allBindingsAccessor) {
        $(element).val(formatNumber(element, valueAccessor, allBindingsAccessor, "(0,0)"));
    }
};

ko.bindingHandlers.percenttext = {
    init: function (element, valueAccessor, allBindingsAccessor) {
        $(element).text(formatNumber(element, valueAccessor, allBindingsAccessor, "(0.000 %)"));
    },
    update: function (element, valueAccessor, allBindingsAccessor) {
        $(element).text(formatNumber(element, valueAccessor, allBindingsAccessor, "(0.000 %)"));
    }
};

ko.bindingHandlers.percentvalue = {
    init: function (element, valueAccessor, allBindingsAccessor) {
        $(element).val(formatNumber(element, valueAccessor, allBindingsAccessor, "(0.000 %)"));

        //handle the field changing
        ko.utils.registerEventHandler(element, "change", function () {
            var observable = valueAccessor();
            observable($(element).val());
        });
    },
    update: function (element, valueAccessor, allBindingsAccessor) {
        $(element).val(formatNumber(element, valueAccessor, allBindingsAccessor, "(0.000 %)"));
    }
};

ko.bindingHandlers.countdown = {
    init: function (element, valueAccessor, allBindingsAccessor) {
        var value = valueAccessor();
        $(element).text(countdown(value()).toString());
    },
    update: function (element, valueAccessor, allBindingsAccessor) {
        var value = valueAccessor();
        $(element).text(countdown(value()).toString());
    }
}


ko.bindingHandlers.timeAgo = {
    init: function (element, valueAccessor) {
        var val = ko.utils.unwrapObservable(valueAccessor()),
            date = new Date(val), // WARNING: this is not compatibile with IE8
            timeAgo = toTimeAgo(date);
        return ko.bindingHandlers.html.update(element, function () {
            return '<time datetime="' + encodeURIComponent(val) + '">' + timeAgo + '</time>';
        });
    },
    update: function (element, valueAccessor) {
        var val = ko.utils.unwrapObservable(valueAccessor()),
            date = new Date(val), // WARNING: this is not compatibile with IE8
            timeAgo = toTimeAgo(date);
        return ko.bindingHandlers.html.update(element, function () {
            return '<time datetime="' + encodeURIComponent(val) + '">' + timeAgo + '</time>';
        });
    }
};

ko.toJS2 = function (model) {
    return JSON.parse(ko.toJSON(model, modelSerializer));
}



function toTimeAgo(dt) {
    var secs = (((new Date()).getTime() - dt.getTime()) / 1000),
        days = Math.floor(secs / 86400);
    /*
    return days === 0 && (
        secs < 60 && "just now" ||
            secs < 120 && "a minute ago" ||
            secs < 3600 && Math.floor(secs / 60) + " minutes ago" ||
            secs < 7200 && "an hour ago" ||
            secs < 86400 && Math.floor(secs / 3600) + " hours ago") ||
        days === 1 && "yesterday" ||
        days < 31 && days + " days ago" ||
        days < 60 && "one month ago" ||
        days < 365 && Math.ceil(days / 30) + " months ago" ||
        days < 730 && "one year ago" ||
        Math.ceil(days / 365) + " years ago";
        */
    return days === 0 && (
       secs < 60 && "ahora mismo" ||
           secs < 120 && "hace un minuto" ||
           secs < 3600 && "hace " + Math.floor(secs / 60) + " minutos" ||
           secs < 7200 && "hace una hora" ||
           secs < 86400 && "hace " + Math.floor(secs / 3600) + " horas") ||
       days === 1 && "ayer" ||
       days < 31 && "hace " + days + " dias" ||
       days < 60 && "hace un mes" ||
       days < 365 && "hace " + Math.ceil(days / 30) + " meses" ||
       days < 730 && "hace un año" ||
       "hace" + Math.ceil(days / 365) + " años";
};

function modelSerializer(key, value) {
    if (isSerializable(value))
        return value;
    else
        return;
}

function isSerializable(object) {
    if (object == null) return true;
    if (typeof object == 'function') return false;
    if (object.mappedProperties != null) return false;

    return true;
}
