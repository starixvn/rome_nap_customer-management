namespace Fanex.Nap.CMS.CustomerClass.DataAccesses
{
    using Fanex.Nap.CMS.CustomerClass.Models;
    using System;
    using System.Data;
    using Rule = Models.Rule;

    internal static class DataReaderToModelHelper
    {
        public static Rule ToPagedRule(IDataReader reader)
            => GetInt(reader["RuleId"]).HasValue
            ? ToRule(
                reader,
                leagueGroups: reader["LeagueGroupNames"].ToString(),
                enable: Convert.ToBoolean(reader["Enable"]))
            : null;

        public static Rule ToRuleList(IDataReader reader)
            => GetInt(reader["RuleId"]).HasValue
            ? ToRule(
                reader,
                leagueGroups: reader["LeagueGroupNames"].ToString(),
                enable: Convert.ToBoolean(reader["Enable"]))
            : null;

        public static Rule ToPolicyRuleList(IDataReader reader)
            => GetInt(reader["RuleId"]).HasValue
            ? ToRule(
                reader,
                leagueGroups: reader["LeagueGroupNames"].ToString(),
                enable: Convert.ToBoolean(reader["Enable"]))
            : null;

        public static Rule ToRule(IDataReader reader)
            => GetInt(reader["RuleId"]).HasValue
            ? ToRule(reader, leagueGroupIds: reader["LeagueGroupIds"].ToString())
            : null;

#pragma warning disable S3427
        private static Rule ToRule(
            IDataReader reader,
            string leagueGroupIds = null,
            string leagueGroups = null,
            bool? enable = false,
            string site = null,
            string currencies = null,
            string platforms = null)
#pragma warning restore S3427
            => new Rule(
                Convert.ToInt32(reader["RuleId"]),
                GetShort(reader["CustomerClass"]),
                GetShort(reader["CustomerClassDetail"]),
                reader["RuleTitle"].ToString(),
                Convert.ToInt16(reader["SportType"]),
                leagueGroupIds,
                leagueGroups,
                Convert.ToByte(reader["IsLive"]),
                GetInt(reader["DiffBalanceRatio"]),
                GetInt(reader["BetDelay"]),
                GetShort(reader["NextBetInterval"]),
                GetShort(reader["SuperBetInterval"]),
                GetShort(reader["SiteBetInterval"]),
                GetInt(reader["MinBet"]),
                GetInt(reader["MaxBet"]),
                GetInt(reader["MaxPerMatch"]),
                GetByte(reader["RepeatedBets"]),
                GetByte(reader["SeizeBetTicket"]),
                Convert.ToBoolean(reader["DisableParlay"]),
                GetShort(reader["PeakHourSpreadAdjustment"]),
                enable,
                site,
                currencies,
                platforms,
                HasColumn(reader, "PolicyTitles") ? reader["PolicyTitles"].ToString() : string.Empty);

        public static RuleDetail ToRuleDetail(IDataReader reader)
            => new RuleDetail(
                reader["Sites"].ToString(),
                Convert.ToInt32(reader["NumberOfSites"]),
                reader["CurrencyName"].ToString(),
                Convert.ToInt32(reader["NumberOfCurrencies"]),
                reader["Platform"].ToString(),
                Convert.ToInt32(reader["NumberOfPlatform"]));

        private static int? GetInt(object value)
            => value == null || value == DBNull.Value
            ? default(int?)
            : Convert.ToInt32(value);

        private static short? GetShort(object value)
            => value == null || value == DBNull.Value
            ? default(short?)
            : Convert.ToInt16(value);

        private static byte? GetByte(object value)
            => value == null || value == DBNull.Value
            ? default(byte?)
            : Convert.ToByte(value);

        private static bool HasColumn(IDataRecord dataRecord, string columnName)
        {
            for (int i = 0; i < dataRecord.FieldCount; i++)
            {
                if (dataRecord.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
