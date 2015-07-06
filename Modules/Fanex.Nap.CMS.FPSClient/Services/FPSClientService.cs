namespace Fanex.Nap.CMS.FPSClient.Services
{
    using System;
    using Fanex.Nap.CMS.FPSClient.Criteria;
    using Fanex.Nap.CMS.FPSClient.Repositories;

    public class FPSClientService : IFPSClientService
    {
        private const string UnblockAction = "placebet";

        private readonly IFPSRepository _fpsRepsitory;
        private readonly FPS.BRClient.Service _fpsClient;

        public FPSClientService(IFPSRepository fpsRepsitory = null, FPS.BRClient.Service fpsClient = null)
        {
            _fpsRepsitory = fpsRepsitory ?? new FPSRepository();
            _fpsClient = fpsClient ?? new FPS.BRClient.Service();
        }

        public string GetSubscriber(GetCustomerTypeAndSiteCriteria criteria)
        {
            _fpsRepsitory.GetCustomerTypeAndSite(criteria);
            if (criteria.IsDeposit)
            {
                return criteria.Site;    
            }

            if (criteria.Site.Equals("789y", StringComparison.OrdinalIgnoreCase))
            {
                return criteria.Site;
            }

            return "maxbet";
        }

        public bool WinLimitControlUnblock(int customerId, string username, string product)
        {
            string subscriber = GetSubscriber(new GetCustomerTypeAndSiteCriteria { CustomerId = customerId });
            return _fpsClient.Unblock(subscriber, username, product, UnblockAction);
        }
    }
}
