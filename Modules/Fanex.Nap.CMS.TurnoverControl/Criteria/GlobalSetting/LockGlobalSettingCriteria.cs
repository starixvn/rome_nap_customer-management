using Fanex.Nap.CMS.TurnoverControl.Models;

namespace Fanex.Nap.CMS.TurnoverControl.Criteria.GlobalSetting
{
    public class LockGlobalSettingCriteria : NonQueryCriteria
    {
        public LockGlobalSettingCriteria(
            LockSettingParam param,
            int userId,
            string ip) : base(param.IsGroupHeadline, param.SportType, userId, ip)
        {
            CustId = param.CustId;
            IsGlobalLockTOLimit = param.IsLock;
        }

        public int CustId { get; }

        public bool IsGlobalLockTOLimit { get; }

        public override string GetSettingKey()
            => "NAP_TOControl_GlobalSettingLockTOLimit_Update";
    }
}
