namespace Fanex.Nap.CMS.ManageChatbots.Criteria.ChatbotMapping
{
    using Fanex.Data.Repository;

    public class GetNapAccountingPolicy : CriteriaBase
    {
        public int CustID { get; set; }

        public override string GetSettingKey()
            => "Acc_Con_FileNameSelByCustID";

        public override bool IsValid()
            => true;
    }
}