using System;
using System.Collections.Generic;
using System.Linq;
using Fanex.Data.Repository;
using Fanex.Nap.CMS.Customers.Constants.ProblemAccount;
using Fanex.Nap.CMS.Customers.Constants.VirtualRateEngine;
using Fanex.Nap.CMS.Customers.Criteria.VirtualRateEngine;
using Fanex.Nap.CMS.Customers.Models.VirtualRateEngine;
using Fanex.Nap.CMS.Customers.Repositories.VirtualRateEngine;
using Fanex.Nap.CMS.Customers.Resources;
using Fanex.Nap.CMS.Customers.ViewModels.VirtualRateEngine;
using Fanex.Nap.CMS.Infrastructure.Extensions;
using Fanex.Nap.CMS.Infrastructure.Models;

namespace Fanex.Nap.CMS.Customers.Services.VirtualRateEngine
{
    public class VREngineService : IVREngineService
    {
        private const string SiteType = "Site";
        private const string CustomerClass = "CustomerClass";
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
        private readonly IVREngineRepository repository;

        public VREngineService(IVREngineRepository repository = null)
        {
            this.repository = repository ?? new VREngineRepository();
        }

        public VREngineMatrix GetVREngineMatrix()
            => repository.GetVREngineMatrix(new GetVREngineMatrixCriteria());

        public IEnumerable<IdNameModel> GetLeagues(string leagueGroupIds, string leagues)
            => repository.GetLeagues(new GetVREngineLeagueCriteria(leagueGroupIds, leagues));

        public IEnumerable<VREngineRule> GetRules()
        {
            var matrix = GetVREngineMatrix();
            var rules = repository.GetRules(new GetVREngineRuleListCriteria());

            foreach (var rule in rules)
            {
                if (!string.IsNullOrWhiteSpace(rule.EnabledCriteria))
                {
                    BuildCriteriaCell(rule, matrix);
                }

                rule.AdjustmentCell = BuildAdjustmentCell(rule);
                rule.StatusName = ActiveConst.MapToActiveName(rule.Status);
                rule.CreatedDate = rule.CreatedDTS.ToShortFormatedDateWithSlash();

                yield return rule;
            }
        }

        #region Build Criteria methods

        private static void BuildCriteriaCell(VREngineRule rule, VREngineMatrix matrix)
        {
            var enabledCriteria = rule.EnabledCriteria.ToArray().ToDictionary(kvp => kvp, kvp => true);

            BuildSiteTypeCriteria(enabledCriteria, rule, matrix.CreditSites, matrix.LicenseeSites);

            rule.EnabledCustomerClass = enabledCriteria.ContainsKey(CustomerClass);

            rule.EnabledCurrentVR = enabledCriteria.ContainsKey(CurrentVR);

            BuildCurrentVRStatusCriteria(enabledCriteria, rule);

            BuildSportCriteria(enabledCriteria, rule, matrix.Sports);

            BuildBetTypeCriteria(enabledCriteria, rule, matrix.BetTypes);

            BuildEventCriteria(enabledCriteria, rule);

            BuildLeagueGroupCriteria(enabledCriteria, rule, matrix.LeagueGroups);

            BuildBetfromCriteria(enabledCriteria, rule, matrix.BetfromList);

            rule.EnabledStake = enabledCriteria.ContainsKey(Stake);

            BuildTicketDoWCriteria(enabledCriteria, rule);

            rule.EnabledTicketTime = enabledCriteria.ContainsKey(TicketTime);
        }

        private static void BuildSiteTypeCriteria(
            IDictionary<string, bool> enabledCriteria,
            VREngineRule rule,
            IEnumerable<IdNameModel> creditSites,
            IEnumerable<IdNameModel> licenseeSites)
        {
            rule.EnabledSiteType = enabledCriteria.ContainsKey(SiteType);

            if (rule.EnabledSiteType)
            {
                IEnumerable<IdNameModel> sites = Enumerable.Empty<IdNameModel>();

                var hasSiteIdList = rule.SiteIdList != "0";
                var hasSiteIdListNotApply = rule.SiteIdListNotIn != "0";

                if (rule.SiteType == SiteTypeConst.Credit)
                {
                    rule.SiteTypeName = nameof(SiteTypeConst.Credit);

                    if (hasSiteIdList || hasSiteIdListNotApply)
                    {
                        sites = creditSites;
                    }
                }
                else if (rule.SiteType == SiteTypeConst.Licensee)
                {
                    rule.SiteTypeName = nameof(SiteTypeConst.Licensee);

                    if (hasSiteIdList || hasSiteIdListNotApply)
                    {
                        sites = licenseeSites;
                    }
                }
                else
                {
                    rule.SiteTypeName = Labels.All;

                    if (hasSiteIdList || hasSiteIdListNotApply)
                    {
                        sites = creditSites.Concat(licenseeSites);
                    }
                }

                var nameTuple = BuildMultipleSelectionCriteria(rule.SiteIdList, rule.SiteIdListNotIn, sites);
                rule.SiteNameList = nameTuple.Item1;
                rule.SiteNameListNotIn = nameTuple.Item2;
            }
        }

