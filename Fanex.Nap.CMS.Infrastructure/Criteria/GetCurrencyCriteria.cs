namespace Fanex.Nap.CMS.Infrastructure.Criteria
{
    using Fanex.Nap.CMS.Infrastructure.Configurations;
    using Fanex.NAP.Framework.Data;

    internal sealed class GetCurrencyCriteria : CriteriaBase
    {
        public GetCurrencyCriteria(byte mode)
            => Mode = mode;

        public byte Mode { get; }

        public bool IsTest { get; } = AppSettings.IsTest;

        public override string GetSettingKey()
            => "Acc_ExchangeSel";
    }
}
