namespace Fanex.Nap.CMS.Customers.Services.VirtualRateEngine
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Fanex.Common.Extensions;
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
        private const string TimeBeforeKO = "TimeBeforeKO";
        private const string SportBetTypeMatrixCachedKey = "SportBetTypeMatrixCachedKey";
        private const int CachedDefaultInMinutes = 60;

        public VREngineMatrix GetVREngineMatrix()
        {
            var result = DynamicRepository.FetchDataSet(new GetVREngineMatrixCriteria());
            var sites = ToSite(result.Tables[0].Rows);
            var creditSites = sites.Where(s => s.SiteType == SiteTypeConst.Credit).Select(s => new IdNameModel { Id =  s.SiteId, Name = s.Site });
            var licenseeSites = sites.Where(s => s.SiteType == SiteTypeConst.Licensee).Select(s => new IdNameModel { Id = s.SiteId, Name = s.Site });
            var betFromList = ToBetFrom(result.Tables[1].Rows);
            var sports = ToSportType(result.Tables[2].Rows);
            var betTypes = ToBetType(result.Tables[3].Rows);
            var leagueGroupList = ToLeagueGroup(result.Tables[4].Rows).OrderBy(lg => lg.Name);
            var currenciesList = ToCurrency(result.Tables[5].Rows);
            var detailSports = ToSportDetail(result.Tables[6].Rows);

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
            var rule = DynamicRepository.Get<VREngineRule>(new GetVREngineRuleCriteria{ RuleId = ruleId});

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
            rule.EnabledTimeBeforeKO = enabledCriteria.ContainsKey(TimeBeforeKO);
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
                case AdjustmentConst.AdjustExtraVirtualRate:
                    return $"Extra {adjustment}%{BuildAccumulatedBalance(rule)}";
                case AdjustmentConst.AdjustCurrentVirtualRate:
                    return $"Member VR {adjustment}%";
                default:
                    return "-";
            }
        }

        private static string BuildAccumulatedBalance(VREngineRule rule)
        {
            return rule.AccumulatedBalance? $"</br>Apply {VREngineResource.VRAccumulatedBalance}" : "";
        }

        #endregion

        public Result AddNewRule(AddNewRuleViewModel viewModel)
        {
            var jsonSportBetType = GetSportBettypeMatrix();
            var criteria = new CheckVREngineRuleCriteria
            {
                CustomerClassList = viewModel.CustomerClass,
                CustomerClassDetailList = viewModel.CustomerClassDetail,
                CCSportTypeList = viewModel.CCSportType,
                SporttypeList = viewModel.SportType,
                BettypeList = viewModel.BetType,
                LeagueGroupList = viewModel.LeagueGroup,
                LeagueList = viewModel.League,
                DateOfWeekList = viewModel.DayOfWeek,
                CurrencyIDList = viewModel.CurrencyIDList,
                Criteria_NotApply = viewModel.CriteriaNotApply,
                SiteIDList = viewModel.Site,
                JsonSportList = jsonSportBetType
            };
            var jsonCheckData = DynamicRepository.Fetch<CheckVREngineRuleResult>(criteria);

            var resultCheck = new Result(criteria.ErrorCode, criteria.ErrorMsg);
            if (!resultCheck.IsSuccess)
            {
                return resultCheck;
            }

            var addNewCriteria = new AddNewVREngineRuleCriteria
            {
                RuleName = viewModel.RuleTitle,
                Priority = viewModel.PriorityOrder,
                SiteType = viewModel.SiteType,
                SiteIDList = viewModel.Site,
                CustomerClassList = viewModel.CustomerClass,
                CustomerClassDetailList = viewModel.CustomerClassDetail,
                CCSportTypeList = viewModel.CCSportType,
                VR_From = viewModel.VRFrom,
                VR_To = viewModel.VRTo,
                VRStatus = 2,
                SporttypeList = viewModel.SportType,
                BettypeList = viewModel.BetType,
                LiveIndicator = viewModel.LiveIndicator,
                LeagueGroupList = viewModel.LeagueGroup,
                LeagueList = viewModel.League,
                Betfrom = viewModel.Betfrom,
                StakeRM_From = viewModel.StakeFrom,
                StakeRM_To = viewModel.StakeTo,
                DateOfWeekList = viewModel.DayOfWeek,
                BeginTime = viewModel.BeginTime,
                EndTime = viewModel.EndTime,
                CurrencyIDList = viewModel.CurrencyIDList,
                Adjustmenttype = viewModel.AdjustmentType,
                AdjustmentValue = viewModel.AdjustmentValue,
                AccumulatedBalance = viewModel.AccumulatedBalance,
                Status = viewModel.Active,
                Criteria_NotApply = viewModel.CriteriaNotApply,
                Enabled_Columns = viewModel.EnabledColumns,
                JsonCheckData = JsonConvert.SerializeObject(jsonCheckData),
                UserId = viewModel.UserId,
                IP = viewModel.IP
            };

            return ExecuteNonQuery(addNewCriteria, VREngineResource.AddedNewRuleSuccessfully);
        }

        public Result UpdateRule(int ruleId, AddNewRuleViewModel viewModel)
        {

            var jsonSportBetType = GetSportBettypeMatrix();
            var criteria = new CheckVREngineRuleCriteria
            {
                CustomerClassList = viewModel.CustomerClass,
                CustomerClassDetailList = viewModel.CustomerClassDetail,
                CCSportTypeList = viewModel.CCSportType,
                SporttypeList = viewModel.SportType,
                BettypeList = viewModel.BetType,
                LeagueGroupList = viewModel.LeagueGroup,
                LeagueList = viewModel.League,
                DateOfWeekList = viewModel.DayOfWeek,
                CurrencyIDList = viewModel.CurrencyIDList,
                Criteria_NotApply = viewModel.CriteriaNotApply,
                SiteIDList = viewModel.Site,
                JsonSportList = jsonSportBetType
            };
            var checkResults = DynamicRepository.Fetch<CheckVREngineRuleResult>(criteria);
            if (criteria.ErrorCode != 0)
            {
                return new Result(criteria.ErrorCode, criteria.ErrorMsg);
            }

            var editCriteria = new UpdateVREngineRuleCriteria
            {
                RuleId = ruleId,
                RuleName = viewModel.RuleTitle,
                Priority = viewModel.PriorityOrder,
                SiteType = viewModel.SiteType,
                SiteIDList = viewModel.Site,
                CustomerClassList = viewModel.CustomerClass,
                CustomerClassDetailList = viewModel.CustomerClassDetail,
                CCSportTypeList = viewModel.CCSportType,
                VR_From = viewModel.VRFrom,
                VR_To = viewModel.VRTo,
                VRStatus = 2,
                SporttypeList = viewModel.SportType,
                BettypeList = viewModel.BetType,
                LiveIndicator = viewModel.LiveIndicator,
                LeagueGroupList = viewModel.LeagueGroup,
                LeagueList = viewModel.League,
                Betfrom = viewModel.Betfrom,
                StakeRM_From = viewModel.StakeFrom,
                StakeRM_To = viewModel.StakeTo,
                DateOfWeekList = viewModel.DayOfWeek,
                BeginTime = viewModel.BeginTime,
                EndTime = viewModel.EndTime,
                CurrencyIDList = viewModel.CurrencyIDList,
                Adjustmenttype = viewModel.AdjustmentType,
                AdjustmentValue = viewModel.AdjustmentValue,
                AccumulatedBalance = viewModel.AccumulatedBalance,
                Status = viewModel.Active,
                Criteria_NotApply = viewModel.CriteriaNotApply,
                Enabled_Columns = viewModel.EnabledColumns,
                JsonCheckData = JsonConvert.SerializeObject(checkResults),
                UserId = viewModel.UserId,
                IP = viewModel.IP
            };

            return ExecuteNonQuery(editCriteria, VREngineResource.UpdatedRuleSuccessfully);
        }

        public Result DeleteRule(int ruleId, int userId, string ip)
        {
            var criteria = new DeleteVREngineRuleCriteria
            {
                RuleId = ruleId,
                UserId = userId,
                IP = ip
            };

            return ExecuteNonQuery(criteria, VREngineResource.DeletedRuleSuccessfully);
        }

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

        private static IEnumerable<VREngineSiteDTO> ToSite(DataRowCollection dataRows)
        {
            var result = new List<VREngineSiteDTO>();
            foreach (DataRow dataRow in dataRows)
            {
                result.Add(new VREngineSiteDTO
                {
                    SiteId = Convert.ToInt32(dataRow["SiteId"]),
                    Site = dataRow["Site"].ToString(),
                    SiteType = Convert.ToInt32(dataRow["SiteType"])
                });
            }

            return result;
        }
        private static IEnumerable<VREngineBetfrom> ToBetFrom(DataRowCollection dataRows)
        {
            var result = new List<VREngineBetfrom>();
            foreach (DataRow dataRow in dataRows)
            {
                result.Add(new VREngineBetfrom(dataRow["betfrom"].ToString(), dataRow["name"].ToString()));
            }

            return result;
        }
        private static IEnumerable<IdNameModel> ToSportType(DataRowCollection dataRows)
        {
            var result = new List<IdNameModel>();
            foreach (DataRow dataRow in dataRows)
            {
                var sportType = Convert.ToInt32(dataRow["sporttype"]);
                result.Add(new IdNameModel
                {
                    Id = sportType,    
                    Name = CacheRef.GetSportName(sportType)
                });
            }

            return result;
        }
        private static IEnumerable<IdNameModel> ToBetType(DataRowCollection dataRows)
        {
            var result = new List<IdNameModel>();
            foreach (DataRow dataRow in dataRows)
            {
                var betType = Convert.ToInt32(dataRow["bettype"]);
                result.Add(new IdNameModel
                {
                    Id = betType,
                    Name = CacheRef.GetBetType(betType)
                });
            }

            return result;
        }
        private static IEnumerable<IdNameModel> ToLeagueGroup(DataRowCollection dataRows)
        {
            var result = new List<IdNameModel>();
            foreach (DataRow dataRow in dataRows)
            {
                result.Add(new IdNameModel
                {
                    Id = Convert.ToInt32(dataRow["leaguegroupid"]),
                    Name = dataRow["groupname"].ToString()
                });
            }

            return result;
        }
        private static IEnumerable<IdNameModel> ToCurrency(DataRowCollection dataRows)
        {
            var result = new List<IdNameModel>();
            foreach (DataRow dataRow in dataRows)
            {
                result.Add(new IdNameModel
                {
                    Id = Convert.ToInt32(dataRow["currencyid"]),
                    Name = dataRow["currencyname"].ToString()
                });
            }

            return result;
        }
        private static IEnumerable<IdNameModel> ToSportDetail(DataRowCollection dataRows)
        {
            var result = new List<IdNameModel>();
            foreach (DataRow dataRow in dataRows)
            {
                var sportType = Convert.ToInt32(dataRow["sporttype"]);

                result.Add(new IdNameModel
                {
                    Id = sportType,
                    Name = dataRow["sportname"].ToString()
                });
            }

            return result;
        }

        #endregion
    }
}
