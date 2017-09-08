function AutoRefresh(container, intervals, autoRefreshFunction) {
    "use strict";
    var resource = {
        NotRefresh: "Not Refresh",
        Loading: "...",
        Seconds: " sec"
    };
    var domElements = {
        countdownContainer: null
    };
    var self = this;
    var timerId = 0;
    var selectedInterval = 0;

    function renderIntervalDropdown() {
        var interval,
            selectedText,
            dropdownHtml = ['<ul class="dropdown-container">'];
        for (var i = 0, max = intervals.length; i < max; i++) {
            interval = intervals[i];
            dropdownHtml.push('<li data-value="');
            dropdownHtml.push(interval);
            dropdownHtml.push('"');
            if (selectedInterval === interval) {
                dropdownHtml.push(' class="checked"');
                selectedText = interval === 0 ? resource.NotRefresh : interval + " sec";
            }

            dropdownHtml.push(">");

            if (interval === 0) {
                dropdownHtml.push(resource.NotRefresh);
            }
            else {
                dropdownHtml.push(interval);
                dropdownHtml.push(resource.Seconds);
            }

            dropdownHtml.push("</li>");
        }

        dropdownHtml.push("</ul>");

        return [
            '<div id="refresh-container">',
            '<div class="timer">',
            '<div title="Refresh" class="countdown" id="countdown">',
            selectedText,
            "</div>",
            '<div class="dropdown" id="dropdown"></div>',
            "</div>",
            dropdownHtml.join(""),
            "</div>"
        ].join("");
    }

    function dispose() {
        if (timerId !== 0) {
            clearTimeout(timerId);
            timerId = 0;
        }
    }

    self.initialize = function (selected) {
        selectedInterval = selected;
        container.append(renderIntervalDropdown());

        var refreshContainer = $("#refresh-container");
        domElements.countdownContainer = refreshContainer.find("#countdown");
        domElements.countdownContainer
            .off("click")
            .on("click", function () {
                if (self.isRunning() && autoRefreshFunction) {
                    self.pause();
                    autoRefreshFunction();
                }
            });

        var dropdownContainer = refreshContainer.find(".dropdown-container");
        dropdownContainer.find("li")
            .off("click")
            .on("click", function () {
                dropdownContainer.find("li.checked").removeClass("checked");

                var $this = $(this);
                $this.addClass("checked");
                selectedInterval = parseInt($this.data("value"));

                if (isNaN(selectedInterval) || selectedInterval === 0) {
                    self.stop();
                }
                else {
                    self.start();
                }
            });
    };

    self.stop = function () {
        dispose();
        selectedInterval = 0;

        domElements.countdownContainer
            .css("text-align", "left")
            .text(resource.NotRefresh);
    };

    self.start = function () {
        dispose();

        if (selectedInterval <= 0) {
            return;
        }

        domElements.countdownContainer
            .css("text-align", "center")
            .text(selectedInterval);

        timerId = setTimeout(function tick() {
            var currentValue = parseInt(domElements.countdownContainer.text());

            if (isNaN(currentValue)) {
                return;
            }

            if (currentValue === 1) {
                self.pause();

                if (autoRefreshFunction) {
                    autoRefreshFunction();
                }
            }
            else {
                domElements.countdownContainer.text(--currentValue);
                timerId = setTimeout(tick, 1000);
            }
        }, 1000);
    };

    self.pause = function () {
        dispose();
        domElements.countdownContainer
            .css("text-align", "center")
            .text(resource.Loading);
    };

    self.isRunning = function () {
        return timerId !== 0 && selectedInterval > 0;
    };

    self.isPausing = function () {
        return selectedInterval > 0 &&
            domElements.countdownContainer.text() === resource.Loading;
    };
}