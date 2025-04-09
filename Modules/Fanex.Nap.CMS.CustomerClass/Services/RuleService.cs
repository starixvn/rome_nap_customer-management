namespace Fanex.Nap.CMS.CustomerClass.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http.Headers;
    using System.Text;
    using Fanex.Nap.CMS.CustomerClass.Criteria;
    using Fanex.Nap.CMS.CustomerClass.Models;
    using Fanex.Nap.CMS.CustomerClass.Requests;
    using Fanex.Nap.CMS.CustomerClass.Resources;
    using Fanex.Nap.CMS.CustomerClass.Responses;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Infrastructure.Services.Implementations;

    internal interface IRuleService
    {
        PagedRule GetRules(SearchCustomerClassSettingRequest request, IEnumerable<IdNameModel> sportTypes);

        IEnumerable<Rule> GetRules(SearchRuleRequest request, IEnumerable<IdNameModel> sportTypes = null);

        IEnumerable<Rule> GetRules(short sportType);

        Rule GetRule(int ruleId);

        IEnumerable<RuleDetail> GetRuleDetail(GetRuleDetailRequest request, IEnumerable<IdNameModel> sportTypes);

        Result AddRule(AddRuleRequest request);

        Result EditRule(int ruleId, AddRuleRequest request);

        Result EnableRule(EnableRuleRequest request);

        CheckRuleResponse CheckRuleDel(DeleteRuleRequest request);

        Result DeleteRules(DeleteRuleRequest request);
    }

    internal class RuleService : ServiceBase, IRuleService
    {
        private readonly IFilterService filterService;
        private static IDictionary<byte, string> Platforms
             => new Dictionary<byte, string>
             {
                 [0] = Labels.AllPlatformDetail,
                 [1] = Labels.Desktop,
#pragma warning disable S109
                [2] = Labels.Mobile,
                 [3] = Labels.App,
                 [4] = Labels.Oddsfeed,
                 [5] = Labels.DirectAPI
#pragma warning restore S109
            };

        public RuleService() : this( new FilterService())
        {
        }

        public RuleService(IFilterService filterService) : base()
        {
            this.filterService = filterService;
        }

        public PagedRule GetRules(SearchCustomerClassSettingRequest request, IEnumerable<IdNameModel> sportTypes)
        {
            List<LeagueGroup> leagueGroups = new List<LeagueGroup>();

            foreach (var sportType in sportTypes)
            {
                leagueGroups.AddRange(filterService.GetLeagueGroups(sportType.Id));
            }
            var sportTypeIds = string.Join(",", sportTypes.Select(sp => sp.Id));
            var criteria = new GetPagedRuleListCriteria
            {
                Site = request.Site,
                CustomerClass = request.CustomerClass ?? string.Empty,
                CustomerClassDetail = request.CustomerClassDetail ?? string.Empty,
                SportTypes = sportTypeIds,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                LeagueGroups = BuildLeageGroupXML(leagueGroups),
                Currencies = request.Currencies
            };
            var rules = DynamicRepository.Fetch<Rule>(criteria);
            var resultError = new Result(criteria.ErrCode, criteria.ErrMsg);

            return new PagedRule(criteria.TotalItems, BuildRules(rules), resultError);
        }

        private IEnumerable<Rule> BuildRules(IEnumerable<Rule> rules)
        {
            var sportTypes = filterService.GetSports();
            foreach (var item in rules)
            {
                var sportType = sportTypes.FirstOrDefault(sp => sp.Id == item.SportType);
                if (sportType != null)
                {
                    item.IsAllLeagueGroups = item.LeagueGroups.Equals(Labels.All, StringComparison.OrdinalIgnoreCase);
                    item.SportTypeName = sportType.Name;
                    yield return item;
                }
            }
        }

        public IEnumerable<Rule> GetRules(SearchRuleRequest request, IEnumerable<IdNameModel> sportTypes = null)
        {
            List<LeagueGroup> leagueGroups = new List<LeagueGroup>();
            var sportTypeIds = request.SportType.ToString();

            if (sportTypes == null)
            {
                leagueGroups.AddRange(filterService.GetLeagueGroups(request.SportType));
            }
            else
            {
                foreach (var sportType in sportTypes)
                {
                    leagueGroups.AddRange(filterService.GetLeagueGroups(sportType.Id));
                }
                sportTypeIds = string.Join(",", sportTypes.Select(sp => sp.Id));
            }

            var criteria = new GetRuleListCriteria
            {
                CustomerClass = request.CustomerClass ?? string.Empty,
                CustomerClassDetail = request.CustomerClassDetail ?? string.Empty,
                SportTypes = sportTypeIds,
                LeagueGroup = request.LeagueGroups,
                IsLive = request.IsLive,
                LeagueGroups = BuildLeageGroupXML(leagueGroups)
            };

            return GetRules(criteria).OrderBy(rule => rule.CustomerClass?.ToString());
        }

        public IEnumerable<Rule> GetRules(short sportType)
        {
            var leagues = filterService.GetLeagueGroups(sportType);

            return GetRules(new GetRuleListBySportType(sportType, BuildLeageGroupXML(leagues)));
        }

        private IEnumerable<Rule> GetRules(QueryCriteriaBase criteria)
        {
            var sportTypes = filterService.GetSports();
            var rules = DynamicRepository.Fetch<Rule>(criteria);

            foreach (var item in rules)
            {
                item.IsAllLeagueGroups = item.LeagueGroups.Equals(Labels.All, StringComparison.OrdinalIgnoreCase);
                item.SportTypeName = sportTypes.FirstOrDefault(sp => sp.Id == item.SportType)?.Name;
                yield return item;
            }
        }

        public Rule GetRule(int ruleId)
            => DynamicRepository.Get<Rule>(new GetRuleByIdCriteria(ruleId));

        public Result AddRule(AddRuleRequest request)
        {
            var criteria = new AddRuleCriteria
            {
                Rules = ToXml(request, 0),
                UserId = request.UserId,
                IP = request.IP
            };

            return DoAction(criteria, Labels.AddRuleSuccessfully);
        }

        public Result EditRule(int ruleId, AddRuleRequest request)
        {
            var criteria = new EditRuleCriteria
            {
                Rules = ToXml(request, ruleId),
                UserId = request.UserId,
                IP = request.IP
            };

            return DoAction(criteria, Labels.EditRuleSuccessfully);
        }

        public Result EnableRule(EnableRuleRequest request)
        {
            var criteria = new EnableRuleCriteria
            {
                RuleId = request.RuleId,
                Enable = request.Enable,
                TopPriority = request.TopPriority,
                UserId = request.UserId,
                IP = request.IP
            };
            return DoAction(criteria, Labels.EditRuleSuccessfully);
        }

        public CheckRuleResponse CheckRuleDel(DeleteRuleRequest request)
        {
            var criteria = new CheckRuleDelCriteria
            {
                RuleIDs = request.RuleIDs,
            };
            var affectedRules = DynamicRepository.Fetch<CheckRuleDTO>(criteria);

            return new CheckRuleResponse(string.Join(", ", affectedRules), criteria.ErrCode, criteria.ErrMsg);
        }

        public Result DeleteRules(DeleteRuleRequest request)
        {
            var criteria = new DeleteRuleCriteria
            {
                RuleIDs = request.RuleIDs,
                UserId = request.UserId,
                IP = request.IP
            };

            return DoAction(criteria, Labels.DeleteRuleSuccessfully);
        }
        
        protected string BuildLeageGroupXML(IEnumerable<LeagueGroup> leagueGroups)
        {
            var xmlBuilder = new StringBuilder();

            foreach (var lg in leagueGroups)
            {
                xmlBuilder
                .Append("<r GroupID=\"").Append(lg.Id).Append("\"")
                .Append(" GroupName=\"").Append(WebUtility.HtmlEncode(lg.Name)).Append("\"")
                .Append(" SportType=\"").Append(lg.SportType).Append("\"/>");
            }

            return $"<ROOT>{xmlBuilder.ToString()}</ROOT>";
        }
        private static string ToXml(AddRuleRequest request, int ruleId)
        {
            var xmlBuilder = new StringBuilder();
            xmlBuilder.Append("<ROOT>");

            foreach (var item in request.CCRuleTitles)
            {
                xmlBuilder.Append("<r");
                if (ruleId != 0)
                {
                    xmlBuilder.Append(" RuleID=\"").Append(ruleId).Append("\"");
                }

                xmlBuilder
                    .Append(" CustomerClass=\"").Append(item.CustomerClass == null ? string.Empty : item.CustomerClass.ToString()).Append("\"")
                    .Append(" CustomerClassDetail=\"").Append(item.CustomerClassDetail == null ? string.Empty : item.CustomerClassDetail.ToString()).Append("\"")
                    .Append(" RuleTitle=\"").Append(item.RuleTitle).Append("\"")
                    .Append(" SportType=\"").Append(request.SportType).Append("\"")
                    .Append(" LeagueGroupIDs=\"").Append(request.LeagueGroups).Append("\"")
                    .Append(" IsLive=\"").Append(request.IsLive).Append("\"")
                    .Append(" DiffBalanceRatio=\"").Append(request.DiffBalanceRatio).Append("\"")
                    .Append(" BetDelay=\"").Append(request.BetDelay).Append("\"")
                    .Append(" NextBetInterval=\"").Append(request.NextBetIntervalSec).Append("\"")
                    .Append(" SuperBetInterval=\"").Append(request.SuperBetIntervalSec).Append("\"")
                    .Append(" SiteBetInterval=\"").Append(request.SiteBetIntervalSec).Append("\"")
                    .Append(" MinBet=\"").Append(request.MinBet).Append("\"")
                    .Append(" MaxBet=\"").Append(request.MaxBet).Append("\"")
                    .Append(" MaxPerMatch=\"").Append(request.MaxPerMatch).Append("\"")
                    .Append(" RepeatedBets=\"").Append(request.RepeatedBets).Append("\"")
                    .Append(" SeizeBetTicket=\"").Append(request.SeizeBet).Append("\"")
                    .Append(" DisableParlay=\"").Append(Convert.ToByte(request.DisableParlay)).Append("\"")
                    .Append(" PeakHourSpreadAdjustment=\"").Append(request.PeakHourSpread).Append("\"/>");
            }

            xmlBuilder.Append("</ROOT>");
            return xmlBuilder.ToString();
        }

        public IEnumerable<RuleDetail> GetRuleDetail(GetRuleDetailRequest request, IEnumerable<IdNameModel> sportTypes)
        {
            var sportTypeIds = string.Join(",", sportTypes.Select(sp => sp.Id));
            var criteria = new GetRuleDetailCriteria(request.RuleId, sportTypeIds, request.Currencies, request.IsMultiSite);
            var ruleDetails = DynamicRepository.Fetch<RuleDetail>(criteria);
            
            foreach (var item in ruleDetails)
            {

                item.Sites = string.Join(", ", item.Sites.Split(','));
                item.CurrencyName = string.Join(", ", item.CurrencyName.Split(','));
                item.PlatformNames = string.Join(", ", item.Platform
                           .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                           .Select(p => Platforms[Convert.ToByte(p)]));

                yield return item;
            }
        }
    }
}
