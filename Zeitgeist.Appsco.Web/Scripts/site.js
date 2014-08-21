/// <reference path="jquery-2.1.1.intellisense.js" />


var site = site || {};
site.baseUrl = site.baseUrl || "";

//$(document).ready(function (e) {
    

//    LoadPartialView("#user_stats", "#user_stats");
//});


function loadPartialView(selector, divToload) {
    $(selector).each(function (index, item) {
        var url = site.baseUrl + $(item).data("url");
        if (url && url.length > 0) {
            $(divToload).load(url);
        }
    });
}


var send = function (url, type, data, callback) {
    if (data === undefined) {
        $.ajax({
            url: url,
            type: type,
            dataType: 'json',
            success: callback
        });
    }
    else {
        $.ajax({
            url: url,
            type: type,
            //data: { dataSave: data },
            data: data,
            dataType: 'json',
            success: callback
        });
    }
};


var sendsubmit = function(selector, url, jsonData) {


    $form = $(selector);
    // We check if jQuery.validator exists on the form
    if (!$form.valid || $form.valid()) {
        //$.post($form.attr('action'), $form.serializeArray())

        //$.post(url, { dataSave: jsonData })

        var headers = {};
        headers['__RequestVerificationToken'] = $(selector+' input[name="__RequestVerificationToken"]').val();
        $.ajax({ url: url, data: { dataSave: jsonData }, type: "POST", headers: headers })
            .done(function(json) {
                json = json || {};

                // In case of success, we redirect to the provided URL or the same page.
                if (json.success) {
                    window.location = json.redirect || location.href;
                } else if (json.errors) {
                    displayErrors($form, json.errors);
                }
            })
            .error(function() {
                displayErrors($form, ['An unknown error happened.']);
            });
    }
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



//$(function() {

//    $("#inicio").on('click', function() {
//        loadPartialView("#inicio", "#load_content");
//    });
//    $("#registro").on('click', function () {
//        loadPartialView("#registro", "#load_content");
//    });
//    $("#about").on('click', function () {
//        loadPartialView("#about", "#load_content");
//    });

//loadPartialView("#inicio", "#load_content");

//});
