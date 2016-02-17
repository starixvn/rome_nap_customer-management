namespace Fanex.Nap.CMS.ColossusBets.Models
{
    using System.Configuration;

    public class DoSettlement
    {
        public string SettlementUrl { get; set; }

        public string Vendor { get; set; }

        public string VendorKey { get; set; }

        public string Type { get; set; }

        public int ProductId { get; set; }

        public int GameId { get; set; }

        public string EventId { get; set; }

        public string TicketId { get; set; }

        public int CustomerId { get; set; }

        public int Status { get; set; }

        public int Payout { get; set; }

        public int Odds { get; set; }

        public int Stake { get; set; }

        public string Ext1 { get; set; }

        public string Ext2 { get; set; }

        public string Ext3 { get; set; }

        public DoSettlement()
        {
            var appSettings = ConfigurationManager.AppSettings;

            SettlementUrl = appSettings["ColossusBetsAPISettlementURL"];

            Vendor = appSettings["ColossusBetsAPISettlementVendor"];
            VendorKey = appSettings["ColossusBetsAPISettlementVendorKey"];

            Type = appSettings["ColossusBetsAPISettlementType"];
            ProductId = int.Parse(appSettings["ColossusBetsAPISettlementProductId"]);
            GameId = int.Parse(appSettings["ColossusBetsAPISettlementGameId"]); ;

            Status = 0;
            Payout = 0;
            Odds = 0;
            Stake = 0;

            Ext1 = "0";
            Ext2 = "0";
        }

        public string SerializeJSON()
        {
            var buffer = new string[] 
            {
                "{",
                "vendor:",
                Vendor,
                ",vendorKey:",
                VendorKey,
                ",data:{type:",
                Type,
                ",productID:",
                ProductId.ToString(),
                ",gameID:",
                GameId.ToString(),
                ",eventID:",
                EventId.ToString(),
                ",tickets:[{ticketID:",
                TicketId,
                ",customerID:",
                CustomerId.ToString(),
                ",status:",
                Status.ToString(),
                ",payout:",
                Payout.ToString(),
                ",odds:",
                Odds.ToString(),
                ",stake:",
                Stake.ToString(),
                ",ext1:",
                Ext1,
                ",ext2:",
                Ext2,
                ",ext3:",
                Ext3,
                "}]",
                "}",
                "}"
            };

            return string.Join(string.Empty, buffer);
        }
    }
}
