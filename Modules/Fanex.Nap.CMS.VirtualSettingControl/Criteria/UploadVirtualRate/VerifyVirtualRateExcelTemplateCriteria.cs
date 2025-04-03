namespace Fanex.Nap.CMS.VirtualSettingControl.Criteria.UploadVirtualRate
{
    using System;
    using Fanex.Data.Repository;

    public class VerifyVirtualRateExcelTemplateCriteria : CriteriaBase
    {
        public VerifyVirtualRateExcelTemplateCriteria(
            int sportType,
            string sessionId,
            int userId,
            string xml)
        {
            SportType = sportType;
            SessionIdString = sessionId;
            UserId = userId;
            XML = xml;
        }

        public int SportType { get; }

        public string SessionIdString { get; }

        public int UserId { get; }

        public string XML { get; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public int ValidUser_Count { get; set; }

        public override string GetSettingKey()
            => "Acc_VR_AutoUpdateVirtualRate_ByExcel_InsTmp";

        public override bool IsValid()
            => true;
    }
}
