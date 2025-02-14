namespace Fanex.Nap.CMS.BetRestriction.Criterias
{
    using Fanex.Data.Repository;

    public class CheckAccountCriteria : CriteriaBase
    {
        public string UserName { get; set; }

        public short? SportType { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_BetRestriction_CheckExistLine";
        }

        public override bool IsValid()
            => true;
    }
}
