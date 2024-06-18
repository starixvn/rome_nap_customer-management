namespace Fanex.Nap.CMS.CustomerClass.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text;
    using Fanex.Nap.CMS.CustomerClass.Criteria;
    using Fanex.Nap.CMS.CustomerClass.DataAccesses;
    using Fanex.Nap.CMS.CustomerClass.Models;
    using Fanex.Nap.CMS.CustomerClass.Requests;
    using Fanex.Nap.CMS.CustomerClass.Resources;
    using Fanex.Nap.CMS.CustomerClass.Responses;
    using Fanex.Nap.CMS.Infrastructure.Models;

    internal interface IRuleService
    {
        PagedRule GetRules(SearchCustomerClassSettingRequest request, IEnumerable<IdNameModel> sportTypes = null);

        IEnumerable<Rule> GetRules(SearchRuleRequest request, IEnumerable<IdNameModel> sportTypes = null);

        IEnumerable<Rule> GetRules(short sportType);

        Rule GetRule(int ruleId);

        IEnumerable<RuleDetail> GetRuleDetail(GetRuleDetailRequest request, IEnumerable<IdNameModel> sportTypes = null);

        Result AddRule(AddRuleRequest request);

        Result EditRule(int ruleId, AddRuleRequest request);

        Result EnableRule(EnableRuleRequest request);

        CheckRuleResponse CheckRuleDel(DeleteRuleRequest request);

        Result DeleteRules(DeleteRuleRequest request);
    }

    internal class RuleService : IRuleService
    {
        private readonly IFilterService filterService;
        private readonly IRuleDataAccess ruleDataAccess;
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

        public RuleService(IFilterService filterService = null, IRuleDataAccess ruleDataAccess = null)
        {
            this.filterService = filterService ?? new FilterService();
            this.ruleDataAccess = ruleDataAccess ?? new RuleDataAccess();
        }

        public PagedRule GetRules(SearchCustomerClassSettingRequest request, IEnumerable<IdNameModel> sportTypes = null)
        {
            List<LeagueGroup> leagueGroups = new List<LeagueGroup>();

            foreach (var sportType in sportTypes)
            {
                leagueGroups.AddRange(filterService.GetLeagueGroups(sportType.Id));
            }
            var sportTypeIds = string.Join(",", sportTypes.Select(sp => sp.Id));

            var resultSet = ruleDataAccess.GetPagedRule(new GetPagedRuleListCriteria(request, leagueGroups, sportTypeIds));

            return new PagedRule(resultSet.Item1, BuildRules(resultSet.Item2));
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

            return GetRules(new GetRuleListCriteria(request, BuildLeageGroupXML(leagueGroups), sportTypeIds));
        }

        public IEnumerable<Rule> GetRules(short sportType)
        {
            var leagues = filterService.GetLeagueGroups(sportType);

            return GetRules(new GetRuleListBySportType(sportType, BuildLeageGroupXML(leagues)));
        }

        private IEnumerable<Rule> GetRules(QueryCriteriaBase criteria)
        {
            var sportTypes = filterService.GetSports();
            var rules = ruleDataAccess.GetRules(criteria);

            foreach (var item in rules)
            {
                item.IsAllLeagueGroups = item.LeagueGroups.Equals(Labels.All, StringComparison.OrdinalIgnoreCase);
                item.SportTypeName = sportTypes.FirstOrDefault(sp => sp.Id == item.SportType)?.Name;
                yield return item;
            }
        }

        public Rule GetRule(int ruleId)
            => ruleDataAccess.GetRule(new GetRuleByIdCriteria(ruleId));

        public Result AddRule(AddRuleRequest request)
            => ruleDataAccess.DoAction(new AddRuleCriteria(request), Labels.AddRuleSuccessfully);

        public Result EditRule(int ruleId, AddRuleRequest request)
            => ruleDataAccess.DoAction(new EditRuleCriteria(request, ruleId), Labels.EditRuleSuccessfully);

        public Result EnableRule(EnableRuleRequest request)
            => ruleDataAccess.DoAction(new EnableRuleCriteria(request), Labels.EditRuleSuccessfully);

        public CheckRuleResponse CheckRuleDel(DeleteRuleRequest request)
            => ruleDataAccess.CheckRule(new CheckRuleDelCriteria(request));

        public Result DeleteRules(DeleteRuleRequest request)
            => ruleDataAccess.DoAction(new DeleteRuleCriteria(request), Labels.DeleteRuleSuccessfully);
        
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

        public IEnumerable<RuleDetail> GetRuleDetail(GetRuleDetailRequest request, IEnumerable<IdNameModel> sportTypes = null)
        {
            var sportTypeIds = string.Join(",", sportTypes.Select(sp => sp.Id));
            var ruleDetails = ruleDataAccess.GetRuleDetail(new GetRuleDetailCriteria(request.RuleId, sportTypeIds, request.Currencies));
            
            foreach (var item in ruleDetails)
            {
                item.PlatformNames = string.Join(", ", item.Platform
                           .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                           .Select(p => Platforms[Convert.ToByte(p)]));

                yield return item;
            }
        }
    }
}
