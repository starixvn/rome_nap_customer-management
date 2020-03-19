namespace Fanex.Nap.CMS.VirtualSettingControl.Criteria.UploadVirtualRate
{
    using System;
    using Fanex.Data.Repository;

    public class UpdateVirtualRateExcelTemplateCriteria : CriteriaBase
    {
        public UpdateVirtualRateExcelTemplateCriteria(
            int sportType,
            Guid sessionId,
            int userId,
            string ip)
        {
            SportType = sportType;
            SessionId = sessionId;
            UserId = userId;
            IP = ip;
        }

        public int SportType { get; }

        public Guid SessionId { get; }

        public int UserId { get; }

        public string IP { get; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
            => "Acc_VR_AutoUpdateVirtualRate_ByExcel";
    }
}
