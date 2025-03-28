namespace Fanex.NAP.CMS.Criteria.LossMonitor
{
    using Fanex.Nap.CMS.Infrastructure.Criteria;

    public class DeleteSMACriteria : CommandBaseCriteria
    {
        public int CustId { get; set; }

        public byte Mode { get; set; }

        public int UserId { get; set; }

        public string IP { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_LimitLoss_SMADel";
        }

        public override bool IsValid()
        {
            return CustId > 0 &&
                UserId > 0 &&
                !string.IsNullOrWhiteSpace(IP);
        }
    }
}
