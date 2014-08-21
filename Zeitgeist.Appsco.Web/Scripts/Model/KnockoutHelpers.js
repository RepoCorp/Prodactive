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