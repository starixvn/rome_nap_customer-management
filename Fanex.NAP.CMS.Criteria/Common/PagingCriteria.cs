namespace Fanex.NAP.CMS.Criteria.Common
{
    using Fanex.Data.Repository;

    public abstract class PagingCriteria : CriteriaBase
    {
        public int ItemsCount { get; set; }
    }
}