        private static void BuildCurrentVRStatusCriteria(
            IDictionary<string, bool> enabledCriteria,
            VREngineRule rule)
        {
            rule.EnabledCurrentVRStatus = enabledCriteria.ContainsKey(CurrentVRStatus);
            rule.VRStatusName = rule.VRStatus == CurrentVRStatusConst.Enable
                ? VREngineResource.Enabled
                : VREngineResource.Disabled;
        }

        private static void BuildSportCriteria(
            IDictionary<string, bool> enabledCriteria,
            VREngineRule rule,
            IEnumerable<IdNameModel> sports)
        {
            rule.EnabledSport = enabledCriteria.ContainsKey(Sport);

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

        private static void BuildBetTypeCriteria(
            IDictionary<string, bool> enabledCriteria,
            VREngineRule rule,
            IEnumerable<IdNameModel> betTypes)
        {
            rule.EnabledBetType = enabledCriteria.ContainsKey(BetType);

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

        private static void BuildEventCriteria(
            IDictionary<string, bool> enabledCriteria,
            VREngineRule rule)
        {
            rule.EnabledEvent = enabledCriteria.ContainsKey(Event);
            rule.LiveIndicatorName = rule.LiveIndicator == EventConst.Live
                ? nameof(EventConst.Live)
                : VREngineResource.NonLive;
        }

        private static void BuildLeagueGroupCriteria(
            IDictionary<string, bool> enabledCriteria,
            VREngineRule rule,
            IEnumerable<IdNameModel> leagueGroups)
        {
            rule.EnabledLeagueGroup = enabledCriteria.ContainsKey(LeagueGroup);

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

        private static void BuildBetfromCriteria(
            IDictionary<string, bool> enabledCriteria,
            VREngineRule rule,
            IEnumerable<VREngineBetfrom> betfromList)
        {
            rule.EnabledBetfrom = enabledCriteria.ContainsKey(Betfrom);
            rule.BetfromName = betfromList.FirstOrDefault(b => b.Id == rule.Betfrom)?.Name;
        }

        private static void BuildTicketDoWCriteria(
            IDictionary<string, bool> enabledCriteria,
            VREngineRule rule)
        {
            rule.EnabledTicketDoW = enabledCriteria.ContainsKey(TicketDoW);

            if (rule.EnabledTicketDoW)
            {
                var nameTuple = BuildMultipleSelectionCriteria(
                    rule.DateOfWeekList,
                    rule.DateOfWeekListNotIn,
                    VREngineDayOfWeek.DayOfWeeks);

                rule.DateOfWeekNameList = nameTuple.Item1;
                rule.DateOfWeekNameListNotIn = nameTuple.Item2;
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

            if (!string.IsNullOrWhiteSpace(idListNotApply) && idListNotApply != allValue)
            {
                nameListNotApply = ToString(GetNameList(idListNotApply, idNameList.ToDictionary(kvp => kvp.Id, kvp => kvp.Name)));
            }
            else if (idList != allValue)
            {
                nameList = ToString(GetNameList(idList, idNameList.ToDictionary(kvp => kvp.Id, kvp => kvp.Name)));
            }
            else
            {
                nameList= Labels.All;
            }

            return new Tuple<string, string>(nameList, nameListNotApply);
        }

        private static string BuildAdjustmentCell(VREngineRule rule)
        {
            var adjustment = rule.AdjustmentValue.ToString("N0");

            return rule.AdjustmentType == AdjustmentConst.FixedValue
                ? $"{adjustment}%"
                : $"Member VR {adjustment}%";
        }

        #endregion

        public Result AddNewRule(AddNewRuleViewModel viewModel)
            => ExecuteNonQuery(
                new AddNewVREngineRuleCriteria(viewModel),
                VREngineResource.AddedNewRuleSuccessfully);

        public Result UpdateRule(int ruleId, AddNewRuleViewModel viewModel)
            => ExecuteNonQuery(
                new UpdateVREngineRuleCriteria(ruleId, viewModel),
                VREngineResource.UpdatedRuleSuccessfully);

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

        private Result ExecuteNonQuery(ICriteria criteria, string successfullyMessage)
        {
            var result = repository.ExecuteNonQuery(criteria);

            if (result.IsSuccess)
            {
                result.Message = successfullyMessage;
            }

            return result;
        }

        #endregion
    }
}
