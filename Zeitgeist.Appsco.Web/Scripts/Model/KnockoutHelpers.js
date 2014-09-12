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
        //$('#range_date').dateRangeSlider(
        $(element).dateRangeSlider(
        {
            bounds: {
                min: new Date(2014, 7, 15),
                max: new Date()
            },
            defaultValues: {
                //min: new Date(2014, 8, 1),
                //max: new Date()
                min: min(),
                max: max()
            },
            step: {
                days: 1
            }
        }
        );
        $(element).on("valuesChanging", function (e, data) {
            if (!isUpdating) {
                isUpdating = true;
                min(data.values.min);
                max(data.values.max);
                update();
                isUpdating = false;
            }
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

ko.toJS2 = function (model) {
    return JSON.parse(ko.toJSON(model, modelSerializer));
}

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