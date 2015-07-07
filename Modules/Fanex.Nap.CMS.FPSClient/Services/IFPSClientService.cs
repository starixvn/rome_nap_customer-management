namespace Fanex.Nap.CMS.FPSClient.Services
{
    using Fanex.Nap.CMS.FPSClient.Criteria;
    using Fanex.Oneyed.BussinessRule.Contracts;

    public interface IFPSClientService
    {
        string GetSubscriber(GetCustomerTypeAndSiteCriteria criteria);

        ResponseCode WinLimitControlUnblock(int customerId, string username, string product);
    }
}
