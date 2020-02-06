// --------------------------------------------------------------------------
// <copyright file="AppSettings.cs">
// Copyright (c). All rights reserved.
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
                return Converter.ToInt(GetValue("Admin_SMATurnover_FunctionId"));
            }
        }

        public static string SMATurnoverMailFunctionName
        {
            get
            {
                return Converter.ToString2(GetValue("Admin_SMATurnover_FunctionName"));
            }
        }

        public static bool IsTest => Converter.ToInt(GetValue("IsTest")) == 1;

        public static string LoggingDefaultCategory => GetValue("Fanex.Logging:DefaultCategory");

        public static string LogLibs => GetValue("LogLibs");

        public static string RabbitMQConnectionStrings => GetValue("Fanex.Logging:RabbitMQConnectionStrings");

        public static string SentryBackendUrl => GetValue("Fanex.Logging:SentryBackendUrl");

        public static string SentryUrl => GetValue("Fanex.Logging:SentryUrl");

        public static string RavenLibPath => GetValue("RavenLibPath");

        public static string Environment => GetValue("Environment");

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
