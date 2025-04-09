namespace Fanex.Nap.CMS.Customers.Criteria.ManageRobots2
{
    using Fanex.Nap.CMS.Infrastructure.Criteria;

    public class DeleteRobotCriteria : CommandBaseCriteria
    {
        public int CustId { get; set; }

        public int UserId { get; set; }

        public string IP { get; set; }

        public override string GetSettingKey()
            => "NAP_Customer_RobotAccountNew_Del";

        public override bool IsValid()
            => true;
    }
}
