namespace Fanex.Nap.CMS.Customers.Models.ManageRobots2
{
    using System;

    public class RobotModel
    {
        public int CustId { get; set; }
        public string Username { get; set; }
        public string Level { get; set; }
        public string Site { get; set; }
        public string IP { get; set; }

        public DateTime? LastChangeTime { get; set; }
        public string LastChangeTimeStr => LastChangeTime?.ToString("MM/dd/yyyy HH:mm:ss") ?? string.Empty;

        public int? Counter { get; set; }
        public string CounterStr => Counter?.ToString("#,##0") ?? string.Empty;

        public string ServerIP { get; set; }
        public string AddedBy { get; set; }

        public DateTime? AddedDate { get; set; }
        public string AddedDateStr => AddedDate?.ToString("MM/dd/yyyy") ?? string.Empty;

        public string Remark { get; set; }
        public string Currency { get; set; }
    }
}
