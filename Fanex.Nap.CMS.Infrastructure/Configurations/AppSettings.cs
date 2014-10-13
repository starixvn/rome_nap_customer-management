// --------------------------------------------------------------------------
// <copyright file="AppSettings.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2014.10.03">Created</change>
// </history>

namespace Fanex.Nap.CMS.Infrastructure.Configurations
{
    using System.Configuration;
    using Fanex.Common;

    /// <summary>
    /// Application settings.
    /// </summary>
    public static class AppSettings
    {
        private static string serverTimezone;
        private static int? turnoverPercentageUsed;
        private static int smaTurnoverMailFunctionId;

        public static string ServerTimezone
        {
            get
            {
                if (serverTimezone == null)
                {
                    serverTimezone = GetValue("ServerTimezone");
                }

                return serverTimezone;
            }
        }

        public static int? TurnoverPercentageUsed
        {
            get
            {
                if (turnoverPercentageUsed == null)
                {
                    turnoverPercentageUsed = Converter.ToInt(GetValue("TOControlPercentageUsed"));
                }

                return turnoverPercentageUsed;
            }
        }

        public static int SMATurnoverMailFunctionId
        {
            get
            {
                smaTurnoverMailFunctionId = Converter.ToInt(GetValue("Admin_SMATurnover_FunctionId"));
                return smaTurnoverMailFunctionId;
            }
        }

        /// <summary>
        /// Gets the value from web configuration.
        /// </summary>
        /// <param name="key">The key which is used to identify the value.</param>
        /// <returns>Associated value of passed key.</returns>
        private static string GetValue(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}