using Fanex.Nap.CMS.Infrastructure.Configurations;
using Fanex.Nap.CMS.Infrastructure.Criteria;

namespace Fanex.Nap.CMS.CustomerClass.Criteria
{
    internal class CustomerClassCommandBaseCriteria : CommandBaseCriteria
    {
        protected CustomerClassCommandBaseCriteria(int userId, string ip)
        {
            UserId = userId;
            IP = ip;
        }
        public bool IsTest { get; } = AppSettings.IsTest;

        public int UserId { get; }

        public string IP { get; }
    }
}
