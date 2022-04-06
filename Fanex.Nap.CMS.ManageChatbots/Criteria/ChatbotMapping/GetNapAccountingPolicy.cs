namespace Fanex.Nap.CMS.ManageChatbots.Criteria.ChatbotMapping
{
    using Fanex.NAP.Framework.Data;

    public class GetNapAccountingPolicy: CriteriaBase
    {
        public GetNapAccountingPolicy(int custID)
        {
            this.CustID = custID;
        }

        public int CustID { get; }

        public override string GetSettingKey()
            => "Acc_Con_FileNameSelByCustID";
    }
}