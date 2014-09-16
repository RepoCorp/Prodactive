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


var registroUsuarioCHat = function () {
    try {
        zg.model.viewInicio.hub.server.registro(zg.model.menuSuperior.user(), zg.model.menuSuperior.liga.id());
    } catch (e) {
        console.log("Errro", e);
    }

};
var updateChatDate      = function () {
    _.each(zg.model.viewInicio.mensajes(), function (elm) {
        elm.fecha.valueHasMutated();
    });
};
var showDialog          = function (htmlMessage, title) {

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
var getRetosByLiga      = function (idLiga) {
    send('/Home/GetRetosByIdLiga/' + idLiga, 'post', null, function (response) {
        zg.model.menu.retos([]);
        _.each(response, function (it, index) {
            zg.model.menu.retos.push(serializeReto(it));
            if (index === 0) {
                zg.model.menu.reto(zg.model.menu.retos()[0]);
            }
        });
    });
};


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
zg.Tips              = function (tipo,titulo, mensaje, imageurl) {
    this.tipo = ko.observable(tipo);
    this.titulo = ko.observable(titulo);
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
zg.Form              = function () {

    this.from = ko.observable(mostrarFecha(-5));
    this.to = ko.observable(new Date());

};
zg.Logro             = function (logro,image,ganado,cantidad) {
    this.logro    = ko.observable(logro);
    this.image    = ko.observable(image);
    this.ganado   = ko.observable(ganado);
    this.cantidad = ko.observable(cantidad);
    this.urlImage = ko.computed(function() {
        if (this.ganado()) {
            return "/Content/medallas/color/" + this.image();
        } else {
            return "/Content/medallas/bw/" + this.image();
        }
        
    }, this);
};

//------ VISTAS ---------//
zg.InicioView        = function () {
    //propiedades
    this.viewName           = ko.observable("inicio");
    this.detallesRetos      = ko.observableArray();
    this.detallesEjercicios = ko.observableArray();
    this.tips               = ko.observableArray();
    this.selected           = ko.observable(true);
    this.label              = ko.observable();
    this.data               = [];
    this.mensaje            = new zg.Mensaje("", "", "", new Date());
    this.mensajes           = ko.observableArray();
    this.hub                = undefined;
    
    //metodos
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
    this.loadLogEjercicio = function() {
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
    };
    this.loadTips = function() {
        send('/Home/GetTips', "POST", null, function(data) {
            _.each(data, function(item) {
               zg.model.viewInicio.tips.push(new zg.Tips(item.Tipo, '', item.Mensaje, item.LinkImage));
               zg.model.viewInicio.tips.valueHasMutated();
            });

        });
    };
    this.LoadDetallesRetosByIdLiga = function (idLiga) {
        send('/Home/GetDetallesRetosByIdLiga/' + idLiga, "post", null, function (response) {
            zg.model.viewInicio.detallesRetos.removeAll();
            _.each(response, function (it) {
                zg.model.viewInicio.detallesRetos.push(serializeDetalleReto(it));
            });
        });
    };
};
zg.RetoView          = function () {
    this.viewName       = ko.observable("reto");
    this.reto           = ko.observable();
    this.selected       = ko.observable(false);
    this.maestroReto    = ko.observable();
    this.equipoSelected = ko.observable();

    this.consultaMaestroDetalleReto = function (idReto) {
        send('/Reto/MaestroDetalle2/' + idReto, 'post', null, function (data) {
            try {
                zg.model.viewReto.maestroReto(serializeMaestroReto(data));
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
};
zg.EstadisticasView  = function () {
    this.viewName = ko.observable("estadisticas");
    this.selected = ko.observable(false);
    this.data     = [];
    this.form     = new zg.Form();

    this.find           = function () {
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
zg.LogrosView        = function () {
    this.viewName = ko.observable("logros");
    this.selected = ko.observable(false);
    this.logros   = ko.observableArray();
    this.isLoaded = ko.observable(false);

    this.load     = function() {

        if (!zg.model.viewLogros.isLoaded())
        {
            send('/User/GetLogros', 'Post', null, function (data) {
                _.each(data, function(item) {
                    zg.model.viewLogros.logros.push(new zg.Logro(item.Logro, item.Icon, item.Ganado, item.Cantidad));
                    zg.model.viewLogros.logros.valueHasMutated();
                });
                zg.model.viewLogros.isLoaded(true);
            });
        }
    };
};
zg.TipsView          = function (name) {
    this.viewName = ko.observable(name);
    this.tips = ko.observableArray();
    this.selected = ko.observable(false);
    var me = this;

    this.loadSalud        = function() {
        send('/Home/GetTipsSalud', "POST", null, function (data) {
            _.each(data, function (item) {
                zg.model.viewTipsSalud.tips.push(new zg.Tips(item.Tipo, item.Titulo, item.Mensaje, item.LinkImage));
                zg.model.viewTipsSalud.tips.valueHasMutated();
            });

        });
    };
    this.loadDeporte      = function() {
        send('/Home/GetTipsDeporte', "POST", null, function (data) {
            _.each(data, function (item) {
                zg.model.viewTipsDeporte.tips.push(new zg.Tips(item.Tipo,item.Titulo, item.Mensaje, item.LinkImage));
                zg.model.viewTipsDeporte.tips.valueHasMutated();
            });

        });
    };
    this.loadAlimentacion = function() {
        send('/Home/GetTipsAlimentacion', "POST", null, function (data) {
            _.each(data, function (item) {
                zg.model.viewTipsAlimentacion.tips.push(new zg.Tips(item.Tipo, item.Titulo, item.Mensaje, item.LinkImage));
                zg.model.viewTipsAlimentacion.tips.valueHasMutated();
            });

        });
    };
};
zg.CalendarioView    = function () {
    this.viewName = ko.observable("calendario");
    this.selected = ko.observable(false);

    this.loadRetos = function() {

    };

};
zg.Galeria           = function () {
    this.viewName = ko.observable("galeria");
    this.selected = ko.observable(false);
    this.loadGaleria = function() {
        
        // Load the classic theme
        //Galleria.loadTheme('../Content/galeria/classic/galleria.classic.min.js');
        // Initialize Galleria
        //Galleria.run('#galleria');

        galri();
    };

    var galri = function() {
        var $overflow = '';
        var colorbox_params = {
            rel: 'colorbox',
            reposition: true,
            scalePhotos: true,
            scrolling: false,
            previous: '<i class="ace-icon fa fa-arrow-left"></i>',
            next: '<i class="ace-icon fa fa-arrow-right"></i>',
            close: '&times;',
            current: '{current} of {total}',
            maxWidth: '100%',
            maxHeight: '100%',
            onOpen: function () {
                $overflow = document.body.style.overflow;
                document.body.style.overflow = 'hidden';
            },
            onClosed: function () {
                document.body.style.overflow = $overflow;
            },
            onComplete: function () {
                $.colorbox.resize();
            }
        };

        $('.ace-thumbnails [data-rel="colorbox"]').colorbox(colorbox_params);
        $("#cboxLoadingGraphic").html("<i class='ace-icon fa fa-spinner orange'></i>");//let's add a custom loading icon
    };
};
zg.FAQ               = function () {
    this.viewName = ko.observable("faq");
    this.selected = ko.observable(false);

};
zg.AcercaDe          = function () {
    this.viewName = ko.observable("acercaDe");
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
zg.Menu             = function () {
    this.reto   = ko.observable().extend({ notify: 'always' });
    this.retos  = ko.observableArray();

    //metodos Menu
    this.selectReto             = function (elm) {
        //zg.model.urvs();
        zg.model.updateView("reto");
        zg.model.urs(elm);
        zg.model.menu.reto(elm);
        //zg.model.viewReto.reto(elm);
        zg.model.cmdr(elm.id());
        updateTitles("Reto", "Resumen reto", "Reto", "");
    };
    this.selectInicio           = function () {
        //zg.model.uivs();
        zg.model.updateView("inicio");
        updateTitles("Tablero", "Resumen General", "Home", "Dashboard");
         zg.model.chart("Caminar", zg.model.viewInicio.data);

    };
    this.selectEstadisticas     = function () {
        //zg.model.uevs();
        zg.model.updateView("estadisticas");
        updateTitles("Estadisticas", "mi progreso personal", "Estadisticas", "");
        loadDatePicker();
        //loadRangeDatePicker();
        zg.model.viewEstadistica.loadStatistics();
    };
    this.selectLogros           = function () {
        //zg.model.ulvs();
        zg.model.updateView("logros");
        zg.model.viewLogros.load();
        updateTitles("Logros", "Logros obtenidos", "Logros", "");
    };
    this.selectCalendario       = function () {
        zg.model.updateView("calendario");
        //zg.model.ucvs();
        updateTitles("Calendario", "Retos Inscritos", "Calendario", "");
        loadCalendario();

    };
    this.selectTipsSalud        = function () {
        zg.model.updateView("tipsSalud");
        updateTitles("Tips", "Salud", "Tips Salud", "");
        zg.model.viewTipsSalud.loadSalud();
    };
    this.selectTipsDeporte      = function () {
        zg.model.updateView("tipsDeporte");
        updateTitles("Tips", "Deporte", "Tips Deporte", "");
        zg.model.viewTipsSalud.loadDeporte();
    };
    this.selectTipsAlimentacion = function () {
        zg.model.updateView("tipsAlimentacion");
        updateTitles("Tips", "Alimentacion", "Tips Alimentacion", "");
        zg.model.viewTipsSalud.loadAlimentacion();
    };
    this.selectGaleria          = function () {
        zg.model.updateView("galeria");
        zg.model.viewGaleria.loadGaleria();
        updateTitles("Galeria", "", "Galeria", "");

    };
    this.selectFAQ              = function () {
        zg.model.updateView("faq");
        updateTitles("Preguntas Frecuentes", "", "FAQ", "");
    };
    this.selectAcercaDe         = function () {
        zg.model.updateView("acercaDe");
        updateTitles("Acerca de", "", "Acerca de", "");
    };
    //helpers Menu
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
    function loadCalendario() {
        $('#con_calendario').fullCalendar();
    };
};
zg.MenuSuperior     = function () {

    this.user = ko.observable();
    this.avatar     = ko.observable();
    this.liga       = ko.observable();
    this.ligas      = ko.observableArray();
    this.mensajes   = ko.observableArray();
    this.urlAvatar  = ko.computed(function() {
        return "/Content/template/assets/avatars/" + this.avatar();
    }, this);

    this.selectLiga          = function (elm) {
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
        }).success(function (response) {
            $("#dialog-message-text").html(response);
            var title = "Envio Invitación";
            showDialog(response, title);
        });



    };
    this.loadLiga            = function() {
        send('/Home/GetLigas', 'post', null, function(data) {
            _.each(data, function(item, index) {
                zg.model.menuSuperior.ligas.push(serializeLiga(item));
                if (index === 0) {
                    var l = zg.model.menuSuperior.ligas()[0];
                    zg.model.menuSuperior.liga = l;
                    getRetosByLiga(l.id());
                    zg.model.viewInicio.LoadDetallesRetosByIdLiga(l.id());
                }
            });
        });
    };
    this.loadUserData        = function() {
        send('/Home/GetUserData', "POST", null, function(data) {
            zg.model.menuSuperior.user(data.usuario);
            zg.model.menuSuperior.avatar(data.avatar);
            registroUsuarioCHat();
        });
    };
};

zg.PageVM = function () {
    //vistas
    var menuSuperior         = new zg.MenuSuperior(),
        menu                 = new zg.Menu(),
        pageName             = new zg.PageName(),
        breadcrumbs          = new zg.Breadcrumbs(),
        viewInicio           = new zg.InicioView(),
        viewReto             = new zg.RetoView(),
        viewEstadistica      = new zg.EstadisticasView(),
        viewLogros           = new zg.LogrosView(),
        viewTipsSalud        = new zg.TipsView("tipsSalud"),
        viewTipsDeporte      = new zg.TipsView("tipsDeporte"),
        viewTipsAlimentacion = new zg.TipsView("tipsAlimentacion"),
        viewCalendario       = new zg.CalendarioView(),
        viewGaleria          = new zg.Galeria(),
        viewFAQ              = new zg.FAQ(),
        viewAcercaDe         = new zg.AcercaDe();
    
    //carga Inicial
    pageName.title("Tablero");
    pageName.description("Resumen General");
    breadcrumbs.view("Home");
    breadcrumbs.description("Dashboard");
        
    var load = function () {

        menuSuperior.loadLiga        (),
        menuSuperior.loadUserData    (),
        viewInicio  .loadLogEjercicio(),
        viewInicio  .loadTips        ();
    };

    function updateRetoSelect(item) {
        viewReto.reto(item);
    };
    function updateview      (name) {
        var list = [viewInicio, viewReto, viewEstadistica, viewLogros, viewTipsSalud, viewTipsDeporte, viewTipsAlimentacion, viewCalendario, viewGaleria,viewFAQ,viewAcercaDe];
        _.each(list, function(data) {
            if (data.viewName() == name) {
                data.selected(true);
            } else {
                data.selected(false);
            }
        });
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

//componentes
        menu:           menu,
        menuSuperior:   menuSuperior,
        pageName:       pageName,
        breadcrumbs:    breadcrumbs,

//vistas
        viewInicio:           viewInicio,
        viewReto:             viewReto,
        viewEstadistica:      viewEstadistica,
        viewLogros:           viewLogros,
        viewTipsSalud:        viewTipsSalud,
        viewTipsDeporte:      viewTipsDeporte,
        viewTipsAlimentacion: viewTipsAlimentacion,
        viewCalendario:       viewCalendario,
        viewGaleria:          viewGaleria,
        viewFAQ:              viewFAQ,
        viewAcercaDe:         viewAcercaDe,

//funciones
        grbl:       getRetosByLiga,
        gdrbl:      viewInicio.LoadDetallesRetosByIdLiga,
        urs:        updateRetoSelect,
        updateView: updateview,
        cmdr:       viewReto.consultaMaestroDetalleReto,
        chart:      chart,
        chart2:     chart2

    };


};

//se carga el modelo inicial
$(function () {
    
    zg.model = new zg.PageVM();
    ko.applyBindings(zg.model.menu,                 document.getElementById("sidebar"));
    ko.applyBindings(zg.model.menuSuperior,         document.getElementById("menuSuperior"));
    ko.applyBindings(zg.model.pageName,             document.getElementById("page-header"));
    ko.applyBindings(zg.model.breadcrumbs,          document.getElementById("breadcrumbs"));

    ko.applyBindings(zg.model.viewInicio,           document.getElementById("home_content"));
    ko.applyBindings(zg.model.viewReto,             document.getElementById("reto_content"));
    ko.applyBindings(zg.model.viewEstadistica,      document.getElementById("estadisticas_content"));
    ko.applyBindings(zg.model.viewLogros,           document.getElementById("logros_content"));
    ko.applyBindings(zg.model.viewTipsSalud,        document.getElementById("tipsSalud"));
    ko.applyBindings(zg.model.viewTipsDeporte,      document.getElementById("tipsDeporte"));
    ko.applyBindings(zg.model.viewTipsAlimentacion, document.getElementById("tipsAlimentacion"));
    ko.applyBindings(zg.model.viewCalendario,       document.getElementById("calendario_content"));
    ko.applyBindings(zg.model.viewGaleria,          document.getElementById("galeria_content"));
    ko.applyBindings(zg.model.viewAcercaDe,         document.getElementById("acercade_content"));
    ko.applyBindings(zg.model.viewFAQ,              document.getElementById("Faq_content"));
    
    //breadcrumbs
    //page-header

    //declaro la conexion con signal r
    // Declare a proxy to reference the hub.
    var chat = $.connection.Chat;
    zg.model.viewInicio.hub = chat;
    // Create a function that the hub can call to broadcast messages.
    
    //eventos signalR
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

    //$(".datepicker").datepicker({
    //    changeMonth: true,
    //    changeYear: true,
    //    showButtonPanel: true,
    //    yearRange: '2013:2014',
    //    dateFormat: 'yy-mm-dd'
    //});

    
});


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
