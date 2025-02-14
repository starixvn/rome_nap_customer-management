using System;
using System.Collections.Generic;
using System.Data;
using Fanex.Data;
using Fanex.Data.Repository;
using Fanex.Nap.CMS.Infrastructure.Criteria;
using Fanex.Nap.CMS.Infrastructure.Models;
using Fanex.Nap.CMS.Infrastructure.Web;
using Fanex.Nap.CMS.TurnoverControl.Models.TurnoverMonitor;

namespace Fanex.Nap.CMS.TurnoverControl.Repositories.TurnoverMonitor
{
    public class TurnoverMonitorRepository : ITurnoverMonitorRepository
    {
        public IEnumerable<IdNameModel> GetSports(CriteriaBase criteria)
        {
            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria))
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

        public IEnumerable<IdNameModel> GetLeagues(CriteriaBase criteria)
        {
            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria))
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

        public IEnumerable<IdNameModel> GetMatches(CriteriaBase criteria)
        {
            if (!criteria.IsValid())
            {
                yield break;
            }

            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria))
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

        public IEnumerable<TurnoverMonitorItem> GetCustomerTurnovers(CriteriaBase criteria)
        {
            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                return db.Query<TurnoverMonitorItem>(criteria);
            }
        }

        public IEnumerable<HouseTurnover> GetHouseTurnovers(CriteriaBase criteria)
        {
            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                return db.Query<HouseTurnover>(criteria);
            }
        }

        public Result ExecuteNonQuery(CommandBaseCriteria criteria)
        {
            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(criteria);

                return new Result(
                    criteria.ErrCode,
                    criteria.ErrMsg);
            }
        }
    }
}
