namespace Fanex.Nap.CMS.VirtualSettingControl.Models.Basketball
{
    public class CustomerInfo
    {
        public CustomerInfo(
            int custId,
            string username,
            string currency,
            decimal actualRate,
            int uplineCustId,
            string uplineUsername,
            byte roleId)
        {
            CustId = custId;
            Username = username;
            Currency = currency;
            ActualRate = actualRate;
            UplineCustId = uplineCustId;
            UplineUsername = uplineUsername;
            RoleId = roleId;
        }

        public int CustId { get; }

        public string Username { get; }

        public string Currency { get; }

        public decimal ActualRate { get; }

        public int UplineCustId { get; }

        public string UplineUsername { get; }

        public byte RoleId { get; }
    }
}
