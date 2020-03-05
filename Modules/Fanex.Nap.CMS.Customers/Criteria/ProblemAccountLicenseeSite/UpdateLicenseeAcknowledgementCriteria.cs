namespace Fanex.Nap.CMS.Customers.Criteria.ProblemAccountLicenseeSite
{
    using Fanex.Data.Repository;

    public class UpdateLicenseeAcknowledgementCriteria : CriteriaBase
    {
        public UpdateLicenseeAcknowledgementCriteria(
            int licenseeId,
            bool isAcknowledgement,
            bool isConfirmed,
            int userId,
            string ip)
        {
            LicenseeId = licenseeId;
            IsAcknowledgement = isAcknowledgement;
            IsConfirmed = isConfirmed;
            UserId = userId;
            IP = ip;
        }

        public int LicenseeId { get; }

        public bool IsAcknowledgement { get; }

        public bool IsConfirmed { get; }

        public int UserId { get; }

        public string IP { get; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
            => "NAP_ProblemAccount_LicenseeAcknowledgementUpd";
    }
}
