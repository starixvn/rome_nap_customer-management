namespace Fanex.Nap.CMS.VirtualSettingControl.Criteria.UploadVirtualRate
{
    using System;
    using Fanex.Nap.CMS.Infrastructure.Criteria;

    public class UpdateVirtualRateExcelTemplateCriteria : CommandBaseCriteria
    {
        public UpdateVirtualRateExcelTemplateCriteria(
            int sportType,
            string sessionId,
            int userId,
            string ip)
        {
            SportType = sportType;
            SessionIdString = sessionId;
            UserId = userId;
            IP = ip;
        }

        public int SportType { get; }

        public string SessionIdString { get; }

        public int UserId { get; }

        public string IP { get; }

        public override string GetSettingKey()
            => "Acc_VR_AutoUpdateVirtualRate_ByExcel";

        public override bool IsValid()
            => true;
    }
}
