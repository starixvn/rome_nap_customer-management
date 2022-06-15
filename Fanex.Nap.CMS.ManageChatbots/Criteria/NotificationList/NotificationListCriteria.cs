
namespace Fanex.Nap.CMS.ManageChatbots.Criteria.NotificationList
{
    using System;
    using Fanex.Data.Repository;

    public class NotificationListCriteria : CriteriaBase
    {
        public string FunctionIds { get; set; }

        public string Username { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public long? NotificationId { get; set; }

        public string Status { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalRows { get; set; }

        public override string GetSettingKey()
            => "NAP_Chatbot_CustBotNotificationListSel";
    }
}
