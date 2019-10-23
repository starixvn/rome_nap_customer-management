namespace Fanex.Nap.CMS.Customers.Criteria.ManageRobots2
{
    using Fanex.Data.Repository;

    public class DeleteRobotCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public int UserId { get; set; }

        public string IP { get; set; }

        public int ErrCode { get; set; } = 0;

        public string ErrMsg { get; set; } = string.Empty;

        public override string GetSettingKey()
            => "NAP_Customer_RobotAccountNew_Del";
    }
}
