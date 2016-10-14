using Fanex.Data.Repository;

namespace Fanex.NAP.CMS.Criteria.Ips
{
    public class GetIpVnCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "GetIpVn";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
