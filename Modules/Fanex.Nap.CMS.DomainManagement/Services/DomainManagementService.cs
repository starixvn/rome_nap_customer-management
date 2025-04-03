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
    using Fanex.Nap.CMS.Infrastructure.Extensions;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Infrastructure.Services.Implementations;
    using Fanex.Nap.CMS.Infrastructure.Services.Interfaces;

    public interface IDomainManagementService
    {
        IEnumerable<Country> GetCountries();

        IEnumerable<Tuple<string, IEnumerable<IdNameModel>>> GetSites();

        IEnumerable<Domain> GetDomains(SearchDomainQuery message);

        Domain GetDomain(int domainId);

        Result AddOrEditDomain(AddOrEditDomainCommand command);

        Result RemoveDomains(IdNameModel[] domains, int userId, string ip);
    }

    public class DomainManagementService : ServiceBase, IDomainManagementService
    {
        private readonly ISettingService settingService;

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

        public Domain GetDomain(int domainId)
            => DynamicRepository.Get<Domain>(new GetDomainByIdCriteria(domainId));

        public Result AddOrEditDomain(AddOrEditDomainCommand command)
        {
            var criteria = new AddOrEditDomainCriteria(BuildDomainListXML(command), command.UserId, command.IP, command.IsEdit);

            var result = DoAction(criteria);
            if (result.IsSuccess)
            {
                result.Message = command.IsEdit
                    ? (command.Domains.Length > 1 ? Labels.EditedDomainsSuccessfully : Labels.EditedDomainSuccessfully)
                    : Labels.AddedDomainSuccessfully;

                AddNormalAnnouncement(criteria.DomainListOutput);
            }

            return result;
        }

        private static string BuildDomainListXML(AddOrEditDomainCommand command)
        {
            var domainList = command.Domains.Select(domain => string.Format(
                "<Domain id=\"{0}\" d=\"{1}\" ss=\"{2}\" s=\"{3}\" stt=\"{4}\" ed=\"{5}\" c=\"{6}\" r=\"{7}\" b=\"{8}\"/>",
                domain.Id == 0 ? string.Empty : domain.Id.ToString(),
                domain.Name,
                command.System,
                command.Site,
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
    }
}
