"use strict";
(function ($) {
    const domElements = {
        tabs: null,
        forms: null
    };

    const cacheDOM = function () {
        domElements.tabs = $('.filter-bar .tabs-area .tab');
        domElements.forms = $('.filter-bar .form-area .form-tab');
    };

    // Initialize fields and handle events.
    const initFields = function () {
        // Add a click event listener to all tabs.
        domElements.tabs.click(function () {
            updateActiveTab(this);
        });

        // Activate the first tab by default.
        updateActiveTab(domElements.tabs[0]);
    };

    const updateActiveTab = function (tab) {
        // Remove the active class from all tabs.
        domElements.tabs.removeClass('active');

        // Add the active class to the selected tab.
        $(tab).addClass('active');

        // Get the form that corresponds to the selected tab.
        const form = document.querySelector('#form-' + tab.id);

        // Hide all form tabs.
        domElements.forms.addClass('display-none');

        // Show the form that corresponds to the selected tab.
        form.classList.remove('display-none');
    };

    const init = function () {
        cacheDOM();
        initFields();
    };

    $(function () {
        init();
    });
})(jQuery);