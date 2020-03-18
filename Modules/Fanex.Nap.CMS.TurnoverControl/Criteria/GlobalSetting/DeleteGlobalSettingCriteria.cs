namespace Fanex.Nap.CMS.TurnoverControl.Criteria.GlobalSetting
{
    public class DeleteGlobalSettingCriteria : NonQueryCriteria
    {
        public DeleteGlobalSettingCriteria(
            int custId,
            bool isGroupHeadline,
            short sportType,
            int userId,
            string ip) : base(isGroupHeadline, sportType, userId, ip)
        {
            CustId = custId;
        }

        public int CustId { get; }

        public override string GetSettingKey()
            => "NAP_TOControl_GlobalSettingDelete";
    }
}
