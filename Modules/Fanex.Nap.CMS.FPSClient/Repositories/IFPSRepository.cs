namespace Fanex.Nap.CMS.FPSClient.Repositories
{
    using Fanex.Nap.CMS.FPSClient.Criteria;

    public interface IFPSRepository
    {
        void GetCustomerTypeAndSite(GetCustomerTypeAndSiteCriteria criteria);
    }
}
