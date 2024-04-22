namespace Fanex.Nap.CMS.CustomerClass.Criteria
{
    using Fanex.NAP.Framework.Data;
    using System;

    internal class GetSiteUploadProgressCriteria : CriteriaBase
    {
        public int UserID { get; set; }

        public int CountBatch { get; set; }

        public int InitialBatch { get; set; }

        public DateTime? LastModifiedTime { get; set; }

        public override string GetSettingKey()
            => "NAP_LM_CustomerClassPolicyProcessImport";
    }
}
