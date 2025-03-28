using Fanex.Nap.CMS.Infrastructure.Web;

namespace Fanex.Nap.CMS.Customers.Models.VirtualRateEngine
{
    public class SportTypeDTO
    {
        public int Sporttype { get; set; }
        public string SportName => CacheRef.GetSportName(Sporttype);
    }
}
