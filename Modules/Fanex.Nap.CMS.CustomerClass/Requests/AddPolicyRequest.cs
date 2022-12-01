namespace Fanex.Nap.CMS.CustomerClass.Requests
{
    using Fanex.Nap.CMS.Infrastructure.Requests;

    public class AddPolicyRequest : CommandBase
    {
        public short SportType { get; set; }

        public string PolicyTitle { get; set; }

        public string RuleIds { get; set; }

        public override bool IsValid()
            => base.IsValid()
            && !string.IsNullOrWhiteSpace(PolicyTitle);
    }
}
