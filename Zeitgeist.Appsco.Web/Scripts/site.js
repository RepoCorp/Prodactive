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
    if (data === undefined || data ===null) {
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

var sendRequestPartialView = function (url, type, data, callback) {
    if (data === undefined || data === null) {
        $.ajax({
            url: url,
            type: type,
            success: callback
        });
    }
    else {
        $.ajax({
            url: url,
            type: type,
            data: data,
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
        $.ajax({
            url    : url,
            data   : { dataSave: jsonData }, 
            type   : "POST", 
            headers: headers,
            cache  :false })
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

    $("#dialog-message-text").empty();
    for (var i = 0; i < errors.length; i++) {
        $("#dialog-message-text").append("<p>"+errors[i]+"</p>");
    }
    
    showDialog();

    //var errorSummary = getValidationSummaryErrors(form)
    //    .removeClass('validation-summary-valid')
    //    .addClass('validation-summary-errors');

    //var items = $.map(errors, function (error) {
    //    return '<li>' + error + '</li>';
    //}).join('');

    //errorSummary.find('ul').empty().append(items);
};

$.widget("ui.dialog", $.extend({}, $.ui.dialog.prototype, {
    _title: function (title) {
        var $title = this.options.title || '&nbsp;'
        if (("title_html" in this.options) && this.options.title_html == true)
            title.html($title);
        else title.text($title);
    }
}));
var showDialog = function() {
    var dialog = $("#dialog-message").removeClass('hide').dialog({
        modal: true,
        title: "<div class='widget-header widget-header-small'><h4 class='smaller'><i class='ace-icon fa fa-error'></i>Error</h4></div>",
        title_html: true,
        buttons: [
            //{
            //    text: "Cancel",
            //    "class": "btn btn-xs",
            //    click: function () {
            //        $(this).dialog("close");
            //    }
            //},
            {
                text: "OK",
                "class": "btn btn-primary btn-xs",
                click: function () {
                    $(this).dialog("close");
                }
            }
        ]
    });
};


