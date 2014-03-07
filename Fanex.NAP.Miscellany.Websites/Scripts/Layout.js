/* File Created: March 3, 2014 */
/* global Miscellany */

'use strict';

$(document).ready(function () {

    var menuEvent = Miscellany.agentIsMobile ? 'click' : 'hover';

    $('nav').on(menuEvent, '.menu > li', function (e) {
        var $this = $(this),
            parent = $this.parents('.menu');

        if ($this.hasClass('selected')) {
            $this.removeClass('selected');
        } else {
            parent.children('li').removeClass('selected');
            $this.addClass('selected');
        }

        e.stopPropagation();
    });

    $('header').on(menuEvent, '.user-info .greeting', function (e) {
        $(this).toggleClass('selected');
        e.stopPropagation();
    });

    if (Miscellany.agentIsMobile) {
        $('body, header, #headline, #body').on('click', function () {
            $('nav .menu > li').removeClass('selected');
            $('header .user-info .greeting').removeClass('selected');
        });
    }

    $('select.combobox-autocomplete').combobox();
    $('input[type=text]').clearfield();
});