namespace Fanex.Nap.CMS.CustomerClass.Criteria
{
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Infrastructure.Configurations;

    internal class QueryCriteriaBase : CriteriaBase
    {
        protected QueryCriteriaBase()
        {
        }

        public bool IsTest { get; } = AppSettings.IsTest;

        public override string GetSettingKey()
            => string.Empty;

        public override bool IsValid()
            => true;
    }
}
