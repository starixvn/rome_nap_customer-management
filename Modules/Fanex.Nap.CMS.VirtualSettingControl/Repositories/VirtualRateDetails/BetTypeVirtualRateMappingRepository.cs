namespace Fanex.Nap.CMS.VirtualSettingControl.Repositories.VirtualRateDetails
{
    using System.Collections.Generic;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.VirtualSettingControl.Models.VirtualRateDetails;

    public interface IBetTypeVirtualRateMappingRepository
    {
        IEnumerable<BetTypeVirtualRateMapping> GetBetTypeVirtualRateMapping(ICriteria criteria);
    }

    public class BetTypeVirtualRateMappingRepository : IBetTypeVirtualRateMappingRepository
    {
        public IEnumerable<BetTypeVirtualRateMapping> GetBetTypeVirtualRateMapping(ICriteria criteria)
        {
            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                return db.Query<BetTypeVirtualRateMapping>(criteria);
            }
        }
    }
}
