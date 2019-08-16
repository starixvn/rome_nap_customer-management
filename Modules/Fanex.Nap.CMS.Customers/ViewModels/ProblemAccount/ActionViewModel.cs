namespace Fanex.Nap.CMS.Customers.ViewModels.ProblemAccount
{
    using Infrastructure.Constants;

    public class ActionViewModel
    {
        public string Usernames { get; set; }

        public byte Level { get; set; }

        public virtual bool IsInvalid()
            => string.IsNullOrWhiteSpace(Usernames)
            || Level < RoleIdConstants.Member || Level > RoleIdConstants.Super;
    }
}
