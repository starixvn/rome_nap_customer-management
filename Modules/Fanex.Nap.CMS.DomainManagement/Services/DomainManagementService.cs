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
    using Fanex.Nap.CMS.DomainManagement.Helpers;
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
                (siteLabel, siteList) => Tuple.Create(siteLabel, siteList.Select(s => new IdNameModel { Id = s.Id, Name = DomainManagementHelper.BuildSiteMarketCode(s.Name, s.MarketCode) })));

        public IEnumerable<Domain> GetDomains(SearchDomainQuery message)
        {
            var domains = DynamicRepository.Fetch<Domain>(
                new GetDomainsCriteria
                {
                    System = message.System,
                    Site = message.Site,
                    Status = message.Status,
                    OfferType = message.OfferType,
                    Remark = message.Remark,
                    Countries = message.Countries
                }).ToList();
            if (domains.Any())
            {
                var siteDictionary = settingService.GetSitesMarketCode().ToDictionary(kvp => kvp.Id, kvp => DomainManagementHelper.BuildSiteMarketCode(kvp.Name, kvp.MarketCode));
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
            if(command.IsEdit)
            {
                return EditDomain(command);
            }

            return AddDomain(command);
        }

        private Result AddDomain(AddOrEditDomainCommand command)
        {
            var criteria = new AddDomainCriteria
            {
                DomainList = DomainManagementHelper.BuildDomainListXML(command),
                UserId = command.UserId,
                IP = command.IP
            };

            var result = DoAction(criteria);
            if (result.IsSuccess)
            {
                result.Message = command.IsEdit
                    ? (command.Domains.Length > 1 ? Labels.EditedDomainsSuccessfully : Labels.EditedDomainSuccessfully)
                    : Labels.AddedDomainSuccessfully;

                AddNormalAnnouncement(criteria.DomainListOutput);
                CallInsertAgentDomain(criteria.DomainListOutput, command.IsEdit, command.OfferType, command.Status, command.System);
            }

            return result;
        }

        private Result EditDomain(AddOrEditDomainCommand command)
        {
            var criteria = new EditDomainCriteria
            {
                DomainList = DomainManagementHelper.BuildDomainListXML(command),
                UserId = command.UserId,
                IP = command.IP
            };

            var result = DoAction(criteria);

            if (result.IsSuccess)
            {
                result.Message = Labels.EditedDomainSuccessfully;

                AddNormalAnnouncement(criteria.DomainListOutput);
            }

            return result;
        }



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

        private void CallInsertAgentDomain(string domainList, bool isEdit, byte offerType, byte domainStatus, string system)
        {
            if(DomainManagementHelper.IsSkipUpdateAgentDomain(domainList, isEdit, offerType, domainStatus, system))
            {
                return;
            }

            Task.Run(() =>
            {
                try
                {
                    var allowCashSiteNames = AppSettings.CashOfferedSites.Split(',');
                    var domainSites = DomainManagementHelper.ParseDomainSiteMapping(domainList);
                    var allSites = settingService.GetSitesMarketCode();
                    var sitesAllowCash = allSites.Where(site => allowCashSiteNames.Contains(site.Name) || AppSettings.CashOfferedSites == "*");
                    var domainSiteListXml = DomainManagementHelper.BuildDomainSiteListXml(domainSites, sitesAllowCash);

                    var criteria = new InsertAgentDomainCriteria { DomainList = domainSiteListXml };
                    
                    DoAction(criteria);
                }
                catch (Exception ex)
                {
                    ExceptionHandler.Handle(ex);
                }
            });
        }

        public Result RemoveDomains(IdNameModel[] domains, int userId, string ip)
        {
            var criteria = new RemoveDomainCriteria
            {
                DomainList = DomainManagementHelper.BuildDomainListXml(domains),
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

    }
}
