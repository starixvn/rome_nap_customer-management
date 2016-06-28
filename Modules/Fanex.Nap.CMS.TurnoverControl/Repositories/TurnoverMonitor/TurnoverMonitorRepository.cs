// --------------------------------------------------------------------------
// <copyright file="TurnoverMonitorRepository.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Nap.CMS.TurnoverControl.Repositories.TurnoverMonitor
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.Nap.CMS.Infrastructure.Extensions;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Infrastructure.Repositories;
    using Fanex.Nap.CMS.TurnoverControl.Criteria.TurnoverMonitor;
    using Fanex.Nap.CMS.TurnoverControl.Models.TurnoverMonitor;

    /// <summary>
    /// Turnover Monitor Repository.
    /// </summary>
    public class TurnoverMonitorRepository : BaseRepository, ITurnoverMonitorRepository
    {
        /// <summary>
        /// Get sport list.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Sport list.</returns>
        public List<IdNameModel> Sports(GetSportsCriteria criteria)
        {
            List<IdNameModel> result = new List<IdNameModel>();
            using (var objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objDb.AssignParamValues(criteria.GetSpParams());
                var reader = objDb.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(new IdNameModel()
                    {
                        Id = Converter.ToInt(reader["sporttype"]),
                        Name = Converter.ToString2(reader["sportname"])
                    });
                }
            }

            return result;
        }

        /// <summary>
        /// Get league list.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>League list.</returns>
        public List<IdNameModel> Leagues(GetLeaguesCriteria criteria)
        {
            List<IdNameModel> result = new List<IdNameModel>();
            using (var objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objDb.AssignParamValues(criteria.GetSpParams());
                var reader = objDb.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(new IdNameModel()
                    {
                        Id = Converter.ToInt(reader["leagueid"]),
                        Name = Converter.ToString2(reader["leaguename"])
                    });
                }
            }

            return result;
        }

        /// <summary>
        /// Get SMA turnover list.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>SMA turnover list.</returns>
        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public List<CustomerTurnoverInfo> SMATurnoverList(GetSMATurnoverListCriteria criteria)
        {
            List<CustomerTurnoverInfo> result = new List<CustomerTurnoverInfo>();
            using (var objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objDb.AssignParamValues(criteria.GetSpParams());
                var reader = objDb.ExecuteReader();
                while (reader.Read())
                {
                    var turnoverInfo = new CustomerTurnoverInfo
                    {
                        Id = Converter.ToInt(reader["custid"]),
                        Name = Converter.ToString2(reader["username"]),
                        RoleInfo = new IdNameModel
                        {
                            Name = Converter.ToString2(reader["level"])
                        },
                        Currency = new IdNameModel
                        {
                            Name = Converter.ToString2(reader["currency"])
                        },
                        League = new IdNameModel
                        {
                            Id = Converter.ToInt(reader["leagueid"]),
                            Name = Converter.ToString2(reader["leaguename"])
                        },
                        MatchInfo = new MatchInfo
                        {
                            Id = Converter.ToInt(reader["matchid"]),
                            HomeTeam = new IdNameModel
                            {
                                Id = Converter.ToInt(reader["homeid"])
                            },
                            AwayTeam = new IdNameModel
                            {
                                Id = Converter.ToInt(reader["awayid"])
                            },
                            EventDate = Converter.ToDateTime(reader["eventdate"]),
                            EventStatus = Converter.ToString2(reader["eventstatus"])
                        },
                        MaxTurnover = Converter.ToDecimal(reader["maxturnover"]),
                        MemberTurnover = Converter.ToDecimal(reader["totalturnover"]),
                        CompanyTurnover = Converter.ToDecimal(reader["houseturnover"]),
                        PercentageUsed = Converter.ToDecimal(reader["percentageused"])
                    };

                    turnoverInfo.MatchInfo.EventDateString = turnoverInfo.MatchInfo.EventDate.ToShortFormatedDateWithSlash();
                    result.Add(turnoverInfo);
                }
            }

            return result;
        }
    }
}
