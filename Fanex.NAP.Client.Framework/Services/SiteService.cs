namespace Fanex.NAP.Client.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Clients;
    using Models;

    public interface ISiteService
    {
        IEnumerable<MenuItem> GetMenuItemBySiteId(int siteId, string environment, bool? isActive = null);

        Task<IEnumerable<MenuItem>> GetMenuItemBySiteIdAsync(int siteId, string environment, bool? isActive = null);
    }

    public class SiteService : ISiteService
    {
        private readonly ISiteClient _siteClient;

        public SiteService(ISiteClient siteClient)
        {
            _siteClient = siteClient;
        }

        public IEnumerable<MenuItem> GetMenuItemBySiteId(int siteId, string environment, bool? isActive = null)
            => GetMenuItemBySiteIdAsync(siteId, environment, isActive).Result;

        public async Task<IEnumerable<MenuItem>> GetMenuItemBySiteIdAsync(int siteId, string environment, bool? isActive = null)
        {
            return await _siteClient.GetMenuItemBySiteId(siteId, environment, isActive).ConfigureAwait(false);
        }
    }
}