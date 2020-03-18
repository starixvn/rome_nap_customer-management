namespace Fanex.Nap.CMS.VirtualSettingControl.Criteria.ManualUpdateVRAccount
{
    using Fanex.Data.Repository;

    public class GetManualUpdateAccountListCriteria : CriteriaBase
    {
        public GetManualUpdateAccountListCriteria(int sportType, string username)
        {
            SportType = sportType;
            Username = username;
        }

        public int SportType { get; }

        public string Username { get; }

        public override string GetSettingKey()
            => "Acc_VR_ManualUpdAcc_Sel";

        public override bool IsValid()
            => SportType > 0;
    }
}
