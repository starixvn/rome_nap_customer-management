namespace Fanex.NAP.CMS.BetLimitControl.Criteria
{
    using Fanex.Data.Repository;

    public class CheckUsernameBetLimitTypeCriteria : CriteriaBase
    {
        public CheckUsernameBetLimitTypeCriteria(string username, int betLimitTypeID, int userid)
        {
            Username = username;
            BetLimitTypeID = betLimitTypeID;
            Userid = userid;
        }

        public string Username { get; set; }

        public int BetLimitTypeID { get; set; }

        public int Userid { get; set; }

        public int CustID { get; set; }

        public int ErrorCode { get; set; }

        public string ErrorMsg { get; set; }

        public override string GetSettingKey()
            => "NAP_BLControl_CustCheck";

        public override bool IsValid()
            => Userid > 0 && !string.IsNullOrWhiteSpace(Username);
    }
}