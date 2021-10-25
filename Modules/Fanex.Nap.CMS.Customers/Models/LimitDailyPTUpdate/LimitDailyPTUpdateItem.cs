namespace Fanex.Nap.CMS.Customers.Models.LimitDailyPTUpdate
{
    using System;

    public class LimitDailyPTUpdateItem
    {
        public string Username { get; set; }

        public byte RoleId { get; set; }

        public string Site { get; set; }

        public string Currency { get; set; }

        public string Product { get; set; }

        // TODO: Maybe update this prop
        public string Type { get; set; }

        public DateTime CreatedDate { get; set; }

    }
}
