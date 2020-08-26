namespace Fanex.Nap.CMS.Customers.Criteria.ProblemAccountLicenseeSite
{
    public class UpdateLicenseeSiteCriteria : MarkNewLicenseeSiteCriteria
    {
        public UpdateLicenseeSiteCriteria(
            string usernames,
            byte preventActionId,
            byte reasonId,
            string comment,
            string voidTime,
            decimal limitAmount,
            bool updateTime,
            bool updateLimitAmount,
            int userId,
            string ip) : base(usernames, preventActionId, reasonId, comment, voidTime, limitAmount, userId, ip)
        {
            UpdateTime = updateTime;
            UpdateLimitAmount = updateLimitAmount;
        }

        public bool UpdateTime { get; }

        public bool UpdateLimitAmount { get; }

        public override string GetSettingKey()
            => "NAP_ProblemAccount_LicenseeAccountReasonUpd";
    }
}
