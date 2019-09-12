namespace Fanex.Nap.CMS.Customers.Criteria.ProblemAccount
{
    public class MarkNewCriteria : NonQueryCriteriaBase
    {
        public MarkNewCriteria(
            string usernames,
            byte roleId,
            byte reasonId,
            string comment,
            int userId,
            string ip) : base(usernames, roleId, userId, ip)
        {
            ReasonId = reasonId;
            Comment = comment;
        }

        public byte ReasonId { get; }

        public string Comment { get; }

        public string SuccessList { get; set; }

        public string FailList { get; set; }

        public override string GetSettingKey()
            => "CS_ProblemAccount_AddCustomer";

        public override bool IsValid()
            => base.IsValid() && ReasonId > 0;
    }
}
