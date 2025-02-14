using System.Linq;
using Fanex.Nap.CMS.Infrastructure.Criteria;
using Fanex.Nap.CMS.Infrastructure.Models;

namespace Fanex.Nap.CMS.DomainManagement.Criteria
{
    public class RemoveDomainCriteria : CommandBaseCriteria
    {
        public RemoveDomainCriteria(IdNameModel[] domains, int userId, string ip)
        {
            DomainList = BuildDomainListXml(domains);
            UserId = userId;
            IP = ip;
        }

        public string DomainList { get; }

        public int UserId { get; }

        public string IP { get; }

        public override string GetSettingKey()
            => "NAP_Domain_Del";

        private static string BuildDomainListXml(IdNameModel[] domains)
        {
            var domainList = domains.Select(domain => $"<Domain id=\"{domain.Id}\" d=\"{domain.Name}\"/>");

            return $"<DomainList>{string.Join(string.Empty, domainList)}</DomainList>";
        }

        public override bool IsValid()
            => true;
    }
}
