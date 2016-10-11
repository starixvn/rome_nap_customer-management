/* File Created: June 20, 2014 */
/* global Namespace, Fanex */
/* jshint strict: false */
// 'use strict';

$(document).ready(function () {
    var manageSmartPunters = new Fanex.CMS.ManageSmartPunters.PageManager();

    manageSmartPunters.InitPage();
});

Namespace.Register("Fanex.CMS.ManageSmartPunters.PageManager");
Fanex.CMS.ManageSmartPunters.PageManager = function () {
    var $this = this;
    var domElements = {
        spnNote: null,
        divFilterFormTag: null,
        divLogFormTag: null,
        divFilterForm: null,
        divLogForm: null,
        divSmartPunterListContainer: null
    };

    var manageSmartPunters = null;
    var controlSmartPuntersLog = null;
    var actionMonitoring = null;

    var cacheDOM = function () {
        domElements.spnNote = $('#spnNote');

        domElements.divFilterFormTag = $('#divFilterFormTag');
        domElements.divLogFormTag = $('#divLogFormTag');
        domElements.divActionMonitoringFormTag = $('#divActionMonitoringFormTag');

        domElements.divFilterForm = $('#divFilterForm');
        domElements.divLogForm = $('#divLogForm');
        domElements.divActionMonitoringForm = $('#divActionMonitoringForm');

        domElements.divSmartPunterListContainer = $('#divSmartPunterListContainer');
        domElements.divSmartPunterLogListContainer = $('#divSmartPunterLogListContainer');
        domElements.divActionMonitoringLogListContainer = $('#divActionMonitoringLogListContainer');
    };

    // Initialize fields and handle events.
    var initFields = function () {
        domElements.divFilterFormTag.click(function () {
            domElements.divFilterFormTag.addClass('active');
            domElements.divFilterForm.removeClass('display-none');

            domElements.divLogFormTag.removeClass('active');
            domElements.divActionMonitoringFormTag.removeClass('active');

            domElements.divLogForm.addClass('display-none');
            domElements.divActionMonitoringForm.addClass('display-none');

            domElements.divSmartPunterLogListContainer.addClass('display-none');
            domElements.divActionMonitoringLogListContainer.addClass('display-none');

            if (!manageSmartPunters) {
                manageSmartPunters = new Fanex.CMS.ManageSmartPunters.ManageSmartPunters();
                manageSmartPunters.InitPage();
            } else {
                manageSmartPunters.ResetForm();
            }
        });
        domElements.divFilterFormTag.click();

        domElements.divLogFormTag.click(function () {
            domElements.divLogFormTag.addClass('active');
            domElements.divLogForm.removeClass('display-none');

            domElements.divFilterFormTag.removeClass('active');
            domElements.divActionMonitoringFormTag.removeClass('active');

            domElements.divFilterForm.addClass('display-none');
            domElements.divActionMonitoringForm.addClass('display-none');

            domElements.divSmartPunterListContainer.addClass('display-none');
            domElements.divActionMonitoringLogListContainer.addClass('display-none');

            if (!controlSmartPuntersLog) {
                controlSmartPuntersLog = new Fanex.CMS.ManageSmartPunters.ControlSmartPuntersLog();
                controlSmartPuntersLog.InitPage();
            } else {
                controlSmartPuntersLog.ResetForm();
            }
        });

        domElements.divActionMonitoringFormTag.click(function () {
            domElements.divActionMonitoringFormTag.addClass('active');
            domElements.divActionMonitoringForm.removeClass('display-none');

            domElements.divFilterFormTag.removeClass('active');
            domElements.divLogFormTag.removeClass('active');

            domElements.divFilterForm.addClass('display-none');
            domElements.divLogForm.addClass('display-none');

            domElements.divSmartPunterListContainer.addClass('display-none');
            domElements.divSmartPunterLogListContainer.addClass('display-none');

            if (!actionMonitoring) {
                actionMonitoring = new Fanex.CMS.ManageSmartPunters.ActionMonitoring();
                actionMonitoring.InitPage();
            } else {
                actionMonitoring.ResetForm();
            }
        });

        domElements.spnNote.tipsy({
            html: true,
            opacity: 0.9,
            gravity: 'ne'
        });
    };

    $this.InitPage = function () {
        cacheDOM();
        initFields();
    };
};