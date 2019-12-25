namespace Fanex.Nap.CMS.Customers.Criteria.ProblemAccount
{
    public class UpdateInformingStatusCriteria : NonQueryCriteriaBase
    {
        public UpdateInformingStatusCriteria(
            string usernames,
            byte roleId,
            int informedType,
            bool applyDownlines,
            int userId,
            string ip) : base(usernames, roleId, userId, ip)
        {
            InformedType = informedType;
            ApplyDownlines = applyDownlines;
        }

        public int InformedType { get; }

        public bool ApplyDownlines { get; }

        public override string GetSettingKey()
            => "CS_ProblemAccount_InformingStatus";
    }
}
