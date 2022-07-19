namespace Fanex.Nap.CMS.ManageChatbots.Criteria.ChatbotMapping
{
    using Fanex.NAP.Framework.Data;

    public class GetNapAccountingPolicy : CriteriaBase
    {
        public int CustID { get; set; }

        public override string GetSettingKey()
            => "Acc_Con_FileNameSelByCustID";
    }
}