namespace Fanex.Nap.CMS.ColossusBets.Models
{
    using System.ComponentModel.DataAnnotations;

    public class BonusTicketDetail
    {
        [Display(Name = "RefNo", ResourceType = typeof(Resources.ColossusBetsLabels))]
        public long RefNo { get; set; }

        [Display(Name = "Pool", ResourceType = typeof(Resources.ColossusBetsLabels))]
        public string Pool { get; set; }

        public int CustId { get; set; }

        [Display(Name = "Member", ResourceType = typeof(Resources.ColossusBetsLabels))]
        public string Username { get; set; }

        [Display(Name = "League", ResourceType = typeof(Resources.ColossusBetsLabels))]
        public string League { get; set; }

        [Display(Name = "Match", ResourceType = typeof(Resources.ColossusBetsLabels))]
        public string Match { get; set; }

        [Display(Name = "Choice", ResourceType = typeof(Resources.ColossusBetsLabels))]
        public string Choice { get; set; }

        [Display(Name = "ResultOptional", ResourceType = typeof(Resources.ColossusBetsLabels))]
        public string Result { get; set; }

        public int StatusId { get; set; }

        [Display(Name = "Status", ResourceType = typeof(Resources.ColossusBetsLabels))]
        public string Status { get; set; }

        [Display(Name = "Amount", ResourceType = typeof(Resources.ColossusBetsLabels))]
        public decimal WinAmount { get; set; }

        public string TranId3rd { get; set; }

        public string EventId { get; set; }
    }
}
