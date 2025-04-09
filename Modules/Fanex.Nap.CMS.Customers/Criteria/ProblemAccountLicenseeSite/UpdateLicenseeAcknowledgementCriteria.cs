using Fanex.Nap.CMS.Infrastructure.Criteria;

namespace Fanex.Nap.CMS.Customers.Criteria.ProblemAccountLicenseeSite
{
    public class UpdateLicenseeAcknowledgementCriteria : CommandBaseCriteria
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

        public override string GetSettingKey()
            => "NAP_ProblemAccount_LicenseeAcknowledgementUpd";

        public override bool IsValid()
         => true;
    }
}
