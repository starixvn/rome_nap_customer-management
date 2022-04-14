namespace Fanex.Nap.CMS.ManageChatbots.Criteria.ChatbotMapping
{
    using Fanex.NAP.Framework.Data;
    public class GetSiteCriteria : CriteriaBase
    {
        public GetSiteCriteria(int isSpecial)
        {
            this.Mode = isSpecial;
        }

        public int Mode { get; }
        public override string GetSettingKey()
            => "Acc_SiteSel_Credit";
    }
}