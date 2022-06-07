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
    using Fanex.Nap.CMS.Infrastructure.Models;

    public class ChatBotMappingRepository : IChatbotMappingRepository
    {
        public IEnumerable<IdNameModel> GetCurrencies(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        yield return new IdNameModel
                        {
                            Name = reader["Currency"].ToString(),
                            Id = int.Parse(reader["ExchangeID"].ToString())
                        };
                    }
                }
            }
        }

        public IEnumerable<IdNameModel> GetSites(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        yield return new IdNameModel
                        {
                            Name = reader["site"].ToString(),
                            Id = int.Parse(reader["siteid"].ToString())
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

        public ChatBotsMappingModel GetChatBots(ICriteria criteria)
        {
            IList<ChatBotModel> chatBotModels = new List<ChatBotModel>();
            var TotalRows = 0;
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        chatBotModels.Add(ChatBotMappingItem(reader));
                    }
                }

                TotalRows = Convert.ToInt32(db.GetOuputParams()["totalrows"]);
            }

            return new ChatBotsMappingModel(TotalRows, chatBotModels);
        }

        public ChatBotsConfigModel GetConfigChatBot(ICriteria criteria)
        {
            IList<ChatBotModel> chatBotModels = new List<ChatBotModel>();
            IList<ChatBotFunctionConfig> functions = new List<ChatBotFunctionConfig>();

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        chatBotModels.Add(ChatBotConfig(reader));
                    }

                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            functions.Add(ChatBotFuncConfig(reader));
                        }
                    }
                }
            }

            return new ChatBotsConfigModel
            {
                ChatBotModel = chatBotModels[0],
                FunctionConfigs = functions
            };
        }

        private static ChatBotModel ChatBotMappingItem(IDataReader reader)
        {
            var chatBotModel = new ChatBotModel
            {
                ChatBotName = reader["BotName"].ToString(),
                ChatID = Convert.ToInt64(reader["ChatID"]),
                UserName = reader["username"].ToString(),
                SubAccount = reader["SubAccount"].ToString(),
                LinkStatus = reader["LinkStatus"].ToString(),
                Language = LanguageNameConstants.GetLanguageName(reader["language"].ToString()),
                Level = RoleIdConstants.GetRoleName(Convert.ToInt16(reader["roleid"])),
                Currency = reader["currency"].ToString(),
                AccStatus = reader["status"].ToString(),
                CusID = Converter.ToInt(reader["custid"]),
                SubAccID = Converter.ToInt(reader["SubAccID"]),
                BotID = Converter.ToInt(reader["BotID"])
            };
            return chatBotModel;
        }

        private static ChatBotModel ChatBotConfig(IDataReader reader)
        {
            var chatBotModel = new ChatBotModel
            {
                ChatBotName = reader["BotName"].ToString(),
                ChatID = Convert.ToInt64(reader["ChatID"]),
                UserName = reader["username"].ToString(),
                SubAccount = reader["SubAccount"].ToString(),
                SubAccID = Converter.ToInt(reader["SubAccID"]),
                CusID = Converter.ToInt(reader["custid"]),
                IsVerifyOTP = Converter.ToBool(reader["isVerifyOTP"]),
                Language = reader["language"].ToString(),
                Status = Converter.ToBool(reader["Status"]),
                UserID = Converter.ToInt(reader["UserID"]),
            };

            return chatBotModel;
        }

        private static ChatBotFunctionConfig ChatBotFuncConfig(IDataReader reader)
        {
            var chatBotModel = new ChatBotFunctionConfig(
                Converter.ToInt(reader["FunctionID"]),
                reader["FunctionName"].ToString(),
                Converter.ToBool(reader["status"])
            );
            return chatBotModel;
        }

        public Result ExecuteNonQuery(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(criteria.GetSpParams());
                var outputParams = db.GetOuputParams();

                return new Result(
                    Convert.ToInt32(outputParams["errcode"]),
                    outputParams["errmsg"].ToString());
            }
        }
    }
}