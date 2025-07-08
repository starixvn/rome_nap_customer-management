namespace Fanex.Nap.CMS.DomainManagement.Helpers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using Fanex.Nap.CMS.DomainManagement.Models;
    using Fanex.Nap.CMS.DomainManagement.Requests;
    using Fanex.Nap.CMS.Infrastructure.Extensions;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using CommonLabels = Infrastructure.Resources.Labels;

    public static class DomainManagementHelper
    {
        private const byte StatusActive = 3;

        public static string BuildSiteMarketCode(string site, string marketCode)
            => string.IsNullOrWhiteSpace(marketCode) ? site : $"{site} ({marketCode})";

        public static string BuildDomainListXML(AddOrEditDomainCommand command)
        {
            var domainList = command.Domains.Select(domain => string.Format(
                "<Domain id=\"{0}\" d=\"{1}\" ss=\"{2}\" s=\"{3}\" cs=\"{4}\" stt=\"{5}\" ed=\"{6}\" c=\"{7}\" r=\"{8}\" b=\"{9}\" />",
                domain.Id == 0 ? string.Empty : domain.Id.ToString(),
                domain.Name,
                command.System,
                command.Site,
                command.System == CommonLabels.Member ? command.OfferType : 0,
                command.Status,
                command.ExpiredDate.HasValue ? command.ExpiredDate.Value.GetEndOfDate().ToString() : string.Empty,
                command.RestrictedCountries,
                string.IsNullOrWhiteSpace(command.Remark) ? command.Remark : EncodeXml(command.Remark),
                command.BrandName));

            return $"<DomainList>{string.Join(string.Empty, domainList)}</DomainList>";
        }

        public static string EncodeXml(string text, bool isAttribute = true, int attrSubStringLength = 5)
            => isAttribute
            ? new XAttribute("__n", text).ToString().Substring(attrSubStringLength).TrimEnd('\"')
            : new XElement("t", text).LastNode.ToString();

        public static bool IsSkipUpdateAgentDomain(string domainList, bool isEdit, byte offerType, byte domainStatus, string system)
        {
            return domainStatus != StatusActive || offerType == 0 || string.IsNullOrWhiteSpace(domainList) || (!isEdit && system != CommonLabels.Member);
        }

        public static string BuildDomainListXml(IdNameModel[] domains)
        {
            var domainList = domains.Select(domain => $"<Domain id=\"{domain.Id}\" d=\"{domain.Name}\"/>");

            return $"<DomainList>{string.Join(string.Empty, domainList)}</DomainList>";
        }

        public static IEnumerable<Domain> ParseDomainSiteMapping(string domainList)
        {
            var xDocument = XDocument.Parse(domainList);

            return xDocument.Descendants("Domain").Select(domain => new Domain
            {
                DomainId = int.Parse(domain.Attribute("id").Value),
                DomainName = domain.Attribute("d").Value,
                Site = domain.Attribute("s").Value
            });
        }

        public static string BuildDomainSiteListXml(IEnumerable<Domain> domains, IEnumerable<IdNameModel> sites)
        {
            var domainList = domains
                .Select(domain =>
                {
                    var domainSites = domain.Site.Split(',');
                    var siteIds = sites.Where(site => domainSites.Any(s => s == site.Name))
                                     .Select(site => site.Id.ToString())
                                     .ToList();
                    return siteIds.Count > 0 
                        ? $"<Domain id=\"{domain.DomainId}\" s=\"{string.Join(",", siteIds)}\"/>"
                        : null;
                })
                .Where(xml => xml != null);

            return $"<DomainList>{string.Join(string.Empty, domainList)}</DomainList>";
        }

        public static bool IsDomainOfferCash(Domain domain)
        {
            // Check if domain offers cash based on rules:
            // 1. Agent Site (System = "Agent")
            // 2. OfferType = 1
            // 3. Status is Active (Status = 3)
            return domain.System == CommonLabels.Agent && 
                   domain.OfferType == 1 && 
                   domain.Status == StatusActive;
        }
        
        public static bool IsDomainOfferCash(AddOrEditDomainCommand domain)
        {
            // Check if domain offers cash based on rules:
            // 1. Agent Site (System = "Agent")
            // 2. OfferType = 1
            // 3. Status is Active (Status = 3)
            return domain.System == CommonLabels.Agent && 
                   domain.OfferType == 1 && 
                   domain.Status == StatusActive;
        }

        public static string BuildDomainListUpdateXml(IEnumerable<Domain> oldDomains, AddOrEditDomainCommand newDomain, IEnumerable<IdNameModel> allowCashSites)
        {
            // Pre-compute cash site lookup for O(1) access
            var cashSiteNames = new HashSet<string>(allowCashSites.Select(site => site.Name));
            var cashSiteNameToId = allowCashSites.ToDictionary(site => site.Name, site => site.Id.ToString());

            // Parse new domain sites once
            var newDomainSites = newDomain.Site.Split(',')
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .ToHashSet(); // Use HashSet for O(1) lookups

            // Check if new domain offers cash once
            var newDomainOffersCash = IsDomainOfferCash(newDomain);

            // Find new domain cash sites once
            var newCashSites = newDomainOffersCash 
                ? newDomainSites.Where(site => cashSiteNames.Contains(site)).ToHashSet()
                : new HashSet<string>();

            // Pre-process old domains to avoid repeated computations
            var oldDomainData = oldDomains.Select(oldDomain =>
            {
                var oldDomainSites = oldDomain.Site.Split(',')
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .ToHashSet();
                
                var oldDomainOffersCash = IsDomainOfferCash(oldDomain);
                var oldCashSites = oldDomainOffersCash 
                    ? oldDomainSites.Where(site => cashSiteNames.Contains(site)).ToHashSet()
                    : new HashSet<string>();

                return new
                {
                    Domain = oldDomain,
                    Sites = oldDomainSites,
                    CashSites = oldCashSites,
                    OffersCash = oldDomainOffersCash
                };
            }).ToList();

            // Build XML efficiently using StringBuilder
            var xmlBuilder = new System.Text.StringBuilder();
            xmlBuilder.Append("<DomainList>");

            foreach (var oldDomainInfo in oldDomainData)
            {
                // Find added cash sites for this specific old domain
                var addedCashSitesForDomain = newCashSites.Except(oldDomainInfo.CashSites).ToList();
                if (addedCashSitesForDomain.Count > 0)
                {
                    var addedCashSiteIds = addedCashSitesForDomain.Select(siteName => cashSiteNameToId[siteName]);
                    xmlBuilder.Append($"<Domain id=\"{oldDomainInfo.Domain.DomainId}\" s=\"{string.Join(",", addedCashSiteIds)}\" cs=\"1\"/>");
                }

                // Find removed cash sites for this specific old domain
                var removedCashSitesForDomain = oldDomainInfo.CashSites.Except(newCashSites).ToList();
                if (removedCashSitesForDomain.Count > 0)
                {
                    var removedCashSiteIds = removedCashSitesForDomain.Select(siteName => cashSiteNameToId[siteName]);
                    xmlBuilder.Append($"<Domain id=\"{oldDomainInfo.Domain.DomainId}\" s=\"{string.Join(",", removedCashSiteIds)}\" cs=\"0\"/>");
                }
            }

            xmlBuilder.Append("</DomainList>");
            return xmlBuilder.ToString();
        }
    }
}
