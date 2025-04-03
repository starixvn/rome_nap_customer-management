using Fanex.Nap.CMS.TurnoverControl.Models;

namespace Fanex.Nap.CMS.TurnoverControl.Criteria.GlobalSetting
{
    public class LockGlobalSettingCriteria : NonQueryCriteria
    {
        public int CustId { get; set; }

        public bool IsGlobalLockTOLimit { get; set; }

        public override string GetSettingKey()
            => "NAP_TOControl_GlobalSettingLockTOLimit_Update";
    }
}
