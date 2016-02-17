(function () {
	'use strict';

	var gridOptions = null,
        freegrid = $('#freegrid');

	function initGridOptions() {
		var columns = [
            { HeaderText: '#', Width: 40, Name: 'colNumber', HeaderAlign: 'center', CellAlign: 'center' },
            { HeaderText: colossusBetsLabels.Member, Width: 200, Name: 'colMember', HeaderAlign: 'center', CellAlign: 'left' },
			{ HeaderText: colossusBetsLabels.RefNo, Width: 140, Name: 'colRefNo', HeaderAlign: 'center', CellAlign: 'center' },
            { HeaderText: colossusBetsLabels.Pool, Width: 200, Name: 'colPool', HeaderAlign: 'center', CellAlign: 'left' },
            { HeaderText: colossusBetsLabels.League, Width: 200, Name: 'colLeague', HeaderAlign: 'center', CellAlign: 'left' },
            { HeaderText: colossusBetsLabels.Match, Width: 200, Name: 'colMatch', HeaderAlign: 'center', CellAlign: 'center' },
            { HeaderText: colossusBetsLabels.Choice, Width: 70, Name: 'colChoice', HeaderAlign: 'center', CellAlign: 'center' },
            { HeaderText: colossusBetsLabels.Status, Width: 70, Name: 'colStatus', HeaderAlign: 'center', CellAlign: 'center' },
            { HeaderText: colossusBetsLabels.Detail, Width: 50, Name: 'colDetail', HeaderAlign: 'center', CellAlign: 'center' }
		];

		return {
			showTitle: false,
			fullWidth: true,
			fullHeight: true,
			showDistinctRow: false,
			showHighlightRow: true,
			headerColumnHeight: 25,
			gridExpandHeight: 150,
			columns: columns,
			showPager: false
		};
	}

	function buildBodyRows(results) {
		var rootUrl = $('#CMSRoot').val();

		var bodyRows = [];
		var cell, ticket;
		var resultLength = results ? results.length : 0;

		for (var i = 0; i < resultLength; i++) {
			ticket = results[i];
			cell = [
                { Name: 'colNumber', Value: (i + 1) },
                { Name: 'colMember', Value: ticket.Username },
				{ Name: 'colRefNo', Value: ticket.RefNo },
                { Name: 'colPool', Value: ticket.Pool },
                { Name: 'colLeague', Value: ticket.League },
                { Name: 'colMatch', Value: ticket.Match },
                { Name: 'colChoice', Value: ticket.Choice },
                { Name: 'colStatus', Value: ticket.Status },
                { Name: 'colDetail', Value: renderDetailRow(rootUrl, ticket.RefNo) }
			];

			bodyRows.push({ Columns: cell });
		}

		return bodyRows;
	}

	function renderDetailRow(rootUrl, refno) {
		return [
			'<a href="',
			rootUrl,
			'ColossusBets/BonusTicket/BonusTicketDetail?refno=',
			refno,
			'" class="ico-edit"></a>'
		].join('');
	}

	function initialize() {
		gridOptions = initGridOptions();
		gridOptions.bodyRows = buildBodyRows(data);

		freegrid.removeClass('display-none').fanexGrid(gridOptions);
	}

	initialize();
})();