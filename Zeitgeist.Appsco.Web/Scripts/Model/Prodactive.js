var zg = zg || {};

zg.Page = function() {
    this.idLiga = ko.observable();
    this.datosLiga = ko.computed(function() {
        send('/Liga/GetLiga/' + this.idLiga(), "POST", null, function loadLiga(data) {

        });
    }, this);
};


zg.PageVM = function() {
    this.Page = new zg.Page();

    return { Page: Page };
};