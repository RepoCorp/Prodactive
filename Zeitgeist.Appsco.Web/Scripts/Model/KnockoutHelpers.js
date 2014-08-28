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

ko.bindingHandlers.date = {
    update: function (element, valueAccessor, allBindingsAccessor, viewModel) {
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


/*
ko.bindingHandlers.allowBindings = {
    init: function (elem, valueAccessor) {
        // Let bindings proceed as normal *only if* my value is false
        var shouldAllowBindings = ko.unwrap(valueAccessor());
        return { controlsDescendantBindings: !shouldAllowBindings };
    }
};


ko.bindingHandlers.subBinding = {
    init: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
        var actualValue = valueAccessor();

        ko.virtualElements.emptyNode(element);

        var subViewModel = {};
        subViewModel.subValue =
            new ko.observable(ko.utils.unwrapObservable(actualValue) * 2);

        var childBindingContext = bindingContext.createChildContext(viewModel);
        ko.utils.extend(childBindingContext, subViewModel);

        var getDomNodesFromHtml = function (html) {
            var div = document.createElement('div');
            div.innerHTML = html;
            var elements = div.childNodes;
            var arr = [];
            for (var i = 0; i < elements.length; i++) {
                arr.push(elements[i]);
            }
            return arr;
        };

        var html = '<p data-bind="text: subValue"></p>' +
            '<pre data-bind="text: JSON.stringify(ko.toJS($data), null, 4)"></pre>';

        ko.virtualElements.setDomNodeChildren(element, getDomNodesFromHtml(html));
        ko.applyBindingsToDescendants(childBindingContext, element);

        return { controlsDescendantBindings: true };
    },
    update: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
    }
};
ko.virtualElements.allowedBindings.subBinding = true;


ko.unapplyBindings = function ($node, remove) {
    // unbind events
    $node.find("*").each(function () {
        $(this).unbind();
    });

    // Remove KO subscriptions and references
    if (remove) {
        ko.removeNode($node[0]);
    } else {
        ko.cleanNode($node[0]);
    }
}; */

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