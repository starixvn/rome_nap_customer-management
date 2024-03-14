namespace Fanex.Nap.CMS.CustomerClass.Requests
{
    using Fanex.Nap.CMS.Infrastructure.Requests;
    using System.Collections.Generic;

    public class DeleteRuleRequest : CommandBase
    {
        public string RuleIDs { get; set; }

        public IEnumerable<short> SportTypes { get; set; }
    }
}
