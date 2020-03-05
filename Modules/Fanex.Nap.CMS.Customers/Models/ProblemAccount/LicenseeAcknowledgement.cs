namespace Fanex.Nap.CMS.Customers.Models.ProblemAccount
{
    public class LicenseeAcknowledgement
    {
        public LicenseeAcknowledgement(
            int licenseeId,
            string licenseeName,
            bool isActive,
            bool isAcknowledgement)
        {
            LicenseeId = licenseeId;
            LicenseeName = licenseeName;
            IsActive = isActive;
            IsAcknowledgement = isAcknowledgement;
        }

        public int LicenseeId { get; }

        public string LicenseeName { get; }

        public bool IsActive { get; }

        public bool IsAcknowledgement { get; }
    }
}
