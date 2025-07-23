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
                        ? $"<Domain id=\"{domain.DomainId}\" s=\"{string.Join(",", siteIds)}\" d=\"{domain.DomainName}\"/>"
                        : null;
                })
                .Where(xml => xml != null);

            return $"<DomainList>{string.Join(string.Empty, domainList)}</DomainList>";
        }

        public static bool IsDomainOfferCash(Domain domain)
        {
            // Check if domain offers cash based on rules:
            // 1. Member Site (System = "Member")
            // 2. OfferType = 1
            // 3. Status is Active (Status = 3)
            return domain.System == CommonLabels.Member && 
                   domain.OfferType == 1 && 
                   domain.Status == StatusActive;
        }
        
        public static bool IsDomainOfferCash(AddOrEditDomainCommand domain)
        {
            // Check if domain offers cash based on rules:
            // 1. Member Site (System = "Member")
            // 2. OfferType = 1
            // 3. Status is Active (Status = 3)
            return domain.System == CommonLabels.Member && 
                   domain.OfferType == 1 && 
                   domain.Status == StatusActive;
        }

        public static string BuildDomainListUpdateXml(IEnumerable<Domain> oldDomains, AddOrEditDomainCommand newDomain, IEnumerable<IdNameModel> allowCashSites)
        {
            if (oldDomains == null || !oldDomains.Any())
            {
                return "<DomainList></DomainList>";
            }

            var cashSiteLookup = BuildCashSiteLookup(allowCashSites);

            // Handle null newDomain (delete operation)
            if (newDomain == null)
            {
                return BuildDeleteDomainXml(oldDomains, cashSiteLookup);
            }

            var newDomainCashSites = GetDomainCashSites(newDomain, cashSiteLookup);
            var oldDomainData = BuildOldDomainData(oldDomains, cashSiteLookup);

            return BuildUpdateDomainXml(oldDomainData, newDomainCashSites);
        }

        private static HashSet<string> BuildCashSiteLookup(IEnumerable<IdNameModel> allowCashSites)
        {
            if (allowCashSites == null || !allowCashSites.Any())
            {
                return new HashSet<string>();
            }

            return new HashSet<string>(allowCashSites.Select(site => site.Id.ToString()));
        }

        private static HashSet<string> GetDomainCashSites(AddOrEditDomainCommand domain, HashSet<string> cashSiteIds)
        {
            if (domain == null || string.IsNullOrWhiteSpace(domain.Site))
            {
                return new HashSet<string>();
            }

            var domainSites = ParseSiteIds(domain.Site);
            var domainOffersCash = IsDomainOfferCash(domain);
            
            return domainOffersCash 
                ? domainSites.Where(siteId => cashSiteIds.Contains(siteId)).ToHashSet()
                : new HashSet<string>();
        }

        private static List<OldDomainInfo> BuildOldDomainData(IEnumerable<Domain> oldDomains, HashSet<string> cashSiteIds)
        {
            return oldDomains.Select(oldDomain =>
            {
                var oldDomainSites = ParseSiteIds(oldDomain.Site);
                var oldDomainOffersCash = IsDomainOfferCash(oldDomain);
                var oldCashSites = oldDomainOffersCash 
                    ? oldDomainSites.Where(siteId => cashSiteIds.Contains(siteId)).ToHashSet()
                    : new HashSet<string>();

                return new OldDomainInfo
                {
                    Domain = oldDomain,
                    Sites = oldDomainSites,
                    CashSites = oldCashSites,
                    OffersCash = oldDomainOffersCash
                };
            }).ToList();
        }

        private static string BuildDeleteDomainXml(IEnumerable<Domain> oldDomains, HashSet<string> cashSiteIds)
        {
            var xmlBuilder = new System.Text.StringBuilder();
            xmlBuilder.Append("<DomainList>");

            foreach (var oldDomain in oldDomains)
            {
                BuildDeleteDomainXmlEntry(oldDomain, cashSiteIds, xmlBuilder);
            }

            xmlBuilder.Append("</DomainList>");
            return xmlBuilder.ToString();
        }

        private static void BuildDeleteDomainXmlEntry(Domain oldDomain, HashSet<string> cashSiteIds, System.Text.StringBuilder xmlBuilder)
        {
            var oldDomainSites = ParseSiteIds(oldDomain.Site);
            var oldDomainOffersCash = IsDomainOfferCash(oldDomain);
            var oldCashSites = oldDomainOffersCash 
                ? oldDomainSites.Where(siteId => cashSiteIds.Contains(siteId)).ToHashSet()
                : new HashSet<string>();

            if (oldCashSites.Count > 0)
            {
                xmlBuilder.Append($"<Domain id=\"{oldDomain.DomainId}\" d=\"{oldDomain.DomainName}\" s=\"{string.Join(",", oldCashSites)}\" cs=\"0\"/>");
            }
        }

        private static string BuildUpdateDomainXml(List<OldDomainInfo> oldDomainData, HashSet<string> newCashSites)
        {
            var xmlBuilder = new System.Text.StringBuilder();
            xmlBuilder.Append("<DomainList>");

            foreach (var oldDomainInfo in oldDomainData)
            {
                BuildDomainXmlEntries(oldDomainInfo, newCashSites, xmlBuilder);
            }

            xmlBuilder.Append("</DomainList>");
            return xmlBuilder.ToString();
        }

        private static void BuildDomainXmlEntries(OldDomainInfo oldDomainInfo, HashSet<string> newCashSites, System.Text.StringBuilder xmlBuilder)
        {
            // Find added cash sites for this specific old domain
            var addedCashSitesForDomain = newCashSites.Except(oldDomainInfo.CashSites).ToList();
            if (addedCashSitesForDomain.Count > 0)
            {
                xmlBuilder.Append($"<Domain id=\"{oldDomainInfo.Domain.DomainId}\" d=\"{oldDomainInfo.Domain.DomainName}\" s=\"{string.Join(",", addedCashSitesForDomain)}\" cs=\"1\"/>");
            }

            // Find removed cash sites for this specific old domain
            var removedCashSitesForDomain = oldDomainInfo.CashSites.Except(newCashSites).ToList();
            if (removedCashSitesForDomain.Count > 0)
            {
                xmlBuilder.Append($"<Domain id=\"{oldDomainInfo.Domain.DomainId}\" d=\"{oldDomainInfo.Domain.DomainName}\" s=\"{string.Join(",", removedCashSitesForDomain)}\" cs=\"0\"/>");
            }
        }

        private static HashSet<string> ParseSiteIds(string siteString)
        {
            if (string.IsNullOrWhiteSpace(siteString))
            {
                return new HashSet<string>();
            }

            return siteString.Split(',')
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .ToHashSet();
        }

        private class OldDomainInfo
        {
            public Domain Domain { get; set; }
            public HashSet<string> Sites { get; set; }
            public HashSet<string> CashSites { get; set; }
            public bool OffersCash { get; set; }
        }
    }
}
