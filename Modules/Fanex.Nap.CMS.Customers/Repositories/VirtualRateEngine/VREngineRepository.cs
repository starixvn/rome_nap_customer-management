namespace Fanex.Nap.CMS.Customers.Repositories.VirtualRateEngine
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Customers.Constants.ProblemAccount;
    using Fanex.Nap.CMS.Customers.Models.VirtualRateEngine;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Infrastructure.Web;
    using Newtonsoft.Json;

    public class VREngineRepository : IVREngineRepository
    {
        public VREngineMatrix GetVREngineMatrix(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    var sites = GetSites(reader);

                    reader.NextResult();
                    var betfromList = GetBetfromList(reader);

                    reader.NextResult();
                    var sports = GetIdNameModels(reader, ToSport);

                    reader.NextResult();
                    var betTypes = GetIdNameModels(reader, ToBetType);

                    reader.NextResult();
                    var leagueGroups = GetIdNameModels(reader, ToLeagueGroup).OrderBy(lg => lg.Name);

                    reader.NextResult();
                    var currencies = GetIdNameModels(reader, ToCurrency);

                    reader.NextResult();
                    var sportCCDetail = GetIdNameModels(reader, ToSportDetail);

                    return new VREngineMatrix(
                        sites.Item1,
                        sites.Item2,
                        sportCCDetail,
                        betfromList,
                        sports,
                        betTypes,
                        leagueGroups,
                        currencies);
                }
            }
        }

        private static Tuple<IEnumerable<IdNameModel>, IEnumerable<IdNameModel>> GetSites(IDataReader reader)
        {
            byte siteType;
            IList<IdNameModel> creditSites = new List<IdNameModel>();
            IList<IdNameModel> licenseeSites = new List<IdNameModel>();

            while (reader.Read())
            {
                siteType = Convert.ToByte(reader["sitetype"]);
                if (siteType == SiteTypeConst.Credit)
                {
                    creditSites.Add(ToSite(reader));
                }
                else
                {
                    licenseeSites.Add(ToSite(reader));
                }
            }

            return new Tuple<IEnumerable<IdNameModel>, IEnumerable<IdNameModel>>(creditSites, licenseeSites);
        }

        private static IdNameModel ToSite(IDataReader reader)
            => new IdNameModel
            {
                Id = Convert.ToInt32(reader["siteid"]),
                Name = reader["site"].ToString()
            };

        private static IEnumerable<VREngineBetfrom> GetBetfromList(IDataReader reader)
        {
            IList<VREngineBetfrom> betfromList = new List<VREngineBetfrom>();

            while (reader.Read())
            {
                betfromList.Add(new VREngineBetfrom(
                    reader["betfrom"].ToString(),
                    reader["name"].ToString()));
            }

            return betfromList;
        }
        
        private static IdNameModel ToSportDetail(IDataReader reader)
            => new IdNameModel
            {
                Id = Convert.ToInt32(reader["sporttype"]),
                Name = reader["sportname"].ToString()
            };

        private static IdNameModel ToSport(IDataReader reader)
        {
            var sportType = Convert.ToInt32(reader["sporttype"]);

            return new IdNameModel
            {
                Id = sportType,
                Name = CacheRef.GetSportName(sportType)
            };
        }

        private static IdNameModel ToBetType(IDataReader reader)
        {
            var betType = Convert.ToInt32(reader["bettype"]);

            return new IdNameModel
            {
                Id = betType,
                Name = CacheRef.GetBetType(betType)
            };
        }

        private static IdNameModel ToLeagueGroup(IDataReader reader)
            => new IdNameModel
            {
                Id = Convert.ToInt32(reader["leaguegroupid"]),
                Name = reader["groupname"].ToString()
            };

        public IEnumerable<IdNameModel> GetLeagues(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    return GetIdNameModels(reader, ToLeague);
                }
            }
        }

        private static IdNameModel ToLeague(IDataReader reader)
            => new IdNameModel
            {
                Id = Convert.ToInt32(reader["leagueid"]),
                Name = reader["leaguename"].ToString()
            };

        private static IdNameModel ToCurrency(IDataReader reader)
            => new IdNameModel
            {
                Id = Convert.ToInt32(reader["currencyid"]),
                Name = reader["currencyname"].ToString()
            };

        private static IEnumerable<IdNameModel> GetIdNameModels(
            IDataReader reader,
            Func<IDataReader, IdNameModel> parser)
        {
            IList<IdNameModel> dataList = new List<IdNameModel>();

            while (reader.Read())
            {
                dataList.Add(parser(reader));
            }

            return dataList;
        }

        public IEnumerable<VREngineRule> GetRules(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        yield return ToRule(reader);
                    }
                }
            }
        }

        public VREngineRule GetRule(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    if (reader.Read())
                    {
                        return ToRule(reader);
                    }
                }
            }

            return null;
        }

        private static VREngineRule ToRule(IDataReader reader)
            => new VREngineRule(
                Convert.ToInt32(reader["ruleid"]),
                reader["rulename"].ToString(),
                Convert.ToInt32(reader["priority"]),
                Convert.ToByte(reader["sitetype"]),
                reader["siteidlist"].ToString(),
                reader["siteidlist_notapply"].ToString(),
                reader["customerclasslist"].ToString(),
                reader["customerclasslist_notapply"].ToString(),
                reader["customerclassdetaillist"].ToString(),
                reader["customerclassdetaillist_notapply"].ToString(),
                reader["ccsporttypelist"].ToString(),
                reader["ccsporttypelist_notapply"].ToString(),
                Convert.ToDecimal(reader["vr_from"]),
                Convert.ToDecimal(reader["vr_to"]),
                Convert.ToByte(reader["vrstatus"]),
                reader["sporttypelist"].ToString(),
                reader["sporttypelist_notapply"].ToString(),
                reader["bettypelist"].ToString(),
                reader["bettypelist_notapply"].ToString(),
                Convert.ToByte(reader["liveindicator"]),
                reader["leaguegrouplist"].ToString(),
                reader["leaguegrouplist_notapply"].ToString(),
                reader["leaguelist"].ToString(),
                reader["leaguelist_notapply"].ToString(),
                reader["betfrom"].ToString(),
                Convert.ToDecimal(reader["stakerm_from"]),
                Convert.ToDecimal(reader["stakerm_to"]),
                reader["dateofweeklist"].ToString(),
                reader["dateofweeklist_notapply"].ToString(),
                reader["begintime"].ToString(),
                reader["endtime"].ToString(),
                reader["currencyidlist"].ToString(),
                reader["currencyidlist_notapply"].ToString(),
                Convert.ToInt32(reader["PreMatchPeriod"]),
                Convert.ToByte(reader["adjustmenttype"]),
                reader["adjustmentvalue"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["adjustmentvalue"]),
                Convert.ToByte(reader["accumulatedbalance"]),
                Convert.ToByte(reader["status"]),
                Convert.ToDateTime(reader["createddts"]),
                reader["createdby"].ToString(),
                reader["enabled_column"].ToString());

        public Result ExecuteNonQuery(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(criteria.GetSpParams());
                var outputParams = db.GetOuputParams();

                return new Result(
                    Convert.ToInt32(outputParams["errorcode"]),
                    outputParams["errormsg"].ToString());
            }
        }

        public string GetSportBettype(ICriteria criteria)
        {
            var sporttypes = new List<object>();
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        sporttypes.Add(new { Sporttype = Convert.ToInt32(reader["sporttype"]) });
                    }
                }
            }

            return JsonConvert.SerializeObject(sporttypes);
        }

        public Result CheckVREngineRule(ICriteria criteria)
        {
            var checkResults = new List<CheckVREngineRuleResult>();
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        checkResults.Add(ToCheckVRRule(reader));
                    }

                    reader.Close();

                    var outputParams = db.GetOuputParams();
                    var result = new Result(
                    Convert.ToInt32(outputParams["errorcode"]),
                    outputParams["errormsg"].ToString());

                    if (result.IsSuccess)
                    {
                        result.Message = JsonConvert.SerializeObject(checkResults);
                    }

                    return result;
                }
            }
        }

        private static CheckVREngineRuleResult ToCheckVRRule(IDataReader reader)
            => new CheckVREngineRuleResult
            {
                Bettype = Convert.ToInt32(reader["Bettype"]),
                Sporttype = Convert.ToInt32(reader["Sporttype"]),
                CCSporttype = Convert.ToInt32(reader["CCSporttype"]),
                CurrencyID = Convert.ToInt32(reader["CurrencyID"]),
                CustomerClass = Convert.ToInt32(reader["CustomerClass"]),
                CustomerClassDetail = Convert.ToInt32(reader["CustomerClassDetail"]),
                DateOfWeek = Convert.ToInt32(reader["DateOfWeek"]),
                LeagueGroupID = Convert.ToInt32(reader["LeagueGroupID"]),
                LeagueID = Convert.ToInt32(reader["LeagueID"]),
                SiteID = Convert.ToInt32(reader["SiteID"]),
                IsApply = Convert.ToByte(reader["IsApply"])
            };
    }
}
