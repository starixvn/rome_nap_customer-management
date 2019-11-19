// --------------------------------------------------------------------------
// <copyright file="CacheRef.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2014.10.03">Created</change>
// </history>

namespace Fanex.Nap.CMS.Infrastructure.Web
{
    using System;
    using System.Collections.Generic;
    using Fanex.Common;
    using RemotingRef;

    /// <summary>
    /// Cache reference.
    /// </summary>
    public static class CacheRef
    {
        private const string LanguageCode = "en-US";

        public static string GetTeamName(object teamId)
        {
            return RefData.GetTeamName(
                Converter.ToString2(teamId),
                LanguageCode,
                string.Empty);
        }

        public static string GetLeagueName(string leagueId)
        {
            return RefData.GetLeagueName(leagueId, LanguageCode);
        }

        /// <summary>
        /// Saves the cache reference response.
        /// </summary>
        /// <param name="identityList">The identifier list.</param>
        /// <param name="cacheRefFunction">The cache reference function.</param>
        /// <param name="currentStorage">The current storage.</param>
        /// <returns>The cache reference response.</returns>
        public static Dictionary<object, string> SaveCacheRefResponse(
            List<object> identityList,
            Func<object, string> cacheRefFunction,
            Dictionary<object, string> currentStorage)
        {
            foreach (var identityInfo in identityList)
            {
                if (!currentStorage.ContainsKey(identityInfo))
                {
                    currentStorage.Add(identityInfo, cacheRefFunction(identityInfo));
                }
            }

            return currentStorage;
        }

        public static string GetBetType(object bettypeId)
        {
            return GetBetType(bettypeId, null);
        }

        public static string GetSportName(object sportId)
        {
            return RefData.GetSportName(sportId.ToString(), LanguageCode);
        }

        /// <summary>
        /// Gets the type of the bet.
        /// </summary>
        /// <param name="betTypeId">The bet type identifier.</param>
        /// <param name="betId">The bet identifier.</param>
        /// <returns>The type of the bet.</returns>
        public static string GetBetType(object betTypeId, object betId)
        {
            // resourceId remove first character
            string resourceId = string.Empty;
            if (betId != null && betId.ToString().Length > 1)
            {
                resourceId = betId.ToString().Substring(1);
            }

            return RefData.GetBetTypeName(
                betTypeId.ToString(),
                LanguageCode,
                string.Empty,
                resourceId);
        }

        private static IRemotingRefService RefData => new RemotingRefService();
    }
}
