var zg = zg || {};

zg.Index = function () {
    this.liga = ko.observable();
};

zg.DetalleReto = function (id, name, total, equipo, reto, porUsuario, porEquipo, por) {
    this.idReto = ko.observable(id);
    this.name = ko.observable(name);
    this.totalUsuario = ko.observable(total);
    this.totalEquipo = ko.observable(equipo);
    this.totalReto = ko.observable(reto);
    this.porcentajeTotalUsuario = ko.computed(function () {
        return ((this.totalUsuario() / this.totalReto()) * 100) + "%";
    }, this);
    this.porcentajeTotalEquipo = ko.computed(function () {
        return (((this.totalEquipo() / this.totalReto()) - (this.totalUsuario() / this.totalReto())) * 100) + "%";
    }, this);
    this.porcentajeTotalReto = ko.computed(function () { return (100 - ((this.totalEquipo() / this.totalReto()) - (this.totalUsuario() / this.totalReto()))) * "%"; }, this);
};

zg.DetalleEjercicio = function () {
    this.fecha = ko.observable();
    this.pasos = ko.observable();
};

zg.Tips = function (tipo, mensaje, imageurl) {
    this.tipo = ko.observable(tipo);
    this.mensaje = ko.observable(mensaje);
    this.linkImage = ko.observable(imageurl);
};

zg.Liga = function () {
    this.id = ko.observable();
    this.nombre = ko.observable();
    this.entrenador = ko.observable();
    this.propia = ko.observable();
};
zg.Reto = function () {
    this.id = ko.observable();
    this.name = ko.observable();
    this.liga = ko.observable();
    this.division = ko.observable();
    this.owner = ko.observable();
    this.entrenador = ko.observable();
    this.fechaInicio = ko.observable();
    this.fechaFin = ko.observable();
    this.tipo = ko.observable();
    this.meta = ko.observable();
    this.isActivo = ko.observable();
    this.premio = ko.observable();
    this.deportes = ko.observableArray();
    this.equipos = ko.observableArray();
};

zg.PageVM = function () {

    
    var Index = new zg.Index(),
        ligas = ko.observableArray(),
        detallesReto = ko.observableArray(),
        tips = ko.observableArray(),
        clickMenuReto = function(elm) {
            /*alert("http://" + window.location.hostname+":"+window.location.port + "/Reto/Index" + elm.idReto());*/
        window.location = "http://" + window.location.hostname+":"+window.location.port + "/Reto/Index/" + elm.idReto();
            /*send("http://" + window.location.hostname + ":" + window.location.port + "/Reto/Index/" + elm.idReto(), 'GET', null, function(data) {
                $('#content_load').empty();
                $('#content_load').html(data);
            });*/
        },
        loadLigas = function() {
            send('/Home/GetLigas', "POST", null, function(data) {
                var i = 0;
                _.each(data, function(item) {
                    var a = new zg.Liga();
                    a.id(item.id);
                    a.nombre(item.nombre);
                    a.entrenador(item.entrenador);
                    a.propia(item.propia);
                    ligas.push(a);

                    if (i === 0) {
                        i++;
                        Index.liga = a;
                        send('/Home/GetDetallesRetosByIdLiga/' + a.id(), "POST", null, function(response) {
                            var j = 0;
                            detallesReto.removeAll();
                            _.each(response, function(Data) {
                                detallesReto.push(new zg.DetalleReto(Data.IdReto, Data.Name, Data.TotalUsuario, Data.TotalEquipo, Data.TotalReto, Data.PorcentajeTotalUsuario, Data.PorcentajeTotalEquipo, Data.PorcentajeTotalReto));
                                if (j === 0) {
                                    j++;
                                    send('/Home/GetLogEjerciciosByIdReto/' + Data.IdReto, "POST", null, function(res) {
                                        var l = [];
                                        _.each(res, function(it, index) {
                                            l.push([new Date(it.dia), it.pasos]);
                                            if (l.length - 1 === index)
                                                chart(l);
                                        });


                                    });


                                }
                                detallesReto.valueHasMutated();
                            });


                        });
                    }
                    ligas.valueHasMutated();
                });


            });
        },
        loadDetalleLiga = function(item) {
            send('/Home/GetDetallesRetosByIdLiga/' + item.id(), "POST", null, function(response) {
                var j = 0;
                detallesReto.removeAll();
                _.each(response, function(Data) {
                    detallesReto.push(new zg.DetalleReto(Data.IdReto, Data.Name, Data.TotalUsuario, Data.TotalEquipo, Data.TotalReto, Data.PorcentajeTotalUsuario, Data.PorcentajeTotalEquipo, Data.PorcentajeTotalReto));
                    if (j === 0) {
                        j++;
                        send('/Home/GetLogEjerciciosByIdReto/' + Data.IdReto, "POST", null, function(res) {
                            var l = [];

                            _.each(res, function(it, index) {
                                l.push([new Date(it.dia), it.pasos]);
                                if (l.length - 1 === index)
                                    chart(l);
                            });


                        });
                    }
                    detallesReto.valueHasMutated();
                });

            });
        },
        loadTips = function() {
            send('/Home/GetTips', "POST", null, function(data) {
                _.each(data, function(item) {
                    tips.push(new zg.Tips(item.Tipo, item.Mensaje, item.LinkImage));
                    tips.valueHasMutated();
                });

            });
        };

    
        loadLigas();
        loadTips();
        


    return {
        Index:           Index,
        ligas:           ligas,
        tips:            tips,
        loadLigas:       loadLigas,
        loadDetalleLiga: loadDetalleLiga,
        detallesReto:    detallesReto,
        clickMenuReto:   clickMenuReto

    };
};


//function chart(l) {
//    $("#sales-charts").css({ 'width': '90%', 'min-height': '350px' });
//    var my_chart = $.plot("#sales-charts", [
//   { label: "Pasos X dia", data: l }
//   //,{ label: "Hosting", data: d2 }
//    ], {
//        hoverable: true,
//        shadowSize: 1,
//        series: {
//            lines: { show: true },
//            points: { show: true }
//        },
//        xaxis: {
//            mode: "time",
//            timeformat: "%Y/%m/%d",
//            ticks: l.length - 1
//        },
//        //yaxis: {
//        //    ticks: 10,
//        //    min: 0,
//        //    max: 2,
//        //    tickDecimals: 3
//        //},
//        grid: {
//            backgroundColor: { colors: ["#fff", "#fff"] },
//            borderWidth: 1,
//            borderColor: '#555'
//        }
//    });
//}