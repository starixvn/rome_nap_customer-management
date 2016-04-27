namespace Fanex.Nap.CMS.VirtualSettingControl.Utility
{
    using Fanex.Data.Repository;

    public abstract class PagingCriteria : CriteriaBase
    {
        public int ItemsCount { get; set; }
    }
}