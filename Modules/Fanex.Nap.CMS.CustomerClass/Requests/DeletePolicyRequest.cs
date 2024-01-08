namespace Fanex.Nap.CMS.CustomerClass.Requests
{
    using Fanex.Nap.CMS.Infrastructure.Requests;
    
    public class DeletePolicyRequest : CommandBase
    {
        public string Policies { get; set; }
    }
}
