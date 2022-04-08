namespace Fanex.Nap.CMS.ManageChatbots.Repositories.ChatbotMapping
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.Data;
    using Fanex.NAP.Framework.Data;
    using System.Data;
    using Fanex.Nap.CMS.ManageChatBots.Models.ChatBotMapping;
    using System;
    using Fanex.Nap.CMS.ManageChatbots.Models.ChatbotMapping;
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

        public ChatBotsMappingModel GetChatBots(ICriteria criteria)
        {
            throw new System.NotImplementedException();
        }

        private static NapAccountingPolicyModel NapAccountingPolicyModel(IDataReader reader)
            => new NapAccountingPolicyModel(
                reader["funcname"].ToString());
        
        public IEnumerable<ChatBotModel> GetList(ICriteria criteria)
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
                Convert.ToInt32(reader["custid"]),
                reader["username"].ToString(),
                reader["level"].ToString(),
                Convert.ToBoolean(reader["IsGlobalLockTOLimit"]),
                reader["currency"].ToString(),
                Convert.ToInt32(reader["profileid"]),
                reader["profilename"].ToString(),
                Convert.ToDecimal(reader["maxturnover"]));

    }
}