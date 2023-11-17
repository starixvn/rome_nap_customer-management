using Fanex.Nap.CMS.Infrastructure.Requests;

namespace Fanex.Nap.CMS.CustomerClass.Requests
{
    public class DeleteRuleRequest : CommandBase
    {
        public string RuleIDs { get; set; }
    }
}
