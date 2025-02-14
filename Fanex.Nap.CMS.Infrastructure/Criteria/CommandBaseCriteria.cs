namespace Fanex.Nap.CMS.Infrastructure.Criteria
{
    using Fanex.Data.Repository;

    public class CommandBaseCriteria : CriteriaBase
    {
        public int ErrCode { get; set; }
        public string ErrMsg { get; set; }

        public override string GetSettingKey()
            => string.Empty;

        public override bool IsValid()
            => true;
    }
}
