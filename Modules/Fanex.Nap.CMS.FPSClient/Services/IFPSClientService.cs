namespace Fanex.Nap.CMS.FPSClient.Services
{
    using Fanex.Nap.CMS.FPSClient.Criteria;

    public interface IFPSClientService
    {
        string GetSubscriber(GetCustomerTypeAndSiteCriteria criteria);

        bool WinLimitControlUnblock(int customerId, string username, string product);
    }
}
