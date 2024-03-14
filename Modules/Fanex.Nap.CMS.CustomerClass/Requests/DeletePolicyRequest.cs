namespace Fanex.Nap.CMS.CustomerClass.Requests
{
    using Fanex.Nap.CMS.Infrastructure.Requests;
    using System.Collections.Generic;

    public class DeletePolicyRequest : CommandBase
    {
        public string Policies { get; set; }

        public IEnumerable<short> SportTypes { get; set; }
    }
}
