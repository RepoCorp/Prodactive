/// <reference path="../jquery-1.9.1.min.js" />
/// <reference path="../knockout-3.1.0.js" />
/// <reference path="../underscore.js" />
/// <reference path="KnockoutHelpers.js" />


var zg = zg || {};
zg.Login = function () {
    this.userName   = ko.observable().extend({ required: "El campo usuario no puede estar vacio." });
    this.password   = ko.observable().extend({ required: "El campo contraseña no puede estar vacio." });
    this.rememberMe = ko.observable();
    //this.__RequestVerificationToken = ko.observable();
};

zg.Cuenta = function() {
    this.usuario = ko.observable();
    this.correo  = ko.observable();
}

zg.Persona = function() {
    this.id              = ko.observable();
    this.tipo            = ko.observable();
    this.nombre          = ko.observable();
    this.apellido        = ko.observable();
    this.identificacion  = ko.observable();
    this.fechaNacimiento = ko.observable();
    this.sexo            = ko.observable();
    this.cuentas         = ko.observableArray();
    this.peso            = ko.observable();
    this.estatura        = ko.observable();
};

zg.Registro = function() {
    this.user             = ko.observable();
    this.password         = ko.observable();
    this.confirmPassword  = ko.observable();
    this.passwordQuestion = ko.observable();
    this.questionAnswer   = ko.observable();
    this.email            = ko.observable();
    //this.datosPersonales  = new zg.Persona();
    //REV: ver como optimizar esta parte.....
    this.id               = ko.observable();
    this.tipo             = ko.observable();
    this.nombre           = ko.observable();
    this.apellido         = ko.observable();
    this.identificacion   = ko.observable();
    this.fechaNacimiento  = ko.observable();
    this.sexo             = ko.observable();
    this.cuentas          = ko.observableArray();
    this.peso             = ko.observable();
    this.estatura         = ko.observable();
};



zg.loginVM = function () {
    var login = new zg.Login(),
        submit = function (elm) {
            
            var $form = $("#loginform");

            // We check if jQuery.validator exists on the form
            if (!$form.valid || $form.valid()) {
                //$.post($form.attr('action'), $form.serializeArray())
                $.post('/Account/Login', { dataSave: ko.toJSON(login) })
                    .done(function (json) {
                        json = json || {};

                        // In case of success, we redirect to the provided URL or the same page.
                        if (json.success) {
                            window.location = json.redirect || location.href;
                        } else if (json.errors) {
                            displayErrors($form, json.errors);
                        }
                    })
                    .error(function () {
                        displayErrors($form, ['An unknown error happened.']);
                    });
            }

            // Prevent the normal behavior since we opened the dialog
            //e.preventDefault();

        };

    return {
        login: login,
        submit: submit
    };
};



var getValidationSummaryErrors = function ($form) {
    var errorSummary = $form.find('.validation-summary-errors, .validation-summary-valid');
    return errorSummary;
};

var displayErrors = function (form, errors) {
    var errorSummary = getValidationSummaryErrors(form)
        .removeClass('validation-summary-valid')
        .addClass('validation-summary-errors');

    var items = $.map(errors, function (error) {
        return '<li>' + error + '</li>';
    }).join('');

    errorSummary.find('ul').empty().append(items);
};

/*submit
 * 
 * 
 * //login.__RequestVerificationToken($('[name=__RequestVerificationToken]').val());
            send('/Account/Login', 'post', { dataSave: ko.toJSON(login) }, function(data) {
                //window.location.replace(site.baseUrl + '/Home/Index');
                window.location.href = '/Home/Index';
            });


 */