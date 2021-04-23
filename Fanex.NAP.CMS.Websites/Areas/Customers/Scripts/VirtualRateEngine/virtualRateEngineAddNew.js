"use strict";
function VirtualRateEngineAddNew(form, reloadData) {
    this.init = function () {
        CMS.modal("Add New Rule", form, { height: "auto", resizable: false });
        initForm();
        bindEvents();
    };
}