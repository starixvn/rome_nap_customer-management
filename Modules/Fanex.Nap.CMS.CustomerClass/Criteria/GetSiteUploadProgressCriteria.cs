namespace Fanex.Nap.CMS.CustomerClass.Criteria
{
    using Fanex.Data.Repository;
    using System;

    internal class GetSiteUploadProgressCriteria : NonQueryCommand
    {
        public int UserID { get; set; }

        public int CountBatch { get; set; }

        public int InitialBatch { get; set; }

        public DateTime? LastModifiedTime { get; set; }

        public override string GetSettingKey()
            => "NAP_LM_CustomerClassPolicyProcessImport";

        public override bool IsValid()
            => true;
    }
}
