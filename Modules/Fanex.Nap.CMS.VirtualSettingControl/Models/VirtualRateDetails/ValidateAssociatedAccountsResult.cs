namespace Fanex.Nap.CMS.VirtualSettingControl.Models.VirtualRateDetails
{
    using System.Collections.Generic;
    using System.Linq;

    public class ValidateAssociatedAccountsResult
    {
        public ValidateAssociatedAccountsResult()
        {
            InvalidAccounts = new List<string>();
            ValidAccounts = new List<string>();
            ValidAccountIds = new List<string>();
        }

        public List<string> InvalidAccounts { get; set; }

        public List<string> ValidAccounts { get; set; }

        public List<string> ValidAccountIds { get; set; }

        public string InvalidAccountsText
            => InvalidAccounts.Any() ? string.Join(",", InvalidAccounts) : string.Empty;

        public string ValidAccountsText
            => ValidAccounts.Any() ? string.Join(",", ValidAccounts) : string.Empty;

        public string ValidAccountIdsText
            => ValidAccountIds.Any() ? string.Join(",", ValidAccountIds) : string.Empty;
    }
}
