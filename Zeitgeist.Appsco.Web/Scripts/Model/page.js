﻿var zg = zg || {};

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
    return new zg.Liga(item.id, item.nombre, item.entrenador, item.propia, item.invitacionesDisponibles);
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

    var arreglo = [];
    for (var i = 0; i < item.Detalles.length; i++)
        arreglo.push(serializeDetalleMiembros(item.Detalles[i]));
    
    var elm = new zg.DetalleEquipo(item.Equipo, item.PuntosTotales, item.Mejor,item.TotalMejor, item.MiEquipo,item.Posicion, item.PorcentajePuntosTotales,arreglo);
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
zg.DetalleReto       = function (id, name, total, equipo, reto, posicion) {
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
zg.Reto              = function () {
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
zg.Liga              = function (id, nombre, entrenador, propia, invitacionesDisponibles) {
    this.id = ko.observable(id);
    this.nombre = ko.observable(nombre);
    this.entrenador = ko.observable(entrenador);
    this.propia = ko.observable(propia);
    this.invitacionesDisponibles = ko.observable(invitacionesDisponibles);
};
zg.Tips              = function (tipo, mensaje, imageurl) {
    this.tipo = ko.observable(tipo);
    this.mensaje = ko.observable(mensaje);
    this.linkImage = ko.observable(imageurl);
};
zg.DetalleEjercicio  = function (fecha, pasos, deporte) {
    this.fecha = ko.observable(fecha);
    this.pasos = ko.observable(pasos);
    this.deporte = ko.observable(deporte);
};
zg.Mensaje           = function (usuario, mensaje, avatar, fecha) {
    this.usuario = ko.observable(usuario);
    this.mensaje = ko.observable(mensaje);
    this.avatar = ko.observable(avatar);
    this.fecha = ko.observable(fecha);
    this.fechaCountdown = ko.computed(function () {
        var result = countdown(this.fecha()).toString();
        if (result === "")
            result = countdown(new Date()).toString();
        return result;
    }, this);
    this.url = ko.computed(function () {
        return "/Content/template/assets/avatars/" + this.avatar();
    }, this);

};
//modelo detalle reto
zg.MaestroReto       = function () {
    this.name = ko.observable();
    this.descripcion = ko.observable();
    this.equipos = ko.observableArray([]);
    this.equipoSelected = ko.observable();
};
zg.DetalleEquipo     = function (equipo,puntosTotales,mejor,totalMejor,miEquipo,posicion,porcentajePuntosTotales,detalles) {
    this.equipo = ko.observable(equipo);
    this.puntosTotales = ko.observable(puntosTotales);
    this.mejor = ko.observable(mejor);
    this.totalMejor = ko.observable(totalMejor);
    this.miEquipo = ko.observable(miEquipo);
    this.posicion = ko.observable(posicion);
    this.porcentajePuntosTotales = ko.observable(porcentajePuntosTotales);
    this.detalles = ko.observableArray(detalles);
    this.puntosUsuario = ko.observable(0);
    this.cssPuntosUsuario = ko.computed(function () {
        if (this.miEquipo() == true) {
            var r = _.find(this.detalles(), function (e) { return e.usuario() == zg.model.menuSuperior.user(); });
            if (r !== undefined) {
                this.puntosUsuario = ko.observable((r.total() * 100 / zg.model.menu.reto().meta()));
                return this.puntosUsuario() + "%";
            }
            
        }
        else
            return "0%";
    }, this);
    this.cssPuntosTotales = ko.computed(function() {
        return (this.porcentajePuntosTotales() - this.puntosUsuario()) + "%";
    }, this);
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
zg.DetalleMiembros   = function () {
    this.usuario = ko.observable();
    this.total = ko.observable();
    this.posicion = ko.observable();
    this.posicionText = ko.computed(function () {
        return this.posicion() + "°";
    }, this);
    this.posicionIcon = ko.computed(function () {
        switch (this.posicion()) {
            case 1:
                return "/Content/Landing/images/rating2.png";
            //case 2:
            //    return "label label-info arrowed";
            //case 3:
            //    return "label label-success arrowed";
            default:
                return "";
        }
        return "";
    }, this);
};
zg.Form = function () {
    
    this.from = ko.observable(mostrarFecha(-5));
    this.to = ko.observable(new Date());

}

//------ VISTAS ---------//

zg.InicioView       = function (idLiga) {

    this.detallesRetos      = ko.observableArray();
    this.detallesEjercicios = ko.observableArray();
    this.tips               = ko.observableArray();
    this.selected           = ko.observable(true);
    this.label              = ko.observable();
    this.data               = [];

    //chat
    this.mensaje = new zg.Mensaje("", "", "", new Date());
    this.mensajes = ko.observableArray();
    this.hub      = undefined;
    
    this.sendMessage = function (elm) {
        if(elm.mensaje()!=="")
        {
            zg.model.viewInicio.hub.server.send(zg.model.menuSuperior.user(), elm.mensaje(), zg.model.menuSuperior.avatar(),zg.model.menuSuperior.liga.id());
            elm.mensaje("");
        }
    };

    this.enterSend = function(elm, event) {
        if (event.keyCode == 13) {
            if (elm.mensaje() !== "") {
                zg.model.viewInicio.hub.server.send(zg.model.menuSuperior.user(), elm.mensaje(), zg.model.menuSuperior.avatar(), zg.model.menuSuperior.liga.id());
                elm.mensaje("");
            }
            
        }
        return true;
    };
    //funciones descarga de la informacion

};
zg.RetoView         = function () {
    this.reto = ko.observable();
    this.selected    = ko.observable(false);
    this.maestroReto = ko.observable();
    this.equipoSelected = ko.observable();
};
zg.EstadisticasView = function () {
    this.selected = ko.observable(false);
    this.data = [];
    this.form = new zg.Form();
    this.find = function (elm) {
        if (elm.to !== undefined && elm.from !== undefined)
            zg.model.viewEstadistica.loadStatistics();
    };
    this.find2 = function () {
        zg.model.viewEstadistica.loadStatistics();
    };
    this.loadStatistics = function() {

        if (zg.model.viewEstadistica.form.from() !== undefined && zg.model.viewEstadistica.form.to() != undefined)
            send("/User/GetStatistics", "Post", { Search: ko.toJSON(zg.model.viewEstadistica.form) }, function (data) {
                zg.model.viewEstadistica.data = [];
                var caminar = [];
                var d = _.where(data, { deporte: "Caminar" });
                var s = d.length;
                var i = 0;
                _.each(d, function (item, index) {
                    caminar.push([new Date(item.fecha), item.pasos]);
                    i++;
                    if (i === s) {
                        //zg.model.chart("Caminar", caminar);
                        zg.model.viewEstadistica.data.push({ label: "Caminar", data: caminar });
                        zg.model.chart2("#estadistica-chart", zg.model.viewEstadistica.data);

                    }
                });
            });


        /*
         * send('/Home/GetLogEjerciciosByUser', "post", null, function(data) {
                var caminar = [];
                var d = _.where(data, { deporte: "Caminar" });
                var s = d.length;
                var i = 0;
                _.each(d, function(item, index) {
                    caminar.push([new Date(item.fecha), item.pasos]);
                    i++;
                    if (i === s) {
                    zg.model.chart("Caminar", caminar);
                    zg.model.viewInicio.label = "Caminar";
                    zg.model.viewInicio.data = caminar;
                    }
                });
            });
         */
    };


};


zg.LogrosView       = function () {
    this.selected = ko.observable(false);
};

zg.PageName         = function () {
    this.title       = ko.observable();
    this.description = ko.observable();
};
zg.Breadcrumbs      = function () {
    this.view        = ko.observable();
    this.description = ko.observable();
};
zg.Menu             = function (model) {
    this.reto = ko.observable().extend({ notify: 'always' });
    this.retos = ko.observableArray();
    this.selectReto = function (elm) {
        zg.model.urvs();
        zg.model.urs(elm);
        zg.model.menu.reto(elm);
        //zg.model.viewReto.reto(elm);
        zg.model.cmdr(elm.id());
        updateTitles("Reto", "Resumen reto", "Reto", "");
    };
    this.selectInicio = function () {
        zg.model.uivs();
        updateTitles("Tablero", "Resumen General", "Home", "Dashboard");
    };
    this.selectEstadisticas = function() {
        zg.model.uevs();
        updateTitles("Estadisticas", "mi progreso personal", "Estadisticas", "");
        loadDatePicker();
        //loadRangeDatePicker();
        zg.model.viewEstadistica.loadStatistics();
    };
    this.selectLogros = function() {
        zg.model.ulvs();
        updateTitles("Logros", "Logros obtenidos", "Logros", "");
    };


    function updateTitles(title,description,view,descriptionView) {
        zg.model.pageName.title(title);
        zg.model.pageName.description(description);
        zg.model.breadcrumbs.view(view);
        zg.model.breadcrumbs.description(descriptionView);
    };
    function loadDatePicker() {
        $.datepicker.setDefaults($.datepicker.regional['es']);
        $(".datepicker").datepicker({
            changeMonth: true,
            changeYear: true,
            showButtonPanel: true,
            yearRange: '2013:2014',
            dateFormat: 'yy-mm-dd'
        });
    };

    function loadRangeDatePicker() {
        $('#range_date').dateRangeSlider(
            {
                bounds: {
                    min: new Date(2014, 7, 15),
                    max: new Date()
                },
                defaultValues: {
                    min: new Date(2014, 8, 1),
                    max: new Date()
                },
                step: {
                    days: 1
                }
            }
        );

        var i = 0;
    };
};
zg.MenuSuperior     = function (model) {
    this.user = ko.observable();
    this.avatar = ko.observable();

    this.liga = ko.observable();
    this.ligas = ko.observableArray();
    this.selectLiga = function (elm) {
        if (zg.model.menuSuperior.liga.id() !== elm.id()) {
            zg.model.menuSuperior.liga = elm;
            zg.model.grbl(elm.id());
            zg.model.gdrbl(elm.id());
            registroUsuarioCHat();
            zg.model.menu.retos.valueHasMutated();
        }
    };
    this.showEnvioInvitacion = function (elm) {

        $.ajax({
            url: "/Liga/EnvioInvitacion/" + elm.id(),
            type: "Get"
        }).success(function(response) {
            $("#dialog-message-text").html(response);
            var title = "Envio Invitación";
            showDialog(response, title);
        });

        

    };
    this.urlAvatar = ko.computed(function() {
        return "/Content/template/assets/avatars/" + this.avatar();
    }, this);
    this.mensajes = ko.observableArray();
};

zg.PageVM = function () {
    var menuSuperior    = new zg.MenuSuperior(this),
        menu            = new zg.Menu(this),
        pageName        = new zg.PageName(),
        breadcrumbs = new zg.Breadcrumbs(),
        viewInicio      = new zg.InicioView(),
        viewReto        = new zg.RetoView(),
        viewEstadistica = new zg.EstadisticasView(),
        viewLogros      = new zg.LogrosView();
        
    //carga Inicial
    pageName.title("Tablero");
    pageName.description("Resumen General");

    breadcrumbs.view("Home");
    breadcrumbs.description("Dashboard");
        
    var load = function () {
        (function() {
                send('/Home/GetLigas', 'post', null, function(data) {
                    _.each(data, function(item, index) {
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
            send('/Home/GetUserData', "POST", null, function (data) {
                zg.model.menuSuperior.user(data.usuario);
                zg.model.menuSuperior.avatar(data.avatar);
                registroUsuarioCHat();
            });
        })(),
        (function() {
            send('/Home/GetLogEjerciciosByUser', "post", null, function(data) {
                var caminar = [];
                var d = _.where(data, { deporte: "Caminar" });
                var s = d.length;
                var i = 0;
                _.each(d, function(item, index) {
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
            (function() {
                send('/Home/GetTips', "POST", null, function(data) {
                    _.each(data, function(item) {
                        viewInicio.tips.push(new zg.Tips(item.Tipo, item.Mensaje, item.LinkImage));
                        viewInicio.tips.valueHasMutated();
                    });

                });
            })();
        

    };
    var getRetosByLiga = function (idLiga) {
        send('/Home/GetRetosByIdLiga/' + idLiga, 'post', null, function (response) {
            menu.retos([]);
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
        send('/Reto/MaestroDetalle2/' + idReto, 'post', null, function (data) {
            try {
                viewReto.maestroReto(serializeMaestroReto(data));
                $(".dds").ddslick({
                    keepJSONItemsOnTop: true,
                    imagePosition: "right",
                    selectText: "Miembros del equipo"
                });
            } catch (e) {
                var p = 0;
            }
            
        });
    };

    var updateRetoViewSelected = function () {
        viewInicio.selected(false);
        viewEstadistica.selected(false);
        viewLogros.selected(false);

        viewReto.selected(true);
    };
    var updateInicioViewSelected = function () {
        viewReto.selected(false);
        viewEstadistica.selected(false);
        viewLogros.selected(false);

        viewInicio.selected(true);
        zg.model.chart(viewInicio.label, viewInicio.data);
    };
    var updateEstadisticasViewSelected = function() {
        viewInicio.selected(false);
        viewReto.selected(false);
        viewLogros.selected(false);
        viewEstadistica.selected(true);
    };
    var updateLogrosViewSelected = function () {
        viewInicio.selected(false);
        viewReto.selected(false);
        viewEstadistica.selected(false);

        viewLogros.selected(true);
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
    };

    function chart2(selector,data) {
        //$(selector).css({ 'width': '90%', 'min-height': '350px' });
        $(selector).css({
            'width': '90%', 'min-height': '350px'
        });
        var my_chart = $.plot(selector,
            data,
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
    };

 
    load();


    return {
        menu: menu,
        menuSuperior: menuSuperior,
        pageName: pageName,
        breadcrumbs:breadcrumbs,

        viewInicio: viewInicio,
        viewReto: viewReto,
        viewEstadistica: viewEstadistica,
        viewLogros:viewLogros,

        grbl: getRetosByLiga,
        gdrbl: getDetallesRetosByIdLiga,
        urs: updateRetoSelect,

        urvs: updateRetoViewSelected,
        uivs: updateInicioViewSelected,
        uevs: updateEstadisticasViewSelected,
        ulvs: updateLogrosViewSelected,

        cmdr:   consultaMaestroDetalleReto,
        chart:  chart,
        chart2: chart2

    };


};

//se carga el modelo inicial
$(function () {
    

    zg.model = new zg.PageVM();
    ko.applyBindings(zg.model.menu,             document.getElementById("sidebar"));
    ko.applyBindings(zg.model.menuSuperior,     document.getElementById("menuSuperior"));
    ko.applyBindings(zg.model.viewInicio,       document.getElementById("home_content"));
    ko.applyBindings(zg.model.viewReto,         document.getElementById("reto_content"));
    ko.applyBindings(zg.model.viewEstadistica,  document.getElementById("estadisticas_content"));
    ko.applyBindings(zg.model.viewLogros,       document.getElementById("logros_content"));

    ko.applyBindings(zg.model.pageName,         document.getElementById("page-header"));
    ko.applyBindings(zg.model.breadcrumbs,      document.getElementById("breadcrumbs"));
    


    //breadcrumbs
    //page-header

    //declaro la conexion con signal r
    // Declare a proxy to reference the hub.
    var chat = $.connection.Chat;
    zg.model.viewInicio.hub = chat;
    // Create a function that the hub can call to broadcast messages.
    
    chat.client.broadcastMessage = function (name, message, avatar,fecha) {
        zg.model.viewInicio.mensajes.unshift(new zg.Mensaje(name, message, avatar, new Date(fecha)));
        updateChatDate();

    };

    chat.disconnected = function() {
        console.log("se ha desconectado del servidor signalr");
    };



    // Get the user name and store it to prepend to messages.
    // Set initial focus to message input box.
    // Start the connection.

    $.connection.hub.start().done(function () {
        console.log("conexion exitosa");
        // Call the Send method on the hub.
        //chat.server.registro(zg.model.menuSuperior.user(), zg.model.menuSuperior.liga.id());
        //});
    });

    setInterval(updateChatDate, 15000);

    $(".datepicker").datepicker({
        changeMonth: true,
        changeYear: true,
        showButtonPanel: true,
        yearRange: '2013:2014',
        dateFormat: 'yy-mm-dd'
    });

    
});

var registroUsuarioCHat = function () {
    try {
        zg.model.viewInicio.hub.server.registro(zg.model.menuSuperior.user(), zg.model.menuSuperior.liga.id());
    } catch (e) {
        console.log("Errro", e);
    }
    
};
var updateChatDate      = function()
{
    _.each(zg.model.viewInicio.mensajes(), function(elm) {
        elm.fecha.valueHasMutated();
    });
};
var showDialog          = function (htmlMessage,title) {

    $("#dialog-message-text").html(htmlMessage);

    var dialog = $("#dialog-message").removeClass('hide').dialog({
        modal: true,
        //title: "<div class='widget-header widget-header-small'><h4 class='smaller'><i class='ace-icon fa fa-info'></i>"+title+"</h4></div>",
        title: title,
        title_html: false,
        buttons: [
            {
                text: "Cancel",
                "class": "btn btn-xs",
                click: function () {
                    $(this).dialog("close");
                }
            },
            {
                text: "OK",
                "class": "btn btn-primary btn-xs",
                click: function () {
                    //$("#dialog-message-text form")[0].submit();
                    $(this).dialog("close");
                }
            }
        ]
    });
};



$.datepicker.regional['es'] = {
    closeText: 'Cerrar',
    prevText: '<Ant',
    nextText: 'Sig>',
    currentText: 'Hoy',
    monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
    monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
    dayNames: ['Domingo', 'Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado'],
    dayNamesShort: ['Dom', 'Lun', 'Mar', 'Mié', 'Juv', 'Vie', 'Sáb'],
    dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sá'],
    weekHeader: 'Sm',
    dateFormat: 'yyyy/mm/dd',
    firstDay: 1,
    isRTL: false,
    showMonthAfterYear: false,
    yearSuffix: ''
};
