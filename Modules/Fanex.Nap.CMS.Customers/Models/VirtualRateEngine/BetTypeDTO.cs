using Fanex.Nap.CMS.Infrastructure.Web;

namespace Fanex.Nap.CMS.Customers.Models.VirtualRateEngine
{
    public class BetTypeDTO
    {
        public int BetType { get; set; }
        public string BetTypeName => CacheRef.GetBetType(BetType);
    }
}
