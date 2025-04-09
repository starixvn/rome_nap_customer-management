namespace Fanex.Nap.CMS.ManageChatbots.Criteria.ChatbotMapping
{
    using Fanex.Data.Repository;
    using System.Collections;
    using System.Text;
    using Fanex.Nap.CMS.ManageChatbots.Models.ChatbotMapping;
    using System;
    using Fanex.Data;

    public class UpdFunctionsChatBotCriteria : NonQueryCommand
    {
        public int UserID { get; set; }

        public int BotID { get; set; }

        public int Creator { get; set; }

        public string Setting => ToXml();

        public string IP { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        [SpParam(Ignored = true)]
        public ChatBotFunctionConfig[] FunctionConfigs { get; set; }

        public override string GetSettingKey()
            => "Age_Chatbot_CustBotFunctionUpd";

        private string ToXml()
        {
            var xmlBuilder = new StringBuilder();
            xmlBuilder.Append("<Root>");

            if (FunctionConfigs != null)
            {
                foreach (var item in FunctionConfigs)
                {
                    xmlBuilder.Append(ToXml(item));
                }
            }


            xmlBuilder.Append("</Root>");

            return xmlBuilder.ToString();
        }

        private static string ToXml(ChatBotFunctionConfig item)
            => $"<r id=\"{item.FunctionId}\" stt=\"{Convert.ToInt16(item.IsEnabled)}\"/>";

        public override bool IsValid()
            => true;
    }
}