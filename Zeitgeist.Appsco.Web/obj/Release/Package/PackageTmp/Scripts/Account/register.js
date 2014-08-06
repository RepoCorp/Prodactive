$(function () {

   
    var btnSubmit = $('#submit');
    var btnNext = $('#btnNext');
    var btnBack = $('#btnBack');
    var viewDatosPersonales = $('#datos_personales');
    var viewDatosCuenta = $('#datos_cuenta');

    viewDatosPersonales.hide();
    btnSubmit.hide();
    btnBack.hide();

    btnNext.click(function (e) {
        e.preventDefault();
        btnNext.hide(1000);
        viewDatosCuenta.hide(1000);
        viewDatosPersonales.show(1000);
        btnSubmit.show(1000);
        btnBack.show(1000);
    });

    btnBack.click(function (e) {
        e.preventDefault();
        btnNext.show(1000);
        viewDatosCuenta.show(1000);
        viewDatosPersonales.hide(1000);
        btnSubmit.hide(1000);
        btnBack.hide(1000);
    });


});