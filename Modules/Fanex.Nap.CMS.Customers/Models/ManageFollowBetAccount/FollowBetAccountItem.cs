namespace Fanex.Nap.CMS.Customers.Models.ManageFollowBetAccount
{
    using Fanex.Nap.CMS.Infrastructure.Extensions;
    using System;

    public class FollowBetAccountItem
    {
        public int CustID { get; set; }

        public string Username { get; set; }

        public string DummyUsername { get; set; }

        public string Agent { get; set; }

        public string AddedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool IsActive { get; set; }

        public string Site { get; set; }

        public string Currency { get; set; }

        public string AddedDate => DateTimeExtensions.ToShortFormatedDateWithSlash(CreatedDate);
    }
}
