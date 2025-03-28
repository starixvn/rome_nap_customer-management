namespace Fanex.Nap.CMS.Customers.Services.VirtualRateEngine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Nap.CMS.Customers.Constants.ProblemAccount;
    using Fanex.Nap.CMS.Customers.Constants.VirtualRateEngine;
    using Fanex.Nap.CMS.Customers.Criteria.VirtualRateEngine;
    using Fanex.Nap.CMS.Customers.Models.VirtualRateEngine;
    using Fanex.Nap.CMS.Customers.Resources;
    using Fanex.Nap.CMS.Customers.ViewModels.VirtualRateEngine;
    using Fanex.Nap.CMS.Infrastructure.Extensions;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Infrastructure.Services.Implementations;
    using Fanex.Nap.CMS.Infrastructure.Web;
    using Newtonsoft.Json;

    public class VREngineService : ServiceBase, IVREngineService
    {
        private const string SiteType = "Site";
        private const string CustomerClass = "CustomerClass";
        private const string CustomerClassDetail = "CustomerClassDetail";
        private const string CurrentVR = "VRValue";
        private const string CurrentVRStatus = "VRStatus";
        private const string Sport = "Sporttype";
        private const string BetType = "Bettype";
        private const string Event = "LiveIndicator";
        private const string LeagueGroup = "LeagueGroup";
        private const string Betfrom = "BetFrom";
        private const string Stake = "StakeRM";
        private const string TicketDoW = "DateOfWeek";
        private const string TicketTime = "TicketTime";
        private const string Currency = "Currency";
        private const string SportBetTypeMatrixCachedKey = "SportBetTypeMatrixCachedKey";
        private const int CachedDefaultInMinutes = 60;

        public VREngineMatrix GetVREngineMatrix()
        {
            var typeList = new[] {
                typeof(VREngineSiteDTO),
                typeof(BetFromDTO),
                typeof(SportTypeDTO),
                typeof(BetTypeDTO),
                typeof(LeagueGroupDTO),
                typeof(CurrencyDTO),
                typeof(SportDetailDTO)
            };
            var result = DynamicRepository.FetchMultiple(new GetVREngineMatrixCriteria(), typeList);
            var sites = result[0].Cast<VREngineSiteDTO>();
            var creditSites = sites.Where(s => s.SiteType == SiteTypeConst.Credit).Select(s => new IdNameModel { Id =  s.SiteId, Name = s.Site });
            var licenseeSites = sites.Where(s => s.SiteType == SiteTypeConst.Licensee).Select(s => new IdNameModel { Id = s.SiteId, Name = s.Site });
            var betFromList = result[1].Cast<BetFromDTO>().Select(b => new VREngineBetfrom(b.BetFrom, b.Name));
            var sports = result[2].Cast<SportTypeDTO>().Select(s => new IdNameModel { Id = s.Sporttype, Name = s.SportName });
            var betTypes = result[3].Cast<BetTypeDTO>().Select(b => new IdNameModel { Id = b.BetType, Name = b.BetTypeName });
            var leagueGroupList = result[4].Cast<LeagueGroupDTO>().Select(l => new IdNameModel { Id = l.LeagueGroupId, Name = l.GroupName }).OrderBy(lg => lg.Name);
            var currenciesList = result[5].Cast<CurrencyDTO>().Select(c => new IdNameModel { Id = c.CurrencyId, Name = c.CurrencyName });
            var detailSports = result[6].Cast<SportDetailDTO>().Select(s => new IdNameModel { Id = s.SportType, Name = s.SportName });

            return new VREngineMatrix(creditSites, licenseeSites, detailSports, betFromList, sports, betTypes, leagueGroupList, currenciesList);
        }

        public IEnumerable<IdNameModel> GetLeagues(
            string leagueGroupIds,
            string leagueGroupListNotIn,
            string leagues)
        {
            var jsonSportBetType = GetSportBettypeMatrix();

            return DynamicRepository.Fetch<LeagueDTO>(
                new GetVREngineLeagueCriteria(leagueGroupIds, leagueGroupListNotIn, leagues, jsonSportBetType))
                .Select(lg => new IdNameModel { Id = lg.LeagueId, Name = lg.LeagueName});
        }

        public VREngineModel GetRules()
        {
            var matrix = GetVREngineMatrix();

            return new VREngineModel(GetRules(matrix), matrix);
        }

        private IEnumerable<VREngineRule> GetRules(VREngineMatrix matrix)
        {
            var rules = DynamicRepository.Fetch<VREngineRule>(new GetVREngineRuleListCriteria());

            foreach (var rule in rules)
            {
                if (!string.IsNullOrWhiteSpace(rule.EnabledCriteria))
                {
                    SetEnabledCriteria(rule);
                    BuildCriteriaCell(rule, matrix);
                }

                rule.AdjustmentCell = BuildAdjustmentCell(rule);
                rule.StatusName = ActiveConst.MapToActiveName(rule.Status);
                rule.CreatedDate = rule.CreatedDTS.ToShortFormatedDateWithSlash();

                yield return rule;
            }
        }

        public VREngineRule GetRule(int ruleId)
        {
            var rule = DynamicRepository.Get<VREngineRule>(new GetVREngineRuleCriteria(ruleId));

            if (rule != null && !string.IsNullOrWhiteSpace(rule.EnabledCriteria))
            {
                SetEnabledCriteria(rule);
            }

            return rule;
        }

        private static void SetEnabledCriteria(VREngineRule rule)
        {
            var enabledCriteria = rule.EnabledCriteria.ToArray().ToDictionary(kvp => kvp, kvp => true);
            rule.EnabledSiteType = enabledCriteria.ContainsKey(SiteType);
            rule.EnabledCustomerClass = enabledCriteria.ContainsKey(CustomerClass);
            rule.EnabledCustomerClassDetail = enabledCriteria.ContainsKey(CustomerClassDetail);
            rule.EnabledCurrentVR = enabledCriteria.ContainsKey(CurrentVR);
            rule.EnabledCurrentVRStatus = enabledCriteria.ContainsKey(CurrentVRStatus);
            rule.EnabledSport = enabledCriteria.ContainsKey(Sport);
            rule.EnabledBetType = enabledCriteria.ContainsKey(BetType);
            rule.EnabledEvent = enabledCriteria.ContainsKey(Event);
            rule.EnabledLeagueGroup = enabledCriteria.ContainsKey(LeagueGroup);
            rule.EnabledBetfrom = enabledCriteria.ContainsKey(Betfrom);
            rule.EnabledStake = enabledCriteria.ContainsKey(Stake);
            rule.EnabledTicketDoW = enabledCriteria.ContainsKey(TicketDoW);
            rule.EnabledTicketTime = enabledCriteria.ContainsKey(TicketTime);
            rule.EnabledTicketCur = enabledCriteria.ContainsKey(Currency);
        }

        #region Build Criteria methods

        private static void BuildCriteriaCell(VREngineRule rule, VREngineMatrix matrix)
        {
            BuildSiteTypeCriteria(rule, matrix.CreditSites, matrix.LicenseeSites);

            BuildSportDetailCriteria(rule, matrix.DetailSports);

            BuildCurrentVRStatusCriteria(rule);

            BuildSportCriteria(rule, matrix.Sports);

            BuildBetTypeCriteria(rule, matrix.BetTypes);

            BuildEventCriteria(rule);

            BuildLeagueGroupCriteria(rule, matrix.LeagueGroups);

            BuildBetfromCriteria(rule, matrix.BetfromList);

            BuildTicketDoWCriteria(rule);

            BuildTicketCurrencyCriteria(rule, matrix.Currencies);
        }

        private static void BuildSiteTypeCriteria(
            VREngineRule rule,
            IEnumerable<IdNameModel> creditSites,
            IEnumerable<IdNameModel> licenseeSites)
        {
            if (!rule.EnabledSiteType)
            {
                return;
            }

            IEnumerable<IdNameModel> sites = Enumerable.Empty<IdNameModel>();
            var isBuildSiteName = !string.IsNullOrWhiteSpace(rule.SiteIdListNotIn) || rule.SiteIdList != "0";

            if (rule.SiteType == SiteTypeConst.Credit)
            {
                rule.SiteTypeName = nameof(SiteTypeConst.Credit);

                if (isBuildSiteName)
                {
                    sites = creditSites;
                }
            }
            else if (rule.SiteType == SiteTypeConst.Licensee)
            {
                rule.SiteTypeName = nameof(SiteTypeConst.Licensee);

                if (isBuildSiteName)
                {
                    sites = licenseeSites;
                }
            }
            else
            {
                rule.SiteTypeName = Labels.All;

                if (isBuildSiteName)
                {
                    sites = creditSites.Concat(licenseeSites);
                }
            }

            var nameTuple = BuildMultipleSelectionCriteria(rule.SiteIdList, rule.SiteIdListNotIn, sites);
            rule.SiteNameList = nameTuple.Item1;
            rule.SiteNameListNotIn = nameTuple.Item2;
        }

        private static void BuildCurrentVRStatusCriteria(VREngineRule rule)
        {
            if (rule.EnabledCurrentVRStatus)
            {
                rule.VRStatusName = rule.VRStatus == CurrentVRStatusConst.Enable
                    ? VREngineResource.Enabled
                    : VREngineResource.Disabled;
            }
        }
        private static void BuildSportDetailCriteria(VREngineRule rule, IEnumerable<IdNameModel> sports)
        {
            if (rule.EnabledCustomerClassDetail)
            {
                var nameTuple = BuildMultipleSelectionCriteria(
                    rule.SportTypeCCList,
                    rule.SportTypeCCListNotIn,
                    sports);

                rule.SportTypeCCNameList = nameTuple.Item1;
                rule.SportTypeCCNameListNotIn = nameTuple.Item2;
            }
        }

        private static void BuildSportCriteria(VREngineRule rule, IEnumerable<IdNameModel> sports)
        {
            if (rule.EnabledSport)
            {
                var nameTuple = BuildMultipleSelectionCriteria(
                    rule.SportTypeList,
                    rule.SportTypeListNotIn,
                    sports);

                rule.SportTypeNameList = nameTuple.Item1;
                rule.SportTypeNameListNotIn = nameTuple.Item2;
            }
        }

        private static void BuildBetTypeCriteria(VREngineRule rule, IEnumerable<IdNameModel> betTypes)
        {
            if (rule.EnabledBetType)
            {
                var nameTuple = BuildMultipleSelectionCriteria(
                    rule.BetTypeList,
                    rule.BetTypeListNotIn,
                    betTypes);

                rule.BetTypeNameList = nameTuple.Item1;
                rule.BetTypeNameListNotIn = nameTuple.Item2;
            }
        }

        private static void BuildEventCriteria(VREngineRule rule)
        {
            if (rule.EnabledEvent)
            {
                rule.LiveIndicatorName = rule.LiveIndicator == EventConst.Live
                    ? nameof(EventConst.Live)
                    : VREngineResource.NonLive;
            }
        }

        private static void BuildLeagueGroupCriteria(VREngineRule rule, IEnumerable<IdNameModel> leagueGroups)
        {
            if (rule.EnabledLeagueGroup)
            {
                var nameTuple = BuildMultipleSelectionCriteria(
                    rule.LeagueGroupList,
                    rule.LeagueGroupListNotIn,
                    leagueGroups,
                    "-1");

                rule.LeagueGroupNameList = nameTuple.Item1;
                rule.LeagueGroupNameListNotIn = nameTuple.Item2;
            }
        }

        private static void BuildBetfromCriteria(VREngineRule rule, IEnumerable<VREngineBetfrom> betfromList)
        {
            if (rule.EnabledBetfrom)
            {
                rule.BetfromName = betfromList.FirstOrDefault(b => b.Id == rule.Betfrom)?.Name;
            }
        }

        private static void BuildTicketDoWCriteria(VREngineRule rule)
        {
            if (rule.EnabledTicketDoW)
            {
                var nameTuple = BuildMultipleSelectionCriteria(
                    rule.DoWList,
                    rule.DoWListNotIn,
                    VREngineDayOfWeek.DayOfWeeks);

                rule.DoWNameList = nameTuple.Item1;
                rule.DoWNameListNotIn = nameTuple.Item2;
            }
        }
        private static void BuildTicketCurrencyCriteria(VREngineRule rule, IEnumerable<IdNameModel> currencies)
        {
            if (rule.EnabledTicketCur)
            {
                var nameTuple = BuildMultipleSelectionCriteria(
                    rule.CurrencyIDList,
                    rule.CurrencyIDListNotIn,
                    currencies);

                rule.CurrencyNameList = nameTuple.Item1;
                rule.CurrencyNameListNotIn = nameTuple.Item2;
            }
        }

        private static Tuple<string, string> BuildMultipleSelectionCriteria(
            string idList,
            string idListNotApply,
            IEnumerable<IdNameModel> idNameList,
            string allValue = "0")
        {
            string nameList = null;
            string nameListNotApply = null;

            if (!string.IsNullOrWhiteSpace(idListNotApply))
            {
                nameListNotApply = ToString(GetNameList(idListNotApply, idNameList.ToDictionary(kvp => kvp.Id, kvp => kvp.Name)));
            }
            else if (idList != allValue)
            {
                nameList = ToString(GetNameList(idList, idNameList.ToDictionary(kvp => kvp.Id, kvp => kvp.Name)));
            }
            else
            {
                nameList = Labels.All;
            }

            return new Tuple<string, string>(nameList, nameListNotApply);
        }

        private static string BuildAdjustmentCell(VREngineRule rule)
        {
            var adjustment = rule.AdjustmentValue.ToString("N0");
            switch (rule.AdjustmentType)
            {
                case AdjustmentConst.FixedValue:
                    return $"{adjustment}%{BuildAccumulatedBalance(rule)}";
                case AdjustmentConst.AdjustCurrentVirtualRate:
                    return $"Member VR {adjustment}%";
                default:
                    return "-";
            }
        }

        private static string BuildAccumulatedBalance(VREngineRule rule)
        {
            return rule.AccumulatedBalance == 1 ? $"</br>Apply {VREngineResource.VRAccumulatedBalance}" : "";
        }

        #endregion

        public Result AddNewRule(AddNewRuleViewModel viewModel)
        {
            var jsonSportBetType = GetSportBettypeMatrix();
            var criteria = new CheckVREngineRuleCriteria(viewModel, jsonSportBetType);
            DynamicRepository.Execute(criteria);
            var resultCheck = new Result(criteria.ErrorCode, criteria.ErrorMsg);
            if (!resultCheck.IsSuccess)
            {
                return resultCheck;
            }

            return ExecuteNonQuery(
                   new AddNewVREngineRuleCriteria(viewModel, resultCheck.Message),
                   VREngineResource.AddedNewRuleSuccessfully);
        }

        public Result UpdateRule(int ruleId, AddNewRuleViewModel viewModel)
        {

            var jsonSportBetType = GetSportBettypeMatrix();
            var criteria = new CheckVREngineRuleCriteria(viewModel, jsonSportBetType);
            var checkResults = DynamicRepository.Fetch<CheckVREngineRuleResult>(criteria);
            if (criteria.ErrorCode != 0)
            {
                return new Result(criteria.ErrorCode, criteria.ErrorMsg);
            }

            return ExecuteNonQuery(
                new UpdateVREngineRuleCriteria(ruleId, viewModel, JsonConvert.SerializeObject(checkResults)),
                VREngineResource.UpdatedRuleSuccessfully);
        }

        public Result DeleteRule(int ruleId, int userId, string ip)
            => ExecuteNonQuery(
                new DeleteVREngineRuleCriteria(ruleId, userId, ip),
                VREngineResource.DeletedRuleSuccessfully);

        #region Helper methods

        private static IEnumerable<string> GetNameList(string idList, IDictionary<int, string> idNameHash)
        {
            var idArray = idList.ToArray();

            foreach (var item in idArray)
            {
                if (idNameHash.TryGetValue(int.Parse(item), out var name))
                {
                    yield return name;
                }
            }
        }

        private static string ToString(IEnumerable<string> values)
            => string.Join(", ", values);

        private Result ExecuteNonQuery(VREngineCriteriaBase criteria, string successfullyMessage)
        {
            DynamicRepository.Execute(criteria);
            var result = new Result(criteria.ErrorCode, criteria.ErrorMsg);

            if (result.IsSuccess)
            {
                result.Message = successfullyMessage;
            }

            return result;
        }

        private string GetSportBettypeMatrix()
        {
            var jsonSBM = CacheManager.Get<string>(SportBetTypeMatrixCachedKey);
            if(jsonSBM == null)
            {
                var sportBetType = DynamicRepository.Fetch<SportTypeDTO>(new GetCustomerVRGetSportBettypeCriteria())
                .Select(spt => new { spt.Sporttype });
                jsonSBM = JsonConvert.SerializeObject(sportBetType);

                CacheManager.Set(SportBetTypeMatrixCachedKey, jsonSBM, DateTime.Now.AddMinutes(CachedDefaultInMinutes));
            }

            return jsonSBM;
        }

        #endregion
    }
}
