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
