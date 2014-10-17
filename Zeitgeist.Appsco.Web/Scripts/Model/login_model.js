/// <reference path="../jquery-1.9.1.min.js" />
/// <reference path="../knockout-3.1.0.js" />
/// <reference path="../underscore.js" />
/// <reference path="KnockoutHelpers.js" />


var zg = zg || {};
zg.Login = function () {
    this.userName = ko.observable();//.extend({ required: "El campo usuario no puede estar vacio." });
    this.password = ko.observable();//.extend({ required: "El campo contraseña no puede estar vacio." });
    this.rememberMe = ko.observable(false);
    this.returnUrl = ko.observable();

};

zg.Cuenta = function() {
    this.usuario = ko.observable();
    this.correo  = ko.observable();
}

zg.SelectItem = function(val,txt) {
    this.value = ko.observable(val);
    this.text  = ko.observable(txt);
}

zg.Persona = function() {
    this.id              = ko.observable();
    this.tipo            = ko.observable();
    this.nombre          = ko.observable();
    this.apellido        = ko.observable();
    this.identificacion  = ko.observable();
    this.fechaNacimiento = ko.observable();
    this.sexo            = ko.observable();
    //this.cuentas         = ko.observableArray();
    this.peso            = ko.observable();
    this.estatura        = ko.observable();
};

zg.Register = function() {
    this.userName         = ko.observable();
    this.password         = ko.observable();
    this.confirmPassword  = ko.observable();
    this.passwordQuestion = ko.observable();
    this.passwordAnswers   = ko.observable();
    this.email            = ko.observable();
    this.datosPersonales  = new zg.Persona();
    //REV: ver como optimizar esta parte.....
    ////this.id               = ko.observable();
    //this.tipo             = ko.observable();
    //this.nombre           = ko.observable();
    //this.apellido         = ko.observable();
    //this.identificacion   = ko.observable();
    //this.fechaNacimiento  = ko.observable();
    //this.sexo             = ko.observable();
    //this.cuentas          = ko.observableArray();
    //this.peso             = ko.observable();
    //this.estatura         = ko.observable();
    this.userAgreement = ko.observable();

    this.enablePersonalData = ko.computed(function() {
        return this.datosPersonales.tipo() === 'Natural';
    },this);
};



zg.loginVM = function () {
    var login = new zg.Login(),
        submit = function (elm) {
            if (!elm.valid || elm.valid()) {
                elm.submit();
            }
            // login.returnUrl($("#returnUrl").val());
           // sendForm(elm, "#loginform", '/Account/Login', ko.toJSON(login));
        };

    return {
        login: login,
        submit: submit
    };
};

zg.registerVM = function () {
    
    var register = new zg.Register(),
        questions = ko.observableArray(),
        submit = function (elm) {
            //var a = ko.toJS2(register);

            //register.datosPersonales.cuentas.push({ key: register.userName, value: register.email });
            sendsubmit("#register-form", '/Account/Register', ko.toJSON(register));
        },
        reset    = function() {
            register = new zg.Register();
        };
        $.post('/Account/Questions').done(function (json) 
        {
            _.forEach(json, function(item) {
                questions.push( new zg.SelectItem(item,item));
            });
        questions.valueHasMutated();

        });

    return {
        register : register ,
        submit   : submit   ,
        reset    : reset    ,
        questions: questions
    };
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