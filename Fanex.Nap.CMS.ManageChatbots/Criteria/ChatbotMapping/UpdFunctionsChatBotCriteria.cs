using System;

namespace Fanex.Nap.CMS.ManageChatbots.Criteria.ChatbotMapping
{
    using Fanex.NAP.Framework.Data;
    using System.Collections;
    using System.Text;
    using Fanex.Nap.CMS.ManageChatbots.Models.ChatbotMapping;

    public class UpdFunctionsChatBotCriteria : CriteriaBase
    {
        public int UserID { get; set; }

        public int Creator { get; set; }
        public string IP { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }
        public ChatBotFunctionConfig[] FunctionConfigs { get; set; }

        public override string GetSettingKey()
            => "Age_Chatbot_CustBotFunctionUpd";

        public override Hashtable GetSpParams()
            => new Hashtable
            {
                [nameof(UserID)] = UserID,
                [nameof(Creator)] = Creator,
                ["Setting"] = ToXml(),
                [nameof(IP)] = IP,
                [nameof(ErrCode)] = ErrCode,
                [nameof(ErrMsg)] = ErrMsg,
            };

        private string ToXml()
        {
            var xmlBuilder = new StringBuilder();
            xmlBuilder.Append("<Root>");

            foreach (var item in FunctionConfigs)
            {
                xmlBuilder.Append(ToXml(item));
            }

            xmlBuilder.Append("</Root>");

            return xmlBuilder.ToString();
        }

        private static string ToXml(ChatBotFunctionConfig item)
            => $"<r id=\"{item.FunctionId}\" stt=\"{Convert.ToInt16(item.IsEnabled)}\"/>";
    }
}