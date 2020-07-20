namespace Fanex.Nap.CMS.VirtualSettingControl.Models.Basketball
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.VirtualSettingControl.Models.VirtualRateDetails;

    public class BasketballVirtualRateDetails
    {
        public BasketballVirtualRateDetails(
            Result result,
            CustomerInfo customer) : this(result, customer, null, null)
        {
        }

        public BasketballVirtualRateDetails(
            Result result,
            CustomerInfo customer,
            IEnumerable<VirtualRateDetailsByGroup> virtualRates,
            IEnumerable<RateInfo> rateInfo)
        {
            Result = result;
            Customer = customer;
            VirtualRates = virtualRates;
            RateInfo = rateInfo;
        }

        public Result Result { get; }

        public CustomerInfo Customer { get; }

        public IEnumerable<VirtualRateDetailsByGroup> VirtualRates { get; }

        public IEnumerable<RateInfo> RateInfo { get; }

        public int MinRate => RateInfo.Min(r => r.Value);

        public int MaxRate => RateInfo.Max(r => r.Value);
    }
}
