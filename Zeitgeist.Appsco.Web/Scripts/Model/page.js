var zg = zg || {};

//serializadores
function serializeDetalleReto(item) {
    return new zg.DetalleReto(item.IdReto, item.Name, item.TotalUsuario, item.TotalEquipo, item.TotalReto, item.PosicionEquipo);
}
function serializeReto(item) {
    var r = new zg.Reto();
    r.id(item.Id);
    r.name(item.Name);
    r.liga(item.Liga);
    r.division(item.Division);
    r.owner(item.Owner);
    r.entrenador(item.Entrenador);
    r.fechaInicio(item.FechaInicio);
    r.fechaFin(item.FechaFin);
    r.tipo(item.Tipo);
    r.meta(item.Meta);
    r.isActivo(item.isActivo);
    r.premio(item.Premio);
    r.deportes(item.Deportes);
    r.equipos(item.Equipos);
    return r;
}
function serializeLiga(item) {
    return new zg.Liga(item.id, item.nombre, item.entrenador);
}
function logEjercicio(item) {
    return new zg.DetalleEjercicio(item.fecha, item.pasos, item.deporte);
}

function serializeDetalleMiembros(item) {
    var elm = new zg.DetalleMiembros();
    elm.usuario(item.Usuario);
    elm.total(item.Total);
    elm.posicion(item.Posicion);
    return elm;
}
function serializeDetalleEquipo(item) {
    var elm = new zg.DetalleEquipo();
    elm.equipo(item.Equipo);
    elm.puntosTotales(item.PuntosTotales);
    elm.mejor(item.Mejor);
    elm.totalMejor(item.TotalMejor);
    elm.miEquipo(item.MiEquipo);
    elm.posicion(item.Posicion);
    for (var i = 0; i < item.Detalles.length; i++) {
        elm.detalles.push(serializeDetalleMiembros(item.Detalles[i]));
    }
    return elm;
}
function serializeMaestroReto(item) {
    var elm = new zg.MaestroReto();
    elm.name(item.name);
    elm.descripcion(item.descripcion);
    
    for (var i = 0; i < item.equipos.length; i++) {
        elm.equipos.push(serializeDetalleEquipo(item.equipos[i]));
    }
    return elm;
}

