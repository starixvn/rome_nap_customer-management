namespace Fanex.Nap.CMS.PayoutManagement.Models
{
    using System;
    using System.Data;
    using Fanex.Common;
    using Fanex.Nap.CMS.Infrastructure.Models;

    public class BetTicket
    {
        public int CustomerId { get; set; }

        public long RefNo { get; set; }

        public string TransId3rd { get; set; }

        public IdNameModel Product { get; set; }

        public DateTime WinlostDate { get; set; }

        public decimal WinAmount { get; set; }

        public string Sitename { get; set; }

        public void ToBaseBetTicket(IDataReader reader)
        { 
            CustomerId = Converter.ToInt(reader["custid"]);
            RefNo = Converter.To<long>(reader["refno"]);
            TransId3rd = Converter.ToString2(reader["TransId3rd"]);
            Product = new IdNameModel 
            {
                Id = Converter.ToInt(reader["ProductId"]),
                Name = Converter.ToString2(reader["ProductName"]),
            };

            WinlostDate = Converter.ToDateTime(reader["WinlostDate"]);
            WinAmount = Converter.ToDecimal(reader["WinAmount"]);
            Sitename = Converter.ToString2(reader["SiteName"]);
        }
    }
}
