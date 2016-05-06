(function () {
	'use strict';
	var self = this;
	var rootUrl = $('#CMSRoot').val();

	var $refno = $('#RefNo'),
		$pool = $('#Pool'),
		$username = $('#Username'),
		$league = $('#League'),
		$match = $('#Match'),
		$choice = $('#Choice'),
		$result = $('#Result'),
		$status = $('#Status'),
		$amount = $('#WinAmount');

	function showMessage(data) {
		if (data.Code !== 0) {
			CMS.showErrorMessage(data.Message);
		}
		else {
			CMS.showSuccessMessage(
				data.Message,
				function () {
					window.location.href = rootUrl + 'ColossusBets/BonusTicket/Index';
				}
			);
		}
	}

	function getBonusTicketByRefno() {
		var refno = parseInt($refno.val().trim());

		if (refno > 0) {
			CMS.getJson(
				rootUrl + 'ColossusBets/BonusTicket/BonusTicketDetailByRefno?refno=' + refno,
				function (result) {
					var data = $.parseJSON(result);

					if (data === null) {
						$pool.html('');
						$username.html('');
						custId = 0;
						CMS.showTipsyError($refno, colossusBetsLabels.WrongRefNo, 'w');
					}
					else {
						$pool.html(data.Pool);
						$username.html(data.Username);
						custId = data.CustId;
						CMS.clearTipsyError($refno);
					}
				}
			);
		}
	}

	function validateEmptyInput(element) {
		var value = element.val().trim();

		if (value.length === 0) {
			CMS.showTipsyError(element, colossusBetsLabels.RequiredField, 'w');
			return false;
		}

		CMS.clearTipsyError(element);
		return true;
	}

	function validateAmount() {
		// 112 = WON
		if ($status.val() === '112') {
			return validateEmptyInput($amount);
		}

		return true;
	}

	function getFormData() {
		if (!validateEmptyInput($refno)) {
			return undefined;
		}

		if (custId === 0) {
			return undefined;
		}

		if (!validateEmptyInput($choice)) {
			return undefined;
		}

		if (!validateAmount()) {
			return undefined;
		}

		var statusId = $status.val();

		return {
			refno: $refno.val().trim(),
			league: $league.val().trim(),
			match: $match.val().trim(),
			choice: $choice.val().trim(),
			winAmount: $amount.val().trim(),
			custid: custId,
			result: $result.val(),
			statusId: isNaN(statusId) ? $status.data('statusid') : statusId
		};
	}

	function saveBonusTicket(form) {
		var formData = getFormData();

		if (formData) {
			var ajax = function () {
				CMS.callAjax(self, form.attr('action'), formData, function (results) {
					var data = $.parseJSON(results);
					showMessage(data);
				});
			};

			var showConfirm = !isNaN($status.val()) && formData.statusId !== '0';

			if (showConfirm) {
				CMS.showConfirmMessage(
					colossusBetsLabels.SaveBonusTicketConfirmMessage,
					ajax
				);
			}
			else {
				ajax();
			}
		}
	}

	function initialize() {
		$('#save-form').submit(function (event) {
			event.preventDefault();

			saveBonusTicket($(this));
		});

		$('#cancel-button').click(function () {
			window.location.href = rootUrl + 'ColossusBets/BonusTicket/Index';
		});

		if (custId === 0) {
			$refno
				.focus()
				.blur(function () {
					getBonusTicketByRefno();
				});
		}
	}

	initialize();
})();