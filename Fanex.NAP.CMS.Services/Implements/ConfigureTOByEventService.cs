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
    using Fanex.Data.Repository;
    using Fanex.NAP.CMS.Core.Resources;
    using Fanex.NAP.CMS.Core.Web;
    using Fanex.NAP.CMS.Criteria.Common;
    using Fanex.NAP.CMS.Criteria.ConfigureTOByEvent;
    using Fanex.NAP.CMS.Models.Common;
    using Fanex.NAP.CMS.Models.ConfigureTOByEvent;
    using Fanex.NAP.CMS.Services.Interfaces;
    using Fanex.NAP.CMS.ViewModels.ConfigureTOByEvent;
    using Fanex.NAP.Framework.Command;

    /// <summary>
    /// Configure turnover by event service.
    /// </summary>
    public class ConfigureTOByEventService : CMSBaseService, IConfigureTOByEventService
    {
        private const int OlympicMode = 1;
        private const int WorldCupMode = 2;

        /// <summary>
        /// Gets the sport turnover.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>List of SportTurnover objects.</returns>
        public IEnumerable<SportTurnover> GetSportTurnover(GetSportTurnoverCriteria criteria)
        {
            IEnumerable<SportTurnoverInfo> rawData = null;
            IEnumerable<SportTurnover> result = null;

            rawData = this.Repository.Fetch<SportTurnoverInfo>(criteria);

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
        public GotLeagueTurnoverResult GetLeagueTurnover(GetLeagueTurnoverCriteria criteria)
        {
            IEnumerable<LeagueTurnoverInfo> rawData = null;
            IEnumerable<LeagueTurnover> turnovers = null;
            LeagueTurnoverInfo firstItem = null;
            GotLeagueTurnoverResult result = null;

            rawData = this.Repository.Fetch<LeagueTurnoverInfo>(criteria);

            if (rawData != null && rawData.Any())
            {
                firstItem = rawData.ElementAt<LeagueTurnoverInfo>(0);

                if (firstItem.LeagueID.HasValue)
                {
                    turnovers = rawData.Select(obj => new LeagueTurnover(obj)).OrderBy(obj => obj.LeagueName).ToList();
                }

                result = new GotLeagueTurnoverResult(firstItem.SportID, firstItem.SportValue, turnovers);
            }
            else
            {
                result = new GotLeagueTurnoverResult();
            }

            return result;
        }

        /// <summary>
        /// Gets the match turnover.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>GotMatchTurnoverResult object.</returns>
        public GotMatchTurnoverResult GetMatchTurnover(GetMatchTurnoverCriteria criteria)
        {
            IEnumerable<MatchTurnoverInfo> rawData = null;
            IEnumerable<MatchTurnover> turnovers = null;
            MatchTurnoverInfo firstItem = null;
            GotMatchTurnoverResult result = null;

            rawData = this.Repository.Fetch<MatchTurnoverInfo>(criteria);

            if (rawData != null && rawData.Any())
            {
                firstItem = rawData.ElementAt<MatchTurnoverInfo>(0);

                if (firstItem.MatchID.HasValue)
                {
                    turnovers = rawData.Select(obj => new MatchTurnover(obj));
                }

                result = new GotMatchTurnoverResult(firstItem.SportID, firstItem.SportValue, firstItem.LeagueID, firstItem.LeagueValue, turnovers);
            }
            else
            {
                result = new GotMatchTurnoverResult();
            }

            return result;
        }

        /// <summary>
        /// Updates the turnover.
        /// </summary>
        /// <param name="criteria">The criteria contain updated data.</param>
        /// <returns>ICommandResult indicates the updating process is successful or not.</returns>
        public ICommandResult UpdateTurnover(ICriteria criteria)
        {
            return this.UpdateData(criteria);
        }

        /// <summary>
        /// Gets the sport list.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>List of SportInfo object.</returns>
        public IEnumerable<SportInfo> GetSportList(GetSportsCriteria criteria)
        {
            IEnumerable<SportInfo> result = this.Repository.Fetch<SportInfo>(criteria);

            return result;
        }

        /// <summary>
        /// Gets the leagues of sport.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>List of LeagueInfo objects.</returns>
        public IEnumerable<LeagueInfo> GetLeaguesOfSport(GetLeaguesOfSportCriteria criteria)
        {
            IEnumerable<LeagueInfo> result = this.Repository.Fetch<LeagueInfo>(criteria);

            return result;
        }

        /// <summary>
        /// Gets the list of countries which have active leagues.
        /// </summary>
        /// <returns>List of CountryInfo objects.</returns>
        public IEnumerable<CountryInfo> GetCountryList()
        {
            IEnumerable<CountryInfo> result = this.Repository.Fetch<CountryInfo>(new GetHaveLeagueCountryCriteria());

            foreach (CountryInfo item in result)
            {
                item.CountryName = CacheRef.GetCountryName(item.CountryCode);

                if (string.IsNullOrEmpty(item.CountryName))
                {
                    item.CountryName = item.CountryCode;
                }
            }

            result = result.OrderBy(obj => obj.CountryName);

            return result;
        }

        /// <summary>
        /// Gets the list of special leagues.
        /// </summary>
        /// <returns>List of CountryInfo objects.</returns>
        public IEnumerable<SpecialLeagueInfo> GetSpecialLeagueList()
        {
            IEnumerable<SpecialLeagueInfo> result = this.Repository.Fetch<SpecialLeagueInfo>(new GetSpecialLeaguesCriteria());
            string specialLeague;

            foreach (SpecialLeagueInfo item in result)
            {
                switch (item.DisplayMode)
                {
                    case OlympicMode:
                        specialLeague = ViewLang.Olympic;
                        break;

                    case WorldCupMode:
                        specialLeague = ViewLang.WorldCup;
                        break;

                    default:
                        specialLeague = string.Empty;
                        break;
                }

                item.DisplayName = string.Format("{0} {1}", specialLeague, item.ProgramID);
            }

            result = result.OrderBy(obj => obj.DisplayName);

            return result;
        }
    }
}