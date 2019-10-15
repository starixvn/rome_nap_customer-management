namespace Fanex.Nap.CMS.Customers.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Criteria.PossessStatistics;
    using Data;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Models.PossessStatistics;

    public interface PossessStatisticsRepository
    {
        IEnumerable<PossessStatisticsModel> PossessStatistics(GetPossessStatisticsCriteria criteria);

        Result UpdateDangerLevel(UpdateDangerLevelCritera criteria);

        Result UpdateMonitorStatus(UpdateMonitorStatusCriteria criteria);
    }

    public class PossessStatisticsRepositoryImpl : PossessStatisticsRepository
    {
        public IEnumerable<PossessStatisticsModel> PossessStatistics(GetPossessStatisticsCriteria criteria)
        {
            var possessStatistics = new List<PossessStatisticsModel>();

            if (criteria.IsValid())
            {
                using (var db = new ObjectDb(criteria.GetSettingKey()))
                {
                    using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                    {
                        while (reader.Read())
                        {
                            possessStatistics.Add(ToPossessStatisticsModel(reader));
                        }
                    }

                    criteria.TotalItems = Convert.ToInt32(db.GetOuputParams()["totalitems"]);
                }
            }

            return possessStatistics;
        }

        public Result UpdateDangerLevel(UpdateDangerLevelCritera criteria)
            => ExecuteNonQuery(criteria);

        public Result UpdateMonitorStatus(UpdateMonitorStatusCriteria criteria)
            => ExecuteNonQuery(criteria);

        private static Result ExecuteNonQuery(NonQueryBaseCriteria criteria)
        {
            if (criteria.IsValid())
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

            return new Result();
        }

        private static PossessStatisticsModel ToPossessStatisticsModel(IDataReader reader)
            => new PossessStatisticsModel(
                Convert.ToInt32(reader["rn"]),
                Convert.ToInt32(reader["custid"]),
                Convert.ToInt32(reader["totalpt_sb"]),
                Convert.ToInt32(reader["totalpt_ns"]),
                reader["super"].ToString(),
                Convert.ToInt32(reader["superpt_sb"]),
                Convert.ToInt32(reader["superpt_ns"]),
                reader["master"].ToString(),
                Convert.ToInt32(reader["masterpt_sb"]),
                Convert.ToInt32(reader["masterpt_ns"]),
                reader["agent"].ToString(),
                Convert.ToInt32(reader["agentpt_sb"]),
                Convert.ToInt32(reader["agentpt_ns"]),
                Convert.ToByte(reader["status"]),
                Convert.ToInt32(reader["danger"]),
                Convert.ToBoolean(reader["ismonitorPT"]));
    }
}