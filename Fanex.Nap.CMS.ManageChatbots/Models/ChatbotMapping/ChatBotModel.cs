namespace Fanex.Nap.CMS.ManageChatbots.Models.ChatbotMapping
{
    using System;
    using Fanex.Nap.CMS.Infrastructure.Extensions;

    public class ChatBotModel
    {
        public string ChatBotName { get; set; }

        public long ChatID { get; set; }

        public string UserName { get; set; }

        public string SubAccount { get; set; }

        public string LinkStatus { get; set; }

        public string Language { get; set; }

        public string Level { get; set; }

        public string Currency { get; set; }

        public string AccStatus { get; set; }

        public bool Status { get; set; }

        public bool IsVerifyOTP { get; set; }

        public int CusID { get; set; }

        public int SubAccID { get; set; }

        public int BotID { get; set; }

        public int UserID { get; set; }

        public DateTime UpdatedTime { get; set; }
        
        public string SiteName { get; set; }

        public string UpdatedTimeStr => UpdatedTime == DateTime.MinValue
            ? string.Empty
            : DateTimeExtensions.ToLongDate24hWithSlash(UpdatedTime);
    }
}