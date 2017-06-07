namespace Fanex.Nap.CMS.ColossusBets.Models
{
    using System;
    using System.Configuration;

    public class DoSettlement
    {
        public DoSettlement()
        {
            var appSettings = ConfigurationManager.AppSettings;

            SettlementLink = appSettings["ColossusBetsAPISettlementURL"];

            Vendor = appSettings["ColossusBetsAPISettlementVendor"];
            VendorKey = appSettings["ColossusBetsAPISettlementVendorKey"];

            Type = appSettings["ColossusBetsAPISettlementType"];
            ProductId = int.Parse(appSettings["ColossusBetsAPISettlementProductId"]);
            GameId = int.Parse(appSettings["ColossusBetsAPISettlementGameId"]);

            Status = 0;
            Payout = 0;
            Odds = 0;
            Stake = 0;

            Ext1 = "0";
            Ext2 = "0";
        }

        public string SettlementLink { get; set; }

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

        public string SerializeJSON()
        {
            string jsonFormat = "{{" +
                "\"vendor\":\"{0}\"," +
                "\"vendorKey\":\"{1}\"," +
                "\"data\":" +
                "{{" +
                    "\"type\":\"{2}\"," +
                    "\"productID\":\"{3}\"," +
                    "\"gameID\":\"{4}\"," +
                    "\"eventID\":\"{5}\"," +
                    "\"tickets\":" +
                    "[{{" +
                        "\"ticketID\":\"{6}\"," +
                        "\"customerID\":\"{7}\"," +
                        "\"status\":\"0\"," +
                        "\"payout\":\"0\"," +
                        "\"odds\":\"0\"," +
                        "\"stake\":\"0\"," +
                        "\"settleDate\":\"{9}\"," +
                        "\"ext1\":\"0\"," +
                        "\"ext2\":\"0\"," +
                        "\"ext3\":\"{8}\"" +
                    "}}]" +
                "}}" +
            "}}";

            return string.Format(jsonFormat, Vendor, VendorKey, Type, ProductId, GameId, EventId, TicketId, CustomerId, Ext3, DateTime.Today.ToString("yyyy/MM/dd"));
        }
    }
}
