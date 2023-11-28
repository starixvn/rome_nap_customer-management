namespace Fanex.Nap.CMS.CustomerClass.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Nap.CMS.CustomerClass.Constants;
    using Fanex.Nap.CMS.CustomerClass.Criteria;
    using Fanex.Nap.CMS.CustomerClass.DataAccesses;
    using Fanex.Nap.CMS.CustomerClass.Models;
    using Fanex.Nap.CMS.CustomerClass.Requests;
    using Fanex.Nap.CMS.CustomerClass.Resources;
    using Fanex.Nap.CMS.CustomerClass.Responses;
    using Fanex.Nap.CMS.Infrastructure.Models;

    internal interface IRuleService
    {
        PagedRule GetRules(SearchCustomerClassSettingRequest request);

        IEnumerable<Rule> GetRules(SearchRuleRequest request);

        IEnumerable<Rule> GetRules(short sportType);

        Rule GetRule(int ruleId);

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

        public RuleService(IFilterService filterService = null, IRuleDataAccess ruleDataAccess = null)
        {
            this.filterService = filterService ?? new FilterService();
            this.ruleDataAccess = ruleDataAccess ?? new RuleDataAccess();
        }

        public PagedRule GetRules(SearchCustomerClassSettingRequest request)
        {
            var leagueGroups = filterService.GetLeagueGroups(request.SportType);

            var resultSet = ruleDataAccess.GetPagedRule(new GetPagedRuleListCriteria(request, leagueGroups));

            return new PagedRule(resultSet.Item1, BuildRules(resultSet.Item2));
        }

        private IEnumerable<Rule> BuildRules(IEnumerable<Rule> rules)
        {
            var allCurrencyCount = filterService.GetAllCurrencies().Count();
            var platformDict = PlatformConst.Platforms;
            var allPlatformCount = platformDict.Count;
            var sportTypes = SportTypeConst.SportTypes;
            foreach (var item in rules)
            {
                item.IsAllCurrencies = allCurrencyCount == NumberOfItemsSeparatedByComma(item.Currencies);

                item.IsAllPlatforms = allPlatformCount == NumberOfItemsSeparatedByComma(item.Platforms);
                item.PlatformNames = string.Join(", ", item.Platforms
                    .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(p => platformDict[Convert.ToByte(p)]));

                item.IsAllLeagueGroups = item.LeagueGroups.Equals(Labels.All, StringComparison.OrdinalIgnoreCase);
                item.SportTypeName = sportTypes[item.SportType];
                yield return item;
            }
        }

        private static int NumberOfItemsSeparatedByComma(string value)
            => value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Length;

        public IEnumerable<Rule> GetRules(SearchRuleRequest request)
            => GetRules(new GetRuleListCriteria(request));

        public IEnumerable<Rule> GetRules(short sportType)
            => GetRules(new GetRuleListBySportType(sportType));

        private IEnumerable<Rule> GetRules(QueryCriteriaBase criteria)
        {
            var sportTypes = SportTypeConst.SportTypes;
            var rules = ruleDataAccess.GetRules(criteria);

            foreach (var item in rules)
            {
                item.IsAllLeagueGroups = item.LeagueGroups.Equals(Labels.All, StringComparison.OrdinalIgnoreCase);
                item.SportTypeName = sportTypes[item.SportType];
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
    }
}
