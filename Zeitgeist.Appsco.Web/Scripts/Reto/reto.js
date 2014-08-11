/// <reference path="../jquery-1.9.1.intellisense.js" />
/// <reference path="../underscore.js" />
/// <reference path="../knockout-3.1.0.js" />
var zg = zg || {};
zg.Reto = function () {
    this.id             = ko.observable();
    this.division       = ko.observable();
    this.owner          = ko.observable();
    this.fechaInicio    = ko.observable();
    this.fechaFin       = ko.observable();
    this.deportes       = ko.observableArray();
    this.tipo           = ko.observable();
    this.isActivo       = ko.observable();
    this.premio         = ko.observable();
};

zg.Deporte = function () {
    this.name    = ko.observable();
    this.valor   = ko.observable();
}

zg.Division = function(id,name) {
    this.id      = ko.observable(id);
    this.name    = ko.observable(name);
}

zg.retoVM = function () {
    var self            = this;
    var reto            = new zg.Reto(),
        deporte         = new zg.Deporte(),
        divisiones      = ko.observableArray(),
        deportes        = ko.observableArray(),
        save            = function (elm) {
                        send('/Reto/Create', 'post', { dataSave: ko.toJSON(reto) },
                            function (data) {
                                if (data.Status)
                                    window.location.href = '/Reto/Index';
                                else
                                    alert("Verifique los datos");
                            });},
        loadDivisiones  = function () {
                            send('/Reto/GetDivisiones', 'post', null, function (data) {
                                _.each(data, function(item) {
                                    divisiones.push(new zg.Division(item.Id, item.Name));
                                    divisiones.valueHasMutated();
                                });
                            });},
        loadDeportes    = function () {
                            send('/Reto/GetDeportes', 'post', null, function (data) {
                                _.each(data, function (item) {
                                    deportes.push(item);
                                    deportes.valueHasMutated();
                                });
                            });},
        add             = function (elm) {
                            reto.deportes.push(elm);
                            reto.deportes.valueHasMutated();},
        remove          = function (elm) {
                            reto.deportes.remove(elm);
                            reto.deportes.valueHasMutated();};

    var IsValidated     = ko.computed(function () {
        if (reto.division() === "undefined")
            return false;
        return true;
    });
    return {
        add             : add,
        remove          : remove,
        save            : save,
        reto            : reto,
        divisiones      : divisiones,
        loadDivisiones  : loadDivisiones,
        loadDeportes    : loadDeportes,
        IsValidated     : IsValidated,
        deporte         : deporte,
        deportes        : deportes
    };
};


$(function () {

    var model = new zg.retoVM();

    model.loadDivisiones();
    model.loadDeportes();
    ko.applyBindings(model);

    $(document).on('focus', ".fecha", function () {
        $(this).datepicker({ format: "yyyy/mm/dd" });
    });

});