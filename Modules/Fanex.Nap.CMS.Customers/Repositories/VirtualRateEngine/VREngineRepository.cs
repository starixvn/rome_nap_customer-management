using System;
using System.Collections.Generic;
using System.Data;
using Fanex.Data;
using Fanex.Data.Repository;
using Fanex.Nap.CMS.Customers.Constants.ProblemAccount;
using Fanex.Nap.CMS.Customers.Models.VirtualRateEngine;
using Fanex.Nap.CMS.Infrastructure.Models;
using Fanex.Nap.CMS.Infrastructure.Web;

namespace Fanex.Nap.CMS.Customers.Repositories.VirtualRateEngine
{
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
                    var customerClasses = GetIdNameModels(reader, ToCustomerClass);

                    reader.NextResult();
                    var betfromList = GetBetfromList(reader);

                    reader.NextResult();
                    var sports = GetIdNameModels(reader, ToSport);

                    reader.NextResult();
                    var betTypes = GetIdNameModels(reader, ToBetType);

                    reader.NextResult();
                    var leagueGroups = GetIdNameModels(reader, ToLeagueGroup);

                    return new VREngineMatrix(
                        sites.Item1,
                        sites.Item2,
                        customerClasses,
                        betfromList,
                        sports,
                        betTypes,
                        leagueGroups);
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
                    reader["betfrom"].ToString()[0],
                    reader["name"].ToString()));
            }

            return betfromList;
        }

        private static IdNameModel ToCustomerClass(IDataReader reader)
            => new IdNameModel
            {
                Id = Convert.ToInt32(reader["customerclass"]),
                Name = reader["name"].ToString()
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
    }
}