//modelos
zg.DetalleReto = function (id, name, total, equipo, reto, posicion) {
    this.idReto = ko.observable(id);
    this.name = ko.observable(name);
    this.totalUsuario = ko.observable(total);
    this.totalEquipo = ko.observable(equipo);
    this.totalReto = ko.observable(reto);
    this.posicion = ko.observable(posicion);
    this.posicionText = ko.computed(function () {
        return this.posicion() + "°";
    }, this);
    this.posicionIcon = ko.computed(function () {
        switch (this.posicion()) {
            case 1:
                return "label label-warning arrowed";
            case 2:
                return "label label-info arrowed";
            case 3:
                return "label label-success arrowed";
            default:
                return "label label-inverse arrowed";
        }
        return "";
    }, this);
    this.porcentajeTotalUsuario = ko.computed(function () {
        return ((this.totalUsuario() / this.totalReto()) * 100) + "%";
    }, this);
    this.porcentajeTotalEquipo = ko.computed(function () {
        return (((this.totalEquipo() / this.totalReto()) - (this.totalUsuario() / this.totalReto())) * 100) + "%";
    }, this);
    this.porcentajeTotalReto = ko.computed(function () { return (100 - ((this.totalEquipo() / this.totalReto()) - (this.totalUsuario() / this.totalReto()))) * "%"; }, this);
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
zg.Liga = function (id, nombre, entrenador) {
    this.id = ko.observable(id);
    this.nombre = ko.observable(nombre);
    this.entrenador = ko.observable(entrenador);
};
zg.Tips = function (tipo, mensaje, imageurl) {
    this.tipo = ko.observable(tipo);
    this.mensaje = ko.observable(mensaje);
    this.linkImage = ko.observable(imageurl);
};
zg.DetalleEjercicio = function (fecha, pasos, deporte) {
    this.fecha = ko.observable(fecha);
    this.pasos = ko.observable(pasos);
    this.deporte = ko.observable(deporte);
};

//modelo detalle reto
zg.MaestroReto = function () {
    this.name = ko.observable();
    this.descripcion = ko.observable();
    this.equipos = ko.observableArray([]);
    this.equipoSelected = ko.observable();
};
zg.DetalleEquipo = function () {
    this.equipo = ko.observable();
    this.puntosTotales = ko.observable();
    this.mejor = ko.observable();
    this.totalMejor = ko.observable();
    this.miEquipo = ko.observable();
    this.posicion = ko.observable();
    this.detalles = ko.observableArray([]);
    this.posicionText = ko.computed(function () {
        return this.posicion() + "°";
    }, this);
    this.posicionIcon = ko.computed(function () {
        switch (this.posicion()) {
            case 1:
                return "label label-warning arrowed";
            case 2:
                return "label label-info arrowed";
            case 3:
                return "label label-success arrowed";
            default:
                return "label label-inverse arrowed";
        }
        return "";
    }, this);
};
zg.DetalleMiembros = function () {
    this.usuario = ko.observable();
    this.total = ko.observable();
    this.posicion = ko.observable();
    this.posicionText = ko.computed(function () {
        return this.posicion() + "°";
    }, this);
    this.posicionIcon = ko.computed(function () {
        switch (this.posicion()) {
            case 1:
                return "label label-warning arrowed";
            case 2:
                return "label label-info arrowed";
            case 3:
                return "label label-success arrowed";
            default:
                return "label label-inverse arrowed";
        }
        return "";
    }, this);
};


//------ VISTAS ---------//

zg.InicioView = function (idLiga) {

    this.detallesRetos = ko.observableArray();
    this.detallesEjercicios = ko.observableArray();
    this.tips = ko.observableArray();
    this.selected = ko.observable(true);
    this.label = ko.observable();
    this.data = [];
    //funciones descarga de la informacion

};
zg.RetoView = function () {
    this.reto = ko.observable();
    this.selected    = ko.observable(false);
    this.maestroReto = ko.observable();
    this.equipoSelected = ko.observable();
};
zg.EstadisticasView = function() {
   
};
zg.Menu = function (model) {
    this.reto = ko.observable().extend({ notify: 'always' });
    this.retos = ko.observableArray();
    this.selectReto = function (elm) {
        zg.model.urvs();
        zg.model.urs(elm);
        zg.model.menu.reto(elm);
        //zg.model.viewReto.reto(elm);
        zg.model.cmdr(elm.id());
    };
    this.selectInicio = function () {
        zg.model.uivs();
    };
};
zg.MenuSuperior = function (model) {
    this.liga = ko.observable();
    this.ligas = ko.observableArray();
    this.selectLiga = function (elm) {
        if (zg.model.menuSuperior.liga.id() !== elm.id()) {
            zg.model.grbl(elm.id());
            zg.model.gdrbl(elm.id());
        }
    };
};

zg.PageVM = function () {
    var menuSuperior = new zg.MenuSuperior(this),
        menu = new zg.Menu(this),
        viewInicio = new zg.InicioView(),
        viewReto = new zg.RetoView();

    var load = function () {
        (function () {
            send('/Home/GetLigas', 'post', null, function (data) {
                _.each(data, function (item, index) {
                    menuSuperior.ligas.push(serializeLiga(item));
                    if (index === 0) {
                        var l = menuSuperior.ligas()[0];
                        menuSuperior.liga = l;
                        getRetosByLiga(l.id());
                        getDetallesRetosByIdLiga(l.id());
                    }
                });
            });
        })(),
        (function () {
        send('/Home/GetLogEjerciciosByUser', "post", null, function (data) {

            var caminar = [];
            var d = _.where(data, { deporte: "Caminar" });
            var s = d.length;
            var i = 0;
            _.each(d, function (item, index) {
                caminar.push([new Date(item.fecha), item.pasos]);
                i++;
                if (i === s) {
                    zg.model.chart("Caminar", caminar);
                    zg.model.viewInicio.label = "Caminar";
                    zg.model.viewInicio.data = caminar;
                }
            });

        });
    })(),
        (function () {
        send('/Home/GetTips', "POST", null, function (data) {
            _.each(data, function (item) {
                viewInicio.tips.push(new zg.Tips(item.Tipo, item.Mensaje, item.LinkImage));
                viewInicio.tips.valueHasMutated();
            });

        });
    })();

    };
    var getRetosByLiga = function (idLiga) {
        send('/Home/GetRetosByIdLiga/' + idLiga, 'post', null, function (response) {
            _.each(response, function (it, index) {
                menu.retos.push(serializeReto(it));
                if (index === 0) {
                    menu.reto(menu.retos()[0]);
                }
            });
        });
    };
    var getDetallesRetosByIdLiga = function (idLiga) {
        send('/Home/GetDetallesRetosByIdLiga/' + idLiga, "post", null, function (response) {
            viewInicio.detallesRetos.removeAll();
            _.each(response, function (it) {
                viewInicio.detallesRetos.push(serializeDetalleReto(it));
            });
        });
    };
    var updateRetoSelect = function (item) {
        viewReto.reto(item);
    };
    var consultaMaestroDetalleReto = function (idReto) {
        send('/Reto/MaestroDetalleReto/' + idReto, 'post', null, function (data) {
            try {
                viewReto.maestroReto(serializeMaestroReto(data));
          //      var a = ko.mapping.fromJS(data);

            } catch (e) {
                var p = 0;
            }
            
        });
    };

    var updateRetoViewSelected = function () {
        viewInicio.selected(false);
        viewReto.selected(true);
    };

    var updateInicioViewSelected = function () {
        viewReto.selected(false);
        viewInicio.selected(true);
        zg.model.chart(viewInicio.label, viewInicio.data);

    };

    function chart(name, dato) {
        $("#sales-charts").css({ 'width': '90%', 'min-height': '350px' });
        var my_chart = $.plot("#sales-charts",
            [{ label: name, data: dato }],
         {
             hoverable: true,
             shadowSize: 1,
             series: {
                 lines: { show: true },
                 points: { show: true }
             },
             xaxis: {
                 mode: "time",
                 timeformat: "%Y/%m/%d",
                 ticks: 4
             },

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
    }

 
    load();


    return {
        menu: menu,
        menuSuperior: menuSuperior,
        viewInicio: viewInicio,
        viewReto: viewReto,
        grbl: getRetosByLiga,
        gdrbl: getDetallesRetosByIdLiga,
        urs: updateRetoSelect,
        urvs: updateRetoViewSelected,
        uivs: updateInicioViewSelected,
        cmdr: consultaMaestroDetalleReto,
        chart: chart
        //,chartT:chartT
    };


};

//se carga el modelo inicial
$(function () {
    zg.model = new zg.PageVM();

    ko.applyBindings(zg.model.menu, document.getElementById("sidebar"));

    ko.applyBindings(zg.model.menuSuperior, document.getElementById("menuSuperior"));

    ko.applyBindings(zg.model.viewInicio, document.getElementById("home_content"));

    ko.applyBindings(zg.model.viewReto, document.getElementById("reto_content"));

});
