namespace Fanex.Nap.CMS.ColossusBets.Criteria
{
    using Fanex.Data.Repository;

    public class SaveBonusTicketCriteria : CriteriaBase
    {
        public long RefNo { get; set; }

        public int CustId { get; set; }

        public string League { get; set; }

        public string Match { get; set; }

        public string Choice { get; set; }

        public string Result { get; set; }

        [SpParam(Ignored = true)]
        public int StatusId { get; set; }

        public string Status { get; set; }

        public decimal WinAmount { get; set; }

        public string IP { get; set; }

        public int UserId { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_ColossusBets_BonusTicket_InsUpd";
        }
    }
}
