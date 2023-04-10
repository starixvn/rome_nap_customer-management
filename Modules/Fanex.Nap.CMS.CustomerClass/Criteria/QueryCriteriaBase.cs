namespace Fanex.Nap.CMS.CustomerClass.Criteria
{
    using Fanex.Nap.CMS.Infrastructure.Configurations;
    using Fanex.NAP.Framework.Data;

    internal class QueryCriteriaBase : CriteriaBase
    {
        protected QueryCriteriaBase()
        {
        }

        public bool IsTest { get; } = AppSettings.IsTest;
    }
}
