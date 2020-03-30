namespace Fanex.Nap.CMS.VirtualSettingControl.Criteria.ManualUpdateVRAccount
{
    using Fanex.Data.Repository;

    public class UnmarkManualUpdateAccountsCriteria : CriteriaBase
    {
        public UnmarkManualUpdateAccountsCriteria(
            int sportType,
            string custIds,
            int userId,
            string ip)
        {
            SportType = sportType;
            CustIds = custIds;
            UserId = userId;
            IP = ip;
        }

        public int SportType { get; }

        public string CustIds { get; }

        public int UserId { get; }

        public string IP { get; }

        public override string GetSettingKey()
            => "Acc_VR_ManualUpdAcc_Remove";
    }
}
