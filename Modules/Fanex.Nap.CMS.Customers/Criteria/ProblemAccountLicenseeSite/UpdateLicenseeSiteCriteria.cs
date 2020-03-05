namespace Fanex.Nap.CMS.Customers.Criteria.ProblemAccountLicenseeSite
{
    public class UpdateLicenseeSiteCriteria : MarkNewLicenseeSiteCriteria
    {
        public UpdateLicenseeSiteCriteria(
            string usernames,
            byte preventActionId,
            byte reasonId,
            string comment,
            int userId,
            string ip) : base(usernames, preventActionId, reasonId, comment, userId, ip)
        {
        }

        public override string GetSettingKey()
            => "NAP_ProblemAccount_LicenseeAccountReasonUpd";
    }
}
