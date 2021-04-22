"use strict";
function VirtualRateEngineAddNew(form, reloadData) {
    this.init = function () {
        CMS.modal("", form, { height: "auto" });

        initForm(usernames, level);
        bindEvents();
    };
}