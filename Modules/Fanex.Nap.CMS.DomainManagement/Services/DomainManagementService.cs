namespace Fanex.Nap.CMS.DomainManagement.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using Fanex.Logging.Extensions.Web;
    using Fanex.Nap.CMS.DomainManagement.Constants;
    using Fanex.Nap.CMS.DomainManagement.Criteria;
    using Fanex.Nap.CMS.DomainManagement.Models;
    using Fanex.Nap.CMS.DomainManagement.Requests;
    using Fanex.Nap.CMS.DomainManagement.Resources;
    using Fanex.Nap.CMS.Infrastructure.Configurations;
    using Fanex.Nap.CMS.Infrastructure.Extensions;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Infrastructure.Services.Implementations;
    using Fanex.Nap.CMS.Infrastructure.Services.Interfaces;
    using CommonLabels = Infrastructure.Resources.Labels;

    public interface IDomainManagementService
    {
        IEnumerable<Country> GetCountries();

        IEnumerable<Tuple<string, IEnumerable<IdNameModel>>> GetSites();

        IEnumerable<Domain> GetDomains(SearchDomainQuery message);

        Domain GetDomain(string domainId);

        Result AddOrEditDomain(AddOrEditDomainCommand command);

        Result RemoveDomains(IdNameModel[] domains, int userId, string ip);
    }

    public class DomainManagementService : ServiceBase, IDomainManagementService
    {
        private readonly ISettingService settingService;
        private const byte StatusActive = 3;

        public DomainManagementService()
            : this(new SettingService())
        {
        }

        public DomainManagementService(ISettingService settingService) : base()
        {
            this.settingService = settingService;
        }

        public IEnumerable<Country> GetCountries()
            => settingService.GetCountries();

        public IEnumerable<Tuple<string, IEnumerable<IdNameModel>>> GetSites()
            => settingService.GetSitesMarketCode().GroupBy(
                s => s.SiteLabel,
                (siteLabel, siteList) => Tuple.Create(siteLabel, siteList.Select(s => new IdNameModel { Id = s.Id, Name = BuildSiteMarketCode(s.Name, s.MarketCode) })));

        private static string BuildSiteMarketCode(string site, string marketCode)
            => string.IsNullOrWhiteSpace(marketCode) ? site : $"{site} ({marketCode})";

        public IEnumerable<Domain> GetDomains(SearchDomainQuery message)
        {
            var domains = DynamicRepository.Fetch<Domain>(
                new GetDomainsCriteria(message.System, message.Site, message.Status, message.Remark, message.Countries)).ToList();
            if (domains.Any())
            {
                var siteDictionary = settingService.GetSitesMarketCode().ToDictionary(kvp => kvp.Id, kvp => BuildSiteMarketCode(kvp.Name, kvp.MarketCode));
                var statusDictionary = DomainStatusConst.DomainStatus;
                var countryDictionary = GetCountries().ToDictionary(kvp => kvp.Code, kvp => kvp.Name); 

                foreach (var domain in domains)
                {
                    domain.SiteNames = string.Join(", ", domain.Site.ToArray().Select(s =>
                        siteDictionary.TryGetValue(int.Parse(s), out var siteName) ? siteName : s
                    ));
                    domain.StatusName = statusDictionary.TryGetValue(domain.Status, out var statusName) ? statusName : domain.Status.ToString();
                    domain.RestrictedCountryNames = string.Join("; ", domain.RestrictedCountries.ToArray().Select(c =>
                        countryDictionary.TryGetValue(c, out var country) ? country : c
                    ));
                }
            }

            return domains;
        }

        public Domain GetDomain(string domainId)
            => DynamicRepository.Get<Domain>(new GetDomainByIdCriteria(domainId));

        public Result AddOrEditDomain(AddOrEditDomainCommand command)
        {
            var criteria = new AddOrEditDomainCriteria
            {
                DomainList = BuildDomainListXML(command),
                UserId = command.UserId,
                IP = command.IP,
                IsEdit = command.IsEdit
            };

            var result = DoAction(criteria);
            if (result.IsSuccess)
            {
                result.Message = command.IsEdit
                    ? (command.Domains.Length > 1 ? Labels.EditedDomainsSuccessfully : Labels.EditedDomainSuccessfully)
                    : Labels.AddedDomainSuccessfully;

                AddNormalAnnouncement(criteria.DomainListOutput);
                CallUpdateAgentDomain(criteria.DomainListOutput, command.IsEdit, command.OfferType, command.Status, command.System);
            }

            return result;
        }

        private static string BuildDomainListXML(AddOrEditDomainCommand command)
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

        private static string EncodeXml(string text, bool isAttribute = true, int attrSubStringLength = 5)
            => isAttribute
            ? new XAttribute("__n", text).ToString().Substring(attrSubStringLength).TrimEnd('\"')
            : new XElement("t", text).LastNode.ToString();

        private void AddNormalAnnouncement(string domainList)
        {
            if (!string.IsNullOrWhiteSpace(domainList))
            {
                Task.Run(() =>
                {
                    try
                    {
                        DynamicRepository.Execute(new AddNormalAnnouncementCriteria(domainList));
                    }
                    catch (Exception ex)
                    {
                        ExceptionHandler.Handle(ex);
                    }
                });
            }
        }

        private void CallUpdateAgentDomain(string domainList, bool isEdit, byte offerType, byte domainStatus, string system)
        {
            if(IsSkipUpdateAgentDomain(domainList, isEdit, offerType, domainStatus, system))
            {
                return;
            }

            Task.Run(() =>
            {
                try
                {
                    var allowCashSiteNames = AppSettings.CashOfferedSites.Split(',');
                    var domainSites = ParseDomainSiteMapping(domainList);
                    var allSites = settingService.GetSitesMarketCode();
                    var sitesAllowCash = allSites.Where(site => allowCashSiteNames.Contains(site.Name) || AppSettings.CashOfferedSites == "*");
                    var domainSiteListXml = BuildDomainSiteListXml(domainSites, sitesAllowCash);

                    var criteria = new UpdateAgentDomainCriteria { DomainList = domainSiteListXml };
                    
                    DoAction(criteria);
                }
                catch (Exception ex)
                {
                    ExceptionHandler.Handle(ex);
                }
            });
        }

        private bool IsSkipUpdateAgentDomain(string domainList, bool isEdit, byte offerType, byte domainStatus, string system)
        {
            return domainStatus != StatusActive || offerType == 0 || string.IsNullOrWhiteSpace(domainList) || (!isEdit && system != CommonLabels.Member);
        }

        public Result RemoveDomains(IdNameModel[] domains, int userId, string ip)
        {
            var criteria = new RemoveDomainCriteria
            {
                DomainList = BuildDomainListXml(domains),
                UserId = userId,
                IP = ip
            };

            var result = DoAction(criteria);

            if (result.IsSuccess)
            {
                result.Message = Labels.RemovedDomainSuccessfully;
            }

            return result;
        }
        private static string BuildDomainListXml(IdNameModel[] domains)
        {
            var domainList = domains.Select(domain => $"<Domain id=\"{domain.Id}\" d=\"{domain.Name}\"/>");

            return $"<DomainList>{string.Join(string.Empty, domainList)}</DomainList>";
        }

        private static IEnumerable<Domain> ParseDomainSiteMapping(string domainList)
        {
            var xDocument = XDocument.Parse(domainList);

            return xDocument.Descendants("Domain").Select(domain => new Domain
            {
                DomainId = int.Parse(domain.Attribute("id").Value),
                DomainName = domain.Attribute("d").Value,
                Site = domain.Attribute("s").Value
            });
        }

        private static string BuildDomainSiteListXml(IEnumerable<Domain> domains, IEnumerable<IdNameModel> sites){
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

        private static bool IsDomainOfferCash(Domain domain)
        {
            // Check if domain offers cash based on rules:
            // 1. Agent Site (System = "Agent")
            // 2. OfferType = 1
            // 3. Status is Active (Status = 3)
            return domain.System == CommonLabels.Agent && 
                   domain.OfferType == 1 && 
                   domain.Status == StatusActive;
        }

        private static string BuildDomainListUpdateXml(IEnumerable<Domain> oldDomains, Domain newDomain, IEnumerable<IdNameModel> allowCashSites)
        {
            var addedOfferCashSites = new List<string>();
            var removedOfferCashSites = new List<string>();

            // addedOfferCashSites contains all new sites that offer cash
            // removedOfferCashSites contains all old sites that offer cash but in newDomain not offer cash or removed in newDomain
            
            // Parse new domain sites
            var newDomainSites = newDomain.Site.Split(',').Where(s => !string.IsNullOrWhiteSpace(s)).ToList();

            // Get cash site names and IDs from allowCashSites
            var cashSiteNames = allowCashSites.Select(site => site.Name).ToList();
            var cashSiteNameToId = allowCashSites.ToDictionary(site => site.Name, site => site.Id.ToString());

            // Check if new domain offers cash
            var newDomainOffersCash = IsDomainOfferCash(newDomain);

            // Find new domain sites that offer cash (only if domain offers cash AND site is in allowCashSites)
            var newCashSites = newDomainOffersCash 
                ? newDomainSites.Where(site => cashSiteNames.Contains(site)).ToList()
                : new List<string>();

            // Aggregate all old domain sites and cash sites
            var allOldDomainSites = new List<string>();
            var allOldCashSites = new List<string>();

            foreach (var oldDomain in oldDomains)
            {
                var oldDomainSites = oldDomain.Site.Split(',').Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
                allOldDomainSites.AddRange(oldDomainSites);

                var oldDomainOffersCash = IsDomainOfferCash(oldDomain);
                if (oldDomainOffersCash)
                {
                    var oldCashSites = oldDomainSites.Where(site => cashSiteNames.Contains(site)).ToList();
                    allOldCashSites.AddRange(oldCashSites);
                }
            }

            // Remove duplicates from aggregated lists
            allOldDomainSites = allOldDomainSites.Distinct().ToList();
            allOldCashSites = allOldCashSites.Distinct().ToList();

            // Find added cash sites (sites that are in newCashSites but not in allOldCashSites)
            addedOfferCashSites = newCashSites.Except(allOldCashSites).ToList();

            // Find removed cash sites (sites that are in allOldCashSites but not in newCashSites)
            removedOfferCashSites = allOldCashSites.Except(newCashSites).ToList();

            // Additional check: sites that were completely removed from newDomain but were cash sites in oldDomains
            var completelyRemovedSites = allOldDomainSites.Except(newDomainSites).ToList();
            var removedCashSitesFromRemoval = completelyRemovedSites.Where(site => cashSiteNames.Contains(site)).ToList();
            
            // Add completely removed cash sites to removedOfferCashSites
            removedOfferCashSites.AddRange(removedCashSitesFromRemoval);

            // Remove duplicates from removedOfferCashSites
            removedOfferCashSites = removedOfferCashSites.Distinct().ToList();

            // Build XML for each old domain
            var domainXmls = new List<string>();

            foreach (var oldDomain in oldDomains)
            {
                // Get old domain sites and cash sites for this specific domain
                var oldDomainSites = oldDomain.Site.Split(',').Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
                var oldDomainOffersCash = IsDomainOfferCash(oldDomain);
                var oldCashSites = oldDomainOffersCash 
                    ? oldDomainSites.Where(site => cashSiteNames.Contains(site)).ToList()
                    : new List<string>();

                // Find added cash sites for this specific old domain
                var addedCashSitesForDomain = newCashSites.Except(oldCashSites).ToList();
                if (addedCashSitesForDomain.Count > 0)
                {
                    var addedCashSiteIds = addedCashSitesForDomain.Select(siteName => cashSiteNameToId[siteName]).ToList();
                    var addedCashXml = $"<Domain id=\"{oldDomain.DomainId}\" s=\"{string.Join(",", addedCashSiteIds)}\" cs=\"1\"/>";
                    domainXmls.Add(addedCashXml);
                }

                // Find removed cash sites for this specific old domain
                var removedCashSitesForDomain = oldCashSites.Except(newCashSites).ToList();
                if (removedCashSitesForDomain.Count > 0)
                {
                    var removedCashSiteIds = removedCashSitesForDomain.Select(siteName => cashSiteNameToId[siteName]).ToList();
                    var removedCashXml = $"<Domain id=\"{oldDomain.DomainId}\" s=\"{string.Join(",", removedCashSiteIds)}\" cs=\"0\"/>";
                    domainXmls.Add(removedCashXml);
                }
            }

            return $"<DomainList>{string.Join(string.Empty, domainXmls)}</DomainList>";
        }

    }
}
