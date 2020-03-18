using System;
using System.Collections.Generic;
using System.Data;
using Fanex.Data;
using Fanex.Nap.CMS.Infrastructure.Models;
using Fanex.Nap.CMS.Infrastructure.Web;
using Fanex.Nap.CMS.TurnoverControl.Models.TurnoverMonitor;
using Fanex.NAP.Framework.Data;

namespace Fanex.Nap.CMS.TurnoverControl.Repositories.TurnoverMonitor
{
    public class TurnoverMonitorRepository : ITurnoverMonitorRepository
    {
        public IEnumerable<IdNameModel> GetSports(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        yield return GetSport(reader);
                    }
                }
            }
        }

        private static IdNameModel GetSport(IDataReader reader)
            => new IdNameModel
            {
                Id = Convert.ToInt32(reader["sporttype"]),
                Name = reader["sportname"].ToString()
            };

        public IEnumerable<IdNameModel> GetLeagues(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        yield return GetLeague(reader);
                    }
                }
            }
        }

        private static IdNameModel GetLeague(IDataReader reader)
        {
            var leagueId = Convert.ToInt32(reader["leagueid"]);

            return new IdNameModel
            {
                Id = leagueId,
                Name = CacheRef.GetLeagueName(leagueId.ToString())
            };
        }

        public IEnumerable<IdNameModel> GetMatches(ICriteria criteria)
        {
            if (!criteria.IsValid())
            {
                yield break;
            }

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        yield return GetMatch(reader);
                    }
                }
            }
        }

        private static IdNameModel GetMatch(IDataReader reader)
            => new IdNameModel
            {
                Id = Convert.ToInt32(reader["matchid"]),
                Name = $"{CacheRef.GetTeamName(reader["homeid"])} -vs- {CacheRef.GetTeamName(reader["awayid"])}"
            };

        public IEnumerable<TurnoverMonitorItem> GetCustomerTurnovers(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                return db.Query<TurnoverMonitorItem>(criteria.GetSpParams());
            }
        }

        public IEnumerable<HouseTurnover> GetHouseTurnovers(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                return db.Query<HouseTurnover>(criteria.GetSpParams());
            }
        }

        public Result ExecuteNonQuery(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(criteria.GetSpParams());
                var outputParams = db.GetOuputParams();

                return new Result(
                    Convert.ToInt32(outputParams["errcode"]),
                    outputParams["errmsg"].ToString());
            }
        }
    }
}
