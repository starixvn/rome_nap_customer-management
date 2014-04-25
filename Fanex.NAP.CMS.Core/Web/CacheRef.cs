// --------------------------------------------------------------------------
// <copyright file="CacheRef.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.11.04">Created</change>
// </history>

namespace Fanex.NAP.CMS.Core.Web
{
    using Fanex.Common;
    using RemotingRef;

    /// <summary>
    /// Caching remote data.
    /// </summary>
    public static class CacheRef
    {
        /// <summary>
        /// Gets bet type name.
        /// </summary>
        /// <param name="bettypeId">Bet type ID.</param>
        /// <returns>Bet type name.</returns>
        public static string GetBettype(object bettypeId)
        {
            return GetBettype(bettypeId, null);
        }

        /// <summary>
        /// Gets bet type name by bet type ID and bet ID.
        /// </summary>
        /// <param name="bettypeId">Bet type ID.</param>
        /// <param name="betID">Bet ID value.</param>
        /// <returns>Bet type name.</returns>
        public static string GetBettype(object bettypeId, object betID)
        {
            // resourceId remove first character
            string resourceId = string.Empty;
            if (betID != null && betID.ToString().Length > 1)
            {
                resourceId = betID.ToString().Substring(1);
            }

            return RefData.GetBetTypeName(bettypeId.ToString(), TWLang("en-US"), string.Empty, resourceId);
        }

        /// <summary>
        /// Gets sport name by ID.
        /// </summary>
        /// <param name="sportID">Sport ID value.</param>
        /// <returns>Sport name.</returns>
        public static string GetSportName(object sportID)
        {
            return RefData.GetSportName(Converter.ToString2(sportID), TWLang("en-US"), string.Empty);
        }

        /// <summary>
        /// Gets team name.
        /// </summary>
        /// <param name="teamID">Team ID value.</param>
        /// <returns>Team name.</returns>
        public static string GetTeamName(object teamID)
        {
            return RefData.GetTeamName(Converter.ToString2(teamID), TWLang("en-US"), string.Empty);
        }

        /// <summary>
        /// Gets league name by ID.
        /// </summary>
        /// <param name="leagueID">League ID.</param>
        /// <returns>League name.</returns>
        public static string GetLeagueName(object leagueID)
        {
            return RefData.GetLeagueName(Converter.ToString2(leagueID), TWLang("en-US"), string.Empty);
        }

        /// <summary>
        /// Gets odd type name.
        /// </summary>
        /// <param name="oddsType">Odd type ID.</param>
        /// <returns>Odd type name.</returns>
        public static string GetOddsTypeName(object oddsType)
        {
            return RefData.GetOddsTypeName(Converter.ToString2(oddsType), TWLang("en-US"));
        }

        /// <summary>
        /// Gets the name of the country.
        /// </summary>
        /// <param name="countryCode">The country code.</param>
        /// <returns>The country name.</returns>
        public static string GetCountryName(string countryCode)
        {
            return RefData.GetCountryName(countryCode, TWLang("en-US"));
        }

        /// <summary>
        /// Gets combination name.
        /// </summary>
        /// <param name="betteam">Bet team name.</param>
        /// <returns>Combination name.</returns>
        public static string GetCombinationName(string betteam)
        {
            string name = string.Empty;
            switch (betteam)
            {
                case "110":
                    name = "Trixie";
                    break;

                case "1110":
                    name = "Yankee";
                    break;

                case "11110":
                    name = "Canadian";
                    break;

                case "111110":
                    name = "Heinz";
                    break;

                case "1111110":
                    name = "Super Heinz";
                    break;

                case "11111110":
                    name = "Goliath";
                    break;
            }

            return name;
        }

        /// <summary>
        /// Gets language ID.
        /// </summary>
        /// <param name="lang">Language string.</param>
        /// <returns>Language ID.</returns>
        private static string TWLang(string lang)
        {
            switch (lang)
            {
                case "en-US": return "en"; // US English
                case "zh-TW": return "ch"; // Traditional
                case "zh-CN": return "cs"; // Simplified
                case "ja-JP": return "jp"; // Japanese
                case "th-TH": return "th"; // Thai
                default: return "en";
            }
        }
    }
}