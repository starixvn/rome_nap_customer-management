namespace Fanex.NAP.CMS.Criteria.StatusLog
{
    using Data.Repository;

    public class LogCriteria : CriteriaBase
    {
        public string Code { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int TotalRows { get; set; }

        public override string GetSettingKey()
            => "NAP_StatusLog_CreditCollection_List";

        public override bool IsValid() => true;
    }
}
