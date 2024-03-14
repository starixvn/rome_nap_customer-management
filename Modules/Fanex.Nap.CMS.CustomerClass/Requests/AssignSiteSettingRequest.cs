namespace Fanex.Nap.CMS.CustomerClass.Requests
{
    using Fanex.Nap.CMS.Infrastructure.Requests;

    public class AssignSiteSettingRequest : CommandBase
    {
        public int PolicyId { get; set; }

        public short SportType { get; set; }

        public SiteCurrencyRequest[] SiteCurrencies { get; set; }

        public bool IsCheck { get; set; } = true;

        public override bool IsValid()
            => base.IsValid()
            && PolicyId > 0
            && SiteCurrencies != null && SiteCurrencies.Length > 0;
    }
}
