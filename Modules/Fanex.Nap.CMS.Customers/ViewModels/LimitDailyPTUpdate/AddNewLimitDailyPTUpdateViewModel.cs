namespace Fanex.Nap.CMS.Customers.ViewModels.LimitDailyPTUpdate
{
    using System.Web.Mvc;

    public class AddNewLimitDailyPTUpdateViewModel
    {
        public byte Type { get; set; }

        public string Product { get; set; }

        [AllowHtml]
        public string SMAs { get; set; }

        public int Site { get; set; }

        public byte Currency { get; set; }

        public int UserId { get; set; }

        public string IP { get; set; }
        public bool LimitNewAccUpdPT { get; set; }
    }
}
