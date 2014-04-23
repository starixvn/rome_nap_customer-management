// --------------------------------------------------------------------------
// <copyright file="ConfigureTOByEventService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.4.16">Created</change>
// </history>

namespace Fanex.NAP.CMS.Services.Implements
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.NAP.CMS.Criteria.ConfigureTOByEvent;
    using Fanex.NAP.CMS.Models.ConfigureTOByEvent;
    using Fanex.NAP.CMS.Services.Interfaces;
    using Fanex.NAP.CMS.ViewModels.ConfigureTOByEvent;
    using Fanex.NAP.Framework.Command;

    /// <summary>
    /// Configure turnover by event service.
    /// </summary>
    public class ConfigureTOByEventService : CMSBaseService, IConfigureTOByEventService
    {
        private const int SportMode = 1;
        private const int LeagueMode = 2;
        private const int MatchMode = 3;

        /// <summary>
        /// Gets the sport turnover.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>List of SportTurnover objects.</returns>
        public IEnumerable<SportTurnover> GetSportTurnover(GetEventTurnoverCriteria criteria)
        {
            IEnumerable<SportTurnoverInfo> rawData = null;
            IEnumerable<SportTurnover> result = null;

            criteria.ViewMode = SportMode;
            rawData = this.GetData<SportTurnoverInfo>(criteria);

            if (rawData != null && rawData.Any())
            {
                result = rawData.Select(obj => new SportTurnover(obj));
            }

            return result;
        }

        /// <summary>
        /// Gets the league turnover.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>GotLeagueTurnoverResult object.</returns>
        public GotLeagueTurnoverResult GetLeagueTurnover(GetEventTurnoverCriteria criteria)
        {
            IEnumerable<LeagueTurnoverInfo> rawData = null;
            IEnumerable<LeagueTurnover> turnovers = null;
            LeagueTurnoverInfo firstItem = null;
            GotLeagueTurnoverResult result = null;

            criteria.ViewMode = LeagueMode;
            rawData = this.GetData<LeagueTurnoverInfo>(criteria);

            if (rawData != null && rawData.Any())
            {
                turnovers = rawData.Select(obj => new LeagueTurnover(obj)).OrderBy(obj => obj.LeagueName).ToList();
                firstItem = rawData.ElementAt<LeagueTurnoverInfo>(0);
                result = new GotLeagueTurnoverResult(firstItem.SportID, firstItem.SportValue, turnovers);
            }

            return result;
        }

        /// <summary>
        /// Gets the match turnover.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>GotMatchTurnoverResult object.</returns>
        public GotMatchTurnoverResult GetMatchTurnover(GetEventTurnoverCriteria criteria)
        {
            IEnumerable<MatchTurnoverInfo> rawData = null;
            IEnumerable<MatchTurnover> turnovers = null;
            MatchTurnoverInfo firstItem = null;
            GotMatchTurnoverResult result = null;

            criteria.ViewMode = MatchMode;
            rawData = this.GetData<MatchTurnoverInfo>(criteria);

            if (rawData != null && rawData.Any())
            {
                turnovers = rawData.Select(obj => new MatchTurnover(obj)).OrderBy(obj => obj.MatchName).ToList();
                firstItem = rawData.ElementAt<MatchTurnoverInfo>(0);
                result = new GotMatchTurnoverResult(firstItem.SportID, firstItem.SportValue, firstItem.LeagueID, firstItem.LeagueValue, turnovers);
            }
            else
            {
                // Get league information to generate the breadcrumd in the case the league has no match.
                LeagueTurnoverInfo leagueInfo = null;

                // Set the ViewMode to LeagueMode and OnlyShowException to false to get the information of current and its sport.
                criteria.ViewMode = LeagueMode;
                criteria.OnlyShowException = false;

                leagueInfo = this.GetItem<LeagueTurnoverInfo>(criteria);
                result = new GotMatchTurnoverResult(leagueInfo.SportID, leagueInfo.SportValue, leagueInfo.LeagueID, leagueInfo.LeagueValue, turnovers);
            }

            return result;
        }

        /// <summary>
        /// Updates sport maximum turnover and get the list of sport turnovers after updating.
        /// </summary>
        /// <param name="filter">The filter that is used to update and query data.</param>
        /// <returns>UpdatedTurnoverResult objects.</returns>
        public UpdatedTurnoverResult UpdateSportTurnover(UpdateSportTurnoverFilter filter)
        {
            var updatingCriteria = new UpdateSportTurnoverCriteria
            {
                SportID = filter.SportID,
                Value = filter.Value,
                UserID = filter.UserID,
                IP = filter.IP
            };
            ICommandResult updatingResult = this.UpdateData(updatingCriteria);
            IEnumerable<SportTurnover> turnovers = null;

            // Get up-to-date data after updating.
            if (updatingResult.Code == 0)
            {
                turnovers = GetSportTurnover(new GetEventTurnoverCriteria());
            }

            return new UpdatedTurnoverResult
            {
                ErrorCode = updatingResult.Code,
                ErrorMessage = updatingResult.Message,
                Turnovers = turnovers
            };
        }

        /// <summary>
        /// Updates league maximum turnover and get the list of league turnovers after updating.
        /// </summary>
        /// <param name="filter">The filter that is used to update and query data.</param>
        /// <returns>UpdatedTurnoverResult objects.</returns>
        public UpdatedTurnoverResult UpdateLeagueTurnover(UpdateLeagueTurnoverFilter filter)
        {
            var updatingCriteria = new UpdateLeagueTurnoverCriteria
            {
                LeagueID = filter.LeagueID,
                Value = filter.Value,
                UserID = filter.UserID,
                IP = filter.IP
            };
            var gettingCriteria = new GetEventTurnoverCriteria
            {
                SportID = filter.SportID,
                OnlyShowException = filter.OnlyShowException
            };
            ICommandResult updatingResult = this.UpdateData(updatingCriteria);
            GotLeagueTurnoverResult turnovers = null;

            // Get up-to-date data after updating.
            if (updatingResult.Code == 0)
            {
                turnovers = GetLeagueTurnover(gettingCriteria);
            }

            return new UpdatedTurnoverResult
            {
                ErrorCode = updatingResult.Code,
                ErrorMessage = updatingResult.Message,
                Turnovers = turnovers
            };
        }

        /// <summary>
        /// Updates match maximum turnover and get the list of match turnovers after updating.
        /// </summary>
        /// <param name="filter">The filter that is used to update and query data.</param>
        /// <returns>UpdatedTurnoverResult objects.</returns>
        public UpdatedTurnoverResult UpdateMatchTurnover(UpdateMatchTurnoverFilter filter)
        {
            var updatingCriteria = new UpdateMatchTurnoverCriteria
            {
                MatchID = filter.MatchID,
                Value = filter.Value,
                UserID = filter.UserID,
                IP = filter.IP
            };
            var gettingCriteria = new GetEventTurnoverCriteria
            {
                SportID = filter.SportID,
                LeagueID = filter.LeagueID,
                OnlyShowException = filter.OnlyShowException
            };
            ICommandResult updatingResult = this.UpdateData(updatingCriteria);
            GotMatchTurnoverResult turnovers = null;

            // Get up-to-date data after updating.
            if (updatingResult.Code == 0)
            {
                turnovers = GetMatchTurnover(gettingCriteria);
            }

            return new UpdatedTurnoverResult
            {
                ErrorCode = updatingResult.Code,
                ErrorMessage = updatingResult.Message,
                Turnovers = turnovers
            };
        }

        /// <summary>
        /// Remove league exception and get the list of league turnovers after updating.
        /// </summary>
        /// <param name="filter">The filter that is used to update and query data.</param>
        /// <returns>UpdatedTurnoverResult objects.</returns>
        public UpdatedTurnoverResult RemoveLeagueException(RemoveLeagueExceptionFilter filter)
        {
            var updatingCriteria = new RemoveLeagueExceptionCriteria
            {
                LeagueID = filter.LeagueID,
                UserID = filter.UserID,
                IP = filter.IP
            };
            var gettingCriteria = new GetEventTurnoverCriteria
            {
                SportID = filter.SportID,
                OnlyShowException = filter.OnlyShowException
            };
            ICommandResult updatingResult = this.UpdateData(updatingCriteria);
            GotLeagueTurnoverResult turnovers = null;

            // Get up-to-date data after updating.
            if (updatingResult.Code == 0)
            {
                turnovers = GetLeagueTurnover(gettingCriteria);
            }

            return new UpdatedTurnoverResult
            {
                ErrorCode = updatingResult.Code,
                ErrorMessage = updatingResult.Message,
                Turnovers = turnovers
            };
        }

        /// <summary>
        /// Remove match exception and get the list of match turnovers after updating.
        /// </summary>
        /// <param name="filter">The filter that is used to update and query data.</param>
        /// <returns>UpdatedTurnoverResult objects.</returns>
        public UpdatedTurnoverResult RemoveMatchException(RemoveMatchExceptionFilter filter)
        {
            var updatingCriteria = new RemoveMatchExceptionCriteria
            {
                MatchID = filter.MatchID,
                UserID = filter.UserID,
                IP = filter.IP
            };
            var gettingCriteria = new GetEventTurnoverCriteria
            {
                SportID = filter.SportID,
                LeagueID = filter.LeagueID,
                OnlyShowException = filter.OnlyShowException
            };
            ICommandResult updatingResult = this.UpdateData(updatingCriteria);
            GotMatchTurnoverResult turnovers = null;

            // Get up-to-date data after updating.
            if (updatingResult.Code == 0)
            {
                turnovers = GetMatchTurnover(gettingCriteria);
            }

            return new UpdatedTurnoverResult
            {
                ErrorCode = updatingResult.Code,
                ErrorMessage = updatingResult.Message,
                Turnovers = turnovers
            };
        }

        /// <summary>
        /// Gets the sport list.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>List of SportInfo object.</returns>
        public IEnumerable<SportInfo> GetSportList(GetSportsCriteria criteria)
        {
            return this.GetData<SportInfo>(criteria);
        }

        /// <summary>
        /// Gets the leagues of sport.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>List of LeagueInfo objects.</returns>
        public IEnumerable<LeagueInfo> GetLeaguesOfSport(GetLeaguesOfSportCriteria criteria)
        {
            return this.GetData<LeagueInfo>(criteria);
        }
    }
}