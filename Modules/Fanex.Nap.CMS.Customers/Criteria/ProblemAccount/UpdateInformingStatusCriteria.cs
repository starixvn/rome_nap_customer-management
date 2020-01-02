namespace Fanex.Nap.CMS.Customers.Criteria.ProblemAccount
{
    public class UpdateInformingStatusCriteria : NonQueryCriteriaBase
    {
        public UpdateInformingStatusCriteria(
            string usernames,
            byte roleId,
            byte informedType,
            bool isApplyDownlines,
            int userId,
            string ip) : base(usernames, roleId, userId, ip)
        {
            InformedType = informedType;
            IsApplyDownlines = isApplyDownlines;
        }

        public byte InformedType { get; }

        public bool IsApplyDownlines { get; }

        public override string GetSettingKey()
            => "CS_ProblemAccount_InformStatusUpd";
    }
}
