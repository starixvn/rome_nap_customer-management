namespace Fanex.Nap.CMS.Infrastructure.Models
{
    public class SiteGroup : IdNameModel
    {
        public SiteGroup(int id, string name, bool isLicensee)
        {
            Id = id;
            Name = name;
            IsLicensee = isLicensee;
        }

        public bool IsLicensee { get; }
    }
}
