/// <reference path="../knockout-3.1.0.js" />
var zg = zg || {};
zg.registro = function () {
    this.id = ko.observable();
    this.user = ko.observable();
    this.fecha = ko.observable();
    this.cantidadPasos = ko.observable();
};


zg.registroVM = function () {
    var self = this;
    var registro = new zg.registro(),
    registros = new ko.observableArray([]),
    save = function() {
        send('/Home/Registro', 'post', { dataSave: ko.toJSON(registros) }, function(data) {
            //window.location.replace(site.baseUrl + '/Home/Index');
            window.location.href='/Home/Index';
        });
    },
    add = function(elm) {
        registros.push(new zg.registro());
        registros.valueHasMutated();
    },
    remove = function (elm) {
        registros.remove(elm);
        registros.valueHasMutated();
    };

    return {
        add:        add,
        remove:     remove,
        registro:   registro,
        registros:  registros,
        save:       save
    };
};

$(function () {
    var model = new zg.registroVM();
    ko.applyBindings(model);

    $(document).on('focus', ".fecha", function () {
        $(this).datepicker({ format: "yyyy/mm/dd" });
    });

});