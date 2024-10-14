namespace Fanex.Nap.CMS.CustomerClass.Requests
{
    using Fanex.Nap.CMS.Infrastructure.Requests;

    public class EnableRuleRequest : CommandBase
    {
        public int RuleId { get; set; }

        public short SportType { get; set; }

        public bool Enable { get; set; }

        public bool TopPriority { get; set; }

        public override bool IsValid()
            => base.IsValid() && RuleId > 0;
    }
}
