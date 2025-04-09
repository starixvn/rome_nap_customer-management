namespace Fanex.Nap.CMS.Customers.Criteria.VirtualRateEngine
{
    using Fanex.Data.Repository;

    public class GetVREngineMatrixCriteria : CriteriaBase
    {
        public override string GetSettingKey()
            => "NAP_VREngine_CriteriaMatrix_Sel";

        public override bool IsValid()
            => true;
    }
}
