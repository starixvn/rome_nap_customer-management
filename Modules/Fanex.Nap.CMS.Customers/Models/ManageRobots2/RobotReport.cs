namespace Fanex.Nap.CMS.Customers.Models.ManageRobots2
{
    using System;
    using System.Collections.Generic;

    public class RobotReport
    {
        public RobotReport()
        {
            Robots = new List<RobotModel>();
        }

        public List<RobotModel> Robots { get; set; }

        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public int ResultCode { get; set; }
        public string ResultMessage { get; set; }
    }
}