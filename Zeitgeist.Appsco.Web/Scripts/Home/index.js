var zg = zg || {};

zg.Index = function () {
    this.liga = ko.observable();
    //this.retos = ko.observable();
    //this.datosLiga = ko.computed(function () {
    //    try {
    //        send('/Home/GetRetosByIdLiga/' + liga.id(), "POST", null, function (data) {

    //            this.retos = ko.mapping.fromJS(data);
    //            var i = 0;

    //        });
    //    } catch (e) {
    //    }
    //}, this);
};

zg.DetalleReto = function(id,name,total,equipo,reto,porUsuario,porEquipo,por) {
    this.idReto = ko.observable(id);
    this.name = ko.observable(name);
    this.totalUsuario = ko.observable(total);
    this.totalEquipo = ko.observable(equipo);
    this.totalReto = ko.observable(reto);
    this.porcentajeTotalUsuario = ko.computed(function() {
        return ((this.totalUsuario() / this.totalReto()) * 100) + "%";
    }, this);
    this.porcentajeTotalEquipo = ko.computed(function() {
        return (((this.totalEquipo() / this.totalReto()) - (this.totalUsuario() / this.totalReto())) * 100) + "%";
    }, this);
    this.porcentajeTotalReto = ko.computed(function () { return (100 - ((this.totalEquipo() / this.totalReto()) - (this.totalUsuario() / this.totalReto()))) * "%"; }, this);
};

zg.DetalleEjercicio = function () {
    this.fecha = ko.observable();
    this.pasos = ko.observable();
}

zg.Liga = function () {
    this.id = ko.observable();
    this.nombre = ko.observable();
    this.entrenador = ko.observable();
}
zg.Reto = function() {
    this.id          = ko.observable();
    this.name        = ko.observable();
    this.liga        = ko.observable();
    this.division    = ko.observable();
    this.owner       = ko.observable();
    this.entrenador  = ko.observable();
    this.fechaInicio = ko.observable();
    this.fechaFin    = ko.observable();
    this.tipo        = ko.observable();
    this.meta        = ko.observable();
    this.isActivo    = ko.observable();
    this.premio      = ko.observable();
    this.deportes    = ko.observableArray();
    this.equipos     = ko.observableArray();
}

zg.PageVM = function () {
    var Index    = new zg.Index(),
    ligas        = ko.observableArray(),
    detallesReto = ko.observableArray(),
    detallesEjercicio = ko.observableArray(),
    loadLigas = function() {
        send('/Home/GetLigas', "POST", null, function (data) {
            var i = 0;
            _.each(data, function (item) {
                var a = new zg.Liga();
                a.id(item.id);
                a.nombre(item.nombre);
                a.entrenador(item.entrenador);
                ligas.push(a);

                if (i === 0) {
                    i++;
                    Index.liga = a;
                    send('/Home/GetRetosByIdLiga/' + a.id(), "POST", null, function (response) {
                        var j = 0;
                        _.each(response, function (Data) {
                            detallesReto.push(new zg.DetalleReto(Data.IdReto, Data.Name, Data.TotalUsuario, Data.TotalEquipo, Data.TotalReto, Data.PorcentajeTotalUsuario, Data.PorcentajeTotalEquipo, Data.PorcentajeTotalReto));
                            if (j === 0) {
                                j++;
                                send('/Home/GetLogEjerciciosByIdReto/' + Data.IdReto, "POST", null, function (res) {
                                    var l = [];
                                    _.each(res, function (it) { l.push([it.dia,it.pasos]); });
                                    $("#sales-charts").css({ 'width': '90%', 'min-height': '150px' });
                                    var my_chart = $.plot("#sales-charts", [
                                   { label: "Pasos X dia", data: l }
                                   //,{ label: "Hosting", data: d2 }
                                    ], {
                                        hoverable: true,
                                        shadowSize: 1,
                                        series: {
                                            lines: { show: true },
                                            points: { show: true }
                                        },
                                        //xaxis: {
                                        //    tickLength: 1
                                        //},
                                        //yaxis: {
                                        //    ticks: 10,
                                        //    min: 0,
                                        //    max: 2,
                                        //    tickDecimals: 3
                                        //},
                                        grid: {
                                            backgroundColor: { colors: ["#fff", "#fff"] },
                                            borderWidth: 1,
                                            borderColor: '#555'
                                        }
                                    });
                                });
                                //var d1 = [];//dataset 1 (random data)
                                //for (var i = 0; i < Math.PI * 2; i += 0.5) {
                                //    d1.push([i, Math.sin(i)]);
                                //}
                                //var d2 = [];//dataset 2 (random data)
                                //for (var i = 0; i < Math.PI * 2; i += 0.5) {
                                //    d2.push([i, Math.cos(i)]);
                                //}

                                //we put the chart inside #sales-charts element
                                //$('#sales-charts').css({ 'width': '100%', 'height': '220px' });
                                //var my_chart = $.plot("#sales-charts", [
                                //   { label: "Domains", data: d1 }
                                //   //,{ label: "Hosting", data: d2 }
                                //], {
                                //    hoverable: true,
                                //    shadowSize: 0,
                                //    series: {
                                //        lines: { show: true },
                                //        points: { show: true }
                                //    },
                                //    xaxis: {
                                //        tickLength: 0
                                //    },
                                //    yaxis: {
                                //        ticks: 10,
                                //        min: -2,
                                //        max: 2,
                                //        tickDecimals: 3
                                //    },
                                //    grid: {
                                //        backgroundColor: { colors: ["#fff", "#fff"] },
                                //        borderWidth: 1,
                                //        borderColor: '#555'
                                //    }
                                //});

                            }
                        });
                        
                    });
                }
                
            });
        });
    };
    loadLigas();

    return {
        Index: Index,
        ligas:ligas,
        loadLigas: loadLigas,
        detallesReto: detallesReto
    };
};

function processDetalleRetos(data) {
};

