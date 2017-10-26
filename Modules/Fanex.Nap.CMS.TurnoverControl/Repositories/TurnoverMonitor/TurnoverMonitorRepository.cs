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

        public IEnumerable<CustomerTurnover> GetCustomerTurnovers(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                return db.Query<CustomerTurnover>(criteria.GetSpParams());
            }
        }

        public IEnumerable<HouseTurnover> GetHouseTurnovers(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                return db.Query<HouseTurnover>(criteria.GetSpParams());
            }
        }

        public Result UpdateMaxTurnover(ICriteria criteria)
        {
            return ExecuteNonQuery(criteria);
        }

        public Result UpdateMultipleMaxTurnover(ICriteria criteria)
        {
            return ExecuteNonQuery(criteria);
        }

        private static Result ExecuteNonQuery(ICriteria criteria)
        {
            Result result = new Result();

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(criteria.GetSpParams());
                var outputParams = db.GetOuputParams();

                result.Code = Convert.ToInt32(outputParams["errcode"]);
                result.Message = outputParams["errmsg"].ToString();
            }

            return result;
        }

        private static IdNameModel GetSport(IDataReader reader)
        {
            return new IdNameModel
            {
                Id = Convert.ToInt32(reader["sporttype"]),
                Name = reader["sportname"].ToString()
            };
        }

        private static IdNameModel GetLeague(IDataReader reader)
        {
            int leagueId = Convert.ToInt32(reader["leagueid"]);

            return new IdNameModel
            {
                Id = leagueId,
                Name = CacheRef.GetLeagueName(leagueId.ToString())
            };
        }

        private static IdNameModel GetMatch(IDataReader reader)
        {
            return new IdNameModel
            {
                Id = Convert.ToInt32(reader["matchid"]),
                Name = string.Format(
                    "{0} -vs- {1}",
                    CacheRef.GetTeamName(reader["homeid"]),
                    CacheRef.GetTeamName(reader["awayid"]))
            };
        }
    }
}
