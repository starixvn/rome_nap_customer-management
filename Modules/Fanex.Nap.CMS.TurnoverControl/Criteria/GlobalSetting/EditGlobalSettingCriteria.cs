using Fanex.Nap.CMS.TurnoverControl.Models;

namespace Fanex.Nap.CMS.TurnoverControl.Criteria.GlobalSetting
{
    public class EditGlobalSettingCriteria : NonQueryCriteria
    {
        public EditGlobalSettingCriteria(
            UpdateMaxTurnoverParam param,
            int userId,
            string ip) : base(param.IsGroupHeadline, param.SportType, userId, ip)
        {
            CustId = param.CustId;
            ProfileId = param.ProfileId;
            MaxTurnover = param.MaxTurnover;
        }

        public int CustId { get; }

        public int ProfileId { get; }

        public decimal MaxTurnover { get; }

        public override string GetSettingKey()
            => "NAP_TOControl_GlobalSettingUpdate";
    }
}
