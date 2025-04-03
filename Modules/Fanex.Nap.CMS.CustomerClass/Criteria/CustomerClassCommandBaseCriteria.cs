using Fanex.Nap.CMS.Infrastructure.Configurations;
using Fanex.Nap.CMS.Infrastructure.Criteria;

namespace Fanex.Nap.CMS.CustomerClass.Criteria
{
    internal class CustomerClassCommandBaseCriteria : CommandBaseCriteria
    {
        public bool IsTest { get; } = AppSettings.IsTest;

        public int UserId { get; set; }

        public string IP { get; set; }
    }
}
