namespace Fanex.Nap.CMS.Customers.Criteria.ManageRobots2
{
    using Fanex.Nap.CMS.Infrastructure.Criteria;

    public class AddRobotCriteria : CommandBaseCriteria
    {
        public string Usernames { get; set; } = string.Empty;

        public string Remark { get; set; } = string.Empty;

        public int Level { get; set; }

        public bool ConfirmUpline { get; set; }

        public int UserId { get; set; }

        public string IP { get; set; }

        public override string GetSettingKey()
            => "NAP_Customer_RobotAccountNew_Ins";

        public override bool IsValid()
            => true;
    }
}
