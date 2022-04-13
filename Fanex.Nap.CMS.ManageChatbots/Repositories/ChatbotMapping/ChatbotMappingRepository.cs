

namespace Fanex.Nap.CMS.ManageChatbots.Repositories.ChatbotMapping
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.Data;
    using Fanex.NAP.Framework.Data;
    using System.Data;
    using Fanex.Nap.CMS.ManageChatBots.Models.ChatbotMapping;
    using System;
    using Fanex.Nap.CMS.Infrastructure.Constants;
    using Fanex.Nap.CMS.ManageChatbots.Models.ChatbotMapping;
    using Fanex.Common;
    public class ChatBotMappingRepository : IChatbotMappingRepository
    {
        public IEnumerable<SelectListItem> GetCurrencies(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                        yield return new SelectListItem
                        {
                            Text = reader["Currency"].ToString(),
                            Value = reader["Id"].ToString()
                        };
                }
            }
        }

        public IEnumerable<SelectListItem> GetSites(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        yield return new SelectListItem
                        {
                            Text = reader["site"].ToString(),
                        };
                    }
                }
            }
        }

        public IEnumerable<NapAccountingPolicyModel> GetNapAccountingPolicy(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        yield return NapAccountingPolicyModel(reader);
                    }
                }
            }
        }

        private static NapAccountingPolicyModel NapAccountingPolicyModel(IDataReader reader)
            => new NapAccountingPolicyModel(
                reader["funcname"].ToString());

        public IEnumerable<ChatBotModel> GetChatBots(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        yield return GetGlobalSettingItem(reader);
                    }
                }
            }
        }

        private static ChatBotModel GetGlobalSettingItem(IDataReader reader)
            => new ChatBotModel(
                reader["BotName"].ToString(),
                reader["MessengerName"].ToString(),
                reader["username"].ToString(), reader["SubAccount"].ToString(),
                reader["LinkStatus"].ToString(), reader["language"].ToString(),
                RoleIdConstants.GetRoleName(Convert.ToInt16(reader["roleid"])),
                reader["currency"].ToString(), reader["status"].ToString(),
                Converter.ToInt(reader["custid"]),Converter.ToInt(reader["SubAccID"]),Converter.ToInt(reader["BotID"])
                );
    }
}